using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LiquidacionDeudaApi.Aplicacion;
using LiquidacionDeudaApi.Infraestructura.Servicios;
using LiquidacionDeudaApi.ViewModels;

namespace LiquidacionDeudaApi.Controllers
{
    public class LiquidacionesController : ApiController
    {
        public async Task<IHttpActionResult> Get([FromBody] RequestLiquidacion payload)
        {
            ResponseCliente respuesta = new ResponseCliente();
            ResponseLiquidacion resposneLiquidacion = new ResponseLiquidacion();
            Neg_Cupon logic = new Neg_Cupon();

            if (payload.RutCliente > 0)
            {
                respuesta = await logic.ObtenerClienteCupon(respuesta, payload.RutCliente, payload.Canal);
                resposneLiquidacion.PdfBuffer = await GenerarPdf(respuesta);
            }
            else
                throw new InvalidEnumArgumentException("rut cliente");

            return Ok(resposneLiquidacion);
        }

        private string GenerarHtml(ResponseCliente respuesta)
        {
            return $"<html><body></body></html>";
        }

        private async Task<byte[]> GenerarPdf(ResponseCliente respuesta)
        {
            var htmlData = GenerarHtml(respuesta);
            var result = await new GeneradorDocumentos().CrearDocumento(htmlData);
            return result;
        }
    }
}
