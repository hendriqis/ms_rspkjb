namespace QIS.Medinfras.ReportDesktop
{
    partial class LDaftarPemeriksaanPasienLaboratorium
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
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell29 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell30 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell31 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell32 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblFromItem = new DevExpress.XtraReports.UI.XRLabel();
            this.lblToItem = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.StylePriority.UseFont = false;
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetDataPemeriksaanPasienPenunjang);
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblToItem,
            this.lblFromItem,
            this.xrLabel1});
            this.ReportHeader.HeightF = 444.9234F;
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel1, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblFromItem, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblToItem, 0);
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 37.04167F;
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("TransactionID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ItemPemeriksaan", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 309.3509F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(4016.25F, 39.89917F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Bulan : [cfMonthText] | Tahun : [Tahun]";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 37.04167F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Dpi = 254F;
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6});
            this.xrTable1.SizeF = new System.Drawing.SizeF(4016.25F, 37.04167F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UsePadding = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell4,
            this.xrTableCell5,
            this.xrTableCell6,
            this.xrTableCell9,
            this.xrTableCell10,
            this.xrTableCell11,
            this.xrTableCell12,
            this.xrTableCell14,
            this.xrTableCell13,
            this.xrTableCell15,
            this.xrTableCell18,
            this.xrTableCell16,
            this.xrTableCell17});
            this.xrTableRow6.Dpi = 254F;
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "No.";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell3.Weight = 0.075070028011204437D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Text = "Transaction No";
            this.xrTableCell4.Weight = 0.238770215117297D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Text = "Registration No";
            this.xrTableCell5.Weight = 0.23451792826505591D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Text = "Patient";
            this.xrTableCell6.Weight = 0.37796816643324016D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "Gender";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell9.Weight = 0.11684784590919706D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Age";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell10.Weight = 0.11969054548902894D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Text = "Address";
            this.xrTableCell11.Weight = 0.28613692710156974D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Text = "Department";
            this.xrTableCell12.Weight = 0.18369195049439044D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Dpi = 254F;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Text = "Service Unit";
            this.xrTableCell14.Weight = 0.24100577446147722D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Dpi = 254F;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.Text = "Room";
            this.xrTableCell13.Weight = 0.096732570388181283D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Text = "Paramedic Name";
            this.xrTableCell15.Weight = 0.28355162234978631D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Dpi = 254F;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.Text = "Business Partner";
            this.xrTableCell18.Weight = 0.23838337382682565D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Dpi = 254F;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Text = "Item";
            this.xrTableCell16.Weight = 0.28976788596501429D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Dpi = 254F;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Text = "Diagnose";
            this.xrTableCell17.Weight = 0.21786516618773164D;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2});
            this.GroupHeader1.Dpi = 254F;
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("TransactionDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 34.6075F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.ForeColor = System.Drawing.Color.Blue;
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(4016.25F, 34.6075F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseForeColor = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Tanggal Transaksi : [cfTransactionDate]";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTable2
            // 
            this.xrTable2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Dpi = 254F;
            this.xrTable2.Font = new System.Drawing.Font("Tahoma", 6.75F);
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow7});
            this.xrTable2.SizeF = new System.Drawing.SizeF(4016.25F, 37.04167F);
            this.xrTable2.StylePriority.UseBorderDashStyle = false;
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UsePadding = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19,
            this.xrTableCell20,
            this.xrTableCell21,
            this.xrTableCell22,
            this.xrTableCell23,
            this.xrTableCell24,
            this.xrTableCell25,
            this.xrTableCell26,
            this.xrTableCell27,
            this.xrTableCell28,
            this.xrTableCell29,
            this.xrTableCell30,
            this.xrTableCell31,
            this.xrTableCell32});
            this.xrTableRow7.Dpi = 254F;
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.Dpi = 254F;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell19.Summary = xrSummary1;
            this.xrTableCell19.Text = "No.";
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell19.Weight = 0.075070028011204437D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TransactionNo")});
            this.xrTableCell20.Dpi = 254F;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.Text = "Transaction No";
            this.xrTableCell20.Weight = 0.238770215117297D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RegistrationNo")});
            this.xrTableCell21.Dpi = 254F;
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.Text = "Registration No";
            this.xrTableCell21.Weight = 0.23451792826505591D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfPatientInfo")});
            this.xrTableCell22.Dpi = 254F;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.Text = "Patient";
            this.xrTableCell22.Weight = 0.37796816643324016D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Gender")});
            this.xrTableCell23.Dpi = 254F;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.StylePriority.UseTextAlignment = false;
            this.xrTableCell23.Text = "Gender";
            this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell23.Weight = 0.11684784590919706D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfPatientAge")});
            this.xrTableCell24.Dpi = 254F;
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UseTextAlignment = false;
            this.xrTableCell24.Text = "Age";
            this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell24.Weight = 0.11969054548902894D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "AddressName")});
            this.xrTableCell25.Dpi = 254F;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.Text = "Address";
            this.xrTableCell25.Weight = 0.28613692710156974D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DepartmentID")});
            this.xrTableCell26.Dpi = 254F;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.Text = "Department";
            this.xrTableCell26.Weight = 0.18369195049439044D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ServiceUnitName")});
            this.xrTableCell27.Dpi = 254F;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.Text = "Service Unit";
            this.xrTableCell27.Weight = 0.24100577446147722D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RoomName")});
            this.xrTableCell28.Dpi = 254F;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.Text = "Room";
            this.xrTableCell28.Weight = 0.096732570388181283D;
            // 
            // xrTableCell29
            // 
            this.xrTableCell29.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ParamedicName")});
            this.xrTableCell29.Dpi = 254F;
            this.xrTableCell29.Name = "xrTableCell29";
            this.xrTableCell29.Text = "Paramedic Name";
            this.xrTableCell29.Weight = 0.28355162234978631D;
            // 
            // xrTableCell30
            // 
            this.xrTableCell30.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "BusinessPartnerName")});
            this.xrTableCell30.Dpi = 254F;
            this.xrTableCell30.Name = "xrTableCell30";
            this.xrTableCell30.Text = "Business Partner";
            this.xrTableCell30.Weight = 0.23838337382682565D;
            // 
            // xrTableCell31
            // 
            this.xrTableCell31.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ItemPemeriksaan")});
            this.xrTableCell31.Dpi = 254F;
            this.xrTableCell31.Name = "xrTableCell31";
            this.xrTableCell31.Text = "Item";
            this.xrTableCell31.Weight = 0.28976788596501429D;
            // 
            // xrTableCell32
            // 
            this.xrTableCell32.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Diagnosa")});
            this.xrTableCell32.Dpi = 254F;
            this.xrTableCell32.Name = "xrTableCell32";
            this.xrTableCell32.Text = "Diagnose";
            this.xrTableCell32.Weight = 0.21786516618773164D;
            // 
            // lblFromItem
            // 
            this.lblFromItem.Dpi = 254F;
            this.lblFromItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromItem.ForeColor = System.Drawing.Color.Black;
            this.lblFromItem.LocationFloat = new DevExpress.Utils.PointFloat(31.74895F, 375.7083F);
            this.lblFromItem.Name = "lblFromItem";
            this.lblFromItem.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblFromItem.SizeF = new System.Drawing.SizeF(4016.25F, 34.6075F);
            this.lblFromItem.StylePriority.UseFont = false;
            this.lblFromItem.StylePriority.UseForeColor = false;
            this.lblFromItem.StylePriority.UseTextAlignment = false;
            this.lblFromItem.Text = "lblFromItem";
            this.lblFromItem.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblToItem
            // 
            this.lblToItem.Dpi = 254F;
            this.lblToItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToItem.ForeColor = System.Drawing.Color.Black;
            this.lblToItem.LocationFloat = new DevExpress.Utils.PointFloat(31.75008F, 410.3159F);
            this.lblToItem.Name = "lblToItem";
            this.lblToItem.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblToItem.SizeF = new System.Drawing.SizeF(4016.25F, 34.6075F);
            this.lblToItem.StylePriority.UseFont = false;
            this.lblToItem.StylePriority.UseForeColor = false;
            this.lblToItem.StylePriority.UseTextAlignment = false;
            this.lblToItem.Text = "lblToItem";
            this.lblToItem.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // LDaftarPemeriksaanPasienLaboratorium
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.PageHeader,
            this.GroupHeader1});
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.GroupHeader1, 0);
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell19;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell20;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell21;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell22;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell23;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell24;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell25;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell26;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell27;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell28;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell29;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell30;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell31;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell32;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell11;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell14;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell13;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell15;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell18;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell16;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell17;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel lblFromItem;
        private DevExpress.XtraReports.UI.XRLabel lblToItem;
    }
}
