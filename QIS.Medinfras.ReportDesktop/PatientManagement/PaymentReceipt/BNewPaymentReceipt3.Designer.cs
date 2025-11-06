namespace QIS.Medinfras.ReportDesktop
{
    partial class BNewPaymentReceipt3
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
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.lblAmountInWordInd = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAmount = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.subPaymentReceiptDt = new DevExpress.XtraReports.UI.XRSubreport();
            this.bNewPaymentReceipt2Dt = new QIS.Medinfras.ReportDesktop.BNewPaymentReceipt2Dt();
            this.lblTTD = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDiagnosa = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRemarks = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTanggal = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReceiptNo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReceivedFrom = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReceiptDate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPrintNumber = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bNewPaymentReceipt2Dt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 32.24997F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 0F;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetPaymentReceiptCustom);
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblPrintNumber,
            this.xrPageBreak1,
            this.lblAmountInWordInd,
            this.xrLabel13,
            this.xrLabel14,
            this.lblAmount,
            this.xrLabel10,
            this.xrLabel11,
            this.subPaymentReceiptDt,
            this.lblTTD,
            this.lblDiagnosa,
            this.lblRemarks,
            this.xrLabel16,
            this.lblTanggal,
            this.lblReceiptNo,
            this.xrLabel4,
            this.xrLabel5,
            this.xrLabel1,
            this.xrLabel2,
            this.xrLabel3,
            this.lblReceivedFrom,
            this.xrLabel8,
            this.xrLabel9,
            this.lblReceiptDate,
            this.xrLabel6,
            this.xrLabel7});
            this.GroupHeader1.Dpi = 254F;
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("BusinessPartnerID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 828.04F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.Dpi = 254F;
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 822.96F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // lblAmountInWordInd
            // 
            this.lblAmountInWordInd.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfReceiptAmountInStringInd")});
            this.lblAmountInWordInd.Dpi = 254F;
            this.lblAmountInWordInd.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmountInWordInd.LocationFloat = new DevExpress.Utils.PointFloat(247.25F, 412.75F);
            this.lblAmountInWordInd.Name = "lblAmountInWordInd";
            this.lblAmountInWordInd.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAmountInWordInd.SizeF = new System.Drawing.SizeF(1749.354F, 61.06583F);
            this.lblAmountInWordInd.StylePriority.UseFont = false;
            this.lblAmountInWordInd.StylePriority.UseTextAlignment = false;
            this.lblAmountInWordInd.Text = "lblAmountInWordInd";
            this.lblAmountInWordInd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Dpi = 254F;
            this.xrLabel13.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 508F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(484.1875F, 29.31583F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.Text = "Keterangan";
            // 
            // xrLabel14
            // 
            this.xrLabel14.Dpi = 254F;
            this.xrLabel14.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(509.1875F, 508F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(60.85419F, 29.31583F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = ":";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblAmount
            // 
            this.lblAmount.Dpi = 254F;
            this.lblAmount.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmount.LocationFloat = new DevExpress.Utils.PointFloat(572.6875F, 285.75F);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAmount.SizeF = new System.Drawing.SizeF(1435.958F, 29.31583F);
            this.lblAmount.StylePriority.UseFont = false;
            this.lblAmount.Text = "Rp [cfReceiptAmountInString]";
            // 
            // xrLabel10
            // 
            this.xrLabel10.Dpi = 254F;
            this.xrLabel10.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 349.25F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(484.1875F, 29.31583F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.Text = "Terbilang";
            // 
            // xrLabel11
            // 
            this.xrLabel11.Dpi = 254F;
            this.xrLabel11.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(509.1875F, 349.25F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(60.85419F, 29.31583F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = ":";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // subPaymentReceiptDt
            // 
            this.subPaymentReceiptDt.Dpi = 254F;
            this.subPaymentReceiptDt.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 666.75F);
            this.subPaymentReceiptDt.Name = "subPaymentReceiptDt";
            this.subPaymentReceiptDt.ReportSource = this.bNewPaymentReceipt2Dt;
            this.subPaymentReceiptDt.SizeF = new System.Drawing.SizeF(1307.041F, 58.41986F);
            // 
            // lblTTD
            // 
            this.lblTTD.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblTTD.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfSignName")});
            this.lblTTD.Dpi = 254F;
            this.lblTTD.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTTD.LocationFloat = new DevExpress.Utils.PointFloat(1390.25F, 748.7708F);
            this.lblTTD.Name = "lblTTD";
            this.lblTTD.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTTD.SizeF = new System.Drawing.SizeF(608.5417F, 29.31586F);
            this.lblTTD.StylePriority.UseBorders = false;
            this.lblTTD.StylePriority.UseFont = false;
            this.lblTTD.StylePriority.UseTextAlignment = false;
            this.lblTTD.Text = "lblTTD";
            this.lblTTD.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblDiagnosa
            // 
            this.lblDiagnosa.Dpi = 254F;
            this.lblDiagnosa.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiagnosa.LocationFloat = new DevExpress.Utils.PointFloat(25.00009F, 748.7708F);
            this.lblDiagnosa.Multiline = true;
            this.lblDiagnosa.Name = "lblDiagnosa";
            this.lblDiagnosa.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblDiagnosa.SizeF = new System.Drawing.SizeF(1307.041F, 29.31573F);
            this.lblDiagnosa.StylePriority.UseFont = false;
            this.lblDiagnosa.StylePriority.UseTextAlignment = false;
            this.lblDiagnosa.Text = "Diagnosa : [Diagnose1]";
            this.lblDiagnosa.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lblDiagnosa.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblDiagnosa_BeforePrint);
            // 
            // lblRemarks
            // 
            this.lblRemarks.Dpi = 254F;
            this.lblRemarks.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemarks.LocationFloat = new DevExpress.Utils.PointFloat(572.6875F, 508F);
            this.lblRemarks.Name = "lblRemarks";
            this.lblRemarks.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblRemarks.SizeF = new System.Drawing.SizeF(1435.958F, 29.31583F);
            this.lblRemarks.StylePriority.UseFont = false;
            this.lblRemarks.Text = "([MedicalNo]) [PatientName] | [RegistrationNo]";
            // 
            // xrLabel16
            // 
            this.xrLabel16.Dpi = 254F;
            this.xrLabel16.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 571.5F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(484.1875F, 58.4201F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.Text = "*** TERIMA KASIH ***";
            // 
            // lblTanggal
            // 
            this.lblTanggal.Dpi = 254F;
            this.lblTanggal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTanggal.LocationFloat = new DevExpress.Utils.PointFloat(1390.25F, 571.5F);
            this.lblTanggal.Name = "lblTanggal";
            this.lblTanggal.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTanggal.SizeF = new System.Drawing.SizeF(608.5417F, 29.31586F);
            this.lblTanggal.StylePriority.UseFont = false;
            this.lblTanggal.StylePriority.UseTextAlignment = false;
            this.lblTanggal.Text = "lblTanggal";
            this.lblTanggal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lblTanggal.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblTanggal_BeforePrint);
            // 
            // lblReceiptNo
            // 
            this.lblReceiptNo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PaymentReceiptNo")});
            this.lblReceiptNo.Dpi = 254F;
            this.lblReceiptNo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptNo.LocationFloat = new DevExpress.Utils.PointFloat(572.6875F, 95.25F);
            this.lblReceiptNo.Name = "lblReceiptNo";
            this.lblReceiptNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReceiptNo.SizeF = new System.Drawing.SizeF(1435.958F, 29.31583F);
            this.lblReceiptNo.StylePriority.UseFont = false;
            this.lblReceiptNo.Text = "lblReceiptNo";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 158.75F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(484.1875F, 29.31583F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "Tanggal Kwitansi";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(509.1875F, 158.75F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(60.85419F, 29.31583F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = ":";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 95.25F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(484.1875F, 29.31583F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Nomor Kwitansi";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(1981F, 58.42F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "KWITANSI";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(509.1875F, 95.25F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(60.85419F, 29.31583F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = ":";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblReceivedFrom
            // 
            this.lblReceivedFrom.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PrintAsName")});
            this.lblReceivedFrom.Dpi = 254F;
            this.lblReceivedFrom.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceivedFrom.LocationFloat = new DevExpress.Utils.PointFloat(572.6875F, 222.25F);
            this.lblReceivedFrom.Name = "lblReceivedFrom";
            this.lblReceivedFrom.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReceivedFrom.SizeF = new System.Drawing.SizeF(1435.958F, 29.31583F);
            this.lblReceivedFrom.StylePriority.UseFont = false;
            this.lblReceivedFrom.Text = "lblReceivedFrom";
            // 
            // xrLabel8
            // 
            this.xrLabel8.Dpi = 254F;
            this.xrLabel8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 285.75F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(484.1875F, 29.31583F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "Uang Sejumlah";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Dpi = 254F;
            this.xrLabel9.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(509.1875F, 285.75F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(60.85419F, 29.31583F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = ":";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblReceiptDate
            // 
            this.lblReceiptDate.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfReceiptDateInString")});
            this.lblReceiptDate.Dpi = 254F;
            this.lblReceiptDate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptDate.LocationFloat = new DevExpress.Utils.PointFloat(572.6875F, 158.75F);
            this.lblReceiptDate.Name = "lblReceiptDate";
            this.lblReceiptDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReceiptDate.SizeF = new System.Drawing.SizeF(1435.958F, 29.31583F);
            this.lblReceiptDate.StylePriority.UseFont = false;
            this.lblReceiptDate.Text = "lblReceiptDate";
            // 
            // xrLabel6
            // 
            this.xrLabel6.Dpi = 254F;
            this.xrLabel6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 222.25F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(484.1875F, 29.31583F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "Sudah Diterima Dari";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Dpi = 254F;
            this.xrLabel7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(509.1875F, 222.25F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(60.85419F, 29.31583F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = ":";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblPrintNumber
            // 
            this.lblPrintNumber.Dpi = 254F;
            this.lblPrintNumber.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintNumber.LocationFloat = new DevExpress.Utils.PointFloat(25.00009F, 793.6442F);
            this.lblPrintNumber.Multiline = true;
            this.lblPrintNumber.Name = "lblPrintNumber";
            this.lblPrintNumber.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPrintNumber.SizeF = new System.Drawing.SizeF(1307.041F, 29.31573F);
            this.lblPrintNumber.StylePriority.UseFont = false;
            this.lblPrintNumber.StylePriority.UseTextAlignment = false;
            this.lblPrintNumber.Text = "cetakan ke - [PrintNumber]";
            this.lblPrintNumber.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.lblPrintNumber.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblPrintNumber_BeforePrint);
            // 
            // BNewPaymentReceipt3
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.GroupHeader1});
            this.DataSource = this.bs;
            this.Margins = new System.Drawing.Printing.Margins(64, 64, 64, 32);
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.GroupHeader1, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bNewPaymentReceipt2Dt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private System.Windows.Forms.BindingSource bs;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel lblAmountInWordInd;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
        private DevExpress.XtraReports.UI.XRLabel lblAmount;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRSubreport subPaymentReceiptDt;
        private BNewPaymentReceipt2Dt bNewPaymentReceipt2Dt;
        private DevExpress.XtraReports.UI.XRLabel lblTTD;
        private DevExpress.XtraReports.UI.XRLabel lblDiagnosa;
        private DevExpress.XtraReports.UI.XRLabel lblRemarks;
        private DevExpress.XtraReports.UI.XRLabel xrLabel16;
        private DevExpress.XtraReports.UI.XRLabel lblTanggal;
        private DevExpress.XtraReports.UI.XRLabel lblReceiptNo;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel lblReceivedFrom;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel lblReceiptDate;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak1;
        private DevExpress.XtraReports.UI.XRLabel lblPrintNumber;
    }
}
