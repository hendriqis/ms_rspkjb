namespace QIS.Medinfras.ReportDesktop
{
    partial class MRPhysicalExamRSDI
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.subVitalSign = new DevExpress.XtraReports.UI.XRSubreport();
            this.mrdtVitalSignRSDI = new QIS.Medinfras.ReportDesktop.MRDTVitalSignRSDI();
            this.subReviewOfSystem = new DevExpress.XtraReports.UI.XRSubreport();
            this.mrdtReviewOfSystemRSDI = new QIS.Medinfras.ReportDesktop.MRDTReviewOfSystemRSDI();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.DetailReport = new DevExpress.XtraReports.UI.DetailReportBand();
            this.Detail1 = new DevExpress.XtraReports.UI.DetailBand();
            ((System.ComponentModel.ISupportInitialize)(this.mrdtVitalSignRSDI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mrdtReviewOfSystemRSDI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.subVitalSign,
            this.subReviewOfSystem});
            this.Detail.HeightF = 52.58334F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // subVitalSign
            // 
            this.subVitalSign.CanShrink = true;
            this.subVitalSign.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.subVitalSign.Name = "subVitalSign";
            this.subVitalSign.ReportSource = this.mrdtVitalSignRSDI;
            this.subVitalSign.SizeF = new System.Drawing.SizeF(685.8333F, 24.04166F);
            // 
            // subReviewOfSystem
            // 
            this.subReviewOfSystem.CanShrink = true;
            this.subReviewOfSystem.LocationFloat = new DevExpress.Utils.PointFloat(0F, 28.54168F);
            this.subReviewOfSystem.Name = "subReviewOfSystem";
            this.subReviewOfSystem.ReportSource = this.mrdtReviewOfSystemRSDI;
            this.subReviewOfSystem.SizeF = new System.Drawing.SizeF(685.8333F, 24.04167F);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 11F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // DetailReport
            // 
            this.DetailReport.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail1});
            this.DetailReport.Level = 0;
            this.DetailReport.Name = "DetailReport";
            // 
            // Detail1
            // 
            this.Detail1.HeightF = 0F;
            this.Detail1.Name = "Detail1";
            // 
            // MRPhysicalExamRSDI
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.DetailReport});
            this.Margins = new System.Drawing.Printing.Margins(26, 123, 0, 11);
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.mrdtVitalSignRSDI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mrdtReviewOfSystemRSDI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRSubreport subVitalSign;
        private MRDTVitalSignRSDI mrdtVitalSignRSDI;
        private DevExpress.XtraReports.UI.XRSubreport subReviewOfSystem;
        private MRDTReviewOfSystemRSDI mrdtReviewOfSystemRSDI;
        private DevExpress.XtraReports.UI.DetailReportBand DetailReport;
        private DevExpress.XtraReports.UI.DetailBand Detail1;
    }
}
