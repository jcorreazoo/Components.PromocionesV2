using ZooLogicSA.Promociones.Informantes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;
using Rhino.Mocks;

namespace ZooLogicSA.Promociones.Tests
{
    /// <summary>
    ///This is a test class for InformantePromocionesTest and is intended
    ///to contain all InformantePromocionesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InformantePromocionesTest
    {
        [TestMethod()]
        public void InformarItemPromocionadoTest()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
            config.ConfiguracionesPorParticipante.Add( "ITEM", new ConfiguracionPorParticipante() { ConsumePorMonto = false } );

            InformantePromociones target = new InformantePromociones( config );

            IParticipante participante = MockRepository.GenerateMock<IParticipante>();
            participante.Expect( x => x.Id ).Return( "guid-locura" );
            participante.Expect( x => x.Clave ).Return( "ITEM" );
            participante.Expect( x => x.Cantidad ).Return( 1 );

            Beneficio beneficio = null;

            CoincidenciaEvaluacion c = new CoincidenciaEvaluacion();

			InformacionPromocion info = new InformacionPromocion( "1" );
            target.InformarAfectacion( info, new Promocion { Id = "1" }, 1 );
            target.InformarItemAfectado( info, "1", c, participante );
            target.InformarItemBeneficiado( info, "1", participante, beneficio );

            Assert.AreEqual( 1, info.DetalleBeneficiado.Count );

            Assert.AreEqual( "guid-locura", info.DetalleBeneficiado[0].Id, "Mal Id" );
            Assert.AreEqual( "guid-locura", info.DetalleBeneficiado[0].Id, "Mal Id" );
            Assert.AreEqual( 1, info.DetalleBeneficiado[0].Cantidad, "Mal cantidad" );
            Assert.AreEqual( Alteracion.Ninguna, info.DetalleBeneficiado[0].Alteracion, "Mal alteracion" );
            Assert.AreEqual( null, info.DetalleBeneficiado[0].AtributoAlterado, "Mal atributoalterado" );
            Assert.AreEqual( null, info.DetalleBeneficiado[0].Valor, "Mal valor" );
        }

        [TestMethod()]
        public void Informar2Items1PromoTest()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
            config.ConfiguracionesPorParticipante.Add( "item-locura", new ConfiguracionPorParticipante() { ConsumePorMonto = false});

            InformantePromociones target = new InformantePromociones( config );

            IParticipante participante = MockRepository.GenerateMock<IParticipante>();
            participante.Expect( x => x.Id ).Return( "guid-locura" );
            participante.Expect( x => x.Clave ).Return( "item-locura" );
            participante.Expect( x => x.Cantidad ).Return( 1 );

            IParticipante participante2 = MockRepository.GenerateMock<IParticipante>();
            participante2.Expect( x => x.Id ).Return( "guid-locura2" );
            participante2.Expect( x => x.Clave ).Return( "item-locura" );
            participante2.Expect( x => x.Cantidad ).Return( 1 );

            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Valor = 30;
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "DESCUENTO";

            CoincidenciaEvaluacion c = new CoincidenciaEvaluacion();

			InformacionPromocion info = new InformacionPromocion( "1" );

            target.InformarAfectacion( info, new Promocion { Id = "1" }, 1 );
            target.InformarItemAfectado( info, "1", c, participante );
            target.InformarItemBeneficiado( info, "1", participante, beneficio );
            target.InformarItemAfectado( info, "1", c, participante2 );
            target.InformarItemBeneficiado( info, "1", participante2, beneficio );

            Assert.AreEqual( 2, info.DetalleBeneficiado.Count, "Mal cantidad de items" );

            Assert.AreEqual( "guid-locura", info.DetalleBeneficiado[0].Id, "Mal Id beneficiado 1" );
            Assert.AreEqual( "item-locura", info.DetalleBeneficiado[0].Clave, "Mal clave beneficiado 1" );
            Assert.AreEqual( 1, info.DetalleBeneficiado[0].Cantidad, "Mal cantidad beneficiado 1" );
            Assert.AreEqual( Alteracion.CambiarValor, info.DetalleBeneficiado[0].Alteracion, "Mal alteracion beneficiado 1" );
            Assert.AreEqual( "DESCUENTO", info.DetalleBeneficiado[0].AtributoAlterado, "Mal atributoalterado beneficiado 1" );
            Assert.AreEqual( 30, info.DetalleBeneficiado[0].Valor, "Mal valor beneficiado 1" );

