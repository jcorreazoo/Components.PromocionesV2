using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Redondeos;

namespace ZooLogicSA.Promociones.FormatoPromociones
{
    public class Promocion
    {
        private string id;
        private List<ParticipanteRegla> participantes;
        private List<Beneficio> beneficios;
        private bool recursiva;
        private string informacionControl;
        private string descripcion;
        private DateTime fechaModificacion;
        private string horaModificacion;
        public string LeyendaAsistente{ get; set; }
        public VisualizacionPromocionAsistenteType Visualizacion { get; set; }

        private string redondeo;
        private EntidadRedondeo objetoRedondeo;

        private Decimal topeBeneficio;
        private String listaDePrecios;
        private UInt16 cuotasSinRecargo;
        private bool aplicaAutomaticamente;
        private string tipo;
        private bool conMedioDePago;

        public EntidadRedondeo ObjetoRedondeo
        {
            get { return objetoRedondeo; }
            set { objetoRedondeo = value; }
        }

        public DateTime FechaModificacion
        {
            get { return this.fechaModificacion;  }
            set { this.fechaModificacion = value; }
        }

        public string HoraModificacion
        {
            get { return this.horaModificacion; }
            set { this.horaModificacion = value; }
        }
        public string Redondeo
        {
            get { return redondeo; }
            set { redondeo = value; }
        }
        public string Descripcion
        {
            get { return this.descripcion; }
            set { this.descripcion = value; }
        }

        public Decimal TopeBeneficio
        {
            get { return this.topeBeneficio; }
            set { this.topeBeneficio = value; }
        }

        public String ListaDePrecios
        {
            get { return this.listaDePrecios; }
            set { this.listaDePrecios = value; }
        }


        private EleccionParticipanteType eleccionParticipante;

        public EleccionParticipanteType EleccionParticipante
        {
            get { return this.eleccionParticipante; }
            set { this.eleccionParticipante = value; }
        }

        public Promocion()
        {
            this.participantes = new List<ParticipanteRegla>();
            this.beneficios = new List<Beneficio>();
        }

        public List<Beneficio> Beneficios
        {
            get { return this.beneficios; }
            set { this.beneficios = value; }
        }

        public List<ParticipanteRegla> Participantes
        {
            get { return this.participantes; }
            set { this.participantes = value; }
        }

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public bool Recursiva
        {
            get { return this.recursiva; }
            set { this.recursiva = value; }
        }

        public string InformacionControl
        {
            get { return this.informacionControl; }
            set { this.informacionControl = value; }
        }
        public UInt16 CuotasSinRecargo
        {
            get { return this.cuotasSinRecargo; }
            set { this.cuotasSinRecargo = value; }
        }

        public bool AplicaAutomaticamente
        {
            get { return this.aplicaAutomaticamente; }
            set { this.aplicaAutomaticamente = value; }
        }

        public string Tipo
        {
            get { return this.tipo; }
            set { this.tipo = value; }
        }

        public bool ConMedioDePago
        {
            get { return this.conMedioDePago; }
            set { this.conMedioDePago = value; }
        }

        public Promocion Clonar()
        {
            Promocion retorno = new Promocion();

            retorno.AplicaAutomaticamente = this.AplicaAutomaticamente;
            retorno.Beneficios.AddRange( this.Beneficios );
            retorno.ConMedioDePago = this.ConMedioDePago;
            retorno.CuotasSinRecargo = this.CuotasSinRecargo;
            retorno.Descripcion = this.Descripcion;
            retorno.EleccionParticipante = this.EleccionParticipante;
            retorno.FechaModificacion = this.FechaModificacion;
            retorno.HoraModificacion = this.HoraModificacion;
            retorno.Id = this.Id;
            retorno.InformacionControl = this.InformacionControl;
            retorno.LeyendaAsistente = this.LeyendaAsistente;
            retorno.ListaDePrecios = this.ListaDePrecios;
            retorno.ObjetoRedondeo = this.ObjetoRedondeo;
            retorno.Participantes.AddRange( this.Participantes );
            retorno.Recursiva = this.Recursiva;
            retorno.Redondeo = this.Redondeo;
            retorno.Tipo = this.Tipo;
            retorno.TopeBeneficio = this.TopeBeneficio;
            retorno.Visualizacion = this.Visualizacion;

            return retorno;
        }

        public bool UtilizaCosumoPorCombinacion()
        {
            bool retorno = false;
            if (Tipo != null) 
            {
                retorno = (new[] { "3", "6" }.Any(x => Tipo.Contains(x)));
            }
            return retorno;
        }
    }
}