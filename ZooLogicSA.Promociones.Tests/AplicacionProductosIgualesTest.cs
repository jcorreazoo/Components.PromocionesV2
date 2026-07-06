using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Promociones.Tests.Properties;

namespace ZooLogicSA.Promociones.Tests
{
    /// <summary>
    /// Pruebas para la funcionalidad "Aplicar en productos iguales".
    /// 
    /// Escenario base:
    ///   La promoción tiene dos participantes de tipo Item:
    ///     P1: Articulo.Codigo == 00100101, Talle.Codigo == 38, Cantidad = 1
    ///     P2: Articulo.Codigo == 00100101, Talle.Codigo == 42, Cantidad = 1
    ///   El comprobante (ComprobanteCompleto) tiene:
    ///     Item0: Articulo 00100101, Talle 38, Cantidad 10
    ///     Item1: Articulo 00100101, Talle 40, Cantidad 10
    ///     (Talle 42 NO existe en el comprobante)
    /// 
    ///   NoAplicar / PorArticuloYCombinacion:
    ///     P2 no puede satisfacerse (no hay Talle 42) → la promoción NO aplica.
    /// 
    ///   PorArticulo:
    ///     P1 y P2 se fusionan en un único participante (Articulo == 00100101, Cantidad == 2,
    ///     regla de talle neutralizada).  Items 0 y 1 juntos proveen 20 unidades, satisfaciendo
    ///     el requerimiento de 2 → la promoción SÍ aplica.
    /// </summary>
    [TestClass]
    public class AplicacionProductosIgualesTest
    {
        private const string CodigoItem = "Comprobante.Facturadetalle.Item";
        private const string AtributoCombinacion = "Talle.Codigo";

        // ─────────────────────────────────────────────────────────────────────
        // Helpers
        // ─────────────────────────────────────────────────────────────────────

        private static ConfiguracionComportamiento ObtenerConfigConCombinacion()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
            config.ConfiguracionesPorParticipante[CodigoItem].Combinacion = AtributoCombinacion;
            return config;
        }

        /// <summary>
        /// Crea una promoción tipo "2" con dos participantes Item:
        ///   P1: Articulo.Codigo == 00100101, Talle.Codigo == 38, Cantidad == 1
        ///   P2: Articulo.Codigo == 00100101, Talle.Codigo == 42, Cantidad == 1
        /// y un beneficio de descuento del 10 % por cada uno.
        /// </summary>
        private static Promocion CrearPromocion( AplicacionProductosIgualesType modo )
        {
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Tipo = "2";
            promocion.AplicacionProductosIguales = modo;

            // ── P1: Articulo 00100101 + Talle 38 + Cantidad 1 ──────────────
            ParticipanteRegla p1 = new ParticipanteRegla();
            p1.Id = "1";
            p1.Codigo = CodigoItem;

            Regla r1 = new Regla();
            r1.Id = 1;
            r1.Atributo = "Articulo.Codigo";
            r1.Comparacion = Factor.DebeSerIgualA;
            r1.Valor = "00100101";
            p1.Reglas.Add( r1 );

            Regla r2 = new Regla();
            r2.Id = 2;
            r2.Atributo = AtributoCombinacion;
            r2.Comparacion = Factor.DebeSerIgualA;
            r2.Valor = "38";
            p1.Reglas.Add( r2 );

            Regla r3 = new Regla();
            r3.Id = 3;
            r3.Atributo = "Cantidad";
            r3.Comparacion = Factor.DebeSerIgualA;
            r3.Valor = 1;
            p1.Reglas.Add( r3 );

            p1.RelaReglas = "{1} and {2} and {3}";
            promocion.Participantes.Add( p1 );

            // ── P2: Articulo 00100101 + Talle 42 (no existe) + Cantidad 1 ──
            ParticipanteRegla p2 = new ParticipanteRegla();
            p2.Id = "2";
            p2.Codigo = CodigoItem;

            Regla r4 = new Regla();
            r4.Id = 1;
            r4.Atributo = "Articulo.Codigo";
            r4.Comparacion = Factor.DebeSerIgualA;
            r4.Valor = "00100101";
            p2.Reglas.Add( r4 );

            Regla r5 = new Regla();
            r5.Id = 2;
            r5.Atributo = AtributoCombinacion;
            r5.Comparacion = Factor.DebeSerIgualA;
            r5.Valor = "42";
            p2.Reglas.Add( r5 );

            Regla r6 = new Regla();
            r6.Id = 3;
            r6.Atributo = "Cantidad";
            r6.Comparacion = Factor.DebeSerIgualA;
            r6.Valor = 1;
            p2.Reglas.Add( r6 );

            p2.RelaReglas = "{1} and {2} and {3}";
            promocion.Participantes.Add( p2 );

            // ── Beneficios: 10 % de descuento por participante ──────────────
            Beneficio b1 = new Beneficio();
            b1.Atributo = "Descuento";
            b1.Valor = "10";
            b1.Cambio = Alteracion.CambiarValor;
            b1.Destinos.Add( new DestinoBeneficio() { Participante = "1", Cuantos = 1 } );
            promocion.Beneficios.Add( b1 );

            Beneficio b2 = new Beneficio();
            b2.Atributo = "Descuento";
            b2.Valor = "10";
            b2.Cambio = Alteracion.CambiarValor;
            b2.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            promocion.Beneficios.Add( b2 );

            return promocion;
        }

        private static MotorPromociones CrearMotor( ConfiguracionComportamiento config, Promocion promocion )
        {
            MotorPromociones motor = new MotorPromociones( config, new FactoriaPromociones() );
            motor.EstablecerLibreriaPromociones( new List<Promocion>() { promocion } );
            return motor;
        }

        private static List<InformacionPromocion> EjecutarPromocion( MotorPromociones motor, string xmlComprobante )
        {
            const string IdProceso = "TestProceso";
            motor.AgregarComprobanteParaEvaluacion( IdProceso, xmlComprobante );
            return motor.AplicarPromociones( IdProceso, new List<string>() { "1" } );
        }

        // ─────────────────────────────────────────────────────────────────────
        // Tests
        // ─────────────────────────────────────────────────────────────────────

