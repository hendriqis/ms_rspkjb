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

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrescriptionEntryQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        private string[] lstDosingUnitValue = null;
        private string[] lstDosingUnitText = null;

        public string GetItemUnitListCode()
        {
            return string.Join(",", lstDosingUnitValue);
        }
        public string GetItemUnitListText()
        {
            return string.Join(",", lstDosingUnitText);
        }

        private PrescriptionEntryDetail DetailPage
        {
            get { return (PrescriptionEntryDetail)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnTransactionID.Value = temp[0];
            hdnLocationID.Value = temp[1];
            hdnDefaultGCMedicationRoute.Value = temp[2];
            hdnParamedicID.Value = temp[3];
            hdnRegistrationID.Value = temp[4];
            hdnVisitID.Value = temp[5];
            hdnChargeClassID.Value = temp[6];
            hdnIsDrugChargesJustDistributionQP.Value = temp[8];
            hdnBusinessPartnerIDCtl.Value = temp[9];

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",
                                                                                            AppSession.UserLogin.HealthcareID, //0
                                                                                            Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT, //1
                                                                                            Constant.SettingParameter.PH_DEFAULT_EMBALACE_CODE_PRESCRIPTION, //2
                                                                                            Constant.SettingParameter.PH_AUTO_INSERT_EMBALACE_PRESCRIPTION, //3
                                                                                            Constant.SettingParameter.PH_CREATE_QUEUE_LABEL, //4
                                                                                            Constant.SettingParameter.PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX, //5
                                                                                            Constant.SettingParameter.PH0083, //6
                                                                                            Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN, //7
                                                                                            Constant.SettingParameter.FN_PENJAMIN_INHEALTH, //8
                                                                                            Constant.SettingParameter.FN_PEMBATASAN_CPOE_INHEALTH, //9
                                                                                            Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //10
                                                                                            Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1, //11
                                                                                            Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS //12
                                                                                        ));
            hdnPrescriptionFeeAmount.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT).ParameterValue;
            hdnDefaultEmbalaceIDCtl.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_DEFAULT_EMBALACE_CODE_PRESCRIPTION).ParameterValue;
            hdnIsAutoInsertEmbalaceCtl.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_AUTO_INSERT_EMBALACE_PRESCRIPTION).ParameterValue;
            hdnIsGenerateQueueLabel.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_CREATE_QUEUE_LABEL).ParameterValue;
            hdnItemQtyWithSpecialQueuePrefix.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX).ParameterValue;

            hdnIsEndingAmountRoundingTo100.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;

            txtNotesQP.Text = temp[7];

            SetControlProperties();

            string oParam1 = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.PH0083).FirstOrDefault().ParameterValue;
            bool isLimitedCPOEItemForBPJS = oParam1 != null ? (oParam1 == "1" ? true : false) : false;

            string oParam2 = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault().ParameterValue;
            int businnessPartnerID1 = oParam2 != null ? Convert.ToInt32(oParam2) : 0;

            string oParam3 = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_TIPE_CUSTOMER_BPJS).FirstOrDefault().ParameterValue;
            string bpjsType = oParam3 != null ? oParam3 : string.Empty;

            bool isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealth;

            if (!isLimitedCPOEItemForInhealth)
            {
                switch (AppSession.RegisteredPatient.DepartmentID)
                {
                    case Constant.Facility.INPATIENT:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthIP;
                        break;
                    case Constant.Facility.OUTPATIENT:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthOP;
                        break;
                    case Constant.Facility.EMERGENCY:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthER;
                        break;
                    case Constant.Facility.DIAGNOSTIC:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthMD;
                        break;
                    case Constant.Facility.PHARMACY:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthPH;
                        break;
                    case Constant.Facility.MEDICAL_CHECKUP:
                        isLimitedCPOEItemForInhealth = AppSession.IsLimitedCPOEItemForInhealthMC;
                        break;
                    default:
                        break;
                } 
            }

            string oParam4 = lstSettingParameterDt.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_INHEALTH).FirstOrDefault().ParameterValue;
            string inHealthCustomerType = oParam4 != null ? oParam4 : string.Empty;

            if (AppSession.RegisteredPatient.GCCustomerType == bpjsType)
            {
                rblItemType.SelectedValue = "2";
                rblItemType.Enabled = !isLimitedCPOEItemForBPJS;
            }

            if (AppSession.RegisteredPatient.GCCustomerType == inHealthCustomerType)
            {
                rblItemType.SelectedValue = "3";
                rblItemType.Enabled = !isLimitedCPOEItemForInhealth;
            }

            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}')", Constant.StandardCode.ITEM_UNIT);
            lstDosingUnitValue = null;
            lstDosingUnitText = null;
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            List<StandardCode> lstItemUnit = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && (sc.TagProperty.Contains("PRE") || sc.TagProperty.Contains("1"))).ToList();
            lstDosingUnitValue = lstItemUnit.Select(lst => lst.StandardCodeID).ToArray();
            lstDosingUnitText = lstItemUnit.Select(lst => lst.StandardCodeName).ToArray();

            BindCboLocation();
        }

        private void BindCboLocation()
        {
            if (Convert.ToInt32(hdnLocationID.Value) > 0)
            {
                Location loc = BusinessLayer.GetLocation(Convert.ToInt32(hdnLocationID.Value));
                List<Location> lstLocation = null;
                if (loc.IsHeader)
                    lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                else
                {
                    lstLocation = new List<Location>();
                    lstLocation.Add(loc);
                }
                Methods.SetComboBoxField<Location>(cboPopupLocation, lstLocation, "LocationName", "LocationID");
                cboPopupLocation.SelectedIndex = 0;
            }
        }

        protected void cboPopupLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
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

            if (hdnItemGroupDrugLogisticID.Value == "")
            {
                filterExpression += string.Format("LocationID = '{0}' AND GCItemType IN ('{2}','{3}') AND IsDeleted = 0 AND (ItemName1 LIKE '%{1}%' OR GenericName LIKE '%{1}%') AND IsDeleted = 0", cboPopupLocation.Value, hdnFilterItem.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);
            }
            else
            {
                filterExpression += string.Format("LocationID = '{0}' AND GCItemType IN ('{3}','{4}') AND IsDeleted = 0 AND (ItemName1 LIKE '%{1}%' OR GenericName LIKE '%{1}%') AND ItemGroupID IN(SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{2}/%') AND IsDeleted = 0", cboPopupLocation.Value, hdnFilterItem.Value, hdnItemGroupDrugLogisticID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);
            }

            filterExpression += string.Format(" AND GCItemStatus != '{0}'", Constant.ItemStatus.IN_ACTIVE);

            if (hdnIsDrugChargesJustDistributionQP.Value == "1")
            {
                filterExpression += string.Format(" AND GCItemRequestType = '{0}'", Constant.ItemRequestType.DISTRIBUTION);
            }

            switch (rblItemType.SelectedValue)
            {
                case "1":
                    filterExpression += " AND IsFormularium = 1 ";
                    break;
                case "2":
                    filterExpression += " AND IsBPJSFormularium = 1 ";
                    break;
                case "3":
                    filterExpression += " AND IsInhealthFormularium = 1 ";
                    break;
                case "4":
                    filterExpression += " AND IsDefault = 1";
                    break;
                default:
                    break;
            }

            filterExpression += string.Format(" AND GCItemStatus = '{0}'", Constant.ItemStatus.ACTIVE);

            // RN (20210121) : ditutup utk issue NHS 202101210000002
            //if (hdnTransactionID.Value != "0" && hdnTransactionID.Value != "") {
            //    filterExpression += string.Format(" AND ItemID NOT IN (SELECT ItemID FROM PatientChargesDt WHERE TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}')", hdnTransactionID.Value, Constant.TransactionStatus.VOID);
            //}

            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemBalanceQuickPick1 entity = e.Row.DataItem as vItemBalanceQuickPick1;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                    chkIsSelected.Checked = true;
                System.Drawing.Color foreColor = System.Drawing.Color.Black;
                System.Drawing.Color backColor = System.Drawing.Color.White;

                if (entity.IsDefault)
                {
                    backColor = System.Drawing.Color.Yellow;
                    foreColor = System.Drawing.Color.Black;
                }

                if (entity.QuantityEND == 0)
                    foreColor = System.Drawing.Color.Red;

                e.Row.Cells[2].BackColor = backColor;
                e.Row.Cells[3].BackColor = backColor;
                e.Row.Cells[4].BackColor = backColor;
                e.Row.Cells[5].BackColor = backColor;
                e.Row.Cells[2].ForeColor = foreColor;
                e.Row.Cells[3].ForeColor = foreColor;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceQuickPick1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vItemBalanceQuickPick1> lstEntity = BusinessLayer.GetvItemBalanceQuickPick1List(filterExpression, 10, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PrescriptionOrderDtDao entityOrderDtDao = new PrescriptionOrderDtDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            ItemProductDao iProductDao = new ItemProductDao(ctx);
            try
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedMemberStrength = hdnSelectedMemberStrength.Value.Split(',');
                string[] lstSelectedMemberSigna = hdnSelectedMemberSigna.Value.Split(',');
                string[] lstSelectedMemberCoenam = hdnSelectedMemberCoenam.Value.Split(',');
                string[] lstSelectedMemberPRN = hdnSelectedMemberPRN.Value.Split(',');
                string[] lstSelectedMemberIMM = hdnSelectedMemberIMM.Value.Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Split(',');
                string[] lstSelectedMemberDosingUnit = hdnSelectedMemberDosingUnit.Value.Split(',');
                string[] lstSelectedMemberDispenseQty = hdnSelectedMemberDispenseQty.Value.Split(',');
                string[] lstSelectedMemberRemarks = hdnSelectedMemberRemarks.Value.Split('|');
                //string[] lstSelectedMemberStartTime = hdnSelectedMemberStartTime.Value.Split(',');
                string[] lstSelectedMemberRoute = hdnSelectedMemberRoute.Value.Split(',');
                string[] lstSelectedMemberDuration = hdnSelectedMemberDuration.Value.Split(',');

                int PrescriptionID = 0, PrescriptionOrderDtID = 0;
                int TransactionID = 0;
                String TransactionNo = "";
                DetailPage.SavePrescriptionHd(ctx, ref PrescriptionID, ref TransactionID, ref TransactionNo);

                PatientChargesHd patientChargesHd = patientChargesHdDao.Get(TransactionID);
                if (patientChargesHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<vDrugInfo1> lstDrugInfo = BusinessLayer.GetvDrugInfo1List(string.Format("ItemID IN ({0})", hdnSelectedMember.Value), ctx);
                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        vDrugInfo1 drugInfo = lstDrugInfo.FirstOrDefault(p => p.ItemID == Convert.ToInt32(itemID));

                        PrescriptionOrderDt entity = new PrescriptionOrderDt();
                        #region PrescriptionOrderDt
                        entity.IsRFlag = true;
                        entity.ItemID = Convert.ToInt32(itemID);
                        entity.DrugName = drugInfo.ItemName1;
                        entity.GenericName = drugInfo.GenericName;
                        if (drugInfo.GCDrugForm != null)
                        {
                            entity.GCDrugForm = drugInfo.GCDrugForm;
                        }
                        entity.SignaID = null;
                        entity.Dose = drugInfo.Dose;
                        if (!String.IsNullOrEmpty(drugInfo.GCDoseUnit))
                        {
                            entity.GCDoseUnit = drugInfo.GCDoseUnit;
                        }

                        bool isDay = true;
                        #region Default Frequency
                        string frequency = "1";
                        entity.GCDosingFrequency = Constant.DosingFrequency.DAY;
                        #endregion

                        bool isTwoDigit = (lstSelectedMemberSigna[ct].Substring(0, 2).Contains('q') || lstSelectedMemberSigna[ct].Substring(0, 2).Contains('d')) ? false : true;
                        if (isTwoDigit)
                            frequency = lstSelectedMemberSigna[ct].Substring(0, 2);
                        else
                            frequency = lstSelectedMemberSigna[ct].Substring(0, 1);

                        if (lstSelectedMemberSigna[ct].ToLower().Contains("qh"))
                        {
                            isDay = false;
                            entity.GCDosingFrequency = Constant.DosingFrequency.HOUR;
                        }
                        else
                        {
                            entity.GCDosingFrequency = Constant.DosingFrequency.DAY;
                        }
                        entity.Frequency = Convert.ToInt16(frequency);
                        entity.NumberOfDosage = Convert.ToDecimal(lstSelectedMemberQty[ct]);

                        entity.GCDosingUnit = lstSelectedMemberDosingUnit[ct];

                        if (lstSelectedMemberCoenam[ct] != "-")
                        {
                            switch (lstSelectedMemberCoenam[ct])
                            {
                                case "ac":
                                    entity.GCCoenamRule = Constant.CoenamRule.AC;
                                    break;
                                case "dc":
                                    entity.GCCoenamRule = Constant.CoenamRule.DC;
                                    break;
                                case "pc":
                                    entity.GCCoenamRule = Constant.CoenamRule.PC;
                                    break;
                                default:
                                    break;
                            }
                        }

                        entity.IsAsRequired = lstSelectedMemberPRN[ct] == "1";
                        entity.IsIMM = lstSelectedMemberIMM[ct] == "1";

                        entity.DispenseQty = Convert.ToDecimal(lstSelectedMemberDispenseQty[ct]);
                        entity.TakenQty = Convert.ToDecimal(lstSelectedMemberDispenseQty[ct]);
                        entity.ResultQty = Convert.ToDecimal(lstSelectedMemberDispenseQty[ct]);
                        if (drugInfo.GCStockDeductionType == Constant.QuantityDeductionType.DIBULATKAN) entity.TakenQty = Math.Ceiling(entity.TakenQty);
                        entity.ChargeQty = entity.TakenQty;

                        //decimal duration = 1;
                        //if (drugInfo.GCDoseUnit != null)
                        //{
                        //    if (isDay)
                        //    {
                        //        if (entity.GCDosingUnit == drugInfo.GCDoseUnit)
                        //        {
                        //            if (drugInfo.Dose != 0)
                        //            {
                        //                decimal numberOfDosage = entity.NumberOfDosage / drugInfo.Dose;
                        //                duration = Math.Ceiling((decimal)(entity.DispenseQty / (entity.Frequency * numberOfDosage)));
                        //            }
                        //            else
                        //            {
                        //                decimal numberOfDosage = entity.NumberOfDosage / 1;
                        //                duration = Math.Ceiling((decimal)(entity.DispenseQty / (entity.Frequency * numberOfDosage)));
                        //            }
                        //        }
                        //        else
                        //        {
                        //            duration = Math.Ceiling((decimal)(entity.DispenseQty / (entity.Frequency * entity.NumberOfDosage)));
                        //        }
                        //    }
                        //    else
                        //    {
                        //        decimal numberOfTaken = Math.Ceiling(Convert.ToDecimal(24 / entity.Frequency));
                        //        duration = Math.Ceiling((decimal)(entity.DispenseQty / (numberOfTaken * entity.NumberOfDosage)));
                        //    }

                        //    if (duration <= 999)
                        //    {
                        //        entity.DosingDuration = duration;
                        //    }
                        //    else
                        //    {
                        //        entity.DosingDuration = 999;
                        //    }
                        //}

                        entity.DosingDuration = Convert.ToDecimal(lstSelectedMemberDuration[ct]);

                        entity.GCRoute = lstSelectedMemberRoute[ct];
                        entity.StartDate = DateTime.Now;
                        entity.StartTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        entity.MedicationPurpose = drugInfo.MedicationPurpose;
                        entity.MedicationAdministration = lstSelectedMemberRemarks[ct];

                        if (!String.IsNullOrEmpty(hdnIsAutoInsertEmbalaceCtl.Value) && hdnIsAutoInsertEmbalaceCtl.Value == "1")
                        {
                            if (!String.IsNullOrEmpty(hdnDefaultEmbalaceIDCtl.Value) && hdnDefaultEmbalaceIDCtl.Value != "0")
                            {
                                entity.EmbalaceID = Convert.ToInt32(hdnDefaultEmbalaceIDCtl.Value);
                                entity.EmbalaceQty = 1;
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

                        entity.GCPrescriptionOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;

                        #endregion

                        #region PatientChargesDt
                        PatientChargesDt entityChargesDt = new PatientChargesDt();
                        entityChargesDt.LocationID = Convert.ToInt32(cboPopupLocation.Value);
                        entityChargesDt.ItemID = (int)entity.ItemID;
                        entityChargesDt.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                        entityChargesDt.ChargeClassID = Convert.ToInt32(hdnChargeClassID.Value);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityChargesDt.ChargeClassID, entityChargesDt.ItemID, 2, DateTime.Now, ctx);

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

                        entityChargesDt.ChargedQuantity = entity.ChargeQty;
                        decimal qtyStock = entity.DispenseQty;
                        if (drugInfo.GCConsumptionDeductionType == Constant.QuantityDeductionType.DIBULATKAN)
                        {
                            qtyStock = Math.Ceiling(qtyStock);
                        }
                        entityChargesDt.BaseQuantity = entityChargesDt.UsedQuantity = qtyStock;
                        entityChargesDt.GCBaseUnit = entityChargesDt.GCItemUnit = drugInfo.GCItemUnit;
                        entityChargesDt.ConversionFactor = 1;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityChargesDt.ItemID), ctx).FirstOrDefault();
                        entityChargesDt.AveragePrice = iPlanning.AveragePrice;
                        entityChargesDt.CostAmount = iPlanning.UnitPrice;

                        if (entityChargesDt.ItemID != null && entityChargesDt.ItemID != 0)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            ItemProduct iProduct = iProductDao.Get(entityChargesDt.ItemID);
                            entityChargesDt.HETAmount = iProduct.HETAmount;
                        }

                        if (entityChargesDt.ChargedQuantity > 0)
                        {
                            if (entity.EmbalaceID != null)
                            {
                                EmbalaceDt embalace = BusinessLayer.GetEmbalaceDtList(string.Format("EmbalaceID = {0} AND StartingQty <= {1} AND EndingQty >= {1}", entity.EmbalaceID, entity.EmbalaceQty)).FirstOrDefault();

                                decimal tariff = 0;

                                if (embalace != null)
                                {
                                    tariff = embalace.Tariff;
                                }

                                entityChargesDt.EmbalaceAmount = tariff * entity.EmbalaceQty;
                            }
                            else
                            {
                                entityChargesDt.EmbalaceAmount = 0;
                            }

                            entityChargesDt.PrescriptionFeeAmount = Convert.ToDecimal(hdnPrescriptionFeeAmount.Value);
                        }
                        else
                        {
                            entityChargesDt.EmbalaceAmount = 0;
                            entityChargesDt.PrescriptionFeeAmount = 0;
                        }

                        decimal qty = entityChargesDt.ChargedQuantity;
                        decimal grossLineAmount = (qty * price) + entityChargesDt.EmbalaceAmount + entityChargesDt.PrescriptionFeeAmount;

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

                        entity.PrescriptionOrderID = PrescriptionID;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        PrescriptionOrderDtID = entityOrderDtDao.InsertReturnPrimaryKeyID(entity);

                        entityChargesDt.PrescriptionOrderDetailID = PrescriptionOrderDtID;
                        entityChargesDt.TransactionID = TransactionID;
                        entityChargesDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityChargesDtDao.Insert(entityChargesDt);

                        ct++;
                    }

                    bool isSpecialTrx = false;
                    if (hdnItemQtyWithSpecialQueuePrefix.Value != "" && hdnItemQtyWithSpecialQueuePrefix.Value != "0")
                    {
                        if (ct > Convert.ToInt32(hdnItemQtyWithSpecialQueuePrefix.Value))
                        {
                            isSpecialTrx = true;
                        }
                    }
                    if (hdnIsGenerateQueueLabel.Value == "1" && string.IsNullOrEmpty(patientChargesHd.QueueNoLabel))
                    {
                        patientChargesHd.QueueNoLabel = BusinessLayer.GenerateChargesQueueNoLabel(patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, isSpecialTrx, ctx);
                        patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        patientChargesHdDao.Update(patientChargesHd);
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi " + patientChargesHd.TransactionNo + " Sudah Diproses. Tidak Bisa Ditambahkan Item";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                }
                retval = TransactionID.ToString();
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