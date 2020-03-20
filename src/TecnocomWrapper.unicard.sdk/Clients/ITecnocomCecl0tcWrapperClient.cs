using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecnocomWrapper.unicard.sdk.RelacionTarjetasCuenta;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public interface ITecnocomCecl0tcWrapperClient
    {
        Respuesta_CECL0TC_Registro_CECL0TC[] ObtieneTarjetasAsociadas(string centalta, string cuenta);
    }
}
