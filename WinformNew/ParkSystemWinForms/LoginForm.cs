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
using ParkSystemWinForms.Model;

namespace ParkSystemWinForms
{
    public partial class LoginForm : Form
    {
        private static LogHelper log = LogFactory.GetLogger("PsFormLog");
        public delegate void LoginHandler(string empName, int userId, int empId, ChargeOnDutyModel mode);
        public LoginHandler HandleLogin;
        public LoginForm()
        {
            InitializeComponent();
        }

        ParkSystemBLL psBll = new ParkSystemBLL();
        private void LoginForm_Load(object sender, EventArgs e)
        {
            //string x = psBll.AccessRules();
            //if (x != "同步保存成功")
            //    MessageBox.Show(x);

            //log.Debug("登陆用户同步结果：" + x);

        }

        private void login_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(usernameInput.Text))
            {
                usernameInput.Focus();
                return;
            }
            if (string.IsNullOrEmpty(passwordInput.Text))
            {
                passwordInput.Focus();
                return;
            }
            string loginMsg = string.Empty;
            LoginControl login = new LoginControl();
            string username = usernameInput.Text.ToString();
            string password = passwordInput.Text.ToString();
            DataSet ds = login.Login(username, password);
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 0)
                {
                    loginMsg = "账户不存在或密码错误"; //账户不存在或密码错误
                    MessageBox.Show(loginMsg);
                }
                else
                {
                    int userId = Convert.ToInt32(ds.Tables[0].Rows[0]["UserId"]);
                    int empId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"]);
                    string empName = Convert.ToString(ds.Tables[0].Rows[0]["EmpName"]);
                    ChargeOnDutyModel mode = login.SaveChargeOnDuty(1, userId, "");


                    //2019-02-14由固定传递Form1改为委托实现
                    if (HandleLogin != null)
                        HandleLogin(empName, userId, empId, mode);
                    this.Dispose();
                }
            }

        }
    }
}
