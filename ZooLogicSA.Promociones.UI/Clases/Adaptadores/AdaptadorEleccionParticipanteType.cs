using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.UI.Clases.Adaptadores
{
    public class AdaptadorEleccionParticipanteType
    {
        private string[] _listaDescripciones;
        private Dictionary<string,EleccionParticipanteType> _diccionario;

        public AdaptadorEleccionParticipanteType()
        {
            this._listaDescripciones = new string[2];
            this._listaDescripciones[0] = "Aplicar al de menor precio";
            this._listaDescripciones[1] = "Aplicar al de mayor precio";
            this._diccionario = new Dictionary<string, EleccionParticipanteType>();
            this._diccionario[this._listaDescripciones[0] ] = EleccionParticipanteType.AplicarAlDeMenorPrecio;
            this._diccionario[this._listaDescripciones[1] ] = EleccionParticipanteType.AplicarAlDeMayorPrecio;
        }

        public string[] listaDescripciones
        {
            get
            {
                return _listaDescripciones;
            }
        }

        public EleccionParticipanteType ObtenerIdEleccionParticipanteTypeSegunValorCombo( string valorCombo )
        {
            EleccionParticipanteType eleccion = EleccionParticipanteType.None;

            try
            {
                eleccion = this._diccionario[valorCombo];
            }
            catch{}

            return eleccion;
        }
    }
}
