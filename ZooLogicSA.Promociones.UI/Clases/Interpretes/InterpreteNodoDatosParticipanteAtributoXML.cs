using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public class InterpreteNodoDatosParticipanteAtributoXML
    {
        private string _nombre;
        private string _valor;

        public InterpreteNodoDatosParticipanteAtributoXML( string valor )
        {
            this._valor = valor;
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public string Valor
        {
            get { return _valor; }
            set { _valor = value; }
        }

    }
}
