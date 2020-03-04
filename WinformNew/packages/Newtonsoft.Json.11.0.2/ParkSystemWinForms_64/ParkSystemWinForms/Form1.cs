using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using System.Configuration;
using ParkSystemWinForms.Model;
using System.Timers;


namespace ParkSystemWinForms
{
    public partial class Form1 : Form
    {
        //1.声明自适应类实例  
        AutoSizeForm asc = new AutoSizeForm(); 
        public Form1()
        {
            InitializeComponent();
        }
        private static int OrderIdPrivate;
        private static string OrderNoPrivate;
        private IntPtr[] pUid = new IntPtr[4] { IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero };
        private ipcsdk.ICE_IPCSDK_OnPlate onPlate;
        private ipcsdk.ICE_IPCSDK_OnFrame_Planar onFrame;
        private ipcsdk.ICE_IPCSDK_OnPastPlate onPastPlate;
        private ipcsdk.ICE_IPCSDK_OnSerialPort onSerialPort;
        private ipcsdk.ICE_IPCSDK_OnSerialPort_RS232 onSerialPortRS232;

        public delegate void UpdatePlateInfo(string strIP, string strNum, string strColor, uint nVehicleColor, uint nAlarmType, uint nCapTime, int index);
        public UpdatePlateInfo updatePlateInfo;
        public delegate void UpdatePortInfo(string strIp, uint len, int index, string data, int type);
        public UpdatePortInfo updatePortInfo;
        public delegate void UpdateStatus(int index, int type);
        public UpdateStatus updateStatus;
        public delegate void UpdateTriggerStatus(int index, uint nStatus);
        public UpdateTriggerStatus triggerStatus;
        //定义赋值控件委托
        public delegate void AssignmentControl(int type, DateTime date, string plateNo, string picIn, string picOut);
        public AssignmentControl assignmentControl;
        public delegate void AssignmentPicControl(int type, DateTime date, string plateNo);
        public AssignmentPicControl assignmentPicControl;
        public delegate void AssignmentOutshowLableControl(string picnumber, DateTime Indate, DateTime Outdate, int parkingTime, string carType, double actucalAmount);
        public AssignmentOutshowLableControl assignmentOutshowLableControl;


        private bool[] bClose = new bool[4] { false, false, false, false };
        private string[] strIp = new string[4];
        private int[] frame_count = new int[4] { 0, 0, 0, 0 };
        private int[] count = new int[4] { 0, 0, 0, 0 };
        private Label[] labelPlate = new Label[4];
        private UInt32[] nStatus = new UInt32[4] { 0, 0, 0, 0 };
        private Label[] labelStatus = new Label[4];
        private StringBuilder[] strMac = new StringBuilder[4];
        private TextBox[] textBoxGateNum = new TextBox[4];
        private TextBox[] textBoxTriggerNum = new TextBox[4];
        private UInt32[] nGateNum = new UInt32[4] { 0, 0, 0, 0 };
        private UInt32[] nTriggerNum = new UInt32[4] { 0, 0, 0, 0 };
        private UInt32[] nGate2Num = new UInt32[4] { 0, 0, 0, 0 };
        private uint[] nRS232Num = new uint[4] { 0, 0, 0, 0 };
        private uint[] nRS485Num = new uint[4] { 0, 0, 0, 0 };
        private uint[] nRecvPortCount_RS232 = new uint[4] { 0, 0, 0, 0 };
        private uint[] nRecvPortCount_RS485 = new uint[4] { 0, 0, 0, 0 };
        private UInt32[] nCurrentStatus = new UInt32[4] { 0, 0, 0, 0 };
        private bool[] bRecord = new bool[4] { false, false, false, false };
        private bool[] bPreview = new bool[4] { true, true, true, true };

        private bool m_bExit = false;
        private long totalCount = 0;
        private long pastCount = 0;

        public ICE_OSDAttr_S osdInfo = new ICE_OSDAttr_S();

        Mutex mutex = new Mutex();
        Mutex mutexThread = new Mutex();
        public static Thread[] mythread = new Thread[4] { null, null, null, null };
        public static Thread threadTrigger = null;
        public static Thread threadOpenGate = null;
        public static Thread threadStatus = null;
        public static Thread threadOpenGate2 = null;
        public static Thread threadRS485 = null;
        public static Thread threadRS232 = null;
        public static Thread[] threadBroadcast = new Thread[4] { null, null, null, null };

        private static LogHelper log = LogFactory.GetLogger("ParkSystemLog");

        private string[] strVehicleColor = new string[11] { "未知", "红色", "绿色", "蓝色", "黄色", "白色", "灰色", "黑色", "紫色", "棕色", "粉色" };
        private string[] strAlarmType = new string[]{
            "实时_硬触发+临时车辆",
	        "实时_视频触发+临时车辆",
	        "实时_软触发+临时车辆",
	        "实时_硬触发+白名单",
	        "实时_视频触发+白名单",
	        "实时_软触发+白名单",
	        "实时_硬触发+黑名单",
	        "实时_视频触发+黑名单",
	        "实时_软触发+黑名单",
	        "脱机_硬触发+临时车辆",
	        "脱机_视频触发+临时车辆",
	        "脱机_软触发+临时车辆",
	        "脱机_硬触发+白名单",
	        "脱机_视频触发+白名单",
	        "脱机_软触发+白名单",
	        "脱机_硬触发+黑名单",
	        "脱机_视频触发+黑名单",
	        "脱机_软触发+黑名单",
            "实时_硬触发+过期白名单",
	        "实时_视频触发+过期白名单",
	        "实时_软触发+过期白名单",
	        "脱机_硬触发+过期白名单",
	        "脱机_视频触发+过期白名单",
	        "脱机_软触发+过期白名单"
        };

        //设置变量
        private string m_strStorePath = "D:\\";

        private int m_bOpenGate = 0;
        private int m_bTrigger = 0;
        private int m_bOpenGate2 = 0;
        private int m_bRS485 = 0;
        private int m_bRS232 = 0;

        private int m_nOpenInterval = 0;
        private int m_nTriggerInterval = 0;
        private int m_nOpenInterval2 = 0;
        private int m_nRS485Interval = 0;
        private int m_nRS232Interval = 0;
        private int m_nRecordInterval = 10;

        private string m_nVideoColor = "000000";
        private string m_nJpegColor = "000000";
        private string m_strLogPath = "D:\\";
        private int m_bEnableLog = 0;

        string iPAddr_In = ConfigHelper.AppSettings("IPAddr_In");
        string iPAddr_Out = ConfigHelper.AppSettings("IPAddr_Out");
        string screenIp_In = ConfigHelper.AppSettings("ScreenIP_In");
        int screenIn_LineCount = Convert.ToInt32(ConfigHelper.AppSettings("Screen_In_Count"));
        int screenIn_Volumn = Convert.ToInt32(ConfigHelper.AppSettings("ScreenIn_Volume"));
        string screenIp_Out = ConfigHelper.AppSettings("ScreenIP_Out");
        int screenOut_LineCount = Convert.ToInt32(ConfigHelper.AppSettings("Screen_Out_Count"));
        int screenOut_Volumn = Convert.ToInt32(ConfigHelper.AppSettings("ScreenOut_Volume"));

        private DataTable dtAttachment;

