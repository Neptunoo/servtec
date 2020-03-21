namespace LiquidacionDeudaApi.Models
{
    public partial class TblFormalizacion
    {
        public int Id { get; set; }
        public int IdSolicitud { get; set; }
        public int RutCliente { get; set; }
        public int? EsValidada { get; set; }
        public int IdTipoEnvio { get; set; }
        public int? AceptaContrato { get; set; }
        public int? AceptaSeguro { get; set; }
        public int? AceptaResumen { get; set; }
    }
}
