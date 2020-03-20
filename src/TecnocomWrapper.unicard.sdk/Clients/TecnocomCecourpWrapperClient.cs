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
using TecnocomWrapper.unicard.sdk.ConsultaPansPorRut;
using TecnocomWrapper.unicard.sdk.Models;
using unicard.sdk.Common;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public class TecnocomCecourpWrapperClient : TecnocomBaseClient, ITecnocomCecourpWrapperClient
    {
        private const string UrlFormatService = "https://{0}/axis2SMU/services/CE_CECOURP_SMUv10?wsdl";

        private static CE_CECOURP_SMUv10 _service;
        private static object sync = new object();
        public TecnocomCecourpWrapperClient()
        {
            IdServicio = "CECOURP";
            NombreServicio = "Consulta de número pan a través del número rut.";
        }

        private static CE_CECOURP_SMUv10 Service
        {
            get
            {
                if (_service == null)
                {
                    lock (sync)
                    {
                        if (_service == null)
                        {
                            _service = new CE_CECOURP_SMUv10();
                            _service.Url = string.Format(UrlFormatService, ConfigurationManager.AppSettings["Tecnocom-Service:HostHttps"]);

                        }
                    }
                }
                return _service;
            }
        }

        public Respuesta_CECOURP_Registro_CECOURP[] ObtienePansPorRutOPan(string rutOPan)
        {


            var respuesta = new Respuesta_CECOURP();

            var paramsEntrada = CrearParametrosEntrada(rutOPan);
            var key = GenerateKeyToken(paramsEntrada);
            var operationData = ObtenerOperacionData(key);

            //TODO:AGREGAR Using para medir tiempos de respuesta
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            using (var tc = new TimerCounterNR("Custom/tecnocom/service/" + IdServicio.ToLowerInvariant()))
            {
                respuesta = Service.runService(operationData, paramsEntrada);
            }

            if (respuesta.retorno != TipoRetornoServicio.Exito) {
                EnviarANewrelic(rutOPan, IdServicio.ToLowerInvariant(), respuesta.retorno, respuesta.descRetorno);
                ThrowExcepcion(respuesta.retorno, respuesta.descRetorno);
            }

            return respuesta.contratos;

        }
        private Entrada_CECOURP CrearParametrosEntrada(string rutOPan)
        {
            return new Entrada_CECOURP()
            {
                docupan = rutOPan
            };

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

        public void EnviarANewrelic(string rut, string servicioId, string errorCodigo, string errorDescripcion)
        {
            
            var errorAttributes = new Dictionary<string, string>() {
                { "internal.cliente.rut", rut},
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
