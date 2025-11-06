using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicalResumeList1 : BasePagePatientPageList
    {
        protected int PageCount = 1;
        protected static bool _isMedicalResumeExists = false;

        public override string OnGetMenuCode()
        {
            if (hdnMenuType.Value == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (hdnDeptType.Value)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_MEDICAL_RESUME;
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_MEDICAL_RESUME;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_MEDICAL_RESUME;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_MEDICAL_RESUME;
                    default:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_MEDICAL_RESUME;
                }
                #endregion
            }
            else if (hdnMenuType.Value == "dp")
            {
                #region Data Pemeriksaan Pasien
                switch (hdnDeptType.Value)
                {
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.DATA_PATIENT_MEDICAL_RESUME;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_MEDICAL_RESUME;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_MEDICAL_RESUME;
                    case Constant.Facility.PHARMACY:
                        return Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_MEDICAL_RESUME;
                    default:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_MEDICAL_RESUME;
                }
                #endregion
            }
            else
            {
                if (hdnDeptType.Value == Constant.Facility.EMERGENCY)
                    return Constant.MenuCode.EmergencyCare.MEDICAL_RESUME;
                else if (hdnDeptType.Value == Constant.Facility.INPATIENT)
                {
                    if (hdnMenuType.Value == "fo")
                        return Constant.MenuCode.Inpatient.FOLLOWUP_MEDICAL_RESUME;
                    else return Constant.MenuCode.Inpatient.MEDICAL_RESUME;
                }
                else if (hdnDeptType.Value == Constant.Facility.OUTPATIENT)
                {
                    if (hdnMenuType.Value == "fo")
                        return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_MEDICAL_RESUME;
                    else if (hdnMenuType.Value == "dp")
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_MEDICAL_RESUME;
                    else return Constant.MenuCode.Outpatient.MEDICAL_RESUME;
                }
                else if (hdnDeptType.Value == Constant.Facility.PHARMACY)
                {
                    if (hdnMenuType.Value == "dp")
                        return Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_MEDICAL_RESUME;
                    else
                        return Constant.MenuCode.Pharmacy.PHARMACY_MEDICAL_RESUME;
                }
                else return Constant.MenuCode.Outpatient.MEDICAL_RESUME;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override bool IsEntryUsePopup()
        {
            return false;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            //if (Page.Request.QueryString["id"] != null)
            //{
            //    hdnModuleID.Value = Page.Request.QueryString["id"];
            //}
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                hdnDeptType.Value = param[0];
                hdnMenuType.Value = param[1];
            }
            else
            {
                hdnDeptType.Value = param[0];
            }
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvChiefComplaintRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vMedicalResume> lstEntity = BusinessLayer.GetvMedicalResumeList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            _isMedicalResumeExists = lstEntity.Count > 0;
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            bool result = true;

            string MedicalEntry = string.Format("~/Libs/Program/Module/PatientManagement/TransactionPage/PatientDisposition/MedicalResume/MedicalResumeEntry1.aspx?id={0}|{1}|{2}", hdnDeptType.Value, hdnMenuType.Value, hdnID.Value);

            url = ResolveUrl(MedicalEntry);
            result = true;
            
            return result;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void cbpCompleted_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = CompletedAssessment(param);
                string[] retValInfo = retVal.Split('|');
                if (retValInfo[0] == "1")
                    result += string.Format("success|{0}", string.Empty);
                else
                    result += string.Format("fail|{0}", retValInfo[1]);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = param;
        }

        private string CompletedAssessment(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                NurseChiefComplaint obj = BusinessLayer.GetNurseChiefComplaint(id);
                if (obj != null)
                {
                    if (string.IsNullOrEmpty(obj.GCAssessmentStatus) || obj.GCAssessmentStatus == Constant.AssessmentStatus.OPEN )
                    {
                        obj.GCAssessmentStatus = Constant.AssessmentStatus.COMPLETED;
                        obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                        obj.LastUpdatedDate = DateTime.Now;
                        BusinessLayer.UpdateNurseChiefComplaint(obj);
                        result = string.Format("1|{0}", string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}", ex.Message);
            }
            finally
            {
            }
            return result;
        }
    }
}