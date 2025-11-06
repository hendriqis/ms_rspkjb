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
    public partial class ReorderPurchaseRequest : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";
        private string[] lstSelectedMember = null;
        private string[] lstQtyPurchaseRequest = null;
        protected string filterExpressionSupplier = "";

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.REORDER_PURCHASE_REQUEST;
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
            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}')", AppSession.UserLogin.HealthcareID,
                            Constant.SettingParameter.IM_REORDER_PR_BY_QTY_END,
                            Constant.SettingParameter.IM_AUTO_APPROVE_PR_FROM_REORDER,
                            Constant.SettingParameter.IM_OUTSTANDING_PO_PR_VISIBLE_IN_ROP_PR,
                            Constant.SettingParameter.IM_QTY_ROP_BOLEH_LEBIH_BESAR_DARI_QTY_MAX,
                            Constant.SettingParameter.IM_IS_PURCHASE_REQUEST_ALLOW_CHANGE_OVER_RECOMMENDATION,
                            Constant.SettingParameter.IM0131
                            );
            List<SettingParameterDt> oParamList = BusinessLayer.GetSettingParameterDtList(filterExpression);
            SettingParameterDt oParam1 = oParamList.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_REORDER_PR_BY_QTY_END).FirstOrDefault();
            SettingParameterDt oParam2 = oParamList.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_AUTO_APPROVE_PR_FROM_REORDER).FirstOrDefault();
            SettingParameterDt oParam3 = oParamList.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_OUTSTANDING_PO_PR_VISIBLE_IN_ROP_PR).FirstOrDefault();
            SettingParameterDt oParam4 = oParamList.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_QTY_ROP_BOLEH_LEBIH_BESAR_DARI_QTY_MAX).FirstOrDefault();
            SettingParameterDt oParam5 = oParamList.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_IS_PURCHASE_REQUEST_ALLOW_CHANGE_OVER_RECOMMENDATION).FirstOrDefault();
            SettingParameterDt oParam6 = oParamList.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM0131).FirstOrDefault();
            hdnIsOutstandingPOPRVisible.Value = oParam3.ParameterValue;
            hdnQtyPRAllowBiggerThanReccomend.Value = oParam5.ParameterValue;
            hdnIM0131.Value = oParam6.ParameterValue;

            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_IS_PURCHASE_REQUEST_SERVICE_UNIT);
            hdnIsUsedPurchaseOrderType.Value = setvardt.ParameterValue;

            List<StandardCode> listStandardCodePO = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.PURCHASE_ORDER_TYPE));
            //listStandardCodePO.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPurchaseOrderType, listStandardCodePO, "StandardCodeName", "StandardCodeID");
            cboPurchaseOrderType.SelectedIndex = 0;

            hdnIsUsedProductLine.Value = AppSession.IsUsedProductLine;

            if (hdnIsUsedProductLine.Value == "1")
            {
                trProductLine.Style.Remove("display");
                lblProductLine.Attributes.Add("class", "lblLink lblMandatory");

                trItemType.Style.Add("display", "none");
            }
            else
            {
                trProductLine.Style.Add("display", "none");
                lblProductLine.Attributes.Remove("class");

                trItemType.Style.Remove("display");
            }

            if (hdnIsUsedPurchaseOrderType.Value == "1")
            {
                trPurchaseOrderType.Style.Remove("display");
            }
            else
            {
                trPurchaseOrderType.Style.Add("display", "none");
            }

            hdnSortByQuantityEND.Value = oParam1 != null ? oParam1.ParameterValue : "0";
            hdnAutoApprovePR.Value = oParam2 != null ? oParam2.ParameterValue : "0";
            hdnQtyPRAllowBiggerThanQtyMax.Value = oParam4 != null ? oParam4.ParameterValue : "1";

            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PURCHASE_REQUEST);
            BindGridView(1, true, ref PageCount);
            hdnPageCount.Value = PageCount.ToString();

            filterExpressionSupplier = string.Format("GCBusinessPartnerType = '{0}' AND IsActive = 1 AND IsDeleted = 0 AND IsBlackList = 0", Constant.BusinessObjectType.SUPPLIER);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnLocationIDFrom, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(txtPurchaseRequestDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPurchaseRequestTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

            if (hdnIsUsedProductLine.Value == "1")
            {
                SetControlEntrySetting(lblProductLine, new ControlEntrySetting(true, false));
                SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(hdnProductLineItemType, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, true));
            }
            else
            {
                SetControlEntrySetting(lblProductLine, new ControlEntrySetting(true, true));
                SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(hdnProductLineItemType, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, false));
            }
        }

        public void SavePurchaseRequestHd(IDbContext ctx, ref int purchaseReqID, ref string retval)
        {
            PurchaseRequestHdDao entityHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestHd entityHd = new PurchaseRequestHd();
            entityHd.FromLocationID = Convert.ToInt32(hdnLocationIDFrom.Value);
            entityHd.TransactionDate = Helper.GetDatePickerValue(txtPurchaseRequestDate.Text);
            entityHd.TransactionTime = txtPurchaseRequestTime.Text;
            entityHd.Remarks = txtNotes.Text;
            if (hdnIsUsedProductLine.Value == "1")
            {
                entityHd.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }
            if (hdnIsUsedPurchaseOrderType.Value == "1")
            {
                entityHd.GCPurchaseOrderType = Convert.ToString(cboPurchaseOrderType.Value);
            }
            entityHd.PurchaseRequestNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.PURCHASE_REQUEST, entityHd.TransactionDate, ctx);
            retval = entityHd.PurchaseRequestNo;
            if (hdnAutoApprovePR.Value == "1")
            {
                entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                entityHd.ApprovedBy = AppSession.UserLogin.UserID;
                entityHd.ApprovedDate = DateTime.Now;
            }
            else
            {
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            }
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            purchaseReqID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (rblROPDynamic.SelectedValue.ToLower().Equals("false"))
            //{
            //    grdView.Columns[5].Visible = false;
            //}
            //else grdView.Columns[5].Visible = true;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemBalance2 entity = e.Row.DataItem as vItemBalance2;
                HtmlInputHidden hdnRecommendQtyPR = (HtmlInputHidden)e.Row.FindControl("hdnRecommendQtyPR");
                TextBox txtPurchaseRequest = e.Row.FindControl("txtPurchaseRequest") as TextBox;
                CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsSelected");
                HtmlGenericControl lblSupplier = (HtmlGenericControl)e.Row.FindControl("lblSupplier");
                HtmlInputHidden hdnSupplierID = (HtmlInputHidden)e.Row.FindControl("hdnSupplierID");

                List<GetItemMasterPurchase> impList = BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, entity.ItemID, 0);
                if (impList.Count > 0)
                {
                    GetItemMasterPurchase imp = impList.FirstOrDefault();
                    if (imp.BusinessPartnerID != 0)
                    {
                        lblSupplier.InnerHtml = imp.BusinessPartnerName;
                        hdnSupplierID.Value = imp.BusinessPartnerID.ToString();
                    }
                    else
                    {
                        lblSupplier.InnerHtml = "Pilih Supplier";
                        hdnSupplierID.Value = "0";
                    }
                }
                else
                {
                    lblSupplier.InnerHtml = "Pilih Supplier";
                    hdnSupplierID.Value = "0";
                }

                Decimal autoQty = 0;
                if (rblROPDynamic.SelectedValue.ToLower().Equals("true"))
                {
                    autoQty = Math.Ceiling(entity.AutoQtyDynamic);
                    if (autoQty < 0)
                    {
                        autoQty = 0;
                    }
                }
                else
                {
                    //autoQty = Math.Ceiling(entity.AutoQtyStatic); -> perhitungan ini sekarang manual sesuai pilihan By Qty & By Location

                    // filter By Qty
                    if (rblByQty.SelectedValue == "min")
                    {
                        // filter By Location
                        if (rblByLocation.SelectedValue == "1")
                        {
                            autoQty = Math.Ceiling((entity.QuantityMAX - entity.QtyOnHandAll - entity.PurchaseRequestQtyOnOrder - entity.PurchaseOrderQtyOnOrder) / entity.ConversionFactor);
                        }
                        else
                        {
                            autoQty = Math.Ceiling((entity.QuantityMAX - entity.QuantityEND - entity.PurchaseRequestQtyOnOrder - entity.PurchaseOrderQtyOnOrder) / entity.ConversionFactor);
                        }
                    }
                    else
                    {
                        // filter By Location
                        if (rblByLocation.SelectedValue == "1")
                        {
                            autoQty = Math.Ceiling((entity.QuantityMAX - entity.QtyOnHandAll - entity.PurchaseRequestQtyOnOrder - entity.PurchaseOrderQtyOnOrder) / entity.ConversionFactor);
                        }
                        else
                        {
                            autoQty = Math.Ceiling((entity.QuantityMAX - entity.QuantityEND - entity.PurchaseRequestQtyOnOrder - entity.PurchaseOrderQtyOnOrder) / entity.ConversionFactor);
                        }
                    }

                    if (autoQty < 0)
                    {
                        autoQty = 0;
                    }
                }

                //txtPurchaseRequest.Text = autoQty.ToString("N");

                if (hdnQtyPRAllowBiggerThanQtyMax.Value == "0")
                {
                    txtPurchaseRequest.Attributes.Add("max", entity.QuantityMAX.ToString());
                    txtPurchaseRequest.Text = entity.QuantityMAX.ToString(Constant.FormatString.NUMERIC_2);
                }
                else
                {
                    txtPurchaseRequest.Attributes.Remove("max");
                    txtPurchaseRequest.Text = autoQty.ToString(Constant.FormatString.NUMERIC_2);
                }

                if (hdnQtyPRAllowBiggerThanReccomend.Value == "0")
                {
                    txtPurchaseRequest.Attributes.Add("max", autoQty.ToString());
                    txtPurchaseRequest.Text = autoQty.ToString(Constant.FormatString.NUMERIC_2);
                }
                else
                {
                    txtPurchaseRequest.Attributes.Remove("max");
                    txtPurchaseRequest.Text = autoQty.ToString(Constant.FormatString.NUMERIC_2);
                }

                hdnRecommendQtyPR.Value = autoQty.ToString();

                if (lstSelectedMember.Contains(entity.ID.ToString()))
                {
                    int idx = Array.IndexOf(lstSelectedMember, entity.ID.ToString());
                    chkIsSelected.Checked = true;
                    txtPurchaseRequest.ReadOnly = false;
                    txtPurchaseRequest.Text = lstQtyPurchaseRequest[idx];
                }

                if (autoQty <= 0)
                {
                    e.Row.BackColor = System.Drawing.Color.LightGray;
                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.White;
                }
            }
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            string isUsingDynamic = "0";
            if (rblROPDynamic.SelectedValue.ToLower().Equals("true")) { isUsingDynamic = "1"; }

            // filter Location
            if (hdnLocationIDFrom.Value != "")
            {
                filterExpression = string.Format("LocationID = {0} AND IsUsingDynamicROP = {1} AND IsProductionItem = 0 AND IsDeleted = 0 AND GCItemStatus != '{2}'",
                                        hdnLocationIDFrom.Value, isUsingDynamic, Constant.ItemStatus.IN_ACTIVE);
            }

            // filter ProductLine
            if (hdnIsUsedProductLine.Value == "1")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }

                if (hdnProductLineID.Value != "")
                {
                    filterExpression += string.Format("ProductLineID = {0}", hdnProductLineID.Value);
                }
                else
                {
                    filterExpression += string.Format("ProductLineID = 0");
                }
            }

            // filter ItemType
            if (rblItemType.SelectedValue != "0")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }

                switch (rblItemType.SelectedValue)
                {
                    case "2":
                        filterExpression += string.Format("GCItemType = '{0}'", Constant.ItemType.OBAT_OBATAN);
                        break;
                    case "3":
                        filterExpression += string.Format("GCItemType = '{0}'", Constant.ItemType.BARANG_MEDIS);
                        break;
                    case "8":
                        filterExpression += string.Format("GCItemType = '{0}'", Constant.ItemType.BARANG_UMUM);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }

                if (hdnGCLocationGroupFrom.Value != "")
                {
                    if (hdnGCLocationGroupFrom.Value == Constant.LocationGroup.DRUG_AND_MEDICAL_SUPPLIES)
                    {
                        filterExpression += string.Format("GCItemType IN ('{0}','{1}')", Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);
                    }
                    else if (hdnGCLocationGroupFrom.Value == Constant.LocationGroup.LOGISTIC)
                    {
                        filterExpression += string.Format("GCItemType IN ('{0}')", Constant.ItemType.BARANG_UMUM);
                    }
                    else
                    {
                        filterExpression += string.Format("GCItemType IN ('{0}','{1}','{2}')",
                                Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM);
                    }
                }
                else
                {
                    filterExpression += string.Format("GCItemType IN ('{0}','{1}','{2}')",
                            Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM);
                }
            }

            // filter ROP Type (Statis / Dinamis)
            if (hdnDisplayOption.Value == "1")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }

                if (isUsingDynamic == "1")
                {
                    filterExpression += "AutoQtyDynamic > 0";
                }
                else
                {
                    ////filterExpression += "AutoQtyStatic > 0"; -> perhitungan ini sekarang manual sesuai pilihan By Qty & By Location

                    // filter By Qty
                    if (rblByQty.SelectedValue == "min")
                    {
                        // filter By Location
                        if (rblByLocation.SelectedValue == "1")
                        {
                            filterExpression += "((QuantityMIN - QtyOnHandAll - PurchaseRequestQtyOnOrder - PurchaseOrderQtyOnOrder) / ConversionFactor) > 0";
                        }
                        else
                        {
                            filterExpression += "((QuantityMIN - QuantityEND - PurchaseRequestQtyOnOrder - PurchaseOrderQtyOnOrder) / ConversionFactor) > 0";
                        }
                    }
                    else
                    {
                        // filter By Location
                        if (rblByLocation.SelectedValue == "1")
                        {
                            filterExpression += "((QuantityMAX - QtyOnHandAll - PurchaseRequestQtyOnOrder - PurchaseOrderQtyOnOrder) / ConversionFactor) > 0";
                        }
                        else
                        {
                            filterExpression += "((QuantityMAX - QuantityEND - PurchaseRequestQtyOnOrder - PurchaseOrderQtyOnOrder) / ConversionFactor) > 0";
                        }
                    }
                }
            }

            // filter outstanding PO&PR visible
            if (hdnIsOutstandingPOPRVisible.Value == "0")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += "PurchaseRequestQtyOnOrder = 0 AND PurchaseOrderQtyOnOrder = 0";
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            lstSelectedMember = hdnSelectedMember.Value.Split('|');
            lstQtyPurchaseRequest = hdnPurchaseRequest.Value.Split('|');
            string orderBy = hdnSortByQuantityEND.Value == "0" ? "ItemName1 ASC" : "QuantityEND ASC";
            filterExpression += string.Format(" Order By {0}", orderBy);
            List<vItemBalance2> lstEntity = BusinessLayer.GetvItemBalance2List(filterExpression);
            List<vItemBalance2> lstTemp = new List<vItemBalance2>();

            if (!chkTanpaSupplier.Checked)
            {
                if (!String.IsNullOrEmpty(hdnBusinessPartnerID.Value))
                {
                    foreach (vItemBalance2 e in lstEntity)
                    {
                        GetItemMasterPurchase imp = BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, e.ItemID, 0).FirstOrDefault();
                        if (imp != null)
                        {
                            if (imp.BusinessPartnerID.ToString() == hdnBusinessPartnerID.Value)
                            {
                                lstTemp.Add(e);
                            }
                        }
                    }
                    lstEntity = lstTemp;
                }
            }
            else
            {
                foreach (vItemBalance2 e in lstEntity)
                {
                    GetItemMasterPurchase imp = BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, e.ItemID, 0).FirstOrDefault();
                    if (imp == null)
                    {
                        lstTemp.Add(e);
                    }
                    else
                    {
                        if (imp.BusinessPartnerID == 0)
                        {
                            lstTemp.Add(e);
                        }
                    }
                }
                lstEntity = lstTemp;
            }

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
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            List<string> paramID = new List<string>();
            List<string> paramPurchaseRequest = new List<string>();
            List<string> paramRecommendQty = new List<string>();
            List<string> paramSupplierID = new List<string>();

            int purchaseRequestID = 0;

            IDbContext ctx = DbFactory.Configure(true);
            PurchaseRequestDtDao entityPurchaseRequestDtDao = new PurchaseRequestDtDao(ctx);
            PurchaseRequestHdDao entityPurchaseRequestHdDao = new PurchaseRequestHdDao(ctx);
            try
            {
                if (type == "processall")
                {
                    List<vItemBalance2> lstEntity = BusinessLayer.GetvItemBalance2List(GetFilterExpression());
                    string lstBalanceID = string.Empty;
                    string lstPurchase = string.Empty;
                    string lstRecommendQty = string.Empty;

                    foreach (vItemBalance2 entity in lstEntity)
                    {
                        Decimal autoQty = 0;
                        if (rblROPDynamic.SelectedValue.ToLower().Equals("true"))
                        {
                            autoQty = Math.Ceiling(entity.AutoQtyDynamic);
                            if (autoQty < 0)
                            {
                                autoQty = 0;
                            }
                        }
                        else
                        {
                            //autoQty = Math.Ceiling(entity.AutoQtyStatic); -> perhitungan ini sekarang manual sesuai pilihan By Qty & By Location

                            // filter By Qty
                            if (rblByQty.SelectedValue == "min")
                            {
                                // filter By Location
                                if (rblByLocation.SelectedValue == "1")
                                {
                                    autoQty = Math.Ceiling((entity.QuantityMIN - entity.QtyOnHandAll - entity.PurchaseRequestQtyOnOrder - entity.PurchaseOrderQtyOnOrder) / entity.ConversionFactor);
                                }
                                else
                                {
                                    autoQty = Math.Ceiling((entity.QuantityMIN - entity.QuantityEND - entity.PurchaseRequestQtyOnOrder - entity.PurchaseOrderQtyOnOrder) / entity.ConversionFactor);
                                }
                            }
                            else
                            {
                                // filter By Location
                                if (rblByLocation.SelectedValue == "1")
                                {
                                    autoQty = Math.Ceiling((entity.QuantityMAX - entity.QtyOnHandAll - entity.PurchaseRequestQtyOnOrder - entity.PurchaseOrderQtyOnOrder) / entity.ConversionFactor);
                                }
                                else
                                {
                                    autoQty = Math.Ceiling((entity.QuantityMAX - entity.QuantityEND - entity.PurchaseRequestQtyOnOrder - entity.PurchaseOrderQtyOnOrder) / entity.ConversionFactor);
                                }
                            }

                            if (autoQty < 0)
                            {
                                autoQty = 0;
                            }
                        }
                        lstBalanceID += "," + entity.ID;
                        lstPurchase += "," + autoQty;
                        lstRecommendQty += "," + autoQty;
                    }

                    paramID = lstBalanceID.Substring(1).Split(',').ToList();
                    paramPurchaseRequest = lstPurchase.Substring(1).Split(',').ToList();
                    paramRecommendQty = lstRecommendQty.Substring(1).Split(',').ToList();

                    if (hdnListSupplierID.Value != null && hdnListSupplierID.Value != "")
                    {
                        paramSupplierID = hdnListSupplierID.Value.Substring(1).Split('|').ToList();
                    }
                }
                else
                {
                    paramID = hdnSelectedMember.Value.Substring(1).Split('|').ToList();
                    paramPurchaseRequest = hdnPurchaseRequest.Value.Substring(1).Split('|').ToList();
                    paramRecommendQty = hdnRecommendedQty.Value.Substring(1).Split('|').ToList();

                    if (hdnListSupplierID.Value != null && hdnListSupplierID.Value != "")
                    {
                        paramSupplierID = hdnListSupplierID.Value.Substring(1).Split('|').ToList();
                    }
                }

                SavePurchaseRequestHd(ctx, ref purchaseRequestID, ref retval);

                PurchaseRequestHd entityHd = entityPurchaseRequestHdDao.Get(purchaseRequestID);

                string lstID = "";
                foreach (String id in paramID)
                {
                    if (lstID != "")
                        lstID += ",";
                    lstID += id;
                }
                List<vItemBalanceAlternateUnit> lstEntityItemBalance = BusinessLayer.GetvItemBalanceAlternateUnitList(string.Format("ID IN ({0}) AND IsDeleted = 0", lstID), ctx);

                string lstItemID = "";
                foreach (vItemBalanceAlternateUnit entityItemBalance in lstEntityItemBalance)
                {
                    if (lstItemID != "")
                        lstItemID += ",";
                    lstItemID += entityItemBalance.ItemID.ToString();
                }

                List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, lstItemID), ctx);
                for (int ct = 0; ct < paramID.Count; ct++)
                {
                    vItemBalanceAlternateUnit entityItemBalance = lstEntityItemBalance.FirstOrDefault(p => p.ID == Convert.ToInt32(paramID[ct]));

                    PurchaseRequestDt entityPurchaseReqDt = new PurchaseRequestDt();

                    entityPurchaseReqDt.PurchaseRequestID = purchaseRequestID;
                    entityPurchaseReqDt.ItemID = entityItemBalance.ItemID;
                    entityPurchaseReqDt.Quantity = Convert.ToDecimal(paramPurchaseRequest[ct]);
                    entityPurchaseReqDt.QtyENDLocation = entityItemBalance.QuantityEND;
                    entityPurchaseReqDt.GCItemUnitQtyENDLocation = entityItemBalance.GCItemUnit;
                    entityPurchaseReqDt.RecommendedQty = Convert.ToDecimal(paramRecommendQty[ct]);
                    entityPurchaseReqDt.GCBaseUnit = entityItemBalance.GCItemUnit;

                    if (hdnAutoApprovePR.Value == "1")
                    {
                        entityPurchaseReqDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                    }
                    else
                    {
                        entityPurchaseReqDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                    }

                    if (paramSupplierID != null && paramSupplierID.Count != 0)
                    {
                        entityPurchaseReqDt.BusinessPartnerID = Convert.ToInt32(paramSupplierID[ct]);

                        if (hdnIM0131.Value == "0")
                        {
                            List<GetItemMasterPurchase> impList = BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, entityItemBalance.ItemID, Convert.ToInt32(entityPurchaseReqDt.BusinessPartnerID), ctx);
                            if (impList.Count > 0)
                            {
                                GetItemMasterPurchase imp = impList.FirstOrDefault();

                                if (imp.ConversionFactor == 1)
                                {
                                    entityPurchaseReqDt.UnitPrice = imp.Price;
                                }
                                else
                                {
                                    entityPurchaseReqDt.UnitPrice = imp.UnitPrice;
                                }
                                entityPurchaseReqDt.GCPurchaseUnit = imp.PurchaseUnit;
                                entityPurchaseReqDt.ConversionFactor = imp.ConversionFactor;
                                entityPurchaseReqDt.DiscountPercentage = imp.Discount;
                                entityPurchaseReqDt.DiscountPercentage2 = imp.Discount2;
                            }
                            else
                            {
                                entityPurchaseReqDt.BusinessPartnerID = null;
                                entityPurchaseReqDt.UnitPrice = 0;
                                entityPurchaseReqDt.DiscountPercentage = 0;
                                entityPurchaseReqDt.DiscountPercentage2 = 0;
                                entityPurchaseReqDt.GCPurchaseUnit = entityItemBalance.GCItemUnit;
                                entityPurchaseReqDt.ConversionFactor = 1;
                            }
                        }
                        else
                        {
                            List<GetItemMasterPurchaseWithDate> impList = BusinessLayer.GetItemMasterPurchaseWithDateList(AppSession.UserLogin.HealthcareID, entityItemBalance.ItemID, Convert.ToInt32(entityPurchaseReqDt.BusinessPartnerID), entityHd.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT_112), ctx);
                            if (impList.Count > 0)
                            {
                                GetItemMasterPurchaseWithDate imp = impList.FirstOrDefault();

                                if (imp.ConversionFactor == 1)
                                {
                                    entityPurchaseReqDt.UnitPrice = imp.Price;
                                }
                                else
                                {
                                    entityPurchaseReqDt.UnitPrice = imp.UnitPrice;
                                }
                                entityPurchaseReqDt.GCPurchaseUnit = imp.PurchaseUnit;
                                entityPurchaseReqDt.ConversionFactor = imp.ConversionFactor;
                                entityPurchaseReqDt.DiscountPercentage = imp.Discount;
                                entityPurchaseReqDt.DiscountPercentage2 = imp.Discount2;
                            }
                            else
                            {
                                entityPurchaseReqDt.BusinessPartnerID = null;
                                entityPurchaseReqDt.UnitPrice = 0;
                                entityPurchaseReqDt.DiscountPercentage = 0;
                                entityPurchaseReqDt.DiscountPercentage2 = 0;
                                entityPurchaseReqDt.GCPurchaseUnit = entityItemBalance.GCItemUnit;
                                entityPurchaseReqDt.ConversionFactor = 1;
                            }
                        }
                    }
                    else
                    {
                        if (hdnIM0131.Value == "0")
                        {
                            List<GetItemMasterPurchase> impList = BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, entityItemBalance.ItemID, 0, ctx);
                            if (impList.Count > 0)
                            {
                                GetItemMasterPurchase imp = impList.FirstOrDefault();
                                entityPurchaseReqDt.BusinessPartnerID = imp.BusinessPartnerID;

                                if (imp.ConversionFactor == 1)
                                {
                                    entityPurchaseReqDt.UnitPrice = imp.Price;
                                }
                                else
                                {
                                    entityPurchaseReqDt.UnitPrice = imp.UnitPrice;
                                }
                                entityPurchaseReqDt.GCPurchaseUnit = imp.PurchaseUnit;
                                entityPurchaseReqDt.ConversionFactor = imp.ConversionFactor;
                                entityPurchaseReqDt.DiscountPercentage = imp.Discount;
                                entityPurchaseReqDt.DiscountPercentage2 = imp.Discount2;
                            }
                            else
                            {
                                entityPurchaseReqDt.BusinessPartnerID = null;
                                entityPurchaseReqDt.UnitPrice = 0;
                                entityPurchaseReqDt.DiscountPercentage = 0;
                                entityPurchaseReqDt.DiscountPercentage2 = 0;
                                entityPurchaseReqDt.GCPurchaseUnit = entityItemBalance.GCItemUnit;
                                entityPurchaseReqDt.ConversionFactor = 1;
                            }
                        }
                        else
                        {
                            List<GetItemMasterPurchaseWithDate> impList = BusinessLayer.GetItemMasterPurchaseWithDateList(AppSession.UserLogin.HealthcareID, entityItemBalance.ItemID, 0, entityHd.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT_112), ctx);
                            if (impList.Count > 0)
                            {
                                GetItemMasterPurchaseWithDate imp = impList.FirstOrDefault();
                                entityPurchaseReqDt.BusinessPartnerID = imp.BusinessPartnerID;

                                if (imp.ConversionFactor == 1)
                                {
                                    entityPurchaseReqDt.UnitPrice = imp.Price;
                                }
                                else
                                {
                                    entityPurchaseReqDt.UnitPrice = imp.UnitPrice;
                                }
                                entityPurchaseReqDt.GCPurchaseUnit = imp.PurchaseUnit;
                                entityPurchaseReqDt.ConversionFactor = imp.ConversionFactor;
                                entityPurchaseReqDt.DiscountPercentage = imp.Discount;
                                entityPurchaseReqDt.DiscountPercentage2 = imp.Discount2;
                            }
                            else
                            {
                                entityPurchaseReqDt.BusinessPartnerID = null;
                                entityPurchaseReqDt.UnitPrice = 0;
                                entityPurchaseReqDt.DiscountPercentage = 0;
                                entityPurchaseReqDt.DiscountPercentage2 = 0;
                                entityPurchaseReqDt.GCPurchaseUnit = entityItemBalance.GCItemUnit;
                                entityPurchaseReqDt.ConversionFactor = 1;
                            }
                        }
                    }

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityPurchaseReqDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityPurchaseRequestDtDao.Insert(entityPurchaseReqDt);
                }

                ctx.CommitTransaction();
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