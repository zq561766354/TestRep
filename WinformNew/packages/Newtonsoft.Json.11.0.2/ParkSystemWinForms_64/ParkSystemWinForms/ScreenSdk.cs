using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;//声明中使用System.Runtime.InteropServices命名空间

namespace ParkSystemWinForms
{
    public class ScreenSdk
    {
        /*
        //声明外部方法:
        [DllImport("DP703_64bit.dll", EntryPoint = "Init")]
        public static extern Boolean Init();
        [DllImport("DP703_64bit.dll", EntryPoint = "UnInit")]
        public static extern Boolean UnInit();
        [DllImport("DP703_64bit.dll", EntryPoint = "Send_To_Show")]
        public static extern Boolean Send_To_Show(string ip, int row, string data, int showMode);
        [DllImport("DP703_64bit.dll", EntryPoint = "Send_To_Voice")]
        public static extern Boolean Send_To_Voice(string ip, string data);
        [DllImport("DP703_64bit.dll", EntryPoint = "Set_Voice")]
        public static extern Boolean Set_Voice(string ip, int data);
        [DllImport("DP703_64bit.dll", EntryPoint = "Set_DisplayLines")]
        public static extern Boolean Set_DisplayLines(string ip, int data);
        */

        public const string dllname = "DP703x64.dll";
        //*声明外部方法:
        [DllImport(dllname, EntryPoint = "Init")]
        public static extern int Init(Boolean utf8, Boolean bMonitor);
        //public static extern int Init();
        [DllImport(dllname, EntryPoint = "UnInit")]
        public static extern int UnInit();
        [DllImport(dllname, EntryPoint = "Send_To_Show")]
        public static extern int Send_To_Show(string ip, int row, string data, int showMode);
        [DllImport(dllname, EntryPoint = "Send_To_Voice")]
        public static extern int Send_To_Voice(string ip, string data);
        [DllImport(dllname, EntryPoint = "Set_Voice")]
        public static extern int Set_Voice(string ip, int data);
        [DllImport(dllname, EntryPoint = "Set_DisplayLines")]
        public static extern int Set_DisplayLines(string ip, int data);
        //*/
        
    }
}
