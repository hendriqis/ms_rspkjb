using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLOPRevenueSharingAccountList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            switch (Page.Request.QueryString["ID"])
            {
                case "ER": return Constant.MenuCode.Accounting.GL_ER_REVENUE_SHARING_ACCOUNT;
                default: return Constant.MenuCode.Accounting.GL_OP_REVENUE_SHARING_ACCOUNT;
            }
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            if (Page.Request.QueryString["ID"] != "OP" && hdnFilterExpression.Value == "")
            {
                String HealthcareServiceUnitID = (BusinessLayer.GetvHealthcareServiceUnitList(String.Format("DepartmentID = '{0}'", Constant.Facility.EMERGENCY))[0]).HealthcareServiceUnitID.ToString();
                hdnFilterExpression.Value += String.Format("HealthcareServiceUnitID = {0}", HealthcareServiceUnitID);
            }
            else if (hdnFilterExpression.Value == "")
            {
                String HealthcareServiceUnitID = (BusinessLayer.GetvHealthcareServiceUnitList(String.Format("DepartmentID = '{0}'", Constant.Facility.EMERGENCY))[0]).HealthcareServiceUnitID.ToString();
                hdnFilterExpression.Value += String.Format("HealthcareServiceUnitID != {0}", HealthcareServiceUnitID);
            } 

            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();

            if (keyValue != "")
            {
                int row = BusinessLayer.GetvGLOPRevenueSharingAccountRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Department", "Komponen Sharing" };
            fieldListValue = new string[] { "DepartmentName", "SharingComponent"};
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("IsDeleted = 0");
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvGLOPRevenueSharingAccountRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vGLOPRevenueSharingAccount> lstEntity = BusinessLayer.GetvGLOPRevenueSharingAccountList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl(String.Format("~/Program/GLSetting/GLOPRevenueSharingAccount/GLOPRevenueSharingAccountEntry.aspx?id={0}", Page.Request.QueryString["ID"]));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(String.Format("~/Program/GLSetting/GLOPRevenueSharingAccount/GLOPRevenueSharingAccountEntry.aspx?id={0}|{1}", Page.Request.QueryString["ID"], hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                GLOPRevenueSharingAccount entity = BusinessLayer.GetGLOPRevenueSharingAccount(Convert.ToInt32(hdnID.Value));
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateGLOPRevenueSharingAccount(entity);
                return true;
            }
            return false;
        }
    }
}