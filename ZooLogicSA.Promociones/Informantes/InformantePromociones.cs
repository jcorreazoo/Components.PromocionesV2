using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using ZooLogicSA.Promociones.Informantes;
using System.IO;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.ComprobanteXml;
using ZooLogicSA.Promociones.Utils;
using System.Globalization;

namespace ZooLogicSA.Promociones.Informantes
{
    public class InformantePromociones : IInformantePromociones
    {
        private List<InformacionPromocion> informaciones;
        private ConvertidorNumerico convertNumber;

        private ConfiguracionComportamiento comportamiento;

        public ConfiguracionComportamiento Comportamiento
        {
            get { return this.comportamiento; }
            set { this.comportamiento = value; }
        }

        public List<InformacionPromocion> Informaciones
        {
            get { return this.informaciones; }
            set { this.informaciones = value; }
        }

        public InformantePromociones( ConfiguracionComportamiento comportamiento )
        {
            this.comportamiento = comportamiento;
            this.convertNumber = new ConvertidorNumerico();
        }

        public void InformarItemAfectado( InformacionPromocion info, string idPromo, CoincidenciaEvaluacion coincidencia, IParticipante participante )
        {
            //InformacionPromocion info = this.informaciones.Find( x => x.IdPromocion == idPromo );

            ParticipanteAfectado participanteExistente = info.DetalleAfectado.Find( x => x.Clave == participante.Clave && x.Id == participante.Id );
            if ( participanteExistente != null )
            {
                participanteExistente.Cantidad += this.convertNumber.ConvertToFloat( participante.Cantidad.ToString() );
            }
            else
            {
                ParticipanteAfectado item = new ParticipanteAfectado();
                item.Clave = participante.Clave;
                item.Id = participante.Id;
                item.Cantidad = this.convertNumber.ConvertToFloat( participante.Cantidad.ToString() );
                item.Atributos = coincidencia.Atributos;
				item.IdParticipanteRegla = coincidencia.IdParticipanteEnRegla;
                info.DetalleAfectado.Add( item );
            }
        }

        public void InformarItemReAfectado( InformacionPromocion info, IParticipante participante, Decimal cantidadAplicaciones )
        {
            ParticipanteAfectado partExistente = info.DetalleAfectado.Find( x => x.Clave == participante.Clave && x.Id == participante.Id );
            if ( partExistente != null )
            {
                partExistente.Cantidad = partExistente.Cantidad + this.convertNumber.ConvertToFloat( cantidadAplicaciones.ToString() );
            }
            else
            {
                ParticipanteBeneficiado partBeneficiado = info.DetalleBeneficiado.Find( x => x.Clave == participante.Clave && x.Id == participante.Id );
                if ( partBeneficiado != null )
                {
                    partBeneficiado.Cantidad = partBeneficiado.Cantidad + this.convertNumber.ConvertToFloat( cantidadAplicaciones.ToString() );
                }
            }
        }

        public void InformarItemNuevoAfectado(InformacionPromocion info, IParticipante participante, Decimal cantidadAplicaciones)
        {
            ParticipanteAfectado partExistente = info.DetalleAfectado.Find(x => x.Clave == participante.Clave && x.Id == participante.Id);
            if (partExistente != null)
            {
                partExistente.Cantidad = this.convertNumber.ConvertToFloat(cantidadAplicaciones.ToString());
            }
            else
            {
                ParticipanteBeneficiado partBeneficiado = info.DetalleBeneficiado.Find(x => x.Clave == participante.Clave && x.Id == participante.Id);
                if (partBeneficiado != null)
                {
                    partBeneficiado.Cantidad = this.convertNumber.ConvertToFloat(cantidadAplicaciones.ToString());
                }
            }
        }

