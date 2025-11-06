namespace QIS.Medinfras.ReportDesktop
{
    partial class BaseA6Rpt
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
            this.tableHeader = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cHealthcareName = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cHealthcareAddress = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cHealthcareCityZipCodes = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cHealthcarePhone = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblReportTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReportProperties1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.lblReportProperties2 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 15F;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tableHeader,
            this.xrLine2,
            this.lblReportTitle});
            this.ReportHeader.HeightF = 140.4949F;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 15F;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblReportProperties2,
            this.xrLine1,
            this.lblReportProperties1});
            this.PageFooter.HeightF = 55.56252F;
            // 
            // tableHeader
            // 
            this.tableHeader.Dpi = 254F;
            this.tableHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableHeader.LocationFloat = new DevExpress.Utils.PointFloat(13.22917F, 0F);
            this.tableHeader.Name = "tableHeader";
            this.tableHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.tableHeader.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow2,
            this.xrTableRow3,
            this.xrTableRow4});
            this.tableHeader.SizeF = new System.Drawing.SizeF(881.7705F, 89.42914F);
            this.tableHeader.StylePriority.UseFont = false;
            this.tableHeader.StylePriority.UsePadding = false;
            this.tableHeader.StylePriority.UseTextAlignment = false;
            this.tableHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
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
            this.cHealthcareName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cHealthcareName.Name = "cHealthcareName";
            this.cHealthcareName.StylePriority.UseFont = false;
            this.cHealthcareName.Text = "cHealthcareName";
            this.cHealthcareName.Weight = 3D;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cHealthcareAddress});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // cHealthcareAddress
            // 
            this.cHealthcareAddress.Dpi = 254F;
            this.cHealthcareAddress.Name = "cHealthcareAddress";
            this.cHealthcareAddress.Text = "cHealthcareAddress";
            this.cHealthcareAddress.Weight = 3D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cHealthcareCityZipCodes});
            this.xrTableRow3.Dpi = 254F;
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // cHealthcareCityZipCodes
            // 
            this.cHealthcareCityZipCodes.Dpi = 254F;
            this.cHealthcareCityZipCodes.Name = "cHealthcareCityZipCodes";
            this.cHealthcareCityZipCodes.Text = "cHealthcareCityZipCodes";
            this.cHealthcareCityZipCodes.Weight = 3D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cHealthcarePhone});
            this.xrTableRow4.Dpi = 254F;
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // cHealthcarePhone
            // 
            this.cHealthcarePhone.Dpi = 254F;
            this.cHealthcarePhone.Name = "cHealthcarePhone";
            this.cHealthcarePhone.Text = "cHealthcarePhone";
            this.cHealthcarePhone.Weight = 3D;
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.Dpi = 254F;
            this.lblReportTitle.Font = new System.Drawing.Font("Tahoma", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(13.22898F, 105.8874F);
            this.lblReportTitle.Name = "lblReportTitle";
            this.lblReportTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportTitle.SizeF = new System.Drawing.SizeF(881.7707F, 34.6075F);
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            this.lblReportTitle.Text = "lblReportTitle";
            this.lblReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblReportProperties1
            // 
            this.lblReportProperties1.Dpi = 254F;
            this.lblReportProperties1.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportProperties1.LocationFloat = new DevExpress.Utils.PointFloat(13.22917F, 13.22917F);
            this.lblReportProperties1.Name = "lblReportProperties1";
            this.lblReportProperties1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportProperties1.SizeF = new System.Drawing.SizeF(881.7707F, 21.16666F);
            this.lblReportProperties1.StylePriority.UseFont = false;
            this.lblReportProperties1.StylePriority.UsePadding = false;
            this.lblReportProperties1.StylePriority.UseTextAlignment = false;
            this.lblReportProperties1.Text = "MEDINFRAS - ReportID";
            this.lblReportProperties1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLine1
            // 
            this.xrLine1.Dpi = 254F;
            this.xrLine1.KeepTogether = false;
            this.xrLine1.LineStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.xrLine1.LineWidth = 3;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(13.22917F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(881.7707F, 13.22917F);
            // 
            // xrLine2
            // 
            this.xrLine2.Dpi = 254F;
            this.xrLine2.LineStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.xrLine2.LineWidth = 3;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(13.22898F, 92.07182F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(881.7707F, 13.81564F);
            // 
            // lblReportProperties2
            // 
            this.lblReportProperties2.Dpi = 254F;
            this.lblReportProperties2.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportProperties2.LocationFloat = new DevExpress.Utils.PointFloat(13.22917F, 34.39583F);
            this.lblReportProperties2.Name = "lblReportProperties2";
            this.lblReportProperties2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReportProperties2.SizeF = new System.Drawing.SizeF(881.7705F, 21.16669F);
            this.lblReportProperties2.StylePriority.UseFont = false;
            this.lblReportProperties2.StylePriority.UsePadding = false;
            this.lblReportProperties2.StylePriority.UseTextAlignment = false;
            this.lblReportProperties2.Text = "Print Date/Time : dd-MMM-yyyy HH:MM:ss, User ID : XXXXX";
            this.lblReportProperties2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // BaseA6Rpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.DataSource = this.bs;
            this.Margins = new System.Drawing.Printing.Margins(15, 15, 15, 15);
            this.PageHeight = 1400;
            this.PageWidth = 950;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ShowPrintMarginsWarning = false;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.tableHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell cHealthcareName;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell cHealthcareAddress;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell cHealthcareCityZipCodes;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell cHealthcarePhone;
        protected DevExpress.XtraReports.UI.XRLabel lblReportProperties1;
        protected System.Windows.Forms.BindingSource bs;
        protected DevExpress.XtraReports.UI.XRLabel lblReportProperties2;
        protected DevExpress.XtraReports.UI.XRLabel lblReportTitle;
        protected DevExpress.XtraReports.UI.XRTable tableHeader;
        protected DevExpress.XtraReports.UI.XRLine xrLine2;
        protected DevExpress.XtraReports.UI.XRLine xrLine1;
    }
}
