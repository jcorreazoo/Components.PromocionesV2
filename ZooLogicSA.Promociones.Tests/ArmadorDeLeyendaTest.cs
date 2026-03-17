using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooLogicSA.Promociones.Asistente;
using ZooLogicSA.Promociones.Asistente.ArmadoDeLeyenda;
using ZooLogicSA.Promociones.FormatoPromociones;


namespace ZooLogicSA.Promociones.Tests
{
    [TestClass]
    public class ArmadorDeLeyendaTest
    {
        [TestMethod]
        public void ObtenerLeyendaFaltantesArtibutoCodigoConUnaSolaCondicion()
        {
			Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante> armadores = new Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante>();
			armadores.Add( "", new ArmadorDeLeyendaSegunParticipanteFaltanteGenerico() );

			ArmadorDeLeyenda Armador = new ArmadorDeLeyenda( armadores );
            List<InformacionPromocionIncumplida> Lista = new List<InformacionPromocionIncumplida>();

            InformacionPromocionIncumplida info1 = new InformacionPromocionIncumplida();

            info1.Resultados = new List<ResultadoReglas>();
            info1.IdPromocion = "4x3";
            info1.Promocion = new Promocion() { Tipo = "0" };

            ParticipanteRegla partpromo = new ParticipanteRegla();
            partpromo.Reglas = new List<Regla>();
            partpromo.RelaReglas = "({1}) And ({2})";
            Regla reglaCantidad = new Regla();
            Regla reglaAtributo = new Regla();

            reglaCantidad.Atributo = "CANTIDAD";
            reglaCantidad.Id = 2;
            reglaAtributo.Atributo = "ARTICULO.CODIGO";
            reglaAtributo.Valor = "00100101";
            reglaAtributo.ValorMuestraRelacion = "Articulo ValorMuestraRelacion";
            reglaAtributo.Id = 0;
            partpromo.Reglas.Add(reglaCantidad);
            partpromo.Reglas.Add(reglaAtributo);

            info1.FaltanteSeguro.Add( new ParticipanteFaltante() { Participante = partpromo, Cantidad = 3 } );

            string leyenda = Armador.Armar(info1);

            Assert.AreEqual("3 Articulo ValorMuestraRelacion", leyenda);

        }
        
        [TestMethod]
        public void ObtenerLeyendaFaltantesArtibutoCodigoConUnaDosCondiciones()
        {
			Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante> armadores = new Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante>();
			armadores.Add( "", new ArmadorDeLeyendaSegunParticipanteFaltanteGenerico() );

			ArmadorDeLeyenda Armador = new ArmadorDeLeyenda( armadores );
            List<InformacionPromocionIncumplida> Lista = new List<InformacionPromocionIncumplida>();

            InformacionPromocionIncumplida incumplida = new InformacionPromocionIncumplida();

            incumplida.Resultados = new List<ResultadoReglas>();
            incumplida.IdPromocion = "4x3";
            incumplida.Promocion = new Promocion() { Tipo = "0" };

            ParticipanteRegla partpromo = new ParticipanteRegla();
            partpromo.Reglas = new List<Regla>();
            partpromo.RelaReglas = "({1} And {2}) And ({3})";
            Regla reglaCantidad = new Regla();
            Regla reglaAtributo = new Regla();

            reglaCantidad.Atributo = "CANTIDAD";
            reglaCantidad.Id = 3;
            reglaAtributo.Atributo = "ARTICULO.CODIGO";
            reglaAtributo.Valor = "00100101";
            reglaAtributo.ValorMuestraRelacion = "Articulo ValorMuestraRelacion";
            reglaAtributo.Id = 1;
            Regla reglaAtributoColor = new Regla();
            reglaAtributoColor.ValorMuestraRelacion = "Color Azul";
            reglaAtributoColor.Id = 2;

            partpromo.Reglas.Add( reglaCantidad );
            partpromo.Reglas.Add( reglaAtributo );
            partpromo.Reglas.Add( reglaAtributoColor );

            incumplida.FaltanteSeguro.Add( new ParticipanteFaltante() { Participante = partpromo, Cantidad = 3 } );

            string leyenda = Armador.Armar( incumplida );

            Assert.AreEqual("3 Articulo ValorMuestraRelacion y Color Azul", leyenda);

        }

