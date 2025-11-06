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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class CashFlowTypeList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.CASH_FLOW_TYPE;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetCashFlowTypeRowIndex(filterExpression, keyValue, "CashFlowTypeCode ASC") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Nama Tipe Arus Kas", "Kode Tipe Arus Kas" };
            fieldListValue = new string[] { "CashFlowTypeName", "CashFlowTypeCode" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
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
                int rowCount = BusinessLayer.GetCashFlowTypeRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vCashFlowType> lstEntity = BusinessLayer.GetvCashFlowTypeList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "CashFlowTypeCode ASC");
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
            url = ResolveUrl("~/Program/Master/CashFlowType/CashFlowTypeEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/Master/CashFlowType/CashFlowTypeEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = false;
            IDbContext ctx = DbFactory.Configure(true);
            CashFlowTypeDao entityDao = new CashFlowTypeDao(ctx);
            try
            {
                if (hdnID.Value.ToString() != "")
                {
                    CashFlowType entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                    string filterExpression = string.Format("ParentTypeID = {0} AND IsDeleted = 0", entity.CashFlowTypeID);
                    List<vCashFlowType> lstEntity = BusinessLayer.GetvCashFlowTypeList(filterExpression, ctx);
                    if (lstEntity.Count() == 0)
                    {
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);

                        result = true;
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Data tidak dapat dihapus. Tipe Arus Kas ini digunakan sebagai Parent dari Tipe Arus Kas lainnya.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "ID Tipe Arus Kas tidak ditemukan.";
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