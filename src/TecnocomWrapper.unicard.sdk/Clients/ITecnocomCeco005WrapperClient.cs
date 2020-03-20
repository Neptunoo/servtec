using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecnocomWrapper.unicard.sdk.PrePagoDiezDias;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public interface ITecnocomCeco005WrapperClient
    {
        Respuesta_CECO005_Registro_CECO005[] PrePagoDiezDias(string cuenta, string pan);
    }

}
