using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public class InterpreteNodoAtributosXML
    {
        private List<InterpreteNodoParticipanteXML> _nodosParticipante;
        private List<InterpreteNodoAtributoXML> _nodosAtributo;
        private InterpreteNodoParticipanteXML _parent;

        public InterpreteNodoAtributosXML( InterpreteNodoParticipanteXML parent )
        {
            this._nodosAtributo = new List<InterpreteNodoAtributoXML>();
            this._nodosParticipante = new List<InterpreteNodoParticipanteXML>();
            this._parent = parent;
        }

        public InterpreteNodoParticipanteXML Parent
        {
            get
            {
                return _parent;
            }
        }

        public List<InterpreteNodoAtributoXML> NodosAtributo
        {
            get
            {
                return _nodosAtributo;
            }
        }

        public List<InterpreteNodoParticipanteXML> NodosParticipante
        {
            get
            {
                return _nodosParticipante;
            }
        }

        public void AgregarNodoParticipante( InterpreteNodoParticipanteXML nodo )
        {
            this._nodosParticipante.Add( nodo );
        }

        public void AgregarNodoAtributo( InterpreteNodoAtributoXML nodo )
        {
            this._nodosAtributo.Add( nodo );
        }
    }
}
