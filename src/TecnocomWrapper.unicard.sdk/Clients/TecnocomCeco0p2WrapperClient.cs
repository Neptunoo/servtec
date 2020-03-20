using AutoMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TecnocomWrapper.unicard.sdk.BusquedaPersonasPorPan;
using TecnocomWrapper.unicard.sdk.Models;
using unicard.sdk.Common;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public class TecnocomCeco0p2WrapperClient : TecnocomBaseClient, ITecnocomCeco0p2WrapperClient
    {
        private const string UrlFormatService = "https://{0}/axis2SMU/services/CE_CECO0P2_v10?wsdl";

        private static CE_CECO0P2_v10 _service;
        private static object sync = new object();

        public TecnocomCeco0p2WrapperClient()
        {
            IdServicio = "CECO0P2";
            NombreServicio = "BÚSQUEDA Y SELECCIÓN DE PERSONAS POR PAN";
        }

        private static CE_CECO0P2_v10 Service
        {
            get
            {
                if(_service == null)
                {
                    lock (sync)
                    {
                        if(_service == null)
                        {
                            _service = new CE_CECO0P2_v10();
                            _service.Url = string.Format(UrlFormatService, ConfigurationManager.AppSettings["Tecnocom-Service:HostHttps"]);
                        }
                    }
                }

                return _service;
            }
        }

        public Respuesta_CECO0P2_Registro_CECO0P2[] ObtienePersonasPorPan(string pan)
        {

            var respuesta = new Respuesta_CECO0P2();

            var paramsEntrada = CrearParametrosEntrada(pan);
            var key = GenerateKeyToken(paramsEntrada);
            var operationData = ObtenerOperacionData(key);

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            using (var tc = new TimerCounterNR("Custom/tecnocom/service/" + IdServicio.ToLowerInvariant()))
            {
                respuesta = Service.runService(operationData, paramsEntrada);
            }

            if (respuesta.retorno != TipoRetornoServicio.Exito)
            {
                EnviarANewrelic(pan, IdServicio.ToLowerInvariant(), respuesta.retorno, respuesta.descRetorno);
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

        private Entrada_CECO0P2 CrearParametrosEntrada(string Pan)
        {
            return new Entrada_CECO0P2()
            {
                pan = Pan
            };
        }

        public void EnviarANewrelic(string pan, string servicioId, string errorCodigo, string errorDescripcion)
        {

            var errorAttributes = new Dictionary<string, string>() {
                { "internal.cliente.pan", pan},
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