		[TestMethod]
		public void ObtenerLeyendaSegunRelaReglasCondicionO()
		{
			Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante> armadores = new Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante>();
			armadores.Add( "", new ArmadorDeLeyendaSegunParticipanteFaltanteGenerico() );

			ArmadorDeLeyenda Armador = new ArmadorDeLeyenda( armadores );
			List<InformacionPromocionIncumplida> Lista = new List<InformacionPromocionIncumplida>();

			InformacionPromocionIncumplida info1 = new InformacionPromocionIncumplida();

			info1.Resultados = new List<ResultadoReglas>();
			info1.IdPromocion = "4x3";
            info1.Promocion = new Promocion() { Tipo = "0" };

            ParticipanteRegla partpromo = new ParticipanteRegla();
			partpromo.Reglas = new List<Regla>();

			partpromo.RelaReglas = "({1} Or {2}) And ({3})";

			Regla reglaCantidad = new Regla();
			Regla reglaAtributo = new Regla();

			reglaCantidad.Atributo = "CANTIDAD";
			reglaCantidad.Id = 3;

			reglaAtributo.Atributo = "ARTICULO.CODIGO";
			reglaAtributo.Valor = "00100101";
			reglaAtributo.ValorMuestraRelacion = "Articulo ValorMuestraRelacion";
			reglaAtributo.Id = 1;
			Regla reglaAtributoColor = new Regla();
			reglaAtributoColor.ValorMuestraRelacion = "Color Azul";
			reglaAtributoColor.Id = 2;

			partpromo.Reglas.Add( reglaCantidad );
			partpromo.Reglas.Add( reglaAtributo );
			partpromo.Reglas.Add( reglaAtributoColor );

			info1.FaltanteSeguro.Add( new ParticipanteFaltante() { Participante = partpromo, Cantidad = 3 } );

			string leyenda = Armador.Armar( info1 );

			Assert.AreEqual( "3 Articulo ValorMuestraRelacion o Color Azul", leyenda );

		}

