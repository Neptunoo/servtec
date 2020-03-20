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
    public class TecnocomCemo0reWrapperClientTests
    {
        [Ignore]
        [TestMethod]
        public void AcuseRecibo_Integration()
        {
            var  acuseRecibo = new TecnocomCemo0reWrapperClient();
            var t = acuseRecibo.AcusaRecibo("6299604900001658");

        }
    }
}
