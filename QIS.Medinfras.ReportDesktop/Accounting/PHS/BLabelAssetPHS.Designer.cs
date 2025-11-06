namespace QIS.Medinfras.ReportDesktop
{
    partial class BLabelAssetPHS
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
            this.qrImg = new DevExpress.XtraReports.UI.XRPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.qrImg});
            this.Detail.HeightF = 112.3472F;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            // 
            // PageFooter
            // 
            this.PageFooter.Expanded = false;
            this.PageFooter.HeightF = 0F;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.ReportDesktop.CBuktiPendaftaranBROS);
            // 
            // qrImg
            // 
            this.qrImg.Dpi = 254F;
            this.qrImg.LocationFloat = new DevExpress.Utils.PointFloat(35.07977F, 12.34722F);
            this.qrImg.Name = "qrImg";
            this.qrImg.SizeF = new System.Drawing.SizeF(100F, 100F);
            this.qrImg.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // BLabelAssetPHS
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.DataSource = this.bs;
            this.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 150;
            this.PageWidth = 200;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.PaperName = "BUKTI_PENDAFTARAN";
            this.ShowPreviewMarginLines = false;
            this.ShowPrintMarginsWarning = false;
            this.SnapToGrid = false;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private System.Windows.Forms.BindingSource bs;
      
        private DevExpress.XtraReports.UI.XRPictureBox qrImg;
    }
}
