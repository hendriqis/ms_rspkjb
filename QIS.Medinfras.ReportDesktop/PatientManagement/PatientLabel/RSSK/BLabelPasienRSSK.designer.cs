namespace QIS.Medinfras.ReportDesktop
{
    partial class BLabelPasienRSSK
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
            this.bLabelPasienRSSKSection11 = new QIS.Medinfras.ReportDesktop.BLabelPasienRSSKSection1();
            this.subSection2 = new DevExpress.XtraReports.UI.XRSubreport();
            this.bLabelPasienRSSKSection12 = new QIS.Medinfras.ReportDesktop.BLabelPasienRSSKSection2();
            this.subSection3 = new DevExpress.XtraReports.UI.XRSubreport();
            this.bLabelPasienRSSKSection13 = new QIS.Medinfras.ReportDesktop.BLabelPasienRSSKSection3();
            this.subSection4 = new DevExpress.XtraReports.UI.XRSubreport();
            this.bLabelPasienRSSKSection14 = new QIS.Medinfras.ReportDesktop.BLabelPasienRSSKSection4();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRSSKSection11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRSSKSection12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRSSKSection13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRSSKSection14)).BeginInit();
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
            this.Detail.HeightF = 752.4846F;
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
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vLabelInpatientRegistration);
            // 
            // subSection1
            // 
            this.subSection1.Dpi = 254F;
            this.subSection1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 116.7581F);
            this.subSection1.Name = "subSection1";
            this.subSection1.ReportSource = this.bLabelPasienRSSKSection11;
            this.subSection1.SizeF = new System.Drawing.SizeF(2091F, 58.42F);
            // 
            // subSection2
            // 
            this.subSection2.Dpi = 254F;
            this.subSection2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 309.1937F);
            this.subSection2.Name = "subSection2";
            this.subSection2.ReportSource = this.bLabelPasienRSSKSection12;
            this.subSection2.SizeF = new System.Drawing.SizeF(2091F, 58.41998F);
            // 
            // subSection3
            // 
            this.subSection3.Dpi = 254F;
            this.subSection3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 501.6292F);
            this.subSection3.Name = "subSection3";
            this.subSection3.ReportSource = this.bLabelPasienRSSKSection13;
            this.subSection3.SizeF = new System.Drawing.SizeF(2091F, 58.41998F);
            // 
            // subSection4
            // 
            this.subSection4.Dpi = 254F;
            this.subSection4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 694.0646F);
            this.subSection4.Name = "subSection4";
            this.subSection4.ReportSource = this.bLabelPasienRSSKSection14;
            this.subSection4.SizeF = new System.Drawing.SizeF(2091F, 58.41998F);
            // 
            // BLabelPasienRanapRSSBB
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.DataSource = this.bs;
            this.Margins = new System.Drawing.Printing.Margins(87, 72, 30, 30);
            this.PageHeight = 4000;
            this.PageWidth = 2250;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRSSKSection11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRSSKSection12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRSSKSection13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bLabelPasienRSSKSection14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        protected System.Windows.Forms.BindingSource bs;
        private DevExpress.XtraReports.UI.XRSubreport subSection1;
        private DevExpress.XtraReports.UI.XRSubreport subSection2;
        private DevExpress.XtraReports.UI.XRSubreport subSection3;
        private DevExpress.XtraReports.UI.XRSubreport subSection4;
        private BLabelPasienRSSKSection1 bLabelPasienRSSKSection11;
        private BLabelPasienRSSKSection2 bLabelPasienRSSKSection12;
        private BLabelPasienRSSKSection3 bLabelPasienRSSKSection13;
        private BLabelPasienRSSKSection4 bLabelPasienRSSKSection14;
    }
}
