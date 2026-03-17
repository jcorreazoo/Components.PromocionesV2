using System;

namespace ZooLogicSA.Promociones.UI.Clases.Adaptadores
{
    public class AdaptadorFactorException : Exception
    {
        private string message;

        public AdaptadorFactorException( string Message )
        {
            this.message = Message;
        }

        public string Mensaje
        {
            get
            {
                return this.message;
            }
        }
    }
}
