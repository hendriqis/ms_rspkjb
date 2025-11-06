namespace QIS.Medinfras.ReportDesktop
{
    partial class BSuratKeteranganSehat4
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
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.txtDateOfBirth = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.txtGender = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.lblStatement = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblTBBB = new DevExpress.XtraReports.UI.XRLabel();
            this.lblWt = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTekananDarah = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblType = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblType,
            this.xrLabel3,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel5,
            this.lblTekananDarah,
            this.xrLabel4,
            this.lblWt,
            this.lblTBBB,
            this.xrTable1,
            this.xrLabel2});
            this.PageHeader.HeightF = 948.0902F;
            this.PageHeader.Controls.SetChildIndex(this.xrLabel2, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrTable1, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblTBBB, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblWt, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel4, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblTekananDarah, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel5, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel6, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel7, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel8, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel9, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel10, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel3, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblType, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblReportSubtitle, 0);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vPatient);
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 182.5F);
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
            this.lblReportSubtitle.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportSubtitle.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 254F);
            this.lblReportSubtitle.StylePriority.UseFont = false;
            this.lblReportSubtitle.StylePriority.UseTextAlignment = false;
            this.lblReportSubtitle.Visible = false;
            // 
            // Detail
            // 
            this.Detail.Expanded = false;
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
            this.GroupFooter1.HeightF = 293.6875F;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.StylePriority.UseBorders = false;
            // 
            // lblPrintDate
            // 
            this.lblPrintDate.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPrintDate.Dpi = 254F;
            this.lblPrintDate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintDate.KeepTogether = true;
            this.lblPrintDate.LocationFloat = new DevExpress.Utils.PointFloat(1460.5F, 0F);
            this.lblPrintDate.Name = "lblPrintDate";
            this.lblPrintDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPrintDate.SizeF = new System.Drawing.SizeF(476.25F, 31.75F);
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
            this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(1460.5F, 63.5F);
            this.xrLabel35.Name = "xrLabel35";
            this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel35.SizeF = new System.Drawing.SizeF(476.25F, 31.75F);
            this.xrLabel35.StylePriority.UseBorders = false;
            this.xrLabel35.StylePriority.UseFont = false;
            this.xrLabel35.StylePriority.UseTextAlignment = false;
            this.xrLabel35.Text = "Dokter Pemeriksa";
            this.xrLabel35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblParamedicName
            // 
            this.lblParamedicName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblParamedicName.Dpi = 254F;
            this.lblParamedicName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParamedicName.KeepTogether = true;
            this.lblParamedicName.LocationFloat = new DevExpress.Utils.PointFloat(1460.5F, 222.25F);
            this.lblParamedicName.Name = "lblParamedicName";
            this.lblParamedicName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblParamedicName.SizeF = new System.Drawing.SizeF(476.25F, 31.75F);
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
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 317.5F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(1968.5F, 31.75F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Dengan ini kami menerangkan bahwa :";
            // 
            // xrTable1
            // 
            this.xrTable1.Dpi = 254F;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(95.25F, 381F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow6,
            this.xrTableRow2,
            this.xrTableRow3,
            this.xrTableRow4});
            this.xrTable1.SizeF = new System.Drawing.SizeF(1905F, 317.5F);
            this.xrTable1.StylePriority.UsePadding = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell6});
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
            // xrTableCell6
            // 
            this.xrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PatientName")});
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Weight = 2.1875000620118428D;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell4,
            this.txtDateOfBirth});
            this.xrTableRow6.Dpi = 254F;
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Text = "Tgl Lahir / Umur";
            this.xrTableCell3.Weight = 0.77620964641343337D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = ":";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell4.Weight = 0.070564485123110865D;
            // 
            // txtDateOfBirth
            // 
            this.txtDateOfBirth.Dpi = 254F;
            this.txtDateOfBirth.Name = "txtDateOfBirth";
            this.txtDateOfBirth.Text = "txtDateOfBirth";
            this.txtDateOfBirth.Weight = 2.1875000620118428D;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5,
            this.xrTableCell7,
            this.txtGender});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Text = "Jenis Kelamin";
            this.xrTableCell5.Weight = 0.77620964641343337D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = ":";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell7.Weight = 0.070564485123110865D;
            // 
            // txtGender
            // 
            this.txtGender.Dpi = 254F;
            this.txtGender.Name = "txtGender";
            this.txtGender.Text = "txtGender";
            this.txtGender.Weight = 2.1875000620118428D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell10,
            this.xrTableCell11,
            this.xrTableCell12});
            this.xrTableRow3.Dpi = 254F;
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.Text = "Alamat";
            this.xrTableCell10.Weight = 0.77620964641343337D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = ":";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell11.Weight = 0.070564485123110865D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "HomeAddress")});
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Text = "xrTableCell12";
            this.xrTableCell12.Weight = 2.1875000620118428D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.lblStatement});
            this.xrTableRow4.Dpi = 254F;
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // lblStatement
            // 
            this.lblStatement.Dpi = 254F;
            this.lblStatement.Name = "lblStatement";
            this.lblStatement.Text = "lblStatement";
            this.lblStatement.Weight = 3.034274193548387D;
            // 
            // lblTBBB
            // 
            this.lblTBBB.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblTBBB.Dpi = 254F;
            this.lblTBBB.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTBBB.KeepTogether = true;
            this.lblTBBB.LocationFloat = new DevExpress.Utils.PointFloat(476.25F, 857.25F);
            this.lblTBBB.Name = "lblTBBB";
            this.lblTBBB.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTBBB.SizeF = new System.Drawing.SizeF(1524F, 31.75F);
            this.lblTBBB.StylePriority.UseBorders = false;
            this.lblTBBB.StylePriority.UseFont = false;
            this.lblTBBB.StylePriority.UseTextAlignment = false;
            this.lblTBBB.Text = "lblTBBB";
            this.lblTBBB.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblWt
            // 
            this.lblWt.Dpi = 254F;
            this.lblWt.LocationFloat = new DevExpress.Utils.PointFloat(476.25F, 825.5F);
            this.lblWt.Name = "lblWt";
            this.lblWt.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblWt.SizeF = new System.Drawing.SizeF(1524F, 31.75F);
            this.lblWt.Text = "lblWt";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 730.25F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(349.25F, 31.75F);
            this.xrLabel4.Text = "Catatan :";
            // 
            // lblTekananDarah
            // 
            this.lblTekananDarah.Dpi = 254F;
            this.lblTekananDarah.LocationFloat = new DevExpress.Utils.PointFloat(476.25F, 889F);
            this.lblTekananDarah.Name = "lblTekananDarah";
            this.lblTekananDarah.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTekananDarah.SizeF = new System.Drawing.SizeF(1524F, 31.75F);
            this.lblTekananDarah.Text = "lblTekananDarah";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(95.25F, 825.5F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(349.25F, 31.75F);
            this.xrLabel5.Text = "Berat Badan";
            // 
            // xrLabel6
            // 
            this.xrLabel6.Dpi = 254F;
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(95.25F, 857.25F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(349.25F, 31.75F);
            this.xrLabel6.Text = "Tinggi Badan";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Dpi = 254F;
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(95.25F, 889F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(349.25F, 31.75F);
            this.xrLabel7.Text = "Tekanan Darah";
            // 
            // xrLabel8
            // 
            this.xrLabel8.Dpi = 254F;
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(444.5F, 825.5F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = ":";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Dpi = 254F;
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(444.5F, 857.25F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = ":";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Dpi = 254F;
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(444.5F, 889F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = ":";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(1301.75F, 730.25F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(190.5F, 63.5F);
            this.xrLabel3.Text = "Buta Warna:";
            // 
            // lblType
            // 
            this.lblType.Dpi = 254F;
            this.lblType.LocationFloat = new DevExpress.Utils.PointFloat(1492.25F, 730.25F);
            this.lblType.Name = "lblType";
            this.lblType.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblType.SizeF = new System.Drawing.SizeF(508F, 31.75F);
            // 
            // BSuratKeteranganSehat4
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
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell11;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell txtDateOfBirth;
        private DevExpress.XtraReports.UI.XRLabel lblTBBB;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell txtGender;
        private DevExpress.XtraReports.UI.XRLabel lblWt;
        private DevExpress.XtraReports.UI.XRLabel lblTekananDarah;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell lblStatement;
        private DevExpress.XtraReports.UI.XRLabel lblType;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
    }
}
