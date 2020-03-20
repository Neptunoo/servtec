using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecnocomWrapper.unicard.sdk.Clients;
using TecnocomWrapper.unicard.sdk.Models;

namespace TecnocomWrapper.unicard.sdk.Tests
{
    /// <summary>
    /// Summary description for TecnocomCemo0reWrapperClientTests
    /// </summary>
    [TestClass]
    public class TecnocomCecl0tcWrapperClientTests
    {
        
        [TestMethod]
        public void RelacionTarjetasCuenta_Integration_ObtienetarjetasAsociadas()
        {
            var  service = new TecnocomCecl0tcWrapperClient();
            var tarjetas = service.ObtieneTarjetasAsociadas("0001", "000000000250");

            Assert.IsTrue(tarjetas.Length >= 2);
        }

      
    }
}
