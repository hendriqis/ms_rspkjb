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
    public partial class ItemDistributionQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private ItemDistribution DetailPage
        {
            get { return (ItemDistribution)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnTransactionID.Value = temp[0];
            hdnLocationID.Value = temp[1];
            hdnGCLocationGroup.Value = temp[2];
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
                        filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}') AND ItemName1 LIKE '%{3}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath LIKE '%/{4}/%') AND IsDeleted = 0",
                            hdnLocationID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);
                }
                else
                {
                    if (hdnItemGroupDrugLogisticID.Value == "")
                        filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}') AND ItemName1 LIKE '%{2}%' AND IsDeleted = 0",
                            hdnLocationID.Value, Constant.ItemType.BARANG_UMUM, hdnFilterItem.Value);
                    else
                        filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}') AND ItemName1 LIKE '%{2}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath LIKE '%/{3}/%') AND IsDeleted = 0",
                            hdnLocationID.Value, Constant.ItemType.BARANG_UMUM, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);
                }
            }
            else
            {
                if (hdnItemGroupDrugLogisticID.Value == "")
                    filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}', '{3}', '{4}') AND ItemName1 LIKE '%{5}%' AND IsDeleted = 0",
                        hdnLocationID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN, hdnFilterItem.Value);
                else
                    filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}', '{3}', '{4}') AND ItemName1 LIKE '%{5}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath LIKE '%/{6}/%') AND IsDeleted = 0",
                        hdnLocationID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);
            }

            filterExpression += string.Format(" AND GCItemStatus != '{0}'", Constant.ItemStatus.IN_ACTIVE);

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
                List<vItemDistributionDt> lstItemID = BusinessLayer.GetvItemDistributionDtList(string.Format("DistributionID = {0} AND GCItemDetailStatus != '{1}'", hdnTransactionID.Value, Constant.TransactionStatus.VOID));
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (vItemDistributionDt itm in lstItemID)
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
            ItemDistributionHdDao entityHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao entityDtDao = new ItemDistributionDtDao(ctx);
            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                int TransactionID = 0;
                DetailPage.SaveItemDistributionHd(ctx, ref TransactionID);
                if (entityHdDao.Get(TransactionID).GCDistributionStatus == Constant.DistributionStatus.OPEN)
                {
                    List<ItemMaster> lstItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);
                    int ct = 0;
                    string itemNameStockEmpty = "";
                    foreach (String itemID in lstSelectedMember)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        ItemMaster itemMaster = lstItemMaster.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));

                        #region check Qty
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        string filterBalance = string.Format("ItemID = '{0}' AND LocationID = '{1}' AND IsDeleted = 0", itemID, hdnLocationID.Value);
                        ItemBalance balance = BusinessLayer.GetItemBalanceList(filterBalance, ctx).FirstOrDefault();
                        if (balance != null)
                        {
                            if (balance.QuantityEND <= 0)
                            {
                                if (!string.IsNullOrEmpty(itemNameStockEmpty))
                                {
                                    itemNameStockEmpty += string.Format(", {0}", itemMaster.ItemName1);
                                }
                                else
                                {
                                    itemNameStockEmpty = itemMaster.ItemName1;
                                }
                            }
                            else
                            {
                                if (Convert.ToDecimal(lstSelectedMemberQty[ct]) > balance.QuantityEND)
                                {
                                    if (!string.IsNullOrEmpty(itemNameStockEmpty))
                                    {
                                        itemNameStockEmpty += string.Format(", {0}", itemMaster.ItemName1);
                                    }
                                    else
                                    {
                                        itemNameStockEmpty = itemMaster.ItemName1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(itemNameStockEmpty))
                            {
                                itemNameStockEmpty += string.Format(", {0}", itemMaster.ItemName1);
                            }
                            else
                            {
                                itemNameStockEmpty = itemMaster.ItemName1;
                            }
                        }
                        #endregion

                        ItemDistributionDt entityDt = new ItemDistributionDt();
                        entityDt.DistributionID = TransactionID;
                        entityDt.ItemID = itemMaster.ItemID;
                        entityDt.Quantity = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                        entityDt.GCItemUnit = itemMaster.GCItemUnit;
                        entityDt.GCBaseUnit = itemMaster.GCItemUnit;
                        entityDt.ConversionFactor = 1;
                        entityDt.Remarks = "";
                        entityDt.GCItemDetailStatus = Constant.DistributionStatus.OPEN;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;

                        ItemPlanning entityPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0}", itemMaster.ItemID)).FirstOrDefault();
                        if (entityPlanning != null)
                        {
                            entityDt.AveragePrice = entityPlanning.AveragePrice;
                        }
                        else
                        {
                            entityDt.AveragePrice = 0;
                        }

                        entityDtDao.Insert(entityDt);
                        ct++;
                    }

                    if (String.IsNullOrEmpty(itemNameStockEmpty))
                    {
                        retval = TransactionID.ToString();
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        errMessage = string.Format("Stok untuk item - item {0} tidak mencukupi", itemNameStockEmpty);
                        result = false;
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    errMessage = "Distribusi tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                result = false;
                errMessage = ex.Message;
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