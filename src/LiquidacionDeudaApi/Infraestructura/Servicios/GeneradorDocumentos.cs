using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestClient.Net;
using RestClient.Net.Abstractions;

namespace LiquidacionDeudaApi.Infraestructura.Servicios
{
    public class GeneradorDocumentos
    {
        public async Task<byte[]> CrearDocumento(string htmlData)
        {
            var client = new Client(new NewtonsoftSerializationAdapter(), new Uri(ConfigurationManager.AppSettings["Api.Documentos"]));
            return await client.PostAsync<byte[], RequestDocumento>(new RequestDocumento
            {
                CodigoDocumento = 1010,
                RutCliente = 12102323,
                CadenaAtributo = "PDF"
            });
        }
    }

    public class RequestDocumento
    {
        public int CodigoDocumento { get; set; }
        public int RutCliente { get; set; }
        public string CadenaAtributo { get; set; }
    }

    public class NewtonsoftSerializationAdapter : ISerializationAdapter
    {
        #region Implementation
        public TResponseBody Deserialize<TResponseBody>(byte[] data, IHeadersCollection responseHeaders)
        {
            var markup = Encoding.UTF8.GetString(data);
            object markupAsObject = markup;

            if (typeof(TResponseBody) == typeof(string))
            {
                return (TResponseBody)markupAsObject;
            }

            return JsonConvert.DeserializeObject<TResponseBody>(markup);
        }

        public byte[] Serialize<TRequestBody>(TRequestBody value, IHeadersCollection requestHeaders)
        {
            var json = JsonConvert.SerializeObject(value);
            var binary = Encoding.UTF8.GetBytes(json);
            return binary;
        }
        #endregion
    }
}