using System.Collections.Generic;
using ZooLogicSA.Promociones.Negocio.Clases;
using ZooLogicSA.Promociones.Negocio.Clases.Beneficios;
using ZooLogicSA.Promociones.UI.Clases.Adaptadores;
using ZooLogicSA.Promociones.UI.Clases.Beneficios;
using ZooLogicSA.Promociones.UI.Controllers;
using System.Linq;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.UI.Clases.Managers
{
    public static class ManagerBeneficios
    {
        public static List<Beneficio> CrearBeneficios( IControllerPromocion control )
        {
            List<Beneficio> listaBeneficios = new List<Beneficio>();
            TipoPromocion tipoPromocion = control.ObtenerTipoPromocionSeleccionada();
            List<string> idParticipante = ObtenerIdParticipante( tipoPromocion, control );

            EleccionParticipanteType eleccion = ObtenerEleccionParticipanteType( tipoPromocion, idParticipante, control );
            ManagerReglas.SetearEleccionParticipante( eleccion );

            if ( DebeCrearVariosBeneficios( idParticipante, tipoPromocion ) )
            {
                List<ParticipanteRegla> listaParticipantes = ObtenerListaParticipantesParaBeneficios( tipoPromocion );
                foreach ( ParticipanteRegla participante in listaParticipantes )
                {
                    ValorBeneficio valorBeneficio = FactoryValorBeneficios.CrearValorBeneficio( tipoPromocion, control, new List<string>() { participante.Id }, participante );
                    Beneficio beneficio = FactoryBeneficio.ObtenerBeneficio( tipoPromocion.BeneficioType, valorBeneficio );
                    listaBeneficios.Add( beneficio );
                }
            }
            else
            {
                ValorBeneficio valorBeneficio = FactoryValorBeneficios.CrearValorBeneficio( tipoPromocion, control, idParticipante );
                Beneficio beneficio = FactoryBeneficio.ObtenerBeneficio( tipoPromocion.BeneficioType, valorBeneficio );
                listaBeneficios.Add( beneficio );
            }

            return listaBeneficios;
        }

        private static List<string> ObtenerIdParticipante( TipoPromocion tipoPromocion, IControllerPromocion control )
        {
            List<string> listaParticipantes = ObtenerListaParticipantesSegunTipoPromocion( tipoPromocion, control );
            string detalle = ManagerReglas.ObtenerCodigoDetalleSegunType( tipoPromocion.codigoDetalleType );

            return ManagerReglas.ObtenerIdParticipanteSegunDetalleYCondicionBeneficiario( listaParticipantes, detalle, control, tipoPromocion );
        }

        private static EleccionParticipanteType ObtenerEleccionParticipanteType( TipoPromocion tipoPromocion, List<string> idParticipante, IControllerPromocion control )
        { 
            EleccionParticipanteType eleccion = EleccionParticipanteType.None;


            string valorComboTipoPrecio = control.ObtenerDescripcionComboTipoPrecio();

                if ( string.IsNullOrEmpty( valorComboTipoPrecio ) )
                    eleccion = tipoPromocion.EleccionParticipanteDefault;
                else
                {
                    AdaptadorEleccionParticipanteType adaptadorEleccionParticipante = new AdaptadorEleccionParticipanteType();
                    eleccion = adaptadorEleccionParticipante.ObtenerIdEleccionParticipanteTypeSegunValorCombo( valorComboTipoPrecio );
                }

            return eleccion;
        }

        private static bool AfectaAMasDeUnParticipante( List<string> idParticipante )
        {
            return idParticipante.Contains("0");
        }

        public static List<string> ObtenerListaParticipantesSegunTipoPromocion( TipoPromocion tipoPromocion, IControllerPromocion control )
        {
            List<string> lista;

            if ( tipoPromocion.TieneParticipantesBeneficiarios )
                lista = control.ObtenerListaReglaParticipantesBeneficio();
            else
                lista = control.ObtenerListaCondicionesSegunTipoPromocion( tipoPromocion );

            return lista;
        }

        private static bool DebeCrearVariosBeneficios( List<string> idParticipante, TipoPromocion tipoPromocion )
        {
            bool debe = true;
            bool forzarUnicoBeneficio = tipoPromocion.TieneQueForzarCrearBeneficioUnico();

            if ( forzarUnicoBeneficio )
            {
                debe = false;
            }
            else
            {
                if ( !AfectaAMasDeUnParticipante( idParticipante ) )
                    debe = false;
                else
                {
                    if ( tipoPromocion.EleccionParticipanteDefault != EleccionParticipanteType.AplicarATodos )
                        debe = false;
                }
            }

            return debe;
        }

        private static List<ParticipanteRegla> ObtenerListaParticipantesParaBeneficios( TipoPromocion tipoPromocion )
        {
            string codigoDetalle = ManagerReglas.ObtenerCodigoDetalleSegunType( tipoPromocion.codigoDetalleType );
            List<ParticipanteRegla> lista = (from x in ManagerReglas.Promo.Participantes select x).ToList();

            lista.RemoveAll( x => x.Codigo != codigoDetalle );

            return lista;
        }

        public static List<string> ObtenerListaParticipantesCondicion(TipoPromocion tipoPromocion, IControllerPromocion control)
        {
            return control.ObtenerListaCondicionesSegunTipoPromocion(tipoPromocion);           
        }
        
    }
}
