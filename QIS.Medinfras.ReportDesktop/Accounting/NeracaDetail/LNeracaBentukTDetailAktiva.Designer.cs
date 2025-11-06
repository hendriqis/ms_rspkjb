namespace QIS.Medinfras.ReportDesktop
{
    partial class LNeracaBentukTDetailAktiva
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
            this.tableAktiva = new DevExpress.XtraReports.UI.XRTable();
            this.rowTableAktiva = new DevExpress.XtraReports.UI.XRTableRow();
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
            this.cSubTotalAktiva = new DevExpress.XtraReports.UI.XRTableCell();
            this.fnBalanceEND = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.tableAktiva)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.tableAktiva});
            this.Detail.HeightF = 15F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // tableAktiva
            // 
            this.tableAktiva.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.tableAktiva.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableAktiva.LocationFloat = new DevExpress.Utils.PointFloat(9.999983F, 0F);
            this.tableAktiva.Name = "tableAktiva";
            this.tableAktiva.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.tableAktiva.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.rowTableAktiva});
            this.tableAktiva.SizeF = new System.Drawing.SizeF(377.0835F, 15F);
            this.tableAktiva.StylePriority.UseBorders = false;
            this.tableAktiva.StylePriority.UseFont = false;
            this.tableAktiva.StylePriority.UsePadding = false;
            this.tableAktiva.StylePriority.UseTextAlignment = false;
            this.tableAktiva.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // rowTableAktiva
            // 
            this.rowTableAktiva.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cAccountName,
            this.xrTableCell10});
            this.rowTableAktiva.Name = "rowTableAktiva";
            this.rowTableAktiva.Weight = 1D;
            this.rowTableAktiva.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.rowTableAktiva_BeforePrint);
            // 
            // cAccountName
            // 
            this.cAccountName.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GLAccountName")});
            this.cAccountName.Name = "cAccountName";
            this.cAccountName.Text = "cAccountName";
            this.cAccountName.Weight = 1.9280284602477318D;
            this.cAccountName.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.cAccountName_BeforePrint);
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.xrTableCell10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "BalanceEND", "{0:n2}")});
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseBorders = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.xrTableCell10.Weight = 1.1091185573917266D;
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
            this.xrLabel1.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0.0001589457F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(387.0833F, 12.58333F);
            this.xrLabel1.StylePriority.UseBackColor = false;
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "ASET";
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
            this.lblServiceUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(193)))));
            this.lblServiceUnit.Borders = DevExpress.XtraPrinting.BorderSide.Right;
            this.lblServiceUnit.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "GLAccountType")});
            this.lblServiceUnit.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceUnit.ForeColor = System.Drawing.Color.Black;
            this.lblServiceUnit.LocationFloat = new DevExpress.Utils.PointFloat(0.0001589457F, 0F);
            this.lblServiceUnit.Name = "lblServiceUnit";
            this.lblServiceUnit.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 100F);
            this.lblServiceUnit.SizeF = new System.Drawing.SizeF(387.0833F, 23F);
            this.lblServiceUnit.StylePriority.UseBackColor = false;
            this.lblServiceUnit.StylePriority.UseBorders = false;
            this.lblServiceUnit.StylePriority.UseFont = false;
            this.lblServiceUnit.StylePriority.UseForeColor = false;
            this.lblServiceUnit.StylePriority.UsePadding = false;
            this.lblServiceUnit.StylePriority.UseTextAlignment = false;
            this.lblServiceUnit.Text = "[GLAccountType]";
            this.lblServiceUnit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.GroupFooter1.HeightF = 24.30556F;
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
            this.xrTable1.SizeF = new System.Drawing.SizeF(377.0834F, 15F);
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
            this.cSubTotalAktiva});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Text = "TOTAL [GLAccountType]";
            this.xrTableCell1.Weight = 1.9280279842204231D;
            // 
            // cSubTotalAktiva
            // 
            this.cSubTotalAktiva.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right)));
            this.cSubTotalAktiva.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "fnBalanceEND")});
            this.cSubTotalAktiva.Name = "cSubTotalAktiva";
            this.cSubTotalAktiva.StylePriority.UseBorders = false;
            this.cSubTotalAktiva.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0:n2}";
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.cSubTotalAktiva.Summary = xrSummary1;
            this.cSubTotalAktiva.Text = "cSubTotalAktiva";
            this.cSubTotalAktiva.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            this.cSubTotalAktiva.Weight = 1.1091188031897734D;
            // 
            // fnBalanceEND
            // 
            this.fnBalanceEND.Expression = "Iif([AccountLevel]=1, [BalanceEND], 0)";
            this.fnBalanceEND.Name = "fnBalanceEND";
            // 
            // LNeracaBentukTDetailAktiva
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
            ((System.ComponentModel.ISupportInitialize)(this.tableAktiva)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private System.Windows.Forms.BindingSource bs;
        private DevExpress.XtraReports.UI.XRTable tableAktiva;
        private DevExpress.XtraReports.UI.XRTableRow rowTableAktiva;
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
        private DevExpress.XtraReports.UI.XRTableCell cSubTotalAktiva;
        private DevExpress.XtraReports.UI.CalculatedField fnBalanceEND;
    }
}
