using System;
using System.IO;
using CertificadoApi.Core.Aplicacion.Certificados.Querys;
using CertificadoApi.Infraestructura.Persistencia.AtencionCliente;
using CertificadoApi.Infraestructura.Persistencia.Canales;
using CertificadoApi.Infraestructura.Servicios;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Certificado.ApiTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var target = new CanalesDbContext();
            var resultado = target.Obtener(17246916);
            Assert.IsTrue(resultado != null);
        }

        [TestMethod]
        public void TestObtenerSecuencia()
        {
            var target = new AtencionClienteDbContext();
            var result = target.ObtenerNumeroCertificado();
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void TestObtenerCuenta()
        {
            var target = new CanalesDbContext();
            var result = target.ObtenerCuenta(12507027);
            Assert.IsTrue(result.FecApertura.Year == 2020);
        }

        [TestMethod]
        public void TestToken()
        {
            var target = new InformacionFinanciera();
            var result = target.ObtenerInformacionFinanciera(18882342, 240);
            Assert.IsTrue(result.fechaFacturacion > 0);
        }

        [TestMethod]
        public void TestUnicardStub()
        {
            var target = new UnicardStub();
            var result = target.ObtenerPrepagoData(11693100);
            Assert.IsTrue(result != null);
        }

        [TestMethod]
        public void TestCertificadoQueryHandler()
        {
            var target = new CertificadoQueryHandler();
            var result = target.Handle(new CertificadoQuery
            {
                Rut = 18882342,
                Canal = 240,
                TipoPago = "TOTAL"
            });
            File.WriteAllBytes(@"C:\temp\me2.pdf", result);
        }
     
    }
}
