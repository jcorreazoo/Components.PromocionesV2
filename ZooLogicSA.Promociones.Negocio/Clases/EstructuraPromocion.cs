using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using ZooLogicSA.Promociones;

namespace ZooLogicSA.Promociones.Negocio.Clases
{
    public class EstructuraPromocion
    {
        private const char cSeparador = '|';
        private const char cSeparadorLabel = ';';
        private const char cSeparadorArray = '@';
        private const string cTipoPromocion = "TIPO";
        private const string cTipoPrecio = "TIPOPRECIO";
        private const string cMaskCondicion = "MASKCONDICION";
        private const string cMaskBeneficio = "MASKBENEFICIO";
        private const string cMaskTopeDescuento = "MASKTOPEDESCUENTO";
        private const string cFechaDesde = "FECHADESDE";
        private const string cFechaHasta = "FECHAHASTA";
        private const string cHoraDesde = "HORADESDE";
        private const string cHoraHasta = "HORAHASTA";
        private const string cFiltroCondicion = "FILTROCONDICION";
        private const string cFiltroBeneficio = "FILTROBENEFICIO";
        private const string cDiasSemana = "DIASSEMANA";
        private const string cReglaCondicion = "REGLACONDICION";
        private const string cReglaBeneficio = "REGLABENEFICIO";
        private const string cLeyenda = "LEYENDA";
        private const string cComportamiento = "COMPORTAMIENTO";
        private const string cListaDePrecios = "LISTADEPRECIOS";
        private const string cMaskCuotasSinRecargo = "MASKCUOTASSINRECARGO";
        private const string cAplicaAutomaticamente = "APLICAAUTOMATICAMENTE";

        private string _tipoPromocion;
        private string _condicionMask;
        private string _beneficioMask;
        private string _topeDescuentoMask;
        private DateTime _vigenciaFechaDesde;
        private DateTime _vigenciaFechaHasta;
        private DateTime _vigenciaHoraDesde;
        private DateTime _vigenciaHoraHasta;
        private string[] _condicionFiltro;
        private string[] _beneficioFiltro;
        private string[] _diasSemana;
        private string _tipoPrecio;
        private string _reglaCondicion;
        private string _reglaBeneficio;
        private string _leyenda;
        private string _comportamiento;
        private string _listaDePrecios;
        private UInt16 _cuotasSinRecargoMask;
        private bool _aplicaAutomaticamente;

        public void Inicializar()
        { 
            this._tipoPromocion = "";
            this._condicionMask = "";
            this._beneficioMask = "";
            this._vigenciaFechaDesde = DateTime.Now;
            this._vigenciaFechaHasta = DateTime.Now;
            this._vigenciaHoraDesde = new DateTime( 1, 1, 1, 0, 0, 0 );
            this._vigenciaHoraHasta = new DateTime( 1, 1, 1, 0, 0, 0 );
            this._condicionFiltro = new string[0];
            this._beneficioFiltro = new string[0];
            this._diasSemana = new string[0];
            this._tipoPrecio = "";
            this._reglaCondicion = "";
            this._reglaBeneficio = "";
            this._leyenda = "";
            this._comportamiento = "";
            this._listaDePrecios = "";
            this._cuotasSinRecargoMask = 0;
            this._aplicaAutomaticamente = false;
        }


        private string FechaAString( DateTime fecha )
        {
            return fecha.ToString( "dd/MM/yyyy" );
        }

        private DateTime StringAFecha( string fecha )
        {
            DateTime retorno;

            int dia = Convert.ToInt32( fecha.Substring( 0, 2 ) );
            int mes = Convert.ToInt32( fecha.Substring( 3, 2 ) );
            int anio = Convert.ToInt32( fecha.Substring( 6, 4 ) );
            retorno = new DateTime( anio, mes, dia, 0, 0, 0 );

            return retorno;
        }

        private string HoraAString( DateTime hora )
        {
            return hora.ToString( "HH:mm:ss" );
        }

