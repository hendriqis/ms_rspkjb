namespace QIS.DesktopTools
{
    partial class ControlPanel
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
            this.tabOption = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnListener = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDbsName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUID = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtListenFrom = new System.Windows.Forms.TextBox();
            this.tabOption.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabOption
            // 
            this.tabOption.Controls.Add(this.tabPage1);
            this.tabOption.Controls.Add(this.tabPage2);
            this.tabOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabOption.Location = new System.Drawing.Point(0, 35);
            this.tabOption.Name = "tabOption";
            this.tabOption.SelectedIndex = 0;
            this.tabOption.Size = new System.Drawing.Size(379, 204);
            this.tabOption.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtListenFrom);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtPassword);
            this.tabPage1.Controls.Add(this.txtUID);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.txtDbsName);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtServerIP);
            this.tabPage1.Controls.Add(this.btnChange);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(371, 178);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Connection";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server IP";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(371, 178);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "About";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 239);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(379, 51);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.btnListener);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(379, 35);
            this.panel2.TabIndex = 2;
            // 
            // btnListener
            // 
            this.btnListener.BackColor = System.Drawing.Color.Lime;
            this.btnListener.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.btnListener.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnListener.Location = new System.Drawing.Point(277, 6);
            this.btnListener.Name = "btnListener";
            this.btnListener.Size = new System.Drawing.Size(94, 23);
            this.btnListener.TabIndex = 0;
            this.btnListener.Text = "START";
            this.btnListener.UseVisualStyleBackColor = false;
            this.btnListener.Click += new System.EventHandler(this.btnListener_Click);
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(106, 149);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(88, 23);
            this.btnChange.TabIndex = 1;
            this.btnChange.Text = "Change";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(106, 14);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtServerIP.Size = new System.Drawing.Size(176, 21);
            this.txtServerIP.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Database";
            // 
            // txtDbsName
            // 
            this.txtDbsName.Location = new System.Drawing.Point(106, 41);
            this.txtDbsName.Name = "txtDbsName";
            this.txtDbsName.Size = new System.Drawing.Size(176, 21);
            this.txtDbsName.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "User ID";
            // 
            // txtUID
            // 
            this.txtUID.Location = new System.Drawing.Point(106, 68);
            this.txtUID.Name = "txtUID";
            this.txtUID.Size = new System.Drawing.Size(176, 21);
            this.txtUID.TabIndex = 6;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(106, 95);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(176, 21);
            this.txtPassword.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Listen From";
            // 
            // txtListenFrom
            // 
            this.txtListenFrom.Location = new System.Drawing.Point(106, 122);
            this.txtListenFrom.Name = "txtListenFrom";
            this.txtListenFrom.Size = new System.Drawing.Size(176, 21);
            this.txtListenFrom.TabIndex = 10;
            // 
            // ControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 290);
            this.Controls.Add(this.tabOption);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ControlPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MEDINFRAS - Desktop Utility v1.0.0";
            this.tabOption.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabOption;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnListener;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServerIP;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDbsName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtListenFrom;
        private System.Windows.Forms.Label label5;
    }
}