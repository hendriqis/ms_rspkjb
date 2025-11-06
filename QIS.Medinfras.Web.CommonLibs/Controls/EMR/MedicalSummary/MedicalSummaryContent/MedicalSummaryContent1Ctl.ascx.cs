using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class MedicalSummaryContent1Ctl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            LoadPatientAndVisitInformation(Convert.ToInt32(queryString));
        }

        private void LoadPatientAndVisitInformation(int visitID)
        {
            vConsultVisit9 registeredPatient = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = {0}", visitID))[0];

            lblPatientName.InnerHtml = registeredPatient.cfPatientNameSalutation;
            lblGender.InnerHtml = registeredPatient.Gender;
            lblDateOfBirth.InnerHtml = string.Format("{0} ({1})", registeredPatient.DateOfBirthInString, Helper.GetPatientAge(words, registeredPatient.DateOfBirth));

            lblRegistrationDateTime.InnerHtml = string.Format("{0} / {1}", registeredPatient.VisitDateInString, registeredPatient.VisitTime);
            lblRegistrationNo.InnerHtml = registeredPatient.RegistrationNo;
            lblPhysician.InnerHtml = registeredPatient.ParamedicName;

            lblMedicalNo.InnerHtml = registeredPatient.MedicalNo;

            List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", registeredPatient.VisitID));
            StringBuilder diagnosis = new StringBuilder();
            foreach (vPatientDiagnosis patientDiagnosis in lstPatientDiagnosis)
            {
                if (patientDiagnosis.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                    diagnosis.AppendLine(string.Format("{0} ({1})", patientDiagnosis.DiagnosisText, patientDiagnosis.DiagnoseType));
                else
                    diagnosis.AppendLine(string.Format("{0}", patientDiagnosis.DiagnosisText));
            }

            lblPayerInformation.InnerHtml = registeredPatient.BusinessPartnerName;
            lblPatientLocation.InnerHtml = registeredPatient.cfPatientLocation;
            lblDiagnosis.InnerHtml = diagnosis.ToString();
            imgPatientImage.Src = registeredPatient.PatientImageUrl;
        }      
    }
}