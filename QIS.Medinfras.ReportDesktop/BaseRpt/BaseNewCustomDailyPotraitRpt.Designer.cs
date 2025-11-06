namespace QIS.Medinfras.ReportDesktop
{
    partial class BaseNewCustomDailyPotraitRpt
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
            this.xrLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.tableHeader = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cHealthcareName = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cHealthcareAddress = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cHealthcareCityZipCodes = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.cHealthcarePhone = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.cHealthcareFax = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.cHealthcareWebsite = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblReportTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.lblReportProperties = new DevExpress.XtraReports.UI.XRLabel();
            this.lineFooter = new DevExpress.XtraReports.UI.XRLine();
            this.xrPageInfo = new DevExpress.XtraReports.UI.XRPageInfo();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.lblReportSubTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo,
            this.lineFooter,
            this.lblReportProperties});
            // 
            // xrLogo
            // 
            this.xrLogo.Dpi = 254F;
            this.xrLogo.ImageUrl = "/qislib/images/logo.png";
            this.xrLogo.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 25.00001F);
            this.xrLogo.Name = "xrLogo";
            this.xrLogo.SizeF = new System.Drawing.SizeF(254F, 273.0502F);
            this.xrLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // tableHeader
            // 
            this.tableHeader.Dpi = 254F;
            this.tableHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableHeader.LocationFloat = new DevExpress.Utils.PointFloat(310.75F, 25.00001F);
            this.tableHeader.Name = "tableHeader";
            this.tableHeader.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow3,
            this.xrTableRow2,
            this.xrTableRow5,
            this.xrTableRow4,
            this.xrTableRow6});
            this.tableHeader.SizeF = new System.Drawing.SizeF(799.0414F, 273.0502F);
            this.tableHeader.StylePriority.UseFont = false;
            this.tableHeader.StylePriority.UseTextAlignment = false;
            this.tableHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cHealthcareName});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // cHealthcareName
            // 
            this.cHealthcareName.Dpi = 254F;
            this.cHealthcareName.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cHealthcareName.Name = "cHealthcareName";
            this.cHealthcareName.StylePriority.UseFont = false;
            this.cHealthcareName.Text = "cHealthcareName";
            this.cHealthcareName.Weight = 3D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cHealthcareAddress});
            this.xrTableRow3.Dpi = 254F;
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // cHealthcareAddress
            // 
            this.cHealthcareAddress.Dpi = 254F;
            this.cHealthcareAddress.Name = "cHealthcareAddress";
            this.cHealthcareAddress.Text = "cHealthcareAddress";
            this.cHealthcareAddress.Weight = 3D;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cHealthcareCityZipCodes});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // cHealthcareCityZipCodes
            // 
            this.cHealthcareCityZipCodes.Dpi = 254F;
            this.cHealthcareCityZipCodes.Name = "cHealthcareCityZipCodes";
            this.cHealthcareCityZipCodes.Text = "cHealthcareCityZipCodes";
            this.cHealthcareCityZipCodes.Weight = 3D;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell8,
            this.cHealthcarePhone});
            this.xrTableRow5.Dpi = 254F;
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Text = "Phone";
            this.xrTableCell7.Weight = 0.51757723076146889D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = ":";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell8.Weight = 0.11069962577913099D;
            // 
            // cHealthcarePhone
            // 
            this.cHealthcarePhone.Dpi = 254F;
            this.cHealthcarePhone.Name = "cHealthcarePhone";
            this.cHealthcarePhone.Text = "cHealthcarePhone";
            this.cHealthcarePhone.Weight = 2.3717231434593997D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.cHealthcareFax});
            this.xrTableRow4.Dpi = 254F;
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "Fax";
            this.xrTableCell1.Weight = 0.51757723076146889D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = ":";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.11069962577913099D;
            // 
            // cHealthcareFax
            // 
            this.cHealthcareFax.Dpi = 254F;
            this.cHealthcareFax.Name = "cHealthcareFax";
            this.cHealthcareFax.Text = "cHealthcareFax";
            this.cHealthcareFax.Weight = 2.3717231434593997D;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell4,
            this.cHealthcareWebsite});
            this.xrTableRow6.Dpi = 254F;
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Text = "Website";
            this.xrTableCell3.Weight = 0.51757723076146889D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = ":";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.11069962577913099D;
            // 
            // cHealthcareWebsite
            // 
            this.cHealthcareWebsite.Dpi = 254F;
            this.cHealthcareWebsite.Name = "cHealthcareWebsite";
            this.cHealthcareWebsite.Text = "cHealthcareWebsite";
            this.cHealthcareWebsite.Weight = 2.3717231434593997D;
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right)));
            this.lblReportTitle.Dpi = 254F;
            this.lblReportTitle.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(1143F, 25.00001F);
            this.lblReportTitle.Name = "lblReportTitle";
            this.lblReportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportTitle.SizeF = new System.Drawing.SizeF(863F, 102.1293F);
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
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 298.0502F);
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
            this.lblReportSubTitle.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.lblReportSubTitle.Dpi = 254F;
            this.lblReportSubTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportSubTitle.LocationFloat = new DevExpress.Utils.PointFloat(1143F, 127.1294F);
            this.lblReportSubTitle.Name = "lblReportSubTitle";
            this.lblReportSubTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportSubTitle.SizeF = new System.Drawing.SizeF(863F, 62.44176F);
            this.lblReportSubTitle.StylePriority.UseBorders = false;
            this.lblReportSubTitle.StylePriority.UseFont = false;
            this.lblReportSubTitle.StylePriority.UseTextAlignment = false;
            this.lblReportSubTitle.Text = "lblReportSubTitle";
            this.lblReportSubTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLogo,
            this.tableHeader,
            this.lblReportTitle,
            this.xrLine1,
            this.lblReportSubTitle});
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 401.2376F;
            this.PageHeader.Name = "PageHeader";
            // 
            // BaseNewCustomDailyPotraitRpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.PageHeader});
            this.DataSource = this.bs;
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.tableHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRTable tableHeader;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell cHealthcareName;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell cHealthcareAddress;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell cHealthcareCityZipCodes;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell8;
        private DevExpress.XtraReports.UI.XRTableCell cHealthcarePhone;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell cHealthcareFax;
        private DevExpress.XtraReports.UI.XRPictureBox xrLogo;
        protected DevExpress.XtraReports.UI.XRPageInfo xrPageInfo;
        protected DevExpress.XtraReports.UI.XRLine lineFooter;
        protected DevExpress.XtraReports.UI.XRLabel lblReportProperties;
        protected System.Windows.Forms.BindingSource bs;
        protected DevExpress.XtraReports.UI.XRLine xrLine1;
        protected DevExpress.XtraReports.UI.XRLabel lblReportTitle;
        protected DevExpress.XtraReports.UI.XRLabel lblReportSubTitle;
        protected DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell cHealthcareWebsite;
    }
}
