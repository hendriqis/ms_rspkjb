namespace QIS.Medinfras.ReportDesktop
{
    partial class BPemesananBarangKonsinyasiTanpaNilai
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
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.total = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.lblCreatedByName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblApprovedByName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSIPANo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPurchaseOrderNo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSupplierCode = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSupplierName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPurchaseOrderDate = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel13,
            this.xrLabel12,
            this.lblPurchaseOrderNo,
            this.lblPurchaseOrderDate,
            this.lblSupplierName,
            this.lblSupplierCode,
            this.xrLabel7,
            this.xrLabel3,
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel8,
            this.xrTable2});
            this.PageHeader.HeightF = 571.5F;
            this.PageHeader.Controls.SetChildIndex(this.lblReportSubtitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrTable2, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel8, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel9, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel10, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel11, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel3, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel7, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblSupplierCode, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblSupplierName, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblPurchaseOrderDate, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblPurchaseOrderNo, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel12, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel13, 0);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vPurchaseOrderDt);
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.Font = new System.Drawing.Font("Courier New", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.lblReportSubtitle.Visible = false;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail.HeightF = 63.5F;
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ItemCode", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ItemName1", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.Detail.StylePriority.UseTextAlignment = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.Dpi = 254F;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(1968.5F, 63.5F);
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell3,
            this.xrTableCell2,
            this.xrTableCell5});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseBorders = false;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell4.Summary = xrSummary1;
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell4.Weight = 0.0967742033927671D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ItemCode")});
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(13, 0, 0, 0, 254F);
            this.xrTableCell3.StylePriority.UseBorders = false;
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.StylePriority.UsePadding = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "xrTableCell3";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell3.Weight = 0.43548385127898187D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ItemName1")});
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(13, 0, 0, 0, 254F);
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "xrTableCell2";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell2.Weight = 1.7903225609564013D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomPurchaseUnit")});
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseBorders = false;
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "xrTableCell5";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell5.Weight = 0.67741938437184968D;
            // 
            // total
            // 
            this.total.Expression = "([Price] * [RequestQty])-([Price] * [RequestQty]*[Discount] / 100)";
            this.total.Name = "total";
            // 
            // xrTable2
            // 
            this.xrTable2.Dpi = 254F;
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 508F);
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
            this.xrTableCell15,
            this.xrTableCell14});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "No";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell9.Weight = 0.25D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(13, 0, 0, 0, 254F);
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "#Item";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell1.Weight = 1.125D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Padding = new DevExpress.XtraPrinting.PaddingInfo(13, 0, 0, 0, 254F);
            this.xrTableCell15.StylePriority.UseBorders = false;
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.StylePriority.UsePadding = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.Text = "Nama Item";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell15.Weight = 4.625D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell14.Dpi = 254F;
            this.xrTableCell14.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StylePriority.UseBorders = false;
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.Text = "Qty";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell14.Weight = 1.75D;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine1,
            this.xrLabel5,
            this.xrLabel6,
            this.xrLine2,
            this.xrLine3,
            this.lblCreatedByName,
            this.lblApprovedByName,
            this.lblSIPANo});
            this.GroupFooter1.Dpi = 254F;
            this.GroupFooter1.HeightF = 505F;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.StylePriority.UseBorders = false;
            // 
            // xrLine1
            // 
            this.xrLine1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLine1.Dpi = 254F;
            this.xrLine1.LineWidth = 3;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(1968.5F, 31.75F);
            this.xrLine1.StylePriority.UseBorders = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(63.5F, 95.25F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(476.25F, 63.50002F);
            this.xrLabel5.StylePriority.UseBorders = false;
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Dibuat Oleh :";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel6.Dpi = 254F;
            this.xrLabel6.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(1460.5F, 95.25F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(508F, 63.50002F);
            this.xrLabel6.StylePriority.UseBorders = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Disetujui Oleh :";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine2
            // 
            this.xrLine2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLine2.Dpi = 254F;
            this.xrLine2.LineWidth = 3;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(1460.5F, 317.5F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(508F, 5.079987F);
            this.xrLine2.StylePriority.UseBorders = false;
            // 
            // xrLine3
            // 
            this.xrLine3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLine3.Dpi = 254F;
            this.xrLine3.LineWidth = 3;
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(63.5F, 317.5F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(476.25F, 5.079987F);
            this.xrLine3.StylePriority.UseBorders = false;
            // 
            // lblCreatedByName
            // 
            this.lblCreatedByName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblCreatedByName.Dpi = 254F;
            this.lblCreatedByName.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreatedByName.LocationFloat = new DevExpress.Utils.PointFloat(63.5F, 254F);
            this.lblCreatedByName.Name = "lblCreatedByName";
            this.lblCreatedByName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblCreatedByName.SizeF = new System.Drawing.SizeF(476.25F, 63.50003F);
            this.lblCreatedByName.StylePriority.UseBorders = false;
            this.lblCreatedByName.StylePriority.UseFont = false;
            this.lblCreatedByName.StylePriority.UseTextAlignment = false;
            this.lblCreatedByName.Text = "CreatedByName";
            this.lblCreatedByName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            // 
            // lblApprovedByName
            // 
            this.lblApprovedByName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblApprovedByName.Dpi = 254F;
            this.lblApprovedByName.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApprovedByName.LocationFloat = new DevExpress.Utils.PointFloat(1460.5F, 254F);
            this.lblApprovedByName.Name = "lblApprovedByName";
            this.lblApprovedByName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblApprovedByName.SizeF = new System.Drawing.SizeF(508F, 63.5F);
            this.lblApprovedByName.StylePriority.UseBorders = false;
            this.lblApprovedByName.StylePriority.UseFont = false;
            this.lblApprovedByName.StylePriority.UseTextAlignment = false;
            this.lblApprovedByName.Text = "ApprovedByName";
            this.lblApprovedByName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            // 
            // lblSIPANo
            // 
            this.lblSIPANo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblSIPANo.Dpi = 254F;
            this.lblSIPANo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSIPANo.LocationFloat = new DevExpress.Utils.PointFloat(1460.5F, 322.58F);
            this.lblSIPANo.Name = "lblSIPANo";
            this.lblSIPANo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblSIPANo.SizeF = new System.Drawing.SizeF(508F, 63.5F);
            this.lblSIPANo.StylePriority.UseBorders = false;
            this.lblSIPANo.StylePriority.UseFont = false;
            this.lblSIPANo.StylePriority.UseTextAlignment = false;
            this.lblSIPANo.Text = "NomorSIPA";
            this.lblSIPANo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Dpi = 254F;
            this.xrLabel11.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(1333.5F, 412.75F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = ":";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(952.5F, 381F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(381F, 31.75F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Tanggal Pemesanan";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Dpi = 254F;
            this.xrLabel7.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(8.89004F, 349.25F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(149.8599F, 31.75F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "Kepada";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Dpi = 254F;
            this.xrLabel8.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(952.5F, 349.25F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(381F, 31.75F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "No Pemesanan";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Dpi = 254F;
            this.xrLabel9.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(952.5F, 412.75F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(381F, 31.75F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "Kode Supplier";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Dpi = 254F;
            this.xrLabel10.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(1333.5F, 349.25F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = ":";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblPurchaseOrderNo
            // 
            this.lblPurchaseOrderNo.Dpi = 254F;
            this.lblPurchaseOrderNo.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPurchaseOrderNo.LocationFloat = new DevExpress.Utils.PointFloat(1365.25F, 349.25F);
            this.lblPurchaseOrderNo.Name = "lblPurchaseOrderNo";
            this.lblPurchaseOrderNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPurchaseOrderNo.SizeF = new System.Drawing.SizeF(603.25F, 31.75F);
            this.lblPurchaseOrderNo.StylePriority.UseFont = false;
            this.lblPurchaseOrderNo.StylePriority.UseTextAlignment = false;
            this.lblPurchaseOrderNo.Text = "[PurchaseOrderNo]";
            this.lblPurchaseOrderNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Dpi = 254F;
            this.xrLabel12.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(158.75F, 349.25F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = ":";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Dpi = 254F;
            this.xrLabel13.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(1333.5F, 381F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = ":";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblSupplierCode
            // 
            this.lblSupplierCode.Dpi = 254F;
            this.lblSupplierCode.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSupplierCode.LocationFloat = new DevExpress.Utils.PointFloat(1365.25F, 412.75F);
            this.lblSupplierCode.Name = "lblSupplierCode";
            this.lblSupplierCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblSupplierCode.SizeF = new System.Drawing.SizeF(603.25F, 31.75F);
            this.lblSupplierCode.StylePriority.UseFont = false;
            this.lblSupplierCode.StylePriority.UseTextAlignment = false;
            this.lblSupplierCode.Text = "[SupplierCode]";
            this.lblSupplierCode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblSupplierName
            // 
            this.lblSupplierName.Dpi = 254F;
            this.lblSupplierName.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSupplierName.LocationFloat = new DevExpress.Utils.PointFloat(8.89001F, 391.16F);
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblSupplierName.SizeF = new System.Drawing.SizeF(603.25F, 53.34003F);
            this.lblSupplierName.StylePriority.UseFont = false;
            this.lblSupplierName.StylePriority.UseTextAlignment = false;
            this.lblSupplierName.Text = "[SupplierName]";
            this.lblSupplierName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblPurchaseOrderDate
            // 
            this.lblPurchaseOrderDate.Dpi = 254F;
            this.lblPurchaseOrderDate.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPurchaseOrderDate.LocationFloat = new DevExpress.Utils.PointFloat(1365.25F, 381F);
            this.lblPurchaseOrderDate.Name = "lblPurchaseOrderDate";
            this.lblPurchaseOrderDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPurchaseOrderDate.SizeF = new System.Drawing.SizeF(603.25F, 31.75F);
            this.lblPurchaseOrderDate.StylePriority.UseFont = false;
            this.lblPurchaseOrderDate.StylePriority.UseTextAlignment = false;
            this.lblPurchaseOrderDate.Text = "[PurchaseOrderDate]";
            this.lblPurchaseOrderDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BPemesananBarangKonsinyasiTanpaNilai
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.PageHeader,
            this.GroupFooter1,
            this.PageFooter,
            this.TopMargin,
            this.ReportFooter,
            this.BottomMargin,
            this.ReportHeader});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.total});
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.ReportFooter, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.GroupFooter1, 0);
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.CalculatedField total;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell15;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell14;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLabel lblPurchaseOrderNo;
        private DevExpress.XtraReports.UI.XRLabel lblPurchaseOrderDate;
        private DevExpress.XtraReports.UI.XRLabel lblSupplierName;
        private DevExpress.XtraReports.UI.XRLabel lblSupplierCode;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRLine xrLine3;
        private DevExpress.XtraReports.UI.XRLabel lblCreatedByName;
        private DevExpress.XtraReports.UI.XRLabel lblApprovedByName;
        private DevExpress.XtraReports.UI.XRLabel lblSIPANo;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
    }
}
