using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPermintaanRujukanLab : BaseDailyPortraitRpt
    {
        public BPermintaanRujukanLab()
        {
            InitializeComponent();
          
        }
        public override void InitializeReport(string[] param) {

            lblReportTitle.Visible = false;
            lblReportSubtitle.Visible = false;
            lblReportProperties.Visible = false;
            lblReportParameterTitle.Visible = false;
            lblParameter1.Visible = false;
            lblParameter2.Visible = false;
            lblParameter3.Visible = false;
            lblParameter4.Visible = false;

            lblParameter5.Visible = false;
            lblParameter6.Visible = false; 
            lblParameter7.Visible = false;
            

            ReportMaster oReport = BusinessLayer.GetReportMasterList(string.Format("ReportCode='{0}'", param[4])).FirstOrDefault();
            LblReport.Text = oReport.ReportTitle1;

            List<vTestOrderDtReport> lstTestOrderDt = BusinessLayer.GetvTestOrderDtReportList(string.Format("TestOrderID='{0}' AND BusinessPartnerID ='{1}' AND  IsDeleted=0", param[0], param[1]));
            Specimen oSpecimen = BusinessLayer.GetSpecimenList(string.Format("SpecimenID='{0}'", param[2])).FirstOrDefault();
            if (oSpecimen != null)
            {
                lblSample.Text = oSpecimen.SpecimenName;
            }
            if (lstTestOrderDt.Count > 0)
            {
                vTestOrderDtReport oData = lstTestOrderDt.FirstOrDefault();
                lblPatientName.Text = oData.PatientName;
                lblGender.Text = oData.Gender;
                lblDOB.Text = oData.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_1);
                lblRM.Text = oData.MedicalNo;
                lblRujukan.Text = oData.BusinessPartnerName;

                string Pemeriksaan = "";
                foreach (vTestOrderDtReport row in lstTestOrderDt) {
                    Pemeriksaan += string.Format("{0},", row.ItemName1);
                }

                lblPemeriksaan.Text = Pemeriksaan.Remove(Pemeriksaan.Length - 1);
            }
            lblTanggal.Text = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_1);
            lblNote.Text = param[3];
            lblPetugas.Text = AppSession.UserLogin.UserFullName;
        }

    }
}
