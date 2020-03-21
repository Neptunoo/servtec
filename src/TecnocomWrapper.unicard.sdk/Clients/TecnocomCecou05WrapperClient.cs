using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TecnocomWrapper.unicard.sdk.PrePagoDiezDias;
using TecnocomWrapper.unicard.sdk.Models;
using unicard.sdk.Common;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public class TecnocomCecou05WrapperClient : TecnocomBaseClient
    {
        private const string UrlFormatService = "https://{0}/axis2SMU/services/CE_CECO005_SMUv10?wsdl";

        private readonly string _urlService;

        private static CE_CECO005_SMUv10 _service;
        private static object sync = new object();

        public TecnocomCecou05WrapperClient()
        {
            IdServicio = "CECO005";
            NombreServicio = "Prepago a Diez Dias";
        }

        private static CE_CECO005_SMUv10 Service
        {
            get
            {
                if (_service == null)
                {
                    lock (sync)
                    {
                        if (_service == null)
                        {
                            _service = new CE_CECO005_SMUv10();
                            _service.Url = string.Format(UrlFormatService, ConfigurationManager.AppSettings["Tecnocom-Service:HostHttps"]);

                        }
                    }
                }
                return _service;
            }
        }

        public Respuesta_CECO005_Registro_CECO005[] PrePagoDiezDias(string codent, string centalta, string cuenta, string pan, string clamon1)
        {


            var respuesta = new Respuesta_CECO005();

            var paramsEntrada = CrearParametrosEntrada(cuenta, pan);
            var key = GenerateKeyToken(paramsEntrada);
            var operationData = ObtenerOperacionData(key);
            operationData.pantPagina = "0";

            //TODO:AGREGAR Using para medir tiempos de respuesta
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            using (var tc = new TimerCounterNR("Custom/tecnocom/service/" + IdServicio.ToLowerInvariant()))
            {
                respuesta = Service.runService(operationData, paramsEntrada);
            }

            if (respuesta.retorno != TipoRetornoServicio.Exito)
            {
                //EnviarANewrelic(IdServicio.ToLowerInvariant(), respuesta.retorno, respuesta.descRetorno);
                ThrowExcepcion(respuesta.retorno, respuesta.descRetorno);
            }


            return respuesta.contratos;

        }


        private OperationData ObtenerOperacionData(string key)
        {
            var paramsOperationData = CreateOperationData(key);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OperationDataModel, OperationData>();
            }); 

            IMapper iMapper = config.CreateMapper();
            return iMapper.Map<OperationDataModel, OperationData>(paramsOperationData);
        }

        private Entrada_CECO005 CrearParametrosEntrada(string cuenta ,string pan)
        {
            return new Entrada_CECO005()
            {
                centalta = "0001",
                cuenta=cuenta,
                pan=pan,
                clamon1 = 152,
                clamon1Specified=true,    
                clamon2 = 152,
                clamon2Specified=true
            
            };

        }

        public void EnviarANewrelic(string servicioId, string errorCodigo, string errorDescripcion)
        {
            var errorAttributes = new Dictionary<string, string>() {
                { "internal.error.codigo",errorCodigo},
                { "internal.error.mensaje",errorDescripcion },
                { "internal.servicio.nombre", servicioId },
                { "internal.servicio.tipo", "servicio" },
                { "internal.servicio.proveedor", "tecnocom" },
            };

            SendToNewRelic(new TecnocomException(string.Format("codigo:{0}, mensaje:{1}", errorCodigo, errorDescripcion)), errorAttributes);
        }
    }
}