		[TestMethod]
		public void ObtenerLeyendaParaComprobante()
		{
			Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante> armadores = new Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante>();
			armadores.Add( "", new ArmadorDeLeyendaSegunParticipanteFaltanteGenerico() );
			armadores.Add( "COMPROBANTE", new ArmadorDeLeyendaSegunParticipanteFaltanteComprobante( new ArmadorDeLeyendaSegunParticipanteFaltanteGenerico() ) );

			ArmadorDeLeyenda Armador = new ArmadorDeLeyenda( armadores );
			List<InformacionPromocionIncumplida> Lista = new List<InformacionPromocionIncumplida>();

			InformacionPromocionIncumplida info1 = new InformacionPromocionIncumplida();

			info1.Resultados = new List<ResultadoReglas>();
			info1.IdPromocion = "CODIGOPROMO";
            info1.Promocion = new Promocion() { Tipo = "0" };

            ParticipanteRegla partpromo = new ParticipanteRegla();
			partpromo.Codigo = "COMPROBANTE";

			partpromo.Reglas = new List<Regla>();
			partpromo.RelaReglas = "( ( ((( {1} and {2} )) and (( {3} and {4} ) )) and ( {5} or {6} or {7} or {8} or {9} or {10} or {11} ) ) And ( ( {12} ) ) ) And {13}";

			partpromo.Reglas.Add( new Regla()
											{
												Atributo = "FECHA",
												ValorMuestraRelacion = "",
												Comparacion = Factor.DebeSerMayorIgualA,
												ValorString = "2016-03-21",
												Valor = DateTime.Parse("2016-03-21"),
												Id = 1
											}
								);

			partpromo.Reglas.Add( new Regla()
											{
												Atributo = "FECHA",
												ValorMuestraRelacion = "",
												Comparacion = Factor.DebeSerMenorIgualA,
												ValorString = "2016-12-31",
												Valor = DateTime.Parse( "2016-12-31" ),
												Id = 2
											}
								);

			partpromo.Reglas.Add( new Regla()
			{
				Atributo = "HORAALTAFW",
				ValorMuestraRelacion = "",
				Comparacion = Factor.DebeSerMayorIgualA,
				ValorString = "00:00:00",
				Id = 3
			}
								);


			partpromo.Reglas.Add( new Regla()
			{
				Atributo = "HORAALTAFW",
				ValorMuestraRelacion = "",
				Comparacion = Factor.DebeSerMenorIgualA,
				ValorString = "23:59:59",
				Id = 4
			}
								);


			partpromo.Reglas.Add( new Regla()
			{
				Atributo = "FECHA",
				ValorMuestraRelacion = "",
				Comparacion = Factor.DebeSerIgualADiaDeLaSemana,
				Valor = 1,
				ValorString = "1",
				Id = 5
			}
								);

			partpromo.Reglas.Add( new Regla()
			{
				Atributo = "FECHA",
				ValorMuestraRelacion = "",
				Comparacion = Factor.DebeSerIgualADiaDeLaSemana,
				Valor = 2,
				ValorString = "2",
				Id = 6
			}
								);

			partpromo.Reglas.Add( new Regla()
			{
				Atributo = "FECHA",
				ValorMuestraRelacion = "",
				Comparacion = Factor.DebeSerIgualADiaDeLaSemana,
				Valor = 3,
				ValorString = "3",
				Id = 7
			}
								);

			partpromo.Reglas.Add( new Regla()
			{
				Atributo = "FECHA",
				ValorMuestraRelacion = "",
				Comparacion = Factor.DebeSerIgualADiaDeLaSemana,
				Valor = 4,
				ValorString = "4",
				Id = 8
			}
								);

			partpromo.Reglas.Add( new Regla()
			{
				Atributo = "FECHA",
				ValorMuestraRelacion = "",
				Comparacion = Factor.DebeSerIgualADiaDeLaSemana,
				Valor = 5,
				ValorString = "5",
				Id = 9
			}
								);

			partpromo.Reglas.Add( new Regla()
			{
				Atributo = "FECHA",
				ValorMuestraRelacion = "",
				Comparacion = Factor.DebeSerIgualADiaDeLaSemana,
				Valor = 6,
				ValorString = "6",
				Id = 10
			}
								);

			partpromo.Reglas.Add( new Regla()
			{
				Atributo = "FECHA",
				ValorMuestraRelacion = "",
				Comparacion = Factor.DebeSerIgualADiaDeLaSemana,
				Valor = 0,
				ValorString = "0",
				Id = 11
			}
								);

			partpromo.Reglas.Add( new Regla()
			{
				Atributo = "CLIENTE.CODIGO",
				ValorMuestraRelacion = "Cliente Martín Gomez ",
				Comparacion = Factor.DebeSerIgualA,
				ValorString = "'0000000001'",
				Id = 12
			}
								);

			partpromo.Reglas.Add( new Regla()
			{
				Atributo = "CANTIDAD",
				ValorMuestraRelacion = "",
				Comparacion = Factor.DebeSerIgualA,
				ValorString = "1",
				Id = 13
			}
								);

			info1.FaltanteSeguro.Add( new ParticipanteFaltante() { Participante = partpromo, Cantidad = 1 } );

			string leyenda = Armador.Armar( info1 );

			string expected = "Cliente Martín Gomez";

			Assert.AreEqual( expected, leyenda );

		}
        [TestMethod]
        public void ObtenerLeyendaFaltantesArtibutoCodigoConUnaSolaCondicionSinRelacion_1()
        {
            Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante> armadores = new Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante>();
            armadores.Add("", new ArmadorDeLeyendaSegunParticipanteFaltanteGenerico());

            ArmadorDeLeyenda Armador = new ArmadorDeLeyenda(armadores);
            List<InformacionPromocionIncumplida> Lista = new List<InformacionPromocionIncumplida>();

            InformacionPromocionIncumplida info1 = new InformacionPromocionIncumplida();

            info1.Resultados = new List<ResultadoReglas>();
            info1.IdPromocion = "4x3";
            info1.Promocion = new Promocion() { Tipo = "0" };

            ParticipanteRegla partpromo = new ParticipanteRegla();
            partpromo.Reglas = new List<Regla>();
            partpromo.RelaReglas = "({1}) And ({2})";
            Regla reglaCantidad = new Regla();
            Regla reglaAtributo = new Regla();

            reglaCantidad.Atributo = "CANTIDAD";
            reglaCantidad.Id = 2;
            reglaAtributo.Atributo = "ARTICULO.CODIGO";
            reglaAtributo.Valor = "00100101";
            reglaAtributo.ValorString = "00100101";
            reglaAtributo.ValorMuestraRelacion = "";
            reglaAtributo.Id = 0;
            partpromo.Reglas.Add(reglaCantidad);
            partpromo.Reglas.Add(reglaAtributo);

            info1.FaltanteSeguro.Add(new ParticipanteFaltante() { Participante = partpromo, Cantidad = 3 });

            string leyenda = Armador.Armar(info1);

            Assert.AreEqual("3 Articulo Codigo igual a 00100101", leyenda);

        }

