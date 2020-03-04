using ParkSystemWinForms.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkSystemWinForms
{
    public partial class FrmSetting : Form
    {
        public delegate void ChangeSettingHandle();
        public ChangeSettingHandle ChangeSettingDele;
        private Dictionary<string, List<WapperGuardItem>> dict = new Dictionary<string, List<WapperGuardItem>>();
        public FrmSetting()
        {
            InitializeComponent();
        }
        

        private void FrmSetting_Load(object sender, EventArgs e)
        {
            //txtImgSavePath.Enabled = false;
            txtImgSavePath.Text = Params.Settings.ImagePath;
            nudInDelay.Value = Convert.ToDecimal(Params.Settings.InDelay/1000);
            nudOutDelay.Value = Convert.ToDecimal(Params.Settings.OutDelay/1000);
            nudInVol.Value = Convert.ToDecimal(Params.Settings.InVolume);
            nudOutVol.Value = Convert.ToDecimal(Params.Settings.OutVolume);
            nudInLine.Value= Convert.ToDecimal(Params.Settings.InLine);
            nudOutLine.Value = Convert.ToDecimal(Params.Settings.OutLine);
            nudScrInDelay.Value = Convert.ToDecimal(Params.Settings.ScreenInDelay);
            nudScrOutDelay.Value= Convert.ToDecimal(Params.Settings.ScreenOutDelay);
            chkWhiteList.Checked = Params.Settings.EnabledWhiteList;
            chkIswhiteorder.Checked = Params.Settings.EnabledWhiteListNoOrder;
            LeftCountChk.Checked = Params.Settings.EnabledShowLeftCount;
            WLgochk.Checked = Params.Settings.EnabledWLGO;
            ISwhiteListchk.Checked = Params.Settings.EnabledWhiteListUsed;
            MonthlyPassChb.Checked = Params.Settings.EnabledMonthlyPass;
            hotelchk.Checked = Params.Settings.EnabledHotel;
            chkYellowCarNum.Checked = Params.Settings.EnabledYellowReCalculation;
            chkFreeTime.Checked = Params.Settings.EnabledFreeTime;
            chkShowBnt.Checked = Params.Settings.EnabledShowBnt;
            cbWelcome.Checked = Params.Settings.IsWelcome;
            //添加checkbox表示是否一位多车
            CBOneSpaceMoreCars.Checked = Params.Settings.EnableOneSpaceMoreCars;

            txtParkNo.Text = Params.Settings.ParkLot.No;
            txtUrl.Text = Params.Settings.Serv.Url;
            nudHeartBeatFreq.Value = Convert.ToDecimal(Params.Settings.Serv.HeartBeatFreq);
            nudHeatBeatFail.Value= Convert.ToDecimal(Params.Settings.Serv.HeartBeatMaxRetryCount);
            nudCheckPay.Value =Convert.ToDecimal(Params.Settings.Serv.PayCheckFreq/1000);
            nudCheckMaxCount.Value = Convert.ToDecimal(Params.Settings.Serv.PayCheckMaxRetryCount);
            nudKeepOpen.Value = Convert.ToDecimal(Params.Settings.Serv.GateKeepOpenFreq/1000);
            nudData.Value= Convert.ToDecimal(Params.Settings.Serv.DataSyncFreq/1000);
            nudOutMistaken.Value = Convert.ToDecimal(Params.Settings.MistakenOutSec / 1000)<1?15: Convert.ToDecimal(Params.Settings.MistakenOutSec / 1000);

            nudHour.Value = Convert.ToInt32(Params.Settings.HourSync);
            nudMin.Value = Convert.ToInt32(Params.Settings.MinSync);

            updateBerthNumChb.Checked = Params.Settings.IsUpdateBerthNum;
            nudCheckLeftCountSec.Value = Params.Settings.CheckLeftCountSec;
            brokenNetOrderFre.Value = Params.Settings.brokenNetOrderFre;
            if (Params.Settings.apiTimeout == 0)
                apiTimeout.Value = 5;
            else
                apiTimeout.Value = Params.Settings.apiTimeout;
            //dateTimeSync.Value = Convert.ToDateTime(Params.Settings.TimeSync);
            if (Params.Settings.MistakenEntranceMin != 0)
                nudEntranceMistaken.Value = Convert.ToDecimal(Params.Settings.MistakenEntranceMin);
            nudUnidentifiedTimer.Value = Params.Settings.UnidentifiedTimerMin;

            foreach (Model.Guard guard in Params.Settings.Guards) 
            {
                string flag = guard.IsExit ? "(出)" : "(入)";
                string name = guard.No + flag;
                TreeNode tn = new TreeNode(name);
                tn.Tag = guard.IsExit;
                tn.ForeColor = !guard.IsExit ? Color.Green : Color.Red;
                tvGuard.Nodes.Add(tn);

                List<WapperGuardItem> list = null;
                 if (!dict.ContainsKey(name))
                     list = new List<WapperGuardItem>();
                 else
                     list = dict[name];

                 list.Add(new WapperGuardItem()
                 {
                     isPrimary=true,
                     guardItem = new GuardItem()
                     {
                         IP = guard.Primary.IP,
                         ScreenIP = guard.Primary.ScreenIP,
                         ScreenType = guard.Primary.ScreenType
                     }
                 });
                 if (guard.Secondaries != null)
                     guard.Secondaries.ForEach((a) =>
                     {
                         list.Add(new WapperGuardItem()
                         {
                             isPrimary = false,
                             guardItem = new GuardItem()
                              {
                                  IP = a.IP,
                                  ScreenIP = a.ScreenIP,
                                  ScreenType = a.ScreenType
                              }
                         });
                     });
                 if (!dict.ContainsKey(name))
                     dict.Add(name, list);
            }
            dgvData.AutoGenerateColumns = false;
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvData.MultiSelect = false;
            dgvData.RowHeadersVisible = false;
            dgvData.Columns[0].Width = (int)(dgvData.Size.Width * 0.19);
            dgvData.Columns[1].Width = (int)(dgvData.Size.Width * 0.29);
            dgvData.Columns[2].Width = (int)(dgvData.Size.Width * 0.23);
            dgvData.Columns[3].Width = (int)(dgvData.Size.Width * 0.29);
            
        }

        /*
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtImgSavePath.Text = fbd.SelectedPath;
            }

        }
         * */
        private void Save() 
        {
            Params.Settings.ImagePath = txtImgSavePath.Text;
            Params.Settings.InVolume = (int)nudInVol.Value;
            Params.Settings.OutVolume = (int)nudOutVol.Value;
            Params.Settings.InDelay = (int)(nudInDelay.Value * 1000);
            Params.Settings.OutDelay = (int)(nudOutDelay.Value * 1000);
            Params.Settings.InLine = (int)nudInLine.Value;
            Params.Settings.OutLine = (int)nudOutLine.Value;
            Params.Settings.ScreenInDelay = (int)nudScrInDelay.Value;
            Params.Settings.ScreenOutDelay = (int)nudScrOutDelay.Value;
            Params.Settings.EnabledWhiteList = chkWhiteList.Checked;
            Params.Settings.EnabledWhiteListNoOrder = chkIswhiteorder.Checked;
            Params.Settings.EnabledShowLeftCount = LeftCountChk.Checked;
            Params.Settings.EnabledWLGO = WLgochk.Checked;
            Params.Settings.EnabledWhiteListUsed = ISwhiteListchk.Checked;
            Params.Settings.EnabledMonthlyPass = MonthlyPassChb.Checked;
            Params.Settings.EnabledHotel= hotelchk.Checked;
            Params.Settings.EnabledYellowReCalculation = chkYellowCarNum.Checked;
            Params.Settings.EnabledFreeTime = chkFreeTime.Checked;
            Params.Settings.EnabledShowBnt = chkShowBnt.Checked;
            Params.Settings.IsWelcome = cbWelcome.Checked;
            Params.Settings.EnableOneSpaceMoreCars = CBOneSpaceMoreCars.Checked;

            Params.Settings.ParkLot.No = txtParkNo.Text;
            Params.Settings.Serv.Url = txtUrl.Text;
            Params.Settings.Serv.HeartBeatFreq = (int)nudHeartBeatFreq.Value;
            Params.Settings.Serv.HeartBeatMaxRetryCount = (int)nudHeatBeatFail.Value;
            Params.Settings.Serv.PayCheckFreq = ((int)nudCheckPay.Value) * 1000;
            Params.Settings.Serv.PayCheckMaxRetryCount = (int)nudCheckMaxCount.Value;
            Params.Settings.Serv.GateKeepOpenFreq = ((int)nudKeepOpen.Value) * 1000;
            Params.Settings.Serv.DataSyncFreq = ((int)nudData.Value) * 1000 ;
            Params.Settings.MistakenOutSec = ((int)nudOutMistaken.Value) * 1000;
            Params.Settings.MistakenEntranceMin = ((int)nudEntranceMistaken.Value);
            Params.Settings.HourSync = ((int)nudHour.Value);
            Params.Settings.MinSync = ((int)nudMin.Value);
            Params.Settings.IsUpdateBerthNum = updateBerthNumChb.Checked;
            Params.Settings.CheckLeftCountSec = (int)nudCheckLeftCountSec.Value;
            Params.Settings.brokenNetOrderFre = (int)brokenNetOrderFre.Value;
            Params.Settings.apiTimeout = (int)apiTimeout.Value;
            Params.Settings.UnidentifiedTimerMin = (int)nudUnidentifiedTimer.Value;

            Params.Settings.Guards = new List<Guard>();
            foreach (TreeNode node in tvGuard.Nodes)
            {
                Guard guard = new Guard();
                guard.No = node.Text.Replace("(出)", "").Replace("(入)", "");
                guard.IsExit = Convert.ToBoolean(node.Tag);
                GuardItem gi = dict[node.Text].First(a => a.isPrimary).guardItem;
                guard.Primary = gi;

                var where = dict[node.Text].Where(a => !a.isPrimary);
                if (where != null)
                {
                    guard.Secondaries = new List<GuardItem>();
                    List<WapperGuardItem> list = where.ToList();
                    foreach (var i in list)
                    {
                        guard.Secondaries.Add(new GuardItem()
                        {
                            IP = i.guardItem.IP,
                            ScreenIP = i.guardItem.ScreenIP,
                            ScreenType = i.guardItem.ScreenType
                        });
                    }
                }

                Params.Settings.Guards.Add(guard);
            }
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtImgSavePath.Text))
            {
                MessageBox.Show("请选择图片保存路径");
                return;
            }

            if (string.IsNullOrEmpty(txtParkNo.Text))
            {
                MessageBox.Show("请输入本停车场系统编号");
                return;
            }
            if (string.IsNullOrEmpty(txtUrl.Text))
            {
                MessageBox.Show("请输入后台接口url地址");
                return;
            }

            //暂时取消必须一进一出的限制
            // if (tvGuard.Nodes.Count < 2)
            // {
            // MessageBox.Show("请设置至少一进一出");
            //  return;
            //   }
            //  else 
            //   {
            int x = tvGuard.Nodes.Cast<TreeNode>().Count(a => Convert.ToBoolean(a.Tag));
            int y = tvGuard.Nodes.Cast<TreeNode>().Count(a => !Convert.ToBoolean(a.Tag));
            if (x == 0 && y == 0)
            {
                MessageBox.Show("必须要设置出入口");
                return;
            }
            //  if (x == 0 || y == 0) 
            //  {
            //  MessageBox.Show("请设置至少一进一出");
            //  return;
            // }

            foreach (TreeNode n in tvGuard.Nodes) 
              {
                  if ((!dict.ContainsKey(n.Text)) || (dict[n.Text].Count < 1))
                  {
                      MessageBox.Show("请设置"+n.Text+"有关的摄像头配置");
                      return;
                  }
              }
        //    }


            Save();  //赋值到变量

            //疑问
            if (ChangeSettingDele != null)
                ChangeSettingDele();

            //疑问
            ParkSystemBLL psbll = new ParkSystemBLL();
            psbll.SaveConfig(Params.Settings);

            this.Close();
        }

        private void ModifyGuard(bool isExit,string no,TreeNode tnNode) 
        {
            //相同类型的编号或者名称不能重复
            string flag = isExit ? "(出)" : "(入)";
            string name = no + flag;
            bool haved = false;
            foreach (TreeNode node in tvGuard.Nodes) 
            {
                if (node.Text.Equals(name)) 
                {
                    haved = true;
                    break;
                }
            }
            if (haved) 
            {
                MessageBox.Show("相同类型不能存在相同的名称");
                return;
            }
            if (tnNode == null)
            {
                TreeNode tn = new TreeNode(name);
                tn.Tag = isExit;
                tn.ForeColor = !isExit ? Color.Green : Color.Red;
                tvGuard.Nodes.Add(tn);

                dict.Add(name, new List<WapperGuardItem>());
            }
            else 
            {
                foreach (TreeNode node in tvGuard.Nodes) 
                {
                    if (node.Text.Equals(tnNode.Text))
                    {
                        dict.Add(name, dict[tnNode.Text]);
                        dict.Remove(tnNode.Text);
                        tnNode.Tag = isExit;
                        tnNode.ForeColor = !isExit ? Color.Green : Color.Red;
                        tnNode.Text = name;
                        break;
                    }
                }
            }

        }
        

        /// <summary>
        /// 添加出入口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd1_Click(object sender, EventArgs e)
        {
            FrmGuard guard = new FrmGuard();
            guard.ModifyGuardDele = ModifyGuard;
            guard.ShowDialog();
        }

        private void btnEdit1_Click(object sender, EventArgs e)
        {
            TreeNode tn= tvGuard.SelectedNode;
            if (tn == null)
                return;
            FrmGuard guard = new FrmGuard(tn);
            guard.ModifyGuardDele = ModifyGuard;
            guard.ShowDialog();
        }

        private void btnDelete1_Click(object sender, EventArgs e)
        {
            TreeNode tn = tvGuard.SelectedNode;
            if (tn == null)
                return;
            if (MessageBox.Show("删除【"+tn.Text+"】将导致相关的相机配置丢失，确定操作?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) 
            {
                tvGuard.Nodes.Remove(tn);
                dict.Remove(tn.Text);
                dgvData.Rows.Clear();
            }
        }


        private void tvGuard_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CheckNode();
        }
        private void ModifyDgv(string ip, bool isPrimary, int screenType, string screenIP,DataGridViewRow row) 
        {
            string name= tvGuard.SelectedNode.Text;
            List<WapperGuardItem> list=new List<WapperGuardItem>();
            if (dict.ContainsKey(name))
                list = dict[name];

            bool havedIP = false;
            bool havedPrimary = false;
            string pip = "";
            foreach (DataGridViewRow r in dgvData.Rows) 
            {
                if (r.Cells[1].Value.ToString().Equals(ip)) 
                {
                    havedIP = true;
                    break;
                }

            }
            foreach (DataGridViewRow r in dgvData.Rows)
            {
                if (Convert.ToBoolean(r.Cells[0].Value))
                {
                    havedPrimary = true;
                    pip = r.Cells[1].Value.ToString();
                    break;
                }
            
            }


            if (row == null)
            {
                if (havedIP)
                {
                    MessageBox.Show("摄像机IP地址已存在");
                    return;
                }

                if (!havedPrimary)
                {
                    if (!isPrimary)
                    {
                        MessageBox.Show("必须存在一个主相机");
                        return;
                    }

                }
                else 
                {
                    if (isPrimary)
                    {
                        MessageBox.Show("一个出入口只能设置一个主相机");
                        return;
                    }
                }
      
                list.Add(new WapperGuardItem()
                {
                    isPrimary = isPrimary,
                    guardItem = new GuardItem()
                    {
                        IP = ip,
                        ScreenIP = screenIP,
                        ScreenType = screenType
                    }

                });

                int index = dgvData.Rows.Add();
                dgvData.Rows[index].Cells[0].Value = isPrimary;
                dgvData.Rows[index].Cells[1].Value = ip;
                dgvData.Rows[index].Cells[2].Value = screenType == 1 ? "普通" : "红门";
                dgvData.Rows[index].Cells[3].Value = screenIP;
            }
            else
            {
                if (havedIP && ip != row.Cells[1].Value.ToString())
                {
                    MessageBox.Show("摄像机IP地址已存在");
                    return;
                }
                if (havedPrimary && (pip != "" && pip != row.Cells[1].Value.ToString()))
                {
                    MessageBox.Show("一个出入口只能设置一个主相机");
                    return;
                }
                WapperGuardItem wgi= list.First(a => a.guardItem.IP == row.Cells[1].Value.ToString());
                wgi.guardItem.IP = ip;
                wgi.guardItem.ScreenIP = screenIP;
                wgi.isPrimary = isPrimary;
                wgi.guardItem.ScreenType = screenType;

                row.Cells[0].Value = isPrimary;
                row.Cells[1].Value = ip;
                row.Cells[2].Value = screenType == 1 ? "普通" : "红门";
                row.Cells[3].Value = screenIP;
            }



       
        }

        private void btnAdd2_Click(object sender, EventArgs e)
        {
            TreeNode tn = tvGuard.SelectedNode;
            if (tn == null)
                return;
            FrmCamera camera = new FrmCamera();
            camera.ModifyDgvDele = ModifyDgv;
            camera.ShowDialog();
        }


        private void btnEdit2_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count < 1)
                return;
      
            FrmCamera camera = new FrmCamera(dgvData.SelectedRows[0]);
            camera.ModifyDgvDele = ModifyDgv;
            camera.ShowDialog();
        }

        private void btnDelete2_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count < 1)
                return;
            DataGridViewRow row = dgvData.SelectedRows[0];
            TreeNode tn = tvGuard.SelectedNode;
            string ip = row.Cells[1].Value.ToString();
            dict[tn.Text].Remove( dict[tn.Text].First(a=>a.guardItem.IP==ip));
            dgvData.Rows.Remove(row);
        }

        private void CheckNode() 
        {
            TreeNode tn = tvGuard.SelectedNode;
            if (tn == null)
                return;
            string name = tn.Text;
            dgvData.Rows.Clear();
            if (dict.ContainsKey(name))
            {
                List<WapperGuardItem> list = dict[name];
                foreach (var item in list)
                {
                    int index = dgvData.Rows.Add();
                    dgvData.Rows[index].Cells[0].Value = item.isPrimary;
                    dgvData.Rows[index].Cells[1].Value = item.guardItem.IP;
                    dgvData.Rows[index].Cells[2].Value = item.guardItem.ScreenType == 1 ? "普通" : "红门";
                    dgvData.Rows[index].Cells[3].Value = item.guardItem.ScreenIP;
                }
            }
        }

        private void tvGuard_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            CheckNode();
        }

        /// <summary>
        /// 读取默认配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiDefault_Click(object sender, EventArgs e)
        {
            txtImgSavePath.Text = @"D:\";
            nudInDelay.Value = 3;
            nudOutDelay.Value = 3;
            nudInVol.Value = 10;
            nudOutVol.Value = 10;
            nudInLine.Value = 2;
            nudOutLine.Value = 2;
            nudScrInDelay.Value = 80;
            nudScrOutDelay.Value = 80;
            chkWhiteList.Checked = false;
            chkIswhiteorder.Checked = false;
            LeftCountChk.Checked = false;
            WLgochk.Checked = false;
            ISwhiteListchk.Checked = false;
            MonthlyPassChb.Checked = false;
            hotelchk.Checked = false;
            cbWelcome.Checked = false;
            txtParkNo.Text = "0";
            txtUrl.Text ="http://47.100.229.60:8060";
            nudHeartBeatFreq.Value = 3;
            nudHeatBeatFail.Value = 5;
            nudCheckPay.Value = 5;
            nudCheckMaxCount.Value = 20;
            nudKeepOpen.Value = 3;
            nudData.Value = 10;
           
            nudHour.Value = 0;
            nudMin.Value = 1;
            updateBerthNumChb.Checked = false;
            brokenNetOrderFre.Value = 60;
            nudCheckLeftCountSec.Value = 60;
            apiTimeout.Value = 5;
            nudUnidentifiedTimer.Value = 0;

            tvGuard.Nodes.Clear();
            TreeNode tn1 = new TreeNode("默认(入)");
            tn1.Tag = false;
            tn1.ForeColor = Color.Green;
            tvGuard.Nodes.Add(tn1);
            TreeNode tn2 = new TreeNode("默认(出)");
            tn2.Tag = true;
            tn2.ForeColor = Color.Red;
            tvGuard.Nodes.Add(tn2);

            dict.Clear();
            dict.Add("默认(入)", new List<WapperGuardItem>()
            {
               new WapperGuardItem(){ guardItem=new GuardItem()
               {
                   IP="192.168.1.101",
                   ScreenIP="192.168.1.101",
                   ScreenType=1
               },isPrimary=true}
            });

            dict.Add("默认(出)", new List<WapperGuardItem>()
            {
               new WapperGuardItem(){ guardItem=new GuardItem()
               {
                   IP="192.168.1.102",
                   ScreenIP="192.168.1.102",
                   ScreenType=1
               },isPrimary=true}
            });

            dgvData.Rows.Clear();

            Save();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }

        private void Label40_Click(object sender, EventArgs e)
        {

        }
    }

    public class WapperGuardItem 
    {
        public bool isPrimary { get; set; }
        public GuardItem guardItem { get; set; }
    }
}
