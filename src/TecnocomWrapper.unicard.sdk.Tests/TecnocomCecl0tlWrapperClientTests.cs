using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecnocomWrapper.unicard.sdk.AutorizadorLineas;
using TecnocomWrapper.unicard.sdk.Clients;

namespace TecnocomWrapper.unicard.sdk.Tests
{
    [TestClass]
    public class TecnocomCecl0tlWrapperClientTests
    {
        [TestMethod]
        public void Integracion_ServicioCecl0tl_ObtieneLineas ()
        {
            var servicio1 = new TecnocomCecl0tlWrapperClient();
            var lineas = servicio1.ObtieneLineasPorContrato("000000000707", "0001");
            Assert.IsInstanceOfType(lineas, typeof(Respuesta_CECL0TL_Registro_CECL0TL[]));
        }

        [TestMethod]
        public void Integracion_ServicioCecl0tl_ObtieneLineaSAV()
        {
            var servicio1 = new TecnocomCecl0tlWrapperClient();
            var lineas = servicio1.ObtineLineaPorContrato("000000000707", "0001", "LSAV");
            Assert.IsInstanceOfType(lineas, typeof(Respuesta_CECL0TL_Registro_CECL0TL));
        }

    }
}
