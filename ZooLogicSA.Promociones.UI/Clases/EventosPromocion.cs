using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.Negocio.Clases;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public class EventosPromocion
    {
        public event EventHandler<DatosParticipanteEventArgs> EventoSeleccionParticipante;
        public event EventHandler<DatosParticipanteEventArgs> EventoLlenaComboDeListaDePrecios;
        public event EventHandler<CustomEventArgs> EventoPerdioFoco;
        public event EventHandler<CustomEventArgs> EventoMostrarAyuda;
        public event EventHandler<CustomEventArgs> EventoControlFin;

        public EventosPromocion()
        { 
        }

        public void MetodoEventoSeleccionParticipante( DatosParticipanteEventArgs datosParticipante )
        {
            DatosParticipanteEventArgs argumentos = datosParticipante;
            OnRaiseEventoSeleccionarParticipante( argumentos );
        }

        public void MetodoEventoLlenaComboDeListaDePrecios(DatosParticipanteEventArgs datosParticipante)
        {
            DatosParticipanteEventArgs argumentos = datosParticipante;
            OnRaiseEventoLlenaComboDeListaDePrecios(argumentos);
        }

        public void MetodoEventoPerdioFoco( string texto )
        {
            CustomEventArgs argumentos = new CustomEventArgs( texto );
            OnRaiseEventoPerdioFoco( argumentos );
        }

        public void MetodoEventoMostrarAyuda( string texto )
        {
            CustomEventArgs argumentos = new CustomEventArgs( texto );
            OnRaiseEventoMostrarAyuda( argumentos );
        }

        public void MetodoEventoControlFin( string texto )
        {
            CustomEventArgs argumentos = new CustomEventArgs( texto );
            OnRaiseEventoControlFin( argumentos );
        }

        protected void OnRaiseEventoSeleccionarParticipante( DatosParticipanteEventArgs e )
        {
            EventHandler<DatosParticipanteEventArgs> handler = EventoSeleccionParticipante;

            if ( handler != null )
            {
                handler( this, e );
            }
        }

        protected void OnRaiseEventoLlenaComboDeListaDePrecios(DatosParticipanteEventArgs e)
        {
            EventHandler<DatosParticipanteEventArgs> handler = EventoLlenaComboDeListaDePrecios;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnRaiseEventoPerdioFoco( CustomEventArgs e )
        {
            EventHandler<CustomEventArgs> handler = EventoPerdioFoco;

            if ( handler != null )
            {
                handler( this, e );
            }
        }

        protected void OnRaiseEventoMostrarAyuda( CustomEventArgs e )
        {
            EventHandler<CustomEventArgs> handler = EventoMostrarAyuda;

            if ( handler != null )
            {
                handler( this, e );
            }
        }

        protected void OnRaiseEventoControlFin( CustomEventArgs e )
        {
            EventHandler<CustomEventArgs> handler = EventoControlFin;

            if ( handler != null )
            {
                handler( this, e );
            }
        }
    }
}
