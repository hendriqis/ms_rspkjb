namespace QIS.Medinfras.ReportDesktop
{
    partial class EpisodeSummaryDiagnosisRptRSAJ
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
            this.txtDiagnosisText = new DevExpress.XtraReports.UI.XRLabel();
            this.txtDiagnosis = new DevExpress.XtraReports.UI.XRLabel();
            this.txtDiagnoseType = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.txtDiagnosisText,
            this.txtDiagnosis});
            this.Detail.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Detail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Detail.HeightF = 36.66666F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseFont = false;
            this.Detail.StylePriority.UseForeColor = false;
            this.Detail.StylePriority.UseTextAlignment = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // txtDiagnosisText
            // 
            this.txtDiagnosisText.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtDiagnosisText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDiagnosisText.LocationFloat = new DevExpress.Utils.PointFloat(48.04167F, 0F);
            this.txtDiagnosisText.Multiline = true;
            this.txtDiagnosisText.Name = "txtDiagnosisText";
            this.txtDiagnosisText.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtDiagnosisText.SizeF = new System.Drawing.SizeF(475F, 18F);
            this.txtDiagnosisText.StylePriority.UseFont = false;
            this.txtDiagnosisText.StylePriority.UseForeColor = false;
            this.txtDiagnosisText.StylePriority.UseTextAlignment = false;
            this.txtDiagnosisText.Text = "[DiagnosisText]";
            this.txtDiagnosisText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // txtDiagnosis
            // 
            this.txtDiagnosis.CanShrink = true;
            this.txtDiagnosis.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Italic);
            this.txtDiagnosis.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDiagnosis.LocationFloat = new DevExpress.Utils.PointFloat(48.04141F, 18.66666F);
            this.txtDiagnosis.Multiline = true;
            this.txtDiagnosis.Name = "txtDiagnosis";
            this.txtDiagnosis.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtDiagnosis.SizeF = new System.Drawing.SizeF(475F, 18F);
            this.txtDiagnosis.StylePriority.UseFont = false;
            this.txtDiagnosis.StylePriority.UseForeColor = false;
            this.txtDiagnosis.StylePriority.UseTextAlignment = false;
            this.txtDiagnosis.Text = "[DiagnoseName] ([DiagnoseID])";
            this.txtDiagnosis.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.txtDiagnosis.Visible = false;
            // 
            // txtDiagnoseType
            // 
            this.txtDiagnoseType.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.txtDiagnoseType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDiagnoseType.LocationFloat = new DevExpress.Utils.PointFloat(26.04167F, 7.000008F);
            this.txtDiagnoseType.Multiline = true;
            this.txtDiagnoseType.Name = "txtDiagnoseType";
            this.txtDiagnoseType.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtDiagnoseType.SizeF = new System.Drawing.SizeF(497F, 18F);
            this.txtDiagnoseType.StylePriority.UseFont = false;
            this.txtDiagnoseType.StylePriority.UseForeColor = false;
            this.txtDiagnoseType.StylePriority.UseTextAlignment = false;
            this.txtDiagnoseType.Text = "[DiagnoseType]";
            this.txtDiagnoseType.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.txtDiagnoseType.Visible = false;
            this.txtDiagnoseType.WordWrap = false;
            // 
            // TopMargin
            // 
            this.TopMargin.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TopMargin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.StylePriority.UseFont = false;
            this.TopMargin.StylePriority.UseForeColor = false;
            this.TopMargin.StylePriority.UseTextAlignment = false;
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BottomMargin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.StylePriority.UseFont = false;
            this.BottomMargin.StylePriority.UseForeColor = false;
            this.BottomMargin.StylePriority.UseTextAlignment = false;
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.txtDiagnoseType});
            this.GroupHeader1.HeightF = 25.00001F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // EpisodeSummaryDiagnosisRptRSAJ
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1});
            this.DetailPrintCountOnEmptyDataSource = 0;
            this.Margins = new System.Drawing.Printing.Margins(0, 28, 0, 0);
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel txtDiagnoseType;
        private DevExpress.XtraReports.UI.XRLabel txtDiagnosis;
        private DevExpress.XtraReports.UI.XRLabel txtDiagnosisText;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
    }
}
