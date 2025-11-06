namespace QIS.Medinfras.ReportDesigner
{
    partial class frmNewReport
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtReportCode = new System.Windows.Forms.TextBox();
            this.txtReportTitle1 = new System.Windows.Forms.TextBox();
            this.txtReportTitle2 = new System.Windows.Forms.TextBox();
            this.txtReportName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboSourceType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboSourceName = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFilterExp = new System.Windows.Forms.TextBox();
            this.grpNewReport = new System.Windows.Forms.GroupBox();
            this.cbBasedOnLogin = new System.Windows.Forms.CheckBox();
            this.cbUsingDotMatrix = new System.Windows.Forms.CheckBox();
            this.cbShowParameter = new System.Windows.Forms.CheckBox();
            this.cbShowHeader = new System.Windows.Forms.CheckBox();
            this.txtCustomUrl = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbCustomSetting = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTopMargin = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.cboReportType = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cboOrientation = new System.Windows.Forms.ComboBox();
            this.grpNewReport.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ReportCode";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "ReportTitle";
            // 
            // txtReportCode
            // 
            this.txtReportCode.Location = new System.Drawing.Point(120, 19);
            this.txtReportCode.Name = "txtReportCode";
            this.txtReportCode.Size = new System.Drawing.Size(113, 20);
            this.txtReportCode.TabIndex = 2;
            // 
            // txtReportTitle1
            // 
            this.txtReportTitle1.Location = new System.Drawing.Point(120, 45);
            this.txtReportTitle1.Name = "txtReportTitle1";
            this.txtReportTitle1.Size = new System.Drawing.Size(148, 20);
            this.txtReportTitle1.TabIndex = 3;
            // 
            // txtReportTitle2
            // 
            this.txtReportTitle2.Location = new System.Drawing.Point(120, 71);
            this.txtReportTitle2.Name = "txtReportTitle2";
            this.txtReportTitle2.Size = new System.Drawing.Size(148, 20);
            this.txtReportTitle2.TabIndex = 4;
            // 
            // txtReportName
            // 
            this.txtReportName.Location = new System.Drawing.Point(120, 97);
            this.txtReportName.Name = "txtReportName";
            this.txtReportName.Size = new System.Drawing.Size(148, 20);
            this.txtReportName.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Report Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Source Type";
            // 
            // cboSourceType
            // 
            this.cboSourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSourceType.FormattingEnabled = true;
            this.cboSourceType.Location = new System.Drawing.Point(120, 150);
            this.cboSourceType.Name = "cboSourceType";
            this.cboSourceType.Size = new System.Drawing.Size(148, 21);
            this.cboSourceType.TabIndex = 8;
            this.cboSourceType.SelectedIndexChanged += new System.EventHandler(this.cboSourceType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 180);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Source Name";
            // 
            // cboSourceName
            // 
            this.cboSourceName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSourceName.Enabled = false;
            this.cboSourceName.FormattingEnabled = true;
            this.cboSourceName.Location = new System.Drawing.Point(120, 177);
            this.cboSourceName.Name = "cboSourceName";
            this.cboSourceName.Size = new System.Drawing.Size(148, 21);
            this.cboSourceName.TabIndex = 10;
            this.cboSourceName.SelectedIndexChanged += new System.EventHandler(this.cboSourceName_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 212);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Additional Filter Exp.";
            // 
            // txtFilterExp
            // 
            this.txtFilterExp.Location = new System.Drawing.Point(120, 209);
            this.txtFilterExp.Multiline = true;
            this.txtFilterExp.Name = "txtFilterExp";
            this.txtFilterExp.Size = new System.Drawing.Size(148, 59);
            this.txtFilterExp.TabIndex = 12;
            // 
            // grpNewReport
            // 
            this.grpNewReport.Controls.Add(this.cboOrientation);
            this.grpNewReport.Controls.Add(this.label10);
            this.grpNewReport.Controls.Add(this.cbBasedOnLogin);
            this.grpNewReport.Controls.Add(this.cbUsingDotMatrix);
            this.grpNewReport.Controls.Add(this.cbShowParameter);
            this.grpNewReport.Controls.Add(this.cbShowHeader);
            this.grpNewReport.Controls.Add(this.txtCustomUrl);
            this.grpNewReport.Controls.Add(this.label7);
            this.grpNewReport.Controls.Add(this.cbCustomSetting);
            this.grpNewReport.Location = new System.Drawing.Point(290, 19);
            this.grpNewReport.Name = "grpNewReport";
            this.grpNewReport.Size = new System.Drawing.Size(267, 229);
            this.grpNewReport.TabIndex = 13;
            this.grpNewReport.TabStop = false;
            this.grpNewReport.Text = "Report Settings";
            // 
            // cbBasedOnLogin
            // 
            this.cbBasedOnLogin.AutoSize = true;
            this.cbBasedOnLogin.Location = new System.Drawing.Point(6, 146);
            this.cbBasedOnLogin.Name = "cbBasedOnLogin";
            this.cbBasedOnLogin.Size = new System.Drawing.Size(125, 17);
            this.cbBasedOnLogin.TabIndex = 19;
            this.cbBasedOnLogin.Text = "Based on User Login";
            this.cbBasedOnLogin.UseVisualStyleBackColor = true;
            // 
            // cbUsingDotMatrix
            // 
            this.cbUsingDotMatrix.AutoSize = true;
            this.cbUsingDotMatrix.Location = new System.Drawing.Point(6, 123);
            this.cbUsingDotMatrix.Name = "cbUsingDotMatrix";
            this.cbUsingDotMatrix.Size = new System.Drawing.Size(104, 17);
            this.cbUsingDotMatrix.TabIndex = 18;
            this.cbUsingDotMatrix.Text = "Using Dot Matrix";
            this.cbUsingDotMatrix.UseVisualStyleBackColor = true;
            // 
            // cbShowParameter
            // 
            this.cbShowParameter.AutoSize = true;
            this.cbShowParameter.Location = new System.Drawing.Point(6, 100);
            this.cbShowParameter.Name = "cbShowParameter";
            this.cbShowParameter.Size = new System.Drawing.Size(104, 17);
            this.cbShowParameter.TabIndex = 17;
            this.cbShowParameter.Text = "Show Parameter";
            this.cbShowParameter.UseVisualStyleBackColor = true;
            // 
            // cbShowHeader
            // 
            this.cbShowHeader.AutoSize = true;
            this.cbShowHeader.Location = new System.Drawing.Point(6, 77);
            this.cbShowHeader.Name = "cbShowHeader";
            this.cbShowHeader.Size = new System.Drawing.Size(91, 17);
            this.cbShowHeader.TabIndex = 16;
            this.cbShowHeader.Text = "Show Header";
            this.cbShowHeader.UseVisualStyleBackColor = true;
            // 
            // txtCustomUrl
            // 
            this.txtCustomUrl.Location = new System.Drawing.Point(117, 49);
            this.txtCustomUrl.Name = "txtCustomUrl";
            this.txtCustomUrl.Size = new System.Drawing.Size(142, 20);
            this.txtCustomUrl.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Custom Settings URL";
            // 
            // cbCustomSetting
            // 
            this.cbCustomSetting.AutoSize = true;
            this.cbCustomSetting.Location = new System.Drawing.Point(6, 25);
            this.cbCustomSetting.Name = "cbCustomSetting";
            this.cbCustomSetting.Size = new System.Drawing.Size(102, 17);
            this.cbCustomSetting.TabIndex = 0;
            this.cbCustomSetting.Text = "Custom Settings";
            this.cbCustomSetting.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 277);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Top Margin";
            // 
            // txtTopMargin
            // 
            this.txtTopMargin.Location = new System.Drawing.Point(120, 274);
            this.txtTopMargin.Name = "txtTopMargin";
            this.txtTopMargin.Size = new System.Drawing.Size(148, 20);
            this.txtTopMargin.TabIndex = 15;
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(290, 254);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(267, 40);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 126);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Report Type";
            // 
            // cboReportType
            // 
            this.cboReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReportType.FormattingEnabled = true;
            this.cboReportType.Location = new System.Drawing.Point(120, 123);
            this.cboReportType.Name = "cboReportType";
            this.cboReportType.Size = new System.Drawing.Size(148, 21);
            this.cboReportType.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 204);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "Page Orientation";
            // 
            // cboOrientation
            // 
            this.cboOrientation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOrientation.FormattingEnabled = true;
            this.cboOrientation.Items.AddRange(new object[] {
            "Portrait",
            "Landscape",
            "Custom"});
            this.cboOrientation.Location = new System.Drawing.Point(98, 201);
            this.cboOrientation.Name = "cboOrientation";
            this.cboOrientation.Size = new System.Drawing.Size(163, 21);
            this.cboOrientation.TabIndex = 19;
            // 
            // frmNewReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 314);
            this.Controls.Add(this.cboReportType);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtTopMargin);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.grpNewReport);
            this.Controls.Add(this.txtFilterExp);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboSourceName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboSourceType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtReportName);
            this.Controls.Add(this.txtReportTitle2);
            this.Controls.Add(this.txtReportTitle1);
            this.Controls.Add(this.txtReportCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmNewReport";
            this.Text = "frmNewReport";
            this.grpNewReport.ResumeLayout(false);
            this.grpNewReport.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtReportCode;
        private System.Windows.Forms.TextBox txtReportTitle1;
        private System.Windows.Forms.TextBox txtReportTitle2;
        private System.Windows.Forms.TextBox txtReportName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboSourceType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboSourceName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFilterExp;
        private System.Windows.Forms.GroupBox grpNewReport;
        private System.Windows.Forms.CheckBox cbCustomSetting;
        private System.Windows.Forms.TextBox txtCustomUrl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbShowHeader;
        private System.Windows.Forms.CheckBox cbShowParameter;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtTopMargin;
        private System.Windows.Forms.CheckBox cbBasedOnLogin;
        private System.Windows.Forms.CheckBox cbUsingDotMatrix;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboReportType;
        private System.Windows.Forms.ComboBox cboOrientation;
        private System.Windows.Forms.Label label10;
    }
}