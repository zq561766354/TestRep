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
    public partial class SetadvertisingForm : Form
    {
        public delegate void SetAdvertHandler(byte[] message);
        public SetAdvertHandler HandleSetAdvert;

        public SetadvertisingForm()
        {
            InitializeComponent();
        }

        private void setadvertisingbutton_Click(object sender, EventArgs e)
        {

            string advertising = advertisingtextBox.Text;
            if (string.IsNullOrEmpty(advertising.Trim()))
            {
                MessageBox.Show("请输入广告语！");
                return;
            }
            int line =Convert.ToInt32(linecomboBox.Text);
            string color = colorcomboBox.Text;
            int colornum = 1;
            switch (color)
            {
                case "红色":
                    colornum = 1;
                    break;
                case "绿色": 
                    colornum = 2;
                    break;
                case "黄色":
                    colornum = 3;
                    break;
                default:
                    colornum = 1;//8是不在识别的颜色中
                    break;
            }
            byte[] advertisementLine1 = ScreenUtil.SetAdvertisement(advertising, line, colornum);
 
            //2019-02-15修改成与其他弹出窗体一致的处理方式
            if (HandleSetAdvert != null)
                HandleSetAdvert(advertisementLine1);

            this.Dispose();
        }

        private void SetadvertisingForm_Load(object sender, EventArgs e)
        {
            this.linecomboBox.SelectedIndex = 0;
            this.colorcomboBox.SelectedIndex = 0;
            this.Text = "设置广告语";
        }
    }
}
