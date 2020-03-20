using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecnocomWrapper.unicard.sdk.ListaLimitesYSaldosPorReferencia;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public interface ITecnocomCecl0lsWrapperClient
    {
        Respuesta_CECL0LS_Registro_CECL0LS[] ObtieneLineasPorContrato(string cuenta, string centroAlta, int lineaReferencia);

        Respuesta_CECL0LS_Registro_CECL0LS ObtineLineaPorContrato(string cuenta, string centroAlta, string idTipoLinea,
            int lineaReferencia);
    }
}
