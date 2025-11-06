namespace QIS.Medinfras.ReportDesktop
{
    partial class BSuratKeteranganRawatInap
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
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDateNow = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblJamPrint = new DevExpress.XtraReports.UI.XRLabel();
            this.lblNamaKepala = new DevExpress.XtraReports.UI.XRLabel();
            this.lblServiceUnit = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 30F;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblServiceUnit,
            this.lblNamaKepala,
            this.lblJamPrint,
            this.xrLabel4,
            this.lblDateNow,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1});
            this.Detail.HeightF = 1455.42F;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 30F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 34.08335F;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vConsultVisit);
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PatientName")});
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(750.2295F, 669.29F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(1029F, 58.41998F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "VisitDateInString")});
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(468.3125F, 809.625F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(254F, 58.42F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfDischargeDateOptions")});
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(1338.791F, 809.625F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(254F, 58.42F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblDateNow
            // 
            this.lblDateNow.Dpi = 254F;
            this.lblDateNow.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateNow.LocationFloat = new DevExpress.Utils.PointFloat(1338.791F, 1155.101F);
            this.lblDateNow.Name = "lblDateNow";
            this.lblDateNow.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblDateNow.SizeF = new System.Drawing.SizeF(254F, 58.42F);
            this.lblDateNow.StylePriority.UseFont = false;
            this.lblDateNow.StylePriority.UseTextAlignment = false;
            this.lblDateNow.Text = "lblDateNow";
            this.lblDateNow.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(1625.771F, 1155.101F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(153.4584F, 58.41998F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "|||||||||||";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblJamPrint
            // 
            this.lblJamPrint.Dpi = 254F;
            this.lblJamPrint.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblJamPrint.LocationFloat = new DevExpress.Utils.PointFloat(13.22917F, 1397F);
            this.lblJamPrint.Name = "lblJamPrint";
            this.lblJamPrint.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblJamPrint.SizeF = new System.Drawing.SizeF(690.3333F, 58.4198F);
            this.lblJamPrint.StylePriority.UseFont = false;
            this.lblJamPrint.StylePriority.UseTextAlignment = false;
            this.lblJamPrint.Text = "lblJamPrint";
            this.lblJamPrint.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblNamaKepala
            // 
            this.lblNamaKepala.Dpi = 254F;
            this.lblNamaKepala.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNamaKepala.LocationFloat = new DevExpress.Utils.PointFloat(1201.208F, 1397F);
            this.lblNamaKepala.Name = "lblNamaKepala";
            this.lblNamaKepala.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblNamaKepala.SizeF = new System.Drawing.SizeF(578.0215F, 58.41943F);
            this.lblNamaKepala.StylePriority.UseFont = false;
            this.lblNamaKepala.StylePriority.UseTextAlignment = false;
            this.lblNamaKepala.Text = "lblNamaKepala";
            this.lblNamaKepala.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblServiceUnit
            // 
            this.lblServiceUnit.Dpi = 254F;
            this.lblServiceUnit.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceUnit.LocationFloat = new DevExpress.Utils.PointFloat(1235.604F, 912.8125F);
            this.lblServiceUnit.Name = "lblServiceUnit";
            this.lblServiceUnit.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblServiceUnit.SizeF = new System.Drawing.SizeF(543.6249F, 58.41998F);
            this.lblServiceUnit.StylePriority.UseFont = false;
            this.lblServiceUnit.StylePriority.UseTextAlignment = false;
            this.lblServiceUnit.Text = "lblServiceUnit";
            this.lblServiceUnit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // BSuratKeteranganRawatInap
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.DataSource = this.bs;
            this.Margins = new System.Drawing.Printing.Margins(99, 109, 30, 30);
            this.PageHeight = 1570;
            this.PageWidth = 1999;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        protected System.Windows.Forms.BindingSource bs;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel lblDateNow;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel lblJamPrint;
        private DevExpress.XtraReports.UI.XRLabel lblNamaKepala;
        private DevExpress.XtraReports.UI.XRLabel lblServiceUnit;
    }
}
