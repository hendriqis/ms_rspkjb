namespace QIS.Medinfras.ReportDesktop
{
    partial class BNursingOutcomeItemSubRpt
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
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.lblNOCHeader = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblNOCHeader});
            this.ReportHeader.HeightF = 25F;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrLabel3});
            this.Detail.HeightF = 12.5F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 0F;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2});
            this.GroupHeader1.Dpi = 254F;
            this.GroupHeader1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Bold);
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("NursingDiagnoseItemID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 33F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.StylePriority.UseFont = false;
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "cfOutcomeDisplayText")});
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0.0001614889F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(539.7497F, 33.02F);
            this.xrLabel2.Text = "xrLabel2";
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NursingIndicatorText", "- {0}")});
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0.0002018611F, 0F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(444.4997F, 31.75F);
            this.xrLabel3.Text = "xrLabel3";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine1});
            this.GroupFooter1.Dpi = 254F;
            this.GroupFooter1.HeightF = 64F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrLine1
            // 
            this.xrLine1.Dpi = 254F;
            this.xrLine1.LineStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.xrLine1.LineWidth = 3;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(539.75F, 31.75F);
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ScaleScore")});
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(444.5F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(95.25F, 31.75F);
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "xrLabel1";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(QIS.Medinfras.Data.Service.vNursingTransactionOutcomeDt);
            // 
            // lblNOCHeader
            // 
            this.lblNOCHeader.Dpi = 254F;
            this.lblNOCHeader.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lblNOCHeader.Name = "lblNOCHeader";
            this.lblNOCHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblNOCHeader.SizeF = new System.Drawing.SizeF(539.75F, 63.5F);
            this.lblNOCHeader.Text = "lblNOCHeader";
            // 
            // BNursingOutcomeItemSubRpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.GroupHeader1,
            this.GroupFooter1});
            this.DataSource = this.bindingSource1;
            this.Margins = new System.Drawing.Printing.Margins(12, 12, 64, 64);
            this.PageWidth = 240;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.SnappingMode = DevExpress.XtraReports.UI.SnappingMode.SnapToGrid;
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.GroupFooter1, 0);
            this.Controls.SetChildIndex(this.GroupHeader1, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRLabel lblNOCHeader;
    }
}
