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
    public partial class GLAccountReceivableList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            switch (Request.QueryString["id"])
            {
                case "PRO": return Constant.MenuCode.Accounting.GL_AR_PROCESS;
                case "INS": return Constant.MenuCode.Accounting.GL_AR_INSTANSI;
                case "ADJ": return Constant.MenuCode.Accounting.GL_AR_ADJUSTMENT;
                default: return Constant.MenuCode.Accounting.GL_AR_PERAWATAN;
            }
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            switch (Request.QueryString["id"])
            {
                case "PRO": hdnGCAccountReceivableType.Value = Constant.GCAccountReceivableType.PIUTANG_DALAM_PROSES; break;
                case "PER": hdnGCAccountReceivableType.Value = Constant.GCAccountReceivableType.PIUTANG_DALAM_PERAWATAN; break;
                case "INS": hdnGCAccountReceivableType.Value = Constant.GCAccountReceivableType.PIUTANG_INSTANSI; break;
                case "ADJ": hdnGCAccountReceivableType.Value = Constant.GCAccountReceivableType.PENYESUAIAN_PIUTANG; break;
            }
            filterExpression = GetFilterExpression();

            if (keyValue != "")
            {
                int row = BusinessLayer.GetGLAccountReceivableRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Kode", "Nama"};
            fieldListValue = new string[] { "GLAccountReceivableCode", "GLAccountReceivableName" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += String.Format("GCAccountReceivableType = '{0}' AND  IsDeleted = 0",hdnGCAccountReceivableType.Value);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetGLAccountReceivableRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vGLAccountReceivable> lstEntity = BusinessLayer.GetvGLAccountReceivableList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            url = ResolveUrl(string.Format("~/Program/GLSetting/GLAccountReceivable/GLAccountReceivableEntry.aspx?id={0}", hdnGCAccountReceivableType.Value));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/GLSetting/GLAccountReceivable/GLAccountReceivableEntry.aspx?id={0}|{1}", hdnGCAccountReceivableType.Value, hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                GLAccountReceivable entity = BusinessLayer.GetGLAccountReceivable(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateGLAccountReceivable(entity);
                return true;
            }
            return false;
        }
    }
}