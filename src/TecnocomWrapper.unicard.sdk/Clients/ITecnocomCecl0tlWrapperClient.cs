using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecnocomWrapper.unicard.sdk.AutorizadorLineas;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public interface ITecnocomCecl0tlWrapperClient
    {
        Respuesta_CECL0TL_Registro_CECL0TL[] ObtieneLineasPorContrato(string cuenta, string centroAlta);
        Respuesta_CECL0TL_Registro_CECL0TL ObtineLineaPorContrato(string cuenta, string centroAlta, string idTipoLinea);
    }
}
