namespace QIS.Medinfras.PrescriptionPrinting
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
            this.lblOrder = new System.Windows.Forms.Label();
            this.lblNoReg = new System.Windows.Forms.Label();
            this.lblKdLayan = new System.Windows.Forms.Label();
            this.tmrPolling = new System.Windows.Forms.Timer(this.components);
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.txtOrder = new System.Windows.Forms.TextBox();
            this.txtRegistrasi = new System.Windows.Forms.TextBox();
            this.txtNamaPasien = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblNoJO = new System.Windows.Forms.Label();
            this.lblPrintCount = new System.Windows.Forms.Label();
            this.txtResep = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.txtDokter = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsNotifyIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.cmsNotifyIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblOrder
            // 
            this.lblOrder.AutoSize = true;
            this.lblOrder.Location = new System.Drawing.Point(6, 22);
            this.lblOrder.Name = "lblOrder";
            this.lblOrder.Size = new System.Drawing.Size(33, 13);
            this.lblOrder.TabIndex = 0;
            this.lblOrder.Text = "Order";
            // 
            // lblNoReg
            // 
            this.lblNoReg.AutoSize = true;
            this.lblNoReg.Location = new System.Drawing.Point(6, 48);
            this.lblNoReg.Name = "lblNoReg";
            this.lblNoReg.Size = new System.Drawing.Size(83, 13);
            this.lblNoReg.TabIndex = 1;
            this.lblNoReg.Text = "No Reg/No RM";
            // 
            // lblKdLayan
            // 
            this.lblKdLayan.AutoSize = true;
            this.lblKdLayan.Location = new System.Drawing.Point(350, 262);
            this.lblKdLayan.Name = "lblKdLayan";
            this.lblKdLayan.Size = new System.Drawing.Size(35, 13);
            this.lblKdLayan.TabIndex = 2;
            this.lblKdLayan.Text = "label3";
            this.lblKdLayan.Visible = false;
            // 
            // tmrPolling
            // 
            this.tmrPolling.Interval = 10000;
            this.tmrPolling.Tick += new System.EventHandler(this.tmrPolling_Tick);
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(285, 302);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(118, 23);
            this.btnStartStop.TabIndex = 3;
            this.btnStartStop.Text = "Stop Fetching";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // txtOrder
            // 
            this.txtOrder.Location = new System.Drawing.Point(92, 19);
            this.txtOrder.Name = "txtOrder";
            this.txtOrder.ReadOnly = true;
            this.txtOrder.Size = new System.Drawing.Size(231, 20);
            this.txtOrder.TabIndex = 4;
            // 
            // txtRegistrasi
            // 
            this.txtRegistrasi.Location = new System.Drawing.Point(92, 45);
            this.txtRegistrasi.Name = "txtRegistrasi";
            this.txtRegistrasi.ReadOnly = true;
            this.txtRegistrasi.Size = new System.Drawing.Size(231, 20);
            this.txtRegistrasi.TabIndex = 5;
            // 
            // txtNamaPasien
            // 
            this.txtNamaPasien.Location = new System.Drawing.Point(92, 71);
            this.txtNamaPasien.Name = "txtNamaPasien";
            this.txtNamaPasien.ReadOnly = true;
            this.txtNamaPasien.Size = new System.Drawing.Size(293, 20);
            this.txtNamaPasien.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblNoJO);
            this.groupBox1.Controls.Add(this.lblPrintCount);
            this.groupBox1.Controls.Add(this.txtResep);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtUnit);
            this.groupBox1.Controls.Add(this.txtDokter);
            this.groupBox1.Controls.Add(this.lblKdLayan);
            this.groupBox1.Controls.Add(this.lblOrder);
            this.groupBox1.Controls.Add(this.lblNoReg);
            this.groupBox1.Controls.Add(this.txtNamaPasien);
            this.groupBox1.Controls.Add(this.txtOrder);
            this.groupBox1.Controls.Add(this.txtRegistrasi);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(391, 284);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Informasi Resep";
            // 
            // lblNoJO
            // 
            this.lblNoJO.AutoSize = true;
            this.lblNoJO.Location = new System.Drawing.Point(309, 262);
            this.lblNoJO.Name = "lblNoJO";
            this.lblNoJO.Size = new System.Drawing.Size(35, 13);
            this.lblNoJO.TabIndex = 14;
            this.lblNoJO.Text = "label3";
            this.lblNoJO.Visible = false;
            // 
            // lblPrintCount
            // 
            this.lblPrintCount.AutoSize = true;
            this.lblPrintCount.Location = new System.Drawing.Point(9, 262);
            this.lblPrintCount.Name = "lblPrintCount";
            this.lblPrintCount.Size = new System.Drawing.Size(68, 13);
            this.lblPrintCount.TabIndex = 13;
            this.lblPrintCount.Text = "Print Count : ";
            // 
            // txtResep
            // 
            this.txtResep.Location = new System.Drawing.Point(9, 149);
            this.txtResep.Multiline = true;
            this.txtResep.Name = "txtResep";
            this.txtResep.ReadOnly = true;
            this.txtResep.Size = new System.Drawing.Size(376, 106);
            this.txtResep.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Unit";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Dokter";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Pasien";
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(92, 123);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.ReadOnly = true;
            this.txtUnit.Size = new System.Drawing.Size(293, 20);
            this.txtUnit.TabIndex = 8;
            // 
            // txtDokter
            // 
            this.txtDokter.Location = new System.Drawing.Point(92, 97);
            this.txtDokter.Name = "txtDokter";
            this.txtDokter.ReadOnly = true;
            this.txtDokter.Size = new System.Drawing.Size(293, 20);
            this.txtDokter.TabIndex = 7;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 332);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(415, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(44, 17);
            this.lblStatus.Text = "Started";
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.cmsNotifyIcon;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "notifyIcon1";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 354);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStartStop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Prescription Print Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.cmsNotifyIcon.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOrder;
        private System.Windows.Forms.Label lblNoReg;
        private System.Windows.Forms.Label lblKdLayan;
        private System.Windows.Forms.Timer tmrPolling;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.TextBox txtOrder;
        private System.Windows.Forms.TextBox txtRegistrasi;
        private System.Windows.Forms.TextBox txtNamaPasien;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.TextBox txtDokter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPrintCount;
        private System.Windows.Forms.TextBox txtResep;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Label lblNoJO;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip cmsNotifyIcon;
        private System.Windows.Forms.ToolStripMenuItem closeProgramToolStripMenuItem;
    }
}

