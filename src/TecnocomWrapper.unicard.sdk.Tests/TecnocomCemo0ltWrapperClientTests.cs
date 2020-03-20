using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecnocomWrapper.unicard.sdk.AutorizadorLineas;
using TecnocomWrapper.unicard.sdk.Clients;
using TecnocomWrapper.unicard.sdk.Models;
using TecnocomWrapper.unicard.sdk.ModificacionLineaContrato;

namespace TecnocomWrapper.unicard.sdk.Tests
{
    
    [TestClass]
    public class TecnocomCemo0ltWrapperClientTests
    {
     
        [TestMethod]
        public void Integracion_AumentaCupoLinea()
        {

            var sLineas = new TecnocomCecl0tlWrapperClient();//"Lista De Referencias De Tipo De Linea Por Contrato"
            var sLimitesSaldos = new TecnocomCecl0lsWrapperClient();//"Lista de límites y saldos por referencia"
            var sAumentoCupo = new TecnocomCemo0ltWrapperClient();

            var cuenta = "000000000004";
            var centroAlta = "0001";
            var tipoLinea = "LNAV";

            var lineas = sLineas.ObtineLineaPorContrato(cuenta, centroAlta, tipoLinea);
            var limites = sLimitesSaldos.ObtineLineaPorContrato(cuenta, centroAlta, lineas.tipolin1, lineas.linref1);

            var linea = new ModificarLimiteLineaInputModel();

            linea.IdTipoLinea = lineas.tipolin1;
            linea.ContadorConcurrencia = limites.contcur;
            linea.CentroAlta = centroAlta;
            linea.Cuenta = cuenta;
            linea.Importe = limites.limcrelin - (double)1;
            linea.LineaReferencia = lineas.linref1;

            sAumentoCupo.ModificarLimiteDeLinea(linea);

        }



        [TestMethod]
        public void Integracion_VerCupoLinea()
        {

            var sLineas = new TecnocomCecl0tlWrapperClient();//"Lista De Referencias De Tipo De Linea Por Contrato"
            var sLimitesSaldos = new TecnocomCecl0lsWrapperClient();//"Lista de límites y saldos por referencia"
            var sAumentoCupo = new TecnocomCemo0ltWrapperClient();

            var cuenta = "000000000707";
            var centroAlta = "0001";
            var tipoLinea = "LSAV";

            var lineas = sLineas.ObtineLineaPorContrato(cuenta, centroAlta, tipoLinea);
            var limites = sLimitesSaldos.ObtineLineaPorContrato(cuenta, centroAlta, lineas.tipolin1, lineas.linref1);

            //string a = "a";

             Assert.IsInstanceOfType(lineas, typeof(Respuesta_CECL0TL_Registro_CECL0TL));
        }

    }
}
