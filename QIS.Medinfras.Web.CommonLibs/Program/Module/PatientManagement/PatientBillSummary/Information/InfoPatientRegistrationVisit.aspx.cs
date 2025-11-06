using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoPatientRegistrationVisit : BasePageTrx
    {
        String filterExpression = "";
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.INFORMATION_PATIENT_REGISTRATION_VISIT_LABORATORY;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.INFORMATION_PATIENT_REGISTRATION_VISIT_IMAGING;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.INFORMATION_PATIENT_REGISTRATION_VISIT;
                    return Constant.MenuCode.MedicalDiagnostic.INFORMATION_PATIENT_REGISTRATION_VISIT_MEDICALDIAGNOSTIC;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.INFORMATION_PATIENT_REGISTRATION_VISIT_MEDICALCHECKUP;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.INFORMATION_PATIENT_REGISTRATION_VISIT_EMERGENCY;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.INFORMATION_PATIENT_REGISTRATION_VISIT_INPATIENT;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.INFORMATION_PATIENT_REGISTRATION_VISIT_INPATIENT;
                default: return Constant.MenuCode.Outpatient.INFORMATION_PATIENT_REGISTRATION_VISIT_OUTPATIENT;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            vConsultVisit4 entityVisit = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];

            hdnLinkedRegistrationID.Value = entityVisit.LinkedRegistrationID.ToString();
            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            hdnVisitID.Value = entityVisit.VisitID.ToString();

            EntityToControl(entityVisit);

            BindGridDetail();
        }

        private void BindGridDetail()
        {
            filterExpression = "";

            if (hdnLinkedRegistrationID.Value == "0")
            {
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            }
            else
            {
                filterExpression = string.Format("RegistrationID IN ({0}, {1})", hdnLinkedRegistrationID.Value, hdnRegistrationID.Value);
            }

            //filterExpression = (string.Format("VisitID = {0}", hdnVisitID.Value));
            List<vRegistrationInformation> lst = BusinessLayer.GetvRegistrationInformationList(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void EntityToControl(vConsultVisit4 entityVisit)
        {
            hdnDepartmentID.Value = entityVisit.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entityVisit.HealthcareServiceUnitID.ToString();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

    }
}