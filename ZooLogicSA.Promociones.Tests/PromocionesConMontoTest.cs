using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Promociones.Tests.Properties;

namespace ZooLogicSA.Promociones.Tests
{
	[TestClass]
	public class PromocionesConMontoTest
	{
		[TestMethod]
		public void TestI_VentaSuperiorAMontoLlevaDescuento()
		{
			ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
			IFactoriaPromociones factoria = new FactoriaPromociones();

			#region Lib Promos
			List<Promocion> promos = new List<Promocion>();

			Promocion promocion = new Promocion();
			promocion.Id = "1";
			promocion.EleccionParticipante = EleccionParticipanteType.AplicarAlDeMenorPrecio;

			ParticipanteRegla participante;
			Regla regla;

			#region Articulo.Familia.Codigo DebeSerIgualA "01"
			participante = new ParticipanteRegla();
			participante.Id = "1";
			participante.Codigo = "Comprobante.Facturadetalle.Item";

			regla = new Regla();
			regla.Id = 3;
			regla.Atributo = "Monto";
			regla.Comparacion = Factor.DebeSerIgualA;
			regla.Valor = "250";
			participante.Reglas.Add( regla );

			participante.RelaReglas = "{3}";
			promocion.Participantes.Add( participante );
			#endregion

			#region Articulo.Codigo DebeSerIgualA "00100101"
			participante = new ParticipanteRegla();
			participante.Id = "2";
			participante.Codigo = "Comprobante.Facturadetalle.Item";

			regla = new Regla();
			regla.Id = 4;
			regla.Atributo = "Articulo.Codigo";
			regla.Comparacion = Factor.DebeSerIgualA;
			regla.Valor = "00100101";
			participante.Reglas.Add( regla );

			participante.RelaReglas = "{4}";
			promocion.Participantes.Add( participante );
			#endregion

			#region Beneficios: Part 2 Descuento es 10
			Beneficio beneficio;
			beneficio = new Beneficio();
			beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
			beneficio.Valor = "10";
			beneficio.Cambio = Alteracion.CambiarValor;
			beneficio.Atributo = "Descuento";
			promocion.Beneficios.Add( beneficio );
			#endregion

			promos.Add( promocion );
			#endregion

			MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
			motorPromociones.EstablecerLibreriaPromociones( promos );

			XmlDocument comprobante = new XmlDocument();
			comprobante.LoadXml( Resources.ComprobanteCompleto );

			//comprobante.Save( @"d:\comprobante.xml" );

			string idProceso = "IdProceso";
			motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

			List<InformacionPromocion> promocionAplicada;
			promocionAplicada = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1" } );

			Assert.IsTrue( promocionAplicada.Count > 0, "La promo debe aplicar" );

			Assert.AreEqual( 20F, promocionAplicada[0].MontoBeneficio, "Mal monto total de la promo" );

			Assert.AreEqual( 1, promocionAplicada[0].DetalleAfectado[0].Cantidad, "Mal cantidad afectada" );

			Assert.AreEqual( "Descuento", promocionAplicada[0].DetalleBeneficiado[0].AtributoAlterado, "Mal atributo Alterado" );

			Assert.AreEqual( "10", promocionAplicada[0].DetalleBeneficiado[0].Valor, "Mal valor beneficio" );
		}

		[TestMethod]
		public void TestI_VentaSuperiorAMontoLlevaDescuento_MontoAConsumirEn2Participantes()
		{
			ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
			IFactoriaPromociones factoria = new FactoriaPromociones();

			#region Lib Promos
			List<Promocion> promos = new List<Promocion>();

			Promocion promocion = new Promocion();
			promocion.Id = "1";
			promocion.EleccionParticipante = EleccionParticipanteType.AplicarAlDeMenorPrecio;

			ParticipanteRegla participante;
			Regla regla;

			#region Comprobante.Facturadetalle.Item Monto DebeSerIgualA 250
			participante = new ParticipanteRegla();
			participante.Id = "1";
			participante.Codigo = "Comprobante.Facturadetalle.Item";

			regla = new Regla();
			regla.Id = 3;
			regla.Atributo = "Monto";
			regla.Comparacion = Factor.DebeSerIgualA;
			regla.Valor = "250";
			participante.Reglas.Add( regla );

			participante.RelaReglas = "{3}";
			promocion.Participantes.Add( participante );
			#endregion

			#region Articulo.Codigo DebeSerIgualA "00100101"
			participante = new ParticipanteRegla();
			participante.Id = "2";
			participante.Codigo = "Comprobante.Facturadetalle.Item";

			regla = new Regla();
			regla.Id = 4;
			regla.Atributo = "Articulo.Codigo";
			regla.Comparacion = Factor.DebeSerIgualA;
			regla.Valor = "00100101";
			participante.Reglas.Add( regla );

			participante.RelaReglas = "{4}";
			promocion.Participantes.Add( participante );
			#endregion

			#region Beneficios: Part 2 Descuento es 10
			Beneficio beneficio;
			beneficio = new Beneficio();
			beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
			beneficio.Valor = "10";
			beneficio.Cambio = Alteracion.CambiarValor;
			beneficio.Atributo = "Descuento";
			promocion.Beneficios.Add( beneficio );
			#endregion

			promos.Add( promocion );
			#endregion

			MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
			motorPromociones.EstablecerLibreriaPromociones( promos );

			XmlDocument comprobante = new XmlDocument();
			comprobante.LoadXml( Resources.ComprobanteCompleto );


			#region bostardaqueo!!!
			XmlNodeList nodos = comprobante.SelectNodes( "Comprobante/Facturadetalle/Item" );
			((XmlElement)nodos[0].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "1" );
			((XmlElement)nodos[1].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "1" );
			((XmlElement)nodos[2].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "1" );
			((XmlElement)nodos[3].SelectSingleNode( "Cantidad" )).SetAttribute( "Valor", "1" );

			//nodos[0].ParentNode.RemoveChild( nodos[0] );
			#endregion
	
			//comprobante.Save( @"d:\comprobante.xml" );

			string idProceso = "IdProceso";
			motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

			List<InformacionPromocion> promocionAplicada;
			promocionAplicada = motorPromociones.AplicarPromociones( idProceso, new List<string>() { "1" } );

			Assert.IsTrue( promocionAplicada.Count > 0, "La promo debe aplicar" );

			Assert.AreEqual( 20F, promocionAplicada[0].MontoBeneficio, "Mal monto total de la promo" );

			Assert.AreEqual( 1, promocionAplicada[0].DetalleAfectado.Sum( x=>x.Cantidad), "Mal cantidad afectada" );

			Assert.AreEqual( "Descuento", promocionAplicada[0].DetalleBeneficiado[0].AtributoAlterado, "Mal atributo Alterado" );

			Assert.AreEqual( "10", promocionAplicada[0].DetalleBeneficiado[0].Valor, "Mal valor beneficio" );
		}
	}
}
