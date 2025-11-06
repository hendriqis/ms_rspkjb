namespace QIS.Medinfras.ReportDesktop
{
    partial class BaseLegalRpt
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
            this.xrLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.lblHealthcareName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPhoneFaxNo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAddressLine1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAddressLine2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReportTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReportSubtitle = new DevExpress.XtraReports.UI.XRLabel();
            this.lineFooter = new DevExpress.XtraReports.UI.XRLine();
            this.lblReportProperties = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo3 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 25F;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 25F;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lineFooter,
            this.lblReportProperties,
            this.xrPageInfo3});
            this.PageFooter.HeightF = 87.3125F;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLogo,
            this.lblHealthcareName,
            this.lblPhoneFaxNo,
            this.lblAddressLine1,
            this.lblAddressLine2,
            this.lblReportTitle,
            this.lblReportSubtitle});
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 359.8333F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrLogo
            // 
            this.xrLogo.Dpi = 254F;
            this.xrLogo.ImageUrl = "/qislib/images/logo.png";
            this.xrLogo.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 13.22917F);
            this.xrLogo.Name = "xrLogo";
            this.xrLogo.SizeF = new System.Drawing.SizeF(193.1458F, 222.9908F);
            this.xrLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // lblHealthcareName
            // 
            this.lblHealthcareName.Dpi = 254F;
            this.lblHealthcareName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHealthcareName.LocationFloat = new DevExpress.Utils.PointFloat(218.1458F, 13.22917F);
            this.lblHealthcareName.Name = "lblHealthcareName";
            this.lblHealthcareName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblHealthcareName.SizeF = new System.Drawing.SizeF(1307.042F, 63.5F);
            this.lblHealthcareName.StylePriority.UseFont = false;
            this.lblHealthcareName.StylePriority.UsePadding = false;
            this.lblHealthcareName.StylePriority.UseTextAlignment = false;
            this.lblHealthcareName.Text = "Healthcare Name";
            this.lblHealthcareName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblPhoneFaxNo
            // 
            this.lblPhoneFaxNo.Dpi = 254F;
            this.lblPhoneFaxNo.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblPhoneFaxNo.LocationFloat = new DevExpress.Utils.PointFloat(1568.987F, 13.22917F);
            this.lblPhoneFaxNo.Name = "lblPhoneFaxNo";
            this.lblPhoneFaxNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPhoneFaxNo.SizeF = new System.Drawing.SizeF(466.5132F, 43.18002F);
            this.lblPhoneFaxNo.StylePriority.UseFont = false;
            this.lblPhoneFaxNo.StylePriority.UsePadding = false;
            this.lblPhoneFaxNo.StylePriority.UseTextAlignment = false;
            this.lblPhoneFaxNo.Text = "Phone";
            this.lblPhoneFaxNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblAddressLine1
            // 
            this.lblAddressLine1.Dpi = 254F;
            this.lblAddressLine1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblAddressLine1.LocationFloat = new DevExpress.Utils.PointFloat(218.1459F, 76.72916F);
            this.lblAddressLine1.Name = "lblAddressLine1";
            this.lblAddressLine1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAddressLine1.SizeF = new System.Drawing.SizeF(1307.042F, 31.75F);
            this.lblAddressLine1.StylePriority.UseFont = false;
            this.lblAddressLine1.StylePriority.UsePadding = false;
            this.lblAddressLine1.StylePriority.UseTextAlignment = false;
            this.lblAddressLine1.Text = "Address Line 1";
            this.lblAddressLine1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblAddressLine2
            // 
            this.lblAddressLine2.Dpi = 254F;
            this.lblAddressLine2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblAddressLine2.LocationFloat = new DevExpress.Utils.PointFloat(218.1458F, 108.4792F);
            this.lblAddressLine2.Name = "lblAddressLine2";
            this.lblAddressLine2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAddressLine2.SizeF = new System.Drawing.SizeF(1307.042F, 31.74999F);
            this.lblAddressLine2.StylePriority.UseFont = false;
            this.lblAddressLine2.StylePriority.UsePadding = false;
            this.lblAddressLine2.StylePriority.UseTextAlignment = false;
            this.lblAddressLine2.Text = "Address Line 2";
            this.lblAddressLine2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.Dpi = 254F;
            this.lblReportTitle.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.lblReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 264.5833F);
            this.lblReportTitle.Name = "lblReportTitle";
            this.lblReportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportTitle.SizeF = new System.Drawing.SizeF(2010.5F, 63.5F);
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            this.lblReportTitle.Text = "Report Title";
            this.lblReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblReportSubtitle
            // 
            this.lblReportSubtitle.Dpi = 254F;
            this.lblReportSubtitle.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblReportSubtitle.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 328.0833F);
            this.lblReportSubtitle.Name = "lblReportSubtitle";
            this.lblReportSubtitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportSubtitle.SizeF = new System.Drawing.SizeF(2010.5F, 31.75F);
            this.lblReportSubtitle.StylePriority.UseFont = false;
            this.lblReportSubtitle.StylePriority.UseTextAlignment = false;
            this.lblReportSubtitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lineFooter
            // 
            this.lineFooter.Dpi = 254F;
            this.lineFooter.LineWidth = 3;
            this.lineFooter.LocationFloat = new DevExpress.Utils.PointFloat(24.99993F, 0F);
            this.lineFooter.Name = "lineFooter";
            this.lineFooter.SizeF = new System.Drawing.SizeF(2010.501F, 31.74993F);
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.Dpi = 254F;
            this.lblReportProperties.Font = new System.Drawing.Font("Tahoma", 7F);
            this.lblReportProperties.LocationFloat = new DevExpress.Utils.PointFloat(24.99993F, 31.75F);
            this.lblReportProperties.Name = "lblReportProperties";
            this.lblReportProperties.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportProperties.SizeF = new System.Drawing.SizeF(1613.959F, 31.75F);
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            this.lblReportProperties.Text = "MEDINFRAS - ReportID, Print Date/Time : dd-MMM-yyyy HH:MM:ss, User ID : XXXXX";
            this.lblReportProperties.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPageInfo3
            // 
            this.xrPageInfo3.Dpi = 254F;
            this.xrPageInfo3.Font = new System.Drawing.Font("Tahoma", 7F);
            this.xrPageInfo3.Format = "Page {0} of {1}";
            this.xrPageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(1680.959F, 31.75F);
            this.xrPageInfo3.Name = "xrPageInfo3";
            this.xrPageInfo3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrPageInfo3.SizeF = new System.Drawing.SizeF(354.5415F, 31.75F);
            this.xrPageInfo3.StylePriority.UseFont = false;
            this.xrPageInfo3.StylePriority.UseTextAlignment = false;
            this.xrPageInfo3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // BaseLegalRpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.PageHeader});
            this.DataSource = this.bs;
            this.DetailPrintCountOnEmptyDataSource = 0;
            this.Margins = new System.Drawing.Printing.Margins(25, 25, 25, 25);
            this.PageHeight = 3300;
            this.PageWidth = 2100;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.PrintOnEmptyDataSource = false;
            this.ShowPrintMarginsWarning = false;
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

        protected DevExpress.XtraReports.UI.XRLine lineFooter;
        protected DevExpress.XtraReports.UI.XRLabel lblReportProperties;
        protected DevExpress.XtraReports.UI.XRPageInfo xrPageInfo3;
        private DevExpress.XtraReports.UI.XRPictureBox xrLogo;
        private DevExpress.XtraReports.UI.XRLabel lblHealthcareName;
        private DevExpress.XtraReports.UI.XRLabel lblPhoneFaxNo;
        private DevExpress.XtraReports.UI.XRLabel lblAddressLine1;
        private DevExpress.XtraReports.UI.XRLabel lblAddressLine2;
        protected DevExpress.XtraReports.UI.XRLabel lblReportTitle;
        protected DevExpress.XtraReports.UI.XRLabel lblReportSubtitle;
        protected DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        protected System.Windows.Forms.BindingSource bs;
    }
}
