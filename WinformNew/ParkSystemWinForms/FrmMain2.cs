using Bealead.ICEIPC;
using Bealead.ICEIPC.Enums;
using Bealead.ICEIPC.Events;
using ParkSystemWinForms.Extends;
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
    public partial class FrmMain2 : Form
    {
        public bool isLogined = false;
        public string configFile = "";

        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        public const int WM_CLOSE = 0x10;

        private string moduleName = "";//本执行程序模块名称
        private string processDir = "";//本执行程序目录
        private string startDir = "";//开始菜单目录

        private static LogHelper log = LogFactory.GetLogger("PsFormLog");
        private Setting setting = new Setting();
        private ParkSystemBLL psBll = new ParkSystemBLL();
        private ICEIPC mysdk = null;

        private List<HandleOrder> handles = new List<HandleOrder>(); //当前每个出入口正在处理的订单
        private Dictionary<string, OrderSnapshot> snapDict = new Dictionary<string, OrderSnapshot>();//每个出入口之前处理的订单快照

        private Color disconnectColor = Color.Gray;
        private Color normalColor = Color.Green;
        int maxCount = 0;//一行最多显示多少个Panel
        Setting2 setting2 = new Setting2();
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)//禁止拖动
                return;
            base.WndProc(ref m);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        private const int WM_NCPAINT = 0x0085;
        private const int WM_NCACTIVATE = 0x0086;
        private const int WM_NCLBUTTONDOWN = 0x00A1;

        public FrmMain2()
        {
            InitializeComponent();

            processDir = System.Windows.Forms.Application.ExecutablePath;// Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            moduleName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
            startDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
            moduleName = moduleName.Replace("vshost.", "");
            processDir = processDir.Substring(0, processDir.LastIndexOf("\\") + 1);

        }

        private void LoadConfig() 
        {
            psBll.LoadConfig(ref setting);
            Params.Settings = setting;
            Params.Settings.ParkLot.Id = ConfigurationManager.AppSettings["ParkingLotId"];
            try
            {
                string content = FileHelper.ReadFile(this.configFile);
                if (!string.IsNullOrEmpty(content)) 
                {
                    setting2 = JsonHelper.JsonToEntity<Setting2>(content);
                    Params.Settings2 = setting2;

                } 
                Params.JsonConfig = this.configFile;
               

            }
            catch (Exception ex)
            {

            }
        }

        #region 其他
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
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法设置成开机启动," + ex.Message);
            }
        }

        private Guard FindGuardByIP(string ip)
        {
            log.Debug("1");
            Guard targetGuard = null;
            foreach (var guard in setting2.Guards)
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
        /// 识别入场
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void IPC_PlateEnterEvent(object sender, PlateEventArgs args)
        {
            IPC ipc = (IPC)sender;

            Console.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + ipc.IP + "开始处理【入场】车牌号:" + args.PlateNum + "的业务逻辑!");
            log.Debug("ip:" + ipc.IP + ",plate:" + args.PlateNum + "开始进行入场逻辑");
            try
            {
                log.Debug("1");
                Guard guard = null;
                guard = FindGuardByIP(ipc.IP);
                log.Debug("2");
                string guardName = (guard.IsExit ? "出口" : "入口") + "_" + guard.No;
                psBll.SaveOrder_count(1, args.PlateColor.GetDisplayName(), args.PlateNum, args.CaptureTime, guardName);
                Console.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ",车牌号:" + args.PlateNum + "的业务逻辑处理结束!");
                log.Info("【入场】,车牌号:" + args.PlateNum);
            }
            catch (Exception ex)
            {
                log.Error("入场识别发生异常:" + ex.Message + "," + ex.StackTrace);
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


            Console.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + ipc.IP + ",开始处理【出场】车牌号:" + args.PlateNum + "的业务逻辑!");
            log.Debug("ip:" + ipc.IP + ",plate:" + args.PlateNum + "开始处理出场业务逻辑");
            try
            {
                Guard guard = null;
                guard = FindGuardByIP(ipc.IP);
                string guardName = (guard.IsExit ? "出口" : "入口") + "_" + guard.No;
                psBll.SaveOrder_count(2, args.PlateColor.GetDisplayName(), args.PlateNum, args.CaptureTime, guardName);
 
            }
            catch (Exception ex)
            {
                log.Error("出场识别发生异常:" + ex.Message + ",其他" + ex.StackTrace);
            }

        }
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

        /// <summary>
        /// 保存图片
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
        /// <summary>
        /// 设备在线、离线变动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void IPC_StatusChangeEvent(object sender, DevStatusChangeEventArgs args)
        {
            IPC ipc = (IPC)sender;
            Console.WriteLine(string.Format("{0},ip:{1},出入口:{2},状态:{3}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), ipc.IP, ipc.Type ? "出口" : "入口", ipc.IsAlive ? "在线" : "离线"));
            string msg = string.Format("ip:{0},出入口:{1},状态:{2}", ipc.IP, ipc.Type ? "出口" : "入口", ipc.IsAlive ? "在线" : "离线");
            if (mysdk.IPCs.Count < 1)
                return;
            if (ipc.IsAlive)
                log.Info(msg);
            else
                log.Warn(msg);

            int findIndex = mysdk.IPCs.FindIndex(a => a.IP == ipc.IP);
            if (findIndex >= 0)
            {
                if (ipc.IsAlive)
                    pnlControls.Controls[findIndex].BackColor = normalColor;
                else
                    pnlControls.Controls[findIndex].BackColor = disconnectColor;
            }
        }

        /// <summary>
        /// 设备IO变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void IPC_IOChangeEvent(object sender, DevIOChangeEventArgs args)
        {
        }
        #endregion

        #region 界面变化

        private void AssignControl(List<Control> ctls, Guard guard)
        {
            string type = guard.IsExit ? "出" : "入";
            ctls[0].Text = guard.No + "(" + type + ")";
            ctls[0].ForeColor = Color.White;
        }

        private void ClearPanel()
        {
            //将界面多余的Panel删除
            for (int i = pnlControls.Controls.Count - 1; i >= 0; i--)
            {
                if (i > setting2.Guards.Count - 1)
                {
                    pnlControls.Controls.RemoveAt(i);
                }
            }
        }
        private void LoadPanels()
        {
            try
            {
                if (setting2.Guards.Count < 1)
                {
                    panel1.Visible = false;
                    ClearPanel();
                }
                else
                {
                    panel1.BackColor = disconnectColor;
                    Dictionary<int, string> locDict = new Dictionary<int, string>();
                    for (int i = 0; i < setting2.Guards.Count; i++)
                    {
                        Guard guard = setting2.Guards[i];
                        List<Control> cs = new List<Control>();
                        if (i == 0)
                        {
                            panel1.Visible = true;
                            locDict.Add(0, panel1.Location.X + "," + panel1.Location.Y);
                        }
                        else
                        {
                            int count = pnlControls.Controls.Cast<Control>().Count(c => c.GetType() == typeof(Panel));
                            if (count <= i)
                            {
                                Panel pnl = ControlExtensions.Clone(panel1);
                                pnl.Name = panel1.Name + i;
                                pnl.MouseLeave += new System.EventHandler(this.panel_MouseLeave);
                                pnl.MouseHover += new System.EventHandler(this.panel_MouseHover);

                                //设置横向间距以及是否换行
                                int x = panel1.Width * i + panel1.Location.X * (i + 1);
                                int x1 = panel1.Width * (i + 1) + panel1.Location.X * (i + 2);
                                int y = panel1.Location.Y;
                                if (x1 >pnlControls.Width)
                                {
                                    if (maxCount == 0)
                                        maxCount = i;
                                    int mod = i % maxCount;
                                    string[] tmp = locDict[mod].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                    x = Convert.ToInt32(tmp[0]);
                                    int rowIndex = (int)Math.Ceiling((double)(i + 1) / maxCount);
                                    y = panel1.Height * (rowIndex - 1) + panel1.Location.Y * (rowIndex);

                                }
                                pnl.Location = new Point(x, y);
                                locDict.Add(i, x + "," + y);
                                pnl.BackColor = disconnectColor;
                                pnlControls.Controls.Add(pnl);

                                foreach (Label l in panel1.Controls.Cast<Control>().OrderBy(a => Convert.ToInt32(a.Tag)))
                                {
                                    Label newlabel = ControlExtensions.Clone(l);
                                    newlabel.Name = l.Name + i;
                                    pnl.Controls.Add(newlabel);
                                    cs.Add(newlabel);
                                }

                            }

                        }
                        cs = pnlControls.Controls[i].Controls.Cast<Control>().OrderBy(a => Convert.ToInt32(a.Tag)).ToList();
                        AssignControl(cs, guard);
                    }
                    ClearPanel();
                }
            }
            catch (Exception ex) 
            {

            }
        }

        #endregion

        #region 控件事件

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            label1.Text = "";
            this.Text = string.Format("停车场监控客户端v1.0.0_{0}_{1}", setting.ParkLot.Name, setting2.Name);

        }
        private void tsmiSetting_Click(object sender, EventArgs e)
        {
            FrmPwd pwd = new FrmPwd();
            pwd.ChangeSettingDele = () =>
            {


            };
            pwd.ShowDialog();
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            FrmLoadCfg loadCfg = new FrmLoadCfg();
            loadCfg.main2 = this;
            loadCfg.ShowDialog();
            if (isLogined)
            {
                LoadConfig();
                InitControl();
                LoadPanels();
                BootOnStart();
                this.WindowState = FormWindowState.Maximized;


                List<Cond> entList = new List<Cond>();
                List<Cond> extList = new List<Cond>();
                foreach (var g in setting2.Guards) 
                {
                    Cond cond = new Cond();
                    cond.IP = g.Primary.IP;
                    if (!g.IsExit)
                        entList.Add(cond);
                    else
                        extList.Add(cond);
                }
                
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
            else
            {
                this.Close();
            }

        }

        /// <summary>
        /// 退出程序释放资源
        /// </summary>
        private void DisposeRes()
        {
            if (mysdk != null)
                mysdk.Fini();
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.isLogined)
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("您确定要登出系统吗?", "提示", messButton);
                if (dr == DialogResult.OK)
                {

                    DisposeRes();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void panel_MouseHover(object sender, EventArgs e)
        {
            Panel p = sender as Panel;
            int index= pnlControls.Controls.Cast<Control>().ToList().IndexOf(p);
            Guard guard = setting2.Guards[index];
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("主:" + guard.Primary.IP);
            if (guard.Secondaries != null && guard.Secondaries.Count > 0) 
            {
                sb.AppendLine("副:");
                guard.Secondaries.ForEach(a =>
                {
                    sb.AppendLine(" "+a.IP);
                });
            }
            panel1.ShowTooltip(toolTip1, sb.ToString(),5000);
        }

        private void panel_MouseLeave(object sender, EventArgs e)
        {
            //toolTip1.Active = false;
        }
        #endregion


    }
}
