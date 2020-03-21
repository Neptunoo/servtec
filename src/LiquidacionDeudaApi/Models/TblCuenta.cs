using System;

namespace LiquidacionDeudaApi.Models
{
    public partial class TblCuenta
    {
        public string IdNumeroCuenta { get; set; }
        public int? EstadoCuenta { get; set; }
        public int? Rut { get; set; }
        public int? IdOrigen { get; set; }
        public int? IdCartera { get; set; }
        public long? Ean13 { get; set; }
        public int? CicloFacturacion { get; set; }
        public string Altacontrato { get; set; }
        public DateTime? FecApertura { get; set; }
    }
}
