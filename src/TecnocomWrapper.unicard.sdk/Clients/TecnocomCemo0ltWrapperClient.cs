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
using TecnocomWrapper.unicard.sdk.ModificacionLineaContrato;
using unicard.sdk.Common;
using unicard.sdk.Common.Models;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public class TecnocomCemo0ltWrapperClient : TecnocomBaseClient, ITecnocomCemo0ltWrapperClient
    {

        private const string UrlFormatService = "https://{0}/axis2SMU/services/CE_CEMO0LT_SMUv10?wsdl";
        private string _urlService;

        private static CE_CEMO0LT_SMUv10 _service;
        private static object sync = new object();
        public TecnocomCemo0ltWrapperClient()
        {

            IdServicio = "CEMO0LT";
            NombreServicio = "Modificación del límite de tipo de línea por contrato.";
        }


        public static CE_CEMO0LT_SMUv10 Service
        {
            get
            {
                if (_service == null)
                {
                    lock (sync)
                    {
                        if (_service == null)
                        {
                            _service = new CE_CEMO0LT_SMUv10();
                            _service.Url = string.Format(UrlFormatService, ConfigurationManager.AppSettings["Tecnocom-Service:HostHttps"]);
                        }
                    }
                }
                return _service;
            }
        }


        public Respuesta_CEMO0LT_Registro_CEMO0LT[] ModificarLimiteDeLinea(ModificarLimiteLineaInputModel linea)
        {

            Respuesta_CEMO0LT respuesta = new Respuesta_CEMO0LT();

            var paramsEntrada = CrearParametrosEntrada(linea);
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
                EnviarANewrelic(linea, IdServicio.ToLowerInvariant(), respuesta.retorno, respuesta.descRetorno);
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

        public Entrada_CEMO0LT CrearParametrosEntrada(ModificarLimiteLineaInputModel linea)
        {
            return new Entrada_CEMO0LT()
            {
                centalta = linea.CentroAlta,
                clamon = 152,
                contcur = linea.ContadorConcurrencia,
                cuenta = linea.Cuenta,
                indporlim = "I",
                limcrelin = linea.Importe,
                linref = linea.LineaReferencia,
                porlim = 0,
                tipolin = linea.IdTipoLinea,
                clamonSpecified = true,
                limcrelinSpecified = true,
                linrefSpecified = true,
                porlimSpecified = true
            };

        }

        public void EnviarANewrelic(ModificarLimiteLineaInputModel linea, string servicioId, string errorCodigo, string errorDescripcion)
        {
            var errorAttributes = new Dictionary<string, string>() {
                { "internal.cliente.cuenta", linea.Cuenta.ToString()},
                { "internal.cliente.centro_alta", linea.CentroAlta.ToString()},
                { "internal.cliente.linea_referencia", linea.LineaReferencia.ToString()},
                { "internal.cliente.linea.importe", linea.Importe.ToString()},
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