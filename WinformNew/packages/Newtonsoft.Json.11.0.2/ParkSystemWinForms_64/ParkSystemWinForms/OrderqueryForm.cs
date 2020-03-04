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
        
 
        public OrderqueryForm()
        {       
            InitializeComponent();          
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
                item1.SubItems.Add(orderList[i].CarColor);
                item1.SubItems.Add(orderList[i].InDate.ToString());
                item1.SubItems.Add(orderList[i].StateDes);
                //item1.SubItems.Add("查看图片");
                item1.SubItems.Add(orderList[i].picPath);
                item1.SubItems.Add(orderList[i].OrderId.ToString());

                listView1.Items.AddRange(new ListViewItem[] { item1 });
            }
        }
        private void listView1_Click(object sender, EventArgs e)
        {
            int selectCount = listView1.SelectedItems.Count; //选中的行数目，listview1是控件名。
            if (selectCount == 0)
                return;//没选中，不做响应
            string picth = listView1.SelectedItems[0].SubItems[6].Text;//第2列
            //MessageBox.Show(sPID);
            if (File.Exists(picth))
                pictureBox1.Load(picth);
        } 

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                //item.Selected = (sender as CheckBox).Checked;
                item.Checked = (sender as CheckBox).Checked;
            }
        }

        private void remove_Click(object sender, EventArgs e)
        {
            int m = listView1.CheckedItems.Count;

            List<int> OrderIdList = new List<int>();
            for (int i = 0; i < m; i++)
            {
                if (listView1.CheckedItems[i].Checked)
                    OrderIdList.Add(Convert.ToInt32(listView1.CheckedItems[i].SubItems[7].Text));

                // MessageBox.Show(listView1.CheckedItems[i].SubItems[1].Text);  
            }
            if (OrderIdList.Count > 0)
            {
                string s = string.Join(",", OrderIdList.ToArray());
                string result= Pbll.deleteOrder(s);
                MessageBox.Show(result);
                search();
                pictureBox1.Image = null;
            }
            else
            {
                MessageBox.Show("请先选择要清除的车辆");
            }
        }
     
    }
}
