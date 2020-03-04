using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkSystemWinForms
{
    public class AutoSizeForm
    {
        //(1).声明结构,只记录窗体和其控件的初始位置和大小。  
   
        public struct controlRect
        {
            public int Left;
            public int Top;
            public int Width;
            public int Height;
        }

        private bool _Flag;
        public bool Flag
        {
            get { return _Flag; }
            set { _Flag = value; }
        }

        private int _Number;
        public int Number
        {
            get { return _Number; }
            set { _Number = value; }
        }

        private List<controlRect> oldCtrl;

        public void Initialize(Form1 mForm)
        {
            oldCtrl = new List<controlRect>();
            controlRect cR;

            cR.Left = mForm.Left;
            cR.Top = mForm.Top;
            cR.Width = mForm.Width;
            cR.Height = mForm.Height;

            oldCtrl.Add(cR);

            foreach (Control c in mForm.Controls)
            {
                controlRect objCtrl;
                objCtrl.Left = c.Left;
                objCtrl.Top = c.Top;
                objCtrl.Width = c.Width;
                objCtrl.Height = c.Height;
                oldCtrl.Add(objCtrl);
            }
            Flag = true;
            Number = mForm.Controls.Count;
        }

        public void ReSize(Form1 mForm)
        {
            if (!Flag) return;

            float wScale = (float)mForm.Width / (float)oldCtrl[0].Width;
            float hScale = (float)mForm.Height / (float)oldCtrl[0].Height;

            int ctrLeft0, ctrTop0, ctrWidth0, ctrHeight0;
            int ctrlNo = 1;

            try
            {
                if (mForm.Controls.Count != Number) return;
                foreach (Control c in mForm.Controls)
                {
                    ctrLeft0 = oldCtrl[ctrlNo].Left;
                    ctrTop0 = oldCtrl[ctrlNo].Top;
                    ctrWidth0 = oldCtrl[ctrlNo].Width;
                    ctrHeight0 = oldCtrl[ctrlNo].Height;

                    c.Left = (int)(ctrLeft0 * wScale);
                    c.Top = (int)(ctrTop0 * hScale);
                    c.Width = (int)(ctrWidth0 * wScale);
                    c.Height = (int)(ctrHeight0 * hScale);
                    ctrlNo += 1;
                }
            }
            catch
            {
                return;
            }
        }

    }
}
