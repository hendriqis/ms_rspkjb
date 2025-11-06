namespace QIS.Medinfras.ReportDesktop
{
    partial class SubTransRegistrationDetailRSDOA6
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
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // lblReportProperties1
            // 
            this.lblReportProperties1.LocationFloat = new DevExpress.Utils.PointFloat(732.7684F, 0F);
            this.lblReportProperties1.SizeF = new System.Drawing.SizeF(6.842712F, 13.92594F);
            this.lblReportProperties1.StylePriority.UseFont = false;
            this.lblReportProperties1.StylePriority.UsePadding = false;
            this.lblReportProperties1.StylePriority.UseTextAlignment = false;
            this.lblReportProperties1.Text = "";
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetTRSSummaryDtTransRegistrationDetailRSDO);
            // 
            // lblReportProperties2
            // 
            this.lblReportProperties2.LocationFloat = new DevExpress.Utils.PointFloat(90.04379F, 0F);
            this.lblReportProperties2.SizeF = new System.Drawing.SizeF(5F, 13.92594F);
            this.lblReportProperties2.StylePriority.UseFont = false;
            this.lblReportProperties2.StylePriority.UsePadding = false;
            this.lblReportProperties2.StylePriority.UseTextAlignment = false;
            this.lblReportProperties2.Text = "";
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(25F, 0F);
            this.lblReportTitle.SizeF = new System.Drawing.SizeF(68.52518F, 7.131291F);
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            this.lblReportTitle.Text = "";
            // 
            // tableHeader
            // 
            this.tableHeader.LocationFloat = new DevExpress.Utils.PointFloat(13.22954F, 0F);
            this.tableHeader.SizeF = new System.Drawing.SizeF(8.565648F, 7.688851F);
            this.tableHeader.StylePriority.UseFont = false;
            this.tableHeader.StylePriority.UsePadding = false;
            this.tableHeader.StylePriority.UseTextAlignment = false;
            // 
            // xrLine2
            // 
            this.xrLine2.ForeColor = System.Drawing.Color.Transparent;
            this.xrLine2.LineWidth = 0;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(5.000009F, 0F);
            this.xrLine2.SizeF = new System.Drawing.SizeF(8.565648F, 8.565648F);
            // 
            // xrLine1
            // 
            this.xrLine1.ForeColor = System.Drawing.Color.Transparent;
            this.xrLine1.LineWidth = 0;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(18.31394F, 0F);
            this.xrLine1.SizeF = new System.Drawing.SizeF(5F, 13.92594F);
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            // 
            // ReportHeader
            // 
            this.ReportHeader.BorderColor = System.Drawing.Color.Transparent;
            this.ReportHeader.HeightF = 8.565648F;
            this.ReportHeader.StylePriority.UseBorderColor = false;
            this.ReportHeader.Visible = false;
            // 
            // Detail
            // 
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("RevenueSharingNameDt", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 15.76861F;
            this.PageFooter.Visible = false;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Dpi = 254F;
            this.GroupFooter1.HeightF = 0F;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.Visible = false;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.GroupHeader1.Dpi = 254F;
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("RevenueSharingName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 33.02F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RevenueSharingNameGroup")});
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(881.77F, 33.02F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "[RevenueSharingNameGroup]";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RevenueSharingNameDt")});
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(482.0644F, 33.02F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "xrLabel2";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel2});
            this.GroupHeader2.Dpi = 254F;
            this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("RevenueSharingNameDt", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader2.HeightF = 33.02F;
            this.GroupHeader2.Name = "GroupHeader2";
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "BrutoAmount")});
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(482.0644F, 0F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(399.7056F, 33.02F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0:n2}";
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel4.Summary = xrSummary1;
            this.xrLabel4.Text = "xrLabel3";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // SubTransRegistrationDetailRSDOA6
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.GroupFooter1,
            this.GroupHeader1,
            this.GroupHeader2});
            this.DetailPrintCountOnEmptyDataSource = 0;
            this.Margins = new System.Drawing.Printing.Margins(30, 30, 0, 0);
            this.PageWidth = 1000;
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.GroupHeader2, 0);
            this.Controls.SetChildIndex(this.GroupHeader1, 0);
            this.Controls.SetChildIndex(this.GroupFooter1, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
    }
}
