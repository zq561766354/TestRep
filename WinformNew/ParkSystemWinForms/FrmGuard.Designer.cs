namespace ParkSystemWinForms
{
    partial class FrmGuard
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
            this.rbtnIn = new System.Windows.Forms.RadioButton();
            this.rbtnOut = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNo = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "类型:";
            // 
            // rbtnIn
            // 
            this.rbtnIn.AutoSize = true;
            this.rbtnIn.Location = new System.Drawing.Point(71, 52);
            this.rbtnIn.Name = "rbtnIn";
            this.rbtnIn.Size = new System.Drawing.Size(69, 22);
            this.rbtnIn.TabIndex = 1;
            this.rbtnIn.TabStop = true;
            this.rbtnIn.Text = "入口";
            this.rbtnIn.UseVisualStyleBackColor = true;
            // 
            // rbtnOut
            // 
            this.rbtnOut.AutoSize = true;
            this.rbtnOut.Location = new System.Drawing.Point(146, 52);
            this.rbtnOut.Name = "rbtnOut";
            this.rbtnOut.Size = new System.Drawing.Size(69, 22);
            this.rbtnOut.TabIndex = 2;
            this.rbtnOut.TabStop = true;
            this.rbtnOut.Text = "出口";
            this.rbtnOut.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(237, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "名称:";
            // 
            // txtNo
            // 
            this.txtNo.Location = new System.Drawing.Point(296, 51);
            this.txtNo.Name = "txtNo";
            this.txtNo.Size = new System.Drawing.Size(133, 28);
            this.txtNo.TabIndex = 4;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(507, 28);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(86, 62);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FrmGuard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 122);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbtnOut);
            this.Controls.Add(this.rbtnIn);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGuard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "出入口";
            this.Load += new System.EventHandler(this.FrmGuard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbtnIn;
        private System.Windows.Forms.RadioButton rbtnOut;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNo;
        private System.Windows.Forms.Button btnOk;
    }
}