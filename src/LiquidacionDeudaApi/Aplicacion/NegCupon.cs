using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LiquidacionDeudaApi.Infraestructura.Persistencia;
using LiquidacionDeudaApi.Models;

namespace LiquidacionDeudaApi.Aplicacion
{
    public class ResponseCliente
    {
        private string _NombreCliente;
        private int _EsPreaprobado;
        private int _ConApertura;
        private String _Fecha;
        private string _codigoCuponUnidata;
        private int? _numeroTelefono;

        public string NombreCliente { get { return _NombreCliente; } set { _NombreCliente = value; } }
        public int EsPreaprobado { get { return _EsPreaprobado; } set { _EsPreaprobado = value; } }
        public int ConApertura { get { return _ConApertura; } set { _ConApertura = value; } }
        public string Fecha { get { return _Fecha; } set { _Fecha = value; } }
        public string codigoCuponUnidata { get { return _codigoCuponUnidata.ToString(); } set { _codigoCuponUnidata = value.ToString(); } }
        public int? numeroTelefono { get { return _numeroTelefono; } set { _numeroTelefono = value; } }


    }

    public class Neg_Cupon
    {
        public static string activo = "SI";
        public static string accion = "CONSULTA";

        public async Task<ResponseCliente> ConsultaClientePreAprobado(ResponseCliente responseCliente, int rutCliente)
        {
            var errorAttributes = new Dictionary<string, string>() {
                         { "internal.cliente.rut", rutCliente.ToString()}
                         };

            try
            {

                using (var db_context = new DbAtencionClienteContext())
                {

                    var respuesta = await db_context.TblClienteOferta.Join(db_context.TblClientesAprobados,
                        clienteOferta => clienteOferta.RutAprobado,
                        clienteAprobado => clienteAprobado.Rut,
                        (clienteOferta, clienteAprobado) => new
                        {
                            rut = clienteOferta.RutAprobado,
                            NombreCliente = clienteAprobado.Nombre,
                            Apellido_P = clienteAprobado.ApellidoPaterno,
                            Apellido_M = clienteAprobado.ApellidoMaterno,
                            Oferta = clienteOferta.UsoOferta
                        }
                        ).Where(c => c.rut == rutCliente).DefaultIfEmpty().FirstOrDefaultAsync();

                    StringBuilder NombrePila = new StringBuilder();

                    if (!string.IsNullOrEmpty(respuesta.NombreCliente)) { NombrePila.Append(respuesta.NombreCliente.Trim()).Append(' ', 1); }
                    if (!string.IsNullOrEmpty(respuesta.Apellido_P)) { NombrePila.Append(respuesta.Apellido_P.Trim()).Append(' ', 1); }
                    if (!string.IsNullOrEmpty(respuesta.Apellido_M)) { NombrePila.Append(respuesta.Apellido_M); }

                    responseCliente.NombreCliente = NombrePila.ToString();


                    if (respuesta.Oferta == 0 && respuesta.Oferta != null)
                    {
                        responseCliente.EsPreaprobado = 1;
                        responseCliente.codigoCuponUnidata = "0";
                    }
                    else
                    {
                        responseCliente.EsPreaprobado = 0;
                        responseCliente.codigoCuponUnidata = "0";

                    }

                    return responseCliente;
                }
            }
            catch (Exception err)
            {
            
                throw err;
            }


        }

        public async Task<ResponseCliente> ConsultaClienteApertura(ResponseCliente responseCliente, int rutCliente)
        {
            var errorAttributes = new Dictionary<string, string>() {
                         { "internal.cliente.rut", rutCliente.ToString()}
                         };
            try
            {
                using (var db_context = new DB_CanalesContext())
                {
                    var respuesta = await db_context.TblDemCreacionCuenta.Join(db_context.TblCuenta,
                        Dem => Dem.RutCliente,
                        Cuenta => Cuenta.Rut,
                        (Dem, Cuenta) => new
                        {
                            rut = Cuenta.Rut,
                            nombreCliente = Dem.NombreCliente,
                            apellidoPaterno = Dem.ApPeterno,
                            apellidoMaterno = Dem.ApMaterno,
                            celular = Dem.FonoParticularCelular,
                            FechaAlta = Cuenta.FecApertura,
                            estadoCuenta = Cuenta.EstadoCuenta
                        }
                        ).Where(c => c.rut == rutCliente && c.estadoCuenta == 1).DefaultIfEmpty().FirstOrDefaultAsync();

                    if (string.IsNullOrEmpty(responseCliente.NombreCliente))
                    {
                        StringBuilder NombrePila = new StringBuilder();
                        if (!string.IsNullOrEmpty(respuesta.nombreCliente)) { NombrePila.Append(respuesta.nombreCliente.Trim()).Append(' ', 1); }
                        if (!string.IsNullOrEmpty(respuesta.apellidoPaterno)) { NombrePila.Append(respuesta.apellidoPaterno.Trim()).Append(' ', 1); }
                        if (!string.IsNullOrEmpty(respuesta.apellidoMaterno)) { NombrePila.Append(respuesta.apellidoMaterno); }
                        responseCliente.NombreCliente = NombrePila.ToString();

                    }

                    if (respuesta.estadoCuenta == 1 && respuesta.estadoCuenta != null)
                    {
                        responseCliente.ConApertura = 0;
                        responseCliente.ConApertura = 1;
                        responseCliente.numeroTelefono = respuesta.celular;
                        responseCliente.Fecha = respuesta.FechaAlta.Value.ToString("yyyy-MM-dd");

                    }
                    else
                    {
                        responseCliente.numeroTelefono = 0;
                        responseCliente.ConApertura = 0;
                    }

                    return responseCliente;
                }
            }
            catch (Exception err)
            {
                throw err;
            }


        }

