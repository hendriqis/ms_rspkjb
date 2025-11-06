namespace QIS.Medinfras.Report
{
    partial class LaboratoryResultRpt
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
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.nolab = new DevExpress.XtraReports.UI.CalculatedField();
            this.textIsVerified = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
            this.lblNoRM = new DevExpress.XtraReports.UI.XRLabel();
            this.lblNoReg = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblParamedic = new DevExpress.XtraReports.UI.XRLabel();
            this.lblGender = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAge = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPatientName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPrintDate = new DevExpress.XtraReports.UI.XRLabel();
            this.lblStreet = new DevExpress.XtraReports.UI.XRLabel();
            this.lblUnit = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRequestDate = new DevExpress.XtraReports.UI.XRLabel();
            this.lblNo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel44 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel45 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel47 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel48 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel49 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel50 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel51 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel53 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel54 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel55 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel56 = new DevExpress.XtraReports.UI.XRLabel();
            this.NilaiRujukan = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine1,
            this.xrTable1,
            this.xrPanel2});
            this.PageHeader.HeightF = 270.8333F;
            this.PageHeader.Controls.SetChildIndex(this.xrPanel2, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrTable1, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLine1, 0);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vLaboratoryResultDt);
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.Detail.HeightF = 29.16667F;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 37.5F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(137.5F, 12.5F);
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Tanggal Cetak Hasil";
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 237.5F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(750F, 25F);
            this.xrTable1.StylePriority.UseBorders = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3,
            this.xrTableCell6});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Test/ Jenis Pemeriksaan";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 2.625D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Hasil";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 1.625D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Nilai Rujukan";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell3.Weight = 1.875D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "Metode";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell6.Weight = 1.375D;
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 262.5F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(750F, 2F);
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel22,
            this.xrLabel20});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ItemGroupName1", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("ItemName1", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 66.66666F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel22
            // 
            this.xrLabel22.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ItemName1")});
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 37.5F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(100F, 25F);
            this.xrLabel22.Text = "xrLabel22";
            // 
            // xrLabel20
            // 
            this.xrLabel20.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ItemGroupName1")});
            this.xrLabel20.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 0F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(187.5F, 25F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.Text = "xrLabel20";
            // 
            // xrTable2
            // 
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(50F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(712.5F, 25F);
            this.xrTable2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTable2_BeforePrint);
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell7,
            this.xrTableCell11,
            this.xrTableCell5});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.BorderColor = System.Drawing.Color.Gray;
            this.xrTableCell4.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FractionName1")});
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseBorderColor = false;
            this.xrTableCell4.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell4.StylePriority.UseBorders = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "xrTableCell4";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell4.Weight = 1.2857142857142858D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.BorderColor = System.Drawing.Color.Gray;
            this.xrTableCell7.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrTableCell7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "MetricResultValue")});
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseBorderColor = false;
            this.xrTableCell7.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell7.StylePriority.UseBorders = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "xrTableCell7";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell7.Weight = 0.9285714285714286D;
            this.xrTableCell7.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell7_BeforePrint);
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.BorderColor = System.Drawing.Color.Gray;
            this.xrTableCell11.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrTableCell11.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NilaiRujukan")});
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseBorderColor = false;
            this.xrTableCell11.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell11.StylePriority.UseBorders = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "xrTableCell11";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell11.Weight = 1.0714285714285714D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.BorderColor = System.Drawing.Color.Gray;
            this.xrTableCell5.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dash;
            this.xrTableCell5.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseBorderColor = false;
            this.xrTableCell5.StylePriority.UseBorderDashStyle = false;
            this.xrTableCell5.StylePriority.UseBorders = false;
            this.xrTableCell5.Weight = 0.78571428571428581D;
            // 
            // nolab
            // 
            this.nolab.Expression = "Iif([TestOrderNo]=\'\', [TransactionNo] ,[TestOrderNo] )";
            this.nolab.Name = "nolab";
            // 
            // textIsVerified
            // 
            this.textIsVerified.DataSource = this.bs;
            this.textIsVerified.Expression = "Iif([IsVerified],\' \',\'NOT VERIFIED\')";
            this.textIsVerified.Name = "textIsVerified";
            // 
            // xrPanel2
            // 
            this.xrPanel2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblNoRM,
            this.lblNoReg,
            this.xrLabel25,
            this.xrLabel24,
            this.lblParamedic,
            this.lblGender,
            this.lblAge,
            this.lblPatientName,
            this.lblPrintDate,
            this.lblStreet,
            this.lblUnit,
            this.lblRequestDate,
            this.lblNo,
            this.xrLabel37,
            this.xrLabel38,
            this.xrLabel39,
            this.xrLabel40,
            this.xrLabel41,
            this.xrLabel42,
            this.xrLabel43,
            this.xrLabel44,
            this.xrLabel45,
            this.xrLabel46,
            this.xrLabel47,
            this.xrLabel48,
            this.xrLabel49,
            this.xrLabel50,
            this.xrLabel51,
            this.xrLabel52,
            this.xrLabel53,
            this.xrLabel54,
            this.xrLabel55,
            this.xrLabel56});
            this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 112.5F);
            this.xrPanel2.Name = "xrPanel2";
            this.xrPanel2.SizeF = new System.Drawing.SizeF(750F, 112.5F);
            this.xrPanel2.StylePriority.UseBorders = false;
            // 
            // lblNoRM
            // 
            this.lblNoRM.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblNoRM.LocationFloat = new DevExpress.Utils.PointFloat(150F, 37.5F);
            this.lblNoRM.Name = "lblNoRM";
            this.lblNoRM.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblNoRM.SizeF = new System.Drawing.SizeF(175F, 12.5F);
            this.lblNoRM.StylePriority.UseBorders = false;
            this.lblNoRM.Text = "lblNoRM";
            // 
            // lblNoReg
            // 
            this.lblNoReg.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblNoReg.LocationFloat = new DevExpress.Utils.PointFloat(150F, 25F);
            this.lblNoReg.Name = "lblNoReg";
            this.lblNoReg.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblNoReg.SizeF = new System.Drawing.SizeF(175F, 12.5F);
            this.lblNoReg.StylePriority.UseBorders = false;
            this.lblNoReg.Text = "lblNoReg";
            // 
            // xrLabel25
            // 
            this.xrLabel25.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel25.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(500F, 50F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(12.5F, 12.5F);
            this.xrLabel25.StylePriority.UseBorders = false;
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.Text = ":";
            // 
            // xrLabel24
            // 
            this.xrLabel24.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel24.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 37.5F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(112.5F, 12.5F);
            this.xrLabel24.StylePriority.UseBorders = false;
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.Text = "No. Rekam Medis";
            // 
            // lblParamedic
            // 
            this.lblParamedic.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblParamedic.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParamedic.LocationFloat = new DevExpress.Utils.PointFloat(525F, 50F);
            this.lblParamedic.Name = "lblParamedic";
            this.lblParamedic.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblParamedic.SizeF = new System.Drawing.SizeF(212.5F, 12.5F);
            this.lblParamedic.StylePriority.UseBorders = false;
            this.lblParamedic.StylePriority.UseFont = false;
            // 
            // lblGender
            // 
            this.lblGender.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblGender.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGender.LocationFloat = new DevExpress.Utils.PointFloat(150F, 75F);
            this.lblGender.Name = "lblGender";
            this.lblGender.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblGender.SizeF = new System.Drawing.SizeF(175F, 12.5F);
            this.lblGender.StylePriority.UseBorders = false;
            this.lblGender.StylePriority.UseFont = false;
            // 
            // lblAge
            // 
            this.lblAge.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblAge.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAge.LocationFloat = new DevExpress.Utils.PointFloat(150F, 62.5F);
            this.lblAge.Name = "lblAge";
            this.lblAge.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblAge.SizeF = new System.Drawing.SizeF(175F, 12.5F);
            this.lblAge.StylePriority.UseBorders = false;
            this.lblAge.StylePriority.UseFont = false;
            // 
            // lblPatientName
            // 
            this.lblPatientName.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPatientName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientName.LocationFloat = new DevExpress.Utils.PointFloat(150F, 50F);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblPatientName.SizeF = new System.Drawing.SizeF(175F, 12.5F);
            this.lblPatientName.StylePriority.UseBorders = false;
            this.lblPatientName.StylePriority.UseFont = false;
            // 
            // lblPrintDate
            // 
            this.lblPrintDate.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblPrintDate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPrintDate.LocationFloat = new DevExpress.Utils.PointFloat(525F, 37.5F);
            this.lblPrintDate.Name = "lblPrintDate";
            this.lblPrintDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblPrintDate.SizeF = new System.Drawing.SizeF(175F, 12.5F);
            this.lblPrintDate.StylePriority.UseBorders = false;
            this.lblPrintDate.StylePriority.UseFont = false;
            // 
            // lblStreet
            // 
            this.lblStreet.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblStreet.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStreet.LocationFloat = new DevExpress.Utils.PointFloat(150F, 87.5F);
            this.lblStreet.Name = "lblStreet";
            this.lblStreet.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblStreet.SizeF = new System.Drawing.SizeF(225F, 12.5F);
            this.lblStreet.StylePriority.UseBorders = false;
            this.lblStreet.StylePriority.UseFont = false;
            // 
            // lblUnit
            // 
            this.lblUnit.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblUnit.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnit.LocationFloat = new DevExpress.Utils.PointFloat(525F, 12.5F);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblUnit.SizeF = new System.Drawing.SizeF(137.5F, 12.5F);
            this.lblUnit.StylePriority.UseBorders = false;
            this.lblUnit.StylePriority.UseFont = false;
            // 
            // lblRequestDate
            // 
            this.lblRequestDate.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblRequestDate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRequestDate.LocationFloat = new DevExpress.Utils.PointFloat(525F, 25F);
            this.lblRequestDate.Name = "lblRequestDate";
            this.lblRequestDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblRequestDate.SizeF = new System.Drawing.SizeF(175F, 12.5F);
            this.lblRequestDate.StylePriority.UseBorders = false;
            this.lblRequestDate.StylePriority.UseFont = false;
            // 
            // lblNo
            // 
            this.lblNo.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblNo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNo.LocationFloat = new DevExpress.Utils.PointFloat(150F, 12.5F);
            this.lblNo.Name = "lblNo";
            this.lblNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblNo.SizeF = new System.Drawing.SizeF(175F, 12.5F);
            this.lblNo.StylePriority.UseBorders = false;
            this.lblNo.StylePriority.UseFont = false;
            this.lblNo.Text = "lblNo";
            // 
            // xrLabel37
            // 
            this.xrLabel37.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel37.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(500F, 37.5F);
            this.xrLabel37.Name = "xrLabel37";
            this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel37.SizeF = new System.Drawing.SizeF(12.5F, 12.5F);
            this.xrLabel37.StylePriority.UseBorders = false;
            this.xrLabel37.StylePriority.UseFont = false;
            this.xrLabel37.Text = ":";
            // 
            // xrLabel38
            // 
            this.xrLabel38.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel38.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(125F, 12.5F);
            this.xrLabel38.Name = "xrLabel38";
            this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel38.SizeF = new System.Drawing.SizeF(12.5F, 12.5F);
            this.xrLabel38.StylePriority.UseBorders = false;
            this.xrLabel38.StylePriority.UseFont = false;
            this.xrLabel38.Text = ":";
            // 
            // xrLabel39
            // 
            this.xrLabel39.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel39.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(125F, 37.5F);
            this.xrLabel39.Name = "xrLabel39";
            this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel39.SizeF = new System.Drawing.SizeF(12.5F, 12.5F);
            this.xrLabel39.StylePriority.UseBorders = false;
            this.xrLabel39.StylePriority.UseFont = false;
            this.xrLabel39.Text = ":";
            // 
            // xrLabel40
            // 
            this.xrLabel40.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel40.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(125F, 50F);
            this.xrLabel40.Name = "xrLabel40";
            this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel40.SizeF = new System.Drawing.SizeF(12.5F, 12.5F);
            this.xrLabel40.StylePriority.UseBorders = false;
            this.xrLabel40.StylePriority.UseFont = false;
            this.xrLabel40.Text = ":";
            // 
            // xrLabel41
            // 
            this.xrLabel41.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel41.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(125F, 87.5F);
            this.xrLabel41.Name = "xrLabel41";
            this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel41.SizeF = new System.Drawing.SizeF(12.5F, 12.5F);
            this.xrLabel41.StylePriority.UseBorders = false;
            this.xrLabel41.StylePriority.UseFont = false;
            this.xrLabel41.Text = ":";
            // 
            // xrLabel42
            // 
            this.xrLabel42.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel42.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(500F, 12.5F);
            this.xrLabel42.Name = "xrLabel42";
            this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel42.SizeF = new System.Drawing.SizeF(12.5F, 12.5F);
            this.xrLabel42.StylePriority.UseBorders = false;
            this.xrLabel42.StylePriority.UseFont = false;
            this.xrLabel42.Text = ":";
            // 
            // xrLabel43
            // 
            this.xrLabel43.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel43.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(500F, 25F);
            this.xrLabel43.Name = "xrLabel43";
            this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel43.SizeF = new System.Drawing.SizeF(12.5F, 12.5F);
            this.xrLabel43.StylePriority.UseBorders = false;
            this.xrLabel43.StylePriority.UseFont = false;
            this.xrLabel43.Text = ":";
            // 
            // xrLabel44
            // 
            this.xrLabel44.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel44.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel44.LocationFloat = new DevExpress.Utils.PointFloat(125F, 62.5F);
            this.xrLabel44.Name = "xrLabel44";
            this.xrLabel44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel44.SizeF = new System.Drawing.SizeF(12.5F, 12.5F);
            this.xrLabel44.StylePriority.UseBorders = false;
            this.xrLabel44.StylePriority.UseFont = false;
            this.xrLabel44.Text = ":";
            // 
            // xrLabel45
            // 
            this.xrLabel45.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel45.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel45.LocationFloat = new DevExpress.Utils.PointFloat(125F, 75F);
            this.xrLabel45.Name = "xrLabel45";
            this.xrLabel45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel45.SizeF = new System.Drawing.SizeF(12.5F, 12.5F);
            this.xrLabel45.StylePriority.UseBorders = false;
            this.xrLabel45.StylePriority.UseFont = false;
            this.xrLabel45.Text = ":";
            // 
            // xrLabel46
            // 
            this.xrLabel46.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel46.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(125F, 25F);
            this.xrLabel46.Name = "xrLabel46";
            this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel46.SizeF = new System.Drawing.SizeF(12.5F, 12.5F);
            this.xrLabel46.StylePriority.UseBorders = false;
            this.xrLabel46.StylePriority.UseFont = false;
            this.xrLabel46.Text = ":";
            // 
            // xrLabel47
            // 
            this.xrLabel47.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel47.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel47.LocationFloat = new DevExpress.Utils.PointFloat(375F, 50F);
            this.xrLabel47.Name = "xrLabel47";
            this.xrLabel47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel47.SizeF = new System.Drawing.SizeF(125F, 12.5F);
            this.xrLabel47.StylePriority.UseBorders = false;
            this.xrLabel47.StylePriority.UseFont = false;
            this.xrLabel47.Text = "Dr. Pengirim";
            // 
            // xrLabel48
            // 
            this.xrLabel48.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel48.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel48.LocationFloat = new DevExpress.Utils.PointFloat(375F, 12.5F);
            this.xrLabel48.Name = "xrLabel48";
            this.xrLabel48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel48.SizeF = new System.Drawing.SizeF(125F, 12.5F);
            this.xrLabel48.StylePriority.UseBorders = false;
            this.xrLabel48.StylePriority.UseFont = false;
            this.xrLabel48.Text = "Unit";
            // 
            // xrLabel49
            // 
            this.xrLabel49.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel49.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel49.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 50F);
            this.xrLabel49.Name = "xrLabel49";
            this.xrLabel49.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel49.SizeF = new System.Drawing.SizeF(112.5F, 12.5F);
            this.xrLabel49.StylePriority.UseBorders = false;
            this.xrLabel49.StylePriority.UseFont = false;
            this.xrLabel49.Text = "Nama Pasien";
            // 
            // xrLabel50
            // 
            this.xrLabel50.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel50.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel50.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 62.5F);
            this.xrLabel50.Name = "xrLabel50";
            this.xrLabel50.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel50.SizeF = new System.Drawing.SizeF(112.5F, 12.5F);
            this.xrLabel50.StylePriority.UseBorders = false;
            this.xrLabel50.StylePriority.UseFont = false;
            this.xrLabel50.Text = "Umur";
            // 
            // xrLabel51
            // 
            this.xrLabel51.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel51.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel51.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 75F);
            this.xrLabel51.Name = "xrLabel51";
            this.xrLabel51.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel51.SizeF = new System.Drawing.SizeF(112.5F, 12.5F);
            this.xrLabel51.StylePriority.UseBorders = false;
            this.xrLabel51.StylePriority.UseFont = false;
            this.xrLabel51.Text = "Jenis Kelamin";
            // 
            // xrLabel52
            // 
            this.xrLabel52.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel52.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel52.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 25F);
            this.xrLabel52.Name = "xrLabel52";
            this.xrLabel52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel52.SizeF = new System.Drawing.SizeF(112.5F, 12.5F);
            this.xrLabel52.StylePriority.UseBorders = false;
            this.xrLabel52.StylePriority.UseFont = false;
            this.xrLabel52.Text = "No. Registrasi";
            // 
            // xrLabel53
            // 
            this.xrLabel53.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel53.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel53.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 87.5F);
            this.xrLabel53.Name = "xrLabel53";
            this.xrLabel53.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel53.SizeF = new System.Drawing.SizeF(112.5F, 12.5F);
            this.xrLabel53.StylePriority.UseBorders = false;
            this.xrLabel53.StylePriority.UseFont = false;
            this.xrLabel53.Text = "Alamat";
            // 
            // xrLabel54
            // 
            this.xrLabel54.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel54.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel54.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 12.5F);
            this.xrLabel54.Name = "xrLabel54";
            this.xrLabel54.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel54.SizeF = new System.Drawing.SizeF(112.5F, 12.5F);
            this.xrLabel54.StylePriority.UseBorders = false;
            this.xrLabel54.StylePriority.UseFont = false;
            this.xrLabel54.Text = "Nomor";
            // 
            // xrLabel55
            // 
            this.xrLabel55.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel55.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel55.LocationFloat = new DevExpress.Utils.PointFloat(375F, 37.5F);
            this.xrLabel55.Name = "xrLabel55";
            this.xrLabel55.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel55.SizeF = new System.Drawing.SizeF(125F, 12.5F);
            this.xrLabel55.StylePriority.UseBorders = false;
            this.xrLabel55.StylePriority.UseFont = false;
            this.xrLabel55.Text = "Tanggal Cetak";
            // 
            // xrLabel56
            // 
            this.xrLabel56.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel56.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel56.LocationFloat = new DevExpress.Utils.PointFloat(375F, 25F);
            this.xrLabel56.Name = "xrLabel56";
            this.xrLabel56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel56.SizeF = new System.Drawing.SizeF(125F, 12.5F);
            this.xrLabel56.StylePriority.UseBorders = false;
            this.xrLabel56.StylePriority.UseFont = false;
            this.xrLabel56.Text = "Tanggal Permintaan";
            // 
            // NilaiRujukan
            // 
            this.NilaiRujukan.DataSource = this.bs;
            this.NilaiRujukan.DisplayName = "NilaiRujukan";
            this.NilaiRujukan.Expression = "Iif(MetricUnit=\'\',\'N/A\' ,[NilaiRujukanMetric] )";
            this.NilaiRujukan.Name = "NilaiRujukan";
            // 
            // LaboratoryResultRpt
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.PageHeader,
            this.PageFooter,
            this.Detail,
            this.GroupHeader1,
            this.TopMargin});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.nolab,
            this.textIsVerified,
            this.NilaiRujukan});
            this.Margins = new System.Drawing.Printing.Margins(25, 24, 100, 100);
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.GroupHeader1, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.PageHeader, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell11;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel22;
        private DevExpress.XtraReports.UI.XRLabel xrLabel20;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.CalculatedField nolab;
        private DevExpress.XtraReports.UI.CalculatedField textIsVerified;
        private DevExpress.XtraReports.UI.XRPanel xrPanel2;
        private DevExpress.XtraReports.UI.XRLabel lblNoRM;
        private DevExpress.XtraReports.UI.XRLabel lblNoReg;
        private DevExpress.XtraReports.UI.XRLabel xrLabel25;
        private DevExpress.XtraReports.UI.XRLabel xrLabel24;
        private DevExpress.XtraReports.UI.XRLabel lblParamedic;
        private DevExpress.XtraReports.UI.XRLabel lblGender;
        private DevExpress.XtraReports.UI.XRLabel lblAge;
        private DevExpress.XtraReports.UI.XRLabel lblPatientName;
        private DevExpress.XtraReports.UI.XRLabel lblPrintDate;
        private DevExpress.XtraReports.UI.XRLabel lblStreet;
        private DevExpress.XtraReports.UI.XRLabel lblUnit;
        private DevExpress.XtraReports.UI.XRLabel lblRequestDate;
        private DevExpress.XtraReports.UI.XRLabel lblNo;
        private DevExpress.XtraReports.UI.XRLabel xrLabel37;
        private DevExpress.XtraReports.UI.XRLabel xrLabel38;
        private DevExpress.XtraReports.UI.XRLabel xrLabel39;
        private DevExpress.XtraReports.UI.XRLabel xrLabel40;
        private DevExpress.XtraReports.UI.XRLabel xrLabel41;
        private DevExpress.XtraReports.UI.XRLabel xrLabel42;
        private DevExpress.XtraReports.UI.XRLabel xrLabel43;
        private DevExpress.XtraReports.UI.XRLabel xrLabel44;
        private DevExpress.XtraReports.UI.XRLabel xrLabel45;
        private DevExpress.XtraReports.UI.XRLabel xrLabel46;
        private DevExpress.XtraReports.UI.XRLabel xrLabel47;
        private DevExpress.XtraReports.UI.XRLabel xrLabel48;
        private DevExpress.XtraReports.UI.XRLabel xrLabel49;
        private DevExpress.XtraReports.UI.XRLabel xrLabel50;
        private DevExpress.XtraReports.UI.XRLabel xrLabel51;
        private DevExpress.XtraReports.UI.XRLabel xrLabel52;
        private DevExpress.XtraReports.UI.XRLabel xrLabel53;
        private DevExpress.XtraReports.UI.XRLabel xrLabel54;
        private DevExpress.XtraReports.UI.XRLabel xrLabel55;
        private DevExpress.XtraReports.UI.XRLabel xrLabel56;
        private DevExpress.XtraReports.UI.CalculatedField NilaiRujukan;
    }
}
