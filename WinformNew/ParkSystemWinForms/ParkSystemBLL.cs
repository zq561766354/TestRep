using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using ParkSystemWinForms.Model;

namespace ParkSystemWinForms
{
   public class ParkSystemBLL
    {
        private static   string dirPath = ""; //程序运行目录
        private string cfgFile = "";
        ParkSystemDAL PDAL = new ParkSystemDAL();
        public ParkSystemBLL()
        {
            string path = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            dirPath = System.IO.Path.GetDirectoryName(path);
            cfgFile = dirPath + "\\config.dat";
        }
      
       #region 保存数据
       public  OrderModel SaveOrder(int dataType, string pcColor, string LicensePlateNo, string PictureAddr, string PictureName, string tmpPictureAddr, string tmpPictureName, string pb, DateTime dateTime,int memberId, int netWorkResult,string RoadRateNo)
       {
            OrderModel mode = new OrderModel();
            OrderModel modeResult = new OrderModel();
            string LicensePlateType = "1";
            switch (pcColor)
            {
                case "蓝色":
                    LicensePlateType = "1";
                    break;
                case "黄色":
                    LicensePlateType = "2";
                    break;
                case "黑色":
                    LicensePlateType = "3";
                    break;
                case "白绿色":
                    LicensePlateType = "4";
                    break;
                case "黄绿色":
                    LicensePlateType = "5";
                    break;
                case "绿色":
                    LicensePlateType = "6";
                    break;
                case "白色":
                    LicensePlateType = "7";
                    break;
                default:
                    LicensePlateType = "8";//8是不在识别的颜色中
                    break;
            }
            //int CarNumLength = LicensePlateNo.Length;
            //if (CarNumLength == 8)
            //{
            //    LicensePlateType = "4";
            //}
            mode.MemberId = memberId;
            mode.InDate = dateTime;
            mode.OutDate = dateTime;
            mode.LicensePlateType = Convert.ToInt32(LicensePlateType);
            mode.LicensePlateNo = LicensePlateNo;
            mode.PictureAddr = PictureAddr;
            mode.PictureName = PictureName;
            mode.TmpPictureAddr = tmpPictureAddr;
            mode.TmpPictureName = tmpPictureName;
            mode.Pb = pb;
            mode.RoadRateNo = RoadRateNo;
            modeResult = PDAL.SaveOrder(dataType, mode, netWorkResult);
            return modeResult;
        }
       #endregion
       #region 查会员id
       public  int GetMemberId( string LicensePlateNo)
       {
           int memberid = PDAL.GetMemberId(LicensePlateNo);
           // string URL = ConfigurationManager.AppSettings["Host_URL"];
           // int memberid =Convert.ToInt32(WebApiHelper.HttpPost("http://localhost:2545/api/WinForms/PostGenMemberId", "{licensePlateNo:\"" + LicensePlateNo + "\"}"));
           // memberid = Convert.ToInt32(WebApiHelper.HttpPost(URL+"/api/WinForms/PostGenMemberId", "{licensePlateNo:\"" + LicensePlateNo + "\"}"));
           return memberid;
       }
       #endregion
       #region 查车辆图片
       public  string Getpic(string licensePlateNo)
       {
           string picPath ="";
           int dataType = 4;
          // int orderId = 0;
           DataTable dt = PDAL.GetOrderTable(dataType, licensePlateNo, null);
           if (dt.Rows.Count > 0 && dt != null)
           {
               string PictureAddr = dt.Rows[0]["PictureAddr"].ToString();
               string PictureName = dt.Rows[0]["PictureName"].ToString();
               picPath = PictureAddr + @"\" + PictureName;
           }
           return picPath;
       }
       #endregion
       #region 查车辆图片列表
       public  List<PicNum> GetpicList(string keywords)
       {
           List<PicNum> picList= new List<PicNum>();

           DataTable dt = PDAL.GetOrderTable(3,keywords,null);
           if (dt.Rows.Count > 0 && dt != null)
           {
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   PicNum p = new PicNum();
                   string picPath = null;
                   string PictureAddr = dt.Rows[i]["PictureAddr"].ToString();
                   string PictureName = dt.Rows[i]["PictureName"].ToString();
                   string[] sArray = PictureName.Split('_');
                   string licensePlateNo = sArray[1];
                   picPath = PictureAddr + @"\" + PictureName;
                   p.picPath = picPath;
                   p.OrderNo = dt.Rows[i]["OrderNo"].ToString();
                   p.LicensePlateNo = licensePlateNo;
                   picList.Add(p);
               }
              
           }
           return picList;
       }
       #endregion
       #region 查订单详细信息
       public  OrderModel GetOrderDetail(string OrderNo)
       {
           OrderModel orderModel = new OrderModel();

           DataTable dt = PDAL.GetOrderTable(2,null,OrderNo);
           if (dt.Rows.Count > 0 && dt != null)
           {
               orderModel.LicensePlateNo = dt.Rows[0]["LicensePlateNo"].ToString();
               orderModel.InDate = Convert.ToDateTime(dt.Rows[0]["InDate"]);
               orderModel.OrderNo = dt.Rows[0]["OrderNo"].ToString();
               orderModel.OrderId = Convert.ToInt32(dt.Rows[0]["OrderId"]);
               string PictureAddr = dt.Rows[0]["PictureAddr"].ToString();
               string PictureName = dt.Rows[0]["PictureName"].ToString();
               string picPath = PictureAddr + @"\" + PictureName;
               orderModel.picPath = picPath;
           }
           return orderModel;
       }
       #endregion
       #region 更新订单车牌
       public  string  UpdateLicensePlateNo(string licensePlateNo,string orderNo)
       {
           ParkSystemUtility.log.Debug("Bll.updateLicensePlateNo，in。licensePlateNo:" + licensePlateNo + ",orderNo:" + orderNo);
           string returnVal = String.Empty;
           int dataType = 4;
           returnVal = PDAL.updateLicensePlateNo(dataType, licensePlateNo, orderNo);
           ParkSystemUtility.log.Debug("returnVal：" + returnVal);
           return returnVal;
       }
       #endregion
       #region 更新订单状态
       public OrderModel UpdateState(int dataType, OrderModel mode, int netWorkResult)
       {
           OrderModel modeResult = new OrderModel();
           modeResult = PDAL.updateState(dataType, mode,netWorkResult);
           return modeResult;
       }
       #endregion
       #region 查询场内车辆---查订单列表BY LicensePlateNo
       public  List<OrderModel> GetOrderList(string LicensePlateNo)
       {
          
           List<OrderModel> orderList = new List<OrderModel>();
           DataTable dt = PDAL.GetOrderTable(6,LicensePlateNo,null);
           if (dt != null && dt.Rows.Count > 0)
           {
               for (int i = 0; i < dt.Rows.Count; i++)
               {

                   OrderModel orderModel = new OrderModel();
                   orderModel.LicensePlateNo = dt.Rows[i]["LicensePlateNo"].ToString();
                   orderModel.InDate = Convert.ToDateTime(dt.Rows[i]["InDate"]);
                   orderModel.State = Convert.ToInt32(dt.Rows[i]["State"]);
                   orderModel.OrderNo = dt.Rows[i]["OrderNo"].ToString();
                   int LicensePlateType = Convert.ToInt32(dt.Rows[i]["LicensePlateType"]); 
                   int ChargeType = Convert.ToInt32(dt.Rows[i]["ChargeType"]);
                    string ChargeTypeDes = String.Empty;
                    //string PictureAddr = dt.Rows[i]["PictureAddr"].ToString();
                    //string PictureName = dt.Rows[i]["PictureName"].ToString();
                    //orderModel.picPath = PictureAddr + @"\" + PictureName;

                    string color = "";
                   switch (LicensePlateType)
                   {
                       case 1:
                           color = "蓝色";
                           break;
                       case 2:
                           color = "黄色";
                           break;
                       case 3:
                           color = "黑色";
                           break;
                       case 4:
                           color = "白绿色";
                           break;
                       case 5:
                           color = "黄绿色";
                           break;
                       case 6:
                           color = "绿色";
                           break;
                       case 7:
                           color = "白色";
                           break;
                       default:
                           color = "未识别";//8是不在识别的颜色中
                           break;
                   }
                    switch (ChargeType)
                    {
                        case 10:
                            ChargeTypeDes = "收费车辆";
                            break;
                        case 20:
                            ChargeTypeDes = "白名单车辆";
                            break;
                        case 30:
                            ChargeTypeDes = "包月车辆";
                            break;
                    }
                    orderModel.CarColor = color;
                   orderModel.StateDes = dt.Rows[i]["StateDes"].ToString();
                    orderModel.ChargeTypeDes = ChargeTypeDes;
                    orderList.Add(orderModel);
               }
                   
           }
           return orderList;
       }
       #endregion
       #region 查询场内车辆---查车辆的图片
       public List<OrderModel> GetOrderPicList(string orderNo)
       {

           List<OrderModel> orderList = new List<OrderModel>();
           DataTable dt = PDAL.GetOrderTable(5, null, orderNo);
           if (dt.Rows.Count > 0 && dt != null)
           {
               for (int i = 0; i < dt.Rows.Count; i++)
               {
                   OrderModel orderModel = new OrderModel();
                   string PictureAddr = dt.Rows[i]["PictureAddr"].ToString();
                   string PictureName = dt.Rows[i]["PictureName"].ToString();
                   int SizeMode =Convert.ToInt32(dt.Rows[i]["SizeMode"]);
                   orderModel.picPath = PictureAddr + @"\" + PictureName;
                   orderModel.State = SizeMode;
                   orderList.Add(orderModel);
               }

           }
           return orderList;
       }
       #endregion
       #region 删除异常订单
       public string deleteOrder(string orderNoStr, int dataType, int chargeEmp)
       {
           string returnVal = String.Empty;

           returnVal = PDAL.deleteOrder(dataType, orderNoStr, chargeEmp);

           return returnVal;
       }
       #endregion
       #region 获取平台规则
       public string AccessRules()
       {
           string returnVal = String.Empty;
           returnVal= PDAL.AccessRulesDAL();
           return returnVal;
       }
       #endregion
       #region 保存当班收费纪录表
       public ChargeOnDutyModel SaveChargeRecord(string orderNo, double amount, string workNo)
       {
           ChargeOnDutyModel mode = new ChargeOnDutyModel();
           string returnVal = String.Empty;
           mode = PDAL.SaveChargeRecord(orderNo, amount, workNo);
           return mode;
       }
       #endregion
       #region 获取订单平台状态
       public OrderReturn GetOrderState(string orderNo)
       {
           OrderReturn mode = new OrderReturn();
           string returnVal = String.Empty;
           mode = PDAL.GetOrderState(orderNo);
           return mode;
       }
       #endregion
       #region 更新本地会员Id
       public int UpdateMember(string licensePlateNo, int memberId)
       {
           int result = 0;//1000是更新成功0是失败
           string returnVal = String.Empty;
           result = PDAL.UpdateMember(licensePlateNo, memberId);
           return result;
       }
       #endregion
       #region 心跳包网络情况
       public int NetworkHead()
       {
           int result = 0;//1000是更新成功0是失败
           string returnVal = String.Empty;
           result = PDAL.NetworkHead();
           if (Params.Settings.IsUpdateBerthNum)  //如果勾选更新泊位数
               PDAL.UpdateRemoteParkingLotBerthNum(Convert.ToInt32(Params.Settings.ParkLot.Id));
           return result;
       }
       #endregion
       #region 读取余位数
       public int getLeftCount()
       {
           int result = 0;//1000是更新成功0是失败
           string returnVal = String.Empty;
           result = PDAL.getLeftCount();
           return result;
       }
        #endregion
       #region 是否包月车
       public int IsLicensePlateIsMonthly(string licensePlateNo)
       {
           int result = 0;//剩余包月天数
           int dataType = 1;
           string returnVal = String.Empty;
           result = PDAL.IsLicensePlateIsMonthly(dataType, licensePlateNo);
           return result;
       }
       #endregion
       #region 黄牌车是否重复收费
       //dataType---1是黄牌车不用重复收费并结束订单，2是免费时间端抬杆结束订单
       public int IsReCalculation(int dataType, string licensePlateNo, string roadRateNo, int networkOk)
       {
           int result = 1;//0是结束1是要往下走流程
           //int dataType = 1;
           string returnVal = String.Empty;
           result = PDAL.IsReCalculation(dataType, licensePlateNo, roadRateNo, networkOk);
           return result;
       }
       #endregion
    
