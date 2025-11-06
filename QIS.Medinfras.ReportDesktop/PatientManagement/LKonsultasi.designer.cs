namespace QIS.Medinfras.ReportDesktop
{
    partial class LKonsultasi
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
            this.lblLastUpdatedDate = new DevExpress.XtraReports.UI.XRLabel();
            this.lblParamedic = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.lblDate = new DevExpress.XtraReports.UI.XRLabel();
            this.cParamedicName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblNama = new DevExpress.XtraReports.UI.XRLabel();
            this.lblUmur = new DevExpress.XtraReports.UI.XRLabel();
            this.lblAlamat = new DevExpress.XtraReports.UI.XRLabel();
            this.lblGender = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRM = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDiagnosa = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // lblReportProperties1
            // 
            this.lblReportProperties1.StylePriority.UseFont = false;
            this.lblReportProperties1.StylePriority.UsePadding = false;
            this.lblReportProperties1.StylePriority.UseTextAlignment = false;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.vRegistration);
            // 
            // lblReportProperties2
            // 
            this.lblReportProperties2.SizeF = new System.Drawing.SizeF(923.7915F, 26.45835F);
            this.lblReportProperties2.StylePriority.UseFont = false;
            this.lblReportProperties2.StylePriority.UsePadding = false;
            this.lblReportProperties2.StylePriority.UseTextAlignment = false;
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.StylePriority.UseFont = false;
            this.lblReportTitle.StylePriority.UseTextAlignment = false;
            // 
            // tableHeader
            // 
            this.tableHeader.StylePriority.UseFont = false;
            this.tableHeader.StylePriority.UsePadding = false;
            this.tableHeader.StylePriority.UseTextAlignment = false;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 30F;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.cParamedicName,
            this.xrLabel3,
            this.xrLabel1});
            this.ReportHeader.HeightF = 333.9043F;
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel1, 0);
            this.ReportHeader.Controls.SetChildIndex(this.lblReportTitle, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLine2, 0);
            this.ReportHeader.Controls.SetChildIndex(this.tableHeader, 0);
            this.ReportHeader.Controls.SetChildIndex(this.xrLabel3, 0);
            this.ReportHeader.Controls.SetChildIndex(this.cParamedicName, 0);
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2,
            this.xrLabel11,
            this.xrLabel7,
            this.lblDiagnosa,
            this.xrLabel10,
            this.lblRM,
            this.xrLabel8,
            this.lblGender,
            this.lblAlamat,
            this.lblUmur,
            this.lblNama,
            this.xrTable1});
            this.Detail.HeightF = 483.2117F;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 30F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 74.08334F;
            // 
            // lblLastUpdatedDate
            // 
            this.lblLastUpdatedDate.Dpi = 254F;
            this.lblLastUpdatedDate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastUpdatedDate.LocationFloat = new DevExpress.Utils.PointFloat(25.00033F, 87.3125F);
            this.lblLastUpdatedDate.Name = "lblLastUpdatedDate";
            this.lblLastUpdatedDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblLastUpdatedDate.SizeF = new System.Drawing.SizeF(869.9991F, 31.74997F);
            this.lblLastUpdatedDate.StylePriority.UseFont = false;
            this.lblLastUpdatedDate.StylePriority.UseTextAlignment = false;
            this.lblLastUpdatedDate.Text = "Salama Sejawat,";
            this.lblLastUpdatedDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblParamedic
            // 
            this.lblParamedic.Dpi = 254F;
            this.lblParamedic.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParamedic.LocationFloat = new DevExpress.Utils.PointFloat(25.00033F, 202.5419F);
            this.lblParamedic.Name = "lblParamedic";
            this.lblParamedic.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblParamedic.SizeF = new System.Drawing.SizeF(869.9991F, 31.75005F);
            this.lblParamedic.StylePriority.UseFont = false;
            this.lblParamedic.StylePriority.UseTextAlignment = false;
            this.lblParamedic.Text = "lbldokrerDPJP";
            this.lblParamedic.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(25.00033F, 150.7067F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(869.9996F, 45.19084F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Kepada Yth";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(25.00033F, 254.3176F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(869.9996F, 79.58667F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Dengan Hormat,\r\nMohon pemeriksaan & pengobatan lebih lanjut ";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.Dpi = 254F;
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(24.99985F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow6,
            this.xrTableRow7});
            this.xrTable1.SizeF = new System.Drawing.SizeF(142.3955F, 127F);
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseTextAlignment = false;
            this.xrTable1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2});
            this.xrTableRow6.Dpi = 254F;
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Dpi = 254F;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 254F);
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Nama";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell1.Weight = 0.60768660891709914D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Dpi = 254F;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = ":";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell2.Weight = 0.093174653563980581D;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell4,
            this.xrTableCell5});
            this.xrTableRow7.Dpi = 254F;
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Dpi = 254F;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 254F);
            this.xrTableCell4.StylePriority.UsePadding = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "Umur";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell4.Weight = 0.60768660891709914D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = ":";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell5.Weight = 0.093174653563980581D;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(25.00033F, 0F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(869.9991F, 45.1908F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Terima kasih.";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblDate,
            this.lblLastUpdatedDate,
            this.lblParamedic,
            this.xrLabel4});
            this.GroupFooter1.Dpi = 254F;
            this.GroupFooter1.HeightF = 234.2919F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // lblDate
            // 
            this.lblDate.Dpi = 254F;
            this.lblDate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDate.LocationFloat = new DevExpress.Utils.PointFloat(25.00033F, 55.5625F);
            this.lblDate.Name = "lblDate";
            this.lblDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblDate.SizeF = new System.Drawing.SizeF(869.9991F, 31.75F);
            this.lblDate.StylePriority.UseFont = false;
            this.lblDate.StylePriority.UseTextAlignment = false;
            this.lblDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // cParamedicName
            // 
            this.cParamedicName.Dpi = 254F;
            this.cParamedicName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cParamedicName.LocationFloat = new DevExpress.Utils.PointFloat(25.00049F, 195.8975F);
            this.cParamedicName.Name = "cParamedicName";
            this.cParamedicName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.cParamedicName.SizeF = new System.Drawing.SizeF(869.9999F, 58.42003F);
            this.cParamedicName.StylePriority.UseFont = false;
            this.cParamedicName.StylePriority.UseTextAlignment = false;
            this.cParamedicName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblNama
            // 
            this.lblNama.Dpi = 254F;
            this.lblNama.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNama.LocationFloat = new DevExpress.Utils.PointFloat(167.3955F, 0F);
            this.lblNama.Name = "lblNama";
            this.lblNama.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblNama.SizeF = new System.Drawing.SizeF(727.6041F, 63.5F);
            this.lblNama.StylePriority.UseFont = false;
            this.lblNama.StylePriority.UseTextAlignment = false;
            this.lblNama.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblUmur
            // 
            this.lblUmur.Dpi = 254F;
            this.lblUmur.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUmur.LocationFloat = new DevExpress.Utils.PointFloat(167.3955F, 63.50003F);
            this.lblUmur.Name = "lblUmur";
            this.lblUmur.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblUmur.SizeF = new System.Drawing.SizeF(322.7924F, 63.50003F);
            this.lblUmur.StylePriority.UseFont = false;
            this.lblUmur.StylePriority.UseTextAlignment = false;
            this.lblUmur.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblAlamat
            // 
            this.lblAlamat.Dpi = 254F;
            this.lblAlamat.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlamat.LocationFloat = new DevExpress.Utils.PointFloat(167.3965F, 127.0001F);
            this.lblAlamat.Name = "lblAlamat";
            this.lblAlamat.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblAlamat.SizeF = new System.Drawing.SizeF(727.6044F, 63.49998F);
            this.lblAlamat.StylePriority.UseFont = false;
            this.lblAlamat.StylePriority.UseTextAlignment = false;
            this.lblAlamat.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblGender
            // 
            this.lblGender.Dpi = 254F;
            this.lblGender.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGender.LocationFloat = new DevExpress.Utils.PointFloat(490.188F, 63.50003F);
            this.lblGender.Name = "lblGender";
            this.lblGender.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblGender.SizeF = new System.Drawing.SizeF(79.37497F, 63.50003F);
            this.lblGender.StylePriority.UseFont = false;
            this.lblGender.StylePriority.UseTextAlignment = false;
            this.lblGender.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Dpi = 254F;
            this.xrLabel8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(569.5629F, 63.50003F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(137.5833F, 63.50003F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "No. RM:";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblRM
            // 
            this.lblRM.Dpi = 254F;
            this.lblRM.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRM.LocationFloat = new DevExpress.Utils.PointFloat(707.1461F, 63.50003F);
            this.lblRM.Name = "lblRM";
            this.lblRM.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblRM.SizeF = new System.Drawing.SizeF(187.8541F, 63.50003F);
            this.lblRM.StylePriority.UseFont = false;
            this.lblRM.StylePriority.UseTextAlignment = false;
            this.lblRM.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Dpi = 254F;
            this.xrLabel10.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(25.00077F, 190.5001F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(869.9996F, 53.12836F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "Diagnosa / gejala : ";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblDiagnosa
            // 
            this.lblDiagnosa.Dpi = 254F;
            this.lblDiagnosa.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiagnosa.LocationFloat = new DevExpress.Utils.PointFloat(25.00077F, 243.6284F);
            this.lblDiagnosa.Name = "lblDiagnosa";
            this.lblDiagnosa.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblDiagnosa.SizeF = new System.Drawing.SizeF(869.9999F, 75.11824F);
            this.lblDiagnosa.StylePriority.UseFont = false;
            this.lblDiagnosa.StylePriority.UseTextAlignment = false;
            this.lblDiagnosa.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Dpi = 254F;
            this.xrLabel7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(25.00029F, 318.7467F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(869.9996F, 63.7117F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "Terapi yang telah diberikan : ";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Dpi = 254F;
            this.xrLabel11.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(25.00077F, 382.4584F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(869.9999F, 69.00333F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable2
            // 
            this.xrTable2.Dpi = 254F;
            this.xrTable2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(25.00029F, 127.0001F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow5});
            this.xrTable2.SizeF = new System.Drawing.SizeF(142.3955F, 63.49995F);
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell6});
            this.xrTableRow5.Dpi = 254F;
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1.9999985582246556D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Dpi = 254F;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 254F);
            this.xrTableCell3.StylePriority.UsePadding = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Alamat";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell3.Weight = 0.60768660891709914D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Dpi = 254F;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = ":";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrTableCell6.Weight = 0.093174653563980581D;
            // 
            // LKonsultasi
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.GroupFooter1});
            this.DetailPrintCountOnEmptyDataSource = 0;
            this.Margins = new System.Drawing.Printing.Margins(28, 28, 30, 30);
            this.PageWidth = 1001;
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.GroupFooter1, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tableHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRLabel lblLastUpdatedDate;
        private DevExpress.XtraReports.UI.XRLabel lblParamedic;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow6;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow7;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLabel lblDate;
        private DevExpress.XtraReports.UI.XRLabel cParamedicName;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel lblDiagnosa;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel lblRM;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel lblGender;
        private DevExpress.XtraReports.UI.XRLabel lblAlamat;
        private DevExpress.XtraReports.UI.XRLabel lblUmur;
        private DevExpress.XtraReports.UI.XRLabel lblNama;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
    }
}
