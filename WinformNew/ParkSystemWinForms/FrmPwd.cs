using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkSystemWinForms
{
    public partial class FrmPwd : Form
    {
        public delegate void ChangeSettingHandle();
        public ChangeSettingHandle ChangeSettingDele;

        public FrmPwd()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPwd.Text))
                return;

            DateTime now = DateTime.Now;
            int year = now.Year;
            int month = now.Month;
            int day = now.Day;
            int dw = (int)now.DayOfWeek;
            string sec=year.ToString().Substring(2,2)+month.ToString().PadLeft(2,'0')+day.ToString().PadLeft(2,'0')+dw;
            if (txtPwd.Text.Equals(sec))
            {
                this.Close();
                this.Dispose();
                int mode=Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Mode"]);
                Form settingForm = null;
                if (mode == 1)
                {
                    settingForm = new FrmSetting();
                    ((FrmSetting)settingForm).ChangeSettingDele = () =>
                    {
                        if (ChangeSettingDele != null)
                            ChangeSettingDele();
                    };
                }
                else if(mode==2)
                {
                    settingForm = new FrmSetting2();
                }
                settingForm.ShowDialog();
            }
            else 
            {
                MessageBox.Show("密码错误");
            }
                
        }

        private void txtPwd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) 
            {
                btnOk_Click(null, EventArgs.Empty);
            }
        }
    }
}
