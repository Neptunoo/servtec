using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CertificadoApi.Core.Model;
using Dapper;

namespace CertificadoApi.Infraestructura.Persistencia.AtencionCliente
{
    public class AtencionClienteDbContext
    {
        private const string NombreCadena = "connectionStringAtencionCliente";
        public int ObtenerNumeroCertificado()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[NombreCadena].ConnectionString))
            {
                return db.ExecuteScalar<int>("SELECT NEXT VALUE FOR [dbo].[SEQ_CERTIFICADO_DEUDA]");
            }
        }

     
    }
}