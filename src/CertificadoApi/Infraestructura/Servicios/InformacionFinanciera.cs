using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace CertificadoApi.Infraestructura.Servicios
{
    public class InformacionFinanciera
    {
        public InfoResponse ObtenerInformacionFinanciera(int rut, int canal)
        {
            var client = new RestClient(ConfigurationManager.AppSettings["InformacionFinanciera.Api.Url"]);

            var tokenRequest = new RestRequest("/token");
            tokenRequest.AddParameter("Authorization", $"Basic {ConfigurationManager.AppSettings["InformacionFinanciera.Api.BasicAuthToken"]}", ParameterType.HttpHeader);
            tokenRequest.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);
            var tokenResponse = client.Post<TokenResponse>(tokenRequest);


            var solicitaDatosRequest = new RestRequest("/api/unicard/global/informacion-financiera/v1/solicita-datos");
            solicitaDatosRequest.AddParameter("Authorization", $"Bearer {tokenResponse.Data.access_token}", ParameterType.HttpHeader);

            solicitaDatosRequest.RequestFormat = DataFormat.Json;
            solicitaDatosRequest.AddBody(new { RutCliente = rut, canal = canal });

            var solicitaDatosResponse = client.Post<InfoResponse>(solicitaDatosRequest);

            return solicitaDatosResponse.Data;
        }
    }

    public class TokenResponse
    {
        public string access_token { get; set; }
    }

    public class InfoResponse
    {
        public int fechaVencimiento { get; set; }

        public int fechaFacturacion { get; set; }
    }
}