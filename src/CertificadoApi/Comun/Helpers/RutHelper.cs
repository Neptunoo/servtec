using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertificadoApi.Comun.Helpers
{
    public class RutHelper
    {
        public static string Dv(Int32 rut)
        {
            double T = rut;
            double M = 0, S = 1;
            while (T != 0)
            {
                S = (S + T % 10 * (9 - M++ % 6)) % 11;
                T = Math.Floor(T / 10);
            }
            string dv = S != 0 ? (S - 1).ToString() : "K";
            return dv;
        }
    }
}