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
    public partial class PurchaseRequestQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        protected string filterExpressionSupplier = "";

        private PurchaseRequest DetailPage
        {
            get { return (PurchaseRequest)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnTransactionID.Value = temp[0];
            hdnLocationIDCtl.Value = temp[1];
            hdnLocationItemGroupIDCtl.Value = temp[2];
            hdnGCLocationGroupCtl.Value = temp[3];
            hdnProductLineIDCtl.Value = temp[4];
            hdnProductLineItemTypeCtl.Value = temp[5];
            hdnIM0131Ctl.Value = temp[6];
            filterExpressionSupplier = string.Format("GCBusinessPartnerType = '{0}' AND IsActive = 1 AND IsDeleted = 0 AND IsBlackList = 0", Constant.BusinessObjectType.SUPPLIER);

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
            string filterExpression = string.Format("LocationID = '{0}' AND ItemName1 LIKE '%{1}%' AND IsDeleted = 0 AND GCItemStatus != '{2}'", hdnLocationIDCtl.Value, hdnFilterItem.Value, Constant.ItemStatus.IN_ACTIVE);

            if (hdnItemGroupDrugLogisticID.Value != "")
            {
                filterExpression += string.Format(" AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath LIKE '%/{0}/%')", hdnItemGroupDrugLogisticID.Value);
            }

            if (hdnProductLineItemTypeCtl.Value != "")
            {
                if (hdnProductLineIDCtl.Value != "")
                {
                    filterExpression += string.Format(" AND ProductLineID = {0}", hdnProductLineIDCtl.Value);
                }
                else
                {
                    filterExpression += string.Format(" AND ProductLineID = 0");
                }
            }
            else
            {
                if (hdnGCLocationGroupCtl.Value != "")
                {
                    if (hdnGCLocationGroupCtl.Value == Constant.LocationGroup.DRUG_AND_MEDICAL_SUPPLIES)
                    {
                        filterExpression += string.Format(" AND GCItemType IN ('{0}','{1}')",
                                Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);
                    }
                    else if (hdnGCLocationGroupCtl.Value == Constant.LocationGroup.LOGISTIC)
                    {
                        filterExpression += string.Format(" AND GCItemType IN ('{0}')",
                                Constant.ItemType.BARANG_UMUM);
                    }
                    else
                    {
                        filterExpression += string.Format(" AND GCItemType IN ('{0}','{1}','{2}','{3}')",
                                Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
                    }
                }
                else
                {
                    filterExpression += string.Format(" AND GCItemType IN ('{0}','{1}','{2}','{3}')",
                            Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
                }
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
                List<PurchaseRequestDt> lstItemID = BusinessLayer.GetPurchaseRequestDtList(string.Format("PurchaseRequestID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0", hdnTransactionID.Value, Constant.TransactionStatus.VOID));
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (PurchaseRequestDt itm in lstItemID)
                        lstSelectedID += "," + itm.ItemID;
                    filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
                }
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceQuickPick2RowCount(filterExpression);
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
            PurchaseRequestHdDao entityHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao entityDtDao = new PurchaseRequestDtDao(ctx);
            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                int TransactionID = 0;
                DetailPage.SavePurchaseRequestHd(ctx, ref TransactionID);
                PurchaseRequestHd entityHd = entityHdDao.Get(TransactionID);
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    int businessPartnerID = 0;
                    if (hdnSupplierID.Value != "")
                        businessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
                    else
                        businessPartnerID = 0;

                    List<vItemBalanceAlternateUnit> lstEntityItemBalance = BusinessLayer.GetvItemBalanceAlternateUnitList(string.Format("LocationID = {0} AND ItemID IN ({1}) AND IsDeleted = 0", entityHdDao.Get(TransactionID).FromLocationID, hdnSelectedMember.Value), ctx);

                    List<ItemMaster> lstItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);
                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        ItemMaster itemMaster = lstItemMaster.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));

                        PurchaseRequestDt entityDt = new PurchaseRequestDt();
                        entityDt.PurchaseRequestID = TransactionID;
                        entityDt.ItemID = itemMaster.ItemID;
                        entityDt.Quantity = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                        entityDt.GCBaseUnit = itemMaster.GCItemUnit;

                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;

                        vItemBalanceAlternateUnit eBalance = lstEntityItemBalance.Where(a => a.ItemID == entityDt.ItemID).FirstOrDefault();
                        entityDt.QtyENDLocation = eBalance.QuantityEND;
                        entityDt.GCItemUnitQtyENDLocation = eBalance.GCItemUnit;

                        if (hdnIM0131Ctl.Value == "0")
                        {
                            List<GetItemMasterPurchase> impList = BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, itemMaster.ItemID, businessPartnerID, ctx);
                            if (impList.Count > 0)
                            {
                                GetItemMasterPurchase imp = impList.FirstOrDefault();
                                if (imp.BusinessPartnerID != 0)
                                {
                                    entityDt.BusinessPartnerID = imp.BusinessPartnerID;
                                }
                                else
                                {
                                    entityDt.BusinessPartnerID = null;
                                }
                                entityDt.GCPurchaseUnit = imp.PurchaseUnit;
                                entityDt.ConversionFactor = imp.ConversionFactor;
                                if (imp.ConversionFactor == 1)
                                {
                                    entityDt.UnitPrice = imp.Price;
                                }
                                else
                                {
                                    entityDt.UnitPrice = imp.UnitPrice;
                                }
                                entityDt.DiscountPercentage = imp.Discount;
                                entityDt.DiscountPercentage2 = imp.Discount2;
                            }
                            else
                            {
                                entityDt.BusinessPartnerID = null;
                                entityDt.UnitPrice = 0;
                                entityDt.DiscountPercentage = 0;
                                entityDt.DiscountPercentage2 = 0;
                                entityDt.GCPurchaseUnit = itemMaster.GCItemUnit;
                                entityDt.ConversionFactor = 1;
                            }
                        }
                        else
                        {
                            List<GetItemMasterPurchaseWithDate> impList = BusinessLayer.GetItemMasterPurchaseWithDateList(AppSession.UserLogin.HealthcareID, itemMaster.ItemID, businessPartnerID, entityHd.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT_112), ctx);
                            if (impList.Count > 0)
                            {
                                GetItemMasterPurchaseWithDate imp = impList.FirstOrDefault();
                                if (imp.BusinessPartnerID != 0)
                                {
                                    entityDt.BusinessPartnerID = imp.BusinessPartnerID;
                                }
                                else
                                {
                                    entityDt.BusinessPartnerID = null;
                                }
                                entityDt.GCPurchaseUnit = imp.PurchaseUnit;
                                entityDt.ConversionFactor = imp.ConversionFactor;
                                if (imp.ConversionFactor == 1)
                                {
                                    entityDt.UnitPrice = imp.Price;
                                }
                                else
                                {
                                    entityDt.UnitPrice = imp.UnitPrice;
                                }
                                entityDt.DiscountPercentage = imp.Discount;
                                entityDt.DiscountPercentage2 = imp.Discount2;
                            }
                            else
                            {
                                entityDt.BusinessPartnerID = null;
                                entityDt.UnitPrice = 0;
                                entityDt.DiscountPercentage = 0;
                                entityDt.DiscountPercentage2 = 0;
                                entityDt.GCPurchaseUnit = itemMaster.GCItemUnit;
                                entityDt.ConversionFactor = 1;
                            }
                        }
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Insert(entityDt);
                        ct++;
                    }
                    retval = TransactionID.ToString();
                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Permintaan pembelian tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
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