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

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class NutritionNotesList : BasePagePatientPageList
    {
        protected int PageCount = 1;
        string menuType = string.Empty;
        
        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.FOLLOWUP_NUTRITION_NOTES;
                }
                return Constant.MenuCode.Inpatient.FOLLOWUP_NUTRITION_NOTES;
            }
            else
            {
                switch (hdnDepartmentID.Value)
                {
                    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_PAGE_NUTRITION_NOTES;
                }
                return Constant.MenuCode.Inpatient.PATIENT_PAGE_NUTRITION_NOTES;
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
            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                if (param.Length > 1)
                {
                    hdnDepartmentID.Value = param[0];
                    menuType = param[1];
                }
                else
                {
                    hdnDepartmentID.Value = Page.Request.QueryString["id"];
                }
            }

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = String.Format("IsDeleted = 0 AND VisitID = {0}", hdnVisitID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvNutritionNotesRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vNutritionNotes> lstEntity = BusinessLayer.GetvNutritionNotesList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID ASC");
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
            url = ResolveUrl("~/Program/NutritionNotes/NutritionNotesCtl.ascx");
            queryString = String.Format("add|{0}", hdnVisitID.Value);
            popupWidth = 800;
            popupHeight = 400;
            popupHeaderText = "Catatan Nutrisi";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Program/NutritionNotes/NutritionNotesCtl.ascx");
                queryString = String.Format("edit|{0}|{1}", hdnVisitID.Value, hdnID.Value);
                popupWidth = 800;
                popupHeight = 400;
                popupHeaderText = "Catatan Nutrisi";
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                NutritionNotes entity = BusinessLayer.GetNutritionNotes(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNutritionNotes(entity);
                return true;
            }
            return false;
        }
    }
}