using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Configuration;
using ParkSystemWinForms.Model;
using Newtonsoft.Json;


namespace ParkSystemWinForms
{
    public class ParkSystemDAL
    {
        Dictionary<String, Object> paraDir = new Dictionary<String, Object>();
       
        #region 保存数据
        public  OrderModel SaveOrder(int dataType, OrderModel mode)
        {
            int result = 0;
            string returnVal = string.Empty;
            OrderModel modeReturn = new OrderModel();
            string errorMsg = string.Empty;
            string OrderNo = string.Empty;
            int BerthNum = 0;
            int ChargeTypeReturn = 0;
            int MonthLeftDayReturn = 0;

            //int ActualAmount = 0;
            string BerthNo = "";
            //string InDate = dt.Rows[0]["InDate"].ToString();
           // string OrderType = "2";
            string ParkingLotId = ConfigurationManager.AppSettings["ParkingLotId"];
           // string ParkingLotNo = ConfigurationManager.AppSettings["ParkingLotNo"];
           // DataTable dtOrder = new DataTable();
            //生成停车信息DataTable
            DataTable dtAttachment = new DataTable("IPS_Order");
            //todo
           // dtAttachment.Columns.Add("OrderNo", typeof(string));
            dtAttachment.Columns.Add("OrderType", typeof(string));
            dtAttachment.Columns.Add("ParkingLotId", typeof(string));
            //dtAttachment.Columns.Add("ParkingLotNo", typeof(string));

            dtAttachment.Columns.Add("BerthNo", typeof(string));
            dtAttachment.Columns.Add("LicensePlateNo", typeof(string));
            dtAttachment.Columns.Add("LicensePlateType", typeof(int));
            dtAttachment.Columns.Add("MemberId", typeof(int));
            dtAttachment.Columns.Add("InDate", typeof(string));
            dtAttachment.Columns.Add("OutDate", typeof(string));
            // dtAttachment.Columns.Add("OutDate", typeof(string));
            DataTable dtAttachmentOrderPic = new DataTable("IPS_OrderPic");
            dtAttachmentOrderPic.Columns.Add("PictureAddr", typeof(string));
            dtAttachmentOrderPic.Columns.Add("PictureName", typeof(string));
            dtAttachmentOrderPic.Columns.Add("SizeMode", typeof(byte));

            DataRow dataRow = dtAttachment.NewRow();
           // dataRow["OrderNo"] = orderNo;
            dataRow["OrderType"] = 2;
            dataRow["MemberId"] = mode.MemberId;
            dataRow["ParkingLotId"] = ParkingLotId;
          // dataRow["ParkingLotNo"] = ParkingLotNo;
            dataRow["BerthNo"] = BerthNo;
            dataRow["LicensePlateNo"] = mode.LicensePlateNo;
            dataRow["LicensePlateType"] = mode.LicensePlateType;
            dataRow["InDate"] = mode.InDate;
            dataRow["OutDate"] = mode.OutDate;
            dtAttachment.Rows.Add(dataRow);

            DataRow dataRowPic = dtAttachmentOrderPic.NewRow();
            dataRowPic["PictureAddr"] = mode.PictureAddr;
            dataRowPic["PictureName"] = mode.PictureName;
            dataRowPic["SizeMode"] = 1;
            dtAttachmentOrderPic.Rows.Add(dataRowPic);
            DataRow dataRowtmpPic = dtAttachmentOrderPic.NewRow();
            dataRowtmpPic["PictureAddr"] = mode.TmpPictureAddr;
            dataRowtmpPic["PictureName"] = mode.TmpPictureName;
            dataRowtmpPic["SizeMode"] = 2;
            dtAttachmentOrderPic.Rows.Add(dataRowtmpPic);

            result = ParkSystemUtility.SaveData(dataType, dtAttachment, dtAttachmentOrderPic, "IPS_SaveOrder_local", ref errorMsg, ref OrderNo, ref BerthNum, ref ChargeTypeReturn, ref MonthLeftDayReturn);
            if (result == 1000)
            {
                //保存成功
                returnVal = "保存成功";              
                DataTable dt= GetOrderTable(2,"",OrderNo);
                modeReturn.OrderNo = dt.Rows[0]["OrderNo"].ToString();
                modeReturn.InDate = Convert.ToDateTime(dt.Rows[0]["InDate"]);
                modeReturn.ChargeType = Convert.ToInt32(dt.Rows[0]["ChargeType"]);
                try
                {
                    if (dataType == 1)
                    {
                        modeReturn.dataType = 1;
                        modeReturn.OrderType = Convert.ToInt32(dt.Rows[0]["OrderType"]);
                        modeReturn.ParkingLotId = Convert.ToInt32(dt.Rows[0]["ParkingLotId"]);
                        modeReturn.BerthNo = dt.Rows[0]["BerthNo"].ToString();
                        modeReturn.MemberId = Convert.ToInt32(dt.Rows[0]["MemberId"]);
                        modeReturn.LicensePlateNo = dt.Rows[0]["LicensePlateNo"].ToString();
                        modeReturn.LicensePlateType = Convert.ToInt32(dt.Rows[0]["LicensePlateType"]);
                        modeReturn.Pb = mode.Pb;
                        modeReturn.PictureName = mode.PictureName;
                        string modeJson = JsonConvert.SerializeObject(modeReturn);
                        string URL = ConfigurationManager.AppSettings["Host_URL"];
                        string json = WebApiHelper.HttpPost(URL + "/api/WinForms/PostFiles", modeJson);
                        OrderReturn or = JsonHelper.DeserializeObject<OrderReturn>(json);
                        if (or.result_code != 1000)
                            ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法 WebApiHelper：" + mode.LicensePlateNo + " " + or.return_msg);
                    }
                    else if (dataType == 2 && modeReturn.MemberId > 0 && modeReturn.ChargeType == 10 && modeReturn.ActualAmount>0)
                    {
                        modeReturn.dataType = 2;
                        modeReturn.OutDate = Convert.ToDateTime(dt.Rows[0]["OutDate"]);
                        modeReturn.ParkingTime = Convert.ToInt32(dt.Rows[0]["ParkingTime"]);
                        modeReturn.MemberId = Convert.ToInt32(dt.Rows[0]["MemberId"]);
                        modeReturn.OrderCharge = Convert.ToDouble(dt.Rows[0]["OrderCharge"]);
                        modeReturn.ActualAmount = Convert.ToDouble(dt.Rows[0]["ActualAmount"]);
                        modeReturn.DiscountAmount = Convert.ToDouble(dt.Rows[0]["DiscountAmount"]);
                        modeReturn.ChargeType = Convert.ToInt32(dt.Rows[0]["ChargeType"]);
                        string modeJson = JsonConvert.SerializeObject(modeReturn);
                        string URL = ConfigurationManager.AppSettings["Host_URL"];
                        string json = WebApiHelper.HttpPost(URL + "/api/WinForms/PostFiles", modeJson);
                        OrderReturn or = JsonHelper.DeserializeObject<OrderReturn>(json);
                        if (or.result_code != 1000)
                            ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法 WebApiHelper：" + mode.LicensePlateNo + " " + or.return_msg);
                        else
                            modeReturn.State = or.state;
                    }
                }
                catch (Exception ex)
                {
                    ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法 WebApiHelper：" + mode.LicensePlateNo +" " + ex.Message);
                }
                modeReturn.berthNum = BerthNum;
                modeReturn.monthLeftDay = MonthLeftDayReturn;
            }
            else if (result == 1001 || result == 1002)
            {
                //保存失败，数据处理失败，请联系数据管理员
                returnVal = "数据库保存失败，数据处理失败，请联系数据管理员";
                ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法数据库保存失败，数据处理失败，请联系数据管理员" + mode.LicensePlateNo);
            }
            else
            {
                returnVal = "ParkSystemDAL.SaveOrder方法保存失败:ErrorMsg = " + errorMsg;
                ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法保存失败:ErrorMsg ：" +mode.LicensePlateNo + errorMsg);
            }
            //if (result == 1000)
            //{
            //    try
            //    {
            //        string URL = ConfigurationManager.AppSettings["Host_URL"];
            //        string json = WebApiHelper.HttpPost(URL + "/api/WinForms/PostFiles", "{dataType:\"" + dataType + "\",OrderNo:\"" + OrderNo + "\",ActualAmount:\"" + ActualAmount + "\",InDate:\"" + mode.InDate + "\",OutDate:\"" + mode.OutDate + "\",OrderType:\"" + OrderType + "\",ParkingLotId:\"" + ParkingLotId + "\",BerthNo:\"" + BerthNo + "\",LicensePlateNo:\"" + mode.LicensePlateNo + "\",LicensePlateType:\"" + mode.LicensePlateType + "\",pic:\"" + mode.Pb + "\",fileName:\"" + mode.PictureName + "\"}");

            //        or = JsonHelper.DeserializeObject<OrderReturn>(json);
            //        if (or.result_code == 1)
            //        {
            //            string orderNo = or.orderNo;
            //            if (dataType == 2 && or.state == 30)
            //            {
            //                updateState(3, orderNo, or.parkingTime, or.orderCharge, or.actualAmount, or.actualAmount, or.discountAmount, or.chargeType, "");
            //            }
            //        }
            //        else
            //        {
            //            ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法 平台：" + or.return_msg );
            //        }
                  
            //    }
            //    catch (Exception ex)
            //    {
            //        ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法 WebApiHelper："+ex.Message);
            //        or.return_msg = ex.Message;
            //    }
               
            //}
            //else
            //{
            //    ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法 本地保存：" + errorMsg  );
            //    or.result_code = 2;
            //    or.return_msg = errorMsg;
            //}

            return modeReturn;

        }

