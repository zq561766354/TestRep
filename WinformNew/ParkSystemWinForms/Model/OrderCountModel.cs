using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSystemWinForms.Model
{
    public class OrderCountModel
    {
        public string OrderNo;
        public int ParkingLotId;
        public string LicensePlateNo;
        public int LicensePlateType;
        public DateTime InDate;
        public DateTime OutDate;
        public int ParkingTime;
        public int State;
        public string Note;
        public string EntranceNo;
        public string RoadRateNo;
    }
}
