namespace QIS.Medinfras.ReportDesktop
{
    partial class LKontrolKelengkapanSEPPasienBPJS
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
            this.lblPeriod = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell28 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1,
            this.lblPeriod});
            this.PageHeader.HeightF = 436.5625F;
            this.PageHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblPhoneFaxNo, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblReportSubtitle, 0);
            this.PageHeader.Controls.SetChildIndex(this.lblPeriod, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrTable1, 0);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vRegistrationBPJS);
            // 
            // lblReportParameter
            // 
            this.lblReportParameter.StylePriority.UseFont = false;
            // 
            // lblPhoneFaxNo
            // 
            this.lblPhoneFaxNo.StylePriority.UseFont = false;
            this.lblPhoneFaxNo.StylePriority.UsePadding = false;
            this.lblPhoneFaxNo.StylePriority.UseTextAlignment = false;
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            // 
            // lblReportSubtitle
            // 
            this.lblReportSubtitle.StylePriority.UseFont = false;
            this.lblReportSubtitle.StylePriority.UseTextAlignment = false;
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
            this.Detail.HeightF = 31.75F;
            // 
            // lblPeriod
            // 
            this.lblPeriod.Dpi = 254F;
            this.lblPeriod.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblPeriod.LocationFloat = new DevExpress.Utils.PointFloat(0F, 285.75F);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPeriod.SizeF = new System.Drawing.SizeF(2825.75F, 31.75F);
            this.lblPeriod.StylePriority.UseFont = false;
            this.lblPeriod.StylePriority.UseTextAlignment = false;
            this.lblPeriod.Text = "Periode : [StartDate!dd-MMM-yyyy] s/d  [EndDate!dd-MMM-yyyy] ";
            this.lblPeriod.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Dpi = 254F;
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 349.25F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(2825.75F, 87.3125F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell4,
            this.xrTableCell2,
            this.xrTableCell15,
            this.xrTableCell26,
            this.xrTableCell28,
            this.xrTableCell5,
            this.xrTableCell3,
            this.xrTableCell6,
            this.xrTableCell8});
            this.xrTableRow1.Dpi = 254F;
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "No.";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell1.Weight = 0.10112359685559488D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell4.StylePriority.UsePadding = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "No. Registrasi";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell4.Weight = 0.37078653170367859D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "No. RM";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.23595505617977539D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell15.StylePriority.UsePadding = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.Text = "Nama Pasien";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell15.Weight = 0.47191011505950431D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.Dpi = 254F;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            this.xrTableCell26.Text = "Tgl Registrasi";
            this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell26.Weight = 0.20224719236121289D;
            // 
            // xrTableCell28
            // 
            this.xrTableCell28.Dpi = 254F;
            this.xrTableCell28.Multiline = true;
            this.xrTableCell28.Name = "xrTableCell28";
            this.xrTableCell28.StylePriority.UseTextAlignment = false;
            this.xrTableCell28.Text = "Tgl \r\nPulang";
            this.xrTableCell28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell28.Weight = 0.2022471916862244D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "Departemen";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell5.Weight = 0.26966292202330311D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell3.StylePriority.UsePadding = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Dokter";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell3.Weight = 0.50561795322850511D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.Multiline = true;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.xrTableCell6.StylePriority.UsePadding = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "No SEP";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell6.Weight = 0.40449438337244881D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Dpi = 254F;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = "Kelas";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell8.Weight = 0.23595505752975227D;
            // 
            // xrTable2
            // 
            this.xrTable2.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Dot;
            this.xrTable2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable2.Dpi = 254F;
            this.xrTable2.Font = new System.Drawing.Font("Tahoma", 9F);
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(2825.75F, 31.75F);
            this.xrTable2.StylePriority.UseBorderDashStyle = false;
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.xrTableCell9,
            this.xrTableCell11,
            this.xrTableCell12,
            this.xrTableCell25,
            this.xrTableCell17,
            this.xrTableCell13,
            this.xrTableCell14,
            this.xrTableCell16,
            this.xrTableCell10});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell7.Dpi = 254F;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseBorders = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrTableCell7.Summary = xrSummary1;
            this.xrTableCell7.Text = "No.";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell7.Weight = 0.10112359685559488D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell9.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RegistrationNo")});
            this.xrTableCell9.Dpi = 254F;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell9.StylePriority.UseBorders = false;
            this.xrTableCell9.StylePriority.UsePadding = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "xrTableCell9";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell9.Weight = 0.37078653170367859D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "MedicalNo")});
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.xrTableCell11.StylePriority.UseBorders = false;
            this.xrTableCell11.StylePriority.UsePadding = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "No. RM";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell11.Weight = 0.23595505617977539D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PatientName")});
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell12.StylePriority.UseBorders = false;
            this.xrTableCell12.StylePriority.UsePadding = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "Nama Pasien";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell12.Weight = 0.4719100813100815D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell25.Dpi = 254F;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell25.StylePriority.UseBorders = false;
            this.xrTableCell25.StylePriority.UsePadding = false;
            this.xrTableCell25.StylePriority.UseTextAlignment = false;
            this.xrTableCell25.Text = "[RegistrationDateInString]";
            this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell25.Weight = 0.20224719134873018D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell17.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "TanggalPulangInString")});
            this.xrTableCell17.Dpi = 254F;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.StylePriority.UseBorders = false;
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.Text = "xrTableCell17";
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell17.Weight = 0.20224719134873018D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell13.Dpi = 254F;
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseBorders = false;
            this.xrTableCell13.Text = "[DepartmentID]";
            this.xrTableCell13.Weight = 0.26966292202330311D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ParamedicName")});
            this.xrTableCell14.Dpi = 254F;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell14.StylePriority.UseBorders = false;
            this.xrTableCell14.StylePriority.UsePadding = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.Text = "Dokter";
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell14.Weight = 0.50561798832790483D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NoSEP")});
            this.xrTableCell16.Dpi = 254F;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            this.xrTableCell16.StylePriority.UseBorders = false;
            this.xrTableCell16.StylePriority.UsePadding = false;
            this.xrTableCell16.StylePriority.UseTextAlignment = false;
            this.xrTableCell16.Text = "xrTableCell16";
            this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell16.Weight = 0.40449438337244892D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
                        | DevExpress.XtraPrinting.BorderSide.Right)
                        | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseBorders = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "[ClassName]";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell10.Weight = 0.23595505752975224D;
            // 
            // LKontrolKelengkapanSEPPasienBPJS
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.PageHeader,
            this.PageFooter,
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRLabel lblPeriod;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell15;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell26;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell28;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell8;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell9;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell11;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell25;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell17;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell13;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell14;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell16;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
    }
}
