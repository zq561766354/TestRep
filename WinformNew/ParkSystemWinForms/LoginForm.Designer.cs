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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.login = new System.Windows.Forms.Button();
            this.passwordInput = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.Label();
            this.usernameInput = new System.Windows.Forms.TextBox();
            this.UserName = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Controls.Add(this.login);
            this.panel1.Controls.Add(this.passwordInput);
            this.panel1.Controls.Add(this.password);
            this.panel1.Controls.Add(this.usernameInput);
            this.panel1.Controls.Add(this.UserName);
            this.panel1.Location = new System.Drawing.Point(0, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(681, 321);
            this.panel1.TabIndex = 0;
            // 
            // login
            // 
            this.login.Location = new System.Drawing.Point(265, 210);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(173, 43);
            this.login.TabIndex = 4;
            this.login.Text = "登陆";
            this.login.UseVisualStyleBackColor = true;
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // passwordInput
            // 
            this.passwordInput.Location = new System.Drawing.Point(265, 146);
            this.passwordInput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.passwordInput.Name = "passwordInput";
            this.passwordInput.PasswordChar = '*';
            this.passwordInput.Size = new System.Drawing.Size(173, 25);
            this.passwordInput.TabIndex = 3;
            // 
            // password
            // 
            this.password.AutoSize = true;
            this.password.BackColor = System.Drawing.Color.Transparent;
            this.password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.password.Location = new System.Drawing.Point(163, 150);
            this.password.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(54, 25);
            this.password.TabIndex = 2;
            this.password.Text = "密码";
            // 
            // usernameInput
            // 
            this.usernameInput.Location = new System.Drawing.Point(265, 92);
            this.usernameInput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.usernameInput.Name = "usernameInput";
            this.usernameInput.Size = new System.Drawing.Size(173, 25);
            this.usernameInput.TabIndex = 1;
            // 
            // UserName
            // 
            this.UserName.AutoSize = true;
            this.UserName.BackColor = System.Drawing.Color.Transparent;
            this.UserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UserName.Location = new System.Drawing.Point(163, 96);
            this.UserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(75, 25);
            this.UserName.TabIndex = 0;
            this.UserName.Text = "用户名";
            // 
            // LoginForm
            // 
            this.AcceptButton = this.login;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 326);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.Text = "登陆";
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