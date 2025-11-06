namespace QIS.Medinfras.ReportDesktop
{
    partial class BSerahTerimaCSSD
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
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCheckBox1 = new DevExpress.XtraReports.UI.XRCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.PageHeader.HeightF = 349.25F;
            this.PageHeader.Controls.SetChildIndex(this.lblPhoneFaxNo, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrTable2, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblReportSubtitle, 0);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vMDServiceRequestHandover);
            // 
            // lblReportParameter
            // 
            this.lblReportParameter.StylePriority.UseFont = false;
            // 
            // lblPhoneFaxNo
            // 
            this.lblPhoneFaxNo.StylePriority.UseFont = false;
            this.lblPhoneFaxNo.StylePriority.UsePadding = false;
            this.lblPhoneFaxNo.StylePriority.UseTextAlignment = false;
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.Font = new System.Drawing.Font("Courier New", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(158.75F, 127F);
            this.lblReportTitle.SizeF = new System.Drawing.SizeF(2667F, 63.5F);
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            // 
            // lblReportSubtitle
            // 
            this.lblReportSubtitle.LocationFloat = new DevExpress.Utils.PointFloat(1746.25F, 63.5F);
            this.lblReportSubtitle.SizeF = new System.Drawing.SizeF(222.25F, 31.75F);
            this.lblReportSubtitle.StylePriority.UseFont = false;
            this.lblReportSubtitle.StylePriority.UseTextAlignment = false;
            // 
            // xrPageInfo3
            // 
            this.xrPageInfo3.StylePriority.UseFont = false;
            this.xrPageInfo3.StylePriority.UseTextAlignment = false;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail.HeightF = 63.5F;
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ItemName1", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // total
            // 
            this.total.Expression = "([Price] * [RequestQty])-([Price] * [RequestQty]*[Discount] / 100)";
            this.total.Name = "total";
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Dpi = 254F;
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 254F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(2825.75F, 95.25003F);
            this.xrTable2.StylePriority.UseBorders = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrTableCell15,
            this.xrTableCell14,
            this.xrTableCell1,
            this.xrTableCell3,
            this.xrTableCell10,
            this.xrTableCell12,
            this.xrTableCell13,
            this.xrTableCell16});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "No";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell9.Weight = 0.25D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell15.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell15.StylePriority.UseBorders = false;
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.Text = "No. Permintaan";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell15.Weight = 1.5104165640684402D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell14.Dpi = 254F;
            this.xrTableCell14.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.xrTableCell14.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell14.StylePriority.UseBorders = false;
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.StylePriority.UsePadding = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.Text = "No. Kirim";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell14.Weight = 1.4895830984373755D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Item";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell1.Weight = 2.62499970705504D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell3.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell3.StylePriority.UseBorders = false;
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Dikirim Oleh\r\nDikirim Tanggal";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell3.Weight = 1.3749998690522303D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell10.Multiline = true;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell10.StylePriority.UseBorders = false;
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Diterima Oleh\r\nDiterima Tanggal";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell10.Weight = 1.3749999338511785D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell12.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell12.StylePriority.UseBorders = false;
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "Pre-Cleaning Volume";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell12.Weight = 0.99999992170122054D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell13.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell13.Dpi = 254F;
            this.xrTableCell13.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell13.StylePriority.UseBorders = false;
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.Text = "Pre-Cleaning Condition";
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell13.Weight = 0.99999990145177686D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell16.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell16.Dpi = 254F;
            this.xrTableCell16.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell16.Multiline = true;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell16.StylePriority.UseBorders = false;
            this.xrTableCell16.StylePriority.UseFont = false;
            this.xrTableCell16.StylePriority.UseTextAlignment = false;
            this.xrTableCell16.Text = "Check\r\nList";
            this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell16.Weight = 0.50000100438273742D;
            // 
            // xrTable1
            // 
            this.xrTable1.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTable1.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTable1.Dpi = 254F;
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(2825.75F, 63.5F);
            this.xrTable1.StylePriority.UseBorderDashStyle = false;
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell7,
            this.xrTableCell6,
            this.xrTableCell8,
            this.xrTableCell18,
            this.xrTableCell19,
            this.xrTableCell20});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Font = new System.Drawing.Font("Courier New", 9F);
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseBorders = false;
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell2.Summary = xrSummary1;
            this.xrTableCell2.Text = "No";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell2.Weight = 0.25D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RequestNo")});
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Font = new System.Drawing.Font("Courier New", 9F);
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseBorders = false;
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "No. Permintaan";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell4.Weight = 1.4999998582524463D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DistributionNo")});
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Font = new System.Drawing.Font("Courier New", 9F);
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.xrTableCell5.StylePriority.UseBorders = false;
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UsePadding = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "No. Kirim";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell5.Weight = 1.4999996240314668D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PackageName")});
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell7.Weight = 2.6249998872769424D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.Font = new System.Drawing.Font("Courier New", 9F);
            this.xrTableCell6.Multiline = true;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseBorders = false;
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "[SentByName]\r\n[cfSendDateInString]";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell6.Weight = 1.3749998690522303D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Font = new System.Drawing.Font("Courier New", 9F);
            this.xrTableCell8.Multiline = true;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseBorders = false;
            this.xrTableCell8.StylePriority.UseFont = false;
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = "[ReceivedByName]\r\n[cfReceivedDateInString]";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell8.Weight = 1.3749999338511785D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PreWashingVolume")});
            this.xrTableCell18.Dpi = 254F;
            this.xrTableCell18.Font = new System.Drawing.Font("Courier New", 9F);
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseBorders = false;
            this.xrTableCell18.StylePriority.UseFont = false;
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.Text = "Pre-Cleaning Volume";
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell18.Weight = 0.99999992170122054D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PreWashingCondition")});
            this.xrTableCell19.Dpi = 254F;
            this.xrTableCell19.Font = new System.Drawing.Font("Courier New", 9F);
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseBorders = false;
            this.xrTableCell19.StylePriority.UseFont = false;
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.Text = "Pre-Cleaning Condition";
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell19.Weight = 0.99999990145177686D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell20.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrCheckBox1});
            this.xrTableCell20.Dpi = 254F;
            this.xrTableCell20.Font = new System.Drawing.Font("Courier New", 9F);
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseBorders = false;
            this.xrTableCell20.StylePriority.UseFont = false;
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell20.Weight = 0.50000100438273742D;
            // 
            // xrCheckBox1
            // 
            this.xrCheckBox1.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrCheckBox1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrCheckBox1.Dpi = 254F;
            this.xrCheckBox1.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 0F);
            this.xrCheckBox1.Name = "xrCheckBox1";
            this.xrCheckBox1.SizeF = new System.Drawing.SizeF(63.5F, 31.75F);
            this.xrCheckBox1.StylePriority.UseBorderDashStyle = false;
            this.xrCheckBox1.StylePriority.UseBorders = false;
            // 
            // BSerahTerimaCSSD
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.PageHeader,
            this.PageFooter,
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.total});
            this.Margins = new System.Drawing.Printing.Margins(63, 63, 64, 64);
            this.ShowPrintMarginsWarning = false;
            this.ShowPrintStatusDialog = false;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.CalculatedField total;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell15;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell14;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell13;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell16;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell8;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell18;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell19;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell20;
        private DevExpress.XtraReports.UI.XRCheckBox xrCheckBox1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
    }
}
