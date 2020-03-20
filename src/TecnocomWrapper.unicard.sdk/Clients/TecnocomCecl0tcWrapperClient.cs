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
using TecnocomWrapper.unicard.sdk.Models;
using TecnocomWrapper.unicard.sdk.RelacionTarjetasCuenta;
using unicard.sdk.Common;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public class TecnocomCecl0tcWrapperClient : TecnocomBaseClient, ITecnocomCecl0tcWrapperClient
    {
        private const string UrlFormatService = "https://{0}/axis2SMU/services/CE_CECL0TC_SMUv10?wsdl";

        private static CE_CECL0TC_SMUv10 _service;
        private static object sync = new object();
        public TecnocomCecl0tcWrapperClient()
        {
            IdServicio = "CECL0TC";
            NombreServicio = "Relación de tarjetas de una cuenta.";
        }

        private static CE_CECL0TC_SMUv10 Service
        {
            get
            {
                if (_service == null)
                {
                    lock (sync)
                    {
                        if (_service == null)
                        {
                            _service = new CE_CECL0TC_SMUv10();
                            _service.Url = string.Format(UrlFormatService, ConfigurationManager.AppSettings["Tecnocom-Service:HostHttps"]);

                        }
                    }
                }
                return _service;
            }
        }

        public Respuesta_CECL0TC_Registro_CECL0TC[] ObtieneTarjetasAsociadas(string centroAlta, string cuenta)
        {


            var respuesta = new Respuesta_CECL0TC();

            var paramsEntrada = CrearParametrosEntrada(centroAlta, cuenta);
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
                EnviarANewrelic(cuenta, centroAlta, IdServicio.ToLowerInvariant(), respuesta.retorno, respuesta.descRetorno);
                ThrowExcepcion(respuesta.retorno, respuesta.descRetorno);
            }

            return respuesta.contratos;

        }

        public Respuesta_CECL0TC_Registro_CECL0TC[] ObtieneTarjetasAsociadas(string centalta, string cuenta, int indicadorTipoTarjeta, int indicadorSituacionTarjeta, int codigoBloqueo)
        {
            var tarjetas = ObtieneTarjetasAsociadas(centalta, cuenta);

            return tarjetas.Where(x => x.indtipt == indicadorTipoTarjeta
                                    && x.indsittar == indicadorSituacionTarjeta
                                    && x.codblq == codigoBloqueo).ToArray();

        }

        private Entrada_CECL0TC CrearParametrosEntrada(string centalta, string cuenta)
        {
            return new Entrada_CECL0TC()
            {
                centalta = centalta,
                cuenta = cuenta
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

            SendToNewRelic(new TecnocomException(string.Format("codigo:{0}, mensaje:{1}", errorCodigo, errorDescripcion)), errorAttributes);
        }

    }
}
