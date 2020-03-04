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
    public partial class FrmGuard : Form
    {
        public delegate void ModifyGuardHandle(bool isExit,string no,TreeNode tn);
        public ModifyGuardHandle ModifyGuardDele;

        private TreeNode editNode = null;

        public FrmGuard()
        {
            InitializeComponent();
        }
        public FrmGuard(TreeNode node) 
        {
            InitializeComponent();
            bool isExit = Convert.ToBoolean(node.Tag);
            if (isExit)
                rbtnOut.Checked = true;
            else
                rbtnIn.Checked = true;
            string name = node.Text;
            name = name.Replace("(出)", "").Replace("(入)", "");
            txtNo.Text = name;
            editNode = node;
        }


        private void FrmGuard_Load(object sender, EventArgs e)
        {
            if (editNode == null)
            {
                rbtnIn.Checked = true;
            }
            else 
            {
                bool isExit = Convert.ToBoolean(editNode.Tag);
                if (isExit)
                    rbtnOut.Checked = true;
                else
                    rbtnIn.Checked = true;
            }

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNo.Text))
                return;
            bool isExit = rbtnOut.Checked;
            if (ModifyGuardDele != null)
            {
                this.Close();
                ModifyGuardDele(isExit, txtNo.Text, editNode);
            }
           
        }
    }
}
