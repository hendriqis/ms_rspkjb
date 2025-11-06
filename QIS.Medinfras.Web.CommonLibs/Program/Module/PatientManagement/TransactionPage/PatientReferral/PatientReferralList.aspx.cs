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
    public partial class PatientReferralList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        private string pageTitle = string.Empty;
        string menuType = string.Empty;
        string deptType = string.Empty;

        public override string OnGetMenuCode()
        {
            switch (deptType)
            {
                case Constant.Facility.OUTPATIENT:
                    if (menuType == "pt")
                        return Constant.MenuCode.Outpatient.TRANSACTION_PAGE_PATIENT_REFERRAL;
                    else if (menuType == "fo")
                        return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_REFERRAL;
                    else
                        return Constant.MenuCode.Outpatient.INFORMATION_VISIT_PATIENT_REFERRAL;
                case Constant.Facility.INPATIENT:
                    if (menuType == "pt")
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_PATIENT_REFERRAL;
                    else if (menuType == "fo")
                        return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_REFERRAL;
                    else
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_PATIENT_REFERRAL;   
                default:
                    if (menuType == "pt")
                        return Constant.MenuCode.Inpatient.PATIENT_PAGE_PATIENT_REFERRAL;
                    else if (menuType == "fo")
                        return Constant.MenuCode.Outpatient.FOLLOWUP_PATIENT_REFERRAL;
                    else
                        return Constant.MenuCode.Inpatient.INFORMATION_PATIENT_REFERRAL;   
            }
        }

        protected string GetPageTitle()
        {
            return pageTitle;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Count() > 1)
                {
                    deptType = param[0];
                    menuType = param[1];
                }
                else
                {
                    hdnDepartmentID.Value = Page.Request.QueryString["id"];
                    deptType = param[0];
                }
            }
            else
            {
                hdnDepartmentID.Value = string.Empty;
            }

            pageTitle = BusinessLayer.GetMenuMasterList(string.Format("MenuCode= '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("RegistrationID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientReferralRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientReferral> lstView = BusinessLayer.GetvPatientReferralList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            grdView.DataSource = lstView;
            grdView.DataBind();
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = false;
            IsAllowEdit = false;
            IsAllowDelete = false;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
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

        protected override bool OnBeforeEditRecord(ref string errMessage)
        {
            return true;
        }

        protected override bool OnBeforeDeleteRecord(ref string errMessage)
        {
            return true;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            return false;
        }
    }
}