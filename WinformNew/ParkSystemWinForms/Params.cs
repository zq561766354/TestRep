using ParkSystemWinForms.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSystemWinForms
{
    public class Params
    {
        //默认无人值守的用户和员工Id
        public static readonly int noBodyUserId = 9999;
        public static readonly int noBodyEmpId = 9999;

        private static LoginUser _user = new LoginUser();
        public static LoginUser User
        {
            get { return _user; }
            set { _user = value; }
        }

        private static ChargeOnDutyModel _duty = new ChargeOnDutyModel();
        public static ChargeOnDutyModel Duty
        {
            get { return _duty; }
            set { _duty = value; }
        }

        public static Setting Settings { get; set; }


        public static string JsonConfig { get; set; }
        public static Setting2 Settings2 { get; set; }
    }
}
