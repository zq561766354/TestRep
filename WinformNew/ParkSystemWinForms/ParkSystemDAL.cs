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
using SortingLine.Utility;


namespace ParkSystemWinForms
{
    public class ParkSystemDAL
    {
        public ParkSystemDAL()
        {

        }
        Dictionary<String, Object> paraDir = new Dictionary<String, Object>();

        #region 保存数据
        public OrderModel SaveOrder(int dataType, OrderModel mode, int netWorkResult)
        {
            int result = 0;
            string returnVal = string.Empty;
            OrderModel modeReturn = new OrderModel();
            //车辆识别入场判断是否有照片，没有照片不产生订单
            if (dataType == 1 && mode.Pb == String.Empty)
            {
                ParkSystemUtility.log.Error("入场保存失败，相机传入图片为空,车牌号：" + mode.LicensePlateNo);
                return modeReturn;
            }
            string errorMsg = string.Empty;
            string OrderNo = string.Empty;
            int BerthNum = 0;
            int ChargeTypeReturn = 0;
            int MonthLeftDayReturn = 0;
            string BerthNo = "";
            string ParkingLotId = Params.Settings.ParkLot.Id;//ConfigurationManager.AppSettings["ParkingLotId"];
            //生成停车信息DataTable
            DataTable dtAttachment = new DataTable("IPS_Order");
            dtAttachment.Columns.Add("OrderType", typeof(string));
            dtAttachment.Columns.Add("ParkingLotId", typeof(string));
            dtAttachment.Columns.Add("BerthNo", typeof(string));
            dtAttachment.Columns.Add("LicensePlateNo", typeof(string));
            dtAttachment.Columns.Add("LicensePlateType", typeof(int));
            dtAttachment.Columns.Add("MemberId", typeof(int));
            dtAttachment.Columns.Add("InDate", typeof(string));
            dtAttachment.Columns.Add("OutDate", typeof(string));
            dtAttachment.Columns.Add("RoadRateNo", typeof(string));
            //时间
            dtAttachment.Columns.Add("InMinute", typeof(int));
            DataTable dtAttachmentOrderPic = new DataTable("IPS_OrderPic");
            dtAttachmentOrderPic.Columns.Add("PictureAddr", typeof(string));
            dtAttachmentOrderPic.Columns.Add("PictureName", typeof(string));
            dtAttachmentOrderPic.Columns.Add("SizeMode", typeof(byte));

            DataRow dataRow = dtAttachment.NewRow();
            dataRow["OrderType"] = 2;
            dataRow["MemberId"] = mode.MemberId;
            dataRow["ParkingLotId"] = ParkingLotId;
            dataRow["BerthNo"] = BerthNo;
            dataRow["LicensePlateNo"] = mode.LicensePlateNo;
            dataRow["LicensePlateType"] = mode.LicensePlateType;
            dataRow["InDate"] = mode.InDate.ToString("yyyy-MM-dd HH:mm:ss");
            dataRow["OutDate"] = mode.OutDate.ToString("yyyy-MM-dd HH:mm:ss");
            dataRow["RoadRateNo"] = mode.RoadRateNo;
            dataRow["InMinute"] = Params.Settings.MistakenEntranceMin;
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
                DataTable dt = GetOrderTable(2, "", OrderNo);
                if(dt.Rows.Count <= 0)
                {
                    ParkSystemUtility.log.Error("未查询到订单，车牌号：" + mode.LicensePlateNo);
                    return modeReturn;
                }
                modeReturn.OrderNo = dt.Rows[0]["OrderNo"].ToString();
                modeReturn.InDate = Convert.ToDateTime(dt.Rows[0]["InDate"]);
                modeReturn.ChargeType = Convert.ToInt32(dt.Rows[0]["ChargeType"]);
                modeReturn.MemberId = Convert.ToInt32(dt.Rows[0]["MemberId"]);
                modeReturn.ParkingTime = Convert.ToInt32(dt.Rows[0]["ParkingTime"]);
                modeReturn.ActualAmount = Convert.ToDouble(dt.Rows[0]["ActualAmount"]);
                modeReturn.PayDifferenceAmount = Convert.ToDouble(dt.Rows[0]["ActualAmount"]);
                modeReturn.ActualGetAmount = Convert.ToDouble(dt.Rows[0]["ActualGetAmount"]);
                modeReturn.OrderCharge = Convert.ToDouble(dt.Rows[0]["OrderCharge"]);
                modeReturn.LicensePlateType = Convert.ToInt32(dt.Rows[0]["LicensePlateType"]);
                modeReturn.CarType = Convert.ToInt32(dt.Rows[0]["CarType"]);
                if (netWorkResult == 1)//联网时
                {
                    try
                    {
                        if (dataType == 1)
                        {
                            modeReturn.dataType = 1;
                            modeReturn.OrderType = Convert.ToInt32(dt.Rows[0]["OrderType"]);
                            modeReturn.ParkingLotId = Convert.ToInt32(dt.Rows[0]["ParkingLotId"]);
                            modeReturn.BerthNo = dt.Rows[0]["BerthNo"].ToString();
                            modeReturn.LicensePlateNo = dt.Rows[0]["LicensePlateNo"].ToString();
                            modeReturn.LicensePlateType = Convert.ToInt32(dt.Rows[0]["LicensePlateType"]);
                            modeReturn.Pb = mode.Pb;
                            modeReturn.PictureName = mode.PictureName;
                            string modeJson = JsonConvert.SerializeObject(modeReturn);
                            string URL = Params.Settings.Serv.Url;// ConfigurationManager.AppSettings["Host_URL"];
                            string json = WebApiHelper.HttpPost(URL + "/api/WinForms/PostFiles", modeJson);
                            OrderReturn or = JsonHelper.DeserializeObject<OrderReturn>(json);
                            if (or.result_code != 1000)
                            {
                                ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法 WebApiHelper：车牌：" + mode.LicensePlateNo + " 错误：" + or.return_msg);
                                //上传失败，纪录并重新上传
                                //将这笔上传失败的订单保存下来
                                string brokenNetTransOrder = String.Empty;
                                result = this.SaveBrokenNetTransOrder(1, OrderNo, 2, 1, ref brokenNetTransOrder);
                                if (result != 1000)
                                    ParkSystemUtility.log.Error("上传订单失败，保存到断网续传中失败：" + brokenNetTransOrder);
                            }
                        }

                        else if (dataType == 2 && modeReturn.ChargeType == 10 && modeReturn.ActualAmount > 0)
                        {
                            modeReturn.RoadRateNo = mode.RoadRateNo;
                            modeReturn.dataType = 2;
                            modeReturn.OutDate = Convert.ToDateTime(dt.Rows[0]["OutDate"]);
                            modeReturn.LicensePlateNo = dt.Rows[0]["LicensePlateNo"].ToString();
                            modeReturn.LicensePlateType = Convert.ToInt32(dt.Rows[0]["LicensePlateType"]);
                            //modeReturn.MemberId = Convert.ToInt32(dt.Rows[0]["MemberId"]);
                            modeReturn.OrderCharge = Convert.ToDouble(dt.Rows[0]["OrderCharge"]);

                            modeReturn.DiscountAmount = Convert.ToDouble(dt.Rows[0]["DiscountAmount"]);
                            modeReturn.ChargeType = Convert.ToInt32(dt.Rows[0]["ChargeType"]);
                            string modeJson = JsonConvert.SerializeObject(modeReturn);
                            string URL = Params.Settings.Serv.Url;// ConfigurationManager.AppSettings["Host_URL"];
                            string json = WebApiHelper.HttpPost(URL + "/api/WinForms/PostFiles", modeJson);
                            OrderReturn or = JsonHelper.DeserializeObject<OrderReturn>(json);
                            if (or.result_code != 1000)
                                ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法 WebApiHelper：车牌：" + mode.LicensePlateNo + " 错误：" + or.return_msg);
                            else
                            {
                                modeReturn.State = or.state;
                                modeReturn.ActualAmount = or.ActualAmount;
                                modeReturn.ParkingTime = or.parkingTime;
                                modeReturn.OrderCharge = or.OrderCharge;
                                modeReturn.DiscountAmount = or.DiscountAmount;
                                modeReturn.Paytime = or.Paytime;
                                modeReturn.PayDifferenceAmount = or.PayDifferenceAmount;
                                modeReturn.ActualGetAmount = or.ActualGetAmount;
                                ParkSystemUtility.log.Debug(or.Paytime);
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        if (dataType == 1)
                        {
                            //上传失败，纪录并重新上传
                            //将这笔上传失败的订单保存下来
                            string brokenNetTransOrder = String.Empty;
                            result = this.SaveBrokenNetTransOrder(1, OrderNo, 3, 1, ref brokenNetTransOrder);
                            if (result != 1000)
                                ParkSystemUtility.log.Error("上传订单异常，保存到断网续传中失败：" + brokenNetTransOrder);
                        }
                        ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法 WebApiHelper：车牌：" + mode.LicensePlateNo + " 错误：" + ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        //入场出现断网情况
                        if (dataType == 1)
                        {
                            //将这笔上传失败的订单保存下来
                            string brokenNetTransOrder = String.Empty;
                            result = this.SaveBrokenNetTransOrder(1, OrderNo, 1, 1, ref brokenNetTransOrder);
                            if (result != 1000)
                                ParkSystemUtility.log.Error("断网订单保存失败：" + brokenNetTransOrder);
                        }
                        //出场出现断网情况
                        if (dataType == 2)
                        {
                            modeReturn.State = Convert.ToInt32(dt.Rows[0]["State"]);
                            modeReturn.ActualAmount = Convert.ToDouble(dt.Rows[0]["ActualAmount"]);
                            modeReturn.ParkingTime = Convert.ToInt32(dt.Rows[0]["ParkingTime"]);
                            modeReturn.OrderCharge = Convert.ToDouble(dt.Rows[0]["OrderCharge"]);
                            modeReturn.DiscountAmount = Convert.ToDouble(dt.Rows[0]["DiscountAmount"]);
                            modeReturn.Paytime = null;
                            modeReturn.PayDifferenceAmount = Convert.ToDouble(dt.Rows[0]["ActualAmount"]);
                            modeReturn.ActualGetAmount = Convert.ToDouble(dt.Rows[0]["ActualGetAmount"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        ParkSystemUtility.log.Error("断网订单处理异常：" + ex.Message);
                    }
                }
                modeReturn.berthNum = BerthNum;
                modeReturn.State = Convert.ToInt32(dt.Rows[0]["State"]);
                modeReturn.monthLeftDay = MonthLeftDayReturn;
            }
            else if (result == 1001 || result == 1002)
            {
                //保存失败，数据处理失败，请联系数据管理员
                returnVal = "数据库保存失败，数据处理失败，请联系数据管理员";
                ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法数据库保存失败，数据处理失败，请联系数据管理员" + mode.LicensePlateNo);
            }
            else if (result == 1004)
            {
                //保存失败，数据处理失败，请联系数据管理员
                returnVal = "该车已经在场内，不重复生成订单";
                ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder，该车已经在场内，不重复生成订单" + mode.LicensePlateNo);
            }
            else
            {
                returnVal = "ParkSystemDAL.SaveOrder方法保存失败:ErrorMsg = " + errorMsg;
                ParkSystemUtility.log.Error("ParkSystemDAL.SaveOrder方法保存失败:ErrorMsg ：" + mode.LicensePlateNo + errorMsg);
            }
            return modeReturn;

        }

        #endregion

        #region 修正订单车牌号 BY OrderNo
        public string updateLicensePlateNo(int dataType, string licensePlateNo, string orderNo)
        {
            ParkSystemUtility.log.Debug("修改订单车牌号：dataType = " + dataType.ToString() + "LicensePlateNo = " + licensePlateNo + "OrderNo=" + orderNo);
            string OrderNo = string.Empty;
            int BerthNum = 0;
            int ChargeTypeReturn = 0;
            int MonthLeftDayReturn = 0;
            string errorMsg = "";
            string returnVal = String.Empty;
            int result = 1000;//1000是更新成功0是失败
            try
            {
                DataTable dtAttachmentOrderPic = new DataTable("IPS_OrderPic");
                DataTable dtAttachment = new DataTable("IPS_Order");
                //todo
                // dtAttachment.Columns.Add("OrderNo", typeof(string));
                dtAttachment.Columns.Add("OrderNo", typeof(string));
                dtAttachment.Columns.Add("LicensePlateNo", typeof(string));
                DataRow dataRow = dtAttachment.NewRow();
                dataRow["OrderNo"] = orderNo;
                dataRow["LicensePlateNo"] = licensePlateNo;
                dtAttachment.Rows.Add(dataRow);
                result = ParkSystemUtility.SaveData(dataType, dtAttachment, dtAttachmentOrderPic, "IPS_SaveOrder_local", ref errorMsg, ref OrderNo, ref BerthNum, ref ChargeTypeReturn, ref MonthLeftDayReturn);
                if (result == 1000)
                {
                    returnVal = "更新成功！";
                    //string URL = ConfigurationManager.AppSettings["Host_URL"];

                    // int PayType = 1;
                    //  WebApiHelper.HttpPost(URL + "/api/WinForms/PostFiles", "{licensePlateNo:\"" + licensePlateNo + "\",OrderNo:\"" + orderNo +  "\"}");
                    try
                    {
                        string URL = Params.Settings.Serv.Url + "/api/WinForms/updateLicensePlateNo";//ConfigurationManager.AppSettings["Host_URL"] 
                        var obj = new { licensePlateNo = licensePlateNo, orderNo = orderNo };
                        //string objJson = JsonConvert.SerializeObject(obj);
                        string param = obj.ToJson();
                        //SortingLine.Utility.HttpOperate.Requst(setting.UpUrl, "POST", uploadTimeOut, "application/x-www-form-urlencoded", "utf-8", param);
                        string json = SortingLine.Utility.HttpOperate.Requst(URL, "POST", 5000, "application/json", "utf-8", param);
                        // string json = WebApiHelper.HttpPost(URL + "/api/WinForms/GetHeadNetwork", "");
                        returnVal = "本地：" + returnVal + ",平台：" + json;
                    }
                    catch (Exception ex)
                    {
                        ParkSystemUtility.log.Error("ParkSystemDAL.updateLicensePlateNo方法 webapi catch报错：：" + ex.Message);
                    }
                }
                else if (result == 1001 || result == 1002)
                {
                    //保存失败，数据处理失败，请联系数据管理员
                    returnVal = "ParkSystemDAL.updateLicensePlateNo方法数据库保存失败，数据处理失败，请联系数据管理员" + ",orderNo:" + orderNo + ",licensePlateNo:" + licensePlateNo;

                }
                else if (result == 1005)
                {
                    //保存失败，数据处理失败，请联系数据管理员
                    returnVal = "更新，该车已存在，请确认是是否有重复识别的情况并标记成异常";

                }
                else
                {
                    returnVal = "ParkSystemDAL.updateLicensePlateNo方法保存失败:ErrorMsg = " + errorMsg;

                }
            }
            catch (Exception ex)
            {
                returnVal = "请联系后台人员处理报错，ParkSystemDAL.updateState方法catch报错 本地更新订单：" + ex.Message + ",licensePlateNo:" + licensePlateNo + ",orderNo:" + orderNo;

            }
            if(returnVal != String.Empty)
                ParkSystemUtility.log.Error(returnVal);
            return returnVal;
        }

        #endregion
        #region 更改订单状态 BY OrderNo
        /// <summary>
        /// 保存订单状态 BY OrderNo
        /// </summary>
        /// <param name="dataType">3是更新订单</param>
        /// <returns></returns>
        public OrderModel updateState(int dataType, OrderModel om, int netWorkResult)
        {
            OrderModel mode = new OrderModel();

            int result = 1000;//1000是更新成功0是失败
            string resultVal = string.Empty;
            string errorMsg = string.Empty;
            string OrderNoReturn = string.Empty;
            int BerthNum = 0;
            int ChargeTypeReturn = 0;
            int MonthLeftDayReturn = 0;
            try
            {
                ParkSystemUtility.log.Debug("updateState方法，netWorkResult:" + netWorkResult + ",OrderNo:" + om.OrderNo + ",ParkingTime:" + om.ParkingTime + ",OrderCharge:" + om.OrderCharge + ",ActualAmount:" + om.ActualAmount + ",DiscountAmount:" + om.DiscountAmount + ",Paytime:" + om.Paytime + ",Note:" + om.Note);
                DataTable dtAttachment = new DataTable("IPS_Order");
                dtAttachment.Columns.Add("OrderNo", typeof(string));
                dtAttachment.Columns.Add("ParkingTime", typeof(int));
                dtAttachment.Columns.Add("OrderCharge", typeof(double));
                dtAttachment.Columns.Add("ActualAmount", typeof(double));
                dtAttachment.Columns.Add("ActualGetAmount", typeof(double));
                dtAttachment.Columns.Add("DiscountAmount", typeof(double));
                dtAttachment.Columns.Add("Paytime", typeof(string));
                dtAttachment.Columns.Add("Note", typeof(string));
                DataRow dataRow = dtAttachment.NewRow();
                dataRow["OrderNo"] = om.OrderNo;
                dataRow["ParkingTime"] = om.ParkingTime;
                dataRow["OrderCharge"] = om.OrderCharge;
                dataRow["ActualAmount"] = om.ActualAmount;
                dataRow["ActualGetAmount"] = om.ActualGetAmount;
                dataRow["DiscountAmount"] = om.DiscountAmount;
                dataRow["Paytime"] = om.Paytime;
                dataRow["Note"] = om.Note;
                dtAttachment.Rows.Add(dataRow);

                DataTable dtOrderPayDetail = new DataTable("IPS_OrderPayDetail");
                dtOrderPayDetail.Columns.Add("OrderNo", typeof(string));
                dtOrderPayDetail.Columns.Add("PayNo", typeof(string));
                dtOrderPayDetail.Columns.Add("PayType", typeof(int));
                dtOrderPayDetail.Columns.Add("PayMoney", typeof(double));
                dtOrderPayDetail.Columns.Add("ChargeEmp", typeof(int));
                dtOrderPayDetail.Columns.Add("Note", typeof(string));
                if (om.PayMoney != 0)
                {
                    DataRow dataRowPay = dtOrderPayDetail.NewRow();
                    dataRowPay["OrderNo"] = om.OrderNo;
                    dataRowPay["PayNo"] = om.PayNo;
                    dataRowPay["PayType"] = om.PayType;
                    dataRowPay["PayMoney"] = om.PayMoney;
                    dataRowPay["ChargeEmp"] = om.ChargeEmp;
                    dataRowPay["Note"] = om.Note;
                    dtOrderPayDetail.Rows.Add(dataRowPay);
                }
                DataTable dt = GetOrderTable(2, "", om.OrderNo);
                result = ParkSystemUtility.SaveData(dataType, dtAttachment, dtOrderPayDetail, "IPS_SaveOrder_local", ref errorMsg, ref OrderNoReturn, ref BerthNum, ref ChargeTypeReturn, ref MonthLeftDayReturn);
                if (result == 1000)
                {
                    mode.berthNum = BerthNum;
                    //保存成功
                    OrderModel modeReturn = new OrderModel();

                    if (netWorkResult == 1)
                    {
                        try
                        {
                            modeReturn.Pb = om.Pb;
                            modeReturn.PictureName = om.PictureName;

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
                            modeReturn.ParkingTime = om.ParkingTime;
                            modeReturn.OrderCharge = om.OrderCharge;
                            modeReturn.ActualAmount = om.ActualAmount;
                            modeReturn.ActualGetAmount = Convert.ToDouble(dt.Rows[0]["ActualGetAmount"]);
                            modeReturn.DiscountAmount = om.DiscountAmount;
                            modeReturn.OrderComplete = Convert.ToInt32(dt.Rows[0]["OrderComplete"]);

                            modeReturn.CarType = Convert.ToInt32(dt.Rows[0]["CarType"]);
                            //  modeReturn.OrderCompleteDate = Convert.ToDateTime(dt.Rows[0]["OrderCompleteDate"]);
                            modeReturn.State = 30;
                            modeReturn.Note = om.Note;
                            modeReturn.RoadRateNo = dt.Rows[0]["RoadRateNo"].ToString();

                            modeReturn.PayNo = om.PayNo;
                            modeReturn.PayMoney = om.PayMoney;
                            modeReturn.PayType = om.PayType;
                            modeReturn.PayNote = om.Note;
                            modeReturn.ChargeEmp = om.ChargeEmp;

                            modeReturn.Paytime = om.Paytime;
                            string modeJson = JsonConvert.SerializeObject(modeReturn);
                            string URL = Params.Settings.Serv.Url;// ConfigurationManager.AppSettings["Host_URL"];
                            string json = WebApiHelper.HttpPost(URL + "/api/WinForms/PostFiles", modeJson);
                            OrderReturn or = JsonHelper.DeserializeObject<OrderReturn>(json);
                            if (or.result_code != 1000)
                                ParkSystemUtility.log.Error("ParkSystemDAL.updateState方法，订单结束上传本地订单失败：" + mode.LicensePlateNo + " " + or.return_msg);

                        }
                        catch (Exception ex)
                        {
                            ParkSystemUtility.log.Error("ParkSystemDAL.updateState方法 订单结束上传本地订单发生异常：" + mode.LicensePlateNo + " " + ex.Message);
                            //将这笔上传失败的订单保存下来
                            string brokenNetTransOrder = String.Empty;
                            result = this.SaveBrokenNetTransOrder(1, om.OrderNo, 3, 2, ref brokenNetTransOrder);
                            if (result != 1000)
                                ParkSystemUtility.log.Error("上传出场订单异常，保存到断网续传中失败：" + brokenNetTransOrder);
                        }
                    }
                    else
                    {
                        //将这笔上传失败的订单保存下来
                        string brokenNetTransOrder = String.Empty;
                        result = this.SaveBrokenNetTransOrder(1, om.OrderNo, 1, 2, ref brokenNetTransOrder);
                        if (result != 1000)
                            ParkSystemUtility.log.Error("上传出场订单异常，保存到断网续传中失败：" + brokenNetTransOrder);
                    }

                    modeReturn.berthNum = BerthNum;
                    modeReturn.monthLeftDay = MonthLeftDayReturn;

                }
                else if (result == 1001 || result == 1002)
                {
                    resultVal = "请联系后台人员处理报错，ParkSystemDAL.updateState方法 存储过程异常：" + errorMsg + ",result" + result;
                }

                else if (result == 2001)
                    resultVal = "请联系后台人员处理报错，ParkSystemDAL.updateState方法 存储过程异常：数据库保存失败，数据处理失败，请联系数据管理员" + errorMsg + ",result" + result;

            }
            catch (Exception ex)
            {
                resultVal = "请联系后台人员处理报错，ParkSystemDAL.updateState方法catch报错 本地更新订单：" + ex.Message;

            }
            ParkSystemUtility.log.Error(resultVal);
            mode.result = result;
            mode.resultVal = resultVal;
            return mode;
            // int result = 1;//1是更新成功0是失败

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
        public DataTable GetOrderTable(int dataType, string licensePlateNo, string orderNo)
        {
            string ParkingLotId = Params.Settings.ParkLot.Id;
            DataTable dtOrder = new DataTable();
            paraDir.Clear();
            paraDir.Add("@DataType", dataType);
            paraDir.Add("@OrderNo", orderNo);
            paraDir.Add("@LicensePlateNo", licensePlateNo);
            paraDir.Add("@ParkingLotId", ParkingLotId);
            DataSet ds = ParkSystemUtility.GetData(paraDir, "IPS_GetOrder_local");
            if (ds.Tables.Count > 0 && ds != null)
                return dtOrder = ds.Tables[0];
            else
                return dtOrder;
        }

        #endregion


        #region 批量处理订单
        public string deleteOrder(int dataType, string orderNoStr, int chargeEmp)
        {
            int result = 0;//1000是更新成功0是失败
            string returnVal = String.Empty;
            try
            {
                string errorMsg = String.Empty;
                paraDir.Clear();
                paraDir.Add("@OrderNoStr", orderNoStr);
                paraDir.Add("@DataType", dataType);
                paraDir.Add("@ChargeEmp", chargeEmp);
                result = ParkSystemUtility.SaveData(paraDir, "IPS_OperateOrderBatch", ref errorMsg);
                if (result == 1000)
                {
                    returnVal = "本地标记成功";
                    var obj = new { dataType = dataType, orderNoStr = orderNoStr, chargeEmp = chargeEmp };
                    string objJson = JsonConvert.SerializeObject(obj);
                    string URL = Params.Settings.Serv.Url;// ConfigurationManager.AppSettings["Host_URL"];
                    string json = WebApiHelper.HttpPost(URL + "/api/WinForms/OperateOrderBatch", objJson);
                    ParkSystemUtility.log.Info("ParkSystemDAL.deleteOrder方法 WebApiHelper：返回结果：" + json);
                }

                else if (result == 1001)
                    returnVal = "本地标记失败，数据处理失败，请联系数据管理员，OrderNoStr：" + orderNoStr;

                else
                    returnVal = "本地标记失败:ErrorMsg = " + errorMsg + "，OrderNoStr：" + orderNoStr;
            }
            catch (Exception ex)
            {
                returnVal = "ParkSystemDAL.deleteOrder方法：" + ex.Message;
            }
            return returnVal;
        }

        #endregion

        #region 查会员ID
        public int GetMemberId(string LicensePlateNo)
        {
            int memberid = 0;
            string URL = Params.Settings.Serv.Url;// ConfigurationManager.AppSettings["Host_URL"];
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
            string returnVal = String.Empty;
            string errorMsg = String.Empty;
            try
            {
                int dataType = 1;
                int result = 0;

                List<DataTable> dtChilds = new List<DataTable>();
                string ParkingLotId = Params.Settings.ParkLot.Id;//ConfigurationManager.AppSettings["ParkingLotId"];
                OrderModel mode = new OrderModel();
                mode.ParkingLotId = Convert.ToInt32(ParkingLotId);
                string modeJson = JsonConvert.SerializeObject(mode);
                string URL = Params.Settings.Serv.Url;// ConfigurationManager.AppSettings["Host_URL"];

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
                DataTable dtWhiteListBindParkingLot = new DataTable();
                dtWhiteListBindParkingLot = ds.Tables[10];
                dtChilds.Add(dtWhiteListBindParkingLot);
                DataTable dtUserParkingLot = new DataTable();
                dtUserParkingLot = ds.Tables[11];
                dtChilds.Add(dtUserParkingLot);
                //Added by Zhiwen_Tian 2019/08/07 15:36
                DataTable dtWeekLimit = new DataTable();
                dtWeekLimit = ds.Tables[12];
                //Added by John_Zhong 2020年3月3日09点52分
                DataTable dtCarGroupClicense = new DataTable();
                dtCarGroupClicense = ds.Tables["IPS_CarGroupLicensePlateNo"];
                dtChilds.Add(dtCarGroupClicense);
                DataTable dtCarGroup = new DataTable();
                dtCarGroup = ds.Tables["IPS_CarGroup"];
                dtChilds.Add(dtCarGroup);

                dtChilds.Add(dtWeekLimit);

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
                Console.WriteLine("ParkSystemDAL.AccessRulesDAL方法：" + ex.Message);
            }
            Console.WriteLine("ParkSystemDAL.AccessRulesDAL方法：" + returnVal);
            return returnVal;
        }
        #endregion
        #region 登陆
        public DataSet Login(string userName, string passWord)
        {
            string passWordMd5 = MD5Helper.Md5(passWord);
            DataTable dtOrder = new DataTable();
            paraDir.Clear();
            paraDir.Add("@UserName", userName);
            paraDir.Add("@Password", passWordMd5);
            DataSet ds = ParkSystemUtility.GetData(paraDir, "IPS_UserLogin");
            return ds;

        }
        #endregion
        #region 生成当班纪录
        /// <summary>
        /// 生成当班纪录
        /// </summary>
        /// <param name="dataType">--1：上班  2：下班</param>
        /// <param name="UserId">用户的ID</param>
        /// <param name="WorkNo">--1：传空  2：传具体的参数</param>
        /// <returns></returns>
        public ChargeOnDutyModel SaveChargeOnDuty(int dataType, int userId, string workNo)
        {
            ChargeOnDutyModel mode = new ChargeOnDutyModel();
            string parkingLotId = Params.Settings.ParkLot.Id;
            int result = 1000;//1000是更新成功0是失败
            string errorMsg = string.Empty;
            string WorkNoReturn = string.Empty;
            DateTime startWorkTime = System.DateTime.Now;
            double cashAmountReturn = 0;

            try
            {
                paraDir.Clear();
                paraDir.Add("@DataType", dataType);
                paraDir.Add("@UserId", userId);
                paraDir.Add("@WorkNo", workNo);
                paraDir.Add("@ParkingLotId", parkingLotId);
                result = ParkSystemUtility.SaveData(paraDir, "IPS_SaveChargeOnDuty", ref errorMsg, ref WorkNoReturn, ref startWorkTime, ref cashAmountReturn);
                mode.WorkNo = WorkNoReturn;
                mode.result = result;
                mode.StartWorkTime = startWorkTime;
                mode.CashAmount = cashAmountReturn;
                if (result == 1000)
                {
                    DataTable dtChargeOnDuty = new DataTable();
                    paraDir.Clear();

                    paraDir.Add("@WorkNo", WorkNoReturn);
                    DataSet ds = ParkSystemUtility.GetData(paraDir, "IPS_GetChargeOnDuty");
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        string dsJson = ds.Tables[0].ToJson();
                        //if (dataType == 2)
                        //{
                        try
                        {
                            string URL = Params.Settings.Serv.Url;
                            string json = WebApiHelper.HttpPost(URL + "/api/WinForms/UploadChargeOnDutyRecord", "{\"data\":" + dsJson + "}");
                            mode.returnResult = Convert.ToInt32(json);

                        }
                        catch (Exception ex)
                        {
                            ParkSystemUtility.log.Error("ParkSystemDAL.SaveChargeOnDuty方法 WebApiHelper" + ex.Message);
                        }
                        //}
                    }
                }
                else if (result == 1001 || result == 1002)
                    ParkSystemUtility.log.Error("ParkSystemDAL.SaveChargeOnDuty方法 存储过程异常：" + errorMsg);
                else if (result == 2001)
                    ParkSystemUtility.log.Error("ParkSystemDAL.SaveChargeOnDuty方法 存储过程异常：数据库保存失败，数据处理失败，请联系数据管理员" + errorMsg);
            }
            catch (Exception ex)
            {
                ParkSystemUtility.log.Error("ParkSystemDAL.SaveChargeOnDuty方法 生成当班纪录：" + ex.Message);
            }
            return mode;

        }
        #endregion
        #region 保存当班收费纪录表
        /// <summary>
        /// 保存当班收费纪录表
        /// </summary>
        /// <param name="orderNo">--订单编号</param>
        /// <param name="UserId">用户的ID</param>
        /// <param name="WorkNo">--班次号</param>
        /// <returns></returns>
        public ChargeOnDutyModel SaveChargeRecord(string orderNo, double amount, string workNo)
        {
            ChargeOnDutyModel mode = new ChargeOnDutyModel();
            int result = 0;//1000是更新成功0是失败
            string errorMsg = string.Empty;
            string resultMsg = string.Empty;
            double amountSum = 0;
            try
            {
                paraDir.Clear();
                paraDir.Add("@WorkNo", workNo);
                paraDir.Add("@OrderNo", orderNo);
                paraDir.Add("@Amount", amount);
                result = ParkSystemUtility.SaveData(paraDir, "IPS_SaveChargeRecord", ref errorMsg, ref amountSum);
                if (result == 1000)
                {
                    resultMsg = "保存成功";

                }
                else if (result == 1001 || result == 1002)
                {
                    ParkSystemUtility.log.Error("ParkSystemDAL.SaveChargeRecord方法 存储过程异常：" + errorMsg);
                    resultMsg = "ParkSystemDAL.SaveChargeRecord方法 存储过程异常：" + errorMsg;
                }

                else if (result == 2001)
                {
                    ParkSystemUtility.log.Error("ParkSystemDAL.SaveChargeRecord方法 存储过程异常：数据库保存失败，数据处理失败，请联系数据管理员" + errorMsg);
                    resultMsg = "ParkSystemDAL.SaveChargeRecord方法 存储过程异常：数据库保存失败，数据处理失败，请联系数据管理员" + errorMsg;
                }

            }
            catch (Exception ex)
            {
                ParkSystemUtility.log.Error("ParkSystemDAL.SaveChargeRecord方法 生成当班纪录：" + ex.Message);
            }
            mode.returnstr = resultMsg;
            mode.CashAmount = amountSum;
            return mode;
            // int result = 1;//1是更新成功0是失败

        }
        #endregion
        #region 查询订单平台支付状态
        /// <summary>
        /// 查询订单平台支付状态
        /// </summary>
        /// <param name="dataType">--1：上班  2：下班</param>
        /// <param name="UserId">用户的ID</param>
        /// <param name="WorkNo">--1：传空  2：传具体的参数</param>
        /// <returns></returns>
        public OrderReturn GetOrderState(string orderNo)
        {
            OrderReturn mode = new OrderReturn();

            int result = 1000;//1000是更新成功0是失败
            string errorMsg = string.Empty;
            string WorkNoReturn = string.Empty;
            DateTime startWorkTime = System.DateTime.Now;
            double cashAmountReturn = 0;

            try
            {

                DataTable dtChargeOnDuty = new DataTable();
                string URL = Params.Settings.Serv.Url;// ConfigurationManager.AppSettings["Host_URL"];
                string json = WebApiHelper.HttpPost(URL + "/api/WinForms/GetOrderHead", "{\"orderNo\":\"" + orderNo + "\"}");
                mode = JsonHelper.DeserializeObject<OrderReturn>(json);
            }
            catch (Exception ex)
            {
                ParkSystemUtility.log.Error("ParkSystemDAL.GetOrderState方法 查询订单平台支付状态：" + ex.Message);
            }
            return mode;
            // int result = 1;//1是更新成功0是失败

        }
        #endregion
        #region 更新本地会员Id
        /// <summary>
        /// 更新本地会员Id
        /// </summary>
        /// <param name="licensePlateNo">车牌号</param>
        /// <param name="memberId">会员号</param>
        /// <returns></returns>
        public int UpdateMember(string licensePlateNo, int memberId)
        {
            ChargeOnDutyModel mode = new ChargeOnDutyModel();
            int result = 0;//1000是更新成功0是失败
            string errorMsg = string.Empty;
            string resultMsg = string.Empty;
            double amountSum = 0;
            try
            {
                paraDir.Clear();
                paraDir.Add("@LicensePlateNo", licensePlateNo);
                paraDir.Add("@MemberId", memberId);

                result = ParkSystemUtility.SaveData(paraDir, "IPS_UpdateMember_Local", ref errorMsg);
                if (result == 1000)
                {
                    resultMsg = "保存成功";

                }
                else if (result == 1001 || result == 1002)
                {
                    ParkSystemUtility.log.Error("ParkSystemDAL.UpdateMember方法 存储过程异常：" + errorMsg);
                    resultMsg = "ParkSystemDAL.UpdateMember方法 存储过程异常：" + errorMsg;
                }

                else if (result == 2001)
                {
                    ParkSystemUtility.log.Error("ParkSystemDAL.UpdateMember方法 存储过程异常：数据库保存失败，数据处理失败，请联系数据管理员" + errorMsg);
                    resultMsg = "ParkSystemDAL.UpdateMember方法 存储过程异常：数据库保存失败，数据处理失败，请联系数据管理员" + errorMsg;
                }

            }
            catch (Exception ex)
            {
                ParkSystemUtility.log.Error("ParkSystemDAL.SaveChargeRecord方法 生成当班纪录：" + ex.Message);
            }
            mode.returnstr = resultMsg;
            mode.CashAmount = amountSum;
            return result;
            // int result = 1;//1是更新成功0是失败

        }
        #endregion

        public int NetworkHead()
        {
            int result = 0;
            try
            {
                string URL = Params.Settings.Serv.Url + "/api/WinForms/GetHeadNetwork";//ConfigurationManager.AppSettings["Host_URL"] 
                string json = SortingLine.Utility.HttpOperate.Requst(URL, "POST", 2000, "application/json", "utf-8", "");
                // string json = WebApiHelper.HttpPost(URL + "/api/WinForms/GetHeadNetwork", "");
                result = Convert.ToInt32(json);
            }
            catch (Exception ex)
            {
                ParkSystemUtility.log.Error("ParkSystemDAL.NetworkHead方法 查询网络状态：" + ex.Message);
            }
            return result;
        }

        #region  更新停车场泊位数
        public void UpdateRemoteParkingLotBerthNum(int parkingLotId)
        {
            try
            {
                //调用接口查询停车场泊位数
                string URL = Params.Settings.Serv.Url + "/api/WinForms/GetParkingLotBerthNum";
                int berthNum = Convert.ToInt32(SortingLine.Utility.HttpOperate.Requst(URL, "POST", 5000, "application/json", "utf-8", parkingLotId.ToString()));
                //更新本地数据库的泊位数
                if (berthNum > 0)
                    UpdateBerthNum(parkingLotId, berthNum);
            }
            catch (Exception ex)
            {
                ParkSystemUtility.log.Error("ParkSystemDAL.UpdateRemoteParkingLotBerthNum方法 更新停车场泊位数失败：" + ex.Message);
            }
        }
        #endregion

        #region 更新本地停车场泊位数
        public void UpdateBerthNum(int parkingLotId, int berthNum)
        {
            string errorMsg = String.Empty;
            paraDir.Clear();
            paraDir.Add("@ParkingLotId", parkingLotId);
            paraDir.Add("@BerthNum", berthNum);
            int errorId = ParkSystemUtility.SaveData(paraDir, "IPS_UpdateParkingLotBerthNum", ref errorMsg);
        }
        #endregion

        #region 读取余位数
        public int getLeftCount()
        {
            int LeftCount = 0;
            string ParkingLotId = Params.Settings.ParkLot.Id;//ConfigurationManager.AppSettings["ParkingLotId"];
            DataTable dtOrder = new DataTable();
            paraDir.Clear();
            paraDir.Add("@ParkingLotId ", ParkingLotId);
            DataSet ds = ParkSystemUtility.GetData(paraDir, "IPS_GetParkingLotLeftCount");
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                LeftCount = Convert.ToInt32(dt.Rows[0]["LeftCount"]);
            }
            return LeftCount;

        }
        #endregion
        #region 是否包月车
        public int IsLicensePlateIsMonthly(int dataType, string licensePlateNo)
        {
            int LeftCount = 0;
            string ParkingLotId = Params.Settings.ParkLot.Id;//ConfigurationManager.AppSettings["ParkingLotId"];
            DataTable dtOrder = new DataTable();
            paraDir.Clear();
            paraDir.Add("@DataType ", dataType);
            paraDir.Add("@LicensePlateNo ", licensePlateNo);
            paraDir.Add("@ParkingLotId ", ParkingLotId);
            DataSet ds = ParkSystemUtility.GetData(paraDir, "IPS_LicensePlateIsMonthly");
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                LeftCount = Convert.ToInt32(dt.Rows[0]["MonthLeftDay"]);
            }


            return LeftCount;

        }
        #endregion

        #region 黄牌车是否重复收费
        public int IsReCalculation(int dataType, string licensePlateNo, string roadRateNo, int networkOk)
        {
            int result = 0;
            string errorMsg = string.Empty;
            int ReCalculationReturn = 1;//0是结束1是要往下走流程
            string OrderNoReturn = string.Empty;
            try
            {
                string ParkingLotId = Params.Settings.ParkLot.Id;//ConfigurationManager.AppSettings["ParkingLotId"];

                paraDir.Clear();
                paraDir.Add("@DataType ", dataType);
                paraDir.Add("@LicensePlateNo ", licensePlateNo);
                paraDir.Add("@ParkingLotId ", ParkingLotId);
                paraDir.Add("@RoadRateNo ", roadRateNo);
                result = ParkSystemUtility.SaveData(paraDir, "IPS_CheckLicensePlateReCalculation", ref errorMsg, ref ReCalculationReturn, ref OrderNoReturn);
                if (result == 1000)
                {
                    if (ReCalculationReturn == 0)
                    {
                        if (networkOk == 0)
                        {
                            ParkSystemUtility.log.Warn("ParkSystemDAL.IsReCalculation方法，断网，平台未处理订单号：" + OrderNoReturn);

                        }
                        else
                        {
                            try
                            {
                                string URL = Params.Settings.Serv.Url;// ConfigurationManager.AppSettings["Host_URL"];
                                string json = WebApiHelper.HttpPost(URL + "/api/WinForms/CompleteReCalculationOrder", "{roadRateNo:\"" + roadRateNo + "\",orderNo:\"" + OrderNoReturn + "\",dataType:\"" + dataType + "\"}");
                                ParkSystemUtility.log.Debug("ParkSystemDAL.IsReCalculation方法 WebApiHelper返回的json:" + json);

                            }
                            catch (Exception ex)
                            {
                                ParkSystemUtility.log.Error("ParkSystemDAL.IsReCalculation方法 WebApiHelper" + ex.Message);
                            }
                        }

                    }
                }
                else
                {
                    ReCalculationReturn = 1;
                    ParkSystemUtility.log.Error("ParkSystemDAL.IsReCalculation方法 ,保存失败，result" + result + ",errorMsg" + errorMsg);
                }

            }
            catch (Exception ex)
            {
                ReCalculationReturn = 1;
                ParkSystemUtility.log.Error("ParkSystemDAL.IsReCalculation方法 :" + ex.Message);
            }
            return ReCalculationReturn;

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
        public OrderModel ReCalculationMediumLarge(int dataType, string orderNo, int carType, int networkOk)
        {
            OrderModel mode = new OrderModel();
            int result = 0;//1000是更新成功0是失败
            string errorMsg = string.Empty;
            string resultMsg = string.Empty;
            double OrderCharge = 0;
            double DiscountAmount = 0;
            double ActualAmount = 0;
            try
            {
                paraDir.Clear();
                paraDir.Add("@DataType", dataType);
                paraDir.Add("@OrderNo", orderNo);
                paraDir.Add("@CarType", carType);
                result = ParkSystemUtility.SaveData(paraDir, "IPS_ReCalculationMediumLarge", ref errorMsg, ref OrderCharge, ref DiscountAmount, ref ActualAmount);
                if (result == 1000)
                {
                    resultMsg = "保存成功";
                    if (networkOk == 0)
                    {
                        ParkSystemUtility.log.Error("ParkSystemDAL.ReCalculationMediumLarge方法webapi 网络不通，不更新平台，networkOk" + networkOk);
                        mode.PayDifferenceAmount = ActualAmount;
                        mode.ActualGetAmount = 0;
                    }
                    else
                    {
                        try
                        {
                            string URL = Params.Settings.Serv.Url + "/api/WinForms/UpdateCarType";//ConfigurationManager.AppSettings["Host_URL"] 
                            var obj = new { dataType = dataType, carType = carType, orderNo = orderNo, orderCharge = OrderCharge, discountAmount = DiscountAmount, actualAmount = ActualAmount };
                            string param = obj.ToJson();
                            string json = SortingLine.Utility.HttpOperate.Requst(URL, "POST", 5000, "application/json", "utf-8", param);
                            OrderReturn or = JsonHelper.DeserializeObject<OrderReturn>(json);
                            if (or.result_code == 1000)
                            {
                                mode.ActualGetAmount = or.ActualGetAmount;
                                mode.PayDifferenceAmount = or.PayDifferenceAmount;
                            }
                            else
                            {
                                ParkSystemUtility.log.Error("ParkSystemDAL.ReCalculationMediumLarge方法webapi 更新平台车型重新计费失败，result_code" + or.result_code + ",return_msg:" + or.return_msg);
                                mode.PayDifferenceAmount = ActualAmount;
                                mode.ActualGetAmount = 0;
                            }
                        }
                        catch (Exception ex)
                        {
                            ParkSystemUtility.log.Error("ParkSystemDAL.ReCalculationMediumLarge方法webapi 更新平台车型重新计费，ex.Message：" + ex.Message);
                        }
                    }


                }
                else if (result == 1001 || result == 1002)
                {
                    ParkSystemUtility.log.Error("ParkSystemDAL.ReCalculationMediumLarge方法,数据处理失败，请联系数据管理员。 存储过程异常,result:" + result + ",errorMsg:" + errorMsg);
                    resultMsg = "数据处理失败，请联系数据管理员";
                }
                else if (result == 1003)
                {
                    ParkSystemUtility.log.Error("ParkSystemDAL.ReCalculationMediumLarge方法 该车无可用订单，没有入场纪录");
                    resultMsg = "该车无可用订单，没有入场纪录";
                }
                else if (result == 2001)
                {
                    ParkSystemUtility.log.Error("ParkSystemDAL.ReCalculationMediumLarge方法 存储过程异常：errorMsg:" + errorMsg);
                    resultMsg = "保存失败,errorMsg:" + errorMsg;
                }

            }
            catch (Exception ex)
            {
                ParkSystemUtility.log.Error("ParkSystemDAL.ReCalculationMediumLarge方法 小中大型车计费 ex.Message：" + ex.Message);
            }
            mode.resultVal = resultMsg;
            mode.result = result;
            mode.OrderCharge = OrderCharge;
            mode.DiscountAmount = DiscountAmount;
            mode.ActualAmount = ActualAmount;
            return mode;
            // int result = 1;//1是更新成功0是失败

        }
        #endregion

        #region 读取停车场信息BYparkingLotNo
        public DataTable GetParkingLotInfo(int dataType, string parkingLotNo)
        {

            paraDir.Clear();
            paraDir.Add("@DataType ", dataType);
            paraDir.Add("@ParkingLotNo ", parkingLotNo);

            DataSet ds = ParkSystemUtility.GetData(paraDir, "IPS_GetParkingLotInfo");
            DataTable dt = new DataTable();
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];

            }


            return dt;

        }
        #endregion

        #region 判断车辆是否白名单
        /// <summary>
        /// 判断车辆是否白名单
        /// </summary>
        /// <param name="DataType">--1 修改计算收费金额</param>
        /// <param name="OrderNo">订单编号ID</param>
        /// <param name="IsWhiteList" --1=是，0=否</param>
        /// <returns></returns>
        public int CheckWhilteList(int dataType, string licensePlateNo)
        {
            int IsWhiteList = 0;
            string ParkingLotId = Params.Settings.ParkLot.Id;//ConfigurationManager.AppSettings["ParkingLotId"];
            DataTable dtOrder = new DataTable();
            paraDir.Clear();
            paraDir.Add("@DataType ", dataType);
            paraDir.Add("@LicensePlateNo ", licensePlateNo);
            paraDir.Add("@ParkingLotId ", ParkingLotId);
            DataSet ds = ParkSystemUtility.GetData(paraDir, "IPS_CheckWhilteList");
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if(dt.Rows.Count > 0)
                    IsWhiteList = Convert.ToInt32(dt.Rows[0]["IsWhiteList"]);
            }


            return IsWhiteList;

        }
        #endregion

