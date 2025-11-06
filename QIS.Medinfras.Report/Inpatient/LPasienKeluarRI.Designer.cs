namespace QIS.Medinfras.Report
{
    partial class LPasienKeluarRI
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
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel29,
            this.xrLabel28,
            this.xrLabel31,
            this.xrLabel30,
            this.xrLabel2,
            this.xrLabel23,
            this.xrLabel25,
            this.xrLabel20,
            this.xrLabel21,
            this.xrLine1,
            this.xrLabel22,
            this.xrLabel3});
            this.PageHeader.HeightF = 162.5F;
            this.PageHeader.Controls.SetChildIndex(this.xrLabel3, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel22, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLine1, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel21, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel20, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel25, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel23, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel2, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel30, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel31, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel28, 0);
            this.PageHeader.Controls.SetChildIndex(this.xrLabel29, 0);
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vConsultVisit);
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel24,
            this.xrLabel27,
            this.xrLabel26,
            this.xrLabel15,
            this.xrLabel13,
            this.xrLabel10,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel18,
            this.xrLabel16});
            this.Detail.HeightF = 12.5F;
            // 
            // xrLabel20
            // 
            this.xrLabel20.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(0F, 137.5F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(25F, 12.5F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.Text = "No";
            // 
            // xrLabel25
            // 
            this.xrLabel25.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ParamedicName")});
            this.xrLabel25.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(475F, 137.5F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(112.5F, 12.5F);
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.Text = "Dokter Utama";
            // 
            // xrLabel23
            // 
            this.xrLabel23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "MedicalNo")});
            this.xrLabel23.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(212.5F, 150F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(112.5F, 12.5F);
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.Text = "No RM/Jn Kel";
            // 
            // xrLabel21
            // 
            this.xrLabel21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RegistrationNo")});
            this.xrLabel21.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(25F, 137.5F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(87.5F, 12.5F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.Text = "Noreg";
            // 
            // xrLabel22
            // 
            this.xrLabel22.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PatientName")});
            this.xrLabel22.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(212.5F, 137.5F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(112.5F, 12.5F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.Text = "Nama";
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 125F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(1112.5F, 12.5F);
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 0F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(1112.5F, 12.5F);
            // 
            // xrLabel31
            // 
            this.xrLabel31.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(1012.5F, 137.5F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(100F, 12.5F);
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.Text = "Tindak Lanjut";
            // 
            // xrLabel28
            // 
            this.xrLabel28.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "BusinessPartnerName")});
            this.xrLabel28.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(600F, 137.5F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(175F, 12.5F);
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.Text = "Pembayar";
            // 
            // xrLabel29
            // 
            this.xrLabel29.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(787.5F, 137.5F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(100F, 12.5F);
            this.xrLabel29.StylePriority.UseFont = false;
            this.xrLabel29.Text = "Cara Keluar";
            // 
            // xrLabel30
            // 
            this.xrLabel30.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(900F, 137.5F);
            this.xrLabel30.Multiline = true;
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(100F, 12.5F);
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.Text = "Keadaan Keluar";
            // 
            // xrLabel1
            // 
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", this.bs, "RoomName")});
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(337.5F, 87.5F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(125F, 12.5F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Ruang / No TT";
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ClassName")});
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(337.5F, 150F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(125F, 12.5F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Kelas";
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "VisitDate")});
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(125F, 137.5F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(75F, 12.5F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "Tgl\r\nRegistrasi";
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLine2});
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("DischargeDate", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 40.625F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel6
            // 
            this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DischargeDate", "{0:dd-MMM-yy}")});
            this.xrLabel6.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(137.5F, 12.5F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(175F, 12.5F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "xrLabel6";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(125F, 12.5F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(12.5F, 12.5F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = ":";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 12.5F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(125F, 12.5F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "Tanggal Keluar";
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(212.5F, 0F);
            this.xrLabel10.Multiline = true;
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(112.5F, 12.5F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.Text = "[PatientName]\r\n[MedicalNo] / [Gender]";
            // 
            // xrLabel13
            // 
            this.xrLabel13.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(337.5F, 0F);
            this.xrLabel13.Multiline = true;
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(125F, 12.5F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.Text = "[WardName] / [BedNumber]\r\n[Class]";
            // 
            // xrLabel18
            // 
            this.xrLabel18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DischargeCondition")});
            this.xrLabel18.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(900F, 0F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(100F, 12.5F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.Text = "[DischargeCondition]";
            // 
            // xrLabel8
            // 
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RegistrationNo")});
            this.xrLabel8.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(25F, 0F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(87.5F, 12.5F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "Noreg";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(25F, 12.5F);
            this.xrLabel7.StylePriority.UseFont = false;
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel7.Summary = xrSummary1;
            this.xrLabel7.Text = "xrLabel4";
            // 
            // xrLabel26
            // 
            this.xrLabel26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Payer")});
            this.xrLabel26.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(600F, 0F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(175F, 12.5F);
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.Text = "xrLabel13";
            // 
            // xrLabel24
            // 
            this.xrLabel24.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(787.5F, 0F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(100F, 12.5F);
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.Text = "[DischargeMethod]";
            // 
            // xrLabel27
            // 
            this.xrLabel27.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "FollowUp")});
            this.xrLabel27.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(1012.5F, 0F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(100F, 12.5F);
            this.xrLabel27.StylePriority.UseFont = false;
            this.xrLabel27.Text = "xrLabel19";
            // 
            // xrLabel15
            // 
            this.xrLabel15.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DOctor")});
            this.xrLabel15.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(475F, 0F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(112.5F, 12.5F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.Text = "xrLabel12";
            // 
            // xrLabel16
            // 
            this.xrLabel16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RegistrationDate", "{0:dd-MMM-yy}")});
            this.xrLabel16.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(125F, 0F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(75F, 12.5F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.Text = "xrLabel16";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine3});
            this.GroupFooter1.HeightF = 2F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrLine3
            // 
            this.xrLine3.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrLine3.LineStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(1112.5F, 2F);
            this.xrLine3.StylePriority.UseBorderDashStyle = false;
            // 
            // LPasienKeluarRI
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.PageHeader,
            this.GroupHeader1,
            this.GroupFooter1,
            this.PageFooter,
            this.TopMargin});
            this.Margins = new System.Drawing.Printing.Margins(25, 26, 100, 100);
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.GroupFooter1, 0);
            this.Controls.SetChildIndex(this.GroupHeader1, 0);
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRLabel xrLabel29;
        private DevExpress.XtraReports.UI.XRLabel xrLabel28;
        private DevExpress.XtraReports.UI.XRLabel xrLabel31;
        private DevExpress.XtraReports.UI.XRLabel xrLabel30;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel23;
        private DevExpress.XtraReports.UI.XRLabel xrLabel25;
        private DevExpress.XtraReports.UI.XRLabel xrLabel20;
        private DevExpress.XtraReports.UI.XRLabel xrLabel21;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel22;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel24;
        private DevExpress.XtraReports.UI.XRLabel xrLabel27;
        private DevExpress.XtraReports.UI.XRLabel xrLabel26;
        private DevExpress.XtraReports.UI.XRLabel xrLabel15;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel18;
        private DevExpress.XtraReports.UI.XRLabel xrLabel16;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLine xrLine3;
    }
}