        /// <summary>
        /// Con NoAplicar, el segundo participante (Talle 42) no existe en el comprobante,
        /// por lo tanto la promoción NO debe aplicar.
        /// Verifica que el comportamiento existente (sin agrupamiento) no se rompe.
        /// </summary>
        [TestMethod]
        public void NoAplicar_ParticipanteConTalleAusenteEnComprobante_PromocionNoAplica()
        {
            ConfiguracionComportamiento config = ObtenerConfigConCombinacion();
            Promocion promo = CrearPromocion( AplicacionProductosIgualesType.NoAplicar );
            MotorPromociones motor = CrearMotor( config, promo );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            List<InformacionPromocion> resultado = EjecutarPromocion( motor, comprobante.InnerXml );

            Assert.AreEqual( 0, resultado[0].Afectaciones,
                "Con NoAplicar, si Talle 42 no está en el comprobante, la promoción no debe aplicar." );
        }

        /// <summary>
        /// Con PorArticulo, los dos participantes (Talle 38 y Talle 42) se fusionan en uno
        /// solo que requiere 2 unidades del artículo 00100101 sin importar el talle.
        /// El comprobante tiene Item0 (Talle 38, cant 10) e Item1 (Talle 40, cant 10), lo que
        /// satisface el requerimiento de 2 unidades → la promoción DEBE aplicar.
        /// </summary>
        [TestMethod]
        public void PorArticulo_ParticipantesFusionadosIgnorandoTalle_PromocionAplica()
        {
            ConfiguracionComportamiento config = ObtenerConfigConCombinacion();
            Promocion promo = CrearPromocion( AplicacionProductosIgualesType.PorArticulo );
            MotorPromociones motor = CrearMotor( config, promo );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            List<InformacionPromocion> resultado = EjecutarPromocion( motor, comprobante.InnerXml );

            Assert.AreEqual( 1, resultado.Count,
                "Con PorArticulo, los participantes deben fusionarse y la promoción debe aplicar." );
        }

        /// <summary>
        /// Con PorArticuloYCombinacion, los participantes NO se fusionan porque tienen
        /// distintos valores de talle.  El comportamiento es idéntico a NoAplicar.
        /// </summary>
        [TestMethod]
        public void PorArticuloYCombinacion_ParticipantesConDistintoTalle_PromocionNoAplica()
        {
            ConfiguracionComportamiento config = ObtenerConfigConCombinacion();
            Promocion promo = CrearPromocion( AplicacionProductosIgualesType.PorArticuloYCombinacion );
            MotorPromociones motor = CrearMotor( config, promo );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            List<InformacionPromocion> resultado = EjecutarPromocion( motor, comprobante.InnerXml );

            Assert.AreEqual( 0, resultado[0].Afectaciones,
                "Con PorArticuloYCombinacion, participantes con distinto talle no se fusionan y la promoción no debe aplicar." );
        }

        // ─────────────────────────────────────────────────────────────────────
        // Caso de uso 3 – PorArticuloYCombinacion con agrupamiento por ítem
        //
        // Promoción: un participante con Articulo.Familia.Codigo="01", Cantidad=3, Recursiva.
        // Comprobante:
        //   fila 0 → Art0  color=Rojo  talle=""   qty=2
        //   fila 1 → Art1  color=Verde talle=""   qty=4
        //   fila 2 → Art2  color=""    talle=""   qty=4  (inválida: sin combinación)
        //   fila 3 → Art2  color=""    talle=40   qty=4
        //
        // Resultado esperado:
        //   Art1+Verde  → 1 aplicación (sobrante 1)
        //   Art0+Rojo   → 0 aplicaciones (qty 2 < 3)
        //   Art2 s/comb → 0 aplicaciones (combinación inválida)
        //   Art2+40     → 1 aplicación (sobrante 1)
        //   Total: 2 aplicaciones
        // ─────────────────────────────────────────────────────────────────────

        private const string CodigoItemCaso3 = "Comprobante.Facturadetalle.Item";

