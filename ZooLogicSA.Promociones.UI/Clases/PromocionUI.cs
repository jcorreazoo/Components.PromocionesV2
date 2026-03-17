using ZooLogicSA.Promociones.UI.Controllers;
using System;

namespace ZooLogicSA.Promociones.UI.Clases
{
    public class PromocionUI
    {
        private ControllerPromocion _kontroler;

        public PromocionUI()
        {
            this._kontroler = new ControllerPromocion();
        }

        public void AsignarKontroler( ControllerPromocion kontroler )
        {
            this._kontroler = kontroler;
        }

//        public void LimpiarParticipantes()
//        {
//            this._kontroler.LimpiarParticipantes();
//        }

//        public void AgregarParticipante( string valor, int idPadre )
//        {
//            this._kontroler.AgregarParticipante( valor, idPadre );
//        }

//        public void AgregarColumnasFiltroParticipante( string columnas, string delimitador )
//        {
//            this._kontroler.AgregarColumnasFiltroParticipante( columnas, delimitador );
//        }

//        public void AgregarFilaFiltroParticipante( string datosParticipante, string delimitador )
//        {
//            this._kontroler.AgregarFilaFiltroParticipante( datosParticipante, delimitador );
//        }

    }
}
