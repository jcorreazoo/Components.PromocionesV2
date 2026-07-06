using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.UI.Clases.Managers;
using ZooLogicSA.Promociones.UI.Controllers;
using Rhino.Mocks;
using ZooLogicSA.Promociones.Negocio.Clases.Promociones;
using System.Collections.Generic;
using System;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class ManagerValidacionesTest
    {
        private ManagerValidaciones _manager;
        private IControllerPromocion _control;

        [TestInitialize]
        public void TestInit()
        {
            _manager = new ManagerValidaciones();
            _control = MockRepository.GenerateMock<IControllerPromocion>();
        }

        [TestCleanup]
        public void TestFinish()
        {
            _control.VerifyAllExpectations();
        }

        [TestMethod]
        public void ValidarCantidadCondicionesDeValores_Ok()
        {           
            DescuentoXcaracteristica promo = new DescuentoXcaracteristica();
            _control.Expect(x => x.ObtenerTipoPromocionSeleccionada()).Return(promo);
            _control.Expect(x => x.ObtenerListaCondicionesSegunTipoPromocion(promo )).Return(new List<String>() { "[VALOR.CODIGO] = '0'", "[ARTICULO.CODIGO] = '00100101'" });
            _control.Expect(x => x.ObtenerTipoDetalleDelParticipante("VALOR.CODIGO")).Return("VALORESDETALLE");
            _control.Expect(x => x.ObtenerTipoDetalleDelParticipante("ARTICULO.CODIGO")).Return("FACTURADETALLE");

            bool resultado = _manager.ValidarCantidadCondicionesDeValores(_control);
            Assert.IsTrue( resultado, "La validación debería ser true" );
        }

        [TestMethod]
        public void ValidarCantidadCondicionesDeValores_Error()
        {
            DescuentoXcaracteristica promo = new DescuentoXcaracteristica();
            _control.Expect(x => x.ObtenerTipoPromocionSeleccionada()).Return(promo);
            _control.Expect(x => x.ObtenerListaCondicionesSegunTipoPromocion(promo)).Return(new List<String>() { "[VALOR.CODIGO] = '1'", "[VALOR.CODIGO] = '0'", "[ARTICULO.CODIGO] = '00100101'" });
            _control.Expect(x => x.ObtenerTipoDetalleDelParticipante("VALOR.CODIGO")).Return("VALORESDETALLE");
            _control.Expect(x => x.ObtenerTipoDetalleDelParticipante("ARTICULO.CODIGO")).Return("FACTURADETALLE");

            bool resultado = _manager.ValidarCantidadCondicionesDeValores(_control);
            Assert.IsFalse(resultado, "La validación debería ser false");
        }

        [TestMethod]
        public void ValidarMediosDePagoYPromoAutomatica_Ok_PromoNoAutomatica_ConValores()
        {
            DescuentoXcaracteristica promo = new DescuentoXcaracteristica();
            _control.Expect(x => x.ObtenerAplicaAutomaticamente()).Return(false);

            bool resultado = _manager.ValidarMediosDePagoYPromoAutomatica(_control);
            Assert.IsTrue(resultado, "La validación debería ser true");
        }

        [TestMethod]
        public void ValidarMediosDePagoYPromoAutomatica_Ok_PromoAutomaticaSinValores()
        {
            DescuentoXcaracteristica promo = new DescuentoXcaracteristica();
            _control.Expect(x => x.ObtenerTipoPromocionSeleccionada()).Return(promo);
            _control.Expect(x => x.ObtenerListaCondicionesSegunTipoPromocion(promo)).Return(new List<String>() { "[ARTICULO.CODIGO] = '00100101'" });
            _control.Expect(x => x.ObtenerTipoDetalleDelParticipante("ARTICULO.CODIGO")).Return("FACTURADETALLE");
            _control.Expect(x => x.ObtenerAplicaAutomaticamente()).Return(true);

            bool resultado = _manager.ValidarMediosDePagoYPromoAutomatica(_control);
            Assert.IsTrue(resultado, "La validación debería ser true");
        }

        [TestMethod]
        public void ValidarMediosDePagoYPromoAutomatica_Error_PromoAutomaticaConValores()
        {
            DescuentoXcaracteristica promo = new DescuentoXcaracteristica();
            _control.Expect(x => x.ObtenerTipoPromocionSeleccionada()).Return(promo);
            _control.Expect(x => x.ObtenerListaCondicionesSegunTipoPromocion(promo)).Return(new List<String>() { "[VALOR.CODIGO] = '0'", "[ARTICULO.CODIGO] = '00100101'" });
            _control.Expect(x => x.ObtenerTipoDetalleDelParticipante("VALOR.CODIGO")).Return("VALORESDETALLE");
            _control.Expect(x => x.ObtenerTipoDetalleDelParticipante("ARTICULO.CODIGO")).Return("FACTURADETALLE");
            _control.Expect(x => x.ObtenerAplicaAutomaticamente()).Return(true);

            bool resultado = _manager.ValidarMediosDePagoYPromoAutomatica(_control);
            Assert.IsFalse(resultado, "La validación debería ser false");
        }

        [TestMethod]
        public void ValidarExistenciaParticipanteTipoDetalle_Ok()
        {
            DescuentoXcaracteristica promo = new DescuentoXcaracteristica();
            _control.Expect(x => x.ObtenerTipoPromocionSeleccionada()).Return(promo);
            _control.Expect(x => x.ObtenerListaCondicionesSegunTipoPromocion(promo)).Return(new List<String>() { "[ARTICULO.CODIGO] = '00100101'" });
            _control.Expect(x => x.ObtenerTipoDetalleDelParticipante("ARTICULO.CODIGO")).Return("FACTURADETALLE");

            bool resultado = _manager.ValidarExistenciaParticipanteTipoDetalle(_control);
            Assert.IsTrue(resultado, "La validación debería ser true");
        }

        [TestMethod]
        public void ValidarExistenciaParticipanteTipoDetalle_Error()
        {
            DescuentoXcaracteristica promo = new DescuentoXcaracteristica();
            _control.Expect(x => x.ObtenerTipoPromocionSeleccionada()).Return(promo);
            _control.Expect(x => x.ObtenerListaCondicionesSegunTipoPromocion(promo)).Return(new List<String>() { "[VALOR.CODIGO] = '0'" });
            _control.Expect(x => x.ObtenerTipoDetalleDelParticipante("VALOR.CODIGO")).Return("VALORESDETALLE");

            bool resultado = _manager.ValidarExistenciaParticipanteTipoDetalle(_control);
            Assert.IsFalse(resultado, "La validación debería ser false");
        }       

    }
}
