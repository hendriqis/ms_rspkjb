using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseRequestEntryPackageCtl : BaseEntryPopupCtl
    {
        private List<ItemBalance> lstItemBalance = null;
        private List<ItemBalance> lstItemBalanceAll = null;

        private PurchaseRequest DetailPage
        {
            get { return (PurchaseRequest)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnRequestIDCtl.Value = temp[0];
            hdnLocationIDCtl.Value = temp[1];
            hdnLocationItemGroupIDCtl.Value = temp[2];
            hdnGCLocationGroupCtl.Value = temp[3];
            hdnProductLineIDCtl.Value = temp[4];
            hdnProductLineItemTypeCtl.Value = temp[5];

            BindGridView();
        }

        protected string OnGetFilterExpressionItemProduct()
        {
            string filterExpression = string.Format(
                                        "GCItemType IN ('{0}','{1}','{2}','{3}') AND ItemID IN (SELECT ItemID FROM ItemProduct WHERE IsProductionItem = 1) AND IsDeleted = 0 AND GCItemStatus != '{4}'",
                                        Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN, Constant.ItemStatus.IN_ACTIVE
                                    );

            //// RN (patch 202301-01) ini ditutup utk issue [QISMS2 : 202212260000032]
            //if (hdnProductLineIDCtl.Value != null && hdnProductLineIDCtl.Value != "" && hdnProductLineIDCtl.Value != "0")
            //{
            //    filterExpression += string.Format(" AND ProductLineID = {0}", hdnProductLineIDCtl.Value);
            //}

            return filterExpression;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (grdView.Rows.Count < 1)
            {
                BindGridView();
            }
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";

            if (hdnPackageID.Value != "")
            {
                filterExpression = string.Format("ItemID = {0} AND IsDeleted = 0 AND BillOfMaterialProductLineID = {1}", hdnPackageID.Value, hdnProductLineIDCtl.Value);

                ////if (hdnRequestIDCtl.Value != "" && hdnRequestIDCtl.Value != "0")
                ////{
                ////    filterExpression += string.Format(" AND BillOfMaterialID NOT IN (SELECT ItemID FROM PurchaseRequestDt WHERE IsDeleted = 0 AND ISNULL(GCItemDetailStatus,'') != '{0}' AND PurchaseRequestID = {1})", Constant.TransactionStatus.VOID, hdnRequestIDCtl.Value);
                ////}
            }
            List<vItemBOM> lstEntity = BusinessLayer.GetvItemBOMList(filterExpression);

            string lstItemID = "";
            foreach (vItemBOM itemBOM in lstEntity)
            {
                if (lstItemID != "")
                {
                    lstItemID += ",";
                }
                lstItemID += itemBOM.BillOfMaterialID.ToString();
            }

            #region Item Balance
            if (lstItemID != "" && hdnLocationIDCtl.Value != "")
            {
                filterExpression = string.Format("LocationID = {0} AND ItemID IN ({1}) AND IsDeleted = 0", hdnLocationIDCtl.Value, lstItemID);
            }
            else if (lstItemID != "")
            {
                filterExpression = string.Format("ItemID IN ({0}) AND IsDeleted = 0", lstItemID);
            }
            else
            {
                filterExpression = "1 = 0";
            }
            lstItemBalance = BusinessLayer.GetItemBalanceList(filterExpression);
            #endregion

            #region Item Balance All Location
            if (lstItemID != "")
            {
                filterExpression = string.Format("ItemID IN ({0}) AND IsDeleted = 0", lstItemID);
            }
            else
            {
                filterExpression = "1 = 0";
            }
            lstItemBalanceAll = BusinessLayer.GetItemBalanceList(filterExpression);
            #endregion

            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HtmlGenericControl divQtyMin = (HtmlGenericControl)e.Row.FindControl("divQtyMin");
                HtmlGenericControl divQtyMax = (HtmlGenericControl)e.Row.FindControl("divQtyMax");
                HtmlGenericControl divQtyOnHand = (HtmlGenericControl)e.Row.FindControl("divQtyOnHand");
                HtmlGenericControl divQtyOnHandRS = (HtmlGenericControl)e.Row.FindControl("divQtyOnHandRS");

                vItemBOM entity = e.Row.DataItem as vItemBOM;

                if (lstItemBalance.Count > 0)
                {
                    ItemBalance itemBalance = lstItemBalance.FirstOrDefault(p => p.ItemID == entity.BillOfMaterialID);

                    decimal qtyMin = 0;
                    decimal qtyMax = 0;
                    decimal qtyOnHand = 0;
                    decimal qtyOnHandRS = 0;

                    if (itemBalance != null)
                    {
                        qtyMin = itemBalance.QuantityMIN;
                        qtyMax = itemBalance.QuantityMAX;
                        qtyOnHand = itemBalance.QuantityEND;
                        qtyOnHandRS = lstItemBalanceAll.Where(a => a.ItemID == entity.BillOfMaterialID).Sum(b => b.QuantityEND);
                    }

                    divQtyMin.InnerHtml = qtyMin.ToString(Constant.FormatString.NUMERIC_2);
                    divQtyMax.InnerHtml = qtyMax.ToString(Constant.FormatString.NUMERIC_2);
                    divQtyOnHand.InnerHtml = qtyOnHand.ToString(Constant.FormatString.NUMERIC_2);
                    divQtyOnHandRS.InnerHtml = qtyOnHandRS.ToString(Constant.FormatString.NUMERIC_2);
                }
            }
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseRequestHdDao requestHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao requestDtDao = new PurchaseRequestDtDao(ctx);

            try
            {
                int TransactionID = 0;
                DetailPage.SavePurchaseRequestHd(ctx, ref TransactionID);

                if (requestHdDao.Get(TransactionID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    String[] bomID = hdnSelectedMember.Value.Substring(1).Split(',');
                    String[] bomQty = hdnSelectedMemberQtyBOM.Value.Substring(1).Split(',');

                    for (int i = 0; i < bomID.Count(); i++)
                    {
                        ItemMaster itemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID = {0}", bomID[i]), ctx).FirstOrDefault();
                        ItemPlanning itemPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0} AND IsDeleted = 0", itemMaster.ItemID), ctx).FirstOrDefault();

                        string filterCheckDt = string.Format("IsDeleted = 0 AND ISNULL(GCItemDetailStatus,'') != '{0}' AND PurchaseRequestID = {1} AND ItemID = {2}",
                                                                Constant.TransactionStatus.VOID, TransactionID, itemMaster.ItemID);
                        List<PurchaseRequestDt> checkDt = BusinessLayer.GetPurchaseRequestDtList(filterCheckDt, ctx);

                        if (checkDt.Count() == 0)
                        {
                            PurchaseRequestDt entityDt = new PurchaseRequestDt();
                            entityDt.PurchaseRequestID = TransactionID;
                            entityDt.ItemID = itemMaster.ItemID;
                            entityDt.Quantity = Convert.ToDecimal(bomQty[i]);
                            entityDt.GCBaseUnit = itemMaster.GCItemUnit;

                            int? businessPartnerID = null;
                            if (hdnSupplierIDCtl.Value != "")
                            {
                                businessPartnerID = Convert.ToInt32(hdnSupplierIDCtl.Value);
                            }
                            else
                            {
                                businessPartnerID = 0;
                            }

                            if (businessPartnerID == 0)
                            {
                                if (itemPlanning.BusinessPartnerID != null && itemPlanning.BusinessPartnerID != 0)
                                {
                                    BusinessPartners businessPartners = BusinessLayer.GetBusinessPartnersList(String.Format("BusinessPartnerID = {0}", itemPlanning.BusinessPartnerID), ctx).FirstOrDefault();

                                    if (businessPartners.IsActive == true)
                                    {
                                        if (businessPartners.IsBlackList == true)
                                        {
                                            entityDt.BusinessPartnerID = null;
                                        }
                                        else
                                        {
                                            entityDt.BusinessPartnerID = itemPlanning.BusinessPartnerID;
                                        }
                                    }
                                    else
                                    {
                                        entityDt.BusinessPartnerID = null;
                                    }
                                }
                                else
                                {
                                    entityDt.BusinessPartnerID = null;
                                }
                            }
                            else
                            {
                                entityDt.BusinessPartnerID = businessPartnerID;
                            }

                            entityDt.Remarks = "";
                            entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;

                            GetItemMasterPurchase itemMasterPurchase = BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, itemMaster.ItemID, (int)businessPartnerID, ctx).FirstOrDefault();
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            if (itemMasterPurchase != null)
                            {
                                if (itemMasterPurchase.ConversionFactor == 1)
                                {
                                    entityDt.UnitPrice = itemMasterPurchase.Price;
                                }
                                else
                                {
                                    entityDt.UnitPrice = itemMasterPurchase.UnitPrice;
                                }
                                entityDt.GCPurchaseUnit = itemMasterPurchase.PurchaseUnit;
                                entityDt.ConversionFactor = itemMasterPurchase.ConversionFactor;
                                entityDt.DiscountPercentage = itemMasterPurchase.Discount;
                                entityDt.DiscountPercentage2 = itemMasterPurchase.Discount2;
                            }
                            else
                            {
                                entityDt.UnitPrice = 0;
                                entityDt.DiscountPercentage = 0;
                                entityDt.DiscountPercentage2 = 0;
                                entityDt.GCPurchaseUnit = itemMaster.GCItemUnit;
                                entityDt.ConversionFactor = 1;
                            }

                            requestDtDao.Insert(entityDt);
                        }
                        else
                        {
                            PurchaseRequestDt entityDt = requestDtDao.Get(checkDt.FirstOrDefault().ID);
                            entityDt.Quantity += Convert.ToDecimal(bomQty[i]);
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            requestDtDao.Update(entityDt);
                        }
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