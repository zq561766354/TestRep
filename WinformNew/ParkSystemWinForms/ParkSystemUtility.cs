using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;

namespace ParkSystemWinForms
{
    public class ParkSystemUtility
    {
        private static string sKey = "Jnipark+";
       //public static string conStr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
     
        public static string conStr=Base64Decode(Decrypt(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString));
        public static LogHelper log = LogFactory.GetLogger("PsFormLog");

        public static int timout;
        public static int chargeEmp;

      
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="pToDecrypt">需要解密的</param>
        /// <param name="sKey">密匙</param>
        /// <returns></returns>
        private static string Decrypt(string pToDecrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            // 如果两次密匙不一样，这一步可能会引发异常
            cs.FlushFinalBlock();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(string result)
        {
            return Base64Decode(Encoding.UTF8, result);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(Encoding encodeType, string result)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        #region 将图片生成二进制流并以Base64返回
        /// <summary>
        /// 将图片生成二进制流并以Base64返回
        /// </summary>
        /// <param name="path">图片地址</param>
        /// <returns></returns>
        public static string GetImageByte(String path)
        {
            string base64Image = String.Empty;
            try
            {
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read); //将图片以文件流的形式进行保存
                BinaryReader br = new BinaryReader(fs);
                byte[] imgBytesIn = br.ReadBytes((int)fs.Length); //将流读入到字节数组中
                base64Image = Convert.ToBase64String(imgBytesIn);
            }catch(Exception ex)
            {
                log.Error("图片生成Base64发生异常:" +  ex.Message);
            }
            return base64Image;
        }
        #endregion


        #region 读取数据通用方法
        /// <summary>
        /// 读取数据通用方法
        /// </summary>
        /// <param name="paraDic">参数的Dictionary</param>
        /// <param name="proceName">存储过程</param>
        /// <returns></returns>
        public static DataSet GetData(Dictionary<String, Object> paraDic, string proceName)
        {
            SqlParameter[] paras = null;
            if (paraDic != null && paraDic.Count > 0)
            {
                paras = new SqlParameter[paraDic.Count];
                int i = 0;
                foreach (KeyValuePair<String, Object> parasKeyVal in paraDic)
                {
                    paras[i] = CreateDbParameter(parasKeyVal.Key, parasKeyVal.Value);
                    i++;
                }
            }

            DataSet ds = SQLHelper.ExecuteDataSet(conStr, System.Data.CommandType.StoredProcedure, proceName, paras);
            return ds;
        }
        #endregion
        /// <summary>
        /// 根据配置文件中所配置的数据库类型
        /// 来创建相应数据库的参数对象
        /// </summary>
        /// <returns></returns>
        public static SqlParameter CreateDbParameter(string paramName, object value)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = paramName;
            param.Value = value;
            return param;
        }
        #region 保存数据通用方法 
        /// <summary>
        /// 保存数据通用方法
        /// </summary>
        /// <param name="paraDic">参数的Dictionary</param>
        /// <param name="proceName">存储过程</param>
        /// <returns></returns>
        public static int SaveData(Dictionary<String, Object> paraDic, string proceName, ref string errorMsg)
        {
            int errorId = 1000;
            errorMsg = String.Empty;
            SqlParameter returnPara = new SqlParameter("@return", errorId);
            returnPara.Direction = ParameterDirection.ReturnValue;
          
            SqlParameter[] paras = null;
            int i = 0;
            try
            {
                if (paraDic != null && paraDic.Count > 0)
                {
                    paras = new SqlParameter[paraDic.Count + 1];
                    foreach (KeyValuePair<String, Object> parasKeyVal in paraDic)
                    {
                        paras[i] = CreateDbParameter(parasKeyVal.Key, parasKeyVal.Value);
                        i++;
                    }
                    paras[i] = returnPara;
                  
                }
                else
                {
                    paras = new SqlParameter[i + 1];
                    paras[i] = returnPara;
                   
                }


                int count = SQLHelper.ExecuteNonQuery(conStr, CommandType.StoredProcedure, proceName, paras);
                errorId = Convert.ToInt32(paras[i].Value);
                
            }
            catch (Exception ex)
            {
                errorId = 2001;
                errorMsg = "ParkSystemUtility.SaveData方法发生异常:" + ex.Message;
            }
            return errorId;
        }
        #endregion
        #region 保存数据  保存当班收费纪录
        /// <summary>
        /// 保存数据通用方法
        /// </summary>
        /// <param name="paraDic">参数的Dictionary</param>
        /// <param name="proceName">存储过程</param>
        /// <returns></returns>
        public static int SaveData(Dictionary<String, Object> paraDic, string proceName, ref string errorMsg, ref double amountSum)
        {
            int errorId = 1000;
            errorMsg = String.Empty;
            SqlParameter returnPara = new SqlParameter("@return", errorId);
            returnPara.Direction = ParameterDirection.ReturnValue;
            SqlParameter amountSumPara = new SqlParameter("@AmountSum", amountSum);
            amountSumPara.Direction = ParameterDirection.Output;
            SqlParameter[] paras = null;
            int i = 0;
            try
            {
                if (paraDic != null && paraDic.Count > 0)
                {
                    paras = new SqlParameter[paraDic.Count + 2];
                    foreach (KeyValuePair<String, Object> parasKeyVal in paraDic)
                    {
                        paras[i] = CreateDbParameter(parasKeyVal.Key, parasKeyVal.Value);
                        i++;
                    }
                    paras[i] = returnPara;
                    paras[i + 1] = amountSumPara;

                }
                else
                {
                    paras = new SqlParameter[i + 2];
                    paras[i] = returnPara;
                    paras[i + 1] = amountSumPara;

                }


                int count = SQLHelper.ExecuteNonQuery(conStr, CommandType.StoredProcedure, proceName, paras);
                errorId = Convert.ToInt32(paras[i].Value);
                amountSum = Convert.ToDouble(paras[i+1].Value);
            }
            catch (Exception ex)
            {
                errorId = 2001;
                errorMsg = "ParkSystemUtility.SaveData方法发生异常:" + ex.Message;
            }
            return errorId;
        }
        #endregion
        #region 保存数据通用方法 生成当班纪录专用
        /// <summary>
        /// 保存数据通用方法
        /// </summary>
        /// <param name="paraDic">参数的Dictionary</param>
        /// <param name="proceName">存储过程</param>
        /// <returns></returns>
        public static int SaveData(Dictionary<String, Object> paraDic, string proceName, ref string errorMsg, ref string workNoReturn, ref DateTime startWorkTime, ref double cashAmountReturn)
        {
            int errorId = 1000;
            errorMsg = String.Empty;
            SqlParameter returnPara = new SqlParameter("@return", errorId);
            returnPara.Direction = ParameterDirection.ReturnValue;
           // SqlParameter workNoPara = new SqlParameter("@WorkNoReturn", workNoReturn);
            SqlParameter workNoPara = new SqlParameter("@WorkNoReturn", SqlDbType.NVarChar, 50) { Value = workNoReturn };
            workNoPara.Direction = ParameterDirection.Output;
            SqlParameter startWorkTimePara = new SqlParameter("@StartWorkTime", startWorkTime);
            startWorkTimePara.Direction = ParameterDirection.Output;
            SqlParameter cashAmountPara = new SqlParameter("@CashAmountReturn", cashAmountReturn);
            cashAmountPara.Direction = ParameterDirection.Output;
            SqlParameter[] paras = null;
            int i = 0;
            try
            {
                if (paraDic != null && paraDic.Count > 0)
                {
                    paras = new SqlParameter[paraDic.Count + 4];
                    foreach (KeyValuePair<String, Object> parasKeyVal in paraDic)
                    {
                        paras[i] = CreateDbParameter(parasKeyVal.Key, parasKeyVal.Value);
                        i++;
                    }
                    paras[i] = returnPara;
                    paras[i+1] = workNoPara;
                    paras[i + 2] = startWorkTimePara;
                    paras[i + 3] = cashAmountPara;
                }
                else
                {
                    paras = new SqlParameter[i + 4];
                    paras[i] = returnPara;
                    paras[i + 1] = workNoPara;
                    paras[i + 2] = startWorkTimePara;
                    paras[i + 3] = cashAmountPara;
                }


                int count = SQLHelper.ExecuteNonQuery(conStr, CommandType.StoredProcedure, proceName, paras);
                errorId = Convert.ToInt32(paras[i].Value);
                workNoReturn = paras[i+1].Value.ToString();
                startWorkTime = Convert.ToDateTime(paras[i+2].Value);
                cashAmountReturn = Convert.ToDouble(paras[i + 3].Value);
            }
            catch (Exception ex)
            {
                errorId = 2001;
                errorMsg = "ParkSystemUtility.SaveData方法发生异常:" + ex.Message;
            }
            return errorId;
        }
        #endregion
        #region 保存数据通用方法 判断黄牌车是否需要重新计算费用专用
        /// <summary>
        /// 保存数据通用方法
        /// </summary>
        /// <param name="paraDic">参数的Dictionary</param>
        /// <param name="proceName">存储过程</param>
        /// <returns></returns>
        public static int SaveData(Dictionary<String, Object> paraDic, string proceName, ref string errorMsg, ref int ReCalculationReturn, ref string  OrderNoReturn)
        {
            int errorId = 1000;
            errorMsg = String.Empty;
            SqlParameter returnPara = new SqlParameter("@return", errorId);
            returnPara.Direction = ParameterDirection.ReturnValue;
            // SqlParameter workNoPara = new SqlParameter("@WorkNoReturn", workNoReturn);
            SqlParameter OrderNoPara = new SqlParameter("@OrderNoReturn", SqlDbType.NVarChar, 50) { Value = OrderNoReturn };
            OrderNoPara.Direction = ParameterDirection.Output;
            SqlParameter ReCalculationPara = new SqlParameter("@ReCalculationReturn", ReCalculationReturn);
            ReCalculationPara.Direction = ParameterDirection.Output;
       
            SqlParameter[] paras = null;
            int i = 0;
            try
            {
                if (paraDic != null && paraDic.Count > 0)
                {
                    paras = new SqlParameter[paraDic.Count + 3];
                    foreach (KeyValuePair<String, Object> parasKeyVal in paraDic)
                    {
                        paras[i] = CreateDbParameter(parasKeyVal.Key, parasKeyVal.Value);
                        i++;
                    }
                    paras[i] = returnPara;
                    paras[i + 1] = OrderNoPara;
                    paras[i + 2] = ReCalculationPara;
                   
                }
                else
                {
                    paras = new SqlParameter[i + 3];
                    paras[i] = returnPara;
                    paras[i + 1] = OrderNoPara;
                    paras[i + 2] = ReCalculationPara;
                   
                }


                int count = SQLHelper.ExecuteNonQuery(conStr, CommandType.StoredProcedure, proceName, paras);
                errorId = Convert.ToInt32(paras[i].Value);
                OrderNoReturn = paras[i + 1].Value.ToString();
                ReCalculationReturn = Convert.ToInt32(paras[i + 2].Value);
              
            }
            catch (Exception ex)
            {
                errorId = 2001;
                errorMsg = "ParkSystemUtility.SaveData方法发生异常:" + ex.Message;
            }
            return errorId;
        }
        #endregion

