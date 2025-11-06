namespace QIS.Medinfras.ReportDesktop
{
    partial class LNeracaBentukTDetailPasiva
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
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.tablePasiva = new DevExpress.XtraReports.UI.XRTable();
            this.rowTablePasiva = new DevExpress.XtraReports.UI.XRTableRow();
            this.cAccountName = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.lblServiceUnit = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.cSubTotalPasiva = new DevExpress.XtraReports.UI.XRTableCell();
            this.fnBalanceEND = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.tablePasiva)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tablePasiva});
            this.Detail.HeightF = 15F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // tablePasiva
            // 
            this.tablePasiva.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.tablePasiva.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tablePasiva.LocationFloat = new DevExpress.Utils.PointFloat(10.00002F, 0F);
            this.tablePasiva.Name = "tablePasiva";
            this.tablePasiva.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.tablePasiva.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.rowTablePasiva});
            this.tablePasiva.SizeF = new System.Drawing.SizeF(378.7499F, 15F);
            this.tablePasiva.StylePriority.UseBorders = false;
            this.tablePasiva.StylePriority.UseFont = false;
            this.tablePasiva.StylePriority.UsePadding = false;
            this.tablePasiva.StylePriority.UseTextAlignment = false;
            this.tablePasiva.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // rowTablePasiva
            // 
            this.rowTablePasiva.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cAccountName,
            this.xrTableCell10});
            this.rowTablePasiva.Name = "rowTablePasiva";
            this.rowTablePasiva.Weight = 1D;
            this.rowTablePasiva.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.rowTablePasiva_BeforePrint);
            // 
            // cAccountName
            // 
            this.cAccountName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.cAccountName.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GLAccountName")});
            this.cAccountName.Name = "cAccountName";
            this.cAccountName.StylePriority.UseBorders = false;
            this.cAccountName.Text = "cAccountName";
            this.cAccountName.Weight = 1.8411936820867958D;
            this.cAccountName.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.cAccountName_BeforePrint);
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "BalanceEND", "{0:n2}")});
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell10.Weight = 1.0719838342468291D;
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
            this.BottomMargin.HeightF = 12F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetGLBalancePerPeriodPerLevel);
            this.bs.Filter = "";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1});
            this.ReportHeader.HeightF = 12.58333F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel1
            // 
            this.xrLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.xrLabel1.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Right | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(388.7499F, 12.58333F);
            this.xrLabel1.StylePriority.UseBackColor = false;
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "LIABILITAS & EKUITAS";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblServiceUnit});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("GCGLAccountType", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 23F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // lblServiceUnit
            // 
            this.lblServiceUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(242)))), ((int)(((byte)(252)))));
            this.lblServiceUnit.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblServiceUnit.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GLAccountType")});
            this.lblServiceUnit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceUnit.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lblServiceUnit.Name = "lblServiceUnit";
            this.lblServiceUnit.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.lblServiceUnit.SizeF = new System.Drawing.SizeF(388.7499F, 23F);
            this.lblServiceUnit.StylePriority.UseBackColor = false;
            this.lblServiceUnit.StylePriority.UseBorders = false;
            this.lblServiceUnit.StylePriority.UseFont = false;
            this.lblServiceUnit.StylePriority.UsePadding = false;
            this.lblServiceUnit.StylePriority.UseTextAlignment = false;
            this.lblServiceUnit.Text = "[GLAccountType]";
            this.lblServiceUnit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.GroupFooter1.HeightF = 22.08332F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.xrTable1.ForeColor = System.Drawing.Color.Black;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(378.7499F, 15F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseForeColor = false;
            this.xrTable1.StylePriority.UsePadding = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.cSubTotalPasiva});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "TOTAL [GLAccountType]";
            this.xrTableCell1.Weight = 1.9149255782507455D;
            // 
            // cSubTotalPasiva
            // 
            this.cSubTotalPasiva.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.cSubTotalPasiva.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "fnBalanceEND")});
            this.cSubTotalPasiva.Name = "cSubTotalPasiva";
            this.cSubTotalPasiva.StylePriority.UseBorders = false;
            this.cSubTotalPasiva.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0:n2}";
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.cSubTotalPasiva.Summary = xrSummary1;
            this.cSubTotalPasiva.Text = "cSubTotalPasiva";
            this.cSubTotalPasiva.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.cSubTotalPasiva.Weight = 1.1149120940140607D;
            // 
            // fnBalanceEND
            // 
            this.fnBalanceEND.Expression = "Iif([AccountLevel]=1, [BalanceEND], 0)";
            this.fnBalanceEND.Name = "fnBalanceEND";
            // 
            // LNeracaBentukTDetailPasiva
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.GroupHeader1,
            this.GroupFooter1});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.fnBalanceEND});
            this.DataSource = this.bs;
            this.Margins = new System.Drawing.Printing.Margins(26, 50, 0, 12);
            this.PrintOnEmptyDataSource = false;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.tablePasiva)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private System.Windows.Forms.BindingSource bs;
        private DevExpress.XtraReports.UI.XRTable tablePasiva;
        private DevExpress.XtraReports.UI.XRTableRow rowTablePasiva;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRTableCell cAccountName;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel lblServiceUnit;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell cSubTotalPasiva;
        private DevExpress.XtraReports.UI.CalculatedField fnBalanceEND;
    }
}
