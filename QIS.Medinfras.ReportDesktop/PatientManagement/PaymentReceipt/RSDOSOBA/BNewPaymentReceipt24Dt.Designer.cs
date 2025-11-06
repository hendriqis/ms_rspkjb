namespace QIS.Medinfras.ReportDesktop
{
    partial class BNewPaymentReceipt24Dt
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
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.lblCheck = new DevExpress.XtraReports.UI.XRLabel();
            this.tableDetail = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.GroupFooter2 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.GroupFooter3 = new DevExpress.XtraReports.UI.GroupFooterBand();
            ((System.ComponentModel.ISupportInitialize)(this.tableDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblCheck,
            this.tableDetail});
            this.Detail.HeightF = 14.58333F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("GroupingPrintOrder", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ChargesParamedicName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblCheck
            // 
            this.lblCheck.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ChargesParamedicName")});
            this.lblCheck.LocationFloat = new DevExpress.Utils.PointFloat(341.0001F, 0F);
            this.lblCheck.Name = "lblCheck";
            this.lblCheck.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
            this.lblCheck.SizeF = new System.Drawing.SizeF(9.72226F, 14.58333F);
            this.lblCheck.Text = "lblCheck";
            this.lblCheck.Visible = false;
            // 
            // tableDetail
            // 
            this.tableDetail.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.tableDetail.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.tableDetail.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Italic);
            this.tableDetail.LocationFloat = new DevExpress.Utils.PointFloat(26.04169F, 0F);
            this.tableDetail.Name = "tableDetail";
            this.tableDetail.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.tableDetail.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.tableDetail.SizeF = new System.Drawing.SizeF(314.9583F, 14.58333F);
            this.tableDetail.StylePriority.UseBorderDashStyle = false;
            this.tableDetail.StylePriority.UseBorders = false;
            this.tableDetail.StylePriority.UseFont = false;
            this.tableDetail.StylePriority.UsePadding = false;
            this.tableDetail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.tableDetail_BeforePrint);
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell6});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ChargesParamedicName")});
            this.xrTableCell6.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Italic);
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.Text = "xrTableCell6";
            this.xrTableCell6.Weight = 1.702045231530098D;
            // 
            // xrTable3
            // 
            this.xrTable3.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTable3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3,
            this.xrTableRow7,
            this.xrTableRow5,
            this.xrTableRow4,
            this.xrTableRow6});
            this.xrTable3.SizeF = new System.Drawing.SizeF(341.0001F, 72.91665F);
            this.xrTable3.StylePriority.UseBorderDashStyle = false;
            this.xrTable3.StylePriority.UseBorders = false;
            this.xrTable3.StylePriority.UseFont = false;
            this.xrTable3.StylePriority.UsePadding = false;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell9});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTableCell7.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseBorders = false;
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.Text = "Total Transaksi";
            this.xrTableCell7.Weight = 1.1212242367208463D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GroupingAmount")});
            this.xrTableCell9.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0:n2}";
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell9.Summary = xrSummary1;
            this.xrTableCell9.Text = "xrTableCell9";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell9.Weight = 0.58082099480925165D;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell15,
            this.xrTableCell16});
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.Text = "Diskon";
            this.xrTableCell15.Weight = 1.1212242367208463D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GroupingDiscount")});
            this.xrTableCell16.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StylePriority.UseFont = false;
            this.xrTableCell16.StylePriority.UseTextAlignment = false;
            xrSummary2.FormatString = "{0:n2}";
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell16.Summary = xrSummary2;
            this.xrTableCell16.Text = "xrTableCell16";
            this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell16.Weight = 0.58082099480925165D;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell11,
            this.xrTableCell12});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseFont = false;
            this.xrTableCell11.Text = "Dijamin";
            this.xrTableCell11.Weight = 1.1212242367208463D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PayerAmount", "{0:n2}")});
            this.xrTableCell12.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "xrTableCell12";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell12.Weight = 0.58082099480925165D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.xrTableCell10});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.Text = "Lain-lain";
            this.xrTableCell2.Weight = 1.1212242367208463D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RoundingAmount")});
            this.xrTableCell10.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "xrTableCell10";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell10.Weight = 0.58082099480925165D;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell13,
            this.xrTableCell14});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell13.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseBorders = false;
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.Text = "TOTAL AKHIR";
            this.xrTableCell13.Weight = 1.1212242367208463D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ReceiptAmount", "{0:n2}")});
            this.xrTableCell14.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StylePriority.UseBorders = false;
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            xrSummary3.FormatString = "{0:n2}";
            this.xrTableCell14.Summary = xrSummary3;
            this.xrTableCell14.Text = "xrTableCell14";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell14.Weight = 0.58082099480925165D;
            // 
            // xrTable4
            // 
            this.xrTable4.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTable4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow8});
            this.xrTable4.SizeF = new System.Drawing.SizeF(341.0001F, 14.58333F);
            this.xrTable4.StylePriority.UseBorderDashStyle = false;
            this.xrTable4.StylePriority.UseBorders = false;
            this.xrTable4.StylePriority.UseFont = false;
            this.xrTable4.StylePriority.UsePadding = false;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell18,
            this.xrTableCell19});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GroupingName")});
            this.xrTableCell18.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseFont = false;
            this.xrTableCell18.Text = "xrTableCell6";
            this.xrTableCell18.Weight = 1.1212240082357039D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GroupingAmount")});
            this.xrTableCell19.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseFont = false;
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            xrSummary4.FormatString = "{0:n2}";
            xrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell19.Summary = xrSummary4;
            this.xrTableCell19.Text = "xrTableCell19";
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell19.Weight = 0.580821223294394D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 10F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 14F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.BackColor = System.Drawing.Color.White;
            this.xrTable1.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.ForeColor = System.Drawing.Color.Black;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(341F, 13.54167F);
            this.xrTable1.StylePriority.UseBackColor = false;
            this.xrTable1.StylePriority.UseBorderDashStyle = false;
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseForeColor = false;
            this.xrTable1.StylePriority.UsePadding = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell4});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Text = "Keterangan";
            this.xrTableCell3.Weight = 1.5686728230653211D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "Nilai";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell4.Weight = 0.8126094904473824D;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.PageHeader.HeightF = 13.54167F;
            this.PageHeader.Name = "PageHeader";
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetPaymentReceiptCustomDetailTransactionDiscount);
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("VisitServiceUnitCode", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 14.66667F;
            this.GroupHeader1.Level = 1;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "VisitServiceUnitName")});
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(341F, 14.66667F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "xrLabel1";
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4});
            this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("GroupingName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader2.HeightF = 14.58333F;
            this.GroupHeader2.Name = "GroupHeader2";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.HeightF = 0F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // GroupFooter2
            // 
            this.GroupFooter2.HeightF = 0F;
            this.GroupFooter2.Level = 1;
            this.GroupFooter2.Name = "GroupFooter2";
            // 
            // GroupFooter3
            // 
            this.GroupFooter3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
            this.GroupFooter3.HeightF = 72.91665F;
            this.GroupFooter3.Level = 2;
            this.GroupFooter3.Name = "GroupFooter3";
            // 
            // BNewPaymentReceipt24Dt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader,
            this.GroupHeader1,
            this.GroupHeader2,
            this.GroupFooter1,
            this.GroupFooter2,
            this.GroupFooter3});
            this.DataSource = this.bs;
            this.Margins = new System.Drawing.Printing.Margins(27, 35, 10, 14);
            this.PageHeight = 583;
            this.PageWidth = 413;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.tableDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTable tableDetail;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private System.Windows.Forms.BindingSource bs;
        private DevExpress.XtraReports.UI.XRTable xrTable3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell13;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell14;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell15;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell16;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell11;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRTable xrTable4;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow8;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell18;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell19;
        private DevExpress.XtraReports.UI.XRLabel lblCheck;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter2;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter3;
    }
}
