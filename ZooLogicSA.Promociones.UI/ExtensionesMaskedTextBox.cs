using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZooLogicSA.Promociones.UI
{
    public static class ExtensionesMaskedTextBox
    {
        public static string ObtenerValor(this MaskedTextBox control)
        {
            MaskFormat mascaraOriginal = control.TextMaskFormat;
            control.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            string valor = control.Text;
            control.TextMaskFormat = mascaraOriginal;
            return valor;
        }
    }
}