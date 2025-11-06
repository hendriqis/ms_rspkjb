namespace QIS.Medinfras.ReportDesktop
{
    partial class BaseCustomReceipt2Rpt
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
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.lblReportSubtitle = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAddressLine2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAddressLine1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPhoneFaxNo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblHealthcareName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReportTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReportProperties = new DevExpress.XtraReports.UI.XRLabel();
            this.lineFooter = new DevExpress.XtraReports.UI.XRLine();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 254F;
            this.BottomMargin.HeightF = 64F;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Dpi = 254F;
            // 
            // Detail
            // 
            this.Detail.Dpi = 254F;
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 254F;
            this.TopMargin.HeightF = 64F;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lineFooter,
            this.lblReportProperties});
            this.PageFooter.Dpi = 254F;
            this.PageFooter.HeightF = 75F;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblReportSubtitle,
            this.lblAddressLine2,
            this.lblAddressLine1,
            this.lblPhoneFaxNo,
            this.lblHealthcareName,
            this.lblReportTitle});
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 256.9375F;
            this.PageHeader.Name = "PageHeader";
            // 
            // lblReportSubtitle
            // 
            this.lblReportSubtitle.Dpi = 254F;
            this.lblReportSubtitle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblReportSubtitle.LocationFloat = new DevExpress.Utils.PointFloat(13.75F, 215.25F);
            this.lblReportSubtitle.Name = "lblReportSubtitle";
            this.lblReportSubtitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportSubtitle.SizeF = new System.Drawing.SizeF(929.5695F, 31.74998F);
            this.lblReportSubtitle.StylePriority.UseFont = false;
            this.lblReportSubtitle.StylePriority.UseTextAlignment = false;
            this.lblReportSubtitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblAddressLine2
            // 
            this.lblAddressLine2.Dpi = 254F;
            this.lblAddressLine2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblAddressLine2.LocationFloat = new DevExpress.Utils.PointFloat(6.350004F, 63.5F);
            this.lblAddressLine2.Name = "lblAddressLine2";
            this.lblAddressLine2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAddressLine2.SizeF = new System.Drawing.SizeF(939.2708F, 31.75F);
            this.lblAddressLine2.StylePriority.UseFont = false;
            this.lblAddressLine2.StylePriority.UsePadding = false;
            this.lblAddressLine2.StylePriority.UseTextAlignment = false;
            this.lblAddressLine2.Text = "Address Line 2";
            this.lblAddressLine2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblAddressLine1
            // 
            this.lblAddressLine1.Dpi = 254F;
            this.lblAddressLine1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblAddressLine1.LocationFloat = new DevExpress.Utils.PointFloat(6.350004F, 31.75F);
            this.lblAddressLine1.Name = "lblAddressLine1";
            this.lblAddressLine1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAddressLine1.SizeF = new System.Drawing.SizeF(939.2708F, 31.75F);
            this.lblAddressLine1.StylePriority.UseFont = false;
            this.lblAddressLine1.StylePriority.UsePadding = false;
            this.lblAddressLine1.StylePriority.UseTextAlignment = false;
            this.lblAddressLine1.Text = "Address Line 1";
            this.lblAddressLine1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblPhoneFaxNo
            // 
            this.lblPhoneFaxNo.Dpi = 254F;
            this.lblPhoneFaxNo.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblPhoneFaxNo.LocationFloat = new DevExpress.Utils.PointFloat(7.5F, 97F);
            this.lblPhoneFaxNo.Name = "lblPhoneFaxNo";
            this.lblPhoneFaxNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPhoneFaxNo.SizeF = new System.Drawing.SizeF(635F, 43.18002F);
            this.lblPhoneFaxNo.StylePriority.UseFont = false;
            this.lblPhoneFaxNo.StylePriority.UsePadding = false;
            this.lblPhoneFaxNo.StylePriority.UseTextAlignment = false;
            this.lblPhoneFaxNo.Text = "Phone";
            this.lblPhoneFaxNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblHealthcareName
            // 
            this.lblHealthcareName.Dpi = 254F;
            this.lblHealthcareName.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblHealthcareName.LocationFloat = new DevExpress.Utils.PointFloat(6.350004F, 0F);
            this.lblHealthcareName.Name = "lblHealthcareName";
            this.lblHealthcareName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblHealthcareName.SizeF = new System.Drawing.SizeF(939.2708F, 31.75F);
            this.lblHealthcareName.StylePriority.UseFont = false;
            this.lblHealthcareName.StylePriority.UsePadding = false;
            this.lblHealthcareName.StylePriority.UseTextAlignment = false;
            this.lblHealthcareName.Text = "Healthcare Name";
            this.lblHealthcareName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.Dpi = 254F;
            this.lblReportTitle.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.lblReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(13.75F, 150.75F);
            this.lblReportTitle.Name = "lblReportTitle";
            this.lblReportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportTitle.SizeF = new System.Drawing.SizeF(929.5695F, 63.5F);
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            this.lblReportTitle.Text = "Report Title";
            this.lblReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.Dpi = 254F;
            this.lblReportProperties.Font = new System.Drawing.Font("Tahoma", 7F);
            this.lblReportProperties.LocationFloat = new DevExpress.Utils.PointFloat(0F, 31.75F);
            this.lblReportProperties.Name = "lblReportProperties";
            this.lblReportProperties.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportProperties.SizeF = new System.Drawing.SizeF(944.5625F, 31.75F);
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            this.lblReportProperties.Text = "ReportID, Print Date/Time : dd-MMM-yyyy HH:MM:ss, User ID : XXXXX";
            this.lblReportProperties.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lineFooter
            // 
            this.lineFooter.Dpi = 254F;
            this.lineFooter.LineWidth = 3;
            this.lineFooter.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lineFooter.Name = "lineFooter";
            this.lineFooter.SizeF = new System.Drawing.SizeF(940.75F, 31.74998F);
            // 
            // ReportFooter
            // 
            this.ReportFooter.Dpi = 254F;
            this.ReportFooter.HeightF = 0F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // BaseCustomReceipt2Rpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.PageHeader,
            this.PageFooter,
            this.Detail,
            this.ReportFooter,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.DataSource = this.bs;
            this.Dpi = 254F;
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 64, 64);
            this.PageHeight = 1350;
            this.PageWidth = 950;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.SnapGridSize = 31.75F;
            this.SnappingMode = DevExpress.XtraReports.UI.SnappingMode.SnapToGrid;
            this.SnapToGrid = false;
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.ReportFooter, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.PageHeader, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        protected DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        protected System.Windows.Forms.BindingSource bs;
        protected DevExpress.XtraReports.UI.XRLabel lblReportTitle;
        protected DevExpress.XtraReports.UI.XRLabel lblAddressLine1;
        protected DevExpress.XtraReports.UI.XRLabel lblPhoneFaxNo;
        protected DevExpress.XtraReports.UI.XRLabel lblAddressLine2;
        protected DevExpress.XtraReports.UI.XRLabel lblReportSubtitle;
        protected DevExpress.XtraReports.UI.XRLine lineFooter;
        protected DevExpress.XtraReports.UI.XRLabel lblReportProperties;
        protected DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        protected DevExpress.XtraReports.UI.XRLabel lblHealthcareName;
    }
}
