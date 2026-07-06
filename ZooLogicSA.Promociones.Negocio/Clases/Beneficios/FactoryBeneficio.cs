using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Negocio.Clases.Beneficios
{
    public static class FactoryBeneficio
    {
        private const string cNombreAtributoDescuento = "DESCUENTO";
        private const string cNombreAtributoMonto = "MONTOFINAL";
        private const string cNombreAtributoMontoValor = "MONTO";
        private const string cNombreAtributoDescuentoMonto = "MONTODESCUENTO";

        public static Beneficio ObtenerBeneficio( BeneficioType beneficioType, ValorBeneficio valoresBeneficio )
        {
            Beneficio beneficio = new Beneficio();

            switch ( beneficioType )
            {
                case BeneficioType.LLevaXPagaY:
                    {
                        beneficio.Cambio = Alteracion.CambiarValor;
                        beneficio.Atributo = cNombreAtributoDescuento;
                        beneficio.Destinos = valoresBeneficio.Destinos;
                        beneficio.Valor = "100";
                        break;
                    }
                case BeneficioType.PorcentajeFijoDeDescuento:
                    {
                        beneficio.Cambio = Alteracion.CambiarValor;
                        beneficio.Atributo = cNombreAtributoDescuento;
                        beneficio.Destinos = valoresBeneficio.Destinos;
                        beneficio.Valor = valoresBeneficio.Valor;
                        break;
                    }
                case BeneficioType.MontoFijoDeDescuento:
                    {
                        beneficio.Cambio = Alteracion.CambiarValor;
                        beneficio.Atributo = cNombreAtributoMonto;
                        beneficio.Destinos = valoresBeneficio.Destinos;
                        beneficio.Valor = valoresBeneficio.Valor;
                        //ZOMBIEATRIBUTO NUEVO
                        break;
                    }
                case BeneficioType.LlevaUnoPagaConDescuentoOtro:
                    {
                        beneficio.Cambio = Alteracion.CambiarValor;
                        beneficio.Atributo = cNombreAtributoDescuento;
                        beneficio.Destinos = valoresBeneficio.Destinos;
                        beneficio.Valor = valoresBeneficio.Valor;
                        break;
                    }
                case BeneficioType.PorcentajeFijoDeDescuentoBancario:
                    {
                        beneficio.Cambio = Alteracion.DisminuirEnPorcentaje;
                        beneficio.Atributo = cNombreAtributoMontoValor;
                        beneficio.Destinos = valoresBeneficio.Destinos;
                        beneficio.Valor = valoresBeneficio.Valor;
                        break;
                    }
                case BeneficioType.ValorDeOtraListaDePrecios:
                    {
                        beneficio.Cambio = Alteracion.CambiarValor;
                        beneficio.Atributo = cNombreAtributoDescuentoMonto;
                        beneficio.Destinos = valoresBeneficio.Destinos;
                        beneficio.Valor = valoresBeneficio.Valor;
                        break;
                    }

            }
            return beneficio;
        }
    }
}
