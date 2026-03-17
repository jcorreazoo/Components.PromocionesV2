using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ZooLogicSA.Promociones.FormatoPromociones
{
    public class Regla
    {
        private int id;
        private object valor;
        private string _valorString;
        private Factor comparacion;
        private string _operador;
        private string atributo;
        private string _descripcionAtributo;
        private string funcion;
        private bool _compuesta = false;
        private Regla _reglaAsociada;
        private string _operadorCompuesto;
        private bool _operadorAlComienzo;
        private string _valorMuestraRelacion;

        public string ValorMuestraRelacion
        {
            get { return _valorMuestraRelacion; }
            set { _valorMuestraRelacion = value; }
        }

        public bool OperadorAlComienzo
        {
            get { return _operadorAlComienzo; }
            set { _operadorAlComienzo = value; }
        }

        public string Funcion
        {
            get { return this.funcion; }
            set { this.funcion = value; }
        }

        public string Operador
        {
            get { return _operador; }
            set { _operador = value; }
        }

        public string OperadorCompuesto
        {
            get { return _operadorCompuesto; }
            set { _operadorCompuesto = value; }
        }

        public string DescripcionAtributo
        {
            get { return _descripcionAtributo; }
            set { _descripcionAtributo = value; }
        }

        public string ValorString
        {
            get { return _valorString; }
            set { _valorString = value; }
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        
        public Factor Comparacion
        {
            get { return this.comparacion; }
            set { this.comparacion = value; }
        }

        public object Valor
        {
            get { return this.valor; }
            set { this.valor = value; }
        }

        public string Atributo
        {
            get { return this.atributo; }
            set { this.atributo = value; }
        }

        public bool Compuesta
        {
            get { return _compuesta; }
            set { _compuesta = value; }
        }

        public Regla ReglaAsociada
        {
            get { return _reglaAsociada; }
            set { _reglaAsociada = value; }
        }

        public string ObtenerTexto()
        {
            return this.Atributo + " " + Factor.DebeSerIgualA + " " + this.Valor.ToString();
        }

        public string ObtenerSentencia()
        {
            string sentencia;
            if ( this.OperadorAlComienzo )
            {
                sentencia = this._operador + this._descripcionAtributo + ", " + this._valorString + ")";
            }
            else
            {
                sentencia = this._descripcionAtributo + this._operador + this._valorString;
            }
            return sentencia;
        }

        public string ObtenerSentenciaCompuesta()
        {
            // La regla compuesta estoy suponiendo que es Between, cuando haya otra, hay que refactorizar
            return this._descripcionAtributo + " Between(" + this._valorString + ", " + this.ReglaAsociada.ValorString + ")";
        }
    }
}