        public void showCount(string strIP, string strNum, string strColor, uint nVehicleColor, uint nAlarmType, uint nCapTime, int index)
        {
            if (listBoxInfo.Items.Count > 1024)
            {
                listBoxInfo.Items.Clear();
            }
            if (pUid[index] != IntPtr.Zero)
            {
                
                if (nCapTime == 0)
                {
                    count[index]++;
                    string strText = count[index].ToString() + ". " + strNum + " " + strColor;
                    labelPlate[index].Text = strText;

                    totalCount++;
                    string textInfo = totalCount.ToString() + ". " + strIP + strNum + " " + strColor + " " + strVehicleColor[nVehicleColor] + " " + strAlarmType[nAlarmType] ;
                    if (textInfo == null)
                        return;
                    listBoxInfo.Items.Insert(0, textInfo);
                }
                else
                {
                    pastCount++;
                    string pastText = pastCount.ToString() + ". 断网续传" + strIP + strNum + " " + strColor + " " + strVehicleColor[nVehicleColor] + " " + strAlarmType[nAlarmType];
                    if (pastText == null)
                        return;
                    listBoxInfo.Items.Insert(0, pastText);
                }
                listBoxInfo.Items.Insert(0, nCapTime.ToString());
            }
        }
        ParkSystemBLL psBll = new ParkSystemBLL();
        OrderModel ormodePrivate = new OrderModel();
        private void Form1_Load(object sender, EventArgs e)
        {
            string x = psBll.AccessRules();
            MessageBox.Show(x);
            asc.Initialize(this); 
            timer1.Start();
            //OrderqueryForm form3 = new OrderqueryForm();
            //form3.ShowDialog();
            // Form4 form4 = new Form4(@"D:\抓拍_C#\192.168.2.229\20181016\20181016092016_苏E810ZL.jpg", "dsf", DateTime.Now);

            // form4.ShowDialog();
            //ParkSystemBLL.CreateTable();
            ipcsdk.ICE_IPCSDK_Init();
            onFrame = new ipcsdk.ICE_IPCSDK_OnFrame_Planar(SDK_OnFrame);
            onPlate = new ipcsdk.ICE_IPCSDK_OnPlate(SDK_OnPlate);  //实时抓拍
            onPastPlate = new ipcsdk.ICE_IPCSDK_OnPastPlate(SDK_OnPastPlate);//断网续传
            onSerialPort = new ipcsdk.ICE_IPCSDK_OnSerialPort(SDK_OnSerialPort);
            onSerialPortRS232 = new ipcsdk.ICE_IPCSDK_OnSerialPort_RS232(SDK_OnSerialPortRS232);

            dtAttachment = new DataTable();
            dtAttachment.Columns.Add("Time", typeof(string));
            dtAttachment.Columns.Add("pcNumber", typeof(string));
            dtAttachment.Columns.Add("type1", typeof(string));
            dtAttachment.Columns.Add("type2", typeof(string));


            labelStatus[0] = this.labelStatus1;
            labelStatus[1] = this.labelStatus2;

            labelPlate[0] = this.labelPlate1;
            labelPlate[1] = this.labelPlate2;

            for (int i = 0; i < 4; i++)
            {
                strMac[i] = new StringBuilder(64);
            }

            updatePlateInfo = new UpdatePlateInfo(showCount);
            updateStatus = new UpdateStatus(showStatus);
            triggerStatus = new UpdateTriggerStatus(showTriggerStatus);
            updatePortInfo = new UpdatePortInfo(showPortData);
            assignmentControl = new AssignmentControl(AssignmentControlMethod);
            assignmentPicControl = new AssignmentPicControl(AssignmentPicControlMethod);
            assignmentOutshowLableControl = new AssignmentOutshowLableControl(AssignmentOutshowLableControlMethod);
            //获得设置
            if (File.Exists(@"./param.dat"))
            {
                FileStream fs = new FileStream("param.dat", FileMode.Open, FileAccess.Read);
                if (fs != null)
                {
                    try
                    {
                        BinaryReader br = new BinaryReader(fs);
                        if (br != null)
                        {
                            m_strStorePath = br.ReadString();
                            m_strLogPath = br.ReadString();
                            m_bEnableLog = br.ReadInt32();
                            m_bOpenGate = br.ReadInt32();
                            m_bTrigger = br.ReadInt32();
                            m_nOpenInterval = br.ReadInt32();
                            m_nTriggerInterval = br.ReadInt32();
                            m_nRecordInterval = br.ReadInt32();

                            osdInfo.u32OSDLocationVideo = (UInt32)br.ReadInt32();
                            m_nVideoColor = br.ReadString();
                            osdInfo.u32DateVideo = (UInt32)br.ReadInt32();
                            osdInfo.u32License = (UInt32)br.ReadInt32();
                            osdInfo.u32CustomVideo = (UInt32)br.ReadInt32();
                            osdInfo.szCustomVideo6 = br.ReadString();

                            osdInfo.u32OSDLocationJpeg = (UInt32)br.ReadInt32();
                            m_nJpegColor = br.ReadString();
                            osdInfo.u32DateJpeg = (UInt32)br.ReadInt32();
                            osdInfo.u32Algo = (UInt32)br.ReadInt32();
                            osdInfo.u32CustomJpeg = (UInt32)br.ReadInt32();
                            osdInfo.szCustomJpeg6 = br.ReadString();

                            m_bOpenGate2 = br.ReadInt32();
                            m_bRS485 = br.ReadInt32();
                            m_bRS232 = br.ReadInt32();
                            m_nOpenInterval2 = br.ReadInt32();
                            m_nRS485Interval = br.ReadInt32();
                            m_nRS232Interval = br.ReadInt32();


                            br.Close();
                        }
                        fs.Close();
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
            }

            if (m_strStorePath == "")
            {
                m_strStorePath = "D:\\";
            }
            if (m_strLogPath == "")
            {
                m_strStorePath = "D:\\";
            }

            string tmp = getColor16(m_nVideoColor);
            osdInfo.u32ColorVideo = Convert.ToUInt32(tmp, 16);
            tmp = "";
            tmp = getColor16(m_nJpegColor);
            osdInfo.u32CustomJpeg = Convert.ToUInt32(tmp, 16);
            loadConfig(m_strStorePath, m_bOpenGate, m_bTrigger, m_nOpenInterval, m_nTriggerInterval, m_nRecordInterval, m_bOpenGate2, m_bRS485, m_bRS232,
                m_nOpenInterval2, m_nRS485Interval, m_nRS232Interval);
            ipcsdk.ICE_IPCSDK_LogConfig(m_bEnableLog, m_strLogPath);

            if (threadStatus != null)
            {
                threadStatus.Abort();
                threadStatus = null;
            }
            threadStatus = new Thread(new ThreadStart(getStatus));
            threadStatus.Start();

            //连接相机
            this.Connect();

            //初始化屏幕
            this.InitScreen();

            tabPage1.Text = "通道视频";
            tabPage2.Text = "车头图片";
            tabPage3.Text = "通道视频";
            tabPage4.Text = "车头图片";

            //创建Timer
            int timeNum =Convert.ToInt32(ConfigurationManager.AppSettings["timeNum"]);
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Enabled = true;
            timer.Interval = timeNum;//执行间隔时间,单位为毫秒;此时时间间隔为1分钟  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(PictureClear);
            timer.Start();
        }

        private void InitScreen()
        {

            ScreenSdk.Init(false, false);
            if (ScreenSdk.Set_DisplayLines(screenIp_In, screenIn_LineCount) == 1)
                listBoxInfo.Items.Insert(0, "显示屏1设置成功");
            if (ScreenSdk.Set_DisplayLines(screenIp_Out, screenOut_LineCount) == 1)
                listBoxInfo.Items.Insert(0, "显示屏2设置成功");
        }

        private void getStatus()
        {
            while (true)
            {
                Thread.Sleep(1000);
                for (int i = 0; i < 4; i++)
                {
                    if (pUid[i] != IntPtr.Zero || bClose[i])
                    {
                        mutexThread.WaitOne();
                        nCurrentStatus[i] = ipcsdk.ICE_IPCSDK_GetStatus(pUid[i]);
                        mutexThread.ReleaseMutex();
                        if (nCurrentStatus[i] != nStatus[i] && pUid[i] != IntPtr.Zero)
                        {
                            mutexThread.WaitOne();
                            ipcsdk.ICE_IPCSDK_GetDevID(pUid[i], strMac[i]);
                            mutexThread.ReleaseMutex();
                            nStatus[i] = nCurrentStatus[i];
                            IAsyncResult syncResult = this.BeginInvoke(updateStatus, i, 0);
                        }
                    }
                }
            }
        }

        private void openGate()
        {
            while (true)
            {
                Thread.Sleep(m_nOpenInterval);
                for (int i = 0; i < 4; i++)
                {
                    if (pUid[i] == IntPtr.Zero || bClose[i])
                        continue;

                    //IntPtr ppUid = new IntPtr(UID[i]);

                    //if (ICE_IPCSDK_GetStatus(pUid[i]) == 0)
                    //    continue;

                    mutexThread.WaitOne();
                    UInt32 success = ipcsdk.ICE_IPCSDK_OpenGate(pUid[i]);
                    mutexThread.ReleaseMutex();
                    if (success == 1)
                    {
                        nGateNum[i]++;
                        IAsyncResult syncResult = this.BeginInvoke(updateStatus, i, 1);
                    }
                }
            }
        }

        private void trigger()
        {
            while (true)
            {
                Thread.Sleep(m_nTriggerInterval);
                for (int i = 0; i < 4; i++)
                {
                    if ((pUid[i] == IntPtr.Zero) || bClose[i])
                        continue;

                    //if (ICE_IPCSDK_GetStatus(pUid[i]) == 0)
                    //    continue;

                    //StringBuilder strNum = new StringBuilder(32);
                    //StringBuilder strColor = new StringBuilder(64);
                    //uint len = 0;
                    //byte[] pdata = new byte[1048576];

                    mutexThread.WaitOne();
                    uint success = ipcsdk.ICE_IPCSDK_TriggerExt(pUid[i]);
                    mutexThread.ReleaseMutex();
                    if (success == 1)
                    {
                        nTriggerNum[i]++;
                        IAsyncResult syncResult = this.BeginInvoke(updateStatus, i, 2);
                    }
                    else
                    {
                        IAsyncResult syncResult = this.BeginInvoke(triggerStatus, i, success);
                    }
                    //pdata = null;
                    //strNum = null;
                    //strColor = null;
                }
            }

        }

        private void openGate2()
        {
            while (true)
            {
                Thread.Sleep(m_nOpenInterval2);
                for (int i = 0; i < 4; i++)
                {
                    if (pUid[i] == IntPtr.Zero || bClose[i])
                        continue;

                    mutexThread.WaitOne();
                    UInt32 success = ipcsdk.ICE_IPCSDK_ControlAlarmOut(pUid[i], 1);
                    mutexThread.ReleaseMutex();
                    if (success == 1)
                    {
                        nGateNum[i]++;
                        IAsyncResult syncResult = this.BeginInvoke(updateStatus, i, 3);
                    }
                }
            }
        }

        private void sendRS485()
        {
            while (true)
            {
                Thread.Sleep(m_nRS485Interval);
                for (int i = 0; i < 4; i++)
                {
                    if (pUid[i] == IntPtr.Zero || bClose[i])
                        continue;

                    mutexThread.WaitOne();
                    string strSendData = @"sdk(" + (i + 1).ToString() + @"): send rs485 data to camera.";
                    uint success = ipcsdk.ICE_IPCSDK_TransSerialPort(pUid[i], strSendData, (UInt32)(strSendData.Length + 2));
                    mutexThread.ReleaseMutex();
                    if (success == 1)
                    {
                        nGateNum[i]++;
                        IAsyncResult syncResult = this.BeginInvoke(updateStatus, i, 4);
                    }
                }
            }
        }

        private void sendRS232()
        {
            while (true)
            {
                Thread.Sleep(m_nRS232Interval);
                for (int i = 0; i < 4; i++)
                {
                    if (pUid[i] == IntPtr.Zero || bClose[i])
                        continue;

                    mutexThread.WaitOne();
                    string strSendData = @"sdk(" + (i + 1).ToString() + @"): send rs232 data to camera.";
                    uint success = ipcsdk.ICE_IPCSDK_TransSerialPort_RS232(pUid[i], strSendData, (UInt32)(strSendData.Length + 2));
                    mutexThread.ReleaseMutex();
                    if (success == 1)
                    {
                        nGateNum[i]++;
                        IAsyncResult syncResult = this.BeginInvoke(updateStatus, i, 5);
                    }
                }
            }
        }

        private string getColor16(string color)
        {
            string realColor = "";
            realColor = color[4].ToString() + color[5].ToString() + color[2].ToString() + color[3].ToString() + color[0].ToString() + color[1].ToString();
            return realColor;
        }

        private void loadOsdConfig(uint nPosVideo, string nColorVideo, uint nDateTimeVideo, uint nLicense, uint nCustomVideo, string bstrCustomVideo,
            uint nPosJpeg, string nColorJpeg, uint nDateTimeJpeg, uint nAlgo, uint nCustomJpeg, string bstrCustomJpeg)
        {
            osdInfo.u32OSDLocationVideo = nPosVideo;
            m_nVideoColor = nColorVideo;
            osdInfo.u32DateVideo = nDateTimeVideo;
            osdInfo.u32License = nLicense;
            osdInfo.u32CustomVideo = nCustomVideo;
            osdInfo.szCustomVideo6 = bstrCustomVideo;/*System.Text.Encoding.Default.GetBytes(bstrCustomVideo.PadRight(6 * 64, '\0'));*/
            osdInfo.u32OSDLocationJpeg = nPosJpeg;
            m_nJpegColor = nColorJpeg;
            osdInfo.u32DateJpeg = nDateTimeJpeg;
            osdInfo.u32Algo = nAlgo;
            osdInfo.u32CustomJpeg = nCustomJpeg;
            osdInfo.szCustomJpeg6 = bstrCustomJpeg;/*System.Text.Encoding.Default.GetBytes(bstrCustomJpeg.PadRight(6 * 64, '\0'));*/

            string tmp = getColor16(m_nVideoColor);
            osdInfo.u32ColorVideo = Convert.ToUInt32(tmp, 16);
            tmp = "";
            tmp = getColor16(m_nJpegColor);
            osdInfo.u32ColorJpeg = Convert.ToUInt32(tmp, 16);

            for (int i = 0; i < 4; i++)
            {
                if (pUid[i] != IntPtr.Zero)
                {
                    //IntPtr pUid = new IntPtr(UID[i]);
                    ipcsdk.ICE_IPCSDK_SetOSDCfg(pUid[i], ref osdInfo);
                }
            }
        }

        private void loadConfig(string strPath, int nOpenGate, int nTrigger, int nOpenGateInterval, int nTriggerInterval, int nRecordInterval,
           int nOpenGate2, int nRS485, int nRS232, int nOpenGate2Interval, int nRS485Interval, int nRS232Interval)
        {
            //mutexMsg.WaitOne();
            m_strStorePath = strPath;
            m_bOpenGate = nOpenGate;
            m_bTrigger = nTrigger;
            m_nOpenInterval = nOpenGateInterval;
            m_nTriggerInterval = nTriggerInterval;
            m_nRecordInterval = nRecordInterval;

            m_bOpenGate2 = nOpenGate2;
            m_bRS485 = nRS485;
            m_bRS232 = nRS232;
            m_nOpenInterval2 = nOpenGate2Interval;
            m_nRS485Interval = nRS485Interval;
            m_nRS232Interval = nRS232Interval;

            if (m_strStorePath == "")
            {
                m_strStorePath = "D:\\";
            }


            if (nOpenGate == 1)
            {
                if (threadOpenGate == null)
                {
                    threadOpenGate = new Thread(new ThreadStart(openGate));
                    threadOpenGate.Start();
                }
            }
            else
            {
                if (threadOpenGate != null)
                {
                    threadOpenGate.Abort();
                    threadOpenGate = null;
                }
            }


            if (nTrigger == 1)
            {
                if (threadTrigger == null)
                {
                    threadTrigger = new Thread(new ThreadStart(trigger));
                    threadTrigger.Start();
                }
            }
            else
            {
                if (threadTrigger != null)
                {
                    threadTrigger.Abort();
                    threadTrigger = null;
                }
            }

            if (nOpenGate2 == 1)
            {
                if (threadOpenGate2 == null)
                {
                    threadOpenGate2 = new Thread(new ThreadStart(openGate2));
                    threadOpenGate2.Start();
                }
            }
            else
            {
                if (threadOpenGate2 != null)
                {
                    threadOpenGate2.Abort();
                    threadOpenGate2 = null;
                }
            }

            if (nRS485 == 1)
            {
                if (threadRS485 == null)
                {
                    threadRS485 = new Thread(new ThreadStart(sendRS485));
                    threadRS485.Start();
                }
            }
            else
            {
                if (threadRS485 != null)
                {
                    threadRS485.Abort();
                    threadRS485 = null;
                }
            }

            if (nRS232 == 1)
            {
                if (threadRS232 == null)
                {
                    threadRS232 = new Thread(new ThreadStart(sendRS232));
                    threadRS232.Start();
                }
            }
            else
            {
                if (threadRS232 != null)
                {
                    threadRS232.Abort();
                    threadRS232 = null;
                }
            }

        }

        private void Connect()
        {

            string ip_in = ConfigHelper.AppSettings("IPAddr_In");
            string ip_out = ConfigHelper.AppSettings("IPAddr_Out");
            this.Connect_In(ip_in);
            this.Connect_Out(ip_out);
        }

        private void Connect_In(string ip_in)
        {
            IntPtr videoHwnd = picBox_In.Handle;
            if (videoHwnd != IntPtr.Zero)
            {

                pUid[0] = ipcsdk.ICE_IPCSDK_OpenPreview(ip_in, 1, 1, (uint)videoHwnd, onPlate, new IntPtr(0));
                if (pUid[0] == IntPtr.Zero)
                {
                    MessageBox.Show("相机1连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }
            else
            {
                MessageBox.Show("未获得视频播放窗口");
                return;
            }



            //断网续传
            ipcsdk.ICE_IPCSDK_SetPastPlateCallBack(pUid[0], onPastPlate, new IntPtr(0));

            //透明串口
            ipcsdk.ICE_IPCSDK_SetSerialPortCallBack(pUid[0], onSerialPort, new IntPtr(0));//485
            ipcsdk.ICE_IPCSDK_SetSerialPortCallBack_RS232(pUid[0], onSerialPortRS232, new IntPtr(0));//232

            bPreview[0] = true;
            bClose[0] = false;

        }

        private void Connect_Out(string ip_out)
        {
            IntPtr videoHwnd = picBox_Out.Handle;
            if (videoHwnd != IntPtr.Zero)
            {

                pUid[1] = ipcsdk.ICE_IPCSDK_OpenPreview(ip_out, 1, 1, (uint)videoHwnd, onPlate, new IntPtr(1));
                if (pUid[1] == IntPtr.Zero)
                {
                    MessageBox.Show("相机2连接失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }
            else
            {
                MessageBox.Show("未获得视频播放窗口");
                return;
            }



            //断网续传
            ipcsdk.ICE_IPCSDK_SetPastPlateCallBack(pUid[0], onPastPlate, new IntPtr(0));

            //透明串口
            ipcsdk.ICE_IPCSDK_SetSerialPortCallBack(pUid[0], onSerialPort, new IntPtr(0));//485
            ipcsdk.ICE_IPCSDK_SetSerialPortCallBack_RS232(pUid[0], onSerialPortRS232, new IntPtr(0));//232

            bPreview[1] = true;
            bClose[1] = false;

        }

        private void btnOpen_In_Click(object sender, EventArgs e)
        {
            if (pUid[0] != IntPtr.Zero)
            {
                ipcsdk.ICE_IPCSDK_OpenGate(pUid[0]);
            }
        }

        private void btnOpen_Out_Click(object sender, EventArgs e)
        {
            if (pUid[1] != IntPtr.Zero)
            {
                ipcsdk.ICE_IPCSDK_OpenGate(pUid[1]);

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_bExit = true;
            //timerStatus.Enabled = false;
            //timerOpenGate.Enabled = false;
            //timerTrigger.Enabled = false;

            //mutexThread.WaitOne();

            if (threadTrigger != null)
            {
                threadTrigger.Abort();
                threadTrigger = null;
            }
            if (threadOpenGate != null)
            {
                threadOpenGate.Abort();
                threadOpenGate = null;
            }
            if (threadStatus != null)
            {
                threadStatus.Abort();
                threadStatus = null;
            }
            if (threadOpenGate2 != null)
            {
                threadOpenGate2.Abort();
                threadOpenGate2 = null;
            }
            if (threadRS485 != null)
            {
                threadRS485.Abort();
                threadRS485 = null;
            }
            if (threadRS232 != null)
            {
                threadRS232.Abort();
                threadRS232 = null;
            }

            for (int i = 0; i < 4; i++)
            {
                if (pUid[i] != IntPtr.Zero)
                {
                    if (bRecord[i])
                    {
                        if (null != mythread[i])
                        {
                            mythread[i].Abort();
                            mythread[i] = null;
                        }
                        bRecord[i] = false;
                        ipcsdk.ICE_IPCSDK_StopRecord(pUid[i]);
                    }
                    ipcsdk.ICE_IPCSDK_Close(pUid[i]);
                    pUid[i] = IntPtr.Zero;
                }
            }
            //mutexThread.ReleaseMutex();
            mutex.Close();
            mutex = null;
            mutexThread.Close();
            mutexThread = null;
            ipcsdk.ICE_IPCSDK_Fini();
        }

        public void on_frame(UInt32 u32Timestamp,
           IntPtr pu8DataY, IntPtr pu8DataU, IntPtr pu8DataV,
           Int32 s32LinesizeY, Int32 s32LinesizeU, Int32 s32LinesizeV,
           Int32 s32Width, Int32 s32Height, Int32 i)
        {
            if (m_bExit)
                return;

            mutex.WaitOne();
            string strDir = m_strStorePath + @"抓拍_C#Frame\" + strIp[i] + @"\" + DateTime.Now.ToString("yyyyMMdd");
            if (!Directory.Exists(strDir))
            {
                Directory.CreateDirectory(strDir);
            }

            string strPicName = strDir + @"\" + "test.bmp";

            if (0 == (frame_count[i] % 30))
            {

                try
                {
                    byte[] datay = new byte[s32Width * s32Height];
                    for (int j = 0; j < s32Height; j++)
                        Marshal.Copy((IntPtr)pu8DataY + j * s32LinesizeY, datay, j * s32Width, s32Width);

                    byte[] datau = new byte[s32Width * s32Height / 4];
                    for (int j = 0; j < s32Height / 2; j++)
                        Marshal.Copy((IntPtr)pu8DataU + j * s32LinesizeU, datau, j * s32Width / 2, s32Width / 2);

                    byte[] datav = new byte[s32Width * s32Height / 4];
                    for (int j = 0; j < s32Height / 2; j++)
                        Marshal.Copy((IntPtr)pu8DataV + j * s32LinesizeV, datav, j * s32Width / 2, s32Width / 2);

                    byte[] rgb24 = new byte[s32Width * s32Height * 3];

                    util.Convert(s32Width, s32Height, datay, datau, datav, ref rgb24);

                    FileStream fs = new FileStream(strPicName, FileMode.Create, FileAccess.Write);
                    BinaryWriter bw = new BinaryWriter(fs);

                    bw.Write('B');
                    bw.Write('M');
                    bw.Write(rgb24.Length + 54);
                    bw.Write(0);
                    bw.Write(54);
                    bw.Write(40);
                    bw.Write(s32Width);
                    bw.Write(s32Height);
                    bw.Write((ushort)1);
                    bw.Write((ushort)24);
                    bw.Write(0);
                    bw.Write(rgb24.Length);
                    bw.Write(0);
                    bw.Write(0);
                    bw.Write(0);
                    bw.Write(0);

                    bw.Write(rgb24, 0, rgb24.Length);
                    bw.Close();
                    fs.Close();

                }
                catch (System.Exception ex)
                {
                    //MessageBox.Show("frame" + ex.Message);
                }

                frame_count[i] = 0;
            }
            frame_count[i]++;
            mutex.ReleaseMutex();
        }

        public void SDK_OnFrame(System.IntPtr pvParam, uint u32Timestamp, System.IntPtr pu8DataY,
            System.IntPtr pu8DataU, System.IntPtr pu8DataV, int s32LinesizeY, int s32LinesizeU,
            int s32LinesizeV, int s32Width, int s32Height)
        {
            int index = (int)pvParam;
            if (m_bExit || bClose[index])
                return;
            on_frame(u32Timestamp, pu8DataY, pu8DataU, pu8DataV, s32LinesizeY,
                s32LinesizeU, s32LinesizeV, s32Width, s32Height, index);
        }

        #region 储存图片
        public void storePic(byte[] picData, string strIP, string strNumber, bool bIsPlate, UInt32 nCapTime, ref string picName, ref string picPath)
        {
            DateTime dt = new DateTime();
            if (nCapTime == 0)
            {
                dt = DateTime.Now;
            }
            else
            {
                dt = DateTime.Parse("1970-01-01 08:00:00").AddSeconds(nCapTime);
            }

            string strDir = m_strStorePath + @"抓拍_C#\" + strIP + @"\" + dt.ToString("yyyyMMdd");
            if (!Directory.Exists(strDir))
            {
                Directory.CreateDirectory(strDir);
            }
            string tmpPicPath = strDir;
            string tmpPicName = dt.ToString("yyyyMMddHHmmss") + "_" + strNumber;
            if (bIsPlate)
            {
                tmpPicName += "_plate";
            }
            tmpPicName += ".jpg";
            string strPicName = strDir + @"\" + tmpPicName;
            /*
            string strPicName = strDir + @"\" + dt.ToString("yyyyMMddHHmmss") + "_" + strNumber;
            if (bIsPlate)
            {
                strPicName += "_plate";
            }
            strPicName += ".jpg";*/
            if (File.Exists(strPicName))
            {
                int count = 1;
                while (count <= 10)
                {
                    strPicName = strDir + @"\" + dt.ToString("yyyyMMddHHmmss") + "_" + strNumber;
                    if (bIsPlate)
                    {
                        strPicName += "_plate";
                    }
                    strPicName += "_" + count.ToString() + ".jpg";

                    if (!File.Exists(strPicName))
                    {
                        break;
                    }
                    count++;
                }
            }

            try
            {
                FileStream fs = new FileStream(strPicName, FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(picData);
                bw.Close();
                fs.Close();
                picName = tmpPicName;
                picPath = tmpPicPath;
            }
            catch (System.Exception ex)
            {

            }
        }
        #endregion

        public void on_plate(string bstrIP, string bstrNumber, string bstrColor, IntPtr vPicData, UInt32 nPicLen,
          IntPtr vCloseUpPicData, UInt32 nCloseUpPicLen, Int16 nPlatePosLeft, Int16 nPlatePosTop,
          Int16 nPlatePosRight, Int16 nPlatePosBottom, Single fPlateConfidence,
          UInt32 nVehicleColor, UInt32 nPlateType, UInt32 nVehicleDir, UInt32 nAlarmType, UInt32 nCapTime, Int32 index, ref string storePicName, ref string storePath, ref string tmpStorePicName, ref string tmpStorePath)
        {
            if (m_bExit)
                return;

            IAsyncResult syncResult = this.BeginInvoke(updatePlateInfo, bstrIP, bstrNumber, bstrColor, nVehicleColor, nAlarmType, nCapTime, index);

            if (nPicLen > 0)
            {
                IntPtr ptr2 = (IntPtr)vPicData;
                byte[] datajpg2 = new byte[nPicLen];
                Marshal.Copy(ptr2, datajpg2, 0, datajpg2.Length);
                storePic(datajpg2, bstrIP, bstrNumber, false, nCapTime, ref storePicName, ref storePath);
            }


            if (nCloseUpPicLen > 0)
            {
                // string tmpStorePicName = "";
                // string tmpStorePath = "";
                IntPtr ptr = (IntPtr)vCloseUpPicData;
                byte[] datajpg = new byte[nCloseUpPicLen];
                Marshal.Copy(ptr, datajpg, 0, datajpg.Length);
                storePic(datajpg, bstrIP, bstrNumber, true, nCapTime, ref tmpStorePicName, ref tmpStorePath);
            }

        }
       
        //实时抓拍
        public void SDK_OnPlate(System.IntPtr pvParam,
                    [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIP,
                    [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcNumber,
                    [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcColor,
                    System.IntPtr pcPicData, uint u32PicLen, System.IntPtr pcCloseUpPicData, uint u32CloseUpPicLen,
                    short s16PlatePosLeft, short s16PlatePosTop, short s16PlatePosRight, short s16PlatePosBottom,
                    float fPlateConfidence, uint u32VehicleColor, uint u32PlateType, uint u32VehicleDir, uint u32AlarmType,
                    uint u32SerialNum, uint u32Reserved2, uint u32Reserved3, uint u32Reserved4)
        {
            int index = (int)pvParam;
            if (m_bExit || bClose[index])
                return;

            string storePicName = "";
            string storePicPath = "";
            string tmpStorePicName = "";
            string tmpStorePath = "";
            on_plate(pcIP, pcNumber, pcColor, pcPicData, u32PicLen, pcCloseUpPicData, u32CloseUpPicLen,
                s16PlatePosLeft, s16PlatePosTop, s16PlatePosRight, s16PlatePosBottom, fPlateConfidence,
                u32VehicleColor, u32PlateType, u32VehicleDir, u32AlarmType, u32Reserved2, index, ref storePicName, ref storePicPath, ref  tmpStorePicName, ref  tmpStorePath);

            try
            {
                DateTime date = DateTime.Parse("1970-01-01 08:00:00").AddSeconds(u32Reserved2);

                if (pcIP == iPAddr_In)
                {
                    //label10.Text = pcNumber;
                    string picInPath = storePicPath + @"\" + storePicName;
                    //通过委托给控件赋值，不然会报从不是创建控件的线程访问他
                    Invoke(assignmentControl, 1, date, pcNumber, picInPath, null);

                    //读取会员
                    //int memberId = 0;
                    int memberId = psBll.GetMemberId(pcNumber);
                    //打开道闸
                    OpenGate_In();

                    Invoke(assignmentPicControl, 1, date, pcNumber);

                    IntPtr ptr2 = (IntPtr)pcPicData;
                    byte[] datajpg2 = new byte[u32PicLen];
                    Marshal.Copy(ptr2, datajpg2, 0, datajpg2.Length);

                    string pb = Convert.ToBase64String(datajpg2);
                    //生成入场订单
                    OrderModel modeIn = psBll.SaveOrder(1, pcColor, pcNumber, storePicPath, storePicName, tmpStorePath, tmpStorePicName, pb, date, memberId);

                    ScreenShow_In(pcNumber, memberId, modeIn.berthNum, modeIn.ChargeType, modeIn.monthLeftDay);
                   // ScreenShow_In(pcNumber, memberId, 100);
                }
                else
                {


                    //返回入场图片地址
                    string storePicPathIn = "";
                    string picOutPath = storePicPath + @"\" + storePicName;
                    storePicPathIn = psBll.Getpic(pcNumber); ;

                    IntPtr ptr2 = (IntPtr)pcCloseUpPicData;
                    byte[] datajpg2 = new byte[u32CloseUpPicLen];
                    Marshal.Copy(ptr2, datajpg2, 0, datajpg2.Length);
                    string pb = Convert.ToBase64String(datajpg2);

                    if (storePicPathIn != null)
                     {
                        Invoke(assignmentControl, 2, date, pcNumber, storePicPathIn, picOutPath);

                         OrderModel modeOut = psBll.SaveOrder(2, pcColor, pcNumber, storePicPath, storePicName, tmpStorePath, tmpStorePicName, pb, date,0);

                        //赋值私有变量
                        //ormodePrivate.OrderNo = Or.orderNo;
                        //ormodePrivate.ParkingTime = Or.parkingTime;
                        //ormodePrivate.OrderCharge = Or.orderCharge;
                        //ormodePrivate.DiscountAmount = Or.discountAmount;
                        //ormodePrivate.ChargeType = Or.chargeType;
                        //ormodePrivate.ActualAmount=Or.actualAmount;
                        //ormodePrivate.MemberId = Or.memberId;
                        //ormodePrivate.LicensePlateNo = pcNumber;

                         int type = modeOut.ChargeType;
                         int memberId = modeOut.MemberId;
                         int monthLeftDay = modeOut.monthLeftDay;
                        string cartype = null;
                        if (type == 10 && memberId==0)
                        {
                            cartype = "临时车";
                        }
                        else if (type == 10 && memberId != 0)
                        {
                            cartype = "会员车";
                        }
                        if (type == 20)
                        {
                            cartype = "贵宾车";
                        }
                        if (type == 30)
                        {
                            cartype = "包月车";
                        }
                        Invoke(assignmentOutshowLableControl, pcNumber, modeOut.InDate, date, modeOut.ParkingTime, cartype, modeOut.ActualAmount);
            
                        Invoke(assignmentPicControl, 2, date, pcNumber);
                        //if (modeOut.State == 30)
                        //{
                        OrderModel mode3 = psBll.UpdateState(3, modeOut.OrderNo, "", 1, modeOut.ActualAmount, 9999, "", pb, storePicName);
                            OpenGate_Out();
                            ScreenShow_Out(pcNumber, modeOut.ActualAmount, type, monthLeftDay, cartype, mode3.berthNum, 1);
                       // }
                    //OpenGate_Out(pcNumber, 0, 0, 0, "临时车");
                     }
                    else
                    {
                        OpenGate_Out();
                        ScreenShow_Out(pcNumber, 0, 0, 0, "",0,2);
                        Invoke(assignmentOutshowLableControl, "", date, date, 0, null, 0.00);
                        Invoke(assignmentControl, 2, date, pcNumber, null, picOutPath);
                        Invoke(assignmentPicControl, 2, date, pcNumber);
                        //Form4 form4 = new Form4(picOutPath, pcColor, pcNumber, storePicPath, storePicName, tmpStorePath, tmpStorePicName, pb, date,this);
                        //form4.ShowDialog();
                    }
                }
            }catch(Exception ex)
            {
                log.Error("Form1.SDK_OnPlate方法" + ex.Message + "_" + "车牌号：" + pcNumber);
            }
        }

        public void AssignmentControlMethod(int type, DateTime date, string plateNo, string picIn, string picOut) //type = 1 进场，type=2 离场
        {
            if (type == 1)
            {
                lbInDate.Text = date.ToString();
                tbPlateNo_In.Text = plateNo;
                if (picIn != "" && picIn != null)
                    picBox_In_1.Load(picIn);
            }
            else if (type == 2)
            {
                lbOutDate.Text = date.ToString();
                tbPlateNo_Out.Text = plateNo;
                if (picIn != "" && picIn != null && File.Exists(picIn))
                    picBox_In_1.Load(picIn);
                if (picOut != "" && picOut != null && File.Exists(picOut))
                    picBox_Out_1.Load(picOut);

            }

        }


        private  void PictureClear(object source, ElapsedEventArgs e)
        {

            picBox_In_1.Image = null;
            picBox_Out_1.Image = null;

        }

        public void AssignmentPicControlMethod(int type, DateTime date, string plateNo) //type = 1 进场，type=2 离场
        {
            if (type == 1)
            {
                listView1.View = View.Details;
                listView1.FullRowSelect = true;
                ListViewItem item1 = new ListViewItem(date.ToString());
                item1.SubItems.Add(plateNo);
                item1.SubItems.Add("入口");
                item1.SubItems.Add("驶入");


                listView1.Items.AddRange(new ListViewItem[] { item1 });
            }
            else if (type == 2)
            {
                listView1.View = View.Details;
                listView1.FullRowSelect = true;
                ListViewItem item1 = new ListViewItem(date.ToString());
                item1.SubItems.Add(plateNo);
                item1.SubItems.Add("出口");
                item1.SubItems.Add("驶出");


                listView1.Items.AddRange(new ListViewItem[] { item1 });
            }
        }
        public void AssignmentOutshowLableControlMethod(string pcNumber, DateTime Indate, DateTime Outdate, int parkingTime, string carType, double actucalAmount) //type = 1 进场，type=2 离场
        {
            if (pcNumber == "")
            {
                label10.Text = "";
                label22.Text = "";
                label21.Text = "";
                label20.Text = "";
                label18.Text = "";
                label19.Text = "";
                textBox5.Text = "0";
            }
            else
            {
                label10.Text = pcNumber;
                label22.Text = Indate.ToString();
                label21.Text = Outdate.ToString();


                double RD = Math.Floor(Convert.ToDouble(parkingTime / 1440));
                double HD = Math.Floor((Convert.ToDouble(parkingTime % (60 * 24)) / 60));//得到小时
                double MD = parkingTime - RD * 60 * 24 - HD * 60;//得到分数
                string parkingTimeText="";
                if (RD > 0)
                    parkingTimeText = parkingTimeText + RD + "天";
                if (HD > 0)
                    parkingTimeText = parkingTimeText + HD + "小时";

                parkingTimeText = parkingTimeText + MD + "分钟";

                label20.Text = parkingTimeText;
                label18.Text = actucalAmount.ToString();
                label19.Text = carType;
                textBox5.Text = actucalAmount.ToString();
            }
            
        }
        public void AssignmentOutshowLableControlWEITUO(string pcNumber, DateTime Indate, DateTime Outdate, int parkingTime, string carType, double actucalAmount) //type = 1 进场，type=2 离场
        {
            Invoke(assignmentOutshowLableControl, pcNumber, Indate, Outdate, parkingTime, null, actucalAmount);
          
        }
        private void CreateOrderIn(int memberId, string plateNo)
        {

        }

        private void OpenGate_In()
        {

            if (pUid[0] != IntPtr.Zero)
            {
                ipcsdk.ICE_IPCSDK_OpenGate(pUid[0]);
            }
        }

        private void ScreenShow_In(string pcNumber, int memberId, int bertNumLeft, int chargeType, int monthLeftDay)
        {
            string carType = "";
            if (chargeType == 10)
            {
                if (memberId > 0)
                    carType = "会员车";
                else
                    carType = "临时车";
                //string plateNo = Encoding.GetEncoding("GB2312").GetString(Encoding.UTF8.GetBytes(pcNumber));
                //carType = Encoding.GetEncoding("GB2312").GetString(Encoding.UTF8.GetBytes(carType));
                ScreenSdk.Send_To_Show(screenIp_In, 1, carType + " " + pcNumber, 2);
               
                ScreenSdk.Send_To_Voice(screenIp_In, carType + pcNumber + "欢迎光临");
            }
            else if (chargeType == 20)
            {
                carType = "贵宾车";
                ScreenSdk.Send_To_Show(screenIp_In, 1, carType + " " + pcNumber, 2);
                ScreenSdk.Send_To_Voice(screenIp_In, carType + pcNumber + "欢迎光临");
            }
            else
            {
                carType = "包月车";
                ScreenSdk.Send_To_Show(screenIp_In, 1, carType + " " + pcNumber + " 剩余天数：" + monthLeftDay, 2);
                ScreenSdk.Send_To_Voice(screenIp_In, carType + pcNumber + "欢迎光临剩余天数" + monthLeftDay + "天");
            }
            ScreenSdk.Send_To_Show(screenIp_In, 2, "余位：" + bertNumLeft.ToString(), 2);

        }
       
        private void OpenGate_Out()
        {

            if (pUid[1] != IntPtr.Zero)
            {
                ipcsdk.ICE_IPCSDK_OpenGate(pUid[1]);
            }

        }
        #region 出场显示屏显示
        //type 1是有入场记录，2是无入场记录
        private void ScreenShow_Out(string pcNumber, double charge, int ChargeType, int monthLeftDay, string carType, int berthNum, int type)
        {
            ScreenSdk.Send_To_Show(screenIp_Out, 1, carType + " " + pcNumber, 2);
            string chargeStr = "";
            if (charge > 0)
                chargeStr = "收费" + charge.ToString() + "元";
            else
                chargeStr = "一路顺风";

            //ScreenSdk.Send_To_Show(screenIp_Out, 2, chargeStr, 2);
            //ScreenSdk.Send_To_Voice(screenIp_Out, pcNumber + chargeStr);
            ScreenSdk.Send_To_Voice(screenIp_Out, carType + pcNumber + "一路顺风");
            if (type == 1)
            {
                ScreenSdk.Send_To_Show(screenIp_In, 1, "江南爱停车", 2);
                ScreenSdk.Send_To_Show(screenIp_In, 2, "余位：" + berthNum.ToString(), 2);
            }
           

        }
        #endregion
        //断网续传
        public void SDK_OnPastPlate(System.IntPtr pvParam,
            [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIP,
            uint u32CapTime,
            [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcNumber,
            [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcColor,
            System.IntPtr pcPicData, uint u32PicLen, System.IntPtr pcCloseUpPicData, uint u32CloseUpPicLen, short s16PlatePosLeft,
            short s16PlatePosTop, short s16PlatePosRight, short s16PlatePosBottom, float fPlateConfidence, uint u32VehicleColor,
            uint u32PlateType, uint u32VehicleDir, uint u32AlarmType, uint u32Reserved1, uint u32Reserved2, uint u32Reserved3,
            uint u32Reserved4)
        {
            int index = (int)pvParam;
            if (m_bExit || bClose[index])
                return;
            string storePicName = "";
            string storePath = "";
            string tmpStorePicName = "";
            string tmpStorePath = "";
            on_plate(pcIP, pcNumber, pcColor, pcPicData, u32PicLen, pcCloseUpPicData, u32CloseUpPicLen,
                s16PlatePosLeft, s16PlatePosTop, s16PlatePosRight, s16PlatePosBottom, fPlateConfidence,
                u32VehicleColor, u32PlateType, u32VehicleDir, u32AlarmType, u32CapTime, index, ref storePicName, ref storePath, ref  tmpStorePicName, ref  tmpStorePath);
            log.Error("断网续传：" + pcNumber + "  " + DateTime.Now.ToString());
        }

        public void SDK_OnSerialPort(System.IntPtr pvParam,
            [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIP,
            System.IntPtr pcData, uint u32Len)
        {
            if (m_bExit)
                return;

            int index = (int)pvParam;
            IntPtr tmp = pcData;
            byte[] dataPort2 = new byte[u32Len];
            Marshal.Copy(tmp, dataPort2, 0, dataPort2.Length);
            string strPort = BitConverter.ToString(dataPort2);
            strPort = strPort.Replace("-", " ");
            IAsyncResult syncResult = this.BeginInvoke(updatePortInfo, pcIP, u32Len, index, strPort, 0);
        }

        public void SDK_OnSerialPortRS232(System.IntPtr pvParam,
            [System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string pcIP,
            System.IntPtr pcData, uint u32Len)
        {
            if (m_bExit)
                return;
            int index = (int)pvParam;
            IntPtr tmp = pcData;
            byte[] dataPort2 = new byte[u32Len];
            Marshal.Copy(tmp, dataPort2, 0, dataPort2.Length);
            string strPort = BitConverter.ToString(dataPort2);
            strPort = strPort.Replace("-", " ");
            IAsyncResult syncResult = this.BeginInvoke(updatePortInfo, pcIP, u32Len, index, strPort, 1);
        }

        public void showStatus(int index, int type)
        {
            switch (type)
            {
                case 0:
                    if (nStatus[index] == 1)
                    {
                        labelStatus[index].Text = "在线 " + strMac[index];
                    }
                    else
                        labelStatus[index].Text = "离线 " + strMac[index];
                    break;
                case 1:
                    textBoxGateNum[index].Text = nGateNum[index].ToString();
                    break;
                case 2:
                    textBoxTriggerNum[index].Text = nTriggerNum[index].ToString();
                    break;
                case 3:
                    nGate2Num[index]++;
                    string strText = nGate2Num[index].ToString() + ": sdk" + (index + 1).ToString() + " 打开道闸2成功。";
                    listBoxInfo.Items.Insert(0, strText);
                    break;
                case 4:
                    nRS485Num[index]++;
                    string strTextRS485 = nRS485Num[index].ToString() + ": sdk" + (index + 1).ToString() + " 发送RS485数据成功";
                    listBoxInfo.Items.Insert(0, strTextRS485);
                    break;
                case 5:
                    nRS232Num[index]++;
                    string strTextRS232 = nRS232Num[index].ToString() + ": sdk" + (index + 1).ToString() + " 发送RS232数据成功";
                    listBoxInfo.Items.Insert(0, strTextRS232);
                    break;
            }
        }

        public void showTriggerStatus(int index, uint nStatus)
        {
            string strText = "相机" + (index + 1).ToString();
            switch (nStatus)
            {
                case 0:
                    strText += "：软触发失败";
                    break;
                case 2:
                    strText += ": 正在识别";
                    break;
                case 3:
                    strText += ": 算法未启动";
                    break;
            }

            if (strText == null)
                return;
            listBoxInfo.Items.Insert(0, strText);
        }

        public void showPortData(string strIp, uint len, int index, string data, int type)
        {
            string strText = "";
            if (type == 0)
            {
                nRecvPortCount_RS485[index]++;
                strText = nRecvPortCount_RS485[index].ToString() + ":" + strIp + "接收到RS485数据 " + len.ToString() + "字节";
            }
            else if (type == 1)
            {
                nRecvPortCount_RS232[index]++;
                strText = nRecvPortCount_RS232[index].ToString() + ":" + strIp + "接收到RS232数据 " + len.ToString() + "字节"; ;
            }

            if (data != null)
                listBoxInfo.Items.Insert(0, data);
            if (strText != null)
                listBoxInfo.Items.Insert(0, strText);
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            if (form2.ShowDialog() == DialogResult.OK)
            {
                loadConfig(form2.m_strStorePath, form2.m_bOpenGate, form2.m_bTrigger, form2.m_nOpenInterval, form2.m_nTriggerInterval, form2.m_nRecordInterval,
                    form2.m_bOpenGate2, form2.m_bRS485, form2.m_bRS232, form2.m_nOpenInterval2, form2.m_nRS485Interval, form2.m_nRS232Interval);
                loadOsdConfig(form2.m_nVideoOsd, form2.m_nVideoColor, form2.m_bVideoDate, form2.m_bVideoLicense, form2.m_bVideoCustom, form2.m_strVideoCustom,
                        form2.m_nJpegOsd, form2.m_nJpegColor, form2.m_bJpegDate, form2.m_bJpegAlgo, form2.m_bJpegCustom, form2.m_strJpegCustom);
                ipcsdk.ICE_IPCSDK_LogConfig(form2.m_bEnableLog, form2.m_strLogPath);
            }
            form2.Dispose();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void ShowData()
        {
            //dataGridView1.DataSource = dtAttachment;
            //dataGridView1.DataBindings
            //DataTable dt = ParkSystem.GetNowData();
            // dataGridView1.DataSource = dt;

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            OrderqueryForm form3 = new OrderqueryForm();

            form3.ShowDialog();
        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void picBox_Out_Click(object sender, EventArgs e)
        {

        }

        private void picBox_Out_1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = System.DateTime.Now.ToString();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            OpenGate_Out();
          
            //if (pUid[1] != IntPtr.Zero)
            //{
            //    ipcsdk.ICE_IPCSDK_OpenGate(pUid[1]);
            //}
             double Amount = Convert.ToDouble(textBox5.Text);
             int chargeEmp = Convert.ToInt16(ChargeEmpTextBox.Text);
             ormodePrivate.Note = "";
             ormodePrivate.ActualGetAmount = Amount;
            // int berthNum = psBll.UpdateState(3, ormodePrivate, 4, chargeEmp);
             int memberId = ormodePrivate.MemberId;
             string cartype = "";
             if (memberId > 0)
                 cartype = "会员车";
             else
                 cartype = "临时车";

            // ScreenShow_Out(ormodePrivate.LicensePlateNo, Amount, ormodePrivate.ChargeType, 0, cartype, berthNum, 1);
        }

        private void button17_Click(object sender, EventArgs e)
        {
          
            string  note = textBox5.Text;
            int chargeEmp = Convert.ToInt16(ChargeEmpTextBox.Text);
            OpenGate_Out();
            
            ormodePrivate.Note = note;
            ormodePrivate.ActualGetAmount = 0;
           // int berthNum = psBll.UpdateState(3, ormodePrivate, 4, chargeEmp);
            int memberId = ormodePrivate.MemberId;
            string cartype = "";
            if (memberId > 0)
                cartype = "会员车";
            else
                cartype = "临时车";

           // ScreenShow_Out(ormodePrivate.LicensePlateNo, 0, ormodePrivate.ChargeType, 0, cartype, berthNum, 1);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            asc.ReSize(this); 
        }

        private void tbPlateNo_In_TextChanged(object sender, EventArgs e)
        {

        }

    }

    #region   util类
    public class util
    {
        private static int width;
        private static int height;
        private static int length;
        private static int v;  //v值的起始位置
        private static int u;  //u值的起始位置
        private static int rdif, invgdif, bdif;
        private static int[] Table_fv1;
        private static int[] Table_fv2;
        private static int[] Table_fu1;
        private static int[] Table_fu2;
        private static int[] rgb = new int[3];
        private static int m, n, i, j, hfWidth;
        private static bool addHalf = true;
        private static int py;
        private static int pos, pos1;//dopod 595 图像调整
        private static byte temp;

        public static void YV12ToRGB(int iWidth, int iHeight)
        {
            Table_fv1 = new int[256] { -180, -179, -177, -176, -174, -173, -172, -170, -169, -167, -166, -165, -163, -162, -160, -159, -158, -156, -155, -153, -152, -151, -149, -148, -146, -145, -144, -142, -141, -139, -138, -137, -135, -134, -132, -131, -130, -128, -127, -125, -124, -123, -121, -120, -118, -117, -115, -114, -113, -111, -110, -108, -107, -106, -104, -103, -101, -100, -99, -97, -96, -94, -93, -92, -90, -89, -87, -86, -85, -83, -82, -80, -79, -78, -76, -75, -73, -72, -71, -69, -68, -66, -65, -64, -62, -61, -59, -58, -57, -55, -54, -52, -51, -50, -48, -47, -45, -44, -43, -41, -40, -38, -37, -36, -34, -33, -31, -30, -29, -27, -26, -24, -23, -22, -20, -19, -17, -16, -15, -13, -12, -10, -9, -8, -6, -5, -3, -2, 0, 1, 2, 4, 5, 7, 8, 9, 11, 12, 14, 15, 16, 18, 19, 21, 22, 23, 25, 26, 28, 29, 30, 32, 33, 35, 36, 37, 39, 40, 42, 43, 44, 46, 47, 49, 50, 51, 53, 54, 56, 57, 58, 60, 61, 63, 64, 65, 67, 68, 70, 71, 72, 74, 75, 77, 78, 79, 81, 82, 84, 85, 86, 88, 89, 91, 92, 93, 95, 96, 98, 99, 100, 102, 103, 105, 106, 107, 109, 110, 112, 113, 114, 116, 117, 119, 120, 122, 123, 124, 126, 127, 129, 130, 131, 133, 134, 136, 137, 138, 140, 141, 143, 144, 145, 147, 148, 150, 151, 152, 154, 155, 157, 158, 159, 161, 162, 164, 165, 166, 168, 169, 171, 172, 173, 175, 176, 178 };
            Table_fv2 = new int[256] { -92, -91, -91, -90, -89, -88, -88, -87, -86, -86, -85, -84, -83, -83, -82, -81, -81, -80, -79, -78, -78, -77, -76, -76, -75, -74, -73, -73, -72, -71, -71, -70, -69, -68, -68, -67, -66, -66, -65, -64, -63, -63, -62, -61, -61, -60, -59, -58, -58, -57, -56, -56, -55, -54, -53, -53, -52, -51, -51, -50, -49, -48, -48, -47, -46, -46, -45, -44, -43, -43, -42, -41, -41, -40, -39, -38, -38, -37, -36, -36, -35, -34, -33, -33, -32, -31, -31, -30, -29, -28, -28, -27, -26, -26, -25, -24, -23, -23, -22, -21, -21, -20, -19, -18, -18, -17, -16, -16, -15, -14, -13, -13, -12, -11, -11, -10, -9, -8, -8, -7, -6, -6, -5, -4, -3, -3, -2, -1, 0, 0, 1, 2, 2, 3, 4, 5, 5, 6, 7, 7, 8, 9, 10, 10, 11, 12, 12, 13, 14, 15, 15, 16, 17, 17, 18, 19, 20, 20, 21, 22, 22, 23, 24, 25, 25, 26, 27, 27, 28, 29, 30, 30, 31, 32, 32, 33, 34, 35, 35, 36, 37, 37, 38, 39, 40, 40, 41, 42, 42, 43, 44, 45, 45, 46, 47, 47, 48, 49, 50, 50, 51, 52, 52, 53, 54, 55, 55, 56, 57, 57, 58, 59, 60, 60, 61, 62, 62, 63, 64, 65, 65, 66, 67, 67, 68, 69, 70, 70, 71, 72, 72, 73, 74, 75, 75, 76, 77, 77, 78, 79, 80, 80, 81, 82, 82, 83, 84, 85, 85, 86, 87, 87, 88, 89, 90, 90 };
            Table_fu1 = new int[256] { -44, -44, -44, -43, -43, -43, -42, -42, -42, -41, -41, -41, -40, -40, -40, -39, -39, -39, -38, -38, -38, -37, -37, -37, -36, -36, -36, -35, -35, -35, -34, -34, -33, -33, -33, -32, -32, -32, -31, -31, -31, -30, -30, -30, -29, -29, -29, -28, -28, -28, -27, -27, -27, -26, -26, -26, -25, -25, -25, -24, -24, -24, -23, -23, -22, -22, -22, -21, -21, -21, -20, -20, -20, -19, -19, -19, -18, -18, -18, -17, -17, -17, -16, -16, -16, -15, -15, -15, -14, -14, -14, -13, -13, -13, -12, -12, -11, -11, -11, -10, -10, -10, -9, -9, -9, -8, -8, -8, -7, -7, -7, -6, -6, -6, -5, -5, -5, -4, -4, -4, -3, -3, -3, -2, -2, -2, -1, -1, 0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 11, 11, 11, 12, 12, 12, 13, 13, 13, 14, 14, 14, 15, 15, 15, 16, 16, 16, 17, 17, 17, 18, 18, 18, 19, 19, 19, 20, 20, 20, 21, 21, 22, 22, 22, 23, 23, 23, 24, 24, 24, 25, 25, 25, 26, 26, 26, 27, 27, 27, 28, 28, 28, 29, 29, 29, 30, 30, 30, 31, 31, 31, 32, 32, 33, 33, 33, 34, 34, 34, 35, 35, 35, 36, 36, 36, 37, 37, 37, 38, 38, 38, 39, 39, 39, 40, 40, 40, 41, 41, 41, 42, 42, 42, 43, 43 };
            Table_fu2 = new int[256] { -227, -226, -224, -222, -220, -219, -217, -215, -213, -212, -210, -208, -206, -204, -203, -201, -199, -197, -196, -194, -192, -190, -188, -187, -185, -183, -181, -180, -178, -176, -174, -173, -171, -169, -167, -165, -164, -162, -160, -158, -157, -155, -153, -151, -149, -148, -146, -144, -142, -141, -139, -137, -135, -134, -132, -130, -128, -126, -125, -123, -121, -119, -118, -116, -114, -112, -110, -109, -107, -105, -103, -102, -100, -98, -96, -94, -93, -91, -89, -87, -86, -84, -82, -80, -79, -77, -75, -73, -71, -70, -68, -66, -64, -63, -61, -59, -57, -55, -54, -52, -50, -48, -47, -45, -43, -41, -40, -38, -36, -34, -32, -31, -29, -27, -25, -24, -22, -20, -18, -16, -15, -13, -11, -9, -8, -6, -4, -2, 0, 1, 3, 5, 7, 8, 10, 12, 14, 15, 17, 19, 21, 23, 24, 26, 28, 30, 31, 33, 35, 37, 39, 40, 42, 44, 46, 47, 49, 51, 53, 54, 56, 58, 60, 62, 63, 65, 67, 69, 70, 72, 74, 76, 78, 79, 81, 83, 85, 86, 88, 90, 92, 93, 95, 97, 99, 101, 102, 104, 106, 108, 109, 111, 113, 115, 117, 118, 120, 122, 124, 125, 127, 129, 131, 133, 134, 136, 138, 140, 141, 143, 145, 147, 148, 150, 152, 154, 156, 157, 159, 161, 163, 164, 166, 168, 170, 172, 173, 175, 177, 179, 180, 182, 184, 186, 187, 189, 191, 193, 195, 196, 198, 200, 202, 203, 205, 207, 209, 211, 212, 214, 216, 218, 219, 221, 223, 225 };
            width = iWidth;
            height = iHeight;
            length = iWidth * iHeight;
            v = length;//nYLen
            u = v + (length >> 2);
            hfWidth = iWidth >> 1;
            addHalf = true;
        }

        public static bool Convert(int cwidth, int cheight, byte[] yv12y, byte[] yv12u, byte[] yv12v, ref  byte[] rgb24)
        {
            try
            {
                YV12ToRGB(cwidth, cheight);
                if (yv12y.Length == 0 || rgb24.Length == 0)
                    return false;
                m = -width;
                n = -hfWidth;
                for (int y = 0; y < height; y++)
                {
                    if (y == 139)
                    {
                    }
                    m += width;
                    if (addHalf)
                    {
                        n += hfWidth;
                        addHalf = false;
                    }
                    else
                    {
                        addHalf = true;
                    }
                    for (int x = 0; x < width; x++)
                    {
                        i = m + x;
                        j = n + (x >> 1);
                        py = (int)yv12y[i];
                        rdif = Table_fv1[(int)yv12v[j]];
                        invgdif = Table_fu1[(int)yv12u[j]] + Table_fv2[(int)yv12v[j]];
                        bdif = Table_fu2[(int)yv12u[j]];

                        rgb[2] = py + rdif;//R
                        rgb[1] = py - invgdif;//G
                        rgb[0] = py + bdif;//B

                        j = v - width - m + x;
                        i = (j << 1) + j;

                        // copy this pixel to rgb data
                        for (j = 0; j < 3; j++)
                        {

                            if (rgb[j] >= 0 && rgb[j] <= 255)
                            {
                                rgb24[i + j] = (byte)rgb[j];
                            }
                            else
                            {
                                rgb24[i + j] = (byte)((rgb[j] < 0) ? 0 : 255);
                            }

                        }
                        if (x % 4 == 3)
                        {
                            pos = (m + x - 1) * 3;
                            pos1 = (m + x) * 3;
                            temp = rgb24[pos];
                            rgb24[pos] = rgb24[pos1];
                            rgb24[pos1] = temp;

                            temp = rgb24[pos + 1];
                            rgb24[pos + 1] = rgb24[pos1 + 1];
                            rgb24[pos1 + 1] = temp;

                            temp = rgb24[pos + 2];
                            rgb24[pos + 2] = rgb24[pos1 + 2];
                            rgb24[pos1 + 2] = temp;
                        }
                    }
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
            return true;
        }
    }
    #endregion
}

