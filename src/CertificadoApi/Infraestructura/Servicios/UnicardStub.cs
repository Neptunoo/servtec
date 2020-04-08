using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CertificadoApi.Comun.Helpers;
using TecnocomWrapper.unicard.sdk.Clients;
using TecnocomWrapper.unicard.sdk.PrePagoDiezDias;

namespace CertificadoApi.Infraestructura.Servicios
{
    public class UnicardStub
    {
        public Respuesta_CECO005_Registro_CECO005 ObtenerPrepagoData(int rut)
        {
            var acuseRecibo = new TecnocomCecourpWrapperClient();
            var t = acuseRecibo.ObtienePansPorRutOPan($"{rut}-{RutHelper.Dv(rut)}").FirstOrDefault();

            string cuenta = t.cuenta;
            string pan = t.pan;

            var prepago = new TecnocomCeco005WrapperClient();
            return prepago.PrePagoDiezDias(cuenta, pan).FirstOrDefault();
        }
    }

    public class PrePagoData
    {
        public double Comision { get; set; }
        public string DeudaAlDiaEmision { get; set; }
        public string ProyeccionInteres { get; set; }
        public string ProyeccionDeuda { get; set; }
        public string MontoTotalAPagar { get; set; }
    }
}