namespace QIS.Medinfras.Report
{
    partial class EpisodeSummaryRpt
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
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tdRegistrationDateTime = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tdPatientAge = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.tdMRN = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblMRN = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRegistrationNoCover = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblPhysicianName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDiagnose = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPatientName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.imgPatientPicture = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRegistrationNo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.EpisodeSummaryHeader = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRegistrationDateTime = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblLOS = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblServiceUnitName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblVisitType = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.sbrChiefComplaint = new DevExpress.XtraReports.UI.XRSubreport();
            this.episodeSummaryChiefComplaintRpt1 = new QIS.Medinfras.Report.EpisodeSummaryChiefComplaintRpt();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.cbrAllergy = new DevExpress.XtraReports.UI.XRSubreport();
            this.episodeSummaryAllergyRpt1 = new QIS.Medinfras.Report.EpisodeSummaryAllergyRpt();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.cbrVitalSign = new DevExpress.XtraReports.UI.XRSubreport();
            this.episodeSummaryVitalSignRpt1 = new QIS.Medinfras.Report.EpisodeSummaryVitalSignRpt();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.cbrReviewOfSystem = new DevExpress.XtraReports.UI.XRSubreport();
            this.episodeSummaryReviewOfSystemRpt1 = new QIS.Medinfras.Report.EpisodeSummaryReviewOfSystemRpt();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo3 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.lblReportProperties = new DevExpress.XtraReports.UI.XRLabel();
            this.lineFooter = new DevExpress.XtraReports.UI.XRLine();
            this.sbrBodyDiagram = new DevExpress.XtraReports.UI.XRSubreport();
            this.episodeSummaryBodyDiagramRpt1 = new QIS.Medinfras.Report.EpisodeSummaryBodyDiagramRpt();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.episodeSummaryChiefComplaintRpt1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.episodeSummaryAllergyRpt1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.episodeSummaryVitalSignRpt1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.episodeSummaryReviewOfSystemRpt1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.episodeSummaryBodyDiagramRpt1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lineFooter,
            this.lblReportProperties,
            this.xrPageInfo3});
            this.PageFooter.HeightF = 34.37501F;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.sbrBodyDiagram,
            this.xrLabel18,
            this.cbrReviewOfSystem,
            this.xrLabel17,
            this.cbrVitalSign,
            this.xrLabel16,
            this.cbrAllergy,
            this.xrLabel15,
            this.xrLabel14,
            this.sbrChiefComplaint,
            this.xrLabel13,
            this.xrLabel10,
            this.lblVisitType,
            this.xrLabel12,
            this.xrLabel11,
            this.lblServiceUnitName,
            this.xrLabel9,
            this.xrLabel8,
            this.lblLOS,
            this.xrLabel7,
            this.xrLabel6,
            this.lblRegistrationDateTime,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel3});
            this.Detail.HeightF = 437.5833F;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1,
            this.xrLabel2,
            this.lblRegistrationNo,
            this.xrLabel1});
            this.PageHeader.HeightF = 303.9583F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrTable1
            // 
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(22.91667F, 104.1667F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow2});
            this.xrTable1.SizeF = new System.Drawing.SizeF(598.9583F, 182.7083F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tdRegistrationDateTime,
            this.xrTableCell2,
            this.tdPatientAge});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // tdRegistrationDateTime
            // 
            this.tdRegistrationDateTime.BackColor = System.Drawing.Color.LightGray;
            this.tdRegistrationDateTime.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tdRegistrationDateTime.Font = new System.Drawing.Font("Arial", 9.75F);
            this.tdRegistrationDateTime.Name = "tdRegistrationDateTime";
            this.tdRegistrationDateTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.tdRegistrationDateTime.StylePriority.UseBackColor = false;
            this.tdRegistrationDateTime.StylePriority.UseBorders = false;
            this.tdRegistrationDateTime.StylePriority.UseFont = false;
            this.tdRegistrationDateTime.StylePriority.UsePadding = false;
            this.tdRegistrationDateTime.StylePriority.UseTextAlignment = false;
            this.tdRegistrationDateTime.Text = "tdRegistrationDateTime";
            this.tdRegistrationDateTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tdRegistrationDateTime.Weight = 1.874999731506461D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseBackColor = false;
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.Weight = 2.3333336954949204D;
            // 
            // tdPatientAge
            // 
            this.tdPatientAge.BackColor = System.Drawing.Color.LightGray;
            this.tdPatientAge.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tdPatientAge.Font = new System.Drawing.Font("Arial", 9.75F);
            this.tdPatientAge.Name = "tdPatientAge";
            this.tdPatientAge.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.tdPatientAge.StylePriority.UseBackColor = false;
            this.tdPatientAge.StylePriority.UseBorders = false;
            this.tdPatientAge.StylePriority.UseFont = false;
            this.tdPatientAge.StylePriority.UsePadding = false;
            this.tdPatientAge.StylePriority.UseTextAlignment = false;
            this.tdPatientAge.Text = "tdPatientAge";
            this.tdPatientAge.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tdPatientAge.Weight = 1.7812498681874232D;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.tdMRN,
            this.xrTableCell5,
            this.xrTableCell6});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 6.3083331298828123D;
            // 
            // tdMRN
            // 
            this.tdMRN.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.tdMRN.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblMRN,
            this.lblRegistrationNoCover,
            this.xrLine1});
            this.tdMRN.Name = "tdMRN";
            this.tdMRN.StylePriority.UseBorders = false;
            this.tdMRN.Text = "tdMRN";
            this.tdMRN.Weight = 1.874999731506461D;
            // 
            // lblMRN
            // 
            this.lblMRN.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblMRN.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblMRN.LocationFloat = new DevExpress.Utils.PointFloat(1.999994F, 2F);
            this.lblMRN.Name = "lblMRN";
            this.lblMRN.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.lblMRN.SizeF = new System.Drawing.SizeF(175.5F, 23F);
            this.lblMRN.StylePriority.UseBorders = false;
            this.lblMRN.StylePriority.UseFont = false;
            this.lblMRN.StylePriority.UsePadding = false;
            this.lblMRN.StylePriority.UseTextAlignment = false;
            this.lblMRN.Text = "MRN ";
            this.lblMRN.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblRegistrationNoCover
            // 
            this.lblRegistrationNoCover.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblRegistrationNoCover.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblRegistrationNoCover.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 132.71F);
            this.lblRegistrationNoCover.Name = "lblRegistrationNoCover";
            this.lblRegistrationNoCover.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.lblRegistrationNoCover.SizeF = new System.Drawing.SizeF(167.5F, 23F);
            this.lblRegistrationNoCover.StylePriority.UseBorders = false;
            this.lblRegistrationNoCover.StylePriority.UseFont = false;
            this.lblRegistrationNoCover.StylePriority.UsePadding = false;
            this.lblRegistrationNoCover.StylePriority.UseTextAlignment = false;
            this.lblRegistrationNoCover.Text = "RegistrationNo";
            this.lblRegistrationNoCover.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine1
            // 
            this.xrLine1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(2F, 125.335F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(183.5F, 7.375F);
            this.xrLine1.StylePriority.UseBorders = false;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblPhysicianName,
            this.lblDiagnose,
            this.lblPatientName});
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseBorders = false;
            this.xrTableCell5.Text = "xrTableCell5";
            this.xrTableCell5.Weight = 2.3333333903191309D;
            // 
            // lblPhysicianName
            // 
            this.lblPhysicianName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPhysicianName.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblPhysicianName.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 125.335F);
            this.lblPhysicianName.Name = "lblPhysicianName";
            this.lblPhysicianName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.lblPhysicianName.SizeF = new System.Drawing.SizeF(213.3333F, 23F);
            this.lblPhysicianName.StylePriority.UseBorders = false;
            this.lblPhysicianName.StylePriority.UseFont = false;
            this.lblPhysicianName.StylePriority.UsePadding = false;
            this.lblPhysicianName.StylePriority.UseTextAlignment = false;
            this.lblPhysicianName.Text = "Physician Name";
            this.lblPhysicianName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblDiagnose
            // 
            this.lblDiagnose.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblDiagnose.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblDiagnose.LocationFloat = new DevExpress.Utils.PointFloat(10F, 35.4167F);
            this.lblDiagnose.Name = "lblDiagnose";
            this.lblDiagnose.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.lblDiagnose.SizeF = new System.Drawing.SizeF(213.3333F, 23F);
            this.lblDiagnose.StylePriority.UseBorders = false;
            this.lblDiagnose.StylePriority.UseFont = false;
            this.lblDiagnose.StylePriority.UsePadding = false;
            this.lblDiagnose.StylePriority.UseTextAlignment = false;
            this.lblDiagnose.Text = "Diagnose";
            this.lblDiagnose.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblPatientName
            // 
            this.lblPatientName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPatientName.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblPatientName.LocationFloat = new DevExpress.Utils.PointFloat(6.666719F, 2.000015F);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.lblPatientName.SizeF = new System.Drawing.SizeF(216.6666F, 22.99998F);
            this.lblPatientName.StylePriority.UseBorders = false;
            this.lblPatientName.StylePriority.UseFont = false;
            this.lblPatientName.StylePriority.UsePadding = false;
            this.lblPatientName.StylePriority.UseTextAlignment = false;
            this.lblPatientName.Text = "Patient Name";
            this.lblPatientName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.imgPatientPicture});
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseBorders = false;
            this.xrTableCell6.Text = "xrTableCell6";
            this.xrTableCell6.Weight = 1.7812501733632127D;
            // 
            // imgPatientPicture
            // 
            this.imgPatientPicture.LocationFloat = new DevExpress.Utils.PointFloat(9.999987F, 10F);
            this.imgPatientPicture.Name = "imgPatientPicture";
            this.imgPatientPicture.SizeF = new System.Drawing.SizeF(160.6251F, 145.71F);
            this.imgPatientPicture.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 60.49998F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(650F, 23F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Summary";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblRegistrationNo
            // 
            this.lblRegistrationNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblRegistrationNo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 37.5F);
            this.lblRegistrationNo.Name = "lblRegistrationNo";
            this.lblRegistrationNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblRegistrationNo.SizeF = new System.Drawing.SizeF(650F, 23F);
            this.lblRegistrationNo.StylePriority.UseFont = false;
            this.lblRegistrationNo.StylePriority.UseTextAlignment = false;
            this.lblRegistrationNo.Text = "Registration No :";
            this.lblRegistrationNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(650F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Episode Summary Report";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // EpisodeSummaryHeader
            // 
            this.EpisodeSummaryHeader.BackColor = System.Drawing.Color.LightGray;
            this.EpisodeSummaryHeader.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EpisodeSummaryHeader.Name = "EpisodeSummaryHeader";
            this.EpisodeSummaryHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.EpisodeSummaryHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(650F, 23F);
            this.xrLabel3.StyleName = "EpisodeSummaryHeader";
            this.xrLabel3.Text = "Episode Information";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel4.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 32.99999F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(119.7916F, 23F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseForeColor = false;
            this.xrLabel4.StylePriority.UsePadding = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Visit Date / Time";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(119.7916F, 32.99999F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(15.625F, 23F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UsePadding = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = ":";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblRegistrationDateTime
            // 
            this.lblRegistrationDateTime.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblRegistrationDateTime.LocationFloat = new DevExpress.Utils.PointFloat(135.4166F, 32.99999F);
            this.lblRegistrationDateTime.Name = "lblRegistrationDateTime";
            this.lblRegistrationDateTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.lblRegistrationDateTime.SizeF = new System.Drawing.SizeF(170.8333F, 23F);
            this.lblRegistrationDateTime.StylePriority.UseFont = false;
            this.lblRegistrationDateTime.StylePriority.UsePadding = false;
            this.lblRegistrationDateTime.StylePriority.UseTextAlignment = false;
            this.lblRegistrationDateTime.Text = ":";
            this.lblRegistrationDateTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel6.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 56.00001F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(119.7916F, 23F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseForeColor = false;
            this.xrLabel6.StylePriority.UsePadding = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "LOS";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(119.7916F, 56.00001F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(15.625F, 23F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UsePadding = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = ":";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblLOS
            // 
            this.lblLOS.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblLOS.LocationFloat = new DevExpress.Utils.PointFloat(135.4166F, 56.00001F);
            this.lblLOS.Name = "lblLOS";
            this.lblLOS.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.lblLOS.SizeF = new System.Drawing.SizeF(170.8333F, 23F);
            this.lblLOS.StylePriority.UseFont = false;
            this.lblLOS.StylePriority.UsePadding = false;
            this.lblLOS.StylePriority.UseTextAlignment = false;
            this.lblLOS.Text = ":";
            this.lblLOS.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel8.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(0F, 78.99996F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(119.7916F, 23F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseForeColor = false;
            this.xrLabel8.StylePriority.UsePadding = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Unit Name";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(119.7916F, 78.99996F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(15.625F, 23F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UsePadding = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = ":";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblServiceUnitName
            // 
            this.lblServiceUnitName.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblServiceUnitName.LocationFloat = new DevExpress.Utils.PointFloat(135.4166F, 78.99996F);
            this.lblServiceUnitName.Name = "lblServiceUnitName";
            this.lblServiceUnitName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.lblServiceUnitName.SizeF = new System.Drawing.SizeF(170.8333F, 23F);
            this.lblServiceUnitName.StylePriority.UseFont = false;
            this.lblServiceUnitName.StylePriority.UsePadding = false;
            this.lblServiceUnitName.StylePriority.UseTextAlignment = false;
            this.lblServiceUnitName.Text = ":";
            this.lblServiceUnitName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel11.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(0F, 102F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(119.7916F, 23F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseForeColor = false;
            this.xrLabel11.StylePriority.UsePadding = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "Visit Type";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(119.7916F, 102F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(15.625F, 23F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UsePadding = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = ":";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblVisitType
            // 
            this.lblVisitType.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblVisitType.LocationFloat = new DevExpress.Utils.PointFloat(135.4166F, 102F);
            this.lblVisitType.Name = "lblVisitType";
            this.lblVisitType.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.lblVisitType.SizeF = new System.Drawing.SizeF(170.8333F, 23F);
            this.lblVisitType.StylePriority.UseFont = false;
            this.lblVisitType.StylePriority.UsePadding = false;
            this.lblVisitType.StylePriority.UseTextAlignment = false;
            this.lblVisitType.Text = ":";
            this.lblVisitType.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel10.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(0F, 125F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(119.7916F, 23F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseForeColor = false;
            this.xrLabel10.StylePriority.UsePadding = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "Chief Complaint";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(119.7916F, 125F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(15.625F, 23F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UsePadding = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = ":";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // sbrChiefComplaint
            // 
            this.sbrChiefComplaint.CanShrink = true;
            this.sbrChiefComplaint.LocationFloat = new DevExpress.Utils.PointFloat(135.4166F, 129F);
            this.sbrChiefComplaint.Name = "sbrChiefComplaint";
            this.sbrChiefComplaint.ReportSource = this.episodeSummaryChiefComplaintRpt1;
            this.sbrChiefComplaint.SizeF = new System.Drawing.SizeF(503.58F, 19F);
            // 
            // xrLabel14
            // 
            this.xrLabel14.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel14.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(0F, 148F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(119.7916F, 23F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseForeColor = false;
            this.xrLabel14.StylePriority.UsePadding = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "Allergies";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel15
            // 
            this.xrLabel15.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(119.7916F, 148F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(15.625F, 23F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UsePadding = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = ":";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // cbrAllergy
            // 
            this.cbrAllergy.CanShrink = true;
            this.cbrAllergy.LocationFloat = new DevExpress.Utils.PointFloat(135.4166F, 152F);
            this.cbrAllergy.Name = "cbrAllergy";
            this.cbrAllergy.ReportSource = this.episodeSummaryAllergyRpt1;
            this.cbrAllergy.SizeF = new System.Drawing.SizeF(503.5799F, 19F);
            // 
            // xrLabel16
            // 
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(0F, 195.8333F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(650F, 23F);
            this.xrLabel16.StyleName = "EpisodeSummaryHeader";
            this.xrLabel16.Text = "Vital Signs and Indicators";
            // 
            // cbrVitalSign
            // 
            this.cbrVitalSign.LocationFloat = new DevExpress.Utils.PointFloat(0F, 218.8334F);
            this.cbrVitalSign.Name = "cbrVitalSign";
            this.cbrVitalSign.ReportSource = this.episodeSummaryVitalSignRpt1;
            this.cbrVitalSign.SizeF = new System.Drawing.SizeF(639.9999F, 19F);
            // 
            // xrLabel17
            // 
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(0F, 254.1667F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(650F, 23F);
            this.xrLabel17.StyleName = "EpisodeSummaryHeader";
            this.xrLabel17.Text = "Review Of System";
            // 
            // cbrReviewOfSystem
            // 
            this.cbrReviewOfSystem.LocationFloat = new DevExpress.Utils.PointFloat(0F, 277.1667F);
            this.cbrReviewOfSystem.Name = "cbrReviewOfSystem";
            this.cbrReviewOfSystem.ReportSource = this.episodeSummaryReviewOfSystemRpt1;
            this.cbrReviewOfSystem.SizeF = new System.Drawing.SizeF(639.9999F, 19F);
            // 
            // xrLabel18
            // 
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(0F, 310.5833F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(650F, 23F);
            this.xrLabel18.StyleName = "EpisodeSummaryHeader";
            this.xrLabel18.Text = "Body Diagram";
            // 
            // xrPageInfo3
            // 
            this.xrPageInfo3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo3.Format = "Page {0} of {1}";
            this.xrPageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(499.9999F, 15.5F);
            this.xrPageInfo3.Name = "xrPageInfo3";
            this.xrPageInfo3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo3.SizeF = new System.Drawing.SizeF(150F, 13.99999F);
            this.xrPageInfo3.StylePriority.UseFont = false;
            this.xrPageInfo3.StylePriority.UseTextAlignment = false;
            this.xrPageInfo3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportProperties.LocationFloat = new DevExpress.Utils.PointFloat(0F, 12.49997F);
            this.lblReportProperties.Name = "lblReportProperties";
            this.lblReportProperties.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblReportProperties.SizeF = new System.Drawing.SizeF(451.0417F, 17.00002F);
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            this.lblReportProperties.Text = "MEDINFRAS - ReportID, Print Date/Time : dd-MMM-yyyy HH:MM:ss, User ID : XXXXX";
            this.lblReportProperties.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lineFooter
            // 
            this.lineFooter.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lineFooter.Name = "lineFooter";
            this.lineFooter.SizeF = new System.Drawing.SizeF(650F, 12.49997F);
            // 
            // sbrBodyDiagram
            // 
            this.sbrBodyDiagram.LocationFloat = new DevExpress.Utils.PointFloat(0F, 333.5833F);
            this.sbrBodyDiagram.Name = "sbrBodyDiagram";
            this.sbrBodyDiagram.ReportSource = this.episodeSummaryBodyDiagramRpt1;
            this.sbrBodyDiagram.SizeF = new System.Drawing.SizeF(639.9999F, 19F);
            // 
            // EpisodeSummaryRpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.PageFooter,
            this.PageHeader});
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.EpisodeSummaryHeader});
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.episodeSummaryChiefComplaintRpt1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.episodeSummaryAllergyRpt1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.episodeSummaryVitalSignRpt1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.episodeSummaryReviewOfSystemRpt1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.episodeSummaryBodyDiagramRpt1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel lblRegistrationNo;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRControlStyle EpisodeSummaryHeader;
        private DevExpress.XtraReports.UI.XRLabel lblRegistrationDateTime;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel lblLOS;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel lblVisitType;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel lblServiceUnitName;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell tdRegistrationDateTime;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell tdPatientAge;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell tdMRN;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRLabel lblRegistrationNoCover;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRLabel lblMRN;
        private DevExpress.XtraReports.UI.XRPictureBox imgPatientPicture;
        private DevExpress.XtraReports.UI.XRLabel lblPhysicianName;
        private DevExpress.XtraReports.UI.XRLabel lblDiagnose;
        private DevExpress.XtraReports.UI.XRLabel lblPatientName;
        private DevExpress.XtraReports.UI.XRSubreport sbrChiefComplaint;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private EpisodeSummaryChiefComplaintRpt episodeSummaryChiefComplaintRpt1;
        private DevExpress.XtraReports.UI.XRSubreport cbrAllergy;
        private DevExpress.XtraReports.UI.XRLabel xrLabel15;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
        private EpisodeSummaryAllergyRpt episodeSummaryAllergyRpt1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel16;
        private DevExpress.XtraReports.UI.XRSubreport cbrVitalSign;
        private EpisodeSummaryVitalSignRpt episodeSummaryVitalSignRpt1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel17;
        private DevExpress.XtraReports.UI.XRSubreport cbrReviewOfSystem;
        private EpisodeSummaryReviewOfSystemRpt episodeSummaryReviewOfSystemRpt1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel18;
        private DevExpress.XtraReports.UI.XRLine lineFooter;
        private DevExpress.XtraReports.UI.XRLabel lblReportProperties;
        private DevExpress.XtraReports.UI.XRPageInfo xrPageInfo3;
        private DevExpress.XtraReports.UI.XRSubreport sbrBodyDiagram;
        private EpisodeSummaryBodyDiagramRpt episodeSummaryBodyDiagramRpt1;
    }
}
