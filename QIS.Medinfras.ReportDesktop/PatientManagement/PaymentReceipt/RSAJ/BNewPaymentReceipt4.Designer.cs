namespace QIS.Medinfras.ReportDesktop
{
    partial class BNewPaymentReceipt4
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
            this.lblReceiptNo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReceivedFrom = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAmount = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAmountInWordInd = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRemarks = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTanggal = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTTD = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRemarks2 = new DevExpress.XtraReports.UI.XRLabel();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.lblPrintNumber = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDiagnosa = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.cSEPInfo = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 20.31255F;
            // 
            // ReportHeader
            // 
            this.ReportHeader.StylePriority.UseTextAlignment = false;
            this.ReportHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 20F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 20.85419F;
            // 
            // lblReceiptNo
            // 
            this.lblReceiptNo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PaymentReceiptNo")});
            this.lblReceiptNo.Dpi = 254F;
            this.lblReceiptNo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptNo.LocationFloat = new DevExpress.Utils.PointFloat(1722.79F, 165.3333F);
            this.lblReceiptNo.Name = "lblReceiptNo";
            this.lblReceiptNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReceiptNo.SizeF = new System.Drawing.SizeF(360.2092F, 31.96164F);
            this.lblReceiptNo.StylePriority.UseFont = false;
            this.lblReceiptNo.StylePriority.UseTextAlignment = false;
            this.lblReceiptNo.Text = "lblReceiptNo";
            this.lblReceiptNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lblReceiptNo.WordWrap = false;
            // 
            // lblReceivedFrom
            // 
            this.lblReceivedFrom.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PrintAsName")});
            this.lblReceivedFrom.Dpi = 254F;
            this.lblReceivedFrom.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceivedFrom.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 216.9583F);
            this.lblReceivedFrom.Name = "lblReceivedFrom";
            this.lblReceivedFrom.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReceivedFrom.SizeF = new System.Drawing.SizeF(1501.188F, 47.83662F);
            this.lblReceivedFrom.StylePriority.UseFont = false;
            this.lblReceivedFrom.StylePriority.UseTextAlignment = false;
            this.lblReceivedFrom.Text = "lblReceivedFrom";
            this.lblReceivedFrom.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblAmount
            // 
            this.lblAmount.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfReceiptAmountInString")});
            this.lblAmount.Dpi = 254F;
            this.lblAmount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmount.LocationFloat = new DevExpress.Utils.PointFloat(95.24992F, 439.0595F);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAmount.SizeF = new System.Drawing.SizeF(521.2292F, 31.96179F);
            this.lblAmount.StylePriority.UseFont = false;
            this.lblAmount.StylePriority.UseTextAlignment = false;
            this.lblAmount.Text = "lblAmount";
            this.lblAmount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblAmountInWordInd
            // 
            this.lblAmountInWordInd.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfReceiptAmountInStringInd")});
            this.lblAmountInWordInd.Dpi = 254F;
            this.lblAmountInWordInd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmountInWordInd.LocationFloat = new DevExpress.Utils.PointFloat(161.3958F, 519F);
            this.lblAmountInWordInd.Name = "lblAmountInWordInd";
            this.lblAmountInWordInd.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAmountInWordInd.SizeF = new System.Drawing.SizeF(1217.084F, 31.96167F);
            this.lblAmountInWordInd.StylePriority.UseFont = false;
            this.lblAmountInWordInd.StylePriority.UseTextAlignment = false;
            this.lblAmountInWordInd.Text = "lblAmountInWordInd";
            this.lblAmountInWordInd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblRemarks
            // 
            this.lblRemarks.Dpi = 254F;
            this.lblRemarks.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemarks.LocationFloat = new DevExpress.Utils.PointFloat(357.1864F, 321.0833F);
            this.lblRemarks.Multiline = true;
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblRemarks.SizeF = new System.Drawing.SizeF(1711.397F, 29.31577F);
            this.lblRemarks.StylePriority.UseFont = false;
            this.lblRemarks.StylePriority.UseTextAlignment = false;
            this.lblRemarks.Text = "BIAYA PENGOBATAN PASIEN [MedicalNo] | [PatientName] | [RegistrationNo] | [Service" +
    "UnitName]";
            this.lblRemarks.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblTanggal
            // 
            this.lblTanggal.Dpi = 254F;
            this.lblTanggal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTanggal.LocationFloat = new DevExpress.Utils.PointFloat(1396.145F, 423.4136F);
            this.lblTanggal.Name = "lblTanggal";
            this.lblTanggal.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTanggal.SizeF = new System.Drawing.SizeF(628.8967F, 31.96167F);
            this.lblTanggal.StylePriority.UseFont = false;
            this.lblTanggal.StylePriority.UseTextAlignment = false;
            this.lblTanggal.Text = "lblTanggal";
            this.lblTanggal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lblTanggal.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblTanggal_BeforePrint);
            // 
            // lblTTD
            // 
            this.lblTTD.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblTTD.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfSignName")});
            this.lblTTD.Dpi = 254F;
            this.lblTTD.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTTD.LocationFloat = new DevExpress.Utils.PointFloat(1396.145F, 621.8337F);
            this.lblTTD.Name = "lblTTD";
            this.lblTTD.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTTD.SizeF = new System.Drawing.SizeF(628.8967F, 31.96155F);
            this.lblTTD.StylePriority.UseBorders = false;
            this.lblTTD.StylePriority.UseFont = false;
            this.lblTTD.StylePriority.UseTextAlignment = false;
            this.lblTTD.Text = "lblTTD";
            this.lblTTD.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblRemarks2
            // 
            this.lblRemarks2.Dpi = 254F;
            this.lblRemarks2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemarks2.LocationFloat = new DevExpress.Utils.PointFloat(25.00025F, 592.5179F);
            this.lblRemarks2.Multiline = true;
            this.lblRemarks2.Name = "lblRemarks2";
            this.lblRemarks2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblRemarks2.SizeF = new System.Drawing.SizeF(1364.063F, 29.31573F);
            this.lblRemarks2.StylePriority.UseFont = false;
            this.lblRemarks2.StylePriority.UseTextAlignment = false;
            this.lblRemarks2.Text = "Tipe Pembayaran : [PaymentMethod]";
            this.lblRemarks2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetPaymentReceiptCustom);
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.cSEPInfo,
            this.lblPrintNumber,
            this.lblDiagnosa,
            this.xrPageBreak1,
            this.lblReceivedFrom,
            this.lblAmount,
            this.lblAmountInWordInd,
            this.lblRemarks,
            this.lblTTD,
            this.lblTanggal,
            this.lblReceiptNo,
            this.lblRemarks2});
            this.GroupHeader1.Dpi = 254F;
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("BusinessPartnerID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 734.2484F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // lblPrintNumber
            // 
            this.lblPrintNumber.Dpi = 254F;
            this.lblPrintNumber.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintNumber.LocationFloat = new DevExpress.Utils.PointFloat(25.00025F, 679.9327F);
            this.lblPrintNumber.Multiline = true;
            this.lblPrintNumber.Name = "lblPrintNumber";
            this.lblPrintNumber.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPrintNumber.SizeF = new System.Drawing.SizeF(1364.063F, 29.31573F);
            this.lblPrintNumber.StylePriority.UseFont = false;
            this.lblPrintNumber.StylePriority.UseTextAlignment = false;
            this.lblPrintNumber.Text = "Cetakan ke - [PrintNumber]";
            this.lblPrintNumber.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lblPrintNumber.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblPrintNumber_BeforePrint);
            // 
            // lblDiagnosa
            // 
            this.lblDiagnosa.Dpi = 254F;
            this.lblDiagnosa.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiagnosa.LocationFloat = new DevExpress.Utils.PointFloat(25.00025F, 621.8337F);
            this.lblDiagnosa.Multiline = true;
            this.lblDiagnosa.Name = "lblDiagnosa";
            this.lblDiagnosa.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblDiagnosa.SizeF = new System.Drawing.SizeF(1364.063F, 29.31573F);
            this.lblDiagnosa.StylePriority.UseFont = false;
            this.lblDiagnosa.StylePriority.UseTextAlignment = false;
            this.lblDiagnosa.Text = "Diagnosa : [Diagnose1]";
            this.lblDiagnosa.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lblDiagnosa.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblDiagnosa_BeforePrint);
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.Dpi = 254F;
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 729.1683F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // cSEPInfo
            // 
            this.cSEPInfo.Dpi = 254F;
            this.cSEPInfo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cSEPInfo.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 651.1495F);
            this.cSEPInfo.Multiline = true;
            this.cSEPInfo.Name = "cSEPInfo";
            this.cSEPInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.cSEPInfo.SizeF = new System.Drawing.SizeF(1364.063F, 28.7832F);
            this.cSEPInfo.StylePriority.UseFont = false;
            this.cSEPInfo.StylePriority.UseTextAlignment = false;
            this.cSEPInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // BNewPaymentReceipt4
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.GroupHeader1});
            this.DataSource = this.bs;
            this.Margins = new System.Drawing.Printing.Margins(24, 0, 20, 20);
            this.PageHeight = 930;
            this.PageWidth = 2150;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ShowPrintMarginsWarning = false;
            this.SnapGridSize = 20F;
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

        private DevExpress.XtraReports.UI.XRLabel lblReceiptNo;
        private DevExpress.XtraReports.UI.XRLabel lblTanggal;
        private DevExpress.XtraReports.UI.XRLabel lblTTD;
        private DevExpress.XtraReports.UI.XRLabel lblRemarks;
        private DevExpress.XtraReports.UI.XRLabel lblReceivedFrom;
        private DevExpress.XtraReports.UI.XRLabel lblAmount;
        private DevExpress.XtraReports.UI.XRLabel lblAmountInWordInd;
        private DevExpress.XtraReports.UI.XRLabel lblRemarks2;
        private System.Windows.Forms.BindingSource bs;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak1;
        private DevExpress.XtraReports.UI.XRLabel lblDiagnosa;
        private DevExpress.XtraReports.UI.XRLabel lblPrintNumber;
        private DevExpress.XtraReports.UI.XRLabel cSEPInfo;
    }
}
