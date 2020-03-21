using System;

namespace LiquidacionDeudaApi.Models
{
    public partial class TblClienteOferta
    {
        public int IdTipoOferta { get; set; }
        public int RutAprobado { get; set; }
        public int? Cupo { get; set; }
        public string Periodo { get; set; }
        public DateTime? InicioVigencia { get; set; }
        public DateTime? FinVigencia { get; set; }
        public int? UsoOferta { get; set; }

        public virtual TblClientesAprobados RutAprobadoNavigation { get; set; }
    }
}
