using System;
using System.Collections;
using System.Collections.Generic;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
namespace ZooLogicSA.Promociones
{
	public interface IMotorPromociones
	{
		void AgregarComprobanteParaEvaluacion( string identificador, string xml );
        void AgregarPreciosAdicionalesParaEvaluacion(string identificador, string xml);
        List<InformacionPromocion> AplicarPromociones( string idProceso, System.Collections.ArrayList promos );
		List<InformacionPromocion> AplicarPromociones( string idProceso, List<string> promos );
		void EstablecerLibreriaPromociones( List<Promocion> promociones );

		List<InformacionPromocion> EvaluarPromocionesIndividualmente(string identificador, ArrayList promos);
		List<InformacionPromocion> EvaluarPromocionesIndividualmente(string identificador, ArrayList promos, bool filtrar);
		List<InformacionPromocion> EvaluarPromocionesIndividualmente(string identificador, List<string> promos);
		List<InformacionPromocion> EvaluarPromocionesIndividualmente(string identificador, List<string> promos, bool filtrar);

		List<Promocion> ObtenerPromociones();
		List<string> ParticipantesNecesarios { get; }

		InformacionPromocionIncumplida ObtenerResultadosParciales( IComprobante comprobante, string idPromocion );
		InformacionPromocionIncumplida ObtenerResultadosParciales( IComprobante comprobante, string idPromocion, TestigoCancelacion testigoCancelacion );

		List<InformacionPromocion> EvaluarYAplicarPromocion( IComprobante comprobante, string promo, TestigoCancelacion cancelacion );

		IComprobante ObtenerCopiaDeComprobante( string idProceso );

		List<string> ObtenerPromocionesQueCumplaElParticipanteComprobante( string comprobante );

        String[] ObtenerListasDePreciosUsadasEnPromociones();
    }
}