            Assert.AreEqual( "guid-locura2", info.DetalleBeneficiado[1].Id, "Mal Id beneficiado 2" );
            Assert.AreEqual( "item-locura", info.DetalleBeneficiado[0].Clave, "Mal clave beneficiado 2" );
            Assert.AreEqual( 1, info.DetalleBeneficiado[1].Cantidad, "Mal cantidad beneficiado 2" );
            Assert.AreEqual( Alteracion.CambiarValor, info.DetalleBeneficiado[1].Alteracion, "Mal alteracion beneficiado 2" );
            Assert.AreEqual( "DESCUENTO", info.DetalleBeneficiado[1].AtributoAlterado, "Mal atributoalterado beneficiado 2" );
            Assert.AreEqual( 30, info.DetalleBeneficiado[1].Valor, "Mal valor beneficiado 2" );
        }

        [TestMethod()]
        public void Informar2Items1CadaPromoTest()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
            config.ConfiguracionesPorParticipante.Add( "Item", new ConfiguracionPorParticipante() { ConsumePorMonto = false } );

            InformantePromociones target = new InformantePromociones( config );

            IParticipante participante = MockRepository.GenerateMock<IParticipante>();
            participante.Expect( x => x.Id ).Return( "guid-locura" );
            participante.Expect( x => x.Clave ).Return( "Item" );
            participante.Expect( x => x.Cantidad ).Return( 1 );

            IParticipante participante2 = MockRepository.GenerateMock<IParticipante>();
            participante2.Expect( x => x.Id ).Return( "guid-locura2" );
            participante2.Expect( x => x.Clave ).Return( "Item" );
            participante2.Expect( x => x.Cantidad ).Return( 1 );

            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Valor = "30";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Atributo = "DESCUENTO";

            CoincidenciaEvaluacion co = new CoincidenciaEvaluacion();

			InformacionPromocion info = new InformacionPromocion( "promo1" );
            target.InformarAfectacion( info, new Promocion { Id = "promo1" }, 1 );
            target.InformarItemAfectado( info, "promo1", co, participante );
            target.InformarItemBeneficiado( info, "promo1", participante, beneficio );

			InformacionPromocion info2 = new InformacionPromocion( "promo2" );
            target.InformarAfectacion( info2, new Promocion { Id = "promo2" }, 1 );
            target.InformarItemAfectado( info2, "promo2", co, participante2 );
            target.InformarItemBeneficiado( info2, "promo2", participante2, beneficio );

            Assert.AreEqual( 1, info.DetalleBeneficiado.Count, "Mal cantidad de items promo1" );
            Assert.AreEqual( "promo1", info.IdPromocion, "Mal id promo1" );

            Assert.AreEqual( 0, info.DetalleAfectado.Count, "Mal afectado promo 1" );

            Assert.AreEqual( "guid-locura", info.DetalleBeneficiado[0].Id, "Mal Id item promo 1" );
            Assert.AreEqual( 1, info.DetalleBeneficiado[0].Cantidad, "Mal cantidad promo 1" );
            Assert.AreEqual( Alteracion.CambiarValor, info.DetalleBeneficiado[0].Alteracion, "Mal alteracion promo 1" );
            Assert.AreEqual( "DESCUENTO", info.DetalleBeneficiado[0].AtributoAlterado, "Mal atributoalterado promo 1" );
            Assert.AreEqual( "30", info.DetalleBeneficiado[0].Valor, "Mal valor promo 1" );

            Assert.AreEqual( 0, info2.DetalleAfectado.Count, "Mal afectado promo 1" );

            Assert.AreEqual( 1, info2.DetalleBeneficiado.Count, "Mal cantidad de items promo2" );
            Assert.AreEqual( "promo2", info2.IdPromocion, "Mal id promo2" );

            Assert.AreEqual( "guid-locura2", info2.DetalleBeneficiado[0].Id, "Mal Id item promo2" );
            Assert.AreEqual( 1, info2.DetalleBeneficiado[0].Cantidad, "Mal cantidad promo2" );
            Assert.AreEqual( Alteracion.CambiarValor, info2.DetalleBeneficiado[0].Alteracion, "Mal alteracion promo2" );
            Assert.AreEqual( "DESCUENTO", info2.DetalleBeneficiado[0].AtributoAlterado, "Mal atributoalterado promo2" );
            Assert.AreEqual( "30", info2.DetalleBeneficiado[0].Valor, "Mal valor promo2" );
        }
    
    }
}
