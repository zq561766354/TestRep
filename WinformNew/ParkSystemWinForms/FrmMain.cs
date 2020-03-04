using Bealead.ICEIPC;
using Bealead.ICEIPC.Enums;
using Bealead.ICEIPC.Events;
using ParkSystemWinForms.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkSystemWinForms
{
    public partial class FrmMain : Form
    {
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        public const int WM_CLOSE = 0x10;

        private string moduleName = "";//本执行程序模块名称
        private string processDir = "";//本执行程序目录
        private string startDir = "";//开始菜单目录

        private bool formEntered = false;//界面是否打开进入打开过
        private bool isMaximizied = false;//界面是否已经进入最大化
        private double normalFHeight = 0;//窗体普通状态时的高度
        private double maxFHeight = 0;//窗体最大化状态时的高度
        private double normalFWidth = 0;//窗体普通状态时的宽度
        private double maxFWidth = 0;//窗体最大化状态时的宽度
        private Color normalBGColor = ColorTranslator.FromHtml("#1199FF");//几个常规的按钮背景颜色

        private bool netWorkOk = true;//连网:true;断网:false
        private int heartTimerEslapsed = 0;    //心跳timer逝去的秒数
        private int heartExpiryStep = 0; //心跳未响应次数记录

        private System.Threading.Timer alwaysOpenTimer = null;//常开道闸线程
        private System.Threading.Timer dataHandleTimer = null;//同步数据线程
        private System.Threading.Timer checkOrderTimer = null;//检测订单状态线程
        private System.Threading.Timer renewalBrokenNetOrderTimer = null;//断网续传订单线程
        private System.Threading.Timer checkLeftCountTimer = null;//检查停车场余位线程
        private System.Threading.Timer checkUnidentifiedTimer = null;//检查出场未识别订单线程
        private List<int> alwaysOpenIPs = new List<int>();    //道闸常开集合
        private List<OrderIpc> checkLeftCountOpenIPC = new List<OrderIpc>();    //检查余位抬杆集合

        private static LogHelper log = LogFactory.GetLogger("PsFormLog");
        private Setting setting = new Setting();
        private ParkSystemBLL psBll = new ParkSystemBLL();
        private ICEIPC mysdk = null;
        private static object qtlocker = new object();

        private List<HandleOrder> handles = new List<HandleOrder>(); //当前每个出入口正在处理的订单
        private Dictionary<string, OrderSnapshot> snapDict = new Dictionary<string, OrderSnapshot>();//每个出入口之前处理的订单快照

        private delegate void ControlUpdate(string guardName, string ip);
        private ControlUpdate CtlUpdateDele;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)//禁止拖动
                return;
            base.WndProc(ref m);
            /*
            Rectangle vRectangle = new Rectangle(3, 3, Width - 6, 21);
            switch (m.Msg)
            {
                case WM_NCPAINT:
                case WM_NCACTIVATE:
                    IntPtr vHandle = GetWindowDC(m.HWnd);
                    Graphics vGraphics = Graphics.FromHdc(vHandle);
                    vGraphics.FillRectangle(new LinearGradientBrush(vRectangle,
                        Color.Pink, Color.Purple, LinearGradientMode.BackwardDiagonal),
                        vRectangle);

                    StringFormat vStringFormat = new StringFormat();
                    vStringFormat.Alignment = StringAlignment.Center;
                    vStringFormat.LineAlignment = StringAlignment.Center;
                    vGraphics.DrawString(this.Name, Font, Brushes.BlanchedAlmond,
                        vRectangle, vStringFormat);

                    vGraphics.Dispose();
                    ReleaseDC(m.HWnd, vHandle);
                    break;
                case WM_NCLBUTTONDOWN:
                    Point vPoint = new Point((int)m.LParam);
                    vPoint.Offset(-Left, -Top);
                    if (vRectangle.Contains(vPoint))
                        MessageBox.Show(vPoint.ToString());
                    break;
            }*/
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        private const int WM_NCPAINT = 0x0085;
        private const int WM_NCACTIVATE = 0x0086;
        private const int WM_NCLBUTTONDOWN = 0x00A1;

        public FrmMain()
        {
            InitializeComponent();
            psBll.LoadConfig(ref setting);      //读写配置文件
            Params.Settings = setting;          //配置文件参数赋予Setting类
            try
            {
                //通过停车场编号读取停车场名称
                DataTable parkdt = psBll.GetParkingLotInfo(1, Params.Settings.ParkLot.No);
                if (parkdt.Rows.Count > 0)
                    Params.Settings.ParkLot.Name = parkdt.Rows[0]["ParkingLotName"].ToString();
            }
            catch (Exception ex) 
            {
                MessageBox.Show("读取停车场失败，请检查数据库连接！");
            }
            //从app.config中读取停车场Id并保存到Setting类中
            if(ConfigurationManager.AppSettings["ParkingLotId"] != null)
                Params.Settings.ParkLot.Id = ConfigurationManager.AppSettings["ParkingLotId"];
            //读取当前程序的物理路径
            processDir = System.Windows.Forms.Application.ExecutablePath;// Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            moduleName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
            startDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
            moduleName = moduleName.Replace("vshost.", "");
            processDir = processDir.Substring(0, processDir.LastIndexOf("\\") + 1);
            //设置超时时间
            ParkSystemUtility.timout = Params.Settings.apiTimeout;
        }

        #region Timer等其他
        private void Timer_Tick(object sender, EventArgs e)
        {
            KillMessageBox();
            //停止Timer 
            //((System.Windows.Forms.Timer)sender).Stop();
        }
        private void StartMsgBoxKiller()
        {
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 3000;
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }
        private void KillMessageBox()
        {
            //按照MessageBox的标题，找到MessageBox的窗口 
            IntPtr ptr = FindWindow(null, "MessageBox");
            if (ptr != IntPtr.Zero)
            {
                //找到则关闭MessageBox窗口 
                PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            }
        }
        #endregion

        #region 开机自启动
        /// <summary>
        /// 开机自启动
        /// </summary>
        private void BootOnStart()
        {
            try
            {
                /*
                //方式一
                // 获取全局 开始 文件夹位置
                string startDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
                // 获取当前登录用户的 开始 文件夹位置
                //Environment.GetFolderPath(Environment.SpecialFolder.Startup);

                SystemHelper.Create(startDir, moduleName, processDir + moduleName, string.Format("停车场收费客户端v1.0_{0}({1})", setting.ParkLot.Name, setting.ParkLot.No), null);
                  */

                //方式二
     
                string apppath = Application.ExecutablePath;
                string appname = apppath.Substring(apppath.LastIndexOf("\\") + 1);
                if (!File.Exists(apppath))
                    return;

                Microsoft.Win32.RegistryKey Rkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (Rkey == null)
                    Rkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                Rkey.SetValue(appname, apppath);

                log.Error("设置成开机启动成功,apppath:" + apppath + ",appname:" + appname);
                
            }
            catch (Exception ex)
            {
                log.Error("无法设置成开机启动," + ex.Message);
                MessageBox.Show("无法设置成开机启动," + ex.Message);
            }
        }
        #endregion

        #region 检测网络情况
        /// <summary>
        /// 检测网络情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void heartBeatTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                int freq = Convert.ToInt32(setting.Serv.HeartBeatFreq);
                int maxcount = Convert.ToInt32(setting.Serv.HeartBeatMaxRetryCount);
                heartTimerEslapsed++;
                if (heartTimerEslapsed == freq)
                {
                    heartTimerEslapsed = 0;
                    int result = psBll.NetworkHead();
                    if (result == 0)
                    {
                        heartExpiryStep++;
                    }
                    else
                    {
                        heartExpiryStep = 0;
                        netWorkOk = true;
                    }
                    if (heartExpiryStep >= maxcount)
                    {
                        netWorkOk = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region 保持道闸常开
        /// <summary>
        /// 保持道闸常开
        /// </summary>
        private void KeepOpenGate()
        {
            for (int i = alwaysOpenIPs.Count - 1; i >= 0; i--)
            {
                if (mysdk != null && mysdk.IPCs.Count > 0)
                {
                    int item = alwaysOpenIPs[i];
                    Guard guard = setting.Guards[item];
                    IPC ipc = mysdk.IPCs.First(a => a.IP == guard.Primary.IP);
                    ipc.OpenGate();
                }
            }
        }

        //道闸常开CheckBox事件
        private void ChkKeepOpen_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Control ctl = checkBox.Parent.Controls.Cast<Control>().FirstOrDefault(a => a.Name.Contains("Raise"));
            Button btn = ctl as Button;
            if (checkBox.Checked)
                alwaysOpenIPs.Add(tabControl1.SelectedIndex);
            else
                alwaysOpenIPs.Remove(tabControl1.SelectedIndex);
            ChangeCommBtnStatus(!checkBox.Checked, btn);
        }
        #endregion

        /// <summary>
        /// 识别到车牌号
        /// </summary>
        /// <param name="args"></param>
        private void on_plate(PlateEventArgs args, string ip, ref string sightName, ref string sightPath, ref string plateName, ref string platePath)
        {
            //保存抓拍到的车牌全景图和车牌识别图
            StorePic(args.SightImage, ip, args.PlateNum, false, args.CaptureTime, ref sightName, ref sightPath);
            StorePic(args.PlateImage, ip, args.PlateNum, true, args.CaptureTime, ref plateName, ref platePath);
        }

        #region  保存出入场图片
        /// <summary>
        /// 保存出入场图片
        /// </summary>
        /// <param name="picData"></param>
        /// <param name="ip"></param>
        /// <param name="plateNumber"></param>
        /// <param name="isPlate"></param>
        /// <param name="capTime"></param>
        /// <param name="picSaveName"></param>
        /// <param name="picSavePath"></param>
        public void StorePic(byte[] picData, string ip, string plateNumber, bool isPlate, DateTime capTime, ref string picSaveName, ref string picSavePath)
        {
            string dir = string.Format("{0}\\{1}\\{2}", setting.ImagePath, ip, capTime.ToString("yyyyMMdd"));
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string picName = capTime.ToString("yyyyMMddHHmmss") + "_" + plateNumber;
            if (isPlate)
            {
                picName += "_plate";
            }
            picName += ".jpg";
            string picFullPath = dir + @"\" + picName;

            if (File.Exists(picFullPath))
            {
                int count = 1;
                while (count <= 10)
                {
                    picFullPath = dir + @"\" + capTime.ToString("yyyyMMddHHmmss") + "_" + plateNumber;
                    if (isPlate)
                    {
                        picFullPath += "_plate";
                    }
                    picFullPath += "_" + count.ToString() + ".jpg";

                    if (!File.Exists(picFullPath))
                    {
                        break;
                    }
                    count++;
                }
            }

            try
            {
                FileStream fs = new FileStream(picFullPath, FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(picData);
                bw.Close();
                fs.Close();
                picSaveName = picName;
                picSavePath = dir;
            }
            catch (System.Exception ex)
            {

            }
        }
        #endregion

        #region 获取车辆类型
        /// <summary>
        /// 获取车辆类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        private string GetCarType(int type, int memberId)
        {
            if (type == 10 && memberId == 0)
            {
                return "临时车";
            }
            else if (type == 10 && memberId != 0)
            {
                return "会员车";
            }
            else if (type == 20)
            {
                return "白名单车";
            }
            else if (type == 30)
            {
                return "包月车";
            }
            else if (type == 40)
            {
                return "特种车";

            }
            else if (type == 50)
            {
                return "临时车";
            }
            return "临时车";
        }
        #endregion

        #region 处理识别到的图片
        /// <summary>
        /// 处理识别到的图片
        /// </summary>
        /// <param name="args"></param>
        private void SaveCapPicture(PlateEventArgs args, string ip, ref string base64, ref string pic1Path, ref string pic1Name, ref string pic2Path, ref string pic2Name)
        {
            if (args.SightImage != null && args.SightImage.Length > 0)
            {
                base64 = Convert.ToBase64String(args.SightImage);
                on_plate(args, ip, ref pic1Name, ref pic1Path, ref pic2Name, ref pic2Path);
            }
            else
            {
                log.Error("【抓拍异常】,车牌号:'" + args.PlateNum + "',未抓拍到图片，相机IP:"+ip);
            }
        }
        #endregion

        #region 设备在线、离线变动
        /// <summary>
        /// 设备在线、离线变动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void IPC_StatusChangeEvent(object sender, DevStatusChangeEventArgs args)
        {
            //执行一些业务(比如记录日志 尝试重连、或者推送断连消息给服务器等操作)
            IPC ipc = (IPC)sender;
            Console.WriteLine(string.Format("{0},ip:{1},出入口:{2},状态:{3}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ipc.IP, ipc.Type ? "出口" : "入口", ipc.IsAlive ? "在线" : "离线"));
            string msg = string.Format("ip:{0},出入口:{1},状态:{2}", ipc.IP, ipc.Type ? "出口" : "入口", ipc.IsAlive ? "在线" : "离线");
            if (mysdk.IPCs.Count < 1)
                return;
            if (ipc.IsAlive)
                log.Info(msg);
            else
                log.Warn(msg);

            //疑问
            int findIndex = mysdk.IPCs.FindIndex(a => a.IP == ipc.IP);
            if (findIndex >= 0)
            {
                Control ctl = tabControl1.TabPages[findIndex].Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("video"));
                if (ctl != null)
                {
                    Control targetCtl = ctl.Controls.Cast<Control>().First(a => a.GetType().FullName == "System.Windows.Forms.PictureBox");
                    PictureBox pb = targetCtl as PictureBox;
                    //Control bctl = ctl.Controls.Cast<Control>().First(a => a.Name.Contains("Raise"));
                    if (ipc.IsAlive)
                        pb.Image = null;
                    else
                        pb.Image = Image.FromFile(processDir + "\\images\\nosignal.jpg");
                }
            }
            /*
            //可能由于网络波动等情况,尝试在1.5s后重连
            Thread.Sleep(1500);
            ipc.Connect(ipc.PfOnPlate, ipc.PfOnPastPlate);
           */
        }
        #endregion

        #region 设备IO变化
        /// <summary>
        /// 设备IO变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void IPC_IOChangeEvent(object sender, DevIOChangeEventArgs args)
        {
        }
        #endregion

        #region  加载相机数据
        /// <summary>
        /// 加载相机数据
        /// </summary>
        private void LoadCameras()
        {
            if (mysdk != null)
                mysdk.Fini();

            handles.Clear();
            snapDict.Clear();

            List<Cond> entList = new List<Cond>();
            List<Cond> extList = new List<Cond>();
            if (!Params.Settings.EnabledShowBnt)
            {
                btnRaise1.Visible = false;
                btnPersonPass.Visible = false;
            }
            else
            {
                btnRaise1.Visible = true;
                btnPersonPass.Visible = true;
            }

            for (int i = 0; i < setting.Guards.Count; i++)
            {
                int pageCount = tabControl1.TabPages.Count;
                Guard guard = setting.Guards[i];
                string guardName = string.Format("{0}_{1}", guard.IsExit ? "出口" : "入口", guard.No);

                TabPage tabPage = null;
                GroupBox gbVideo = null;
                PictureBox pbVideo = null;

                //主相机
                Cond cond = new Cond();
                cond.IP = guard.Primary.IP;
                if (guard.IsExit)
                    extList.Add(cond);
                else
                    entList.Add(cond);
                if (i == 0)
                {
                    tabPage1.Text = guardName;
                    tabPage1.Tag = guard.IsExit;
                    if (entList.Count == 1 && extList.Count == 0 || extList.Count == 1 && entList.Count == 0)
                    {
                        if (entList.Count > 0)
                            entList[0].VideoHwnd = pbVideo1.Handle;
                        else
                            extList[0].VideoHwnd = pbVideo1.Handle;
                    }
                }
                else
                {
                    bool pageHaved = tabControl1.TabPages.Cast<TabPage>().Count(a => a.Text == guardName) > 0;
                    if (!pageHaved)
                    {
                        tabPage = new TabPage(guardName);
                        tabPage.Tag = guard.IsExit;
                        tabControl1.TabPages.Add(tabPage);

                        tabPage.TabIndex = pageCount;
                        tabPage.Name = "tabPage" + (pageCount + 1);
                        tabPage.Padding = tabPage1.Padding;
                        tabPage.Size = tabPage1.Size;
                        tabPage.UseVisualStyleBackColor = true;

                        gbVideo = new System.Windows.Forms.GroupBox();
                        GroupBox gbImage = new System.Windows.Forms.GroupBox();
                        tabPage.Controls.Add(gbVideo);
                        tabPage.Controls.Add(gbImage);

                        gbVideo.Location = gbVideo1.Location;
                        gbVideo.Name = "gbVideo" + (pageCount + 1);
                        gbVideo.Tag = "video";
                        gbVideo.Size = gbVideo1.Size;
                        gbVideo.TabIndex = 0;
                        gbVideo.TabStop = false;

                        pbVideo = new PictureBox();
                        pbVideo.SizeMode = PictureBoxSizeMode.StretchImage;
                        pbVideo.Anchor = pbVideo1.Anchor;
                        pbVideo.Location = pbVideo1.Location;
                        pbVideo.Name = "pbVideo" + (pageCount + 1);
                        pbVideo.Size = pbVideo1.Size;
                        pbVideo.TabIndex = 0;
                        pbVideo.TabStop = false;
                        pbVideo.Image = Image.FromFile(processDir + "\\images\\nosignal.jpg");
                        pbVideo.Visible = true;
                        cond.VideoHwnd = pbVideo.Handle;

                        if (Params.Settings.EnabledShowBnt)
                        {

                            Button btnRaise = new Button();
                            btnRaise.Location = btnRaise1.Location;
                            btnRaise.Size = btnRaise1.Size;
                            btnRaise.Font = btnRaise1.Font;
                            btnRaise.Name = "btnRaise" + (pageCount + 1);
                            btnRaise.Anchor = btnRaise1.Anchor;
                            btnRaise.TabIndex = btnRaise1.TabIndex;
                            btnRaise.Text = btnRaise1.Text;
                            btnRaise.UseVisualStyleBackColor = true;
                            btnRaise.ForeColor = Color.White;
                            btnRaise.Click += new System.EventHandler(this.btnRaise_Click);
                            ChangeCommBtnStatus(true, btnRaise);

                            if (!guard.IsExit)
                            {
                                Button btnPerPass = new Button();
                                btnPerPass.Location = btnPersonPass.Location;
                                btnPerPass.Location = btnPersonPass.Location;
                                btnPerPass.Size = btnPersonPass.Size;
                                btnPerPass.Font = btnPersonPass.Font;
                                btnPerPass.Name = "btnPersonPass" + (pageCount + 1);
                                btnPerPass.Anchor = btnPersonPass.Anchor;
                                btnPerPass.TabIndex = btnPersonPass.TabIndex;
                                btnPerPass.Text = btnPersonPass.Text;
                                btnPerPass.ForeColor = Color.White;
                                btnPerPass.Click += new EventHandler(this.btnRaiseOther_Click);
                                ChangeCommBtnStatus(true, btnPerPass);
                                gbVideo.Controls.Add(btnPerPass);
                            }
                            gbVideo.Controls.Add(btnRaise);
                        }


                        CheckBox chkKeepOpen = new CheckBox();
                        chkKeepOpen.Anchor = chkKeepOpen1.Anchor;
                        chkKeepOpen.AutoSize = true;
                        chkKeepOpen.Location = chkKeepOpen1.Location;
                        chkKeepOpen.Name = "chkKeepOpen" + (pageCount + 1);
                        chkKeepOpen.Font = chkKeepOpen1.Font;
                        chkKeepOpen.Size = chkKeepOpen1.Size;
                        chkKeepOpen.TabIndex = chkKeepOpen1.TabIndex;
                        chkKeepOpen.Text = chkKeepOpen1.Text;
                        chkKeepOpen.UseVisualStyleBackColor = true;
                        chkKeepOpen.CheckedChanged += new System.EventHandler(this.ChkKeepOpen_CheckedChanged);

                        gbVideo.Controls.Add(chkKeepOpen);

                        gbVideo.Controls.Add(pbVideo);

                        gbImage.Location = gbImage1.Location;
                        gbImage.Name = "gbImage" + (pageCount + 1);
                        gbImage.Tag = "images";
                        gbImage.Size = gbImage1.Size;
                        gbImage.TabIndex = 1;
                        gbImage.TabStop = false;

                        GroupBox gbImage_2 = new System.Windows.Forms.GroupBox();
                        GroupBox gbImage_1 = new System.Windows.Forms.GroupBox();

                        gbImage.Controls.Add(gbImage_2);
                        gbImage.Controls.Add(gbImage_1);

                        gbImage_2.Anchor = gbImage1_2.Anchor;
                        gbImage_2.Location = gbImage1_2.Location;
                        gbImage_2.Font = gbImage1_2.Font;
                        gbImage_2.Name = "gbImage" + (pageCount + 1) + "_2";
                        gbImage_2.Tag = "image_exit";
                        gbImage_2.Size = gbImage1_2.Size;
                        gbImage_2.TabIndex = 1;
                        gbImage_2.TabStop = false;
                        gbImage_2.Text = "出口图";

                        gbImage_1.Anchor = gbImage1_1.Anchor;
                        gbImage_1.Location = gbImage1_1.Location;
                        gbImage_1.Font = gbImage1_1.Font;
                        gbImage_1.Name = "gbImage" + (pageCount + 1) + "_1";
                        gbImage_1.Tag = "image_enter";
                        gbImage_1.Size = gbImage1_1.Size;
                        gbImage_1.TabIndex = 0;
                        gbImage_1.TabStop = false;
                        gbImage_1.Text = "入口图";

                        PictureBox pictureBox_1 = new PictureBox();
                        pictureBox_1.Location = pictureBox1_1.Location;
                        pictureBox_1.Name = "pictureBox" + (pageCount + 1) + "_1";
                        pictureBox_1.Tag = "enter";
                        pictureBox_1.Size = pictureBox1_1.Size;
                        pictureBox_1.TabIndex = 0;
                        pictureBox_1.TabStop = false;

                        PictureBox pictureBox_2 = new PictureBox();
                        pictureBox_2.Location = pictureBox1_2.Location;
                        pictureBox_2.Name = "pictureBox" + (pageCount + 1) + "_2";
                        pictureBox_2.Tag = "exit";
                        pictureBox_2.Size = pictureBox1_2.Size;
                        pictureBox_2.TabIndex = 0;
                        pictureBox_2.TabStop = false;

                        pictureBox_1.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBox_2.SizeMode = PictureBoxSizeMode.StretchImage;
                        gbImage_1.Controls.Add(pictureBox_1);
                        gbImage_2.Controls.Add(pictureBox_2);
                    }
                }

                //副相机
                if (guard.Secondaries == null)
                    continue;
                for (int j = 0; j < guard.Secondaries.Count; j++)
                {
                    cond = new Cond();
                    cond.IP = guard.Secondaries[j].IP;
                    if (i == 0)
                    {
                        pbVideo = new PictureBox();
                        pbVideo.SizeMode = PictureBoxSizeMode.StretchImage;
                        pbVideo.Anchor = pbVideo1.Anchor;
                        pbVideo.Location = pbVideo1.Location;
                        pbVideo.Name = "pbVideo1" + "_" + (j + 1);
                        pbVideo.Size = pbVideo1.Size;
                        pbVideo.TabIndex = 1;
                        pbVideo.TabStop = false;
                        pbVideo.Image = Image.FromFile(processDir + "\\images\\nosignal.jpg");
                        pbVideo.Visible = false;
                        gbVideo1.Controls.Add(pbVideo);

                        cond.VideoHwnd = pbVideo.Handle;
                    }
                    else
                    {
                        tabPage = tabControl1.TabPages[pageCount - 1];
                        gbVideo = tabPage.Controls.Cast<Control>().FirstOrDefault(c => c.Tag.Equals("video")) as GroupBox;

                        pbVideo = new PictureBox();
                        pbVideo.SizeMode = PictureBoxSizeMode.StretchImage;
                        pbVideo.Anchor = pbVideo1.Anchor;
                        pbVideo.Location = pbVideo1.Location;
                        pbVideo.Name = "pbVideo" + (pageCount + 1) + "_" + (j + 1);
                        pbVideo.Size = pbVideo1.Size;
                        pbVideo.TabIndex = 1;
                        pbVideo.TabStop = false;
                        pbVideo.Image = Image.FromFile(processDir + "\\images\\nosignal.jpg");
                        gbVideo.Controls.Add(pbVideo);
                        pbVideo.Visible = false;
                        cond.VideoHwnd = pbVideo.Handle;
                    }

                    if (guard.IsExit)
                        extList.Add(cond);
                    else
                        entList.Add(cond);
                }
            }

            btnRaise1.Click += new System.EventHandler(this.btnRaise_Click);
            if (tabPage1.Text.Contains("出"))
            {
                btnPersonPass.Visible = false;
            }
            else
            {
                btnPersonPass.Click += new EventHandler(this.btnRaiseOther_Click);
            }

            if (entList.Count > 0 || extList.Count > 0)
            {
                mysdk = new ICEIPC(entList.ToArray(), extList.ToArray());
                //注册每个ipc事件的处理方法
                mysdk.IPCs.ForEach((l) =>
                {
                    l.PlateEnterEvent += IPC_PlateEnterEvent;
                    l.PlateExitEvent += IPC_PlateExitEvent;
                    l.DeviceOnOffLineEvent += IPC_StatusChangeEvent;
                    l.DeviceIOEvent += IPC_IOChangeEvent;
                });
                mysdk.PlateEvent = on_plate;
                mysdk.Init();
            }
        }
        #endregion

        #region 切换tab界面变化
        /// <summary>
        /// 切换tab界面变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeControlSize();
            string guardName = tabControl1.SelectedTab.Text;
            string ip = "";
            lblGuard.Text = tabControl1.SelectedTab.Text;
            txtPlateNum.Text = "";
            lblInDate.Text = "";
            lblOutDate.Text = "";
            lblStayed.Text = "";
            lblType.Text = "";
            lblSummary.Text = "应收:0已收:0待收:0优惠:0";

            Control ctl = tabControl1.TabPages[tabControl1.SelectedIndex].Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("images"));
            Control imageCtl1 = ctl.Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("image_enter"));
            Control imageCtl2 = ctl.Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("image_exit"));
            Control targetCtl1 = imageCtl1.Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("enter"));
            PictureBox picBox1 = targetCtl1 as PictureBox;
            Control targetCtl2 = imageCtl2.Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("exit"));
            PictureBox picBox2 = targetCtl2 as PictureBox;

            if (guardName.Contains("入"))
            {
                imageCtl2.Visible = false;
                picBox2.Visible = false;
                ChangeCommBtnStatus(false, btnModify, btnConfPass, btnFree, btnFreeTicket, bigCarbutton, smallCarbutton, mediumCarbutton);
            }
            else if (guardName.Contains("出"))
            {
                imageCtl2.Visible = true;
                picBox2.Visible = true;

                HandleOrder curOrder = handles.FirstOrDefault(h => h.IsExit && h.GuardNo == setting.Guards[tabControl1.SelectedIndex].No);
                if (curOrder != null)
                {
                    ChangeCommBtnStatus(!curOrder.HandledOver, btnModify, btnConfPass, btnFree, btnFreeTicket, bigCarbutton, smallCarbutton, mediumCarbutton);
                }

            }

            if (snapDict.ContainsKey(guardName))
            {
                OrderSnapshot snapshot = snapDict[guardName];
                ip = snapshot.IP;
            }
            CtlUpdate(guardName, ip);

        }
        #endregion

        #region 界面更新(加载出入图以及显示收费信息)
        /// <summary>
        /// 界面更新(加载出入图以及显示收费信息)
        /// </summary>
        /// <param name="guardName"></param>
        /// <param name="ip"></param>
        private void CtlUpdate(string guardName, string ip)
        {
            int index = FindPageIndexByIP(ip);
            int curIndex = tabControl1.SelectedIndex;
            string plateNum = "";
            string indate = "";
            string outdate = "";
            string staytime = "";
            string cartype = "";
            string chargeMsg = "";

            Control ctl = tabControl1.TabPages[curIndex].Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("images"));
            Control imageCtl1 = ctl.Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("image_enter"));
            Control imageCtl2 = ctl.Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("image_exit"));
            Control targetCtl1 = imageCtl1.Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("enter"));
            PictureBox picBox1 = targetCtl1 as PictureBox;
            Control targetCtl2 = imageCtl2.Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("exit"));
            PictureBox picBox2 = targetCtl2 as PictureBox;

            lblGuard.Text = tabControl1.SelectedTab.Text;

            if (index == curIndex)
            {
                if (snapDict.ContainsKey(guardName))
                {
                    OrderSnapshot snapshot = snapDict[guardName];
                    plateNum = snapshot.PlateNum;
                    indate = snapshot.InDate;
                    outdate = snapshot.OutDate;

                    string imgPath1 = snapshot.InFilePath + "\\" + snapshot.InFileName;
                    if (File.Exists(imgPath1))
                        picBox1.Load(imgPath1);
                    if (snapshot.IsExit)
                    {
                        string imgPath2 = snapshot.OutFilePath + "\\" + snapshot.OutFileName;
                        if (File.Exists(imgPath2))
                            picBox2.Load(imgPath2);

                        staytime = snapshot.StayTime;
                        cartype = snapshot.CarType;
                        chargeMsg = snapshot.ChargeMsg;
                    }
                    txtPlateNum.Text = plateNum;
                    lblInDate.Text = indate;
                    lblOutDate.Text = outdate;
                    lblStayed.Text = staytime;
                    lblType.Text = cartype;
                    lblSummary.Text = chargeMsg;
                    txtMoney.Text = snapshot.Money.ToString();
                }
            }

        }
        #endregion

        #region 清除界面上的抓拍的出入图片
        /// <summary>
        /// 清除界面上的抓拍的出入图片
        /// </summary>
        /// <param name="index"></param>
        private void PictureClear(int index)
        {
            Control ctl = tabControl1.TabPages[index].Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("images"));
            Control imageCtl1 = ctl.Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("image_enter"));
            Control imageCtl2 = ctl.Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("image_exit"));
            Control targetCtl1 = imageCtl1.Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("enter"));
            Control targetCtl2 = imageCtl2.Controls.Cast<Control>().FirstOrDefault(a => a.Tag.Equals("exit"));
            PictureBox picBox1 = targetCtl1 as PictureBox;
            PictureBox picBox2 = targetCtl2 as PictureBox;
            picBox1.Image = null;
            picBox2.Image = null;
        }
        #endregion

        #region 改变按钮颜色
        /// <summary>
        /// 改变按钮颜色
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="ctls"></param>
        private void ChangeBtnColor(bool enabled, Control[] ctls)
        {
            foreach (var ctl in ctls)
            {
                if (!enabled && ctl.BackColor != Color.Gray)
                {
                    ControlHelper.SetControlEnabled(enabled, ctl);
                    ctl.BackColor = Color.Gray;

                }
                else if (enabled && ctl.BackColor != normalBGColor)
                {
                    ControlHelper.SetControlEnabled(enabled, ctl);
                    ctl.BackColor = normalBGColor;
                }
            }
        }
        #endregion

        #region 改变按钮状态
        /// <summary>
        /// 改变按钮状态
        /// </summary>
        /// <param name="enabled"></param>
        /// <param name="ctls"></param>
        private void ChangeCommBtnStatus(bool enabled, params Control[] ctls)
        {
            if (ctls != null)
            {
                if (!ctls[0].InvokeRequired)
                    ChangeBtnColor(enabled, ctls);
                else
                    ctls[0].Invoke(new MethodInvoker(() =>
                    {
                        ChangeBtnColor(enabled, ctls);
                    }));
            }
        }
        #endregion

        private void _ButtonChangeGrayOrBlue(Guard guard, bool enabled) 
        {
            string guardName = tabControl1.SelectedTab.Text;
            if (guardName.Equals(((guard.IsExit ? "出口" : "入口") + "_" + guard.No)))
            {
                ChangeCommBtnStatus(enabled, btnModify, btnConfPass, btnFree, btnFreeTicket, bigCarbutton, smallCarbutton, mediumCarbutton);
            }
        }
        /// <summary>
        /// 和上面的可以重新封装成一个
        /// </summary>
        /// <param name="guard"></param>
        /// <param name="enabled"></param>
        private void ButtonChangeGrayOrBlue(Guard guard, bool enabled)
        {
            if (tabControl1.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    _ButtonChangeGrayOrBlue(guard, enabled);
                }));
            }
            else
            {
                _ButtonChangeGrayOrBlue(guard,enabled);
            }
           
        }

        #region 修改控件随着窗体大小而进行尺寸、位置变动
        /// <summary>
        /// 修改控件随着窗体大小而进行尺寸、位置变动
        /// </summary>
        private void ChangeControlSize()
        {
           
            //左侧tab
            foreach (TabPage page in tabControl1.TabPages)
            {
                int pageHeight = page.Height;
                int pageWidth = page.Width;
                int axisY = (page as Control).Location.Y;
                List<Control> subCtls = page.Controls.Cast<Control>().ToList();

                Control ctlVideo = subCtls.FirstOrDefault(a => a.Tag.Equals("video"));
                GroupBox gbVideo = ctlVideo as GroupBox;
                gbVideo.Height = (int)(pageHeight * 0.55);
                gbVideo.Width = (int)(pageWidth * 0.99);

                Control ctlImages = subCtls.FirstOrDefault(a => a.Tag.Equals("images"));
                GroupBox gbImages = ctlImages as GroupBox;
                gbImages.Location = new Point(gbVideo.Location.X, gbVideo.Height + gbVideo.Location.Y);
                gbImages.Width = (int)(pageWidth * 0.99);
                gbImages.Height = pageHeight - gbVideo.Height - 2;

                List<Control> imgCtls = gbImages.Controls.Cast<Control>().ToList();
                Control ctlImage1 = imgCtls.FirstOrDefault(a => a.Tag.Equals("image_enter"));
                GroupBox gbImgEnter = ctlImage1 as GroupBox;
                Control ctlImage2 = imgCtls.FirstOrDefault(a => a.Tag.Equals("image_exit"));
                GroupBox gbImgExit = ctlImage2 as GroupBox;
                gbImgEnter.Width = (int)(gbImages.Width * 0.498);
                gbImgExit.Width = gbImgEnter.Width;

                PictureBox picEnter = gbImgEnter.Controls[0] as PictureBox;
                picEnter.Width = gbImgEnter.Width;
                picEnter.Height = gbImgEnter.Height;

                gbImgExit.Location = new Point(gbImages.Width - gbImgExit.Width, gbImgEnter.Location.Y);
                PictureBox picExit = gbImgExit.Controls[0] as PictureBox;
                picExit.Width = gbImgExit.Width;
                picExit.Height = gbImgExit.Height;
            }
            if (this.WindowState == FormWindowState.Maximized)
            {
                maxFWidth = this.Width;
                maxFHeight = this.Height;
                double rate1 = ((double)maxFWidth / normalFWidth);
                double rate2 = ((double)maxFHeight / normalFHeight);
                rate1 = Math.Round(rate1, 2, MidpointRounding.AwayFromZero);
                rate2 = Math.Round(rate2, 2, MidpointRounding.AwayFromZero);
                if (!isMaximizied)
                {
               
                    groupBox1.Location = new Point((int)(maxFWidth - groupBox1.Width), groupBox1.Location.Y);
                    tabControl1.Size = new Size((int)(maxFWidth - groupBox1.Width), tabControl1.Size.Height);

                    //右侧groupbox
                    gbOther.Size = new Size(gbOther.Size.Width, (int)(gbOther.Size.Height * rate2*0.9));
                    gbTollTaker.Location = new Point(gbTollTaker.Location.X, gbOther.Size.Height + 0);
                    gbTollTaker.Size = new Size(gbTollTaker.Size.Width, (int)(gbTollTaker.Size.Height * rate2));

                    //方式一
                   // gbFee.Location = new Point(gbFee.Location.X, gbOther.Size.Height + 0 + gbTollTaker.Size.Height + 0);
                   // gbFee.Size = new Size(gbFee.Size.Width, (int)(gbFee.Size.Height * rate));
                   // gbPass.Location = new Point(gbPass.Location.X, gbOther.Size.Height + 0+ gbTollTaker.Size.Height + 0 + gbFee.Size.Height + 0);
                    //gbPass.Size = new Size(gbPass.Size.Width, (int)(gbPass.Size.Height * rate));

                     //方式二
                    gbPass.Size = new Size(gbPass.Size.Width, (int)(gbPass.Size.Height * rate2));
                    double lastLocaHeight=maxFHeight-gbPass.Size.Height-40;
                    gbPass.Location = new Point(gbPass.Location.X, (int)lastLocaHeight);
                    double leftTotalHeight = lastLocaHeight - gbTollTaker.Location.Y - gbTollTaker.Size.Height;
                    gbFee.Location = new Point(gbFee.Location.X, (int)leftTotalHeight);
                    double sizeHeight = maxFHeight - leftTotalHeight - gbPass.Size.Height;
                    gbFee.Size = new Size(gbFee.Size.Width, (int)(sizeHeight-40));
                }
                isMaximizied = true;
            }
            else
            {
                normalFHeight = this.Height;
            }

        }
        #endregion

        #region tab标签页绘制选中效果
        /// <summary>
        /// tab标签页绘制选中效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Color color = Color.White;
            if (tabControl1.SelectedIndex == e.Index)
                color = Color.Wheat;
            using (Brush br = new SolidBrush(color))
            {
                e.Graphics.FillRectangle(br, e.Bounds);
                SizeF sz = e.Graphics.MeasureString(tabControl1.TabPages[e.Index].Text, e.Font);
                e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + (e.Bounds.Width - sz.Width) / 2, e.Bounds.Top + (e.Bounds.Height - sz.Height) / 2 + 1);
                /*
                Rectangle rect = e.Bounds;
                rect.Offset(0, 1);
                rect.Inflate(0, -1);
                e.Graphics.DrawRectangle(Pens.DarkGray, rect);
                e.DrawFocusRectangle();
                */
            }
        }
        #endregion 

        private void FrmMain_SizeChanged(object sender, EventArgs e)
        {
            if (formEntered)
                ChangeControlSize();
            formEntered = true;
        }

        #region 检查余位并抬杆
        /// <summary>
        /// 检查余位并抬杆
        /// </summary>
        public void CheckLeftCountOpenGrate()
        {
            log.Warn("checkLeftCountOpenIPC.Count = " + checkLeftCountOpenIPC.Count.ToString());
            if (checkLeftCountOpenIPC.Count > 0)
            {
                try
                {
                    for (int i = checkLeftCountOpenIPC.Count - 1; i >= 0; i--)
                    {
                        //查询停车场余位数
                        int leftCount = psBll.getLeftCount();
                        if (leftCount > 0)
                        {
                            OrderIpc orderIpc = checkLeftCountOpenIPC[i];
                            orderIpc.ipcCaramer.OpenGate();
                            log.Warn("【抬杆命令：】自动判断余位并抬杆,Ip=" + orderIpc.ipcCaramer.IP);
                            //保存订单
                            //生成入场订单
                            OrderModel order = psBll.SaveOrder(1, orderIpc.licensePlateColor, orderIpc.licensePlateNo,
                            orderIpc.picturePath, orderIpc.pictureName, orderIpc.platePath, orderIpc.plateName,
                            orderIpc.imageBase64, DateTime.Now, orderIpc.memberId, Convert.ToInt32(netWorkOk), "");  //入场时间改成系统当前时间
                            checkLeftCountOpenIPC.RemoveAt(i); //Remove必须从后面移除，不然索引会变
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("CheckLeftCountOpenGrate方法发生异常：ErrorMsg = " + ex.Message);
                }
            }
        }
        #endregion

        #region 检查出场未识别订单并标记异常
        /// <summary>
        /// 检查出场未识别订单并标记异常
        /// </summary>
        private void CheckUnidentifiedOrder()
        {
            try
            {
                log.Info("检查出场未识别订单并标记异常");
                //查询出场未识别订单列表
                string unidentifiedOrderStr = psBll.GetUnidentifiedOrder(Params.Duty.ParkingLotId);
                if (unidentifiedOrderStr != String.Empty)
                {
                    string result = psBll.deleteOrder(unidentifiedOrderStr, 1, 0);//系统自动标记异常
                    log.Info("自动处理出场未识别订单：" + result + "订单编号：" + unidentifiedOrderStr);
                }
            }
            catch (Exception ex)
            {
                log.Error("CheckUnidentifiedOrder：ErrorMsg = " + ex.Message);
            }
        }
        #endregion

        #region 界面按钮功能
        /// <summary>
        /// 设置参数按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetting_Click(object sender, EventArgs e)
        {
            FrmPwd pwd = new FrmPwd();
            pwd.ChangeSettingDele = () =>
            {
                alwaysOpenTimer.Change(0, Params.Settings.Serv.GateKeepOpenFreq);

                if (dataHandleTimer != null)
                {
                    if (Params.Settings.Serv.DataSyncFreq > 0)
                        dataHandleTimer.Change(0, Params.Settings.Serv.DataSyncFreq);
                    else
                        dataHandleTimer.Change(Timeout.Infinite, Timeout.Infinite);
                }
                else 
                {
                     if (Params.Settings.Serv.DataSyncFreq > 0)
                         dataHandleTimer = new System.Threading.Timer((a) =>
                         {
                             System.DateTime currentTime = new System.DateTime();
                             currentTime = System.DateTime.Now;
                             int hour = currentTime.Hour;
                             int min = currentTime.Minute;
                             if (hour == Params.Settings.HourSync && min ==Params.Settings.MinSync)
                             {
                                 string result = psBll.AccessRules();
                                 log.Info("【数据同步】," + result);
                             }
                         }, null, 0, Params.Settings.Serv.DataSyncFreq);
                }

                //开启关闭断网续传线程
                if (renewalBrokenNetOrderTimer != null)
                {
                    if (Params.Settings.brokenNetOrderFre > 0)
                        renewalBrokenNetOrderTimer.Change(0, Params.Settings.brokenNetOrderFre * 1000);
                    else
                        renewalBrokenNetOrderTimer.Change(Timeout.Infinite, Timeout.Infinite);
                }
                else
                {
                    if (Params.Settings.brokenNetOrderFre > 0)
                        renewalBrokenNetOrderTimer = new System.Threading.Timer((a) =>
                        {
                            psBll.OrderRenewal(Convert.ToInt32(netWorkOk));
                        }, null, 0, Params.Settings.brokenNetOrderFre * 1000);
                }

                //开启关闭余位查询线程
                if (checkLeftCountTimer != null)
                {
                    if (Params.Settings.CheckLeftCountSec > 0)
                        checkLeftCountTimer.Change(0, Params.Settings.CheckLeftCountSec * 1000);
                    else
                        checkLeftCountTimer.Change(Timeout.Infinite, Timeout.Infinite);
                }
                else
                {
                    if (Params.Settings.CheckLeftCountSec > 0)
                        checkLeftCountTimer = new System.Threading.Timer((a) =>
                        {
                            //开启余位查询并抬杆
                            this.CheckLeftCountOpenGrate();
                        }, null, 0, Params.Settings.CheckLeftCountSec * 1000);
                }

                //开启关闭异常订单处理线程
                if (checkUnidentifiedTimer != null)
                {
                    if (Params.Settings.UnidentifiedTimerMin > 0)
                        checkUnidentifiedTimer.Change(0, Params.Settings.UnidentifiedTimerMin * 1000);
                    else
                        checkUnidentifiedTimer.Change(Timeout.Infinite, Timeout.Infinite);
                }
                else
                {
                    if (Params.Settings.UnidentifiedTimerMin > 0)
                        checkUnidentifiedTimer = new System.Threading.Timer((a) =>
                        {
                            //异常订单检查并标记
                            this.CheckUnidentifiedOrder();
                        }, null, 0, Params.Settings.UnidentifiedTimerMin * 60 * 1000);
                }

                ScreenSdk.UnInit();
                InitScreen();
                ScreenShow_In_LeftCount();

                if (checkOrderTimer == null)
                    checkOrderTimer = new System.Threading.Timer(GetOrderState, null, 0, Params.Settings.Serv.PayCheckFreq);
                else
                    checkOrderTimer.Change(0, Params.Settings.Serv.PayCheckFreq);

            };
            pwd.ShowDialog();
        }

        /// <summary>
        /// 登陆功能
        /// </summary>
        /// <param name="empName"></param>
        /// <param name="userId"></param>
        /// <param name="empId"></param>
        /// <param name="mode"></param>
        public void login(string empName, int userId, int empId, ChargeOnDutyModel mode)
        {
            Params.User.Id = userId.ToString();
            Params.User.StaffId = empId.ToString();
            Params.User.Name = empName;

            ParkSystemUtility.chargeEmp = empId;

            Params.Duty.WorkNo = mode.WorkNo;
            Params.Duty.UserName = mode.UserName;
            Params.Duty.StartWorkTime = mode.StartWorkTime;
            Params.Duty.EndWorkTime = mode.EndWorkTime;
            Params.Duty.ParkingLotId = mode.ParkingLotId;
            Params.Duty.State = mode.State;
            Params.Duty.CashAmount = mode.CashAmount;

            lblCash.Text = mode.CashAmount.ToString();
            if (empName != "无人值守")
                lblEmpName.Text = empName + "(值守)";
            else
                lblEmpName.Text = empName;
            btnChangeMode.Text = "设置无人模式";
        }

        /// <summary>
        /// 上下班按钮功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOnOffDuty_Click(object sender, EventArgs e)
        {
            if (btnOnOffDuty.Text == "下班")
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("您确定要下班吗?", "下班", messButton);
                if (dr == DialogResult.OK)//如果点击“确定”按钮
                {
                    LoginControl loginctl = new LoginControl();
                    ChargeOnDutyModel modereturn = loginctl.SaveChargeOnDuty(2, Convert.ToInt32(Params.User.Id), Params.Duty.WorkNo);
                    if (modereturn.returnResult != 1000)
                        MessageBox.Show("下班上传数据保存失败，请联系管理员！");
                    else
                    {
                        btnOnOffDuty.Text = "上班";
                        btnChangeMode.Text = "设置值守模式";
                        login("无人值守", Params.noBodyUserId, Params.noBodyEmpId, modereturn);
                        ChangeCommBtnStatus(false, btnModify, btnConfPass, btnFree, btnFreeTicket, btnChangeMode, bigCarbutton, smallCarbutton, mediumCarbutton);
                    }
                }
                else
                {
                    //e.Cancel = true;
                }
            }
            else
            {
                LoginForm loginForm = new LoginForm();
                loginForm.HandleLogin = login;
                loginForm.StartPosition = FormStartPosition.CenterScreen;
                loginForm.ShowDialog();
                int empId = Convert.ToInt32(Params.User.StaffId);
                if (empId != Params.noBodyEmpId)
                {
                    btnOnOffDuty.Text = "下班";
                    btnChangeMode.Text = "设置无人模式";
                }
                ChangeCommBtnStatus(true,  btnChangeMode);
            }
        }

        /// <summary>
        /// 交接班按钮功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHandOver_Click(object sender, EventArgs e)
        {
            int empId = Convert.ToInt32(Params.User.StaffId);
            if (empId == Params.noBodyEmpId)
            {
                MessageBox.Show("目前是无人值守，请点击上班按钮！");
                return;
            }
            SuccessionForm succession = new SuccessionForm();
            succession.HandleLogin = login;
            succession.StartPosition = FormStartPosition.CenterScreen;
            succession.ShowDialog();
        }

        /// <summary>
        /// 场内车辆查询按钮功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryPlate_Click(object sender, EventArgs e)
        {
            int empId = Convert.ToInt32(Params.User.StaffId);
            OrderqueryForm queryForm = new OrderqueryForm(empId);
            queryForm.StartPosition = FormStartPosition.CenterScreen;
            queryForm.ShowDialog();
        }

        /// <summary>
        /// 同步数据按钮功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSyncData_Click(object sender, EventArgs e)
        {
            string result = psBll.AccessRules();
            log.Info("【手动同步】," + result);
            MessageBox.Show("结果:" + result);
        }

        /// <summary>
        /// 设置广告(目前同一个停车场所有出入口一致)
        /// </summary>
        /// <param name="advertisementLine1"></param>
        public void setadvertising(byte[] advertisementLine1)
        {
            mysdk.IPCs.ForEach((l) =>
            {
                l.SendData(advertisementLine1);
            });
        }

        /// <summary>
        /// 设置广告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdvt_Click(object sender, EventArgs e)
        {
            SetadvertisingForm advertising = new SetadvertisingForm();
            advertising.StartPosition = FormStartPosition.CenterScreen;
            advertising.HandleSetAdvert = setadvertising;
            advertising.ShowDialog();
        }

        /// <summary>
        /// 切换模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeMode_Click(object sender, EventArgs e)
        {
            string flag = "";
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("您确定要" + btnChangeMode.Text + "吗?", "设置值守模式", messButton);
            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                if (btnChangeMode.Text == "设置无人模式")
                {
                    btnChangeMode.Text = "设置值守模式";
                    flag = "(无人)";
                }
                else
                {
                    btnChangeMode.Text = "设置无人模式";
                    flag = "(值守)";
                }
                if (lblEmpName.Text != "无人值守")
                    lblEmpName.Text = lblEmpName.Text.Replace("(无人)", "").Replace("(值守)", "") + flag;
            }
            
        }

        /// <summary>
        /// 修正车牌
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPlateNum.Text))
                return;


             Guard guard= setting.Guards[tabControl1.SelectedIndex];
             HandleOrder curOrder=   handles.FirstOrDefault(h => h.IsExit == guard.IsExit && h.GuardNo == guard.No);
             if (curOrder != null && !curOrder.HandledOver)
             {
                 string plateNumber = txtPlateNum.Text.Trim();
                 string result = psBll.UpdateLicensePlateNo(plateNumber, curOrder.Order.OrderNo);
                 MessageBox.Show("修改结果:" + result);

             }
        }

        /// <summary>
        /// 大型车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bigCarbutton_Click(object sender, EventArgs e)
        {
            ReCalculationMediumLarge(2);
        }

        /// <summary>
        /// 中型车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mediumCarbutton_Click(object sender, EventArgs e)
        {
            ReCalculationMediumLarge(3);
        }

        /// <summary>
        /// 小型车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void smallCarbutton_Click(object sender, EventArgs e)
        {
            ReCalculationMediumLarge(1);
        }

        /// <summary>
        /// 小中大型车计费
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReCalculationMediumLarge(int carType)
        {
            OrderModel mode = new OrderModel();
            Guard guard = setting.Guards[tabControl1.SelectedIndex];
            HandleOrder curOrder = handles.FirstOrDefault(h => h.IsExit == guard.IsExit && h.GuardNo == guard.No);
            if (curOrder != null && !curOrder.HandledOver)
            {
                string orderNo = string.Empty;
                orderNo = curOrder.Order.OrderNo;
                mode = psBll.ReCalculationMediumLarge(orderNo, 1, carType, Convert.ToInt32(netWorkOk));
            }
            if (mode.result == 1000)
            {
                string message = "应收:" + mode.OrderCharge + " 已收:" + mode.ActualGetAmount + " 待收:" + mode.PayDifferenceAmount + " 优惠:" + mode.DiscountAmount;
                curOrder.Order.ActualAmount = mode.ActualAmount;
                curOrder.Order.OrderCharge = mode.OrderCharge;
                curOrder.Order.DiscountAmount = mode.DiscountAmount;
                string guardName = (guard.IsExit ? "出口" : "入口") + "_" + guard.No;

                OrderSnapshot snapshot = snapDict[guardName];
                string ip = snapshot.IP;
                snapshot.ChargeMsg = message;
                snapDict[guardName] = snapshot;
                this.Invoke(CtlUpdateDele, guardName, ip);
                txtMoney.Text = mode.ActualAmount.ToString();
            }
            else
            {
                MessageBox.Show(mode.resultVal);
            }
           
        }
        #endregion

        #region 显示屏相关功能
         /*
          * 建议是否可进一步将多个显示函数封装一下
          */

        /// <summary>
        /// 初始化显示屏
        /// </summary>
        private void InitScreen()
        {
            foreach (Guard guard in setting.Guards)
            {
                if (guard.Primary.ScreenType == 2)
                {
                    ScreenSdk.Init(false, false);
                    ScreenSdk.Set_DisplayLines(guard.Primary.ScreenIP, guard.IsExit ? Params.Settings.OutLine : Params.Settings.InLine);
                }
                else
                {

                    if (mysdk != null)
                    {
                        byte[] voiceByte = ScreenUtil.SetVoice(guard.IsExit ? Params.Settings.OutVolume : Params.Settings.InVolume);
                        IPC ipc = mysdk.IPCs.FirstOrDefault(m => m.IP == guard.Primary.IP);
                        if (ipc != null)
                            ipc.SendData(voiceByte);
                    }

                }

                if (guard.Secondaries != null)
                {
                    guard.Secondaries.ForEach((gi) =>
                    {
                        if (gi.ScreenType == 2)
                        {
                            ScreenSdk.Init(false, false);
                            ScreenSdk.Set_DisplayLines(gi.ScreenIP, guard.IsExit ? Params.Settings.OutLine : Params.Settings.InLine);
                        }
                        else
                        {
                            if (mysdk != null)
                            {
                                byte[] voiceByte = ScreenUtil.SetVoice(guard.IsExit ? Params.Settings.OutVolume : Params.Settings.InVolume);
                                IPC ipc = mysdk.IPCs.FirstOrDefault(m => m.IP == gi.IP);
                                if (ipc != null)
                                    ipc.SendData(voiceByte);
                            }

                        }

                    });
                }

            }
        }

        /// <summary>
        /// 显示屏:入场无剩余车位
        /// </summary>
        /// <param name="ipc"></param>
        private void ScreenShow_In_NoleftCount(IPC ipc)
        {
            GuardItem guardItem = FindGuardItemByIP(ipc.IP);
            if (guardItem.ScreenType == 2)
            {
                ScreenSdk.Send_To_Show(guardItem.ScreenIP, 1, "车位已满", 2);
                ScreenSdk.Send_To_Voice(guardItem.ScreenIP, "车位已满");
            }
            else
            {
                IPC ipcshow = mysdk.IPCs.First(a => a.IP == guardItem.IP);
                byte[] data = ScreenUtil.ScreenByteValue("车位已满", 2, Params.Settings.ScreenInDelay, 1);
                ipcshow.SendData(data);
            }
        }

        /// <summary>
        /// 显示屏:车辆入场
        /// </summary>
        /// <param name="ipc">相机类</param>
        /// <param name="plateNumber">车牌号</param>
        private void ScreenShow_In(IPC ipc, string plateNumber)
        {
            GuardItem guardItem = FindGuardItemByIP(ipc.IP);
            if (guardItem.ScreenType == 2)
            {
                if (Params.Settings.IsWelcome)  //根据参数配置是否显示欢迎光临
                {
                    ScreenSdk.Send_To_Show(guardItem.ScreenIP, 1, plateNumber, 2);
                    ScreenSdk.Send_To_Show(guardItem.ScreenIP, 2, "欢迎光临", 2); 
                    ScreenSdk.Send_To_Voice(guardItem.ScreenIP, plateNumber + "欢迎光临");   
                }
                else
                {
                    ScreenSdk.Send_To_Show(guardItem.ScreenIP, 1, plateNumber, 2);
                    ScreenSdk.Send_To_Voice(guardItem.ScreenIP, plateNumber);     
                }
            }
            else
            {
                IPC ipcshow = mysdk.IPCs.First(a => a.IP == guardItem.IP);
                if (Params.Settings.IsWelcome) //根据参数配置是否显示欢迎光临
                {
                    byte[] welcome = ScreenUtil.ScreenByteValue(plateNumber + " 欢迎光临", 1, Params.Settings.ScreenInDelay, 1);  
                    byte[] broadcast = ScreenUtil.BroadcastValue(plateNumber, null, new byte[] { 1 });//1=欢迎光临     
                    ipcshow.SendData(welcome);
                    ipcshow.SendData(broadcast);
                }
                else
                {
                    byte[] welcome = ScreenUtil.ScreenByteValue(plateNumber, 1, Params.Settings.ScreenInDelay, 1);  
                    byte[] broadcast = ScreenUtil.BroadcastValue(plateNumber, null, null);
                    ipcshow.SendData(welcome);
                    ipcshow.SendData(broadcast);
                }
                /*byte[] welcome = ScreenUtil.ScreenByteValue(plateNumber + " 欢迎光临", 1, Params.Settings.ScreenInDelay, 1);  //Modified by Zhiwen_Tian  2019-08-05 10:16
                byte[] broadcast = ScreenUtil.BroadcastValue(plateNumber, null, new byte[] { 1 });//1=欢迎光临                //Modified by Zhiwen_Tian  2019-08-05 10:16
                byte[] welcome = ScreenUtil.ScreenByteValue(plateNumber, 1, Params.Settings.ScreenInDelay, 1);  //Modified by Zhiwen_Tian  2019-08-05 10:16
                byte[] broadcast = ScreenUtil.BroadcastValue(plateNumber, null, null);
                ipcshow.SendData(welcome);
                ipcshow.SendData(broadcast);*/
            }
        }

        /// <summary>
        /// 显示屏：一位多车禁止进入
        /// </summary>
        /// <param name="ipc">相机类</param>
        /// <param name="plateNumber">车牌号</param>
        public void ScreenShow_NoOccupy(IPC ipc, string plateNumber)
        {
            GuardItem guardItem = FindGuardItemByIP(ipc.IP);
            if (guardItem.ScreenType == 2)
            {
                ScreenSdk.Send_To_Show(guardItem.ScreenIP, 1, "此车位已有车辆停入", 2);
                ScreenSdk.Send_To_Voice(guardItem.ScreenIP, "此车位已有车辆停入");
            }
            else
            {
                IPC ipcshow = mysdk.IPCs.First(a => a.IP == guardItem.IP);
                byte[] data = ScreenUtil.ScreenByteValue("此车位已有车辆停入", 2, Params.Settings.ScreenInDelay, 1);
                ipcshow.SendData(data);
            }
        }

        /// <summary>
        /// 显示屏：外来车辆禁止通行
        /// </summary>
        /// <param name="ipc">相机类</param>
        /// <param name="plateNumber">车牌号</param>
        private void ScreenShow_NoIn(IPC ipc, string plateNumber)
        {
            GuardItem guardItem = FindGuardItemByIP(ipc.IP);
            if (guardItem.ScreenType == 2)
            {
                ScreenSdk.Send_To_Show(guardItem.ScreenIP, 1, plateNumber, 2);
                ScreenSdk.Send_To_Show(guardItem.ScreenIP, 2, "外来车辆禁止通行", 2);
                ScreenSdk.Send_To_Voice(guardItem.ScreenIP, plateNumber + "外来车辆禁止通行");
            }
            else
            {
                IPC ipcshow = mysdk.IPCs.First(a => a.IP == guardItem.IP);
                byte[] welcome = ScreenUtil.ScreenByteValue(plateNumber + " 外来车辆禁止通行", 1, Params.Settings.ScreenInDelay, 1);
               // byte[] broadcast = ScreenUtil.BroadcastValue(plateNumber, null, new byte[] { 1 });//1=欢迎光临
                ipcshow.SendData(welcome);
               // ipcshow.SendData(broadcast);
            }
        }

        /// <summary>
        /// 显示屏:停车场余位数
        /// </summary>
        /// <param name="pcNumber"></param>
        private void ScreenShow_In_LeftCount()
        {
            if (Params.Settings.EnabledShowLeftCount)
            {
                int leftCount = psBll.getLeftCount();
                foreach (var guard in setting.Guards)
                {
                    if (!guard.IsExit)
                    {
                        string ip = guard.Primary.IP;
                        if (guard.Primary.ScreenType == 2)
                        {
                            ScreenSdk.Send_To_Show(ip, 2, "余位数：" + leftCount, 2);
                        }
                        else
                        {
                          
                                IPC ipcshow = mysdk.IPCs.First(a => a.IP == guard.Primary.IP);
                                //获取配置的行数
                                int line = Params.Settings.InLine;
                                byte[] welcome = ScreenUtil.SetRemainingPosition(line, 1, 2, 0, leftCount);
                                ipcshow.SendData(welcome);
                            
                           
                        }
                    }
                }
            }
            else
            {
                ScreenUtil.SetRemainingPosition(0, 1, 2, 0, 1);
            }
          
           
           
        }

        /// <summary>
        /// 显示屏:时间
        /// </summary>
        /// <param name="pcTime"></param>
        private void ScreenShow_Time()
        {
            //int leftCount = psBll.getLeftCount();
            foreach (var guard in setting.Guards)
            {
                    //string ip = guard.Primary.IP;
                    if (guard.Primary.ScreenType == 1)
                    {
                        IPC ipcshow = mysdk.IPCs.First(a => a.IP == guard.Primary.IP);
                        DateTime t = DateTime.Now;
                        //DateTime t = Convert.ToDateTime("2019-07-01 12:30:44");
                        byte[] welcome = ScreenUtil.SetTime(t);
                        ipcshow.SendData(welcome);
                        byte[] showTimeMode = ScreenUtil.SetTimeShowModel(1,1,0);
                        ipcshow.SendData(showTimeMode);
                }
            }
        }

        /// <summary>
        /// 显示屏:车辆出场异常（识别错误/无入场订单）
        /// </summary>
        /// <param name="pcNumber"></param>
        private void ScreenShow_OutError(IPC ipc, string plateNumber)
        {
            GuardItem guardItem = FindGuardItemByIP(ipc.IP);
            if (guardItem.ScreenType == 2)
            {
                ScreenSdk.Send_To_Show(guardItem.ScreenIP, 1, plateNumber, 2);
                ScreenSdk.Send_To_Show(guardItem.ScreenIP, 2, "车牌识别有误，请等待收费员矫正!", 2);
                ScreenSdk.Send_To_Voice(guardItem.ScreenIP, plateNumber + "车牌识别有误，请等待收费员矫正！");
            }
            else
            {
                IPC ipcshow = mysdk.IPCs.First(a => a.IP == guardItem.IP);
                byte[] plateShow = ScreenUtil.ScreenByteValue(plateNumber, 1, Params.Settings.ScreenOutDelay, 1);
                byte[] outShow = ScreenUtil.ScreenByteValue("车牌识别有误，请等待收费员矫正！", 2, Params.Settings.ScreenOutDelay, 2);
                ipcshow.SendData(plateShow);
                ipcshow.SendData(outShow);
            }
        }

        /// <summary>
        /// 显示屏:出场缴费
        /// </summary>
        /// <param name="pcNumber"></param>
        /// <param name="charge"></param>
        /// <param name="carType"></param>
        private void ScreenShow_Charge(IPC ipc, string pcNumber, double charge, string carType)
        {
            GuardItem guardItem = FindGuardItemByIP(ipc.IP);
            if (guardItem.ScreenType == 2)
            {
                ScreenSdk.Send_To_Show(guardItem.ScreenIP, 1, carType + " " + pcNumber, 2);
                ScreenSdk.Send_To_Show(guardItem.ScreenIP, 2, "请缴费" + charge.ToString() + "元", 2);
                ScreenSdk.Send_To_Voice(guardItem.ScreenIP, "请缴费" + charge.ToString() + "元");
            }
            else
            {
                IPC ipcshow = mysdk.IPCs.First(a => a.IP == guardItem.IP);
                byte[] plateShow = ScreenUtil.ScreenByteValue(pcNumber, 1, Params.Settings.ScreenOutDelay, 1);
                byte[] chargeShow = ScreenUtil.ScreenByteValue("请缴费：" + charge.ToString() + "元", 2, Params.Settings.ScreenOutDelay, 2);
                byte[] broadcast = ScreenUtil.BroadcastNum(1, Convert.ToInt32(charge * 10));
                ipcshow.SendData(plateShow);
                ipcshow.SendData(chargeShow);
                ipcshow.SendData(broadcast);
            }
        }

        /// <summary>
        /// 显示屏:车辆出场
        /// </summary>
        /// <param name="ipc"></param>
        /// <param name="plateNumber"></param>
        /// <param name="carType"></param>
        private void ScreenShow_Out(IPC ipc, string plateNumber, string carType)
        {
            GuardItem guardItem = FindGuardItemByIP(ipc.IP);
            if (guardItem.ScreenType == 2)
            {
                ScreenSdk.Send_To_Show(guardItem.ScreenIP, 1, carType + " " + plateNumber, 2);
                ScreenSdk.Send_To_Show(guardItem.ScreenIP, 2, "一路顺风", 2);
                ScreenSdk.Send_To_Voice(guardItem.ScreenIP, carType + plateNumber + "一路顺风");
            }
            else
            {
                IPC ipcshow = mysdk.IPCs.First(a => a.IP == guardItem.IP);
                //ipc.IP = guardItem.IP;
                byte[] plateShow = ScreenUtil.ScreenByteValue(plateNumber, 1, Params.Settings.ScreenOutDelay, 1);
                byte[] outShow = ScreenUtil.ScreenByteValue("一路顺风", 2, Params.Settings.ScreenOutDelay, 2);
                byte[] outBroadcast = ScreenUtil.BroadcastValue(plateNumber, null, new byte[] { 95 }); //95代表一路顺风
                ipcshow.SendData(plateShow);
                ipcshow.SendData(outShow);
                ipcshow.SendData(outBroadcast);
            }
        }
        #endregion

        #region Form控件事件相关
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            this.Text = string.Format("停车场收费客户端v1.0_{0}({1})", setting.ParkLot.Name, setting.ParkLot.No);

            lblEmpName.ForeColor = Color.Green;
            lblEmpName.Text = Params.User.Name;

            lblCash.ForeColor = Color.Red;
            lblCash.Text = "0";

            lblGuard.ForeColor = Color.Orange;
            lblOperator.Text = "";
            lblFee.Text = "0";
            tabPage1.Text = "入口_未命名";
            lblFreeTitle.ForeColor = ColorTranslator.FromHtml("#5950EB");
            txtMoney.Text = "0";

            foreach (Control ctl in tableLayoutPanel1.Controls)
            {
                foreach (Control subCtl in ctl.Controls)
                {
                    if (subCtl is System.Windows.Forms.Button)
                    {
                        subCtl.BackColor = normalBGColor;
                        subCtl.ForeColor = Color.White;
                    }
                }
            }
            foreach (Control ctl in tableLayoutPanel4.Controls)
            {
                foreach (Control subCtl in ctl.Controls)
                {
                    if (subCtl is System.Windows.Forms.Button)
                    {
                        subCtl.BackColor = normalBGColor;
                        subCtl.ForeColor = Color.White;
                    }
                }
            }
            btnModify.BackColor = normalBGColor;
            btnModify.ForeColor = Color.White;
            btnRaise1.BackColor = normalBGColor;
            btnRaise1.ForeColor = Color.White;
            btnSetting.BackColor = Color.Green;
            btnSetting.ForeColor = Color.White;
            btnAdvt.BackColor = Color.Purple;
            btnAdvt.ForeColor = Color.White;
            btnSyncData.BackColor = Color.Orange;
            btnSyncData.ForeColor = Color.White;
            bigCarbutton.ForeColor = Color.White;
            bigCarbutton.BackColor = normalBGColor;
            smallCarbutton.ForeColor = Color.White;
            smallCarbutton.BackColor = normalBGColor;
            mediumCarbutton.ForeColor = Color.White;
            mediumCarbutton.BackColor = normalBGColor;
            btnPersonPass.ForeColor = Color.White;
            btnPersonPass.BackColor = normalBGColor;
    
            gbVideo1.Tag = "video";
            pbVideo1.Image = Image.FromFile(processDir + "\\images\\nosignal.jpg");
            gbImage1.Tag = "images";
            gbImage1_1.Tag = "image_enter";
            gbImage1_2.Tag = "image_exit";
            pictureBox1_1.Tag = "enter";
            pictureBox1_2.Tag = "exit";
            pictureBox1_1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1_2.SizeMode = PictureBoxSizeMode.StretchImage;

            ChangeCommBtnStatus(false, btnModify, btnConfPass, btnFree, btnFreeTicket, btnChangeMode, bigCarbutton, smallCarbutton,mediumCarbutton);
           
        }

        #region   页面屏蔽回车和空格事件
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == Keys.Space)
            {
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        /// <summary>
        /// Form加载时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Load(object sender, EventArgs e)
        {
            CtlUpdateDele = new ControlUpdate(CtlUpdate);

            Params.User.Id = Params.noBodyUserId.ToString();
            Params.User.Name = "无人值守";
            Params.User.StaffId = Params.noBodyEmpId.ToString();
            Params.Settings = setting;

            InitControl();
            LoadCameras();

            heartBeatTimer.Tick += heartBeatTimer_Tick;
            heartBeatTimer.Enabled = true;

            alwaysOpenTimer = new System.Threading.Timer((a) =>
           {
                KeepOpenGate();

            }, null, 0, Params.Settings.Serv.GateKeepOpenFreq);

            if (Params.Settings.Serv.DataSyncFreq > 0)
            {
                dataHandleTimer = new System.Threading.Timer((a) =>
                {
                    System.DateTime currentTime = new System.DateTime();
                    currentTime = System.DateTime.Now;
                    int hour = currentTime.Hour;
                    int min = currentTime.Minute;
                    if (hour == Params.Settings.HourSync && min == Params.Settings.MinSync)
                    {
                        string result = psBll.AccessRules();
                        log.Info("【数据同步】," + result);
                    }
                }, null, 0, Params.Settings.Serv.DataSyncFreq);
            }
            //判断并开启断网续传线程
            if (Params.Settings.brokenNetOrderFre > 0)
            {
                renewalBrokenNetOrderTimer = new System.Threading.Timer((a) =>
                {
                    psBll.OrderRenewal(Convert.ToInt32(netWorkOk));

                }, null, 0, Params.Settings.brokenNetOrderFre * 1000);
            }

            //判断并开启余位查询线程
            if (Params.Settings.CheckLeftCountSec > 0)
            {
                checkLeftCountTimer = new System.Threading.Timer((a) =>
                {
                    //开启余位查询并抬杆
                    this.CheckLeftCountOpenGrate();
                }, null, 0, Params.Settings.CheckLeftCountSec * 1000);
            }

            //判断并开启出场未识别订单处理线程
            if (Params.Settings.UnidentifiedTimerMin > 0)
            {
                checkUnidentifiedTimer = new System.Threading.Timer((a) =>
                {
                    ///异常订单检查并标记
                    this.CheckUnidentifiedOrder();
                }, null, 0, Params.Settings.UnidentifiedTimerMin * 60 * 1000);
            }

            InitScreen();

            normalFHeight = this.Height;
            normalFWidth = this.Width;

            BootOnStart();

            this.WindowState = FormWindowState.Maximized; //窗体最大化

            //如果有出口的话，默认选中第一个出口
            int i = 0;
            int index = 0;
            foreach (TabPage page in tabControl1.TabPages)
            {
                if (page.Text.Contains("出口"))
                {
                    index = i;
                    break;
                }
                i++;
            }
            tabControl1.SelectTab(index);
            if (tabControl1.SelectedTab.Text.Contains("入"))
            {
                gbImage1_2.Visible = false;
                pictureBox1_2.Visible = false;
            }
            lblGuard.Text = tabControl1.SelectedTab.Text;

            //if (index ==0) 
            //{
                ChangeControlSize();
            //}
            //余位
            ScreenShow_In_LeftCount();
            ScreenShow_Time();


        }

        /// <summary>
        /// 特殊出场的快捷键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            //按回车或者空格键不起作用，主要针对免费放行/收费放行/免费券放行
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                return;
            }
            switch (e.KeyCode)
            {
                case Keys.F1:
                    if (btnConfPass.BackColor != Color.Gray)
                        btnPass_Click(btnConfPass, EventArgs.Empty);
                    break;
                case Keys.F10:
                    if (btnFree.BackColor != Color.Gray)
                        btnPass_Click(btnFree, EventArgs.Empty);
                    break;
                case Keys.F11:
                    if (btnFreeTicket.BackColor != Color.Gray)
                        btnPass_Click(btnFreeTicket, EventArgs.Empty);
                    break;
                
            }
        }

        /// <summary>
        /// 退出程序释放资源
        /// </summary>
        private void DisposeRes()
        {
            if (alwaysOpenTimer != null)
            {
                alwaysOpenTimer.Change(Timeout.Infinite, Timeout.Infinite);
                alwaysOpenTimer.Dispose();
                alwaysOpenTimer = null;
            }
            if (dataHandleTimer != null)
            {
                dataHandleTimer.Change(Timeout.Infinite, Timeout.Infinite);
                dataHandleTimer.Dispose();
                dataHandleTimer = null;
            }
            if (checkOrderTimer != null)
            {
                checkOrderTimer.Change(Timeout.Infinite, Timeout.Infinite);
                checkOrderTimer.Dispose();
                checkOrderTimer = null;
            }
            if (renewalBrokenNetOrderTimer != null)
            {
                renewalBrokenNetOrderTimer.Change(Timeout.Infinite, Timeout.Infinite);
                renewalBrokenNetOrderTimer.Dispose();
                renewalBrokenNetOrderTimer = null;
            }
            if (checkLeftCountTimer != null)
            {
                checkLeftCountTimer.Change(Timeout.Infinite, Timeout.Infinite);
                checkLeftCountTimer.Dispose();
                checkLeftCountTimer = null;
            }
            if (checkUnidentifiedTimer != null)
            {
                checkUnidentifiedTimer.Change(Timeout.Infinite, Timeout.Infinite);
                checkUnidentifiedTimer.Dispose();
                checkUnidentifiedTimer = null;
            }
            heartBeatTimer.Enabled = false;
            if (mysdk != null)
                mysdk.Fini();
        }

        /// <summary>
        /// Form关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("您确定要登出系统吗?\n\r 该操作一般是为了重新启动软件使用。\n\r 如果只是想切换用户继续工作。建议使用下面的【交接班】按钮。\n\r 您还要继续此操作吗？", "退出系统", messButton);
            if (dr == DialogResult.OK)//如果点击“确定”按钮
            {
                //记录关闭日志
                log.Info("关闭系统,姓名:" + lblEmpName.Text);
                DisposeRes();
            }
            else
            {
                e.Cancel = true;
            }

        }
        #endregion

        #region 主要业务功能
        /// <summary>
        /// 保存出入场订单信息快照
        /// </summary>
        /// <param name="guard"></param>
        /// <param name="plateNumber"></param>
        /// <param name="order"></param>
        /// <param name="memberId"></param>
        private void AddOrderSnapshot(string ip, Guard guard, string plateNumber, OrderModel order, int memberId, string inImgPath, string inImgName, string outImgPath, string outImgName)
        {
            string guardName = (guard.IsExit ? "出口" : "入口") + "_" + guard.No;
            OrderSnapshot snapshot = null;
            if (snapDict.ContainsKey(guardName))
            {
                snapshot = snapDict[guardName];
            }
            else
            {
                snapshot = new OrderSnapshot();
                snapDict.Add(guardName, snapshot);
            }
            snapshot.PlateNum = plateNumber;
            snapshot.IP = ip;
            if (!guard.IsExit)
            {
                snapshot.IsExit = false;
                snapshot.InDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                snapshot.InFileName = inImgName;
                snapshot.InFilePath = inImgPath;
                snapshot.Money = 0;
            }
            else
            {
                snapshot.IsExit = true;
                snapshot.InDate = order.InDate.ToString("yyyy-MM-dd HH:mm:ss");
                snapshot.OutDate = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                snapshot.CarType = GetCarType(order.ChargeType, memberId);

                snapshot.OutFileName = outImgName;
                snapshot.OutFilePath = outImgPath;

                double RD = Math.Floor(Convert.ToDouble(order.ParkingTime / (60 * 24)));//天
                double HD = Math.Floor((Convert.ToDouble(order.ParkingTime % (60 * 24)) / 60));//小时
                double MD = order.ParkingTime - RD * 60 * 24 - HD * 60;//分钟
                string parkingTimeText = "";
                if (RD > 0)
                    parkingTimeText = parkingTimeText + RD + "天";
                if (HD > 0)
                    parkingTimeText = parkingTimeText + HD + "小时";
                if (MD > 0)
                    parkingTimeText = parkingTimeText + MD + "分钟";

                string message = "应收:" + order.OrderCharge + " 已收:" + order.ActualGetAmount + " 待收:" + order.PayDifferenceAmount + " 优惠:" + order.DiscountAmount;

                snapshot.Money = order.PayDifferenceAmount;
                snapshot.StayTime = parkingTimeText == "" ? "0分钟" : parkingTimeText;
                snapshot.ChargeMsg = message;

            }
            snapDict[guardName] = snapshot;
        }
        
        /// <summary>
        /// 根据ip查询出入口Guard索引号
        /// </summary>
        /// <param name="ip"></param>
        private int FindPageIndexByIP(string ip)
        {
            int index = -1;
            Guard guard = FindGuardByIP(ip);
            if (guard != null)
            {
                if (tabControl1.InvokeRequired)
                {
                    tabControl1.Invoke(new MethodInvoker(() =>
                    {
                        index = FindPageIndexByIP(ip);
                    }));
                }
                else
                {
                    for (int i = 0; i < tabControl1.TabPages.Count; i++)
                    {
                        TabPage page = tabControl1.TabPages[i];
                        string name = page.Text;
                        string[] tmp = name.Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                        if (tmp[0] == (guard.IsExit ? "出口" : "入口") && tmp[1] == guard.No)
                        {
                            index = i;
                            break;
                        }
                    }

                }
            }
            return index;
        }

        /// <summary>
        /// 根据摄像头ip查询出入口的子项(摄像机管控的屏幕数据)
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private GuardItem FindGuardItemByIP(string ip)
        {
            GuardItem targetItem = null;
            foreach (var guard in setting.Guards)
            {
                if (guard.Primary.IP == ip)
                {
                    targetItem = guard.Primary;
                    break;
                }
                if (guard.Secondaries != null)
                {
                    //for (int i = 0; i < guard.Secondaries.Count;i++ )
                    //{
                    //    if(guard.Secondaries[i].IP==ip)
                    //        targetItem = guard.Primary;
                    //    break;
                    //}

                    targetItem = guard.Secondaries.FirstOrDefault(g => g.IP == ip);
                    if (targetItem != null)
                    {
                        targetItem = guard.Primary;
                        break;
                    }
                       
                }
            }
            //log.Info(targetItem.IP);
            return targetItem;
        }
        
        /// <summary>
        /// 根据摄像头ip查询出入口Guard
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private Guard FindGuardByIP(string ip)
        {
            Guard targetGuard = null;
            foreach (var guard in setting.Guards)
            {
                if (guard.Primary.IP == ip)
                {
                    targetGuard = guard;
                    break;
                }
                if (guard.Secondaries != null && guard.Secondaries.Count(s => s.IP == ip) > 0)
                {
                    targetGuard = guard;
                    break;
                }
            }
            return targetGuard;
        }

        /// <summary>
        /// 车牌识别进行过滤处理(出入识别最先执行的处理逻辑)
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="args"></param>
        /// <param name="isExit"></param>
        /// <param name="curOrder"></param>
        /// <param name="guard"></param>
        /// <returns></returns>
        private bool PlateFilter(IPC ipc, PlateEventArgs args, ref HandleOrder curOrder, ref Guard guard)
        {
            bool canContinue = false;
            string errorMsg = "";
            string ip = ipc.IP;
            string plateNum = args.PlateNum;

            //出入口多个相机处理逻辑
            guard = FindGuardByIP(ip);
            string guardNo = guard.No;
            bool isExit = guard.IsExit;
            string opstr = isExit ? "出场" : "入场";
            curOrder = handles.FirstOrDefault(h => h.IsExit == isExit && h.GuardNo == guardNo);
            if (curOrder != null)
            {
                int delay = isExit ? Params.Settings.OutDelay : Params.Settings.InDelay;

                /*
                 *2019-04-16 入场修改:车牌最优先原则
                 *拍摄到不同的车牌(无论是失败错误还是的确是另一辆车)，不管俩摄像头间隔时间
                 *只有相同车牌，判断俩摄像头间隔时间
                 */

                //***出场***
                //判断刚才车子已支付钱但未真正出场，又被拍到情况
                if (isExit)
                {
                    if (curOrder.PlateNumber.Equals(plateNum))
                    {
                        if (DateTime.Now.Subtract(curOrder.CaptureTime).TotalMilliseconds <= delay)
                        {
                            errorMsg = "【多相机并发】,车牌号:" + plateNum + "被" + ip + "识别,同一时刻被" + curOrder.IP + "处理其他车或者本车的" + opstr + "业务逻辑";
                            Console.WriteLine(errorMsg);
                            log.Debug(errorMsg);
                            return canContinue;
                        }
                    }
                    //if (DateTime.Now.Subtract(curOrder.CaptureTime).TotalMilliseconds <= delay)
                    //{
                    //    errorMsg = "【多相机并发】,车牌号:" + plateNum + "被" + ip + "识别,同一时刻被" + curOrder.IP + "处理其他车或者本车的" + opstr + "业务逻辑";
                    //    Console.WriteLine(errorMsg);
                    //    log.Debug(errorMsg);
                    //    return canContinue;
                    //}

                   // log.Debug(curOrder.HandledOver + ",设置的时间：" + Params.Settings.MistakenOutSec + ",车辆时间：" + args.CaptureTime.Subtract(curOrder.HandledOverTime).TotalMilliseconds);
                    ////***如果第二辆车恰好错误识别成前一辆车可能导致不触发正常逻辑***
                    if (curOrder.HandledOver && curOrder.PlateNumber.Equals(plateNum))
                    {
                        //log.Debug("车牌号:" + plateNum + ",结束订单又被抓拍" );
                        //在一定时间内未真正驶离
                        if (args.CaptureTime.Subtract(curOrder.HandledOverTime).TotalMilliseconds <= Params.Settings.MistakenOutSec)
                        {
                            errorMsg = "车牌号:" + plateNum + "已出场？但又被抓拍到";
                            Console.WriteLine(errorMsg);
                            log.Debug(errorMsg + ",设置的时间：" + Params.Settings.MistakenOutSec + ",车辆时间：" + args.CaptureTime.ToString() + ",车辆驶离间隔（毫秒）：" + args.CaptureTime.Subtract(curOrder.HandledOverTime).TotalMilliseconds);
                            return canContinue;
                        }
                    }
                }
                else
                {//入场
                    if (curOrder.PlateNumber.Equals(plateNum))
                    {
                        if (DateTime.Now.Subtract(curOrder.CaptureTime).TotalMilliseconds <= delay)
                        {
                            errorMsg = "【多相机并发】,车牌号:" + plateNum + "被" + ip + "识别,同一时刻被" + curOrder.IP + "处理其他车或者本车的" + opstr + "业务逻辑";
                            Console.WriteLine(errorMsg);
                            log.Debug(errorMsg);
                            return canContinue;
                        }
                    }


                }
            }
            else
            {
                curOrder = new HandleOrder();
                handles.Add(curOrder);
            }
            curOrder.PlateNumber = args.PlateNum;
            curOrder.GuardNo = guard.No;
            curOrder.IsExit = isExit;
            curOrder.CaptureTime = args.CaptureTime;
            curOrder.IP = ip;
            curOrder.HandledOver = false;
            curOrder.HandledOverTime = DateTime.MinValue;
            curOrder.HandleCount = 0;

            //1.误触发:在出入口倒车被识别
            VehicleDir direction = args.Direction;
            if (direction == VehicleDir.ICE_VD_VERHICLE_DIR_TAIL)
            {
                errorMsg = "【倒车误触发】,车牌号:" + plateNum + opstr + "被抓拍到存在车尾";
                Console.WriteLine(errorMsg);
                log.Warn(errorMsg);
                return canContinue;
            }

            //bool enterLimited = false;
            if (!isExit)  //入场判断
            {
                /*int isWhiteList = 0;
                if (Params.Settings.EnabledWLGO)
                {
                     isWhiteList = psBll.CheckWhilteList(args.PlateNum);
                }
                //判断是否有入场数量限制
                chkLimit.Invoke(new MethodInvoker(() => {
                    if (chkLimit.Checked  && isWhiteList==0)
                    {
                        int leftCount = psBll.getLeftCount();
                        if (leftCount < 1)
                        {
                            enterLimited = true;
                            ScreenShow_In_NoleftCount(ipc);//显示屏显示限制信息

                            errorMsg = "【入场限制】,车牌号:" + args.PlateNum + "无法进入满员场地";
                            Console.WriteLine(errorMsg);
                            log.Warn(errorMsg);
                        }
                    }

                }));

                if (enterLimited)
                    return canContinue;*/

                //判断是否无牌车
                if (string.IsNullOrEmpty(args.PlateNum) || args.PlateNum.Contains("无牌")) 
                {
                    log.Debug("无牌车还是进入了识别事件里");
                    return canContinue;
                }

                //该车牌上一笔的订单还未完成(存在：套牌车进入停车、上次订单车牌识别有误而此次进入的正是被识别错误的车牌)
                List<OrderModel> orders = psBll.GetOrderList(args.PlateNum);
                if (orders != null && orders.Count > 0)
                {
                    OrderModel firstOrder = orders.OrderByDescending(o => o.InDate).ToList().FirstOrDefault();
                    switch (firstOrder.State)
                    {
                        //逻辑上还在场内(推送异常信息给运维人员)
                        case 10://订单进行中
                        case 15://支付未驶离
                        case 20: //欠费驶离(目前GetOrderList调用的存储过程无法获取除10、15以外的情况)
                        case 40://异常
                            log.Warn("【入场异常】,车牌号:" + plateNum + ",上一笔订单状态码:" + firstOrder.State);
                            break;
                        default:
                            break;
                    }
                }

                //判断没有余位不抬杆，有余位后才抬杆
                /*if (Params.Settings.CheckLeftCountSec > 0)
                {
                    int leftCount = psBll.getLeftCount();
                    if (leftCount < 1)
                    {
                        canContinue = false;
                        ScreenShow_In_NoleftCount(ipc);//显示屏显示限制信息
                        log.Warn("【入场限制-没有余位不抬杆】,车牌号:" + args.PlateNum + "无法进入满员场地");
                        //纪录需要抬杆的相机
                        if (!checkLeftCountOpenIPC.Contains(ipc))
                            checkLeftCountOpenIPC.Add(ipc);
                    }
                }*/

            }

            canContinue = true;
            return canContinue;
        }
        /// <summary>
        /// 识别入场
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void IPC_PlateEnterEvent(object sender, PlateEventArgs args)
        {
            IPC ipc = (IPC)sender;
            //如果是白牌车直接开道闸
            if (args.PlateColor.GetDisplayName().Equals("白色"))
            {
                ipc.OpenGate();//打开道闸
                log.Debug("【抬杆命令：】白牌车入口识别抬杆，车牌号：" + args.PlateNum);
                return;
            }

            //只允许白名单车辆进入
            if (Params.Settings.EnabledWhiteListUsed)
            {
                int isWhiteList = psBll.CheckWhilteList(args.PlateNum);
                if (isWhiteList == 0)
                {
                    ScreenShow_NoIn(ipc, args.PlateNum);
                    return;
                }
                if (Params.Settings.EnabledWhiteListNoOrder)
                {
                    //白名单车辆不产生订单
                    ScreenShow_In(ipc, args.PlateNum);
                    ipc.OpenGate();//打开道闸
                    log.Debug("【抬杆命令：】白名单入口口识别抬杆，不产生订单");
                    return;
                }
            }
            
            if (!Params.Settings.EnableOneSpaceMoreCars)
            {
                //不允许一位多车
                int leftBerthNum = psBll.CheckBerthNoOccupy(args.PlateNum);
                if (leftBerthNum==0)
                {
                    ScreenShow_NoOccupy(ipc, args.PlateNum);
                    return;
                }
            }

            //只允许包月车辆进入
            if (Params.Settings.EnabledMonthlyPass)
            {
                int isMontyly = psBll.CheckMonthly(args.PlateNum);
                if (isMontyly == 0)
                {
                    ScreenShow_NoIn(ipc, args.PlateNum);
                    return;
                }
            }

            log.Debug("ip:" + ipc.IP + ",plate:" + args.PlateNum + "开始进行入场逻辑");
            try
            {
                HandleOrder curOrder = null;
                Guard guard = null;
                string sightName = "";//抓拍的全景图
                string sightPath = "";
                string plateName = "";//抓拍的车牌局部图
                string platePath = "";
                string imagebase64 = "";//全景图base64

                bool canContinue = PlateFilter(ipc, args, ref curOrder, ref guard);
                if (!canContinue)
                    return;
                string guardName = (guard.IsExit ? "出口" : "入口") + "_" + guard.No;

                //识别
                SaveCapPicture(args, ipc.IP, ref imagebase64, ref sightPath, ref sightName, ref platePath, ref plateName);//抓拍
               
                ScreenShow_In(ipc, args.PlateNum);//显示屏显示信息
                //白名单启用--只允许黄牌车和特殊车牌进入--尚湖用
                if (Params.Settings.EnabledWhiteList)
                {
                    //判断是不是蓝色，蓝色判断是不是包月车

                    if (args.PlateColor.GetDisplayName().Equals("蓝色"))
                    {
                        int isWhiteList = psBll.CheckWhilteList(args.PlateNum);
                        if (isWhiteList == 0)
                        {
                            int monthleftday = psBll.IsLicensePlateIsMonthly(args.PlateNum);
                            if (monthleftday <= 0)
                                return;
                        }
                      
                    }

                }

                //读取会员Id
                int memberId = 0;
                if (!args.PlateColor.GetDisplayName().Equals("黄色"))
                {
                    if (netWorkOk)
                    {
                        memberId = psBll.GetMemberId(args.PlateNum);
                        //if (memberId != 0)
                        //psBll.UpdateMember(args.PlateNum, memberId);
                    }
                }

                //判断没有余位不抬杆，有余位后才抬杆
                bool isOpenGate = true;
                if(Params.Settings.CheckLeftCountSec > 0) 
                {
                    int isWhiteList = 0;
                    if (Params.Settings.EnabledWLGO)  //余位满后是否允许白名单车辆驶入
                    {
                        isWhiteList = psBll.CheckWhilteList(args.PlateNum);
                    }

                    int leftCount = psBll.getLeftCount();
                    if (leftCount < 1 && isWhiteList == 0) //没有余位并且非白名单车辆是不允许进入
                    {
                        OrderIpc orderIpc = new OrderIpc();
                        orderIpc.ipcCaramer = ipc;
                        orderIpc.licensePlateColor = args.PlateColor.GetDisplayName();
                        orderIpc.licensePlateNo = args.PlateNum;
                        orderIpc.picturePath = sightPath;
                        orderIpc.pictureName = sightName;
                        orderIpc.platePath = platePath;
                        orderIpc.plateName = plateName;
                        orderIpc.imageBase64 = imagebase64;
                        orderIpc.memberId = memberId;
                        isOpenGate = false;
                        ScreenShow_In_NoleftCount(ipc);//显示屏显示限制信息
                        string noLeftMsg = "【入场限制-没有余位不抬杆】,车牌号:" + args.PlateNum + "无法进入满员场地";
                        log.Warn(noLeftMsg);
                        //纪录需要抬杆的相机以及订单
                        bool isAdd = true;
                        try
                        {
                            foreach (OrderIpc orderIpctmp in checkLeftCountOpenIPC)
                            {
                                if (orderIpctmp.licensePlateNo == orderIpc.licensePlateNo) //该车辆已在等待区
                                {
                                    log.Warn("该车辆已在等待区,车牌号：" + orderIpctmp.licensePlateNo);
                                    isAdd = false;
                                    break;
                                }
                                else
                                {
                                    if (orderIpctmp.ipcCaramer.IP == orderIpc.ipcCaramer.IP)  //同一个入口，不同车牌号,把原来的删除重新增加当前的车牌
                                    {
                                        log.Warn("该入口[" + orderIpctmp.ipcCaramer.IP + "]等待车辆由" + orderIpctmp.licensePlateNo + "变更为" + orderIpc.licensePlateNo);
                                        checkLeftCountOpenIPC.Remove(orderIpctmp);
                                        break;
                                    }

                                }
                            }
                            if (isAdd)
                                checkLeftCountOpenIPC.Add(orderIpc);
                        }catch(Exception e)
                        {
                            log.Error("增加等待区车辆发生异常：" + e.Message);
                        }
                    }
                }

                if (isOpenGate)
                {
                    ipc.OpenGate();//打开道闸
                    log.Warn("【抬杆命令：】入口识别抬杆");
                    
                    //生成入场订单
                    OrderModel order = psBll.SaveOrder(1, args.PlateColor.GetDisplayName(), args.PlateNum,
                         sightPath, sightName, platePath, plateName,
                        imagebase64, DateTime.Now, memberId, Convert.ToInt32(netWorkOk), "");  //入场时间改成系统当前时间
                }
                else
                    log.Warn("入口识别无余位不抬杆");

                //读取会员信息
                /*int memberId = 0;
                if (!args.PlateColor.GetDisplayName().Equals("黄色"))
                {
                    if (netWorkOk)
                    {
                        memberId = psBll.GetMemberId(args.PlateNum);
                        //if (memberId != 0)
                            //psBll.UpdateMember(args.PlateNum, memberId);
                    }
                }
              
                //生成入场订单
                OrderModel order = psBll.SaveOrder(1, args.PlateColor.GetDisplayName(), args.PlateNum,
                     sightPath, sightName, platePath, plateName,
                    imagebase64, args.CaptureTime, memberId, Convert.ToInt32(netWorkOk), "");*/

                //余位
                ScreenShow_In_LeftCount();

                //界面控件更新
                AddOrderSnapshot(ipc.IP, guard, args.PlateNum, null, memberId, sightPath, sightName, "", "");
                this.Invoke(CtlUpdateDele, guardName, ipc.IP);

                curOrder.HandledOver = true;
                curOrder.HandledOverTime = System.DateTime.Now;
                //Console.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ",车牌号:" + args.PlateNum + "的业务逻辑处理结束!");
                log.Info("【入场】,车牌号:" + args.PlateNum);
            }
            catch (Exception ex)
            {
                log.Error("入场识别发生异常:" + ex.Message + "," + ex.StackTrace);
            }
        }
        /// <summary>
        /// 车辆入场保存订单
        /// </summary>
        /// <param name="paramOrders"></param>
        /// <returns></returns>
        public void CarIn(PlateEventArgs args)
        {
            //int memberId = 0;
            //if (netWorkOk)
            //    memberId = psBll.GetMemberId(args.PlateNum);
            //if (memberId != 0)
            //    psBll.UpdateMember(args.PlateNum, memberId);

            ////生成入场订单
            //OrderModel order = psBll.SaveOrder(1, args.PlateColor.GetDisplayName(), args.PlateNum,
            //     sightPath, sightName, platePath, plateName,
            //    imagebase64, args.CaptureTime, memberId, Convert.ToInt32(netWorkOk), "");

            ////界面控件更新
            //AddOrderSnapshot(ipc.IP, guard, args.PlateNum, null, memberId, sightPath, sightName, "", "");
            //this.Invoke(CtlUpdateDele, guardName, ipc.IP);

            //curOrder.HandledOver = true;
            //curOrder.HandledOverTime = System.DateTime.Now;
            //Console.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ",车牌号:" + args.PlateNum + "的业务逻辑处理结束!");
            //log.Info("【入场】,车牌号:" + args.PlateNum);
        }

        /// <summary>
        /// 检查查询订单的线程情况
        /// </summary>
        /// <param name="paramOrders"></param>
        /// <returns></returns>
        private bool CheckQueryThread()
        {
            log.Debug("进行CheckQueryThread");
            foreach (var i in handles)
            {
                log.Debug(i.IsExit + "," + i.HandledOver + "," + i.GuardNo + "," + i.HandleCount + "," + i.IP + ",（" + (i.Order == null) + "," + i.CaptureTime.ToString());
            }
            if (handles.Count(a => a.IsExit) == 0)
            {
                checkOrderTimer.Change(Timeout.Infinite, Timeout.Infinite);
                return false;
            }
            int count1 = handles.Count(a => a.IsExit && a.HandledOver);
            List<HandleOrder> exitOrders = handles.Where(a => a.IsExit).ToList();
            if (count1 > 0 && count1 == exitOrders.Count)
            {
                checkOrderTimer.Change(Timeout.Infinite, Timeout.Infinite);
                return false;
            }
            int count2 = handles.Count(a => a.IsExit && !a.HandledOver && a.HandleCount >= Params.Settings.Serv.PayCheckMaxRetryCount);
            List<HandleOrder> noHandleOrders = handles.Where(a => a.IsExit && !a.HandledOver).ToList();
            if (count2 > 0 && count2 == noHandleOrders.Count)
            {
                checkOrderTimer.Change(Timeout.Infinite, Timeout.Infinite);
                return false;
            }
            log.Debug("fanhui true");
            return true;
        }

        /// <summary>
        /// 获取订单支付状态  
        /// </summary>
        /// <param name="state"></param>
        private void GetOrderState(object state)
        {
            try
            {
                lock (qtlocker)
                {
                    if (!CheckQueryThread())
                        return;

                    for (int j = 0; j < handles.Count; j++)
                    {
                        HandleOrder item = handles[j];
                        log.Debug("IsExit:" + item.IsExit + ",HandledOver:" + item.HandledOver + ",item.HandleCount:" + item.HandleCount + ", item.Order is null" + (item.Order == null));

                        //出口订单中只执行次数还未超过阈值并且还未处理完毕的数据
                        if (!(item.IsExit && !item.HandledOver && item.HandleCount < Params.Settings.Serv.PayCheckMaxRetryCount && item.Order != null))
                            continue;

                        item.HandleCount++;
                        Console.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + item.IP + "," + item.GuardNo + ",【" + item.Order.LicensePlateNo + "】" + item.HandleCount);

                        log.Debug(item.IP + "," + item.GuardNo + ",【" + item.Order.LicensePlateNo + "】" + item.HandleCount);

                        int memberId = item.Order.MemberId;
                        if (item.Order.LicensePlateType==1)
                        {
                            if (netWorkOk)
                                memberId = psBll.GetMemberId(item.Order.LicensePlateNo);
                            if (memberId != 0)
                                psBll.UpdateMember(item.Order.LicensePlateNo, memberId);
                        }
                        

                        OrderReturn modereturn = psBll.GetOrderState(item.Order.OrderNo);
                        double actualAmount = modereturn.ActualAmount;
                        double actualGetAmount = modereturn.ActualGetAmount;

                        //支付未驶离并且收到金额和待支付金额一致
                        if (modereturn.state == 15 && actualAmount == actualGetAmount)
                        {
                            log.Debug(item.IP + "," + item.GuardNo + ",付费结束");
                            //应收已收界面变化
                            string guardName = (item.IsExit ? "出口" : "入口") + "_" + item.GuardNo;
                            lblSummaryChange(guardName, actualGetAmount, modereturn.OrderCharge, modereturn.DiscountAmount);

                            item.Order.Paytime = modereturn.Paytime;
                            item.Order.ActualGetAmount = modereturn.ActualGetAmount;
                            item.Order.ActualAmount = modereturn.ActualAmount;
                            item.Order.OrderCharge = modereturn.OrderCharge;
                            item.Order.DiscountAmount = modereturn.DiscountAmount;
                            item.Order.ParkingTime = modereturn.parkingTime;
                            item.Order.PayMoney = 0;
                            item.Order.Note = "";
                            OrderModel mode = psBll.UpdateState(3, item.Order, Convert.ToInt32(netWorkOk));
                            if (mode.result != 1000)
                            {
                                log.Error("【出场】," + item.IP + "," + item.Order.LicensePlateNo + "已支付,但是保存数据失败");
                                if (Params.User.StaffId != Params.noBodyEmpId.ToString())
                                    MessageBox.Show(mode.resultVal);
                            }
                            //余位
                            ScreenShow_In_LeftCount();

                            string cartype = memberId > 0 ? "会员车" : "临时车";
                            IPC ipc = mysdk.IPCs.First(a => a.IP == item.IP);
                            ipc.OpenGate();
                            log.Debug("【抬杆命令：】出口更新订单状态抬杆");
                            ScreenShow_Out(ipc, item.Order.LicensePlateNo, cartype);

                            Guard guard = setting.Guards.FirstOrDefault(g => g.IsExit == item.IsExit && g.No == item.GuardNo);
                            if (guard != null)
                                ButtonChangeGrayOrBlue(guard, false);

                            log.Info("【出场】,车牌号:" + item.Order.LicensePlateNo);
                            handles[j].HandledOver = true;
                            handles[j].HandledOverTime = System.DateTime.Now;
                            handles[j].Order = null;
                         
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("getOrderState" + ex + "," + ex.StackTrace);
            }
        }
        /// <summary>
        /// 识别出场
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void IPC_PlateExitEvent(object sender, PlateEventArgs args)
        {
            IPC ipc = (IPC)sender;
            //如果是白牌车直接开道闸
            if (args.PlateColor.GetDisplayName().Equals("白色"))
            {
                ipc.OpenGate();//打开道闸
                log.Debug("【抬杆命令：】白牌车出口识别抬杆，车牌号：" + args.PlateNum);
                return;
            }

            if (Params.Settings.EnabledWhiteListNoOrder)
            {
                int isWhiteList = psBll.CheckWhilteList(args.PlateNum);
                //判断是不是白名单车辆
                if (isWhiteList == 1)
                {
                    ipc.OpenGate();//打开道闸
                    log.Debug("【抬杆命令：】白名单出口识别抬杆");
                    return;
                }
               
            }
            Console.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + ipc.IP + ",开始处理【出场】车牌号:" + args.PlateNum + "的业务逻辑!");
            log.Debug("ip:" + ipc.IP + ",plate:" + args.PlateNum + "开始处理出场业务逻辑");
            try
            {
                HandleOrder curOrder = null;
                Guard guard = null;
                string sightName = "";
                string sightPath = "";
                string plateName = "";
                string platePath = "";
                string imagebase64 = "";
                bool canContinue = PlateFilter(ipc, args, ref curOrder, ref guard);
                if (!canContinue)
                    return;
                string guardName = (guard.IsExit ? "出口" : "入口") + "_" + guard.No;

                //2.订单找不到:入场或出场识别有误、或者没抓拍到入场图片
                string enterImgPath = psBll.Getpic(args.PlateNum);
                if (string.IsNullOrEmpty(enterImgPath))
                {
                    Console.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "【订单未找到】,车牌号:" + args.PlateNum + "出场");
                    bool ignoreTreated = true;
                    if (Params.User.StaffId != Params.noBodyEmpId.ToString() && btnChangeMode.Text == "设置无人模式")
                    {
                        ignoreTreated = false;

                        //有人值守，判断是否包月或者白名单车辆，是的话直接放行
                        int isWhiteList = psBll.CheckWhilteList(args.PlateNum);
                        //判断是不是白名单车辆
                        if (isWhiteList == 1)
                        {
                            ipc.OpenGate();//打开道闸
                            log.Debug("【抬杆命令：】白名单车辆识别错误或无入场订单出口直接抬杆");
                            return;
                        }

                        //显示识别有误
                        ScreenShow_OutError(ipc, args.PlateNum);


                        IntPtr hwnd = FindWindow(null, guardName);
                        log.Debug("name:" + guardName + ",hwnd" + hwnd);

                        if (hwnd == IntPtr.Zero)
                        {
                            SaveCapPicture(args, ipc.IP, ref imagebase64, ref sightPath, ref sightName, ref platePath, ref plateName);

                            //弹窗进行修正车牌号
                            Thread t = new Thread(() =>
                            {
                                //FrmModifyPlate form = new FrmModifyPlate(sightPath + "\\" + sightName, args.PlateColor.GetDisplayName(), args.PlateNum, sightPath, sightName, platePath, plateName, imagebase64, args.CaptureTime, guard);
                                //出场时间为系统当前时间
                                FrmModifyPlate form = new FrmModifyPlate(sightPath + "\\" + sightName, args.PlateColor.GetDisplayName(), args.PlateNum, sightPath, sightName, platePath, plateName, imagebase64, DateTime.Now, guard);
                                form.CarOutDele = CarOut;
                                form.NoneConfirmDele = Noneconfirm;
                                form.Ipc = ipc;
                                form.CurOrder = curOrder;
                                form.TopMost = true;
                                form.TopLevel = true;
                                form.ShowDialog();
                            });
                            t.Start();
                        }

                    }

                    if (ignoreTreated)
                    {
                        //自动选择最佳匹配(匹配度>=0.8)
                        List<PicNum> piclist = psBll.GetpicList(args.PlateNum);
                        if (piclist != null && piclist.Count > 0)
                        {
                            StringCompute stringcompute1 = new StringCompute();
                            for (int i = 0; i < piclist.Count; i++)
                            {
                                stringcompute1.SpeedyCompute(args.PlateNum, piclist[i].LicensePlateNo);    // 计算相似度， 不记录比较时间
                                decimal rate = stringcompute1.ComputeResult.Rate;    // 相似度百分之几，完全匹配相似度为1
                                piclist[i].similarity = rate;
                            }
                            piclist.Sort(delegate (PicNum p1, PicNum p2) { return p2.similarity.CompareTo(p1.similarity); });
                            // if (Convert.ToDouble(piclist[0].similarity) >= 0.8)
                            log.Warn("【出场】,状态:无人值守,车牌号:" + args.PlateNum + ",找到匹配度" + piclist[0].similarity + "的车牌" + piclist[0].LicensePlateNo);
                        }

                        SaveCapPicture(args, ipc.IP, ref imagebase64, ref sightPath, ref sightName, ref platePath, ref plateName);
                        ipc.OpenGate();
                        log.Debug("【抬杆命令：】出口无人模式识别错误抬杆");
                        ScreenShow_Out(ipc, args.PlateNum, "");

                        curOrder.HandledOver = true;
                        curOrder.HandledOverTime = System.DateTime.Now;
                        log.Warn("【出场】,车牌号:" + args.PlateNum + "非正常情况出场");
                    }
                    Console.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ",车牌号:" + args.PlateNum + "的业务逻辑处理结束!");
                }
                else
                {
                    if (Params.Settings.EnabledHotel)
                    {
                        psBll.checkWhiteMonthly(args.PlateNum, Convert.ToInt32(netWorkOk));
                    }
                    SaveCapPicture(args, ipc.IP, ref imagebase64, ref sightPath, ref sightName, ref platePath, ref plateName);
                    //出场时间改成系统当前时间
                    CarOut(ipc, guard, curOrder, args.PlateColor.GetDisplayName(), args.PlateNum, sightPath, sightName, platePath, plateName, imagebase64, DateTime.Now, enterImgPath);
                    //CarOut(ipc, guard, curOrder, args.PlateColor.GetDisplayName(), args.PlateNum, sightPath, sightName, platePath, plateName, imagebase64, args.CaptureTime, enterImgPath);
                }
            }
            catch (Exception ex)
            {
                log.Error("出场识别发生异常:" + ex.Message + ",其他" + ex.StackTrace);
            }

        }
        /// <summary>
        /// 车辆出场主要处理逻辑
        /// </summary>
        /// <param name="ipc"></param>
        /// <param name="guard"></param>
        /// <param name="curOrder"></param>
        /// <param name="plateColor"></param>
        /// <param name="plateNumber"></param>
        /// <param name="sightPath"></param>
        /// <param name="sightName"></param>
        /// <param name="platePath"></param>
        /// <param name="plateName"></param>
        /// <param name="imgbase64"></param>
        /// <param name="captureDate"></param>
        /// <param name="inImgPath"></param>
        private void CarOut(IPC ipc, Guard guard, HandleOrder curOrder, string plateColor, string plateNumber, string sightPath, string sightName, string platePath, string plateName, string imgbase64, DateTime captureDate, string inImgPath)
        {
            string RoadRateNoOut = "出口_" + guard.No;
            if (Params.Settings.EnabledYellowReCalculation)
            {
                int ReCalculation=psBll.IsReCalculation(1,plateNumber, RoadRateNoOut, Convert.ToInt32(netWorkOk));
                if (ReCalculation == 0)
                {
                    ipc.OpenGate();
                    log.Debug("【抬杆命令：】出口识别正确，黄牌车不用重复收费并且抬杆，逻辑结束");
                    return;
                }

            }
            //免费时间端前进入的车
            if (Params.Settings.EnabledFreeTime)
            {
                int ReCalculation = psBll.IsReCalculation(2,plateNumber, RoadRateNoOut, Convert.ToInt32(netWorkOk));
                if (ReCalculation == 0)
                {
                    ipc.OpenGate();
                    log.Debug("【抬杆命令：】出口识别正确，免费时间端前进入的车直接抬杆放行，逻辑结束");
                   // AddOrderSnapshot(ipc.IP, guard, plateNumber, order, memberId, "", "", sightPath, sightName);
                    return;
                }

            }
            //读取、更新会员信息
            int memberId = 0;
            if (plateColor != "黄色")
            {
                if (netWorkOk)
                    memberId = psBll.GetMemberId(plateNumber);
                if (memberId != 0)
                    psBll.UpdateMember(plateNumber, memberId);
            }
          

            //查询订单所需支付金额,会访问云平台
          
            OrderModel order = psBll.SaveOrder(2, plateColor, plateNumber,
            sightPath, sightName, platePath, plateName,
             imgbase64, captureDate, memberId, Convert.ToInt32(netWorkOk), RoadRateNoOut);

            if (order.OrderNo == String.Empty || order.OrderNo == null)
            {
                log.Warn("该车无入场图片并未找到订单,车牌号：" + plateNumber);
                return;
            }
            order.Pb = imgbase64;
            order.PictureName = sightName;
            order.LicensePlateNo = plateNumber;
            curOrder.Order = order;

            double needPayMoney = order.PayDifferenceAmount;
            string carType = GetCarType(order.ChargeType, memberId);
            if (needPayMoney > 0)
            {
                //显示屏显示缴费
                ScreenShow_Charge(ipc, plateNumber, needPayMoney, carType);

                //界面上一些"放行"按钮是否可用
                if (Params.User.StaffId != Params.noBodyEmpId.ToString())
                {
                    //只有切到正在发生业务的tab按钮可用
                    ButtonChangeGrayOrBlue(guard, true);
                }

            }

            //界面控件更新数据
            string guardName = (guard.IsExit ? "出口" : "入口") + "_" + guard.No;
            AddOrderSnapshot(ipc.IP, guard, plateNumber, order, memberId, "", "", sightPath, sightName);
            if (inImgPath == null || inImgPath == "\\") 
            {
                inImgPath = processDir + "\\images\\nosignal.jpg";
            }
            snapDict[guardName].InFilePath = System.IO.Path.GetDirectoryName(inImgPath);
            snapDict[guardName].InFileName = System.IO.Path.GetFileName(inImgPath);
            this.Invoke(CtlUpdateDele, guardName, ipc.IP);

            //订单已支付结束或者无需付费
            if (order.State == 30 || needPayMoney == 0)
            {
                ipc.OpenGate();
                log.Debug("【抬杆命令：】出口识别正确并且无收费抬杆,车牌号：" + plateNumber);
                ScreenShow_Out(ipc, plateNumber, carType);
                ButtonChangeGrayOrBlue(guard, false);

                order.PayMoney = needPayMoney;
                order.Pb = imgbase64;
                order.PictureName = sightName;

                OrderModel resultOrder = psBll.UpdateState(3, order, Convert.ToInt32(netWorkOk));
                if (resultOrder.result != 1000)
                    log.Error("CarOut方法发生异常:" + plateNumber + resultOrder.resultVal);
                    //MessageBox.Show(resultOrder.resultVal);

                //余位
                ScreenShow_In_LeftCount();

                curOrder.HandledOver = true;
                curOrder.HandledOverTime = System.DateTime.Now;
                curOrder.Order = null;
                Console.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ",车牌号:" + plateNumber + "的业务逻辑处理结束!");
                log.Info("【出场】,车牌号:" + plateNumber);
            }
            else
            {
                if (netWorkOk)
                {
                    if (checkOrderTimer == null)
                        checkOrderTimer = new System.Threading.Timer(GetOrderState, null, 0, Params.Settings.Serv.PayCheckFreq);
                    else
                        checkOrderTimer.Change(0, Params.Settings.Serv.PayCheckFreq);
                }
            }
        }

        /// <summary>
        /// 未找到订单-弹窗，其他选项(非修正车牌)业务处理
        /// </summary>
        /// <param name="guard"></param>
        /// <param name="ipc"></param>
        /// <param name="type"></param>
        private void Noneconfirm(Guard guard, IPC ipc, int type)
        {
            if (type == 0) //弹出框点击"关闭"
            {

            }
            else if (type == 1) //弹出框点击"放行"
            {
                ipc.OpenGate();
                log.Debug("【抬杆命令：】识别错误，弹窗里的放行抬杆按钮");
                ButtonChangeGrayOrBlue(guard, false);

                HandleOrder curOrder = handles.FirstOrDefault(h => h.IsExit == guard.IsExit && h.GuardNo == guard.No);
                if (curOrder != null)
                {
                    curOrder.HandledOver = true;
                    curOrder.HandledOverTime = System.DateTime.Now;
                    curOrder.Order = null;
                }

            }

        }
        #endregion

        #region 次要业务功能
        /// <summary>
        /// 特殊放行(主要针对于找不到订单的情况)
        /// </summary>
        /// <param name="type"></param>
        private void Discharged(int type)
        {
            if(Params.Duty.WorkNo != null && Params.Duty.WorkNo != "")
            { }
            int tabindex = tabControl1.SelectedIndex;
            Guard guard = setting.Guards[tabindex];
            HandleOrder curOrder = handles.FirstOrDefault(h => h.GuardNo == guard.No && h.IsExit == guard.IsExit);
            if (!guard.IsExit || curOrder == null || curOrder.HandledOver || curOrder.Order == null || string.IsNullOrEmpty(curOrder.Order.OrderNo))
                return;

            string note = "";
            if (type == 0)
            {
                note = "收费放行";
            }
            else if (type == 1)
            {
                note = "免费放行";
            }
            else if (type == 2)
            {
                note = "免费券放行";
            }
            double Amount = string.IsNullOrEmpty(txtMoney.Text) ? 0 : Convert.ToDouble(txtMoney.Text);
            Amount = type == 0 ? Amount : 0;

            ChargeOnDutyModel modereturn = psBll.SaveChargeRecord(curOrder.Order.OrderNo, Amount, Params.Duty.WorkNo);
            if (!modereturn.returnstr.Equals("保存成功"))
                MessageBox.Show(modereturn.returnstr);

            lblCash.Text = modereturn.CashAmount.ToString();
            curOrder.Order.Note = note;
            curOrder.Order.Paytime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            curOrder.Order.PayMoney = Amount;
            curOrder.Order.PayNo = "";
            curOrder.Order.PayType = 4;
            curOrder.Order.ChargeEmp = Convert.ToInt32(Params.User.StaffId);

            OrderModel mode = psBll.UpdateState(3, curOrder.Order, Convert.ToInt32(netWorkOk));
            if (mode.result != 1000)
                MessageBox.Show(mode.resultVal);

            //余位
            ScreenShow_In_LeftCount();

            int memberId = curOrder.Order.MemberId;
            string cartype = "";
            if (memberId > 0)
                cartype = "会员车";
            else
                cartype = "临时车";

            IPC ipc = mysdk.IPCs.First(a => a.IP == guard.Primary.IP);
            ipc.OpenGate();
            log.Debug("【抬杆命令：】确认收费，免费（券）放行抬杆按钮");
            ScreenShow_Out(ipc, curOrder.Order.LicensePlateNo, cartype);
            ButtonChangeGrayOrBlue(guard, false);

            curOrder.HandledOver = true;
            curOrder.HandledOverTime = System.DateTime.Now;
            //改变应收以收未收
            string guardName = (guard.IsExit ? "出口" : "入口") + "_" + guard.No;
            lblSummaryChange(guardName, Amount, curOrder.Order.OrderCharge,curOrder.Order.DiscountAmount);
           
            if (type == 0)
            {
                log.Info("【收费放行】,订单号：" + curOrder.Order.OrderNo + "车牌号：" + curOrder.Order.LicensePlateNo + ",收费:" + Amount);
            }
            else if (type == 1)
            {
                log.Info("【免费放行】，订单号：" + curOrder.Order.OrderNo + "车牌号：" + curOrder.Order.LicensePlateNo);
            }
            else if (type == 2)
            {
                log.Info("【免费券放行】，订单号：" + curOrder.Order.OrderNo + "车牌号：" + curOrder.Order.LicensePlateNo);
            }
            curOrder.Order = null;
            CheckQueryThread();
        }

        /// <summary>
        /// 触发特殊放行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPass_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button == btnConfPass)//确定进行手动放行
            {
                Discharged(0);
            }
            else if (button == btnFree)//免费放行
            {
                Discharged(1);
            }
            else if (button == btnFreeTicket)// 免费券放行
            {
                Discharged(2);
            }
        }
        /// <summary>
        /// 放行后应收已收待收变换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblSummaryChange(string guardName, double Amount, double OrderCharge, double DiscountAmount)
        {
            string message = "应收:" + OrderCharge + " 已收:" + Amount + " 待收:0" + " 优惠:" + DiscountAmount;

            if (Amount == 0)
                message = message + ",免费放行";

            OrderSnapshot snapshot = snapDict[guardName];
            string ip = snapshot.IP;
            snapshot.ChargeMsg = message;
            snapDict[guardName] = snapshot;
            this.Invoke(CtlUpdateDele, guardName, ip);
        }

        /// <summary>
        /// 抬杆(无业务，纯粹的抬杆)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRaise_Click(object sender, EventArgs e)
        {
            if (mysdk != null && mysdk.IPCs.Count > 0)
            {
                int tabindex = tabControl1.SelectedIndex;
                Guard guard = setting.Guards[tabindex];
                IPC ipc = mysdk.IPCs.First(a => a.IP == guard.Primary.IP);
                ipc.OpenGate();
                log.Debug("【抬杆命令：】抬杆按钮" + ipc.IP);

                //new add 2019-03-02
              
               // Guard guard = setting.Guards[tabindex];
                HandleOrder curOrder = handles.FirstOrDefault(h => h.IsExit == guard.IsExit && h.GuardNo == guard.No);
                if (curOrder != null)
                {
                    curOrder.HandledOver = true;
                    curOrder.HandledOverTime = System.DateTime.Now;
                }
                   

            }
        }

        /// <summary>
        /// 抬杆(有业务)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRaiseOther_Click(object sender, EventArgs e)
        {
            log.Debug("进行了软触发");

            if (mysdk != null && mysdk.IPCs.Count > 0)
            {
                
                int tabindex = tabControl1.SelectedIndex;
                Guard guard = setting.Guards[tabindex];
                HandleOrder curOrder = handles.FirstOrDefault(h => h.IsExit == guard.IsExit && h.GuardNo == guard.No);
                if (curOrder != null)
                {
                   curOrder.HandledOver = true;
                   curOrder.HandledOverTime = System.DateTime.Now;
                }

                IPC ipc = mysdk.IPCs[tabControl1.SelectedIndex];
                if (ipc != null) 
                {
                    //手工抓拍
                    SoftTriggerData args = ipc.SoftTrigger();
                    if (args == null)
                        return;
                    long timestamp = Convert.ToInt64(SortingLine.Utility.DataHelper.GetLongTimeStamp());
                    string plateColor = args.PlateColor.GetDisplayName();

                    if (args.PlateNum == "无牌车")
                    {
                        MessageBox.Show("未检测到车牌，请退至识别区域!");
                        return;
                    }
                    else
                    {
                        PlateEventArgs plateargs = new PlateEventArgs(args.ImgData, args.ImgData, args.PlateNum, plateColor, 0, 0, 0, 0, 0, 0, timestamp, 0);
                        IPC_PlateEnterEvent(ipc, plateargs);
                    }

                }

            }
        }
        #endregion

        private void btnRaise1_Click(object sender, EventArgs e)
        {

        }

        private void pbVideo1_Click(object sender, EventArgs e)
        {

        }

        private void btnPersonPass_Click(object sender, EventArgs e)
        {

        }

        private void heartBeatTimer_Tick_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int leftBerthNum = psBll.CheckBerthNoOccupy(textBoxTestOne.Text.Trim());
            if (leftBerthNum == 0)
            {
                //ScreenShow_NoOccupy(ipc, args.PlateNum);
                //return;
            }


            OrderModel order = psBll.SaveOrder(1, /*args.PlateColor.GetDisplayName()*/"蓝色", textBoxTestOne.Text.Trim(),
                         "", "", "", "",
                        "", DateTime.Now, 9001, Convert.ToInt32(netWorkOk), ""); ;
        }
    }
    #region 实体类模型
    /// <summary>
    /// 每个出入口正在处理的实体模型(包括业务上的单据)
    /// </summary>
    public class HandleOrder 
    {
        //上次识别到的车牌
        public string PlateNumber { get; set; }

        //出入口编号或名称
        public string GuardNo { get; set; }

        //是否出口
        public bool IsExit { get; set; }

        //正在处理的订单
        public OrderModel Order { get; set; }

        //此出入口上次抓拍到车牌时间
        public DateTime CaptureTime { get; set; }

        //正在处理的相机ip
        public string IP { get; set; }

        //处理完毕(入场完毕|出场支付完毕|出场完毕)
        public bool HandledOver { get; set; }

        //处理完成的时间点
        public DateTime HandledOverTime { get; set; }

        //查询订单已执行次数(出口有效)
        public int HandleCount { get; set; }
    }

    /// <summary>
    /// 每个出入口刚才发生的入场或者出场数据快照
    /// </summary>
    public class OrderSnapshot 
    {
        //哪个ip的摄像头抓拍的
        public string IP { get; set; }

        //车牌号
        public string PlateNum { get; set; }

        public bool IsExit { get; set; }

        //入场时间
        public string InDate { get; set; }

        //入场保存的图片路径
        public string InFilePath { get; set; }

        //入场保存的图片名称
        public string InFileName { get; set; }

        //出场时间
        public string OutDate { get; set; }

        //出场保存的图片路径
        public string OutFilePath { get; set; }

        //出场保存的图片名称
        public string OutFileName { get; set; }

        //停留时间
        public string StayTime { get; set; }

        //车辆类型
        public string CarType { get; set; }

        //收费信息
        public string ChargeMsg { get; set; }

        //还需要支付多少钱
        public double Money { get; set; }
    }
    #endregion


}
