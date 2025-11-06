namespace QIS.Medinfras.ReportDesktop
{
    partial class BSensusHarian
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
            this.SubPasienSensusSummary = new DevExpress.XtraReports.UI.XRSubreport();
            this.pasienSensusSummary1 = new QIS.Medinfras.ReportDesktop.PasienSensusSummary();
            this.subPatientDeadBefore48 = new DevExpress.XtraReports.UI.XRSubreport();
            this.pasienSensusMeninggalSebelum481 = new QIS.Medinfras.ReportDesktop.PasienSensusMeninggalSebelum48();
            this.subPatientDeadAfter48 = new DevExpress.XtraReports.UI.XRSubreport();
            this.pasienSensusMeninggalSetelah481 = new QIS.Medinfras.ReportDesktop.PasienSensusMeninggalSetelah48();
            this.subPasienCencusClass = new DevExpress.XtraReports.UI.XRSubreport();
            this.pasienSensusPerKelas1 = new QIS.Medinfras.ReportDesktop.PasienSensusPerKelas();
            this.subPasienPindahKeluar = new DevExpress.XtraReports.UI.XRSubreport();
            this.pasienSensusPindahKeluar1 = new QIS.Medinfras.ReportDesktop.PasienSensusPindahKeluar();
            this.subPasienPindahMasuk = new DevExpress.XtraReports.UI.XRSubreport();
            this.pasienSensusPindahMasuk1 = new QIS.Medinfras.ReportDesktop.PasienSensusPindahMasuk();
            this.subPasienSensusMasuk = new DevExpress.XtraReports.UI.XRSubreport();
            this.pasienSensusMasuk1 = new QIS.Medinfras.ReportDesktop.PasienSensusMasuk();
            this.subPasienSensusKeluar = new DevExpress.XtraReports.UI.XRSubreport();
            this.pasienSensusKeluar1 = new QIS.Medinfras.ReportDesktop.PasienSensusKeluar();
            this.lblTanggal = new DevExpress.XtraReports.UI.XRLabel();
            this.lblServiceUnit = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusSummary1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusMeninggalSebelum481)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusMeninggalSetelah481)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusPerKelas1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusPindahKeluar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusPindahMasuk1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusMasuk1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusKeluar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.StylePriority.UseFont = false;
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
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
            this.BottomMargin.HeightF = 64.93741F;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblServiceUnit,
            this.lblTanggal});
            this.ReportHeader.HeightF = 382.7993F;
            this.ReportHeader.Controls.SetChildIndex(this.lblReportSubTitle, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLine1, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblTanggal, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblServiceUnit, 0);
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 87.00002F;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.SubPasienSensusSummary,
            this.subPatientDeadBefore48,
            this.subPatientDeadAfter48,
            this.subPasienCencusClass,
            this.subPasienPindahKeluar,
            this.subPasienPindahMasuk,
            this.subPasienSensusMasuk,
            this.subPasienSensusKeluar});
            this.ReportFooter.Dpi = 254F;
            this.ReportFooter.HeightF = 233.68F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // SubPasienSensusSummary
            // 
            this.SubPasienSensusSummary.Dpi = 254F;
            this.SubPasienSensusSummary.LocationFloat = new DevExpress.Utils.PointFloat(1649.812F, 175.2601F);
            this.SubPasienSensusSummary.Name = "SubPasienSensusSummary";
            this.SubPasienSensusSummary.ReportSource = this.pasienSensusSummary1;
            this.SubPasienSensusSummary.SizeF = new System.Drawing.SizeF(715.5624F, 58.41998F);
            // 
            // subPatientDeadBefore48
            // 
            this.subPatientDeadBefore48.Dpi = 254F;
            this.subPatientDeadBefore48.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 116.8399F);
            this.subPatientDeadBefore48.Name = "subPatientDeadBefore48";
            this.subPatientDeadBefore48.ReportSource = this.pasienSensusMeninggalSebelum481;
            this.subPatientDeadBefore48.SizeF = new System.Drawing.SizeF(906.3333F, 58.42001F);
            // 
            // subPatientDeadAfter48
            // 
            this.subPatientDeadAfter48.Dpi = 254F;
            this.subPatientDeadAfter48.LocationFloat = new DevExpress.Utils.PointFloat(1271.187F, 116.8399F);
            this.subPatientDeadAfter48.Name = "subPatientDeadAfter48";
            this.subPatientDeadAfter48.ReportSource = this.pasienSensusMeninggalSetelah481;
            this.subPatientDeadAfter48.SizeF = new System.Drawing.SizeF(967.6465F, 58.41999F);
            // 
            // subPasienCencusClass
            // 
            this.subPasienCencusClass.Dpi = 254F;
            this.subPasienCencusClass.LocationFloat = new DevExpress.Utils.PointFloat(25.00047F, 175.2599F);
            this.subPasienCencusClass.Name = "subPasienCencusClass";
            this.subPasienCencusClass.ReportSource = this.pasienSensusPerKelas1;
            this.subPasienCencusClass.SizeF = new System.Drawing.SizeF(1626.458F, 58.42003F);
            // 
            // subPasienPindahKeluar
            // 
            this.subPasienPindahKeluar.Dpi = 254F;
            this.subPasienPindahKeluar.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 58.41996F);
            this.subPasienPindahKeluar.Name = "subPasienPindahKeluar";
            this.subPasienPindahKeluar.ReportSource = this.pasienSensusPindahKeluar1;
            this.subPasienPindahKeluar.SizeF = new System.Drawing.SizeF(906.3333F, 58.41999F);
            // 
            // subPasienPindahMasuk
            // 
            this.subPasienPindahMasuk.Dpi = 254F;
            this.subPasienPindahMasuk.LocationFloat = new DevExpress.Utils.PointFloat(1271.187F, 58.41996F);
            this.subPasienPindahMasuk.Name = "subPasienPindahMasuk";
            this.subPasienPindahMasuk.ReportSource = this.pasienSensusPindahMasuk1;
            this.subPasienPindahMasuk.SizeF = new System.Drawing.SizeF(967.6464F, 58.41999F);
            // 
            // subPasienSensusMasuk
            // 
            this.subPasienSensusMasuk.Dpi = 254F;
            this.subPasienSensusMasuk.LocationFloat = new DevExpress.Utils.PointFloat(1271.187F, 0F);
            this.subPasienSensusMasuk.Name = "subPasienSensusMasuk";
            this.subPasienSensusMasuk.ReportSource = this.pasienSensusMasuk1;
            this.subPasienSensusMasuk.SizeF = new System.Drawing.SizeF(967.6465F, 58.42F);
            // 
            // subPasienSensusKeluar
            // 
            this.subPasienSensusKeluar.Dpi = 254F;
            this.subPasienSensusKeluar.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 0F);
            this.subPasienSensusKeluar.Name = "subPasienSensusKeluar";
            this.subPasienSensusKeluar.ReportSource = this.pasienSensusKeluar1;
            this.subPasienSensusKeluar.SizeF = new System.Drawing.SizeF(906.3333F, 58.42F);
            // 
            // lblTanggal
            // 
            this.lblTanggal.Dpi = 254F;
            this.lblTanggal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTanggal.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 337.6084F);
            this.lblTanggal.Name = "lblTanggal";
            this.lblTanggal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lblTanggal.SizeF = new System.Drawing.SizeF(2604.228F, 45.19083F);
            this.lblTanggal.StylePriority.UseFont = false;
            this.lblTanggal.StylePriority.UseTextAlignment = false;
            this.lblTanggal.Text = "lblTanggal";
            this.lblTanggal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblServiceUnit
            // 
            this.lblServiceUnit.Dpi = 254F;
            this.lblServiceUnit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceUnit.LocationFloat = new DevExpress.Utils.PointFloat(24.99993F, 292.4176F);
            this.lblServiceUnit.Name = "lblServiceUnit";
            this.lblServiceUnit.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblServiceUnit.SizeF = new System.Drawing.SizeF(2604.228F, 45.19083F);
            this.lblServiceUnit.StylePriority.UseFont = false;
            this.lblServiceUnit.StylePriority.UseTextAlignment = false;
            this.lblServiceUnit.Text = "lblServiceUnit";
            this.lblServiceUnit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // BSensusHarian
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.ReportFooter});
            this.Margins = new System.Drawing.Printing.Margins(64, 64, 64, 65);
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.ReportFooter, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusSummary1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusMeninggalSebelum481)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusMeninggalSetelah481)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusPerKelas1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusPindahKeluar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusPindahMasuk1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusMasuk1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pasienSensusKeluar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRSubreport subPasienSensusMasuk;
        private PasienSensusMasuk pasienSensusMasuk1;
        private DevExpress.XtraReports.UI.XRSubreport subPasienSensusKeluar;
        private PasienSensusKeluar pasienSensusKeluar1;
        private DevExpress.XtraReports.UI.XRSubreport subPasienPindahMasuk;
        private PasienSensusPindahMasuk pasienSensusPindahMasuk1;
        private DevExpress.XtraReports.UI.XRSubreport subPasienPindahKeluar;
        private PasienSensusPindahKeluar pasienSensusPindahKeluar1;
        private DevExpress.XtraReports.UI.XRSubreport subPasienCencusClass;
        private PasienSensusPerKelas pasienSensusPerKelas1;
        private DevExpress.XtraReports.UI.XRSubreport subPatientDeadAfter48;
        private PasienSensusMeninggalSetelah48 pasienSensusMeninggalSetelah481;
        private DevExpress.XtraReports.UI.XRSubreport subPatientDeadBefore48;
        private PasienSensusMeninggalSebelum48 pasienSensusMeninggalSebelum481;
        private DevExpress.XtraReports.UI.XRSubreport SubPasienSensusSummary;
        private PasienSensusSummary pasienSensusSummary1;
        private DevExpress.XtraReports.UI.XRLabel lblServiceUnit;
        private DevExpress.XtraReports.UI.XRLabel lblTanggal;
    }
}
