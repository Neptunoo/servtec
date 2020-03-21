using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using LiquidacionDeudaApi.Models;

namespace LiquidacionDeudaApi.Infraestructura.Persistencia
{
    public partial class DB_CanalesContext : DbContext
    {
        public DB_CanalesContext()
        {
        }
      

        public virtual DbSet<HistDespliegue> HistDespliegue { get; set; }
        public virtual DbSet<TblCuenta> TblCuenta { get; set; }
        public virtual DbSet<TblCupon> TblCupon { get; set; }
        public virtual DbSet<TblDemCreacionCuenta> TblDemCreacionCuenta { get; set; }
        public virtual DbSet<TblPromocion> TblPromocion { get; set; }
        public virtual DbSet<TblRutCuponCanal> TblRutCuponCanal { get; set; }

    }
}