using System;
using System.Collections.Generic;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.EvaluacionReglas;
using ZooLogicSA.Promociones.FormatoPromociones;
using System.Reflection;
using System.Configuration;

namespace ZooLogicSA.Promociones.Comparadores
{
    public class GestorComparaciones
    {
        private Dictionary<Factor, IComparador> comparadores;
        private List<ErrorEvaluador> excepciones;

        public GestorComparaciones()
        {
            this.InicializarComparadores();
            this.excepciones = new List<ErrorEvaluador>();
            int count = Enum.GetValues( typeof( Factor ) ).Length;
        }

        public Dictionary<Factor, IComparador> Comparadores
        {
            get { return this.comparadores; }
            set { this.comparadores = value; }
        }

        private void InicializarComparadores()
        {
            bool caseSensitive = this.ObtenerValorCaseSensitive();

            this.comparadores = new Dictionary<Factor, IComparador>();
            this.comparadores.Add( Factor.DebeSerIgualA, new ComparadorDebeSerIgual(caseSensitive) );
            this.comparadores.Add( Factor.DebeSerMayorA, new ComparadorDebeSerMayorA() );
            this.comparadores.Add( Factor.DebeSerMenorA, new ComparadorDebeSerMenorA() );
            this.comparadores.Add( Factor.DebeSerIgualADiaDeLaSemana, new ComparadorDebeSerIgualADiaDeLaSemana() );
            this.comparadores.Add( Factor.DebeSerDistintoA, new ComparadorDebeSerDistintoA(caseSensitive) );
            this.comparadores.Add( Factor.DebeSerMayorIgualA, new ComparadorDebeSerMayorIgualA() );
            this.comparadores.Add( Factor.DebeSerMenorIgualA, new ComparadorDebeSerMenorIgualA() );
            this.comparadores.Add( Factor.DebeSerContieneA, new ComparadorDebeSerContieneA(caseSensitive) );
            this.comparadores.Add( Factor.DebeSerComienzaCon, new ComparadorDebeSerComienzaCon(caseSensitive) );
            this.comparadores.Add( Factor.DebeSerTerminaCon, new ComparadorDebeSerTerminaCon(caseSensitive) );
            this.comparadores.Add( Factor.None, new ComparadorNone() );
        }

        public bool Comparar( Factor factor, TipoDato tipoDato, object p, object cantidadParaRegla, Regla regla )
        {
            bool retorno = false;
            try
            {
                retorno = this.comparadores[factor].Comparar(tipoDato, p, cantidadParaRegla);
            }
            catch (Exception error)
            {
                ErrorEvaluador err = new ErrorEvaluador();
                err.Mensaje = "Ocurrió un error en la validación de la regla " + regla.DescripcionAtributo + " de la promoción [" + regla.Id.ToString() + "]\n";
                err.Mensaje += "Tipo de dato esperado : [" + tipoDato.ToString() + "] - Valor asignado: " + p.ToString() + " (" + p.GetType().ToString() + ")" + "- Mensaje de error: " + error.Message;
                this.excepciones.Add(err);
                retorno =  false;
            }
            return retorno; 
        }

        public List<ErrorEvaluador> ObtenerExcepciones()
        {
            return this.excepciones;
        }

        public void LimpiarExcepciones()
        {
            this.excepciones.Clear();
        }

        public bool ObtenerValorCaseSensitive()
        {
            bool retorno = true;
            try
            {
                string ruta = Assembly.GetExecutingAssembly().Location;
                Configuration config = ConfigurationManager.OpenExeConfiguration(ruta);
                AppSettingsSection appSettings = (AppSettingsSection)config.GetSection("appSettings");
                retorno = this.ObtenerValorAppSettings(appSettings, "ComparacionStringCaseSensitive");
            }
            catch (Exception)
            {
            }

            return retorno;            
        }

        private bool ObtenerValorAppSettings(AppSettingsSection appSettings, string clave)
        {
            bool retorno = true;

            try
            {
                retorno = appSettings.Settings[clave].Value.Equals("0");
            }
            catch (Exception)
            {
            }

            return retorno;
        }
    }
}
