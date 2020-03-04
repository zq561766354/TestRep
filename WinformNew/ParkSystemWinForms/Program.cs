using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace ParkSystemWinForms
{
    static class Program
    {
        private static LogHelper log = LogFactory.GetLogger("PsFormLog");

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Process current = Process.GetCurrentProcess();
                int mode = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["Mode"]);
                int openMode = 1;
                if(System.Configuration.ConfigurationManager.AppSettings["OpenMode"] != null)
                    openMode = Convert.ToInt32(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["OpenMode"]));
                switch (mode)
                {
                    case 1:
                        if (openMode == 1)
                        {
                            var runningProcess = Process.GetProcessesByName(current.ProcessName).FirstOrDefault(p => p.Id != current.Id);
                            if (runningProcess == null)
                            {
                                Application.ThreadException += Application_ThreadException;
                                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                                Application.ApplicationExit += Application_ApplicationExit;
                                Application.EnableVisualStyles();
                                Application.SetCompatibleTextRenderingDefault(false);
                                Application.Run(new FrmMain());
                            }
                            else
                            {
                                MessageBox.Show("一台电脑只能运行单个实例!", "提示");

                            }
                        }
                        else
                        {
                            Application.ThreadException += Application_ThreadException;
                            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                            Application.ApplicationExit += Application_ApplicationExit;
                            Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            Application.Run(new FrmMain());
                        }

                        //bool ret;

                        //  System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out ret);
                        //  if (ret)
                        //  {
                        //      Application.EnableVisualStyles();
                        //      Application.SetCompatibleTextRenderingDefault(false);
                        //      //Application.Run(new FrmLogin());
                        //      Application.Run(new  Form1());//文件下的窗体
                        //      mutex.ReleaseMutex();//释放一次
                        //  }
                        //  else {
                        //      MessageBox.Show(null, "有一个和本程序相同的应用程序已经在运行，请不要同时运行多个本程序。\n\n这个程序即将退出。", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //      //   提示信息，可以删除。   
                        //      Application.Exit();//退出程序  
                        //  }

                        //Application.EnableVisualStyles();
                        //Application.SetCompatibleTextRenderingDefault(false);
                        //Application.Run(new Form1());
                        break;
                    case 2:
                        Application.ThreadException += Application_ThreadException;
                        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                        Application.ApplicationExit += Application_ApplicationExit;
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new FrmMain2());
                        break;
                }

            }
            catch (Exception ex) 
            {
                log.Error("应用程序的主入口点发生异常：" + ex.Message);
            }

        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            log.Error(ex);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            log.Error(e.Exception);
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {


        }
    }
}
