using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public class InterpreteNodoParticipanteXML
    {
        private List<InterpreteNodoAtributosXML> _nodosAtributos;
        private InterpreteNodoAtributosXML _parent;
        private string _qentidad;
        private string _descripcion;
        private string _detalle;
        private string _atributo;
        private string _tipoDato;
        private int _longitud;
        private int _decimales;

        public InterpreteNodoParticipanteXML( InterpreteNodoAtributosXML parent )
        {
            this._detalle = "";
            this._qentidad = "";
            this._descripcion = "";
            this._parent = parent;
            this._nodosAtributos = new List<InterpreteNodoAtributosXML>();
        }

        public string qEntidad
        {
            get { return _qentidad; }
            set { _qentidad = value; }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        public string Detalle
        {
            get { return _detalle; }
            set { _detalle = value; }
        }

        public string Atributo
        {
            get { return _atributo; }
            set { _atributo = value; }
        }

        public InterpreteNodoAtributosXML Parent
        {
            get
            {
                return _parent;
            }
        }

        public List<InterpreteNodoAtributosXML> NodosAtributos
        {
            get
            {
                return _nodosAtributos;
            }
        }

        public void AgregarNodoAtributos( InterpreteNodoAtributosXML nodo )
        {
            this._nodosAtributos.Add( nodo );
        }

        public int Decimales
        {
            get { return this._decimales; }
            set { this._decimales = value; }
        }

        public int Longitud
        {
            get { return this._longitud; }
            set { this._longitud = value; }
        }

        public string TipoDato
        {
            get { return this._tipoDato; }
            set { this._tipoDato = value; }
        }


    }
}
