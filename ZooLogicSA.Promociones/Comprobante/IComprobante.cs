using System.Collections.Generic;
using System.Xml;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones.Comprobante
{
    public interface IComprobante
    {
        string Hash { get; set; }
        void Cargar( string xml );
        void CargarPreciosAdicionales(string xml);
        XmlDocument ObtenerXml();
        XmlDocument ObtenerXmlOriginal();
        XmlDocument ObtenerXmlPreciosAdicionales();

        IParticipante ObtenerNodoParticipante( string clave, string id, string promo, string beneficio );
        List<IParticipante> ObtenerParticipantesSegunClave( string claveParticipante );
        List<IParticipante> ObtenerParticipantesSegunCondicionDeReglas( ParticipanteRegla participante, int regla );
    }
}