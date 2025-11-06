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
    public partial class PatientDiagnose : BasePagePatientPageList
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType)
                {
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.FOLLOWUP_TRANSACTION_PAGE_PATIENT_DIAGNOSIS;
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.FOLLOWUP_TRANSACTION_PAGE_PATIENT_DIAGNOSIS;
                    default: return Constant.MenuCode.Outpatient.PATIENT_PAGE_FINAL_DIAGNOSIS;
                }            
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.TRANSACTION_PAGE_PATIENT_DIAGNOSIS;
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.TRANSACTION_PAGE_PATIENT_DIAGNOSIS;
                    case Constant.Facility.DIAGNOSTIC:
                        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Nutrition)
                        {
                            if (menuType == "nt") 
                            {
                                    return Constant.MenuCode.Nutrition.NUTRITION_PATIENT_DIAGNOSE;
                            }
                        }
                        return Constant.MenuCode.Nutrition.NUTRITION_PATIENT_DIAGNOSE;
                    case Constant.Facility.PHARMACY:
                            if (menuType == "cp") 
                            {
                                return Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_DIAGNOSE;
                            }
                        return Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_DIAGNOSE;
                    default: return Constant.MenuCode.Outpatient.PATIENT_PAGE_FINAL_DIAGNOSIS;
                }            
                #endregion
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            //IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
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

            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnDepartmentID.Value = entityVisit.DepartmentID;
            BindGridView(1, true, ref PageCount);
        }


        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
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

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientAssessment/PatientDiagnose/PatientDiagnoseCtl.ascx");
            queryString = "";
            popupWidth = 900;
            popupHeight = 500;
            popupHeaderText = "Diagnosa Pasien";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientAssessment/PatientDiagnose/PatientDiagnoseCtl.ascx");
                queryString = hdnID.Value;
                popupWidth = 900;
                popupHeight = 500;
                popupHeaderText = "Diagnosa Pasien";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                int recordID = Convert.ToInt32(hdnID.Value);
                PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(recordID);
                if (entity != null)
                {
                    if (entity.ClaimDiagnosisID != null && entity.ClaimDiagnosisID != "")
                    {
                        errMessage = string.Format("Data diagnosa pasien ini tidak dapat dihapus karena sudah dilengkapi oleh Casemix.");
                        return false;
                    }
                    else
                    {
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePatientDiagnosis(entity);

                        return true;
                    }
                }
                else
                {
                    errMessage = string.Format("Invalid Patient Diagnosis Record Information");
                    return false;
                }
            }
            else
            {
                errMessage = string.Format("Invalid Patient Diagnosis Record Information");
                return false;
            }
        }
    }
}