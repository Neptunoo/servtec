using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecnocomWrapper.unicard.sdk.ConsultaPansPorRut;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public interface ITecnocomCecourpWrapperClient
    {
        Respuesta_CECOURP_Registro_CECOURP[] ObtienePansPorRutOPan(string rutOPan);
    }
}
