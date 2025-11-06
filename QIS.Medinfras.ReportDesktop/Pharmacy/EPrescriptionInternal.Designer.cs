namespace QIS.Medinfras.ReportDesktop
{
    partial class EPrescriptionInternal
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
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.cPrescriber = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cSIPNo = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.cLicenseNo = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.cRegistrationNo = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.cPatient = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.cDOB = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable4 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cR1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.cCompoundName = new DevExpress.XtraReports.UI.XRTableCell();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.lblSignaLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbldet = new DevExpress.XtraReports.UI.XRLabel();
            this.lblMFLA = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTTD = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.cR2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // xrPageInfo
            // 
            this.xrPageInfo.StylePriority.UseFont = false;
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
            // 
            // lblReportProperties
            // 
            this.lblReportProperties.StylePriority.UseFont = false;
            this.lblReportProperties.StylePriority.UsePadding = false;
            this.lblReportProperties.StylePriority.UseTextAlignment = false;
            // 
            // bs
            // 
            this.bs.DataSource = typeof(QIS.Medinfras.Data.Service.GetPrescriptionOrderDtCustom);
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.PageHeader.HeightF = 668.6083F;
            this.PageHeader.Controls.SetChildIndex(this.xrTable2, 0);
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
            this.Detail.HeightF = 31.75F;
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("PrescriptionOrderDetailID", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
            // 
            // xrTable2
            // 
            this.xrTable2.Dpi = 254F;
            this.xrTable2.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(24.99908F, 443.7126F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow8,
            this.xrTableRow13,
            this.xrTableRow9,
            this.xrTableRow10,
            this.xrTableRow11});
            this.xrTable2.SizeF = new System.Drawing.SizeF(1303.001F, 224.8958F);
            this.xrTable2.StylePriority.UseFont = false;
            this.xrTable2.StylePriority.UsePadding = false;
            this.xrTable2.StylePriority.UseTextAlignment = false;
            this.xrTable2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell5,
            this.xrTableCell10,
            this.cPrescriber});
            this.xrTableRow8.Dpi = 254F;
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.Dpi = 254F;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Text = "Prescriber";
            this.xrTableCell5.Weight = 0.51971359676213091D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.Dpi = 254F;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = ":";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell10.Weight = 0.079193382611743246D;
            // 
            // cPrescriber
            // 
            this.cPrescriber.Dpi = 254F;
            this.cPrescriber.Name = "cPrescriber";
            this.cPrescriber.Text = "cPrescriber";
            this.cPrescriber.Weight = 2.4010930206261256D;
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cSIPNo,
            this.xrTableCell19,
            this.cLicenseNo});
            this.xrTableRow13.Dpi = 254F;
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 1D;
            // 
            // cSIPNo
            // 
            this.cSIPNo.Dpi = 254F;
            this.cSIPNo.Name = "cSIPNo";
            this.cSIPNo.Text = "SIP No";
            this.cSIPNo.Weight = 0.51971359676213091D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.Dpi = 254F;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.Text = ":";
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell19.Weight = 0.079193382611743246D;
            // 
            // cLicenseNo
            // 
            this.cLicenseNo.Dpi = 254F;
            this.cLicenseNo.Name = "cLicenseNo";
            this.cLicenseNo.Text = "cLicenseNo";
            this.cLicenseNo.Weight = 2.4010930206261256D;
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell11,
            this.xrTableCell12,
            this.cRegistrationNo});
            this.xrTableRow9.Dpi = 254F;
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 1D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.Dpi = 254F;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.Text = "Registration No";
            this.xrTableCell11.Weight = 0.51971359676213091D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.Dpi = 254F;
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = ":";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell12.Weight = 0.079193382611743246D;
            // 
            // cRegistrationNo
            // 
            this.cRegistrationNo.Dpi = 254F;
            this.cRegistrationNo.Name = "cRegistrationNo";
            this.cRegistrationNo.Text = "cRegistrationNo";
            this.cRegistrationNo.Weight = 2.4010930206261256D;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell14,
            this.xrTableCell15,
            this.cPatient});
            this.xrTableRow10.Dpi = 254F;
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Weight = 1D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.Dpi = 254F;
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.Text = "Patient";
            this.xrTableCell14.Weight = 0.51971359676213091D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.Dpi = 254F;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.Text = ":";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell15.Weight = 0.079193382611743246D;
            // 
            // cPatient
            // 
            this.cPatient.Dpi = 254F;
            this.cPatient.Name = "cPatient";
            this.cPatient.Text = "cPatient";
            this.cPatient.Weight = 2.4010930206261256D;
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell17,
            this.xrTableCell18,
            this.cDOB});
            this.xrTableRow11.Dpi = 254F;
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Weight = 1D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Dpi = 254F;
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Text = "Date of Birth";
            this.xrTableCell17.Weight = 0.51971359676213091D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.Dpi = 254F;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.Text = ":";
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell18.Weight = 0.079193382611743246D;
            // 
            // cDOB
            // 
            this.cDOB.Dpi = 254F;
            this.cDOB.Name = "cDOB";
            this.cDOB.Text = "cDOB";
            this.cDOB.Weight = 2.4010930206261256D;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel3,
            this.xrTable4});
            this.GroupHeader1.Dpi = 254F;
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("ParentIDForRpt", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 29.31583F;
            this.GroupHeader1.Name = "GroupHeader1";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(1109.791F, 0F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(179.9167F, 29.31583F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "No. [DispenseQtyInString]";
            // 
            // xrTable4
            // 
            this.xrTable4.Dpi = 254F;
            this.xrTable4.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable4.LocationFloat = new DevExpress.Utils.PointFloat(24.99993F, 0F);
            this.xrTable4.Name = "xrTable4";
            this.xrTable4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTable4.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow12});
            this.xrTable4.SizeF = new System.Drawing.SizeF(1069.719F, 27.91667F);
            this.xrTable4.StylePriority.UseFont = false;
            this.xrTable4.StylePriority.UsePadding = false;
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cR1,
            this.cCompoundName});
            this.xrTableRow12.Dpi = 254F;
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Weight = 1D;
            // 
            // cR1
            // 
            this.cR1.Dpi = 254F;
            this.cR1.Name = "cR1";
            this.cR1.StylePriority.UseTextAlignment = false;
            this.cR1.Text = "R/";
            this.cR1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.cR1.Weight = 0.16232112628451909D;
            // 
            // cCompoundName
            // 
            this.cCompoundName.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "DrugCustomName")});
            this.cCompoundName.Dpi = 254F;
            this.cCompoundName.Name = "cCompoundName";
            this.cCompoundName.Weight = 2.2542127552603457D;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblSignaLabel,
            this.xrLabel4,
            this.lbldet,
            this.lblMFLA,
            this.xrLine2});
            this.GroupFooter1.Dpi = 254F;
            this.GroupFooter1.HeightF = 125.0902F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // lblSignaLabel
            // 
            this.lblSignaLabel.Dpi = 254F;
            this.lblSignaLabel.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSignaLabel.LocationFloat = new DevExpress.Utils.PointFloat(59.39673F, 31.31583F);
            this.lblSignaLabel.Name = "lblSignaLabel";
            this.lblSignaLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblSignaLabel.SizeF = new System.Drawing.SizeF(1230.312F, 29.31583F);
            this.lblSignaLabel.StylePriority.UseFont = false;
            this.lblSignaLabel.Text = "Signa";
            this.lblSignaLabel.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblSignaLabel_BeforePrint);
            // 
            // xrLabel4
            // 
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(59.39572F, 64.45856F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(1230.312F, 29.31583F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "[MedicationAdministration] [MedicationPurpose]";
            // 
            // lbldet
            // 
            this.lbldet.Dpi = 254F;
            this.lbldet.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldet.LocationFloat = new DevExpress.Utils.PointFloat(1109.792F, 93.77441F);
            this.lbldet.Name = "lbldet";
            this.lbldet.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lbldet.SizeF = new System.Drawing.SizeF(179.9167F, 29.31583F);
            this.lbldet.StylePriority.UseFont = false;
            this.lbldet.Text = "lbldet";
            this.lbldet.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lbldet_BeforePrint);
            // 
            // lblMFLA
            // 
            this.lblMFLA.Dpi = 254F;
            this.lblMFLA.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMFLA.LocationFloat = new DevExpress.Utils.PointFloat(59.39576F, 0F);
            this.lblMFLA.Name = "lblMFLA";
            this.lblMFLA.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblMFLA.SizeF = new System.Drawing.SizeF(1230.312F, 29.31583F);
            this.lblMFLA.StylePriority.UseFont = false;
            this.lblMFLA.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblMFLA_BeforePrint);
            // 
            // xrLine2
            // 
            this.xrLine2.Dpi = 254F;
            this.xrLine2.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.xrLine2.LineWidth = 3;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(24.99993F, 93.77441F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(1069.719F, 26.67001F);
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.lblTTD});
            this.ReportFooter.Dpi = 254F;
            this.ReportFooter.HeightF = 209.0208F;
            this.ReportFooter.KeepTogether = true;
            this.ReportFooter.Name = "ReportFooter";
            this.ReportFooter.PrintAtBottom = true;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(698.2928F, 152.0592F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(629.7082F, 37.25339F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "P.c.c.";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblTTD
            // 
            this.lblTTD.Dpi = 254F;
            this.lblTTD.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTTD.LocationFloat = new DevExpress.Utils.PointFloat(698.2927F, 25.00001F);
            this.lblTTD.Name = "lblTTD";
            this.lblTTD.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.lblTTD.SizeF = new System.Drawing.SizeF(629.7083F, 37.25333F);
            this.lblTTD.StylePriority.UseFont = false;
            this.lblTTD.StylePriority.UseTextAlignment = false;
            this.lblTTD.Text = "lblTTD";
            this.lblTTD.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTable3
            // 
            this.xrTable3.Dpi = 254F;
            this.xrTable3.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(25.00001F, 0F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable3.SizeF = new System.Drawing.SizeF(1069.719F, 31.75F);
            this.xrTable3.StylePriority.UseFont = false;
            this.xrTable3.StylePriority.UsePadding = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.cR2,
            this.xrTableCell16});
            this.xrTableRow2.Dpi = 254F;
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // cR2
            // 
            this.cR2.Dpi = 254F;
            this.cR2.Name = "cR2";
            this.cR2.StylePriority.UseTextAlignment = false;
            this.cR2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.cR2.Weight = 0.16543533017168355D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.Dpi = 254F;
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.Text = "[ItemName] [CompoundQtyInString] [CompoundUnit]";
            this.xrTableCell16.Weight = 2.29746321113024D;
            // 
            // EPrescriptionInternal
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter,
            this.GroupHeader1,
            this.GroupFooter1,
            this.ReportFooter,
            this.PageHeader});
            this.Version = "11.1";
            this.Watermark.ForeColor = System.Drawing.Color.Gray;
            this.Watermark.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Zoom;
            this.Watermark.Text = "COPY";
            this.Controls.SetChildIndex(this.PageHeader, 0);
            this.Controls.SetChildIndex(this.ReportFooter, 0);
            this.Controls.SetChildIndex(this.GroupFooter1, 0);
            this.Controls.SetChildIndex(this.GroupHeader1, 0);
            this.Controls.SetChildIndex(this.PageFooter, 0);
            this.Controls.SetChildIndex(this.ReportHeader, 0);
            this.Controls.SetChildIndex(this.BottomMargin, 0);
            this.Controls.SetChildIndex(this.TopMargin, 0);
            this.Controls.SetChildIndex(this.Detail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow8;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell5;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell10;
        private DevExpress.XtraReports.UI.XRTableCell cPrescriber;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow9;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell11;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell12;
        private DevExpress.XtraReports.UI.XRTableCell cRegistrationNo;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow10;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell14;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell15;
        private DevExpress.XtraReports.UI.XRTableCell cPatient;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow11;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell17;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell18;
        private DevExpress.XtraReports.UI.XRTableCell cDOB;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel lblTTD;
        private DevExpress.XtraReports.UI.XRTable xrTable3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTableCell cR2;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell16;
        private DevExpress.XtraReports.UI.XRTable xrTable4;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow12;
        private DevExpress.XtraReports.UI.XRTableCell cR1;
        private DevExpress.XtraReports.UI.XRTableCell cCompoundName;
        private DevExpress.XtraReports.UI.XRLabel lblMFLA;
        private DevExpress.XtraReports.UI.XRLabel lbldet;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel lblSignaLabel;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow13;
        private DevExpress.XtraReports.UI.XRTableCell cSIPNo;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell19;
        private DevExpress.XtraReports.UI.XRTableCell cLicenseNo;
    }
}
