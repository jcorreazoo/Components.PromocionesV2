using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public class InterpreteNodoDatosParticipanteRegistroXML
    {
        private List<InterpreteNodoDatosParticipanteAtributoXML> _listaAtributos;

        public InterpreteNodoDatosParticipanteRegistroXML()
        {
            this._listaAtributos = new List<InterpreteNodoDatosParticipanteAtributoXML>();
        }

        public List<InterpreteNodoDatosParticipanteAtributoXML> ListaAtributos
        {
            get 
            {
                return this._listaAtributos; 
            }
        }

        public void AgregarNodoAtributos( InterpreteNodoDatosParticipanteAtributoXML nodoAtributos )
        {
            this._listaAtributos.Add( nodoAtributos );
        }
        

    }
}
