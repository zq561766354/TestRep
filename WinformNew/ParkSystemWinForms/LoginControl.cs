using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using ParkSystemWinForms.Model;

namespace ParkSystemWinForms
{
    public class LoginControl
    {
        ParkSystemDAL PDAL = new ParkSystemDAL();
        Dictionary<String, Object> paraDir = new Dictionary<String, Object>();
        public DataSet Login(string userName, string passWord)
        {
            
            DataSet ds = PDAL.Login(userName, passWord);
            return ds;
        }
        #region 生成当班纪录
        public ChargeOnDutyModel SaveChargeOnDuty(int dataType, int userId, string workNo)
        {
            string returnVal = String.Empty;

            ChargeOnDutyModel  mode = PDAL.SaveChargeOnDuty(dataType, userId, workNo);

            return mode;
        }
        #endregion
    }
}
