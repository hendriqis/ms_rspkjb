namespace QIS.Medinfras.ReportDesktop
{
    partial class LPasienIGD
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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LPasienIGD));
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cNo = new DevExpress.XtraReports.UI.XRTableCell();
            this.cPatient = new DevExpress.XtraReports.UI.XRTableCell();
            this.cChiefComplaint = new DevExpress.XtraReports.UI.XRTableCell();
            this.cVitalSign = new DevExpress.XtraReports.UI.XRTableCell();
            this.cReviewOfSystem = new DevExpress.XtraReports.UI.XRTableCell();
            this.cPlanningNotes = new DevExpress.XtraReports.UI.XRTableCell();
            this.cDiagTriageCase = new DevExpress.XtraReports.UI.XRTableCell();
            this.cMedication = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblPeriod = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.StylePriority.UseFont = false;
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetPatientIGD);
            // 
            // lblReportSubTitle
            // 
            this.lblReportSubTitle.StylePriority.UseBorders = false;
            this.lblReportSubTitle.StylePriority.UseFont = false;
            this.lblReportSubTitle.StylePriority.UseTextAlignment = false;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel5,
            this.xrLabel1,
            this.lblPeriod});
            this.ReportHeader.HeightF = 434.7868F;
            this.ReportHeader.Controls.SetChildIndex(this.lblReportSubTitle, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblPeriod, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel1, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel5, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel4, 0);
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 32.83266F;
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ActualVisitDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ActualVisitTime", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 63.49993F;
            // 
            // xrTable1
            // 
            this.xrTable1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Dpi = 254F;
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.ForeColor = System.Drawing.Color.Black;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(31.74901F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTable1.SizeF = new System.Drawing.SizeF(2766.813F, 60.85416F);
            this.xrTable1.StylePriority.UseBackColor = false;
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseForeColor = false;
            this.xrTable1.StylePriority.UsePadding = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell4,
            this.xrTableCell10,
            this.xrTableCell6,
            this.xrTableCell9,
            this.xrTableCell11,
            this.xrTableCell12,
            this.xrTableCell14});
            this.xrTableRow6.Dpi = 254F;
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "No";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell3.Weight = 0.07062080990194039D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Text = "Patient";
            this.xrTableCell4.Weight = 0.43625750906975225D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Text = "Chief Complaint";
            this.xrTableCell10.Weight = 0.41964931957600093D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.Multiline = true;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Text = "Vital Sign";
            this.xrTableCell6.Weight = 0.40944789975420581D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Text = "Review of System";
            this.xrTableCell9.Weight = 0.40723565301540487D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Text = "Planning Notes";
            this.xrTableCell11.Weight = 0.41030518515998171D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Text = "Diagnose/Triage/Case";
            this.xrTableCell12.Weight = 0.41170822604595969D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Dpi = 254F;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Text = "Medication";
            this.xrTableCell14.Weight = 0.41598668178971304D;
            // 
            // xrTable2
            // 
            this.xrTable2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Dpi = 254F;
            this.xrTable2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(31.75105F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7});
            this.xrTable2.SizeF = new System.Drawing.SizeF(2766.812F, 32.83266F);
            this.xrTable2.StylePriority.UseBorderDashStyle = false;
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UsePadding = false;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cNo,
            this.cPatient,
            this.cChiefComplaint,
            this.cVitalSign,
            this.cReviewOfSystem,
            this.cPlanningNotes,
            this.cDiagTriageCase,
            this.cMedication});
            this.xrTableRow7.Dpi = 254F;
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // cNo
            // 
            this.cNo.Dpi = 254F;
            this.cNo.Name = "cNo";
            this.cNo.StylePriority.UseTextAlignment = false;
            xrSummary3.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.cNo.Summary = xrSummary3;
            this.cNo.Text = "No";
            this.cNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.cNo.Weight = 0.070620823853334513D;
            // 
            // cPatient
            // 
            this.cPatient.Dpi = 254F;
            this.cPatient.Multiline = true;
            this.cPatient.Name = "cPatient";
            this.cPatient.Text = resources.GetString("cPatient.Text");
            this.cPatient.Weight = 0.4362576088268933D;
            // 
            // cChiefComplaint
            // 
            this.cChiefComplaint.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NewChiefComplaint")});
            this.cChiefComplaint.Dpi = 254F;
            this.cChiefComplaint.Multiline = true;
            this.cChiefComplaint.Name = "cChiefComplaint";
            this.cChiefComplaint.Text = "cChiefComplaint";
            this.cChiefComplaint.Weight = 0.41964946810852954D;
            // 
            // cVitalSign
            // 
            this.cVitalSign.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NewVitalSign")});
            this.cVitalSign.Dpi = 254F;
            this.cVitalSign.Multiline = true;
            this.cVitalSign.Name = "cVitalSign";
            this.cVitalSign.Weight = 0.40944557667429027D;
            // 
            // cReviewOfSystem
            // 
            this.cReviewOfSystem.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NewReviewOfSystem")});
            this.cReviewOfSystem.Dpi = 254F;
            this.cReviewOfSystem.Multiline = true;
            this.cReviewOfSystem.Name = "cReviewOfSystem";
            this.cReviewOfSystem.Text = "cReviewOfSystem";
            this.cReviewOfSystem.Weight = 0.407235208600257D;
            // 
            // cPlanningNotes
            // 
            this.cPlanningNotes.Dpi = 254F;
            this.cPlanningNotes.Multiline = true;
            this.cPlanningNotes.Name = "cPlanningNotes";
            this.cPlanningNotes.Text = "[NewDoctorNotes]\r\n\r\n[NewLabResult]\r\n\r\n[NewRadResult]";
            this.cPlanningNotes.Weight = 0.41030558978755138D;
            // 
            // cDiagTriageCase
            // 
            this.cDiagTriageCase.Dpi = 254F;
            this.cDiagTriageCase.Multiline = true;
            this.cDiagTriageCase.Name = "cDiagTriageCase";
            this.cDiagTriageCase.Text = "[Diagnose]\r\n\r\n[NewTriage]\r\n\r\n[NewVisitType]\r\n\r\n[NewVisitReason]";
            this.cDiagTriageCase.Weight = 0.41170822083317943D;
            // 
            // cMedication
            // 
            this.cMedication.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NewPrescription")});
            this.cMedication.Dpi = 254F;
            this.cMedication.Multiline = true;
            this.cMedication.Name = "cMedication";
            this.cMedication.Text = "cMedication";
            this.cMedication.Weight = 0.41598772880307971D;
            // 
            // lblPeriod
            // 
            this.lblPeriod.Dpi = 254F;
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriod.LocationFloat = new DevExpress.Utils.PointFloat(31.75107F, 357.1875F);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPeriod.SizeF = new System.Drawing.SizeF(2766.814F, 31.75F);
            this.lblPeriod.StylePriority.UseFont = false;
            this.lblPeriod.StylePriority.UseTextAlignment = false;
            this.lblPeriod.Text = "Periode : ";
            this.lblPeriod.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(31.74901F, 309.3509F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(2766.814F, 31.96167F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Shift : [Shift]";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(2205.229F, 405.471F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(375.709F, 29.31584F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            xrSummary2.Func = DevExpress.XtraReports.UI.SummaryFunc.Count;
            this.xrLabel5.Summary = xrSummary2;
            this.xrLabel5.Text = "Jumlah Pasien :";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "MRN")});
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(2586.229F, 405.471F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(212.3359F, 29.31584F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.Count;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrLabel4.Summary = xrSummary1;
            this.xrLabel4.Text = "xrLabel4";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 60.85416F;
            this.PageHeader.Name = "PageHeader";
            // 
            // LPasienIGD
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.PageHeader});
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell11;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow7;
        private DevExpress.XtraReports.UI.XRTableCell cNo;
        private DevExpress.XtraReports.UI.XRTableCell cPatient;
        private DevExpress.XtraReports.UI.XRTableCell cChiefComplaint;
        private DevExpress.XtraReports.UI.XRTableCell cVitalSign;
        private DevExpress.XtraReports.UI.XRTableCell cReviewOfSystem;
        private DevExpress.XtraReports.UI.XRTableCell cPlanningNotes;
        private DevExpress.XtraReports.UI.XRTableCell cDiagTriageCase;
        private DevExpress.XtraReports.UI.XRLabel lblPeriod;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRTableCell cMedication;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell14;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
    }
}
