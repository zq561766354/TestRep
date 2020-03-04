using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bealead.ICEIPC;
using Bealead.ICEIPC.Events;
using ParkSystemWinForms.Model;

namespace ParkSystemWinForms
{
    public partial class FrmModifyPlate : Form
    {
        private static LogHelper log = LogFactory.GetLogger("PsFormLog");
        ParkSystemBLL PBLL = new ParkSystemBLL();

        public delegate void CarOutHandler(IPC ipc, Guard guard, HandleOrder curOrder, string plateColor, string plateNumber, string sightPath, string sightName, string platePath, string plateName, string imgbase64, DateTime captureDate, string inImgPath);
        public CarOutHandler CarOutDele;
        public delegate void NoneConfirmHandle(Guard guard,IPC ipc,int type);
        public NoneConfirmHandle NoneConfirmDele;

        public HandleOrder CurOrder;
        private Guard guard;
        public IPC Ipc;

        private string pcColor ;
        private string LicensePlateNo;
        private string PictureAddr;
        private string PictureName;
        private string tmpPictureAddr;
        private string tmpPictureName;
        private string picbase64;
        private DateTime dateTime;
        private string picOutPath;

        public FrmModifyPlate() 
        {
            InitializeComponent();
            this.ControlBox = false;
        }
        public FrmModifyPlate(string picth, string Color, string Number, string PicAddr, string PicName, string tmpPicAddr, string tmpPicName, string pb, DateTime date,Guard guard)
        {
            InitializeComponent();
            this.ControlBox = false;
   
            pcColor = Color;
            LicensePlateNo = Number;
            PictureAddr = PicAddr;
            PictureName = PicName;
            tmpPictureAddr = tmpPicAddr;
            tmpPictureName = tmpPicName;
            picbase64 = pb;
            dateTime = date;
            picOutPath = picth;
            //PictureBox pic = new PictureBox();
            //pic.Load(picth);
            //pic.Tag = 1;
            //pic.SizeMode = PictureBoxSizeMode.AutoSize;

            //tabPage1.Controls.Add(pic);
            pictureBox1.Load(picth);
            label17.Text = "出口";
            label15.Text = Number;
            label19.Text = date.ToString();

            this.guard = guard;
            if (this.guard == null)
            {
                log.Debug("guard is null");
            }
            else
            {
                this.Text = (this.guard.IsExit ? "出口" : "入口") + "_" + this.guard.No;
            }
          
        }

