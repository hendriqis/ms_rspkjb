namespace QIS.Medinfras.ReportDesktop
{
    partial class LKadaluarsa
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
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.total = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.lblPeriod = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblPeriod,
            this.xrTable2});
            this.PageHeader.HeightF = 444.5F;
            this.PageHeader.Controls.SetChildIndex(this.lblReportSubtitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrTable2, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblPeriod, 0);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetItemProductExpiredDate);
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            // 
            // xrPageInfo3
            // 
            this.xrPageInfo3.StylePriority.UseFont = false;
            this.xrPageInfo3.StylePriority.UseTextAlignment = false;
            // 
            // lblReportParameterTitle
            // 
            this.lblReportParameterTitle.StylePriority.UseFont = false;
            // 
            // lblParameter7
            // 
            this.lblParameter7.StylePriority.UseFont = false;
            // 
            // lblParameter3
            // 
            this.lblParameter3.StylePriority.UseFont = false;
            // 
            // lblParameter6
            // 
            this.lblParameter6.StylePriority.UseFont = false;
            // 
            // lblParameter5
            // 
            this.lblParameter5.StylePriority.UseFont = false;
            // 
            // lblParameter4
            // 
            this.lblParameter4.StylePriority.UseFont = false;
            // 
            // lblParameter2
            // 
            this.lblParameter2.StylePriority.UseFont = false;
            // 
            // lblParameter1
            // 
            this.lblParameter1.StylePriority.UseFont = false;
            // 
            // lblParameter0
            // 
            this.lblParameter0.StylePriority.UseFont = false;
            // 
            // lblReportSubtitle
            // 
            this.lblReportSubtitle.StylePriority.UseFont = false;
            this.lblReportSubtitle.StylePriority.UseTextAlignment = false;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine2,
            this.xrTable1});
            this.Detail.HeightF = 63.5F;
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ItemName1", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // xrTable1
            // 
            this.xrTable1.Dpi = 254F;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(1968.5F, 31.75F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell2,
            this.xrTableCell5,
            this.xrTableCell7});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell4.StylePriority.UseBorders = false;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UsePadding = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell4.Summary = xrSummary1;
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.14516130016696066D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ItemCode")});
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "xrTableCell2";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell2.Weight = 0.33870960629952279D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ItemName1")});
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell5.StylePriority.UseBorders = false;
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UsePadding = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "xrTableCell5";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell5.Weight = 0.77419357028502889D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ExpiredDate")});
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell7.StylePriority.UseBorders = false;
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.StylePriority.UsePadding = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "xrTableCell7";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell7.Weight = 1.7419355232484881D;
            this.xrTableCell7.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell7_BeforePrint);
            // 
            // total
            // 
            this.total.Expression = "([Price] * [RequestQty])-([Price] * [RequestQty]*[Discount] / 100)";
            this.total.Name = "total";
            // 
            // xrTable2
            // 
            this.xrTable2.Dpi = 254F;
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 381F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(1968.5F, 63.5F);
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrTableCell1,
            this.xrTableCell14,
            this.xrTableCell10});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UsePadding = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "No";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell9.Weight = 0.375D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Kode Item";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell1.Weight = 0.875D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell14.Dpi = 254F;
            this.xrTableCell14.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell14.StylePriority.UseBorders = false;
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.StylePriority.UsePadding = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.Text = "Nama Item";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell14.Weight = 2D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell10.StylePriority.UseBorders = false;
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UsePadding = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Informasi Kadaluarsa";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell10.Weight = 4.5D;
            // 
            // xrLine2
            // 
            this.xrLine2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrLine2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLine2.Dpi = 254F;
            this.xrLine2.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine2.LineWidth = 3;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 31.75F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(1968.5F, 31.75F);
            this.xrLine2.StylePriority.UseBorderDashStyle = false;
            this.xrLine2.StylePriority.UseBorders = false;
            // 
            // lblPeriod
            // 
            this.lblPeriod.Dpi = 254F;
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblPeriod.LocationFloat = new DevExpress.Utils.PointFloat(0F, 285.75F);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPeriod.SizeF = new System.Drawing.SizeF(1968.5F, 31.75F);
            this.lblPeriod.StylePriority.UseFont = false;
            this.lblPeriod.StylePriority.UseTextAlignment = false;
            this.lblPeriod.Text = "Periode : [StartDate!dd-MMM-yyyy] s/d  [EndDate!dd-MMM-yyyy] ";
            this.lblPeriod.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.GroupHeader1.Dpi = 254F;
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("GCItemType", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 71.4375F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ItemType")});
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 31.75F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(1968.5F, 31.75F);
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // LKadaluarsa
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.PageHeader,
            this.PageFooter,
            this.Detail,
            this.ReportFooter,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.GroupHeader1});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.total});
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.GroupHeader1, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.ReportFooter, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.PageHeader, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.CalculatedField total;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell14;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRLabel lblPeriod;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
    }
}