        #endregion

        #region 修正订单车牌号 BY OrderNo
        public  int updateLicensePlateNo(int dataType, string licensePlateNo, string orderNo)
        {
            string errorMsg = "";
            int result = 1000;//1000是更新成功0是失败
            DataTable dtAttachment = new DataTable("IPS_Order");
            //todo
            // dtAttachment.Columns.Add("OrderNo", typeof(string));
            dtAttachment.Columns.Add("OrderNo", typeof(string));
            dtAttachment.Columns.Add("LicensePlateNo", typeof(string));
            DataRow dataRow = dtAttachment.NewRow();
            dataRow["OrderNo"] = orderNo;
            dataRow["LicensePlateNo"] = licensePlateNo;
            dtAttachment.Rows.Add(dataRow);
            result = ParkSystemUtility.SaveData(dataType, dtAttachment, "IPS_SaveOrder_local", ref errorMsg);
           
            return result;
        }

        #endregion
        #region 更改订单状态 BY OrderNo
        /// <summary>
        /// 保存订单状态 BY OrderNo
        /// </summary>
        /// <param name="dataType">3是更新订单</param>
        /// <returns></returns>
        public OrderModel updateState(int dataType, string orderNo, string payNo, int payType, double payMoney, int chargeEmp, string note, string pb, string pictureName)
        {
            OrderModel mode = new OrderModel();

            int result = 1000;//1000是更新成功0是失败
            string errorMsg = string.Empty;
            string OrderNoReturn = string.Empty;
            int BerthNum = 0;
            int ChargeTypeReturn = 0;
            int MonthLeftDayReturn = 0;
            try
            {
               
                DataTable dtAttachment = new DataTable("IPS_Order");
                dtAttachment.Columns.Add("OrderNo", typeof(string));
                dtAttachment.Columns.Add("Note", typeof(string));
                DataRow dataRow = dtAttachment.NewRow();
                dataRow["OrderNo"] = orderNo;
                dataRow["Note"] = note;
                dtAttachment.Rows.Add(dataRow);
                
                DataTable dtOrderPayDetail = new DataTable("IPS_OrderPayDetail");
                dtOrderPayDetail.Columns.Add("OrderNo", typeof(string));
                dtOrderPayDetail.Columns.Add("PayNo", typeof(string));
                dtOrderPayDetail.Columns.Add("PayType", typeof(int));
                dtOrderPayDetail.Columns.Add("PayMoney", typeof(double));
                dtOrderPayDetail.Columns.Add("ChargeEmp", typeof(int));
                dtOrderPayDetail.Columns.Add("Note", typeof(string));
                 if (payMoney != 0)
                {
                    DataRow dataRowPay = dtOrderPayDetail.NewRow();
                    dataRowPay["OrderNo"] = orderNo;
                    dataRowPay["PayNo"] = payNo;
                    dataRowPay["PayType"] = payType;
                    dataRowPay["PayMoney"] = payMoney;
                    dataRowPay["ChargeEmp"] = chargeEmp;
                    dataRowPay["Note"] = note;
                    dtOrderPayDetail.Rows.Add(dataRowPay);
                }
                 DataTable dt = GetOrderTable(2, "", orderNo);
                 result = ParkSystemUtility.SaveData(dataType, dtAttachment, dtOrderPayDetail, "IPS_SaveOrder_local", ref errorMsg, ref OrderNoReturn, ref BerthNum, ref ChargeTypeReturn, ref MonthLeftDayReturn);
                if (result == 1000)
                {
                    mode.berthNum = BerthNum;
                    //保存成功
                    OrderModel modeReturn = new OrderModel();
                   
                   
                    try
                    {
                        modeReturn.Pb =pb;
                        modeReturn.PictureName = pictureName;
                       
                            modeReturn.dataType = 3;
                            modeReturn.OrderNo = dt.Rows[0]["OrderNo"].ToString();
                            modeReturn.InDate = Convert.ToDateTime(dt.Rows[0]["InDate"]);
                            modeReturn.ChargeType = Convert.ToInt32(dt.Rows[0]["ChargeType"]);
                            modeReturn.OrderType = Convert.ToInt32(dt.Rows[0]["OrderType"]);
                            modeReturn.ParkingLotId = Convert.ToInt32(dt.Rows[0]["ParkingLotId"]);
                            modeReturn.BerthNo = dt.Rows[0]["BerthNo"].ToString();
                            modeReturn.MemberId = Convert.ToInt32(dt.Rows[0]["MemberId"]);
                            modeReturn.LicensePlateNo = dt.Rows[0]["LicensePlateNo"].ToString();
                            modeReturn.LicensePlateType = Convert.ToInt32(dt.Rows[0]["LicensePlateType"]);
                        //
                            modeReturn.OutDate = Convert.ToDateTime(dt.Rows[0]["OutDate"]);
                            modeReturn.ParkingTime = Convert.ToInt32(dt.Rows[0]["ParkingTime"]);
                            modeReturn.OrderCharge = Convert.ToDouble(dt.Rows[0]["OrderCharge"]);
                            modeReturn.ActualAmount = Convert.ToDouble(dt.Rows[0]["ActualAmount"]);
                            modeReturn.ActualGetAmount = Convert.ToDouble(dt.Rows[0]["ActualGetAmount"]);
                            modeReturn.DiscountAmount = Convert.ToDouble(dt.Rows[0]["DiscountAmount"]);
                            modeReturn.OrderComplete = Convert.ToInt32(dt.Rows[0]["OrderComplete"]);
                          //  modeReturn.OrderCompleteDate = Convert.ToDateTime(dt.Rows[0]["OrderCompleteDate"]);
                            modeReturn.State = 30;
                            modeReturn.Note = dt.Rows[0]["State"].ToString();

                            modeReturn.PayNo = payNo;
                            modeReturn.PayMoney = payMoney;
                            modeReturn.PayType = payType;
                            modeReturn.PayNote = note;
                            modeReturn.ChargeEmp = chargeEmp;
                            string modeJson = JsonConvert.SerializeObject(modeReturn);
                            string URL = ConfigurationManager.AppSettings["Host_URL"];
                            string json = WebApiHelper.HttpPost(URL + "/api/WinForms/PostFiles", modeJson);
                            OrderReturn or = JsonHelper.DeserializeObject<OrderReturn>(json);
                            if (or.result_code != 1000)
                                ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法 WebApiHelper：" + mode.LicensePlateNo + " " + or.return_msg);
                        
                    }
                    catch (Exception ex)
                    {
                        ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法 WebApiHelper：" + mode.LicensePlateNo + " " + ex.Message);
                    }
                    modeReturn.berthNum = BerthNum;
                    modeReturn.monthLeftDay = MonthLeftDayReturn;

                }
                else if(result==1001||result==1002)
                    ParkSystemUtility.log.Error("ParkSystemDAL.updateState方法 存储过程异常：" + errorMsg);
                else if(result==2001)
                    ParkSystemUtility.log.Error("ParkSystemDAL.updateState方法 存储过程异常：数据库保存失败，数据处理失败，请联系数据管理员");
            }
            catch (Exception ex)
            {
                ParkSystemUtility.log.Error("ParkSystemDAL.updateState方法 本地更新订单：" + ex.Message);
            }
            return mode;
           // int result = 1;//1是更新成功0是失败
          
        }
        #endregion
        #region 保存数据到平台
        public OrderReturn updateStateToYun(int dataType, OrderModel mode, int payType, int chargeEmp)
        {
            //int berthNum = 0;
            OrderReturn or = new OrderReturn();
            //string URL = ConfigurationManager.AppSettings["Host_URL"];
            //int result=updateState(3, mode.OrderNo, mode.ParkingTime, mode.OrderCharge, mode.ActualGetAmount, mode.ActualAmount, mode.DiscountAmount, mode.ChargeType, mode.Note);
            //if (result == 1000)
            //{
            //    try
            //    {
            //        string json = WebApiHelper.HttpPost(URL + "/api/WinForms/PostFiles", "{dataType:\"" + dataType + "\",OrderNo:\"" + mode.OrderNo + "\",PayMoney:\"" + mode.ActualGetAmount + "\",PayType:\"" + payType + "\",ChargeEmp:\"" + chargeEmp + "\",Note:\"" + mode.Note + "\"}");
            //        or = JsonHelper.DeserializeObject<OrderReturn>(json);
            //       // berthNum = or.berthNum;
            //    }
            //    catch (Exception ex)
            //    {
            //        ParkSystemUtility.log.Error("ParkSystemDAL.updateStateToYun方法 WebApiHelper：" + ex.Message);
            //        or.return_msg = ex.Message;
            //    }
            //}

            return or;
        }

