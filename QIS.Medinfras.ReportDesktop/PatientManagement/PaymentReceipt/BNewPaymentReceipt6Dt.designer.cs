namespace QIS.Medinfras.ReportDesktop
{
    partial class BNewPaymentReceipt6Dt
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
            this.lblRegistrationNo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblHealthcareName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblNoKwitansi = new DevExpress.XtraReports.UI.XRLabel();
            this.lblJudulNoKwitansi = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTTD = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTanggal = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDiagnosa = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAmountInWordEng = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAmount = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReceivedFrom = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAmountInWordInd = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.lblJudulKwitansi = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 31.43752F;
            this.PageFooter.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.PageFooter_BeforePrint);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetPaymentReceiptCustom);
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageBreak1,
            this.lblRegistrationNo,
            this.lblHealthcareName,
            this.lblNoKwitansi,
            this.lblJudulNoKwitansi,
            this.xrLabel26,
            this.xrLabel25,
            this.xrLabel24,
            this.xrLabel23,
            this.xrLabel22,
            this.xrLabel17,
            this.xrLabel16,
            this.lblTTD,
            this.lblTanggal,
            this.lblDiagnosa,
            this.lblAmountInWordEng,
            this.lblAmount,
            this.lblReceivedFrom,
            this.lblAmountInWordInd,
            this.xrLine1,
            this.lblJudulKwitansi,
            this.xrLabel2,
            this.xrLabel13,
            this.xrLabel12,
            this.xrLabel15,
            this.xrLabel14,
            this.xrLabel9,
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel27,
            this.xrLabel5});
            this.GroupHeader1.Dpi = 254F;
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("BusinessPartnerID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 903.8987F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // lblRegistrationNo
            // 
            this.lblRegistrationNo.Dpi = 254F;
            this.lblRegistrationNo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegistrationNo.LocationFloat = new DevExpress.Utils.PointFloat(1488.146F, 216.9583F);
            this.lblRegistrationNo.Name = "lblRegistrationNo";
            this.lblRegistrationNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblRegistrationNo.SizeF = new System.Drawing.SizeF(485.6469F, 58.41998F);
            this.lblRegistrationNo.StylePriority.UseFont = false;
            this.lblRegistrationNo.StylePriority.UseTextAlignment = false;
            this.lblRegistrationNo.Text = "[PaymentReceiptNo] - [PrintNumber]";
            this.lblRegistrationNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblHealthcareName
            // 
            this.lblHealthcareName.Dpi = 254F;
            this.lblHealthcareName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHealthcareName.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 45.19084F);
            this.lblHealthcareName.Name = "lblHealthcareName";
            this.lblHealthcareName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblHealthcareName.SizeF = new System.Drawing.SizeF(612.6457F, 47.83671F);
            this.lblHealthcareName.StylePriority.UseFont = false;
            this.lblHealthcareName.Text = "lblHealthcareName";
            // 
            // lblNoKwitansi
            // 
            this.lblNoKwitansi.Dpi = 254F;
            this.lblNoKwitansi.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoKwitansi.LocationFloat = new DevExpress.Utils.PointFloat(19.71214F, 865.3986F);
            this.lblNoKwitansi.Name = "lblNoKwitansi";
            this.lblNoKwitansi.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblNoKwitansi.SizeF = new System.Drawing.SizeF(988.3523F, 33.42004F);
            this.lblNoKwitansi.StylePriority.UseFont = false;
            this.lblNoKwitansi.StylePriority.UseTextAlignment = false;
            this.lblNoKwitansi.Text = "lblNoKwitansi";
            this.lblNoKwitansi.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblJudulNoKwitansi
            // 
            this.lblJudulNoKwitansi.Dpi = 254F;
            this.lblJudulNoKwitansi.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJudulNoKwitansi.LocationFloat = new DevExpress.Utils.PointFloat(19.71214F, 830.7912F);
            this.lblJudulNoKwitansi.Name = "lblJudulNoKwitansi";
            this.lblJudulNoKwitansi.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblJudulNoKwitansi.SizeF = new System.Drawing.SizeF(609.9986F, 34.60754F);
            this.lblJudulNoKwitansi.StylePriority.UseFont = false;
            this.lblJudulNoKwitansi.Text = "No Kwitansi yang sudah dibayar";
            // 
            // xrLabel26
            // 
            this.xrLabel26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PatientName")});
            this.xrLabel26.Dpi = 254F;
            this.xrLabel26.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(538.2925F, 288.6073F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(698.5014F, 61.0658F);
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.StylePriority.UseTextAlignment = false;
            this.xrLabel26.Text = "lblReceivedFrom";
            this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel25
            // 
            this.xrLabel25.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel25.Dpi = 254F;
            this.xrLabel25.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(469.5006F, 288.6073F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(68.79169F, 61.0658F);
            this.xrLabel25.StylePriority.UseBorders = false;
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UseTextAlignment = false;
            this.xrLabel25.Text = ":";
            this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel24
            // 
            this.xrLabel24.Dpi = 254F;
            this.xrLabel24.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(25.00082F, 317.9231F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(444.5F, 31.74994F);
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UseTextAlignment = false;
            this.xrLabel24.Text = "Patient\'s Name";
            this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel23
            // 
            this.xrLabel23.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel23.Dpi = 254F;
            this.xrLabel23.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(25.00082F, 288.6073F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(444.5F, 29.31583F);
            this.xrLabel23.StylePriority.UseBorders = false;
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.Text = "Nama Pasien";
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel22
            // 
            this.xrLabel22.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel22.Dpi = 254F;
            this.xrLabel22.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 0F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(1948.792F, 45.19083F);
            this.xrLabel22.StylePriority.UseBorders = false;
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.Text = "Catatan : Bila ada biaya - biaya lain yang belum termasuk dalam rincian ini akan " +
    "ditagihkan kemudian.";
            // 
            // xrLabel17
            // 
            this.xrLabel17.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel17.Dpi = 254F;
            this.xrLabel17.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(472.1471F, 438.9968F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(68.79166F, 61.0658F);
            this.xrLabel17.StylePriority.UseBorders = false;
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = ":";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel16
            // 
            this.xrLabel16.Dpi = 254F;
            this.xrLabel16.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(27.64742F, 468.3127F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(444.5F, 29.31583F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "Amount in Word";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblTTD
            // 
            this.lblTTD.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblTTD.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfSignName")});
            this.lblTTD.Dpi = 254F;
            this.lblTTD.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTTD.LocationFloat = new DevExpress.Utils.PointFloat(1322.918F, 833.437F);
            this.lblTTD.Name = "lblTTD";
            this.lblTTD.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTTD.SizeF = new System.Drawing.SizeF(642.939F, 31.96155F);
            this.lblTTD.StylePriority.UseBorders = false;
            this.lblTTD.StylePriority.UseFont = false;
            this.lblTTD.StylePriority.UseTextAlignment = false;
            this.lblTTD.Text = "lblTTD";
            this.lblTTD.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblTanggal
            // 
            this.lblTanggal.Dpi = 254F;
            this.lblTanggal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTanggal.LocationFloat = new DevExpress.Utils.PointFloat(1322.918F, 603.25F);
            this.lblTanggal.Name = "lblTanggal";
            this.lblTanggal.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTanggal.SizeF = new System.Drawing.SizeF(642.9388F, 37.25336F);
            this.lblTanggal.StylePriority.UseFont = false;
            this.lblTanggal.StylePriority.UseTextAlignment = false;
            this.lblTanggal.Text = "lblTanggal";
            this.lblTanggal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.lblTanggal.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblTanggal_BeforePrint);
            // 
            // lblDiagnosa
            // 
            this.lblDiagnosa.Dpi = 254F;
            this.lblDiagnosa.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiagnosa.LocationFloat = new DevExpress.Utils.PointFloat(19.71214F, 730.2493F);
            this.lblDiagnosa.Name = "lblDiagnosa";
            this.lblDiagnosa.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblDiagnosa.SizeF = new System.Drawing.SizeF(1209.146F, 29.31586F);
            this.lblDiagnosa.StylePriority.UseFont = false;
            this.lblDiagnosa.StylePriority.UseTextAlignment = false;
            this.lblDiagnosa.Text = "Diagnosa : [Diagnose1], [Diagnose2]";
            this.lblDiagnosa.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblAmountInWordEng
            // 
            this.lblAmountInWordEng.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfReceiptAmountInStringEng")});
            this.lblAmountInWordEng.Dpi = 254F;
            this.lblAmountInWordEng.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmountInWordEng.LocationFloat = new DevExpress.Utils.PointFloat(207.5646F, 540.1731F);
            this.lblAmountInWordEng.Name = "lblAmountInWordEng";
            this.lblAmountInWordEng.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAmountInWordEng.SizeF = new System.Drawing.SizeF(1758.292F, 31.96149F);
            this.lblAmountInWordEng.StylePriority.UseFont = false;
            this.lblAmountInWordEng.StylePriority.UseTextAlignment = false;
            this.lblAmountInWordEng.Text = "lblAmountInWord";
            this.lblAmountInWordEng.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblAmount
            // 
            this.lblAmount.Dpi = 254F;
            this.lblAmount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmount.LocationFloat = new DevExpress.Utils.PointFloat(538.2922F, 362.4792F);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAmount.SizeF = new System.Drawing.SizeF(698.5014F, 61.0658F);
            this.lblAmount.StylePriority.UseFont = false;
            this.lblAmount.StylePriority.UseTextAlignment = false;
            this.lblAmount.Text = "Rp [cfReceiptAmountInString]";
            this.lblAmount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblReceivedFrom
            // 
            this.lblReceivedFrom.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PrintAsName")});
            this.lblReceivedFrom.Dpi = 254F;
            this.lblReceivedFrom.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceivedFrom.LocationFloat = new DevExpress.Utils.PointFloat(538.292F, 216.9583F);
            this.lblReceivedFrom.Name = "lblReceivedFrom";
            this.lblReceivedFrom.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReceivedFrom.SizeF = new System.Drawing.SizeF(698.5014F, 61.0658F);
            this.lblReceivedFrom.StylePriority.UseFont = false;
            this.lblReceivedFrom.StylePriority.UseTextAlignment = false;
            this.lblReceivedFrom.Text = "lblReceivedFrom";
            this.lblReceivedFrom.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblAmountInWordInd
            // 
            this.lblAmountInWordInd.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfReceiptAmountInStringInd")});
            this.lblAmountInWordInd.Dpi = 254F;
            this.lblAmountInWordInd.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmountInWordInd.LocationFloat = new DevExpress.Utils.PointFloat(207.5646F, 510.8575F);
            this.lblAmountInWordInd.Name = "lblAmountInWordInd";
            this.lblAmountInWordInd.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAmountInWordInd.SizeF = new System.Drawing.SizeF(1758.292F, 29.31555F);
            this.lblAmountInWordInd.StylePriority.UseFont = false;
            this.lblAmountInWordInd.StylePriority.UseTextAlignment = false;
            this.lblAmountInWordInd.Text = "lblAmountInWordInd";
            this.lblAmountInWordInd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLine1
            // 
            this.xrLine1.Dpi = 254F;
            this.xrLine1.LineWidth = 3;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(764.6458F, 140.8642F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(510.6461F, 13.44083F);
            // 
            // lblJudulKwitansi
            // 
            this.lblJudulKwitansi.Dpi = 254F;
            this.lblJudulKwitansi.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJudulKwitansi.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 93.02755F);
            this.lblJudulKwitansi.Multiline = true;
            this.lblJudulKwitansi.Name = "lblJudulKwitansi";
            this.lblJudulKwitansi.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblJudulKwitansi.SizeF = new System.Drawing.SizeF(1948.792F, 47.83668F);
            this.lblJudulKwitansi.StylePriority.UseFont = false;
            this.lblJudulKwitansi.StylePriority.UseTextAlignment = false;
            this.lblJudulKwitansi.Text = "lblJudulKwitansi";
            this.lblJudulKwitansi.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 154.305F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(1948.792F, 45.19083F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "RECEIPT";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Dpi = 254F;
            this.xrLabel13.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(25.00057F, 394.2292F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(444.5F, 29.31583F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "Amount";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel12.Dpi = 254F;
            this.xrLabel12.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(25.00057F, 362.4792F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(444.5F, 29.31583F);
            this.xrLabel12.StylePriority.UseBorders = false;
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "Uang Sejumlah";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel15
            // 
            this.xrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel15.Dpi = 254F;
            this.xrLabel15.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(27.64742F, 438.9968F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(444.5F, 29.31583F);
            this.xrLabel15.StylePriority.UseBorders = false;
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "Terbilang";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel14
            // 
            this.xrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel14.Dpi = 254F;
            this.xrLabel14.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(469.5006F, 362.4792F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(68.79169F, 61.0658F);
            this.xrLabel14.StylePriority.UseBorders = false;
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = ":";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrLabel9.Dpi = 254F;
            this.xrLabel9.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(25.00033F, 216.9583F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(444.5F, 29.31583F);
            this.xrLabel9.StylePriority.UseBorders = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "Sudah Diterima Dari";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel11.Dpi = 254F;
            this.xrLabel11.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(469.5003F, 216.9583F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(68.79169F, 61.0658F);
            this.xrLabel11.StylePriority.UseBorders = false;
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = ":";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Dpi = 254F;
            this.xrLabel10.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(25.00033F, 246.2742F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(444.5F, 29.31583F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "Received From";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel27
            // 
            this.xrLabel27.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel27.Dpi = 254F;
            this.xrLabel27.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(1330.855F, 216.9583F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(88.49878F, 58.42F);
            this.xrLabel27.StylePriority.UseBorders = false;
            this.xrLabel27.StylePriority.UseFont = false;
            this.xrLabel27.StylePriority.UseTextAlignment = false;
            this.xrLabel27.Text = "No.";
            this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(1419.354F, 216.9583F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(68.79169F, 58.42F);
            this.xrLabel5.StylePriority.UseBorders = false;
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = ":";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.Dpi = 254F;
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 898.8187F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // BNewPaymentReceipt6Dt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.GroupHeader1});
            this.DataSource = this.bs;
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Italic);
            this.Margins = new System.Drawing.Printing.Margins(64, 64, 0, 64);
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
        private DevExpress.XtraReports.UI.XRLabel xrLabel17;
        private DevExpress.XtraReports.UI.XRLabel xrLabel16;
        private DevExpress.XtraReports.UI.XRLabel lblTTD;
        private DevExpress.XtraReports.UI.XRLabel lblTanggal;
        private DevExpress.XtraReports.UI.XRLabel lblDiagnosa;
        private DevExpress.XtraReports.UI.XRLabel lblAmountInWordEng;
        private DevExpress.XtraReports.UI.XRLabel lblAmount;
        private DevExpress.XtraReports.UI.XRLabel lblReceivedFrom;
        private DevExpress.XtraReports.UI.XRLabel lblAmountInWordInd;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRLabel lblJudulKwitansi;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLabel xrLabel15;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel22;
        private DevExpress.XtraReports.UI.XRLabel xrLabel23;
        private DevExpress.XtraReports.UI.XRLabel xrLabel24;
        private DevExpress.XtraReports.UI.XRLabel xrLabel25;
        private DevExpress.XtraReports.UI.XRLabel xrLabel26;
        private DevExpress.XtraReports.UI.XRLabel xrLabel27;
        private DevExpress.XtraReports.UI.XRLabel lblJudulNoKwitansi;
        private DevExpress.XtraReports.UI.XRLabel lblNoKwitansi;
        private DevExpress.XtraReports.UI.XRLabel lblHealthcareName;
        private DevExpress.XtraReports.UI.XRLabel lblRegistrationNo;
        private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak1;
    }
}
