namespace ParkSystemWinForms
{
    partial class FrmLoadCfg
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
            this.cboCfg = new System.Windows.Forms.ComboBox();
            this.btnLoading = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "配置项:";
            // 
            // cboCfg
            // 
            this.cboCfg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCfg.FormattingEnabled = true;
            this.cboCfg.Location = new System.Drawing.Point(151, 40);
            this.cboCfg.Name = "cboCfg";
            this.cboCfg.Size = new System.Drawing.Size(178, 26);
            this.cboCfg.TabIndex = 1;
            this.cboCfg.SelectedIndexChanged += new System.EventHandler(this.cboCfg_SelectedIndexChanged);
            // 
            // btnLoading
            // 
            this.btnLoading.Location = new System.Drawing.Point(374, 34);
            this.btnLoading.Name = "btnLoading";
            this.btnLoading.Size = new System.Drawing.Size(95, 37);
            this.btnLoading.TabIndex = 2;
            this.btnLoading.Text = "加载";
            this.btnLoading.UseVisualStyleBackColor = true;
            this.btnLoading.Click += new System.EventHandler(this.btnLoading_Click);
            // 
            // FrmLoadCfg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 135);
            this.Controls.Add(this.btnLoading);
            this.Controls.Add(this.cboCfg);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLoadCfg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "读取配置";
            this.Load += new System.EventHandler(this.FrmLoadCfg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboCfg;
        private System.Windows.Forms.Button btnLoading;
    }
}