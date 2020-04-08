using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CertificadoApi.Core.Model
{
    public class CertificadoData
    {
        public string Rut { get; set; }
        public string Nombre { get; set; }
        public string NumeroOperacion { get; set; }
        public string TipoPago { get; set; }
        public string FechaAperturaContrato { get; set; }
        public string FechaUltimaFacturacion { get; set; }
        public string FechaUltimoVencimiento { get; set; }
        public string FechaProximaFacturacion { get; set; }
        public string TasaInteresMensual { get; set; }
        public string ComisionPorPagoAnticipado { get; set; }
        public string DeudaAlDiaEmision { get; set; }
        public string ProyeccionInteres { get; set; }

        public string GastoAdministracionProyectado { get; set; }
        public string ProyeccionDeudaPeriodoActualHasta { get; set; }
        public string MontoTotalPagarFechaProyeccion { get; set; }

    }
}