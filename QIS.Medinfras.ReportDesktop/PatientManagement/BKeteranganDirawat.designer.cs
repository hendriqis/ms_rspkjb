namespace QIS.Medinfras.ReportDesktop
{
    partial class BKeteranganDirawat
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
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDOB = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAddress = new DevExpress.XtraReports.UI.XRLabel();
            this.lblGender = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPrintDate = new DevExpress.XtraReports.UI.XRLabel();
            this.lblParamedic = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDate = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTgl = new DevExpress.XtraReports.UI.XRLabel();
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
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vRegistration);
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.StylePriority.UseBorders = false;
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            // 
            // lblReportSubTitle
            // 
            this.lblReportSubTitle.StylePriority.UseBorders = false;
            this.lblReportSubTitle.StylePriority.UseFont = false;
            this.lblReportSubTitle.StylePriority.UseTextAlignment = false;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.ReportHeader.HeightF = 370.5225F;
            this.ReportHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLine1, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblReportSubTitle, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel1, 0);
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblTgl,
            this.lblDate,
            this.lblParamedic,
            this.lblPrintDate,
            this.xrLabel17,
            this.lblGender,
            this.lblAddress,
            this.lblDOB,
            this.lblName,
            this.xrLabel14,
            this.xrLabel12,
            this.xrLabel11,
            this.xrLabel9,
            this.xrLabel5,
            this.xrLabel3});
            this.Detail.HeightF = 756.92F;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 32F;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 312.1025F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(1981F, 58.41995F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "SURAT KETERANGAN DIRAWAT";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(27.64584F, 75.27084F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(421.3397F, 37.25335F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Nama";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(448.9855F, 75.27092F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(34.39587F, 37.25328F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = ":";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Dpi = 254F;
            this.xrLabel9.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(27.64568F, 140.2292F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(421.3398F, 37.2533F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = " Tanggal Lahir/Umur";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Dpi = 254F;
            this.xrLabel11.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(448.9855F, 140.2292F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(34.39587F, 37.2533F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = ":";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Dpi = 254F;
            this.xrLabel12.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(27.64568F, 198.4375F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(421.3398F, 37.2533F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "Alamat";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel14
            // 
            this.xrLabel14.Dpi = 254F;
            this.xrLabel14.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(448.9855F, 198.4375F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(34.39587F, 37.2533F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = ":";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblName
            // 
            this.lblName.Dpi = 254F;
            this.lblName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.LocationFloat = new DevExpress.Utils.PointFloat(499.2563F, 75.27092F);
            this.lblName.Name = "lblName";
            this.lblName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblName.SizeF = new System.Drawing.SizeF(1509.39F, 37.25327F);
            this.lblName.StylePriority.UseFont = false;
            this.lblName.StylePriority.UseTextAlignment = false;
            this.lblName.Text = "lblName";
            this.lblName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblDOB
            // 
            this.lblDOB.Dpi = 254F;
            this.lblDOB.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDOB.LocationFloat = new DevExpress.Utils.PointFloat(499.2563F, 140.2292F);
            this.lblDOB.Name = "lblDOB";
            this.lblDOB.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblDOB.SizeF = new System.Drawing.SizeF(391.9666F, 37.2533F);
            this.lblDOB.StylePriority.UseFont = false;
            this.lblDOB.StylePriority.UseTextAlignment = false;
            this.lblDOB.Text = "lblDOB";
            this.lblDOB.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblAddress
            // 
            this.lblAddress.Dpi = 254F;
            this.lblAddress.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.LocationFloat = new DevExpress.Utils.PointFloat(499.2567F, 198.4375F);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAddress.SizeF = new System.Drawing.SizeF(1509.39F, 37.25333F);
            this.lblAddress.StylePriority.UseFont = false;
            this.lblAddress.StylePriority.UseTextAlignment = false;
            this.lblAddress.Text = "lblAddress";
            this.lblAddress.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblGender
            // 
            this.lblGender.Dpi = 254F;
            this.lblGender.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGender.LocationFloat = new DevExpress.Utils.PointFloat(891.2228F, 140.2292F);
            this.lblGender.Name = "lblGender";
            this.lblGender.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblGender.SizeF = new System.Drawing.SizeF(1117.424F, 37.2533F);
            this.lblGender.StylePriority.UseFont = false;
            this.lblGender.StylePriority.UseTextAlignment = false;
            this.lblGender.Text = "lblGender";
            this.lblGender.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel17
            // 
            this.xrLabel17.Dpi = 254F;
            this.xrLabel17.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 0F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(1981F, 37.2533F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = "Yang bertanda tangan dibawah ini menerangkan dengan sesungguhnya bahwa : ";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblPrintDate
            // 
            this.lblPrintDate.Dpi = 254F;
            this.lblPrintDate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintDate.LocationFloat = new DevExpress.Utils.PointFloat(1389.715F, 513.2917F);
            this.lblPrintDate.Name = "lblPrintDate";
            this.lblPrintDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPrintDate.SizeF = new System.Drawing.SizeF(544.312F, 37.2533F);
            this.lblPrintDate.StylePriority.UseFont = false;
            this.lblPrintDate.StylePriority.UseTextAlignment = false;
            this.lblPrintDate.Text = "lblPrintDate";
            this.lblPrintDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblParamedic
            // 
            this.lblParamedic.Dpi = 254F;
            this.lblParamedic.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParamedic.LocationFloat = new DevExpress.Utils.PointFloat(1389.715F, 714.375F);
            this.lblParamedic.Name = "lblParamedic";
            this.lblParamedic.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblParamedic.SizeF = new System.Drawing.SizeF(544.312F, 37.2533F);
            this.lblParamedic.StylePriority.UseFont = false;
            this.lblParamedic.StylePriority.UseTextAlignment = false;
            this.lblParamedic.Text = "lblParamedic";
            this.lblParamedic.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblDate
            // 
            this.lblDate.Dpi = 254F;
            this.lblDate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.LocationFloat = new DevExpress.Utils.PointFloat(27.64581F, 330.7292F);
            this.lblDate.Name = "lblDate";
            this.lblDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblDate.SizeF = new System.Drawing.SizeF(1981.001F, 37.2533F);
            this.lblDate.StylePriority.UseFont = false;
            this.lblDate.StylePriority.UseTextAlignment = false;
            this.lblDate.Text = "lblDate";
            this.lblDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblTgl
            // 
            this.lblTgl.Dpi = 254F;
            this.lblTgl.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTgl.LocationFloat = new DevExpress.Utils.PointFloat(27.64587F, 261.9375F);
            this.lblTgl.Name = "lblTgl";
            this.lblTgl.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTgl.SizeF = new System.Drawing.SizeF(1978.355F, 37.2533F);
            this.lblTgl.StylePriority.UseFont = false;
            this.lblTgl.StylePriority.UseTextAlignment = false;
            this.lblTgl.Text = "lblTgl";
            this.lblTgl.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // BKeteranganDirawat
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel lblName;
        private DevExpress.XtraReports.UI.XRLabel lblAddress;
        private DevExpress.XtraReports.UI.XRLabel lblDOB;
        private DevExpress.XtraReports.UI.XRLabel lblGender;
        private DevExpress.XtraReports.UI.XRLabel xrLabel17;
        private DevExpress.XtraReports.UI.XRLabel lblPrintDate;
        private DevExpress.XtraReports.UI.XRLabel lblParamedic;
        private DevExpress.XtraReports.UI.XRLabel lblDate;
        private DevExpress.XtraReports.UI.XRLabel lblTgl;
    }
}
