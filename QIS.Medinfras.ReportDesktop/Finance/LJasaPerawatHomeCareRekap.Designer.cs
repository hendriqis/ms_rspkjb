namespace QIS.Medinfras.ReportDesktop
{
    partial class LJasaPerawatHomeCareRekap
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
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.lblPeriod = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrTable5 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell33 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine5 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.lblDate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.StylePriority.UseFont = false;
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetPendapatanJasaPerawatBrutoRekap);
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.StylePriority.UseBorders = false;
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
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
            this.lblPeriod});
            this.ReportHeader.HeightF = 334.751F;
            this.ReportHeader.Controls.SetChildIndex(this.lblReportSubTitle, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLine1, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblPeriod, 0);
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 62.31253F;
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ParamedicName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 89.64585F;
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Dpi = 254F;
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(25.00063F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTable1.SizeF = new System.Drawing.SizeF(1981F, 94.06247F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UsePadding = false;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell12,
            this.xrTableCell10,
            this.xrTableCell20});
            this.xrTableRow6.Dpi = 254F;
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "No.";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.092332924211573561D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Multiline = true;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "Nama Perawat";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell12.Weight = 0.62730616882999346D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Honor Total Pembayaran";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell10.Weight = 0.68060712552517033D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Dpi = 254F;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.Text = "Paraf";
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell20.Weight = 0.4493666613451518D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Tgl \r\nKunjungan";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.31630142918862669D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Multiline = true;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "Tgl\r\nInput";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell7.Weight = 0.25219196200428173D;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 94.06247F;
            this.PageHeader.Name = "PageHeader";
            // 
            // lblPeriod
            // 
            this.lblPeriod.Dpi = 254F;
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblPeriod.LocationFloat = new DevExpress.Utils.PointFloat(25.00065F, 292.4176F);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPeriod.SizeF = new System.Drawing.SizeF(1981F, 31.75F);
            this.lblPeriod.StylePriority.UseFont = false;
            this.lblPeriod.StylePriority.UseTextAlignment = false;
            this.lblPeriod.Text = "Periode : [StartDate!dd-MMM-yyyy] s/d [EndDate!dd-MMM-yyyy] ";
            this.lblPeriod.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTable2
            // 
            this.xrTable2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Dpi = 254F;
            this.xrTable2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(24.99944F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7});
            this.xrTable2.SizeF = new System.Drawing.SizeF(1981.001F, 62.31253F);
            this.xrTable2.StylePriority.UseBorderDashStyle = false;
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UsePadding = false;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell13,
            this.xrTableCell9,
            this.xrTableCell26,
            this.xrTableCell24});
            this.xrTableRow7.Dpi = 254F;
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Dpi = 254F;
            this.xrTableCell13.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell13.Summary = xrSummary1;
            this.xrTableCell13.Text = "No.";
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell13.Weight = 0.085524796682055454D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ParamedicName")});
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "[ParamedicName] ([ParamedicCode])";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell9.Weight = 0.58104514915646055D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TotalPayment", "{0:n2}")});
            this.xrTableCell26.Dpi = 254F;
            this.xrTableCell26.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.StylePriority.UseBorders = false;
            this.xrTableCell26.StylePriority.UseFont = false;
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            this.xrTableCell26.Text = "xrTableCell26";
            this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell26.Weight = 0.63041537151270544D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.Dpi = 254F;
            this.xrTableCell24.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UseFont = false;
            this.xrTableCell24.StylePriority.UseTextAlignment = false;
            this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell24.Weight = 0.41622773779044797D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfRegistrationDateInString")});
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "xrTableCell2";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.31630126899195121D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfTransactionDateInString")});
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = "xrTableCell8";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell8.Weight = 0.25219183341127649D;
            // 
            // xrLine3
            // 
            this.xrLine3.Dpi = 254F;
            this.xrLine3.LineStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.xrLine3.LineWidth = 3;
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(24.99826F, 92.81572F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(1981.002F, 29.31582F);
            // 
            // xrLine2
            // 
            this.xrLine2.Dpi = 254F;
            this.xrLine2.LineStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.xrLine2.LineWidth = 3;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(25.00065F, 0F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(1980.999F, 29.31583F);
            // 
            // xrTable5
            // 
            this.xrTable5.Dpi = 254F;
            this.xrTable5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable5.LocationFloat = new DevExpress.Utils.PointFloat(25.00382F, 29.31578F);
            this.xrTable5.Name = "xrTable5";
            this.xrTable5.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow10});
            this.xrTable5.SizeF = new System.Drawing.SizeF(1980.996F, 63.5F);
            this.xrTable5.StylePriority.UseFont = false;
            this.xrTable5.StylePriority.UseTextAlignment = false;
            this.xrTable5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell29,
            this.xrTableCell30,
            this.xrTableCell33});
            this.xrTableRow10.Dpi = 254F;
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Weight = 1D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.Dpi = 254F;
            this.xrTableCell29.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell29.Multiline = true;
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.StylePriority.UseFont = false;
            this.xrTableCell29.StylePriority.UseTextAlignment = false;
            this.xrTableCell29.Text = "GRAND TOTAL\t:\r\n";
            this.xrTableCell29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell29.Weight = 3.0344735592126431D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TotalPayment")});
            this.xrTableCell30.Dpi = 254F;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell30.StylePriority.UsePadding = false;
            this.xrTableCell30.StylePriority.UseTextAlignment = false;
            xrSummary2.FormatString = "{0:n2}";
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell30.Summary = xrSummary2;
            this.xrTableCell30.Text = "xrTableCell30";
            this.xrTableCell30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell30.Weight = 2.8699000555710397D;
            // 
            // xrTableCell33
            // 
            this.xrTableCell33.Dpi = 254F;
            this.xrTableCell33.Name = "xrTableCell33";
            this.xrTableCell33.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell33.StylePriority.UsePadding = false;
            this.xrTableCell33.StylePriority.UseTextAlignment = false;
            xrSummary3.FormatString = "{0:n2}";
            xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell33.Summary = xrSummary3;
            this.xrTableCell33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell33.Weight = 1.8948320063771202D;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(1514.128F, 196.2149F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(481.2877F, 39.01721F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Mengetahui,";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(24.99826F, 196.215F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(481.5416F, 39.01721F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Dibuat oleh,";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLine5
            // 
            this.xrLine5.Dpi = 254F;
            this.xrLine5.LineWidth = 3;
            this.xrLine5.LocationFloat = new DevExpress.Utils.PointFloat(1514.128F, 368.959F);
            this.xrLine5.Name = "xrLine5";
            this.xrLine5.SizeF = new System.Drawing.SizeF(481.2876F, 30.19775F);
            // 
            // xrLine4
            // 
            this.xrLine4.Dpi = 254F;
            this.xrLine4.LineWidth = 3;
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(24.99826F, 368.959F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(481.5417F, 30.19778F);
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrLabel1,
            this.lblDate,
            this.xrLine2,
            this.xrTable5,
            this.xrLine3,
            this.xrLine4,
            this.xrLine5,
            this.xrLabel4,
            this.xrLabel5});
            this.GroupFooter1.Dpi = 254F;
            this.GroupFooter1.HeightF = 471.4762F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // lblDate
            // 
            this.lblDate.Dpi = 254F;
            this.lblDate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.LocationFloat = new DevExpress.Utils.PointFloat(24.99277F, 157.1978F);
            this.lblDate.Name = "lblDate";
            this.lblDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 0, 0, 254F);
            this.lblDate.SizeF = new System.Drawing.SizeF(481.5416F, 39.01721F);
            this.lblDate.StylePriority.UseFont = false;
            this.lblDate.StylePriority.UseTextAlignment = false;
            this.lblDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(24.99826F, 399.1567F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(481.5416F, 39.01721F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Kabag. Home Care";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(1513.874F, 399.1567F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(481.5416F, 39.01721F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Kabid. Keperawatan III";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // LJasaPerawatHomeCareRekap
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.PageHeader,
            this.GroupFooter1});
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.GroupFooter1, 0);
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRLabel lblPeriod;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell13;
        private DevExpress.XtraReports.UI.XRTable xrTable5;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow10;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell29;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRLine xrLine3;
        private DevExpress.XtraReports.UI.XRLine xrLine5;
        private DevExpress.XtraReports.UI.XRLine xrLine4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell26;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell8;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell24;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell20;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell30;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell33;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLabel lblDate;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
    }
}
