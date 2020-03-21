using System;
using System.Collections.Generic;

namespace LiquidacionDeudaApi.Models
{
    public partial class TblClientesAprobados
    {
        public TblClientesAprobados()
        {
            TblClienteOferta = new HashSet<TblClienteOferta>();
        }

        public int Rut { get; set; }
        public string Dv { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public int? EsCliente { get; set; }

        public virtual ICollection<TblClienteOferta> TblClienteOferta { get; set; }
    }
}
