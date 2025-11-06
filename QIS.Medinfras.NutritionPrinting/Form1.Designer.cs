namespace QIS.Medinfras.NutritionPrinting
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tmrPolling = new System.Windows.Forms.Timer(this.components);
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtDiagnosa = new System.Windows.Forms.TextBox();
            this.txtTglJamOrder = new System.Windows.Forms.TextBox();
            this.txtNoOrder = new System.Windows.Forms.TextBox();
            this.txtMealTime = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRuang = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNoTTKelas = new System.Windows.Forms.TextBox();
            this.lblDtID = new System.Windows.Forms.Label();
            this.lblPrintCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNamaDiit = new System.Windows.Forms.TextBox();
            this.txtAgama = new System.Windows.Forms.TextBox();
            this.lblKdLayan = new System.Windows.Forms.Label();
            this.lblNoReg = new System.Windows.Forms.Label();
            this.txtNamaPasien = new System.Windows.Forms.TextBox();
            this.txtRegistrasi = new System.Windows.Forms.TextBox();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.cmsNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.cmsNotifyIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrPolling
            // 
            this.tmrPolling.Interval = 10000;
            this.tmrPolling.Tick += new System.EventHandler(this.tmPolling_Tick);
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 446);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(416, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(44, 17);
            this.lblStatus.Text = "Started";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtDiagnosa);
            this.groupBox1.Controls.Add(this.txtTglJamOrder);
            this.groupBox1.Controls.Add(this.txtNoOrder);
            this.groupBox1.Controls.Add(this.txtMealTime);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtRemarks);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtRuang);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtNoTTKelas);
            this.groupBox1.Controls.Add(this.lblDtID);
            this.groupBox1.Controls.Add(this.lblPrintCount);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtNamaDiit);
            this.groupBox1.Controls.Add(this.txtAgama);
            this.groupBox1.Controls.Add(this.lblKdLayan);
            this.groupBox1.Controls.Add(this.lblNoReg);
            this.groupBox1.Controls.Add(this.txtNamaPasien);
            this.groupBox1.Controls.Add(this.txtRegistrasi);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(391, 382);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Informasi Gizi";
            // 
            // txtDiagnosa
            // 
            this.txtDiagnosa.Location = new System.Drawing.Point(92, 282);
            this.txtDiagnosa.Multiline = true;
            this.txtDiagnosa.Name = "txtDiagnosa";
            this.txtDiagnosa.ReadOnly = true;
            this.txtDiagnosa.Size = new System.Drawing.Size(291, 54);
            this.txtDiagnosa.TabIndex = 23;
            // 
            // txtTglJamOrder
            // 
            this.txtTglJamOrder.Location = new System.Drawing.Point(92, 256);
            this.txtTglJamOrder.Name = "txtTglJamOrder";
            this.txtTglJamOrder.ReadOnly = true;
            this.txtTglJamOrder.Size = new System.Drawing.Size(231, 20);
            this.txtTglJamOrder.TabIndex = 23;
            // 
            // txtNoOrder
            // 
            this.txtNoOrder.Location = new System.Drawing.Point(92, 230);
            this.txtNoOrder.Name = "txtNoOrder";
            this.txtNoOrder.ReadOnly = true;
            this.txtNoOrder.Size = new System.Drawing.Size(231, 20);
            this.txtNoOrder.TabIndex = 23;
            // 
            // txtMealTime
            // 
            this.txtMealTime.Location = new System.Drawing.Point(92, 152);
            this.txtMealTime.Name = "txtMealTime";
            this.txtMealTime.ReadOnly = true;
            this.txtMealTime.Size = new System.Drawing.Size(148, 20);
            this.txtMealTime.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 155);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Jam Makan";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Location = new System.Drawing.Point(92, 178);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ReadOnly = true;
            this.txtRemarks.Size = new System.Drawing.Size(293, 20);
            this.txtRemarks.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 181);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Keterangan";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 259);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Tgl / Jam Order";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 233);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "No Order";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 207);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Ruang";
            // 
            // txtRuang
            // 
            this.txtRuang.Location = new System.Drawing.Point(92, 204);
            this.txtRuang.Name = "txtRuang";
            this.txtRuang.ReadOnly = true;
            this.txtRuang.Size = new System.Drawing.Size(148, 20);
            this.txtRuang.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "No TT/ Kelas";
            // 
            // txtNoTTKelas
            // 
            this.txtNoTTKelas.Location = new System.Drawing.Point(92, 48);
            this.txtNoTTKelas.Name = "txtNoTTKelas";
            this.txtNoTTKelas.ReadOnly = true;
            this.txtNoTTKelas.Size = new System.Drawing.Size(231, 20);
            this.txtNoTTKelas.TabIndex = 16;
            // 
            // lblDtID
            // 
            this.lblDtID.AutoSize = true;
            this.lblDtID.Location = new System.Drawing.Point(307, 352);
            this.lblDtID.Name = "lblDtID";
            this.lblDtID.Size = new System.Drawing.Size(35, 13);
            this.lblDtID.TabIndex = 14;
            this.lblDtID.Text = "label3";
            this.lblDtID.Visible = false;
            // 
            // lblPrintCount
            // 
            this.lblPrintCount.AutoSize = true;
            this.lblPrintCount.Location = new System.Drawing.Point(7, 352);
            this.lblPrintCount.Name = "lblPrintCount";
            this.lblPrintCount.Size = new System.Drawing.Size(84, 13);
            this.lblPrintCount.TabIndex = 13;
            this.lblPrintCount.Text = "Last Printed By: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Nama Diit";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Agama";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Pasien";
            // 
            // txtNamaDiit
            // 
            this.txtNamaDiit.Location = new System.Drawing.Point(92, 126);
            this.txtNamaDiit.Name = "txtNamaDiit";
            this.txtNamaDiit.ReadOnly = true;
            this.txtNamaDiit.Size = new System.Drawing.Size(293, 20);
            this.txtNamaDiit.TabIndex = 8;
            // 
            // txtAgama
            // 
            this.txtAgama.Location = new System.Drawing.Point(92, 100);
            this.txtAgama.Name = "txtAgama";
            this.txtAgama.ReadOnly = true;
            this.txtAgama.Size = new System.Drawing.Size(148, 20);
            this.txtAgama.TabIndex = 7;
            // 
            // lblKdLayan
            // 
            this.lblKdLayan.AutoSize = true;
            this.lblKdLayan.Location = new System.Drawing.Point(348, 352);
            this.lblKdLayan.Name = "lblKdLayan";
            this.lblKdLayan.Size = new System.Drawing.Size(35, 13);
            this.lblKdLayan.TabIndex = 2;
            this.lblKdLayan.Text = "label3";
            this.lblKdLayan.Visible = false;
            // 
            // lblNoReg
            // 
            this.lblNoReg.AutoSize = true;
            this.lblNoReg.Location = new System.Drawing.Point(6, 25);
            this.lblNoReg.Name = "lblNoReg";
            this.lblNoReg.Size = new System.Drawing.Size(83, 13);
            this.lblNoReg.TabIndex = 1;
            this.lblNoReg.Text = "No Reg/No RM";
            // 
            // txtNamaPasien
            // 
            this.txtNamaPasien.Location = new System.Drawing.Point(92, 74);
            this.txtNamaPasien.Name = "txtNamaPasien";
            this.txtNamaPasien.ReadOnly = true;
            this.txtNamaPasien.Size = new System.Drawing.Size(293, 20);
            this.txtNamaPasien.TabIndex = 6;
            // 
            // txtRegistrasi
            // 
            this.txtRegistrasi.Location = new System.Drawing.Point(92, 22);
            this.txtRegistrasi.Name = "txtRegistrasi";
            this.txtRegistrasi.ReadOnly = true;
            this.txtRegistrasi.Size = new System.Drawing.Size(231, 20);
            this.txtRegistrasi.TabIndex = 5;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(285, 420);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(118, 23);
            this.btnStartStop.TabIndex = 9;
            this.btnStartStop.Text = "Stop Fetching";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // cmsNotifyIcon
            // 
            this.cmsNotifyIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeProgramToolStripMenuItem});
            this.cmsNotifyIcon.Name = "cmsNotifyIcon";
            this.cmsNotifyIcon.Size = new System.Drawing.Size(153, 26);
            this.cmsNotifyIcon.Text = "Close Program";
            // 
            // closeProgramToolStripMenuItem
            // 
            this.closeProgramToolStripMenuItem.Name = "closeProgramToolStripMenuItem";
            this.closeProgramToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeProgramToolStripMenuItem.Text = "Close Program";
            this.closeProgramToolStripMenuItem.Click += new System.EventHandler(this.closeProgramToolStripMenuItem_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.cmsNotifyIcon;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Medinfras Nutrition Printing";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 285);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Diagnosa";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 468);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Medinfras Nutrition Printing";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.cmsNotifyIcon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrPolling;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblDtID;
        private System.Windows.Forms.Label lblPrintCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNamaDiit;
        private System.Windows.Forms.TextBox txtAgama;
        private System.Windows.Forms.Label lblKdLayan;
        private System.Windows.Forms.Label lblNoReg;
        private System.Windows.Forms.TextBox txtNamaPasien;
        private System.Windows.Forms.TextBox txtRegistrasi;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRuang;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNoTTKelas;
        private System.Windows.Forms.TextBox txtMealTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.ContextMenuStrip cmsNotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem closeProgramToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.TextBox txtTglJamOrder;
        private System.Windows.Forms.TextBox txtNoOrder;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDiagnosa;
        private System.Windows.Forms.Label label10;
    }
}

