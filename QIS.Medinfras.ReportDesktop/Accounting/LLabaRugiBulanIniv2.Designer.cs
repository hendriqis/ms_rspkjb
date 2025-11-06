namespace QIS.Medinfras.ReportDesktop
{
    partial class LLabaRugiBulanIniv2
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
            DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblGLAccountNo = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblProfitLoss = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.fnBalanceENDLastMonth = new DevExpress.XtraReports.UI.CalculatedField();
            this.fnProfitLoss = new DevExpress.XtraReports.UI.CalculatedField();
            this.fnBalanceEND = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.lblPeriod = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.fnProfitPembagi = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1,
            this.lblPeriod});
            this.PageHeader.HeightF = 412.75F;
            this.PageHeader.Controls.SetChildIndex(this.lblReportSubtitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblPeriod, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrTable1, 0);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetGLBalanceProfitLossPerPeriodPerLevel2);
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
            this.lblReportParameterTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 317.5F);
            this.lblReportParameterTitle.StylePriority.UseFont = false;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3,
            this.xrLine1,
            this.xrLine2});
            this.ReportFooter.HeightF = 508F;
            this.ReportFooter.Controls.SetChildIndex(this.xrLine2, 0);
            this.ReportFooter.Controls.SetChildIndex(this.xrLine1, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter7, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter3, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblReportParameterTitle, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter5, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter4, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter2, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter1, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter0, 0);
            this.ReportFooter.Controls.SetChildIndex(this.lblParameter6, 0);
            this.ReportFooter.Controls.SetChildIndex(this.xrTable3, 0);
            // 
            // lblParameter7
            // 
            this.lblParameter7.LocationFloat = new DevExpress.Utils.PointFloat(1016F, 476.25F);
            this.lblParameter7.StylePriority.UseFont = false;
            // 
            // lblParameter3
            // 
            this.lblParameter3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 476.25F);
            this.lblParameter3.StylePriority.UseFont = false;
            // 
            // lblParameter6
            // 
            this.lblParameter6.LocationFloat = new DevExpress.Utils.PointFloat(1016F, 444.5F);
            this.lblParameter6.StylePriority.UseFont = false;
            // 
            // lblParameter5
            // 
            this.lblParameter5.LocationFloat = new DevExpress.Utils.PointFloat(1016F, 412.75F);
            this.lblParameter5.StylePriority.UseFont = false;
            // 
            // lblParameter4
            // 
            this.lblParameter4.LocationFloat = new DevExpress.Utils.PointFloat(1016F, 381F);
            this.lblParameter4.StylePriority.UseFont = false;
            // 
            // lblParameter2
            // 
            this.lblParameter2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 444.5F);
            this.lblParameter2.StylePriority.UseFont = false;
            // 
            // lblParameter1
            // 
            this.lblParameter1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 412.75F);
            this.lblParameter1.StylePriority.UseFont = false;
            // 
            // lblParameter0
            // 
            this.lblParameter0.LocationFloat = new DevExpress.Utils.PointFloat(0F, 381F);
            this.lblParameter0.StylePriority.UseFont = false;
            // 
            // lblReportSubtitle
            // 
            this.lblReportSubtitle.StylePriority.UseFont = false;
            this.lblReportSubtitle.StylePriority.UseTextAlignment = false;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 38F;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 31.75F;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 38F;
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Dpi = 254F;
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(31.75002F, 349.25F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(1936.75F, 63.5F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell6,
            this.xrTableCell2});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.Text = "Akun";
            this.xrTableCell1.Weight = 3.5860656418381844D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseBorders = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "[cfBulan] [ParamYear] ";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell6.Weight = 1.6393442084182794D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(11, 0, 0, 0, 254F);
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Persentase (%)";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell2.Weight = 1.0245901136115858D;
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable2.Dpi = 254F;
            this.xrTable2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(1936.75F, 31.75F);
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5,
            this.lblGLAccountNo,
            this.lblProfitLoss,
            this.xrTableCell3});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            this.xrTableRow2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableRow2_BeforePrint);
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            xrSummary3.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            this.xrTableCell5.Summary = xrSummary3;
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell5.Weight = 0.125D;
            // 
            // lblGLAccountNo
            // 
            this.lblGLAccountNo.Dpi = 254F;
            this.lblGLAccountNo.Name = "lblGLAccountNo";
            this.lblGLAccountNo.StylePriority.UseTextAlignment = false;
            this.lblGLAccountNo.Text = "[GLAccountNo] [GLAccountName]";
            this.lblGLAccountNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.lblGLAccountNo.Weight = 4.25D;
            this.lblGLAccountNo.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblGLAccountNo_BeforePrint);
            // 
            // lblProfitLoss
            // 
            this.lblProfitLoss.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfProfitLoss", "{0,15:#,##0.00 ;(#,##0.00);-}")});
            this.lblProfitLoss.Dpi = 254F;
            this.lblProfitLoss.Name = "lblProfitLoss";
            this.lblProfitLoss.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 8, 0, 0, 254F);
            this.lblProfitLoss.StylePriority.UsePadding = false;
            this.lblProfitLoss.StylePriority.UseTextAlignment = false;
            this.lblProfitLoss.Text = "lblProfitLoss";
            this.lblProfitLoss.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.lblProfitLoss.Weight = 2D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Percentage", "{0:n2}")});
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(11, 0, 0, 0, 254F);
            this.xrTableCell3.StylePriority.UsePadding = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            xrSummary4.FormatString = "{0:n2}";
            this.xrTableCell3.Summary = xrSummary4;
            this.xrTableCell3.Text = "xrTableCell3";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell3.Weight = 1.25D;
            // 
            // fnBalanceENDLastMonth
            // 
            this.fnBalanceENDLastMonth.DataSource = this.bs;
            this.fnBalanceENDLastMonth.Expression = "Iif([AccountLevel]=1,[BalanceENDLastMonth],0 )";
            this.fnBalanceENDLastMonth.Name = "fnBalanceENDLastMonth";
            // 
            // fnProfitLoss
            // 
            this.fnProfitLoss.DataSource = this.bs;
            this.fnProfitLoss.Expression = "Iif([AccountLevel]=1,[ProfitLoss],0 )";
            this.fnProfitLoss.Name = "fnProfitLoss";
            // 
            // fnBalanceEND
            // 
            this.fnBalanceEND.DataSource = this.bs;
            this.fnBalanceEND.Expression = "Iif([AccountLevel]=1,[BalanceEND],0 )";
            this.fnBalanceEND.Name = "fnBalanceEND";
            // 
            // xrLine2
            // 
            this.xrLine2.Dpi = 254F;
            this.xrLine2.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine2.LineWidth = 3;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 63.5F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(1936.75F, 31.75F);
            // 
            // lblPeriod
            // 
            this.lblPeriod.Dpi = 254F;
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblPeriod.LocationFloat = new DevExpress.Utils.PointFloat(0F, 285.75F);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPeriod.SizeF = new System.Drawing.SizeF(1968.5F, 31.75F);
            this.lblPeriod.StylePriority.UseFont = false;
            this.lblPeriod.StylePriority.UseTextAlignment = false;
            this.lblPeriod.Text = "Periode : [cfBulan] [ParamYear] ";
            this.lblPeriod.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine1
            // 
            this.xrLine1.Dpi = 254F;
            this.xrLine1.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine1.LineWidth = 3;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(1936.75F, 31.75F);
            // 
            // fnProfitPembagi
            // 
            this.fnProfitPembagi.DataSource = this.bs;
            this.fnProfitPembagi.Expression = "Iif([AccountLevel]=1,([ProfitLoss]/[Pembagi]) * 100,0 )";
            this.fnProfitPembagi.Name = "fnProfitPembagi";
            // 
            // xrTable3
            // 
            this.xrTable3.Dpi = 254F;
            this.xrTable3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 31.75F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
            this.xrTable3.SizeF = new System.Drawing.SizeF(1936.75F, 31.75F);
            this.xrTable3.StylePriority.UseFont = false;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell7,
            this.xrTableCell8});
            this.xrTableRow3.Dpi = 254F;
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "Total Laba /(Rugi) : ";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell4.Weight = 1.7213115069240512D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "fnProfitLoss")});
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0:n2}";
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell7.Summary = xrSummary1;
            this.xrTableCell7.Text = "xrTableCell7";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell7.Weight = 0.78688527741585457D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "fnProfitPembagi")});
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            xrSummary2.FormatString = "{0:n2}";
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell8.Summary = xrSummary2;
            this.xrTableCell8.Text = "xrTableCell8";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell8.Weight = 0.49180321566009422D;
            // 
            // LLabaRugiBulanIniv2
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.PageHeader,
            this.PageFooter,
            this.Detail,
            this.ReportFooter,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.fnBalanceENDLastMonth,
            this.fnProfitLoss,
            this.fnBalanceEND,
            this.fnProfitPembagi});
            this.Margins = new System.Drawing.Printing.Margins(64, 66, 38, 38);
            this.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTableCell lblGLAccountNo;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell lblProfitLoss;
        private DevExpress.XtraReports.UI.CalculatedField fnBalanceENDLastMonth;
        private DevExpress.XtraReports.UI.CalculatedField fnProfitLoss;
        private DevExpress.XtraReports.UI.CalculatedField fnBalanceEND;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRLabel lblPeriod;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.CalculatedField fnProfitPembagi;
        private DevExpress.XtraReports.UI.XRTable xrTable3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell8;

    }
}