        public async Task<ResponseCliente> ConsultaCodigoCupon(ResponseCliente responseCliente, int rutCliente, int canal)
        {
            var errorAttributes = new Dictionary<string, string>() {
                         { "internal.cliente.rut", rutCliente.ToString()}
                         };

            try
            {
                using (var db_context = new DB_CanalesContext())
                {

                    var cupones_Cliente = await db_context.TblRutCuponCanal.Where(c => c.RutCupon == rutCliente).OrderBy(c => c.OrdenDespliegue).ToListAsync();

                    if (cupones_Cliente.Count == 0) { await RegistroConsultaCupon(0, rutCliente, canal); return responseCliente; }

                    foreach (var item in cupones_Cliente)
                    {
                        DateTime? date = DateTime.Now;
                        var respuesta = await db_context.TblCupon.Join(db_context.TblPromocion,
                                                    cupon => cupon.IdPromocion,
                                                    promocion => promocion.IdPromocion,
                                                    (cupon, promocion) => new
                                                    {
                                                        idCupon = cupon.IdCupon,
                                                        cuponPromocion = cupon.IdPromocion,
                                                        promocionID = promocion.IdPromocion,
                                                        codigoCupon = cupon.CodigoSmuCupon,
                                                        tipoCupon = cupon.TipoCupon,
                                                        montoCupon = cupon.MontoCupon,
                                                        cuponActivo = cupon.Activo,
                                                        nombrePromocion = promocion.NombrePromocion,
                                                        fechaInicioPromocion = promocion.FechaInicioPromocion,
                                                        fechaTerminoPromocion = promocion.FechaTerminoPromocion,
                                                        promocionActivo = promocion.Activo

                                                    }).Where(c => c.fechaInicioPromocion <= date && date <= c.fechaTerminoPromocion && c.idCupon == item.IdCupon).DefaultIfEmpty().FirstOrDefaultAsync();

                        if (respuesta.idCupon == 0) { await RegistroConsultaCupon(respuesta.idCupon, rutCliente, canal); return responseCliente; }

                        if (item.OrdenDespliegue.Equals("1") && respuesta.cuponActivo.Equals(activo) && respuesta.promocionActivo.Equals(activo) && !string.IsNullOrEmpty(item.OrdenDespliegue))
                        {
                            responseCliente.codigoCuponUnidata = respuesta.codigoCupon;
                            await RegistroConsultaCupon(respuesta.idCupon, rutCliente, canal);
                        }
                    }


                    return responseCliente;
                }
            }
            catch (Exception err)
            {
                throw err;
            }


        }

        public async Task RegistroConsultaCupon(int idCupon, int rutCliente, int canal)
        {
            var errorAttributes = new Dictionary<string, string>() {
                         { "internal.cliente.rut", rutCliente.ToString()},
                         { "internal.cliente.canal", canal.ToString()},
                         { "internal.cliente.cupon", idCupon.ToString()}
                         };

            try
            {
                using (var db_Context = new DB_CanalesContext())
                {
                    int num;
                    var connection = db_Context.Database.Connection;
                    connection.Open();

                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = "SELECT NEXT VALUE FOR [dbo].[seq_ConsultaCupon];";
                        var seq = cmd.ExecuteScalar();
                        num = int.Parse(seq.ToString());
                    }

                    DateTime date = DateTime.Now;

                    var obj = new HistDespliegue
                    {
                        IdDespligue = num,
                        RutCupon = rutCliente,
                        IdCupon = idCupon,
                        IdCanal = canal,
                        FlagDesplegar = true,
                        FechaHoraDespliegue = date,
                        Accion = accion
                    };

                    await db_Context.SaveChangesAsync();
                }
            }
          
            catch (Exception error)
            {
                throw error;

            }

        }

        public async Task<ResponseCliente> ObtenerClienteCupon(ResponseCliente responseCliente, int rutCliente, int canal)
        {
            responseCliente = await ConsultaClientePreAprobado(responseCliente, rutCliente);

            if (responseCliente.EsPreaprobado == 0)
            {
                responseCliente = await ConsultaClienteApertura(responseCliente, rutCliente);
            }

            responseCliente = await ConsultaCodigoCupon(responseCliente, rutCliente, canal);



            return responseCliente;
        }
    }
}