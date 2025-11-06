namespace QIS.Medinfras.ReportDesktop
{
    partial class MRInstructionNew
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
            this.lblLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblInstruction = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblLabel1,
            this.lblLabel2,
            this.lblInstruction});
            this.Detail.HeightF = 23.20833F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblLabel1
            // 
            this.lblLabel1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 5.208332F);
            this.lblLabel1.Multiline = true;
            this.lblLabel1.Name = "lblLabel1";
            this.lblLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblLabel1.SizeF = new System.Drawing.SizeF(228.4724F, 18F);
            this.lblLabel1.StylePriority.UseFont = false;
            this.lblLabel1.StylePriority.UseTextAlignment = false;
            this.lblLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblLabel2
            // 
            this.lblLabel2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabel2.LocationFloat = new DevExpress.Utils.PointFloat(228.4724F, 5.208332F);
            this.lblLabel2.Multiline = true;
            this.lblLabel2.Name = "lblLabel2";
            this.lblLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblLabel2.SizeF = new System.Drawing.SizeF(10.27205F, 18F);
            this.lblLabel2.StylePriority.UseFont = false;
            this.lblLabel2.StylePriority.UseTextAlignment = false;
            this.lblLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblInstruction
            // 
            this.lblInstruction.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstruction.LocationFloat = new DevExpress.Utils.PointFloat(238.7444F, 5.208332F);
            this.lblInstruction.Multiline = true;
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblInstruction.SizeF = new System.Drawing.SizeF(435.6306F, 18F);
            this.lblInstruction.StylePriority.UseFont = false;
            this.lblInstruction.StylePriority.UseTextAlignment = false;
            this.lblInstruction.Text = "lblInstruction";
            this.lblInstruction.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
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
            this.BottomMargin.HeightF = 6.249905F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // MRInstructionNew
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Margins = new System.Drawing.Printing.Margins(26, 123, 0, 6);
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel lblLabel1;
        private DevExpress.XtraReports.UI.XRLabel lblLabel2;
        private DevExpress.XtraReports.UI.XRLabel lblInstruction;
    }
}
