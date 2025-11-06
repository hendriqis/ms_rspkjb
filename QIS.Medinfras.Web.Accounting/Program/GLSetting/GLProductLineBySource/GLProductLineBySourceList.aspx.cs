using System;
using System.Collections.Generic;
using System.Data;
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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLProductLineBySourceList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_PRODUCT_LINE_ACCOUNT_SOURCEID;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();

            CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "ProductLineName", "DepartmentID", "ServiceUnitName" };
            fieldListValue = new string[] { "ProductLineName", "DepartmentID", "ServiceUnitName" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvGLProductLineBySourceIDRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vGLProductLineBySourceID> lstEntity = BusinessLayer.GetvGLProductLineBySourceIDList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            url = ResolveUrl("~/Program/GLSetting/GLProductLineBySource/GLProductLineBySourceEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/GLSetting/GLProductLineBySource/GLProductLineBySourceEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLProductLineBySourceIDDao entityDao = new GLProductLineBySourceIDDao(ctx);
            try
            {
                if (hdnID.Value.ToString() != "")
                {
                    string[] oID = hdnID.Value.Split('|');
                    Int32 oProductLineID = Convert.ToInt32(oID[0]);
                    Int32 oHealthCareServiceUnitID = Convert.ToInt32(oID[1]);
                    Int32 oSourceHealthCareServiceUnitID = Convert.ToInt32(oID[2]);
                    Int32 oClassID = Convert.ToInt32(oID[3]);

                    GLProductLineBySourceID entityDelete = entityDao.Get(oProductLineID, oHealthCareServiceUnitID, oSourceHealthCareServiceUnitID, oClassID);
                    entityDao.Delete(entityDelete.ProductLineID, entityDelete.HealthCareServiceUnitID, entityDelete.SourceID, entityDelete.ClassID);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "No data to delete.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
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