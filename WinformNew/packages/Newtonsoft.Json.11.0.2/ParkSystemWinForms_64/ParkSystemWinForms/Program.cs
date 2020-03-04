using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkSystemWinForms
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
           
            //string LicensePlateNo = "苏Es78UJ6";
            //int memberid =Convert.ToInt32( WebApiHelper.HttpPost("http://localhost:2545/api/WinForms/PostGenMemberId", "{licensePlateNo:\"" + LicensePlateNo + "\"}"));
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
