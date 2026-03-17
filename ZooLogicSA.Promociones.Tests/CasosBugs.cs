using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Promociones.Tests.Properties;

namespace ZooLogicSA.Promociones.Tests
{
    /// <summary>
    /// Summary description for CasosBugs
    /// </summary>
    [TestClass]
    public class CasosBugs
    {
		[TestMethod]
		public void PruebaQA_PromoPorMontoConTope()
		{
			#region Lib Promos
			List<Promocion> promos = new List<Promocion>();

			Promocion promocion = new Promocion();
			promocion.Id = "1";
			promocion.Recursiva = true;
			promocion.EleccionParticipante = EleccionParticipanteType.AplicarAlDeMenorPrecio;

			ParticipanteRegla participante;
			Regla regla;

			#region participante 1 Item Articulo.Codigo DebeSerIgualA 00100101 and Cantidad DebeSerIgualA 1
			participante = new ParticipanteRegla();
			participante.Id = "1";
			participante.Codigo = "Comprobante.Facturadetalle.Item";

			#region Articulo.Codigo DebeSerIgualA 00100101
			regla = new Regla();
			regla.Id = 1;
			regla.Atributo = "Articulo.Codigo";
			regla.Comparacion = Factor.DebeSerIgualA;
			regla.Valor = "00100101";
			participante.Reglas.Add( regla );
			#endregion

			#region Cantidad DebeSerIgualA 1
			regla = new Regla();
			regla.Id = 2;
			regla.Atributo = "Monto";
			regla.Comparacion = Factor.DebeSerIgualA;
			regla.Valor = 200;
			participante.Reglas.Add( regla );
			#endregion

			participante.RelaReglas = "{1} and {2}";

			promocion.Participantes.Add( participante );
			#endregion

			#region participante 1 Item Articulo.Codigo DebeSerIgualA 00100102 and Cantidad DebeSerIgualA 1
			participante = new ParticipanteRegla();
			participante.Id = "2";
			participante.Codigo = "Comprobante.Facturadetalle.Item";

			#region Articulo.Codigo DebeSerIgualA 00100102
			regla = new Regla();
			regla.Id = 1;
			regla.Atributo = "Articulo.Codigo";
			regla.Comparacion = Factor.DebeSerIgualA;
			regla.Valor = "00100102";
			participante.Reglas.Add( regla );
			#endregion

			#region Cantidad DebeSerIgualA 1
			regla = new Regla();
			regla.Id = 2;
			regla.Atributo = "Cantidad";
			regla.Comparacion = Factor.DebeSerIgualA;
			regla.Valor = 1;
			participante.Reglas.Add( regla );
			#endregion

			participante.RelaReglas = "{1} and {2}";

			promocion.Participantes.Add( participante );
			#endregion

			#region Beneficios:Participante indeterminado, 30% de descuento
			Beneficio beneficio;
			beneficio = new Beneficio();
			beneficio.Valor = "10";
			beneficio.Cambio = Alteracion.CambiarValor;
			beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
			beneficio.Atributo = "Descuento";
			promocion.Beneficios.Add( beneficio );
			#endregion

			promocion.Recursiva = false;
			promocion.TopeBeneficio = 10000;
			
			promos.Add( promocion );
			#endregion

			ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

			IFactoriaPromociones factoria = new FactoriaPromociones();

			MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
			motorPromociones.EstablecerLibreriaPromociones( promos );

			XmlDocument comprobante = new XmlDocument();
			comprobante.LoadXml( Resources.ComprobanteCompleto );

			InformantePromociones informante = new InformantePromociones( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );

			string idProceso = "IdProceso";
			motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

			List<InformacionPromocion> promocionesAplicadas = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1", "2" } );

			Assert.AreEqual( 1, promocionesAplicadas[0].DetalleAfectado.Count, "Art 00100101" );
			Assert.AreEqual( 1, promocionesAplicadas[0].DetalleBeneficiado.Count, "Art 00100102" );

			Assert.AreEqual( 26, promocionesAplicadas[0].MontoBeneficio, "Mal monto beneficio" );

			Assert.AreEqual( "Comprobante.Facturadetalle.Item", promocionesAplicadas[0].DetalleAfectado[0].Clave );
			Assert.AreEqual( "0", promocionesAplicadas[0].DetalleAfectado[0].Id );

			Assert.AreEqual( "Comprobante.Facturadetalle.Item", promocionesAplicadas[0].DetalleBeneficiado[0].Clave );
			Assert.AreEqual( "3", promocionesAplicadas[0].DetalleBeneficiado[0].Id );

		}
		
		/// <summary>
        /// http://www.zoologicnet.com.ar/pivotal/redirect.ashx?clave=bug-2075
        /// 
        /// En este caso, se carga 2 participantes items en la promo, dejando el
        /// destino del beneficio como 0 lo cual es invalido hasta ahora.
        /// (el alta de promos no determina cual deberia ser el beneficiario)
        /// </summary>
        [TestMethod]
        public void BUG_2075_LlevaUnaCantidadPagaOtraCantidad_2ParticipantesItems()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();

            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;
            promocion.EleccionParticipante = EleccionParticipanteType.AplicarAlDeMenorPrecio;

            ParticipanteRegla participante;
            Regla regla;

            #region participante 1 Item Articulo.Codigo DebeSerIgualA 00100101 and Cantidad DebeSerIgualA 1
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            #region Articulo.Codigo DebeSerIgualA 00100101
            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "00100101";
            participante.Reglas.Add( regla );
            #endregion            

            #region Cantidad DebeSerIgualA 1
            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 1;
            participante.Reglas.Add( regla );
            #endregion
            
            participante.RelaReglas = "{1} and {2}";

            promocion.Participantes.Add( participante );
            #endregion

            #region participante 1 Item Articulo.Codigo DebeSerIgualA 00100102 and Cantidad DebeSerIgualA 1
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            #region Articulo.Codigo DebeSerIgualA 00100102
            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "00100102";
            participante.Reglas.Add( regla );
            #endregion

            #region Cantidad DebeSerIgualA 1
            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 1;
            participante.Reglas.Add( regla );
            #endregion

            participante.RelaReglas = "{1} and {2}";

            promocion.Participantes.Add( participante );
            #endregion

            #region Beneficios:Participante indeterminado, 30% de descuento
            Beneficio beneficio;
            beneficio = new Beneficio();
            beneficio.Valor = "30";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "1", Cuantos = 1 } );
            beneficio.Atributo = "Descuento";
            promocion.Beneficios.Add( beneficio );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobanteCompleto );

            InformantePromociones informante = new InformantePromociones( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento() );

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> promocionesAplicadas = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1", "2" } );

            Assert.AreEqual( 1, promocionesAplicadas[0].DetalleAfectado.Count, "Art 00100101" );
            Assert.AreEqual( 1, promocionesAplicadas[0].DetalleBeneficiado.Count, "Art 00100102" );

            Assert.AreEqual( "Comprobante.Facturadetalle.Item", promocionesAplicadas[0].DetalleAfectado[0].Clave );
            Assert.AreEqual( "3", promocionesAplicadas[0].DetalleAfectado[0].Id );

            Assert.AreEqual( "Comprobante.Facturadetalle.Item", promocionesAplicadas[0].DetalleBeneficiado[0].Clave );
            Assert.AreEqual( "0", promocionesAplicadas[0].DetalleBeneficiado[0].Id );

        }
    }
}