       #region 读写配置文件
       public void LoadConfig(ref Setting setting)
        {
            //读取异常,将加载系统默认设置
            if (!System.IO.File.Exists(cfgFile) ||setting==null)
            {
                setting = new Setting();
                setting.ParkLot = new ParkingLot();
                setting.ParkLot.Id = "0";
                setting.ParkLot.No = "0";
                setting.ParkLot.Name = "未命名";
                setting.ParkLot.Addr = "";

                Guard guard1 = new Guard();
                guard1.IsExit = false;
                guard1.No = "默认";
                guard1.Primary = new GuardItem() { IP = "192.168.1.101", ScreenType = 1, ScreenIP = "192.168.1.101" };


                Guard guard2 = new Guard();
                guard2.IsExit = true;
                guard2.No = "默认";
                guard2.Primary = new GuardItem() { IP = "192.168.1.102", ScreenType = 1, ScreenIP = "192.168.1.102" };
                setting.Guards = new List<Guard>() {guard1,guard2 };
                setting.EnabledShowLeftCount = false;
                setting.EnabledHotel = false;
                setting.EnabledWhiteListNoOrder = false;
                setting.EnabledWhiteList = false;
                setting.EnabledWLGO = false;
                setting.ImagePath = @"D:\";
                setting.InVolume = 10;
                setting.OutVolume = 10;
                setting.InDelay = 3000;
                setting.OutDelay = 3000;
                setting.InLine = 4;
                setting.OutLine = 4;
                setting.ScreenInDelay = 80;
                setting.ScreenOutDelay = 80;
                setting.EnableOneSpaceMoreCars = false;

                setting.Serv = new Server()
                {
                    HeartBeatFreq = 3,
                    Url = "http://47.100.229.60:8080",   
                    HeartBeatMaxRetryCount = 5,
                    PayCheckFreq = 5000,
                    PayCheckMaxRetryCount = 25,
                    GateKeepOpenFreq = 3000,
                    DataSyncFreq = 600000
                };


                SaveConfig(setting);
            }
            else
            {
                setting=SerOrDerModel<Setting>.Deserialize(cfgFile);
            }

        }
        public void SaveConfig(Setting setting)
        {
            if (setting == null||string.IsNullOrEmpty(cfgFile))
                return;
            SerOrDerModel<Setting>.Serialize(setting, cfgFile);
        }
        #endregion

