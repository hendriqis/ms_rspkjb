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
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ApprovedItemRequestDetailList : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        private string[] lstDistribution = null;
        private string[] lstConsumption = null;
        private string[] lstPurchaseRequest = null;

        protected List<DataTempApp> lstTemp = new List<DataTempApp>();

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.APPROVED_ITEM_REQUEST;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            hdnIsUsedProductLine.Value = AppSession.IsUsedProductLine;

            hdnOrderID.Value = Page.Request.QueryString["id"];
            vItemRequestHd2 entityItemRequest = BusinessLayer.GetvItemRequestHd2List(string.Format("ItemRequestID = {0}", hdnOrderID.Value)).FirstOrDefault();
            hdnProductLineID.Value = entityItemRequest.ProductLineID.ToString();

            bool IsAllowPurchaseRequest = true;
            bool IsAllowItemConsumption = true;
            bool IsAllowItemDistribution = true;

            int? restrictionID = BusinessLayer.GetLocation(entityItemRequest.ToLocationID).RestrictionID;
            if (restrictionID != null)
            {
                List<RestrictionDt> lstRestrictionDt = BusinessLayer.GetRestrictionDtList(string.Format("RestrictionID = {0}", restrictionID));
                IsAllowPurchaseRequest = lstRestrictionDt.FirstOrDefault(p => p.TransactionCode == Constant.TransactionCode.PURCHASE_REQUEST) != null;
                IsAllowItemConsumption = lstRestrictionDt.FirstOrDefault(p => p.TransactionCode == Constant.TransactionCode.ITEM_CONSUMPTION) != null;
                IsAllowItemDistribution = lstRestrictionDt.FirstOrDefault(p => p.TransactionCode == Constant.TransactionCode.ITEM_DISTRIBUTION) != null;
            }

            hdnIsAllowPurchaseRequest.Value = IsAllowPurchaseRequest ? "1" : "0";
            hdnIsAllowItemConsumption.Value = IsAllowItemConsumption ? "1" : "0";
            hdnIsAllowItemDistribution.Value = IsAllowItemDistribution ? "1" : "0";

            EntityToControl(entityItemRequest);

            List<StandardCode> lstGCConsumptionType = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.CONSUMPTION_TYPE));
            StandardCode GCConsumptionType = lstGCConsumptionType.FirstOrDefault(p => p.IsDefault);
            if (GCConsumptionType == null)
                GCConsumptionType = lstGCConsumptionType.FirstOrDefault();
            hdnDefaultGCConsumptionType.Value = GCConsumptionType.StandardCodeID;

            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_IS_PURCHASE_REQUEST);
            hdnIsPurchaseRequest.Value = setvardt.ParameterValue;
        }

        private void EntityToControl(vItemRequestHd2 entity)
        {
            hdnOrderID.Value = entity.ItemRequestID.ToString();
            txtOrderNo.Text = entity.ItemRequestNo;
            txtItemOrderDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtItemOrderTime.Text = entity.TransactionTime;
            txtApprovedBy.Text = entity.ApprovedByName;
            txtItemOrderApprovedDate.Text = entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtItemOrderApprovedTime.Text = entity.LastUpdatedDate.ToString(Constant.FormatString.TIME_FORMAT);
            hdnLocationIDFrom.Value = entity.FromLocationID.ToString();
            txtLocationCode.Text = entity.FromLocationCode;
            txtLocationName.Text = entity.FromLocationName;
            hdnLocationIDTo.Value = entity.ToLocationID.ToString();
            txtLocationCodeTo.Text = entity.ToLocationCode;
            txtLocationNameTo.Text = entity.ToLocationName;
            txtNotes.Text = entity.Remarks;

            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            if (entity.RegistrationID != 0 && entity.RegistrationID != null)
            {
                txtRegistrationNo.Text = string.Format("{0} | {1} | ({2}) {3}", entity.RegistrationNo, entity.ServiceUnitName, entity.MedicalNo, entity.PatientName);
            }
            else
            {
                txtRegistrationNo.Text = "";
            }

            hdnIsRequestCopy.Value = entity.ReferenceNo != "" && entity.ReferenceNo != null ? "1" : "0";
            hdnIsHasCopyItemRequest.Value = entity.IsHasCopy ? "1" : "0";
            hdnIsHasOutstandingConsumption.Value = entity.IsHasOutstandingConsumptionType ? "1" : "0";
            hdnIsHasRequestProcess.Value = entity.IsHasProcessDistribution || entity.IsHasProcessTransaction || entity.IsHasProcessPurchase ? "1" : "0";

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnOrderID.Value != "")
                filterExpression = string.Format("ItemRequestID = {0} AND GCItemDetailStatus IN ('{1}','{2}') AND IsDeleted = 0 ORDER BY ItemName1 ASC", hdnOrderID.Value, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED);

            //if (isCountPageCount)
            //{
            //    int rowCount = BusinessLayer.GetvItemRequestDtRowCount(filterExpression);
            //    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            //}

            //List<vItemRequestDt> lstEntity = BusinessLayer.GetvItemRequestDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            List<vItemRequestDt> lstEntity = BusinessLayer.GetvItemRequestDtList(filterExpression);


            lstItemRequestDtRealizationPerItem = new List<vItemRequestDtRealizationPerItem>();
            foreach (DataTempApp e in lstTemp)
            {
                vItemRequestDtRealizationPerItem obj = BusinessLayer.GetvItemRequestDtRealizationPerItemList(string.Format("ItemID IN ({0})", e.ItemID)).FirstOrDefault();
                if (obj != null)
                {
                    lstItemRequestDtRealizationPerItem.Add(obj);
                }
                else
                {
                    vItemRequestDtRealizationPerItem obj1 = new vItemRequestDtRealizationPerItem();
                    lstItemRequestDtRealizationPerItem.Add(obj1);
                }
            }

            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

        }

        List<vItemRequestDtRealizationPerItem> lstItemRequestDtRealizationPerItem = null;
        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vItemRequestDt entity = e.Item.DataItem as vItemRequestDt;
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");
                TextBox txtDistribution = (TextBox)e.Item.FindControl("txtDistribution");
                TextBox txtPurchaseRequest = (TextBox)e.Item.FindControl("txtPurchaseRequest");
                TextBox txtConsumption = (TextBox)e.Item.FindControl("txtConsumption");
                HtmlGenericControl lblAvailableStock = (HtmlGenericControl)e.Item.FindControl("lblAvailableStock");

                Helper.SetControlEntrySetting(txtDistribution, new ControlEntrySetting(true, true, true), "mpEntry");
                Helper.SetControlEntrySetting(txtPurchaseRequest, new ControlEntrySetting(true, true, true), "mpEntry");
                Helper.SetControlEntrySetting(txtConsumption, new ControlEntrySetting(true, true, true), "mpEntry");

                decimal availableQty = 0;

                vItemRequestDtRealizationPerItem itemRequestDtRealizationPerItem = lstItemRequestDtRealizationPerItem.FirstOrDefault(p => p.ItemID == entity.ItemID);
                if (itemRequestDtRealizationPerItem != null)
                {
                    //availableQty = entity.EndingBalance - itemRequestDtRealizationPerItem.ItemRequestQuantity; // x conv
                    //if (entity.PurchaseRequestQty > 0)
                    //    availableQty += entity.Quantity; // x conv

                    availableQty = entity.EndingBalance - (itemRequestDtRealizationPerItem.ItemRequestQuantity * entity.ConversionFactor);
                    if (entity.PurchaseRequestQty > 0)
                    {
                        availableQty += (entity.Quantity * entity.ConversionFactor);
                    }
                }
                else
                {
                    availableQty = entity.EndingBalance;
                }

                if (availableQty < 0)
                {
                    availableQty = 0;
                }

                lblAvailableStock.InnerHtml = availableQty.ToString();

                string itemReqType = entity.GCItemRequestType;

                if (itemReqType == null || itemReqType == "")
                {
                    string filter = string.Format("LocationID = {0} AND ItemID = {1} AND IsDeleted = 0", entity.ToLocationID, entity.ItemID);
                    List<ItemBalance> ibalanceLst = BusinessLayer.GetItemBalanceList(filter);
                    if (ibalanceLst.Count() > 0)
                    {
                        ItemBalance ibalance = ibalanceLst.FirstOrDefault();
                        if (ibalance.GCItemRequestType != null && ibalance.GCItemRequestType != "")
                        {
                            itemReqType = ibalance.GCItemRequestType;
                        }
                    }

                    if (itemReqType == null || itemReqType == "")
                    {
                        ItemProduct iproduct = BusinessLayer.GetItemProduct(entity.ItemID);
                        itemReqType = iproduct.GCItemRequestType;
                    }
                }

                if (itemReqType == Constant.ItemRequestType.CONSUMPTION)
                {
                    hdnIsAllowItemDistribution.Value = "0";
                    hdnIsAllowItemConsumption.Value = "1";
                }
                else if (itemReqType == Constant.ItemRequestType.DISTRIBUTION)
                {
                    hdnIsAllowItemDistribution.Value = "1";
                    hdnIsAllowItemConsumption.Value = "0";
                }

                if (entity.GCItemDetailStatus == Constant.TransactionStatus.APPROVED)
                {
                    // Item Request BELUM PERNAH ADA proses sebelumnya 
                    if ((entity.Quantity * entity.ConversionFactor) > availableQty)
                    {
                        Decimal qtyFinal = 0;
                        Decimal purchaseReqQty = 0;

                        qtyFinal = Math.Floor(availableQty / entity.ConversionFactor);

                        if (itemReqType == Constant.ItemRequestType.CONSUMPTION)
                        {
                            txtConsumption.Text = Math.Round(qtyFinal, 2).ToString();
                            txtConsumption.Attributes.Add("max", Math.Round(qtyFinal, 2).ToString());
                        }
                        else
                        {
                            txtDistribution.Text = Math.Round(qtyFinal, 2).ToString();
                            txtDistribution.Attributes.Add("max", Math.Round(qtyFinal, 2).ToString());
                        }

                        //boleh minta beli atau tidak
                        SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_IS_PURCHASE_REQUEST);
                        if (setvardt.ParameterValue == "1")
                        {
                            if (hdnIsAllowPurchaseRequest.Value == "1")
                            {
                                purchaseReqQty = (((entity.Quantity * entity.ConversionFactor) - availableQty) / entity.ConversionFactor);

                                txtPurchaseRequest.Text = Math.Round(purchaseReqQty, 2).ToString();
                                txtPurchaseRequest.Attributes.Add("max", Math.Round(purchaseReqQty, 2).ToString());
                            }
                            else
                            {
                                txtPurchaseRequest.Text = Math.Round(purchaseReqQty, 2).ToString();
                                txtPurchaseRequest.Attributes.Add("max", Math.Round(purchaseReqQty, 2).ToString());
                            }
                        }
                        else
                        {
                            txtPurchaseRequest.Text = Math.Round(purchaseReqQty, 2).ToString();
                            txtPurchaseRequest.Attributes.Add("max", Math.Round(purchaseReqQty, 2).ToString());
                        }
                    }
                    else
                    {
                        if (itemReqType == Constant.ItemRequestType.CONSUMPTION)
                        {
                            txtConsumption.Text = Math.Round(entity.Quantity, 2).ToString();
                            txtConsumption.Attributes.Add("max", Math.Round(entity.Quantity, 2).ToString());
                        }
                        else
                        {
                            txtDistribution.Text = Math.Round(entity.Quantity, 2).ToString();
                            txtDistribution.Attributes.Add("max", Math.Round(entity.Quantity, 2).ToString());
                        }

                    }
                }
                else
                {
                    // Item Request SUDAH PERNAH ADA proses sebelumnya 

                    if (itemReqType == Constant.ItemRequestType.CONSUMPTION)
                    {
                        if ((entity.Quantity - entity.ConsumptionQty) > availableQty)
                        {
                            txtConsumption.Text = availableQty.ToString();

                            if ((entity.Quantity - entity.ConsumptionQty) != entity.PurchaseRequestQty)
                            {
                                if (hdnIsAllowPurchaseRequest.Value == "1")
                                {
                                    txtPurchaseRequest.Text = (((entity.Quantity - entity.ConsumptionQty) - availableQty) - entity.PurchaseRequestQty).ToString();
                                    txtPurchaseRequest.Attributes.Add("max", (((entity.Quantity - entity.ConsumptionQty) - availableQty) - entity.PurchaseRequestQty).ToString());
                                }
                            }

                            txtConsumption.Attributes.Add("max", availableQty.ToString());
                        }
                        else
                        {
                            txtConsumption.Text = (entity.Quantity - entity.ConsumptionQty).ToString();
                            txtConsumption.Attributes.Add("max", (entity.Quantity - entity.ConsumptionQty).ToString());
                        }
                    }
                    else
                    {
                        if ((entity.Quantity - entity.DistributionQty) > availableQty)
                        {
                            txtDistribution.Text = availableQty.ToString();

                            if ((entity.Quantity - entity.DistributionQty) != entity.PurchaseRequestQty)
                            {
                                SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_IS_PURCHASE_REQUEST);
                                if (setvardt.ParameterValue == "1")
                                {
                                    if (hdnIsAllowPurchaseRequest.Value == "1")
                                    {
                                        txtPurchaseRequest.Text = (((entity.Quantity - entity.DistributionQty) - availableQty) - entity.PurchaseRequestQty).ToString();
                                        txtPurchaseRequest.Attributes.Add("max", (((entity.Quantity - entity.DistributionQty) - availableQty) - entity.PurchaseRequestQty).ToString());
                                    }
                                }
                                else
                                {
                                    txtPurchaseRequest.Text = "0";
                                }
                            }

                            txtDistribution.Attributes.Add("max", availableQty.ToString());
                        }
                        else
                        {
                            txtDistribution.Text = (entity.Quantity - entity.DistributionQty).ToString();
                            txtDistribution.Attributes.Add("max", (entity.Quantity - entity.DistributionQty).ToString());
                        }
                    }
                }

                DataTempApp data = lstTemp.Where(t => t.Key == entity.ID).FirstOrDefault();
                if (data != null)
                {
                    if (hdnIsAllowItemDistribution.Value == "1")
                    {
                        txtDistribution.ReadOnly = false;
                    }
                    if (hdnIsAllowPurchaseRequest.Value == "1")
                    {
                        txtPurchaseRequest.ReadOnly = false;
                    }
                    if (hdnIsAllowItemConsumption.Value == "1")
                    {
                        txtConsumption.ReadOnly = false;
                    }
                    txtDistribution.Text = data.QtyDistribution.ToString();
                    txtPurchaseRequest.Text = data.QtyPR.ToString();
                    txtConsumption.Text = data.QtyConsumption.ToString();
                    chkIsSelected.Checked = true;
                }                
            }
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

        public void SavePurchaseRequestHd(IDbContext ctx, ref int purchaseRequestID, ref string purchaseRequestNo)
        {
            PurchaseRequestHdDao entityHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestHd entityHd = new PurchaseRequestHd();
            entityHd.ItemRequestID = Convert.ToInt32(hdnOrderID.Value);
            entityHd.FromLocationID = Convert.ToInt32(hdnLocationIDTo.Value);
            entityHd.TransactionDate = DateTime.Now;
            entityHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            if (hdnIsUsedProductLine.Value == "1")
            {
                if (hdnProductLineID.Value != "")
                {
                    entityHd.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
                }
            }
            entityHd.Remarks = string.Format("Permintaan Pembelian untuk permintaan Nomor {0} dari {1}", Request.Form[txtOrderNo.UniqueID], Request.Form[txtLocationName.UniqueID]);
            entityHd.PurchaseRequestNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.PURCHASE_REQUEST, entityHd.TransactionDate, ctx);
            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            purchaseRequestID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            purchaseRequestNo = entityHd.PurchaseRequestNo;
        }

        public void SaveItemDistributionHd(IDbContext ctx, ref int distributionID, ref string distributionNo)
        {
            ItemDistributionHdDao entityHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionHd entityHd = new ItemDistributionHd();
            entityHd.ItemRequestID = Convert.ToInt32(hdnOrderID.Value);
            entityHd.FromLocationID = Convert.ToInt32(hdnLocationIDTo.Value);
            entityHd.ToLocationID = Convert.ToInt32(hdnLocationIDFrom.Value);
            entityHd.TransactionDate = DateTime.Now;
            entityHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            entityHd.DeliveryRemarks = string.Format("Distribusi untuk permintaan Nomor {0} dari {1}", Request.Form[txtOrderNo.UniqueID], Request.Form[txtLocationName.UniqueID]);
            entityHd.DistributionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.ITEM_DISTRIBUTION, entityHd.TransactionDate, ctx);
            entityHd.GCDistributionStatus = Constant.DistributionStatus.OPEN;

            if (hdnRegistrationID.Value != "" && hdnRegistrationID.Value != "0")
            {
                entityHd.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
            }

            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            distributionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            distributionNo = entityHd.DistributionNo;
        }

        public void SaveItemConsumptionHd(IDbContext ctx, ref int transactionID, ref string transactionNo)
        {
            ItemTransactionHdDao entityHdDao = new ItemTransactionHdDao(ctx);
            ItemTransactionHd entityHd = new ItemTransactionHd();
            entityHd.ItemRequestID = Convert.ToInt32(hdnOrderID.Value);
            entityHd.FromLocationID = Convert.ToInt32(hdnLocationIDTo.Value);
            entityHd.ToLocationID = null;
            entityHd.TransactionDate = Helper.GetDatePickerValue(txtItemOrderDate.Text);
            entityHd.GCConsumptionType = hdnDefaultGCConsumptionType.Value;
            entityHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            //entityHd.DeliveryTime = txtItemOrderTime.Text;

            if (hdnRegistrationID.Value != "" && hdnRegistrationID.Value != "0")
            {
                entityHd.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
            }

            Location entityLocation = BusinessLayer.GetLocation(Convert.ToInt32(hdnLocationIDFrom.Value));
            entityHd.GCHealthcareUnit = entityLocation.GCHealthcareUnit;

            entityHd.Remarks = string.Format("Pemakaian untuk permintaan Nomor {0} dari {1}", Request.Form[txtOrderNo.UniqueID], Request.Form[txtLocationName.UniqueID]);
            entityHd.TransactionCode = Constant.TransactionCode.ITEM_CONSUMPTION;
            entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TransactionDate, ctx);
            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            transactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            transactionNo = entityHd.TransactionNo;
        }

        protected string BeforeDistributionConsumption()
        {
            string param = "0";

            bool flagID = false;
            bool flagIC = false;

            List<ItemDistributionDt> lstID = new List<ItemDistributionDt>();
            List<ItemTransactionDt> lstIC = new List<ItemTransactionDt>();

            foreach (DataTempApp e in lstTemp)
            {
                if (e.QtyDistribution != 0)
                {
                    flagID = true;
                }

                if (e.QtyConsumption != 0)
                {
                    flagIC = true;
                }

                string filterID = string.Format("DistributionID IN (SELECT DistributionID FROM ItemDistributionHd WHERE ItemRequestID = {0} AND GCDistributionStatus IN ('{1}','{2}')) AND ItemID IN ({3}) AND IsDeleted = 0",
                                    hdnOrderID.Value, Constant.DistributionStatus.OPEN, Constant.DistributionStatus.WAIT_FOR_APPROVAL, e.ItemID);
                List<ItemDistributionDt> lstDist = BusinessLayer.GetItemDistributionDtList(filterID);
                lstID.AddRange(lstDist);

                string filterIC = string.Format("TransactionID IN (SELECT TransactionID FROM ItemTransactionHd WHERE ItemRequestID = {0} AND GCTransactionStatus IN ('{1}','{2}')) AND ItemID IN ({3}) AND GCItemDetailStatus != '{4}'",
                                    hdnOrderID.Value, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL, e.ItemID, Constant.TransactionStatus.VOID);
                List<ItemTransactionDt> lstCon = BusinessLayer.GetItemTransactionDtList(filterIC);
                lstIC.AddRange(lstCon);
            }

            if (flagID)
            {
                if (lstID.Count() > 0)
                {
                    param = "1";
                }
            }

            if (flagIC)
            {
                if (lstIC.Count() > 0)
                {
                    param = "1";
                }
            }

            return param;
        }

        protected void setDataBeforeSave()
        {
            string[] paramNew = hdnDataSave.Value.Split('$');
            lstTemp = new List<DataTempApp>();

            for (int i = 0; i < paramNew.Length; i++)
            {
                if (!String.IsNullOrEmpty(paramNew[i]))
                {
                    string[] paramNewSplit = paramNew[i].Split('|');

                    int keyNew = Convert.ToInt32(paramNewSplit[1]);
                    decimal qtyDistributionNew = Convert.ToDecimal(paramNewSplit[2]);
                    decimal qtyConsumptionNew = Convert.ToDecimal(paramNewSplit[3]);
                    decimal qtyPRNew = Convert.ToDecimal(paramNewSplit[4]);
                    int itemIDNew = Convert.ToInt32(paramNewSplit[5]);
                    string remarkNew = paramNewSplit[6];

                    DataTempApp oData = new DataTempApp();
                    oData.Key = keyNew;
                    oData.QtyDistribution = qtyDistributionNew;
                    oData.QtyConsumption = qtyConsumptionNew;
                    oData.QtyPR = qtyPRNew;
                    oData.ItemID = itemIDNew;
                    oData.remark = remarkNew;
                    lstTemp.Add(oData);
                }
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            setDataBeforeSave();

            if (type == "checkbefore")
            {
                string hasil = BeforeDistributionConsumption();
                if (hasil == "1")
                {
                    retval = "failed|";
                }
                else
                {
                    retval = "next|";
                }
            }
            else
            {
                bool flagPR = false;
                bool flagID = false;
                bool flagIC = false;

                string purchaseRequestNo = "";
                string distributionNo = "";
                string itemConsumptionNo = "";

                foreach (DataTempApp e in lstTemp)
                {
                    if (e.QtyPR != 0)
                    {
                        flagPR = true;
                    }

                    if (e.QtyDistribution != 0)
                    {
                        flagID = true;
                    }

                    if (e.QtyConsumption != 0)
                    {
                        flagIC = true;
                    }
                }

                IDbContext ctx = DbFactory.Configure(true);
                int purchaseRequestID = 0;
                int distributionID = 0;
                int itemConsumptionID = 0;
                PurchaseRequestDtDao prDtDao = new PurchaseRequestDtDao(ctx);
                ItemDistributionDtDao idDtDao = new ItemDistributionDtDao(ctx);
                ItemTransactionDtDao itDtDao = new ItemTransactionDtDao(ctx);
                ItemRequestDtDao entityItemRequestDtDao = new ItemRequestDtDao(ctx);
                ItemRequestHdDao entityItemRequestHdDao = new ItemRequestHdDao(ctx);
                ItemTransactionHdDao entityItemTransactionHdDao = new ItemTransactionHdDao(ctx);

                SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_ITEM_REQUEST_ALLOW_OUTSTANDING);
                string isAllowOutstanding = setvardt.ParameterValue == null ? "1" : setvardt.ParameterValue;

                try
                {
                    // ditutup oleh RN pd 20200528
                    //string filterIRCheck = string.Format("ReferenceNo = '{0}' AND GCTransactionStatus != '{0}'", txtOrderNo.Text, Constant.TransactionStatus.VOID);
                    //List<ItemRequestHd> lstItemRequestCheck = BusinessLayer.GetItemRequestHdList(filterIRCheck, ctx);

                    string filterIRCheck = string.Format("ItemRequestNo = '{0}' AND GCTransactionStatus NOT IN ('{1}','{2}')", txtOrderNo.Text, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED);
                    List<ItemRequestHd> lstItemRequestCheck = BusinessLayer.GetItemRequestHdList(filterIRCheck, ctx);
                    if (lstItemRequestCheck.Count() == 0)
                    {
                        if (flagPR || flagIC || flagID)
                        {
                            if (type == "approve")
                            {
                                #region PurchaseRequestDt
                                if (flagPR)
                                {
                                    SavePurchaseRequestHd(ctx, ref purchaseRequestID, ref purchaseRequestNo);
                                    foreach (DataTempApp entityPR in lstTemp.Where(t => t.QtyPR != 0).ToList())
                                    {
                                        ItemRequestDt entityItemReqDt = entityItemRequestDtDao.Get(entityPR.Key);
                                        PurchaseRequestDt itemDt = new PurchaseRequestDt();

                                        //entityItemReqDt.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                                        itemDt.PurchaseRequestID = purchaseRequestID;
                                        itemDt.ItemID = Convert.ToInt32(entityItemReqDt.ItemID);
                                        itemDt.Quantity = entityPR.QtyPR;
                                        itemDt.ConversionFactor = entityItemReqDt.ConversionFactor;
                                        itemDt.GCPurchaseUnit = entityItemReqDt.GCItemUnit;
                                        itemDt.GCBaseUnit = entityItemReqDt.GCBaseUnit;

                                        vSupplierItemPlaning vPlan = BusinessLayer.GetvSupplierItemPlaningList(string.Format("ItemID = {0}", entityItemReqDt.ItemID), ctx).FirstOrDefault();
                                        //if (vPlan.Count > 0)
                                        //{
                                        //    itemDt.BusinessPartnerID = vPlan[0].BusinessPartnerID;
                                        //    itemDt.UnitPrice = vPlan[0].UnitPrice * entityItemReqDt.ConversionFactor;
                                        //    itemDt.DiscountPercentage = vPlan[0].Discount;
                                        //}
                                        //else
                                        //{
                                        //    itemDt.BusinessPartnerID = null;
                                        //    itemDt.UnitPrice = Convert.ToDecimal(0.00);
                                        //    itemDt.DiscountPercentage = Convert.ToDecimal(0.00);
                                        //}

                                        if (vPlan.BusinessPartnerID != 0 && vPlan.BusinessPartnerID != null)
                                        {
                                            GetItemMasterPurchase itemMasterPurchase = BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, vPlan.ItemID, (int)vPlan.BusinessPartnerID, ctx).FirstOrDefault();
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            if (itemMasterPurchase != null)
                                            {
                                                itemDt.BusinessPartnerID = itemMasterPurchase.BusinessPartnerID;
                                                if (itemDt.ConversionFactor == 1)
                                                {
                                                    itemDt.UnitPrice = itemMasterPurchase.Price;
                                                }
                                                else
                                                {
                                                    itemDt.UnitPrice = itemMasterPurchase.UnitPrice;
                                                }
                                                //itemDt.GCPurchaseUnit = itemMasterPurchase.PurchaseUnit;
                                                //itemDt.ConversionFactor = itemMasterPurchase.ConversionFactor;
                                                itemDt.DiscountPercentage = itemMasterPurchase.Discount;
                                                itemDt.DiscountPercentage2 = itemMasterPurchase.Discount2;
                                            }
                                            else
                                            {
                                                itemDt.UnitPrice = 0;
                                                itemDt.DiscountPercentage = 0;
                                                itemDt.DiscountPercentage2 = 0;
                                                //itemDt.GCPurchaseUnit = entityItemReqDt.GCItemUnit;
                                                //itemDt.ConversionFactor = 1;
                                            }
                                        }
                                        else
                                        {
                                            itemDt.BusinessPartnerID = null;
                                            itemDt.UnitPrice = Convert.ToDecimal(0.00);
                                            itemDt.DiscountPercentage = Convert.ToDecimal(0.00);
                                        }

                                        itemDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                                        itemDt.CreatedBy = AppSession.UserLogin.UserID;

                                        entityItemReqDt.PurchaseRequestQty += itemDt.Quantity;
                                        entityItemReqDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityItemRequestDtDao.Update(entityItemReqDt);
                                        prDtDao.Insert(itemDt);
                                    }
                                }
                                #endregion

                                #region ItemDistributionDt
                                if (result)
                                {
                                    if (flagID)
                                    {
                                        SaveItemDistributionHd(ctx, ref distributionID, ref distributionNo);
                                        foreach (DataTempApp entityDistribution in lstTemp.Where(t => t.QtyDistribution != 0).ToList())
                                        {
                                            ItemRequestDt entityItemReqDt = entityItemRequestDtDao.Get(entityDistribution.Key);
                                            ItemDistributionDt itemDt = new ItemDistributionDt();
                                            //entityItemReqDt.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                                            itemDt.DistributionID = distributionID;
                                            itemDt.ItemID = Convert.ToInt32(entityItemReqDt.ItemID);
                                            itemDt.Quantity = entityDistribution.QtyDistribution;
                                            itemDt.ConversionFactor = entityItemReqDt.ConversionFactor;
                                            itemDt.GCItemUnit = entityItemReqDt.GCItemUnit;
                                            itemDt.GCBaseUnit = entityItemReqDt.GCBaseUnit;
                                            itemDt.GCItemDetailStatus = Constant.DistributionStatus.OPEN;
                                            itemDt.CreatedBy = AppSession.UserLogin.UserID;

                                            ItemPlanning entityPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0}", Convert.ToInt32(entityItemReqDt.ItemID))).FirstOrDefault();
                                            if (entityPlanning != null)
                                            {
                                                itemDt.AveragePrice = entityPlanning.AveragePrice;
                                            }
                                            else
                                            {
                                                itemDt.AveragePrice = 0;
                                            }

                                            entityItemReqDt.DistributionQty += itemDt.Quantity;
                                            entityItemReqDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            entityItemRequestDtDao.Update(entityItemReqDt);
                                            idDtDao.Insert(itemDt);
                                        }
                                    }
                                }
                                #endregion

                                #region ItemConsumptionDt
                                if (result)
                                {
                                    if (flagIC)
                                    {
                                        SaveItemConsumptionHd(ctx, ref itemConsumptionID, ref itemConsumptionNo);
                                        foreach (DataTempApp entityConsumsion in lstTemp.Where(t => t.QtyConsumption != 0).ToList())
                                        {
                                            ItemRequestDt entityItemReqDt = entityItemRequestDtDao.Get(entityConsumsion.Key);
                                            List<vSupplierItemPlaning> vPlan = BusinessLayer.GetvSupplierItemPlaningList(string.Format("ItemID = {0}", entityItemReqDt.ItemID), ctx);
                                            ItemTransactionDt itemDt = new ItemTransactionDt();
                                            //entityItemReqDt.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                                            itemDt.TransactionID = itemConsumptionID;
                                            itemDt.ItemID = Convert.ToInt32(entityItemReqDt.ItemID);
                                            itemDt.Quantity = entityConsumsion.QtyConsumption;
                                            itemDt.ConversionFactor = entityItemReqDt.ConversionFactor;
                                            itemDt.GCItemUnit = entityItemReqDt.GCItemUnit;
                                            itemDt.GCBaseUnit = entityItemReqDt.GCBaseUnit;
                                            if (vPlan.Count > 0)
                                            {
                                                itemDt.CostAmount = vPlan.FirstOrDefault().AveragePrice * entityItemReqDt.ConversionFactor;
                                            }
                                            else
                                            {
                                                itemDt.CostAmount = Convert.ToDecimal(0.00);
                                            }
                                            itemDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                                            itemDt.CreatedBy = AppSession.UserLogin.UserID;

                                            entityItemReqDt.ConsumptionQty += itemDt.Quantity;
                                            entityItemReqDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            entityItemRequestDtDao.Update(entityItemReqDt);
                                            itDtDao.Insert(itemDt);
                                        }
                                    }
                                }
                                #endregion

                                foreach (DataTempApp entity in lstTemp)
                                {
                                    ItemRequestDt entityItemReqDt = entityItemRequestDtDao.Get(entity.Key);
                                    if (entityItemReqDt.GCItemDetailStatus == Constant.TransactionStatus.APPROVED || entityItemReqDt.GCItemDetailStatus == Constant.TransactionStatus.PROCESSED)
                                    {
                                        if (entityItemReqDt.Quantity == (entityItemReqDt.DistributionQty + entityItemReqDt.ConsumptionQty))
                                        {
                                            entityItemReqDt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                                        }
                                        else
                                        {
                                            if (isAllowOutstanding == "0")
                                            {
                                                entityItemReqDt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                                            }
                                            else
                                            {
                                                entityItemReqDt.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                                            }
                                        }
                                        entityItemReqDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityItemRequestDtDao.Update(entityItemReqDt);
                                    }
                                    else
                                    {
                                        errMessage = "Harap refresh halaman ini.";
                                        Exception ex = new Exception(errMessage);
                                        Helper.InsertErrorLog(ex);
                                        result = false;
                                    }
                                }
                            }
                            else if (type == "close")
                            {
                                foreach (DataTempApp entity in lstTemp)
                                {
                                    ItemRequestDt itemReq = BusinessLayer.GetItemRequestDt(entity.Key);
                                    if (itemReq.GCItemDetailStatus == Constant.TransactionStatus.APPROVED || itemReq.GCItemDetailStatus == Constant.TransactionStatus.PROCESSED)
                                    {
                                        itemReq.ClosedReasonRemarks = entity.remark;
                                        itemReq.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                                        itemReq.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityItemRequestDtDao.Update(itemReq);
                                    }
                                    else
                                    {
                                        errMessage = "Harap refresh halaman ini.";
                                        Exception ex = new Exception(errMessage);
                                        Helper.InsertErrorLog(ex);
                                        result = false;
                                    }
                                }
                            }
                            else
                            {
                                foreach (DataTempApp entity in lstTemp)
                                {
                                    ItemRequestDt itemReq = BusinessLayer.GetItemRequestDt(entity.Key);
                                    if (itemReq.GCItemDetailStatus == Constant.TransactionStatus.APPROVED || itemReq.GCItemDetailStatus == Constant.TransactionStatus.PROCESSED)
                                    {
                                        itemReq.DeclineReasonRemarks = entity.remark;
                                        itemReq.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                                        itemReq.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityItemRequestDtDao.Update(itemReq);
                                    }
                                    else
                                    {
                                        errMessage = "Harap refresh halaman ini.";
                                        Exception ex = new Exception(errMessage);
                                        Helper.InsertErrorLog(ex);
                                        result = false;
                                    }
                                }
                            }

                            int countALL = BusinessLayer.GetItemRequestDtRowCount(string.Format("ItemRequestID = {0} AND GCItemDetailStatus NOT IN ('{1}') AND IsDeleted = 0", hdnOrderID.Value, Constant.TransactionStatus.VOID), ctx);
                            int countApproved = BusinessLayer.GetItemRequestDtRowCount(string.Format("ItemRequestID = {0} AND GCItemDetailStatus IN ('{1}') AND IsDeleted = 0", hdnOrderID.Value, Constant.TransactionStatus.APPROVED), ctx);
                            int countProcessed = BusinessLayer.GetItemRequestDtRowCount(string.Format("ItemRequestID = {0} AND GCItemDetailStatus IN ('{1}') AND IsDeleted = 0", hdnOrderID.Value, Constant.TransactionStatus.PROCESSED), ctx);
                            int countClosed = BusinessLayer.GetItemRequestDtRowCount(string.Format("ItemRequestID = {0} AND GCItemDetailStatus IN ('{1}') AND IsDeleted = 0", hdnOrderID.Value, Constant.TransactionStatus.CLOSED), ctx);
                            int countNonVoid = BusinessLayer.GetItemRequestDtRowCount(string.Format("ItemRequestID = {0} AND GCItemDetailStatus NOT IN ('{1}') AND IsDeleted = 0", hdnOrderID.Value, Constant.TransactionStatus.VOID), ctx);
                            int countApprovedAndVoid = BusinessLayer.GetItemRequestDtRowCount(string.Format("ItemRequestID = {0} AND GCItemDetailStatus IN ('{1}') AND IsDeleted = 0 AND (DistributionQty != 0 OR ConsumptionQty != 0 OR PurchaseRequestQty != 0)", hdnOrderID.Value, Constant.TransactionStatus.VOID), ctx);
                            //retval = string.Format("{0}|{1}|{2}|{3}", (countApproved + countProcessed), purchaseRequestNo, distributionNo, itemConsumptionNo);
                            if (type == "approve")
                            {
                                retval = string.Format("approve|{0}|{1}|{2}|{3}", (countApproved + countProcessed), purchaseRequestNo, distributionNo, itemConsumptionNo);
                            }
                            else if (type == "close")
                            {
                                retval = string.Format("close|{0}|{1}|{2}|{3}", (countApproved + countProcessed), purchaseRequestNo, distributionNo, itemConsumptionNo);
                            }
                            else
                            {
                                retval = string.Format("decline|{0}|{1}|{2}|{3}", (countApproved + countProcessed), purchaseRequestNo, distributionNo, itemConsumptionNo);
                            }
                            if (countApproved == 0)
                            {
                                ItemRequestHd entityItemRequestHd = entityItemRequestHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                                if (type == "approve")
                                {
                                    if (countProcessed == 0)
                                    {
                                        entityItemRequestHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                    }
                                    else
                                    {
                                        if (isAllowOutstanding == "0")
                                        {
                                            entityItemRequestHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                        }
                                        else
                                        {
                                            entityItemRequestHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                        }
                                    }
                                }
                                else if (type == "close")
                                {
                                    string close = "close";
                                    retval = string.Format("{0}", close);
                                    if (countClosed == countALL)
                                    {
                                        entityItemRequestHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                    }
                                    else
                                    {
                                        if (isAllowOutstanding == "0")
                                        {
                                            entityItemRequestHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                        }
                                        else
                                        {
                                            entityItemRequestHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                        }
                                    }
                                }
                                else
                                {
                                    string decline = "decline";
                                    retval = string.Format("{0}", decline);
                                    if (countNonVoid == 0 && countApprovedAndVoid == 0)
                                    {
                                        entityItemRequestHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    }
                                    else if (countProcessed == 0 || countApprovedAndVoid != 0)
                                    {
                                        entityItemRequestHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                    }
                                }
                                entityItemRequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityItemRequestHdDao.Update(entityItemRequestHd);
                            }
                        }
                        else
                        {
                            if (type == "close")
                            {
                                foreach (DataTempApp entity in lstTemp)
                                {
                                    ItemRequestDt itemReq = BusinessLayer.GetItemRequestDt(entity.Key);
                                    if (itemReq.GCItemDetailStatus == Constant.TransactionStatus.APPROVED || itemReq.GCItemDetailStatus == Constant.TransactionStatus.PROCESSED)
                                    {
                                        itemReq.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                                        itemReq.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityItemRequestDtDao.Update(itemReq);
                                    }
                                    else
                                    {
                                        errMessage = "Harap refresh halaman ini.";
                                        Exception ex = new Exception(errMessage);
                                        Helper.InsertErrorLog(ex);
                                        result = false;
                                    }
                                }

                                string close = "close";
                                int countALL = BusinessLayer.GetItemRequestDtRowCount(string.Format("ItemRequestID = {0} AND GCItemDetailStatus NOT IN ('{1}') AND IsDeleted = 0", hdnOrderID.Value, Constant.TransactionStatus.VOID), ctx);
                                int countClosed = BusinessLayer.GetItemRequestDtRowCount(string.Format("ItemRequestID = {0} AND GCItemDetailStatus IN ('{1}') AND IsDeleted = 0", hdnOrderID.Value, Constant.TransactionStatus.CLOSED), ctx);
                                retval = string.Format("{0}", close);

                                ItemRequestHd entityItemRequestHd = entityItemRequestHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                                if (countClosed == countALL)
                                {
                                    entityItemRequestHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                }
                                else
                                {
                                    if (isAllowOutstanding == "0")
                                    {
                                        entityItemRequestHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                    }
                                    else
                                    {
                                        entityItemRequestHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                    }
                                }
                                entityItemRequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityItemRequestHdDao.Update(entityItemRequestHd);
                            }
                            else
                            {
                                foreach (DataTempApp entity in lstTemp)
                                {
                                    ItemRequestDt itemReq = BusinessLayer.GetItemRequestDt(entity.Key);
                                    if (itemReq.GCItemDetailStatus == Constant.TransactionStatus.APPROVED)
                                    {
                                        itemReq.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                                        itemReq.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityItemRequestDtDao.Update(itemReq);
                                    }
                                    else
                                    {
                                        errMessage = "Harap refresh halaman ini.";
                                        Exception ex = new Exception(errMessage);
                                        Helper.InsertErrorLog(ex);
                                        result = false;
                                    }
                                }

                                string decline = "decline";
                                int countNonVoid = BusinessLayer.GetItemRequestDtRowCount(string.Format("ItemRequestID = {0} AND GCItemDetailStatus NOT IN ('{1}') AND IsDeleted = 0", hdnOrderID.Value, Constant.TransactionStatus.VOID), ctx);
                                int countProcessed = BusinessLayer.GetItemRequestDtRowCount(string.Format("ItemRequestID = {0} AND GCItemDetailStatus IN ('{1}','{2}') AND IsDeleted = 0", hdnOrderID.Value, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.APPROVED), ctx);
                                retval = string.Format("{0}", decline);

                                ItemRequestHd entityItemRequestHd = entityItemRequestHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                                if (countNonVoid == 0)
                                {
                                    entityItemRequestHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                }
                                else if (countProcessed == 0)
                                {
                                    entityItemRequestHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                }
                                entityItemRequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityItemRequestHdDao.Update(entityItemRequestHd);
                            }
                            //errMessage = "Tidak ada permintaan yang bisa diproses.";
                            //Exception ex = new Exception(errMessage);
                            //Helper.InsertErrorLog(ex);
                            //result = false;
                        }

                        if (result)
                        {
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Nomor permintaan {0} tidak dapat diproses karena sudah diproses salin permintaan barang.", txtOrderNo.Text);
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
            }
            return result;
        }
    }

    public class DataTempApp
    {
        public int Key { get; set; }
        public decimal QtyDistribution { get; set; }
        public decimal QtyConsumption { get; set; }
        public decimal QtyPR { get; set; }
        public int ItemID { get; set; }
        public string remark { get; set; }
    }
}