        public void InformarItemBeneficiado( InformacionPromocion info, string idPromo, IParticipante participante, Beneficio beneficio )
        {
            //InformacionPromocion info = this.informaciones.Find( x => x.IdPromocion == idPromo );
            ParticipanteAfectado itemAfectado = info.DetalleAfectado.Find( x => x.Clave == participante.Clave && x.Id == participante.Id );
            ParticipanteBeneficiado participanteExistente = info.DetalleBeneficiado.Find( x => x.Clave == participante.Clave && x.Id == participante.Id );

            if ( participanteExistente != null && participanteExistente.IdParticipanteRegla == itemAfectado.IdParticipanteRegla )
            {
                participanteExistente.Cantidad += this.convertNumber.ConvertToFloat( participante.Cantidad.ToString() );
            }
            else
            {
                ParticipanteBeneficiado itemBeneficiado = new ParticipanteBeneficiado();
                itemBeneficiado.Clave = participante.Clave;
                itemBeneficiado.Id = participante.Id;
                itemBeneficiado.Cantidad = this.convertNumber.ConvertToFloat( participante.Cantidad.ToString() );
                itemBeneficiado.Participante = participante;

                if ( beneficio != null )
                {
                    itemBeneficiado.Promocion = idPromo;
                    itemBeneficiado.Beneficio = beneficio.Id;
                    itemBeneficiado.Alteracion = beneficio.Cambio;
                    itemBeneficiado.AtributoAlterado = beneficio.Atributo;
                    itemBeneficiado.Valor = beneficio.Valor;
                }

				itemBeneficiado.IdParticipanteRegla = itemAfectado.IdParticipanteRegla;

                info.DetalleBeneficiado.Add( itemBeneficiado );
            }

            if ( !this.comportamiento.ConfiguracionesPorParticipante[itemAfectado.Clave].ConsumePorMonto )
            {
                itemAfectado.Cantidad = itemAfectado.Cantidad - this.convertNumber.ConvertToFloat( participante.Cantidad.ToString() );
            }

            if ( itemAfectado.Cantidad == 0 )
            {
                info.DetalleAfectado.Remove( itemAfectado );
            }
        }

        public void InformarAfectacion( InformacionPromocion info, Promocion promocion, int cantidadAplicaciones )
        {
            //InformacionPromocion info = this.informaciones.Find( x => x.IdPromocion == promocion.Id );

            //if ( info == null )
            //{
            //    info = new InformacionPromocion();
            //    this.informaciones.Add( info );
            //}

            info.Afectaciones += cantidadAplicaciones;
        }

        public void InformarActualizacionAtributoItemBeneficiado(InformacionPromocion info, string codigoPromo, IParticipante participante, Beneficio beneficio, string atributo)
        {
            ParticipanteAfectado itemAfectado = info.DetalleAfectado.Find(x => x.Clave == participante.Clave && x.Id == participante.Id);
            ParticipanteBeneficiado participanteExistente = info.DetalleBeneficiado.Find(x => x.Clave == participante.Clave && x.Id == participante.Id);

            if (participanteExistente != null)
            {
				participanteExistente.Valor = this.convertNumber.ConvertToString( this.convertNumber.ConvertToDecimal( participante.ObtenerAtributo(atributo).Valor.ToString() ), true );
            }
        }

        #region deprecated
        //    public InformacionPromocion ObtenerRespuesta( IComprobante comprobante )
        //    {
        //        InformacionPromocion retorno = new InformacionPromocion();

        //        RespuestaPromocionesParaGrillaOrganic respuesta = new RespuestaPromocionesParaGrillaOrganic();

        //        XmlNodeList itemsPromocionados = comprobante.ObtenerXml().SelectNodes( "//*[string-length(@Promo)>0]" );

        //        foreach ( XmlNode nodo in itemsPromocionados )
        //        {
        //            string promo = nodo.Attributes["Promo"].Value;
        //            decimal importeBeneficio = this.CalcularBeneficio( nodo, comprobante );