        ParkSystemBLL PBll = new ParkSystemBLL();
        private void button4_Click(object sender, EventArgs e)
        {
            this.panel3.Controls.Clear();
            List<PicNum> piclist = new List<PicNum>();
            piclist=search();
            if (piclist.Count > 0 && piclist != null)
            {
                for (int i = 0; i < piclist.Count; i++)
                {
                    PictureBox pic = new PictureBox();
                    pic.Load(piclist[i].picPath);
                    pic.Tag = piclist[i].OrderNo;
                    pic.Size = new Size(300, 100);
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    //pic.Height = 100;
                    //listView1.Controls.Add(pic);
                    pic.Location = new Point(10, i * 100);
                    pic.Click += new System.EventHandler(this.pictureBox_Click);//添加Click事件
                    this.panel3.Controls.Add(pic);
                }
            }
            else
            {
                Label lable = new Label();
                lable.Text = "无数据";
                this.panel3.Controls.Add(lable);
            }
        }
        private List<PicNum> search()
        {
         
            List<PicNum> piclist = new List<PicNum>();
            //string keyword = textBox2.Text.ToString();
            string keyword = "";
            ImageList imageList = new ImageList();
            piclist = PBll.GetpicList(keyword);
            StringCompute stringcompute1 = new StringCompute();
            if (piclist.Count > 0 && piclist != null)
            {
                for (int i = 0; i < piclist.Count; i++)
                {
                    stringcompute1.SpeedyCompute(LicensePlateNo, piclist[i].LicensePlateNo);    // 计算相似度
                    decimal rate = stringcompute1.ComputeResult.Rate;    // 相似度百分之几，完全匹配相似度为1
                    piclist[i].similarity = rate;
                }
                piclist.Sort(delegate(PicNum p1, PicNum p2) { return p2.similarity.CompareTo(p1.similarity); });

            }
            return piclist;
           
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            
            this.panel3.Controls.Clear();
            List<PicNum> piclist = new List<PicNum>();
            piclist = search();  
            if (piclist.Count > 0 && piclist != null)
            {
                int len = piclist.Count >= 3 ? 3 : piclist.Count;
                for (int i = 0; i < len; i++)
                {
                    string imgPath=piclist[i].picPath;
                    if (!System.IO.File.Exists(imgPath))
                        continue;
                    PictureBox pic = new PictureBox();
                    pic.Load(imgPath);
                    pic.Tag = piclist[i].OrderNo;
                    pic.Size = new Size(300, 100);
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    //pic.Height = 100;
                    //listView1.Controls.Add(pic);
                    pic.Location = new Point(10, i * 100);
                    pic.Click += new System.EventHandler(this.pictureBox_Click);//添加Click事件
                    this.panel3.Controls.Add(pic);
                }
                if (Convert.ToDouble(piclist[0].similarity)>0.8)
                    InShow(piclist[0].OrderNo);
            }
            else
            {
                Label lable = new Label();
                lable.Text = "无数据";
                this.panel3.Controls.Add(lable);
            }
        }
        private string InOrderNo=null;
        private string storePicPathIn;
        /// <summary>
        /// 点击图片按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;//定义鼠标当前点击picureBox的行为
            if (pic == null)
            {
                return;
            }
            string tag = pic.Tag.ToString();
            //MessageBox.Show(tag);//显示每一个图片位置编号,其他的功能根据自己需要扩展
            //this.groupBox.Refresh();
            InShow(tag);

        }
        private void InShow(string orderNo)
        {
            OrderModel orderModel = PBLL.GetOrderDetail(orderNo);
            InLicensePlateNo.Text = orderModel.LicensePlateNo;
            label2.Text = orderModel.InDate.ToString();
            label11.Text = "入口";
            pictureBox2.Load(orderModel.picPath);
            LicensePlateNo = orderModel.LicensePlateNo;
            InOrderNo = orderModel.OrderNo;
            storePicPathIn = orderModel.picPath;
        }
        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
             LicensePlateNo = InLicensePlateNo.Text.ToString();
             if (LicensePlateNo == "")
             {
                 MessageBox.Show("请选手动选择入场车辆！");
             }
             else
             {
                //2019-02-26原先逻辑进行注释修改
                if (CarOutDele != null)
                {
                    this.Close();
                    this.Dispose();

                    CarOutDele(Ipc, this.guard, CurOrder, pcColor, LicensePlateNo, PictureAddr, PictureName, tmpPictureAddr, tmpPictureName, picbase64, dateTime, storePicPathIn);
                }
             }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (InOrderNo == null)
            {
                MessageBox.Show("请选手动选择入场车辆");
                return;
            }
            string LicensePlateNo = InLicensePlateNo.Text.ToString();
            string result = PBLL.UpdateLicensePlateNo(LicensePlateNo, InOrderNo);
            MessageBox.Show(result);
        }

        private void InLicensePlateNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (Ipc != null)
            {
                this.Close();
                this.Dispose();

                if (NoneConfirmDele != null)
                    NoneConfirmDele(this.guard,Ipc, 0);

            }
               

        }

        private void freebutton_Click(object sender, EventArgs e)
        {
            
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
                
        }

        private void btnPass_Click(object sender, EventArgs e)
        {
            if (Ipc != null)
            {
                this.Close();
                this.Dispose();

                if (NoneConfirmDele != null)
                    NoneConfirmDele(this.guard,Ipc, 1);
            }
        }
    }
}
