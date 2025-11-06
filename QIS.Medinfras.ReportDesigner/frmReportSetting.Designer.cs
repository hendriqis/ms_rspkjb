namespace QIS.Medinfras.ReportDesigner
{
    partial class frmReportSetting
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
            this.txtReportRootFolder = new System.Windows.Forms.TextBox();
            this.btnChange = new System.Windows.Forms.Button();
            this.fbdRootFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Report Root Folder";
            // 
            // txtReportRootFolder
            // 
            this.txtReportRootFolder.Location = new System.Drawing.Point(115, 17);
            this.txtReportRootFolder.Name = "txtReportRootFolder";
            this.txtReportRootFolder.ReadOnly = true;
            this.txtReportRootFolder.Size = new System.Drawing.Size(285, 20);
            this.txtReportRootFolder.TabIndex = 1;
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(115, 43);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(107, 20);
            this.btnChange.TabIndex = 2;
            this.btnChange.Text = "Select Root Folder";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(295, 128);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(105, 34);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save Changes";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmReportSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 174);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.txtReportRootFolder);
            this.Controls.Add(this.label1);
            this.Name = "frmReportSetting";
            this.Text = "frmReportSetting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReportRootFolder;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.FolderBrowserDialog fbdRootFolder;
        private System.Windows.Forms.Button btnSave;
    }
}