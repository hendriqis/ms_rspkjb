namespace QIS.Medinfras.ReportDesktop
{
    partial class LDetailSaldoPiutangDalamProses
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
            this.lblPeriode = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.StylePriority.UseFont = false;
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetLDetailSaldoPiutangDalamProses);
            // 
            // lblReportSubTitle
            // 
            this.lblReportSubTitle.StylePriority.UseBorders = false;
            this.lblReportSubTitle.StylePriority.UseFont = false;
            this.lblReportSubTitle.StylePriority.UseTextAlignment = false;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 74.89578F;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblPeriode});
            this.ReportHeader.HeightF = 341.3127F;
            this.ReportHeader.Controls.SetChildIndex(this.lblReportSubTitle, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblPeriode, 0);
            // 
            // Detail
            // 
            this.Detail.HeightF = 37.04168F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 92.29169F;
            // 
            // lblPeriode
            // 
            this.lblPeriode.Dpi = 254F;
            this.lblPeriode.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeriode.LocationFloat = new DevExpress.Utils.PointFloat(25.00009F, 309.351F);
            this.lblPeriode.Name = "lblPeriode";
            this.lblPeriode.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPeriode.SizeF = new System.Drawing.SizeF(4023F, 31.96161F);
            this.lblPeriode.StylePriority.UseFont = false;
            this.lblPeriode.StylePriority.UseTextAlignment = false;
            this.lblPeriode.Text = "lblPeriode";
            this.lblPeriode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // LDetailSaldoUtangHonorDokter
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.Margins = new System.Drawing.Printing.Margins(64, 64, 64, 75);
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRLabel lblPeriode;
    }
}
