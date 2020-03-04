using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkSystemWinForms
{
    public partial class FrmLoadCfg : Form
    {
        public FrmMain2 main2;

        public FrmLoadCfg()
        {
            InitializeComponent();
        }
        List<string> cfgFiles = new List<string>();
        private void FrmLoadCfg_Load(object sender, EventArgs e)
        {
            string processDir = System.Windows.Forms.Application.ExecutablePath;
            string moduleName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
            string startDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
            moduleName = moduleName.Replace("vshost.", "");
            processDir = processDir.Substring(0, processDir.LastIndexOf("\\") + 1);

            //读取Cfg里面的json配置文件

            FileHelper.GetFiles(processDir,cfgFiles ,new List<string>() { ".json" });
            foreach(var f in cfgFiles)
            {
                string name = Path.GetFileName(f);
                cboCfg.Items.Add(name);
            }
            if (cboCfg.Items.Count > 0)
                cboCfg.SelectedIndex = 0;
          

        }

        private void btnLoading_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboCfg.SelectedItem.ToString())) 
            {
                MessageBox.Show("缺少配置文件无法启动","提示");
                return;
            }
            main2.isLogined = true;
            main2.configFile = selectedFile;
            this.Close();
        }

        private string selectedFile = "";
        private void cboCfg_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filename = cboCfg.SelectedItem.ToString();
            foreach (var f in cfgFiles) 
            {
              int index=f.LastIndexOf("\\");
              string name=f.Substring(index+1, f.Length - index-1);
              if (filename.Equals(name)) 
              {
                  selectedFile = f;
                  break;
              }
            }
        }
    }
}
