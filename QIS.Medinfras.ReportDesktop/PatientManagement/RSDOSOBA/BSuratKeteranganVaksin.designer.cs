namespace QIS.Medinfras.ReportDesktop
{
    partial class BSuratKeteranganVaksin
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
            this.total = new DevExpress.XtraReports.UI.CalculatedField();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.lblPrintDate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblParamedicName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblName = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblDOB = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblAddress = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblVaksin1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblVaksin2 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblVaksin2,
            this.xrLabel5,
            this.lblVaksin1,
            this.xrTable1,
            this.xrLabel2});
            this.PageHeader.HeightF = 790.2222F;
            this.PageHeader.Controls.SetChildIndex(this.xrLabel2, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrTable1, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblReportSubtitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblVaksin1, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel5, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblVaksin2, 0);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vPurchaseOrderDt);
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 190.5F);
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            this.lblReportTitle.Text = "SURAT KETERANGAN VAKSIN";
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
            this.lblReportSubtitle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportSubtitle.LocationFloat = new DevExpress.Utils.PointFloat(127F, 285.75F);
            this.lblReportSubtitle.SizeF = new System.Drawing.SizeF(1746.25F, 31.75F);
            this.lblReportSubtitle.StylePriority.UseFont = false;
            this.lblReportSubtitle.StylePriority.UseTextAlignment = false;
            this.lblReportSubtitle.Visible = false;
            // 
            // Detail
            // 
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ItemName1", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // PageFooter
            // 
            this.PageFooter.Visible = false;
            // 
            // total
            // 
            this.total.Expression = "([Price] * [RequestQty])-([Price] * [RequestQty]*[Discount] / 100)";
            this.total.Name = "total";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblPrintDate,
            this.xrLabel35,
            this.lblParamedicName});
            this.GroupFooter1.Dpi = 254F;
            this.GroupFooter1.HeightF = 224.8958F;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.StylePriority.UseBorders = false;
            // 
            // lblPrintDate
            // 
            this.lblPrintDate.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPrintDate.Dpi = 254F;
            this.lblPrintDate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintDate.KeepTogether = true;
            this.lblPrintDate.LocationFloat = new DevExpress.Utils.PointFloat(1270F, 0F);
            this.lblPrintDate.Name = "lblPrintDate";
            this.lblPrintDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPrintDate.SizeF = new System.Drawing.SizeF(603.25F, 31.75F);
            this.lblPrintDate.StylePriority.UseBorders = false;
            this.lblPrintDate.StylePriority.UseFont = false;
            this.lblPrintDate.StylePriority.UseTextAlignment = false;
            this.lblPrintDate.Text = "lblPrintDate";
            this.lblPrintDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel35
            // 
            this.xrLabel35.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel35.Dpi = 254F;
            this.xrLabel35.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel35.KeepTogether = true;
            this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(1270F, 31.75F);
            this.xrLabel35.Name = "xrLabel35";
            this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel35.SizeF = new System.Drawing.SizeF(603.25F, 31.75F);
            this.xrLabel35.StylePriority.UseBorders = false;
            this.xrLabel35.StylePriority.UseFont = false;
            this.xrLabel35.StylePriority.UseTextAlignment = false;
            this.xrLabel35.Text = "DOKTER YANG MEMERIKSA";
            this.xrLabel35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblParamedicName
            // 
            this.lblParamedicName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblParamedicName.Dpi = 254F;
            this.lblParamedicName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParamedicName.KeepTogether = true;
            this.lblParamedicName.LocationFloat = new DevExpress.Utils.PointFloat(1270F, 190.5F);
            this.lblParamedicName.Name = "lblParamedicName";
            this.lblParamedicName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblParamedicName.SizeF = new System.Drawing.SizeF(603.25F, 31.75F);
            this.lblParamedicName.StylePriority.UseBorders = false;
            this.lblParamedicName.StylePriority.UseFont = false;
            this.lblParamedicName.StylePriority.UseTextAlignment = false;
            this.lblParamedicName.Text = "lblParamedicName";
            this.lblParamedicName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomSubTotal")});
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(650F, 12.5F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(125F, 25F);
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0:n2}";
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrLabel1.Summary = xrSummary1;
            this.xrLabel1.Text = "xrLabel1";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(127F, 317.5F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(1746.25F, 31.75F);
            this.xrLabel2.Text = "Dengan ini menerangkan bahwa :";
            // 
            // xrTable1
            // 
            this.xrTable1.Dpi = 254F;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(254F, 381F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(13, 13, 0, 0, 254F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow2,
            this.xrTableRow6});
            this.xrTable1.SizeF = new System.Drawing.SizeF(1619.25F, 190.5F);
            this.xrTable1.StylePriority.UsePadding = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.lblName});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "Nama";
            this.xrTableCell1.Weight = 0.77620964641343337D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = ":";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell2.Weight = 0.070564485123110865D;
            // 
            // lblName
            // 
            this.lblName.Dpi = 254F;
            this.lblName.Name = "lblName";
            this.lblName.Weight = 3.5282258684634558D;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell8,
            this.lblDOB});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Text = "Umur";
            this.xrTableCell7.Weight = 0.77620964641343337D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = ":";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell8.Weight = 0.070564485123110865D;
            // 
            // lblDOB
            // 
            this.lblDOB.Dpi = 254F;
            this.lblDOB.Name = "lblDOB";
            this.lblDOB.Weight = 3.5282258684634558D;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell15,
            this.xrTableCell17,
            this.lblAddress});
            this.xrTableRow6.Dpi = 254F;
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Text = "Alamat";
            this.xrTableCell15.Weight = 0.77620964641343337D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Dpi = 254F;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.Text = ":";
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell17.Weight = 0.070564485123110865D;
            // 
            // lblAddress
            // 
            this.lblAddress.Dpi = 254F;
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Weight = 3.5282258684634558D;
            // 
            // lblVaksin1
            // 
            this.lblVaksin1.Dpi = 254F;
            this.lblVaksin1.LocationFloat = new DevExpress.Utils.PointFloat(127F, 603.25F);
            this.lblVaksin1.Name = "lblVaksin1";
            this.lblVaksin1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblVaksin1.SizeF = new System.Drawing.SizeF(1746.25F, 31.75F);
            // 
            // xrLabel5
            // 
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(127F, 730.25F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(1746.25F, 31.75F);
            this.xrLabel5.Text = "Demikian surat keterangan ini dibuat untuk dipergunakan seperlunya.";
            // 
            // lblVaksin2
            // 
            this.lblVaksin2.Dpi = 254F;
            this.lblVaksin2.LocationFloat = new DevExpress.Utils.PointFloat(127F, 666.75F);
            this.lblVaksin2.Name = "lblVaksin2";
            this.lblVaksin2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblVaksin2.SizeF = new System.Drawing.SizeF(1746.25F, 31.75F);
            // 
            // BSuratKeteranganVaksin
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.PageHeader,
            this.GroupFooter1,
            this.PageFooter,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.ReportFooter});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.total});
            this.ShowPrintMarginsWarning = false;
            this.ShowPrintStatusDialog = false;
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.ReportFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.GroupFooter1, 0);
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.CalculatedField total;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel lblParamedicName;
        private DevExpress.XtraReports.UI.XRLabel lblPrintDate;
        private DevExpress.XtraReports.UI.XRLabel xrLabel35;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell8;
        private DevExpress.XtraReports.UI.XRTableCell lblName;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell15;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell17;
        private DevExpress.XtraReports.UI.XRTableCell lblAddress;
        private DevExpress.XtraReports.UI.XRTableCell lblDOB;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel lblVaksin1;
        private DevExpress.XtraReports.UI.XRLabel lblVaksin2;
    }
}
