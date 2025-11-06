using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class DirectPurchaseQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private DirectPurchase DetailPage
        {
            get { return (DirectPurchase)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnPurchaseID.Value = temp[0];
            hdnLocationID.Value = temp[1];
            hdnLocationItemGroupID.Value = temp[2];
            hdnSupplierID.Value = temp[3];
            hdnGCLocationGroupCtl.Value = temp[4];
            hdnProductLineIDCtl.Value = temp[5];
            hdnProductLineItemTypeCtl.Value = temp[6];
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

            filterExpression += string.Format("GCItemType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0 AND GCItemStatus != '{4}'",
                    Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN, Constant.ItemStatus.IN_ACTIVE);

            if (hdnLocationItemGroupID.Value != "" && hdnLocationItemGroupID.Value != "0")
            {
                filterExpression += " AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath like '%/" + hdnLocationItemGroupID.Value + "/%')";
            }

            if (hdnItemGroupDrugLogisticID.Value == "")
            {
                filterExpression += string.Format(" AND LocationID = '{0}' AND ItemName1 LIKE '%{1}%' AND IsDeleted = 0", hdnLocationID.Value, hdnFilterItem.Value);
            }
            else
            {
                filterExpression += string.Format(" AND LocationID = '{0}' AND ItemName1 LIKE '%{1}%' AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath LIKE '%/{2}/%') AND IsDeleted = 0", hdnLocationID.Value, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value);
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
            if (hdnPurchaseID.Value != "0" && hdnPurchaseID.Value != "")
            {
                List<vDirectPurchaseDt> lstItemID = BusinessLayer.GetvDirectPurchaseDtList(string.Format(
                    "DirectPurchaseID = {0} AND GCItemDetailStatus != '{1}'", hdnPurchaseID.Value, Constant.TransactionStatus.VOID));
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (vDirectPurchaseDt itm in lstItemID)
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
            DirectPurchaseHdDao entityHdDao = new DirectPurchaseHdDao(ctx);
            DirectPurchaseDtDao entityDtDao = new DirectPurchaseDtDao(ctx);
            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                int PurchaseID = 0;
                string PurchaseNo = "";
                DetailPage.SaveDirectPurchaseHd(ctx, ref PurchaseID, ref PurchaseNo);
                if (entityHdDao.Get(PurchaseID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<ItemMaster> lstItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);
                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        ItemMaster itemMaster = lstItemMaster.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));

                        DirectPurchaseDt entityDt = new DirectPurchaseDt();
                        entityDt.DirectPurchaseID = PurchaseID;
                        entityDt.ItemID = itemMaster.ItemID;
                        entityDt.Quantity = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                        entityDt.GCBaseUnit = itemMaster.GCItemUnit;
                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;

                        List<GetItemMasterPurchase> impList = BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, itemMaster.ItemID, Convert.ToInt32(hdnSupplierID.Value), ctx);
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

                            decimal subTotal = (entityDt.Quantity * entityDt.UnitPrice);
                            decimal discAmount = (subTotal * imp.Discount / 100);
                            decimal discAmount2 = ((subTotal - discAmount) * imp.Discount2 / 100);

                            entityDt.IsDiscountInPercentage = imp.Discount != 0 ? true : false;
                            entityDt.DiscountPercentage = imp.Discount;
                            entityDt.DiscountAmount = discAmount;

                            entityDt.IsDiscountInPercentage2 = imp.Discount2 != 0 ? true : false;
                            entityDt.DiscountPercentage2 = imp.Discount2;
                            entityDt.DiscountAmount2 = discAmount2;
                        }
                        else
                        {
                            entityDt.UnitPrice = 0;
                            entityDt.DiscountPercentage = 0;
                            entityDt.DiscountAmount = 0;
                            entityDt.DiscountPercentage2 = 0;
                            entityDt.DiscountAmount2 = 0;
                            entityDt.GCItemUnit = itemMaster.GCItemUnit;
                            entityDt.ConversionFactor = 1;
                        }
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Insert(entityDt);
                        ct++;
                    }
                    retval = PurchaseID.ToString();
                    ctx.CommitTransaction();
                }
                else 
                {
                    result = false;    
                    errMessage = "Pembelian tidak dapat diubah. Harap refresh halaman ini.";
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