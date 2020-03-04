using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSystemWinForms.Model
{
    public class OrderModel
    {
        //Order表字段
        public int OrderId;
        public string OrderNo;
        public int OrderType;
        public int ParkingLotId;
        public string BerthNo;
        public int MemberId;
        public string LicensePlateNo;
        public int LicensePlateType;
        public DateTime InDate;
        public DateTime OutDate;
        public int ParkingTime;
        public double OrderCharge;
        public double ActualGetAmount;
        public double ActualAmount;
        public double DiscountAmount;
        public int OrderComplete;
        public DateTime OrderCompleteDate;
        public int State;
        public int ChargeType;
        public string ChargeTypeDes;
        public string Note;
        public int CarType;
        //OrderPayDetail表字段
        public string PayNo;
        public double PayMoney;
        public int PayType;
        public string PayNote;
        public int ChargeEmp;
        public string Paytime;

        //用到的合成字段
        public string picPath;
        public string StateDes;
        public string PictureAddr;
        public string PictureName;
        public string TmpPictureAddr;
        public string TmpPictureName;
        public string Pb;//图片的base64
        public string CarColor;
        public int monthLeftDay;
        public int berthNum;
        public int dataType;

        public double PayDifferenceAmount;
        //double orderCharge, double actualGetAmount, double actualAmount, double discountAmount, int chargeType, string note

        public string RoadRateNo;
        public int result;
        public string resultVal;



    }
}
