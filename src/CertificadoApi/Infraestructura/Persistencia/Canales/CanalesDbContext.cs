using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CertificadoApi.Core.Model;
using Dapper;

namespace CertificadoApi.Infraestructura.Persistencia.Canales
{
    public class CanalesDbContext
    {
        private const string NombreCadena = "connectionStringCanales";

        public Cliente Obtener(int rut)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[NombreCadena].ConnectionString))
            {
                return db.Query<Cliente>("Select NombreCliente, ApPeterno, ApMaterno From [CCuenta].[Tbl_DemCreacionCuenta]  " +
                    "WHERE RutCliente = @rut", new { rut }).FirstOrDefault();
            }
        }

        public Cuenta ObtenerCuenta(int rut)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings[NombreCadena].ConnectionString))
            {
                return db.Query<Cuenta>("select FecApertura from [dbo].[Tbl_Cuenta]  " +
                                        "WHERE Rut = @rut", new { rut }).FirstOrDefault();
            }
        }
    }
}