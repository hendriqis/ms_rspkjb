using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class BPJSKecamatanList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.BPJS_KECAMATAN;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvBPJSKecamatanRowIndex(filterExpression, keyValue, "NamaKecamatan ASC") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Kode Kecamatan", "Nama Kecamatan", "Nama Kabupaten", "Propinsi"};
            fieldListValue = new string[] { "KodeKecamatan", "NamaKecamatan", "NamaKabupaten", "Propinsi"};
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
                int rowCount = BusinessLayer.GetvBPJSKecamatanRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vBPJSKecamatan> lstEntity = BusinessLayer.GetvBPJSKecamatanList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "NamaKecamatan ASC");
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
                url = ResolveUrl("~/Program/Master/Kecamatan/BPJSKecamatanEntry.aspx");
                return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/Master/Kecamatan/BPJSKecamatanEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (!String.IsNullOrEmpty(hdnID.Value))
            {
                Kecamatan entity = BusinessLayer.GetKecamatanList(string.Format("KecamatanID='{0}'", hdnID.Value)).FirstOrDefault();
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateKecamatan(entity);
                return true;
            }
            return false;
        }
    }
}