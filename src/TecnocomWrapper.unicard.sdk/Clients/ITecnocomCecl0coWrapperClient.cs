using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecnocomWrapper.unicard.sdk.ConsultaMediosContactoPersona;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public interface ITecnocomCecl0coWrapperClient
    {
        Respuesta_CECL0CO_Registro_CECL0CO[] ObtieneMediosContactoPersona(String rut, int pagina);
    }
}
