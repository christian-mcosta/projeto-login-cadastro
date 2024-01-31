namespace desenvolvimento_teste
{
    partial class Form1_Login
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
            this.bt_login = new System.Windows.Forms.Button();
            this.bt_cadastro = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_login_senha = new System.Windows.Forms.TextBox();
            this.tb_login_nome = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bt_login
            // 
            this.bt_login.Location = new System.Drawing.Point(86, 214);
            this.bt_login.Name = "bt_login";
            this.bt_login.Size = new System.Drawing.Size(75, 23);
            this.bt_login.TabIndex = 0;
            this.bt_login.Text = "Login";
            this.bt_login.UseVisualStyleBackColor = true;
            this.bt_login.Click += new System.EventHandler(this.bt_login_Click);
            // 
            // bt_cadastro
            // 
            this.bt_cadastro.Location = new System.Drawing.Point(181, 214);
            this.bt_cadastro.Name = "bt_cadastro";
            this.bt_cadastro.Size = new System.Drawing.Size(75, 23);
            this.bt_cadastro.TabIndex = 1;
            this.bt_cadastro.Text = "Cadastro";
            this.bt_cadastro.UseVisualStyleBackColor = true;
            this.bt_cadastro.Click += new System.EventHandler(this.bt_cadastro_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Usuario:";
            // 
            // tb_login_senha
            // 
            this.tb_login_senha.Location = new System.Drawing.Point(138, 151);
            this.tb_login_senha.MaxLength = 20;
            this.tb_login_senha.Name = "tb_login_senha";
            this.tb_login_senha.PasswordChar = '*';
            this.tb_login_senha.Size = new System.Drawing.Size(100, 20);
            this.tb_login_senha.TabIndex = 3;
            // 
            // tb_login_nome
            // 
            this.tb_login_nome.Location = new System.Drawing.Point(138, 115);
            this.tb_login_nome.Name = "tb_login_nome";
            this.tb_login_nome.Size = new System.Drawing.Size(100, 20);
            this.tb_login_nome.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(97, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Senha:";
            // 
            // Form1_Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(337, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_login_nome);
            this.Controls.Add(this.tb_login_senha);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bt_cadastro);
            this.Controls.Add(this.bt_login);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1_Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tela Login";
            this.Load += new System.EventHandler(this.Form1_Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bt_login;
        private System.Windows.Forms.Button bt_cadastro;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_login_senha;
        private System.Windows.Forms.TextBox tb_login_nome;
        private System.Windows.Forms.Label label2;
    }
}