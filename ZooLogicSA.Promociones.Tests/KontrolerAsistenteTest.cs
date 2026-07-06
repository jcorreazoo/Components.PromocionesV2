using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Asistente;
using ZooLogicSA.Promociones.Informantes;
using System.Collections.Generic;
using System.Linq;
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

        // -----------------------------------------------------------
        // Tarea 3.1 — SatisfechoEfectivo determina Estado y EstadoDibujo
        // -----------------------------------------------------------

        [TestMethod]
        public void EstadoParcialCuandoSatisfechoEfectivoEsMayorACero()
        {
            Aspecto asp = new Aspecto();
            KontrolerAsistente kontroler = new KontrolerAsistente( asp );

            InformacionPromocionIncumplida info = new InformacionPromocionIncumplida();
            info.IdPromocion = "promo1";
            info.Promocion = new Promocion() { Descripcion = "Promo1" };
            info.Resultados = new List<ResultadoReglas>();
            info.SatisfechoEfectivo = 2m;

            IArmadorDeLeyenda armador = MockRepository.GenerateMock<IArmadorDeLeyenda>();
            kontroler.armador = armador;
            armador.Stub( x => x.Armar( info ) ).Return( "" );
            armador.Stub( x => x.ArmarLeyendaAMostrarEnControl( info ) ).Return( "" );

            BindingList<PromocionesEstado> bindinglist = new BindingList<PromocionesEstado>();
            BindingSource source = new BindingSource( bindinglist, null );

            kontroler.AgregarPromociones( new List<InformacionPromocionIncumplida>() { info }, source );
            kontroler.AgregarListaPromocionesASource( source );

            Assert.AreEqual( 1, source.Count );
            Assert.AreEqual( estado.Parcial, bindinglist[0].Estado );
            Assert.AreEqual(
                ZooLogicSA.Promociones.Asistente.Properties.Resources.Amarillo.GetPixel( 0, 0 ),
                bindinglist[0].EstadoDibujo.GetPixel( 0, 0 ) );
        }

        [TestMethod]
        public void EstadoIncumplidoCuandoSatisfechoEfectivoEsCero()
        {
            Aspecto asp = new Aspecto();
            KontrolerAsistente kontroler = new KontrolerAsistente( asp );

            InformacionPromocionIncumplida info = new InformacionPromocionIncumplida();
            info.IdPromocion = "promo1";
            info.Promocion = new Promocion() { Descripcion = "Promo1" };
            info.Resultados = new List<ResultadoReglas>()
            {
                new ResultadoReglas() { Satisfecho = 3, PartPromo = new ParticipanteRegla() { Codigo = "COMPROBANTE.FACTURADETALLE.ITEM" } }
            };
            info.SatisfechoEfectivo = 0m;

            IArmadorDeLeyenda armador = MockRepository.GenerateMock<IArmadorDeLeyenda>();
            kontroler.armador = armador;
            armador.Stub( x => x.Armar( info ) ).Return( "" );
            armador.Stub( x => x.ArmarLeyendaAMostrarEnControl( info ) ).Return( "" );

            BindingList<PromocionesEstado> bindinglist = new BindingList<PromocionesEstado>();
            BindingSource source = new BindingSource( bindinglist, null );

            kontroler.AgregarPromociones( new List<InformacionPromocionIncumplida>() { info }, source );
            kontroler.AgregarListaPromocionesASource( source );

            Assert.AreEqual( 1, source.Count );
            Assert.AreEqual( estado.Incumplida, bindinglist[0].Estado );
            Assert.AreEqual(
                ZooLogicSA.Promociones.Asistente.Properties.Resources.Blanco.GetPixel( 0, 0 ),
                bindinglist[0].EstadoDibujo.GetPixel( 0, 0 ) );
        }

        [TestMethod]
        public void EstadoParcialCuandoSatisfechoEfectivoEsCentinelaYResultadosSatisfechos()
        {
            Aspecto asp = new Aspecto();
            KontrolerAsistente kontroler = new KontrolerAsistente( asp );

            InformacionPromocionIncumplida info = new InformacionPromocionIncumplida();
            info.IdPromocion = "promo1";
            info.Promocion = new Promocion() { Descripcion = "Promo1" };
            info.Resultados = new List<ResultadoReglas>()
            {
                new ResultadoReglas() { Satisfecho = 2, PartPromo = new ParticipanteRegla() { Codigo = "COMPROBANTE.FACTURADETALLE.ITEM" } }
            };
            // SatisfechoEfectivo = -1 (centinela, no establecido por defecto)

            IArmadorDeLeyenda armador = MockRepository.GenerateMock<IArmadorDeLeyenda>();
            kontroler.armador = armador;
            armador.Stub( x => x.Armar( info ) ).Return( "" );
            armador.Stub( x => x.ArmarLeyendaAMostrarEnControl( info ) ).Return( "" );

            BindingList<PromocionesEstado> bindinglist = new BindingList<PromocionesEstado>();
            BindingSource source = new BindingSource( bindinglist, null );

            kontroler.AgregarPromociones( new List<InformacionPromocionIncumplida>() { info }, source );
            kontroler.AgregarListaPromocionesASource( source );

            Assert.AreEqual( estado.Parcial, bindinglist[0].Estado );
            Assert.AreEqual(
                ZooLogicSA.Promociones.Asistente.Properties.Resources.Amarillo.GetPixel( 0, 0 ),
                bindinglist[0].EstadoDibujo.GetPixel( 0, 0 ) );
        }

        [TestMethod]
        public void EstadoIncumplidoCuandoSatisfechoEfectivoEsCentinelaYSinResultadosSatisfechos()
        {
            Aspecto asp = new Aspecto();
            KontrolerAsistente kontroler = new KontrolerAsistente( asp );

            InformacionPromocionIncumplida info = new InformacionPromocionIncumplida();
            info.IdPromocion = "promo1";
            info.Promocion = new Promocion() { Descripcion = "Promo1" };
            info.Resultados = new List<ResultadoReglas>()
            {
                new ResultadoReglas() { Satisfecho = 0, PartPromo = new ParticipanteRegla() { Codigo = "COMPROBANTE.FACTURADETALLE.ITEM" } }
            };
            // SatisfechoEfectivo = -1 (centinela)

            IArmadorDeLeyenda armador = MockRepository.GenerateMock<IArmadorDeLeyenda>();
            kontroler.armador = armador;
            armador.Stub( x => x.Armar( info ) ).Return( "" );
            armador.Stub( x => x.ArmarLeyendaAMostrarEnControl( info ) ).Return( "" );

            BindingList<PromocionesEstado> bindinglist = new BindingList<PromocionesEstado>();
            BindingSource source = new BindingSource( bindinglist, null );

            kontroler.AgregarPromociones( new List<InformacionPromocionIncumplida>() { info }, source );
            kontroler.AgregarListaPromocionesASource( source );

            Assert.AreEqual( estado.Incumplida, bindinglist[0].Estado );
            Assert.AreEqual(
                ZooLogicSA.Promociones.Asistente.Properties.Resources.Blanco.GetPixel( 0, 0 ),
                bindinglist[0].EstadoDibujo.GetPixel( 0, 0 ) );
        }

        // -----------------------------------------------------------
        // Tarea 3.2 — Transiciones de estado al re-evaluar
        // -----------------------------------------------------------

        [TestMethod]
        public void TransicionDeCumplidaAParcialAlReEvaluar()
        {
            Aspecto asp = new Aspecto();
            KontrolerAsistente kontroler = new KontrolerAsistente( asp );

            // Primera llamada: promo cumplida (verde)
            InformacionPromocion cumplida = new InformacionPromocion( "promo1" );
            cumplida.Promocion = new Promocion() { Descripcion = "Promo1" };
            cumplida.MontoBeneficio = 100;

            BindingList<PromocionesEstado> bindinglist = new BindingList<PromocionesEstado>();
            BindingSource source = new BindingSource( bindinglist, null );

            kontroler.AgregarPromociones( new List<InformacionPromocion>() { cumplida }, source );
            kontroler.AgregarListaPromocionesASource( source );

            Assert.AreEqual( estado.Cumplida, bindinglist[0].Estado );

            // Segunda llamada: misma promo ahora parcial (SatisfechoEfectivo > 0)
            InformacionPromocionIncumplida parcial = new InformacionPromocionIncumplida();
            parcial.IdPromocion = "promo1";
            parcial.Promocion = new Promocion() { Descripcion = "Promo1" };
            parcial.Resultados = new List<ResultadoReglas>();
            parcial.SatisfechoEfectivo = 1m;

            IArmadorDeLeyenda armador = MockRepository.GenerateMock<IArmadorDeLeyenda>();
            kontroler.armador = armador;
            armador.Stub( x => x.Armar( parcial ) ).Return( "" );
            armador.Stub( x => x.ArmarLeyendaAMostrarEnControl( parcial ) ).Return( "" );

            kontroler.AgregarPromociones( new List<InformacionPromocionIncumplida>() { parcial }, source );
            kontroler.AgregarListaPromocionesASource( source );

            Assert.AreEqual( 1, source.Count );
            Assert.AreEqual( estado.Parcial, bindinglist[0].Estado );
            Assert.AreEqual(
                ZooLogicSA.Promociones.Asistente.Properties.Resources.Amarillo.GetPixel( 0, 0 ),
                bindinglist[0].EstadoDibujo.GetPixel( 0, 0 ) );
        }

        [TestMethod]
        public void TransicionDeCumplidaAIncumplidaAlReEvaluar()
        {
            Aspecto asp = new Aspecto();
            KontrolerAsistente kontroler = new KontrolerAsistente( asp );

            // Primera llamada: promo cumplida (verde)
            InformacionPromocion cumplida = new InformacionPromocion( "promo1" );
            cumplida.Promocion = new Promocion() { Descripcion = "Promo1" };
            cumplida.MontoBeneficio = 100;

            BindingList<PromocionesEstado> bindinglist = new BindingList<PromocionesEstado>();
            BindingSource source = new BindingSource( bindinglist, null );

            kontroler.AgregarPromociones( new List<InformacionPromocion>() { cumplida }, source );
            kontroler.AgregarListaPromocionesASource( source );

            Assert.AreEqual( estado.Cumplida, bindinglist[0].Estado );

            // Segunda llamada: misma promo ahora incumplida (SatisfechoEfectivo = 0)
            InformacionPromocionIncumplida incumplida = new InformacionPromocionIncumplida();
            incumplida.IdPromocion = "promo1";
            incumplida.Promocion = new Promocion() { Descripcion = "Promo1" };
            incumplida.Resultados = new List<ResultadoReglas>();
            incumplida.SatisfechoEfectivo = 0m;

            IArmadorDeLeyenda armador = MockRepository.GenerateMock<IArmadorDeLeyenda>();
            kontroler.armador = armador;
            armador.Stub( x => x.Armar( incumplida ) ).Return( "" );
            armador.Stub( x => x.ArmarLeyendaAMostrarEnControl( incumplida ) ).Return( "" );

            kontroler.AgregarPromociones( new List<InformacionPromocionIncumplida>() { incumplida }, source );
            kontroler.AgregarListaPromocionesASource( source );

            Assert.AreEqual( 1, source.Count );
            Assert.AreEqual( estado.Incumplida, bindinglist[0].Estado );
            Assert.AreEqual(
                ZooLogicSA.Promociones.Asistente.Properties.Resources.Blanco.GetPixel( 0, 0 ),
                bindinglist[0].EstadoDibujo.GetPixel( 0, 0 ) );
        }

        // -----------------------------------------------------------
        // Tarea 3.3 — Null-guard: infoIncumplida nula no genera excepción
        // -----------------------------------------------------------

        [TestMethod]
        public void ListaConInfoIncumplidaNulaNoGeneraExcepcionYOtrasPromosSeProcesanCorrectas()
        {
            Aspecto asp = new Aspecto();
            KontrolerAsistente kontroler = new KontrolerAsistente( asp );

            // Una promo con infoIncumplida null y otra válida
            InformacionPromocion conNulo = new InformacionPromocion( "promoNula" );
            conNulo.Promocion = new Promocion() { Descripcion = "PromoNula" };
            conNulo.Afectaciones = 0;
            conNulo.infoIncumplida = null;

            InformacionPromocion valida = new InformacionPromocion( "promoValida" );
            valida.Promocion = new Promocion() { Descripcion = "PromoValida" };
            valida.MontoBeneficio = 50;
            valida.Afectaciones = 1;

            BindingList<PromocionesEstado> bindinglist = new BindingList<PromocionesEstado>();
            BindingSource source = new BindingSource( bindinglist, null );

            // Simular el filtrado que hace FrmAsistente (null-guard ya aplicado antes de llegar aquí)
            List<InformacionPromocion> verdes = new List<InformacionPromocion>() { valida };
            List<InformacionPromocionIncumplida> amarillas = new List<InformacionPromocion>() { conNulo }
                .Where( x => x.Afectaciones == 0 )
                .Select( x => x.infoIncumplida )
                .Where( x => x != null )
                .ToList();

            kontroler.AgregarPromociones( verdes, source );
            // amarillas está vacía porque el null-guard filtró la entrada nula
            kontroler.AgregarPromociones( amarillas, source );
            kontroler.AgregarListaPromocionesASource( source );

            // Solo la promo válida aparece; no hay excepción
            Assert.AreEqual( 1, source.Count );
            Assert.AreEqual( "promoValida", ( (PromocionesEstado)source[0] ).Id );
        }

    }
}
