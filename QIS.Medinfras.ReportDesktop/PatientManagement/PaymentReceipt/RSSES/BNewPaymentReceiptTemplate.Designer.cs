namespace QIS.Medinfras.ReportDesktop
{
    partial class BNewPaymentReceiptTemplate
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
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAmount = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPrintNumber = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.lblReceiptNo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTanggal = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAmountInWordEng = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReceivedFrom = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRemarks = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAmountInWordInd = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 64.00001F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 31.43752F;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetPaymentReceiptCustom);
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.lblAmount,
            this.lblPrintNumber,
            this.xrPageBreak1,
            this.lblReceiptNo,
            this.lblTanggal,
            this.lblAmountInWordEng,
            this.lblReceivedFrom,
            this.lblRemarks,
            this.lblAmountInWordInd});
            this.GroupHeader1.Dpi = 254F;
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("BusinessPartnerID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 1246.231F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Remarks")});
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(454.6251F, 627.0625F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(233.2912F, 58.41998F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "xrLabel1";
            // 
            // lblAmount
            // 
            this.lblAmount.Dpi = 254F;
            this.lblAmount.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmount.LocationFloat = new DevExpress.Utils.PointFloat(293.6866F, 894.2918F);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAmount.SizeF = new System.Drawing.SizeF(1344.083F, 61.06586F);
            this.lblAmount.StylePriority.UseFont = false;
            this.lblAmount.StylePriority.UseTextAlignment = false;
            this.lblAmount.Text = "[cfReceiptAmountInString]";
            this.lblAmount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblPrintNumber
            // 
            this.lblPrintNumber.Dpi = 254F;
            this.lblPrintNumber.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintNumber.LocationFloat = new DevExpress.Utils.PointFloat(47.62597F, 1183.977F);
            this.lblPrintNumber.Name = "lblPrintNumber";
            this.lblPrintNumber.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPrintNumber.SizeF = new System.Drawing.SizeF(1209.146F, 37.25342F);
            this.lblPrintNumber.StylePriority.UseFont = false;
            this.lblPrintNumber.StylePriority.UseTextAlignment = false;
            this.lblPrintNumber.Text = "cetakan ke - [PrintNumber]";
            this.lblPrintNumber.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.lblPrintNumber.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblPrintNumber_BeforePrint);
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.Dpi = 254F;
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 1231.417F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // lblReceiptNo
            // 
            this.lblReceiptNo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "MedicalNo")});
            this.lblReceiptNo.Dpi = 254F;
            this.lblReceiptNo.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptNo.LocationFloat = new DevExpress.Utils.PointFloat(454.6255F, 140.2292F);
            this.lblReceiptNo.Name = "lblReceiptNo";
            this.lblReceiptNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReceiptNo.SizeF = new System.Drawing.SizeF(1376.291F, 71.64917F);
            this.lblReceiptNo.StylePriority.UseFont = false;
            this.lblReceiptNo.StylePriority.UseTextAlignment = false;
            this.lblReceiptNo.Text = "lblReceiptNo";
            this.lblReceiptNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblTanggal
            // 
            this.lblTanggal.Dpi = 254F;
            this.lblTanggal.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTanggal.LocationFloat = new DevExpress.Utils.PointFloat(1497.542F, 769.9375F);
            this.lblTanggal.Name = "lblTanggal";
            this.lblTanggal.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTanggal.SizeF = new System.Drawing.SizeF(333.3754F, 37.25336F);
            this.lblTanggal.StylePriority.UseFont = false;
            this.lblTanggal.StylePriority.UseTextAlignment = false;
            this.lblTanggal.Text = "lblTanggal";
            this.lblTanggal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lblTanggal.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblTanggal_BeforePrint);
            // 
            // lblAmountInWordEng
            // 
            this.lblAmountInWordEng.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfReceiptAmountInStringEng")});
            this.lblAmountInWordEng.Dpi = 254F;
            this.lblAmountInWordEng.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Italic);
            this.lblAmountInWordEng.LocationFloat = new DevExpress.Utils.PointFloat(467.8548F, 452.6492F);
            this.lblAmountInWordEng.Name = "lblAmountInWordEng";
            this.lblAmountInWordEng.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAmountInWordEng.SizeF = new System.Drawing.SizeF(1551.374F, 58.41989F);
            this.lblAmountInWordEng.StylePriority.UseFont = false;
            this.lblAmountInWordEng.StylePriority.UseTextAlignment = false;
            this.lblAmountInWordEng.Text = "lblAmountInWord";
            this.lblAmountInWordEng.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblReceivedFrom
            // 
            this.lblReceivedFrom.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PrintAsName")});
            this.lblReceivedFrom.Dpi = 254F;
            this.lblReceivedFrom.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceivedFrom.LocationFloat = new DevExpress.Utils.PointFloat(454.6255F, 272.5208F);
            this.lblReceivedFrom.Name = "lblReceivedFrom";
            this.lblReceivedFrom.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReceivedFrom.SizeF = new System.Drawing.SizeF(1376.291F, 61.0658F);
            this.lblReceivedFrom.StylePriority.UseFont = false;
            this.lblReceivedFrom.StylePriority.UseTextAlignment = false;
            this.lblReceivedFrom.Text = "lblReceivedFrom";
            this.lblReceivedFrom.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblRemarks
            // 
            this.lblRemarks.Dpi = 254F;
            this.lblRemarks.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemarks.LocationFloat = new DevExpress.Utils.PointFloat(454.6255F, 544.83F);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblRemarks.SizeF = new System.Drawing.SizeF(1193.728F, 61.0658F);
            this.lblRemarks.StylePriority.UseFont = false;
            this.lblRemarks.StylePriority.UseTextAlignment = false;
            this.lblRemarks.Text = "([MedicalNo]) [PatientName] | [RegistrationNo] | [PaymentReceiptNo]";
            this.lblRemarks.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblAmountInWordInd
            // 
            this.lblAmountInWordInd.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfReceiptAmountInStringInd")});
            this.lblAmountInWordInd.Dpi = 254F;
            this.lblAmountInWordInd.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmountInWordInd.LocationFloat = new DevExpress.Utils.PointFloat(454.6255F, 394.2292F);
            this.lblAmountInWordInd.Name = "lblAmountInWordInd";
            this.lblAmountInWordInd.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAmountInWordInd.SizeF = new System.Drawing.SizeF(1551.375F, 58.41992F);
            this.lblAmountInWordInd.StylePriority.UseFont = false;
            this.lblAmountInWordInd.StylePriority.UseTextAlignment = false;
            this.lblAmountInWordInd.Text = "lblAmountInWordInd";
            this.lblAmountInWordInd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // BNewPaymentReceiptTemplate
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.GroupHeader1});
            this.DataSource = this.bs;
            this.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Italic);
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.GroupHeader1, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private System.Windows.Forms.BindingSource bs;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel lblReceiptNo;
        private DevExpress.XtraReports.UI.XRLabel lblTanggal;
        private DevExpress.XtraReports.UI.XRLabel lblAmountInWordEng;
        private DevExpress.XtraReports.UI.XRLabel lblReceivedFrom;
        private DevExpress.XtraReports.UI.XRLabel lblRemarks;
        private DevExpress.XtraReports.UI.XRLabel lblAmountInWordInd;
        private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak1;
        private DevExpress.XtraReports.UI.XRLabel lblPrintNumber;
        private DevExpress.XtraReports.UI.XRLabel lblAmount;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
    }
}
