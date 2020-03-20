using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TecnocomWrapper.unicard.sdk.AutorizadorLineas;
using TecnocomWrapper.unicard.sdk.Models;
using unicard.sdk.Common;
using unicard.sdk.Common.Models;
using System.Security.Cryptography.X509Certificates;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public class TecnocomCecl0tlWrapperClient : TecnocomBaseClient, ITecnocomCecl0tlWrapperClient
    {

        private const string UrlFormatService = "https://{0}/axis2SMU/services/CE_CECL0TL_SMUv10?wsdl";

        private readonly string _urlService;

        private static CE_CECL0TL_SMUv10 _service;
        private static object sync = new object();

        public TecnocomCecl0tlWrapperClient()
        {
            IdServicio = "CECL0TL";
            NombreServicio = "Lista De Referencias De Tipo De Linea Por Contrato";
        }

        public static CE_CECL0TL_SMUv10 Service
        {
            get
            {
                if (_service == null)
                {
                    lock (sync)
                    {
                        if (_service == null)
                        {
                            _service = new CE_CECL0TL_SMUv10();
                            _service.Url = string.Format(UrlFormatService, ConfigurationManager.AppSettings["Tecnocom-Service:HostHttps"]);

                        }
                    }
                }
                return _service;
            }
        }

        public Respuesta_CECL0TL_Registro_CECL0TL[] ObtieneLineasPorContrato(string cuenta, string centroAlta)
        {


            Respuesta_CECL0TL respuesta = new Respuesta_CECL0TL();

            var paramsEntrada = CrearParametrosEntrada(cuenta, centroAlta);
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
                EnviarANewrelic(cuenta, centroAlta, IdServicio.ToLowerInvariant(), respuesta.retorno, respuesta.descRetorno);
                ThrowExcepcion(respuesta.retorno, respuesta.descRetorno);
            }

            return respuesta.contratos;

        }


        public OperationData ObtenerOperacionData(string key)
        {
            var paramsOperationData = CreateOperationData(key);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OperationDataModel, OperationData>();
            });

            IMapper iMapper = config.CreateMapper();
            return iMapper.Map<OperationDataModel, OperationData>(paramsOperationData);

        }


        public Respuesta_CECL0TL_Registro_CECL0TL ObtineLineaPorContrato(string cuenta, string centroAlta, string idTipoLinea)
        {
            var tiposLinea = ObtieneLineasPorContrato(cuenta, centroAlta);
            return tiposLinea.Where(x => x.tipolin1 == idTipoLinea).FirstOrDefault();
        }


        public Entrada_CECL0TL CrearParametrosEntrada(string cuenta, string centroAlta)
        {
            return new Entrada_CECL0TL()
            {
                centalta = centroAlta,
                cuenta = cuenta,
                linref = 0,
                linrefSpecified = true
            };

        }
        public void EnviarANewrelic(string cuenta, string centroAlta, string servicioId, string errorCodigo, string errorDescripcion)
        {
            var errorAttributes = new Dictionary<string, string>() {
                { "internal.cliente.cuenta", cuenta},
                { "internal.cliente.centro_alta", centroAlta},
                { "internal.error.codigo",errorCodigo},
                { "internal.error.mensaje",errorDescripcion },
                { "internal.servicio.nombre", servicioId },
                { "internal.servicio.tipo", "servicio" },
                { "internal.servicio.proveedor", "tecnocom" },
            };

            SendToNewRelic(new TecnocomException(string.Format("codigo:{0}, mensaje:{1}", errorCodigo, errorDescripcion)), null);
        }
    }
}