        private DateTime StringAHora( string valor )
        {
            DateTime retorno;

            int hora = Convert.ToInt32( valor.Substring( 0, 2 ) );
            int min = Convert.ToInt32( valor.Substring( 3, 2 ) );
            int segundos = Convert.ToInt32( valor.Substring( 6, 2 ) );

            retorno = new DateTime( 1, 1, 1, hora, min, segundos );

            return retorno;
        }

        
        /// <summary> 
        /// Tiene la siguiente estructura: 
        /// tipoPromocion|condicionMask|beneficioMas|fechaDesde|fechaHasta|horaDesde|
        /// horaHasta|filtroCondicion1@filtroCondicion2|filtroBeneficio1@filtroBeneficio2|
        /// lunes@martes@mier@jue@vie@sab@dom
        /// </summary>
        public string Resultado
        {
            get
            {
                string result;

                result = cTipoPromocion + cSeparadorLabel + this._tipoPromocion + cSeparador +
                        cMaskCondicion + cSeparadorLabel + this._condicionMask + cSeparador +
                        cMaskBeneficio + cSeparadorLabel + this._beneficioMask + cSeparador +
                        cMaskTopeDescuento + cSeparadorLabel + this._topeDescuentoMask + cSeparador +
                        cMaskCuotasSinRecargo + cSeparadorLabel + this._cuotasSinRecargoMask + cSeparador +
                        cFechaDesde + cSeparadorLabel + this.FechaAString( this._vigenciaFechaDesde ) + cSeparador +
                        cFechaHasta + cSeparadorLabel + this.FechaAString( this._vigenciaFechaHasta ) + cSeparador +
                        cHoraDesde + cSeparadorLabel + this.HoraAString( this._vigenciaHoraDesde ) + cSeparador +
                        cHoraHasta + cSeparadorLabel + this.HoraAString( this._vigenciaHoraHasta ) + cSeparador + 
                        cTipoPrecio + cSeparadorLabel + this._tipoPrecio + cSeparador +
                        cReglaBeneficio + cSeparadorLabel + this._reglaBeneficio + cSeparador +
                        cReglaCondicion + cSeparadorLabel + this._reglaCondicion + cSeparador +
                        cListaDePrecios + cSeparadorLabel + this._listaDePrecios + cSeparador ;
                result = result + cFiltroCondicion + cSeparadorLabel;
                
                int cant = 0;
                foreach ( string cadena in _condicionFiltro )
                {
                    cant = cant + 1;
                    if ( cant < _condicionFiltro.Length )
                        result = result + cadena + cSeparadorArray;
                    else
                        result = result + cadena;
                }
                result = result + cSeparador + cFiltroBeneficio + cSeparadorLabel;
                cant = 0;
                foreach ( string cadena in _beneficioFiltro )
                {
                    cant = cant + 1;
                    if ( cant < _beneficioFiltro.Length )
                        result = result + cadena + cSeparadorArray;
                    else
                        result = result + cadena;
                }
                result = result + cSeparador + cDiasSemana + cSeparadorLabel;
                cant = 0;
                foreach ( string cadena in _diasSemana )
                {
                    cant = cant + 1;
                    if ( cant < _diasSemana.Length )
                        result = result + cadena + cSeparadorArray;
                    else
                        result = result + cadena;
                }
                result += cSeparador + cLeyenda + cSeparadorLabel + _leyenda;
                result += cSeparador + cComportamiento + cSeparadorLabel + _comportamiento;
                result += cSeparador + cAplicaAutomaticamente + cSeparadorLabel + _aplicaAutomaticamente;

                return result;
            }
            set
            {
                string[] propiedades;
                string[] valores;
                
                propiedades = value.Split( cSeparador );
                foreach ( string propiedad in propiedades )
                {
                    valores = propiedad.Split( cSeparadorLabel );
                    if ( valores.Length > 1 )
                        LlenarAtributo( valores[0], valores[1] );
                }
            }
        }