        [TestMethod]
        public void ObtenerLeyendaFaltantesArtibutoCodigoConUnaDosCondicionesSinRelacion_2()
        {
            Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante> armadores = new Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante>();
            armadores.Add("", new ArmadorDeLeyendaSegunParticipanteFaltanteGenerico());

            ArmadorDeLeyenda Armador = new ArmadorDeLeyenda(armadores);
            List<InformacionPromocionIncumplida> Lista = new List<InformacionPromocionIncumplida>();

            InformacionPromocionIncumplida incumplida = new InformacionPromocionIncumplida();

            incumplida.Resultados = new List<ResultadoReglas>();
            incumplida.IdPromocion = "4x3";
            incumplida.Promocion = new Promocion() { Tipo = "0" };

            ParticipanteRegla partpromo = new ParticipanteRegla();
            partpromo.Reglas = new List<Regla>();
            partpromo.RelaReglas = "({1} And {2}) And ({3})";
            Regla reglaCantidad = new Regla();
            Regla reglaAtributo = new Regla();

            reglaCantidad.Atributo = "CANTIDAD";
            reglaCantidad.Id = 3;
            reglaAtributo.Atributo = "ARTICULO.CODIGO";
            reglaAtributo.Valor = "00100101";
            reglaAtributo.ValorMuestraRelacion = "";
            reglaAtributo.ValorString = "00100101";
            reglaAtributo.Id = 1;
            Regla reglaAtributoColor = new Regla();
            reglaAtributoColor.Atributo = "COLOR.CODIGO";
            reglaAtributoColor.Valor = "AZUL";
            reglaAtributoColor.ValorMuestraRelacion = "";
            reglaAtributoColor.ValorString = "AZUL";
            reglaAtributoColor.Id = 2;

            partpromo.Reglas.Add(reglaCantidad);
            partpromo.Reglas.Add(reglaAtributo);
            partpromo.Reglas.Add(reglaAtributoColor);

            incumplida.FaltanteSeguro.Add(new ParticipanteFaltante() { Participante = partpromo, Cantidad = 3 });

            string leyenda = Armador.Armar(incumplida);

            Assert.AreEqual("3 Articulo Codigo igual a 00100101 y Color Codigo igual a AZUL", leyenda);

        }

        [TestMethod]
        public void ObtenerLeyendaSegunRelaReglasCondicionOSinRelacion_3()
        {
            Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante> armadores = new Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante>();
            armadores.Add("", new ArmadorDeLeyendaSegunParticipanteFaltanteGenerico());

            ArmadorDeLeyenda Armador = new ArmadorDeLeyenda(armadores);
            List<InformacionPromocionIncumplida> Lista = new List<InformacionPromocionIncumplida>();

            InformacionPromocionIncumplida info1 = new InformacionPromocionIncumplida();

            info1.Resultados = new List<ResultadoReglas>();
            info1.IdPromocion = "4x3";
            info1.Promocion = new Promocion() { Tipo = "0" };

            ParticipanteRegla partpromo = new ParticipanteRegla();
            partpromo.Reglas = new List<Regla>();

            partpromo.RelaReglas = "({1} Or {2}) And ({3})";

            Regla reglaCantidad = new Regla();
            Regla reglaAtributo = new Regla();

            reglaCantidad.Atributo = "CANTIDAD";
            reglaCantidad.Id = 3;

            reglaAtributo.Atributo = "ARTICULO.CODIGO";
            reglaAtributo.Valor = "00100101";
            reglaAtributo.ValorMuestraRelacion = "";
            reglaAtributo.ValorString = "00100101";
            reglaAtributo.Id = 1;
            Regla reglaAtributoGrupo = new Regla();
            reglaAtributoGrupo.ValorMuestraRelacion = "";
            reglaAtributoGrupo.Id = 2;
            reglaAtributoGrupo.Atributo = "ARTICULO.GRUPO.CODIGO";
            reglaAtributoGrupo.Valor = "GRUP1";
            reglaAtributoGrupo.ValorMuestraRelacion = "";
            reglaAtributoGrupo.ValorString = "GRUP1";
            partpromo.Reglas.Add(reglaCantidad);
            partpromo.Reglas.Add(reglaAtributo);
            partpromo.Reglas.Add(reglaAtributoGrupo);

            info1.FaltanteSeguro.Add(new ParticipanteFaltante() { Participante = partpromo, Cantidad = 3 });

            string leyenda = Armador.Armar(info1);

            Assert.AreEqual("3 Articulo Codigo igual a 00100101 o Grupo Codigo igual a GRUP1", leyenda);

        }