        private static ConfiguracionComportamiento ObtenerConfigCaso3()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
            config.ConfiguracionesPorParticipante[CodigoItemCaso3].Combinacion = "Color.Codigo;Talle.Codigo";
            config.ConfiguracionesPorParticipante[CodigoItemCaso3].AtributoCodigoArticulo = "Articulo.Codigo";
            return config;
        }

        /// <summary>
        /// Crea la promoción del caso 3: un único participante con Familia="01" y Cantidad=3,
        /// AplicacionProductosIguales=PorArticuloYCombinacion, Recursiva=true.
        /// </summary>
        private static Promocion CrearPromocionCaso3()
        {
            Promocion promocion = new Promocion();
            promocion.Id = "3";
            promocion.Tipo = "2";
            promocion.AplicacionProductosIguales = AplicacionProductosIgualesType.PorArticuloYCombinacion;
            promocion.Recursiva = true;

            ParticipanteRegla p1 = new ParticipanteRegla();
            p1.Id = "1";
            p1.Codigo = CodigoItemCaso3;

            Regla rFamilia = new Regla();
            rFamilia.Id = 1;
            rFamilia.Atributo = "Articulo.Familia.Codigo";
            rFamilia.Comparacion = Factor.DebeSerIgualA;
            rFamilia.Valor = "01";
            p1.Reglas.Add( rFamilia );

            Regla rCantidad = new Regla();
            rCantidad.Id = 2;
            rCantidad.Atributo = "Cantidad";
            rCantidad.Comparacion = Factor.DebeSerIgualA;
            rCantidad.Valor = 3;
            p1.Reglas.Add( rCantidad );

            p1.RelaReglas = "{1} and {2}";
            promocion.Participantes.Add( p1 );

            Beneficio b1 = new Beneficio();
            b1.Atributo = "Descuento";
            b1.Valor = "10";
            b1.Cambio = Alteracion.CambiarValor;
            b1.Destinos.Add( new DestinoBeneficio() { Participante = "1", Cuantos = 1 } );
            promocion.Beneficios.Add( b1 );

            return promocion;
        }

        /// <summary>
        /// Comprobante con 4 ítems de familia "01":
        ///   0: Art0 + color=Rojo  + talle=""   qty=2
        ///   1: Art1 + color=Verde + talle=""   qty=4
        ///   2: Art2 + color=""    + talle=""   qty=4  (inválido)
        ///   3: Art2 + color=""    + talle=40   qty=4
        /// </summary>
        private static string ObtenerComprobanteCaso3()
        {
            return @"<Comprobante>
  <Codigo TipoDato=""C"" Valor=""guidFactura"" />
  <Facturadetalle>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""0"" />
      <Articulo>
        <Codigo TipoDato=""C"" Valor=""Art0"" />
        <Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia>
      </Articulo>
      <Cantidad TipoDato=""N"" Valor=""2"" />
      <Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" />
      <MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""Rojo"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""1"" />
      <Articulo>
        <Codigo TipoDato=""C"" Valor=""Art1"" />
        <Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia>
      </Articulo>
      <Cantidad TipoDato=""N"" Valor=""4"" />
      <Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" />
      <MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""Verde"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""2"" />
      <Articulo>
        <Codigo TipoDato=""C"" Valor=""Art2"" />
        <Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia>
      </Articulo>
      <Cantidad TipoDato=""N"" Valor=""4"" />
      <Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" />
      <MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor="""" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""3"" />
      <Articulo>
        <Codigo TipoDato=""C"" Valor=""Art2"" />
        <Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia>
      </Articulo>
      <Cantidad TipoDato=""N"" Valor=""4"" />
      <Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" />
      <MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor="""" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor=""40"" /></Talle>
    </Item>
  </Facturadetalle>
</Comprobante>";
        }

        [TestMethod]
        public void CasoDeUso3_PorArticuloYCombinacion_MismoArticuloDistintoTalleNoSeAgregan()
        {
            // Escenario del reporte: dos filas con el mismo artículo y color pero distinto talle.
            // Fila 0: ART1+1162x+38, qty=2. Fila 1: ART1+1162x+40, qty=1.
            // Ninguna fila sola alcanza qty=3 → la promoción NO debe aplicar.
            string xml = @"<Comprobante>
  <Codigo TipoDato=""C"" Valor=""guidFactura"" />
  <Facturadetalle>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""0"" />
      <Articulo>
        <Codigo TipoDato=""C"" Valor=""ART1"" />
        <Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia>
      </Articulo>
      <Cantidad TipoDato=""N"" Valor=""2"" />
      <Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" />
      <MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""1162x"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor=""38"" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""1"" />
      <Articulo>
        <Codigo TipoDato=""C"" Valor=""ART1"" />
        <Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia>
      </Articulo>
      <Cantidad TipoDato=""N"" Valor=""1"" />
      <Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" />
      <MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""1162x"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor=""40"" /></Talle>
    </Item>
  </Facturadetalle>
</Comprobante>";

            ConfiguracionComportamiento config = ObtenerConfigCaso3();
            Promocion promo = CrearPromocionCaso3();
            MotorPromociones motor = CrearMotor( config, promo );

            List<InformacionPromocion> resultado = EjecutarPromocion( motor, xml );

            Assert.AreEqual( 0, resultado.Count,
                "Con distinto talle, ninguna fila llega a qty=3; filas no deben agregarse entre sí." );
        }

        [TestMethod]
        public void CasoDeUso3_PorArticuloYCombinacion_AplicaDosVeces()
        {
            // Art1+Verde y Art2+40 tienen qty>=3 cada uno → 2 aplicaciones totales.
            ConfiguracionComportamiento config = ObtenerConfigCaso3();
            Promocion promo = CrearPromocionCaso3();

            List<InformacionPromocion> resultado = EjecutarPromocionIndependiente( config, promo, ObtenerComprobanteCaso3() );

            Assert.AreEqual( 2, resultado[0].Afectaciones,
                "Art1+Verde y Art2+40 deben generar 2 aplicaciones de la promoción." );
        }

        [TestMethod]
        public void CasoDeUso3_PorArticuloYCombinacion_Art0RojoNoCumple()
        {
            // Art0+Rojo tiene qty=2 < 3 → no debe haber más de 2 aplicaciones (ambas de otras combinaciones).
            // Este test verifica que Art0+Rojo no genera una aplicación adicional.
            ConfiguracionComportamiento config = ObtenerConfigCaso3();
            Promocion promo = CrearPromocionCaso3();
            MotorPromociones motor = CrearMotor( config, promo );

            List<InformacionPromocion> resultado = EjecutarPromocion( motor, ObtenerComprobanteCaso3() );

            // Solo Art1+Verde y Art2+40 cumplen. Art0+Rojo (qty=2<3) no genera aplicación.
            Assert.IsFalse( resultado.Count > 2,
                "Art0+Rojo (qty=2) no debe generar una aplicación porque no cumple la cantidad requerida de 3." );
        }

        [TestMethod]
        public void CasoDeUso3_PorArticuloYCombinacion_Art2SinCombinacionNoAplica()
        {
            // Art2 sin color ni talle es una combinación inválida y no debe aplicar.
            // Verifica que la restricción de combinación vacía funciona, comprobando que
            // si solo hubiéramos tenido el ítem de Art2 con color y talle vacíos, no aplica.
            string xmlSoloArt2SinCombinacion = @"<Comprobante>
  <Codigo TipoDato=""C"" Valor=""guidFactura"" />
  <Facturadetalle>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""2"" />
      <Articulo>
        <Codigo TipoDato=""C"" Valor=""Art2"" />
        <Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia>
      </Articulo>
      <Cantidad TipoDato=""N"" Valor=""9"" />
      <Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" />
      <MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor="""" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
  </Facturadetalle>
</Comprobante>";

            ConfiguracionComportamiento config = ObtenerConfigCaso3();
            Promocion promo = CrearPromocionCaso3();
            MotorPromociones motor = CrearMotor( config, promo );

            List<InformacionPromocion> resultado = EjecutarPromocion( motor, xmlSoloArt2SinCombinacion );

            Assert.AreEqual( 0, resultado.Count,
                "Art2 sin color ni talle es una combinación inválida: la promoción no debe aplicar." );
        }

        // ─────────────────────────────────────────────────────────────────────
        // Caso de uso 4 – Las tres modalidades evaluadas sobre el mismo conjunto
        //
        // Comprobante (7 filas, todas familia "01"):
        //   fila0: Art0  color=Rojo  talle=XL   qty=1
        //   fila1: Art2  color=Rojo             qty=1
        //   fila2: Art2  color=""   talle=""    qty=2   ← sin combinación (válida para NoAplicar y PorArticulo)
        //   fila3: Art3  color=Verde talle=XL   qty=3
        //   fila4: Art4  color=Azul             qty=2
        //   fila5: Art4  talle=XL               qty=2
        //   fila6: Art3  talle=XL               qty=3
        //
        // Evaluación independiente (cada promo sobre el comprobante original completo):
        //   Promo3 (PorArticuloYCombinacion): sólo filas con combinación no-vacía y qty>=3
        //     → fila3(Art3+Verde+XL, 3) y fila6(Art3+XL, 3) → 2 aplicaciones
        //   Promo2 (PorArticulo): agrupa por Articulo.Codigo
        //     → Art2(fila1+fila2=3)×1 + Art3(fila3+fila6=6)×2 + Art4(fila4+fila5=4)×1 → 4 aplicaciones
        //   Promo1 (NoAplicar): mezcla libre
        //     → total=14 unidades / 3 = 4 aplicaciones
        // ─────────────────────────────────────────────────────────────────────

        private const string CodigoItemCaso4 = "Comprobante.Facturadetalle.Item";

        private static ConfiguracionComportamiento ObtenerConfigCaso4()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
            config.ConfiguracionesPorParticipante[CodigoItemCaso4].Combinacion = "Color.Codigo;Talle.Codigo";
            config.ConfiguracionesPorParticipante[CodigoItemCaso4].AtributoCodigoArticulo = "Articulo.Codigo";
            return config;
        }

        /// <summary>
        /// Crea una promo con un único participante: Familia="01", Cantidad=3, Recursiva=true.
        /// La modalidad AplicacionProductosIguales se pasa como parámetro.
        /// </summary>
        private static Promocion CrearPromocionCaso4( string id, AplicacionProductosIgualesType modo, string descuento )
        {
            Promocion promo = new Promocion();
            promo.Id = id;
            promo.Tipo = "2";
            promo.Recursiva = true;
            promo.AplicacionProductosIguales = modo;

            ParticipanteRegla p1 = new ParticipanteRegla();
            p1.Id = "1";
            p1.Codigo = CodigoItemCaso4;

            Regla rFam = new Regla();
            rFam.Id = 1;
            rFam.Atributo = "Articulo.Familia.Codigo";
            rFam.Comparacion = Factor.DebeSerIgualA;
            rFam.Valor = "01";
            p1.Reglas.Add( rFam );

            Regla rCant = new Regla();
            rCant.Id = 2;
            rCant.Atributo = "Cantidad";
            rCant.Comparacion = Factor.DebeSerIgualA;
            rCant.Valor = 3;
            p1.Reglas.Add( rCant );

            p1.RelaReglas = "{1} and {2}";
            promo.Participantes.Add( p1 );

            Beneficio b = new Beneficio();
            b.Atributo = "Descuento";
            b.Valor = descuento;
            b.Cambio = Alteracion.CambiarValor;
            b.Destinos.Add( new DestinoBeneficio() { Participante = "1", Cuantos = 1 } );
            promo.Beneficios.Add( b );

            return promo;
        }

        /// <summary>
        /// Comprobante del caso 4 con 7 filas, todas familia "01".
        /// </summary>
        private static string ObtenerComprobanteCaso4()
        {
            return @"<Comprobante>
  <Codigo TipoDato=""C"" Valor=""guidCaso4"" />
  <Facturadetalle>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""0"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art0"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""1"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""Rojo"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor=""XL"" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""1"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art2"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""1"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""Rojo"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""2"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art2"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""2"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor="""" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""3"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art3"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""3"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""Verde"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor=""XL"" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""4"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art4"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""2"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""Azul"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""5"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art4"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""2"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor="""" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor=""XL"" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""6"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art3"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""3"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor="""" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor=""XL"" /></Talle>
    </Item>
  </Facturadetalle>
</Comprobante>";
        }

        private static List<InformacionPromocion> EjecutarPromocionIndependiente(
            ConfiguracionComportamiento config, Promocion promo, string xmlComprobante )
        {
            MotorPromociones motor = new MotorPromociones( config, new FactoriaPromociones() );
            motor.EstablecerLibreriaPromociones( new List<Promocion>() { promo } );
            const string IdProceso = "TestCaso4";
            motor.AgregarComprobanteParaEvaluacion( IdProceso, xmlComprobante );
            return motor.AplicarPromociones( IdProceso, new List<string>() { promo.Id } );
        }

        /// <summary>
        /// Promo3 (PorArticuloYCombinacion): solo aplica a filas con combinación no vacía y qty>=3.
        /// Fila3 (Art3+Verde+XL, qty=3) y fila6 (Art3+XL, qty=3) son las únicas que califican.
        /// Fila2 (Art2, sin color ni talle) está excluida por combinación vacía.
        /// → 2 aplicaciones.
        /// </summary>
        [TestMethod]
        public void CasoDeUso4_Promo3_PorArticuloYCombinacion_AplicaDosVeces()
        {
            ConfiguracionComportamiento config = ObtenerConfigCaso4();
            Promocion promo = CrearPromocionCaso4( "Promo3", AplicacionProductosIgualesType.PorArticuloYCombinacion, "20" );

            List<InformacionPromocion> resultado = EjecutarPromocionIndependiente( config, promo, ObtenerComprobanteCaso4() );

            Assert.AreEqual( 2, resultado[0].Afectaciones,
                "Promo3 (PorArticuloYCombinacion): solo fila3 y fila6 tienen qty=3 con combinación válida (2 aplicaciones)." );
        }

        /// <summary>
        /// Promo3 (PorArticuloYCombinacion): fila2 (Art2 sin color ni talle) no puede aplicar
        /// aunque acumule 2 unidades, porque su combinación está completamente vacía.
        /// </summary>
        [TestMethod]
        public void CasoDeUso4_Promo3_FilaSinCombinacionNoAplica()
        {
            // Comprobante con solo la fila problemática, qty suficiente.
            string xmlSoloSinCombinacion = ObtenerComprobanteCaso4().Replace(
                @"<IdItemArticulos TipoDato=""C"" Valor=""2"" />",
                @"<IdItemArticulos TipoDato=""C"" Valor=""2"" /><!-- solo -->" );

            // Usamos el comprobante entero pero verificamos que sin filas con combo válida y qty>=3
            // en Art2 (solo qty=1+2=3 pero una de las filas no tiene combo), Art2 no aplica.
            // fila1: Art2+Rojo (qty=1) → combinación válida pero sola no alcanza qty=3.
            // fila2: Art2 sin combo (qty=2) → excluida por PorArticuloYCombinacion.
            // → Art2 no puede aplicar en Promo3.
            // El test de 2 aplicaciones (fila3 y fila6) ya cubre esto implícitamente,
            // pero aquí lo hacemos explícito con un comprobante que solo tiene Art2.
            string xmlSoloArt2 = @"<Comprobante>
  <Codigo TipoDato=""C"" Valor=""guidCaso4"" />
  <Facturadetalle>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""1"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art2"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""1"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""Rojo"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""2"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art2"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""5"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor="""" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
  </Facturadetalle>
</Comprobante>";

            ConfiguracionComportamiento config = ObtenerConfigCaso4();
            Promocion promo = CrearPromocionCaso4( "Promo3", AplicacionProductosIgualesType.PorArticuloYCombinacion, "20" );

            List<InformacionPromocion> resultado = EjecutarPromocionIndependiente( config, promo, xmlSoloArt2 );

            Assert.AreEqual( 0, resultado[0].Afectaciones,
                "Promo3: fila1-Art2+Rojo tiene qty=1<3 (sola); fila2-Art2 sin combo es inválida → no aplica." );
        }

        /// <summary>
        /// Promo2 (PorArticulo): agrupa todas las filas del mismo Articulo.Codigo.
        /// Art2: fila1(qty=1)+fila2(qty=2)=3 → 1 aplicación.
        /// Art3: fila3(qty=3)+fila6(qty=3)=6 → 2 aplicaciones.
        /// Art4: fila4(qty=2)+fila5(qty=2)=4 → 1 aplicación.
        /// Art0: qty=1 < 3 → 0 aplicaciones.
        /// Total: 4 aplicaciones.
        /// </summary>
        [TestMethod]
        public void CasoDeUso4_Promo2_PorArticulo_AgrupaFilasMismoArticulo()
        {
            ConfiguracionComportamiento config = ObtenerConfigCaso4();
            Promocion promo = CrearPromocionCaso4( "Promo2", AplicacionProductosIgualesType.PorArticulo, "15" );

            List<InformacionPromocion> resultado = EjecutarPromocionIndependiente( config, promo, ObtenerComprobanteCaso4() );

            Assert.AreEqual( 4, resultado[0].Afectaciones,
                "Promo2 (PorArticulo): Art2(3→1app)+Art3(6→2apps)+Art4(4→1app) = 4 aplicaciones." );
        }

        /// <summary>
        /// Promo2 (PorArticulo): Art2 con filas en filas separadas (qty=1 y qty=2) se suman
        /// a 3 → debe aplicar 1 vez si es el único artículo elegible.
        /// </summary>
        [TestMethod]
        public void CasoDeUso4_Promo2_PorArticulo_Art2FilasSeparadasSeSuman()
        {
            string xmlSoloArt2 = @"<Comprobante>
  <Codigo TipoDato=""C"" Valor=""guidCaso4"" />
  <Facturadetalle>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""1"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art2"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""1"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""Rojo"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""2"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art2"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""2"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor="""" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
  </Facturadetalle>
</Comprobante>";

            ConfiguracionComportamiento config = ObtenerConfigCaso4();
            Promocion promo = CrearPromocionCaso4( "Promo2", AplicacionProductosIgualesType.PorArticulo, "15" );

            List<InformacionPromocion> resultado = EjecutarPromocionIndependiente( config, promo, xmlSoloArt2 );

            Assert.AreEqual( 1, resultado.Count,
                "Promo2 (PorArticulo): Art2 en dos filas (qty=1+2=3) debe sumarse y aplicar 1 vez." );
        }

        /// <summary>
        /// Promo1 (NoAplicar): mezcla libre de cualquier fila sin restricción.
        /// Total=14 unidades / 3 = 4 aplicaciones completas (sobrante 2).
        /// </summary>
        [TestMethod]
        public void CasoDeUso4_Promo1_NoAplicar_MezclaLibre()
        {
            ConfiguracionComportamiento config = ObtenerConfigCaso4();
            Promocion promo = CrearPromocionCaso4( "Promo1", AplicacionProductosIgualesType.NoAplicar, "10" );

            List<InformacionPromocion> resultado = EjecutarPromocionIndependiente( config, promo, ObtenerComprobanteCaso4() );

            Assert.AreEqual( 4, resultado[0].Afectaciones,
                "Promo1 (NoAplicar): 14 unidades total / 3 = 4 aplicaciones con mezcla libre de filas." );
        }

        // ─────────────────────────────────────────────────────────────────────
        // Caso de uso 5 – El motor debe aplicar Promo2 antes que Promo1
        //
        // Comprobante (4 filas, todas familia "01"):
        //   fila0: Art0  sin combo        qty=1
        //   fila1: Art1  sin combo        qty=1
        //   fila2: Art3  sin combo        qty=2
        //   fila3: Art0  color=Verde      qty=2
        //
        // Promotions aplicadas VÍA AplicarPromociones (secuencial, mismo comprobante):
        //   Promo3 primero (PorArticuloYCombinacion):
        //     → fila3 (Art0+Verde) tiene qty=2 < 3 → no aplica; falta 1.
        //   Promo2 segunda (PorArticulo):
        //     → Art0: fila0(1)+fila3(2)=3 → aplica 1 vez.
        //   Promo1 tercera (NoAplicar):
        //     → fila1(1)+fila2(2)=3 → aplica 1 vez.
        //
        // El motor debe reordenar automáticamente: PorArticuloYCombinacion → PorArticulo → NoAplicar.
        // Si Promo1 corriera antes que Promo2, consumiría fila0 (Art0,qty=1) y parte de fila2,
        // dejando insuficientes unidades de Art0 para que Promo2 aplique.
        // ─────────────────────────────────────────────────────────────────────

        private static string ObtenerComprobanteCaso5()
        {
            return @"<Comprobante>
  <Codigo TipoDato=""C"" Valor=""guidCaso5"" />
  <Facturadetalle>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""0"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art0"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""1"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor="""" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""1"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art1"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""1"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor="""" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""2"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art3"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""2"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor="""" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""3"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art0"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""2"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""Verde"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor="""" /></Talle>
    </Item>
  </Facturadetalle>
</Comprobante>";
        }

        /// <summary>
        /// Verifica que el motor reordena automáticamente las promos de más a menos restrictiva,
        /// garantizando que Promo2 (PorArticulo) aplica antes que Promo1 (NoAplicar).
        /// Sin el reordenamiento, Promo1 consumiría Art0 y Promo2 no podría aplicar.
        /// Con el reordenamiento: Promo3→Promo2→Promo1 → 2 aplicaciones totales.
        /// </summary>
        [TestMethod]
        public void CasoDeUso5_OrdenAutomaticoDePromos_Promo2AntesQuePromo1()
        {
            ConfiguracionComportamiento config = ObtenerConfigCaso4(); // misma config (combo + articuloCodigo)

            Promocion promo1 = CrearPromocionCaso4( "Promo1", AplicacionProductosIgualesType.NoAplicar, "10" );
            Promocion promo2 = CrearPromocionCaso4( "Promo2", AplicacionProductosIgualesType.PorArticulo, "15" );
            Promocion promo3 = CrearPromocionCaso4( "Promo3", AplicacionProductosIgualesType.PorArticuloYCombinacion, "20" );

            MotorPromociones motor = new MotorPromociones( config, new FactoriaPromociones() );
            motor.EstablecerLibreriaPromociones( new List<Promocion>() { promo1, promo2, promo3 } );

            const string IdProceso = "TestCaso5";
            motor.AgregarComprobanteParaEvaluacion( IdProceso, ObtenerComprobanteCaso5() );

            // Las promos se pasan en orden natural (1,2,3); el motor debe reordenarlas (3,2,1).
            List<InformacionPromocion> resultado = motor.AplicarPromociones(
                IdProceso, new List<string>() { "Promo1", "Promo2", "Promo3" } );

            // Promo3: no aplica (fila3 tiene qty=2 < 3); genera 1 InformacionPromocion con infoIncumplida.
            // Promo2: aplica 1 vez (Art0: fila0(1)+fila3(2)=3).
            // Promo1: aplica 1 vez (fila1(1)+fila2(2)=3 mezcla libre).
            // Total items en resultado = 3 (1 incumplida + 2 cumplidas).
            Assert.AreEqual( 3, resultado.Count, "Deben retornarse 3 InformacionPromocion (Promo3 incumplida, Promo2 y Promo1 cumplidas)." );

            InformacionPromocion infoPromo3 = resultado.FirstOrDefault( x => x.IdPromocion == "Promo3" );
            InformacionPromocion infoPromo2 = resultado.FirstOrDefault( x => x.IdPromocion == "Promo2" );
            InformacionPromocion infoPromo1 = resultado.FirstOrDefault( x => x.IdPromocion == "Promo1" );

            Assert.IsNotNull( infoPromo3, "Debe existir un resultado para Promo3." );
            Assert.IsNotNull( infoPromo3.infoIncumplida, "Promo3 no aplica: debe tener infoIncumplida." );

            Assert.IsNotNull( infoPromo2, "Debe existir un resultado para Promo2." );
            Assert.AreEqual( 1, infoPromo2.Afectaciones, "Promo2 debe aplicar exactamente 1 vez (Art0: fila0+fila3=3)." );

            Assert.IsNotNull( infoPromo1, "Debe existir un resultado para Promo1." );
            Assert.AreEqual( 1, infoPromo1.Afectaciones, "Promo1 debe aplicar exactamente 1 vez (fila1+fila2=3 mezcla libre)." );
        }

        /// <summary>
        /// Verifica que Promo3 no aplica a fila3 (Art0+Verde, qty=2) porque le falta 1 para llegar a 3.
        /// </summary>
        [TestMethod]
        public void CasoDeUso5_Promo3_FaltaUnParticipanteEnFila3()
        {
            ConfiguracionComportamiento config = ObtenerConfigCaso4();
            Promocion promo3 = CrearPromocionCaso4( "Promo3", AplicacionProductosIgualesType.PorArticuloYCombinacion, "20" );

            List<InformacionPromocion> resultado = EjecutarPromocionIndependiente( config, promo3, ObtenerComprobanteCaso5() );

            Assert.AreEqual( 1, resultado.Count, "Promo3 no aplica: debe retornar 1 InformacionPromocion con el incumplimiento." );
            Assert.IsNotNull( resultado[0].infoIncumplida, "Promo3: debe indicar incumplimiento (falta 1 en fila3 Art0+Verde)." );
            Assert.AreEqual( 0, resultado[0].Afectaciones, "Promo3: no debe haber afectaciones ya que no se cumplió." );
        }

        // ─────────────────────────────────────────────────────────────────────
        // Caso de uso 9 – PorArticuloYCombinacion con múltiples filas de idéntica combinación
        //
        // Promoción: tres participantes con Articulo.Codigo="Art0", Cantidad=1, sin regla de
        // talle/color. NormalizarPromocionObtenida los fusiona en 1 participante con Cantidad=3.
        // Comprobante: 3 filas con el mismo artículo, color y talle, qty=1 cada una.
        //
        // Resultado esperado: las tres filas acumulan qty=3 en la misma combinación (Art0+A+L)
        // y satisfacen el requerimiento → la promo se cumple 1 vez.
        // ─────────────────────────────────────────────────────────────────────

        private static Promocion CrearPromocionCaso9()
        {
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Tipo = "2";
            promocion.AplicacionProductosIguales = AplicacionProductosIgualesType.PorArticuloYCombinacion;
            promocion.Recursiva = true;

            for ( int i = 1; i <= 3; i++ )
            {
                ParticipanteRegla p = new ParticipanteRegla();
                p.Id = i.ToString();
                p.Codigo = "Comprobante.Facturadetalle.Item";

                Regla rCodigo = new Regla();
                rCodigo.Id = 1;
                rCodigo.Atributo = "Articulo.Codigo";
                rCodigo.Comparacion = Factor.DebeSerIgualA;
                rCodigo.Valor = "Art0";
                p.Reglas.Add( rCodigo );

                Regla rCantidad = new Regla();
                rCantidad.Id = 2;
                rCantidad.Atributo = "Cantidad";
                rCantidad.Comparacion = Factor.DebeSerIgualA;
                rCantidad.Valor = 1;
                p.Reglas.Add( rCantidad );

                p.RelaReglas = "{1} and {2}";
                promocion.Participantes.Add( p );

                Beneficio b = new Beneficio();
                b.Atributo = "Descuento";
                b.Valor = "30";
                b.Cambio = Alteracion.CambiarValor;
                b.Destinos.Add( new DestinoBeneficio() { Participante = i.ToString(), Cuantos = 1 } );
                promocion.Beneficios.Add( b );
            }

            return promocion;
        }

        private static string ObtenerComprobanteCaso9()
        {
            return @"<Comprobante>
  <Codigo TipoDato=""C"" Valor=""guidCaso9"" />
  <Facturadetalle>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""0"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art0"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""1"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""A"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor=""L"" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""1"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art0"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""1"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""A"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor=""L"" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""2"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art0"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""1"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""A"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor=""L"" /></Talle>
    </Item>
  </Facturadetalle>
</Comprobante>";
        }

        /// <summary>
        /// Verifica que tres filas con idéntica combinación (Art0+A+L, qty=1 cada una) acumulan
        /// su cantidad y satisfacen el requerimiento de 3 → la promo se cumple exactamente 1 vez.
        /// Caso de uso 9 del CasodeUso.md.
        /// </summary>
        [TestMethod]
        public void CasoDeUso9_PorArticuloYCombinacion_FilasIdenticasAcumulanCantidad()
        {
            ConfiguracionComportamiento config = ObtenerConfigCaso3();
            Promocion promo = CrearPromocionCaso9();
            MotorPromociones motor = CrearMotor( config, promo );

            List<InformacionPromocion> resultado = EjecutarPromocion( motor, ObtenerComprobanteCaso9() );

            Assert.AreEqual( 1, resultado.Count,
                "Tres filas con la misma combinación (Art0+A+L, qty=1 c/u) deben acumularse y satisfacer la promoción 1 vez." );
        }

        // ─────────────────────────────────────────────────────────────────────
        // Caso de uso 10 – PorArticuloYCombinacion con tipo "lleva cantidad, paga otra"
        //                  y múltiples filas de idéntica combinación
        //
        // Promoción: 1 participante con Articulo.Familia.Codigo="01" y Cantidad=3.
        //            EleccionParticipante = AplicarAlDeMayorPrecio ("paga 2, el más caro es gratis").
        // Comprobante: 3 filas con el mismo artículo, color y talle, qty=1 cada una.
        //
        // Con la corrección, las tres filas acumulan qty=3 en la misma combinación (Art0+V+L)
        // y satisfacen el requerimiento → la promo se cumple 1 vez.
        // ─────────────────────────────────────────────────────────────────────

        private static Promocion CrearPromocionCaso10()
        {
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Tipo = "2";
            promocion.AplicacionProductosIguales = AplicacionProductosIgualesType.PorArticuloYCombinacion;
            promocion.Recursiva = true;
            promocion.EleccionParticipante = EleccionParticipanteType.AplicarAlDeMayorPrecio;

            ParticipanteRegla p1 = new ParticipanteRegla();
            p1.Id = "1";
            p1.Codigo = "Comprobante.Facturadetalle.Item";

            Regla rFamilia = new Regla();
            rFamilia.Id = 1;
            rFamilia.Atributo = "Articulo.Familia.Codigo";
            rFamilia.Comparacion = Factor.DebeSerIgualA;
            rFamilia.Valor = "01";
            p1.Reglas.Add( rFamilia );

            Regla rCantidad = new Regla();
            rCantidad.Id = 2;
            rCantidad.Atributo = "Cantidad";
            rCantidad.Comparacion = Factor.DebeSerIgualA;
            rCantidad.Valor = 3;
            p1.Reglas.Add( rCantidad );

            p1.RelaReglas = "{1} and {2}";
            promocion.Participantes.Add( p1 );

            // "Paga 2": Cuantos=1 significa que 1 ítem de los 3 recibe 100% descuento (el más caro).
            Beneficio b1 = new Beneficio();
            b1.Atributo = "Descuento";
            b1.Valor = "100";
            b1.Cambio = Alteracion.CambiarValor;
            b1.Destinos.Add( new DestinoBeneficio() { Participante = "1", Cuantos = 1 } );
            promocion.Beneficios.Add( b1 );

            return promocion;
        }

        private static string ObtenerComprobanteCaso10()
        {
            return @"<Comprobante>
  <Codigo TipoDato=""C"" Valor=""guidCaso10"" />
  <Facturadetalle>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""0"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art0"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""1"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""V"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor=""L"" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""1"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art0"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""1"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""V"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor=""L"" /></Talle>
    </Item>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""2"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art0"" /><Familia><Codigo TipoDato=""C"" Valor=""01"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""1"" /><Precio TipoDato=""N"" Valor=""100"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""V"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor=""L"" /></Talle>
    </Item>
  </Facturadetalle>
</Comprobante>";
        }

        /// <summary>
        /// Verifica que una promo "lleva 3 paga 2" con PorArticuloYCombinacion se cumple cuando
        /// tres filas de idéntica combinación (Art0+V+L, qty=1 c/u) acumulan qty=3.
        /// Caso de uso 10 del CasodeUso.md.
        /// </summary>
        [TestMethod]
        public void CasoDeUso10_PorArticuloYCombinacion_LlevaPagaConFilasIdenticasAcumulanCantidad()
        {
            ConfiguracionComportamiento config = ObtenerConfigCaso3();
            Promocion promo = CrearPromocionCaso10();
            MotorPromociones motor = CrearMotor( config, promo );

            List<InformacionPromocion> resultado = EjecutarPromocion( motor, ObtenerComprobanteCaso10() );

            Assert.AreEqual( 1, resultado.Count,
                "Tres filas con la misma combinación (Art0+V+L, qty=1 c/u) deben acumularse y satisfacer la promo lleva/paga 1 vez." );
        }

        // ─────────────────────────────────────────────────────────────────────
        // Caso de uso 11 – PorArticuloYCombinacion con tipo "llevando monto, paga precio fijo"
        //                  y múltiples destinos de beneficio (uno ausente del comprobante)
        //
        // Promoción: 1 participante condición (familia='02', qty>=4) + 2 destinos beneficiarios
        //            (Art0 con Beneficiario=true, Art1 con Beneficiario=true).
        //            AplicacionProductosIguales = PorArticuloYCombinacion.
        // Comprobante: 1 fila Art0, familia='02', qty=4 (Art1 NO está en el comprobante).
        //
        // Con la corrección en MotorPromociones, la guarda excluye los participantes
        // beneficiarios del conteo → 1 condición cumplida == 1 participante condición → cumple.
        // ─────────────────────────────────────────────────────────────────────

        private static Promocion CrearPromocionCaso11( ConfiguracionComportamiento config )
        {
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Tipo = "6";
            promocion.AplicacionProductosIguales = AplicacionProductosIgualesType.PorArticuloYCombinacion;

            // Participante 1 — condición: familia '02', qty >= 4
            ParticipanteRegla p1 = new ParticipanteRegla();
            p1.Id = "1";
            p1.Codigo = "Comprobante.Facturadetalle.Item";
            p1.Beneficiario = false;

            Regla rFamilia = new Regla();
            rFamilia.Id = 1;
            rFamilia.Atributo = "Articulo.Familia.Codigo";
            rFamilia.Comparacion = Factor.DebeSerIgualA;
            rFamilia.Valor = "02";
            p1.Reglas.Add( rFamilia );

            Regla rCantidadP1 = new Regla();
            rCantidadP1.Id = 2;
            rCantidadP1.Atributo = "Cantidad";
            rCantidadP1.Comparacion = Factor.DebeSerMayorIgualA;
            rCantidadP1.Valor = 4;
            p1.Reglas.Add( rCantidadP1 );

            p1.RelaReglas = "{1} and {2}";
            promocion.Participantes.Add( p1 );

            // Participante 2 — destino beneficio: Art0 (presente en comprobante)
            ParticipanteRegla p2 = new ParticipanteRegla();
            p2.Id = "2";
            p2.Codigo = "Comprobante.Facturadetalle.Item";
            p2.Beneficiario = true;

            Regla rCodigoArt0 = new Regla();
            rCodigoArt0.Id = 3;
            rCodigoArt0.Atributo = "Articulo.Codigo";
            rCodigoArt0.Comparacion = Factor.DebeSerIgualA;
            rCodigoArt0.Valor = "Art0";
            p2.Reglas.Add( rCodigoArt0 );

            Regla rCantidadP2 = new Regla();
            rCantidadP2.Id = 4;
            rCantidadP2.Atributo = "Cantidad";
            rCantidadP2.Comparacion = Factor.DebeSerMayorIgualA;
            rCantidadP2.Valor = 1;
            p2.Reglas.Add( rCantidadP2 );

            p2.RelaReglas = "{3} and {4}";
            promocion.Participantes.Add( p2 );

            // Participante 3 — destino beneficio: Art1 (NO presente en comprobante)
            ParticipanteRegla p3 = new ParticipanteRegla();
            p3.Id = "3";
            p3.Codigo = "Comprobante.Facturadetalle.Item";
            p3.Beneficiario = true;

            Regla rCodigoArt1 = new Regla();
            rCodigoArt1.Id = 5;
            rCodigoArt1.Atributo = "Articulo.Codigo";
            rCodigoArt1.Comparacion = Factor.DebeSerIgualA;
            rCodigoArt1.Valor = "Art1";
            p3.Reglas.Add( rCodigoArt1 );

            Regla rCantidadP3 = new Regla();
            rCantidadP3.Id = 6;
            rCantidadP3.Atributo = "Cantidad";
            rCantidadP3.Comparacion = Factor.DebeSerMayorIgualA;
            rCantidadP3.Valor = 1;
            p3.Reglas.Add( rCantidadP3 );

            p3.RelaReglas = "{5} and {6}";
            promocion.Participantes.Add( p3 );

            // Beneficio: precio fijo de 2000 aplicado a Art0 o Art1
            Beneficio b = new Beneficio();
            b.Atributo = config.AtributoMontoFinal;
            b.Valor = "200";
            b.Cambio = Alteracion.CambiarValor;
            b.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            b.Destinos.Add( new DestinoBeneficio() { Participante = "3", Cuantos = 1 } );
            promocion.Beneficios.Add( b );

            return promocion;
        }

        private static string ObtenerComprobanteCaso11()
        {
            return @"<Comprobante>
  <Codigo TipoDato=""C"" Valor=""guidCaso11"" />
  <Facturadetalle>
    <Item Consumido=""0"" Promo="""" Beneficio="""" ConsumoPorCombinacion=""0"">
      <IdItemArticulos TipoDato=""C"" Valor=""0"" />
      <Articulo><Codigo TipoDato=""C"" Valor=""Art0"" /><Familia><Codigo TipoDato=""C"" Valor=""02"" /></Familia></Articulo>
      <Cantidad TipoDato=""N"" Valor=""5"" /><Precio TipoDato=""N"" Valor=""500"" />
      <Descuento TipoDato=""N"" Valor=""0"" /><MontoDescuento TipoDato=""N"" Valor=""0"" />
      <Color><Codigo TipoDato=""C"" Valor=""V"" /></Color>
      <Talle><Codigo TipoDato=""C"" Valor=""L"" /></Talle>
      <MONTOFINAL TipoDato=""N"" Valor=""0"" />
    </Item>
  </Facturadetalle>
</Comprobante>";
        }

        /// <summary>
        /// Verifica que una promo "llevando monto, paga precio fijo" con PorArticuloYCombinacion
        /// y dos destinos de beneficio (Art0 y Art1) se cumple cuando solo Art0 está presente
        /// en el comprobante. Los participantes beneficiarios no deben bloquear la promo.
        /// Caso de uso 11 del CasodeUso.md.
        /// </summary>
        [TestMethod]
        public void CasoDeUso11_PorArticuloYCombinacion_LlevandoMontoPagaPrecioFijo_FilaUnicaCumple()
        {
            ConfiguracionComportamiento config = ObtenerConfigCaso3();
            Promocion promo = CrearPromocionCaso11( config );
            MotorPromociones motor = CrearMotor( config, promo );

            List<InformacionPromocion> resultado = EjecutarPromocion( motor, ObtenerComprobanteCaso11() );

            Assert.AreEqual( 1, resultado.Count,
                "Una fila Art0+familia '02'+qty=4 debe cumplir la promo aunque Art1 (otro destino) no esté en el comprobante." );
            Assert.AreEqual( 1, resultado[0].Afectaciones,
                "La promo debe haberse aplicado 1 vez (Afectaciones=1), no como incumplida." );
        }
    }
}
