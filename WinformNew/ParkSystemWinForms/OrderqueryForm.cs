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
using System.IO;

namespace ParkSystemWinForms
{
    public partial class OrderqueryForm : Form
    {
        private string xiuzhengOrderNo;
        private int chargeEmp=Params.noBodyEmpId;
        public OrderqueryForm(int empid)
        {       
            InitializeComponent();
            chargeEmp = empid;
        }
        private void OrderqueryForm_Load(object sender, EventArgs e)
        {
            //2019-02-15只有有人操作时才允许除"查询"以外的操作
            bool canEdit = !(Params.User.StaffId == Params.noBodyEmpId.ToString());
            if (!canEdit)
            {
                ControlHelper.SetControlEnabled(false, buttonXiuzheng, remove);
                buttonXiuzheng.BackColor = remove.BackColor = Color.Gray;
                buttonXiuzheng.ForeColor = remove.ForeColor = Color.White;
            }
            this.listView1.Scrollable = true;
            //
            int num = Pbll.getLeftCount();
            leftBerthNum.Text = num.ToString();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        ParkSystemBLL Pbll = new ParkSystemBLL();
        private Button btn = new Button();
        private void button1_Click(object sender, EventArgs e)
        {
            
            search();
           // listView1.Columns["checkboxAll"].Width =-1;//根据内容设置宽度
           // listView1.Columns["Num"].Width = 30;//根据内容设置宽度
           // listView1.Columns["carNum"].Width = 80;//根据内容设置宽度
           // listView1.Columns["carColor"].Width = 70;//根据内容设置宽度
           // listView1.Columns["InDate"].Width = 140;//根据内容设置宽度
           // listView1.Columns["StateDes"].Width = 100;//根据内容设置宽度
           //// listView1.Columns["Pic"].Width = 100;//根据内容设置宽度
           // listView1.Columns["PicPath"].Width = 0;//根据内容设置宽度
          
        }
        private void search()
        {
            listView1.Items.Clear();
            string keyword = textBox1.Text.ToString();
            List<OrderModel> orderList = new List<OrderModel>();
            orderList = Pbll.GetOrderList(keyword);
          
            for (int i = 0; i < orderList.Count; i++)
            {
                listView1.View = View.Details;
                listView1.FullRowSelect = true;
                ListViewItem item1 = new ListViewItem();
                item1.SubItems.Add((i + 1).ToString());
                item1.SubItems.Add(orderList[i].LicensePlateNo);
                item1.SubItems.Add(orderList[i].InDate.ToString("yyyy-MM-dd HH:mm:ss"));
                item1.SubItems.Add(orderList[i].CarColor);
                item1.SubItems.Add(orderList[i].ChargeTypeDes.ToString());
                item1.SubItems.Add(orderList[i].StateDes);
                //item1.SubItems.Add("查看图片");
                item1.SubItems.Add(orderList[i].OrderNo.ToString());
                
                listView1.Items.AddRange(new ListViewItem[] { item1 });
            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            this.panel2.Controls.Clear();
            int selectCount = listView1.SelectedItems.Count; //选中的行数目，listview1是控件名。
            if (selectCount == 0)
                return;//没选中，不做响应
            string OrderNo = listView1.SelectedItems[0].SubItems[7].Text;//第6列
            string carNum = listView1.SelectedItems[0].SubItems[2].Text;//第2列
            textBoxCarNum.Text = carNum;
            xiuzhengOrderNo = OrderNo;
            List<OrderModel> piclist = new List<OrderModel>();
            piclist = Pbll.GetOrderPicList(OrderNo);
            if (piclist.Count > 0 && piclist != null)
            {
                for (int i = 0; i < piclist.Count; i++)
                {
                    //imageList.Images.Add(Image.FromFile(piclist[i].picPath));
                    //imageList.ImageSize = new Size(250, 100);
                    //imageList.Tag = piclist[i].OrderId;

                    //2019-02-15增加图片路径判断只有存在时才加载图片
                    string imgPath = piclist[i].picPath;
                    if (System.IO.File.Exists(imgPath))
                    {
                        if (piclist[i].State == 1)
                        {
                            PictureBox pic = new PictureBox();
                            pic.Load(imgPath);
                            pic.Tag = piclist[i].OrderNo;
                            pic.Size = new Size(400, 250);
                            pic.SizeMode = PictureBoxSizeMode.StretchImage;
                            //pic.Height = 100;
                            //listView1.Controls.Add(pic);
                            pic.Location = new Point(10, i * 250);
                            // pic.Click += new System.EventHandler(this.pictureBox_Click);//添加Click事件
                            this.panel2.Controls.Add(pic);

                        }
                        else
                        {
                            pictureBoxsmallcarnum.Load(imgPath);

                        }
                    }
                }
            }
            else
            {
                Label lable = new Label();
                lable.Text = "无数据";

                this.panel2.Controls.Add(lable);
            }
        } 

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = (sender as CheckBox).Checked;
            }
        }

        /// <summary>
        /// 标记车牌号码异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remove_Click(object sender, EventArgs e)
        {
            //2019-02-15由于界面进入的时候按钮能否使用已经有了判断，因此以下逻辑进行了注释
            //if (chargeEmp != Params.noBodyEmpId)
            // {
            int m = listView1.CheckedItems.Count;

            List<string> OrderNoList = new List<string>();
            for (int i = 0; i < m; i++)
            {
                if (listView1.CheckedItems[i].Checked)
                    OrderNoList.Add(listView1.CheckedItems[i].SubItems[7].Text.ToString());
            }
            if (OrderNoList.Count > 0)
            {
                string s = string.Join(",", OrderNoList.ToArray());
                int dataType = 1;
                string result = Pbll.deleteOrder(s, dataType, chargeEmp);
                MessageBox.Show(result);
                search();
                //pictureBox1.Image = null;
                this.panel2.Controls.Clear();
            }
            else
            {
                MessageBox.Show("请先选择要清除的车辆");
            }
            // }
            // else
            // {
            //   MessageBox.Show("请收费员登陆后再操作！");
            //  }

        }

        /// <summary>
        /// 修正车牌号码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonXiuzheng_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(xiuzhengOrderNo))
            {
                MessageBox.Show("请在左侧选择需要修正的车牌数据！");
                return;
            }
            if (string.IsNullOrEmpty(textBoxCarNum.Text.ToString().Trim()))
            {
                MessageBox.Show("请输入修正的车牌号码！");
                return;
            }
            string LicensePlateNo = textBoxCarNum.Text.ToString();
            string result = Pbll.UpdateLicensePlateNo(LicensePlateNo, xiuzhengOrderNo);
            MessageBox.Show(result);
        }


    }
}
