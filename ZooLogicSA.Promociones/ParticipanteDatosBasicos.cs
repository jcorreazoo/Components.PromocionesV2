using System;

namespace ZooLogicSA.Promociones
{
    public class ParticipanteDatosBasicos
    {
        private string id;
        private string clave;
        private decimal cantidad;
        private decimal consumoPorCombinacion;
        private int usado;

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string Clave
        {
            get { return this.clave; }
            set { this.clave = value; }
        }

        public decimal Cantidad
        {
            get { return this.cantidad; }
            set { this.cantidad = value; }
        }

        public int Usado
        {
            get { return this.usado; }
            set { this.usado = value; }
        }

        public decimal ConsumoPorCombinacion
        {
            get { return this.consumoPorCombinacion; }
            set { this.consumoPorCombinacion = value; }
        }
    }
}
