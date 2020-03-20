using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecnocomWrapper.unicard.sdk.BusquedaPersonasPorPan;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public interface ITecnocomCeco0p2WrapperClient
    {
        Respuesta_CECO0P2_Registro_CECO0P2[] ObtienePersonasPorPan(string pan);
    }
}
