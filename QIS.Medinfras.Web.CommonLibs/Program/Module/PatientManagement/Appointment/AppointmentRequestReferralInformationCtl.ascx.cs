using System;
using System.Collections.Generic;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class PatientDischargeEntryCtl : BaseEntryPopupCtl
    {
        protected string RegistrationDateTime = "";

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                PatientReferral entity = BusinessLayer.GetPatientReferralList(string.Format("AppointmentRequestID = {0} AND IsDeleted = 0", param)).FirstOrDefault();
                if (entity != null)
                {
                    txtDiagnosisText.Text = entity.DiagnosisText;
                    txtMedicalResumeText.Text = entity.MedicalResumeText;
                    txtPlanningResumeText.Text = entity.PlanningResumeText;

                    ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", entity.VisitID)).FirstOrDefault();
                    vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityCV.RegistrationID)).FirstOrDefault();
                    txtRegistrationNo.Text = entityReg.RegistrationNo;
                    txtServiceUnit.Text = entityReg.ServiceUnitName;
                    txtChargeClass.Text = entityReg.ChargeClassName;
                    txtParamedic.Text = entityReg.ParamedicName;
                    txtBusinessPartner.Text = entityReg.BusinessPartnerName;
                }
            }
        }
    }
}