using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using ZooLogicSA.Promociones.FormatoPromociones;

namespace ZooLogicSA.Promociones
{
    public class Serializador
    {
        public string Serializar<T>( T valor )
        {
            string retorno;

            TextWriter salida = new StringWriter();
            XmlSerializer serializador = new XmlSerializer( typeof( T ) );
            serializador.Serialize( salida, valor );
            retorno = salida.ToString();
            salida.Dispose();

            return retorno;
        }

        public T DeSerializar<T>( string xml )
        {
            StringReader salida = new StringReader( xml );
            XmlSerializer serializador = new XmlSerializer( typeof( T ) );
            T retorno = (T)serializador.Deserialize( salida );
            salida.Dispose();

            return retorno;
        }

        public Promocion DeserializarPromocion( string xml )
        {
            Promocion retorno = this.DeserializarPromocion("", xml);
            retorno.Tipo = this.ObtenerTipo(retorno.InformacionControl);
            return retorno;
        }

        public Promocion DeserializarPromocion( string descripcion, string xml )
        {
            Promocion retorno = this.DeSerializar<Promocion>( xml );
            retorno.Descripcion = descripcion;
            retorno.Tipo = this.ObtenerTipo(retorno.InformacionControl);
            return retorno;
        }

        public Promocion DeserializarPromocion(string descripcion, string redondeo, string xml)
        {
            Promocion retorno = this.DeSerializar<Promocion>(xml);
            retorno.Descripcion = descripcion;
            retorno.Redondeo = redondeo;
            retorno.Tipo = this.ObtenerTipo(retorno.InformacionControl);
            return retorno;
        }
        public Promocion DeserializarPromocion(DateTime fechaModificacion, string horaModificacion, string descripcion, string redondeo, string xml)
        {
            Promocion retorno = this.DeSerializar<Promocion>(xml);
            retorno.Descripcion = descripcion;
            retorno.Redondeo = redondeo;
            retorno.FechaModificacion = fechaModificacion;
            retorno.HoraModificacion = horaModificacion.Replace(":", "");
            retorno.Tipo = this.ObtenerTipo(retorno.InformacionControl);
            return retorno;
        }
        public Promocion ObtenerPromocionDeserializada(string descripcion, string redondeo, string xml, DateTime fechaModificacion, string horaModificacion, bool Automatica, UInt16 Cuotas)
        {
            Promocion retorno = this.DeSerializar<Promocion>(xml);
            retorno.Descripcion = descripcion;
            retorno.Redondeo = redondeo;
            retorno.FechaModificacion = fechaModificacion;
            retorno.HoraModificacion = horaModificacion.Replace(":", "");
            retorno.AplicaAutomaticamente = Automatica;
            retorno.CuotasSinRecargo = Cuotas;
            retorno.Tipo = this.ObtenerTipo(retorno.InformacionControl);
            return retorno;
        }

        public string ObtenerTipo(string informacionControl)
        {
            string tipo = "";

            if (informacionControl != null)
                tipo = informacionControl.Split('|').ToList().Where(x => x.Split(';')[0] == "TIPO").Select(x => x.Split(';')[1]).FirstOrDefault() ?? "";

            return tipo;
        }
    }
}