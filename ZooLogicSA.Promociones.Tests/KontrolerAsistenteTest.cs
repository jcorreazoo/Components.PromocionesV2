using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Asistente;
using ZooLogicSA.Promociones.Informantes;
using System.Collections.Generic;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Core.Visual;
using System.ComponentModel;
using System.Windows.Forms;
using Rhino.Mocks;
using System.Drawing;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class KontrolerAsistenteTest
    {
        [TestMethod]
        public void ListaDePromocionesALista()
        {
            Aspecto asp = new Aspecto();

            ParametrosAsistente param = new ParametrosAsistente();

            KontrolerAsistente kontroler = new KontrolerAsistente( asp );
            List<InformacionPromocion> Lista = new List<InformacionPromocion>();

            InformacionPromocion info1 = new InformacionPromocion("id1");

            info1.Promocion = new Promocion();
            info1.Promocion.Descripcion = "Desc1";
            info1.MontoBeneficio = 400;

            Lista.Add( info1 );

            InformacionPromocion info2 = new InformacionPromocion("id2");

            info2.Promocion = new Promocion();
            info2.Promocion.Descripcion = "Desc2";
            info2.MontoBeneficio = 500;

            Lista.Add( info2 );


            BindingList<PromocionesEstado> bindinglist = new BindingList<PromocionesEstado>();
            BindingSource source = new BindingSource( bindinglist, null );

            kontroler.AgregarPromociones( Lista, source );
			kontroler.AgregarListaPromocionesASource( source );

            Assert.AreEqual( 2, source.Count );
        }

        [TestMethod]
        public void ListaDePromocionesCumplidasParcialmenteALista()
        {
            Aspecto asp = new Aspecto();

            KontrolerAsistente kontroler = new KontrolerAsistente(asp);
            List<InformacionPromocionIncumplida> Lista = new List<InformacionPromocionIncumplida>();

            InformacionPromocionIncumplida info1 = new InformacionPromocionIncumplida();

            info1.Resultados = new List<ResultadoReglas>();
            info1.Resultados.Add( new ResultadoReglas() { Satisfecho = 1, PartPromo = new ParticipanteRegla() { Codigo = "COMPROBANTE.FACTURADETALLE.ITEM" } } );
            info1.IdPromocion = "id1";
            info1.Promocion = new Promocion() { Descripcion = "IdPromocion" };
            info1.Resultados.Add( new ResultadoReglas() { Cumple = false } );
            info1.Resultados.Add( new ResultadoReglas() { Cumple = true } );

            Lista.Add(info1);
            IArmadorDeLeyenda armador = MockRepository.GenerateMock<IArmadorDeLeyenda>();
            kontroler.armador = armador;
            armador.Expect(x => x.Armar(info1)).Return("");

             BindingList<PromocionesEstado> bindinglist = new BindingList<PromocionesEstado>();
            BindingSource source = new BindingSource( bindinglist, null );


            kontroler.AgregarPromociones( Lista, source );
			kontroler.AgregarListaPromocionesASource( source );

            Assert.AreEqual( 1, source.Count );
            Assert.AreEqual( bindinglist[0].EstadoDibujo.GetPixel( 0, 0 ), ZooLogicSA.Promociones.Asistente.Properties.Resources.Amarillo.GetPixel( 0, 0 ) );

        }

        [TestMethod]
        public void ListaDePromocionesIncumplidasALista()
        {
            Aspecto asp = new Aspecto();

            KontrolerAsistente kontroler = new KontrolerAsistente(asp);
            List<InformacionPromocionIncumplida> Lista = new List<InformacionPromocionIncumplida>();

            InformacionPromocionIncumplida info1 = new InformacionPromocionIncumplida();

            info1.Resultados = new List<ResultadoReglas>();
            info1.IdPromocion = "id1";
            info1.Promocion = new Promocion() { Descripcion = "IdPromocion" };
            info1.Resultados.Add( new ResultadoReglas() { Cumple = false } );
            info1.Resultados.Add(new ResultadoReglas() { Cumple = false });
            IArmadorDeLeyenda armador = MockRepository.GenerateMock<IArmadorDeLeyenda>();
            kontroler.armador = armador;
            armador.Expect(x => x.Armar(info1)).Return("");

            Lista.Add(info1);

            BindingList<PromocionesEstado> bindinglist = new BindingList<PromocionesEstado>();
            BindingSource source = new BindingSource(bindinglist, null);


            kontroler.AgregarPromociones(Lista, source);
			kontroler.AgregarListaPromocionesASource( source );

            Assert.AreEqual(1, source.Count);
            Assert.AreEqual(bindinglist[0].EstadoDibujo.GetPixel(0, 0), ZooLogicSA.Promociones.Asistente.Properties.Resources.Blanco.GetPixel(0, 0));

        }

        [TestMethod]
        public void QuitarPromocionesCuandoNoDebenAparecer()
        {
            Aspecto asp = new Aspecto();

            KontrolerAsistente kontroler = new KontrolerAsistente( asp );
            List<InformacionPromocion> Lista = new List<InformacionPromocion>();

            InformacionPromocion info1 = new InformacionPromocion("id1");

            info1.Promocion = new Promocion();
            info1.Promocion.Descripcion = "Desc1";
            info1.MontoBeneficio = 400;

            Lista.Add( info1 );

            InformacionPromocion info2 = new InformacionPromocion("id2");

            info2.Promocion = new Promocion() { Visualizacion = VisualizacionPromocionAsistenteType.NoVisible };
            info2.Promocion.Descripcion = "Desc2";
            info2.MontoBeneficio = 500;

            Lista.Add( info2 );
            BindingList<PromocionesEstado> bindinglist = new BindingList<PromocionesEstado>();
            BindingSource source = new BindingSource( bindinglist, null );
            kontroler.AgregarPromociones( Lista, source );

            List<InformacionPromocionIncumplida> listaIncumplida = new List<InformacionPromocionIncumplida>();
            kontroler.AgregarPromociones( listaIncumplida, source );
			kontroler.AgregarListaPromocionesASource( source );

            Assert.AreEqual( 1, source.Count );
        }

        [TestMethod]
        public void AjustarFormularioAsistente_IgualResolucion()
        {
            Aspecto asp = new Aspecto();
            Rectangle rec = new Rectangle();
            rec.Width = 500;

            ParametrosAsistente paramAsistente = new ParametrosAsistente();
            paramAsistente.LeftComprobante = 0;
            paramAsistente.WidthComprobante = 500;

            FrmAsistente formuAsistente = new FrmAsistente();

            formuAsistente.AjustarUbicacion( paramAsistente, rec );

            Assert.AreEqual( 185, formuAsistente.Left );
        }

        [TestMethod]
        public void AjustarFormularioAsistente_MenorResolucion()
        {
            Aspecto asp = new Aspecto();
            Rectangle rec = new Rectangle();
            rec.Width = 499;

            ParametrosAsistente paramAsistente = new ParametrosAsistente();
            paramAsistente.LeftComprobante = 0;
            paramAsistente.WidthComprobante = 500;

            FrmAsistente formuAsistente = new FrmAsistente();

            formuAsistente.AjustarUbicacion( paramAsistente, rec );

            Assert.AreEqual( 185, formuAsistente.Left );
        }
         [TestMethod]
        public void AjustarFormularioAsistente_ResolucionMayorAlFomulario()
        {
            Aspecto asp = new Aspecto();
            Rectangle rec = new Rectangle();
            rec.Width = 600;

            ParametrosAsistente paramAsistente = new ParametrosAsistente();
            paramAsistente.LeftComprobante = 50;
            paramAsistente.WidthComprobante = 500;

            FrmAsistente formuAsistente = new FrmAsistente();

            formuAsistente.AjustarUbicacion( paramAsistente, rec );

            Assert.AreEqual( 550, formuAsistente.Left );
        }

    }
}
