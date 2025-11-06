namespace QIS.Medinfras.ReportDesktop
{
    partial class KartuPasien12Rpt
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
            DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator1 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
            this.lblPatientName = new DevExpress.XtraReports.UI.XRLabel();
            this.barcodeMedicalNo = new DevExpress.XtraReports.UI.XRBarCode();
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
            this.barcodeMedicalNo,
            this.lblMedicalNo,
            this.lblPatientName});
            this.Detail.HeightF = 696.6754F;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 0F;
            // 
            // lblPatientName
            // 
            this.lblPatientName.Angle = 270F;
            this.lblPatientName.Dpi = 254F;
            this.lblPatientName.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientName.LocationFloat = new DevExpress.Utils.PointFloat(212.5599F, 40.43496F);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.lblPatientName.SizeF = new System.Drawing.SizeF(60.45482F, 644.2155F);
            this.lblPatientName.StylePriority.UseFont = false;
            this.lblPatientName.StylePriority.UsePadding = false;
            this.lblPatientName.StylePriority.UseTextAlignment = false;
            this.lblPatientName.Text = "lblPatientName";
            this.lblPatientName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // barcodeMedicalNo
            // 
            this.barcodeMedicalNo.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.barcodeMedicalNo.AutoModule = true;
            this.barcodeMedicalNo.BarCodeOrientation = DevExpress.XtraPrinting.BarCode.BarCodeOrientation.RotateRight;
            this.barcodeMedicalNo.Dpi = 254F;
            this.barcodeMedicalNo.LocationFloat = new DevExpress.Utils.PointFloat(47.4001F, 41.24192F);
            this.barcodeMedicalNo.Module = 5.08F;
            this.barcodeMedicalNo.Name = "barcodeMedicalNo";
            this.barcodeMedicalNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 254F);
            this.barcodeMedicalNo.ShowText = false;
            this.barcodeMedicalNo.SizeF = new System.Drawing.SizeF(83.31258F, 264.9097F);
            this.barcodeMedicalNo.StylePriority.UseFont = false;
            this.barcodeMedicalNo.StylePriority.UsePadding = false;
            this.barcodeMedicalNo.StylePriority.UseTextAlignment = false;
            this.barcodeMedicalNo.Symbology = code128Generator1;
            this.barcodeMedicalNo.Text = "00-00-00-00";
            this.barcodeMedicalNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblMedicalNo
            // 
            this.lblMedicalNo.Angle = 270F;
            this.lblMedicalNo.Dpi = 254F;
            this.lblMedicalNo.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMedicalNo.LocationFloat = new DevExpress.Utils.PointFloat(151.7809F, 40.43496F);
            this.lblMedicalNo.Name = "lblMedicalNo";
            this.lblMedicalNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.lblMedicalNo.SizeF = new System.Drawing.SizeF(60.45F, 644.2277F);
            this.lblMedicalNo.StylePriority.UseFont = false;
            this.lblMedicalNo.StylePriority.UsePadding = false;
            this.lblMedicalNo.StylePriority.UseTextAlignment = false;
            this.lblMedicalNo.Text = "lblMedicalNo";
            this.lblMedicalNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // KartuPasien12Rpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 800;
            this.PageWidth = 499;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ShowPrintMarginsWarning = false;
            this.SnappingMode = DevExpress.XtraReports.UI.SnappingMode.SnapToGrid;
            this.SnapToGrid = false;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRLabel lblPatientName;
        private DevExpress.XtraReports.UI.XRBarCode barcodeMedicalNo;
        private DevExpress.XtraReports.UI.XRLabel lblMedicalNo;

    }
}
