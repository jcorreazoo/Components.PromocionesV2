using System;
namespace ZooLogicSA.Promociones.Asistente
{
    public interface IArmadorDeLeyenda
    {
        string Armar(ZooLogicSA.Promociones.InformacionPromocionIncumplida info);
        string ArmarLeyendaAMostrarEnControl(InformacionPromocionIncumplida info);
    }
}
