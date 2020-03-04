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
       ParkSystemDAL PDAL = new ParkSystemDAL();
        #region 保存数据
       public  OrderModel SaveOrder(int dataType, string pcColor, string LicensePlateNo, string PictureAddr, string PictureName, string tmpPictureAddr, string tmpPictureName, string pb, DateTime dateTime,int memberId)
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
           mode.MemberId = memberId;
           mode.InDate = dateTime;
           mode.OutDate = dateTime;
           mode.LicensePlateType =Convert.ToInt32(LicensePlateType);
           mode.LicensePlateNo = LicensePlateNo;
           mode.PictureAddr = PictureAddr;
           mode.PictureName = PictureName;
           mode.TmpPictureAddr = tmpPictureAddr;
           mode.TmpPictureName = tmpPictureName;
           mode.Pb = pb;

           modeResult = PDAL.SaveOrder(dataType, mode);
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
           string picPath =null;
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
                   picPath = PictureAddr + @"\" + PictureName;
                   p.picPath = picPath;
                   p.OrderNo = dt.Rows[i]["OrderNo"].ToString();
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
       public  int UpdateLicensePlateNo(string licensePlateNo,string orderNo)
       {
           
           int resultnum = 1;
           int dataType = 4;
           resultnum = PDAL.updateLicensePlateNo(dataType, licensePlateNo, orderNo);
           if (resultnum == 1000)
           {
               string URL = ConfigurationManager.AppSettings["Host_URL"];
             
              // int PayType = 1;
               WebApiHelper.HttpPost(URL + "/api/WinForms/PostFiles", "{dataType:\"" + dataType + "\",OrderNo:\"" + orderNo + "\",LicensePlateNo:\"" + licensePlateNo + "\"}");
           }
           return resultnum;
       }
       #endregion
       #region 更新订单状态
       public OrderModel UpdateState(int dataType, string orderNo, string payNo, int payType, double payMoney, int chargeEmp, string note, string pb, string pictureName)
       {
           OrderModel modeResult = new OrderModel();
           modeResult = PDAL.updateState(dataType, orderNo, payNo, payType, payMoney, chargeEmp, note, pb, pictureName);

           //int berthNum=-1;
           //OrderReturn or = PDAL.updateStateToYun(dataType, mode, payType, chargeEmp);
           //berthNum = or.berthNum;
           //resultnum = PDAL.updateState(dataTyte, orderNo, actucalAmount,note);
           //if (resultnum == 1)
           //{
           //    string URL = ConfigurationManager.AppSettings["Host_URL"];
           //    int dataType = 3;
           //    int PayType = 1;
           //    WebApiHelper.HttpPost(URL + "/api/WinForms/PostFiles", "{dataType:\"" + dataType + "\",OrderNo:\"" + orderNo + "\",ActualAmount:\"" + actucalAmount + "\",PayType:\"" + PayType + "\"}");
           //}
           return modeResult;
       }
       #endregion
       #region 查订单列表BY LicensePlateNo
       public  List<OrderModel> GetOrderList(string LicensePlateNo)
       {
          
           List<OrderModel> orderList = new List<OrderModel>();
           DataTable dt = PDAL.GetOrderTable(1,LicensePlateNo,null);
           if (dt.Rows.Count > 0 && dt != null)
           {
               for (int i = 0; i < dt.Rows.Count; i++)
               {

                   OrderModel orderModel = new OrderModel();
                   orderModel.LicensePlateNo = dt.Rows[i]["LicensePlateNo"].ToString();
                   orderModel.InDate = Convert.ToDateTime(dt.Rows[i]["InDate"]);
                  
                   orderModel.OrderId = Convert.ToInt32(dt.Rows[i]["OrderId"]);
                   int LicensePlateType = Convert.ToInt32(dt.Rows[i]["LicensePlateType"]); 

                   string PictureAddr = dt.Rows[i]["PictureAddr"].ToString();
                   string PictureName = dt.Rows[i]["PictureName"].ToString();
                   orderModel.picPath = PictureAddr + @"\" + PictureName;

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
                   orderModel.CarColor = color;
                   orderModel.StateDes = dt.Rows[i]["StateDes"].ToString();
                   //int state = Convert.ToInt32(dt.Rows[i]["State"]);
                   //if (state == 10)
                   //{
                   //    orderModel.StateDes = "订单进行中";
                   //}
                   //else if (state == 15)
                   //{
                   //    orderModel.StateDes = "支付未驶离";
                   //}
                   //else if (state == 20)
                   //{
                   //    orderModel.StateDes = "欠费驶离";
                   //}
                   //else if (state == 30)
                   //{
                   //    orderModel.StateDes = "订单结束";
                   //}
                   //else
                   //{
                   //    orderModel.StateDes = "异常";
                   //}
                   orderList.Add(orderModel);
               }
                   
           }
           return orderList;
       }
       #endregion
       #region 删除异常订单
       public  string  deleteOrder(string orderIdStr)
       {
           string returnVal = String.Empty;
          
           returnVal = PDAL.deleteOrder(orderIdStr);

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
      
    }
}
