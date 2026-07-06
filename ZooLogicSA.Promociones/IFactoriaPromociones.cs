using System.Collections.Generic;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.EvaluacionReglas;
using ZooLogicSA.Promociones.FormatoPromociones;
using ZooLogicSA.Promociones.Informantes;
using ZooLogicSA.Promociones.Utils;
using ZooLogicSA.Redondeos;

namespace ZooLogicSA.Promociones
{
    public interface IFactoriaPromociones
    {
        IArmadorDeCoincidencias ObtenerArmadorDeCoincidencias( ConfiguracionComportamiento comportamiento );
        CalculadorDeIncumplimiento ObtenerCalculadorDeIncumplimiento( ConfiguracionComportamiento configuracionComportamiento );
        CalculadorMontoBeneficio ObtenerCalculadorPrecios( ConfiguracionComportamiento configuracionComportamiento );
        IComparadorDeParticipantes ObtenerComparadorDeParticipantes( ConfiguracionComportamiento configuracionComportamiento );
        Dictionary<string, IEvaluadorMatematico> ObtenerEvaluadoresMatematicos( ConfiguracionComportamiento comportamiento );
        IInformantePromociones ObtenerInformantePromociones( ConfiguracionComportamiento configuracionComportamiento );
        List<InformacionPromocion> ObtenerListaInformacionPromocion();
        List<Promocion> ObtenerListaPromociones();
        NotificadorServicioPromociones ObtenerNotificadorServicioPromociones();
        IComprobante ObtenerNuevoComprobante( ConfiguracionComportamiento configuracionComportamiento );
        Dictionary<EleccionParticipanteType, ISeleccionadorParticipantes> ObtenerSeleccionadoresParticipantes();
        TransformadorComprobante ObtenerTransformadorDeComprobantes( ConfiguracionComportamiento configuracionComportamiento );
        IValidadorPromociones ObtenerValidadoresPromociones( ConfiguracionComportamiento configuracionComportamiento );
        ICalculadorMonto ObtenerCalculadorMonto( ConfiguracionComportamiento configuracionComportamiento );

        List<EntidadRedondeo> ObtenerRedondeos(string xmlEntidad, string xmlDetTabla, string xmlDetCent, string xmlDetEnt);
    }
}
