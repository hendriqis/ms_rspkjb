namespace QIS.Medinfras.ReportDesktop
{
    partial class BaseCustomDailyPotrait2Rpt
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
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.lblReportProperties = new DevExpress.XtraReports.UI.XRLabel();
            this.lineFooter = new DevExpress.XtraReports.UI.XRLine();
            this.xrPageInfo = new DevExpress.XtraReports.UI.XRPageInfo();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.tableHeader = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cHealthcareName = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cHealthcareAddress = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cEmail = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cHealthcareZipCode = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblReportTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReportSubTitle = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblReportSubTitle,
            this.lblReportTitle,
            this.tableHeader,
            this.xrLine1,
            this.xrLogo});
            this.ReportHeader.HeightF = 408.9635F;
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
            // xrLogo
            // 
            this.xrLogo.Dpi = 254F;
            this.xrLogo.ImageUrl = "/qislib/images/logo.png";
            this.xrLogo.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 25.00001F);
            this.xrLogo.Name = "xrLogo";
            this.xrLogo.SizeF = new System.Drawing.SizeF(227.3301F, 227.5417F);
            this.xrLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrLine1
            // 
            this.xrLine1.Dpi = 254F;
            this.xrLine1.LineWidth = 5;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(25.00017F, 265.7708F);
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
            // tableHeader
            // 
            this.tableHeader.Dpi = 254F;
            this.tableHeader.LocationFloat = new DevExpress.Utils.PointFloat(285.75F, 25.00001F);
            this.tableHeader.Name = "tableHeader";
            this.tableHeader.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6,
            this.xrTableRow7,
            this.xrTableRow8,
            this.xrTableRow9});
            this.tableHeader.SizeF = new System.Drawing.SizeF(1720.25F, 179.9167F);
            this.tableHeader.StylePriority.UseTextAlignment = false;
            this.tableHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cHealthcareName});
            this.xrTableRow6.Dpi = 254F;
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // cHealthcareName
            // 
            this.cHealthcareName.Dpi = 254F;
            this.cHealthcareName.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cHealthcareName.Name = "cHealthcareName";
            this.cHealthcareName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.cHealthcareName.StylePriority.UseFont = false;
            this.cHealthcareName.StylePriority.UsePadding = false;
            this.cHealthcareName.Text = "cHealthcareName";
            this.cHealthcareName.Weight = 3D;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cHealthcareAddress});
            this.xrTableRow7.Dpi = 254F;
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // cHealthcareAddress
            // 
            this.cHealthcareAddress.Dpi = 254F;
            this.cHealthcareAddress.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cHealthcareAddress.Name = "cHealthcareAddress";
            this.cHealthcareAddress.StylePriority.UseFont = false;
            this.cHealthcareAddress.Text = "cHealthcareAddress";
            this.cHealthcareAddress.Weight = 3D;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cEmail});
            this.xrTableRow8.Dpi = 254F;
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // cEmail
            // 
            this.cEmail.Dpi = 254F;
            this.cEmail.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cEmail.Multiline = true;
            this.cEmail.Name = "cEmail";
            this.cEmail.StylePriority.UseFont = false;
            this.cEmail.Text = "cEmail\r\ncEmail";
            this.cEmail.Weight = 3D;
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cHealthcareZipCode});
            this.xrTableRow9.Dpi = 254F;
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 1D;
            // 
            // cHealthcareZipCode
            // 
            this.cHealthcareZipCode.Dpi = 254F;
            this.cHealthcareZipCode.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cHealthcareZipCode.Multiline = true;
            this.cHealthcareZipCode.Name = "cHealthcareZipCode";
            this.cHealthcareZipCode.StylePriority.UseFont = false;
            this.cHealthcareZipCode.Text = "cHealthcareZipCode\r\n";
            this.cHealthcareZipCode.Weight = 3D;
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblReportTitle.Dpi = 254F;
            this.lblReportTitle.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(25.00017F, 315.83F);
            this.lblReportTitle.Name = "lblReportTitle";
            this.lblReportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportTitle.SizeF = new System.Drawing.SizeF(1981F, 43.9209F);
            this.lblReportTitle.StylePriority.UseBorders = false;
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            this.lblReportTitle.Text = "lblReportTitle";
            this.lblReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblReportSubTitle
            // 
            this.lblReportSubTitle.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblReportSubTitle.Dpi = 254F;
            this.lblReportSubTitle.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportSubTitle.LocationFloat = new DevExpress.Utils.PointFloat(25.00017F, 372.9801F);
            this.lblReportSubTitle.Name = "lblReportSubTitle";
            this.lblReportSubTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportSubTitle.SizeF = new System.Drawing.SizeF(1981F, 35.98346F);
            this.lblReportSubTitle.StylePriority.UseBorders = false;
            this.lblReportSubTitle.StylePriority.UseFont = false;
            this.lblReportSubTitle.StylePriority.UseTextAlignment = false;
            this.lblReportSubTitle.Text = "lblReportSubTitle";
            this.lblReportSubTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // BaseCustomDailyPotrait2Rpt
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
            ((System.ComponentModel.ISupportInitialize)(this.tableHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRPictureBox xrLogo;
        protected DevExpress.XtraReports.UI.XRPageInfo xrPageInfo;
        protected DevExpress.XtraReports.UI.XRLine lineFooter;
        protected DevExpress.XtraReports.UI.XRLabel lblReportProperties;
        protected System.Windows.Forms.BindingSource bs;
        protected DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRTable tableHeader;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell cHealthcareName;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow7;
        private DevExpress.XtraReports.UI.XRTableCell cHealthcareAddress;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow8;
        private DevExpress.XtraReports.UI.XRTableCell cEmail;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow9;
        private DevExpress.XtraReports.UI.XRTableCell cHealthcareZipCode;
        protected DevExpress.XtraReports.UI.XRLabel lblReportSubTitle;
        protected DevExpress.XtraReports.UI.XRLabel lblReportTitle;
    }
}
