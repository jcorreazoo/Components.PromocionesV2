using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public class InterpreteNodoDatosParticipanteXML
    {
        private List<InterpreteNodoDatosParticipanteRegistroXML> _listaRegistros;

        public InterpreteNodoDatosParticipanteXML()
        {
            this._listaRegistros = new List<InterpreteNodoDatosParticipanteRegistroXML>();
        }

        public List<InterpreteNodoDatosParticipanteRegistroXML> ListaRegistros
        {
            get 
            {
                return this._listaRegistros; 
            }
        }

        public void AgregarNodoRegistro( InterpreteNodoDatosParticipanteRegistroXML nodoRegistro )
        {
            this._listaRegistros.Add( nodoRegistro );
        }

    }
}
