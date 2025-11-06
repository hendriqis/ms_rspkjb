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
    public partial class PatientDiagnoseInfo : BasePageTrx
    {
        string menuType = string.Empty;
        String filterExpression = "";
        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_DIAGNOSE_DIAGNOSTIC;
            }
            else if (menuType == "dp")
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_DIAGNOSE_DIAGNOSTIC;
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.DATA_PATIENT_PATIENT_DIAGNOSE_DIAGNOSTIC;
                }
                return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_DIAGNOSE_DIAGNOSTIC;
            }
            else
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.PATIENT_DIAGNOSE_DIAGNOSTIC;
                    case Constant.Facility.IMAGING: return Constant.MenuCode.Imaging.PATIENT_PAGE_PATIENT_DIAGNOSIS;
                    case Constant.Facility.LABORATORY: return Constant.MenuCode.Laboratory.PATIENT_DIAGNOSE_DIAGNOSTIC;
                    case Constant.Module.RADIOTHERAPHY: return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_PATIENT_DIAGNOSIS;
                    default: return Constant.MenuCode.MedicalDiagnostic.PATIENT_DIAGNOSE_DIAGNOSTIC;
                }
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Count() > 1)
                {
                    hdnDepartmentID.Value = param[0];
                    menuType = param[1];
                }
                else
                {
                    hdnDepartmentID.Value = param[0];
                }
            }
            else
            {
                hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();

            BindGridDetail();
        }

        private void BindGridDetail()
        {
            filterExpression = (string.Format("VisitID = {0}", hdnVisitID.Value));
            List<vPatientDiagnosis1> lst = BusinessLayer.GetvPatientDiagnosis1List(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

    }
}