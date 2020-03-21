using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using LiquidacionDeudaApi.Models;

namespace LiquidacionDeudaApi.Infraestructura.Persistencia
{
    public class DbAtencionClienteContext : DbContext
    {
        public DbAtencionClienteContext()
        {
        }

        public virtual DbSet<TblClienteOferta> TblClienteOferta { get; set; }
        public virtual DbSet<TblClientesAprobados> TblClientesAprobados { get; set; }
        public virtual DbSet<TblFormalizacion> TblFormalizacion { get; set; }
    }
}
