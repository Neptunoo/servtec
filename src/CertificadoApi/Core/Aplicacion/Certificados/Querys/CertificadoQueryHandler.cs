using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using CertificadoApi.Comun.Helpers;
using CertificadoApi.Core.Model;
using CertificadoApi.Infraestructura.Persistencia.AtencionCliente;
using CertificadoApi.Infraestructura.Persistencia.Canales;
using CertificadoApi.Infraestructura.Servicios;

namespace CertificadoApi.Core.Aplicacion.Certificados.Querys
{
    public class CertificadoQueryHandler
    {
        private readonly CanalesDbContext canalDb;
        private readonly AtencionClienteDbContext atencionClienteDb;
        private readonly InformacionFinanciera iffService;
        private readonly UnicardStub unicardService;
        private readonly ConvertidorPdfService pdfService;
        public CertificadoQueryHandler()
        {
            this.canalDb = new CanalesDbContext();
            this.atencionClienteDb = new AtencionClienteDbContext();
            this.iffService = new InformacionFinanciera();
            this.unicardService = new UnicardStub();
            this.pdfService = new ConvertidorPdfService();
        }


        public byte[] Handle(CertificadoQuery request)
        {

            var cliente = canalDb.Obtener(request.Rut);
            var cuenta = canalDb.ObtenerCuenta(request.Rut);
            var fechasData = iffService.ObtenerInformacionFinanciera(request.Rut, request.Canal);
            var pagoDd = unicardService.ObtenerPrepagoData(request.Rut);

            var data = new CertificadoData
            {
                Rut = $"{request.Rut}-{RutHelper.Dv(request.Rut)}",
                Nombre = $"{cliente.NombreCliente} {cliente.ApPeterno} {cliente.ApMaterno}",
                NumeroOperacion = atencionClienteDb.ObtenerNumeroCertificado().ToString(),
                TipoPago = request.TipoPago,
                FechaAperturaContrato = cuenta.FecApertura.ToString("dd-MM-yyyy"),
                FechaUltimaFacturacion = FechasHelper.Formatear(fechasData.fechaFacturacion),
                FechaUltimoVencimiento = FechasHelper.Formatear(fechasData.fechaVencimiento),
                ComisionPorPagoAnticipado = pagoDd.comis_prepago.ToString(),
                DeudaAlDiaEmision = pagoDd.dtotal1.ToString(),
                ProyeccionInteres = pagoDd.intere1.ToString(),
                ProyeccionDeudaPeriodoActualHasta = pagoDd.dcuper1.ToString(),
                MontoTotalPagarFechaProyeccion = pagoDd.salact1.ToString()
            };

            return pdfService.GenerarPdf(data);

        }
    }
}