namespace ParkSystemWinForms
{
    partial class LoginForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.UserName = new System.Windows.Forms.Label();
            this.usernameInput = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.Label();
            this.passwordInput = new System.Windows.Forms.TextBox();
            this.login = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.login);
            this.panel1.Controls.Add(this.passwordInput);
            this.panel1.Controls.Add(this.password);
            this.panel1.Controls.Add(this.usernameInput);
            this.panel1.Controls.Add(this.UserName);
            this.panel1.Location = new System.Drawing.Point(0, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(511, 257);
            this.panel1.TabIndex = 0;
            // 
            // UserName
            // 
            this.UserName.AutoSize = true;
            this.UserName.Font = new System.Drawing.Font("隶书", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UserName.Location = new System.Drawing.Point(122, 77);
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(59, 16);
            this.UserName.TabIndex = 0;
            this.UserName.Text = "用户名";
            // 
            // usernameInput
            // 
            this.usernameInput.Location = new System.Drawing.Point(199, 74);
            this.usernameInput.Name = "usernameInput";
            this.usernameInput.Size = new System.Drawing.Size(131, 21);
            this.usernameInput.TabIndex = 1;
            // 
            // password
            // 
            this.password.AutoSize = true;
            this.password.Font = new System.Drawing.Font("隶书", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.password.Location = new System.Drawing.Point(122, 120);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(42, 16);
            this.password.TabIndex = 2;
            this.password.Text = "密码";
            // 
            // passwordInput
            // 
            this.passwordInput.Location = new System.Drawing.Point(199, 117);
            this.passwordInput.Name = "passwordInput";
            this.passwordInput.PasswordChar = '*';
            this.passwordInput.Size = new System.Drawing.Size(131, 21);
            this.passwordInput.TabIndex = 3;
            // 
            // login
            // 
            this.login.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.login.Font = new System.Drawing.Font("隶书", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.login.Location = new System.Drawing.Point(199, 167);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(131, 43);
            this.login.TabIndex = 4;
            this.login.Text = "登陆";
            this.login.UseVisualStyleBackColor = false;
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 261);
            this.Controls.Add(this.panel1);
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox passwordInput;
        private System.Windows.Forms.Label password;
        private System.Windows.Forms.TextBox usernameInput;
        private System.Windows.Forms.Label UserName;
        private System.Windows.Forms.Button login;
    }
}