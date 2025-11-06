namespace QIS.Medinfras.ReportDesktop
{
    partial class BAmplopSuratKeteranganRawatInap_RSSBB
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
            this.lblPatientName = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 30F;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblPatientName});
            this.Detail.HeightF = 1307.253F;
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
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vRegistrationInfo);
            // 
            // lblPatientName
            // 
            this.lblPatientName.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FullName")});
            this.lblPatientName.Dpi = 254F;
            this.lblPatientName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientName.LocationFloat = new DevExpress.Utils.PointFloat(825.5F, 542.6913F);
            this.lblPatientName.Multiline = true;
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPatientName.SizeF = new System.Drawing.SizeF(574.15F, 108F);
            this.lblPatientName.StylePriority.UseFont = false;
            this.lblPatientName.StylePriority.UseTextAlignment = false;
            this.lblPatientName.Text = "lblPatientName";
            this.lblPatientName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BAmplopSuratKeteranganRawatInap_RSSBB
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.DataSource = this.bs;
            this.Margins = new System.Drawing.Printing.Margins(99, 109, 30, 30);
            this.PageHeight = 1570;
            this.PageWidth = 1999;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        protected System.Windows.Forms.BindingSource bs;
        private DevExpress.XtraReports.UI.XRLabel lblPatientName;
    }
}
