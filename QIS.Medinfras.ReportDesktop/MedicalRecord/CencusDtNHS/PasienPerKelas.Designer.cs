namespace QIS.Medinfras.ReportDesktop
{
    partial class PasienPerKelas
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
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell205 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell206 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell207 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell196 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell197 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell198 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell208 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell209 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell210 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable4});
            this.Detail.HeightF = 35.41667F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable4
            // 
            this.xrTable4.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable4.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow13});
            this.xrTable4.SizeF = new System.Drawing.SizeF(313.8621F, 35.41667F);
            this.xrTable4.StylePriority.UseBorders = false;
            this.xrTable4.StylePriority.UseFont = false;
            this.xrTable4.StylePriority.UsePadding = false;
            this.xrTable4.StylePriority.UseTextAlignment = false;
            this.xrTable4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell205,
            this.xrTableCell206,
            this.xrTableCell207,
            this.xrTableCell196,
            this.xrTableCell197,
            this.xrTableCell198,
            this.xrTableCell208,
            this.xrTableCell209,
            this.xrTableCell210});
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 1D;
            // 
            // xrTableCell205
            // 
            this.xrTableCell205.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "SUITE")});
            this.xrTableCell205.Name = "xrTableCell205";
            this.xrTableCell205.Weight = 0.31779898701672776D;
            // 
            // xrTableCell206
            // 
            this.xrTableCell206.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "VVIP")});
            this.xrTableCell206.Name = "xrTableCell206";
            this.xrTableCell206.Weight = 0.30743329601665415D;
            // 
            // xrTableCell207
            // 
            this.xrTableCell207.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "VIP")});
            this.xrTableCell207.Name = "xrTableCell207";
            this.xrTableCell207.Weight = 0.34408142866875313D;
            // 
            // xrTableCell196
            // 
            this.xrTableCell196.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "EKSEKUTIF")});
            this.xrTableCell196.Name = "xrTableCell196";
            this.xrTableCell196.Weight = 0.34026545805948283D;
            // 
            // xrTableCell197
            // 
            this.xrTableCell197.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "I")});
            this.xrTableCell197.Name = "xrTableCell197";
            this.xrTableCell197.Weight = 0.28695457054821466D;
            // 
            // xrTableCell198
            // 
            this.xrTableCell198.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "IB")});
            this.xrTableCell198.Name = "xrTableCell198";
            this.xrTableCell198.Weight = 0.30702796850134989D;
            // 
            // xrTableCell208
            // 
            this.xrTableCell208.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "II")});
            this.xrTableCell208.Name = "xrTableCell208";
            this.xrTableCell208.Weight = 0.28435453083246903D;
            // 
            // xrTableCell209
            // 
            this.xrTableCell209.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "III")});
            this.xrTableCell209.Name = "xrTableCell209";
            this.xrTableCell209.Weight = 0.29638373526316952D;
            // 
            // xrTableCell210
            // 
            this.xrTableCell210.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ISO")});
            this.xrTableCell210.Name = "xrTableCell210";
            this.xrTableCell210.Weight = 0.33165899236450158D;
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
            this.BottomMargin.HeightF = 11F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetPatientCensusPerClassNHS);
            // 
            // ReportHeader
            // 
            this.ReportHeader.HeightF = 0F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // PageHeader
            // 
            this.PageHeader.HeightF = 0F;
            this.PageHeader.Name = "PageHeader";
            // 
            // PasienPerKelas
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageHeader});
            this.DataSource = this.bs;
            this.Margins = new System.Drawing.Printing.Margins(26, 49, 0, 11);
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private System.Windows.Forms.BindingSource bs;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRTable xrTable4;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow13;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell205;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell206;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell207;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell196;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell197;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell198;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell208;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell209;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell210;
    }
}
