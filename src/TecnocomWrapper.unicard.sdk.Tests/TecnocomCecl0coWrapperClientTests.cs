using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecnocomWrapper.unicard.sdk.Clients;

namespace TecnocomWrapper.unicard.sdk.Tests
{
    [TestClass]
    public class TecnocomCecl0coWrapperClientTests
    {
        [TestMethod]
        public void ConsultaMediosContactoPersona()
        {
            var service = new TecnocomCecl0coWrapperClient();
            var medioContactoPersona = service.ObtieneMediosContactoPersona("00235122", 1).FirstOrDefault();
            var recibir = medioContactoPersona.fecalta;
            Debug.WriteLine("Fecha Alta: " + recibir.anno+"-"+recibir.mes+"-"+recibir.dia+"||"+medioContactoPersona.tipmedio);

        }
    }
}
