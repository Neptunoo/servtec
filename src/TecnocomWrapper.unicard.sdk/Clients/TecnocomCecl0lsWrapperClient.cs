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
using TecnocomWrapper.unicard.sdk.ListaLimitesYSaldosPorReferencia;
using TecnocomWrapper.unicard.sdk.Models;
using unicard.sdk.Common;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public class TecnocomCecl0lsWrapperClient : TecnocomBaseClient, ITecnocomCecl0lsWrapperClient
    {

        private const string UrlFormatService = "https://{0}/axis2SMU/services/CE_CECL0LS_SMUv10?wsdl";

        private readonly string _urlService;

        private static CE_CECL0LS_SMUv10 _service;
        private static object sync = new object();

        public TecnocomCecl0lsWrapperClient()
        {
            IdServicio = "CECL0LS";
            NombreServicio = "Lista de límites y saldos por referencia";
        }

        public static CE_CECL0LS_SMUv10 Service
        {
            get
            {
                if (_service == null)
                {
                    lock (sync)
                    {
                        if (_service == null)
                        {
                            _service = new CE_CECL0LS_SMUv10();
                            _service.Url = string.Format(UrlFormatService, ConfigurationManager.AppSettings["Tecnocom-Service:HostHttps"]);

                        }
                    }
                }
                return _service;
            }
        }
        public Respuesta_CECL0LS_Registro_CECL0LS[] ObtieneLineasPorContrato(string cuenta, string centroAlta, int lineaReferencia )
        {


            Respuesta_CECL0LS respuesta = new Respuesta_CECL0LS();

            var paramsEntrada = CrearParametrosEntrada(cuenta, centroAlta, lineaReferencia);
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
                EnviarANewrelic(cuenta, centroAlta, lineaReferencia, IdServicio.ToLowerInvariant(), respuesta.retorno, respuesta.descRetorno);
                ThrowExcepcion(respuesta.retorno, respuesta.descRetorno);
            }
             

            return respuesta.contratos;

        }

        public Respuesta_CECL0LS_Registro_CECL0LS ObtineLineaPorContrato(string cuenta, string centroAlta, string idTipoLinea, int lineaReferencia)
        {
            var tiposLinea = ObtieneLineasPorContrato(cuenta, centroAlta, lineaReferencia);
            return tiposLinea.Where(x => x.tipolin == idTipoLinea).FirstOrDefault();
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

        public Entrada_CECL0LS CrearParametrosEntrada(string cuenta, string centroAlta, int lineaReferencia)
        {
            return new Entrada_CECL0LS()
            {
                centalta = centroAlta,
                cuenta = cuenta,
                linref = lineaReferencia,
                linrefSpecified = true
            };

        }

        public void EnviarANewrelic(string cuenta, string centroAlta, int lineaReferencia, string servicioId, string errorCodigo, string errorDescripcion)
        {

            var errorAttributes = new Dictionary<string, string>() {
                { "internal.cliente.cuenta", cuenta},
                { "internal.cliente.centro_alta", centroAlta},
                { "internal.cliente.linea_referencia", lineaReferencia.ToString()},
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
