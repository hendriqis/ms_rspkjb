namespace QIS.Medinfras.ReportDesktop
{
    partial class LPiutangJatuhTempo
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
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.totalPrice = new DevExpress.XtraReports.UI.CalculatedField();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblPeriod = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblPeriod,
            this.xrTable1});
            this.PageHeader.HeightF = 444.5F;
            this.PageHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblPhoneFaxNo, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblReportSubtitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrTable1, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblPeriod, 0);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vARInvoicePatientPaymentHd);
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
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            // 
            // lblReportSubtitle
            // 
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
            this.xrTable2});
            this.Detail.HeightF = 31.75F;
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Noreg", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Dpi = 254F;
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 381F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(2825.75F, 63.5F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell7,
            this.xrTableCell1,
            this.xrTableCell6,
            this.xrTableCell8,
            this.xrTableCell5,
            this.xrTableCell2,
            this.xrTableCell27,
            this.xrTableCell23,
            this.xrTableCell25});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Text = "No Piutang";
            this.xrTableCell4.Weight = 0.43548386973719444D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "Tgl Piutang";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell7.Weight = 0.38709677542409582D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "No Invoice";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell1.Weight = 0.48387098743069545D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "Tgl Invoice";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell6.Weight = 0.38709676434916857D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = "Tgl Jatuh Tempo";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell8.Weight = 0.43548387588993215D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "Total Piutang";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell5.Weight = 0.435483866045552D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Total Diskon";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell2.Weight = 0.43548386604555167D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.Dpi = 254F;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 10, 254F);
            this.xrTableCell27.StylePriority.UsePadding = false;
            this.xrTableCell27.StylePriority.UseTextAlignment = false;
            this.xrTableCell27.Text = "Total Pemutihan";
            this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell27.Weight = 0.38709677296300071D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.Dpi = 254F;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 254F);
            this.xrTableCell23.StylePriority.UsePadding = false;
            this.xrTableCell23.StylePriority.UseTextAlignment = false;
            this.xrTableCell23.Text = "Total Sudah Terima";
            this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell23.Weight = 0.48387096651138811D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.Dpi = 254F;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.StylePriority.UseTextAlignment = false;
            this.xrTableCell25.Text = "Sisa Piutang";
            this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell25.Weight = 0.43548386850664678D;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel10,
            this.xrLabel8,
            this.xrLabel9,
            this.xrLine2});
            this.GroupFooter1.Dpi = 254F;
            this.GroupFooter1.HeightF = 64F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrLabel10
            // 
            this.xrLabel10.Dpi = 254F;
            this.xrLabel10.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(2540F, 31.75F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(285.75F, 31.75F);
            this.xrLabel10.StylePriority.UseFont = false;
            xrSummary1.FormatString = "{0:n2}";
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel10.Summary = xrSummary1;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Dpi = 254F;
            this.xrLabel8.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(1746.25F, 31.75F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(762F, 31.75F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Subtotal([BusinessPartnerName])";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Dpi = 254F;
            this.xrLabel9.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(2508.25F, 31.75F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = ":";
            // 
            // xrLine2
            // 
            this.xrLine2.Dpi = 254F;
            this.xrLine2.LineStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.xrLine2.LineWidth = 3;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(2825.75F, 31.75F);
            // 
            // totalPrice
            // 
            this.totalPrice.Expression = "[TotalAR] - [TotalPembayaran]";
            this.totalPrice.Name = "totalPrice";
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel1});
            this.GroupHeader2.Dpi = 254F;
            this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("BusinessPartnerCode", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader2.HeightF = 32F;
            this.GroupHeader2.Name = "GroupHeader2";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(254F, 0F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(1778F, 31.75F);
            this.xrLabel5.StylePriority.UseFont = false;
            xrSummary2.FormatString = "{0:dd-MMM-yy}";
            this.xrLabel5.Summary = xrSummary2;
            this.xrLabel5.Text = "[BusinessPartnerCode] - [BusinessPartnerName]";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(222.25F, 0F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = ":";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(222.25F, 31.75F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Supplier";
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Dpi = 254F;
            this.xrTable2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(2825.75F, 31.75F);
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell10,
            this.xrTableCell11,
            this.xrTableCell12,
            this.xrTableCell13,
            this.xrTableCell14,
            this.xrTableCell15,
            this.xrTableCell17,
            this.xrTableCell18,
            this.xrTableCell28});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PaymentNo")});
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseBorders = false;
            this.xrTableCell3.Text = "xrTableCell3";
            this.xrTableCell3.Weight = 0.4354838697371945D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PaymentDate", "{0:dd-MMM-yyyy}")});
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseBorders = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "xrTableCell10";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell10.Weight = 0.38709677542409576D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ARInvoiceNo")});
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseBorders = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "xrTableCell11";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell11.Weight = 0.48387095789755535D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ARInvoiceDate", "{0:dd-MMM-yyyy}")});
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseBorders = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "xrTableCell12";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell12.Weight = 0.38709676434916862D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DueDate", "{0:dd-MMM-yyyy}")});
            this.xrTableCell13.Dpi = 254F;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseBorders = false;
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.Text = "xrTableCell13";
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell13.Weight = 0.4354838758899322D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TransactionAmount", "{0:n2}")});
            this.xrTableCell14.Dpi = 254F;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StylePriority.UseBorders = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.Text = "xrTableCell14";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell14.Weight = 0.43548389557869227D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DiscountAmount", "{0:n2}")});
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseBorders = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.Text = "xrTableCell15";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell15.Weight = 0.43548386604555167D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell17.Dpi = 254F;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 10, 254F);
            this.xrTableCell17.StylePriority.UseBorders = false;
            this.xrTableCell17.StylePriority.UsePadding = false;
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell17.Weight = 0.38709677296300071D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell18.Dpi = 254F;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 0, 0, 0, 254F);
            this.xrTableCell18.StylePriority.UseBorders = false;
            this.xrTableCell18.StylePriority.UsePadding = false;
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell18.Weight = 0.48387096651138811D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTableCell28.Dpi = 254F;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseBorders = false;
            this.xrTableCell28.StylePriority.UseTextAlignment = false;
            this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell28.Weight = 0.43548386850664678D;
            // 
            // lblPeriod
            // 
            this.lblPeriod.Dpi = 254F;
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblPeriod.LocationFloat = new DevExpress.Utils.PointFloat(0F, 285.75F);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPeriod.SizeF = new System.Drawing.SizeF(2825.75F, 31.75F);
            this.lblPeriod.StylePriority.UseFont = false;
            this.lblPeriod.StylePriority.UseTextAlignment = false;
            this.lblPeriod.Text = "Periode : [StartDate!dd-MMM-yyyy] s/d  [EndDate!dd-MMM-yyyy] ";
            this.lblPeriod.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // LPiutangJatuhTempo
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.PageHeader,
            this.GroupFooter1,
            this.GroupHeader2,
            this.PageFooter,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.totalPrice});
            this.Margins = new System.Drawing.Printing.Margins(25, 24, 64, 64);
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.GroupHeader2, 0);
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
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell8;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.CalculatedField totalPrice;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell27;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell23;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell25;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell11;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell13;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell14;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell15;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell17;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell18;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell28;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel lblPeriod;
    }
}
