using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MSTForm : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        string deptType = string.Empty;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_MST_FORM;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_MST_FORM;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_MST_FORM;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_MST_FORM;
                    default:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_MST_FORM;
                }
            }
            else if (menuType == "dp")
            {
                #region Data Pemeriksaan Pasien
                switch (deptType)
                {
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.DATA_PATIENT_MST_FORM;
                    default: return Constant.MenuCode.EmergencyCare.DATA_PATIENT_MST_FORM;
                }
                #endregion
            }
            else if (menuType == "nt")
            {
                return Constant.MenuCode.Nutrition.MST_FORM;
            }
            else
            {
                switch (deptType)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.MST_FORM;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.MST_FORM;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.MST_FORM;
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.MST_FORM;
                    default:
                        return Constant.MenuCode.Outpatient.MST_FORM;
                }
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = true;
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

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = String.Format("IsDeleted = 0 AND VisitID = {0}", hdnVisitID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMSTAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vMSTAssessment> lstEntity = BusinessLayer.GetvMSTAssessmentList(filterExpression);
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
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/MedicalForm/MSTFormEntryCtl.ascx");
            queryString = String.Format("add|{0}", hdnVisitID.Value);
            popupWidth = 600;
            popupHeight = 400;
            popupHeaderText = string.Format("Pengkajian MST : {0}", AppSession.RegisteredPatient.PatientName);
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                MSTAssessment entity = BusinessLayer.GetMSTAssessment(Convert.ToInt32(hdnID.Value));
                if (entity.IsDeleted != true)
                {
                    url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/MedicalForm/MSTFormEntryCtl.ascx");
                    queryString = String.Format("edit|{0}", hdnID.Value);
                    popupWidth = 600;
                    popupHeight = 400;
                    popupHeaderText = string.Format("Pengkajian MST : {0}", AppSession.RegisteredPatient.PatientName);
                    return true;
                }
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            //if (hdnID.Value != "")
            MSTAssessment entity = BusinessLayer.GetMSTAssessment(Convert.ToInt32(hdnID.Value));
            if (hdnID.Value != "" && entity.IsDeleted != true)
            {
                //MSTAssessment entity = BusinessLayer.GetMSTAssessment(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMSTAssessment(entity);
                return true;
            }
            return false;
        }
    }
}