using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class EditPatientChargesItemCtl1 : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            this.PopupTitle = "Edit Charges Item";

            SetControlProperties();

            if (!string.IsNullOrEmpty(param))
            {
                List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                                                                        AppSession.UserLogin.HealthcareID, //0
                                                                        Constant.SettingParameter.FN_DISKON_DOKTER_KOMPONEN_2, //1
                                                                        Constant.SettingParameter.EMR_ALLOW_CHANGE_CHARGES_QTY, //2
                                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1, //3
                                                                        Constant.SettingParameter.EM_IS_DOCTOR_FEE_ALLOW_PREVIEW_TARIFF //4
                                                                    ));
                hdnIsDiscountApplyToTariffComp2Only.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_DISKON_DOKTER_KOMPONEN_2).ParameterValue;
                hdnIsAllowChangeChargesQty.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EMR_ALLOW_CHANGE_CHARGES_QTY).ParameterValue;
                hdnIsEndingAmountRoundingTo1.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;
                hdnIsAllowPreviewTariffEditCtl.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.EM_IS_DOCTOR_FEE_ALLOW_PREVIEW_TARIFF).FirstOrDefault().ParameterValue;

                if (hdnIsDiscountApplyToTariffComp2Only.Value == "1")
                {
                    chkIsDiscount1.Enabled = false;
                }
                else
                {
                    chkIsDiscount1.Enabled = true;
                }

                if (hdnIsAllowPreviewTariffEditCtl.Value == "0")
                {
                    chkIsVariable.Attributes.Add("style", "display:none");

                    tdTariffLabel.Attributes.Add("style", "display:none");
                    tdTariffComp1Label.Attributes.Add("style", "display:none");
                    tdTariffComp2Label.Attributes.Add("style", "display:none");
                    tdPatientTotalLabel.Attributes.Add("style", "display:none");
                    tdPayerTotalLabel.Attributes.Add("style", "display:none");
                    tdLineAmountLabel.Attributes.Add("style", "display:none");

                    tdTariff.Attributes.Add("style", "display:none");
                    tdTariffComp1.Attributes.Add("style", "display:none");
                    tdTariffComp2.Attributes.Add("style", "display:none");
                    tdCITOAmount.Attributes.Add("style", "display:none");
                    tdDiscountAmountComp1.Attributes.Add("style", "display:none");
                    tdDiscountAmountComp2.Attributes.Add("style", "display:none");
                    tdComplicationAmount.Attributes.Add("style", "display:none");
                    tdPatientTotal.Attributes.Add("style", "display:none");
                    tdPayerTotal.Attributes.Add("style", "display:none");
                    tdLineAmount.Attributes.Add("style", "display:none");
                }
                else
                {
                    chkIsVariable.Attributes.Remove("style");

                    tdTariffLabel.Attributes.Remove("style");
                    tdTariffComp1Label.Attributes.Remove("style");
                    tdTariffComp2Label.Attributes.Remove("style");
                    tdPatientTotalLabel.Attributes.Remove("style");
                    tdPayerTotalLabel.Attributes.Remove("style");
                    tdLineAmountLabel.Attributes.Remove("style");

                    tdTariff.Attributes.Remove("style");
                    tdTariffComp1.Attributes.Remove("style");
                    tdTariffComp2.Attributes.Remove("style");
                    tdCITOAmount.Attributes.Remove("style");
                    tdDiscountAmountComp1.Attributes.Remove("style");
                    tdDiscountAmountComp2.Attributes.Remove("style");
                    tdComplicationAmount.Attributes.Remove("style");
                    tdPatientTotal.Attributes.Remove("style");
                    tdPayerTotal.Attributes.Remove("style");
                    tdLineAmount.Attributes.Remove("style");
                }

                string[] paramInfo = param.Split('|');
                hdnTransactionID.Value = paramInfo[0];
                hdnSelectedID.Value = paramInfo[1];
                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                int detailID = Convert.ToInt32(hdnSelectedID.Value);
                string filterExp = string.Format("TransactionID = {0}", transactionID);
                PatientChargesHd chargesHd = BusinessLayer.GetPatientChargesHdList(filterExp).FirstOrDefault();
                if (chargesHd != null)
                {
                    vPatientChargesDt1 chargesDt = BusinessLayer.GetvPatientChargesDt1List(string.Format("ID = {0}", detailID)).FirstOrDefault();
                    if (chargesDt != null)
                    {
                        //Get Current Item Tariff based on Transaction Date
                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(AppSession.RegisteredPatient.RegistrationID), AppSession.RegisteredPatient.VisitID, chargesDt.ChargeClassID, chargesDt.ItemID, 1, chargesHd.TransactionDate);
                        decimal discountAmount = 0;
                        decimal coverageAmount = 0;
                        decimal price = 0;
                        decimal basePrice = 0;
                        bool isCoverageInPercentage = false;
                        bool isDiscountInPercentage = false;
                        decimal baseComp1 = 0;
                        decimal baseComp2 = 0;
                        decimal baseComp3 = 0;
                        decimal tariffComp1 = 0;
                        decimal tariffComp2 = 0;
                        decimal tariffComp3 = 0;
                        if (list.Count > 0)
                        {
                            GetCurrentItemTariff obj = list[0];
                            discountAmount = obj.DiscountAmount;
                            coverageAmount = obj.CoverageAmount;
                            basePrice = obj.BasePrice;
                            baseComp1 = obj.BasePriceComp1;
                            baseComp2 = obj.BasePriceComp2;
                            baseComp3 = obj.BasePriceComp3;
                            price = obj.Price;
                            tariffComp1 = obj.PriceComp1;
                            tariffComp2 = obj.PriceComp2;
                            tariffComp3 = obj.PriceComp3;

                            isCoverageInPercentage = obj.IsCoverageInPercentage;
                            isDiscountInPercentage = obj.IsDiscountInPercentage;

                            hdnMasterItemBaseTariff.Value = obj.BasePrice.ToString();
                            hdnMasterItemBaseTariffComp1.Value = obj.BasePriceComp1.ToString();
                            hdnMasterItemBaseTariffComp2.Value = obj.BasePriceComp2.ToString();
                            hdnMasterItemBaseTariffComp3.Value = obj.BasePriceComp3.ToString();

                            hdnMasterItemTariff.Value = obj.Price.ToString();
                            hdnMasterItemTariffComp1.Value = obj.PriceComp1.ToString();
                            hdnMasterItemTariffComp2.Value = obj.PriceComp2.ToString();
                            hdnMasterItemTariffComp3.Value = obj.PriceComp3.ToString();
                        }

                        List<vItemService> oItemLst = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", chargesDt.ItemID));
                        if (oItemLst.Count() > 0)
                        {
                            vItemService oItem = oItemLst.FirstOrDefault();

                            chkIsVariable.Enabled = oItem.IsAllowVariable;
                            chkIsDiscount.Enabled = oItem.IsAllowDiscount;
                            chkIsCITO.Enabled = oItem.IsAllowCito;
                            hdnDefaultTariffComp.Value = oItem.DefaultTariffComp.ToString();

                            hdnIsCITOInPercentageMaster.Value = oItem.IsCITOInPercentage ? "1" : "0";
                            hdnCITOAmountMaster.Value = oItem.CITOAmount.ToString();

                            if (hdnIsAllowChangeChargesQty.Value == "1")
                            {
                                if (oItem.IsQtyAllowChangeForDoctor)
                                {
                                    txtQty.ReadOnly = false;
                                }
                                else
                                {
                                    txtQty.ReadOnly = true;
                                }
                            }
                            else
                            {
                                txtQty.ReadOnly = true;
                            }
                        }
                        else
                        {
                            hdnIsCITOInPercentageMaster.Value = "0";
                            hdnCITOAmountMaster.Value = "0";
                            txtQty.ReadOnly = true;
                        }

                        hdnCoverageAmount.Value = coverageAmount.ToString();
                        hdnIsCoverageInPercentage.Value = isCoverageInPercentage ? "1" : "0";
                        hdnIsDiscountInPercentage.Value = isDiscountInPercentage ? "1" : "0";

                        hdnBaseTariff.Value = chargesDt.BaseTariff.ToString();
                        hdnBaseTariffComp1.Value = chargesDt.BaseComp1.ToString();
                        hdnBaseTariffComp2.Value = chargesDt.BaseComp2.ToString();
                        hdnBaseTariffComp3.Value = chargesDt.BaseComp3.ToString();

                        hdnItemID.Value = chargesDt.ItemID.ToString();
                        txtItemCode.Text = chargesDt.ItemCode;
                        txtItemName.Text = chargesDt.ItemName1;
                        hdnServicePhysicianID.Value = chargesDt.ParamedicID.ToString();
                        txtParamedicCode.Text = chargesDt.ParamedicCode;
                        txtParamedicName.Text = chargesDt.ParamedicName;
                        cboChargeClassID.Value = chargesDt.ChargeClassID.ToString();
                        chkIsCITO.Checked = chargesDt.IsAllowCITO;
                        chkIsVariable.Checked = chargesDt.IsVariable;
                        chkIsUnbilledItem.Checked = chargesDt.IsUnbilledItem;

                        hdnTariff.Value = chargesDt.Tariff.ToString();
                        hdnTariffComp1.Value = chargesDt.TariffComp1.ToString();
                        hdnTariffComp2.Value = chargesDt.TariffComp2.ToString();
                        hdnTariffComp3.Value = chargesDt.TariffComp3.ToString();

                        txtTariff.Text = chargesDt.Tariff.ToString(Constant.FormatString.NUMERIC_2);
                        txtTariffComp1.Text = chargesDt.TariffComp1.ToString(Constant.FormatString.NUMERIC_2);
                        txtTariffComp2.Text = chargesDt.TariffComp2.ToString(Constant.FormatString.NUMERIC_2);

                        if (chargesDt.IsVariable)
                        {
                            txtTariff.Attributes.Add("readonly", "readonly");
                            txtPatientTotal.Attributes.Add("readonly", "readonly");
                            txtPayerTotal.Attributes.Add("readonly", "readonly");
                        }
                        else
                        {
                            txtTariff.Attributes.Remove("readonly");
                            txtPatientTotal.Attributes.Remove("readonly");
                            txtPayerTotal.Attributes.Remove("readonly");
                        }

                        chkIsDiscount.Checked = chargesDt.IsDiscount;
                        if (chargesDt.IsDiscount)
                        {
                            txtDiscountPctAmountComp2.Attributes.Remove("readonly");
                            txtDiscountAmountComp2.Attributes.Remove("readonly");
                        }
                        else
                        {
                            txtDiscountPctAmountComp2.Attributes.Add("readonly", "readonly");
                            txtDiscountAmountComp2.Attributes.Add("readonly", "readonly");
                        }

                        if (chargesDt.DiscountComp1 != 0 || chargesDt.DiscountPercentageComp1 != 0)
                        {
                            chkIsDiscount1.Checked = true;
                            txtDiscountPctAmountComp1.Attributes.Remove("readonly");
                            txtDiscountAmountComp1.Attributes.Remove("readonly");
                        }
                        else
                        {
                            chkIsDiscount1.Checked = false;
                            txtDiscountPctAmountComp1.Attributes.Add("readonly", "readonly");
                            txtDiscountAmountComp1.Attributes.Add("readonly", "readonly");
                        }

                        hdnDiscountAmount.Value = chargesDt.DiscountAmount.ToString();
                        hdnDiscountAmount1.Value = chargesDt.DiscountComp1.ToString();
                        hdnDiscountAmount2.Value = chargesDt.DiscountComp2.ToString();
                        hdnDiscountAmount3.Value = chargesDt.DiscountComp3.ToString();

                        if (hdnIsDiscountApplyToTariffComp2Only.Value == "0")
                        {
                            if (chargesDt.Tariff != 0)
                            {
                                //txtDiscountPctAmountComp2.Text = ((chargesDt.DiscountAmount / chargesDt.ChargedQuantity) / chargesDt.Tariff * 100).ToString(Constant.FormatString.NUMERIC_2);
                                //txtDiscountAmountComp2.Text = (chargesDt.DiscountAmount / chargesDt.ChargedQuantity).ToString(Constant.FormatString.NUMERIC_2);

                                txtDiscountPctAmountComp2.Text = chargesDt.DiscountPercentageComp2.ToString(Constant.FormatString.NUMERIC_2);
                                txtDiscountAmountComp2.Text = chargesDt.DiscountComp2.ToString(Constant.FormatString.NUMERIC_2);
                            }
                            else
                            {
                                txtDiscountPctAmountComp2.Text = "0";
                                txtDiscountAmountComp2.Text = "0";
                            }
                        }
                        else
                        {
                            txtDiscountPctAmountComp2.Text = chargesDt.DiscountPercentageComp2.ToString(Constant.FormatString.NUMERIC_2);
                            txtDiscountAmountComp2.Text = chargesDt.DiscountComp2.ToString(Constant.FormatString.NUMERIC_2);
                        }

                        txtDiscountAmountComp1.Text = chargesDt.DiscountComp1.ToString(Constant.FormatString.NUMERIC_2);
                        txtDiscountPctAmountComp1.Text = chargesDt.DiscountPercentageComp1.ToString(Constant.FormatString.NUMERIC_2);

                        cboDiscountReason.Enabled = chargesDt.IsDiscount;
                        if (!string.IsNullOrEmpty(chargesDt.GCDiscountReason))
                        {
                            cboDiscountReason.Value = chargesDt.GCDiscountReason;
                        }

                        chkIsCITO.Checked = chargesDt.IsCITO;
                        hdnIsCITOInPercentage.Value = chargesDt.IsCITOInPercentage ? "1" : "0";
                        txtCITOAmount.Text = chargesDt.CITOAmount.ToString(Constant.FormatString.NUMERIC_2);

                        txtQty.Text = chargesDt.ChargedQuantity.ToString(Constant.FormatString.NUMERIC_2);

                        txtPatientTotal.Text = chargesDt.PatientAmount.ToString(Constant.FormatString.NUMERIC_2);
                        txtPayerTotal.Text = chargesDt.PayerAmount.ToString(Constant.FormatString.NUMERIC_2);
                        txtLineAmount.Text = chargesDt.LineAmount.ToString(Constant.FormatString.NUMERIC_2);

                    }
                }
            }
        }

        private void SetControlProperties()
        {
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0");
            Methods.SetComboBoxField(cboChargeClassID, lstClassCare, "ClassName", "ClassID");

            List<StandardCode> lstDiscountReason = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DISCOUNT_REASON));
            Methods.SetComboBoxField<StandardCode>(cboDiscountReason, lstDiscountReason, "StandardCodeName", "StandardCodeID");
            cboDiscountReason.SelectedIndex = 0;

            cboChargeClassID.Enabled = false;
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string recordID = hdnSelectedID.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            result = UpdatePatientChargesDt(recordID);

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string UpdatePatientChargesDt(string detailID)
        {
            bool isError = false;
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtParamedicDao patientChargesDtParamedicDao = new PatientChargesDtParamedicDao(ctx);
            PatientChargesDtPackageDao entityDtPackageDao = new PatientChargesDtPackageDao(ctx);
            HealthcareServiceUnitDao chargesHSUDao = new HealthcareServiceUnitDao(ctx);
            ItemMasterDao itemMasterDao = new ItemMasterDao(ctx);
            ItemServiceDao itemServiceDao = new ItemServiceDao(ctx);

            try
            {
                string validationErrMsg = string.Empty;
                if (IsValid(ref validationErrMsg))
                {
                    PatientChargesDt chargesDt = entityChargesDtDao.Get(Convert.ToInt32(detailID));
                    if (chargesDt != null)
                    {
                        if (chargesDt.GCTransactionDetailStatus == Constant.TransactionStatus.OPEN)
                        {
                            ItemService its = itemServiceDao.Get(chargesDt.ItemID);

                            decimal entryAmount = Convert.ToDecimal(Request.Form[txtTariff.UniqueID]);

                            decimal basePrice = 0;
                            decimal basePriceComp1 = 0;
                            decimal basePriceComp2 = 0;
                            decimal basePriceComp3 = 0;
                            decimal price = 0;
                            decimal priceComp1 = 0;
                            decimal priceComp2 = 0;
                            decimal priceComp3 = 0;
                            bool isDiscountUsedComp = false;
                            decimal discountAmount = 0;
                            decimal discountAmountComp1 = 0;
                            decimal discountAmountComp2 = 0;
                            decimal discountAmountComp3 = 0;
                            decimal coverageAmount = 0;
                            bool isDiscountInPercentage = false;
                            bool isDiscountInPercentageComp1 = false;
                            bool isDiscountInPercentageComp2 = false;
                            bool isDiscountInPercentageComp3 = false;
                            bool isCoverageInPercentage = false;
                            decimal costAmount = 0;
                            decimal grossLineAmount = 0;

                            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID, chargesDt.ChargeClassID, chargesDt.ItemID, 1, DateTime.Now, ctx);
                            if (list.Count > 0)
                            {
                                GetCurrentItemTariff obj = list[0];
                                discountAmount = obj.DiscountAmount;
                                coverageAmount = obj.CoverageAmount;
                                basePrice = obj.BasePrice;
                                basePriceComp1 = obj.BasePriceComp1;
                                basePriceComp2 = obj.BasePriceComp2;
                                basePriceComp3 = obj.BasePriceComp3;
                                price = obj.Price;
                                priceComp1 = obj.PriceComp1;
                                priceComp2 = obj.PriceComp2;
                                priceComp3 = obj.PriceComp3;
                                isCoverageInPercentage = obj.IsCoverageInPercentage;
                                isDiscountInPercentage = obj.IsDiscountInPercentage;
                            }

                            if (chkIsVariable.Checked)
                            {
                                decimal factor = Math.Round(entryAmount / price, 2);

                                chargesDt.IsVariable = true;
                                chargesDt.BaseTariff = basePrice;
                                chargesDt.BaseComp1 = basePriceComp1;
                                chargesDt.BaseComp2 = basePriceComp2;
                                chargesDt.BaseComp3 = basePriceComp3;
                                chargesDt.Tariff = entryAmount;
                                chargesDt.TariffComp1 = factor * priceComp1;
                                chargesDt.TariffComp2 = factor * priceComp2;
                                chargesDt.TariffComp3 = factor * priceComp3;

                                price = entryAmount;
                            }
                            else
                            {
                                chargesDt.IsVariable = false;
                                chargesDt.BaseTariff = basePrice;
                                chargesDt.BaseComp1 = basePriceComp1;
                                chargesDt.BaseComp2 = basePriceComp2;
                                chargesDt.BaseComp3 = basePriceComp3;
                                chargesDt.Tariff = price;
                                chargesDt.TariffComp1 = priceComp1;
                                chargesDt.TariffComp2 = priceComp2;
                                chargesDt.TariffComp3 = priceComp3;
                            }
                            //TODO : ChargesDt.CostAmount

                            chargesDt.BaseQuantity = chargesDt.ChargedQuantity = chargesDt.UsedQuantity = Convert.ToDecimal(Request.Form[txtQty.UniqueID]);
                            chargesDt.ConversionFactor = 1;

                            chargesDt.CITOAmount = chkIsCITO.Checked ? Convert.ToDecimal(Request.Form[txtCITOAmount.UniqueID]) : 0;
                            chargesDt.IsCITO = chkIsCITO.Checked && chargesDt.CITOAmount != 0 ? true : false;

                            decimal qty = chargesDt.BaseQuantity;
                            grossLineAmount = (qty * price);
                            decimal amountBeforeDiscount = grossLineAmount;

                            if (hdnIsDiscountApplyToTariffComp2Only.Value == "1")
                                amountBeforeDiscount = qty * priceComp2;

                            if (chkIsDiscount.Checked)
                            {
                                decimal discountPct2 = Convert.ToDecimal(Request.Form[txtDiscountPctAmountComp2.UniqueID]);
                                discountAmountComp2 = Convert.ToDecimal(Request.Form[txtDiscountAmountComp2.UniqueID]);

                                if (discountPct2 != 0)
                                {
                                    isDiscountInPercentage = true;
                                    chargesDt.IsDiscountInPercentageComp2 = true;
                                    chargesDt.DiscountPercentageComp2 = discountPct2;
                                }
                                chargesDt.DiscountComp2 = discountAmountComp2;
                                chargesDt.GCDiscountReason = cboDiscountReason.Value.ToString();
                            }
                            else
                            {
                                chargesDt.IsDiscountInPercentageComp2 = false;
                                chargesDt.DiscountPercentageComp2 = 0;
                                chargesDt.DiscountComp2 = 0;
                                chargesDt.GCDiscountReason = null;
                                chargesDt.DiscountReason = null;
                            }

                            if (chkIsDiscount1.Checked)
                            {
                                decimal discountPct1 = Convert.ToDecimal(Request.Form[txtDiscountPctAmountComp1.UniqueID]);
                                discountAmountComp1 = Convert.ToDecimal(Request.Form[txtDiscountAmountComp1.UniqueID]);

                                if (discountPct1 != 0)
                                {
                                    isDiscountInPercentage = true;
                                    chargesDt.IsDiscountInPercentageComp1 = true;
                                    chargesDt.DiscountPercentageComp1 = discountPct1;
                                }
                                chargesDt.DiscountComp1 = discountAmountComp1;
                                chargesDt.GCDiscountReason = cboDiscountReason.Value.ToString();
                            }
                            else
                            {
                                chargesDt.IsDiscountInPercentageComp1 = false;
                                chargesDt.DiscountPercentageComp1 = 0;
                                chargesDt.DiscountComp1 = 0;
                                chargesDt.GCDiscountReason = null;
                                chargesDt.DiscountReason = null;
                            }

                            chargesDt.DiscountAmount = (chargesDt.DiscountComp1 + chargesDt.DiscountComp2 + chargesDt.DiscountComp3) * chargesDt.ChargedQuantity;
                            chargesDt.IsDiscount = chargesDt.DiscountAmount != 0 ? true : false;

                            //if (hdnIsDiscountApplyToTariffComp2Only.Value == "0" && chargesDt.TariffComp1 > 0)
                            //{
                            //    chargesDt.DiscountComp1 = Math.Round((chargesDt.TariffComp1 / chargesDt.Tariff) * discountAmount, 2);
                            //}
                            //else
                            //{
                            //    chargesDt.DiscountComp1 = 0;
                            //}
                            //if (hdnIsDiscountApplyToTariffComp2Only.Value == "0" && chargesDt.TariffComp2 > 0)
                            //{
                            //    chargesDt.DiscountComp2 = Math.Round((chargesDt.TariffComp2 / chargesDt.Tariff) * discountAmount, 2);
                            //}
                            //else
                            //{
                            //    chargesDt.DiscountComp2 = discountAmount;
                            //}

                            //chargesDt.PatientAmount = Convert.ToDecimal(Request.Form[txtPatientTotal.UniqueID]);
                            //chargesDt.PayerAmount = Convert.ToDecimal(Request.Form[txtPayerTotal.UniqueID]);
                            //chargesDt.LineAmount = Convert.ToDecimal(Request.Form[txtLineAmount.UniqueID]);

                            decimal total = grossLineAmount - chargesDt.DiscountAmount + chargesDt.CITOAmount;

                            decimal totalPayer = 0;
                            if (isCoverageInPercentage)
                            {
                                totalPayer = total * coverageAmount / 100;
                            }
                            else
                            {
                                totalPayer = coverageAmount * qty;
                            }

                            if (total == 0)
                            {
                                totalPayer = total;
                            }
                            else
                            {
                                if (totalPayer < 0 && totalPayer < total)
                                {
                                    totalPayer = total;
                                }
                                else if (totalPayer > 0 & totalPayer > total)
                                {
                                    totalPayer = total;
                                }
                            }

                            decimal oPatientAmount = total - totalPayer;
                            decimal oPayerAmount = totalPayer;
                            decimal oLineAmount = total;

                            if (hdnIsEndingAmountRoundingTo1.Value == "1")
                            {
                                decimal upPatientAmount = Math.Ceiling(oPatientAmount);
                                decimal diffUpPatientAmount = upPatientAmount - oPatientAmount;
                                if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
                                {
                                    oPatientAmount = Math.Floor(oPatientAmount);
                                }
                                else
                                {
                                    oPatientAmount = Math.Ceiling(oPatientAmount);
                                }

                                decimal upPayerAmount = Math.Ceiling(oPayerAmount);
                                decimal diffUpPayerAmount = upPayerAmount - oPayerAmount;
                                if (diffUpPayerAmount >= Convert.ToDecimal("0.5"))
                                {
                                    oPayerAmount = Math.Floor(oPayerAmount);
                                }
                                else
                                {
                                    oPayerAmount = Math.Ceiling(oPayerAmount);
                                }

                                oLineAmount = oPatientAmount + oPayerAmount;
                            }

                            chargesDt.PatientAmount = oPatientAmount;
                            chargesDt.PayerAmount = oPayerAmount;
                            chargesDt.LineAmount = oLineAmount;

                            chargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;

                            decimal oAccumulatedDiscountAmount = 0;
                            decimal oAccumulatedDiscountAmountComp1 = 0;
                            decimal oAccumulatedDiscountAmountComp2 = 0;
                            decimal oAccumulatedDiscountAmountComp3 = 0;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            string filterPackage = string.Format("PatientChargesDtID = {0} AND IsDeleted = 0", chargesDt.ID);
                            List<PatientChargesDtPackage> entityPackageLst = BusinessLayer.GetPatientChargesDtPackageList(filterPackage, ctx);
                            foreach (PatientChargesDtPackage dtpackage in entityPackageLst)
                            {
                                ItemMaster im = itemMasterDao.Get(dtpackage.ItemID);
                                ItemService isd = itemServiceDao.Get(dtpackage.ItemID);

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                decimal serviceDtQty = 0;
                                string filterServiceDt = string.Format("ItemID = {0} AND DetailItemID = {1} AND IsDeleted = 0", chargesDt.ItemID, dtpackage.ItemID);
                                List<ItemServiceDt> lstServiceDt = BusinessLayer.GetItemServiceDtList(filterServiceDt, ctx);
                                if (lstServiceDt.Count() > 0)
                                {
                                    serviceDtQty = lstServiceDt.FirstOrDefault().Quantity;
                                }

                                dtpackage.ChargedQuantity = (serviceDtQty * chargesDt.ChargedQuantity);

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                int itemType = im.GCItemType == Constant.ItemType.OBAT_OBATAN || im.GCItemType == Constant.ItemType.BARANG_MEDIS ? 2 : im.GCItemType == Constant.ItemType.BARANG_UMUM || im.GCItemType == Constant.ItemType.BAHAN_MAKANAN ? 3 : 1;
                                GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID, chargesDt.ChargeClassID, dtpackage.ItemID, itemType, DateTime.Now, ctx).FirstOrDefault();

                                basePrice = tariff.BasePrice;
                                basePriceComp1 = tariff.BasePriceComp1;
                                basePriceComp2 = tariff.BasePriceComp2;
                                basePriceComp3 = tariff.BasePriceComp3;
                                price = tariff.Price;
                                priceComp1 = tariff.PriceComp1;
                                priceComp2 = tariff.PriceComp2;
                                priceComp3 = tariff.PriceComp3;
                                isDiscountUsedComp = tariff.IsDiscountUsedComp;
                                discountAmount = tariff.DiscountAmount;
                                discountAmountComp1 = tariff.DiscountAmountComp1;
                                discountAmountComp2 = tariff.DiscountAmountComp2;
                                discountAmountComp3 = tariff.DiscountAmountComp3;
                                coverageAmount = tariff.CoverageAmount;
                                isDiscountInPercentage = tariff.IsDiscountInPercentage;
                                isDiscountInPercentageComp1 = tariff.IsDiscountInPercentageComp1;
                                isDiscountInPercentageComp2 = tariff.IsDiscountInPercentageComp2;
                                isDiscountInPercentageComp3 = tariff.IsDiscountInPercentageComp3;
                                isCoverageInPercentage = tariff.IsCoverageInPercentage;
                                costAmount = tariff.CostAmount;
                                grossLineAmount = dtpackage.ChargedQuantity * price;

                                dtpackage.BaseTariff = tariff.BasePrice;
                                dtpackage.BaseComp1 = tariff.BasePriceComp1;
                                dtpackage.BaseComp2 = tariff.BasePriceComp2;
                                dtpackage.BaseComp3 = tariff.BasePriceComp3;
                                dtpackage.Tariff = tariff.Price;
                                dtpackage.TariffComp1 = tariff.PriceComp1;
                                dtpackage.TariffComp2 = tariff.PriceComp2;
                                dtpackage.TariffComp3 = tariff.PriceComp3;
                                dtpackage.CostAmount = tariff.CostAmount;

                                decimal totalDiscountAmount = 0;
                                decimal totalDiscountAmount1 = 0;
                                decimal totalDiscountAmount2 = 0;
                                decimal totalDiscountAmount3 = 0;

                                if (isDiscountInPercentage || isDiscountInPercentageComp1 || isDiscountInPercentageComp2 || isDiscountInPercentageComp3)
                                {
                                    if (isDiscountUsedComp)
                                    {
                                        if (priceComp1 > 0)
                                        {
                                            if (isDiscountInPercentageComp1)
                                            {
                                                totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                                                dtpackage.DiscountPercentageComp1 = discountAmountComp1;
                                            }
                                            else
                                            {
                                                totalDiscountAmount1 = discountAmountComp1;
                                                dtpackage.DiscountPercentageComp1 = 0;
                                            }
                                        }

                                        if (priceComp2 > 0)
                                        {
                                            if (isDiscountInPercentageComp2)
                                            {
                                                totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                                dtpackage.DiscountPercentageComp2 = discountAmountComp2;
                                            }
                                            else
                                            {
                                                totalDiscountAmount2 = discountAmountComp2;
                                                dtpackage.DiscountPercentageComp2 = 0;
                                            }
                                        }

                                        if (priceComp3 > 0)
                                        {
                                            if (isDiscountInPercentageComp3)
                                            {
                                                totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                                dtpackage.DiscountPercentageComp3 = discountAmountComp3;
                                            }
                                            else
                                            {
                                                totalDiscountAmount3 = discountAmountComp3;
                                                dtpackage.DiscountPercentageComp3 = 0;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (priceComp1 > 0)
                                        {
                                            totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                                            dtpackage.DiscountPercentageComp1 = discountAmount;
                                        }

                                        if (priceComp2 > 0)
                                        {
                                            totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                            dtpackage.DiscountPercentageComp2 = discountAmount;
                                        }

                                        if (priceComp3 > 0)
                                        {
                                            totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                            dtpackage.DiscountPercentageComp3 = discountAmount;
                                        }
                                    }

                                    if (dtpackage.DiscountPercentageComp1 > 0)
                                    {
                                        dtpackage.IsDiscountInPercentageComp1 = true;
                                    }

                                    if (dtpackage.DiscountPercentageComp2 > 0)
                                    {
                                        dtpackage.IsDiscountInPercentageComp2 = true;
                                    }

                                    if (dtpackage.DiscountPercentageComp3 > 0)
                                    {
                                        dtpackage.IsDiscountInPercentageComp3 = true;
                                    }
                                }
                                else
                                {
                                    dtpackage.DiscountPercentageComp1 = 0;
                                    dtpackage.DiscountPercentageComp2 = 0;
                                    dtpackage.DiscountPercentageComp3 = 0;

                                    dtpackage.IsDiscountInPercentageComp1 = false;
                                    dtpackage.IsDiscountInPercentageComp2 = false;
                                    dtpackage.IsDiscountInPercentageComp3 = false;

                                    if (isDiscountUsedComp)
                                    {
                                        if (priceComp1 > 0)
                                            totalDiscountAmount1 = discountAmountComp1;
                                        if (priceComp2 > 0)
                                            totalDiscountAmount2 = discountAmountComp2;
                                        if (priceComp3 > 0)
                                            totalDiscountAmount3 = discountAmountComp3;
                                    }
                                    else
                                    {
                                        if (priceComp1 > 0)
                                            totalDiscountAmount1 = discountAmount;
                                        if (priceComp2 > 0)
                                            totalDiscountAmount2 = discountAmount;
                                        if (priceComp3 > 0)
                                            totalDiscountAmount3 = discountAmount;
                                    }
                                }

                                totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (dtpackage.ChargedQuantity);

                                if (grossLineAmount > 0)
                                {
                                    if (totalDiscountAmount > grossLineAmount)
                                    {
                                        totalDiscountAmount = grossLineAmount;
                                    }
                                }

                                dtpackage.DiscountAmount = totalDiscountAmount;
                                dtpackage.DiscountComp1 = totalDiscountAmount1;
                                dtpackage.DiscountComp2 = totalDiscountAmount2;
                                dtpackage.DiscountComp3 = totalDiscountAmount3;

                                if (isd != null)
                                {
                                    if (isd.IsPackageItem && isd.IsUsingAccumulatedPrice)
                                    {
                                        oAccumulatedDiscountAmount += dtpackage.DiscountAmount;
                                        oAccumulatedDiscountAmountComp1 += dtpackage.DiscountComp1;
                                        oAccumulatedDiscountAmountComp2 += dtpackage.DiscountComp2;
                                        oAccumulatedDiscountAmountComp3 += dtpackage.DiscountComp3;
                                    }
                                }

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                string filterIP = string.Format("ItemID = {0} AND IsDeleted = 0", dtpackage.ItemID);
                                List<ItemPlanning> iplan = BusinessLayer.GetItemPlanningList(filterIP, ctx);
                                if (iplan.Count() > 0)
                                {
                                    dtpackage.AveragePrice = iplan.FirstOrDefault().AveragePrice;
                                }
                                else
                                {
                                    dtpackage.AveragePrice = 0;
                                }


                                dtpackage.LastUpdatedBy = AppSession.UserLogin.UserID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityDtPackageDao.Update(dtpackage);
                            }

                            if (oAccumulatedDiscountAmount != 0)
                            {
                                chargesDt.IsDiscount = true;
                                chargesDt.DiscountAmount = oAccumulatedDiscountAmount;
                                chargesDt.DiscountComp1 = oAccumulatedDiscountAmountComp1;
                                chargesDt.DiscountComp2 = oAccumulatedDiscountAmountComp2;
                                chargesDt.DiscountComp3 = oAccumulatedDiscountAmountComp3;
                                chargesDt.DiscountPercentageComp1 = 0;
                                chargesDt.DiscountPercentageComp2 = 0;
                                chargesDt.DiscountPercentageComp3 = 0;
                                chargesDt.IsDiscountInPercentageComp1 = false;
                                chargesDt.IsDiscountInPercentageComp2 = false;
                                chargesDt.IsDiscountInPercentageComp3 = false;
                                chargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            }

                            if (its.IsUsingAccumulatedPrice && its.IsPackageItem)
                            {
                                decimal BaseTariff = 0;
                                decimal BaseComp1 = 0;
                                decimal BaseComp2 = 0;
                                decimal BaseComp3 = 0;
                                decimal Tariff = 0;
                                decimal TariffComp1 = 0;
                                decimal TariffComp2 = 0;
                                decimal TariffComp3 = 0;
                                decimal DiscountAmount = 0;
                                decimal DiscountComp1 = 0;
                                decimal DiscountComp2 = 0;
                                decimal DiscountComp3 = 0;
                                foreach (PatientChargesDtPackage e in entityPackageLst)
                                {
                                    BaseTariff += e.BaseTariff * e.ChargedQuantity;
                                    BaseComp1 += e.BaseComp1 * e.ChargedQuantity;
                                    BaseComp2 += e.BaseComp2 * e.ChargedQuantity;
                                    BaseComp3 += e.BaseComp3 * e.ChargedQuantity;
                                    Tariff += e.Tariff * e.ChargedQuantity;
                                    TariffComp1 += e.TariffComp1 * e.ChargedQuantity;
                                    TariffComp2 += e.TariffComp2 * e.ChargedQuantity;
                                    TariffComp3 += e.TariffComp3 * e.ChargedQuantity;
                                    DiscountAmount += e.DiscountAmount;
                                    DiscountComp1 += e.DiscountComp1 * e.ChargedQuantity;
                                    DiscountComp2 += e.DiscountComp2 * e.ChargedQuantity;
                                    DiscountComp3 += e.DiscountComp3 * e.ChargedQuantity;

                                }

                                chargesDt.BaseTariff = BaseTariff / chargesDt.ChargedQuantity;
                                chargesDt.BaseComp1 = BaseComp1 / chargesDt.ChargedQuantity;
                                chargesDt.BaseComp2 = BaseComp2 / chargesDt.ChargedQuantity;
                                chargesDt.BaseComp3 = BaseComp3 / chargesDt.ChargedQuantity;
                                chargesDt.Tariff = Tariff / chargesDt.ChargedQuantity;
                                chargesDt.TariffComp1 = TariffComp1 / chargesDt.ChargedQuantity;
                                chargesDt.TariffComp2 = TariffComp2 / chargesDt.ChargedQuantity;
                                chargesDt.TariffComp3 = TariffComp3 / chargesDt.ChargedQuantity;
                                chargesDt.DiscountAmount = DiscountAmount;
                                chargesDt.DiscountComp1 = DiscountComp1 / chargesDt.ChargedQuantity;
                                chargesDt.DiscountComp2 = DiscountComp2 / chargesDt.ChargedQuantity;
                                chargesDt.DiscountComp3 = DiscountComp3 / chargesDt.ChargedQuantity;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                list = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID, chargesDt.ChargeClassID, chargesDt.ItemID, 1, chargesDt.CreatedDate, ctx);

                                coverageAmount = 0;
                                isCoverageInPercentage = false;
                                if (list.Count > 0)
                                {
                                    GetCurrentItemTariff obj = list[0];
                                    coverageAmount = obj.CoverageAmount;
                                    isCoverageInPercentage = obj.IsCoverageInPercentage;
                                }

                                grossLineAmount = (chargesDt.Tariff * chargesDt.ChargedQuantity) + (chargesDt.CITOAmount - chargesDt.CITODiscount);
                                decimal totalDiscountAmount = chargesDt.DiscountAmount;
                                if (grossLineAmount > 0)
                                {
                                    if (totalDiscountAmount > grossLineAmount)
                                    {
                                        totalDiscountAmount = grossLineAmount;
                                    }
                                }

                                total = grossLineAmount - totalDiscountAmount;
                                totalPayer = 0;
                                if (isCoverageInPercentage)
                                {
                                    totalPayer = total * coverageAmount / 100;
                                }
                                else
                                {
                                    totalPayer = coverageAmount * chargesDt.ChargedQuantity;
                                }

                                if (total == 0)
                                {
                                    totalPayer = total;
                                }
                                else
                                {
                                    if (totalPayer < 0 && totalPayer < total)
                                    {
                                        totalPayer = total;
                                    }
                                    else if (totalPayer > 0 & totalPayer > total)
                                    {
                                        totalPayer = total;
                                    }
                                }

                                oPatientAmount = total - totalPayer;
                                oPayerAmount = totalPayer;
                                oLineAmount = total;

                                if (hdnIsEndingAmountRoundingTo1.Value == "1")
                                {
                                    decimal upPatientAmount = Math.Ceiling(oPatientAmount);
                                    decimal diffUpPatientAmount = upPatientAmount - oPatientAmount;
                                    if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
                                    {
                                        oPatientAmount = Math.Floor(oPatientAmount);
                                    }
                                    else
                                    {
                                        oPatientAmount = Math.Ceiling(oPatientAmount);
                                    }

                                    decimal upPayerAmount = Math.Ceiling(oPayerAmount);
                                    decimal diffUpPayerAmount = upPayerAmount - oPayerAmount;
                                    if (diffUpPayerAmount >= Convert.ToDecimal("0.5"))
                                    {
                                        oPayerAmount = Math.Floor(oPayerAmount);
                                    }
                                    else
                                    {
                                        oPayerAmount = Math.Ceiling(oPayerAmount);
                                    }

                                    oLineAmount = oPatientAmount + oPayerAmount;
                                }

                                chargesDt.PatientAmount = oPatientAmount;
                                chargesDt.PayerAmount = oPayerAmount;
                                chargesDt.LineAmount = oLineAmount;
                            }
                            else if (!its.IsUsingAccumulatedPrice && its.IsPackageItem)
                            {
                                foreach (PatientChargesDtPackage e in entityPackageLst)
                                {
                                    if (e.TariffComp1 != 0)
                                    {
                                        e.TariffComp1 = ((e.BaseComp1 / entityPackageLst.Sum(t => t.BaseTariff)) * chargesDt.Tariff);
                                        e.DiscountComp1 = ((e.Tariff / chargesDt.Tariff) * chargesDt.DiscountComp1);
                                    }
                                    if (e.TariffComp2 != 0)
                                    {
                                        e.TariffComp2 = ((e.BaseComp2 / entityPackageLst.Sum(t => t.BaseTariff)) * chargesDt.Tariff);
                                        e.DiscountComp2 = ((e.Tariff / chargesDt.Tariff) * chargesDt.DiscountComp2);
                                    }
                                    if (e.TariffComp3 != 0)
                                    {
                                        e.TariffComp3 = ((e.BaseComp3 / entityPackageLst.Sum(t => t.BaseTariff)) * chargesDt.Tariff);
                                        e.DiscountComp3 = ((e.Tariff / chargesDt.Tariff) * chargesDt.DiscountComp3);
                                    }

                                    e.Tariff = e.TariffComp1 + e.TariffComp2 + e.TariffComp3;
                                    e.DiscountAmount = (e.DiscountComp1 + e.DiscountComp2 + e.DiscountComp3) * e.ChargedQuantity;
                                    e.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityDtPackageDao.Update(e);
                                }
                            }

                            chargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityChargesDtDao.Update(chargesDt);
                        }
                    }
                    else
                    {
                        string message = string.Format("Invalid Charges Detail ID {0}", detailID);
                        result = string.Format("process|0|{0}||", message);
                        isError = true;
                    }
                    result = string.Format("process|1|||");
                }
                else
                {
                    string message = string.Format("Validation Error for Charges Detail ID {0} : {1}", detailID, validationErrMsg);
                    result = string.Format("process|0|{0}||", message);
                    isError = true;
                }
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}||", ex.Message);
                isError = true;
            }

            if (!isError)
            {
                ctx.CommitTransaction();
            }
            else
            {
                ctx.RollBackTransaction();
            }
            return result;
        }

        private bool IsValid(ref string errMessage)
        {
            //if (string.IsNullOrEmpty(txtCompleteDate.Text))
            //{
            //    errMessage = "Complete Date must be entried";
            //    return false;
            //}

            //if (string.IsNullOrEmpty(txtCompleteTime.Text) || txtCompleteTime.Text == "__:__")
            //{
            //    errMessage = "Complete Time must be entried";
            //    return false;
            //}
            //else
            //{
            //    Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
            //    if (!reg.IsMatch(txtCompleteTime.Text))
            //    {
            //        errMessage = "Complete time must be entried in correct format (hh:mm)";
            //        return false;
            //    }
            //} 

            return true;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }
    }
}