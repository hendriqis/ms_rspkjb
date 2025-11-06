namespace QIS.Medinfras.ReportDesktop
{
    partial class BKwitansiPiutangRanapRSDOSOBA
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
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAmountInWordInd = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReceivedFrom = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAmount = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTanggal = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblReceiptNo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPrintDate = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTTD1 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.Font = new System.Drawing.Font("Tahoma", 5F);
            this.xrPageInfo.LocationFloat = new DevExpress.Utils.PointFloat(807.6099F, 31.75F);
            this.xrPageInfo.SizeF = new System.Drawing.SizeF(118.639F, 31.75F);
            this.xrPageInfo.StylePriority.UseFont = false;
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
            // 
            // lineFooter
            // 
            this.lineFooter.SizeF = new System.Drawing.SizeF(899.9999F, 31.74993F);
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.Font = new System.Drawing.Font("Tahoma", 5F);
            this.lblReportProperties.SizeF = new System.Drawing.SizeF(769.472F, 31.75F);
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vARInvoiceDt3);
            // 
            // xrLine1
            // 
            this.xrLine1.SizeF = new System.Drawing.SizeF(875.5601F, 39.47583F);
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
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 32F;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Visible = false;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblTTD1,
            this.xrLabel7,
            this.xrLabel10,
            this.xrLabel13,
            this.xrLabel17,
            this.lblReceiptNo,
            this.xrLabel19,
            this.xrLabel20,
            this.lblPrintDate,
            this.xrLabel16,
            this.xrLabel8,
            this.xrLabel18,
            this.xrLabel15,
            this.xrLabel12,
            this.xrLabel9,
            this.xrLabel14,
            this.lblAmount,
            this.lblTanggal,
            this.xrLabel11,
            this.lblAmountInWordInd,
            this.lblReceivedFrom});
            this.Detail.HeightF = 1048.19F;
            this.Detail.KeepTogether = true;
            this.Detail.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("SequenceNo", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("PaymentDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("PaymentID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 32F;
            // 
            // PageHeader
            // 
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 0F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel9.Dpi = 254F;
            this.xrLabel9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(107.5972F, 363.3611F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(414.5139F, 61.0658F);
            this.xrLabel9.StylePriority.UseBorders = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "Telah Terima Dari";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel14
            // 
            this.xrLabel14.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel14.Dpi = 254F;
            this.xrLabel14.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(523.8746F, 607.6242F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(29.98611F, 61.0658F);
            this.xrLabel14.StylePriority.UseBorders = false;
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = ":";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel15
            // 
            this.xrLabel15.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel15.Dpi = 254F;
            this.xrLabel15.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(107.5964F, 668.69F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(416.2783F, 61.06586F);
            this.xrLabel15.StylePriority.UseBorders = false;
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "Terbilang";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel12.Dpi = 254F;
            this.xrLabel12.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(107.5972F, 607.6242F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(416.2773F, 61.0658F);
            this.xrLabel12.StylePriority.UseBorders = false;
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "Uang Sejumlah";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblAmountInWordInd
            // 
            this.lblAmountInWordInd.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfClaimedAmountInStringInd")});
            this.lblAmountInWordInd.Dpi = 254F;
            this.lblAmountInWordInd.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.lblAmountInWordInd.LocationFloat = new DevExpress.Utils.PointFloat(553.8622F, 668.69F);
            this.lblAmountInWordInd.Name = "lblAmountInWordInd";
            this.lblAmountInWordInd.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAmountInWordInd.SizeF = new System.Drawing.SizeF(1105.958F, 58.41992F);
            this.lblAmountInWordInd.StylePriority.UseFont = false;
            this.lblAmountInWordInd.StylePriority.UseTextAlignment = false;
            this.lblAmountInWordInd.Text = "lblAmountInWordInd";
            this.lblAmountInWordInd.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblReceivedFrom
            // 
            this.lblReceivedFrom.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "CustomerBillToName")});
            this.lblReceivedFrom.Dpi = 254F;
            this.lblReceivedFrom.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceivedFrom.LocationFloat = new DevExpress.Utils.PointFloat(552.0972F, 363.3612F);
            this.lblReceivedFrom.Name = "lblReceivedFrom";
            this.lblReceivedFrom.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReceivedFrom.SizeF = new System.Drawing.SizeF(1107.723F, 61.0658F);
            this.lblReceivedFrom.StylePriority.UseFont = false;
            this.lblReceivedFrom.StylePriority.UseTextAlignment = false;
            this.lblReceivedFrom.Text = "lblReceivedFrom";
            this.lblReceivedFrom.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblAmount
            // 
            this.lblAmount.Dpi = 254F;
            this.lblAmount.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmount.LocationFloat = new DevExpress.Utils.PointFloat(553.8624F, 607.6242F);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAmount.SizeF = new System.Drawing.SizeF(1105.958F, 61.06573F);
            this.lblAmount.StylePriority.UseFont = false;
            this.lblAmount.StylePriority.UseTextAlignment = false;
            this.lblAmount.Text = "Rp [cfClaimedAmountInString]";
            this.lblAmount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblTanggal
            // 
            this.lblTanggal.Dpi = 254F;
            this.lblTanggal.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTanggal.LocationFloat = new DevExpress.Utils.PointFloat(1022.174F, 767.2917F);
            this.lblTanggal.Name = "lblTanggal";
            this.lblTanggal.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTanggal.SizeF = new System.Drawing.SizeF(637.6461F, 37.25336F);
            this.lblTanggal.StylePriority.UseFont = false;
            this.lblTanggal.StylePriority.UseTextAlignment = false;
            this.lblTanggal.Text = "lblTanggal";
            this.lblTanggal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel11.Dpi = 254F;
            this.xrLabel11.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(522.1111F, 363.3611F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(29.98618F, 61.0658F);
            this.xrLabel11.StylePriority.UseBorders = false;
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = ":";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel17
            // 
            this.xrLabel17.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel17.Dpi = 254F;
            this.xrLabel17.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(523.8746F, 668.69F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(29.98618F, 61.06586F);
            this.xrLabel17.StylePriority.UseBorders = false;
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = ":";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblReceiptNo
            // 
            this.lblReceiptNo.Dpi = 254F;
            this.lblReceiptNo.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReceiptNo.LocationFloat = new DevExpress.Utils.PointFloat(1157.111F, 304.9412F);
            this.lblReceiptNo.Name = "lblReceiptNo";
            this.lblReceiptNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblReceiptNo.SizeF = new System.Drawing.SizeF(502.7091F, 58.41994F);
            this.lblReceiptNo.StylePriority.UseFont = false;
            this.lblReceiptNo.StylePriority.UseTextAlignment = false;
            this.lblReceiptNo.Text = "[cfARInvoiceDateInString]";
            this.lblReceiptNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel7.Dpi = 254F;
            this.xrLabel7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(109.3603F, 424.4269F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(444.5005F, 61.06583F);
            this.xrLabel7.StylePriority.UseBorders = false;
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "UNTUK BIAYA PERAWATAN";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel10.Dpi = 254F;
            this.xrLabel10.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(523.8746F, 546.5585F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(29.98611F, 61.06577F);
            this.xrLabel10.StylePriority.UseBorders = false;
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = ":";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel13.Dpi = 254F;
            this.xrLabel13.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(109.3603F, 546.5585F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(414.5139F, 61.06577F);
            this.xrLabel13.StylePriority.UseBorders = false;
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "Keterangan";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel16
            // 
            this.xrLabel16.Dpi = 254F;
            this.xrLabel16.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(553.8633F, 546.5585F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(1105.957F, 61.0658F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "[RegistrationNo] | [ServiceUnitName] [cfRegistrationDateInString] s/d [cfPaymentD" +
                "ateInString]";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Dpi = 254F;
            this.xrLabel8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(553.8611F, 424.4269F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(1105.958F, 61.06577F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "([MedicalNo]) [PatientName]";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel18
            // 
            this.xrLabel18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PatientStreetName")});
            this.xrLabel18.Dpi = 254F;
            this.xrLabel18.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(553.8624F, 485.4927F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(1105.958F, 61.0658F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "xrLabel18";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel19
            // 
            this.xrLabel19.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel19.Dpi = 254F;
            this.xrLabel19.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(523.8746F, 485.4927F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(29.98611F, 61.06577F);
            this.xrLabel19.StylePriority.UseBorders = false;
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.Text = ":";
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel20
            // 
            this.xrLabel20.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel20.Dpi = 254F;
            this.xrLabel20.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(109.3603F, 485.4927F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(414.5139F, 61.06577F);
            this.xrLabel20.StylePriority.UseBorders = false;
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "Alamat";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblPrintDate
            // 
            this.lblPrintDate.Dpi = 254F;
            this.lblPrintDate.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintDate.LocationFloat = new DevExpress.Utils.PointFloat(101.424F, 989.5417F);
            this.lblPrintDate.Name = "lblPrintDate";
            this.lblPrintDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPrintDate.SizeF = new System.Drawing.SizeF(444.5006F, 37.25342F);
            this.lblPrintDate.StylePriority.UseFont = false;
            this.lblPrintDate.StylePriority.UseTextAlignment = false;
            this.lblPrintDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblTTD1
            // 
            this.lblTTD1.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.lblTTD1.Dpi = 254F;
            this.lblTTD1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTTD1.LocationFloat = new DevExpress.Utils.PointFloat(1022.174F, 975.4307F);
            this.lblTTD1.Name = "lblTTD1";
            this.lblTTD1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTTD1.SizeF = new System.Drawing.SizeF(637.6463F, 29.3158F);
            this.lblTTD1.StylePriority.UseBorders = false;
            this.lblTTD1.StylePriority.UseFont = false;
            this.lblTTD1.StylePriority.UseTextAlignment = false;
            this.lblTTD1.Text = "lblTTD1";
            this.lblTTD1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // BKwitansiPiutangRanapRSDOSOBA
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.PageHeader});
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 32, 32);
            this.PageHeight = 1853;
            this.PageWidth = 1750;
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
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRLabel xrLabel17;
        private DevExpress.XtraReports.UI.XRLabel lblReceiptNo;
        private DevExpress.XtraReports.UI.XRLabel xrLabel19;
        private DevExpress.XtraReports.UI.XRLabel xrLabel20;
        private DevExpress.XtraReports.UI.XRLabel lblPrintDate;
        private DevExpress.XtraReports.UI.XRLabel xrLabel16;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel18;
        private DevExpress.XtraReports.UI.XRLabel xrLabel15;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
        private DevExpress.XtraReports.UI.XRLabel lblAmount;
        private DevExpress.XtraReports.UI.XRLabel lblTanggal;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel lblAmountInWordInd;
        private DevExpress.XtraReports.UI.XRLabel lblReceivedFrom;
        private DevExpress.XtraReports.UI.XRLabel lblTTD1;
    }
}
