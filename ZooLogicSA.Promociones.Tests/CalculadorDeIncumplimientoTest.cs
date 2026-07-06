using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.EvaluacionReglas;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class CalculadorDeIncumplimientoTest
    {
        private ResultadoReglas ObtenerResultadoReglas( ParticipanteRegla participantePromocion, int idRegla, string atributo, int satisfecho, int requerido, List<IParticipante> items )
        {
            ResultadoReglas resultado = new ResultadoReglas();

            resultado = new ResultadoReglas();
            resultado.PartPromo = participantePromocion;
            resultado.Regla = new Regla() { Id = idRegla, Atributo = atributo };
            resultado.Cumple = satisfecho >= requerido;
            resultado.Satisfecho = satisfecho;
            resultado.Requerido = requerido;
            resultado.Participantes = items;

            return resultado;
        }

        [TestMethod]
        public void CalculadorDeIncumplimiento()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            #region Promocion
            Promocion promocion;
            ParticipanteRegla partpromo;
            Regla regla;

            promocion = new Promocion();
            promocion.Id = "5x4";

            #region Participante 1: Codigo = 1
            partpromo = new ParticipanteRegla();
            partpromo.Id = "1";
            partpromo.Codigo = "Comprobante.Facturadetalle.Item";

            partpromo.Reglas = new List<Regla>();
            partpromo.RelaReglas = "({1}) And ({2})";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Cantidad";
            regla.Valor = "1";
            regla.Comparacion = Factor.DebeSerIgualA;
            partpromo.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Articulo.Codigo";
            regla.ValorMuestraRelacion = "Codigo1";
            regla.Valor = "00100101";
            regla.Comparacion = Factor.DebeSerIgualA;
            partpromo.Reglas.Add( regla );

            promocion.Participantes.Add( partpromo );
            #endregion

            #region Participante 2: Color = 2 Cantidad 2
            partpromo = new ParticipanteRegla();
            partpromo.Id = "2";
            partpromo.Codigo = "Comprobante.Facturadetalle.Item";

            partpromo.Reglas = new List<Regla>();
            partpromo.RelaReglas = "({3}) And ({4})";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Cantidad";
            regla.Valor = 2;
            regla.Comparacion = Factor.DebeSerIgualA;
            partpromo.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "Color.Codigo";
            regla.ValorMuestraRelacion = "Color2";
            regla.Valor = "116 2X";
            regla.Comparacion = Factor.DebeSerIgualA;
            partpromo.Reglas.Add( regla );

            promocion.Participantes.Add( partpromo );
            #endregion

            #region Participante 3: Talle = Talle3
            partpromo = new ParticipanteRegla();
            partpromo.Id = "3";
            partpromo.Codigo = "Comprobante.Facturadetalle.Item";

            partpromo.Reglas = new List<Regla>();
            partpromo.RelaReglas = "({5}) And ({6})";

            regla = new Regla();
            regla.Id = 5;
            regla.Atributo = "Cantidad";
            regla.Valor = 1;
            regla.Comparacion = Factor.DebeSerIgualA;
            partpromo.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 6;
            regla.Atributo = "Talle.Codigo";
            regla.ValorMuestraRelacion = "Talle3";
            regla.Valor = "38";
            regla.Comparacion = Factor.DebeSerIgualA;
            partpromo.Reglas.Add( regla );

            promocion.Participantes.Add( partpromo );
            #endregion

            #region Participante 4: Articulo = 2
            partpromo = new ParticipanteRegla();
            partpromo.Id = "4";
            partpromo.Codigo = "Comprobante.Facturadetalle.Item";

            partpromo.Reglas = new List<Regla>();
            partpromo.RelaReglas = "({7}) And ({8})";

            regla = new Regla();
            regla.Id = 7;
            regla.Atributo = "Cantidad";
            regla.Valor = 1;
            regla.Comparacion = Factor.DebeSerIgualA;
            partpromo.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 8;
            regla.Atributo = "Articulo.Codigo";
            regla.ValorMuestraRelacion = "Codigo2";
            regla.Valor = "00100102";
            regla.Comparacion = Factor.DebeSerIgualA;
            partpromo.Reglas.Add( regla );

            promocion.Participantes.Add( partpromo );
            #endregion

            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Atributo = "Descuento";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Valor = "-20";

            promocion.Beneficios.Add( beneficio );

            #endregion

            #region ResultadoReglas
            IParticipante item1 = MockRepository.GenerateMock<IParticipante>();
            item1.Expect( x => x.Clave ).Return( "Comprobante.Facturadetalle.Item" );
            item1.Expect( x => x.Id ).Return( "0" );
            item1.Expect( x => x.Id ).Return( "0" );
            item1.Expect( x => x.Cantidad ).Return( 2 );

            IParticipante item2 = MockRepository.GenerateMock<IParticipante>();
            item2.Expect( x => x.Clave ).Return( "Comprobante.Facturadetalle.Item" );
            item2.Expect( x => x.Id ).Return( "1" );
            item2.Expect( x => x.Cantidad ).Return( 1 ); 

            List<ResultadoReglas> resultadoReglas = new List<ResultadoReglas>();

            resultadoReglas.Add( this.ObtenerResultadoReglas( promocion.Participantes[0], 2, "Articulo.Codigo", 1, 1, new List<IParticipante>() { item1 } ) );
            resultadoReglas.Add( this.ObtenerResultadoReglas( promocion.Participantes[0], 2, "Articulo.Codigo", 0, 1, new List<IParticipante>() { item2 } ) );
            resultadoReglas.Add( this.ObtenerResultadoReglas( promocion.Participantes[0], 1, "Cantidad", 2, 1, new List<IParticipante>() { item1 } ) );

            resultadoReglas.Add( this.ObtenerResultadoReglas( promocion.Participantes[1], 4, "Color.Codigo", 1, 1, new List<IParticipante>() { item1 } ) );
            resultadoReglas.Add( this.ObtenerResultadoReglas( promocion.Participantes[1], 4, "Color.Codigo", 1, 1, new List<IParticipante>() { item2 } ) );
            resultadoReglas.Add( this.ObtenerResultadoReglas( promocion.Participantes[1], 3, "Cantidad", 3, 2, new List<IParticipante>() { item1, item2 } ) );

            resultadoReglas.Add( this.ObtenerResultadoReglas( promocion.Participantes[2], 6, "Talle.Codigo", 1, 1, new List<IParticipante>() { item1 } ) );
            resultadoReglas.Add( this.ObtenerResultadoReglas( promocion.Participantes[2], 6, "Talle.Codigo", 0, 1, new List<IParticipante>() { item2 } ) );
            resultadoReglas.Add( this.ObtenerResultadoReglas( promocion.Participantes[2], 5, "Cantidad", 2, 1, new List<IParticipante>() { item1 } ) );

            resultadoReglas.Add( this.ObtenerResultadoReglas( promocion.Participantes[3], 8, "Articulo.Codigo", 0, 1, new List<IParticipante>() { item1 } ) );
            resultadoReglas.Add( this.ObtenerResultadoReglas( promocion.Participantes[3], 8, "Articulo.Codigo", 1, 1, new List<IParticipante>() { item2 } ) );
            resultadoReglas.Add( this.ObtenerResultadoReglas( promocion.Participantes[3], 7, "Cantidad", 1, 1, new List<IParticipante>() { item2 } ) );
            #endregion

            #region Consumock
            List<ConsumoParticipanteEvaluado> consumock = new List<ConsumoParticipanteEvaluado>();

            ConsumoParticipanteEvaluado itemConsumo;

            itemConsumo = new ConsumoParticipanteEvaluado();
            itemConsumo.CodigoParticipanteEnComprobante = "Comprobante.Facturadetalle.Item";
            itemConsumo.ParticipantesEnComprobante = new List<string>() { "0" };
            itemConsumo.IdParticipanteEnRegla = "1";
            itemConsumo.Satisfecho = 3;
            itemConsumo.Requerido = 5;
            itemConsumo.Atributos = new List<string>() { "" };
            consumock.Add( itemConsumo ); 
            #endregion

            #region IArmadorDeCoincidencias
            IArmadorDeCoincidencias armadorDeCoincidencias = MockRepository.GenerateMock<IArmadorDeCoincidencias>();
            armadorDeCoincidencias.Expect( x => x.ObtenerCoincidencias( promocion, resultadoReglas ) ).IgnoreArguments().Return( consumock ); 
            #endregion

            #region extractorCoincidencias
            IExtractorDeCoincidenciasDesdeConsumo extractorCoincidencias = MockRepository.GenerateMock<IExtractorDeCoincidenciasDesdeConsumo>();
            List<CoincidenciaEvaluacion> coincidenciasMock = new List<CoincidenciaEvaluacion>();
            coincidenciasMock.Add( new CoincidenciaEvaluacion() { Consume = 1 } );
            extractorCoincidencias.Expect( x => x.Obtener( consumock ) ).Return( coincidenciasMock ); 
            #endregion

            #region ICalculadorDePartipantesCumplidos
            ICalculadorDePartipantesCumplidos calculadorCumplidos = MockRepository.GenerateMock<ICalculadorDePartipantesCumplidos>();
            List<ParticipanteFaltante> cumplidosMock = new List<ParticipanteFaltante>() { new ParticipanteFaltante() { Participante = new ParticipanteRegla { Id = "Cumplido_Test" } } };
            calculadorCumplidos.Expect( x => x.Obtener( resultadoReglas ) ).Return( cumplidosMock );
            #endregion

            #region ICalculadorDeParticipantesFaltantes
            ICalculadorDeParticipantesFaltantes calculadorFaltantes = MockRepository.GenerateMock<ICalculadorDeParticipantesFaltantes>();
            List<ParticipanteFaltante> faltantesMock = new List<ParticipanteFaltante>();
            ParticipanteFaltante faltanteMock = new ParticipanteFaltante() { Participante = new ParticipanteRegla() };
            faltantesMock.Add( faltanteMock );
            calculadorFaltantes.Expect( x => x.Obtener( resultadoReglas ) ).Return( faltantesMock ); 
            #endregion

            #region ISimuladorDeResultadoReglas
            ISimuladorDeResultadoReglas simuladorDeResultadoReglas = MockRepository.GenerateMock<ISimuladorDeResultadoReglas>();
            simuladorDeResultadoReglas.Expect( x => x.AgregarDummies( null, null, null ) ).IgnoreArguments();
            simuladorDeResultadoReglas.Expect( x => x.QuitarDummies( null, null, null ) ).IgnoreArguments(); 
            #endregion

            #region ICalculadorDeCombinacionesDeParticipantes
            ICalculadorDeCombinacionesDeParticipantes calculadorCombinaciones = MockRepository.GenerateMock<ICalculadorDeCombinacionesDeParticipantes>();
            List<CombinacionParticipanteFaltantes> combinacionesPosiblesFaltantesMock = new List<CombinacionParticipanteFaltantes>();
            calculadorCombinaciones.Expect( x => x.ObtenerCombinaciones( null, 1 ) ).IgnoreArguments().Return( combinacionesPosiblesFaltantesMock ); 
            #endregion

            CalculadorDeIncumplimiento calculador = new CalculadorDeIncumplimiento( config, extractorCoincidencias, calculadorCumplidos, calculadorFaltantes, simuladorDeResultadoReglas, calculadorCombinaciones );
            InformacionPromocionIncumplida info = calculador.Calcular( promocion, resultadoReglas, consumock, armadorDeCoincidencias );

            Assert.AreEqual( promocion.Id, info.IdPromocion, "Mal Id promocion" );
            Assert.AreEqual( promocion, info.Promocion, "Mal promocion" );
            
            // solo los de cantidad
            Assert.AreEqual( 4, info.Resultados.Count, "Mal resultados" );
            
            Assert.AreEqual( 2, info.TotalFaltante, "Mal cantidad Total Faltante" );
            Assert.AreEqual( cumplidosMock, info.Cumplidos, "Mal cumplidos" );
            Assert.AreEqual( faltantesMock, info.FaltanteSeguro, "Mal Faltantes Seguro" );
            Assert.AreEqual( combinacionesPosiblesFaltantesMock.Count, info.FaltantePosibles.Count, "Mal Faltantes Posibles" );
        }
    }
}
