using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecnocomWrapper.unicard.sdk.AcuseDeRecibo;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public interface ITecnocomCemo0reWrapperClient
    {
        Respuesta_CEMO0RE_Registro_CEMO0RE[] AcusaRecibo(string pan);
    }
}
