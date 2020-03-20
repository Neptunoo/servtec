using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecnocomWrapper.unicard.sdk.Clients
{
    public struct TipoRetornoServicio
    {
        public const string Exito = "000";
        public const string ErrorEstructura = "100";
        public const string ErrorTipoOperacion = "120";
        public const string ErrorPeticionIncorrecta = "130";
        public const string ErrorEstructuraMensaje = "140";
        public const string ErrorFormatoXML = "160";
        public const string ErrorEjecucionServicio = "200";
        public const string NoSeEncontraronDatos = "210";
        public const string ErrorTecnico = "220";


    }
}
