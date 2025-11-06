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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ReviewOfSystemList : BasePagePatientPageList
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                    switch (deptType)
                    {
                        case Constant.Facility.INPATIENT:
                            return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_PAGE_REVIEW_OF_SYSTEM;
                        case Constant.Facility.OUTPATIENT:
                            return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_PAGE_REVIEW_OF_SYSTEM;
                        case Constant.Facility.EMERGENCY:
                            return Constant.MenuCode.EmergencyCare.FOLLOWUP_PATIENT_PAGE_REVIEW_OF_SYSTEM;
                        case Constant.Facility.DIAGNOSTIC:
                            return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_REVIEW_OF_SYSTEM;
                        default: return Constant.MenuCode.EMR.REVIEW_OF_SYSTEM;
                    }
                #endregion
            }
            else if (menuType == "dp")
            {
                #region Data Pemeriksaan Pasien
                switch (deptType)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_REVIEW_OF_SYSTEM;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.DATA_PATIENT_REVIEW_OF_SYSTEM;
                    default: return Constant.MenuCode.Outpatient.DATA_PATIENT_REVIEW_OF_SYSTEM;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_REVIEW_OF_SYSTEM;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.PATIENT_PAGE_REVIEW_OF_SYSTEM;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_REVIEW_OF_SYSTEM;
                    case Constant.Facility.DIAGNOSTIC:
                        if (hdnSubMenuType.Value == "nt")
                            return Constant.MenuCode.Nutrition.NUTRITION_REVIEW_OF_SYSTEM;
                        else
                            return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_REVIEW_OF_SYSTEM;
                    case Constant.Facility.LABORATORY:
                        return Constant.MenuCode.Laboratory.PATIENT_PAGE_REVIEW_OF_SYSTEM;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_REVIEW_OF_SYSTEM;
                    default: return Constant.MenuCode.EMR.REVIEW_OF_SYSTEM;
                }
                #endregion
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
                hdnDepartmentID.Value = param[0];
                hdnSubMenuType.Value = param[1];
                if (param.Length > 1)
                {
                    hdnSubMenuType.Value = param[1];
                    deptType = param[0];
                    menuType = param[1];
                }
                else
                {
                    deptType = param[0];
                }
            }
            else
            {
                hdnDepartmentID.Value = string.Empty;
                hdnSubMenuType.Value = string.Empty;
            }
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            int cvLinkedID = 0;
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                if (entityLinkedRegistration != null)
                {
                    cvLinkedID = entityLinkedRegistration.VisitID;
                }
            }

            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0},{1}) AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, cvLinkedID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvReviewOfSystemHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vReviewOfSystemHd> lstEntity = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex,"ID DESC");
            lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID IN ({0},{1})", AppSession.RegisteredPatient.VisitID, cvLinkedID));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vReviewOfSystemHd obj = (vReviewOfSystemHd)e.Row.DataItem;
                Repeater rptReviewOfSystemDt = (Repeater)e.Row.FindControl("rptReviewOfSystemDt");
                rptReviewOfSystemDt.DataSource = GetVitalSignDt(obj.ID);
                rptReviewOfSystemDt.DataBind();
            }
        }

        protected List<vReviewOfSystemDt> GetVitalSignDt(Int32 ID)
        {
            return lstReviewOfSystemDt.Where(p => p.ID == ID).ToList();
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
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/ReviewOfSystem/ReviewOfSystemEntryCtl.ascx");
            queryString = "";
            popupWidth = 700;
            popupHeight = 500;
            popupHeaderText = "Review Of System";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/ReviewOfSystem/ReviewOfSystemEntryCtl.ascx");
                queryString = hdnID.Value;
                popupWidth = 800;
                popupHeight = 640;
                popupHeaderText = "Review Of System";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                ReviewOfSystemHd entity = BusinessLayer.GetReviewOfSystemHd(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateReviewOfSystemHd(entity);
                return true;
            }
            return false;
        }
    }
}