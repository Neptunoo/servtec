using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecnocomWrapper.unicard.sdk.Clients;
using TecnocomWrapper.unicard.sdk.Models;
using System.Linq;
using TecnocomWrapper.unicard.sdk.ConsultaPansPorRut;
using System.Diagnostics;

namespace TecnocomWrapper.unicard.sdk.Tests
{
    /// <summary>
    /// Summary description for TecnocomCemo0reWrapperClientTests
    /// </summary>
    [TestClass]
    public class TecnocomCecourpWrapperClientTests
    {   
        [TestMethod]
        public void ConsultaPansPorRut_Integracion_BuscaPorRut()
        {
            var  acuseRecibo = new TecnocomCecourpWrapperClient();

            //var t = acuseRecibo.ObtienePansPorRutOPan("12345876-1").FirstOrDefault();
            var t = acuseRecibo.ObtienePansPorRutOPan("8191479-6").FirstOrDefault();
           
            Debug.WriteLine("Cliente Pan:"+ t.pan);

            // Assert
            Assert.IsInstanceOfType(t, typeof(Respuesta_CECOURP_Registro_CECOURP));
     
        }

        [TestMethod]
        public void ConsultaPansPorRut_Integracion_BuscaPorPan()
        {
            var acuseRecibo = new TecnocomCecourpWrapperClient();
            var t = acuseRecibo.ObtienePansPorRutOPan("6299604900001658").FirstOrDefault();

            // Assert
            Assert.IsInstanceOfType(t, typeof(Respuesta_CECOURP_Registro_CECOURP));
        }



    }
}
