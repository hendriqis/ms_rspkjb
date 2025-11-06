namespace QIS.Medinfras.ReportDesktop
{
    partial class NewMCUDiagnosticResultRSPKSB
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.subImagingResultForDt = new DevExpress.XtraReports.UI.XRSubreport();
            this.NewMCUDiagnosticResultDtRSPKSB = new QIS.Medinfras.ReportDesktop.NewMCUDiagnosticResultDtRSPKSB();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            ((System.ComponentModel.ISupportInitialize)(this.NewMCUDiagnosticResultDtRSPKSB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.subImagingResultForDt});
            this.Detail.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Detail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Detail.HeightF = 23F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseFont = false;
            this.Detail.StylePriority.UseForeColor = false;
            this.Detail.StylePriority.UseTextAlignment = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
            // 
            // subImagingResultForDt
            // 
            this.subImagingResultForDt.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.subImagingResultForDt.Name = "subImagingResultForDt";
            this.subImagingResultForDt.ReportSource = this.NewMCUDiagnosticResultDtRSPKSB;
            this.subImagingResultForDt.SizeF = new System.Drawing.SizeF(724.0001F, 23F);
            // 
            // TopMargin
            // 
            this.TopMargin.Font = new System.Drawing.Font("Tahoma", 9F);
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
            this.BottomMargin.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BottomMargin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BottomMargin.HeightF = 13.02778F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.StylePriority.UseFont = false;
            this.BottomMargin.StylePriority.UseForeColor = false;
            this.BottomMargin.StylePriority.UseTextAlignment = false;
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.ImagingResultHd);
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ChargeTransactionID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 0F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageBreak1});
            this.GroupFooter1.HeightF = 18.05556F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.999996F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // NewMCUDiagnosticResultRSPKSB
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1,
            this.GroupFooter1});
            this.DataSource = this.bs;
            this.DetailPrintCountOnEmptyDataSource = 0;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(0, 26, 0, 13);
            this.PageHeight = 719;
            this.PageWidth = 750;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.PrintOnEmptyDataSource = false;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.NewMCUDiagnosticResultDtRSPKSB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private System.Windows.Forms.BindingSource bs;
        private DevExpress.XtraReports.UI.XRSubreport subImagingResultForDt;
        private NewMCUDiagnosticResultDtRSPKSB NewMCUDiagnosticResultDtRSPKSB;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak1;
    }
}
