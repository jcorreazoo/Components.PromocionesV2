
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ZooLogicSA.Promociones.FormatoPromociones;

    namespace ZooLogicSA.Promociones.UI.Clases.Adaptadores
    {
        public class AdaptadorEleccionVisualizacionPromocionAsistenteType
        {
            private string[] _listaDescripciones;
            private Dictionary<string, VisualizacionPromocionAsistenteType> _diccionario;

            public AdaptadorEleccionVisualizacionPromocionAsistenteType()
            {
                this._listaDescripciones = new string[3];
                this._listaDescripciones[0] = "Normal";
                this._listaDescripciones[1] = "Destacada";
                this._listaDescripciones[2] = "No visible";

                this._diccionario = new Dictionary<string, VisualizacionPromocionAsistenteType>();
                this._diccionario[this._listaDescripciones[0]] = VisualizacionPromocionAsistenteType.Normal;
                this._diccionario[this._listaDescripciones[1]] = VisualizacionPromocionAsistenteType.Destacada;
                this._diccionario[this._listaDescripciones[2]] = VisualizacionPromocionAsistenteType.NoVisible;
            }

            public string[] listaDescripciones
            {
                get
                {
                    return _listaDescripciones;
                }
            }

            public VisualizacionPromocionAsistenteType ObtenerIdVisualizacionPromocionTypeSegunValorCombo( string valorCombo )
            {
                VisualizacionPromocionAsistenteType eleccion = VisualizacionPromocionAsistenteType.Normal;

                try
                {
                    eleccion = this._diccionario[valorCombo];
                }
                catch { }

                return eleccion;
            }
        }
    }


