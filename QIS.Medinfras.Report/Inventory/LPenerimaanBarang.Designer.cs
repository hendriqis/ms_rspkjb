namespace QIS.Medinfras.Report
{
    partial class LPenerimaanBarang
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
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblSubTotal = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblHeaderPPN = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblHeaderDisc = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblHeaderMaterai = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblHeaderOngkos = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblTotal = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeader2 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TotalPrice = new DevExpress.XtraReports.UI.CalculatedField();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.GroupFooter2 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalTgl = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter3 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblGrandTotal = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.PageHeader.HeightF = 125F;
            this.PageHeader.StylePriority.UseTextAlignment = false;
            this.PageHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.PageHeader.Controls.SetChildIndex(this.xrTable1, 0);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vPurchaseReceiveHd);
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("PurchaseReceiveNo", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 12.5F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupHeader1_BeforePrint);
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable2.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(1112.5F, 12.5F);
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell11,
            this.xrTableCell12,
            this.xrTableCell13,
            this.xrTableCell14,
            this.lblSubTotal,
            this.lblHeaderPPN,
            this.lblHeaderDisc,
            this.lblHeaderMaterai,
            this.lblHeaderOngkos,
            this.lblTotal});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseFont = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.Avg;
            this.xrTableCell11.Summary = xrSummary1;
            this.xrTableCell11.Text = "lblNo";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell11.Weight = 0.067415716621313182D;
            this.xrTableCell11.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell11_BeforePrint);
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PurchaseReceiveNo")});
            this.xrTableCell12.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.Text = "xrTableCell12";
            this.xrTableCell12.Weight = 0.43820227462254213D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SupplierName")});
            this.xrTableCell13.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.Text = "xrTableCell13";
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell13.Weight = 0.640449321618241D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ReferenceNo")});
            this.xrTableCell14.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.Text = "xrTableCell14";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell14.Weight = 0.30337077794450057D;
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TransactionAmount")});
            this.lblSubTotal.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.StylePriority.UseFont = false;
            this.lblSubTotal.StylePriority.UseTextAlignment = false;
            xrSummary2.FormatString = "{0:#,##0.00}";
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.lblSubTotal.Summary = xrSummary2;
            this.lblSubTotal.Text = "lblSubTotal";
            this.lblSubTotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.lblSubTotal.Weight = 0.30337077794450051D;
            this.lblSubTotal.SummaryCalculated += new DevExpress.XtraReports.UI.TextFormatEventHandler(this.lblSubTotal_SummaryCalculated);
            // 
            // lblHeaderPPN
            // 
            this.lblHeaderPPN.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "VATPercentage", "{0:#,##0.00}")});
            this.lblHeaderPPN.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblHeaderPPN.Name = "lblHeaderPPN";
            this.lblHeaderPPN.StylePriority.UseFont = false;
            this.lblHeaderPPN.StylePriority.UseTextAlignment = false;
            this.lblHeaderPPN.Text = "lblHeaderPPN";
            this.lblHeaderPPN.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.lblHeaderPPN.Weight = 0.23595505189359861D;
            // 
            // lblHeaderDisc
            // 
            this.lblHeaderDisc.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FinalDiscount", "{0:#,##0.00}")});
            this.lblHeaderDisc.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblHeaderDisc.Name = "lblHeaderDisc";
            this.lblHeaderDisc.StylePriority.UseFont = false;
            this.lblHeaderDisc.StylePriority.UseTextAlignment = false;
            this.lblHeaderDisc.Text = "[FinalDiscount]";
            this.lblHeaderDisc.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.lblHeaderDisc.Weight = 0.23595505189359861D;
            // 
            // lblHeaderMaterai
            // 
            this.lblHeaderMaterai.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "StampAmount")});
            this.lblHeaderMaterai.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblHeaderMaterai.Name = "lblHeaderMaterai";
            this.lblHeaderMaterai.StylePriority.UseFont = false;
            this.lblHeaderMaterai.StylePriority.UseTextAlignment = false;
            this.lblHeaderMaterai.Text = "lblHeaderMaterai";
            this.lblHeaderMaterai.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.lblHeaderMaterai.Weight = 0.23595504760742186D;
            // 
            // lblHeaderOngkos
            // 
            this.lblHeaderOngkos.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ChargesAmount", "{0:#,##0.00}")});
            this.lblHeaderOngkos.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblHeaderOngkos.Name = "lblHeaderOngkos";
            this.lblHeaderOngkos.StylePriority.UseFont = false;
            this.lblHeaderOngkos.StylePriority.UseTextAlignment = false;
            this.lblHeaderOngkos.Text = "lblHeaderOngkos";
            this.lblHeaderOngkos.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.lblHeaderOngkos.Weight = 0.23595506303765798D;
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.StylePriority.UseFont = false;
            this.lblTotal.StylePriority.UseTextAlignment = false;
            xrSummary3.FormatString = "{0:#,##0.00}";
            xrSummary3.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.lblTotal.Summary = xrSummary3;
            this.lblTotal.Text = "Total";
            this.lblTotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.lblTotal.Weight = 0.30337091681662565D;
            this.lblTotal.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.lblTotal_SummaryGetResult);
            // 
            // GroupHeader2
            // 
            this.GroupHeader2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6,
            this.xrLabel2,
            this.xrLabel5});
            this.GroupHeader2.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ReceivedDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader2.HeightF = 12.5F;
            this.GroupHeader2.Level = 1;
            this.GroupHeader2.Name = "GroupHeader2";
            this.GroupHeader2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.GroupHeader2_BeforePrint);
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(550F, 12.5F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Tanggal Terima : [PurchaseReceiveDate!dd-MMM-yyyy]";
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 112.5F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(1112.5F, 12.5F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell4,
            this.xrTableCell2,
            this.xrTableCell5,
            this.xrTableCell10,
            this.xrTableCell6,
            this.xrTableCell9,
            this.xrTableCell8,
            this.xrTableCell3,
            this.xrTableCell7});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.Text = "No";
            this.xrTableCell1.Weight = 0.067415716621313182D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.Text = "No Terima";
            this.xrTableCell4.Weight = 0.43820227462254213D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.Text = "Nama Supplier";
            this.xrTableCell2.Weight = 0.640449321618241D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.Text = "No Faktur";
            this.xrTableCell5.Weight = 0.30337077794450057D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseFont = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Sub Total";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell10.Weight = 0.30337077794450051D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "PPN";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell6.Weight = 0.23595507246724679D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseFont = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "Diskon";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell9.Weight = 0.23595505189359861D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseFont = false;
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = "Meterai";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell8.Weight = 0.23595504760742192D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Ongkos";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell3.Weight = 0.23595489844847259D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "Total";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell7.Weight = 0.30337106083216292D;
            // 
            // TotalPrice
            // 
            this.TotalPrice.Name = "TotalPrice";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.HeightF = 0F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrLine1
            // 
            this.xrLine1.LineStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(1112.5F, 12.5F);
            // 
            // GroupFooter2
            // 
            this.GroupFooter2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.lblTotalTgl,
            this.xrLine1});
            this.GroupFooter2.HeightF = 25F;
            this.GroupFooter2.Level = 1;
            this.GroupFooter2.Name = "GroupFooter2";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(562.5F, 12.5F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(425F, 12.5F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Total Tanggal [ReceivedDateInString] : ";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // lblTotalTgl
            // 
            this.lblTotalTgl.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblTotalTgl.LocationFloat = new DevExpress.Utils.PointFloat(1000F, 12.5F);
            this.lblTotalTgl.Name = "lblTotalTgl";
            this.lblTotalTgl.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalTgl.SizeF = new System.Drawing.SizeF(112.5F, 12.5F);
            this.lblTotalTgl.StylePriority.UseFont = false;
            this.lblTotalTgl.StylePriority.UseTextAlignment = false;
            xrSummary4.FormatString = "{0:#,##0.00}";
            xrSummary4.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            xrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.lblTotalTgl.Summary = xrSummary4;
            this.lblTotalTgl.Text = "lblTotalTgl";
            this.lblTotalTgl.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.lblTotalTgl.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.lblTotalTgl_SummaryGetResult);
            // 
            // GroupFooter3
            // 
            this.GroupFooter3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.lblGrandTotal,
            this.xrLine2});
            this.GroupFooter3.HeightF = 25F;
            this.GroupFooter3.Level = 2;
            this.GroupFooter3.Name = "GroupFooter3";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(562.5F, 12.5F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(425F, 12.5F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Grand Total : ";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // lblGrandTotal
            // 
            this.lblGrandTotal.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.lblGrandTotal.LocationFloat = new DevExpress.Utils.PointFloat(1000F, 12.5F);
            this.lblGrandTotal.Name = "lblGrandTotal";
            this.lblGrandTotal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblGrandTotal.SizeF = new System.Drawing.SizeF(112.5F, 12.5F);
            this.lblGrandTotal.StylePriority.UseFont = false;
            this.lblGrandTotal.StylePriority.UseTextAlignment = false;
            xrSummary5.FormatString = "{0:#,##0.00}";
            xrSummary5.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            xrSummary5.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.lblGrandTotal.Summary = xrSummary5;
            this.lblGrandTotal.Text = "lblTotalTgl";
            this.lblGrandTotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.lblGrandTotal.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.lblGrandTotal_SummaryGetResult);
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(1112.5F, 12.5F);
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(62.5F, 0F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(12.5F, 12.5F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = ":";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(62.5F, 12.5F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Tanggal";
            // 
            // xrLabel6
            // 
            this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ReceivedDateInString")});
            this.xrLabel6.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(75F, 0F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(350F, 12.5F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "xrLabel6";
            // 
            // LPenerimaanBarang
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.PageHeader,
            this.GroupHeader1,
            this.GroupHeader2,
            this.GroupFooter1,
            this.GroupFooter2,
            this.GroupFooter3,
            this.PageFooter,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.TotalPrice});
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.GroupFooter3, 0);
            this.Controls.SetChildIndex(this.GroupFooter2, 0);
            this.Controls.SetChildIndex(this.GroupFooter1, 0);
            this.Controls.SetChildIndex(this.GroupHeader2, 0);
            this.Controls.SetChildIndex(this.GroupHeader1, 0);
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell8;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell11;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell13;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell14;
        private DevExpress.XtraReports.UI.XRTableCell lblSubTotal;
        private DevExpress.XtraReports.UI.XRTableCell lblHeaderPPN;
        private DevExpress.XtraReports.UI.XRTableCell lblHeaderDisc;
        private DevExpress.XtraReports.UI.XRTableCell lblHeaderMaterai;
        private DevExpress.XtraReports.UI.XRTableCell lblHeaderOngkos;
        private DevExpress.XtraReports.UI.XRTableCell lblTotal;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.CalculatedField TotalPrice;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel lblTotalTgl;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel lblGrandTotal;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
    }
}
