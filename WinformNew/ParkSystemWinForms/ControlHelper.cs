using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkSystemWinForms
{
   public class ControlHelper
    {
        [System.Runtime.InteropServices.DllImport("user32.dll ")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int wndproc);
        [System.Runtime.InteropServices.DllImport("user32.dll ")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public const int GWL_STYLE = -16;
        public const int WS_DISABLED = 0x8000000;

        public static void SetControlEnabled(bool enabled, params Control[] cs)
        {
            if (cs == null)
                return;
            foreach (Control c in cs)
            {
                if (enabled)
                    SetWindowLong(c.Handle, GWL_STYLE, (~WS_DISABLED) & GetWindowLong(c.Handle, GWL_STYLE));
                else
                    SetWindowLong(c.Handle, GWL_STYLE, WS_DISABLED + GetWindowLong(c.Handle, GWL_STYLE));
            }
        }
    }
}
