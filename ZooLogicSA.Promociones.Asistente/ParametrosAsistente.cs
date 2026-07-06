using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZooLogicSA.Core.Escalares;

namespace ZooLogicSA.Promociones.Asistente
{
    public class ParametrosAsistente
    {
        public InformacionAplicacion InformacionAplicacion { get; set; }
        public int TopComprobante { get; set; }
        public int LeftComprobante { get; set; }
        public int HeightComprobante { get; set; }
        public int WidthComprobante { get; set; }
    }
}
