using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public class InterpreteNodoAtributoXML
    {
        private string _qnombre;
        private string _atributo;
        private string _descripcion;
        private InterpreteNodoAtributosXML _parent;
        private string _tipoDato;
        private int _longitud;
        private int _decimales;

        public InterpreteNodoAtributoXML( InterpreteNodoAtributosXML parent )
        {
            this._parent = parent;
            this._descripcion = "";
            this._qnombre = "";
        }

        public string qNombre
        {
            get { return _qnombre; }
            set { _qnombre = value; }
        }

        public string Atributo
        {
            get { return _atributo; }
            set { _atributo = value; }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        public InterpreteNodoAtributosXML Parent
        {
            get
            {
                return _parent;
            }
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
