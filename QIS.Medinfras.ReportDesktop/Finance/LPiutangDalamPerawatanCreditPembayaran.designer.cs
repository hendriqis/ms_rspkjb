namespace QIS.Medinfras.ReportDesktop
{
    partial class LPiutangDalamPerawatanCreditPembayaran
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
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary5 = new DevExpress.XtraReports.UI.XRSummary();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.totalPrice = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.lblPeriod = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.BackColor = System.Drawing.Color.Transparent;
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblPeriod,
            this.xrTable1});
            this.PageHeader.Font = new System.Drawing.Font("Tahoma", 8F);
            this.PageHeader.ForeColor = System.Drawing.Color.Black;
            this.PageHeader.HeightF = 412.75F;
            this.PageHeader.StylePriority.UseBackColor = false;
            this.PageHeader.StylePriority.UseFont = false;
            this.PageHeader.StylePriority.UseForeColor = false;
            this.PageHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblPhoneFaxNo, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblReportSubtitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrTable1, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblPeriod, 0);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetARinCareCreditPembayaranPasien);
            // 
            // lblReportParameter
            // 
            this.lblReportParameter.StylePriority.UseFont = false;
            // 
            // lblPhoneFaxNo
            // 
            this.lblPhoneFaxNo.LocationFloat = new DevExpress.Utils.PointFloat(1682.75F, 0F);
            this.lblPhoneFaxNo.StylePriority.UseFont = false;
            this.lblPhoneFaxNo.StylePriority.UsePadding = false;
            this.lblPhoneFaxNo.StylePriority.UseTextAlignment = false;
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.SizeF = new System.Drawing.SizeF(2889.25F, 63.5F);
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            // 
            // lblReportSubtitle
            // 
            this.lblReportSubtitle.SizeF = new System.Drawing.SizeF(2889.25F, 31.75F);
            this.lblReportSubtitle.StylePriority.UseFont = false;
            this.lblReportSubtitle.StylePriority.UseTextAlignment = false;
            // 
            // xrPageInfo3
            // 
            this.xrPageInfo3.LocationFloat = new DevExpress.Utils.PointFloat(2508.25F, 31.75F);
            this.xrPageInfo3.StylePriority.UseFont = false;
            this.xrPageInfo3.StylePriority.UseTextAlignment = false;
            // 
            // lineFooter
            // 
            this.lineFooter.SizeF = new System.Drawing.SizeF(2889.25F, 5F);
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 31.75F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 63.5F;
            // 
            // xrTable1
            // 
            this.xrTable1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Dpi = 254F;
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.ForeColor = System.Drawing.Color.Black;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 349.25F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(2889.25F, 63.5F);
            this.xrTable1.StylePriority.UseBackColor = false;
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
            this.xrTableCell4,
            this.xrTableCell12,
            this.xrTableCell7,
            this.xrTableCell1,
            this.xrTableCell27,
            this.xrTableCell8,
            this.xrTableCell16,
            this.xrTableCell23,
            this.xrTableCell5,
            this.xrTableCell2});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "No";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.096774192317839569D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Text = "Registration No";
            this.xrTableCell12.Weight = 0.38709677511645918D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "Payment No";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell7.Weight = 0.38709677450118535D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Created Date";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.29032259048954129D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.Dpi = 254F;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.Text = "GLAccountName";
            this.xrTableCell27.Weight = 0.43548388081212169D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Text = "Sub Ledger";
            this.xrTableCell8.Weight = 0.6774193536081623D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Dpi = 254F;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Text = "Business Partner";
            this.xrTableCell16.Weight = 0.87096774070493632D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Dpi = 254F;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.StylePriority.UseTextAlignment = false;
            this.xrTableCell23.Text = "Total Amount";
            this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell23.Weight = 0.43548386850664711D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Multiline = true;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "Debit Amount";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell5.Weight = 0.43548386358445651D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Credit Amount";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell2.Weight = 0.38709676681026339D;
            // 
            // totalPrice
            // 
            this.totalPrice.Expression = "([ReturnQty] * [Price]) - [Discount]";
            this.totalPrice.Name = "totalPrice";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(87.5F, 12.5F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Dokter";
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable2.Dpi = 254F;
            this.xrTable2.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(2889.25F, 31.75F);
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UsePadding = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell9,
            this.xrTableCell17,
            this.xrTableCell10,
            this.xrTableCell11,
            this.xrTableCell28,
            this.xrTableCell13,
            this.xrTableCell19,
            this.xrTableCell24,
            this.xrTableCell14,
            this.xrTableCell3});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell9.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell9.Summary = xrSummary1;
            this.xrTableCell9.Text = "No";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell9.Weight = 0.096774192317839569D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell17.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RegistrationNo")});
            this.xrTableCell17.Dpi = 254F;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell17.StylePriority.UseBorders = false;
            this.xrTableCell17.Text = "xrTableCell17";
            this.xrTableCell17.Weight = 0.3870967748088221D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell10.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PaymentNo")});
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell10.StylePriority.UseBorders = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "xrTableCell10";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell10.Weight = 0.38709677480882215D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell11.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfCreatedDateInString")});
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell11.StylePriority.UseBorders = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell11.Weight = 0.29032259048954145D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell28.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell28.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GLAccountName")});
            this.xrTableCell28.Dpi = 254F;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell28.StylePriority.UseBorders = false;
            this.xrTableCell28.Text = "xrTableCell28";
            this.xrTableCell28.Weight = 0.43548388081212208D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell13.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SubLedgerName")});
            this.xrTableCell13.Dpi = 254F;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell13.StylePriority.UseBorders = false;
            this.xrTableCell13.Text = "xrTableCell13";
            this.xrTableCell13.Weight = 0.67741935360816241D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell19.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell19.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "BusinessPartnerName")});
            this.xrTableCell19.Dpi = 254F;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell19.StylePriority.UseBorders = false;
            this.xrTableCell19.Weight = 0.87096774070493654D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell24.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell24.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TotalAmount", "{0:n}")});
            this.xrTableCell24.Dpi = 254F;
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell24.StylePriority.UseBorders = false;
            this.xrTableCell24.StylePriority.UseTextAlignment = false;
            this.xrTableCell24.Text = "xrTableCell24";
            this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell24.Weight = 0.43548386850664733D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell14.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DebitAmount", "{0:n2}")});
            this.xrTableCell14.Dpi = 254F;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell14.StylePriority.UseBorders = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.Text = "xrTableCell14";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell14.Weight = 0.43548386358445645D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTableCell3.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CreditAmount", "{0:n}")});
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell3.StylePriority.UseBorders = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "xrTableCell3";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell3.Weight = 0.38709676681026323D;
            // 
            // xrTable3
            // 
            this.xrTable3.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable3.Dpi = 254F;
            this.xrTable3.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 31.75F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3});
            this.xrTable3.SizeF = new System.Drawing.SizeF(2889.25F, 31.75F);
            this.xrTable3.StylePriority.UseBorders = false;
            this.xrTable3.StylePriority.UseFont = false;
            this.xrTable3.StylePriority.UsePadding = false;
            this.xrTable3.StylePriority.UseTextAlignment = false;
            this.xrTable3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell15,
            this.xrTableCell26,
            this.xrTableCell20,
            this.xrTableCell21});
            this.xrTableRow3.Dpi = 254F;
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseBorders = false;
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            xrSummary2.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            this.xrTableCell15.Summary = xrSummary2;
            this.xrTableCell15.Text = "GRAND TOTAL";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell15.Weight = 3.1451612890920333D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TotalAmount")});
            this.xrTableCell26.Dpi = 254F;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.StylePriority.UseBorders = false;
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            xrSummary3.FormatString = "{0:n}";
            xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell26.Summary = xrSummary3;
            this.xrTableCell26.Text = "xrTableCell26";
            this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell26.Weight = 0.43548388696486007D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell20.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DebitAmount")});
            this.xrTableCell20.Dpi = 254F;
            this.xrTableCell20.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.StylePriority.UseBorders = false;
            this.xrTableCell20.StylePriority.UseFont = false;
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            xrSummary4.FormatString = "{0:#,##0.00}";
            xrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell20.Summary = xrSummary4;
            this.xrTableCell20.Text = "xrTableCell20";
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell20.Weight = 0.43548386358445645D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CreditAmount")});
            this.xrTableCell21.Dpi = 254F;
            this.xrTableCell21.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.StylePriority.UseBorders = false;
            this.xrTableCell21.StylePriority.UseFont = false;
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            xrSummary5.FormatString = "{0:#,##0.00}";
            xrSummary5.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell21.Summary = xrSummary5;
            this.xrTableCell21.Text = "xrTableCell21";
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell21.Weight = 0.38709676681026323D;
            // 
            // xrLine3
            // 
            this.xrLine3.Dpi = 254F;
            this.xrLine3.LineWidth = 3;
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(2889.25F, 31.75F);
            // 
            // lblPeriod
            // 
            this.lblPeriod.Dpi = 254F;
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblPeriod.LocationFloat = new DevExpress.Utils.PointFloat(0F, 285.75F);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPeriod.SizeF = new System.Drawing.SizeF(2889.25F, 31.75F);
            this.lblPeriod.StylePriority.UseFont = false;
            this.lblPeriod.StylePriority.UseTextAlignment = false;
            this.lblPeriod.Text = "Periode : [StartDate!dd-MMM-yyyy] s/d  [EndDate!dd-MMM-yyyy] ";
            this.lblPeriod.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3,
            this.xrLine3});
            this.GroupFooter1.Dpi = 254F;
            this.GroupFooter1.HeightF = 63.5F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // LPiutangDalamPerawatanCreditPembayaran
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.PageHeader,
            this.PageFooter,
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.GroupFooter1});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.totalPrice});
            this.Margins = new System.Drawing.Printing.Margins(25, 24, 64, 64);
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.GroupFooter1, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.PageHeader, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.CalculatedField totalPrice;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell11;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell14;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRLine xrLine3;
        private DevExpress.XtraReports.UI.XRLabel lblPeriod;
        private DevExpress.XtraReports.UI.XRTable xrTable3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell15;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell20;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell21;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell17;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell23;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell24;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell26;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell27;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell28;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell8;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell13;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell16;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell19;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
    }
}
