using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CertificadoApi.Core.Model;
using RestSharp;

namespace CertificadoApi.Infraestructura.Servicios
{
    public class ConvertidorPdfService
    {
        public byte[] GenerarPdf(CertificadoData data)
        {
            var client = new RestClient(ConfigurationManager.AppSettings["GeneradorPdf.Api.Url"]);
            var pdfRequest = new RestRequest("/HTMLToPDF/html/html-to-pdf");
            pdfRequest.RequestFormat = DataFormat.Json;
            pdfRequest.AddBody(new { base64 = true, includeWatermark = false, body = BindearTemplate(data), pdfBase64 = true });
            var tokenResponse = client.Post(pdfRequest);
            return Convert.FromBase64String(tokenResponse.Content);
        }

        public string BindearTemplate(CertificadoData data)
        {
            var template = this.HtmlTemplate;
            template = template.Replace("#Nombre#", data.Nombre);
            template = template.Replace("#NumeroOperacion#", data.NumeroOperacion);
            template = template.Replace("#TipoPago#", data.TipoPago);
            template = template.Replace("#FechaAperturaContrato#", data.FechaAperturaContrato);
            template = template.Replace("#FechaUltimaFacturacion#", data.FechaUltimaFacturacion);
            template = template.Replace("#FechaUltimoVencimiento#", data.FechaUltimoVencimiento);
            template = template.Replace("#FechaProximaFacturacion#", data.FechaProximaFacturacion);
            template = template.Replace("#TasaInteresMensual#", data.TasaInteresMensual);
            template = template.Replace("#ComisionPorPagoAnticipado#", data.ComisionPorPagoAnticipado);
            template = template.Replace("#DeudaAlDiaEmision#", data.DeudaAlDiaEmision);
            template = template.Replace("#ProyeccionInteres#", data.ProyeccionInteres);
            template = template.Replace("#GastoAdministracionProyectado#", data.GastoAdministracionProyectado);
            template = template.Replace("#ProyeccionDeudaPeriodoActualHasta#", data.ProyeccionDeudaPeriodoActualHasta);
            template = template.Replace("#MontoTotalPagarFechaProyeccion#", data.MontoTotalPagarFechaProyeccion);

            return template;
        }



        public readonly string HtmlTemplate = @"<!DOCTYPE html><html lang='es'>

<head>
    <title>LIQ</title>
    <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' integrity='sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T' crossorigin='anonymous'>
</head>

<body>
    <div class='container' style='font-family: Arial;'>
        <br>
        <h4 class='text-center'>LIQUIDACION DE LA TARJETA DE CREDITO PARA TERMINO ANTICIPADO</h4>
        <br>
        <br>
        <br>
        <table class='table table-bordered'>
            <tr><td>NOMBRE:</td><td>#Nombre#</td></tr>
            <tr><td>NÚMERO DE LA OPERACIÓN:</td><td>#NumeroOperacion#</td></tr>
            <tr><td>TIPO DE PAGO:</td><td>#TipoPago#</td></tr>
            <tr><td>FECHA DE APERTURA DEL CONTRATO:</td><td>#FechaAperturaContrato#</td></tr>
            <tr><td>FECHA ÚLTIMA FACTURACION:</td><td>#FechaUltimaFacturacion#</td></tr>
            <tr><td>FECHA ÚLTIMO VENCIMIENTO:</td><td>#FechaUltimoVencimiento#</td></tr>
            <tr><td>FECHA PRÓXIMA FACTURACIÓN:</td><td>#FechaProximaFacturacion#</td></tr>
            <tr><td>TASA DE INTERÉS MENSUAL:</td><td>#TasaInteresMensual#</td></tr>
            <tr><td>COMISIÓN POR PAGO ANTICIPADO:</td><td>#ComisionPorPagoAnticipado#</td></tr>
            <tr><td>DEUDA AL DIA DE EMISION (1):</td><td>#DeudaAlDiaEmision#</td></tr>
            <tr><td>PROYECCIÓN DE INTERESES (2):</td><td>#ProyeccionInteres#</td></tr>
            <tr><td>GASTO DE ADMINISTRACIÓN PROYECTADO (2):</td><td>#GastoAdministracionProyectado#</td></tr>
            <tr><td>PROYECCIÓN DE DEUDA DEL PERÍODO ACTUAL HASTA (3):</td><td>#ProyeccionDeudaPeriodoActualHasta#</td></tr>
            <tr><td>MONTO TOTAL A PAGAR A LA FECHA DE PROYECCIÓN (4):</td><td>#MontoTotalPagarFechaProyeccion#</td></tr>

        </TABLE>
        <br>
        
      <p class='text-justify'>(1) Corresponde a la deuda total del cliente considerando toda la deuda nacional hasta la fecha indicada y expresada en pesos chilenos, e incluye comision de prepago</p>
        <p class='text-justify'>(2) Cobro de administración Mensual correspondiente a UF 0.11. Valor referencial según UF a la fecha de emisión.</p>
         <p class='text-justify'>(3) Proyección de los intereses que se devengaran hasta la fecha o plazo de pago de la Tarjeta de Crédito Unimarc, que corresponde al mes en curso</p>
          <p class='text-justify'>(4) Monto proyectado no incluye transacciones efectuadas con posterioridad a la emisión del presente documento.</p>
            <br>
    </div>
    <script src='https://code.jquery.com/jquery-3.2.1.slim.min.js' integrity='sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN' crossorigin='anonymous'></script>
    <script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js' integrity='sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q' crossorigin='anonymous'></script>
    <script src='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js' integrity='sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl' crossorigin='anonymous'></script>
</body>

</html>";
    }
}