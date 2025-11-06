namespace QIS.Medinfras.ReportDesktop
{
    partial class BSensusHarianRSCK
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
            this.subPasienSensusKeluarMeninggal = new DevExpress.XtraReports.UI.XRSubreport();
            this.pasienSensusKeluarMeninggal1 = new QIS.Medinfras.ReportDesktop.PasienSensusKeluarMeninggal_RSCK();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblUserName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPrintDate = new DevExpress.XtraReports.UI.XRLabel();
            this.subPasienPindahKeluar = new DevExpress.XtraReports.UI.XRSubreport();
            this.pasienSensusPindahKeluar1 = new QIS.Medinfras.ReportDesktop.PasienSensusPindahKeluar_RSCK();
            this.subPasienPindahMasuk = new DevExpress.XtraReports.UI.XRSubreport();
            this.pasienSensusPindahMasuk1 = new QIS.Medinfras.ReportDesktop.PasienSensusPindahMasuk_RSCK();
            this.subPasienSensusMasuk = new DevExpress.XtraReports.UI.XRSubreport();
            this.pasienSensusMasuk1 = new QIS.Medinfras.ReportDesktop.PasienSensusMasuk_RSCK();
            this.subPasienSensusKeluar = new DevExpress.XtraReports.UI.XRSubreport();
            this.pasienSensusKeluar1 = new QIS.Medinfras.ReportDesktop.PasienSensusKeluar_RSCK();
            this.subPasienAll = new DevExpress.XtraReports.UI.XRSubreport();
            this.pasienSensusAll1 = new QIS.Medinfras.ReportDesktop.PasienSensusAll_RSCK();
            this.pasienSensusKeluar_RSCK1 = new QIS.Medinfras.ReportDesktop.PasienSensusKeluar_RSCK();
            this.lblServiceUnit = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTanggal = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusKeluarMeninggal1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusPindahKeluar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusPindahMasuk1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusMasuk1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusKeluar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusAll1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusKeluar_RSCK1)).BeginInit();
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
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 65F;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblTanggal,
            this.lblServiceUnit});
            this.ReportHeader.HeightF = 382.3993F;
            this.ReportHeader.Controls.SetChildIndex(this.lblReportSubTitle, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLine1, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblServiceUnit, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblTanggal, 0);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 32F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 87.00002F;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.subPasienSensusKeluarMeninggal,
            this.xrLabel3,
            this.lblUserName,
            this.lblPrintDate,
            this.subPasienPindahKeluar,
            this.subPasienPindahMasuk,
            this.subPasienSensusMasuk,
            this.subPasienSensusKeluar,
            this.subPasienAll});
            this.ReportFooter.Dpi = 254F;
            this.ReportFooter.HeightF = 756.5702F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // subPasienSensusKeluarMeninggal
            // 
            this.subPasienSensusKeluarMeninggal.Dpi = 254F;
            this.subPasienSensusKeluarMeninggal.LocationFloat = new DevExpress.Utils.PointFloat(25.00138F, 351.8958F);
            this.subPasienSensusKeluarMeninggal.Name = "subPasienSensusKeluarMeninggal";
            this.subPasienSensusKeluarMeninggal.ReportSource = this.pasienSensusKeluarMeninggal1;
            this.subPasienSensusKeluarMeninggal.SizeF = new System.Drawing.SizeF(1981F, 58.42F);
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(1518.709F, 722.7505F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(487.2914F, 33.72556F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Kepala Bagian";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblUserName
            // 
            this.lblUserName.Dpi = 254F;
            this.lblUserName.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblUserName.LocationFloat = new DevExpress.Utils.PointFloat(1518.709F, 689.0251F);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 0, 0, 254F);
            this.lblUserName.SizeF = new System.Drawing.SizeF(487.2914F, 33.72556F);
            this.lblUserName.StylePriority.UseFont = false;
            this.lblUserName.StylePriority.UseTextAlignment = false;
            this.lblUserName.Text = "lblUserName";
            this.lblUserName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblPrintDate
            // 
            this.lblPrintDate.Dpi = 254F;
            this.lblPrintDate.Font = new System.Drawing.Font("Tahoma", 8F);
            this.lblPrintDate.LocationFloat = new DevExpress.Utils.PointFloat(1518.709F, 554.2512F);
            this.lblPrintDate.Name = "lblPrintDate";
            this.lblPrintDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 0, 0, 254F);
            this.lblPrintDate.SizeF = new System.Drawing.SizeF(487.2914F, 33.72556F);
            this.lblPrintDate.StylePriority.UseFont = false;
            this.lblPrintDate.StylePriority.UseTextAlignment = false;
            this.lblPrintDate.Text = "lblPrintDate";
            this.lblPrintDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // subPasienPindahKeluar
            // 
            this.subPasienPindahKeluar.Dpi = 254F;
            this.subPasienPindahKeluar.LocationFloat = new DevExpress.Utils.PointFloat(24.99958F, 177.0959F);
            this.subPasienPindahKeluar.Name = "subPasienPindahKeluar";
            this.subPasienPindahKeluar.ReportSource = this.pasienSensusPindahKeluar1;
            this.subPasienPindahKeluar.SizeF = new System.Drawing.SizeF(1981F, 58.41999F);
            // 
            // subPasienPindahMasuk
            // 
            this.subPasienPindahMasuk.Dpi = 254F;
            this.subPasienPindahMasuk.LocationFloat = new DevExpress.Utils.PointFloat(24.99958F, 88.54795F);
            this.subPasienPindahMasuk.Name = "subPasienPindahMasuk";
            this.subPasienPindahMasuk.ReportSource = this.pasienSensusPindahMasuk1;
            this.subPasienPindahMasuk.SizeF = new System.Drawing.SizeF(1981F, 58.41999F);
            // 
            // subPasienSensusMasuk
            // 
            this.subPasienSensusMasuk.Dpi = 254F;
            this.subPasienSensusMasuk.LocationFloat = new DevExpress.Utils.PointFloat(25.00009F, 0F);
            this.subPasienSensusMasuk.Name = "subPasienSensusMasuk";
            this.subPasienSensusMasuk.ReportSource = this.pasienSensusMasuk1;
            this.subPasienSensusMasuk.SizeF = new System.Drawing.SizeF(1981F, 58.42F);
            // 
            // subPasienSensusKeluar
            // 
            this.subPasienSensusKeluar.Dpi = 254F;
            this.subPasienSensusKeluar.LocationFloat = new DevExpress.Utils.PointFloat(24.99958F, 265.6438F);
            this.subPasienSensusKeluar.Name = "subPasienSensusKeluar";
            this.subPasienSensusKeluar.ReportSource = this.pasienSensusKeluar1;
            this.subPasienSensusKeluar.SizeF = new System.Drawing.SizeF(1981F, 58.42F);
            // 
            // subPasienAll
            // 
            this.subPasienAll.Dpi = 254F;
            this.subPasienAll.LocationFloat = new DevExpress.Utils.PointFloat(24.99958F, 433.5667F);
            this.subPasienAll.Name = "subPasienAll";
            this.subPasienAll.ReportSource = this.pasienSensusAll1;
            this.subPasienAll.SizeF = new System.Drawing.SizeF(1981F, 58.42F);
            // 
            // lblServiceUnit
            // 
            this.lblServiceUnit.Dpi = 254F;
            this.lblServiceUnit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceUnit.LocationFloat = new DevExpress.Utils.PointFloat(25.00017F, 292.0175F);
            this.lblServiceUnit.Name = "lblServiceUnit";
            this.lblServiceUnit.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblServiceUnit.SizeF = new System.Drawing.SizeF(1981F, 45.19083F);
            this.lblServiceUnit.StylePriority.UseFont = false;
            this.lblServiceUnit.StylePriority.UseTextAlignment = false;
            this.lblServiceUnit.Text = "lblServiceUnit";
            this.lblServiceUnit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblTanggal
            // 
            this.lblTanggal.Dpi = 254F;
            this.lblTanggal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTanggal.LocationFloat = new DevExpress.Utils.PointFloat(25.00063F, 337.2085F);
            this.lblTanggal.Name = "lblTanggal";
            this.lblTanggal.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTanggal.SizeF = new System.Drawing.SizeF(1981F, 45.19083F);
            this.lblTanggal.StylePriority.UseFont = false;
            this.lblTanggal.StylePriority.UseTextAlignment = false;
            this.lblTanggal.Text = "lblTanggal";
            this.lblTanggal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // BSensusHarianRSCK
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.ReportFooter});
            this.Margins = new System.Drawing.Printing.Margins(64, 64, 32, 65);
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.ReportFooter, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusKeluarMeninggal1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusPindahKeluar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusPindahMasuk1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusMasuk1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusKeluar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusAll1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusKeluar_RSCK1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRSubreport subPasienSensusMasuk;
        private PasienSensusMasuk_RSCK pasienSensusMasuk1;
        private DevExpress.XtraReports.UI.XRSubreport subPasienSensusKeluar;
        private PasienSensusKeluar_RSCK pasienSensusKeluar1;
        private DevExpress.XtraReports.UI.XRSubreport subPasienPindahMasuk;
        private PasienSensusPindahMasuk_RSCK pasienSensusPindahMasuk1;
        private DevExpress.XtraReports.UI.XRSubreport subPasienPindahKeluar;
        private PasienSensusPindahKeluar_RSCK pasienSensusPindahKeluar1;
        private DevExpress.XtraReports.UI.XRSubreport subPasienAll;
        private PasienSensusAll_RSCK pasienSensusAll1;
        private DevExpress.XtraReports.UI.XRLabel lblTanggal;
        private DevExpress.XtraReports.UI.XRLabel lblServiceUnit;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel lblUserName;
        private DevExpress.XtraReports.UI.XRLabel lblPrintDate;
        private PasienSensusKeluar_RSCK pasienSensusKeluar_RSCK1;
        private DevExpress.XtraReports.UI.XRSubreport subPasienSensusKeluarMeninggal;
        private PasienSensusKeluarMeninggal_RSCK pasienSensusKeluarMeninggal1;
    }
}
