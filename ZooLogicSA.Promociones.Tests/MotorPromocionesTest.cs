using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Promociones.Tests.Properties;

namespace ZooLogicSA.Promociones.Tests
{
    [TestClass()]
    public class MotorPromocionesTest
    {
        [TestMethod()]
        public void ValidarPromocionUsandoCasoReal()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();
            Promocion promocion = new Promocion() { Id = "1" };
            promocion.Id = "1";

            ParticipanteRegla participante;
            Regla regla;

            #region Comprobante.Fecha DebeSerIgualA  00001
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Fecha";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = DateTime.ParseExact("29/03/2011", "dd/MM/yyyy", null);
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cliente.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "0000000001";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Cliente.Provincia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "1";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1} and ( {2} or {3} )";
            promocion.Participantes.Add( participante );
            #endregion

            #region Item.Articulo = ART21 OR Item.PRECIO = 50
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerMayorIgualA;
            regla.Valor = "00100101";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "Precio";
            regla.Comparacion = Factor.DebeSerMayorIgualA;
            regla.Valor = "200";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3} or {4}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Articulo.Codigo DebeSerIgualA 00100102
            participante = new ParticipanteRegla();
            participante.Id = "3";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 5;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "00100102";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{5}";
            promocion.Participantes.Add( participante );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            XmlDocument comprobante = new XmlDocument();
            comprobante.LoadXml( Resources.ComprobantePoli );

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> respuesta = motorPromociones.EvaluarPromocionesIndividualmente( idProceso, new List<string>() { "1" } );
            
            //RespuestaEvaluacion respuesta = motorPromociones.ValidarPromocion( idProceso, "1" );
            Assert.IsTrue( respuesta[0] != null, "Debe validar, la promo aplica" );
        }
        
        [TestMethod()]
        public void ValidarComprobanteYPromocionComplejos()
		{
			#region Lib Promos 1-Comprobante.Fecha DebeSerIgualA  12/07/2012 Cliente.Codigo DebeSerIgualA 00001 Cliente.Provincia.Codigo DebeSerIgualA 01, Item.Articulo = ART21 OR Item.PRECIO = 50, Item.Articulo = OTRO
			List<Promocion> promos = new List<Promocion>();
            Promocion promocion = new Promocion() { Id = "1" };
            promocion.Id = "1";

            ParticipanteRegla participante;
            Regla regla;

            #region Comprobante.Fecha DebeSerIgualA  12/07/2012 Cliente.Codigo DebeSerIgualA 00001 Cliente.Provincia.Codigo DebeSerIgualA 01
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Fecha";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = DateTime.ParseExact("12/07/2012", "dd/MM/yyyy", null);
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cliente.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "00001";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Cliente.Provincia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "1";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1} and ( {2} or {3} )";
            promocion.Participantes.Add( participante );
            #endregion

            #region Item.Articulo = ART21 OR Item.PRECIO = 50
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "ART21";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "Precio";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "50";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3} or {4}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Item.Articulo = OTRO
            participante = new ParticipanteRegla();
            participante.Id = "3";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 5;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "OTRO";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{5}";
            promocion.Participantes.Add( participante );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
            config.ConfiguracionesPorParticipante["Comprobante"].AtributoId = "Codigo";

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            #region XML... cliente codigo 00001, provincia 01-Prov, 2 items (OTRO $50, ART105 $50)
            XmlDocument comprobante = new XmlDocument();
            XmlElement nodoComprobante = comprobante.CreateElement( "Comprobante" );
            comprobante.AppendChild( nodoComprobante );

            XmlElement nodoId = comprobante.CreateElement( "Codigo" );
            nodoId.SetAttribute( "TipoDato", "C" );
            nodoId.SetAttribute( "Valor", "guidFactu" );
            nodoComprobante.AppendChild( nodoId );

            XmlElement nodoFecha = comprobante.CreateElement( "Fecha" );
            nodoFecha.SetAttribute( "TipoDato", "D" );
            nodoFecha.SetAttribute( "Valor", "12/07/2012" );
            nodoComprobante.AppendChild( nodoFecha );

            XmlElement nodoProvincia = comprobante.CreateElement( "Provincia" );
            XmlElement Provincia_Codigo = comprobante.CreateElement( "Codigo" );
            XmlElement nodoCliente = comprobante.CreateElement( "Cliente" );
            XmlElement Cliente_Codigo = comprobante.CreateElement( "Codigo" );
            Cliente_Codigo.SetAttribute( "TipoDato", "C" );
            Cliente_Codigo.SetAttribute( "Valor", "00002" );
            nodoCliente.AppendChild( Cliente_Codigo );
            nodoComprobante.AppendChild( nodoCliente );

            XmlElement Provincia_Descri = comprobante.CreateElement( "Descripcion" );
            Provincia_Codigo.SetAttribute( "TipoDato", "N" );
            Provincia_Codigo.SetAttribute( "Valor", "1" );
            Provincia_Descri.SetAttribute( "TipoDato", "C" );
            Provincia_Descri.SetAttribute( "Valor", "Prov" );
            nodoProvincia.AppendChild( Provincia_Codigo );
            nodoProvincia.AppendChild( Provincia_Descri );
            nodoCliente.AppendChild( nodoProvincia );

            XmlElement nodoDetalle = comprobante.CreateElement( "Facturadetalle" );
            nodoComprobante.AppendChild( nodoDetalle );

            // OTRO $50
            XmlElement item1 = comprobante.CreateElement( "Item" );
            XmlElement articulo1 = comprobante.CreateElement( "Articulo" );
            XmlElement codigoArticulo1 = comprobante.CreateElement( "Codigo" );
            codigoArticulo1.SetAttribute( "TipoDato", "C" );
            codigoArticulo1.SetAttribute( "Valor", "OTRO" );
            articulo1.AppendChild( codigoArticulo1 );
            item1.AppendChild( articulo1 );
            XmlElement precioArticulo1 = comprobante.CreateElement( "Precio" );
            precioArticulo1.SetAttribute( "TipoDato", "N" );
            precioArticulo1.SetAttribute( "Valor", "50" );
            item1.AppendChild( precioArticulo1 );

            XmlElement IdArticulo1 = comprobante.CreateElement( "IdItemArticulos" );
            IdArticulo1.SetAttribute( "TipoDato", "C" );
            IdArticulo1.SetAttribute( "Valor", "0" );
            item1.AppendChild( IdArticulo1 );

            nodoDetalle.AppendChild( item1 );

            // ART105 $50
            XmlElement item2 = comprobante.CreateElement( "Item" );
            XmlElement articulo2 = comprobante.CreateElement( "Articulo" );
            XmlElement articulo21 = comprobante.CreateElement( "Codigo" );
            articulo21.SetAttribute( "TipoDato", "C" );
            articulo21.SetAttribute( "Valor", "ART105" );
            articulo2.AppendChild( articulo21 );
            item2.AppendChild( articulo2 );
            
            XmlElement articulo2Cantidad = comprobante.CreateElement( "Cantidad" );
            articulo2Cantidad.SetAttribute( "TipoDato", "N" );
            articulo2Cantidad.SetAttribute( "Valor", "4" );
            item2.AppendChild( articulo2Cantidad );

            XmlElement IdArticulo2 = comprobante.CreateElement( "IdItemArticulos" );
            IdArticulo2.SetAttribute( "TipoDato", "C" );
            IdArticulo2.SetAttribute( "Valor", "1" );
            item2.AppendChild( IdArticulo2 );

            XmlElement precio2 = comprobante.CreateElement( "Precio" );
            precio2.SetAttribute( "TipoDato", "N" );
            precio2.SetAttribute( "Valor", "50" );
            item2.AppendChild( precio2 );
            nodoDetalle.AppendChild( item2 );
            #endregion

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, comprobante.InnerXml );

            List<InformacionPromocion> respuesta = motorPromociones.EvaluarPromocionesIndividualmente( idProceso, new List<string>() { "1" } );

            Assert.IsTrue( respuesta[0] != null, "Debe validar, la promo aplica" );
            Assert.AreEqual( respuesta[0].DetalleAfectado.Count + respuesta[0].DetalleBeneficiado.Count , 3, "Debe validar, la promo aplica" );

            Assert.AreEqual( "Comprobante.Facturadetalle.Item", respuesta[0].DetalleAfectado[0].Clave, "Error en coincidencia 2 (Codigo)" );
            Assert.AreEqual( "1", respuesta[0].DetalleAfectado[0].Id, "Error en coincidencia 2 (IdParticipanteEnComprobante)" );

            Assert.AreEqual( "Comprobante.Facturadetalle.Item", respuesta[0].DetalleAfectado[1].Clave, "Error en coincidencia 0 (Codigo)" );
            Assert.AreEqual( "0", respuesta[0].DetalleAfectado[1].Id, "Error en coincidencia 0 (IdParticipanteEnComprobante)" );

            Assert.AreEqual( "Comprobante", respuesta[0].DetalleAfectado[2].Clave, "Error en coincidencia 1 (Codigo)" );
            Assert.AreEqual( "guidFactu", respuesta[0].DetalleAfectado[2].Id, "Error en coincidencia 1 (IdParticipanteEnComprobante)" );
        }

        [TestMethod()]
        public void ExponerParticipantes()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();
            Promocion promocion = new Promocion() { Id = "1" };
            promocion.Id = "1";

            ParticipanteRegla participante;
            Regla regla;

            #region Comprobante.Fecha DebeSerIgualA 12/07/2012, clienteCodigo DebeSerIgualA 00001, Cliente.Provincia.Codigo DebeSerIgualA 01
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Fecha";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = DateTime.ParseExact("12/07/2012", "dd/MM/yyyy", null);
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cliente.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "00001";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Cliente.Provincia.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "01";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1} and ( {2} or {3} )";
            promocion.Participantes.Add( participante );
            #endregion

            #region Item.Articulo = ART21 OR Item.PRECIO = 50
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "ART21";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "Precio";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "50";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3} or {4}";
            promocion.Participantes.Add( participante );
            #endregion

            #region Item.Articulo = OTRO
            participante = new ParticipanteRegla();
            participante.Id = "3";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 5;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "OTRO";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{5}";
            promocion.Participantes.Add( participante );
            #endregion

            promos.Add( promocion );
            #endregion

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento(), factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            Assert.IsTrue( motorPromociones.ParticipantesNecesarios.Count == 5, "Debe tener 5 participantes+atributos necesarios" );
            Assert.AreEqual( "Comprobante.Fecha", motorPromociones.ParticipantesNecesarios[0], "Mal participante 0" );
            Assert.AreEqual( "Comprobante.Cliente.Codigo", motorPromociones.ParticipantesNecesarios[1], "Mal participante 1" );
            Assert.AreEqual( "Comprobante.Cliente.Provincia.Codigo", motorPromociones.ParticipantesNecesarios[2], "Mal participante 2" );
            Assert.AreEqual( "Comprobante.Facturadetalle.Item.Articulo.Codigo", motorPromociones.ParticipantesNecesarios[3], "Mal participante 3" );
            Assert.AreEqual( "Comprobante.Facturadetalle.Item.Precio", motorPromociones.ParticipantesNecesarios[4], "Mal participante 4" );
        }

        [TestMethod()]
        public void EvaluarYDevolverCantidadQueConsume()
        {
            XmlDocument xmlComprobante = new XmlDocument();
            xmlComprobante.LoadXml( Resources.ComprobanteBasicoPruebas );

            #region comprobante
            XmlNode nodoItem1 = xmlComprobante.SelectSingleNode( "Comprobante/Facturadetalle/Item[Articulo/Codigo[@Valor='ART21']]" );
            XmlNode nodoItem2 = xmlComprobante.SelectSingleNode( "Comprobante/Facturadetalle/Item[Articulo/Codigo[@Valor='ART01']]" );
            //((XmlElement)nodoItem1).SetAttribute( "Id", "0" );
            //((XmlElement)nodoItem2).SetAttribute( "Id", "1" );
            #endregion

            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();
            Promocion promocion = new Promocion();
            promocion.Id = "1";

            ParticipanteRegla participante;
            Regla regla;

            #region Art21 y cant 4 o Art105 y cant 2
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "ART21";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 4;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "2";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 5;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "ART105";
            participante.Reglas.Add( regla );

            regla = new Regla();
            regla.Id = 6;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "4";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "({3} and {4}) or ( {5} AND {6} )";
            promocion.Participantes.Add( participante );
            #endregion

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, xmlComprobante.InnerXml );

            List<InformacionPromocion> respuesta = motorPromociones.EvaluarPromocionesIndividualmente( idProceso, new List<string>() { "1" } );

            Assert.AreEqual( 2, respuesta[0].DetalleAfectado.Sum( x => x.Cantidad ), "No devolvio la cantidad de consumidos correcta" );
        }

        [TestMethod()]
        public void EvaluarYAplicarPromoRecursiva()
        {
            XmlDocument xmlComprobante = new XmlDocument();
            xmlComprobante.LoadXml( Resources.ComprobanteBasicoPruebas );

            #region comprobante
            XmlNode nodoItem1 = xmlComprobante.SelectSingleNode( "Comprobante/Facturadetalle/Item[Articulo/Codigo[@Valor='ART21']]" );
            XmlNode nodoItem2 = xmlComprobante.SelectSingleNode( "Comprobante/Facturadetalle/Item[Articulo/Codigo[@Valor='ART01']]" );

            //((XmlElement)nodoItem1).SetAttribute( "Id", "0" );
            //((XmlElement)nodoItem2).SetAttribute( "Id", "1" );
            #endregion

            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;

            ParticipanteRegla participante;
            Regla regla;

            #region fecha 12/7 e items con cant 1
            #region MyRegion
            participante = new ParticipanteRegla();
            participante.Id = "1";
            participante.Codigo = "Comprobante";

            regla = new Regla();
            regla.Id = 3;
            regla.Atributo = "Fecha";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = DateTime.ParseExact( "12/07/2012", "dd/MM/yyyy", null );
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{3}";
            promocion.Participantes.Add( participante ); 
            #endregion

            #region MyRegion
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "1";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante ); 
            #endregion
            #endregion

            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Atributo = "Precio";
            beneficio.Cambio = Alteracion.DisminuirEnCantidad;
            beneficio.Valor = "2";

            promocion.Beneficios.Add( beneficio );

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, xmlComprobante.InnerXml );

            List<InformacionPromocion> respuesta = motorPromociones.AplicarPromociones( "IdProceso", new List<string>() { "1" } );

            Assert.AreEqual( 4, respuesta[0].DetalleBeneficiado[0].Cantidad, "No aplicó el beneficio a la cantidad correcta (1)" );
            Assert.AreEqual( 4, respuesta[0].DetalleBeneficiado[0].Cantidad, "No aplicó el beneficio a la cantidad correcta (2)" );
        }

        [TestMethod()]
        public void EvaluarYAplicar_CantidadDeAfectaciones()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;

            ParticipanteRegla participante;
            Regla regla;

            #region item Cantidad DebeSerIgualA 1
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "1";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Atributo = "Descuento";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Valor = "2";

            promocion.Beneficios.Add( beneficio );

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, Resources.ComprobanteBasicoPruebas );

            List<InformacionPromocion> respuesta = motorPromociones.AplicarPromociones( "IdProceso", new List<string>() { "1" } );

            Assert.AreEqual( 8, respuesta[0].Afectaciones, "No afecto la promo la cantidad de veces esperada" );
        }

        [TestMethod()]
        public void EvaluarYAplicar_MontoBeneficio()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;

            ParticipanteRegla participante;
            Regla regla;

            #region item Cantidad DebeSerIgualA 1
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "1";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 2 } );
            beneficio.Atributo = "Descuento";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Valor = "20";

            promocion.Beneficios.Add( beneficio );

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, Resources.ComprobanteBasicoPruebas );

            List<InformacionPromocion> respuesta = motorPromociones.AplicarPromociones( "IdProceso", new List<string>() { "1" } );

            Assert.AreEqual( 16, respuesta[0].MontoBeneficio, "No aplico el monto esperado" );
        }

        [TestMethod()]
        public void Aplicar_EvitarBeneficioNegativo()
        {
            #region Lib Promos
            List<Promocion> promos = new List<Promocion>();
            Promocion promocion = new Promocion();
            promocion.Id = "1";
            promocion.Recursiva = true;

            ParticipanteRegla participante;
            Regla regla;

            #region item Cantidad DebeSerIgualA 1
            participante = new ParticipanteRegla();
            participante.Id = "2";
            participante.Codigo = "Comprobante.Facturadetalle.Item";

            regla = new Regla();
            regla.Id = 1;
            regla.Atributo = "Articulo.Codigo";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = "ART21";
            participante.Reglas.Add( regla );

            participante.RelaReglas = "{1}";
            promocion.Participantes.Add( participante );
            #endregion

            Beneficio beneficio = new Beneficio();
            beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
            beneficio.Atributo = "Descuento";
            beneficio.Cambio = Alteracion.CambiarValor;
            beneficio.Valor = "-20";

            promocion.Beneficios.Add( beneficio );

            promos.Add( promocion );
            #endregion

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            string idProceso = "IdProceso";
            motorPromociones.AgregarComprobanteParaEvaluacion( idProceso, Resources.ComprobanteBasicoPruebas );

            List<InformacionPromocion> respuesta = motorPromociones.AplicarPromociones( "IdProceso", new List<string>() { "1" } );

            Assert.AreEqual( 0, respuesta[0].Afectaciones, "No debe aplicar" );
        }

        [TestMethod]
        public void ObtenerPromociones()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();
            
            IFactoriaPromociones factoria = new FactoriaPromociones();
            
            MotorPromociones mp = new MotorPromociones( config, factoria );

            List<Promocion> listaPromociones = new List<Promocion>();

            Promocion promo1 = new Promocion();
            promo1.Id = "CODIGOPROMO1";
            promo1.Descripcion = "DESCRIP1"; 
            listaPromociones.Add( promo1 );
            mp.EstablecerLibreriaPromociones( listaPromociones );

            List<Promocion> ListaResultado = mp.ObtenerPromociones();

            Assert.AreEqual( 1, ListaResultado.Count );
            Assert.AreEqual( "CODIGOPROMO1", ListaResultado[0].Id );
            Assert.AreEqual( "DESCRIP1", ListaResultado[0].Descripcion );

        }
        

        [TestMethod]
        public void ObtenerResultadosParcialesas_ConDetallePosiblesFaltantes()
        {
            List<Promocion> promos;
            Promocion promo;
            ParticipanteRegla partpromo;
            Regla regla;

            promos = new List<Promocion>();

            #region Promocion
            promo = new Promocion();
            promo.Id = "5x4";

			#region Participante 1: Codigo = 00100101
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

            promo.Participantes.Add( partpromo );
            #endregion

			#region Participante 2: Color = 116 2X Cantidad 2
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

            promo.Participantes.Add( partpromo );
            #endregion

            #region Participante 3: Talle = 38
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

            promo.Participantes.Add( partpromo );
            #endregion

			#region Participante 4: Articulo = 00100102
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

            promo.Participantes.Add( partpromo );
            #endregion

			#region Beneficio
			Beneficio beneficio = new Beneficio();
			beneficio.Destinos.Add( new DestinoBeneficio() { Participante = "2", Cuantos = 1 } );
			beneficio.Atributo = "Descuento";
			beneficio.Cambio = Alteracion.CambiarValor;
			beneficio.Valor = "-20";

			promo.Beneficios.Add( beneficio ); 
			#endregion

            #endregion
            
			promos.Add( promo );

            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            IFactoriaPromociones factoria = new FactoriaPromociones();

            MotorPromociones motorPromociones = new MotorPromociones( config, factoria );
            motorPromociones.EstablecerLibreriaPromociones( promos );

            XmlDocument xmlComprobante = new XmlDocument();
            xmlComprobante.LoadXml( Resources.ComprobanteCompleto );

            #region comprobante
            this.CambiarValor( xmlComprobante, "Comprobante/Facturadetalle/Item[IdItemArticulos[@Valor='0']]/Cantidad", "2" );
            this.CambiarValor( xmlComprobante, "Comprobante/Facturadetalle/Item[IdItemArticulos[@Valor='1']]/Cantidad", "1" );
            this.CambiarValor( xmlComprobante, "Comprobante/Facturadetalle/Item[IdItemArticulos[@Valor='1']]/Articulo/Codigo", "00100102" );
            //this.CambiarValor( xmlComprobante, "Comprobante/Facturadetalle/Item[IdItemArticulos[@Valor='1']]/Color/Codigo", "115 2X" );
            
            XmlNode nodoDetalle = xmlComprobante.SelectSingleNode( "Comprobante/Facturadetalle" );
            nodoDetalle.RemoveChild( xmlComprobante.SelectSingleNode( "Comprobante/Facturadetalle/Item[IdItemArticulos[@Valor='2']]" ) );
            nodoDetalle.RemoveChild( xmlComprobante.SelectSingleNode( "Comprobante/Facturadetalle/Item[IdItemArticulos[@Valor='3']]" ) );
            #endregion


			XmlNodeList n = xmlComprobante.SelectNodes( "Comprobante/Facturadetalle/Item" );

			//foreach ( XmlNode nodo in n )
			//{
			//	File.AppendAllText( @"d:\comprobantedebug.txt", nodo.SelectSingleNode( "Articulo/Codigo" ).Attributes["Valor"].Value + "_" );
			//	File.AppendAllText( @"d:\comprobantedebug.txt", nodo.SelectSingleNode( "Color/Codigo" ).Attributes["Valor"].Value + "_" );
			//	File.AppendAllText( @"d:\comprobantedebug.txt", nodo.SelectSingleNode( "Talle/Codigo" ).Attributes["Valor"].Value + "_" );
			//	File.AppendAllText( @"d:\comprobantedebug.txt", nodo.SelectSingleNode( "Cantidad" ).Attributes["Valor"].Value + "_" );
			//	File.AppendAllText( @"d:\comprobantedebug.txt", "\r\n" );
			//}

			//xmlComprobante.Save( @"d:\comprobante.xml" );

            IComprobante comprobante = new ComprobanteXML( config );
            comprobante.Cargar( xmlComprobante.InnerXml );

            InformacionPromocionIncumplida informacionPromociones = motorPromociones.ObtenerResultadosParciales( comprobante, "5x4" );

			//string archivo = @"d:\temp\debug_Faltantes.txt";

			//File.WriteAllText( archivo, "" );

			//foreach ( CombinacionParticipanteFaltantes combinacion in informacionPromociones.FaltantePosibles )
			//{
			//	File.AppendAllText( archivo, String.Join( "_", combinacion.Faltantes.Select( x => x.Participante.Id ).ToArray() ) + "\r\n" );
			//}

            Assert.AreEqual( 0, informacionPromociones.Cumplidos.Count );
            Assert.AreEqual(16, informacionPromociones.FaltantePosibles.Count);
            Assert.AreEqual( 0, informacionPromociones.FaltanteSeguro.Count );
        }

        private void CambiarValor( XmlDocument xmlComprobante, string p1, string p2 )
        {
            XmlNode nodoDetalle = xmlComprobante.SelectSingleNode( p1 );
            nodoDetalle.Attributes["Valor"].Value = p2;
        }
    }
}