        #region 小中大型车计费
        /// <summary>
        /// 小中大型车计费
        /// </summary>
        /// <param name="DataType">--1 修改计算收费金额</param>
        /// <param name="OrderNo">订单编号ID</param>
        /// <param name="CarType" --1=小型车，2=大型车，3=中型车</param>
        /// <returns></returns>
        public OrderModel ReCalculationMediumLarge(string orderNo, int dataType, int carType, int networkOk)
        {
            OrderModel mode = new OrderModel();
            mode = PDAL.ReCalculationMediumLarge(dataType, orderNo, carType, networkOk);
            return mode;
        }
        #endregion

        #region 通过停车场编号读取停车场信息
        public DataTable GetParkingLotInfo(int dataType, string parkingLotNo)
        {   
            DataTable dt = new DataTable();
            dt = PDAL.GetParkingLotInfo(dataType, parkingLotNo);
            return dt;
        }
        #endregion

        #region 检查车位是否已占用
        public int CheckBerthNoOccupy(string licensePlateNo)
        {
            int result = 0;
            result = PDAL.CheckBerthNoOccupy(licensePlateNo);
            return result;
        }
        #endregion

        #region 判断车辆是否白名单
        public int CheckWhilteList(string licensePlateNo)
        {
            int result = 0;//0不是，1是
            int dataType = 1;
            string returnVal = String.Empty;
            result = PDAL.CheckWhilteList(dataType, licensePlateNo);
            return result;
        }
        #endregion

