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
using TecnocomWrapper.unicard.sdk.Clients;
using TecnocomWrapper.unicard.sdk.PrePagoDiezDias;

namespace LiquidacionDeudaApi.Controllers
{
    public class LiquidacionesController : ApiController
    {
        public async Task<IHttpActionResult> Get([FromBody] RequestLiquidacion payload)
        {
            ResponseCliente respuesta = new ResponseCliente();
            ResponseLiquidacion resposneLiquidacion = new ResponseLiquidacion();
            Neg_Cupon logic = new Neg_Cupon();


            var tecnocomCeco005WrapperClient = new TecnocomCeco005WrapperClient();
            var prepagoDd = tecnocomCeco005WrapperClient.PrePagoDiezDias(payload.Cuenta, payload.Pan).FirstOrDefault();
            if (prepagoDd != null)
            {
                respuesta = await logic.ObtenerClienteCupon(respuesta, payload.RutCliente, payload.Canal);
                resposneLiquidacion.PdfBuffer = await GenerarPdf(respuesta, prepagoDd);
            }
            else
            {
                throw new InvalidEnumArgumentException("No encontrado");
            }

            return Ok(resposneLiquidacion);
        }

        private string GenerarHtml(ResponseCliente respuesta, Respuesta_CECO005_Registro_CECO005 registro)
        {
            return $"<html><body></body></html>";
        }

        private async Task<byte[]> GenerarPdf(ResponseCliente respuesta, Respuesta_CECO005_Registro_CECO005 registro)
        {
            var htmlData = GenerarHtml(respuesta, registro);
            var result = await new GeneradorDocumentos().CrearDocumento(htmlData);
            return result;
        }
    }
}
