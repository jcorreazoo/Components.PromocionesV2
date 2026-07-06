using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Utils;

namespace ZooLogicSA.Promociones
{
    public class ParticipanteXml : IParticipante
    {
        private string clave;
        private string promocion;
        private string beneficio;
        private object nodo;
        private ConfiguracionComportamiento comportamiento;
		private List<string> coincidenciasExcluidas;
        private ConvertidorNumerico convertNumber;

        public ParticipanteXml( ConfiguracionComportamiento comportamiento )
        {
            this.comportamiento = comportamiento;
            this.convertNumber = new ConvertidorNumerico();
        }

        public string Clave
        {
            get { return this.clave; }
            set { this.clave = value; }
        }

        public string Id
        {
            get
            {
                string retorno = "";

                ConfiguracionPorParticipante configuracion;
                if ( !this.comportamiento.ConfiguracionesPorParticipante.TryGetValue( this.clave, out configuracion ) )
                {
                    retorno = ((XmlElement)this.nodo).Attributes["Id"].Value;
                }
                else
                {
                    XmlNode nodoId = ((XmlElement)this.nodo).SelectSingleNode( configuracion.AtributoId );
                    if ( nodoId != null )
                    {
                        retorno = nodoId.Attributes["Valor"].Value;
                    }
                }
                
                return retorno;
            }
        }

        public string Promocion
        {
            get
            {
                string promo = ((XmlElement)this.nodo).Attributes["Promo"].Value;
                return promo;
            }
            set
            {
                ((XmlElement)this.nodo).SetAttribute( "Promo", value );
                this.promocion = value;
            }
        }

        public string Beneficio
        {
            get
            {
                string beneficio = ((XmlElement)this.nodo).Attributes["Beneficio"].Value;
                return beneficio;
            }
            set
            {
                ((XmlElement)this.nodo).SetAttribute( "Beneficio", value );
                this.beneficio = value;
            }
        }

        public object Nodo
        {
            get { return this.nodo; }
            set { this.nodo = value; }
        }

        public decimal Cantidad
        {
            get
            {
                decimal retorno = 1;
                XmlNode nodoCantidad = ((XmlElement)this.nodo).SelectSingleNode( this.comportamiento.ConfiguracionesPorParticipante[this.Clave].Cantidad );
                if ( nodoCantidad != null )
                {
                    retorno = this.convertNumber.ConvertToDecimal( nodoCantidad.Attributes["Valor"].Value );
                }

                return retorno;
            }
            set
            {
                XmlNode nodoCantidad = ((XmlElement)this.nodo).SelectSingleNode( this.comportamiento.ConfiguracionesPorParticipante[this.Clave].Cantidad );
                ((XmlElement)nodoCantidad).SetAttribute( "Valor", this.convertNumber.ConvertToString( value, true ) );
            }
        }

        public decimal ConsumoPorCombinacion
        {
            get
            {
                decimal retorno = 0;
                string consumoxcomb = ((XmlElement)this.nodo).Attributes["ConsumoPorCombinacion"]?.Value ?? "0";
                retorno = this.convertNumber.ConvertToDecimal(consumoxcomb);
                return retorno;
            }
            set
            {
                ((XmlElement)this.nodo).SetAttribute("ConsumoPorCombinacion", this.convertNumber.ConvertToString(value, true));
            }
        }

        public decimal Consumido
        {
            get
            {
                string consumido = ((XmlElement)this.nodo).Attributes["Consumido"].Value;
                return this.convertNumber.ConvertToDecimal( consumido );
            }
            set
            {
                ((XmlElement)this.nodo).SetAttribute( "Consumido", this.convertNumber.ConvertToString( value, true ) );
            }
        }

        public IParticipante Clonar()
        {
            ParticipanteXml retorno = new ParticipanteXml( this.comportamiento );
            retorno.nodo = ((XmlElement)this.nodo).Clone();
            retorno.Clave = this.Clave;
            retorno.Consumido = this.Consumido;
            return retorno;
        }

        public void AplicarValorAAtributo( string atributo, Alteracion alteracion, object valor )
        {
            XmlElement nodoAtributo = (XmlElement)((XmlElement)this.nodo).SelectSingleNode( atributo );

            string valorBeneficio;
            valorBeneficio = this.convertNumber.ConvertToString( this.convertNumber.ConvertToDecimal( valor.ToString() ), true );

            switch ( alteracion )
            {
                case Alteracion.CambiarValor:
                    nodoAtributo.Attributes["Valor"].Value = valorBeneficio;
                    break;
                case Alteracion.IncrementarEnCantidad:
                    nodoAtributo.Attributes["Valor"].Value = this.Sumar( valorBeneficio, nodoAtributo.Attributes["Valor"].Value, nodoAtributo.Attributes["TipoDato"].Value );
                    break;
                case Alteracion.DisminuirEnCantidad:
                    nodoAtributo.Attributes["Valor"].Value = this.Restar( valorBeneficio, nodoAtributo.Attributes["Valor"].Value, nodoAtributo.Attributes["TipoDato"].Value );
                    break;
                case Alteracion.IncrementarEnPorcentaje:
                    throw new NotImplementedException();
                case Alteracion.DisminuirEnPorcentaje:
                    nodoAtributo.Attributes["Valor"].Value = this.Descontar( valorBeneficio, nodoAtributo.Attributes["Valor"].Value, nodoAtributo.Attributes["TipoDato"].Value );
                    break;
                default:
                    break;
            }
        }

        private string Descontar( string valorBeneficio, string valorElemento, string tipoDato )
        {
            decimal valorActual = this.convertNumber.ConvertToDecimal( valorElemento );
            decimal factor = this.convertNumber.ConvertToDecimal( valorBeneficio );

            decimal nuevoValor = valorActual * (1 - factor / 100);

            decimal diferencia = nuevoValor - valorActual;

            return this.convertNumber.ConvertToString( diferencia, true );
        }

