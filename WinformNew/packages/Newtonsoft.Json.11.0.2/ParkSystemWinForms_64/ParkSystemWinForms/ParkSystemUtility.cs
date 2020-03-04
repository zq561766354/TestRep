using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ParkSystemWinForms
{
    public class ParkSystemUtility
    {
        public static string conStr = ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
        public static LogHelper log = LogFactory.GetLogger("ParkSystemDAL");

        
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
                returnPara.Direction = ParameterDirection.ReturnValue;
                SqlParameter OrderNoPara = new SqlParameter("@OrderNoReturn", SqlDbType.NVarChar, 50) { Value = "0" };
                SqlParameter[] paras = {
                                          new SqlParameter("@DataType", dataType),
                                          new SqlParameter("@XMLData", xmlPara),
                                          returnPara,
                                          OrderNoPara
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
