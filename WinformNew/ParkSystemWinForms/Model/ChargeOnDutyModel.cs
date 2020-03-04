using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSystemWinForms.Model
{
    public class ChargeOnDutyModel
    {
        public string WorkNo { get; set; }
        public DateTime StartWorkTime { get; set; }
        public int ParkingLotId { get; set; }
        public DateTime EndWorkTime { get; set; }
        public double  CashAmount { get; set; }
        public string UserName { get; set; }
        public string ChargeEmpName { get; set; }
        public string Note { get; set; }
        public int State { get; set; }


        public int result { get; set; }
        public int returnResult { get; set; }
        public string  returnstr { get; set; }
        
    }
}
