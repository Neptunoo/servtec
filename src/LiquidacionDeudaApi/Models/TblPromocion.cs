using System;
using System.Collections.Generic;

namespace LiquidacionDeudaApi.Models
{
    public partial class TblPromocion
    {
        public TblPromocion()
        {
            TblCupon = new HashSet<TblCupon>();
        }

        public int IdPromocion { get; set; }
        public string NombrePromocion { get; set; }
        public DateTime FechaInicioPromocion { get; set; }
        public DateTime FechaTerminoPromocion { get; set; }
        public string Activo { get; set; }

        public virtual ICollection<TblCupon> TblCupon { get; set; }
    }
}
