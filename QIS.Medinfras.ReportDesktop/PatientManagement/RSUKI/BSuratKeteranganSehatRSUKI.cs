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
    public partial class BSuratKeteranganSehatRSUKI : BaseDailyPortrait2Rpt
    {
        public BSuratKeteranganSehatRSUKI()
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
            vVitalSignDt VitalSignP = BusinessLayer.GetvVitalSignDtList(string.Format("RegistrationID = {0} AND VitalSignLabel = 'HR' AND IsDeleted = 0 ORDER BY ID ", entityVisit.RegistrationID)).FirstOrDefault();
            vVitalSignDt VitalSignT = BusinessLayer.GetvVitalSignDtList(string.Format("RegistrationID = {0} AND VitalSignLabel = 'TEMP' AND IsDeleted = 0 ORDER BY ID", entityVisit.RegistrationID)).FirstOrDefault();
            vVitalSignDt VitalSignR = BusinessLayer.GetvVitalSignDtList(string.Format("RegistrationID = {0} AND VitalSignLabel = 'RR' AND IsDeleted = 0 ORDER BY ID", entityVisit.RegistrationID)).FirstOrDefault();
            vParamedicMaster entityParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", entityVisit.ParamedicID)).FirstOrDefault();

            DateTime dateNow = DateTime.Now;
            txtGender.Text = entityPatient.cfGender;
            if (VitalSignW == null)
            {
                lblTBBB.Text = string.Format("-");
            }
            else
            {
                lblTBBB.Text = string.Format("{0} cm", VitalSignH.VitalSignValue);
            }
            if (VitalSignH == null)
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
            if (VitalSignP == null)
            {
                lblNadi.Text = string.Format("-");
            }
            else
            {
                lblNadi.Text = string.Format("{0} x/mntst", VitalSignP.VitalSignValue);
            }
            if (VitalSignT == null)
            {
                lblTemp.Text = string.Format("-");
            }
            else
            {
                lblTemp.Text = string.Format("{0}{1}", VitalSignT.VitalSignValue, VitalSignT.ValueUnit);
            }
            if (VitalSignR == null)
            {
                lblRR.Text = string.Format("-");
            }
            else
            {
                lblRR.Text = string.Format("{0} x/mnt", VitalSignR.VitalSignValue);
            }
            txtDateOfBirth.Text = string.Format("{0} /{1} tahun ", entityVisit.DateOfBirthInString, entityVisit.AgeInYear);
            lblPrintDate.Text = string.Format("{0}, {1}", entityHealthcare.City, dateNow.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedicName.Text = entityVisit.ParamedicName;
            txtKeterangan.Text = param[1];
            txtHasil.Text = param[2];
            if (entityParamedic.LicenseNo == null)
            {
                lblNoSIP.Text = "";
            }
            else
            {
                lblNoSIP.Text = entityParamedic.LicenseNo;
            }
            base.InitializeReport(param);
        }
    }
}
