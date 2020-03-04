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
    public partial class FrmCamera : Form
    {
        public FrmCamera()
        {
            InitializeComponent();
        }
        public FrmCamera(DataGridViewRow row)
        {
            InitializeComponent();
            editRow = row;
        }

        public delegate void ModifyDgvHandle(string ip, bool isPrimary, int screenType, string screenIP,DataGridViewRow row);
        public ModifyDgvHandle ModifyDgvDele;
        private DataGridViewRow editRow = null;
        private void FrmCamera_Load(object sender, EventArgs e)
        {
            int mode = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Mode"]);
            if (mode == 2)
            {
                label3.Visible = false;
                cboScreenType.Visible = false;
                label4.Visible = false;
                txtScreenIP.Visible = false;
            }

            cboScreenType.Items.Add("普通");
            cboScreenType.Items.Add("红门");
            if (editRow == null)
            {
                cboScreenType.SelectedIndex = 0;
                rbtnPrimary.Checked = true;
            }
            else 
            {
                txtIP.Text = editRow.Cells[1].Value.ToString();
                if (mode == 1) 
                {
                    txtScreenIP.Text = editRow.Cells[3].Value.ToString();
                    cboScreenType.Text = editRow.Cells[2].Value.ToString();
                }

                if (Convert.ToBoolean(editRow.Cells[0].Value))
                    rbtnPrimary.Checked = true;
                else
                    rbtnSecondary.Checked = true;
              
            }

        }

        private void cboScreenType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIP.Text)) 
            {
                MessageBox.Show("请输入摄像机IP地址","提示");
                return;
            }
            if (  txtScreenIP.Visible &&string.IsNullOrEmpty(txtScreenIP.Text)) 
            {
                MessageBox.Show("请输入屏幕IP地址","提示");
                return;
            }
            string ip=txtIP.Text;
            string screenip=txtScreenIP.Text;
            this.Close();
            this.Dispose();
            if (ModifyDgvDele != null)
                ModifyDgvDele(ip, rbtnPrimary.Checked, cboScreenType.SelectedIndex + 1, screenip, editRow);
        }
    }
}
