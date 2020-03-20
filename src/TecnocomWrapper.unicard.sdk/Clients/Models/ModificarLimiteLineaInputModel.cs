using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecnocomWrapper.unicard.sdk.Models
{
    public class ModificarLimiteLineaInputModel
    {
        public string Cuenta { get; set; }
        public string IdTipoLinea { get; set; }
        public string CentroAlta { get; set; }
        public string ContadorConcurrencia { get; set; }
        public double Importe { get; set; }
        public int LineaReferencia { get; set; }

    }
}
