using System;
using System.Collections.Generic;
using ZooLogicSA.Promociones.Comprobante;
using ZooLogicSA.Promociones.FormatoPromociones;
namespace ZooLogicSA.Promociones.Informantes
{
    public interface IInformantePromociones
    {
        //List<InformacionPromocion> Informaciones { get; set; }
        void InformarItemAfectado( InformacionPromocion info, string idPromo, CoincidenciaEvaluacion coincidencia, IParticipante participante );
        void InformarItemBeneficiado( InformacionPromocion info, string codigoPromo, IParticipante participante, Beneficio beneficio );
        void InformarItemReAfectado( InformacionPromocion info, IParticipante participante, Decimal cantidadAplicaciones );
        void InformarItemNuevoAfectado( InformacionPromocion info, IParticipante participante, Decimal cantidadAplicaciones );
        void InformarAfectacion( InformacionPromocion info, Promocion promocion, int cantidadAplicaciones );
        void InformarActualizacionAtributoItemBeneficiado(InformacionPromocion info, string codigoPromo, IParticipante participante, Beneficio beneficio, string atributo);
    }
}