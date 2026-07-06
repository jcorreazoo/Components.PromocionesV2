using System;

namespace ZooLogicSA.Promociones.Asistente.ArmadoDeLeyenda
{
	public interface IArmadorDeLeyendaSegunParticipanteFaltante
	{
		string ObtenerLeyendaSegunRegla( ParticipanteFaltante participante, InformacionPromocionIncumplida info );
	}
}
