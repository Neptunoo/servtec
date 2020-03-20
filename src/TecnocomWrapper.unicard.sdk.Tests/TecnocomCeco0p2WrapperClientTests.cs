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
    public class TecnocomCeco0p2WrapperClientTests
    {
        [TestMethod]
        public void BuscarClientePorPan()
        {
            var servicio = new TecnocomCeco0p2WrapperClient();
            var respuesta = servicio.ObtienePersonasPorPan("6299604543738336").FirstOrDefault();

            Debug.WriteLine("Identificador Cliente:" + respuesta.identcli);

            //Assert.IsInstanceOfType(lineas, typeof(Respuesta_CECL0TL_Registro_CECL0TL[]));
        }

    }
}
