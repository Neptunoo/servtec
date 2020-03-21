using System;
using System.Collections.Generic;

namespace LiquidacionDeudaApi.Models
{
    public partial class TblCupon
    {
        public TblCupon()
        {
            HistDespliegue = new HashSet<HistDespliegue>();
            TblRutCuponCanal = new HashSet<TblRutCuponCanal>();
        }

        public int IdCupon { get; set; }
        public string CodigoSmuCupon { get; set; }
        public DateTime FechaInicioCupon { get; set; }
        public DateTime FechaTerminoCupon { get; set; }
        public int IdPromocion { get; set; }
        public string TipoCupon { get; set; }
        public decimal? MontoCupon { get; set; }
        public string NombreCupon { get; set; }
        public string TextoCupon { get; set; }
        public int IdCuponAnterior { get; set; }
        public string Activo { get; set; }

        public virtual TblPromocion IdPromocionNavigation { get; set; }
        public virtual ICollection<HistDespliegue> HistDespliegue { get; set; }
        public virtual ICollection<TblRutCuponCanal> TblRutCuponCanal { get; set; }
    }
}
