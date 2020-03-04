namespace ParkSystemWinForms
{
    partial class FrmCamera
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbtnPrimary = new System.Windows.Forms.RadioButton();
            this.rbtnSecondary = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.cboScreenType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtScreenIP = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP地址：";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(151, 29);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(276, 28);
            this.txtIP.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(96, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "类型：";
            // 
            // rbtnPrimary
            // 
            this.rbtnPrimary.AutoSize = true;
            this.rbtnPrimary.Location = new System.Drawing.Point(164, 74);
            this.rbtnPrimary.Name = "rbtnPrimary";
            this.rbtnPrimary.Size = new System.Drawing.Size(87, 22);
            this.rbtnPrimary.TabIndex = 3;
            this.rbtnPrimary.TabStop = true;
            this.rbtnPrimary.Text = "主相机";
            this.rbtnPrimary.UseVisualStyleBackColor = true;
            // 
            // rbtnSecondary
            // 
            this.rbtnSecondary.AutoSize = true;
            this.rbtnSecondary.Location = new System.Drawing.Point(257, 74);
            this.rbtnSecondary.Name = "rbtnSecondary";
            this.rbtnSecondary.Size = new System.Drawing.Size(87, 22);
            this.rbtnSecondary.TabIndex = 4;
            this.rbtnSecondary.TabStop = true;
            this.rbtnSecondary.Text = "副相机";
            this.rbtnSecondary.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "屏幕类型：";
            // 
            // cboScreenType
            // 
            this.cboScreenType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScreenType.FormattingEnabled = true;
            this.cboScreenType.Location = new System.Drawing.Point(151, 113);
            this.cboScreenType.Name = "cboScreenType";
            this.cboScreenType.Size = new System.Drawing.Size(121, 26);
            this.cboScreenType.TabIndex = 6;
            this.cboScreenType.SelectedIndexChanged += new System.EventHandler(this.cboScreenType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(78, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "屏幕IP：";
            // 
            // txtScreenIP
            // 
            this.txtScreenIP.Location = new System.Drawing.Point(151, 163);
            this.txtScreenIP.Name = "txtScreenIP";
            this.txtScreenIP.Size = new System.Drawing.Size(276, 28);
            this.txtScreenIP.TabIndex = 8;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(209, 215);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(119, 55);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FrmCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 300);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtScreenIP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboScreenType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rbtnSecondary);
            this.Controls.Add(this.rbtnPrimary);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCamera";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "相机配置";
            this.Load += new System.EventHandler(this.FrmCamera_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbtnPrimary;
        private System.Windows.Forms.RadioButton rbtnSecondary;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboScreenType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtScreenIP;
        private System.Windows.Forms.Button btnOk;
    }
}