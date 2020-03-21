using System;

namespace LiquidacionDeudaApi.Models
{
    public partial class TblDemCreacionCuenta
    {
        public int IdDemCeacion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public TimeSpan? HoraRegistro { get; set; }
        public string TipoProducto { get; set; }
        public int RutCliente { get; set; }
        public string DvCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApPeterno { get; set; }
        public string ApMaterno { get; set; }
        public DateTime? FechaNac { get; set; }
        public string NumeroSerie { get; set; }
        public string Sexo { get; set; }
        public string EstadoCivil { get; set; }
        public string DireccionParticular { get; set; }
        public string ComunaParticular { get; set; }
        public string RegionParticular { get; set; }
        public string Ciudad { get; set; }
        public int? FonoParticularCelular { get; set; }
        public int? FonoParticularFijo { get; set; }
        public string CorreoElectronico { get; set; }
        public int? CupoNacional { get; set; }
        public int? DiaPago { get; set; }
        public int? Canal { get; set; }
        public int? EstadoCreacionCuenta { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public int? CodigoLocal { get; set; }
        public int? RutEjecutiva { get; set; }
        public string OrigenServidor { get; set; }
        public int? IdOrigen { get; set; }
        public int? IdEtapa { get; set; }
        public string Nacionalidad { get; set; }
    }
}
