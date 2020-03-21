using System;

namespace LiquidacionDeudaApi.Models
{
    public partial class HistDespliegue
    {
        public int IdDespligue { get; set; }
        public int RutCupon { get; set; }
        public int IdCupon { get; set; }
        public int IdCanal { get; set; }
        public bool? FlagDesplegar { get; set; }
        public DateTime FechaHoraDespliegue { get; set; }
        public string Accion { get; set; }

        public virtual TblCupon IdCuponNavigation { get; set; }
    }
}
