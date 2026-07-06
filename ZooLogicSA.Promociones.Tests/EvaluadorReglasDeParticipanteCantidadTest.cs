using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Comparadores;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Tests.Properties;

namespace ZooLogicSA.Promociones.Tests
{
    /// <summary>
    ///This is a test class for EvaluadorReglasDeParticipanteTest and is intended
    ///to contain all EvaluadorReglasDeParticipanteTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EvaluadorReglasDeParticipanteCantidadTest
    {
        [TestMethod()]
        public void ObtenerResultadosTest()
        {
            ConfiguracionComportamiento config = FactoriaRecursosAdicionalesParaTest.ObtenerConfiguracionComportamiento();

            EvaluadorReglasDeParticipanteCantidad evaluador = new EvaluadorReglasDeParticipanteCantidad( config );

            ComprobanteXML comprobante = new ComprobanteXML( config );
            comprobante.Cargar( Resources.ComprobanteBasicoPruebas );

            #region Participante
            ParticipanteRegla participanteEnRegla = new ParticipanteRegla();
            participanteEnRegla.Id = "idParti";
            participanteEnRegla.Codigo = "Comprobante.Facturadetalle.Item";

            Regla regla = new Regla();
            regla.Id = 2;
            regla.Atributo = "Cantidad";
            regla.Comparacion = Factor.DebeSerIgualA;
            regla.Valor = 5;
            participanteEnRegla.Reglas.Add( regla );

            participanteEnRegla.RelaReglas = "{2}"; 
            #endregion
            
            List<ResultadoReglas> resultados = evaluador.ObtenerResultados( comprobante, participanteEnRegla, new GestorComparaciones() );

            Assert.AreEqual( 1, resultados.Count, "Mal cantidad de resultados" );

            Assert.AreEqual( 8, resultados[0].Satisfecho, "Mal cantidad satisfecho en resultado 0" );
            Assert.AreEqual( 5, resultados[0].Requerido, "Mal cantidad requerido en resultado 0" );
            Assert.AreEqual( true, resultados[0].Cumple, "Mal cumple en resultado 0" );
            Assert.AreEqual( "Comprobante.Facturadetalle.Item0-Comprobante.Facturadetalle.Item1", String.Join( "-", resultados[0].Participantes.Select( s => s.Clave + s.Id ).ToArray() ), "Mal participantes en resultado 0" );
            Assert.AreEqual( "idParti", resultados[0].PartPromo.Id, "Mal participante en resultado 0" );
            Assert.AreEqual( 2, resultados[0].Regla.Id, "Mal regla en resultado 0" );


        }

        // deprecated
        //[TestMethod()]
        //public void ObtenerResultados_Agrupamiento_Test()
        //{
        //    ConfiguracionComportamiento configuracionComportamiento = new ConfiguracionComportamiento();

        //    EvaluadorReglasDeParticipanteNoCantidad evaluador = new EvaluadorReglasDeParticipanteNoCantidad( configuracionComportamiento );

        //    ComprobanteXML comprobante = new ComprobanteXML();
        //    comprobante.Cargar( Resources.ComprobanteBasicoPruebas );

        //    #region Participante
        //    ParticipanteRegla participanteEnRegla = new ParticipanteRegla();
        //    participanteEnRegla.Id = "idParti";
        //    participanteEnRegla.Codigo = "Comprobante.Facturadetalle.Item";

        //    Regla regla;

        //    regla = new Regla();
        //    regla.Id = 2;
        //    regla.Atributo = "Cantidad";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = 5;
        //    participanteEnRegla.Reglas.Add( regla );

        //    regla = new Regla();
        //    regla.Id = 1;
        //    regla.Atributo = "Articulo.Codigo";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "ART21";
        //    participanteEnRegla.Reglas.Add( regla );

        //    regla = new Regla();
        //    regla.Id = 3;
        //    regla.Atributo = "Precio";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "10";
        //    participanteEnRegla.Reglas.Add( regla );
        //    participanteEnRegla.RelaReglas = "{1} and {2} and {3}" ;
        //    #endregion

        //    List<ResultadoReglas> r = evaluador.ObtenerResultados( comprobante, participanteEnRegla );

        //    File.AppendAllText( @"c:\resultadoreglas.txt", "= resultadoreglas =========================\r\n" );
        //    r.ForEach( x => File.AppendAllText( @"c:\resultadoreglas.txt", x.PartPromo.Id + "__" + x.Regla.Id + "\t" + x.Cumple + "\t(" + x.Satisfecho + "/" + x.Requerido + ")\t " + x.Regla.Atributo.PadLeft( 50 ) + "\t" + String.Join( "*", x.Participantes.Select( y => y.Clave + y.Id ).ToArray() ) + "\r\n" ) );

        //    Assert.AreEqual( 5, r.Count, "Mal cantidad de resultados (no agrupados?)" );
        //    Assert.AreEqual( 4, r[4].Satisfecho, "Mal satisfecho en resultado 0" );

        //    Assert.AreEqual( "Comprobante.Facturadetalle.Item0", String.Join( "-", r[0].Participantes.Select( s => s.Clave + s.Id ).ToArray() ), "Mal participantes en resultado 0" );
        //    Assert.AreEqual( "Comprobante.Facturadetalle.Item0", String.Join( "-", r[1].Participantes.Select( s => s.Clave + s.Id ).ToArray() ), "Mal participantes en resultado 1" );
        //    Assert.AreEqual( "Comprobante.Facturadetalle.Item1", String.Join( "-", r[2].Participantes.Select( s => s.Clave + s.Id ).ToArray() ), "Mal participantes en resultado 2" );
        //    Assert.AreEqual( "Comprobante.Facturadetalle.Item1", String.Join( "-", r[3].Participantes.Select( s => s.Clave + s.Id ).ToArray() ), "Mal participantes en resultado 4" );
        //}

        //[TestMethod()]
        //public void ObtenerResultados_AgrupamientoSiNecesario_Test()
        //{
        //    ConfiguracionComportamiento configuracionComportamiento = new ConfiguracionComportamiento();

        //    EvaluadorReglasDeParticipanteNoCantidad evaluador = new EvaluadorReglasDeParticipanteNoCantidad( configuracionComportamiento );

        //    XmlDocument comprobante = new XmlDocument();
        //    comprobante.LoadXml( Resources.ComprobanteBasicoPruebas );

        //    XmlNode nodoItem = comprobante.SelectSingleNode( "Comprobante/Facturadetalle/Item[Articulo/Codigo[@Valor='ART01']]" );
        //    XmlNode nodoCreadoPorTest = nodoItem.Clone();

        //    nodoItem.ParentNode.AppendChild( nodoCreadoPorTest );

        //    ComprobanteXML comprobanteXml = new ComprobanteXML();
        //    comprobanteXml.Cargar( comprobante.InnerXml );

        //    #region Participante
        //    ParticipanteRegla participanteEnRegla = new ParticipanteRegla();
        //    participanteEnRegla.Id = "idParti";
        //    participanteEnRegla.Codigo = "Comprobante.Facturadetalle.Item";

        //    Regla regla;

        //    regla = new Regla();
        //    regla.Id = 2;
        //    regla.Atributo = "Cantidad";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = 5;
        //    participanteEnRegla.Reglas.Add( regla );

        //    regla = new Regla();
        //    regla.Id = 1;
        //    regla.Atributo = "Articulo.Codigo";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "ART21";
        //    participanteEnRegla.Reglas.Add( regla );

        //    regla = new Regla();
        //    regla.Id = 3;
        //    regla.Atributo = "Precio";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "10";
        //    participanteEnRegla.Reglas.Add( regla );
        //    #endregion

        //    List<ResultadoReglas> r = evaluador.ObtenerResultados( comprobanteXml, participanteEnRegla );

        //    Assert.AreEqual( 4, r.Count, "Mal cantidad de resultados (no agrupados?)" );
        //    Assert.AreEqual( 8, r[0].Satisfecho, "Mal satisfecho en resultado 0" );

        //    Assert.AreEqual( "Comprobante.Facturadetalle.Item0-Comprobante.Facturadetalle.Item1", String.Join( "-", r[0].Participantes.Select( s => s.Clave + s.Id ).ToArray() ), "Mal participantes en resultado 0" );
        //    Assert.AreEqual( "Comprobante.Facturadetalle.Item0", String.Join( "-", r[1].Participantes.Select( s => s.Clave + s.Id ).ToArray() ), "Mal participantes en resultado 1" );
        //    Assert.AreEqual( "Comprobante.Facturadetalle.Item1", String.Join( "-", r[2].Participantes.Select( s => s.Clave + s.Id ).ToArray() ), "Mal participantes en resultado 2" );
        //    Assert.AreEqual( "Comprobante.Facturadetalle.Item0-Comprobante.Facturadetalle.Item1", String.Join( "-", r[3].Participantes.Select( s => s.Clave + s.Id ).ToArray() ), "Mal participantes en resultado 4" );
        //}

        //[TestMethod()]
        //public void ObtenerResultados_ExponerParticipantesCompatibles_Test()
        //{
        //    ConfiguracionComportamiento configuracionComportamiento = new ConfiguracionComportamiento();

        //    EvaluadorReglasDeParticipanteNoCantidad evaluador = new EvaluadorReglasDeParticipanteNoCantidad( configuracionComportamiento );

        //    XmlDocument comprobante = new XmlDocument();
        //    comprobante.LoadXml( Resources.ComprobanteBasicoPruebas );

        //    XmlNode nodoItem = comprobante.SelectSingleNode( "Comprobante/Facturadetalle/Item[Articulo/Codigo[@Valor='ART01']]" );
        //    XmlNode nodoCreadoPorTest = nodoItem.Clone();

        //    nodoItem.ParentNode.AppendChild( nodoCreadoPorTest );

        //    ComprobanteXML comprobanteXml = new ComprobanteXML();
        //    comprobanteXml.Cargar( comprobante.InnerXml );

        //    File.WriteAllText( @"c:\comprobante.xml", Serializador.Serializar<XmlDocument>( comprobante ) );

        //    #region Participante
        //    ParticipanteRegla participanteEnRegla = new ParticipanteRegla();
        //    participanteEnRegla.Id = "idParti";
        //    participanteEnRegla.Codigo = "Comprobante.Facturadetalle.Item";

        //    Regla regla;

        //    regla = new Regla();
        //    regla.Id = 2;
        //    regla.Atributo = "Cantidad";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = 5;
        //    participanteEnRegla.Reglas.Add( regla );

        //    regla = new Regla();
        //    regla.Id = 1;
        //    regla.Atributo = "Articulo.Codigo";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "ART21";
        //    participanteEnRegla.Reglas.Add( regla );

        //    regla = new Regla();
        //    regla.Id = 4;
        //    regla.Atributo = "Articulo.Codigo";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "ART01";
        //    participanteEnRegla.Reglas.Add( regla );

        //    regla = new Regla();
        //    regla.Id = 3;
        //    regla.Atributo = "Precio";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "10";
        //    participanteEnRegla.Reglas.Add( regla );
        //    #endregion

        //    List<ResultadoReglas> r = evaluador.ObtenerResultados( comprobanteXml, participanteEnRegla );

        //    //File.AppendAllText( @"c:\resultadoreglas.txt", "=========================================\r\n" );
        //    //r.ForEach( x => File.AppendAllText( @"c:\resultadoreglas.txt", x.Satisfecho + "/" + x.Requerido + "\t" + x.Cumple + "\t" + String.Join( "-", x.Participantes.Select( s => s.Clave + s.Id ).ToArray() ) + "\t" + x.Regla.Id + "\t(" + x.Regla.Atributo + ")\r\n" ) );

        //    participanteEnRegla.Reglas.Clear();
        //    regla = new Regla();
        //    regla.Id = 8;
        //    regla.Atributo = "Precio";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "10";
        //    participanteEnRegla.Reglas.Add( regla );
        //    regla = new Regla();
        //    regla.Id = 9;
        //    regla.Atributo = "Articulo.Familia";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "01";
        //    participanteEnRegla.Reglas.Add( regla );

        //    r = evaluador.ObtenerResultados( comprobanteXml, participanteEnRegla );

        //    //File.AppendAllText( @"c:\resultadoreglas.txt", "=========================================\r\n" );
        //    //r.ForEach( x => File.AppendAllText( @"c:\resultadoreglas.txt", x.Satisfecho + "/" + x.Requerido + "\t" + x.Cumple + "\t" + String.Join( "-", x.Participantes.Select( s => s.Clave + s.Id ).ToArray() ) + "\t" + x.Regla.Id + "\t(" + x.Regla.Atributo + ")\r\n" ) );

        //    Assert.Inconclusive("bla bla bla");

        //}

        //[TestMethod()]
        //public void ObtenerResultados_Agrupamiento_Test()
        //{
        //    ConfiguracionComportamiento configuracionComportamiento = new ConfiguracionComportamiento();
        //    EvaluadorReglasDeParticipante evaluador = new EvaluadorReglasDeParticipante( configuracionComportamiento );

        //    ComprobanteXML comprobante = new ComprobanteXML();
        //    comprobante.Cargar( Resources.ComprobanteBasicoPruebas );

        //    #region Participante
        //    ParticipanteRegla participanteEnRegla = new ParticipanteRegla();
        //    participanteEnRegla.Id = "idParti";
        //    participanteEnRegla.Codigo = "Comprobante.Facturadetalle.Item";

        //    Regla regla;

        //    regla = new Regla();
        //    regla.Id = 2;
        //    regla.Atributo = "Cantidad";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = 5;
        //    participanteEnRegla.Reglas.Add( regla );

        //    regla = new Regla();
        //    regla.Id = 1;
        //    regla.Atributo = "Articulo.Codigo";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "ART21";
        //    participanteEnRegla.Reglas.Add( regla );

        //    regla = new Regla();
        //    regla.Id = 3;
        //    regla.Atributo = "Precio";
        //    regla.Comparacion = Factor.DebeSerIgualA;
        //    regla.Valor = "10";
        //    participanteEnRegla.Reglas.Add( regla );
        //    #endregion

        //    List<ResultadoReglas> r = evaluador.ObtenerResultados( comprobante, participanteEnRegla );

        //    Assert.AreEqual( 4, r.Count, "Mal cantidad de resultados (no agrupados?)" );
        //    Assert.AreEqual( 8, r[0].Satisfecho, "Mal satisfecho en resultado 0" );
        //}
    }
}
