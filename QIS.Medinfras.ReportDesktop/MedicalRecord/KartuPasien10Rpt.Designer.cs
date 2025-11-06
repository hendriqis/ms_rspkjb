namespace QIS.Medinfras.ReportDesktop
{
    partial class KartuPasien10Rpt
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
            this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
            this.lblMedicalNo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAddress = new DevExpress.XtraReports.UI.XRLabel();
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
            this.lblAddress,
            this.lblMedicalNo,
            this.xrBarCode1,
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
            this.lblPatientName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientName.LocationFloat = new DevExpress.Utils.PointFloat(42.39583F, 236.3749F);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPatientName.SizeF = new System.Drawing.SizeF(694.9585F, 31.75009F);
            this.lblPatientName.StylePriority.UseFont = false;
            this.lblPatientName.StylePriority.UseTextAlignment = false;
            this.lblPatientName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrBarCode1
            // 
            this.xrBarCode1.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrBarCode1.AutoModule = true;
            this.xrBarCode1.Dpi = 254F;
            this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(73.29167F, 389.7084F);
            this.xrBarCode1.Module = 5.08F;
            this.xrBarCode1.Name = "xrBarCode1";
            this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 254F);
            this.xrBarCode1.ShowText = false;
            this.xrBarCode1.SizeF = new System.Drawing.SizeF(250F, 70F);
            this.xrBarCode1.StylePriority.UseFont = false;
            this.xrBarCode1.StylePriority.UsePadding = false;
            this.xrBarCode1.StylePriority.UseTextAlignment = false;
            this.xrBarCode1.Symbology = code128Generator1;
            this.xrBarCode1.Text = "00-01-09-06";
            this.xrBarCode1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblMedicalNo
            // 
            this.lblMedicalNo.Dpi = 254F;
            this.lblMedicalNo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMedicalNo.LocationFloat = new DevExpress.Utils.PointFloat(40.54174F, 177.3958F);
            this.lblMedicalNo.Name = "lblMedicalNo";
            this.lblMedicalNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblMedicalNo.SizeF = new System.Drawing.SizeF(698.5F, 42.33333F);
            this.lblMedicalNo.StylePriority.UseFont = false;
            this.lblMedicalNo.StylePriority.UseTextAlignment = false;
            this.lblMedicalNo.Text = "00-01-09-06";
            this.lblMedicalNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblAddress
            // 
            this.lblAddress.Dpi = 254F;
            this.lblAddress.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddress.LocationFloat = new DevExpress.Utils.PointFloat(39.90623F, 278.8958F);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAddress.SizeF = new System.Drawing.SizeF(697.6041F, 39.6875F);
            this.lblAddress.StylePriority.UseFont = false;
            this.lblAddress.StylePriority.UseTextAlignment = false;
            this.lblAddress.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // KartuPasien10Rpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 2, 0);
            this.PageHeight = 500;
            this.PageWidth = 828;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ShowPrintMarginsWarning = false;
            this.SnappingMode = DevExpress.XtraReports.UI.SnappingMode.SnapToGrid;
            this.SnapToGrid = false;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRBarCode xrBarCode1;
        private DevExpress.XtraReports.UI.XRLabel lblPatientName;
        private DevExpress.XtraReports.UI.XRLabel lblMedicalNo;
        private DevExpress.XtraReports.UI.XRLabel lblAddress;
    }
}
