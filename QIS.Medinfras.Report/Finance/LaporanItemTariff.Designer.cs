namespace QIS.Medinfras.Report
{
    partial class LaporanItemTariff
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
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdCol1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdCol2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdCol3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdCol4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdCol5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdCol6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdCol7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdCol8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.Col1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.Col2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.Col3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.Col4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.Col5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.Col6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.Col7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.Col8 = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.PageHeader.HeightF = 150F;
            this.PageHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrTable1, 0);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vItemTariffCustom);
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            // 
            // xrPageInfo3
            // 
            this.xrPageInfo3.StylePriority.UseFont = false;
            this.xrPageInfo3.StylePriority.UseTextAlignment = false;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 12.5F;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ItemGroupID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 13.54167F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ItemGroupName1")});
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(25F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(475F, 12.5F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "xrLabel2";
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(12.50001F, 137.5F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(762.5F, 12.5F);
            this.xrTable1.StylePriority.UseBorders = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell2,
            this.hdCol1,
            this.hdCol2,
            this.hdCol3,
            this.hdCol4,
            this.hdCol5,
            this.hdCol6,
            this.hdCol7,
            this.hdCol8});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UsePadding = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "No";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell4.Weight = 0.25000000018458168D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Item Name";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 3.2499999573615295D;
            // 
            // hdCol1
            // 
            this.hdCol1.Name = "hdCol1";
            this.hdCol1.StylePriority.UseTextAlignment = false;
            this.hdCol1.Text = "C1";
            this.hdCol1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.hdCol1.Weight = 1.0000000009229106D;
            // 
            // hdCol2
            // 
            this.hdCol2.Name = "hdCol2";
            this.hdCol2.StylePriority.UseTextAlignment = false;
            this.hdCol2.Text = "C2";
            this.hdCol2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.hdCol2.Weight = 0.37500000138436596D;
            // 
            // hdCol3
            // 
            this.hdCol3.Name = "hdCol3";
            this.hdCol3.StylePriority.UseTextAlignment = false;
            this.hdCol3.Text = "C3";
            this.hdCol3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.hdCol3.Weight = 0.4999999983849065D;
            // 
            // hdCol4
            // 
            this.hdCol4.Name = "hdCol4";
            this.hdCol4.StylePriority.UseTextAlignment = false;
            this.hdCol4.Text = "C4";
            this.hdCol4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.hdCol4.Weight = 0.49999999734663192D;
            // 
            // hdCol5
            // 
            this.hdCol5.Name = "hdCol5";
            this.hdCol5.StylePriority.UseTextAlignment = false;
            this.hdCol5.Text = "C5";
            this.hdCol5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.hdCol5.Weight = 0.49999999682749474D;
            // 
            // hdCol6
            // 
            this.hdCol6.Name = "hdCol6";
            this.hdCol6.StylePriority.UseTextAlignment = false;
            this.hdCol6.Text = "C6";
            this.hdCol6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.hdCol6.Weight = 0.37499999749083668D;
            // 
            // hdCol7
            // 
            this.hdCol7.Name = "hdCol7";
            this.hdCol7.StylePriority.UseTextAlignment = false;
            this.hdCol7.Text = "C7";
            this.hdCol7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.hdCol7.Weight = 0.49999999643814175D;
            // 
            // hdCol8
            // 
            this.hdCol8.Name = "hdCol8";
            this.hdCol8.StylePriority.UseTextAlignment = false;
            this.hdCol8.Text = "C8";
            this.hdCol8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.hdCol8.Weight = 0.37499999736105238D;
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(762.5F, 12.5F);
            this.xrTable2.StylePriority.UseBorders = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5,
            this.xrTableCell7,
            this.Col1,
            this.Col2,
            this.Col3,
            this.Col4,
            this.Col5,
            this.Col6,
            this.Col7,
            this.Col8});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrTableCell5.Summary = xrSummary1;
            this.xrTableCell5.Text = "No";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell5.Weight = 0.25D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ItemName1")});
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "xrTableCell7";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell7.Weight = 3.25D;
            // 
            // Col1
            // 
            this.Col1.Name = "Col1";
            this.Col1.StylePriority.UseTextAlignment = false;
            this.Col1.Text = "C1";
            this.Col1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.Col1.Weight = 0.953125D;
            // 
            // Col2
            // 
            this.Col2.Name = "Col2";
            this.Col2.StylePriority.UseTextAlignment = false;
            this.Col2.Text = "C2";
            this.Col2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.Col2.Weight = 0.453125D;
            // 
            // Col3
            // 
            this.Col3.Name = "Col3";
            this.Col3.StylePriority.UseTextAlignment = false;
            this.Col3.Text = "C3";
            this.Col3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.Col3.Weight = 0.453125D;
            // 
            // Col4
            // 
            this.Col4.Name = "Col4";
            this.Col4.StylePriority.UseTextAlignment = false;
            this.Col4.Text = "C4";
            this.Col4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.Col4.Weight = 0.453125D;
            // 
            // Col5
            // 
            this.Col5.Name = "Col5";
            this.Col5.StylePriority.UseTextAlignment = false;
            this.Col5.Text = "C5";
            this.Col5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.Col5.Weight = 0.453125D;
            // 
            // Col6
            // 
            this.Col6.Name = "Col6";
            this.Col6.StylePriority.UseTextAlignment = false;
            this.Col6.Text = "C6";
            this.Col6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.Col6.Weight = 0.453125D;
            // 
            // Col7
            // 
            this.Col7.Name = "Col7";
            this.Col7.StylePriority.UseTextAlignment = false;
            this.Col7.Text = "C7";
            this.Col7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.Col7.Weight = 0.453125D;
            // 
            // Col8
            // 
            this.Col8.Name = "Col8";
            this.Col8.StylePriority.UseTextAlignment = false;
            this.Col8.Text = "C8";
            this.Col8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.Col8.Weight = 0.453125D;
            // 
            // LaporanItemTariff
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.PageHeader,
            this.PageFooter,
            this.Detail,
            this.TopMargin,
            this.GroupHeader1,
            this.BottomMargin,
            this.ReportHeader});
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.GroupHeader1, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.PageHeader, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell hdCol1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell Col1;
        private DevExpress.XtraReports.UI.XRTableCell hdCol2;
        private DevExpress.XtraReports.UI.XRTableCell hdCol3;
        private DevExpress.XtraReports.UI.XRTableCell hdCol4;
        private DevExpress.XtraReports.UI.XRTableCell hdCol5;
        private DevExpress.XtraReports.UI.XRTableCell hdCol6;
        private DevExpress.XtraReports.UI.XRTableCell hdCol7;
        private DevExpress.XtraReports.UI.XRTableCell hdCol8;
        private DevExpress.XtraReports.UI.XRTableCell Col2;
        private DevExpress.XtraReports.UI.XRTableCell Col3;
        private DevExpress.XtraReports.UI.XRTableCell Col4;
        private DevExpress.XtraReports.UI.XRTableCell Col5;
        private DevExpress.XtraReports.UI.XRTableCell Col6;
        private DevExpress.XtraReports.UI.XRTableCell Col7;
        private DevExpress.XtraReports.UI.XRTableCell Col8;

    }
}
