namespace ParkSystemWinForms
{
    partial class OrderqueryForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.remove = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.chkboxAll = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Num = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.carNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.InDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.carColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PicPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.orderId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ChargeTypeDes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StateDes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonXiuzheng = new System.Windows.Forms.Button();
            this.textBoxCarNum = new System.Windows.Forms.TextBox();
            this.checkBoxAll = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pictureBoxsmallcarnum = new System.Windows.Forms.PictureBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.leftBerthNum = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxsmallcarnum)).BeginInit();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 14F);
            this.textBox1.Location = new System.Drawing.Point(195, 15);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(309, 34);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.remove);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Location = new System.Drawing.Point(13, 11);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(870, 60);
            this.panel1.TabIndex = 1;
            // 
            // remove
            // 
            this.remove.BackColor = System.Drawing.Color.Brown;
            this.remove.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.remove.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.remove.Location = new System.Drawing.Point(696, 15);
            this.remove.Margin = new System.Windows.Forms.Padding(4);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(128, 35);
            this.remove.TabIndex = 4;
            this.remove.Text = "标记异常";
            this.remove.UseVisualStyleBackColor = false;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Highlight;
            this.button1.Font = new System.Drawing.Font("宋体", 9F);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(517, 15);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 36);
            this.button1.TabIndex = 3;
            this.button1.Text = "查询";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14F);
            this.label1.Location = new System.Drawing.Point(23, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "请输入车牌：";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chkboxAll,
            this.Num,
            this.carNum,
            this.InDate,
            this.carColor,
            this.ChargeTypeDes,
            this.StateDes,
            this.PicPath,
            this.orderId});
            this.listView1.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(13, 161);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(870, 581);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.Click += new System.EventHandler(this.listView1_Click);
            // 
            // chkboxAll
            // 
            this.chkboxAll.Text = "";
            this.chkboxAll.Width = 30;
            // 
            // Num
            // 
            this.Num.Text = "序号";
            this.Num.Width = 50;
            // 
            // carNum
            // 
            this.carNum.Text = "车牌号";
            this.carNum.Width = 106;
            // 
            // InDate
            // 
            this.InDate.Text = "入场时间";
            this.InDate.Width = 150;
            // 
            // carColor
            // 
            this.carColor.Text = "车牌颜色";
            this.carColor.Width = 102;
            // 
            // PicPath
            // 
            this.PicPath.Text = "图片路径";
            this.PicPath.Width = 0;
            // 
            // orderId
            // 
            this.orderId.Text = "订单ID";
            this.orderId.Width = 0;
            // 
            // ChargeTypeDes
            // 
            this.ChargeTypeDes.DisplayIndex = 7;
            this.ChargeTypeDes.Text = "车辆属性";
            this.ChargeTypeDes.Width = 110;
            // 
            // StateDes
            // 
            this.StateDes.DisplayIndex = 8;
            this.StateDes.Text = "状态";
            this.StateDes.Width = 110;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(908, 83);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(487, 532);
            this.panel2.TabIndex = 3;
            // 
            // buttonXiuzheng
            // 
            this.buttonXiuzheng.BackColor = System.Drawing.SystemColors.Highlight;
            this.buttonXiuzheng.Font = new System.Drawing.Font("宋体", 9F);
            this.buttonXiuzheng.ForeColor = System.Drawing.Color.White;
            this.buttonXiuzheng.Location = new System.Drawing.Point(265, 15);
            this.buttonXiuzheng.Margin = new System.Windows.Forms.Padding(4);
            this.buttonXiuzheng.Name = "buttonXiuzheng";
            this.buttonXiuzheng.Size = new System.Drawing.Size(116, 36);
            this.buttonXiuzheng.TabIndex = 5;
            this.buttonXiuzheng.Text = "修正";
            this.buttonXiuzheng.UseVisualStyleBackColor = false;
            this.buttonXiuzheng.Click += new System.EventHandler(this.buttonXiuzheng_Click);
            // 
            // textBoxCarNum
            // 
            this.textBoxCarNum.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxCarNum.Location = new System.Drawing.Point(16, 15);
            this.textBoxCarNum.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxCarNum.Name = "textBoxCarNum";
            this.textBoxCarNum.Size = new System.Drawing.Size(223, 35);
            this.textBoxCarNum.TabIndex = 0;
            // 
            // checkBoxAll
            // 
            this.checkBoxAll.AutoSize = true;
            this.checkBoxAll.Location = new System.Drawing.Point(22, 171);
            this.checkBoxAll.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxAll.Name = "checkBoxAll";
            this.checkBoxAll.Size = new System.Drawing.Size(18, 17);
            this.checkBoxAll.TabIndex = 4;
            this.checkBoxAll.UseVisualStyleBackColor = true;
            this.checkBoxAll.CheckedChanged += new System.EventHandler(this.checkBoxAll_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.buttonXiuzheng);
            this.panel3.Controls.Add(this.textBoxCarNum);
            this.panel3.Location = new System.Drawing.Point(908, 9);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(389, 66);
            this.panel3.TabIndex = 5;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.pictureBoxsmallcarnum);
            this.panel4.Location = new System.Drawing.Point(908, 641);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(487, 105);
            this.panel4.TabIndex = 4;
            // 
            // pictureBoxsmallcarnum
            // 
            this.pictureBoxsmallcarnum.Location = new System.Drawing.Point(14, 7);
            this.pictureBoxsmallcarnum.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxsmallcarnum.Name = "pictureBoxsmallcarnum";
            this.pictureBoxsmallcarnum.Size = new System.Drawing.Size(460, 94);
            this.pictureBoxsmallcarnum.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxsmallcarnum.TabIndex = 0;
            this.pictureBoxsmallcarnum.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.leftBerthNum);
            this.panel5.Controls.Add(this.label2);
            this.panel5.Location = new System.Drawing.Point(13, 83);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(870, 62);
            this.panel5.TabIndex = 6;
            // 
            // leftBerthNum
            // 
            this.leftBerthNum.AutoSize = true;
            this.leftBerthNum.Font = new System.Drawing.Font("宋体", 14F);
            this.leftBerthNum.Location = new System.Drawing.Point(191, 20);
            this.leftBerthNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.leftBerthNum.Name = "leftBerthNum";
            this.leftBerthNum.Size = new System.Drawing.Size(46, 24);
            this.leftBerthNum.TabIndex = 4;
            this.leftBerthNum.Text = "100";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 14F);
            this.label2.Location = new System.Drawing.Point(23, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "余位数：";
            // 
            // OrderqueryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(500, 360);
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1398, 786);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.checkBoxAll);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderqueryForm";
            this.Text = "查询场内车辆";
            this.Load += new System.EventHandler(this.OrderqueryForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxsmallcarnum)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.CheckBox checkBoxAll;
        private System.Windows.Forms.ColumnHeader chkboxAll;
        private System.Windows.Forms.ColumnHeader Num;
        private System.Windows.Forms.ColumnHeader carNum;
        private System.Windows.Forms.ColumnHeader carColor;
        private System.Windows.Forms.ColumnHeader InDate;
        private System.Windows.Forms.ColumnHeader StateDes;
        private System.Windows.Forms.ColumnHeader PicPath;
        private System.Windows.Forms.ColumnHeader orderId;
        private System.Windows.Forms.Button buttonXiuzheng;
        private System.Windows.Forms.TextBox textBoxCarNum;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox pictureBoxsmallcarnum;
        private System.Windows.Forms.ColumnHeader ChargeTypeDes;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label leftBerthNum;
    }
}