using System;
using System.Collections.Generic;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Comprobante
{
    public interface IParticipante
    {
        string Id { get; }
        string Clave { get; set; }
        decimal Cantidad { get; set; }
        object Nodo { get; set; }

        decimal Consumido { get; set; }
        string Promocion { get; set; }
        string Beneficio { get; set; }
        decimal ConsumoPorCombinacion { get; set; }

        bool CompararSegunContenido( IParticipante participanteNuevo );
        void AgregarAlMismoNivel( IParticipante participanteNuevo );
        void AplicarValorAAtributo( string atributo, Alteracion alteracion, object valor );

        IParticipante Clonar();
        IAtributo ObtenerAtributo( string rutaAtributo );

        IAtributo ObtenerCantidadSinRestarConsumido(string rutaAtributo);
        
        decimal ObtenerPrecioUnitario();

        void Destruir();

		string[] CoincidenciasExcluidas { get; set; }

        void ModificarCantidadSegunMonto();

    }
}