        #region 判断车辆是否包月车
        public int CheckMonthly(string licensePlateNo)
        {
            int result = 0;//0不是，1是
            string returnVal = String.Empty;
            result = PDAL.CheckMonthly(licensePlateNo);
            return result;
        }
        #endregion

        #region 查询车辆白名单或者包月车(天铭酒店)
        public void checkWhiteMonthly(string licensePlateNo, int networkOk)
        {

            PDAL.CheckWhiteMonthly(licensePlateNo, networkOk);
           
        }
        #endregion

        #region 续传订单
        /// <summary>
        /// 续传订单
        /// </summary>
        /// <param name="netWorkResult">平台通讯状态</param>
        /// <returns></returns>
        public void OrderRenewal(int netWorkResult)
        {
            //网络通的情况续传订单
            if(netWorkResult == 1)
                PDAL.OrderRenewal(netWorkResult);
        }
        #endregion

        #region 读取出场未识别的订单
        /// <summary>
        /// 读取出场未识别的订单
        /// </summary>
        /// <param name="parkingLotId">停车场Id</param>
        /// <returns></returns>
        public string GetUnidentifiedOrder(int parkingLotId)
        {
            string unidentifiedOrderStr = String.Empty;
            DataTable dtUnidentifiedOrder = PDAL.GetUnidentifiedOrder(parkingLotId);
            try
            {
                if (dtUnidentifiedOrder != null && dtUnidentifiedOrder.Rows.Count > 0)
                {
                    foreach (DataRow row in dtUnidentifiedOrder.Rows)
                    {
                        int minuteSpan = Convert.ToInt32(row["MinuteSpan"]);
                        int unidentifiedTimerMin = Params.Settings.UnidentifiedTimerMin;
                        ParkSystemUtility.log.Info("minuteSpan：" + minuteSpan.ToString() + " unidentifiedTimerMin: " + unidentifiedTimerMin.ToString());
                        //判断拍到照片和当前时间相比超过清理时间才清理
                        if (minuteSpan >= unidentifiedTimerMin)
                        {
                            if (unidentifiedOrderStr == String.Empty)
                                unidentifiedOrderStr = row["OrderNo"].ToString();
                            else
                                unidentifiedOrderStr = unidentifiedOrderStr + "," + row["OrderNo"].ToString();
                        }
                    }
                }
            }catch(Exception ex)
            {
                ParkSystemUtility.log.Error("ParkSystemBLL.GetUnidentifiedOrder方法发生异常：" + ex.Message);
            }
            return unidentifiedOrderStr;
        }
        #endregion



