using AutoMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TecnocomWrapper.unicard.sdk.ConsultaMediosContactoPersona;
using TecnocomWrapper.unicard.sdk.Models;
using unicard.sdk.Common;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public class TecnocomCecl0coWrapperClient : TecnocomBaseClient, ITecnocomCecl0coWrapperClient
    {
        private const string UrlFormatService = "https://{0}/axis2SMU/services/CE_CECL0CO_v10?wsdl";

        private static CE_CECL0CO_v10 _service;
        private static object sync = new object();

        public TecnocomCecl0coWrapperClient()
        {
            IdServicio = "CECL0CO";
            NombreServicio = "CONSULTA PAGINABLE DE MEDIOS DE CONTACTO DE PERSONA";
        }

        private static CE_CECL0CO_v10 Service
        {
            get
            {
                if(_service == null)
                {
                    lock (sync)
                    {
                        if(_service == null)
                        {
                            _service = new CE_CECL0CO_v10();
                            _service.Url = string.Format(UrlFormatService, ConfigurationManager.AppSettings["Tecnocom-Service:HostHttps"]);
                        }
                    }
                }

                return _service;
            }
        }

        public Respuesta_CECL0CO_Registro_CECL0CO[] ObtieneMediosContactoPersona(String IdentificadorCliente, int pagina)
        {

            var respuesta = new Respuesta_CECL0CO();
            var paramsEntrada = CrearParametroEntrada(IdentificadorCliente, pagina);
            var Key = GenerateKeyToken(paramsEntrada);
            var operationData = ObtenerOperacionData(Key);

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            using (var tc = new TimerCounterNR("Custom/tecnocom/service/"+ IdServicio.ToLowerInvariant()))
            {
                respuesta = Service.runService(operationData, paramsEntrada);
            }

            if(respuesta.retorno != TipoRetornoServicio.Exito)
            {
                EnviarANewrelic(IdentificadorCliente, IdServicio.ToLowerInvariant(), respuesta.retorno, respuesta.descRetorno);
                ThrowExcepcion(respuesta.retorno, respuesta.descRetorno);
            }

            return respuesta.contratos;

        }

        private void EnviarANewrelic(string identificadorCliente, string servicioId, string errorCodigo, string errorDescripcion)
        {
            var errorAttributes = new Dictionary<string, string>() {
                { "internal.cliente.rut", identificadorCliente},
                { "internal.error.codigo",errorCodigo},
                { "internal.error.mensaje",errorDescripcion },
                { "internal.servicio.nombre", servicioId },
                { "internal.servicio.tipo", "servicio" },
                { "internal.servicio.proveedor", "tecnocom" },
            };

            SendToNewRelic(new TecnocomException(string.Format("codigo:{0}, mensaje:{1}", errorCodigo, errorDescripcion)), errorAttributes);
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

        private Entrada_CECL0CO CrearParametroEntrada(string identificadorCliente, int pagina)
        {
            return new Entrada_CECL0CO()
            {
                identcli = identificadorCliente,
                numsec = pagina,
                numsecSpecified = true

            };
        }

        public Respuesta_CECL0CO_Registro_CECL0CO[] ObtieneConsultaMediosContactoPersona(string rut)
        {
            throw new NotImplementedException();
        }
    }
}