        private string Restar( string valorBeneficio, string valorElemento, string tipoDato )
        {
            return this.convertNumber.ConvertToString( this.convertNumber.ConvertToInt( valorElemento ) - this.convertNumber.ConvertToInt( valorBeneficio ), true );
        }

        private string Sumar( string valorBeneficio, string valorElemento, string tipoDato )
        {
            return this.convertNumber.ConvertToString( this.convertNumber.ConvertToInt( valorElemento ) + this.convertNumber.ConvertToInt( valorBeneficio ), true );
        }

        public bool CompararSegunContenido( IParticipante participanteNuevo )
        {
            decimal cantNuevo = participanteNuevo.Cantidad;

            XmlNode nodoPromocionadoYaExistenteClon = ((XmlElement)this.nodo).Clone();

            nodoPromocionadoYaExistenteClon.SelectSingleNode( this.comportamiento.ConfiguracionesPorParticipante[this.Clave].Cantidad ).Attributes["Valor"].Value = this.convertNumber.ConvertToString( cantNuevo, true );

            bool retorno = false;
            if ( nodoPromocionadoYaExistenteClon.InnerXml.Equals( ((XmlElement)participanteNuevo.Nodo).InnerXml ) )
            {
                retorno = true;
            }

            return retorno;
        }

        public void AgregarAlMismoNivel( IParticipante participanteNuevo )
        {
            ((XmlElement)this.nodo).ParentNode.AppendChild( (XmlElement)participanteNuevo.Nodo );
        }

        public IAtributo ObtenerAtributo( string rutaAtributo )
        {
            IAtributo retorno = new AtributoXml();

            XmlNode nodoAtributo = (XmlElement)( (XmlElement)this.nodo ).SelectSingleNode( rutaAtributo.Replace( ".", "/" ) );

            if ( nodoAtributo != null )
            {
                retorno.TipoDato = this.CastearTipoDato( nodoAtributo.Attributes["TipoDato"].Value );
                retorno.Valor = this.ConvertirValor( nodoAtributo.Attributes["Valor"].Value, retorno.TipoDato );
            }

            if ( rutaAtributo == this.comportamiento.ConfiguracionesPorParticipante[this.Clave].Cantidad && ((XmlElement)this.nodo).Attributes["Consumido"] != null )
            {
                retorno.Valor = this.convertNumber.ConvertToInt( retorno.Valor.ToString() ) - this.convertNumber.ConvertToInt( ((XmlElement)this.nodo).Attributes["Consumido"].Value );
            }

            return retorno;
        }

        public IAtributo ObtenerCantidadSinRestarConsumido(string rutaAtributo)
        {
            XmlNode nodoAtributo = (XmlElement)((XmlElement)this.nodo).SelectSingleNode(rutaAtributo.Replace(".", "/"));

            IAtributo retorno = new AtributoXml();

            retorno.TipoDato = this.CastearTipoDato(nodoAtributo.Attributes["TipoDato"].Value);
            retorno.Valor = this.ConvertirValor(nodoAtributo.Attributes["Valor"].Value, retorno.TipoDato);

            return retorno;
        }

        private object ConvertirValor( string valor, TipoDato tipo )
        {
            object retorno = null;

            switch ( tipo )
            {
                case TipoDato.C:
                    retorno = valor;
                    break;
                case TipoDato.N:
                    retorno = this.convertNumber.ConvertToDecimal( valor );
                    break;
                case TipoDato.D:
                    retorno = new DateTime( this.convertNumber.ConvertToInt( valor.Substring( 6, 4 ) ), this.convertNumber.ConvertToInt( valor.Substring( 3, 2 ) ), this.convertNumber.ConvertToInt( valor.Substring( 0, 2 ) ), 0, 0, 0 );
                    break;
                case TipoDato.L:
                    retorno = bool.Parse( valor );
                    break;
                default:
                    break;
            }

            return retorno;
        }

        private TipoDato CastearTipoDato( string tipoDato )
        {
            return (TipoDato)TipoDato.Parse( typeof( TipoDato ), tipoDato );
        }

        public void Destruir()
        {
            ((XmlNode)this.nodo).ParentNode.RemoveChild( (XmlNode)this.nodo );
        }

        public decimal ObtenerPrecioUnitario()
        {
            decimal retorno = 1;

            string claveNodoPrecio = this.comportamiento.ConfiguracionesPorParticipante[this.clave].Precio;

            if ( !string.IsNullOrEmpty( claveNodoPrecio ) )
            {
                XmlNode nodoPrecio = ((XmlElement)this.nodo).SelectSingleNode( this.comportamiento.ConfiguracionesPorParticipante[this.clave].Precio );
                retorno = this.convertNumber.ConvertToDecimal( nodoPrecio.Attributes["Valor"].Value );
            }

            return retorno;
        }

		public string[] CoincidenciasExcluidas
		{
			get
			{
				string[] lista = new string[0];

				XmlAttribute atributoCoincidencias = ((XmlElement)this.nodo).Attributes["CoincidenciasExcluidas"];
				if ( atributoCoincidencias != null )
				{
					lista = atributoCoincidencias.Value.Split( ',' );
				}
				
				return lista;
			}
			set
			{
				string listaRaw = String.Join( ",", value );
				((XmlElement)this.nodo).SetAttribute( "CoincidenciasExcluidas", listaRaw );
			}
		}

        public void ModificarCantidadSegunMonto()
        {
            var atributo = this.ObtenerAtributo("MONTO");
            decimal monto = this.convertNumber.ConvertToDecimal( atributo.Valor.ToString() );
            this.Cantidad = monto;
        }
    }
}
