namespace QIS.Medinfras.ReportDesktop
{
    partial class LJournalTemplate
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
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.sbrJournalTemplateKDt = new DevExpress.XtraReports.UI.XRSubreport();
            this.lJournalTemplateDtRpt1 = new QIS.Medinfras.ReportDesktop.LJournalTemplateDt();
            this.sbrJournalTemplateDDt = new DevExpress.XtraReports.UI.XRSubreport();
            this.lJournalTemplateDtRpt2 = new QIS.Medinfras.ReportDesktop.LJournalTemplateDt();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lJournalTemplateDtRpt1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lJournalTemplateDtRpt2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.HeightF = 285.75F;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.JournalTemplateHd);
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
            this.sbrJournalTemplateDDt,
            this.sbrJournalTemplateKDt,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1});
            this.Detail.HeightF = 68.75F;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(31.75F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(1905F, 38.1F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "[TemplateName] :";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(63.5F, 38.1F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(952.5F, 38.1F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Debit";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(1492.25F, 38.1F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(285.75F, 38.1F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Kredit";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // sbrJournalTemplateKDt
            // 
            this.sbrJournalTemplateKDt.Dpi = 254F;
            this.sbrJournalTemplateKDt.LocationFloat = new DevExpress.Utils.PointFloat(1492.25F, 76.2F);
            this.sbrJournalTemplateKDt.Name = "sbrJournalTemplateKDt";
            this.sbrJournalTemplateKDt.ReportSource = this.lJournalTemplateDtRpt1;
            this.sbrJournalTemplateKDt.SizeF = new System.Drawing.SizeF(1333.5F, 63.5F);
            this.sbrJournalTemplateKDt.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sbrJournalTemplateKDt_BeforePrint);
            // 
            // sbrJournalTemplateDDt
            // 
            this.sbrJournalTemplateDDt.Dpi = 254F;
            this.sbrJournalTemplateDDt.LocationFloat = new DevExpress.Utils.PointFloat(63.5F, 76.2F);
            this.sbrJournalTemplateDDt.Name = "sbrJournalTemplateDDt";
            this.sbrJournalTemplateDDt.ReportSource = this.lJournalTemplateDtRpt2;
            this.sbrJournalTemplateDDt.SizeF = new System.Drawing.SizeF(1333.5F, 63.5F);
            this.sbrJournalTemplateDDt.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sbrJournalTemplateDDt_BeforePrint);
            // 
            // LJournalTemplate
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
            ((System.ComponentModel.ISupportInitialize)(this.lJournalTemplateDtRpt1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lJournalTemplateDtRpt2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRSubreport sbrJournalTemplateKDt;
        private LJournalTemplateDt lJournalTemplateDtRpt1;
        private DevExpress.XtraReports.UI.XRSubreport sbrJournalTemplateDDt;
        private LJournalTemplateDt lJournalTemplateDtRpt2;

    }
}
