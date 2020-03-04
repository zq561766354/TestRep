using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSystemWinForms.Model
{
    public class OrderReturn
    {
        public int result_code;//1001，1002，1003是平台保存失败，1000是平台保存成功，99是调用平台WebApi方法异常
        public string return_msg;
        public int state;
        public int parkingTime;
        public double OrderCharge;
        public double ActualAmount;
        public double DiscountAmount;
        public string Paytime;
        public double PayDifferenceAmount;
        public double ActualGetAmount;
    }
}