        #region 保存数据  小中大型车计费专用
        /// <summary>
        /// 保存数据专用方法
        /// </summary>
        /// <param name="paraDic">参数的Dictionary</param>
        /// <param name="proceName">存储过程</param>
        /// <returns></returns>
        public static int SaveData(Dictionary<String, Object> paraDic, string proceName, ref string errorMsg, ref double OrderCharge, ref double DiscountAmount, ref double ActualAmount)
        {
            int errorId = 1000;
            errorMsg = String.Empty;
            SqlParameter returnPara = new SqlParameter("@return", errorId);
            returnPara.Direction = ParameterDirection.ReturnValue;
            SqlParameter OrderChargePara = new SqlParameter("@OrderCharge", OrderCharge);
            OrderChargePara.Direction = ParameterDirection.Output;
            SqlParameter DiscountAmountPara = new SqlParameter("@DiscountAmount", DiscountAmount);
            DiscountAmountPara.Direction = ParameterDirection.Output;
            SqlParameter ActualAmountPara = new SqlParameter("@ActualAmount", ActualAmount);
            ActualAmountPara.Direction = ParameterDirection.Output;
            SqlParameter[] paras = null;
            int i = 0;
            try
            {
                if (paraDic != null && paraDic.Count > 0)
                {
                    paras = new SqlParameter[paraDic.Count + 4];
                    foreach (KeyValuePair<String, Object> parasKeyVal in paraDic)
                    {
                        paras[i] = CreateDbParameter(parasKeyVal.Key, parasKeyVal.Value);
                        i++;
                    }
                    paras[i] = returnPara;
                    paras[i + 1] = OrderChargePara;
                    paras[i + 2] = DiscountAmountPara;
                    paras[i + 3] = ActualAmountPara;

                }
                else
                {
                    paras = new SqlParameter[i + 4];
                    paras[i] = returnPara;
                    paras[i + 1] = OrderChargePara;
                    paras[i + 2] = DiscountAmountPara;
                    paras[i + 3] = ActualAmountPara;

                }


                int count = SQLHelper.ExecuteNonQuery(conStr, CommandType.StoredProcedure, proceName, paras);
                errorId = Convert.ToInt32(paras[i].Value);
                OrderCharge = Convert.ToDouble(paras[i + 1].Value);
                DiscountAmount = Convert.ToDouble(paras[i + 2].Value);
                ActualAmount = Convert.ToDouble(paras[i + 3].Value);
            }
            catch (Exception ex)
            {
                errorId = 2001;
                errorMsg = "ParkSystemUtility.SaveData小中大型车计费专用方法发生异常:" + ex.Message;
            }
            return errorId;
        }
        #endregion
        #region 保存数据
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="datatype">1=新增，2=修改状态，3修改</param>
        /// <param name="dtData">保存表数据</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public static int SaveData(int dataType, DataTable dtData, string procName, ref string errorMsg)
        {
            int errorId = 1000;
            errorMsg = "";
            string xmlPara = String.Empty;
            try
            {
                xmlPara = ConvertDataTableToXml(dtData);
               // log.Error(xmlPara);
                SqlParameter returnPara = new SqlParameter("@return", errorId);
                returnPara.Direction = ParameterDirection.ReturnValue;
               
                SqlParameter[] paras = {
                                          new SqlParameter("@DataType", dataType),
                                          new SqlParameter("@XMLData", xmlPara),
                                          returnPara
                                      };
                int count = SQLHelper.ExecuteNonQuery(conStr, CommandType.StoredProcedure, procName, paras);
                errorId = Convert.ToInt32(paras[2].Value);
                /*if (count < 0)   //存储过程发生异常或者回滚返回-1
                {
                    errorId = 1100;
                    errorMsg = "存储过程" + procName + "保存失败";
                }*/
            }
            catch (Exception ex)
            {
                errorId = 2001;
                errorMsg = "ParkSystemUtility.SaveData方法发生异常:" + ex.Message;
            }
            return errorId;
        }
        #endregion
        #region 保存数据(主表+从表列表)
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="datatype">1=新增，2=修改状态，3修改</param>
        /// <param name="dtData">保存表数据</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public static int SaveData(int dataType, DataTable dtData, List<DataTable> dtChilds, string procName, ref string errorMsg)
        {
            int errorId = 1000;
            errorMsg = "";
            string xmlPara = String.Empty;
            try
            {
                xmlPara = ConvertDataTableToXml(dtData, dtChilds);
                //log.Debug(xmlPara);
                SqlParameter returnPara = new SqlParameter("@return", errorId);
                returnPara.Direction = ParameterDirection.ReturnValue;
                SqlParameter[] paras = {
                                          new SqlParameter("@DataType", dataType),
                                          new SqlParameter("@XMLData", xmlPara),
                                          returnPara
                                      };
                int count = SQLHelper.ExecuteNonQuery(conStr, CommandType.StoredProcedure, procName, paras);
                errorId = Convert.ToInt32(paras[2].Value);
                /*if (count < 0)   //存储过程发生异常或者回滚返回-1
                {
                    errorId = 1100;
                    errorMsg = "存储过程" + procName + "保存失败";
                }*/
            }
            catch (Exception ex)
            {
                errorId = 2001;
                errorMsg = "ParkSystemDAL.SaveData方法发生异常:" + ex.Message;
            }
            return errorId;
        }
        #endregion
        #region 保存数据(主表+从表)--保存订单专用
        /// <summary>
        /// 保存数据(主表+从表)
        /// </summary>
        /// <param name="datatype">1=新增，2=修改状态，3修改</param>
        /// <param name="dtMain">主表</param>
        /// <param name="dtChild">从表List</param>
        /// <param name="procName">存储过程名称</param>
        /// <returns></returns>
        public static int SaveData(int dataType, DataTable dtMain, DataTable dtChild, string procName, ref string errorMsg, ref string OrderNo, ref int BerthNum, ref int ChargeTypeReturn, ref int MonthLeftDayReturn)
        {
            int errorId = 1000;
            errorMsg = "";
            try
            {
                string xmlPara = ConvertDataTableToXml(dtMain, dtChild);
                SqlParameter returnPara = new SqlParameter("@return", errorId);
                returnPara.Direction = ParameterDirection.ReturnValue;
                SqlParameter OrderNoPara = new SqlParameter("@OrderNoReturn", SqlDbType.NVarChar, 50){ Value = OrderNo};
                OrderNoPara.Direction = ParameterDirection.Output;
                SqlParameter BerthNumPara = new SqlParameter("@BerthNum", BerthNum);
                BerthNumPara.Direction = ParameterDirection.Output;
                SqlParameter ChargeTypePara = new SqlParameter("@ChargeTypeReturn", ChargeTypeReturn);
                ChargeTypePara.Direction = ParameterDirection.Output;
                SqlParameter MonthLeftDayPara = new SqlParameter("@MonthLeftDayReturn", MonthLeftDayReturn);
                MonthLeftDayPara.Direction = ParameterDirection.Output;
                SqlParameter[] paras = {
                                          new SqlParameter("@DataType", dataType),
                                          new SqlParameter("@XMLData", xmlPara),
                                          OrderNoPara,
                                          BerthNumPara,
                                          ChargeTypePara,
                                          MonthLeftDayPara,
                                          returnPara
                                      };
                int count = SQLHelper.ExecuteNonQuery(conStr, CommandType.StoredProcedure, procName, paras);
                errorId = Convert.ToInt32(paras[6].Value);
                BerthNum = Convert.ToInt32(paras[3].Value);
                ChargeTypeReturn = Convert.ToInt32(paras[4].Value);
                //if()
                MonthLeftDayReturn = Convert.ToInt32(paras[5].Value);
                if (dataType<3)
                    OrderNo = paras[2].Value.ToString();
                /*if (count < 0)
                {
                    errorId = 1100;
                    errorMsg = "存储过程" + procName + "保存失败";
                }*/

                if (errorId != 1000)
                {
                    ParkSystemUtility.log.Debug("保存订单专用:dataType:" + dataType + "xml:" + xmlPara);
                }
            }
            catch (Exception ex)
            {
                errorId = 2001;
                errorMsg = "ParkSystemDAL.SaveData方法发生异常:" + ex.Message;
            }
           
            return errorId;
        }
        #endregion
        #region  DataTable生成XML
        /// <summary>
        /// DataTable生成XML(集合)
        /// </summary>
        /// <typeparam name="dt">DataTable数据集</typeparam>
        /// <returns></returns>
        public static string ConvertDataTableToXml(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendLine("<" + dt.TableName + ">");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        sb.AppendLine("<Rows>");
                        foreach (DataColumn col in dt.Columns)
                        {
                            sb.AppendLine("<" + col.ColumnName + ">");
                            sb.AppendLine("<![CDATA[" + row[col].ToString() + "]]>");
                            sb.AppendLine("</" + col.ColumnName + ">");
                        }
                        sb.AppendLine("</Rows>");
                    }
                }
                sb.AppendLine("</" + dt.TableName + ">");
            }
            catch (Exception ex)
            { }
            return sb.ToString();
        }
        #endregion
        #region  DataTable生成XML,主从表
        /// <summary>
        /// DataTable生成XML,主从表
        /// </summary>
        /// <typeparam name="dtMain">主表</typeparam>
        /// <typeparam name="dtChild">从表列表</typeparam>
        /// <returns></returns>
        public static string ConvertDataTableToXml(DataTable dtMain, List<DataTable> dtChild)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendLine("<" + dtMain.TableName + ">");
                if (dtMain.Rows.Count > 0)
                {
                    foreach (DataRow row in dtMain.Rows)
                    {
                        sb.AppendLine("<Rows>");

                        foreach (DataColumn col in dtMain.Columns)
                        {
                            sb.AppendLine("<" + col.ColumnName + ">");
                            sb.AppendLine("<![CDATA[" + row[col].ToString() + "]]>");
                            sb.AppendLine("</" + col.ColumnName + ">");
                        }

                        //if (dtChild != null)
                        //sb.AppendLine(ConvertDataTableToXml(dtChild));

                        if (dtChild != null)
                        {
                            foreach (DataTable dt in dtChild)
                            {
                                sb.AppendLine(ConvertDataTableToXml(dt));
                            }
                        }
                        sb.AppendLine("</Rows>");
                    }
                }
                sb.AppendLine("</" + dtMain.TableName + ">");
            }
            catch (Exception ex)
            { }
            return sb.ToString();
        }
        #endregion
        #region  DataTable生成XML,主从表
        /// <summary>
        /// DataTable生成XML,主从表
        /// </summary>
        /// <typeparam name="dtMain">主表</typeparam>
        /// <typeparam name="dtChild">从表</typeparam>
        /// <returns></returns>
        public static string ConvertDataTableToXml(DataTable dtMain, DataTable dtChild)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendLine("<" + dtMain.TableName + ">");
                if (dtMain.Rows.Count > 0)
                {
                    foreach (DataRow row in dtMain.Rows)
                    {
                        sb.AppendLine("<Rows>");
                        foreach (DataColumn col in dtMain.Columns)
                        {
                            sb.AppendLine("<" + col.ColumnName + ">");
                            sb.AppendLine("<![CDATA[" + row[col].ToString() + "]]>");
                            sb.AppendLine("</" + col.ColumnName + ">");
                        }
                        if (dtChild != null)
                            sb.AppendLine(ConvertDataTableToXml(dtChild));
                        sb.AppendLine("</Rows>");
                    }
                }
                sb.AppendLine("</" + dtMain.TableName + ">");
            }
            catch (Exception ex)
            { }
            return sb.ToString();
        }
        #endregion
    }
}