        #region 检查车位是否已占用
        /// <summary>
        /// 判断车辆是一位多车且车位已被占用
        /// </summary>
        /// <param name="licensePlateNo">车牌号</param>
        /// <returns></returns>
        public int CheckBerthNoOccupy(string licensePlateNo)
        {
            int result = 0;
            string ParkingLotId = Params.Settings.ParkLot.Id;
            DataTable dtOrder = new DataTable();
            try
            {
                paraDir.Clear();
                paraDir.Add("@ParkingLotId", ParkingLotId);
                paraDir.Add("@LicensePlateNo", licensePlateNo);
                DataSet ds = ParkSystemUtility.GetData(paraDir, "IPS_CheckBerthNoOccupy");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                        result = Convert.ToInt32(dt.Rows[0]["LeftBerthNum"]);
                }
            }
            catch (Exception ex)
            {
                ParkSystemUtility.log.Error("CheckMonthly方法发生异常：" + ex.Message);
            }
            return result;
        }
        #endregion

        #region 判断车辆是否包月车
        /// <summary>
        /// 判断车辆是否包月车
        /// </summary>
        /// <param name="licensePlateNo">车牌号</param>
        /// <returns></returns>
        public int CheckMonthly(string licensePlateNo)
        {
            int isMonthly = 0;
            string ParkingLotId = Params.Settings.ParkLot.Id;
            DataTable dtOrder = new DataTable();
            try
            {
                paraDir.Clear();
                paraDir.Add("@LicensePlateNo ", licensePlateNo);
                paraDir.Add("@ParkingLotId ", ParkingLotId);
                paraDir.Add("@LicensePlateType ", 0);
                DataSet ds = ParkSystemUtility.GetData(paraDir, "IPS_CheckMonthly");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                        isMonthly = Convert.ToInt32(dt.Rows[0]["IsMonthly"]);
                }
            }catch(Exception ex)
            {
                ParkSystemUtility.log.Error("CheckMonthly方法发生异常：" + ex.Message);
            }
            return isMonthly;
        }
        #endregion

        #region 查询车辆白名单或者包月车(天铭酒店)
        public void CheckWhiteMonthly(string licensePlateNo, int networkOk)
        {
            string ParkingLotId = Params.Settings.ParkLot.Id;
            DataTable dttOrder = GetOrderTable(1, licensePlateNo, "");
            int ChargeType = Convert.ToInt32(dttOrder.Rows[0]["ChargeType"]);
            if (ChargeType == 10)
            {
                if (networkOk == 0)
                {
                    ParkSystemUtility.log.Error("ParkSystemDAL.CheckWhiteMonthly方法webapi 网络不通，不能在线查询白名单包月信息，networkOk" + networkOk);

                }
                else
                {
                    string URL = Params.Settings.Serv.Url + "/api/WinForms/CheckWhiteMonthly";//ConfigurationManager.AppSettings["Host_URL"] 
                    var obj = new { LicensePlateNo = licensePlateNo, ParkingLotId = ParkingLotId };
                    string param = obj.ToJson();
                    int ChargeTypereturn = Convert.ToInt32(SortingLine.Utility.HttpOperate.Requst(URL, "POST", 5000, "application/json", "utf-8", param));
                    if (ChargeTypereturn != 10)
                    {
                        updateOrderChargeType(licensePlateNo, ChargeTypereturn);
                    }
                }
            }


        }
        #endregion

        #region 更新订单收费类型
        public void updateOrderChargeType(string licensePlateNo, int chargeType)
        {

            string errorMsg = string.Empty;

            string OrderNoReturn = string.Empty;
            try
            {
                string ParkingLotId = Params.Settings.ParkLot.Id;//ConfigurationManager.AppSettings["ParkingLotId"];

                paraDir.Clear();
                paraDir.Add("@ChargeType ", chargeType);
                paraDir.Add("@LicensePlateNo ", licensePlateNo);
                paraDir.Add("@ParkingLotId ", ParkingLotId);

                ParkSystemUtility.SaveData(paraDir, "IPS_UpdateOrderChargeType", ref errorMsg);


            }
            catch (Exception ex)
            {

                ParkSystemUtility.log.Error("ParkSystemDAL.updateOrderChargeType方法 :" + ex.Message + ",licensePlateNo:" + licensePlateNo + ",chargeType:" + chargeType);
            }


        }
        #endregion

        #region 保存上传失败的订单
        /// <summary>
        /// 保存上传失败的订单
        /// </summary>
        /// <param name="dataType">1:生成,2:标记成已上传</param>
        /// <param name="orderNoStr">订单列表</param>
        /// <param name="errorType">失败类别</param>
        public int SaveBrokenNetTransOrder(int dataType, string orderNoStr, int errorType, int orderType, ref string errorMsg)
        {
            paraDir.Clear();
            paraDir.Add("@DataType", dataType);
            paraDir.Add("@OrderNoStr", orderNoStr);
            paraDir.Add("@ErrorType", errorType);
            paraDir.Add("@OrderType", orderType);
            int errorId = ParkSystemUtility.SaveData(paraDir, "IPS_SaveBrokenNetTransOrder", ref errorMsg);
            return errorId;
        }
        #endregion

        #region 读取断网的订单
        /// <summary>
        /// 读取断网的订单
        /// </summary>
        /// <returns></returns>
        private DataSet GetBrokenNetOrder()
        {
            return ParkSystemUtility.GetData(null, "IPS_GetBrokenNetOrder");
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
            int result = 0;
            string returnVal = string.Empty;
            string orderNo = String.Empty;
            string imgBase64 = String.Empty;
            DataSet dsBrokenNetOrder = this.GetBrokenNetOrder();
            if (dsBrokenNetOrder == null || dsBrokenNetOrder.Tables.Count == 0)
                return;
            DataTable dtBrokenNetOrder = dsBrokenNetOrder.Tables[0];
            DataTable dtBrokenNetPayDetail = dsBrokenNetOrder.Tables[1];
            OrderModel brokenNetOrderModel = new OrderModel();
            if (dtBrokenNetOrder.Rows.Count > 0)
            {
                if (netWorkResult == 1)//联网时
                {
                    try
                    {
                        //循环上传订单
                        foreach (DataRow brokenNetOrder in dtBrokenNetOrder.Rows)
                        {
                            int orderType = Convert.ToInt32(brokenNetOrder["DataType"]);
                            int dataType = 1;
                            imgBase64 = ParkSystemUtility.GetImageByte(brokenNetOrder["PictureAddr"].ToString());
                            if (imgBase64 != String.Empty)
                            {
                                //出入场共同参数
                                orderNo = brokenNetOrder["OrderNo"].ToString();
                                brokenNetOrderModel.Pb = imgBase64;
                                brokenNetOrderModel.PictureName = brokenNetOrder["PictureName"].ToString();
                                brokenNetOrderModel.OrderNo = brokenNetOrder["OrderNo"].ToString();
                                brokenNetOrderModel.InDate = Convert.ToDateTime(brokenNetOrder["InDate"]);
                                brokenNetOrderModel.ChargeType = Convert.ToInt32(brokenNetOrder["ChargeType"]);
                                brokenNetOrderModel.OrderType = Convert.ToInt32(brokenNetOrder["OrderType"]);
                                brokenNetOrderModel.ParkingLotId = Convert.ToInt32(brokenNetOrder["ParkingLotId"]);
                                brokenNetOrderModel.BerthNo = brokenNetOrder["BerthNo"].ToString();
                                brokenNetOrderModel.MemberId = Convert.ToInt32(brokenNetOrder["MemberId"]);
                                brokenNetOrderModel.LicensePlateNo = brokenNetOrder["LicensePlateNo"].ToString();
                                brokenNetOrderModel.LicensePlateType = Convert.ToInt32(brokenNetOrder["LicensePlateType"]);
                                brokenNetOrderModel.CarType = Convert.ToInt32(brokenNetOrder["CarType"]);
                                if (orderType == 1)  //续传入场订单
                                    dataType = 1;
                                else
                                    dataType = 3;    //续传出场订单
                                brokenNetOrderModel.dataType = dataType;
                                if (dataType == 3)   //出场订单参数
                                {
                                    brokenNetOrderModel.OutDate = Convert.ToDateTime(brokenNetOrder["OutDate"]);
                                    brokenNetOrderModel.ParkingTime = Convert.ToInt32(brokenNetOrder["ParkingTime"]);
                                    brokenNetOrderModel.OrderCharge = Convert.ToDouble(brokenNetOrder["OrderCharge"]);
                                    brokenNetOrderModel.ActualAmount = Convert.ToDouble(brokenNetOrder["ActualAmount"]);
                                    brokenNetOrderModel.ActualGetAmount = Convert.ToDouble(brokenNetOrder["ActualGetAmount"]);
                                    brokenNetOrderModel.DiscountAmount = Convert.ToDouble(brokenNetOrder["DiscountAmount"]);
                                    brokenNetOrderModel.OrderComplete = Convert.ToInt32(brokenNetOrder["OrderComplete"]);
                                    brokenNetOrderModel.State = 30;
                                    brokenNetOrderModel.Note = brokenNetOrder["Note"].ToString();
                                    brokenNetOrderModel.RoadRateNo = brokenNetOrder["RoadRateNo"].ToString();
                                    //读取支付明细
                                    DataRow[] rows = dtBrokenNetPayDetail.Select("OrderNo = '" + orderNo + "'");
                                    if (rows.Length > 0)
                                    {
                                        brokenNetOrderModel.PayNo = "";
                                        brokenNetOrderModel.PayMoney = Convert.ToDouble(rows[0]["Amount"]);
                                        brokenNetOrderModel.PayType = 4;  //现金支付
                                        brokenNetOrderModel.PayNote = rows[0]["Note"].ToString();
                                        brokenNetOrderModel.ChargeEmp = ParkSystemUtility.chargeEmp;
                                    }
                                    else
                                        brokenNetOrderModel.PayMoney = 0;
                                }
                                string modeJson = JsonConvert.SerializeObject(brokenNetOrderModel);
                                string URL = Params.Settings.Serv.Url;// ConfigurationManager.AppSettings["Host_URL"];
                                string json = WebApiHelper.HttpPost(URL + "/api/WinForms/PostFiles", modeJson);
                                OrderReturn or = JsonHelper.DeserializeObject<OrderReturn>(json);
                                //if (or.result_code != 1000)
                                if (or == null)
                                    ParkSystemUtility.log.Error("断网续传，上传订单失败，订单号：" + orderNo + "，错误信息：" + or.return_msg);
                                else
                                {
                                    //上传成功，更新本地续传订单状态
                                    result = this.SaveBrokenNetTransOrder(2, orderNo, 0, orderType, ref returnVal);
                                    if (result != 1000)
                                        ParkSystemUtility.log.Error("续传订单后本地更新失败，订单号：" + orderNo + " 错误：" + returnVal);
                                }
                            }
                            else
                                ParkSystemUtility.log.Error("续传订单图片Base64为空，订单号：" + orderNo);

                        }
                    }
                    catch (Exception ex)
                    {
                        ParkSystemUtility.log.Error("ParkSystemDAL.OrderRenewal方法发生异常：" + ex.Message);
                    }
                }
                else
                {
                    ParkSystemUtility.log.Error("断网续传时判断网络状态异常");
                }
            }


        }

        #endregion

        #region 读取出场未识别订单
        /// <summary>
        /// 读取出场未识别订单
        /// </summary>
        /// <param name="parkingLotId">停车场Id</param>
        /// <returns></returns>
        public DataTable GetUnidentifiedOrder(int parkingLotId)
        {
            string ParkingLotId = Params.Settings.ParkLot.Id;
            DataTable dtOrderUnidentified = new DataTable();
            paraDir.Clear();
            paraDir.Add("@DataType", 1);
            paraDir.Add("@ParkingLotId", ParkingLotId);
            DataSet ds = ParkSystemUtility.GetData(paraDir, "IPS_GetUnidentifiedOrder_local");
            if (ds != null && ds.Tables.Count > 0)
                dtOrderUnidentified = ds.Tables[0];
            return dtOrderUnidentified;
        }

        #endregion

        //FrmMain2业务逻辑//
        #region 保存数据
        public void SaveOrder_Count(int dataType, OrderCountModel mode)
        {
            int result = 0;
            string returnVal = string.Empty;

            string errorMsg = string.Empty;
            string OrderNo = string.Empty;
            string ParkingLotId = Params.Settings2.ID;//ConfigurationManager.AppSettings["ParkingLotId"];

            //生成停车信息DataTable
            DataTable dtAttachment = new DataTable("IPS_Order_Count");
            //todo
            dtAttachment.Columns.Add("OrderNo", typeof(string));
            dtAttachment.Columns.Add("ParkingLotId", typeof(string));
            dtAttachment.Columns.Add("LicensePlateNo", typeof(string));
            dtAttachment.Columns.Add("LicensePlateType", typeof(int));
            dtAttachment.Columns.Add("InDate", typeof(string));
            dtAttachment.Columns.Add("OutDate", typeof(string));
            dtAttachment.Columns.Add("ParkingTime", typeof(int));
            dtAttachment.Columns.Add("State", typeof(int));
            dtAttachment.Columns.Add("Note", typeof(string));
            dtAttachment.Columns.Add("EntranceNo", typeof(string));
            dtAttachment.Columns.Add("RoadRateNo", typeof(string));
            DataRow dataRow = dtAttachment.NewRow();
            // dataRow["OrderNo"] = orderNo;
            dataRow["ParkingLotId"] = ParkingLotId;
            dataRow["LicensePlateNo"] = mode.LicensePlateNo;
            dataRow["LicensePlateType"] = mode.LicensePlateType;
            if (dataType == 1)
            {
                dataRow["InDate"] = mode.InDate.ToString("yyyy-MM-dd HH:mm:ss");
                dataRow["EntranceNo"] = mode.EntranceNo;
            }
            else
            {
                dataRow["OutDate"] = mode.OutDate.ToString("yyyy-MM-dd HH:mm:ss");
                dataRow["RoadRateNo"] = mode.RoadRateNo;
            }


            dtAttachment.Rows.Add(dataRow);



            result = ParkSystemUtility.SaveData(dataType, dtAttachment, "IPS_SaveOrder_Count", ref errorMsg);
            ParkSystemUtility.log.Error("FrmMain2,SaveOrder_Count:result" + result + ",LicensePlateNo:" + mode.LicensePlateNo);
            // return result;

        }

        #endregion

    }
}
