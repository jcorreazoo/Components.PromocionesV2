using System;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.EvaluacionReglas
{
    public class ParticipanteDummy : IParticipante
    {
        private string idDummy;
        private string claveDummy;
        private decimal cantidadDummy;
        private decimal precioDummy;
        private decimal consumoPorCombinacion;



        public ParticipanteDummy( ParticipanteRegla participante, decimal cantidadDummy )
        {
            this.claveDummy = participante.Codigo;
            this.idDummy = "fake_" + Guid.NewGuid();
            this.cantidadDummy = cantidadDummy;
            this.precioDummy = 1;
        }

        #region IParticipante Members

        public string Id
        {
            get { return this.idDummy; }
        }

        public string Clave
        {
            get { return this.claveDummy; }
            set { this.claveDummy = value; }
        }

        public decimal Cantidad
        {
            get { return this.cantidadDummy; }
            set { this.cantidadDummy = value; }
        }

        public decimal ConsumoPorCombinacion
        {
            get { return this.consumoPorCombinacion; }
            set { this.consumoPorCombinacion = value; }
        }

        public object Nodo
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public decimal Consumido
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Promocion
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

		public string Beneficio
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public string[] CoincidenciasExcluidas
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public bool CompararSegunContenido( IParticipante participanteNuevo )
        {
            return true;
        }

        public void AgregarAlMismoNivel( IParticipante participanteNuevo )
        {
        }

        public void AplicarValorAAtributo( string atributo, FormatoPromociones.Alteracion alteracion, object valor )
        {
            throw new NotImplementedException();
        }

        public IParticipante Clonar()
        {
            return this;
        }

        public IAtributo ObtenerAtributo( string rutaAtributo )
        {
            throw new NotImplementedException();
        }
        public IAtributo ObtenerCantidadSinRestarConsumido(string rutaAtributo)
        {
            throw new NotImplementedException();
        }

        public decimal ObtenerPrecioUnitario()
        {
            return this.precioDummy;
        }

        public void Destruir()
        {
            throw new NotImplementedException();
        }

        public void ModificarCantidadSegunMonto()
        { 
        }

        #endregion
    }
}
