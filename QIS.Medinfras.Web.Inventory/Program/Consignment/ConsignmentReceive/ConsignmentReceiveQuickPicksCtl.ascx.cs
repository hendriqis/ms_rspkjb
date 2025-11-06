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
    public partial class ConsignmentReceiveQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private ConsignmentReceive DetailPage
        {
            get { return (ConsignmentReceive)Page; }
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
            hdnSupplierIDCtl.Value = temp[6];
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
            string filterExpression = string.Format("LocationID = '{0}' AND ItemName1 LIKE '%{1}%' AND IsDeleted = 0 AND GCItemStatus != '{2}'",
                                    hdnLocationIDCtl.Value, hdnFilterItem.Value, Constant.ItemStatus.IN_ACTIVE);

            filterExpression += string.Format(" AND ItemID IN (SELECT ItemID FROM ItemProduct WHERE IsConsigmentItem = 1)");

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
                        filterExpression += string.Format(" AND GCItemType IN ('{0}','{1}','{2}')",
                                Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM);
                    }
                }
                else
                {
                    filterExpression += string.Format(" AND GCItemType IN ('{0}','{1}','{2}')",
                            Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM);
                }
            }

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
                List<PurchaseReceiveDt> lstItemID = BusinessLayer.GetPurchaseReceiveDtList(string.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", hdnTransactionID.Value, Constant.TransactionStatus.VOID));
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (PurchaseReceiveDt itm in lstItemID)
                    {
                        lstSelectedID += "," + itm.ItemID;
                    }
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
            PurchaseReceiveHdDao entityHdDao = new PurchaseReceiveHdDao(ctx);
            PurchaseReceiveDtDao entityDtDao = new PurchaseReceiveDtDao(ctx);
            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                int TransactionID = 0;
                string purchaseReceiveNo = "";

                DetailPage.SavePurchaseReceiveHd(ctx, ref TransactionID, ref purchaseReceiveNo);
                if (entityHdDao.Get(TransactionID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    int? businessPartnerID = null;
                    if (hdnSupplierIDCtl.Value != "")
                    {
                        businessPartnerID = Convert.ToInt32(hdnSupplierIDCtl.Value);
                    }
                    else
                    {
                        businessPartnerID = 0;
                    }

                    List<ItemMaster> lstItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);
                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        ItemMaster itemMaster = lstItemMaster.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));
                        ItemPlanning itemPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0} AND IsDeleted = 0", itemMaster.ItemID)).FirstOrDefault();

                        PurchaseReceiveDt entityDt = new PurchaseReceiveDt();
                        entityDt.PurchaseReceiveID = TransactionID;
                        entityDt.ItemID = itemMaster.ItemID;
                        entityDt.Quantity = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                        entityDt.GCBaseUnit = itemMaster.GCItemUnit;
                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;

                        List<GetItemMasterPurchase> impList = BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, itemMaster.ItemID, (int)businessPartnerID, ctx);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        if (impList.Count > 0)
                        {
                            GetItemMasterPurchase imp = impList.FirstOrDefault();
                            entityDt.GCItemUnit = imp.PurchaseUnit;
                            entityDt.ConversionFactor = imp.ConversionFactor;
                            if (imp.ConversionFactor == 1)
                            {
                                entityDt.UnitPrice = imp.Price;
                            }
                            else
                            {
                                entityDt.UnitPrice = imp.UnitPrice;
                            }

                            entityDt.IsDiscountInPercentage1 = true;
                            entityDt.IsDiscountInPercentage2 = true;
                            entityDt.DiscountPercentage1 = imp.Discount;
                            entityDt.DiscountPercentage2 = imp.Discount2;

                            decimal subtotal = entityDt.UnitPrice * entityDt.Quantity;
                            entityDt.DiscountAmount1 = ((entityDt.DiscountPercentage1 * subtotal) / 100);
                            decimal subtotal2 = subtotal - entityDt.DiscountAmount1;
                            entityDt.DiscountAmount2 = ((entityDt.DiscountPercentage2 * subtotal2) / 100);
                        }
                        else
                        {
                            entityDt.UnitPrice = 0;
                            entityDt.DiscountPercentage1 = 0;
                            entityDt.DiscountPercentage2 = 0;
                            entityDt.GCItemUnit = itemMaster.GCItemUnit;
                            entityDt.ConversionFactor = 1;
                        }

                        entityDt.LineAmount = entityDt.CustomSubTotal2;
                        entityDtDao.Insert(entityDt);
                        ct++;
                    }
                    retval = purchaseReceiveNo;
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;          
                    errMessage = string.Format("Penerimaan konsinyasi {0} tidak dapat diubah. Harap refresh halaman ini.", purchaseReceiveNo);
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