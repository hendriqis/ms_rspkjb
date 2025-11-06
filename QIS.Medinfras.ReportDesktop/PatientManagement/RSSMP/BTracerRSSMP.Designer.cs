namespace QIS.Medinfras.ReportDesktop
{
    partial class BTracerRSSMP
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
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.lblQueueNo = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.lblMedicalNo = new DevExpress.XtraReports.UI.XRLabel();
            this.lblPatientName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblServiceUnit = new DevExpress.XtraReports.UI.XRLabel();
            this.lblBusinessPartnerName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRegistrationDateTime = new DevExpress.XtraReports.UI.XRLabel();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.xrPageBreak1 = new DevExpress.XtraReports.UI.XRPageBreak();
            this.lblPhysician = new DevExpress.XtraReports.UI.XRLabel();
            this.lblGender = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 15F;
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblGender,
            this.xrPageBreak1,
            this.lblRegistrationDateTime,
            this.lblBusinessPartnerName,
            this.lblPhysician,
            this.lblServiceUnit,
            this.lblPatientName});
            this.Detail.HeightF = 423.2343F;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 15F;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 5F;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblQueueNo,
            this.xrLine1,
            this.lblMedicalNo});
            this.PageHeader.Dpi = 254F;
            this.PageHeader.HeightF = 129.3745F;
            this.PageHeader.Name = "PageHeader";
            // 
            // lblQueueNo
            // 
            this.lblQueueNo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "QueueNo")});
            this.lblQueueNo.Dpi = 254F;
            this.lblQueueNo.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQueueNo.LocationFloat = new DevExpress.Utils.PointFloat(431.7291F, 24.99997F);
            this.lblQueueNo.Name = "lblQueueNo";
            this.lblQueueNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblQueueNo.SizeF = new System.Drawing.SizeF(243.2706F, 71.16669F);
            this.lblQueueNo.StylePriority.UseFont = false;
            this.lblQueueNo.StylePriority.UseTextAlignment = false;
            this.lblQueueNo.Text = "999";
            this.lblQueueNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine1
            // 
            this.xrLine1.BorderDashStyle = DevExpress.XtraPrinting.BorderDashStyle.Solid;
            this.xrLine1.Dpi = 254F;
            this.xrLine1.LineStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.xrLine1.LineWidth = 3;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(37.19021F, 96.16666F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(623.0174F, 13.22918F);
            this.xrLine1.StylePriority.UseBorderDashStyle = false;
            // 
            // lblMedicalNo
            // 
            this.lblMedicalNo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "MedicalNo")});
            this.lblMedicalNo.Dpi = 254F;
            this.lblMedicalNo.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMedicalNo.LocationFloat = new DevExpress.Utils.PointFloat(37.1908F, 25.00001F);
            this.lblMedicalNo.Name = "lblMedicalNo";
            this.lblMedicalNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblMedicalNo.SizeF = new System.Drawing.SizeF(394.5384F, 71.16669F);
            this.lblMedicalNo.StylePriority.UseFont = false;
            this.lblMedicalNo.StylePriority.UseTextAlignment = false;
            this.lblMedicalNo.Text = "lblMedicalNo";
            this.lblMedicalNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblPatientName
            // 
            this.lblPatientName.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "PatientName")});
            this.lblPatientName.Dpi = 254F;
            this.lblPatientName.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientName.LocationFloat = new DevExpress.Utils.PointFloat(37.19068F, 0F);
            this.lblPatientName.Name = "lblPatientName";
            this.lblPatientName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPatientName.SizeF = new System.Drawing.SizeF(623.0177F, 58.42001F);
            this.lblPatientName.StylePriority.UseFont = false;
            this.lblPatientName.StylePriority.UseTextAlignment = false;
            this.lblPatientName.Text = "lblPatientName";
            this.lblPatientName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblServiceUnit
            // 
            this.lblServiceUnit.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "ServiceUnit")});
            this.lblServiceUnit.Dpi = 254F;
            this.lblServiceUnit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceUnit.LocationFloat = new DevExpress.Utils.PointFloat(37.19083F, 154.2811F);
            this.lblServiceUnit.Name = "lblServiceUnit";
            this.lblServiceUnit.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblServiceUnit.SizeF = new System.Drawing.SizeF(623.0177F, 39.41663F);
            this.lblServiceUnit.StylePriority.UseFont = false;
            this.lblServiceUnit.StylePriority.UseTextAlignment = false;
            this.lblServiceUnit.Text = "s";
            this.lblServiceUnit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblBusinessPartnerName
            // 
            this.lblBusinessPartnerName.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "BusinessPartnerName")});
            this.lblBusinessPartnerName.Dpi = 254F;
            this.lblBusinessPartnerName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBusinessPartnerName.LocationFloat = new DevExpress.Utils.PointFloat(37.19076F, 348.3784F);
            this.lblBusinessPartnerName.Name = "lblBusinessPartnerName";
            this.lblBusinessPartnerName.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblBusinessPartnerName.SizeF = new System.Drawing.SizeF(623.0176F, 39.41663F);
            this.lblBusinessPartnerName.StylePriority.UseFont = false;
            this.lblBusinessPartnerName.StylePriority.UseTextAlignment = false;
            this.lblBusinessPartnerName.Text = "lblBusinessPartnerName";
            this.lblBusinessPartnerName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblRegistrationDateTime
            // 
            this.lblRegistrationDateTime.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "RegistrationDateTime")});
            this.lblRegistrationDateTime.Dpi = 254F;
            this.lblRegistrationDateTime.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRegistrationDateTime.LocationFloat = new DevExpress.Utils.PointFloat(37.19021F, 286.031F);
            this.lblRegistrationDateTime.Name = "lblRegistrationDateTime";
            this.lblRegistrationDateTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblRegistrationDateTime.SizeF = new System.Drawing.SizeF(623.0176F, 39.41666F);
            this.lblRegistrationDateTime.StylePriority.UseFont = false;
            this.lblRegistrationDateTime.StylePriority.UseTextAlignment = false;
            this.lblRegistrationDateTime.Text = "lblRegistrationDateTime";
            this.lblRegistrationDateTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.ReportDesktop.TempClassTracerRegistration);
            // 
            // xrPageBreak1
            // 
            this.xrPageBreak1.Dpi = 254F;
            this.xrPageBreak1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 410.5536F);
            this.xrPageBreak1.Name = "xrPageBreak1";
            // 
            // lblPhysician
            // 
            this.lblPhysician.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Physician")});
            this.lblPhysician.Dpi = 254F;
            this.lblPhysician.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPhysician.LocationFloat = new DevExpress.Utils.PointFloat(37.19096F, 218.3921F);
            this.lblPhysician.Name = "lblPhysician";
            this.lblPhysician.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblPhysician.SizeF = new System.Drawing.SizeF(623.0176F, 39.41666F);
            this.lblPhysician.StylePriority.UseFont = false;
            this.lblPhysician.StylePriority.UseTextAlignment = false;
            this.lblPhysician.Text = "lblPhysician";
            this.lblPhysician.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblGender
            // 
            this.lblGender.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "NumberGender")});
            this.lblGender.Dpi = 254F;
            this.lblGender.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGender.LocationFloat = new DevExpress.Utils.PointFloat(37.19064F, 88.40611F);
            this.lblGender.Name = "lblGender";
            this.lblGender.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblGender.SizeF = new System.Drawing.SizeF(623.0177F, 39.41663F);
            this.lblGender.StylePriority.UseFont = false;
            this.lblGender.StylePriority.UseTextAlignment = false;
            this.lblGender.Text = "s";
            this.lblGender.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // BTracerRSSMP
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.PageFooter,
            this.PageHeader,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.DataSource = this.bs;
            this.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 15, 15);
            this.PageHeight = 900;
            this.PageWidth = 700;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.PaperName = "BUKTI_PENDAFTARAN";
            this.ShowPreviewMarginLines = false;
            this.ShowPrintMarginsWarning = false;
            this.SnapToGrid = false;
            this.Version = "11.1";
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRLabel lblPatientName;
        private DevExpress.XtraReports.UI.XRLabel lblServiceUnit;
        private DevExpress.XtraReports.UI.XRLabel lblBusinessPartnerName;
        private DevExpress.XtraReports.UI.XRLabel lblRegistrationDateTime;
        private DevExpress.XtraReports.UI.XRLabel lblMedicalNo;
        private System.Windows.Forms.BindingSource bs;
        private DevExpress.XtraReports.UI.XRLabel lblQueueNo;
        private DevExpress.XtraReports.UI.XRPageBreak xrPageBreak1;
        private DevExpress.XtraReports.UI.XRLabel lblPhysician;
        private DevExpress.XtraReports.UI.XRLabel lblGender;
    }
}
