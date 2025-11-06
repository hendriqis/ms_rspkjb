using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CompoundTemplateList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            String moduleID = Request.QueryString["id"];
            switch (moduleID)
            {
                case Constant.Module.PHARMACY: return Constant.MenuCode.Pharmacy.COMPOUND_TEMPLATE;
                case Constant.Module.EMR: return Constant.MenuCode.EMR.COMPOUND_TEMPLATE;
                default: return Constant.MenuCode.EMR.COMPOUND_TEMPLATE;
            }
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            String TemplateType = Request.QueryString["id"];
            hdnTemplateType.Value = TemplateType;

            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvCompoundTemplateHdRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Template Name", "Template Code" };
            fieldListValue = new string[] { "CompoundTemplateName", "CompoundTemplateCode" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            //string GCTemplateGroup = "";
            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += "IsDeleted = 0";
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvCompoundTemplateHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vCompoundTemplateHd> lstEntity = BusinessLayer.GetvCompoundTemplateHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "CompoundTemplateCode ASC");
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
            url = ResolveUrl(string.Format("~/Libs/Program/Master/CompoundTemplate/CompoundTemplateEntry.aspx?id={0}", hdnTemplateType.Value));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Libs/Program/Master/CompoundTemplate/CompoundTemplateEntry.aspx?id={0}|{1}", hdnTemplateType.Value, hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                CompoundTemplateHd entity = BusinessLayer.GetCompoundTemplateHd(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateCompoundTemplateHd(entity);
                return true;
            }
            return false;
        }
    }
}