using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratKeteranganSehat4 : BaseDailyPortrait2Rpt
    {
        public BSuratKeteranganSehat4()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit9 entityVisit = BusinessLayer.GetvConsultVisit9List(param[0])[0];
            vPatient entityPatient = BusinessLayer.GetvPatientList(String.Format("MRN = {0}", entityVisit.MRN))[0];
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vVitalSignDt VitalSignW = BusinessLayer.GetvVitalSignDtList(string.Format("RegistrationID = {0} AND VitalSignLabel = 'WEIGHT' AND IsDeleted = 0", entityVisit.RegistrationID)).FirstOrDefault();
            vVitalSignDt VitalSignH = BusinessLayer.GetvVitalSignDtList(string.Format("RegistrationID = {0} AND VitalSignLabel = 'HEIGHT' AND IsDeleted = 0", entityVisit.RegistrationID)).FirstOrDefault();
            vVitalSignDt VitalSignS = BusinessLayer.GetvVitalSignDtList(string.Format("RegistrationID = {0} AND VitalSignLabel = 'TDs' AND IsDeleted = 0 ORDER BY ID ", entityVisit.RegistrationID)).FirstOrDefault();
            vVitalSignDt VitalSignD = BusinessLayer.GetvVitalSignDtList(string.Format("RegistrationID = {0} AND VitalSignLabel = 'TDd' AND IsDeleted = 0 ORDER BY ID", entityVisit.RegistrationID)).FirstOrDefault();

            DateTime dateNow = DateTime.Now;
            txtGender.Text = entityPatient.cfGender;
            if (VitalSignH == null)
            {
                lblTBBB.Text = string.Format("-");
            }
            else
            {
                lblTBBB.Text = string.Format("{0} cm", VitalSignH.VitalSignValue);
            }
            if (VitalSignW == null)
            {
                lblWt.Text = string.Format("-");
            }
            else
            {
                lblWt.Text = string.Format("{0} kg", VitalSignW.VitalSignValue);
            }
            if (VitalSignS == null || VitalSignD == null)
            {
                lblTekananDarah.Text = string.Format("-");
            }
            else
            {
                lblTekananDarah.Text = string.Format("{0}/{1} mmHg", VitalSignS.VitalSignValue, VitalSignD.VitalSignValue);
            }

            txtDateOfBirth.Text = string.Format("{0} /{1} tahun ", entityVisit.DateOfBirthInString, entityVisit.AgeInYear);
            String type = param[2];
            if (type == "0")
            {
                lblType.Visible = false;
                xrLabel3.Visible = false;
            }
            else if (type == "1")
            {
                lblType.Text = "Tidak Buta Warna";
            }
            else
            {
                lblType.Text = "Buta Warna";
            }
            lblPrintDate.Text = string.Format("{0}, {1}",entityHealthcare.City, dateNow.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedicName.Text = entityVisit.ParamedicName;
            lblStatement.Text = string.Format("Pada tanggal : {0} , telah di periksa kesehatan fisik dan ternyata Sehat untuk mengikuti / menjadi : {1}", dateNow.ToString(Constant.FormatString.DATE_FORMAT), param[1]);
            base.InitializeReport(param);

        }
    }
}