        #endregion
        #region 查订单信息
        
        /// <summary>
        /// 读取数据通用方法
        /// </summary>
        /// <param name="dataType">1：读取订单列表，2：读取详细信息，3：读取订单小图片列表，4：读取订单对应的入场图片</param>
        /// <param name="licensePlateNo">车牌号</param>
        /// <param name="orderId">订单ID</param>
        /// <returns></returns>
        public  DataTable GetOrderTable(int dataType,string licensePlateNo,string orderNo)
        {
            DataTable dtOrder = new DataTable();
            paraDir.Clear();
            paraDir.Add("@DataType", dataType);
            paraDir.Add("@OrderNo", orderNo);
            paraDir.Add("@LicensePlateNo", licensePlateNo);
            DataSet ds = ParkSystemUtility.GetData(paraDir, "IPS_GetOrder_local");
            if (ds.Tables.Count > 0 &&ds!=null)
                return dtOrder=ds.Tables[0];
            else
                return dtOrder;

        }

        #endregion
        
      
        #region 删除订单
        public  string deleteOrder(string  orderIdStr)
        {
            int result = 0;//1000是更新成功0是失败
            string returnVal = String.Empty;
            try
            {
                string errorMsg = String.Empty;
                paraDir.Clear();
                paraDir.Add("@OrderIdStr", orderIdStr);
                result = ParkSystemUtility.SaveData(paraDir, "IPS_DelOrder_local", ref errorMsg);
                if (result == 1000)
                    returnVal = "删除成功";
                else if (result == 1001)
                    returnVal = "删除失败，数据处理失败，请联系数据管理员";
             
                else
                    returnVal = "删除失败:ErrorMsg = " + errorMsg;
            }
            catch (Exception ex)
            {
                ParkSystemUtility.log.Error("ParkSystemDAL.deleteOrder方法：" + ex.Message);
                returnVal = "ParkSystemDAL.deleteOrder方法：" + ex.Message;
            }
            return returnVal;

        }

