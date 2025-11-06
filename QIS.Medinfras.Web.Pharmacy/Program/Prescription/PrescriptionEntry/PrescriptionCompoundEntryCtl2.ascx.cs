using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrescriptionCompoundEntryCtl2 : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            hdnQueryString.Value = queryString;
            string[] param = queryString.Split('|');
            hdnLocationID.Value = param[9];
            hdnIsDrugChargesJustDistributionCP.Value = param[17];

            if (hdnIsDrugChargesJustDistributionCP.Value == "1")
            {
                ledProduct.FilterExpression = string.Format("LocationID = {0} AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '{1}' AND GCItemRequestType = '{2}'", hdnLocationID.Value, Constant.ItemStatus.IN_ACTIVE, Constant.ItemRequestType.DISTRIBUTION);
            }
            else
            {
                ledProduct.FilterExpression = string.Format("LocationID = {0} AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '{1}'", hdnLocationID.Value, Constant.ItemStatus.IN_ACTIVE);
            }

            if (param[0] == "add")
            {
                IsAdd = true;
                SetControlProperties();
                tblTemplate.Style.Add("display", "table");
            }
            else
            {
                IsAdd = true;
                SetControlProperties();
                int prescriptionDetailID = Convert.ToInt32(param[5]);
                PrescriptionOrderDt entity = BusinessLayer.GetPrescriptionOrderDt(prescriptionDetailID);
                EntityToControl(entity);
                tblTemplate.Style.Add("display", "none");
            }

            hdnImagingServiceUnitID.Value = param[15];
            hdnLaboratoryServiceUnitID.Value = param[16];

            hdnParamedicID.Value = param[10];
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtCompoundMedicationName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(cboFrequencyTimelineCompoundCtl, new ControlEntrySetting(true, true, true, Constant.DosingFrequency.DAY));
            SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(cboDosingUnitCompoundCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(chkIsUsingSweetener, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsPRN, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboEmbalace, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmbalaceQty, new ControlEntrySetting(true, true, false, "1"));
            SetControlEntrySetting(cboMedicationRouteCompoundCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCoenamRuleCompoundCtl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtStartTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMedicationPurpose, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(PrescriptionOrderDt entity)
        {
            txtDispenseQty.Text = entity.DispenseQty.ToString();
            txtTakenQty.Text = entity.TakenQty.ToString();
            txtCompoundMedicationName.Text = entity.CompoundDrugname;
            cboMedicationRouteCompoundCtl.Value = entity.GCRoute;
            cboCoenamRuleCompoundCtl.Value = entity.GCCoenamRule;
            txtDosingDose.Text = entity.NumberOfDosage.ToString();
            cboDosingUnitCompoundCtl.Value = entity.GCDosingUnit;
            chkIsUsingSweetener.Checked = entity.IsUseSweetener;
            chkIsPRN.Checked = entity.IsAsRequired;

            cboFrequencyTimelineCompoundCtl.Value = entity.GCDosingFrequency;
            txtFrequencyNumber.Text = entity.Frequency.ToString();
            txtDosingDuration.Text = entity.DosingDuration.ToString();

            if (entity.SignaID != null & entity.SignaID != 0)
            {
                Signa signa = BusinessLayer.GetSigna((int)entity.SignaID);
                hdnSignaID.Value = entity.SignaID.ToString();
                txtSignaLabel.Text = signa.SignaLabel;
                txtSignaName1.Text = signa.SignaName1;
            }

            if (entity.EmbalaceID != null && entity.EmbalaceID != 0)
            {
                EmbalaceHd embalace = BusinessLayer.GetEmbalaceHd((int)entity.EmbalaceID);
                cboEmbalace.Value = entity.EmbalaceID.ToString();
                if (entity.TakenQty == 0)
                {
                    txtEmbalaceQty.Text = "0.00";
                }
                else
                {
                    txtEmbalaceQty.Text = entity.EmbalaceQty.ToString(Constant.FormatString.NUMERIC_2);
                }
                //txtEmbalaceQty.Text = entity.EmbalaceQty.ToString(Constant.FormatString.NUMERIC_2);
                if (!embalace.IsUsingRangePricing)
                {
                    txtEmbalaceQty.Attributes.Remove("readonly");
                }
                else
                {
                    txtEmbalaceQty.Attributes.Add("readonly", "readonly");
                }
            }

            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = entity.StartTime;

            txtMedicationAdministration.Text = entity.MedicationAdministration;
            txtMedicationPurpose.Text = entity.MedicationPurpose;

        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.COENAM_RULE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboCompoundUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimelineCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCompoundStrengthUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnitCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1" || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRouteCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRuleCompoundCtl, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            filterExpression = string.Format("HealthcareID = '{0}'  AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                                                    AppSession.UserLogin.HealthcareID, //0
                                                    Constant.SettingParameter.PH_USED_STRENGTH_UNIT_AS_DEFAULT, //1
                                                    Constant.SettingParameter.PH0037, //2
                                                    Constant.SettingParameter.PH_CREATE_QUEUE_LABEL, //3
                                                    Constant.SettingParameter.PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX, //4
                                                    Constant.SettingParameter.FN_PEMBATASAN_CPOE_BPJS, //5
                                                    Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS, //6
                                                    Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN, //7
                                                    Constant.SettingParameter.FN_PENJAMIN_INHEALTH, //8
                                                    Constant.SettingParameter.FN_PEMBATASAN_CPOE_INHEALTH, //9
                                                    Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //10
                                                    Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1 //11
                                                );
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);

            hdnIsEndingAmountRoundingTo100.Value = lstParam.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstParam.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;

            string oParam = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.PH_USED_STRENGTH_UNIT_AS_DEFAULT).FirstOrDefault().ParameterValue;
            hdnIsDefaultUsingStrengthUnit.Value = oParam != null ? oParam : "0";

            string oParam1 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PEMBATASAN_CPOE_BPJS).FirstOrDefault().ParameterValue;
            bool isLimitedCPOEItemForBPJS = oParam1 != null ? (oParam1 == "1" ? true : false) : false;

            string oParam2 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault().ParameterValue;
            if (string.IsNullOrEmpty(oParam2))
            {
                oParam2 = "0";
            }

            string oParam3 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PEMBATASAN_CPOE_INHEALTH).FirstOrDefault().ParameterValue;
            bool isLimitedCPOEItemForInhealth = oParam3 != null ? (oParam3 == "1" ? true : false) : false;

            string oParam4 = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_INHEALTH).FirstOrDefault().ParameterValue;
            string inhealthCustomerType = oParam4 != null ? oParam4 : string.Empty;

            string lookupEditFilterExp = "IsDeleted = 0";

            if (hdnIsDrugChargesJustDistributionCP.Value == "1")
            {
                lookupEditFilterExp = string.Format("LocationID = {0} AND IsDeleted = 0 AND GCItemRequestType = '{1}'", hdnLocationID.Value, Constant.ItemRequestType.DISTRIBUTION);
            }
            else
            {
                lookupEditFilterExp = string.Format("LocationID = {0} AND IsDeleted = 0", hdnLocationID.Value);
            }

            if (AppSession.RegisteredPatient.BusinessPartnerID.ToString() == oParam2 && isLimitedCPOEItemForBPJS)
            {
                lookupEditFilterExp += " AND IsBPJSFormularium = 1 AND IsDeleted = 0";
            }

            if (AppSession.RegisteredPatient.GCCustomerType == inhealthCustomerType && isLimitedCPOEItemForInhealth)
            {
                lookupEditFilterExp += " AND IsInhealthFormularium = 1 AND IsDeleted = 0";
            }

            lookupEditFilterExp += string.Format(" AND ISNULL(GCItemStatus,'') != '{0}'", Constant.ItemStatus.IN_ACTIVE);

            ledProduct.FilterExpression = lookupEditFilterExp;

            hdnIsAutoGenerateReferenceNo.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH0037).ParameterValue;
            hdnIsGenerateQueueLabel.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_CREATE_QUEUE_LABEL).ParameterValue;
            hdnItemQtyWithSpecialQueuePrefix.Value = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX).ParameterValue;

            List<EmbalaceHd> lstEmbalace = BusinessLayer.GetEmbalaceHdList("IsDeleted = 0");
            lstEmbalace.Insert(0, new EmbalaceHd() { EmbalaceID = 0, EmbalaceName = "" });
            Methods.SetComboBoxField<EmbalaceHd>(cboEmbalace, lstEmbalace, "EmbalaceName", "EmbalaceID");
            cboEmbalace.SelectedIndex = 0;

            cboFrequencyTimelineCompoundCtl.Value = Constant.DosingFrequency.DAY;
            cboMedicationRouteCompoundCtl.SelectedIndex = 0;

            txtStartDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtStartTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtDispenseQty.Text = "1";
            txtTakenQty.Text = "1";
        }

        private void ControlToEntity(PrescriptionOrderDt entityDt, String[] data)
        {
            bool isChanged = data[0] == "1" ? true : false;
            string itemID = data[3];
            if (itemID != "")
            {
                entityDt.IsCompound = true;
                entityDt.GenericName = data[2];
                if (data[3] != "")
                    entityDt.ItemID = Convert.ToInt32(data[3]);
                else
                    entityDt.ItemID = null;
                if (data[5] != "")
                {
                    entityDt.Dose = Convert.ToDecimal(data[4]);
                    entityDt.GCDoseUnit = data[5];
                }
                else
                {
                    entityDt.Dose = null;
                    entityDt.GCDoseUnit = null;
                }
                string GCItemUnit = data[14];

                entityDt.GCCompoundUnit = data[7];
                entityDt.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
                entityDt.TakenQty = Convert.ToDecimal(txtTakenQty.Text);
                if (GCItemUnit != entityDt.GCCompoundUnit)
                {
                    string compoundQty = data[6].Replace(',', '.');
                    decimal qty = 0;
                    if (compoundQty.Contains('/'))
                    {
                        string[] compoundInfo = compoundQty.Split('/');
                        decimal num1 = Convert.ToDecimal(compoundInfo[0].Replace(',', '.'));
                        decimal num2 = Convert.ToDecimal(compoundInfo[1].Replace(',', '.'));
                        //qty = Math.Round(num1 / num2, 2);

                        decimal dose = Convert.ToDecimal(data[10]);
                        entityDt.CompoundQtyInString = data[6].Replace(',', '.');
                        entityDt.CompoundQty = num1 / num2 / dose;
                        entityDt.ResultQty = num1 / num2 * entityDt.TakenQty / dose;
                    }
                    else
                    {
                        qty = Convert.ToDecimal(compoundQty);

                        decimal dose = Convert.ToDecimal(data[10]);
                        entityDt.CompoundQtyInString = data[6].Replace(',', '.');
                        entityDt.CompoundQty = qty / dose;
                        entityDt.ResultQty = qty * entityDt.TakenQty / dose;
                    }
                }
                else
                {
                    string compoundQty = data[6].Replace(',', '.');
                    decimal qty = 0;
                    if (compoundQty.Contains('/'))
                    {
                        string[] compoundInfo = compoundQty.Split('/');
                        decimal num1 = Convert.ToDecimal(compoundInfo[0].Replace(',', '.'));
                        decimal num2 = Convert.ToDecimal(compoundInfo[1].Replace(',', '.'));
                        //qty = Math.Round(num1 / num2, 2);

                        entityDt.CompoundQty = num1 / num2;
                        entityDt.CompoundQtyInString = data[6].Replace(',', '.');
                        entityDt.ResultQty = num1 / num2 * entityDt.TakenQty;
                    }
                    else
                    {
                        qty = Convert.ToDecimal(compoundQty.Replace(',', '.'));

                        entityDt.CompoundQty = qty;
                        entityDt.CompoundQtyInString = data[6].Replace(',', '.');
                        entityDt.ResultQty = qty * entityDt.TakenQty;
                    }
                }

                entityDt.DrugName = data[9];
                //entityDt.ChargeQty = entityDt.ResultQty;
                //if (data[15] == Constant.QuantityDeductionType.DIBULATKAN) entityDt.ChargeQty = Math.Ceiling(entityDt.ChargeQty);
                //if (data[16] == Constant.QuantityDeductionType.DIBULATKAN) entityDt.ResultQty = Math.Ceiling(entityDt.ResultQty);

                entityDt.ChargeQty = entityDt.ResultQty = Math.Round(entityDt.ResultQty, 2);
                //if (data[15] == Constant.QuantityDeductionType.DIBULATKAN) entityDt.ChargeQty = entityDt.ChargeQty < 1 ? 1 : Math.Ceiling(entityDt.ChargeQty);
                if (data[15] == Constant.QuantityDeductionType.DIBULATKAN)
                {
                    if (entityDt.ChargeQty != 0 && entityDt.ChargeQty < 1)
                    {
                        entityDt.ChargeQty = 1;
                    }
                    else if (entityDt.ChargeQty == 0)
                    {
                        entityDt.ChargeQty = 0;
                    }
                    else
                    {
                        entityDt.ChargeQty = Math.Ceiling(entityDt.ChargeQty);
                    }
                }
                else
                {
                    entityDt.ChargeQty = entityDt.ChargeQty;
                }

                //if (data[16] == Constant.QuantityDeductionType.DIBULATKAN) entityDt.ResultQty = entityDt.ResultQty < 1 ? 1 : Math.Ceiling(entityDt.ResultQty);
                if (data[16] == Constant.QuantityDeductionType.DIBULATKAN)
                {
                    if (entityDt.ResultQty != 0 && entityDt.ResultQty < 1)
                    {
                        entityDt.ResultQty = 1;
                    }
                    else if (entityDt.ResultQty == 0)
                    {
                        entityDt.ResultQty = 0;
                    }
                    else
                    {
                        entityDt.ResultQty = Math.Ceiling(entityDt.ResultQty);
                    }
                }
                else
                {
                    entityDt.ResultQty = entityDt.ResultQty;
                }

                entityDt.CompoundDrugname = txtCompoundMedicationName.Text;

                if (cboMedicationRouteCompoundCtl.Value != null)
                {
                    entityDt.GCRoute = cboMedicationRouteCompoundCtl.Value.ToString();
                }

                if (cboCoenamRuleCompoundCtl.Value != null)
                {
                    entityDt.GCCoenamRule = cboCoenamRuleCompoundCtl.Value.ToString();
                }
                else
                {
                    entityDt.GCCoenamRule = null;
                }

                if (hdnSignaID.Value != null && hdnSignaID.Value != "")
                {
                    entityDt.SignaID = Convert.ToInt32(hdnSignaID.Value);
                }
                else
                {
                    entityDt.SignaID = null;
                }

                entityDt.NumberOfDosage = txtDosingDose.Text != "" ? Convert.ToDecimal(txtDosingDose.Text) : 0;
                entityDt.GCDosingUnit = cboDosingUnitCompoundCtl.Value.ToString();
                entityDt.IsUseSweetener = chkIsUsingSweetener.Checked;
                entityDt.IsAsRequired = chkIsPRN.Checked;
                entityDt.GCDosingFrequency = cboFrequencyTimelineCompoundCtl.Value.ToString();
                entityDt.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
                entityDt.DosingDuration = Convert.ToInt16(txtDosingDuration.Text);
                entityDt.StartDate = Helper.GetDatePickerValue(txtStartDate);
                entityDt.StartTime = txtStartTime.Text;
                entityDt.MedicationAdministration = txtMedicationAdministration.Text;
                entityDt.MedicationPurpose = txtMedicationPurpose.Text;
                entityDt.GCPrescriptionOrderStatus = Constant.OrderStatus.IN_PROGRESS;
                entityDt.IsRFlag = false;
                entityDt.ParentID = null;

                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
            }
        }

        private void ControlToEntity2(PatientChargesDt entityChargesDt, PrescriptionOrderDt entityDt, string stockDeduction, string gcItemUnit, IDbContext ctx)
        {
            string[] paramHeader = hdnQueryString.Value.ToString().Split('|');
            #region PatientChargesDt
            entityChargesDt.ItemID = (int)entityDt.ItemID;
            entityChargesDt.LocationID = Convert.ToInt32(paramHeader[9]);
            entityChargesDt.ChargeClassID = Convert.ToInt32(paramHeader[14]);
            entityChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(AppSession.RegisteredPatient.RegistrationID), AppSession.RegisteredPatient.VisitID, entityChargesDt.ChargeClassID, entityChargesDt.ItemID, 2, DateTime.Now, ctx);

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

            if (list.Count > 0)
            {
                GetCurrentItemTariff obj = list[0];
                basePrice = obj.BasePrice;
                basePriceComp1 = obj.BasePriceComp1;
                basePriceComp2 = obj.BasePriceComp2;
                basePriceComp3 = obj.BasePriceComp3;
                price = obj.Price;
                priceComp1 = obj.PriceComp1;
                priceComp2 = obj.PriceComp2;
                priceComp3 = obj.PriceComp3;
                isDiscountUsedComp = obj.IsDiscountUsedComp;
                discountAmount = obj.DiscountAmount;
                discountAmountComp1 = obj.DiscountAmountComp1;
                discountAmountComp2 = obj.DiscountAmountComp2;
                discountAmountComp3 = obj.DiscountAmountComp3;
                coverageAmount = obj.CoverageAmount;
                isDiscountInPercentage = obj.IsDiscountInPercentage;
                isDiscountInPercentageComp1 = obj.IsDiscountInPercentageComp1;
                isDiscountInPercentageComp2 = obj.IsDiscountInPercentageComp2;
                isDiscountInPercentageComp3 = obj.IsDiscountInPercentageComp3;
                isCoverageInPercentage = obj.IsCoverageInPercentage;
                costAmount = obj.CostAmount;
            }

            entityChargesDt.BaseTariff = basePrice;
            entityChargesDt.Tariff = price;
            entityChargesDt.BaseComp1 = basePriceComp1;
            entityChargesDt.BaseComp2 = basePriceComp2;
            entityChargesDt.BaseComp3 = basePriceComp3;
            entityChargesDt.TariffComp1 = priceComp1;
            entityChargesDt.TariffComp2 = priceComp2;
            entityChargesDt.TariffComp3 = priceComp3;
            entityChargesDt.CostAmount = costAmount;

            entityChargesDt.UsedQuantity = entityDt.ResultQty;
            //if (stockDeduction == Constant.QuantityDeductionType.DIBULATKAN) entityChargesDt.UsedQuantity = Math.Ceiling(entityChargesDt.UsedQuantity);

            //if (stockDeduction == Constant.QuantityDeductionType.DIBULATKAN) entityChargesDt.UsedQuantity = entityChargesDt.UsedQuantity < 1 ? 1 : Math.Ceiling(entityChargesDt.UsedQuantity);
            if (stockDeduction == Constant.QuantityDeductionType.DIBULATKAN)
            {
                if (entityChargesDt.UsedQuantity != 0 && entityChargesDt.UsedQuantity < 1)
                {
                    entityChargesDt.UsedQuantity = 1;
                }
                else if (entityChargesDt.UsedQuantity == 0)
                {
                    entityChargesDt.UsedQuantity = 0;
                }
                else
                {
                    entityChargesDt.UsedQuantity = Math.Ceiling(entityChargesDt.UsedQuantity);
                }
            }
            else
            {
                entityChargesDt.UsedQuantity = entityChargesDt.UsedQuantity;
            }

            entityChargesDt.ChargedQuantity = entityDt.ChargeQty;
            entityChargesDt.BaseQuantity = entityChargesDt.UsedQuantity;
            entityChargesDt.GCItemUnit = gcItemUnit;
            entityChargesDt.GCBaseUnit = gcItemUnit;

            decimal grossLineAmount = 0;
            if (entityDt.IsRFlag)
            {
                if (entityDt.EmbalaceID != null && entityDt.EmbalaceID != 0)
                {
                    EmbalaceHd embalaceHd = BusinessLayer.GetEmbalaceHd((int)entityDt.EmbalaceID);
                    if (entityDt.TakenQty != 0)
                    {
                        entityDt.EmbalaceQty = Convert.ToDecimal(txtEmbalaceQty.Text);
                    }
                    if (embalaceHd.IsUsingRangePricing)
                    {
                        EmbalaceDt embalaceDt = BusinessLayer.GetEmbalaceDtList(string.Format("EmbalaceID = {0} AND StartingQty <= {1} AND EndingQty >= {1}", entityDt.EmbalaceID, entityDt.DispenseQty)).FirstOrDefault();
                        if (embalaceDt != null)
                        {
                            if (entityDt.TakenQty == 0)
                            {
                                entityChargesDt.EmbalaceAmount = 0;
                            }
                            else
                            {
                                entityChargesDt.EmbalaceAmount = Convert.ToDecimal(embalaceDt.Tariff);
                            }
                        }
                        else
                        {
                            entityChargesDt.EmbalaceAmount = 0;
                        }
                    }
                    else
                    {
                        if (entityDt.TakenQty == 0)
                        {
                            entityChargesDt.EmbalaceAmount = 0;
                        }
                        else
                        {
                            entityChargesDt.EmbalaceAmount = Convert.ToDecimal(embalaceHd.Tariff * entityDt.EmbalaceQty);
                        }
                    }
                }
                else
                {
                    entityChargesDt.EmbalaceAmount = 0;
                }

                if (entityDt.TakenQty != 0)
                {
                    entityChargesDt.PrescriptionFeeAmount = Convert.ToDecimal(paramHeader[8]);
                }
                else
                {
                    entityChargesDt.PrescriptionFeeAmount = 0;
                }
            }

            grossLineAmount = (entityChargesDt.ChargedQuantity * price) + entityChargesDt.EmbalaceAmount + entityChargesDt.PrescriptionFeeAmount;

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
                            entityChargesDt.DiscountPercentageComp1 = discountAmountComp1;
                        }
                        else
                        {
                            totalDiscountAmount1 = discountAmountComp1;
                        }
                    }

                    if (priceComp2 > 0)
                    {
                        if (isDiscountInPercentageComp2)
                        {
                            totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                            entityChargesDt.DiscountPercentageComp2 = discountAmountComp2;
                        }
                        else
                        {
                            totalDiscountAmount2 = discountAmountComp2;
                        }
                    }

                    if (priceComp3 > 0)
                    {
                        if (isDiscountInPercentageComp3)
                        {
                            totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                            entityChargesDt.DiscountPercentageComp3 = discountAmountComp3;
                        }
                        else
                        {
                            totalDiscountAmount3 = discountAmountComp3;
                        }
                    }
                }
                else
                {
                    if (priceComp1 > 0)
                    {
                        totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                        entityChargesDt.DiscountPercentageComp1 = discountAmount;
                    }

                    if (priceComp2 > 0)
                    {
                        totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                        entityChargesDt.DiscountPercentageComp2 = discountAmount;
                    }

                    if (priceComp3 > 0)
                    {
                        totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                        entityChargesDt.DiscountPercentageComp3 = discountAmount;
                    }
                }

                if (entityChargesDt.DiscountPercentageComp1 > 0)
                {
                    entityChargesDt.IsDiscountInPercentageComp1 = true;
                }

                if (entityChargesDt.DiscountPercentageComp2 > 0)
                {
                    entityChargesDt.IsDiscountInPercentageComp2 = true;
                }

                if (entityChargesDt.DiscountPercentageComp3 > 0)
                {
                    entityChargesDt.IsDiscountInPercentageComp3 = true;
                }
            }
            else
            {
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

            totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (entityChargesDt.ChargedQuantity);

            if (grossLineAmount > 0)
            {
                if (totalDiscountAmount > grossLineAmount)
                {
                    totalDiscountAmount = grossLineAmount;
                }
            }

            decimal total = grossLineAmount - totalDiscountAmount;
            decimal totalPayer = 0;
            if (isCoverageInPercentage)
            {
                totalPayer = total * coverageAmount / 100;
            }
            else
            {
                totalPayer = coverageAmount * entityChargesDt.ChargedQuantity;

                if (totalPayer != 0)
                {
                    totalPayer = totalPayer + entityChargesDt.EmbalaceAmount;
                }
            }

            //    totalPayer = (coverageAmount * entityChargesDt.ChargedQuantity) + entityChargesDt.EmbalaceAmount + entityChargesDt.PrescriptionFeeAmount;
            //if (totalPayer > total)
            if (total > 0 && totalPayer > total)
                totalPayer = total;

            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityChargesDt.ItemID), ctx).FirstOrDefault();
            entityChargesDt.AveragePrice = iPlanning.AveragePrice;
            entityChargesDt.CostAmount = iPlanning.UnitPrice;

            if (entityChargesDt.ItemID != null && entityChargesDt.ItemID != 0)
            {
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                ItemProduct iProduct = BusinessLayer.GetItemProductList(string.Format("ItemID = {0}", entityChargesDt.ItemID), ctx).FirstOrDefault();
                entityChargesDt.HETAmount = iProduct.HETAmount;
            }

            if (entityDt.ConversionFactor != 0)
            {
                entityChargesDt.ConversionFactor = entityDt.ConversionFactor;
            }
            else
            {
                entityChargesDt.ConversionFactor = 1;
            }
            entityChargesDt.IsCITO = false;
            entityChargesDt.CITOAmount = 0;
            entityChargesDt.IsComplication = false;
            entityChargesDt.ComplicationAmount = 0;

            entityChargesDt.IsDiscount = totalDiscountAmount != 0;
            entityChargesDt.DiscountAmount = totalDiscountAmount;
            entityChargesDt.DiscountComp1 = totalDiscountAmount1;
            entityChargesDt.DiscountComp2 = totalDiscountAmount2;
            entityChargesDt.DiscountComp3 = totalDiscountAmount3;

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

            if (hdnIsEndingAmountRoundingTo100.Value == "1")
            {
                oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                oLineAmount = oPatientAmount + oPayerAmount;
            }

            entityChargesDt.PatientAmount = oPatientAmount;
            entityChargesDt.PayerAmount = oPayerAmount;
            entityChargesDt.LineAmount = oLineAmount;

            entityChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
            entityChargesDt.CreatedBy = AppSession.UserLogin.UserID;
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            //isAdd;0;genericName;itemID;doseqty;gcdoseunit;compoundqty;gcitemunit;conversionfactor;itemname;doselabel;doseunit;gcdoseunit;itemunit;gcitemunit
            bool result = true;
            String[] listParam = hdnInlineEditingData.Value.Split('|');
            foreach (String param in listParam)
            {
                String[] dataSplit = param.Split(';');

                bool isChanged = dataSplit[0] == "1" ? true : false;
                int ID = Convert.ToInt32(dataSplit[1]);
                if (isChanged || ID > 0)
                {
                    //if (Convert.ToDecimal(dataSplit[4]) <= 0 || Convert.ToDecimal(dataSplit[6]) <= 0)
                    //{
                    //    result = false;
                    //    errMessage = "Dose Quantity or Compound Quantity should be greater than 0";
                    //}

                    if (dataSplit[6] != null && dataSplit[6] != "" && !dataSplit[6].Contains('/'))
                    {
                        if (Convert.ToDecimal(dataSplit[6]) < 0)
                        {
                            result = false;
                            errMessage = "Compound Quantity should be greater than 0";
                        }
                    }
                }

                if (Convert.ToInt16(txtFrequencyNumber.Text) < 0)
                {
                    result = false;
                    errMessage = "Frequency Quantity should be greater than 0";
                }

                if (Convert.ToDecimal(txtDosingDose.Text) < 0)
                {
                    result = false;
                    errMessage = "Dose Quantity should be greater than 0";
                }

                if (Convert.ToInt16(txtDosingDuration.Text) < 0)
                {
                    result = false;
                    errMessage = "Duration should be greater than 0";
                }

                if (Convert.ToDecimal(txtEmbalaceQty.Text) < 0)
                {
                    result = false;
                    errMessage = "Embalace Quantity should be greater than 0";
                }
            }

            if (result)
            {
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdDao entityHdDao = new PrescriptionOrderHdDao(ctx);
                PrescriptionOrderDtDao entityDtDao = new PrescriptionOrderDtDao(ctx);
                PrescriptionOrderDtLogDao orderLogDao = new PrescriptionOrderDtLogDao(ctx);
                PatientChargesHdDao entityChargesHdDao = new PatientChargesHdDao(ctx);
                PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
                try
                {
                    int prescriptionID = -1;
                    int transactionID = -1;
                    string transactionNo;
                    string[] paramHeader = hdnQueryString.Value.ToString().Split('|');
                    if (paramHeader[2] == "0")
                    {
                        #region Save Header
                        DateTime prescriptionDate = Helper.DateInStringToDateTime(paramHeader[3]);
                        string prescriptionTime = paramHeader[4];
                        int paramedicID = Convert.ToInt32(hdnParamedicID.Value);

                        PrescriptionOrderHd entityHd = new PrescriptionOrderHd();
                        entityHd.PrescriptionDate = prescriptionDate;
                        entityHd.PrescriptionTime = prescriptionTime;
                        entityHd.ParamedicID = paramedicID;
                        entityHd.VisitID = Convert.ToInt32(paramHeader[7]);
                        entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                        entityHd.ClassID = Convert.ToInt32(paramHeader[14]);
                        entityHd.GCPrescriptionType = paramHeader[13];
                        entityHd.DispensaryServiceUnitID = Convert.ToInt32(paramHeader[11]);
                        entityHd.LocationID = Convert.ToInt32(paramHeader[9]);
                        entityHd.GCRefillInstruction = paramHeader[6];
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        entityHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                        switch (paramHeader[12])
                        {
                            case Constant.Facility.EMERGENCY:
                                entityHd.TransactionCode = Constant.TransactionCode.ER_MEDICATION_ORDER;
                                break;
                            case Constant.Facility.OUTPATIENT:
                                entityHd.TransactionCode = Constant.TransactionCode.OP_MEDICATION_ORDER;
                                break;
                            case Constant.Facility.INPATIENT:
                                entityHd.TransactionCode = Constant.TransactionCode.IP_MEDICATION_ORDER;
                                break;
                            case Constant.Facility.DIAGNOSTIC:
                                if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() == hdnImagingServiceUnitID.Value)
                                    entityHd.TransactionCode = Constant.TransactionCode.IMAGING_MEDICATION_ORDER;
                                else if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() == hdnLaboratoryServiceUnitID.Value)
                                    entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_MEDICATION_ORDER;
                                else
                                    entityHd.TransactionCode = Constant.TransactionCode.PH_MEDICATION_ORDER;
                                break;
                            case Constant.Facility.PHARMACY:
                                entityHd.TransactionCode = Constant.TransactionCode.PH_MEDICATION_ORDER;
                                break;
                            default:
                                entityHd.TransactionCode = Constant.TransactionCode.PH_MEDICATION_ORDER;
                                break;
                        }
                        entityHd.PrescriptionOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.PrescriptionDate, ctx);
                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        entityHd.LastUpdatedDate = DateTime.Now;
                        entityHd.IsCreatedBySystem = true;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        prescriptionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                        #endregion

                        #region PatientChargesHd
                        PatientChargesHd entityPatientChargesHd = new PatientChargesHd();

                        entityPatientChargesHd.VisitID = Convert.ToInt32(paramHeader[7]);
                        entityPatientChargesHd.TransactionDate = prescriptionDate;
                        entityPatientChargesHd.TransactionTime = prescriptionTime;
                        entityPatientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entityPatientChargesHd.PrescriptionOrderID = prescriptionID;
                        entityPatientChargesHd.HealthcareServiceUnitID = Convert.ToInt32(paramHeader[11]);
                        switch (paramHeader[12])
                        {
                            case Constant.Facility.EMERGENCY:
                                entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_EMERGENCY;
                                break;
                            case Constant.Facility.OUTPATIENT:
                                entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_OUTPATIENT;
                                break;
                            case Constant.Facility.INPATIENT:
                                entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_INPATIENT;
                                break;
                            case Constant.Facility.PHARMACY:
                                entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PH_CHARGES;
                                break;
                            case Constant.Facility.MEDICAL_CHECKUP:
                                entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_OTHER;
                                break;
                            case Constant.Facility.DIAGNOSTIC:
                                if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() == hdnImagingServiceUnitID.Value)
                                    entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_IMAGING;
                                else if (AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString() == hdnLaboratoryServiceUnitID.Value)
                                    entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_LABORATORY;
                                else
                                    entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_OTHER;
                                break;
                        }
                        entityPatientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityPatientChargesHd.TransactionCode, entityPatientChargesHd.TransactionDate, ctx);

                        if (hdnIsAutoGenerateReferenceNo.Value == "1")
                        {
                            entityPatientChargesHd.ReferenceNo = BusinessLayer.GeneratePrescriptionReferenceNo(entityPatientChargesHd.HealthcareServiceUnitID, entityPatientChargesHd.TransactionDate, ctx);
                        }

                        bool isSpecialTrx = true; // Compound Prescription always true
                        if (hdnIsGenerateQueueLabel.Value == "1" && string.IsNullOrEmpty(entityPatientChargesHd.QueueNoLabel))
                        {
                            entityPatientChargesHd.QueueNoLabel = BusinessLayer.GenerateChargesQueueNoLabel(entityPatientChargesHd.HealthcareServiceUnitID, entityPatientChargesHd.TransactionDate, isSpecialTrx, ctx);
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityPatientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
                        transactionID = entityChargesHdDao.InsertReturnPrimaryKeyID(entityPatientChargesHd);
                        transactionNo = entityPatientChargesHd.TransactionNo;
                        #endregion
                    }
                    else
                    {
                        prescriptionID = Convert.ToInt32(paramHeader[1]);
                        transactionID = Convert.ToInt32(paramHeader[2]);
                    }

                    if (transactionID > 0)
                    {
                        int prescriptionDetailID = 0;
                        List<PrescriptionOrderDt> lstEntityPrescription = null;
                        List<PatientChargesDt> lstEntityChargesDt = null;
                        if (paramHeader[5] != "")
                        {
                            prescriptionDetailID = Convert.ToInt32(paramHeader[5]);
                            string filterExpression = String.Format("(PrescriptionOrderDetailID = {0} OR ParentID = {0}) AND IsCompound = 1 AND IsDeleted = 0", prescriptionDetailID);
                            lstEntityPrescription = BusinessLayer.GetPrescriptionOrderDtList(filterExpression);
                        }
                        else
                            lstEntityPrescription = new List<PrescriptionOrderDt>();

                        string lstPrescriptionOrderID = string.Join(",", lstEntityPrescription.Select(x => x.PrescriptionOrderDetailID));
                        if (!string.IsNullOrEmpty(lstPrescriptionOrderID))
                        {
                            lstEntityChargesDt = BusinessLayer.GetPatientChargesDtList(String.Format("TransactionID = {0} AND PrescriptionOrderDetailID IN ({1}) AND IsDeleted = 0", transactionID, lstPrescriptionOrderID), ctx);
                        }
                        else lstEntityChargesDt = new List<PatientChargesDt>();
                        List<PrescriptionOrderDt> lstPOUpdated = new List<PrescriptionOrderDt>();
                        List<PrescriptionOrderDt> lstPOInserted = new List<PrescriptionOrderDt>();

                        List<Variable> lstGCStockDeduction = new List<Variable>();
                        List<string> lstGCItemUnitUpdated = new List<string>();
                        List<string> lstGCItemUnitInserted = new List<string>();

                        foreach (String param in listParam)
                        {
                            String[] data = param.Split(';');
                            if (data[3] == "") break;

                            bool flagIsUpdated = false;
                            for (int i = 0; i < lstEntityPrescription.Count; i++)
                            {
                                if (lstEntityPrescription[i].PrescriptionOrderDetailID == Convert.ToInt32(data[17]))
                                {
                                    //update
                                    ControlToEntity(lstEntityPrescription[i], data);
                                    lstPOUpdated.Add(lstEntityPrescription[i]);
                                    flagIsUpdated = true;
                                    lstEntityPrescription.RemoveAt(i);
                                    lstGCItemUnitUpdated.Add(data[14]);
                                    break;
                                }
                            }

                            if (!flagIsUpdated)
                            {
                                //new
                                PrescriptionOrderDt entityPO = new PrescriptionOrderDt();
                                ControlToEntity(entityPO, data);
                                lstGCItemUnitInserted.Add(data[14]);
                                lstPOInserted.Add(entityPO);
                            }
                            Variable tempVar = new Variable();
                            tempVar.Code = data[3];
                            tempVar.Value = data[16];
                            lstGCStockDeduction.Add(tempVar);
                        }

                        foreach (PrescriptionOrderDt entity in lstEntityPrescription)
                        {
                            hdnSelectedItem.Value = JsonConvert.SerializeObject(entity);

                            if (entity.IsRFlag)
                            {
                                if (cboEmbalace.Value != null)
                                {
                                    if (cboEmbalace.Value.ToString() != "" && cboEmbalace.Value.ToString() != "0")
                                    {
                                        entity.EmbalaceID = Convert.ToInt32(cboEmbalace.Value);
                                        if (entity.TakenQty != 0)
                                        {
                                            entity.EmbalaceQty = Convert.ToDecimal(txtEmbalaceQty.Text);
                                        }
                                        else
                                        {
                                            entity.EmbalaceQty = 0;
                                        }
                                    }
                                    else
                                    {
                                        entity.EmbalaceID = null;
                                        entity.EmbalaceQty = 0;
                                    }
                                }
                                else
                                {
                                    entity.EmbalaceID = null;
                                    entity.EmbalaceQty = 0;
                                }
                            }
                            else
                            {
                                entity.EmbalaceID = null;
                                entity.EmbalaceQty = 0;
                            }

                            entity.IsDeleted = true;
                            entity.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);

                            PrescriptionOrderDtLog orderDtLog = new PrescriptionOrderDtLog();
                            orderDtLog.LogDate = DateTime.Now;
                            orderDtLog.PrescriptionOrderDetailID = entity.PrescriptionOrderDetailID;
                            orderDtLog.OldValues = hdnSelectedItem.Value;
                            orderDtLog.NewValues = JsonConvert.SerializeObject(entity);
                            orderDtLog.UserID = AppSession.UserLogin.UserID;
                            orderLogDao.Insert(orderDtLog);

                            PatientChargesDt entityChargesDt = lstEntityChargesDt.Where(t => t.PrescriptionOrderDetailID == entity.PrescriptionOrderDetailID).First();
                            entityChargesDt.PrescriptionOrderDetailID = null;
                            entityChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            entityChargesDt.IsDeleted = true;
                            entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityChargesDtDao.Update(entityChargesDt);
                        }

                        int parentID = 0;
                        int ct = 0;
                        for (int a = 0; a < lstPOUpdated.Count; a++)
                        {
                            if (ct == 0)
                            {
                                lstPOUpdated[a].IsRFlag = true;

                                if (cboEmbalace.Value != null)
                                {
                                    if (cboEmbalace.Value.ToString() != "" && cboEmbalace.Value.ToString() != "0")
                                    {
                                        lstPOUpdated[a].EmbalaceID = Convert.ToInt32(cboEmbalace.Value);
                                        if (lstPOUpdated[a].TakenQty != 0)
                                        {
                                            lstPOUpdated[a].EmbalaceQty = Convert.ToDecimal(txtEmbalaceQty.Text);
                                        }
                                        else
                                        {
                                            lstPOUpdated[a].EmbalaceQty = 0;
                                        }
                                    }
                                    else
                                    {
                                        lstPOUpdated[a].EmbalaceID = null;
                                        lstPOUpdated[a].EmbalaceQty = 0;
                                    }
                                }
                                else
                                {
                                    lstPOUpdated[a].EmbalaceID = null;
                                    lstPOUpdated[a].EmbalaceQty = 0;
                                }
                            }
                            else
                            {
                                lstPOUpdated[a].ParentID = parentID;
                                lstPOUpdated[a].EmbalaceID = null;
                                lstPOUpdated[a].EmbalaceQty = 0;
                            }

                            PatientChargesDt entityPC = lstEntityChargesDt.Where(t => t.TransactionID == transactionID && t.PrescriptionOrderDetailID == lstPOUpdated[a].PrescriptionOrderDetailID).First();
                            Variable temp = lstGCStockDeduction.Where(t => t.Code == lstPOUpdated[a].ItemID.ToString()).First();
                            ControlToEntity2(entityPC, lstPOUpdated[a], temp.Value, lstGCItemUnitUpdated[a], ctx);
                            if (ct == 0)
                            {
                                parentID = Convert.ToInt32(entityPC.PrescriptionOrderDetailID);
                            }
                            entityPC.PrescriptionOrderDetailID = lstPOUpdated[a].PrescriptionOrderDetailID;
                            entityPC.TransactionID = transactionID;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityChargesDtDao.Update(entityPC);
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityDtDao.Update(lstPOUpdated[a]);
                            ct++;
                        }

                        for (int b = 0; b < lstPOInserted.Count; b++)
                        {
                            if (ct == 0)
                            {
                                lstPOInserted[b].IsRFlag = true;

                                if (cboEmbalace.Value != null)
                                {
                                    if (cboEmbalace.Value.ToString() != "" && cboEmbalace.Value.ToString() != "0")
                                    {
                                        lstPOInserted[b].EmbalaceID = Convert.ToInt32(cboEmbalace.Value);
                                        if (lstPOInserted[b].TakenQty != 0)
                                        {
                                            lstPOInserted[b].EmbalaceQty = Convert.ToDecimal(txtEmbalaceQty.Text);
                                        }
                                        else
                                        {
                                            lstPOInserted[b].EmbalaceQty = 0;
                                        }
                                    }
                                    else
                                    {
                                        lstPOInserted[b].EmbalaceID = null;
                                        lstPOInserted[b].EmbalaceQty = 0;
                                    }
                                }
                                else
                                {
                                    lstPOInserted[b].EmbalaceID = null;
                                    lstPOInserted[b].EmbalaceQty = 0;
                                }
                            }
                            else
                            {
                                lstPOInserted[b].ParentID = parentID;
                                lstPOInserted[b].EmbalaceID = null;
                                lstPOInserted[b].EmbalaceQty = 0;
                            }

                            lstPOInserted[b].PrescriptionOrderID = prescriptionID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            int prescriptionDtID = entityDtDao.InsertReturnPrimaryKeyID(lstPOInserted[b]);
                            if (ct == 0)
                            {
                                parentID = prescriptionDtID;
                            }
                            PatientChargesDt entityPC = new PatientChargesDt();
                            Variable temp = lstGCStockDeduction.Where(t => t.Code == lstPOInserted[b].ItemID.ToString()).First();
                            ControlToEntity2(entityPC, lstPOInserted[b], temp.Value, lstGCItemUnitInserted[b], ctx);
                            entityPC.PrescriptionOrderDetailID = prescriptionDtID;
                            entityPC.TransactionID = transactionID;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityChargesDtDao.Insert(entityPC);
                            ct++;
                        }
                    }
                    retval = transactionID.ToString();
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = string.Format("<strong>{0} ({1})</strong><br/><br/><i>{2}</i>", ex.Message, ex.Source, ex.StackTrace);
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
}