        //            if ( retorno.Detalle.Exists( x => x.CodigoPromocion == promo ) )
        //            {
        //                respuesta = retorno.Detalle.Find( x => x.CodigoPromocion == promo );
        //                respuesta.ImporteTotal = respuesta.ImporteTotal + importeBeneficio;
        //            }
        //            else
        //            {
        //                respuesta = new RespuestaPromocionesParaGrillaOrganic();
        //                respuesta.CodigoPromocion = promo;
        //                respuesta.ImporteTotal = respuesta.ImporteTotal + importeBeneficio;
        //                retorno.Detalle.Add( respuesta );
        //            }
        //        }

        //        return retorno;
        //    }

        //    private decimal CalcularBeneficio( XmlNode nodo, IComprobante comprobante )
        //    {
        //        string idParticipante = nodo.Attributes["Id"].Value;
        //        string ruta = this.GetPath( (XmlElement)nodo );

        //        IParticipante participanteOriginal = comprobante.ObtenerNodoParticipante( ruta, idParticipante, "", "" );
        //        XmlNode nodoOriginal = ((XmlNode)participanteOriginal.Nodo);

        //        EvaluadorMatematico c = new EvaluadorMatematico();

        //        string formulaNodo = this.ObtenerCadenaFormula( this.configuracionComportamiento, nodo, Convert.ToInt32( nodo.SelectSingleNode( configuracionComportamiento.CantidadEnItem ).Attributes["Valor"].Value ) );
        //        string formulaNodoOriginal = this.ObtenerCadenaFormula( this.configuracionComportamiento, nodoOriginal, Convert.ToInt32( nodo.SelectSingleNode( configuracionComportamiento.CantidadEnItem ).Attributes["Valor"].Value ) );

        //        Decimal beneficio = c.Evaluate( "(" + formulaNodoOriginal + ")-(" + formulaNodo + ")" );
        //        return beneficio;

        //    }

        //    private string ObtenerCadenaFormula( ConfiguracionComportamiento configuracionComportamiento, XmlNode nodo, int cantidad )
        //    {
        //        string retorno;
        //        retorno = configuracionComportamiento.FormulaCalculoPrecio;
        //        retorno = retorno.Replace( "<<PRECIO>>", nodo.SelectSingleNode( configuracionComportamiento.PrecioEnItem ).Attributes["Valor"].Value + "M" );
        //        retorno = retorno.Replace( "<<CANTIDAD>>", cantidad.ToString() + "M" );
        //        retorno = retorno.Replace( "<<DESCUENTO>>", nodo.SelectSingleNode( configuracionComportamiento.DescuentoEnItem ).Attributes["Valor"].Value + "M" );
        //        retorno = retorno.Replace( "<<MONTODESCUENTO>>", nodo.SelectSingleNode( configuracionComportamiento.MontoDescuentoEnItem ).Attributes["Valor"].Value + "M" );

        //        // falta la mierda del redondeo segun mascara
        //        return retorno;
        //    }

        ////        *-----------------------------------------------------------------------------------------
        ////protected function CalcularMonto() as Void
        ////    local lMonto as Integer
        ////    dodefault()
        ////    with this
        ////        if .CargaManual()
        ////            && Verificar que el cargaManual Devuelva .T. Antes de llamar a este metodo
        ////            lMonto = .Precio * .Cantidad
        ////            lMonto = lMonto - ( ( lMonto * .Descuento ) / 100 ) - .MontoDescuento
        ////            .Monto = goLibrerias.RedondearSegunMascara( lMonto )
        ////        endif
        ////    endwith
        ////endfunc 


        //    private void CalcularDiferencia( XmlNode nodo, XmlNode nodoOriginal )
        //    {

        //        throw new NotImplementedException();
        //    }

        //    public string GetPath( XmlElement node )
        //    {
        //        string path = node.Name.ToString();
        //        XmlElement currentNode = node;
        //        while ( currentNode.ParentNode != null && currentNode.ParentNode is XmlElement )
        //        {
        //            currentNode = (XmlElement)currentNode.ParentNode;
        //            path = currentNode.Name.ToString() + @"/" + path;
        //        }
        //        return path;
        //    } 
        #endregion


    }
}