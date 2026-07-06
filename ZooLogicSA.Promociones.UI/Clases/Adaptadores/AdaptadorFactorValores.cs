using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.UI.Clases.Adaptadores
{
    class AdaptadorFactorValores
    {
        private string valor;
        private Factor valorFactor;
        private bool _single;
        private Factor _secondValorFactor;
        private bool _operadorAlComienzo;

        public AdaptadorFactorValores( string Valor, Factor ValorFactor, bool Single )
        {
            this.valor = Valor;
            this.valorFactor = ValorFactor;
            this._single = Single;
            this._operadorAlComienzo = false;
        }

        public bool OperadorAlComienzo
        {
            get { return _operadorAlComienzo; }
            set { _operadorAlComienzo = value; }
        }

        public string Valor
        {
            get
            {
                return this.valor;
            }
        }

        public Factor ValorFactor
        {
            get
            {
                return this.valorFactor;
            }
        }

        public bool Single
        {
            get
            {
                return this._single;
            }
        }

        public Factor SecondValorFactor
        {
            get { return _secondValorFactor; }
            set { _secondValorFactor = value; }
        }

    }
}
