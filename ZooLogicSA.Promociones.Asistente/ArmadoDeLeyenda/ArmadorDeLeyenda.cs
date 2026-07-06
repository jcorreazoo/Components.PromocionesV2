using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Asistente.ArmadoDeLeyenda
{
    public class ArmadorDeLeyenda : IArmadorDeLeyenda
    {
		private Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante> armadoresDeLeyendaSegunRegla;

		public ArmadorDeLeyenda( Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante> armadoresDeLeyendaSegunRegla )
		{
			this.armadoresDeLeyendaSegunRegla = armadoresDeLeyendaSegunRegla;
		}

        public string ArmarLeyendaAMostrarEnControl(InformacionPromocionIncumplida info)
        {
            string retorno = "";
            string leyendaFaltantesPosibles = "";

            if (info.Promocion != null && !(String.IsNullOrEmpty(info.Promocion.ListaDePrecios)) && info.CumplioTodasLasReglasPeroNoElMontoBeneficio)
            {
                retorno = "Al menos uno de los artículos debe tener un menor precio en la lista de precios " + info.Promocion.ListaDePrecios + " que en el comprobante.";
            }
            else
            {

                List<string> seguros = this.obtenerLeyendaFaltanteSeguro(info);

                if (info.FaltantePosibles.Count > 0)
                {
                    List<string> seguroEnOpcional = this.ObtenerLeyendaSeguroEnOpcional(info.FaltantePosibles[0], info);

                    leyendaFaltantesPosibles = String.Join(" y ", seguroEnOpcional.ToArray()) + "...";
                }

                retorno = String.Join((seguros.Count > 1 ? " y " : ""), seguros.ToArray()) + (seguros.Count > 0 && !String.IsNullOrEmpty(leyendaFaltantesPosibles) ? " y " : "") + leyendaFaltantesPosibles;
            }
            return retorno;
        }

        public string Armar(InformacionPromocionIncumplida info)
        {
            List<string> retorno = new List<string>();

            if (info.Promocion != null &&  !(String.IsNullOrEmpty(info.Promocion.ListaDePrecios)) && info.CumplioTodasLasReglasPeroNoElMontoBeneficio )
            {
                retorno.Add("Al menos uno de los artículos debe tener un menor precio en la lista de precios " + info.Promocion.ListaDePrecios + " que en el comprobante.");
            }
            else
            {
                List<string> seguros = this.obtenerLeyendaFaltanteSeguro(info);

                retorno.Add( String.Join( " y ", seguros.ToArray() ) );

                List<string> opcionales = new List<string>();

                foreach ( CombinacionParticipanteFaltantes combinacion in info.FaltantePosibles )
                {
                    List<string> seguroEnOpcional = this.ObtenerLeyendaSeguroEnOpcional(combinacion, info);

                    if ( seguroEnOpcional.Count == 1 )
                    {
                        opcionales.Add( seguroEnOpcional[0] );
                    }
                    else
                    {
                        opcionales.Add( " ( " + String.Join( " y ", seguroEnOpcional.ToArray() ) + " )" );
                    }
                }

                string todasLasOpciones = String.Join( " o ", opcionales.ToArray() );

                if ( opcionales.Count > 1 )
                {
                    todasLasOpciones = " ( " + todasLasOpciones + " ) ";
                }
                retorno.Add( todasLasOpciones );

                retorno.RemoveAll( x => String.IsNullOrEmpty( x ) );
            }
            return String.Join( " y ", retorno.ToArray() );
        }

        private List<string> ObtenerLeyendaSeguroEnOpcional(CombinacionParticipanteFaltantes combinacion, InformacionPromocionIncumplida info)
        {
            List<string> seguroEnOpcional = new List<string>();
            foreach (ParticipanteFaltante faltante in combinacion.Faltantes)
            {
				IArmadorDeLeyendaSegunParticipanteFaltante armador = this.ObtenerArmadorDeLeyendaSegunParticipanteFaltante( faltante.Participante.Codigo );
				seguroEnOpcional.Add( armador.ObtenerLeyendaSegunRegla( faltante, info ) );
            }
            return seguroEnOpcional;
        }

        private List<string> obtenerLeyendaFaltanteSeguro(InformacionPromocionIncumplida info)
        {
            List<string> seguros = new List<string>();

            foreach (ParticipanteFaltante resultadoReglas in info.FaltanteSeguro)
            {
				IArmadorDeLeyendaSegunParticipanteFaltante armador = this.ObtenerArmadorDeLeyendaSegunParticipanteFaltante( resultadoReglas.Participante.Codigo );
				seguros.Add( armador.ObtenerLeyendaSegunRegla( resultadoReglas, info ) );
            }
            return seguros;
        }

		private IArmadorDeLeyendaSegunParticipanteFaltante ObtenerArmadorDeLeyendaSegunParticipanteFaltante( string codigo )
		{
			IArmadorDeLeyendaSegunParticipanteFaltante valor;

			codigo = ( String.IsNullOrEmpty(codigo) ) ? "" : codigo;

			if ( !this.armadoresDeLeyendaSegunRegla.TryGetValue( codigo, out valor ) )
			{
				valor = this.armadoresDeLeyendaSegunRegla[""];
			}

			return valor;
		}

        

   
    }
}
