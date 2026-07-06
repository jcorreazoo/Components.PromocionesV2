using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering;

namespace ZooLogicSA.Promociones.UI
{
    public class ZooFilterControl : FilterControl
    {
        public new string FilterString
        {
            get { return base.FilterString; }
            set { base.FocusInfo = new FilterControlFocusInfo();  base.FilterString = value; }
        }
    }
}
