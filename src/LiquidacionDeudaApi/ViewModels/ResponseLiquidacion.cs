using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiquidacionDeudaApi.ViewModels
{
    public class ResponseLiquidacion
    {
        public string NombreArchivo { get; set; }
        public byte[] PdfBuffer { get; set; }

    }
}