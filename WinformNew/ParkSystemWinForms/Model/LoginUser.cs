using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSystemWinForms.Model
{
    /// <summary>
    /// 登陆账户
    /// </summary>
    public class LoginUser
    {
        /// <summary>
        /// 登陆id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 员工id
        /// </summary>
        public string StaffId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 员工名称
        /// </summary>
        public string Name { get; set; }
    }
}