        #endregion
      
        #region 查会员ID
        public int GetMemberId(string LicensePlateNo)
        {
            int memberid = 0;
            string URL = ConfigurationManager.AppSettings["Host_URL"];
            try
            {
                memberid = Convert.ToInt32(WebApiHelper.HttpPost(URL + "/api/WinForms/PostGenMemberId", "{licensePlateNo:\"" + LicensePlateNo + "\"}"));
            }
            catch (Exception ex)
            {
                ParkSystemUtility.log.Error("ParkSystemDAL.GetMemberId方法：" + ex.Message);
            }
            return memberid;
        }
        #endregion

        #region 获取平台规则
        public string AccessRulesDAL()
        {
            int dataType = 1;
            int result = 0;
            string returnVal = String.Empty;
            string errorMsg = String.Empty;
            List<DataTable> dtChilds = new List<DataTable>();
            string ParkingLotId = ConfigurationManager.AppSettings["ParkingLotId"];
            OrderModel mode = new OrderModel();
            mode.ParkingLotId =Convert.ToInt32(ParkingLotId);
            string modeJson = JsonConvert.SerializeObject(mode);
            string URL = ConfigurationManager.AppSettings["Host_URL"];
            try {
                string json = WebApiHelper.HttpPost(URL + "/api/WinForms/PostDownloadParkingLotData", modeJson);
                DataSet ds = json.JsonToDataSet();
                DataTable dtAttachment = new DataTable();
                dtAttachment = ds.Tables[0];
                DataTable dtWhiteList = new DataTable();
                dtWhiteList = ds.Tables[1];
                dtChilds.Add(dtWhiteList);
                DataTable dtParkingLotMonthly = new DataTable();
                dtParkingLotMonthly = ds.Tables[2];
                dtChilds.Add(dtParkingLotMonthly);
                DataTable dtMothlyLicensePlate = new DataTable();
                dtMothlyLicensePlate = ds.Tables[3];
                dtChilds.Add(dtMothlyLicensePlate);
                DataTable dtChargePlan = new DataTable();
                dtChargePlan = ds.Tables[4];
                dtChilds.Add(dtChargePlan);
                DataTable dtChargeRule = new DataTable();
                dtChargeRule = ds.Tables[5];
                dtChilds.Add(dtChargeRule);
                DataTable dtChargeRuleTime = new DataTable();
                dtChargeRuleTime = ds.Tables[6];
                dtChilds.Add(dtChargeRuleTime);
                DataTable dtChargeConfig = new DataTable();
                dtChargeConfig = ds.Tables[7];
                dtChilds.Add(dtChargeConfig);
                DataTable dtUser = new DataTable();
                dtUser = ds.Tables[8];
                dtChilds.Add(dtUser);
                DataTable dtMothlyGroup = new DataTable();
                dtMothlyGroup = ds.Tables[9];
                dtChilds.Add(dtMothlyGroup);
                result = ParkSystemUtility.SaveData(dataType, dtAttachment, dtChilds, "IPS_SyncParkingLotBaseData", ref errorMsg);
                if (result == 1000)
                {
                    //保存成功
                    returnVal = "同步保存成功";
                }
                else if (result == 1001 || result == 1002)
                {
                    //保存失败，数据处理失败，请联系数据管理员
                    returnVal = "数据库保存失败，数据处理失败，请联系数据管理员";
                    ParkSystemUtility.log.Error("AccessRulesDAL方法数据库保存失败，数据处理失败，请联系数据管理员");
                }
                else
                {
                    returnVal = "AccessRulesDAL方法保存失败:ErrorMsg = " + errorMsg;
                    ParkSystemUtility.log.Error("AccessRulesDAL方法保存失败:ErrorMsg ：" + errorMsg);
                }
            }
            catch (Exception ex)
            {
                returnVal = "ParkSystemDAL.AccessRulesDAL方法发生异常:" + ex.Message;
                ParkSystemUtility.log.Error("ParkSystemDAL.AccessRulesDAL方法：" + ex.Message);
            }
            return returnVal;
        }
        #endregion
        #region 登陆
        public DataSet Login(string userName, string passWord)
        {
            string Password = MD5Helper.Md5(passWord);
            DataTable dtOrder = new DataTable();
            paraDir.Clear();
            paraDir.Add("@UserName", userName);
            paraDir.Add("@Password", Password);

            DataSet ds = ParkSystemUtility.GetData(paraDir, "IPS_UserLogin");
            return ds;

        }
        #endregion
    }
}
