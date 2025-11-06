namespace QIS.Medinfras.ReportDesktop
{
    partial class LJournalTemplateRpt
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
            this.lJournalTemplateDtRpt1 = new QIS.Medinfras.ReportDesktop.LJournalTemplateDtRpt();
            this.sbrJournalTemplateDDt = new DevExpress.XtraReports.UI.XRSubreport();
            this.lJournalTemplateDtRpt2 = new QIS.Medinfras.ReportDesktop.LJournalTemplateDtRpt();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lJournalTemplateDtRpt1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lJournalTemplateDtRpt2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // PageHeader
            // 
            this.PageHeader.HeightF = 112.5F;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.JournalTemplateHd);
            // 
            // lblReportParameter
            // 
            this.lblReportParameter.StylePriority.UseFont = false;
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
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(12.5F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(750F, 15F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "[TemplateName] :";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(25F, 15F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(375F, 15F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Debit";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(587.5F, 15F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(112.5F, 15F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "Kredit";
            // 
            // sbrJournalTemplateKDt
            // 
            this.sbrJournalTemplateKDt.LocationFloat = new DevExpress.Utils.PointFloat(587.5F, 30F);
            this.sbrJournalTemplateKDt.Name = "sbrJournalTemplateKDt";
            this.sbrJournalTemplateKDt.ReportSource = this.lJournalTemplateDtRpt1;
            this.sbrJournalTemplateKDt.SizeF = new System.Drawing.SizeF(525F, 25F);
            this.sbrJournalTemplateKDt.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sbrJournalTemplateKDt_BeforePrint);
            // 
            // sbrJournalTemplateDDt
            // 
            this.sbrJournalTemplateDDt.LocationFloat = new DevExpress.Utils.PointFloat(25F, 30F);
            this.sbrJournalTemplateDDt.Name = "sbrJournalTemplateDDt";
            this.sbrJournalTemplateDDt.ReportSource = this.lJournalTemplateDtRpt2;
            this.sbrJournalTemplateDDt.SizeF = new System.Drawing.SizeF(525F, 25F);
            this.sbrJournalTemplateDDt.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sbrJournalTemplateDDt_BeforePrint);
            // 
            // LJournalTemplateRpt
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
        private LJournalTemplateDtRpt lJournalTemplateDtRpt1;
        private DevExpress.XtraReports.UI.XRSubreport sbrJournalTemplateDDt;
        private LJournalTemplateDtRpt lJournalTemplateDtRpt2;

    }
}
