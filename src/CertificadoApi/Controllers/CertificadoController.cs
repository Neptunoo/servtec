using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using CertificadoApi.Core.Aplicacion.Certificados.Querys;
using CertificadoApi.Infraestructura.Resutlts;

namespace CertificadoApi.Controllers
{
    public class CertificadoController : ApiController
    {
        public IHttpActionResult Post([FromBody] CertificadoQuery payload)
        {
            return new PdfResult(new MemoryStream(new CertificadoQueryHandler().Handle(payload)), Request, "Certificado.pdf");
        }
    }
}
