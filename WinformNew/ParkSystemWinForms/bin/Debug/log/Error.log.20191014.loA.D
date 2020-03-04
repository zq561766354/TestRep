2019-10-14 18:16:22,173 [9] ERROR PsFormLog - 无法设置成开机启动,不允许所请求的注册表访问权。
2019-10-14 18:16:56,989 [10] ERROR PsFormLog - 无法设置成开机启动,不允许所请求的注册表访问权。
2019-10-14 18:18:05,924 [10] ERROR PsFormLog - 设置成开机启动成功,apppath:D:\VS项目\WinformNew\ParkSystemWinForms\bin\Debug\ParkSystemWinForms.EXE,appname:ParkSystemWinForms.EXE
2019-10-14 18:24:25,038 [8] ERROR PsFormLog - 设置成开机启动成功,apppath:D:\VS项目\WinformNew\ParkSystemWinForms\bin\Debug\ParkSystemWinForms.EXE,appname:ParkSystemWinForms.EXE
2019-10-14 18:24:58,277 [11] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<btnSetting_Click>b__45(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1069
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:98960147-dab9-403b-bb90-e2924779099f
Error Number:8146,State:2,Class:16
2019-10-14 18:24:58,493 [12] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<btnSetting_Click>b__45(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1069
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:f7d10725-7eea-4051-9808-218dc1dc3f83
Error Number:8146,State:2,Class:16
2019-10-14 18:24:58,583 [13] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<btnSetting_Click>b__45(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1069
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:c7e08310-f597-4d9e-b303-563852e419eb
Error Number:8146,State:2,Class:16
2019-10-14 18:24:58,762 [15] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<btnSetting_Click>b__45(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1069
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:c886b284-e1ff-4dfc-a43a-4fe3e3d410dd
Error Number:8146,State:2,Class:16
2019-10-14 18:24:58,797 [14] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<btnSetting_Click>b__45(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1069
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:a557e869-710e-490d-b069-c5e060dc46ed
Error Number:8146,State:2,Class:16
2019-10-14 18:24:59,012 [18] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<btnSetting_Click>b__45(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1069
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:a150c2e9-56e1-40fc-8cfe-f46ef7e56aab
Error Number:8146,State:2,Class:16
2019-10-14 18:24:59,267 [20] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<btnSetting_Click>b__45(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1069
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:605ab80d-ff1b-47b6-8f41-be54dc6e3460
Error Number:8146,State:2,Class:16
2019-10-14 18:24:59,297 [19] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<btnSetting_Click>b__45(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1069
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:cdeb1dfa-3bd4-4178-8791-48b32e6c81e5
Error Number:8146,State:2,Class:16
2019-10-14 18:26:17,090 [9] ERROR PsFormLog - 设置成开机启动成功,apppath:D:\VS项目\WinformNew\ParkSystemWinForms\bin\Debug\ParkSystemWinForms.EXE,appname:ParkSystemWinForms.EXE
2019-10-14 18:26:35,398 [12] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:16c55a7e-4cf8-430b-892d-813295c06b33
Error Number:8146,State:2,Class:16
2019-10-14 18:26:35,735 [13] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:49a87f27-9174-4cb1-8849-3eb672ae7296
Error Number:8146,State:2,Class:16
2019-10-14 18:26:35,793 [14] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:4c88626f-7eae-416a-abf5-c98f544dd961
Error Number:8146,State:2,Class:16
2019-10-14 18:26:35,735 [15] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:bae86564-1e6a-4175-a6b1-fd6bd1a4d38b
Error Number:8146,State:2,Class:16
2019-10-14 18:26:36,620 [16] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:aac102bb-91d6-44c6-93f9-08e4cf8baaba
Error Number:8146,State:2,Class:16
2019-10-14 18:26:36,706 [18] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:3ad0637d-5042-4115-9f66-5ae4a0097ada
Error Number:8146,State:2,Class:16
2019-10-14 18:26:36,750 [19] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:fc49cfca-3c81-44b6-91a4-e6d0546a59b7
Error Number:8146,State:2,Class:16
2019-10-14 18:26:36,763 [17] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:3e1af9f5-26b2-49bb-94b7-6dcb7a14f5a6
Error Number:8146,State:2,Class:16
2019-10-14 18:26:37,789 [22] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:9d559f78-4968-4745-958b-97dbd0c118b2
Error Number:8146,State:2,Class:16
2019-10-14 18:26:38,788 [23] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:2ec125cb-c635-45da-b86a-b510af693a0a
Error Number:8146,State:2,Class:16
2019-10-14 18:26:39,803 [24] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:026e451c-3f3d-40c9-80cb-5fbfd878af46
Error Number:8146,State:2,Class:16
2019-10-14 18:27:42,201 [8] ERROR PsFormLog - 设置成开机启动成功,apppath:D:\VS项目\WinformNew\ParkSystemWinForms\bin\Debug\ParkSystemWinForms.EXE,appname:ParkSystemWinForms.EXE
2019-10-14 18:27:57,486 [14] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:6bf9fc4d-a56d-4c5a-8b5d-d903063c8f96
Error Number:8146,State:2,Class:16
2019-10-14 18:27:57,487 [13] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:2a60a2e8-bce7-411e-a98b-fa78d1d78b82
Error Number:8146,State:2,Class:16
2019-10-14 18:27:57,487 [12] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:e73c2c8a-5b21-49bf-b1eb-f2739078f0d6
Error Number:8146,State:2,Class:16
2019-10-14 18:27:57,487 [15] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:4a82a700-ed9d-4b59-8885-7523ef091ad5
Error Number:8146,State:2,Class:16
2019-10-14 18:27:57,487 [11] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:66d8eac5-770a-413a-ac80-9a5cbff416d8
Error Number:8146,State:2,Class:16
2019-10-14 18:27:57,655 [17] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:8efa9eec-c52b-41ee-8402-bad7db63bf3b
Error Number:8146,State:2,Class:16
2019-10-14 18:27:57,962 [18] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:ed5455db-8007-412d-9c28-bcd69adc709c
Error Number:8146,State:2,Class:16
2019-10-14 18:27:57,986 [16] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:0bff233e-9b66-4c0b-bf91-805f8a6103da
Error Number:8146,State:2,Class:16
2019-10-14 18:27:58,433 [19] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:3fd76b2e-4f73-407a-86cb-53e33fd9514b
Error Number:8146,State:2,Class:16
2019-10-14 18:28:00,438 [21] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:bfb4a963-5596-4a2b-a241-416cdaaf0f14
Error Number:8146,State:2,Class:16
2019-10-14 18:28:01,432 [22] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:e83566d9-bbda-4bbe-9b57-fdb3afeac64f
Error Number:8146,State:2,Class:16
2019-10-14 18:28:02,446 [23] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:aceea2a3-593b-4228-94fb-ba8b8153d743
Error Number:8146,State:2,Class:16
2019-10-14 18:28:03,927 [24] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:ddbffa95-3878-4977-b052-bddf5f35e9a2
Error Number:8146,State:2,Class:16
2019-10-14 18:28:04,937 [25] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:f64e2073-03eb-49c8-b3b2-61240d891153
Error Number:8146,State:2,Class:16
2019-10-14 18:28:05,935 [26] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 462
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:fd989305-4c8c-41f0-88c1-13a224eba766
Error Number:8146,State:2,Class:16
2019-10-14 18:29:34,238 [9] ERROR PsFormLog - 设置成开机启动成功,apppath:D:\VS项目\WinformNew\ParkSystemWinForms\bin\Debug\ParkSystemWinForms.EXE,appname:ParkSystemWinForms.EXE
2019-10-14 18:32:02,175 [10] ERROR PsFormLog - 设置成开机启动成功,apppath:D:\VS项目\WinformNew\ParkSystemWinForms\bin\Debug\ParkSystemWinForms.EXE,appname:ParkSystemWinForms.EXE
2019-10-14 18:39:02,549 [8] ERROR PsFormLog - 设置成开机启动成功,apppath:D:\VS项目\WinformNew\ParkSystemWinForms\bin\Debug\ParkSystemWinForms.EXE,appname:ParkSystemWinForms.EXE
2019-10-14 18:39:21,423 [8] ERROR PsFormLog - System.ArgumentOutOfRangeException: “600000”的值对于“Value”无效。“Value”应介于 'Minimum' 和 'Maximum' 之间。
参数名: Value
   在 System.Windows.Forms.NumericUpDown.set_Value(Decimal value)
   在 ParkSystemWinForms.FrmSetting.FrmSetting_Load(Object sender, EventArgs e) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmSetting.cs:行号 61
   在 System.Windows.Forms.Form.OnLoad(EventArgs e)
   在 System.Windows.Forms.Form.OnCreateControl()
   在 System.Windows.Forms.Control.CreateControl(Boolean fIgnoreVisible)
   在 System.Windows.Forms.Control.CreateControl()
   在 System.Windows.Forms.Control.WmShowWindow(Message& m)
   在 System.Windows.Forms.Control.WndProc(Message& m)
   在 System.Windows.Forms.Form.WmShowWindow(Message& m)
   在 System.Windows.Forms.NativeWindow.Callback(IntPtr hWnd, Int32 msg, IntPtr wparam, IntPtr lparam)
2019-10-14 18:41:30,590 [9] ERROR PsFormLog - 设置成开机启动成功,apppath:D:\VS项目\WinformNew\ParkSystemWinForms\bin\Debug\ParkSystemWinForms.EXE,appname:ParkSystemWinForms.EXE
2019-10-14 18:46:31,535 [9] ERROR PsFormLog - 设置成开机启动成功,apppath:D:\VS项目\WinformNew\ParkSystemWinForms\bin\Debug\ParkSystemWinForms.EXE,appname:ParkSystemWinForms.EXE
2019-10-14 18:48:49,910 [9] ERROR PsFormLog - 设置成开机启动成功,apppath:D:\VS项目\WinformNew\ParkSystemWinForms\bin\Debug\ParkSystemWinForms.EXE,appname:ParkSystemWinForms.EXE
2019-10-14 18:52:10,862 [9] ERROR PsFormLog - 设置成开机启动成功,apppath:D:\VS项目\WinformNew\ParkSystemWinForms\bin\Debug\ParkSystemWinForms.EXE,appname:ParkSystemWinForms.EXE
2019-10-14 18:52:15,695 [12] ERROR PsFormLog - System.Data.SqlClient.SqlException (0x80131904): 过程 IPS_GetBrokenNetOrder 没有参数，但却为该过程提供了参数。
   在 System.Data.SqlClient.SqlConnection.OnError(SqlException exception, Boolean breakConnection, Action`1 wrapCloseInAction)
   在 System.Data.SqlClient.TdsParser.ThrowExceptionAndWarning(TdsParserStateObject stateObj, Boolean callerHasConnectionLock, Boolean asyncClose)
   在 System.Data.SqlClient.TdsParser.TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, Boolean& dataReady)
   在 System.Data.SqlClient.SqlDataReader.TryConsumeMetaData()
   在 System.Data.SqlClient.SqlDataReader.get_MetaData()
   在 System.Data.SqlClient.SqlCommand.FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, String resetOptionsString, Boolean isInternal, Boolean forDescribeParameterEncryption, Boolean shouldCacheForAlwaysEncrypted)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, Boolean async, Int32 timeout, Task& task, Boolean asyncWrite, Boolean inRetry, SqlDataReader ds, Boolean describeParameterEncryptionRequest)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method, TaskCompletionSource`1 completion, Int32 timeout, Task& task, Boolean& usedCache, Boolean asyncWrite, Boolean inRetry)
   在 System.Data.SqlClient.SqlCommand.RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, Boolean returnStream, String method)
   在 System.Data.SqlClient.SqlCommand.ExecuteReader(CommandBehavior behavior, String method)
   在 System.Data.Common.DbDataAdapter.FillInternal(DataSet dataset, DataTable[] datatables, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet, Int32 startRecord, Int32 maxRecords, String srcTable, IDbCommand command, CommandBehavior behavior)
   在 System.Data.Common.DbDataAdapter.Fill(DataSet dataSet)
   在 ParkSystemWinForms.SQLHelper.ExecuteDataSet(String ConnectString, CommandType cmdType, String cmdText, SqlParameter[] CommandParams) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\SQLHelper.cs:行号 238
   在 ParkSystemWinForms.ParkSystemUtility.GetData(Dictionary`2 paraDic, String proceName) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemUtility.cs:行号 122
   在 ParkSystemWinForms.ParkSystemDAL.GetBrokenNetOrder() 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1235
   在 ParkSystemWinForms.ParkSystemDAL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemDAL.cs:行号 1255
   在 ParkSystemWinForms.ParkSystemBLL.OrderRenewal(Int32 netWorkResult) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\ParkSystemBLL.cs:行号 463
   在 ParkSystemWinForms.FrmMain.<FrmMain_Load>b__7d(Object a) 位置 d:\VS项目\WinformNew\ParkSystemWinForms\FrmMain.cs:行号 1691
   在 System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
   在 System.Threading.TimerQueueTimer.CallCallback()
   在 System.Threading.TimerQueueTimer.Fire()
   在 System.Threading.TimerQueue.FireNextTimers()
ClientConnectionId:d1584eb2-c357-4685-b22c-d9206612245e
Error Number:8146,State:2,Class:16
2019-10-14 18:53:45,599 [9] ERROR PsFormLog - 设置成开机启动成功,apppath:D:\VS项目\WinformNew\ParkSystemWinForms\bin\Debug\ParkSystemWinForms.EXE,appname:ParkSystemWinForms.EXE
