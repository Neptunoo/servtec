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
using TecnocomWrapper.unicard.sdk.AcuseDeRecibo;
using TecnocomWrapper.unicard.sdk.Models;
using unicard.sdk.Common;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public class TecnocomCemo0reWrapperClient : TecnocomBaseClient, ITecnocomCemo0reWrapperClient
    {

        private const string UrlFormatService = "https://{0}/axis2SMU/services/CE_CEMO0RE_SMUv10?wsdl";

        private readonly string _urlService;

        private static CE_CEMO0RE_SMUv10 _service;
        private static object sync = new object();


        public TecnocomCemo0reWrapperClient()
        {
            IdServicio = "CEMO0RE";
            NombreServicio = "Acuse de recibo de Tarjeta";
        }


        private static CE_CEMO0RE_SMUv10 Service
        {
            get
            {
                if (_service == null)
                {
                    lock (sync)
                    {
                        if (_service == null)
                        {
                            _service = new CE_CEMO0RE_SMUv10();
                            _service.Url = string.Format(UrlFormatService, ConfigurationManager.AppSettings["Tecnocom-Service:HostHttps"]);

                        }
                    }
                }
                return _service;
            }
        }

        public Respuesta_CEMO0RE_Registro_CEMO0RE[] AcusaRecibo(string pan)
        {


            var respuesta = new Respuesta_CEMO0RE();

            var paramsEntrada = CrearParametrosEntrada(pan);
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
                EnviarANewrelic(IdServicio.ToLowerInvariant(), respuesta.retorno, respuesta.descRetorno);
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

        private Entrada_CEMO0RE CrearParametrosEntrada(string pan)
        {
            return new Entrada_CEMO0RE()
            {
                pan = pan
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
