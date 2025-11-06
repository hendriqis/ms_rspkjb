namespace QIS.Medinfras.ReportDesktop
{
    partial class BaseCustomA6Rpt
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
            this.components = new System.ComponentModel.Container();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.lblReportProperties = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo = new DevExpress.XtraReports.UI.XRPageInfo();
            this.lineFooter = new DevExpress.XtraReports.UI.XRLine();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 15F;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 15F;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lineFooter,
            this.xrPageInfo,
            this.lblReportProperties});
            this.PageFooter.HeightF = 76.41663F;
            // 
            // PageHeader
            // 
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 50.76499F;
            this.PageHeader.Name = "PageHeader";
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.Dpi = 254F;
            this.lblReportProperties.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportProperties.LocationFloat = new DevExpress.Utils.PointFloat(0F, 19.40272F);
            this.lblReportProperties.Name = "lblReportProperties";
            this.lblReportProperties.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportProperties.SizeF = new System.Drawing.SizeF(713.3335F, 31.75F);
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            this.lblReportProperties.Text = "MEDINFRAS - ReportID, Print Date/Time : dd-MMM-yyyy HH:MM:ss, User ID : XXXXX";
            this.lblReportProperties.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.Dpi = 254F;
            this.xrPageInfo.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo.Format = "Page {0} of {1}";
            this.xrPageInfo.LocationFloat = new DevExpress.Utils.PointFloat(862.791F, 19.40272F);
            this.xrPageInfo.Name = "xrPageInfo";
            this.xrPageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrPageInfo.SizeF = new System.Drawing.SizeF(151.271F, 31.75F);
            this.xrPageInfo.StylePriority.UseFont = false;
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
            this.xrPageInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lineFooter
            // 
            this.lineFooter.Dpi = 254F;
            this.lineFooter.LineWidth = 3;
            this.lineFooter.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lineFooter.Name = "lineFooter";
            this.lineFooter.SizeF = new System.Drawing.SizeF(1014.062F, 19.40271F);
            // 
            // BaseCustomA6Rpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.PageHeader});
            this.DataSource = this.bs;
            this.Margins = new System.Drawing.Printing.Margins(16, 26, 15, 15);
            this.PageHeight = 1400;
            this.PageWidth = 1069;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        protected System.Windows.Forms.BindingSource bs;
        public DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        protected DevExpress.XtraReports.UI.XRLine lineFooter;
        protected DevExpress.XtraReports.UI.XRPageInfo xrPageInfo;
        protected DevExpress.XtraReports.UI.XRLabel lblReportProperties;
    }
}
