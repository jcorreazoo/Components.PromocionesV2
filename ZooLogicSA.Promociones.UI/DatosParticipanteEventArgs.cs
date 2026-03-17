using System;
using ZooLogicSA.Promociones.Negocio.Clases;
using System.Collections.Generic;

namespace ZooLogicSA.Promociones.UI
{
    public class DatosParticipanteEventArgs : EventArgs
    {
        private string entidad;
        private List<string> atributos;
        private string datos;
        private string whereAdicional;

        public DatosParticipanteEventArgs( string nombreEntidad )
        {
            this.entidad = nombreEntidad;
            this.atributos = new List<string>();
        }

        
        public void AgregarAtributo( string nombreAtributo )
        {
            this.atributos.Add( nombreAtributo );
        }

        public string Entidad
        {
            get
            {
                return entidad;
            }
        }

        public List<string> Atributos
        {
            get
            {
                return atributos;
            }
        }

        public string Datos
        {
            get
            {
                return datos;
            }
            set
            {
                datos = value;
            }
        }

        public string WhereAdicional
        {
            get
            {
                return whereAdicional;
            }
            set
            {
                whereAdicional = value;
            }
        }

    }
}