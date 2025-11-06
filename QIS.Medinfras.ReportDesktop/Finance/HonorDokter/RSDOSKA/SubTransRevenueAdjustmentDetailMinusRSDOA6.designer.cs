namespace QIS.Medinfras.ReportDesktop
{
    partial class SubTransRevenueAdjustmentDetailMinusRSDOA6
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
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // lblReportProperties1
            // 
            this.lblReportProperties1.LocationFloat = new DevExpress.Utils.PointFloat(891.5184F, 0F);
            this.lblReportProperties1.SizeF = new System.Drawing.SizeF(5F, 13.92594F);
            this.lblReportProperties1.StylePriority.UseFont = false;
            this.lblReportProperties1.StylePriority.UsePadding = false;
            this.lblReportProperties1.StylePriority.UseTextAlignment = false;
            this.lblReportProperties1.Text = "";
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vTransRevenueSharingSummaryAdj);
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
            this.lblReportTitle.SizeF = new System.Drawing.SizeF(68.52518F, 5.512153F);
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
            this.ReportHeader.HeightF = 8.565648F;
            this.ReportHeader.Visible = false;
            // 
            // Detail
            // 
            this.Detail.Visible = false;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 33.02F;
            this.PageFooter.Visible = false;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "AdjustmentAmount")});
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(543.9401F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(337.8298F, 33.02F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0:n2}";
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel2.Summary = xrSummary1;
            this.xrLabel2.Text = "[AdjustmentAmount]";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel2,
            this.xrLabel1});
            this.GroupHeader1.Dpi = 254F;
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("RSAdjustmentType", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 33.02F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RSAdjustmentType")});
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(495.94F, 33.02F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "xrLabel4";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(495.94F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(3, 3, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(48F, 33.02F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "(-)";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // SubTransRevenueAdjustmentDetailMinusRSDOA6
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.GroupHeader1});
            this.DetailPrintCountOnEmptyDataSource = 0;
            this.Margins = new System.Drawing.Printing.Margins(30, 30, 0, 0);
            this.PageWidth = 1000;
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.GroupHeader1, 0);
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

        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
    }
}
