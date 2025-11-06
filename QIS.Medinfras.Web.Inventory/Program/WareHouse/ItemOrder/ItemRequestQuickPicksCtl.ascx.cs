using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemRequestQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private ItemRequest DetailPage
        {
            get { return (ItemRequest)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnTransactionID.Value = temp[0];
            hdnItemLocationIs.Value = temp[1];
            hdnIsFilterQtyOnHand.Value = temp[2];
            hdnLocationIDFromCtl.Value = temp[3];
            hdnGCLocationGroupFromCtl.Value = temp[4];
            hdnFromLocationItemGroupIDCtl.Value = temp[5];
            hdnLocationIDToCtl.Value = temp[6];
            hdnGCLocationGroupToCtl.Value = temp[7];
            hdnToLocationItemGroupIDCtl.Value = temp[8];
            hdnProductLineIDCtl.Value = temp[9];
            hdnProductLineItemTypeCtl.Value = temp[10];
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
            
            if (hdnProductLineItemTypeCtl.Value != "")
            {
                if (hdnProductLineIDCtl.Value != "")
                {
                    filterExpression += string.Format("ProductLineID = {0}", hdnProductLineIDCtl.Value);
                }
                else
                {
                    filterExpression += string.Format("ProductLineID = 0");
                }
            }

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }

            if (hdnItemLocationIs.Value == "from")
            {
                if (hdnGCLocationGroupFromCtl.Value != "")
                {
                    if (hdnGCLocationGroupFromCtl.Value == Constant.LocationGroup.DRUG_AND_MEDICAL_SUPPLIES)
                    {
                        if (hdnItemGroupDrugLogisticID.Value == "")
                        {
                            filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}', '{3}') AND ItemName1 LIKE '%{4}%' AND IsDeleted = 0",
                                hdnLocationIDFromCtl.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BAHAN_MAKANAN, hdnFilterItem.Value);
                        }
                        else
                        {
                            filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}', '{3}') AND ItemName1 LIKE '%{4}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath LIKE '%/{5}/%') AND IsDeleted = 0",
                                hdnLocationIDFromCtl.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);
                        }
                    }
                    else
                    {
                        if (hdnItemGroupDrugLogisticID.Value == "")
                        {
                            filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}','{2}') AND ItemName1 LIKE '%{2}%' AND IsDeleted = 0",
                                hdnLocationIDFromCtl.Value, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN, hdnFilterItem.Value);
                        }
                        else
                        {
                            filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}','{2}') AND ItemName1 LIKE '%{3}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath LIKE '%/{4}/%') AND IsDeleted = 0",
                                hdnLocationIDFromCtl.Value, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);
                        }
                    }
                }
                else
                {
                    if (hdnItemGroupDrugLogisticID.Value == "")
                    {
                        filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}', '{3}', '{4}') AND ItemName1 LIKE '%{5}%' AND IsDeleted = 0",
                            hdnLocationIDFromCtl.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN, hdnFilterItem.Value);
                    }
                    else
                    {
                        filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}', '{3}','{4}') AND ItemName1 LIKE '%{5}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath LIKE '%/{6}/%') AND IsDeleted = 0",
                            hdnLocationIDFromCtl.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);
                    }
                }
            }
            else
            {
                if (hdnGCLocationGroupToCtl.Value != "")
                {
                    if (hdnGCLocationGroupToCtl.Value == Constant.LocationGroup.DRUG_AND_MEDICAL_SUPPLIES)
                    {
                        if (hdnItemGroupDrugLogisticID.Value == "")
                        {
                            filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}') AND ItemName1 LIKE '%{3}%' AND IsDeleted = 0",
                                hdnLocationIDToCtl.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, hdnFilterItem.Value);
                        }
                        else
                        {
                            filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}') AND ItemName1 LIKE '%{3}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath LIKE '%/{4}/%') AND IsDeleted = 0",
                                hdnLocationIDToCtl.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);
                        }
                    }
                    else
                    {
                        if (hdnItemGroupDrugLogisticID.Value == "")
                        {
                            filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}','{2}') AND ItemName1 LIKE '%{3}%' AND IsDeleted = 0",
                                hdnLocationIDToCtl.Value, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN, hdnFilterItem.Value);
                        }
                        else
                        {
                            filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}','{2}') AND ItemName1 LIKE '%{3}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath LIKE '%/{4}/%') AND IsDeleted = 0",
                                hdnLocationIDToCtl.Value, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);
                        }
                    }
                }
                else
                {
                    if (hdnItemGroupDrugLogisticID.Value == "")
                    {
                        filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}', '{3}', '{4}') AND ItemName1 LIKE '%{5}%' AND IsDeleted = 0",
                            hdnLocationIDToCtl.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM,Constant.ItemType.BAHAN_MAKANAN, hdnFilterItem.Value);
                    }
                    else
                    {
                        filterExpression += string.Format("LocationID IN ('{0}') AND GCItemType IN ('{1}', '{2}', '{3}', '{4}') AND ItemName1 LIKE '%{5}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath LIKE '%/{6}/%') AND IsDeleted = 0",
                            hdnLocationIDToCtl.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);
                    }
                }
            }

            filterExpression += string.Format(" AND GCItemStatus != '{0}'", Constant.ItemStatus.IN_ACTIVE);

            if (hdnIsFilterQtyOnHand.Value == "1")
            {
                filterExpression += string.Format(" AND ItemID IN (SELECT ItemID FROM ItemBalance WHERE LocationID = {0} AND IsDeleted = 0)", hdnLocationIDToCtl.Value);
            }

            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemBalanceQuickPick2 entity = e.Row.DataItem as vItemBalanceQuickPick2;
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
                List<vItemRequestDt> lstItemID = BusinessLayer.GetvItemRequestDtList(string.Format("ItemRequestID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0", hdnTransactionID.Value, Constant.TransactionStatus.VOID));
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (vItemRequestDt itm in lstItemID)
                        lstSelectedID += "," + itm.ItemID;
                    filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
                }
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vItemBalanceQuickPick2> lstEntity = BusinessLayer.GetvItemBalanceQuickPick2List(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemRequestHdDao entityHdDao = new ItemRequestHdDao(ctx);
            ItemRequestDtDao entityDtDao = new ItemRequestDtDao(ctx);
            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                int TransactionID = 0;

                Int32 fromLocationID = Convert.ToInt32(hdnLocationIDFromCtl.Value);
                Int32 ToLocationID = Convert.ToInt32(hdnLocationIDToCtl.Value);

                DetailPage.SaveItemRequestHd(ctx, ref TransactionID);
                if (entityHdDao.Get(TransactionID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    //if (hdnItemLocationIs.Value == "from")
                    //{
                    //    lstItemBalance = BusinessLayer.GetItemBalanceList(string.Format("ItemID IN ({0}) AND LocationID = {1}", hdnSelectedMember.Value, entityHdDao.Get(TransactionID).FromLocationID), ctx);
                    //}
                    //else
                    //{
                    //    lstItemBalance = BusinessLayer.GetItemBalanceList(string.Format("ItemID IN ({0}) AND LocationID = {1}", hdnSelectedMember.Value, entityHdDao.Get(TransactionID).ToLocationID), ctx);
                    //}

                    List<ItemMaster> lstItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);

                    List<ItemBalance> lstItemBalanceFromLocation = null;
                    List<ItemBalance> lstItemBalanceToLocation = null;
                    List<Location> lstLocationFrom = null;
                    List<Location> lstLocationTo = null;
                    List<ItemProduct> lstItemProduct = null;

                    lstItemBalanceFromLocation = BusinessLayer.GetItemBalanceList(string.Format("ItemID IN ({0}) AND LocationID = {1} AND IsDeleted = 0", hdnSelectedMember.Value, entityHdDao.Get(TransactionID).FromLocationID), ctx);
                    lstItemBalanceToLocation = BusinessLayer.GetItemBalanceList(string.Format("ItemID IN ({0}) AND LocationID = {1} AND IsDeleted = 0", hdnSelectedMember.Value, entityHdDao.Get(TransactionID).ToLocationID), ctx);
                    lstLocationFrom = BusinessLayer.GetLocationList(string.Format("LocationID = {0}", entityHdDao.Get(TransactionID).FromLocationID), ctx);
                    lstLocationTo = BusinessLayer.GetLocationList(string.Format("LocationID = {0}", entityHdDao.Get(TransactionID).ToLocationID), ctx);
                    lstItemProduct = BusinessLayer.GetItemProductList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);

                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        ItemMaster itemMaster = lstItemMaster.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));
                        ItemBalance itemBalanceFromLocation = lstItemBalanceFromLocation.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));
                        ItemBalance itemBalanceToLocation = lstItemBalanceToLocation.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));
                        String fromLocation = lstLocationFrom.FirstOrDefault().GCItemRequestType;
                        String toLocation = lstLocationTo.FirstOrDefault().GCItemRequestType;
                        String itemProduct = lstItemProduct.FirstOrDefault().GCItemRequestType;

                        ItemRequestDt entityDt = new ItemRequestDt();
                        entityDt.ItemRequestID = TransactionID;
                        entityDt.ItemID = itemMaster.ItemID;
                        entityDt.Quantity = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                        entityDt.GCItemUnit = itemMaster.GCItemUnit;
                        entityDt.GCBaseUnit = itemMaster.GCItemUnit;
                        entityDt.ConversionFactor = 1;

                        if (itemBalanceFromLocation != null)
                        {
                            entityDt.GCItemRequestType = itemBalanceFromLocation.GCItemRequestType;
                        }
                        else
                        {
                            if (fromLocation != null && fromLocation != "")
                            {
                                entityDt.GCItemRequestType = fromLocation;
                            }
                            else
                            {
                                if (itemBalanceToLocation != null)
                                {
                                    entityDt.GCItemRequestType = itemBalanceToLocation.GCItemRequestType;
                                }
                                else
                                {
                                    if (toLocation != null && fromLocation != "")
                                    {
                                        entityDt.GCItemRequestType = toLocation;
                                    }
                                    else
                                    {
                                        entityDt.GCItemRequestType = itemProduct;
                                    }
                                }
                            }
                        }
                        
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
                    result = false;
                    errMessage = "Permintaan barang tidak dapat diubah. Harap refresh halaman ini.";
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