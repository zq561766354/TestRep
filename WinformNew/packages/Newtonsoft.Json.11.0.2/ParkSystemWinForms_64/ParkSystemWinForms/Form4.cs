using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParkSystemWinForms.Model;

namespace ParkSystemWinForms
{
    public partial class Form4 : Form
    {
        ParkSystemBLL PBLL = new ParkSystemBLL();
        private Form1 f1;

        private string pcColor ;
        private string LicensePlateNo;
        private string PictureAddr;
        private string PictureName;
        private string tmpPictureAddr;
        private string tmpPictureName;
        private string picbase64;
        private DateTime dateTime;


        public Form4(string picth, string Color, string Number, string PicAddr, string PicName, string tmpPicAddr, string tmpPicName, string pb, DateTime date, Form1 f)
        {
           
            InitializeComponent();

            f1 = f;

            pcColor = Color;
            LicensePlateNo = Number;
            PictureAddr = PicAddr;
            PictureName = PicName;
            tmpPictureAddr = tmpPicAddr;
            tmpPictureName = tmpPicName;
            picbase64 = pb;
            dateTime = date;

            //PictureBox pic = new PictureBox();
            //pic.Load(picth);
            //pic.Tag = 1;
            //pic.SizeMode = PictureBoxSizeMode.AutoSize;
          
            //tabPage1.Controls.Add(pic);
            pictureBox1.Load(picth);
            label17.Text = "出口";
            label15.Text = Number;
            label19.Text = date.ToString();

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        ParkSystemBLL PBll = new ParkSystemBLL();
        private void button4_Click(object sender, EventArgs e)
        {
            this.panel3.Controls.Clear();
            List<PicNum> piclist=new List<PicNum>();
            string keyword = textBox2.Text.ToString();
            ImageList imageList = new ImageList();
            piclist = PBll.GetpicList(keyword);
            if (piclist.Count > 0 && piclist!=null)
            {
                for (int i = 0; i < piclist.Count; i++)
                {
                    //imageList.Images.Add(Image.FromFile(piclist[i].picPath));
                    //imageList.ImageSize = new Size(250, 100);
                    //imageList.Tag = piclist[i].OrderId;

                    PictureBox pic = new PictureBox();
                    pic.Load(piclist[i].picPath);
                    pic.Tag = piclist[i].OrderNo;
                    pic.Size = new Size(300, 100);
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    //pic.Height = 100;
                    //listView1.Controls.Add(pic);
                    pic.Location = new Point(10, i* 100);
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

        private void Form4_Load(object sender, EventArgs e)
        {

        }
        private string InOrderNo;
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
            OrderModel orderModel=PBLL.GetOrderDetail(tag);
            InLicensePlateNo.Text = orderModel.LicensePlateNo;
            label2.Text = orderModel.InDate.ToString();
            label11.Text = "入口";
            pictureBox2.Load(orderModel.picPath);
            LicensePlateNo = orderModel.LicensePlateNo;
            InOrderNo = orderModel.OrderNo;


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
                 OrderModel Or = PBLL.SaveOrder(2, pcColor, LicensePlateNo, PictureAddr, PictureName, tmpPictureAddr, tmpPictureName, picbase64, dateTime,0);
                 f1.AssignmentOutshowLableControlWEITUO(LicensePlateNo, Or.InDate, dateTime, Or.ParkingTime, null, Or.ActualAmount);
                 this.Dispose();
             }
          
            //if (Or.actucalAmount == 0)
            //{  ss   
            //    int result = ParkSystem.UpdateState(30, Or.orderNo, Or.actucalAmount);
            //    if (result == 0)
            //    {
            //        MessageBox.Show("订单更新失败，请联系管理员！");
            //    }

            //}
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string LicensePlateNo = InLicensePlateNo.Text.ToString();
            int result = PBLL.UpdateLicensePlateNo(LicensePlateNo, InOrderNo);
            if (result == 0)
            {
                MessageBox.Show("车牌修正失败，请联系管理员！");
            }
            else
            {
                MessageBox.Show("车牌修正成功！");
            }
        }

        private void InLicensePlateNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

    }
}
