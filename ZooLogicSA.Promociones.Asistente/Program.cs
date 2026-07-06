using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;

namespace ZooLogicSA.Promociones.Asistente
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
          
            ParametrosAsistente param = new ParametrosAsistente();
            param.InformacionAplicacion = new Core.Escalares.InformacionAplicacion();
            param.InformacionAplicacion.NombreProyecto = "Colroytalel";
            param.InformacionAplicacion.RutaDeImagenes = "Ruta";
            FrmAsistente frm = new FrmAsistente( param );



            #region Generacion Promociones Cumplidas
            List<InformacionPromocion> infocabecera = new List<InformacionPromocion>();
            InformacionPromocion info = new InformacionPromocion( "PROMO1" );
            info.Promocion = new Promocion() { Descripcion = "Desc promo 1" , LeyendaAsistente= "Leyanda de prueba para el asistente"};
            info.MontoBeneficio = 400;
            infocabecera.Add( info );

            info = new InformacionPromocion("PROMO2        ");
            info.Promocion = new Promocion() { Descripcion = "Desc promo 2 CON UNDSNSMMSMSDMSD  ,D,DMDMDMDLLDKDM M MDMDMDMDM  DMDMDMDMDM",
                Visualizacion = VisualizacionPromocionAsistenteType.Destacada };
                
            info.MontoBeneficio = 500;
            infocabecera.Add( info );

            info = new InformacionPromocion("PROMO3             ");
            info.Promocion = new Promocion() { Descripcion = "Desc promo 3" };
            info.MontoBeneficio = 600;
            infocabecera.Add( info );

            #endregion
            frm.kontroler.AgregarPromociones( infocabecera, frm.source );

            #region Promociones Incumplidas
            List<InformacionPromocionIncumplida> Incumplidas = new List<InformacionPromocionIncumplida>();
            InformacionPromocionIncumplida Incumplida = new InformacionPromocionIncumplida();
            Incumplida.IdPromocion = "PARCIAL";
            Incumplida.Promocion = new Promocion() { Descripcion = "PARCIAL" };

            #region Version vieja
            Incumplida.Resultados = new List<ResultadoReglas>();

            ResultadoReglas Resregla = new ResultadoReglas();
            Resregla.Requerido = 4;
            Resregla.Satisfecho = 1;
            Resregla.Cumple = false;

            #region Promo
            ParticipanteRegla partpromo = new ParticipanteRegla();
            partpromo.Codigo = "COMPROBANTE.FACTURADETALLE.ITEM";
            partpromo.Reglas = new List<Regla>();
            partpromo.RelaReglas = "({1} And {2}) And ({3})";
            Resregla.PartPromo = partpromo;

            Regla reglaCantidad = new Regla();
            reglaCantidad.Atributo = "CANTIDAD";
            reglaCantidad.Id = 3;
            partpromo.Reglas.Add( reglaCantidad );

            Regla reglaAtributo = new Regla();
            reglaAtributo.Atributo = "ARTICULO.CODIGO";
            reglaAtributo.Valor = "00100101";
            reglaAtributo.ValorMuestraRelacion = "Codigo 00100101";
            reglaAtributo.Id = 1;
            partpromo.Reglas.Add( reglaAtributo );

            Regla reglaAtributoColor = new Regla();
            reglaAtributoColor.ValorMuestraRelacion = "Color Azul";
            reglaAtributoColor.Id = 2;
            partpromo.Reglas.Add( reglaAtributoColor );
            #endregion

            Incumplida.Resultados.Add( Resregla );
            Incumplida.Resultados.Add( Resregla );
            Incumplida.Resultados.Add( Resregla );
            Incumplida.Resultados.Add( Resregla );
            Incumplida.Resultados.Add( Resregla );
            Incumplida.Resultados.Add( Resregla );
            Incumplida.Resultados.Add( Resregla );
            Incumplida.Resultados.Add( Resregla );
            Incumplida.Resultados.Add( Resregla );
            Incumplida.Resultados.Add( Resregla );
            Incumplida.Resultados.Add( Resregla );
            Incumplida.Resultados.Add( Resregla );
            Incumplida.Resultados.Add( Resregla );
            Incumplida.Resultados.Add( Resregla );
            Incumplida.Resultados.Add( Resregla );

            Resregla = new ResultadoReglas();
            #region Promo
            ParticipanteRegla partpromo2 = new ParticipanteRegla();
            partpromo2.Codigo = "COMPROBANTE.FACTURADETALLE.ITEM";
            partpromo2.Reglas = new List<Regla>();
            partpromo2.RelaReglas = "({1} And {2}) And ({3})";
            Resregla.PartPromo = partpromo;

            reglaCantidad = new Regla();
            reglaCantidad.Atributo = "CANTIDAD";
            reglaCantidad.Id = 3;
            partpromo2.Reglas.Add( reglaCantidad );

            reglaAtributo = new Regla();
            reglaAtributo.Atributo = "ARTICULO.CODIGO";
            reglaAtributo.Valor = "00100102";
            reglaAtributo.ValorMuestraRelacion = "Codigo 00100102";
            reglaAtributo.Id = 1;
            partpromo2.Reglas.Add( reglaAtributo );

            reglaAtributoColor = new Regla();
            reglaAtributoColor.ValorMuestraRelacion = "Color Azul";
            reglaAtributoColor.Id = 2;
            partpromo2.Reglas.Add( reglaAtributoColor );
            #endregion

            #region Promo
            ParticipanteRegla partpromo3 = new ParticipanteRegla();
            partpromo3.Codigo = "COMPROBANTE.FACTURADETALLE.ITEM";
            partpromo3.Reglas = new List<Regla>();
            partpromo3.RelaReglas = "({1} And {2}) And ({3})";
            Resregla.PartPromo = partpromo;

            reglaCantidad = new Regla();
            reglaCantidad.Atributo = "CANTIDAD";
            reglaCantidad.Id = 3;
            partpromo3.Reglas.Add( reglaCantidad );

            reglaAtributo = new Regla();
            reglaAtributo.Atributo = "ARTICULO.CODIGO";
            reglaAtributo.Valor = "00100102";
            reglaAtributo.ValorMuestraRelacion = "Codigo 00100103";
            reglaAtributo.Id = 1;
            partpromo3.Reglas.Add( reglaAtributo );

            reglaAtributoColor = new Regla();
            reglaAtributoColor.ValorMuestraRelacion = "Color Azul";
            reglaAtributoColor.Id = 2;
            partpromo3.Reglas.Add( reglaAtributoColor );
            #endregion
            #endregion 
            

            Incumplida.Cumplidos = new List<ParticipanteFaltante>();
            Incumplida.FaltanteSeguro = new List<ParticipanteFaltante>();
            Incumplida.FaltanteSeguro.Add( new ParticipanteFaltante() { Participante = partpromo, Cantidad = 1 } );

            Incumplida.FaltantePosibles = new List<CombinacionParticipanteFaltantes>();

            CombinacionParticipanteFaltantes comb ;
            comb = new CombinacionParticipanteFaltantes();
            comb.Faltantes.Add( new ParticipanteFaltante() { Participante = partpromo2, Cantidad = 1 } );
            Incumplida.FaltantePosibles.Add( comb );

            comb = new CombinacionParticipanteFaltantes();
            comb.Faltantes.Add( new ParticipanteFaltante() { Participante = partpromo3, Cantidad = 1 } );
            Incumplida.FaltantePosibles.Add( comb );

            comb = new CombinacionParticipanteFaltantes();
            comb.Faltantes.Add( new ParticipanteFaltante() { Participante = partpromo, Cantidad = 1 } );
            Incumplida.FaltantePosibles.Add( comb );


            Incumplida.TotalFaltante = 1;

            #endregion

            Incumplidas.Add( Incumplida );

            frm.kontroler.AgregarPromociones(Incumplidas, frm.source);
            frm.ArmarGrilla();
            frm.ShowDialog();
        }
    }
}
