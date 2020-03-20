using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecnocomWrapper.unicard.sdk.Models;
using TecnocomWrapper.unicard.sdk.ModificacionLineaContrato;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public interface ITecnocomCemo0ltWrapperClient
    {
        Respuesta_CEMO0LT_Registro_CEMO0LT[] ModificarLimiteDeLinea(ModificarLimiteLineaInputModel linea);
    }
}
