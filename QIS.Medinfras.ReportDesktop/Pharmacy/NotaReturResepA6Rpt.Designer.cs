namespace QIS.Medinfras.ReportDesktop
{
    partial class NotaReturResepA6Rpt
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
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblUserName = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblsubTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPatientInfo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRegistrationInfo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPhysicianName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPatientLocation = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // lblReportProperties1
            // 
            this.lblReportProperties1.StylePriority.UseFont = false;
            this.lblReportProperties1.StylePriority.UsePadding = false;
            this.lblReportProperties1.StylePriority.UseTextAlignment = false;
            this.lblReportProperties1.Text = "MEDINFRAS - ReportID , dd-MMM-yyyy HH:MM:ss, User ID : XXXXX";
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vPrescriptionReturnOrderDt);
            // 
            // lblReportProperties2
            // 
            this.lblReportProperties2.SizeF = new System.Drawing.SizeF(881.7703F, 14.68751F);
            this.lblReportProperties2.StylePriority.UseFont = false;
            this.lblReportProperties2.StylePriority.UsePadding = false;
            this.lblReportProperties2.StylePriority.UseTextAlignment = false;
            this.lblReportProperties2.Text = "";
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Underline);
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            this.lblReportTitle.Text = "RETUR RESEP";
            // 
            // tableHeader
            // 
            this.tableHeader.StylePriority.UseFont = false;
            this.tableHeader.StylePriority.UsePadding = false;
            this.tableHeader.StylePriority.UseTextAlignment = false;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1,
            this.lblPatientLocation,
            this.lblPhysicianName,
            this.lblRegistrationInfo,
            this.xrLabel13,
            this.lblPatientInfo,
            this.xrLabel8,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1,
            this.lblsubTitle});
            this.ReportHeader.HeightF = 423.3333F;
            this.ReportHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLine2, 0);
            this.ReportHeader.Controls.SetChildIndex(this.tableHeader, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblsubTitle, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel1, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel2, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel3, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel4, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel5, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel6, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel8, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblPatientInfo, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel13, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblRegistrationInfo, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblPhysicianName, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblPatientLocation, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrTable1, 0);
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 38.1871F;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 30F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 74.08334F;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
            this.ReportFooter.Dpi = 254F;
            this.ReportFooter.HeightF = 34.39583F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrTable3
            // 
            this.xrTable3.Dpi = 254F;
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(13.22896F, 0F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7});
            this.xrTable3.SizeF = new System.Drawing.SizeF(881.7712F, 34.39583F);
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.lblUserName,
            this.xrTableCell9,
            this.xrTableCell10});
            this.xrTableRow7.Dpi = 254F;
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell7.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Font = new System.Drawing.Font("Arial", 8F);
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell7.StylePriority.UseBorders = false;
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.Text = "user name :";
            this.xrTableCell7.Weight = 0.82291670671598183D;
            // 
            // lblUserName
            // 
            this.lblUserName.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.lblUserName.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.lblUserName.Dpi = 254F;
            this.lblUserName.Font = new System.Drawing.Font("Arial", 8F);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.StylePriority.UseBorderDashStyle = false;
            this.lblUserName.StylePriority.UseBorders = false;
            this.lblUserName.StylePriority.UseFont = false;
            this.lblUserName.Weight = 0.83333413431963588D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell9.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.Font = new System.Drawing.Font("Arial", 8F);
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "TOTAL";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell9.Weight = 0.86458299147206163D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell10.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "LineAmount")});
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Font = new System.Drawing.Font("Arial", 8F);
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell10.StylePriority.UseBorders = false;
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0:#,#}";
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell10.Summary = xrSummary1;
            this.xrTableCell10.Text = "xrTableCell10";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell10.Weight = 0.9507060027839056D;
            // 
            // lblsubTitle
            // 
            this.lblsubTitle.Dpi = 254F;
            this.lblsubTitle.Font = new System.Drawing.Font("Arial", 9F);
            this.lblsubTitle.LocationFloat = new DevExpress.Utils.PointFloat(13.22917F, 140.4949F);
            this.lblsubTitle.Name = "lblsubTitle";
            this.lblsubTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblsubTitle.SizeF = new System.Drawing.SizeF(881.7704F, 33.51811F);
            this.lblsubTitle.StylePriority.UseFont = false;
            this.lblsubTitle.StylePriority.UseTextAlignment = false;
            this.lblsubTitle.Text = "No. : ";
            this.lblsubTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 9F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(13.22914F, 206.2754F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(272.9875F, 38.18718F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Nama Pasien";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 9F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(13.22896F, 244.4626F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(272.9877F, 38.18712F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "No. Reg/No.RM";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 9F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(13.22898F, 282.6497F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(272.9878F, 38.18713F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "Dokter";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 9F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(13.22917F, 320.8368F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(272.9875F, 38.18713F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "Lokasi Pasien";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(286.2166F, 320.8369F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(28.32596F, 38.1871F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = ":";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Dpi = 254F;
            this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(286.2167F, 206.2755F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(28.32596F, 38.18712F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = ":";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Dpi = 254F;
            this.xrLabel8.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(286.2167F, 282.6497F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(28.32596F, 38.18707F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = ":";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblPatientInfo
            // 
            this.lblPatientInfo.Dpi = 254F;
            this.lblPatientInfo.Font = new System.Drawing.Font("Arial", 9F);
            this.lblPatientInfo.LocationFloat = new DevExpress.Utils.PointFloat(314.5427F, 206.2755F);
            this.lblPatientInfo.Name = "lblPatientInfo";
            this.lblPatientInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPatientInfo.SizeF = new System.Drawing.SizeF(580.4569F, 38.18713F);
            this.lblPatientInfo.StylePriority.UseFont = false;
            this.lblPatientInfo.Text = "lblPatientInfo";
            // 
            // xrLabel13
            // 
            this.xrLabel13.Dpi = 254F;
            this.xrLabel13.Font = new System.Drawing.Font("Times New Roman", 9F);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(286.2167F, 244.4626F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(28.32596F, 38.18712F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = ":";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblRegistrationInfo
            // 
            this.lblRegistrationInfo.Dpi = 254F;
            this.lblRegistrationInfo.Font = new System.Drawing.Font("Arial", 9F);
            this.lblRegistrationInfo.LocationFloat = new DevExpress.Utils.PointFloat(314.5426F, 244.4626F);
            this.lblRegistrationInfo.Name = "lblRegistrationInfo";
            this.lblRegistrationInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblRegistrationInfo.SizeF = new System.Drawing.SizeF(580.4569F, 38.18713F);
            this.lblRegistrationInfo.StylePriority.UseFont = false;
            this.lblRegistrationInfo.Text = "lblRegistrationInfo";
            // 
            // lblPhysicianName
            // 
            this.lblPhysicianName.Dpi = 254F;
            this.lblPhysicianName.Font = new System.Drawing.Font("Arial", 9F);
            this.lblPhysicianName.LocationFloat = new DevExpress.Utils.PointFloat(314.5427F, 282.6497F);
            this.lblPhysicianName.Name = "lblPhysicianName";
            this.lblPhysicianName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPhysicianName.SizeF = new System.Drawing.SizeF(580.4569F, 38.18713F);
            this.lblPhysicianName.StylePriority.UseFont = false;
            this.lblPhysicianName.Text = "lblPhysicianName";
            // 
            // lblPatientLocation
            // 
            this.lblPatientLocation.Dpi = 254F;
            this.lblPatientLocation.Font = new System.Drawing.Font("Arial", 9F);
            this.lblPatientLocation.LocationFloat = new DevExpress.Utils.PointFloat(314.5427F, 320.8368F);
            this.lblPatientLocation.Name = "lblPatientLocation";
            this.lblPatientLocation.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPatientLocation.SizeF = new System.Drawing.SizeF(580.4569F, 38.18713F);
            this.lblPatientLocation.StylePriority.UseFont = false;
            this.lblPatientLocation.Text = "lblPatientLocation";
            // 
            // xrTable1
            // 
            this.xrTable1.Dpi = 254F;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(13.22896F, 385.1462F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5});
            this.xrTable1.SizeF = new System.Drawing.SizeF(881.7709F, 38.1871F);
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3});
            this.xrTableRow5.Dpi = 254F;
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Font = new System.Drawing.Font("Arial", 9F);
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.Text = "Obat";
            this.xrTableCell1.Weight = 1.6562499247228482D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Font = new System.Drawing.Font("Arial", 9F);
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Qty";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell2.Weight = 1.0937508302816423D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Font = new System.Drawing.Font("Arial", 9F);
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell3.StylePriority.UseBorders = false;
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Jumlah";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell3.Weight = 0.72153772089010892D;
            // 
            // xrTable2
            // 
            this.xrTable2.Dpi = 254F;
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(13.22925F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTable2.SizeF = new System.Drawing.SizeF(881.7709F, 38.1871F);
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6});
            this.xrTableRow6.Dpi = 254F;
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ItemName1")});
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Font = new System.Drawing.Font("Arial", 8F);
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell4.StylePriority.UseBorders = false;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.Text = "xrTableCell4";
            this.xrTableCell4.Weight = 1.6562499247228482D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrTableCell5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfQtyItemUnit")});
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Font = new System.Drawing.Font("Arial", 8F);
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell5.StylePriority.UseBorders = false;
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "xrTableCell5";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell5.Weight = 1.0937498690981897D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrTableCell6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "LineAmount")});
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.Font = new System.Drawing.Font("Arial", 8F);
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell6.StylePriority.UseBorders = false;
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "xrTableCell6";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell6.Weight = 0.72153868207356164D;
            // 
            // NotaReturResepA6Rpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.ReportFooter});
            this.DetailPrintCountOnEmptyDataSource = 0;
            this.Margins = new System.Drawing.Printing.Margins(30, 30, 30, 0);
            this.PageWidth = 1000;
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.ReportFooter, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRLabel lblsubTitle;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel lblPatientInfo;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel lblPatientLocation;
        private DevExpress.XtraReports.UI.XRLabel lblPhysicianName;
        private DevExpress.XtraReports.UI.XRLabel lblRegistrationInfo;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRTable xrTable3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell lblUserName;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
    }
}
