namespace QIS.Medinfras.ReportDesktop
{
    partial class BaseCustomDailyPotraitRSRTRpt
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
            this.lblReportTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.lblReportProperties = new DevExpress.XtraReports.UI.XRLabel();
            this.lineFooter = new DevExpress.XtraReports.UI.XRLine();
            this.xrPageInfo = new DevExpress.XtraReports.UI.XRPageInfo();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.lblReportSubTitle = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblReportSubTitle,
            this.xrLine1,
            this.lblReportTitle});
            this.ReportHeader.HeightF = 321.3333F;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 32.25001F;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo,
            this.lineFooter,
            this.lblReportProperties});
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblReportTitle.Dpi = 254F;
            this.lblReportTitle.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(25.00015F, 90.61669F);
            this.lblReportTitle.Name = "lblReportTitle";
            this.lblReportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportTitle.SizeF = new System.Drawing.SizeF(1981F, 102.1293F);
            this.lblReportTitle.StylePriority.UseBorders = false;
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            this.lblReportTitle.Text = "lblReportTitle";
            this.lblReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine1
            // 
            this.xrLine1.Dpi = 254F;
            this.xrLine1.LineWidth = 3;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(25.00028F, 265.3242F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(1981F, 39.47585F);
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.Dpi = 254F;
            this.lblReportProperties.Font = new System.Drawing.Font("Tahoma", 7F);
            this.lblReportProperties.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 31.75F);
            this.lblReportProperties.Name = "lblReportProperties";
            this.lblReportProperties.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportProperties.SizeF = new System.Drawing.SizeF(1562.288F, 31.75F);
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            this.lblReportProperties.Text = "MEDINFRAS - ReportID | ReportCaption, Print Date/Time : dd-MMM-yyyy HH:MM:ss, Use" +
                "r ID : XXXXX";
            this.lblReportProperties.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lineFooter
            // 
            this.lineFooter.Dpi = 254F;
            this.lineFooter.LineWidth = 3;
            this.lineFooter.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 0F);
            this.lineFooter.Name = "lineFooter";
            this.lineFooter.SizeF = new System.Drawing.SizeF(1981F, 31.74993F);
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.Dpi = 254F;
            this.xrPageInfo.Font = new System.Drawing.Font("Tahoma", 7F);
            this.xrPageInfo.Format = "Page {0} of {1}";
            this.xrPageInfo.LocationFloat = new DevExpress.Utils.PointFloat(1651F, 31.75F);
            this.xrPageInfo.Name = "xrPageInfo";
            this.xrPageInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrPageInfo.SizeF = new System.Drawing.SizeF(355.0001F, 31.75F);
            this.xrPageInfo.StylePriority.UseFont = false;
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
            this.xrPageInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblReportSubTitle
            // 
            this.lblReportSubTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblReportSubTitle.Dpi = 254F;
            this.lblReportSubTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportSubTitle.LocationFloat = new DevExpress.Utils.PointFloat(25.00022F, 192.746F);
            this.lblReportSubTitle.Name = "lblReportSubTitle";
            this.lblReportSubTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportSubTitle.SizeF = new System.Drawing.SizeF(1981F, 62.44177F);
            this.lblReportSubTitle.StylePriority.UseBorders = false;
            this.lblReportSubTitle.StylePriority.UseFont = false;
            this.lblReportSubTitle.StylePriority.UseTextAlignment = false;
            this.lblReportSubTitle.Text = "lblReportSubTitle";
            this.lblReportSubTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // BaseCustomDailyPotraitRSRTRpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.DataSource = this.bs;
            this.Margins = new System.Drawing.Printing.Margins(64, 64, 32, 64);
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        protected DevExpress.XtraReports.UI.XRPageInfo xrPageInfo;
        protected DevExpress.XtraReports.UI.XRLine lineFooter;
        protected DevExpress.XtraReports.UI.XRLabel lblReportProperties;
        protected System.Windows.Forms.BindingSource bs;
        protected DevExpress.XtraReports.UI.XRLine xrLine1;
        protected DevExpress.XtraReports.UI.XRLabel lblReportTitle;
        protected DevExpress.XtraReports.UI.XRLabel lblReportSubTitle;
    }
}
