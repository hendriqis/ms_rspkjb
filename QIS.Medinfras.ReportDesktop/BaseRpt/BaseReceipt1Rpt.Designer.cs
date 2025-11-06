namespace QIS.Medinfras.ReportDesktop
{
    partial class BaseReceipt1Rpt
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
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.lblReportSubTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReportTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReportProperties = new DevExpress.XtraReports.UI.XRLabel();
            this.lineFooter = new DevExpress.XtraReports.UI.XRLine();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 254F;
            this.BottomMargin.HeightF = 0F;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Dpi = 254F;
            // 
            // Detail
            // 
            this.Detail.Dpi = 254F;
            this.Detail.Expanded = false;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 254F;
            this.TopMargin.HeightF = 0F;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lineFooter,
            this.lblReportProperties});
            this.PageFooter.Dpi = 254F;
            this.PageFooter.HeightF = 87F;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblReportTitle,
            this.lblReportSubTitle});
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 103.1875F;
            this.PageHeader.Name = "PageHeader";
            // 
            // lblReportSubTitle
            // 
            this.lblReportSubTitle.Dpi = 254F;
            this.lblReportSubTitle.Font = new System.Drawing.Font("Courier New", 8F);
            this.lblReportSubTitle.LocationFloat = new DevExpress.Utils.PointFloat(4F, 58F);
            this.lblReportSubTitle.Name = "lblReportSubTitle";
            this.lblReportSubTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportSubTitle.SizeF = new System.Drawing.SizeF(932.0841F, 31.75003F);
            this.lblReportSubTitle.StylePriority.UseFont = false;
            this.lblReportSubTitle.StylePriority.UseTextAlignment = false;
            xrSummary2.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            this.lblReportSubTitle.Summary = xrSummary2;
            this.lblReportSubTitle.Text = "No. : TransactionNo";
            this.lblReportSubTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.Dpi = 254F;
            this.lblReportTitle.Font = new System.Drawing.Font("Courier New", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(4F, 10F);
            this.lblReportTitle.Name = "lblReportTitle";
            this.lblReportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportTitle.SizeF = new System.Drawing.SizeF(932.0841F, 42.42826F);
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            this.lblReportTitle.Summary = xrSummary1;
            this.lblReportTitle.Text = "NOTA RESEP";
            this.lblReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.Dpi = 254F;
            this.lblReportProperties.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportProperties.LocationFloat = new DevExpress.Utils.PointFloat(3.999998F, 16.74995F);
            this.lblReportProperties.Name = "lblReportProperties";
            this.lblReportProperties.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportProperties.SizeF = new System.Drawing.SizeF(932.0841F, 43.18005F);
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            this.lblReportProperties.Text = "MEDINFRAS - ReportID,  dd-MMM-yyyy HH:MM:ss, User ID : XXXXX";
            this.lblReportProperties.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lineFooter
            // 
            this.lineFooter.Dpi = 254F;
            this.lineFooter.LineWidth = 3;
            this.lineFooter.LocationFloat = new DevExpress.Utils.PointFloat(4F, 0F);
            this.lineFooter.Name = "lineFooter";
            this.lineFooter.SizeF = new System.Drawing.SizeF(932.5507F, 13.22917F);
            // 
            // BaseReceipt1Rpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.PageHeader});
            this.Dpi = 254F;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 1350;
            this.PageWidth = 950;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.SnapGridSize = 31.75F;
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        protected internal DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        protected internal DevExpress.XtraReports.UI.XRLabel lblReportTitle;
        protected internal DevExpress.XtraReports.UI.XRLabel lblReportSubTitle;
        protected internal DevExpress.XtraReports.UI.XRLine lineFooter;
        protected internal DevExpress.XtraReports.UI.XRLabel lblReportProperties;
    }
}
