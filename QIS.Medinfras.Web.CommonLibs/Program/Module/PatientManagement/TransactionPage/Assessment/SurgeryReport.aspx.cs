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
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SurgeryReport : BasePagePatientPageList
    {
        protected int PageCount = 1;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_SURGERY_REPORT;
                default: return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_SURGERY_REPORT;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnDepartmentID.Value = entityVisit.DepartmentID;
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnMRN.Value = Convert.ToString(entityVisit.MRN);
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientSurgeryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientSurgery> lstEntity = BusinessLayer.GetvPatientSurgeryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/SurgeryReportEntryCtl.ascx");
            queryString = "add"+'|'+hdnVisitID.Value+'|'+hdnMRN.Value;
            popupWidth = 1250;
            popupHeight = 500;
            popupHeaderText = "Entry Patient Surgery";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/SurgeryReportEntryCtl.ascx");
                queryString = "edit"+'|'+hdnID.Value;
                popupWidth = 1250;
                popupHeight = 500;
                popupHeaderText = "Edit Patient Surgery";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                PatientSurgery entity = BusinessLayer.GetPatientSurgery(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientSurgery(entity);
                return true;
            }
            return false;
        }
    }
}