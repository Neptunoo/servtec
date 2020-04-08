namespace LiquidacionDeudaApi.ViewModels
{
    public class RequestLiquidacion
    {
        public string Cuenta { get; set; }
        public string Pan { get; set; }
        public int RutCliente { get; set; }
        public int Canal { get; set; }
    }
}