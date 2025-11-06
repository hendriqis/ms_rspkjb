namespace QIS.Medinfras.ReportDesktop
{
    partial class BFormulirMultigunaNiaga
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
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.cReceiptHealthcareName = new DevExpress.XtraReports.UI.XRLabel();
            this.cNoRekening = new DevExpress.XtraReports.UI.XRLabel();
            this.cNameBank = new DevExpress.XtraReports.UI.XRLabel();
            this.cNameSupplier = new DevExpress.XtraReports.UI.XRLabel();
            this.cCity = new DevExpress.XtraReports.UI.XRLabel();
            this.cPayment = new DevExpress.XtraReports.UI.XRLabel();
            this.cPhone = new DevExpress.XtraReports.UI.XRLabel();
            this.cAdress = new DevExpress.XtraReports.UI.XRLabel();
            this.cSay = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.StylePriority.UseFont = false;
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vSupplierPaymentHutang);
            // 
            // xrLine1
            // 
            this.xrLine1.Visible = false;
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.StylePriority.UseBorders = false;
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            this.lblReportTitle.Visible = false;
            // 
            // lblReportSubTitle
            // 
            this.lblReportSubTitle.StylePriority.UseBorders = false;
            this.lblReportSubTitle.StylePriority.UseFont = false;
            this.lblReportSubTitle.StylePriority.UseTextAlignment = false;
            this.lblReportSubTitle.Visible = false;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Visible = false;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.cSay,
            this.cAdress,
            this.cPhone,
            this.cPayment,
            this.cCity,
            this.cNameSupplier,
            this.cNameBank,
            this.cNoRekening,
            this.cReceiptHealthcareName});
            this.Detail.HeightF = 1139.461F;
            this.Detail.KeepTogether = true;
            this.Detail.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("PurchaseInvoiceID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // PageHeader
            // 
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 0F;
            this.PageHeader.Name = "PageHeader";
            // 
            // cReceiptHealthcareName
            // 
            this.cReceiptHealthcareName.Dpi = 254F;
            this.cReceiptHealthcareName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cReceiptHealthcareName.LocationFloat = new DevExpress.Utils.PointFloat(1254.125F, 883.6024F);
            this.cReceiptHealthcareName.Name = "cReceiptHealthcareName";
            this.cReceiptHealthcareName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.cReceiptHealthcareName.SizeF = new System.Drawing.SizeF(738.6447F, 47.83661F);
            this.cReceiptHealthcareName.StylePriority.UseFont = false;
            this.cReceiptHealthcareName.StylePriority.UseTextAlignment = false;
            this.cReceiptHealthcareName.Text = "cReceiptHealthcareName";
            this.cReceiptHealthcareName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // cNoRekening
            // 
            this.cNoRekening.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SupplierBankAccountNo")});
            this.cNoRekening.Dpi = 254F;
            this.cNoRekening.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cNoRekening.LocationFloat = new DevExpress.Utils.PointFloat(378.3542F, 113.665F);
            this.cNoRekening.Name = "cNoRekening";
            this.cNoRekening.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.cNoRekening.SizeF = new System.Drawing.SizeF(489.4791F, 58.42F);
            this.cNoRekening.StylePriority.UseFont = false;
            this.cNoRekening.StylePriority.UseTextAlignment = false;
            this.cNoRekening.Text = "cNoRekening";
            this.cNoRekening.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // cNameBank
            // 
            this.cNameBank.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SupplierBankName")});
            this.cNameBank.Dpi = 254F;
            this.cNameBank.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cNameBank.LocationFloat = new DevExpress.Utils.PointFloat(127F, 298.8732F);
            this.cNameBank.Name = "cNameBank";
            this.cNameBank.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.cNameBank.SizeF = new System.Drawing.SizeF(740.8333F, 58.42001F);
            this.cNameBank.StylePriority.UseFont = false;
            this.cNameBank.StylePriority.UseTextAlignment = false;
            this.cNameBank.Text = "[BankName]";
            this.cNameBank.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // cNameSupplier
            // 
            this.cNameSupplier.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SupplierBankAccountName")});
            this.cNameSupplier.Dpi = 254F;
            this.cNameSupplier.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cNameSupplier.LocationFloat = new DevExpress.Utils.PointFloat(127F, 211.5607F);
            this.cNameSupplier.Name = "cNameSupplier";
            this.cNameSupplier.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.cNameSupplier.SizeF = new System.Drawing.SizeF(740.8333F, 58.42001F);
            this.cNameSupplier.StylePriority.UseFont = false;
            this.cNameSupplier.StylePriority.UseTextAlignment = false;
            this.cNameSupplier.Text = "cNameSupplier";
            this.cNameSupplier.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // cCity
            // 
            this.cCity.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "BillingAddressCity")});
            this.cCity.Dpi = 254F;
            this.cCity.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cCity.LocationFloat = new DevExpress.Utils.PointFloat(127F, 497.3107F);
            this.cCity.Name = "cCity";
            this.cCity.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.cCity.SizeF = new System.Drawing.SizeF(378.3542F, 58.41998F);
            this.cCity.StylePriority.UseFont = false;
            this.cCity.StylePriority.UseTextAlignment = false;
            this.cCity.Text = "cCity";
            this.cCity.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // cPayment
            // 
            this.cPayment.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TotalTransactionAmount", "{0:n2}")});
            this.cPayment.Dpi = 254F;
            this.cPayment.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cPayment.LocationFloat = new DevExpress.Utils.PointFloat(1386.416F, 539.6442F);
            this.cPayment.Name = "cPayment";
            this.cPayment.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.cPayment.SizeF = new System.Drawing.SizeF(606.354F, 58.41998F);
            this.cPayment.StylePriority.UseFont = false;
            this.cPayment.StylePriority.UseTextAlignment = false;
            this.cPayment.Text = "cPayment";
            this.cPayment.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // cPhone
            // 
            this.cPhone.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "BillingAddressPhoneNo1")});
            this.cPhone.Dpi = 254F;
            this.cPhone.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cPhone.LocationFloat = new DevExpress.Utils.PointFloat(505.3542F, 497.3106F);
            this.cPhone.Name = "cPhone";
            this.cPhone.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.cPhone.SizeF = new System.Drawing.SizeF(362.4792F, 58.42007F);
            this.cPhone.StylePriority.UseFont = false;
            this.cPhone.StylePriority.UseTextAlignment = false;
            this.cPhone.Text = "cPhone";
            this.cPhone.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // cAdress
            // 
            this.cAdress.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "BillingAddressStreetName")});
            this.cAdress.Dpi = 254F;
            this.cAdress.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cAdress.LocationFloat = new DevExpress.Utils.PointFloat(127F, 388.8316F);
            this.cAdress.Name = "cAdress";
            this.cAdress.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.cAdress.SizeF = new System.Drawing.SizeF(740.8333F, 58.41995F);
            this.cAdress.StylePriority.UseFont = false;
            this.cAdress.StylePriority.UseTextAlignment = false;
            this.cAdress.Text = "cAdress";
            this.cAdress.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // cSay
            // 
            this.cSay.Dpi = 254F;
            this.cSay.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cSay.LocationFloat = new DevExpress.Utils.PointFloat(1254.125F, 634.8942F);
            this.cSay.Name = "cSay";
            this.cSay.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.cSay.SizeF = new System.Drawing.SizeF(738.6454F, 69.0033F);
            this.cSay.StylePriority.UseFont = false;
            this.cSay.StylePriority.UseTextAlignment = false;
            this.cSay.Text = "[TotalPaymentAmountInString]";
            this.cSay.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // BFormulirMultigunaNiaga
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.PageHeader});
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRLabel cReceiptHealthcareName;
        private DevExpress.XtraReports.UI.XRLabel cNoRekening;
        private DevExpress.XtraReports.UI.XRLabel cAdress;
        private DevExpress.XtraReports.UI.XRLabel cPhone;
        private DevExpress.XtraReports.UI.XRLabel cPayment;
        private DevExpress.XtraReports.UI.XRLabel cCity;
        private DevExpress.XtraReports.UI.XRLabel cNameSupplier;
        private DevExpress.XtraReports.UI.XRLabel cNameBank;
        private DevExpress.XtraReports.UI.XRLabel cSay;
    }
}
