using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SurgeryReportList : BasePageTrx
    {
        string menuType = string.Empty;
        string deptType = string.Empty;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_INFORMATION_PATIENT_SURGERY_REPORT;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_INFORMATION_PATIENT_SURGERY_REPORT;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_INFORMATION_PATIENT_SURGERY_REPORT;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_INFORMATION_PATIENT_SURGERY_REPORT;
                    default:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_INFORMATION_PATIENT_SURGERY_REPORT;
                }
            }
            else if (menuType == "dp")
            {
                switch (deptType)
                {
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.DATA_PATIENT_INFORMATION_PATIENT_SURGERY_REPORT;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_INFORMATION_PATIENT_SURGERY_REPORT;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_INFORMATION_PATIENT_SURGERY_REPORT;
                    default:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_INFORMATION_PATIENT_SURGERY_REPORT;
                }
            }
            else
            {
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.INFORMATION_PATIENT_SURGERY_REPORT;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.INFORMATION_PATIENT_SURGERY_REPORT;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.INFORMATION_PATIENT_SURGERY_REPORT;
                    default:
                        return Constant.MenuCode.Inpatient.INFORMATION_PATIENT_SURGERY_REPORT;
                }
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                deptType = param[0];
                menuType = param[1];
            }
            else
            {
                deptType = param[0];
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();

            hdnVisitID.Value = entity.VisitID.ToString();
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            
            BindGridDetail();
        }

        private void BindGridDetail()
        {
            List<vPatientSurgery> lst = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY PatientSurgeryID DESC", hdnVisitID.Value));
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