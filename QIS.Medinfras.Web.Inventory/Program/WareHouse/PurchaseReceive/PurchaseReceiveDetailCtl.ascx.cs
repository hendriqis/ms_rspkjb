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
    public partial class PurchaseReceiveDetailCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] lstParam = param.Split('|');

            hdnSupplierIDCtl.Value = lstParam[0];
            hdnIsFilterPurchaseOrderNoCtl.Value = lstParam[1];
            hdnProductLineIDCtl.Value = lstParam[2];
            hdnLocationIDCtl.Value = lstParam[3];
            hdnMenuType.Value = lstParam[4];
            hdnIsPORWithPriceInformation.Value = lstParam[5];

            string filterSetvar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.ALLOW_POR_QTY_BIGGER_THEN_PO, //1
                                                        Constant.SettingParameter.ALLOW_PURCHASE_RECEIVE_AFTER_PRINT_PURCHASE_ORDER, //2
                                                        Constant.SettingParameter.IM0127, //3
                                                        Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE, //4
                                                        Constant.SettingParameter.IM0131, //5
                                                        Constant.SettingParameter.IM_IS_COPY_PO_ALLOW_CHANGE_PRICE_AND_DISCOUNT //6
                                                    );
            List<SettingParameterDt> lstSetparDt = BusinessLayer.GetSettingParameterDtList(filterSetvar);
            hdnIsValidateVAT.Value = lstSetparDt.Where(t => t.ParameterCode == Constant.SettingParameter.IM0127).FirstOrDefault().ParameterValue;
            hdnIsAllowPORQtyBiggerThanPO.Value = lstSetparDt.Where(t => t.ParameterCode == Constant.SettingParameter.ALLOW_POR_QTY_BIGGER_THEN_PO).FirstOrDefault().ParameterValue;
            hdnIM0131Ctl.Value = lstSetparDt.Where(t => t.ParameterCode == Constant.SettingParameter.IM0131).FirstOrDefault().ParameterValue;
            hdnIsCopyPOAllowChangePriceAndDiscount.Value = lstSetparDt.Where(t => t.ParameterCode == Constant.SettingParameter.IM_IS_COPY_PO_ALLOW_CHANGE_PRICE_AND_DISCOUNT).FirstOrDefault().ParameterValue;

            hdnIsUsedProductLine.Value = lstSetparDt.Where(t => t.ParameterCode == Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE).FirstOrDefault().ParameterValue;

            hdnIsAllowPORAfterPrintPO.Value = lstSetparDt.Where(t => t.ParameterCode == Constant.SettingParameter.ALLOW_PURCHASE_RECEIVE_AFTER_PRINT_PURCHASE_ORDER).FirstOrDefault().ParameterValue;

            if (hdnIsAllowPORAfterPrintPO.Value == "1")
            {
                hdnFilterExpressionPurchaseOrderCtl.Value = string.Format(
                    "BusinessPartnerID = '{0}' AND GCTransactionStatus = '{1}' AND TransactionCode = '{2}' AND PurchaseOrderID IN (SELECT PurchaseOrderID FROM PurchaseOrderDt WHERE (Quantity - ReceivedQuantity) > 0 AND IsDeleted = 0) AND PrintNumber > 0 AND OrderDate <= '{3}' AND LocationID = {4}",
                        hdnSupplierIDCtl.Value, Constant.TransactionStatus.APPROVED, Constant.TransactionCode.PURCHASE_ORDER, DateTime.Now, hdnLocationIDCtl.Value);
            }
            else
            {
                hdnFilterExpressionPurchaseOrderCtl.Value = string.Format(
                    "BusinessPartnerID = '{0}' AND GCTransactionStatus = '{1}' AND TransactionCode = '{2}' AND PurchaseOrderID IN (SELECT PurchaseOrderID FROM PurchaseOrderDt WHERE (Quantity - ReceivedQuantity) > 0 AND IsDeleted = 0) AND OrderDate <= '{3}' AND LocationID = {4}",
                        hdnSupplierIDCtl.Value, Constant.TransactionStatus.APPROVED, Constant.TransactionCode.PURCHASE_ORDER, DateTime.Now, hdnLocationIDCtl.Value);
            }

            if (hdnIsUsedProductLine.Value == "1")
            {
                if (hdnProductLineIDCtl.Value != "0")
                {
                    hdnFilterExpressionPurchaseOrderCtl.Value += string.Format(" AND ProductLineID = {0}", hdnProductLineIDCtl.Value);
                }
            }

            List<StandardCode> listStandardCodePO = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.PURCHASE_ORDER_TYPE));
            listStandardCodePO.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPurchaseOrderType, listStandardCodePO, "StandardCodeName", "StandardCodeID");
            cboPurchaseOrderType.SelectedIndex = 0;

            BindGridView();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (grdView.Rows.Count < 1)
                BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("GCTransactionStatus = '{0}' AND GCItemDetailStatus = '{0}' AND TransactionCode = '{1}' AND IsDeleted = 0 AND OrderDate <= '{2}' AND (Quantity - ReceivedQuantity) > 0",
                                                Constant.TransactionStatus.APPROVED, Constant.TransactionCode.PURCHASE_ORDER, DateTime.Now);

            if (hdnIsAllowPORAfterPrintPO.Value == "1")
            {
                if (hdnOrderID.Value != null && hdnOrderID.Value != "")
                {
                    filterExpression += string.Format(" AND PurchaseOrderID = {0} AND PrintNumber > 0", hdnOrderID.Value);
                }
                else
                {
                    if (hdnIsFilterPurchaseOrderNoCtl.Value != "1")
                    {
                        filterExpression += string.Format(" AND SupplierID = {0} AND PrintNumber > 0", hdnSupplierIDCtl.Value);
                    }
                }
            }
            else
            {
                if (hdnOrderID.Value != null && hdnOrderID.Value != "")
                {
                    filterExpression += string.Format(" AND PurchaseOrderID = {0}", hdnOrderID.Value);
                }
                else
                {
                    if (hdnIsFilterPurchaseOrderNoCtl.Value != "1")
                    {
                        filterExpression += string.Format(" AND SupplierID = {0}", hdnSupplierIDCtl.Value);
                    }
                }
            }

            if (cboPurchaseOrderType.Value != null)
            {
                filterExpression += string.Format(" AND GCPurchaseOrderType = '{0}'", cboPurchaseOrderType.Value);
            }

            if (hdnIsUsedProductLine.Value == "1")
            {
                filterExpression += string.Format(" AND ProductLineID = {0}", hdnProductLineIDCtl.Value);
            }

            filterExpression += string.Format(" AND LocationID = {0} ORDER BY ItemName1, PurchaseOrderNo", hdnLocationIDCtl.Value);

            List<vPurchaseOrderDt> lstEntity = BusinessLayer.GetvPurchaseOrderDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
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

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseOrderDt entity = e.Row.DataItem as vPurchaseOrderDt;
                TextBox txtReceivedItem = e.Row.FindControl("txtReceivedItem") as TextBox;
                HtmlGenericControl lblPurchaseUnit = e.Row.FindControl("lblPurchaseUnit") as HtmlGenericControl;
                TextBox txtUnitPrice = e.Row.FindControl("txtUnitPrice") as TextBox;
                CheckBox chkIsDiscInPct1 = e.Row.FindControl("chkIsDiscInPct1") as CheckBox;
                TextBox txtIsDiscInPct1 = e.Row.FindControl("txtIsDiscInPct1") as TextBox;
                TextBox txtDiscountPercentage1 = e.Row.FindControl("txtDiscountPercentage1") as TextBox;
                TextBox txtDiscountAmount1 = e.Row.FindControl("txtDiscountAmount1") as TextBox;
                CheckBox chkIsDiscInPct2 = e.Row.FindControl("chkIsDiscInPct2") as CheckBox;
                TextBox txtIsDiscInPct2 = e.Row.FindControl("txtIsDiscInPct2") as TextBox;
                TextBox txtDiscountPercentage2 = e.Row.FindControl("txtDiscountPercentage2") as TextBox;
                TextBox txtDiscountAmount2 = e.Row.FindControl("txtDiscountAmount2") as TextBox;
                CheckBox chkIsAsset = e.Row.FindControl("chkIsAsset") as CheckBox;
                CheckBox chkIsPPN = e.Row.FindControl("chkIsPPN") as CheckBox;

                txtReceivedItem.Text = (entity.DraftQuantity - entity.ReceivedQuantity).ToString();
                txtUnitPrice.Text = entity.DraftUnitPrice.ToString(Constant.FormatString.NUMERIC_2);

                String DiscountPercentage1 = Convert.ToString(entity.IsDiscountInPercentage1);
                String DiscountPercentage2 = Convert.ToString(entity.IsDiscountInPercentage2);
                decimal discountAmount1 = 0;
                decimal discountAmount2 = 0;

                if (entity.IsDiscountInPercentage1)
                {
                    chkIsDiscInPct1.Checked = true;
                    txtIsDiscInPct1.Text = "1";
                    txtDiscountPercentage1.Text = entity.DraftDiscountPercentage1.ToString();

                    discountAmount1 = ((entity.UnitPrice * (entity.DraftQuantity - entity.ReceivedQuantity)) * entity.DraftDiscountPercentage1) / 100;
                    txtDiscountAmount1.Text = discountAmount1.ToString(Constant.FormatString.NUMERIC_2);

                    txtDiscountPercentage1.Attributes.Remove("readonly");
                    txtDiscountAmount1.Attributes.Add("readonly", "readonly");
                }
                else
                {
                    chkIsDiscInPct1.Checked = false;
                    txtIsDiscInPct1.Text = "0";
                    txtDiscountPercentage1.Text = entity.DraftDiscountPercentage1.ToString();

                    if (entity.DraftQuantity == (entity.DraftQuantity - entity.ReceivedQuantity))
                    {
                        discountAmount1 = entity.DraftDiscountAmount1;
                    }
                    else
                    {
                        discountAmount1 = ((entity.UnitPrice * (entity.DraftQuantity - entity.ReceivedQuantity)) * entity.DraftDiscountPercentage1) / 100;
                    }
                    txtDiscountAmount1.Text = discountAmount1.ToString(Constant.FormatString.NUMERIC_2);

                    txtDiscountPercentage1.Attributes.Add("readonly", "readonly");
                    txtDiscountAmount1.Attributes.Remove("readonly");
                }

                if (entity.IsDiscountInPercentage2)
                {
                    chkIsDiscInPct2.Checked = true;
                    txtIsDiscInPct2.Text = "1";
                    txtDiscountPercentage2.Text = entity.DraftDiscountPercentage2.ToString();

                    discountAmount2 = (((entity.UnitPrice * (entity.DraftQuantity - entity.ReceivedQuantity)) - discountAmount1) * entity.DraftDiscountPercentage2) / 100;
                    txtDiscountAmount2.Text = discountAmount2.ToString(Constant.FormatString.NUMERIC_2);

                    txtDiscountPercentage2.Attributes.Remove("readonly");
                    txtDiscountAmount2.Attributes.Add("readonly", "readonly");
                }
                else
                {
                    chkIsDiscInPct2.Checked = false;
                    txtIsDiscInPct2.Text = "0";
                    txtDiscountPercentage2.Text = entity.DraftDiscountPercentage2.ToString();

                    if (entity.DraftQuantity == (entity.DraftQuantity - entity.ReceivedQuantity))
                    {
                        discountAmount2 = entity.DraftDiscountAmount2;
                    }
                    else
                    {
                        discountAmount2 = (((entity.UnitPrice * (entity.DraftQuantity - entity.ReceivedQuantity)) - discountAmount1) * entity.DraftDiscountPercentage2) / 100;
                    }
                    txtDiscountAmount2.Text = discountAmount2.ToString();

                    txtDiscountPercentage2.Attributes.Add("readonly", "readonly");
                    txtDiscountAmount2.Attributes.Remove("readonly");
                }

                lblPurchaseUnit.InnerHtml = entity.PurchaseUnit;
                chkIsAsset.Checked = entity.IsFixedAsset;
                chkIsPPN.Checked = entity.IsIncludeVAT;

                if (hdnMenuType.Value == "v2" && hdnIsPORWithPriceInformation.Value == "0")
                {
                    txtUnitPrice.Visible = false;
                    chkIsDiscInPct1.Visible = false;
                    txtDiscountPercentage1.Visible = false;
                    txtDiscountAmount1.Visible = false;
                    chkIsDiscInPct2.Visible = false;
                    txtDiscountPercentage2.Visible = false;
                    txtDiscountAmount2.Visible = false;
                }
            }
        }

        protected string DateTimeNowDatePicker()
        {
            return DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected class PRDtExpired
        {
            public PurchaseReceiveDt purchaseReceiveDt { get; set; }
            public PurchaseReceiveDtExpired purchaseReceiveDtExpired { get; set; }
        }

        private void ControlToEntity(IDbContext ctx, List<PRDtExpired> lstPRDt, ref int oTermID, ref string errMessage)
        {
            PurchaseOrderHdDao entityPOHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao entityPODtDao = new PurchaseOrderDtDao(ctx);

            foreach (GridViewRow row in grdView.Rows)
            {
                CheckBox chkIsSelected = row.FindControl("chkIsSelected") as CheckBox;

                if (chkIsSelected.Checked)
                {
                    HtmlInputHidden hdnPurchaseOrderDtID = (HtmlInputHidden)row.FindControl("keyField");
                    HtmlInputHidden hdnPurchaseOrderHdID = (HtmlInputHidden)row.FindControl("hdnPOHdID");
                    TextBox txtQtyReceive = (TextBox)row.FindControl("txtReceivedItem");
                    TextBox txtUnitPrice = (TextBox)row.FindControl("txtUnitPrice");
                    TextBox txtBatchNo = (TextBox)row.FindControl("txtBatchNo");
                    TextBox txtExpired = (TextBox)row.FindControl("txtExpired");
                    CheckBox chkIsDiscInPct1 = row.FindControl("chkIsDiscInPct1") as CheckBox;
                    TextBox txtIsDiscInPct1 = (TextBox)row.FindControl("txtIsDiscInPct1");
                    TextBox txtDiscountPercentage1 = (TextBox)row.FindControl("txtDiscountPercentage1");
                    TextBox txtDiscountAmount1 = (TextBox)row.FindControl("txtDiscountAmount1");
                    CheckBox chkIsDiscInPct2 = row.FindControl("chkIsDiscInPct2") as CheckBox;
                    TextBox txtIsDiscInPct2 = (TextBox)row.FindControl("txtIsDiscInPct2");
                    TextBox txtDiscountPercentage2 = (TextBox)row.FindControl("txtDiscountPercentage2");
                    TextBox txtDiscountAmount2 = (TextBox)row.FindControl("txtDiscountAmount2");
                    TextBox txtRemarksDetailCtl = (TextBox)row.FindControl("txtRemarksDetailCtl");
                    HtmlInputHidden hdnConversionFactor = (HtmlInputHidden)row.FindControl("hdnConversionFactor");
                    HtmlInputHidden hdnDefaultConversionFactor = (HtmlInputHidden)row.FindControl("hdnDefaultConversionFactor");
                    HtmlInputHidden hdnGCPurchaseUnit = (HtmlInputHidden)row.FindControl("hdnGCPurchaseUnit");
                    HtmlInputHidden hdnUnitPrice = (HtmlInputHidden)row.FindControl("hdnUnitPrice");
                    HtmlInputHidden hdnQuantity = (HtmlInputHidden)row.FindControl("hdnQuantity");
                    HtmlInputHidden hdnIsDiscountInPercentage1 = (HtmlInputHidden)row.FindControl("hdnIsDiscountInPercentage1");
                    HtmlInputHidden hdnDisc1Pct = (HtmlInputHidden)row.FindControl("hdnDisc1Pct");
                    HtmlInputHidden hdnDisc1 = (HtmlInputHidden)row.FindControl("hdnDisc1");
                    HtmlInputHidden hdnIsDiscountInPercentage2 = (HtmlInputHidden)row.FindControl("hdnIsDiscountInPercentage2");
                    HtmlInputHidden hdnDisc2Pct = (HtmlInputHidden)row.FindControl("hdnDisc2Pct");
                    HtmlInputHidden hdnDisc2 = (HtmlInputHidden)row.FindControl("hdnDisc2");
                    HtmlInputHidden hdnReceivedQuantity = (HtmlInputHidden)row.FindControl("hdnReceivedQuantity");
                    HtmlInputHidden hdnItemName1 = (HtmlInputHidden)row.FindControl("hdnItemName1");
                    CheckBox chkIsAsset = row.FindControl("chkIsAsset") as CheckBox;


                    PurchaseOrderDt entityPODt = entityPODtDao.Get(Convert.ToInt32(hdnPurchaseOrderDtID.Value));
                    Decimal defaultConversionFactor = Convert.ToDecimal(hdnDefaultConversionFactor.Value);

                    PurchaseOrderHd entityPOHd = entityPOHdDao.Get(entityPODt.PurchaseOrderID);
                    oTermID = entityPOHd.TermID;

                    PurchaseReceiveDt entityPRDt = new PurchaseReceiveDt();
                    entityPRDt.PurchaseOrderID = Convert.ToInt32(hdnPurchaseOrderHdID.Value);
                    entityPRDt.PurchaseOrderDtID = Convert.ToInt32(hdnPurchaseOrderDtID.Value);
                    entityPRDt.ItemID = entityPODt.ItemID;
                    entityPRDt.Quantity = Convert.ToDecimal(Request.Form[txtQtyReceive.UniqueID]);
                    entityPRDt.GCItemUnit = hdnGCPurchaseUnit.Value;
                    entityPRDt.GCBaseUnit = entityPODt.GCBaseUnit;
                    entityPRDt.ConversionFactor = Convert.ToDecimal(hdnConversionFactor.Value);
                    if (hdnMenuType.Value == "v2" && hdnIsPORWithPriceInformation.Value == "0")
                    {
                        entityPRDt.UnitPrice = Convert.ToDecimal((Convert.ToDecimal(hdnUnitPrice.Value) / defaultConversionFactor * entityPRDt.ConversionFactor).ToString("N2"));

                        entityPRDt.IsDiscountInPercentage1 = hdnIsDiscountInPercentage1.Value == "1" ? true : false;
                        entityPRDt.DiscountPercentage1 = Convert.ToDecimal(hdnDisc1Pct.Value);
                        entityPRDt.DiscountAmount1 = Convert.ToDecimal(hdnDisc1.Value);

                        entityPRDt.IsDiscountInPercentage2 = hdnIsDiscountInPercentage2.Value == "1" ? true : false;
                        entityPRDt.DiscountPercentage2 = Convert.ToDecimal(hdnDisc2Pct.Value);
                        entityPRDt.DiscountAmount2 = Convert.ToDecimal(hdnDisc2.Value);
                    }
                    else
                    {
                        entityPRDt.UnitPrice = Convert.ToDecimal(Request.Form[txtUnitPrice.UniqueID]);

                        entityPRDt.IsDiscountInPercentage1 = Request.Form[txtIsDiscInPct1.UniqueID].ToString() == "1" ? true : false;
                        entityPRDt.DiscountPercentage1 = Convert.ToDecimal(Request.Form[txtDiscountPercentage1.UniqueID]);
                        entityPRDt.DiscountAmount1 = Convert.ToDecimal(Request.Form[txtDiscountAmount1.UniqueID]);

                        entityPRDt.IsDiscountInPercentage2 = Request.Form[txtIsDiscInPct2.UniqueID].ToString() == "1" ? true : false;
                        entityPRDt.DiscountPercentage2 = Convert.ToDecimal(Request.Form[txtDiscountPercentage2.UniqueID]);
                        entityPRDt.DiscountAmount2 = Convert.ToDecimal(Request.Form[txtDiscountAmount2.UniqueID]);
                    }
                    entityPRDt.IsBonusItem = entityPODt.IsBonusItem;

                    entityPRDt.GCPPHType = entityPOHd.GCPPHType;
                    entityPRDt.PPHMode = entityPOHd.PPHMode;
                    entityPRDt.IsPPHInPercentage = entityPOHd.IsPPHInPercentage;
                    entityPRDt.PPHPercentage = entityPOHd.PPHPercentage;
                    if (entityPRDt.IsPPHInPercentage)
                    {
                        decimal total = (entityPRDt.Quantity * entityPRDt.UnitPrice) - entityPRDt.DiscountAmount1 - entityPRDt.DiscountAmount2;

                        entityPRDt.PPHAmount = entityPRDt.PPHPercentage * total / 100;
                    }
                    else
                    {
                        entityPRDt.PPHAmount = entityPOHd.PPHAmount;
                    }

                    if (!entityPOHd.PPHMode)
                    {
                        entityPRDt.PPHAmount = entityPRDt.PPHAmount * -1;
                    }

                    hdnPPH.Value = entityPOHd.PPHPercentage.ToString();

                    if (hdnUnitPrice != null && hdnQuantity != null && hdnDisc1Pct != null && hdnDisc2Pct != null)
                    {
                        entityPRDt.IsNeedConfirmation =
                                    (Convert.ToDecimal(hdnUnitPrice.Value) != entityPRDt.UnitPrice) ||
                                    (Convert.ToDecimal(hdnDisc1Pct.Value) != entityPRDt.DiscountPercentage1) ||
                                    (Convert.ToDecimal(hdnDisc2Pct.Value) != entityPRDt.DiscountPercentage2) ||
                                    ((Convert.ToDecimal(hdnQuantity.Value) - Convert.ToDecimal(hdnReceivedQuantity.Value)) < (entityPRDt.Quantity * entityPRDt.ConversionFactor / defaultConversionFactor));
                    }

                    if (hdnQuantity != null)
                    {
                        if (Math.Round((Convert.ToDecimal(hdnQuantity.Value) - Convert.ToDecimal(hdnReceivedQuantity.Value)), 2) < Math.Round((entityPRDt.Quantity * entityPRDt.ConversionFactor / defaultConversionFactor), 2))
                        {
                            if (hdnIsAllowPORQtyBiggerThanPO.Value == "0")
                            {
                                errMessage = string.Format("Item {0} tidak dapat dilakukan penerimaan, karena melebihi jumlah pemesanan.", hdnItemName1.Value);
                                break;
                            }
                        }
                    }

                    entityPRDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                    entityPRDt.RemarksDetail = Request.Form[txtRemarksDetailCtl.UniqueID];

                    entityPRDt.LineAmount = entityPRDt.CustomSubTotal2;

                    PurchaseReceiveDtExpired entityExpiredDt = new PurchaseReceiveDtExpired();
                    entityExpiredDt.BatchNumber = Request.Form[txtBatchNo.UniqueID];
                    entityExpiredDt.Quantity = entityPRDt.Quantity;
                    entityExpiredDt.ExpiredDate = Helper.GetDatePickerValue(Request.Form[txtExpired.UniqueID]);

                    PRDtExpired entity = new PRDtExpired();
                    entity.purchaseReceiveDt = entityPRDt;
                    entity.purchaseReceiveDtExpired = entityExpiredDt;

                    lstPRDt.Add(entity);
                }
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReceiveHdDao entityPurchaseReceiveHdDao = new PurchaseReceiveHdDao(ctx);
            PurchaseReceiveDtDao entityPurchaseReceiveDtDao = new PurchaseReceiveDtDao(ctx);
            PurchaseReceiveDtExpiredDao entityPurchaseReceiveDtExpiredDao = new PurchaseReceiveDtExpiredDao(ctx);
            PurchaseOrderDtDao entityPurchaseOrderDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseOrderHdDao entityPurchaseOrderHdDao = new PurchaseOrderHdDao(ctx);
            try
            {
                List<PRDtExpired> lstPRDt = new List<PRDtExpired>();
                int oTermID = 0;
                errMessage = string.Empty;

                ControlToEntity(ctx, lstPRDt, ref oTermID, ref errMessage);
                if (string.IsNullOrEmpty(errMessage))
                {
                    int purchaseReceiveID = 0;
                    string purchaseReceiveNo = "";

                    #region validate TotalDP
                    //if (!string.IsNullOrEmpty(hdnOrderID.Value) && hdnOrderID.Value != "0")
                    //{
                    //    string filterDt = string.Format("PurchaseOrderID = '{0}' AND GCItemDetailStatus != '{1}'", hdnOrderID.Value, Constant.TransactionStatus.VOID);
                    //    ctx.CommandType = CommandType.Text;
                    //    ctx.Command.Parameters.Clear();
                    //    List<PurchaseReceiveDt> lstReceiveDt = BusinessLayer.GetPurchaseReceiveDtList(filterDt, ctx);
                    //    if (lstReceiveDt.Count > 0)
                    //    {
                    //        foreach (PurchaseReceiveDt dt in lstReceiveDt)
                    //        {
                    //            PurchaseReceiveHd receiveHd = entityPurchaseReceiveHdDao.Get(dt.PurchaseReceiveID);
                    //            if (receiveHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                    //            {
                    //                hdnTotalDP.Value = "";
                    //            }
                    //        }
                    //    }
                    //}

                    int prID = 0;
                    decimal tmpDownPayment = 0;
                    decimal tmpChargesAmount = 0;
                    ((PurchaseReceive)Page).GetPurchaseReceiveHdID(ref prID);

                    List<PRDtExpired> lstPRDTFinal = lstPRDt.GroupBy(test => test.purchaseReceiveDt.PurchaseOrderID).Select(grp => grp.First()).ToList().OrderBy(x => x.purchaseReceiveDt.PurchaseOrderID).ToList();
                    foreach (PRDtExpired entityPRDt in lstPRDTFinal)
                    {
                        bool isAlreadyHasReceived = false;
                        PurchaseReceiveDt entityDt = entityPRDt.purchaseReceiveDt;
                        if (entityDt.IsBonusItem == false)
                        {
                            string filterDt = string.Format("PurchaseOrderID = '{0}' AND GCItemDetailStatus != '{1}'", entityDt.PurchaseOrderID, Constant.TransactionStatus.VOID);
                            if (prID != 0)
                            {
                                filterDt += string.Format(" AND PurchaseReceiveID != {0}", prID);
                            }

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<PurchaseReceiveDt> lstReceiveDt = BusinessLayer.GetPurchaseReceiveDtList(filterDt, ctx);
                            if (lstReceiveDt.Count > 0)
                            {
                                foreach (PurchaseReceiveDt dt in lstReceiveDt)
                                {
                                    PurchaseReceiveHd receiveHd = entityPurchaseReceiveHdDao.Get(dt.PurchaseReceiveID);
                                    if (receiveHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                    {
                                        isAlreadyHasReceived = true;
                                    }
                                }
                            }

                            if (!isAlreadyHasReceived)
                            {
                                PurchaseOrderHd orderHd = BusinessLayer.GetPurchaseOrderHd(Convert.ToInt32(entityDt.PurchaseOrderID));
                                tmpDownPayment += orderHd.DownPaymentAmount;
                                tmpChargesAmount += orderHd.ChargesAmount;
                            }
                        }
                    }

                    hdnTotalDP.Value = tmpDownPayment.ToString();
                    hdnTotalCharges.Value = tmpChargesAmount.ToString();
                    #endregion

                    ((PurchaseReceive)Page).SavePurchaseReceiveHd(ctx, ref purchaseReceiveID, ref purchaseReceiveNo, oTermID, hdnTotalDP.Value, hdnTotalCharges.Value);

                    PurchaseReceiveHd hd = entityPurchaseReceiveHdDao.Get(purchaseReceiveID);

                    foreach (PRDtExpired entityPRDt in lstPRDt)
                    {
                        if (hdnIsValidateVAT.Value == "1")
                        {
                            PurchaseOrderHd orderHd = entityPurchaseOrderHdDao.Get(Convert.ToInt32(entityPRDt.purchaseReceiveDt.PurchaseOrderID));
                            if (hd.IsIncludeVAT)
                            {
                                if (orderHd != null)
                                {
                                    if (!orderHd.IsIncludeVAT)
                                    {
                                        errMessage = "Penerimaan Ini Menggunakan PPN, Hanya PO yang memiliki PPN yang bisa disalin ke penerimaan ini.";
                                        result = false;
                                    }
                                }
                            }
                            else
                            {
                                if (orderHd != null)
                                {
                                    if (orderHd.IsIncludeVAT)
                                    {
                                        errMessage = "Penerimaan Ini Tidak Menggunakan PPN, Hanya PO yang tidak memiliki PPN yang bisa disalin ke penerimaan ini.";
                                        result = false;
                                    }
                                }
                            }
                        }

                        PurchaseReceiveDt entityDt = entityPRDt.purchaseReceiveDt;
                        if (entityDt.IsBonusItem == false)
                        {
                            if (entityPurchaseOrderDtDao.Get(Convert.ToInt32(entityDt.PurchaseOrderDtID)).IsDeleted == false && entityPurchaseOrderDtDao.Get(Convert.ToInt32(entityDt.PurchaseOrderDtID)).GCItemDetailStatus == Constant.TransactionStatus.APPROVED)
                            {
                                entityDt.PurchaseReceiveID = purchaseReceiveID;
                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                Int32 ID = entityPurchaseReceiveDtDao.InsertReturnPrimaryKeyID(entityDt);

                                PurchaseReceiveDtExpired entityPRDtExpired = entityPRDt.purchaseReceiveDtExpired;
                                entityPRDtExpired.ID = ID;
                                if (entityPRDtExpired.BatchNumber != "")
                                {
                                    entityPurchaseReceiveDtExpiredDao.Insert(entityPRDtExpired);
                                }
                            }
                        }
                        else
                        {
                            entityDt.PurchaseReceiveID = purchaseReceiveID;
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                            Int32 ID = entityPurchaseReceiveDtDao.InsertReturnPrimaryKeyID(entityDt);

                            PurchaseReceiveDtExpired entityPRDtExpired = entityPRDt.purchaseReceiveDtExpired;
                            entityPRDtExpired.ID = ID;
                            if (entityPRDtExpired.BatchNumber != "")
                            {
                                entityPurchaseReceiveDtExpiredDao.Insert(entityPRDtExpired);
                            }
                        }
                    }

                    if (result)
                    {
                        retval = purchaseReceiveNo;
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
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