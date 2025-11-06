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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAItemList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.FA_ITEM;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            hdnMenuCode.Value = OnGetMenuCode();

            List<StandardCode> lstFAAcceptance = new List<StandardCode>();
            lstFAAcceptance.Insert(0, new StandardCode { StandardCodeName = "Semua", StandardCodeID = "0" });
            lstFAAcceptance.Insert(1, new StandardCode { StandardCodeName = "Sudah Dibuat", StandardCodeID = "1" });
            lstFAAcceptance.Insert(2, new StandardCode { StandardCodeName = "Belum Dibuat", StandardCodeID = "2" });
            Methods.SetComboBoxField<StandardCode>(cboStatusFAAcceptance, lstFAAcceptance, "StandardCodeName", "StandardCodeID");
            cboStatusFAAcceptance.SelectedIndex = 0;

            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvFAItemRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
            {
                CurrPage = 1;
            }

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Nama Aset", "Kode Aset", "Nomor Serial", "Kategori Anggaran", "Nomor Anggaran", "Kode Kelompok Aset", "Kode Lokasi", "Nama Lokasi", "Nama Vendor", "No. Faktur Pembelian", "Tanggal Perolehan (dd MMM yyyy)", "Tanggal Akhir Garansi (dd MMM yyyy)" };
            fieldListValue = new string[] { "FixedAssetName", "FixedAssetCode", "SerialNumber", "BudgetCategory", "BudgetPlanNo", "FAGroupCode", "FALocationCode", "FALocationName", "BusinessPartnerName", "ProcurementReferenceNumber", "ProcurementDateInString", "GuaranteeEndDateInString" };
        }

        private string GetFilterExpression()
        {
            string status = "";
            string statusAcceptance = "";
            statusAcceptance = cboStatusFAAcceptance.Value.ToString();

            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += String.Format("IsDeleted = 0 AND GCItemStatus != '{0}'", Constant.ItemStatus.IN_ACTIVE);

            if (statusAcceptance == "1")
            {
                status += string.Format(" AND FAAcceptanceNo IS NOT NULL");
            }
            else if (statusAcceptance == "2")
            {
                status += string.Format(" AND FAAcceptanceNo IS NULL");
            }

            if (statusAcceptance != "0")
            {
                filterExpression += string.Format("{0}", status);
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFAItemRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            string faAcceptanceStatus = cboStatusFAAcceptance.Value.ToString();

            List<vFAItem> lstEntity = BusinessLayer.GetvFAItemList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "FixedAssetCode");
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
            url = ResolveUrl("~/Program/Master/FAItem/FAItemEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/Master/FAItem/FAItemEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                string filterAcceptance = string.Format("FixedAssetID = {0} AND IsDeleted = 0 AND GCTransactionStatus != '{1}'", hdnID.Value, Constant.TransactionStatus.VOID);
                List<vFAAcceptanceDt> lstAcceptance = BusinessLayer.GetvFAAcceptanceDtList(filterAcceptance);
                if (lstAcceptance.Count() > 0)
                {
                    errMessage = string.Format("Master aset ini tidak dapat diubah karena sudah memiliki nomor Berita Acara Aset di nomor <b>{0}</b>", lstAcceptance.FirstOrDefault().FAAcceptanceNo);
                    return false;
                }
                else
                {
                    FAItem entity = BusinessLayer.GetFAItem(Convert.ToInt32(hdnID.Value));
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateFAItem(entity);
                    return true;
                }
            }
            return false;
        }
    }
}