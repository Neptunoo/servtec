using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertificadoApi.Core.Aplicacion.Certificados.Querys
{
    public class CertificadoQuery
    {
        public int Rut { get; set; }
        public int Canal { get; set; }
        public string TipoPago { get; set; }
    }
}