        //FrmMain2业务逻辑//
        #region 保存数据
        public void SaveOrder_count(int dataType, string pcColor, string LicensePlateNo ,DateTime dateTime, string RoadRateNo)
        {
            OrderCountModel mode = new OrderCountModel();
            string LicensePlateType = "1";
            switch (pcColor)
            {
                case "蓝色":
                    LicensePlateType = "1";
                    break;
                case "黄色":
                    LicensePlateType = "2";
                    break;
                case "黑色":
                    LicensePlateType = "3";
                    break;
                case "白绿色":
                    LicensePlateType = "4";
                    break;
                case "黄绿色":
                    LicensePlateType = "5";
                    break;
                case "绿色":
                    LicensePlateType = "6";
                    break;
                case "白色":
                    LicensePlateType = "7";
                    break;
                default:
                    LicensePlateType = "8";//8是不在识别的颜色中
                    break;
            }
            
          
            mode.LicensePlateType = Convert.ToInt32(LicensePlateType);
            mode.LicensePlateNo = LicensePlateNo;
          if(dataType==1){
              mode.EntranceNo = RoadRateNo;
              mode.InDate = dateTime;
          }
          else
          {
              mode.RoadRateNo = RoadRateNo;
              mode.OutDate = dateTime;
          }
           
           //int Result =
               PDAL.SaveOrder_Count(dataType, mode);
            //return Result;
        }
        #endregion
    }
}
