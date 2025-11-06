using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class CSSDConsumptionEntryQuickPickCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private CSSDHandoverConfirmationDetailList DetailPage
        {
            get { return (CSSDHandoverConfirmationDetailList)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnTransactionID.Value = temp[0];
            hdnLocationID.Value = temp[1];
            hdnGCLocationGroup.Value = temp[2];
            hdnServiceRequestIDCtl.Value = temp[3];
            BindGridView(1, true, ref PageCount);
        }

        protected void cbpPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        private string GetFilterExpression()
        {
            string filterExpression = "";
            if (hdnGCLocationGroup.Value != "")
            {
                if (hdnGCLocationGroup.Value == Constant.LocationGroup.DRUG_AND_MEDICAL_SUPPLIES)
                {
                    if (hdnItemGroupDrugLogisticID.Value == "")
                        filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}') AND ItemName1 LIKE '%{3}%' AND IsDeleted = 0",
                            hdnLocationID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, hdnFilterItem.Value);
                    else
                        filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}') AND ItemName1 LIKE '%{3}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{4}/%') AND IsDeleted = 0",
                            hdnLocationID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);
                }
                else
                {
                    if (hdnItemGroupDrugLogisticID.Value == "")
                        filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}') AND ItemName1 LIKE '%{2}%' AND IsDeleted = 0",
                            hdnLocationID.Value, Constant.ItemType.BARANG_UMUM, hdnFilterItem.Value);
                    else
                        filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}') AND ItemName1 LIKE '%{2}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{3}/%') AND IsDeleted = 0",
                            hdnLocationID.Value, Constant.ItemType.BARANG_UMUM, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);
                }
            }
            else
            {
                if (hdnItemGroupDrugLogisticID.Value == "")
                    filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}', '{3}') AND ItemName1 LIKE '%{4}%' AND IsDeleted = 0",
                        hdnLocationID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, hdnFilterItem.Value);
                else
                    filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}', '{3}') AND ItemName1 LIKE '%{4}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{5}/%') AND IsDeleted = 0",
                        hdnLocationID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);
            }

            filterExpression += " AND QuantityEND > 0";

            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemBalance entity = e.Row.DataItem as vItemBalance;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (hdnTransactionID.Value != "0" && hdnTransactionID.Value != "")
            {
                List<vItemTransactionDt> lstItemID = BusinessLayer.GetvItemTransactionDtList(string.Format("TransactionID = {0} AND GCItemDetailStatus != '{1}'", hdnTransactionID.Value, Constant.TransactionStatus.VOID));
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (vItemTransactionDt itm in lstItemID)
                        lstSelectedID += "," + itm.ItemID;
                    filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
                }
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vItemBalance> lstEntity = BusinessLayer.GetvItemBalanceList(filterExpression, 10, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemTransactionHdDao entityHdDao = new ItemTransactionHdDao(ctx);
            ItemTransactionDtDao entityDtDao = new ItemTransactionDtDao(ctx);
            try
            {
                string filterCons = String.Format("TransactionCode = '{0}' AND ServiceRequestID = {1} AND GCTransactionStatus != '{2}' AND TransactionID != {3}", Constant.TransactionCode.SERVICE_CONSUMPTION, hdnServiceRequestIDCtl.Value, Constant.TransactionStatus.VOID, hdnTransactionID.Value);
                List<ItemTransactionHd> lstConsumptionHd = BusinessLayer.GetItemTransactionHdList(filterCons);

                if (lstConsumptionHd.Count > 0)
                {
                    result = false;
                    errMessage = "Tidak bisa menyimpan pemakaian CSSD karena sudah ada proses pemakaian di nomor <b>" + lstConsumptionHd.FirstOrDefault().TransactionNo + "</b>";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                else
                {
                    lstSelectedMember = hdnSelectedMember.Value.Split(',');
                    string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                    string[] lstSelectedMemberAveragePrice = hdnSelectedMemberAveragePrice.Value.Split(',');
                    int TransactionID = 0;
                    DetailPage.SaveItemConsumptionHd(ctx, ref TransactionID);
                    if (entityHdDao.Get(TransactionID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        List<ItemMaster> lstItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);
                        int ct = 0;
                        foreach (String itemID in lstSelectedMember)
                        {
                            ItemMaster itemMaster = lstItemMaster.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));

                            ItemTransactionDt entityDt = new ItemTransactionDt();
                            entityDt.TransactionID = TransactionID;
                            entityDt.ItemID = itemMaster.ItemID;
                            entityDt.Quantity = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                            entityDt.GCItemUnit = itemMaster.GCItemUnit;
                            entityDt.GCBaseUnit = itemMaster.GCItemUnit;
                            entityDt.ConversionFactor = 1;
                            entityDt.BaseQuantity = entityDt.Quantity * entityDt.ConversionFactor;
                            entityDt.CostAmount = Convert.ToDecimal(lstSelectedMemberAveragePrice[ct]);
                            entityDt.Remarks = "";
                            entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Insert(entityDt);
                            ct++;
                        }
                        retval = TransactionID.ToString();
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        errMessage = "Pemakaian barang tidak dapat diubah. Harap refresh halaman ini.";
                        result = false;
                        ctx.RollBackTransaction();
                    }
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