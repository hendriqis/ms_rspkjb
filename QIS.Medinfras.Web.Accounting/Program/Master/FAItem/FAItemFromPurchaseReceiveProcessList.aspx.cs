using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAItemFromPurchaseReceiveProcessList : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.FA_ITEM_FROM_PURCHASE_RECEIVE_PROCESS_LIST;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            txtReceivedDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReceivedDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                AppSession.UserLogin.HealthcareID, //0
                                                Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_FA_ITEM, //1
                                                Constant.SettingParameter.IS_PPN_APPLIED_TO_FA_ITEM //2
                                            ));

            hdnIsDiscountAppliedToFAItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_FA_ITEM).ParameterValue;
            hdnIsPPNAppliedToFAItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_FA_ITEM).ParameterValue;
            
            BindGridView();
        }

        private void BindGridView()
        {
            string oReceivedDate = string.Format("{0}|{1}",
                                        Helper.GetDatePickerValue(txtReceivedDateFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112),
                                        Helper.GetDatePickerValue(txtReceivedDateTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
            int oBusinessPartnerID = (hdnSupplierID.Value != "" && hdnSupplierID.Value != "0") ? Convert.ToInt32(hdnSupplierID.Value) : 0;
            int oItemID = (hdnItemID.Value != "" && hdnItemID.Value != "0") ? Convert.ToInt32(hdnItemID.Value) : 0;
            int oProductLineID = (hdnProductLineID.Value != "" && hdnProductLineID.Value != "0") ? Convert.ToInt32(hdnProductLineID.Value) : 0;

            List<GetPurchaseReceiveDtFixedAsset> lstEntity = BusinessLayer.GetPurchaseReceiveDtFixedAssetList(oReceivedDate, oBusinessPartnerID, oItemID, oProductLineID);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                GetPurchaseReceiveDtFixedAsset entity = e.Item.DataItem as GetPurchaseReceiveDtFixedAsset;

                CheckBox chkIsSelected = e.Item.FindControl("chkIsSelected") as CheckBox;
                chkIsSelected.Checked = false;

                TextBox txtDepreciationStartDate = e.Item.FindControl("txtDepreciationStartDate") as TextBox;
                txtDepreciationStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnCustomButtonClick(string type, ref string retval, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAItemDao entityDao = new FAItemDao(ctx);
            FAItemCOADao entityCOADao = new FAItemCOADao(ctx);
            DirectPurchaseHdDao entityDPDao = new DirectPurchaseHdDao(ctx);
            PurchaseReceiveHdDao entityPORDao = new PurchaseReceiveHdDao(ctx);

            try
            {
                List<String> lstSelectedRowNumber = hdnSelectedRowNumber.Value.Split(',').ToList();
                List<String> lstSelectedID = hdnSelectedID.Value.Split(',').ToList();
                List<String> lstSelectedParentID = hdnSelectedParentID.Value.Split(',').ToList();
                List<String> lstSelectedSerialNumber = hdnSelectedSerialNumber.Value.Split(',').ToList();
                List<String> lstSelectedFAGroupID = hdnSelectedFAGroupID.Value.Split(',').ToList();
                List<String> lstSelectedFALocationID = hdnSelectedFALocationID.Value.Split(',').ToList();
                List<String> lstSelectedGCBudgetCategory = hdnSelectedGCBudgetCategory.Value.Split(',').ToList();
                List<String> lstSelectedBudgetPlanNo = hdnSelectedBudgetPlanNo.Value.Split(',').ToList();
                List<String> lstSelectedMethodID = hdnSelectedMethodID.Value.Split(',').ToList();
                List<String> lstSelectedDepreciationLength = hdnSelectedDepreciationLength.Value.Split(',').ToList();
                List<String> lstSelectedDepreciationStartDate = hdnSelectedDepreciationStartDate.Value.Split(',').ToList();
                List<String> lstSelectedAssetFinalValue = hdnSelectedAssetFinalValue.Value.Split(',').ToList();

                lstSelectedRowNumber.RemoveAt(0);
                lstSelectedID.RemoveAt(0);
                lstSelectedParentID.RemoveAt(0);
                lstSelectedSerialNumber.RemoveAt(0);
                lstSelectedFAGroupID.RemoveAt(0);
                lstSelectedFALocationID.RemoveAt(0);
                lstSelectedGCBudgetCategory.RemoveAt(0);
                lstSelectedBudgetPlanNo.RemoveAt(0);
                lstSelectedMethodID.RemoveAt(0);
                lstSelectedDepreciationLength.RemoveAt(0);
                lstSelectedDepreciationStartDate.RemoveAt(0);
                lstSelectedAssetFinalValue.RemoveAt(0);

                if (type == "process")
                {
                    for (int i = 0; i < lstSelectedRowNumber.Count(); i++)
                    {
                        string[] selectedID = lstSelectedID[i].Split('|');

                        if (selectedID[0] == Constant.TransactionCode.DIRECT_PURCHASE)
                        {
                            #region DIRECT PURCHASE

                            List<vDirectPurchaseDt> listDp = BusinessLayer.GetvDirectPurchaseDtList(string.Format("ID = {0}", selectedID[1]), ctx);
                            foreach (vDirectPurchaseDt dp in listDp)
                            {
                                DirectPurchaseHd entityHd = entityDPDao.Get(dp.DirectPurchaseID);

                                FAItem entity = new FAItem();
                                FAItemCOA entityCOA = new FAItemCOA();

                                entity.HealthcareID = entityCOA.HealthcareID = AppSession.UserLogin.HealthcareID;

                                #region Data Aktiva Tetap
                                entity.FixedAssetName = dp.ItemName1;
                                entity.ItemID = dp.ItemID;
                                entity.SerialNumber = lstSelectedSerialNumber[i];
                                entity.FAGroupID = Convert.ToInt32(lstSelectedFAGroupID[i]);
                                ////entity.FALocationID = dp.LocationID; //ditutup per patch 202212-04 issue rsdosoba202212210000024
                                entity.FALocationID = Convert.ToInt32(lstSelectedFALocationID[i]);

                                if (lstSelectedParentID[i] != "0" && lstSelectedParentID[i] != "")
                                {
                                    entity.ParentID = Convert.ToInt32(lstSelectedParentID[i]);
                                }
                                else
                                {
                                    entity.ParentID = null;
                                }

                                if (lstSelectedGCBudgetCategory[i] != "" && lstSelectedGCBudgetCategory[i] != null)
                                {
                                    entity.GCBudgetCategory = lstSelectedGCBudgetCategory[i];
                                }
                                else
                                {
                                    entity.GCBudgetCategory = null;
                                }

                                if (lstSelectedBudgetPlanNo[i] != "" && lstSelectedBudgetPlanNo[i] != null)
                                {
                                    entity.BudgetPlanNo = lstSelectedBudgetPlanNo[i];
                                }
                                else
                                {
                                    entity.BudgetPlanNo = null;
                                }

                                entity.Sequence = null;
                                entity.Remarks = null;
                                entity.ContractNumber = null;
                                entity.BusinessPartnerID = dp.BusinessPartnerID;
                                entity.BusinessPartnerName = "";

                                #endregion

                                #region Data Perolehan Aktiva Tetap
                                entity.NoPPB = null;
                                entity.TransactionCode = Constant.TransactionCode.DIRECT_PURCHASE;
                                entity.PurchaseReceiveID = dp.DirectPurchaseID;
                                entity.ProcurementNumber = dp.DirectPurchaseNo;
                                entity.ProcurementDate = dp.PurchaseDate;
                                entity.GuaranteePeriod = 0;

                                decimal unitPrice = dp.UnitPrice;
                                decimal discountAmount1, discountAmount2;

                                if (dp.IsDiscountInPercentage)
                                {
                                    discountAmount1 = (unitPrice * dp.DiscountPercentage) / 100;
                                }
                                else
                                {
                                    discountAmount1 = dp.DiscountAmount;
                                }


                                if (dp.IsDiscountInPercentage2)
                                {
                                    discountAmount2 = ((unitPrice - discountAmount1) * dp.DiscountPercentage2) / 100;
                                }
                                else
                                {
                                    discountAmount2 = dp.DiscountAmount2;
                                }

                                if (hdnIsDiscountAppliedToFAItem.Value == "1")
                                {
                                    unitPrice = unitPrice - (discountAmount1 + discountAmount2);
                                }

                                if (hdnIsPPNAppliedToFAItem.Value == "1")
                                {
                                    decimal ppnAmountUnitPrice = unitPrice * (Convert.ToDecimal(dp.VATPercentage) / Convert.ToDecimal(100));
                                    unitPrice = unitPrice + ppnAmountUnitPrice;
                                }

                                Decimal total = unitPrice;

                                entity.ProcurementAmount = Math.Round(total, 2);
                                entity.ProcurementQuantity = (dp.Quantity / dp.Quantity);
                                entity.GCProcurementUnit = dp.GCItemUnit;

                                entity.GuaranteeStartDate = DateTime.Now;
                                entity.GuaranteeEndDate = DateTime.Now;
                                #endregion

                                #region Data Perhitungan Penyusutan Aktiva Tetap
                                entity.IsUsedDepreciation = true;
                                entity.MethodID = Convert.ToInt32(lstSelectedMethodID[i]);
                                entity.DepreciationStartDate = Helper.GetDatePickerValue(lstSelectedDepreciationStartDate[i]);
                                entity.DepreciationLength = Convert.ToInt16(lstSelectedDepreciationLength[i]);
                                entity.AssetFinalValue = Convert.ToDecimal(lstSelectedAssetFinalValue[i]);
                                #endregion

                                #region Pengaturan Perkiraan untuk Aktiva Tetap

                                string filterCOA = string.Format("FAGroupID = {0}", entity.FAGroupID);
                                List<vFAGroupCOA> coaLst = BusinessLayer.GetvFAGroupCOAList(filterCOA, ctx);
                                if (coaLst.Count() > 0)
                                {
                                    vFAGroupCOA coa = coaLst.FirstOrDefault();

                                    entityCOA.GLAccount1 = coa.GLAccount1;
                                    entityCOA.GLAccount2 = coa.GLAccount2;
                                    entityCOA.GLAccount3 = coa.GLAccount3;
                                    entityCOA.GLAccount4 = coa.GLAccount4;
                                    entityCOA.GLAccount5 = coa.GLAccount5;
                                    entityCOA.GLAccount6 = coa.GLAccount6;
                                }
                                else
                                {
                                    entityCOA.GLAccount1 = null;
                                    entityCOA.GLAccount2 = null;
                                    entityCOA.GLAccount3 = null;
                                    entityCOA.GLAccount4 = null;
                                    entityCOA.GLAccount5 = null;
                                    entityCOA.GLAccount6 = null;
                                }

                                entityCOA.SubLedger1 = null;
                                entityCOA.SubLedger2 = null;
                                entityCOA.SubLedger3 = null;
                                entityCOA.SubLedger4 = null;
                                entityCOA.SubLedger5 = null;
                                entityCOA.SubLedger6 = null;

                                #endregion

                                if (entity.ParentID != null && entity.ParentID != 0)
                                {
                                    entity.FixedAssetCode = GenerateChildFixedAssetCode(ctx, Convert.ToInt32(entity.ParentID));
                                }
                                else
                                {
                                    entity.FixedAssetCode = GenerateFixedAssetCode(ctx, entity.FALocationID, entity.FAGroupID, entity.GCBudgetCategory, entity.ProcurementDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
                                }

                                entity.GCItemStatus = Constant.ItemStatus.ACTIVE;
                                entity.CreatedBy = AppSession.UserLogin.UserID;
                                int oFixedAssetID = entityDao.InsertReturnPrimaryKeyID(entity);

                                entityCOA.CreatedBy = AppSession.UserLogin.UserID;
                                entityCOA.FixedAssetID = oFixedAssetID;
                                entityCOADao.Insert(entityCOA);

                                if (!string.IsNullOrEmpty(hdnNewFixedAssetCode.Value))
                                {
                                    hdnNewFixedAssetCode.Value += ", ";
                                }
                                hdnNewFixedAssetCode.Value += entity.FixedAssetCode;
                                retval = hdnNewFixedAssetCode.Value;
                            }

                            #endregion
                        }
                        else if (selectedID[0] == Constant.TransactionCode.PURCHASE_RECEIVE)
                        {
                            #region PURCHASE RECEIVE

                            List<vPurchaseReceiveDt> listPOR = BusinessLayer.GetvPurchaseReceiveDtList(string.Format("ID = {0}", selectedID[1]), ctx);
                            foreach (vPurchaseReceiveDt por in listPOR)
                            {
                                PurchaseReceiveHd entityHd = entityPORDao.Get(por.PurchaseReceiveID);

                                FAItem entity = new FAItem();
                                FAItemCOA entityCOA = new FAItemCOA();

                                entity.HealthcareID = entityCOA.HealthcareID = AppSession.UserLogin.HealthcareID;

                                #region Data Aktiva Tetap
                                entity.FixedAssetName = por.ItemName1;
                                entity.ItemID = por.ItemID;
                                entity.SerialNumber = lstSelectedSerialNumber[i];
                                entity.FAGroupID = Convert.ToInt32(lstSelectedFAGroupID[i]);
                                ////entity.FALocationID = por.LocationID; //ditutup per patch 202212-04 issue rsdosoba202212210000024
                                entity.FALocationID = Convert.ToInt32(lstSelectedFALocationID[i]);

                                if (lstSelectedParentID[i] != "0" && lstSelectedParentID[i] != "")
                                {
                                    entity.ParentID = Convert.ToInt32(lstSelectedParentID[i]);
                                }
                                else
                                {
                                    entity.ParentID = null;
                                }

                                if (lstSelectedGCBudgetCategory[i] != "" && lstSelectedGCBudgetCategory[i] != null)
                                {
                                    entity.GCBudgetCategory = lstSelectedGCBudgetCategory[i];
                                }
                                else
                                {
                                    entity.GCBudgetCategory = por.GCBudgetCategory;
                                }

                                if (lstSelectedBudgetPlanNo[i] != "" && lstSelectedBudgetPlanNo[i] != null)
                                {
                                    entity.BudgetPlanNo = lstSelectedBudgetPlanNo[i];
                                }
                                else
                                {
                                    entity.BudgetPlanNo = por.OtherReferenceNo;
                                }

                                entity.Sequence = null;
                                entity.Remarks = null;
                                entity.ContractNumber = null;
                                entity.BusinessPartnerID = por.SupplierID;
                                entity.BusinessPartnerName = "";

                                #endregion

                                #region Data Perolehan Aktiva Tetap
                                if (por.OtherRequestReferenceNo != null && por.OtherRequestReferenceNo != "")
                                {
                                    entity.NoPPB = por.OtherRequestReferenceNo;
                                }
                                else
                                {
                                    entity.NoPPB = por.PurchaseOrderNo;
                                }
                                entity.TransactionCode = Constant.TransactionCode.PURCHASE_RECEIVE;
                                entity.PurchaseReceiveID = por.PurchaseReceiveID;
                                entity.ProcurementNumber = por.PurchaseReceiveNo;
                                entity.ProcurementDate = por.ReceivedDate;
                                entity.GuaranteePeriod = 0;

                                decimal unitPrice = por.UnitPrice;
                                decimal discountAmount1, discountAmount2;

                                if (por.IsDiscountInPercentage1)
                                {
                                    discountAmount1 = (unitPrice * por.DiscountPercentage1) / 100;
                                }
                                else
                                {
                                    discountAmount1 = por.DiscountAmount1;
                                }


                                if (por.IsDiscountInPercentage2)
                                {
                                    discountAmount2 = ((unitPrice - discountAmount1) * por.DiscountPercentage2) / 100;
                                }
                                else
                                {
                                    discountAmount2 = por.DiscountAmount2;
                                }

                                if (hdnIsDiscountAppliedToFAItem.Value == "1")
                                {
                                    unitPrice = unitPrice - (discountAmount1 + discountAmount2);
                                }

                                if (hdnIsPPNAppliedToFAItem.Value == "1")
                                {
                                    decimal ppnAmountUnitPrice = unitPrice * (Convert.ToDecimal(entityHd.VATPercentage) / Convert.ToDecimal(100));
                                    unitPrice = unitPrice + ppnAmountUnitPrice;
                                }

                                Decimal PPHPercent = entityHd.IsPPHInPercentage ? entityHd.PPHPercentage : 0;

                                Decimal total = (unitPrice * (100 + PPHPercent)) / 100;

                                string filterDt = string.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", por.PurchaseReceiveID, Constant.TransactionStatus.VOID);
                                List<PurchaseReceiveDt> lstPORDT = BusinessLayer.GetPurchaseReceiveDtList(filterDt, ctx);
                                Decimal countQtyDt = lstPORDT.Sum(a => a.Quantity);
                                Decimal ongkir = (entityHd.ChargesAmount / countQtyDt);

                                total = total + ongkir;

                                entity.ProcurementAmount = Math.Round(total, 2);
                                entity.ProcurementQuantity = (por.Quantity / por.Quantity);
                                entity.GCProcurementUnit = por.GCItemUnit;

                                entity.GuaranteeStartDate = DateTime.Now;
                                entity.GuaranteeEndDate = DateTime.Now;
                                #endregion

                                #region Data Perhitungan Penyusutan Aktiva Tetap
                                entity.IsUsedDepreciation = true;
                                entity.MethodID = Convert.ToInt32(lstSelectedMethodID[i]);
                                entity.DepreciationStartDate = Helper.GetDatePickerValue(lstSelectedDepreciationStartDate[i]);
                                entity.DepreciationLength = Convert.ToInt16(lstSelectedDepreciationLength[i]);
                                entity.AssetFinalValue = Convert.ToDecimal(lstSelectedAssetFinalValue[i]);
                                #endregion

                                #region Pengaturan Perkiraan untuk Aktiva Tetap

                                string filterCOA = string.Format("FAGroupID = {0}", entity.FAGroupID);
                                List<vFAGroupCOA> coaLst = BusinessLayer.GetvFAGroupCOAList(filterCOA, ctx);
                                if (coaLst.Count() > 0)
                                {
                                    vFAGroupCOA coa = coaLst.FirstOrDefault();

                                    entityCOA.GLAccount1 = coa.GLAccount1;
                                    entityCOA.GLAccount2 = coa.GLAccount2;
                                    entityCOA.GLAccount3 = coa.GLAccount3;
                                    entityCOA.GLAccount4 = coa.GLAccount4;
                                    entityCOA.GLAccount5 = coa.GLAccount5;
                                    entityCOA.GLAccount6 = coa.GLAccount6;
                                }
                                else
                                {
                                    entityCOA.GLAccount1 = null;
                                    entityCOA.GLAccount2 = null;
                                    entityCOA.GLAccount3 = null;
                                    entityCOA.GLAccount4 = null;
                                    entityCOA.GLAccount5 = null;
                                    entityCOA.GLAccount6 = null;
                                }

                                entityCOA.SubLedger1 = null;
                                entityCOA.SubLedger2 = null;
                                entityCOA.SubLedger3 = null;
                                entityCOA.SubLedger4 = null;
                                entityCOA.SubLedger5 = null;
                                entityCOA.SubLedger6 = null;

                                #endregion

                                if (entity.ParentID != null && entity.ParentID != 0)
                                {
                                    entity.FixedAssetCode = GenerateChildFixedAssetCode(ctx, Convert.ToInt32(entity.ParentID));
                                }
                                else
                                {
                                    entity.FixedAssetCode = GenerateFixedAssetCode(ctx, entity.FALocationID, entity.FAGroupID, entity.GCBudgetCategory, entity.ProcurementDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
                                }

                                entity.GCItemStatus = Constant.ItemStatus.ACTIVE;
                                entity.CreatedBy = AppSession.UserLogin.UserID;
                                int oFixedAssetID = entityDao.InsertReturnPrimaryKeyID(entity);

                                entityCOA.CreatedBy = AppSession.UserLogin.UserID;
                                entityCOA.FixedAssetID = oFixedAssetID;
                                entityCOADao.Insert(entityCOA);

                                if (!string.IsNullOrEmpty(hdnNewFixedAssetCode.Value))
                                {
                                    hdnNewFixedAssetCode.Value += ", ";
                                }
                                hdnNewFixedAssetCode.Value += entity.FixedAssetCode;
                                retval = hdnNewFixedAssetCode.Value;
                            }

                            #endregion
                        }
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

        private String GenerateFixedAssetCode(IDbContext ctx, int oLocationID, int oFAGroupID, string oGCBudgetCategory, string oProcurementDate)
        {
            HealthcareDao oHealthcareDao = new HealthcareDao(ctx);
            FAGroupDao oFAGroupDao = new FAGroupDao(ctx);
            FALocationDao oFALocationDao = new FALocationDao(ctx);
            StandardCodeDao oStandardCodeDao = new StandardCodeDao(ctx);
            SettingParameterDtDao oSettingParameterDtDao = new SettingParameterDtDao(ctx);

            StringBuilder result = new StringBuilder();

            #region Get Data

            string delimiter = "/";

            string initial = oHealthcareDao.Get(AppSession.UserLogin.HealthcareID).Initial;

            string prefix = oSettingParameterDtDao.Get(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.AC_PREFIX_ASSET_CODE).ParameterValue;

            string location = oFALocationDao.Get(oLocationID).FALocationCode;

            string faGroup = oFAGroupDao.Get(oFAGroupID).FAGroupCode;

            string budgetCategory = oStandardCodeDao.Get(oGCBudgetCategory).TagProperty;

            string procurementDate = Helper.GetDatePickerValue(oProcurementDate).ToString("MMyy");
            if (initial == "RSMD")
            {
                procurementDate = Helper.GetDatePickerValue(oProcurementDate).ToString("yyMM");
            }

            #endregion

            if (initial == "RSSBB")
            {
                delimiter = "";
                procurementDate = Helper.GetDatePickerValue(oProcurementDate).ToString("yy");
                result = result.Append(prefix).Append(delimiter).Append(location).Append(delimiter).Append(faGroup).Append(delimiter).Append(budgetCategory).Append(delimiter).Append(procurementDate).Append(delimiter);
            }
            else if (initial == "RSCK")
            {
                string txtLocation = string.Format("{0}/", location);
                result = result.Append(prefix).Append(delimiter).Append(txtLocation).Append(faGroup).Append(delimiter).Append(budgetCategory).Append(delimiter).Append(procurementDate).Append(delimiter);
            }
            else
            {
                result = result.Append(prefix).Append(delimiter).Append(faGroup).Append(delimiter).Append(procurementDate).Append(delimiter);
            }

            FAItem fai = BusinessLayer.GetFAItemList(string.Format("FixedAssetCode LIKE '{0}%' AND ParentID IS NULL", result.ToString()), 1, 1, "FixedAssetCode DESC", ctx).FirstOrDefault();
            int newNumber = 1;
            if (fai != null)
            {
                newNumber = Convert.ToInt32(fai.FixedAssetCode.Substring(result.ToString().Length)) + 1;
            }

            return result.Append(newNumber.ToString().PadLeft(6, '0')).ToString();
        }

        private String GenerateChildFixedAssetCode(IDbContext ctx, int oParentID)
        {
            HealthcareDao oHealthcareDao = new HealthcareDao(ctx);
            FAItemDao oFAItemDao = new FAItemDao(ctx);
            FALocationDao oFALocationDao = new FALocationDao(ctx);
            StandardCodeDao oStandardCodeDao = new StandardCodeDao(ctx);

            StringBuilder result = new StringBuilder();

            FAItem pfai = BusinessLayer.GetFAItemList(string.Format("FixedAssetID = {0} AND ParentID IS NULL", oParentID), 1, 1, "FixedAssetCode DESC", ctx).FirstOrDefault();

            string parentCode = pfai.FixedAssetCode;

            int newNumber = 1;

            List<FAItem> lstFAItem = BusinessLayer.GetFAItemList(string.Format("ParentID = {0}", oParentID), 1, 1, "FixedAssetCode DESC", ctx);
            if (lstFAItem.Count > 0)
            {
                FAItem fai = oFAItemDao.Get(lstFAItem.FirstOrDefault().FixedAssetID);
                parentCode = fai.FixedAssetCode.Substring(0, (fai.FixedAssetCode.Length - 4));
                int numberParentCode = Convert.ToInt32(fai.FixedAssetCode.Substring((fai.FixedAssetCode.Length - 3), 3));
                newNumber = numberParentCode + 1;
            }

            return string.Format("{0}/{1}", parentCode, newNumber.ToString().PadLeft(3, '0'));
        }

    }
}