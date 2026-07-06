using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.Asistente.ArmadoDeLeyenda
{
	public class FactoriaArmadorDeLeyenda
	{
		public IArmadorDeLeyenda ObtenerArmadorDeLeyenda()
		{
			Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante> armadores = new Dictionary<string, IArmadorDeLeyendaSegunParticipanteFaltante>();

			ArmadorDeLeyendaSegunParticipanteFaltanteGenerico armadorGenerico = new ArmadorDeLeyendaSegunParticipanteFaltanteGenerico();

			armadores.Add( "", armadorGenerico );
			armadores.Add( "COMPROBANTE", new ArmadorDeLeyendaSegunParticipanteFaltanteComprobante( armadorGenerico ) );

			return new ArmadorDeLeyenda( armadores );
		}
	}
}
