using System;

namespace LiquidacionDeudaApi.Models
{
    public partial class TblRutCuponCanal
    {
        public int Id { get; set; }
        public int RutCupon { get; set; }
        public int IdCupon { get; set; }
        public int IdCanal { get; set; }
        public bool? FlagDesplegar { get; set; }
        public DateTime FechaProximoDespliegue { get; set; }
        public string OrdenDespliegue { get; set; }

        public virtual TblCupon IdCuponNavigation { get; set; }
    }
}
