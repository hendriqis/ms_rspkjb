using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class EpisodeSummaryCtl : BaseViewPopupCtl
    {
        protected int VisitID = 0;
        public override void InitializeDataControl(string param)
        {
            if (param != "")
                VisitID = Convert.ToInt32(param);
            else
                VisitID = AppSession.RegisteredPatient.VisitID;

            vConsultVisit9 registeredPatient = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = {0}", VisitID))[0];

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
            getListIDPatientBodyDiagram();

        }
        public void getListIDPatientBodyDiagram()
        {
            string lstID;
            string filterExpression = String.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
            List<Int32> lstPatientBodyDiagramHdID = BusinessLayer.GetvPatientBodyDiagramHdIDList(filterExpression);

            StringBuilder sb = new StringBuilder();
            foreach (Int32 ID in lstPatientBodyDiagramHdID)
            {
                sb.Append(ID);
                sb.Append('|');
            }
            lstID = sb.ToString();
            hdnIDBodyDiagram.Value = lstID.Substring(0, (lstID.Length > 0 ? lstID.Length - 1 : lstID.Length));
        }
    }
}