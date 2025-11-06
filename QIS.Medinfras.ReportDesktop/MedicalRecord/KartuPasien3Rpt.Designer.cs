namespace QIS.Medinfras.ReportDesktop
{
    partial class KartuPasien3Rpt
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
            this.lblPatientName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblMedicalNo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSinceDate = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Expanded = false;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblSinceDate,
            this.lblMedicalNo,
            this.lblPatientName});
            this.Detail.HeightF = 520.875F;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 2F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 0F;
            // 
            // lblPatientName
            // 
            this.lblPatientName.Dpi = 254F;
            this.lblPatientName.Font = new System.Drawing.Font("Farrington7BN", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(0)));
            this.lblPatientName.LocationFloat = new DevExpress.Utils.PointFloat(29.00005F, 296.5F);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPatientName.SizeF = new System.Drawing.SizeF(745.2292F, 58.2084F);
            this.lblPatientName.StylePriority.UseFont = false;
            this.lblPatientName.StylePriority.UseTextAlignment = false;
            this.lblPatientName.Text = "Ryan Gerald Herman";
            this.lblPatientName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // lblMedicalNo
            // 
            this.lblMedicalNo.Dpi = 254F;
            this.lblMedicalNo.Font = new System.Drawing.Font("Farrington7BN", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMedicalNo.LocationFloat = new DevExpress.Utils.PointFloat(28.79185F, 400F);
            this.lblMedicalNo.Name = "lblMedicalNo";
            this.lblMedicalNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblMedicalNo.SizeF = new System.Drawing.SizeF(285.75F, 58.20834F);
            this.lblMedicalNo.StylePriority.UseFont = false;
            this.lblMedicalNo.StylePriority.UseTextAlignment = false;
            this.lblMedicalNo.Text = "00-01-09-06";
            this.lblMedicalNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblSinceDate
            // 
            this.lblSinceDate.Dpi = 254F;
            this.lblSinceDate.Font = new System.Drawing.Font("Farrington7BN", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSinceDate.LocationFloat = new DevExpress.Utils.PointFloat(439.6875F, 400F);
            this.lblSinceDate.Name = "lblSinceDate";
            this.lblSinceDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblSinceDate.SizeF = new System.Drawing.SizeF(254F, 58.20834F);
            this.lblSinceDate.StylePriority.UseFont = false;
            this.lblSinceDate.StylePriority.UseTextAlignment = false;
            this.lblSinceDate.Text = "10-APR-2014";
            this.lblSinceDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // KartuPasien3Rpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 2, 0);
            this.PageHeight = 500;
            this.PageWidth = 800;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ShowPrintMarginsWarning = false;
            this.SnappingMode = DevExpress.XtraReports.UI.SnappingMode.SnapToGrid;
            this.SnapToGrid = false;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRLabel lblPatientName;
        private DevExpress.XtraReports.UI.XRLabel lblMedicalNo;
        private DevExpress.XtraReports.UI.XRLabel lblSinceDate;
    }
}