        [TestMethod]
        public void ObtenerLeyendaSiCumplioTodasLasReglasPeroNoElMontoBeneficio()
        {
            Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante> armadores = new Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante>();
            armadores.Add("", new ArmadorDeLeyendaSegunParticipanteFaltanteGenerico());

            ArmadorDeLeyenda Armador = new ArmadorDeLeyenda(armadores);
            List<InformacionPromocionIncumplida> Lista = new List<InformacionPromocionIncumplida>();

            InformacionPromocionIncumplida info1 = new InformacionPromocionIncumplida();
            info1.Resultados = new List<ResultadoReglas>();
            info1.IdPromocion = "ListaDePrecios";
            info1.Promocion = new Promocion() { Tipo = "0", ListaDePrecios = "LISTA3" };
            info1.CumplioTodasLasReglasPeroNoElMontoBeneficio = true;            

            string leyenda = Armador.Armar(info1);
            Assert.AreEqual("Al menos uno de los artículos debe tener un menor precio en la lista de precios LISTA3 que en el comprobante.", leyenda);
        }

        [TestMethod]
        public void ArmarLeyendaAMostrarEnControl()
        {
            Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante> armadores = new Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante>();
            armadores.Add("", new ArmadorDeLeyendaSegunParticipanteFaltanteGenerico());

            ArmadorDeLeyenda Armador = new ArmadorDeLeyenda(armadores);
            List<InformacionPromocionIncumplida> Lista = new List<InformacionPromocionIncumplida>();

            InformacionPromocionIncumplida info1 = new InformacionPromocionIncumplida();

            info1.Resultados = new List<ResultadoReglas>();
            info1.IdPromocion = "Promo11";
            info1.Promocion = new Promocion() { Tipo = "2" };

            ParticipanteRegla partpromo = new ParticipanteRegla();
            partpromo.Reglas = new List<Regla>();
            partpromo.RelaReglas = "({1}) And ({2})";
            Regla reglaCantidad = new Regla();
            Regla reglaAtributo = new Regla();

            reglaCantidad.Atributo = "CANTIDAD";
            reglaCantidad.Id = 2;
            reglaAtributo.Atributo = "ARTICULO.CODIGO";
            reglaAtributo.Valor = "00100101";
            reglaAtributo.ValorMuestraRelacion = "Articulo ValorMuestraRelacion";
            reglaAtributo.Id = 0;
            partpromo.Reglas.Add(reglaCantidad);
            partpromo.Reglas.Add(reglaAtributo);

            info1.FaltanteSeguro.Add(new ParticipanteFaltante() { Participante = partpromo, Cantidad = 3 });

            ParticipanteRegla partpromo2 = new ParticipanteRegla();
            partpromo2.Reglas = new List<Regla>();
            partpromo2.RelaReglas = "({3}) And ({4})";
            Regla reglaCantidad2 = new Regla();
            Regla reglaAtributo2 = new Regla();
            reglaCantidad2.Atributo = "CANTIDAD";
            reglaCantidad2.Id = 4;
            reglaAtributo2.Atributo = "ARTICULO.CODIGO";
            reglaAtributo2.Valor = "00100102";
            reglaAtributo2.ValorMuestraRelacion = "Articulo ValorMuestraRelacion2";
            reglaAtributo2.Id = 3;
            partpromo2.Reglas.Add(reglaCantidad2);
            partpromo2.Reglas.Add(reglaAtributo2);

            CombinacionParticipanteFaltantes listaFaltantesPosibles = new CombinacionParticipanteFaltantes();
            ParticipanteFaltante faltantePosible = new ParticipanteFaltante() { Participante = partpromo2, Cantidad = 7 };
            listaFaltantesPosibles.Faltantes.Add(faltantePosible);
            info1.FaltantePosibles.Add(listaFaltantesPosibles );

            string leyenda = Armador.ArmarLeyendaAMostrarEnControl(info1);

            Assert.AreEqual("3 Articulo ValorMuestraRelacion y 7 Articulo ValorMuestraRelacion2...", leyenda);

        }
    }
}
