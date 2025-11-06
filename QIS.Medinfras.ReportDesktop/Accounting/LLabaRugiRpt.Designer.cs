namespace QIS.Medinfras.ReportDesktop
{
    partial class LLabaRugiRpt
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
            DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblGLAccountNo = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblBalanceENDLastMonth = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblProfitLoss = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblBalanceEND = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.cfBalanceENDLastMonth = new DevExpress.XtraReports.UI.CalculatedField();
            this.cfProfitLoss = new DevExpress.XtraReports.UI.CalculatedField();
            this.cfBalanceEND = new DevExpress.XtraReports.UI.CalculatedField();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.lblPeriod = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1,
            this.lblPeriod});
            this.PageHeader.HeightF = 164.5834F;
            this.PageHeader.Controls.SetChildIndex(this.lblPeriod, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrTable1, 0);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetGLBalanceProfitLossPerPeriodPerLevel);
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            // 
            // xrPageInfo3
            // 
            this.xrPageInfo3.StylePriority.UseFont = false;
            this.xrPageInfo3.StylePriority.UseTextAlignment = false;
            // 
            // lblReportParameterTitle
            // 
            this.lblReportParameterTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 37.5F);
            this.lblReportParameterTitle.StylePriority.UseFont = false;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine1,
            this.xrLabel2,
            this.xrLabel3,
            this.xrLabel4,
            this.xrLabel1,
            this.xrLine2});
            this.ReportFooter.HeightF = 120.8333F;
            this.ReportFooter.Controls.SetChildIndex(this.xrLine2, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter7, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter3, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblReportParameterTitle, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter5, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter4, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter2, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter1, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter0, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter6, 0);
            this.ReportFooter.Controls.SetChildIndex(this.xrLabel1, 0);
            this.ReportFooter.Controls.SetChildIndex(this.xrLabel4, 0);
            this.ReportFooter.Controls.SetChildIndex(this.xrLabel3, 0);
            this.ReportFooter.Controls.SetChildIndex(this.xrLabel2, 0);
            this.ReportFooter.Controls.SetChildIndex(this.xrLine1, 0);
            // 
            // lblParameter7
            // 
            this.lblParameter7.LocationFloat = new DevExpress.Utils.PointFloat(400F, 100F);
            this.lblParameter7.StylePriority.UseFont = false;
            // 
            // lblParameter3
            // 
            this.lblParameter3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 100F);
            this.lblParameter3.StylePriority.UseFont = false;
            // 
            // lblParameter6
            // 
            this.lblParameter6.LocationFloat = new DevExpress.Utils.PointFloat(400F, 87.5F);
            this.lblParameter6.StylePriority.UseFont = false;
            // 
            // lblParameter5
            // 
            this.lblParameter5.LocationFloat = new DevExpress.Utils.PointFloat(400F, 75F);
            this.lblParameter5.StylePriority.UseFont = false;
            // 
            // lblParameter4
            // 
            this.lblParameter4.LocationFloat = new DevExpress.Utils.PointFloat(400F, 62.5F);
            this.lblParameter4.StylePriority.UseFont = false;
            // 
            // lblParameter2
            // 
            this.lblParameter2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 87.5F);
            this.lblParameter2.StylePriority.UseFont = false;
            // 
            // lblParameter1
            // 
            this.lblParameter1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 75F);
            this.lblParameter1.StylePriority.UseFont = false;
            // 
            // lblParameter0
            // 
            this.lblParameter0.LocationFloat = new DevExpress.Utils.PointFloat(0F, 62.5F);
            this.lblParameter0.StylePriority.UseFont = false;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 12.5F;
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(12.50001F, 137.5F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(762.5F, 25F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell7,
            this.xrTableCell6,
            this.xrTableCell9});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.Text = "Akun";
            this.xrTableCell1.Weight = 2.7663933184654161D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseBorders = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "Bulan Lalu";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell7.Weight = 1.2295081749574592D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseBorders = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "Bulan Ini";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell6.Weight = 1.229508174957459D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "s/d Bulan Ini";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell9.Weight = 1.0245902954877151D;
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(762.5F, 12.5F);
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5,
            this.lblGLAccountNo,
            this.lblBalanceENDLastMonth,
            this.lblProfitLoss,
            this.lblBalanceEND});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            this.xrTableRow2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableRow2_BeforePrint);
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            xrSummary4.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            this.xrTableCell5.Summary = xrSummary4;
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell5.Weight = 0.125D;
            // 
            // lblGLAccountNo
            // 
            this.lblGLAccountNo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GLAccountName")});
            this.lblGLAccountNo.Name = "lblGLAccountNo";
            this.lblGLAccountNo.StylePriority.UseTextAlignment = false;
            this.lblGLAccountNo.Text = "lblGLAccountNo";
            this.lblGLAccountNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.lblGLAccountNo.Weight = 3.25D;
            this.lblGLAccountNo.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblGLAccountNo_BeforePrint);
            // 
            // lblBalanceENDLastMonth
            // 
            this.lblBalanceENDLastMonth.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "BalanceENDLastMonth", "{0,15:#,##0.00 ;(#,##0.00);-}")});
            this.lblBalanceENDLastMonth.Name = "lblBalanceENDLastMonth";
            this.lblBalanceENDLastMonth.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 3, 0, 0, 100F);
            this.lblBalanceENDLastMonth.StylePriority.UsePadding = false;
            this.lblBalanceENDLastMonth.StylePriority.UseTextAlignment = false;
            this.lblBalanceENDLastMonth.Text = "lblBalanceENDLastMonth";
            this.lblBalanceENDLastMonth.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.lblBalanceENDLastMonth.Weight = 1.5D;
            // 
            // lblProfitLoss
            // 
            this.lblProfitLoss.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ProfitLoss", "{0,15:#,##0.00 ;(#,##0.00);-}")});
            this.lblProfitLoss.Name = "lblProfitLoss";
            this.lblProfitLoss.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 3, 0, 0, 100F);
            this.lblProfitLoss.StylePriority.UsePadding = false;
            this.lblProfitLoss.StylePriority.UseTextAlignment = false;
            this.lblProfitLoss.Text = "lblProfitLoss";
            this.lblProfitLoss.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.lblProfitLoss.Weight = 1.5D;
            // 
            // lblBalanceEND
            // 
            this.lblBalanceEND.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "BalanceEND", "{0,15:#,##0.00 ;(#,##0.00);-}")});
            this.lblBalanceEND.Name = "lblBalanceEND";
            this.lblBalanceEND.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 3, 0, 0, 100F);
            this.lblBalanceEND.StylePriority.UsePadding = false;
            this.lblBalanceEND.StylePriority.UseTextAlignment = false;
            this.lblBalanceEND.Text = "lblBalanceEND";
            this.lblBalanceEND.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.lblBalanceEND.Weight = 1.25D;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("GCGLAccountType", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 12.5F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel6
            // 
            this.xrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GLAccountType")});
            this.xrLabel6.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 0F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(137.5F, 12.5F);
            this.xrLabel6.StylePriority.UseBorders = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(200F, 12.5F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(150F, 12.5F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Total Laba /(Rugi):";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfBalanceENDLastMonth")});
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(350F, 12.5F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(150F, 12.5F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            xrSummary3.FormatString = "{0,15:#,##0.00 ;(#,##0.00);-}";
            xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrLabel1.Summary = xrSummary3;
            this.xrLabel1.Text = "xrLabel1";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfProfitLoss")});
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(500F, 12.5F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(150F, 12.5F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0,15:#,##0.00 ;(#,##0.00);-}";
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrLabel2.Summary = xrSummary1;
            this.xrLabel2.Text = "xrLabel2";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfBalanceEND")});
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(650F, 12.5F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(125F, 12.5F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            xrSummary2.FormatString = "{0,15:#,##0.00 ;(#,##0.00);-}";
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrLabel3.Summary = xrSummary2;
            this.xrLabel3.Text = "xrLabel3";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // cfBalanceENDLastMonth
            // 
            this.cfBalanceENDLastMonth.DataSource = this.bs;
            this.cfBalanceENDLastMonth.Expression = "Iif([IsHeader]=0,[BalanceENDLastMonth],0 )";
            this.cfBalanceENDLastMonth.Name = "cfBalanceENDLastMonth";
            // 
            // cfProfitLoss
            // 
            this.cfProfitLoss.DataSource = this.bs;
            this.cfProfitLoss.Expression = "Iif([IsHeader]=0,[ProfitLoss],0 )";
            this.cfProfitLoss.Name = "cfProfitLoss";
            // 
            // cfBalanceEND
            // 
            this.cfBalanceEND.DataSource = this.bs;
            this.cfBalanceEND.Expression = "Iif([IsHeader]=0,[BalanceEND],0 )";
            this.cfBalanceEND.Name = "cfBalanceEND";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.HeightF = 15.625F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrLine1
            // 
            this.xrLine1.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(200F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(575F, 12.5F);
            // 
            // xrLine2
            // 
            this.xrLine2.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(200F, 25F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(575F, 12.5F);
            // 
            // lblPeriod
            // 
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblPeriod.LocationFloat = new DevExpress.Utils.PointFloat(0F, 112.5F);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblPeriod.SizeF = new System.Drawing.SizeF(775F, 12.5F);
            this.lblPeriod.StylePriority.UseFont = false;
            this.lblPeriod.StylePriority.UseTextAlignment = false;
            this.lblPeriod.Text = "Periode";
            this.lblPeriod.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // LLabaRugiRpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.PageHeader,
            this.PageFooter,
            this.Detail,
            this.ReportFooter,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.GroupHeader1,
            this.GroupFooter1});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.cfBalanceENDLastMonth,
            this.cfProfitLoss,
            this.cfBalanceEND});
            this.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.GroupFooter1, 0);
            this.Controls.SetChildIndex(this.GroupHeader1, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.ReportFooter, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.PageHeader, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRTableCell lblGLAccountNo;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableCell lblBalanceENDLastMonth;
        private DevExpress.XtraReports.UI.XRTableCell lblProfitLoss;
        private DevExpress.XtraReports.UI.XRTableCell lblBalanceEND;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.CalculatedField cfBalanceENDLastMonth;
        private DevExpress.XtraReports.UI.CalculatedField cfProfitLoss;
        private DevExpress.XtraReports.UI.CalculatedField cfBalanceEND;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRLabel lblPeriod;

    }
}
