namespace QIS.Medinfras.ReportDesktop
{
    partial class KartuPasien7Rpt
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
            this.lblPatientName,
            this.lblMedicalNo});
            this.Detail.HeightF = 522F;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 3F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 0F;
            // 
            // lblPatientName
            // 
            this.lblPatientName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPatientName.Dpi = 254F;
            this.lblPatientName.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientName.LocationFloat = new DevExpress.Utils.PointFloat(62.7337F, 401.2693F);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPatientName.SizeF = new System.Drawing.SizeF(268F, 39.69F);
            this.lblPatientName.StylePriority.UseBorders = false;
            this.lblPatientName.StylePriority.UseFont = false;
            this.lblPatientName.StylePriority.UseTextAlignment = false;
            this.lblPatientName.Text = "lblPatientName";
            this.lblPatientName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblMedicalNo
            // 
            this.lblMedicalNo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblMedicalNo.Dpi = 254F;
            this.lblMedicalNo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMedicalNo.LocationFloat = new DevExpress.Utils.PointFloat(61.66682F, 344.7493F);
            this.lblMedicalNo.Name = "lblMedicalNo";
            this.lblMedicalNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblMedicalNo.SizeF = new System.Drawing.SizeF(268F, 58.21F);
            this.lblMedicalNo.StylePriority.UseBorders = false;
            this.lblMedicalNo.StylePriority.UseFont = false;
            this.lblMedicalNo.StylePriority.UseTextAlignment = false;
            this.lblMedicalNo.Text = "lblMedicalNo";
            this.lblMedicalNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // KartuPasien7Rpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 3, 0);
            this.PageHeight = 831;
            this.PageWidth = 828;
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
    }
}
