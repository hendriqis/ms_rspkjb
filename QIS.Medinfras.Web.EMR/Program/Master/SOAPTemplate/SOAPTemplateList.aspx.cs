using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class SOAPTemplateList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            String MenuID = Request.QueryString["id"];
            switch (MenuID)
            {
                default: return Constant.MenuCode.EMR.MASTER_PHYSICIAN_SOAP_TEMPLATE;
            }
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnProcedureGroupID.Value = keyValue;
            BindGridView(1, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Kode Template", "Nama Template" };
            fieldListValue = new string[] { "TemplateCode", "TemplateName" };
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("UserID = {0} AND IsDeleted = 0 ", AppSession.UserLogin.UserID);

            if (hdnFilterExpression.Value != "")
            {
                filterExpression += "AND " + hdnFilterExpression.Value;
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPhysicianTextRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPhysicianText> lstEntity = BusinessLayer.GetvPhysicianTextList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TemplateCode ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            int pageCount = 1;
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
           
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

                result += "|" + pageCount;
                ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
                panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            String MenuID = Request.QueryString["id"];
            url = ResolveUrl(string.Format("~/Libs/Program/Master/ProcedureGroup/ProcedureGroupEntry.aspx?id={0}", MenuID));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            String MenuID = Request.QueryString["id"];
            if (hdnProcedureGroupID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Libs/Program/Master/ProcedureGroup/ProcedureGroupEntry.aspx?id={0}|{1}", MenuID, hdnProcedureGroupID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ProcedureGroupDao entityDao = new ProcedureGroupDao(ctx);
            try
            {
                if (hdnProcedureGroupID.Value.ToString() != "")
                {
                    ProcedureGroup entity = entityDao.Get(Convert.ToInt32(hdnProcedureGroupID.Value));
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);
                    ctx.CommitTransaction();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}