using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace ParkSystemWinForms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void login_Click(object sender, EventArgs e)
        {
            string username = usernameInput.Text.ToString();
            string password = passwordInput.Text.ToString();
            string passwordMd5 = MD5Helper.Md5(password);
            string URL = ConfigurationManager.AppSettings["Host_URL"];
            string json = WebApiHelper.HttpPost(URL + "/Login/AndroidLogin", "{userName:\"" + username + "\",password:\"" + passwordMd5 + "\"}");

        }
    }
}
