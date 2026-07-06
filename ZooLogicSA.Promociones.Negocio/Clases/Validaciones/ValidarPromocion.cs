using System;
using System.Collections.Generic;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Negocio.Clases.Validaciones
{
    public class ValidarPromocion
    {
        public bool ValidarFechas( DateTime desde, DateTime hasta )
        {
            return (desde.Date <= hasta.Date);
        }

        public bool ValidarHorario( DateTime desde, DateTime hasta )
        {
            return (desde <= hasta);
        }

        public bool ValidarDatoObligatorio(string valor)
        {
            return (!string.IsNullOrEmpty(valor));
        }

        public bool ValidarDiasDeLaSemana( string[] dias )
        {
            bool esValido = false;
            foreach (var dia in dias)
            {
                if (dia.ToUpper() == "TRUE")
                {
                    esValido = true;
                    break;
                }

            }
            
            return esValido;
        }

        public bool ValidarCantidadDeReglasTotales( List<string> participantes )
        {
            bool esValido = false;
            int reglas = 0;
            foreach (var participante in participantes)
            {
                reglas += ( participante.Split('[').Length - 1 );
            }

            if ( reglas <= 500)
                {
                esValido = true;
            }
            return esValido;
        }
    }
}