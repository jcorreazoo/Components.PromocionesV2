using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ZooLogicSA.Promociones.Asistente
{
    public class PromocionesEstado
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
        public float Beneficio { get; set; }
        public estado Estado { get; set; }
        public Bitmap EstadoDibujo { get; set; }
        public string LeyendaCliente { get; set; }
        public bool Destacada { get; set; }
        public string Faltante { get; set; }
        public string FaltanteCompleto { get; set; }
        public bool Automatica { get; set; }
    }
}
