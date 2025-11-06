namespace QIS.Medinfras.ReportDesktop
{
    partial class BLabelPasienLabRSSBB
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
            this.bLabelPasienLabRSSBBSection11 = new QIS.Medinfras.ReportDesktop.BLabelPasienLabRSSBBSection1();
            this.subSection2 = new DevExpress.XtraReports.UI.XRSubreport();
            this.subSection3 = new DevExpress.XtraReports.UI.XRSubreport();
            this.subSection4 = new DevExpress.XtraReports.UI.XRSubreport();
            this.bLabelPasienLabRSSBBSection21 = new QIS.Medinfras.ReportDesktop.BLabelPasienLabRSSBBSection2();
            this.bLabelPasienLabRSSBBSection31 = new QIS.Medinfras.ReportDesktop.BLabelPasienLabRSSBBSection3();
            this.bLabelPasienLabRSSBBSection41 = new QIS.Medinfras.ReportDesktop.BLabelPasienLabRSSBBSection4();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienLabRSSBBSection11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienLabRSSBBSection21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienLabRSSBBSection31)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienLabRSSBBSection41)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 30F;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.subSection4,
            this.subSection3,
            this.subSection2,
            this.subSection1});
            this.Detail.HeightF = 344.0641F;
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
            this.subSection1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.subSection1.Name = "subSection1";
            this.subSection1.ReportSource = this.bLabelPasienLabRSSBBSection11;
            this.subSection1.SizeF = new System.Drawing.SizeF(2042F, 58.42F);
            // 
            // subSection2
            // 
            this.subSection2.Dpi = 254F;
            this.subSection2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 92.81588F);
            this.subSection2.Name = "subSection2";
            this.subSection2.ReportSource = this.bLabelPasienLabRSSBBSection21;
            this.subSection2.SizeF = new System.Drawing.SizeF(2042F, 58.41999F);
            // 
            // subSection3
            // 
            this.subSection3.Dpi = 254F;
            this.subSection3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 187.7483F);
            this.subSection3.Name = "subSection3";
            this.subSection3.ReportSource = this.bLabelPasienLabRSSBBSection31;
            this.subSection3.SizeF = new System.Drawing.SizeF(2042F, 58.41998F);
            // 
            // subSection4
            // 
            this.subSection4.Dpi = 254F;
            this.subSection4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 285.6441F);
            this.subSection4.Name = "subSection4";
            this.subSection4.ReportSource = this.bLabelPasienLabRSSBBSection41;
            this.subSection4.SizeF = new System.Drawing.SizeF(2042F, 58.42001F);
            // 
            // BLabelPasienLabRSSBB
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.DataSource = this.bs;
            this.Margins = new System.Drawing.Printing.Margins(99, 109, 30, 30);
            this.PageHeight = 4000;
            this.PageWidth = 2250;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienLabRSSBBSection11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienLabRSSBBSection21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienLabRSSBBSection31)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienLabRSSBBSection41)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        protected System.Windows.Forms.BindingSource bs;
        private DevExpress.XtraReports.UI.XRSubreport subSection1;
        private DevExpress.XtraReports.UI.XRSubreport subSection2;
        private DevExpress.XtraReports.UI.XRSubreport subSection3;
        private DevExpress.XtraReports.UI.XRSubreport subSection4;
        private BLabelPasienLabRSSBBSection1 bLabelPasienLabRSSBBSection11;
        private BLabelPasienLabRSSBBSection2 bLabelPasienLabRSSBBSection21;
        private BLabelPasienLabRSSBBSection3 bLabelPasienLabRSSBBSection31;
        private BLabelPasienLabRSSBBSection4 bLabelPasienLabRSSBBSection41;
    }
}
