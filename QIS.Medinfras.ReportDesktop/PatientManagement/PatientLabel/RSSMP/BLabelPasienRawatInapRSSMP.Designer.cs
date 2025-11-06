namespace QIS.Medinfras.ReportDesktop
{
    partial class BLabelPasienRawatInapRSSMP
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
            this.subSection1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.bLabelPasienRawatInapRSSMPSection1 = new QIS.Medinfras.ReportDesktop.BLabelPasienRawatInapRSSMPSection1();
            this.subSection2 = new DevExpress.XtraReports.UI.XRSubreport();
            this.bLabelPasienRawatInapRSSMPSection2 = new QIS.Medinfras.ReportDesktop.BLabelPasienRawatInapRSSMPSection2();
            this.subSection3 = new DevExpress.XtraReports.UI.XRSubreport();
            this.bLabelPasienRawatInapRSSMPSection3 = new QIS.Medinfras.ReportDesktop.BLabelPasienRawatInapRSSMPSection3();
            this.subSection4 = new DevExpress.XtraReports.UI.XRSubreport();
            this.bLabelPasienRawatInapRSSMPSection4 = new QIS.Medinfras.ReportDesktop.BLabelPasienRawatInapRSSMPSection4();
            this.subSection5 = new DevExpress.XtraReports.UI.XRSubreport();
            this.bLabelPasienRawatInapRSSMPSection5 = new QIS.Medinfras.ReportDesktop.BLabelPasienRawatInapRSSMPSection5();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRawatInapRSSMPSection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRawatInapRSSMPSection2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRawatInapRSSMPSection3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRawatInapRSSMPSection4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRawatInapRSSMPSection5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 30F;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.subSection5,
            this.subSection4,
            this.subSection3,
            this.subSection2,
            this.subSection1});
            this.Detail.HeightF = 788.9407F;
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
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vLabelPatientRegistration);
            // 
            // subSection1
            // 
            this.subSection1.Dpi = 254F;
            this.subSection1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 56.75001F);
            this.subSection1.Name = "subSection1";
            this.subSection1.ReportSource = this.bLabelPasienRawatInapRSSMPSection1;
            this.subSection1.SizeF = new System.Drawing.SizeF(2042F, 58.42F);
            // 
            // subSection2
            // 
            this.subSection2.Dpi = 254F;
            this.subSection2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 223.1319F);
            this.subSection2.Name = "subSection2";
            this.subSection2.ReportSource = this.bLabelPasienRawatInapRSSMPSection2;
            this.subSection2.SizeF = new System.Drawing.SizeF(2042F, 58.42F);
            // 
            // subSection3
            // 
            this.subSection3.Dpi = 254F;
            this.subSection3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 386.1859F);
            this.subSection3.Name = "subSection3";
            this.subSection3.ReportSource = this.bLabelPasienRawatInapRSSMPSection3;
            this.subSection3.SizeF = new System.Drawing.SizeF(2043F, 58.41995F);
            // 
            // subSection4
            // 
            this.subSection4.Dpi = 254F;
            this.subSection4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 549.3455F);
            this.subSection4.Name = "subSection4";
            this.subSection4.ReportSource = this.bLabelPasienRawatInapRSSMPSection4;
            this.subSection4.SizeF = new System.Drawing.SizeF(2042F, 58.41998F);
            // 
            // subSection5
            // 
            this.subSection5.Dpi = 254F;
            this.subSection5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 707.2137F);
            this.subSection5.Name = "subSection5";
            this.subSection5.ReportSource = this.bLabelPasienRawatInapRSSMPSection5;
            this.subSection5.SizeF = new System.Drawing.SizeF(2043F, 58.41992F);
            // 
            // BLabelPasienRSSMP
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.DataSource = this.bs;
            this.Margins = new System.Drawing.Printing.Margins(98, 108, 30, 30);
            this.PageHeight = 4000;
            this.PageWidth = 2249;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRawatInapRSSMPSection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRawatInapRSSMPSection2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRawatInapRSSMPSection3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRawatInapRSSMPSection4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRawatInapRSSMPSection5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        protected System.Windows.Forms.BindingSource bs;
        private DevExpress.XtraReports.UI.XRSubreport subSection1;
        private BLabelPasienRawatInapRSSMPSection1 bLabelPasienRawatInapRSSMPSection1;
        private DevExpress.XtraReports.UI.XRSubreport subSection2;
        private BLabelPasienRawatInapRSSMPSection2 bLabelPasienRawatInapRSSMPSection2;
        private DevExpress.XtraReports.UI.XRSubreport subSection3;
        private BLabelPasienRawatInapRSSMPSection3 bLabelPasienRawatInapRSSMPSection3;
        private DevExpress.XtraReports.UI.XRSubreport subSection4;
        private BLabelPasienRawatInapRSSMPSection4 bLabelPasienRawatInapRSSMPSection4;
        private DevExpress.XtraReports.UI.XRSubreport subSection5;
        private BLabelPasienRawatInapRSSMPSection5 bLabelPasienRawatInapRSSMPSection5;
    }
}
