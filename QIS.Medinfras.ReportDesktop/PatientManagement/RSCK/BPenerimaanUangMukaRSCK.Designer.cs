namespace QIS.Medinfras.ReportDesktop
{
    partial class BPenerimaanUangMukaRSCK
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
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.lblRegistrationNo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPaymentDate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPatientName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPaymentNo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalAmount = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalAmountString = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPatientName2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblLastUpdatedDate = new DevExpress.XtraReports.UI.XRLabel();
            this.lblLastUpdatedBy = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblUnitPelayanan = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblParamedicName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
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
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vPatientPaymentHd);
            // 
            // PageHeader
            // 
            this.PageHeader.HeightF = 418.4417F;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel13,
            this.lblPatientName2,
            this.xrLabel10,
            this.xrLabel11,
            this.xrLabel20,
            this.xrLabel26,
            this.lblTotalAmount,
            this.lblTotalAmountString,
            this.lblLastUpdatedDate,
            this.xrLabel9,
            this.xrLabel12,
            this.lblParamedicName,
            this.lblUnitPelayanan,
            this.lblLastUpdatedBy,
            this.xrLabel3,
            this.xrLabel6,
            this.lblPaymentDate,
            this.xrLabel2,
            this.lblPatientName,
            this.xrLabel18,
            this.lblRegistrationNo,
            this.xrLabel21,
            this.xrLabel1,
            this.xrLabel4,
            this.xrLabel17,
            this.xrLabel14,
            this.xrLabel15,
            this.xrLabel8,
            this.xrLabel5,
            this.lblPaymentNo,
            this.xrLabel7});
            this.Detail.HeightF = 604.4374F;
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("TempParentID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("PrescriptionOrderDetailID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            // 
            // ReportFooter
            // 
            this.ReportFooter.Dpi = 254F;
            this.ReportFooter.HeightF = 0F;
            this.ReportFooter.KeepTogether = true;
            this.ReportFooter.Name = "ReportFooter";
            this.ReportFooter.PrintAtBottom = true;
            // 
            // lblRegistrationNo
            // 
            this.lblRegistrationNo.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.lblRegistrationNo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblRegistrationNo.Dpi = 254F;
            this.lblRegistrationNo.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegistrationNo.LocationFloat = new DevExpress.Utils.PointFloat(437.7498F, 63.5F);
            this.lblRegistrationNo.Name = "lblRegistrationNo";
            this.lblRegistrationNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblRegistrationNo.SizeF = new System.Drawing.SizeF(890.25F, 31.75F);
            this.lblRegistrationNo.StylePriority.UseBorderDashStyle = false;
            this.lblRegistrationNo.StylePriority.UseBorders = false;
            this.lblRegistrationNo.StylePriority.UseFont = false;
            this.lblRegistrationNo.Text = "[RegistrationNo]";
            // 
            // xrLabel21
            // 
            this.xrLabel21.Dpi = 254F;
            this.xrLabel21.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(24.99985F, 0F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(381F, 31.75F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.Text = "No. Pembayaran";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(405.9998F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = ":";
            // 
            // xrLabel18
            // 
            this.xrLabel18.Dpi = 254F;
            this.xrLabel18.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(405.9998F, 342.5F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.Text = ":";
            // 
            // lblPaymentDate
            // 
            this.lblPaymentDate.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.lblPaymentDate.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPaymentDate.Dpi = 254F;
            this.lblPaymentDate.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentDate.LocationFloat = new DevExpress.Utils.PointFloat(437.7498F, 31.75F);
            this.lblPaymentDate.Name = "lblPaymentDate";
            this.lblPaymentDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPaymentDate.SizeF = new System.Drawing.SizeF(890.25F, 31.75F);
            this.lblPaymentDate.StylePriority.UseBorderDashStyle = false;
            this.lblPaymentDate.StylePriority.UseBorders = false;
            this.lblPaymentDate.StylePriority.UseFont = false;
            this.lblPaymentDate.Text = "[PaymentDateInString]";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(405.9998F, 63.5F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = ":";
            // 
            // lblPatientName
            // 
            this.lblPatientName.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.lblPatientName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPatientName.Dpi = 254F;
            this.lblPatientName.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientName.LocationFloat = new DevExpress.Utils.PointFloat(437.7498F, 95.25F);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPatientName.SizeF = new System.Drawing.SizeF(890.25F, 31.75F);
            this.lblPatientName.StylePriority.UseBorderDashStyle = false;
            this.lblPatientName.StylePriority.UseBorders = false;
            this.lblPatientName.StylePriority.UseFont = false;
            this.lblPatientName.Text = "[PatientName]";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(24.99985F, 63.5F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(381F, 31.75F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "No. Registrasi";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(405.9998F, 95.25F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = ":";
            // 
            // lblPaymentNo
            // 
            this.lblPaymentNo.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.lblPaymentNo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPaymentNo.Dpi = 254F;
            this.lblPaymentNo.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPaymentNo.LocationFloat = new DevExpress.Utils.PointFloat(437.7498F, 0F);
            this.lblPaymentNo.Name = "lblPaymentNo";
            this.lblPaymentNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPaymentNo.SizeF = new System.Drawing.SizeF(890.25F, 31.75F);
            this.lblPaymentNo.StylePriority.UseBorderDashStyle = false;
            this.lblPaymentNo.StylePriority.UseBorders = false;
            this.lblPaymentNo.StylePriority.UseFont = false;
            this.lblPaymentNo.Text = "[PaymentNo]";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Dpi = 254F;
            this.xrLabel7.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(24.99985F, 95.25F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(381F, 31.75F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "Nama Pasien";
            // 
            // xrLabel8
            // 
            this.xrLabel8.Dpi = 254F;
            this.xrLabel8.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(405.9998F, 31.75F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = ":";
            // 
            // xrLabel17
            // 
            this.xrLabel17.Dpi = 254F;
            this.xrLabel17.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(24.99985F, 279F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(381F, 31.75F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.Text = "Uang Sejumlah";
            // 
            // xrLabel14
            // 
            this.xrLabel14.Dpi = 254F;
            this.xrLabel14.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(24.99985F, 247.25F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(381F, 31.75F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.Text = "Sudah Diterima Dari";
            // 
            // xrLabel15
            // 
            this.xrLabel15.Dpi = 254F;
            this.xrLabel15.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(405.9998F, 279F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.Text = ":";
            // 
            // xrLabel26
            // 
            this.xrLabel26.Dpi = 254F;
            this.xrLabel26.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(56.74977F, 400.7083F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(381.0001F, 44.97919F);
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.StylePriority.UseTextAlignment = false;
            this.xrLabel26.Text = "Penyetor";
            this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.lblTotalAmount.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblTotalAmount.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TotalPaymentAmount", "{0:#,##0.00}")});
            this.lblTotalAmount.Dpi = 254F;
            this.lblTotalAmount.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmount.LocationFloat = new DevExpress.Utils.PointFloat(437.7498F, 279.0001F);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTotalAmount.SizeF = new System.Drawing.SizeF(890.25F, 31.75F);
            this.lblTotalAmount.StylePriority.UseBorderDashStyle = false;
            this.lblTotalAmount.StylePriority.UseBorders = false;
            this.lblTotalAmount.StylePriority.UseFont = false;
            this.lblTotalAmount.Text = "lblTotalAmount";
            // 
            // lblTotalAmountString
            // 
            this.lblTotalAmountString.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.lblTotalAmountString.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblTotalAmountString.Dpi = 254F;
            this.lblTotalAmountString.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalAmountString.LocationFloat = new DevExpress.Utils.PointFloat(437.7498F, 342.5001F);
            this.lblTotalAmountString.Name = "lblTotalAmountString";
            this.lblTotalAmountString.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTotalAmountString.SizeF = new System.Drawing.SizeF(890.25F, 31.75F);
            this.lblTotalAmountString.StylePriority.UseBorderDashStyle = false;
            this.lblTotalAmountString.StylePriority.UseBorders = false;
            this.lblTotalAmountString.StylePriority.UseFont = false;
            this.lblTotalAmountString.Text = "lblTotalAmountString";
            // 
            // xrLabel20
            // 
            this.xrLabel20.Dpi = 254F;
            this.xrLabel20.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(24.99985F, 342.5F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(381F, 31.75F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.Text = "Terbilang ";
            // 
            // lblPatientName2
            // 
            this.lblPatientName2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.lblPatientName2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPatientName2.Dpi = 254F;
            this.lblPatientName2.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientName2.LocationFloat = new DevExpress.Utils.PointFloat(437.7498F, 247.25F);
            this.lblPatientName2.Name = "lblPatientName2";
            this.lblPatientName2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPatientName2.SizeF = new System.Drawing.SizeF(890.25F, 31.74998F);
            this.lblPatientName2.StylePriority.UseBorderDashStyle = false;
            this.lblPatientName2.StylePriority.UseBorders = false;
            this.lblPatientName2.StylePriority.UseFont = false;
            this.lblPatientName2.Text = "[PatientName]";
            // 
            // xrLabel10
            // 
            this.xrLabel10.Dpi = 254F;
            this.xrLabel10.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(24.99985F, 31.75F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(381F, 31.75F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.Text = "Tanggal Pembayaran";
            // 
            // xrLabel11
            // 
            this.xrLabel11.Dpi = 254F;
            this.xrLabel11.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(405.9998F, 247.25F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.Text = ":";
            // 
            // lblLastUpdatedDate
            // 
            this.lblLastUpdatedDate.Dpi = 254F;
            this.lblLastUpdatedDate.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastUpdatedDate.LocationFloat = new DevExpress.Utils.PointFloat(795.4623F, 498.6042F);
            this.lblLastUpdatedDate.Name = "lblLastUpdatedDate";
            this.lblLastUpdatedDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblLastUpdatedDate.SizeF = new System.Drawing.SizeF(532.5376F, 31.75F);
            this.lblLastUpdatedDate.StylePriority.UseFont = false;
            this.lblLastUpdatedDate.StylePriority.UseTextAlignment = false;
            this.lblLastUpdatedDate.Text = "lblLastUpdatedDate";
            this.lblLastUpdatedDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblLastUpdatedBy
            // 
            this.lblLastUpdatedBy.Dpi = 254F;
            this.lblLastUpdatedBy.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastUpdatedBy.LocationFloat = new DevExpress.Utils.PointFloat(795.4623F, 530.3542F);
            this.lblLastUpdatedBy.Name = "lblLastUpdatedBy";
            this.lblLastUpdatedBy.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblLastUpdatedBy.SizeF = new System.Drawing.SizeF(532.5377F, 31.75F);
            this.lblLastUpdatedBy.StylePriority.UseFont = false;
            this.lblLastUpdatedBy.StylePriority.UseTextAlignment = false;
            this.lblLastUpdatedBy.Text = "lblLastUpdatedBy";
            this.lblLastUpdatedBy.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(405.9998F, 127F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = ":";
            // 
            // xrLabel6
            // 
            this.xrLabel6.Dpi = 254F;
            this.xrLabel6.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(24.99985F, 127F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(381F, 31.75F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "Unit Pelayanan";
            // 
            // lblUnitPelayanan
            // 
            this.lblUnitPelayanan.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.lblUnitPelayanan.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblUnitPelayanan.Dpi = 254F;
            this.lblUnitPelayanan.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnitPelayanan.LocationFloat = new DevExpress.Utils.PointFloat(437.7498F, 127F);
            this.lblUnitPelayanan.Name = "lblUnitPelayanan";
            this.lblUnitPelayanan.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblUnitPelayanan.SizeF = new System.Drawing.SizeF(890.25F, 31.74999F);
            this.lblUnitPelayanan.StylePriority.UseBorderDashStyle = false;
            this.lblUnitPelayanan.StylePriority.UseBorders = false;
            this.lblUnitPelayanan.StylePriority.UseFont = false;
            this.lblUnitPelayanan.Text = "Unit Pelayanan";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Dpi = 254F;
            this.xrLabel9.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(24.99985F, 158.75F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(381F, 31.75F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = "Nama Dokter";
            // 
            // xrLabel12
            // 
            this.xrLabel12.Dpi = 254F;
            this.xrLabel12.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(405.9998F, 158.75F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(31.75F, 31.75F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.Text = ":";
            // 
            // lblParamedicName
            // 
            this.lblParamedicName.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.lblParamedicName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblParamedicName.Dpi = 254F;
            this.lblParamedicName.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParamedicName.LocationFloat = new DevExpress.Utils.PointFloat(437.7498F, 158.75F);
            this.lblParamedicName.Name = "lblParamedicName";
            this.lblParamedicName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblParamedicName.SizeF = new System.Drawing.SizeF(890.25F, 31.75F);
            this.lblParamedicName.StylePriority.UseBorderDashStyle = false;
            this.lblParamedicName.StylePriority.UseBorders = false;
            this.lblParamedicName.StylePriority.UseFont = false;
            this.lblParamedicName.Text = "lblParamedicName";
            // 
            // xrLabel13
            // 
            this.xrLabel13.Dpi = 254F;
            this.xrLabel13.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(24.99985F, 543.5834F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(449.7917F, 46.43738F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "(..............)";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // BPenerimaanUangMukaRSCK
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.ReportFooter,
            this.PageHeader});
            this.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Double;
            this.Margins = new System.Drawing.Printing.Margins(65, 57, 64, 64);
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.ReportFooter, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRLabel lblPatientName2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel20;
        private DevExpress.XtraReports.UI.XRLabel xrLabel26;
        private DevExpress.XtraReports.UI.XRLabel lblTotalAmount;
        private DevExpress.XtraReports.UI.XRLabel lblTotalAmountString;
        private DevExpress.XtraReports.UI.XRLabel lblLastUpdatedDate;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLabel lblParamedicName;
        private DevExpress.XtraReports.UI.XRLabel lblUnitPelayanan;
        private DevExpress.XtraReports.UI.XRLabel lblLastUpdatedBy;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel lblPaymentDate;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel lblPatientName;
        private DevExpress.XtraReports.UI.XRLabel xrLabel18;
        private DevExpress.XtraReports.UI.XRLabel lblRegistrationNo;
        private DevExpress.XtraReports.UI.XRLabel xrLabel21;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel17;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
        private DevExpress.XtraReports.UI.XRLabel xrLabel15;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel lblPaymentNo;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
    }
}
