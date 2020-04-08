using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertificadoApi.Comun.Helpers
{
    public class FechasHelper
    {
        public static string Formatear(int anioMesDia)
        {
            var anio = anioMesDia.ToString().Substring(0, 4);
            var mes = anioMesDia.ToString().Substring(4, 2);
            var dia = anioMesDia.ToString().Substring(6, 2);

            return new DateTime(Convert.ToInt32(anio), Convert.ToInt32(mes), Convert.ToInt32(dia))
                .ToString("dd-MM-yyyy");
        }
    }
}