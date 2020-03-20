using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecnocomWrapper.unicard.sdk.Models;
using unicard.sdk.Common;
using unicard.sdk.Common.Models;


namespace TecnocomWrapper.unicard.sdk.Clients
{
    public abstract class TecnocomBaseClient
    {

        public string IdServicio = "";
        public string NombreServicio = "";
        public OperationDataModel CreateOperationData(string key)
        {
            return new OperationDataModel()
            {
                avanzar = false,
                avanzarSpecified = true,
                canal = "C_UNICARD",
                claveFin = "0",
                claveInicio = "0",
                entidad = "0288",
                idioma = "ES",
                pantPagina = "0",
                retroceder = false,
                retrocederSpecified = true,
                securityHash = key
            };
        }

        public void ThrowExcepcion(string codigoError, string descripcion)
        {

            var e = new TecnocomException("Error en servicio Tecnocom (" + IdServicio + " | " + NombreServicio + ") ");
            var errores = new List<ErrorModel>() { new ErrorModel() { codigo = codigoError, mensaje = descripcion } };
            e.Data.Add("detalle", errores);
            throw e;

        }

        public string GenerateKeyToken(object instanciaEntrada)
        {
            string key = "0288C_UNICARDSecuritySMUAPPKey" + ConcatenarValoresDeInstancia(instanciaEntrada, "Specified");
            return Helpers.GetHashSha256(key);
        }

        public static string ConcatenarValoresDeInstancia(object instancia, string nombrePropiedadNoIncluye = null)
        {

            Dictionary<string, object> values = instancia.GetType().GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(instancia));
            IEnumerable<KeyValuePair<string, object>> values2;

            if (string.IsNullOrEmpty(nombrePropiedadNoIncluye))
                values2 = values.Select(p => p);
            else
                values2 = values.Where(p => !p.Key.Contains(nombrePropiedadNoIncluye));


            var specifier = "F";

            var culture = CultureInfo.CreateSpecificCulture("en-CA");

            return string.Join("", values2.OrderBy(o => o.Key).Select(v =>  v.Value is double? ((double)v.Value).ToString(specifier, culture): v.Value).ToArray());


        }

        public void SendToNewRelic(Exception exception, Dictionary<string, string> errorAttributes) {
            NewRelic.Api.Agent.NewRelic.NoticeError(exception, errorAttributes);
        }
    }
}