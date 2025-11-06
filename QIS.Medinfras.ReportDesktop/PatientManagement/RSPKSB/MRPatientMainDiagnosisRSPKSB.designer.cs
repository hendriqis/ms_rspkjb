namespace QIS.Medinfras.ReportDesktop
{
    partial class MRPatientMainDiagnosisRSPKSB
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDiagnoseText = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.tbDiagnosisID = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Detail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Detail.HeightF = 25F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseFont = false;
            this.Detail.StylePriority.UseForeColor = false;
            this.Detail.StylePriority.UseTextAlignment = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(53.33323F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(668.6667F, 25F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell3,
            this.xrTableCell4,
            this.xrTableCell15,
            this.tbDiagnoseText,
            this.xrTableCell6,
            this.tbDiagnosisID});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Nama Diagnosis";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell3.Weight = 4.7050815615086146D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Italic);
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.Text = "(Diagnosis Name)";
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell4.Weight = 4.6673406210320163D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseFont = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.Text = ":";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell15.Weight = 0.68353805601622986D;
            // 
            // tbDiagnoseText
            // 
            this.tbDiagnoseText.Font = new System.Drawing.Font("Tahoma", 8F);
            this.tbDiagnoseText.Name = "tbDiagnoseText";
            this.tbDiagnoseText.StylePriority.UseFont = false;
            this.tbDiagnoseText.StylePriority.UseTextAlignment = false;
            this.tbDiagnoseText.Text = "[DiagnoseText]";
            this.tbDiagnoseText.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.tbDiagnoseText.Weight = 15.67521501571402D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.Text = "ICD X:";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell6.Weight = 2.1973796576830011D;
            // 
            // tbDiagnosisID
            // 
            this.tbDiagnosisID.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.tbDiagnosisID.Name = "tbDiagnosisID";
            this.tbDiagnosisID.StylePriority.UseFont = false;
            this.tbDiagnosisID.StylePriority.UseTextAlignment = false;
            this.tbDiagnosisID.Text = "[DiagnosisID]";
            this.tbDiagnosisID.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.tbDiagnosisID.Weight = 5.71990494254373D;
            // 
            // TopMargin
            // 
            this.TopMargin.Font = new System.Drawing.Font("Tahoma", 9F);
            this.TopMargin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.StylePriority.UseFont = false;
            this.TopMargin.StylePriority.UseForeColor = false;
            this.TopMargin.StylePriority.UseTextAlignment = false;
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BottomMargin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.StylePriority.UseFont = false;
            this.BottomMargin.StylePriority.UseForeColor = false;
            this.BottomMargin.StylePriority.UseTextAlignment = false;
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // MRPatientMainDiagnosisRSPKSB
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.DetailPrintCountOnEmptyDataSource = 0;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(0, 28, 0, 0);
            this.PageHeight = 719;
            this.PageWidth = 750;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.PrintOnEmptyDataSource = false;
            this.Version = "11.1";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell3;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell4;
        private DevExpress.XtraReports.UI.XRTableCell tbDiagnoseText;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell6;
        private DevExpress.XtraReports.UI.XRTableCell tbDiagnosisID;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell15;
    }
}
