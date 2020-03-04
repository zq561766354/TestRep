using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParkSystemWinForms.Model;

namespace ParkSystemWinForms
{
    public partial class SuccessionForm : Form
    {
        private static LogHelper log = LogFactory.GetLogger("PsFormLog");
        private string workNo;
        private string empName;
        private string userId;

        public delegate void LoginHandler(string empName, int userId, int empId, ChargeOnDutyModel mode);
        public LoginHandler HandleLogin;
        public SuccessionForm()
        {
            InitializeComponent();

            workNo = Params.Duty.WorkNo;
            empName = Params.User.Name;
            userId = Params.User.Id;
            WorkNolabel.Text = workNo; 
            UserNamelabel.Text = empName;
            StartWorkTimelabel.Text = Params.Duty.StartWorkTime.ToString("yyyy-MM-dd hh:mm:ss");
            EndWorkTimelabel.Text = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
        public SuccessionForm( string EmpName, string UserId, string WorkNo, string StartWorkTime)
        {
            InitializeComponent();
            workNo=WorkNo;
            empName = EmpName;
            userId = UserId;
            WorkNolabel.Text = WorkNo;
            UserNamelabel.Text = EmpName;
            StartWorkTimelabel.Text = StartWorkTime;
            EndWorkTimelabel.Text = System.DateTime.Now.ToString();
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string loginMsg = string.Empty;
            LoginControl login = new LoginControl();
            string username = userNametextBox.Text.ToString();
            string password = passWordtextBox.Text.ToString();
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
                    int userIdNow = Convert.ToInt32(ds.Tables[0].Rows[0]["UserId"]);
                    int empId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"]);
                    if (userIdNow == Convert.ToInt32(Params.User.Id))
                    {
                        MessageBox.Show("请输入其他账户进行交接班！");
                        return;
                    }

                    string empName = Convert.ToString(ds.Tables[0].Rows[0]["EmpName"]);
                    ChargeOnDutyModel modereturn = login.SaveChargeOnDuty(2, Convert.ToInt32(userId), workNo);
                    if (modereturn.returnResult != 1000)
                        MessageBox.Show("交接班上传数据保存失败，请联系管理员！");

                    ChargeOnDutyModel mode = login.SaveChargeOnDuty(1, userIdNow, "");

                    //2019-02-15和LoginForm一样处理
                    if (HandleLogin != null)
                        HandleLogin(empName, userIdNow, empId, mode);

                    this.Dispose();
                }
            }
        }
        ParkSystemBLL psBll = new ParkSystemBLL();
        private void SuccessionForm_Load(object sender, EventArgs e)
        {
            //string x = psBll.AccessRules();
            //if (x != "同步保存成功")
            //    MessageBox.Show(x);

            //log.Debug("交接班用户同步结果：" + x);
        }

      
    }
}
