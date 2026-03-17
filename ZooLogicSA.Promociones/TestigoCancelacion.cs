using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones
{
	public class TestigoCancelacion
	{

		private bool isCancellationRequested;
		private int cantidadPedidosDeCancelacionAtendidos;

		//public bool IsCancellationRequested
		//{
		//	get { return this.isCancellationRequested; }
		//	set { this.isCancellationRequested = value; }
		//}

		public void ThrowIfCancellationRequested( string mensaje )
		{
			//if ( this.cantidadPedidosDeCancelacionAtendidos > 5 )
			//{
			//	this.isCancellationRequested = false;
			//}

			if ( this.isCancellationRequested )
			{
				//this.cantidadPedidosDeCancelacionAtendidos++;
				throw new OperationCanceledException( mensaje );
			}
		}

		public void PedirCancelacion()
		{
			this.isCancellationRequested = true;
		}

		public void ResetearEstadoCancelacion()
		{
			this.cantidadPedidosDeCancelacionAtendidos = 0;
			this.isCancellationRequested = false;
		}
	}
}
