namespace QIS.Medinfras.ReportDesktop
{
    partial class MRDischargeOPRSPKSB
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
            this.lblDischarge = new DevExpress.XtraReports.UI.XRLabel();
            this.lblLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblDischarge,
            this.lblLabel1,
            this.lblLabel2});
            this.Detail.HeightF = 18.00004F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblDischarge
            // 
            this.lblDischarge.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDischarge.LocationFloat = new DevExpress.Utils.PointFloat(210.6194F, 0F);
            this.lblDischarge.Multiline = true;
            this.lblDischarge.Name = "lblDischarge";
            this.lblDischarge.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblDischarge.SizeF = new System.Drawing.SizeF(463.7556F, 18F);
            this.lblDischarge.StylePriority.UseFont = false;
            this.lblDischarge.StylePriority.UseTextAlignment = false;
            this.lblDischarge.Text = "lblDischarge";
            this.lblDischarge.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblLabel1
            // 
            this.lblLabel1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lblLabel1.Multiline = true;
            this.lblLabel1.Name = "lblLabel1";
            this.lblLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblLabel1.SizeF = new System.Drawing.SizeF(200.3474F, 18F);
            this.lblLabel1.StylePriority.UseFont = false;
            this.lblLabel1.StylePriority.UseTextAlignment = false;
            this.lblLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblLabel2
            // 
            this.lblLabel2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabel2.LocationFloat = new DevExpress.Utils.PointFloat(200.3474F, 4.238552E-05F);
            this.lblLabel2.Multiline = true;
            this.lblLabel2.Name = "lblLabel2";
            this.lblLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblLabel2.SizeF = new System.Drawing.SizeF(10.27205F, 18F);
            this.lblLabel2.StylePriority.UseFont = false;
            this.lblLabel2.StylePriority.UseTextAlignment = false;
            this.lblLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
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
            this.BottomMargin.HeightF = 6F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // MRDischargeOPRSPKSB
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margins = new System.Drawing.Printing.Margins(26, 130, 0, 6);
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel lblDischarge;
        private DevExpress.XtraReports.UI.XRLabel lblLabel1;
        private DevExpress.XtraReports.UI.XRLabel lblLabel2;
    }
}