        private void LlenarAtributo( string atributo, string valor )
        { 
            switch (atributo)
            {
                case cTipoPromocion:
                    {
                        this._tipoPromocion = valor;
                        break;
                    }
                case cMaskCondicion:
                    {
                        this._condicionMask = valor;
                        break;
                    }
                case cMaskBeneficio:
                    {
                        this._beneficioMask = valor;
                        break;
                    }
                case cMaskTopeDescuento:
                    {
                        this._topeDescuentoMask = valor;
                        break;
                    }
                case cFechaDesde:
                    {
                        this._vigenciaFechaDesde = this.StringAFecha( valor );
                        //DateTime.TryParse( valor, out this._vigenciaFechaDesde );
                        break;
                    }
                case cFechaHasta:
                    {
                        this._vigenciaFechaHasta = this.StringAFecha( valor );
                        //DateTime.TryParse( valor, out this._vigenciaFechaHasta );
                        break;
                    }
                case cHoraDesde:
                    {
                        this._vigenciaHoraDesde = this.StringAHora(valor);
                        break;
                    }
                case cHoraHasta:
                    {
                        this._vigenciaHoraHasta = this.StringAHora( valor );
                        break;
                    }
                case cTipoPrecio:
                    {
                        this._tipoPrecio = valor;
                        break;
                    }
                case cFiltroCondicion:
                    {
                        string[] filtros;
                        filtros = valor.Split( cSeparadorArray );
                        this._condicionFiltro = filtros;
                        break;
                    }
                case cFiltroBeneficio:
                    {
                        string[] filtros;
                        filtros = valor.Split( cSeparadorArray );
                        this._beneficioFiltro = filtros;
                        break;
                    }
                case cDiasSemana:
                    {
                        string[] dias;
                        dias = valor.Split( cSeparadorArray );
                        this._diasSemana = dias;
                        break;
                    }
                case cReglaCondicion:
                    {
                        this._reglaCondicion = valor;
                        break;
                    }
                case cReglaBeneficio:
                    {
                        this._reglaBeneficio = valor;
                        break;
                    }
                case cListaDePrecios:
                    {
                        this._listaDePrecios = valor;
                        break;
                    }
                case cLeyenda:
                    {
                        this._leyenda = valor;
                        break;
                    }
                case cComportamiento:
                    {
                        this._comportamiento = valor;
                        break;
                    }
                case cMaskCuotasSinRecargo:
                    {
                        this._cuotasSinRecargoMask = Convert.ToUInt16(valor);
                        break;
                    }
                case cAplicaAutomaticamente:
                    {
                        if (string.IsNullOrEmpty(valor))
                        {
                            this._aplicaAutomaticamente = false;
                        } else {
                            this._aplicaAutomaticamente = Convert.ToBoolean(valor);
                        }
                        break;
                    }
            }
        }

        public string tipoPromocion
        {
            get
            {
                return _tipoPromocion;
            }
            set
            {
                _tipoPromocion = value;
            }
        }

        public string tipoPrecio
        {
            get
            {
                return _tipoPrecio;
            }
            set
            {
                _tipoPrecio = value;
            }
        }

        public string condicionMask
        {
            get
            {
                return _condicionMask;
            }
            set 
            {
                _condicionMask = value;
            }
        }

        public string beneficioMask
        {
            get
            {
                return _beneficioMask;
            }
            set 
            {
                _beneficioMask = value;
            }
        }

        public string topeDescuentoMask
        {
            get
            {
                return _topeDescuentoMask;
            }
            set
            {
                _topeDescuentoMask = value;
            }
        }

        public DateTime vigenciaFechaDesde
        {
            get
            {
                return _vigenciaFechaDesde;
            }
            set
            {
                _vigenciaFechaDesde = value;
            }
        }

        public DateTime vigenciaFechaHasta
        {
            get
            {
                return _vigenciaFechaHasta;
            }
            set
            {
                _vigenciaFechaHasta = value;
            }
        }

        public DateTime vigenciaHoraDesde
        {
            get
            {
                return _vigenciaHoraDesde;
            }
            set
            {
                _vigenciaHoraDesde = value;
            }
        }

        public DateTime vigenciaHoraHasta
        {
            get
            {
                return _vigenciaHoraHasta;
            }
            set
            {
                _vigenciaHoraHasta = value;
            }
        }

        public string[] condicionFiltro
        {
            get
            {
                return _condicionFiltro;
            }
            set
            {
                _condicionFiltro = value;
            }
        }

        public string[] beneficioFiltro
        {
            get
            {
                return _beneficioFiltro;
            }
            set
            {
                _beneficioFiltro = value;
            }
        }

        public string[] diasSemana
        {
            get
            {
                return _diasSemana;
            }
            set
            {
                _diasSemana = value;
            }
        }

        public string reglaCondicion
        {
            get
            {
                return _reglaCondicion;
            }
            set
            {
                _reglaCondicion = value;
            }
        }

        public string reglaBeneficio
        {
            get
            {
                return _reglaBeneficio;
            }
            set
            {
                _reglaBeneficio = value;
            }
        }

        public string listaDePrecios
        {
            get
            {
                return _listaDePrecios;
            }
            set
            {
                _listaDePrecios = value;
            }
        }

        public string leyenda
        {
            get
            {
                return _leyenda;
            }
            set
            {
                _leyenda = value;
            }
        }

        public string comportamiento
        {
            get { return _comportamiento; }
            set { _comportamiento = value; }
        }

        public UInt16 cuotasSinRecargoMask
        {
            get
            {
                return _cuotasSinRecargoMask;
            }
            set
            {
                _cuotasSinRecargoMask = value;
            }
        }
        public Boolean aplicaAutomaticamente
        {
            get
            {
                return _aplicaAutomaticamente;
            }
            set
            {
                _aplicaAutomaticamente = value;
            }
        }
    }
}
