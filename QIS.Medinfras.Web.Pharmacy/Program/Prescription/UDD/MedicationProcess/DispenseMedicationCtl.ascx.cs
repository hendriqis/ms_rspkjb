using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class DispenseMedicationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnDispensaryServiceUnitID.Value = paramInfo[0];
            hdnPrescriptionFeeAmount.Value = paramInfo[1];
            txtMedicationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            hdnDateToday.Value = DateTime.Now.AddDays(1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            SetControlProperties();

            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                                                            AppSession.UserLogin.HealthcareID, //0
                                                            Constant.SettingParameter.FM_UDD_ROUNDING_SYSTEM, //1
                                                            Constant.SettingParameter.MAX_DAY_DISPENSE_UDD, //2
                                                            Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //3
                                                            Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1 //4
                                                   );
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(filterExpression);

            hdnUDDRoundingSystem.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FM_UDD_ROUNDING_SYSTEM).FirstOrDefault().ParameterValue;
            hdnMaxDispenseDay.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.MAX_DAY_DISPENSE_UDD).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo100.Value = lstParam.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstParam.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;

            hdnDateToday.Value = DateTime.Now.AddDays(Convert.ToInt32(hdnMaxDispenseDay.Value)).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected void cbpPopupProcessDispense_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string lstRecordID = hdnSelectedID.Value;
            string lstScheduleID = hdnSelectedScheduleID.Value;
            string lstTime = hdnSelectedTime.Value;
            string lstTimeTransaction = hdnSelectedTimeTransaction.Value;
            string lstMultiDose = hdnSelectedMultiDose.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            //result = PrintUDDDrugLabel(lstRecordID);
            string validationErrMsg = string.Empty;
            string validationTransactionErrMsg = string.Empty;
            bool isError = false;
            string printResult = "";
            string oResult = "";

            if (IsValid(lstTime, ref validationErrMsg))
            {
                if (IsValidTransaction(lstTimeTransaction, ref validationTransactionErrMsg))
                {
                    result = ProcessMedicationSchedule(lstRecordID, lstScheduleID, lstTime, lstMultiDose, ref isError, ref oResult);

                    if (!isError)
                    {
                        if (chkIsAutoPrintReceipt.Checked)
                            printResult = PrintUDDDrugLabel(lstScheduleID);
                        else
                        {
                            if (chkIsPrintDrugLabel.Checked)
                            {
                                printResult = PrintUDDDrugLabel(lstScheduleID);
                            }
                        }

                        if (!string.IsNullOrEmpty(printResult))
                        {
                            string[] printResultInfo = printResult.Split('|');
                            if (printResultInfo[1] == "0")
                            {
                                result = string.Format("{0}|{1}", result, printResultInfo[2]);
                            }
                        }
                    }
                    else
                    {
                        result = oResult;
                    }
                }
                else 
                {
                    result = string.Format("process|0|{0}||", validationTransactionErrMsg);
                }
            }
            
            else
            {
                result = string.Format("process|0|{0}||", validationErrMsg);
            }
            
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }


        private bool IsValid(string lstTime, ref string validationErrMsg)
        {
            // RN (patch 202110-04) : ini ditutup karna sudah dijaga di depan
            //int maxDayDispense = Convert.ToInt32(hdnMaxDispenseDay.Value);
            //int addDays = maxDayDispense - 3;

            //if (Helper.GetDatePickerValue(txtTransactionDate) <= Helper.GetDatePickerValue(txtMedicationDate).AddDays(addDays))
            //{
            //    validationErrMsg = string.Format("Tanggal pemberian hanya diperbolehkan maksimal H+{0} dari tanggal transaksi.", hdnMaxDispenseDay.Value);
            //    return false;
            //}

            string[] lstMedicationTime = lstTime.Split(',');
            foreach (string time in lstMedicationTime)
            {
                if (string.IsNullOrEmpty(time) || time == "__:__")
                {
                    validationErrMsg = "Medication Time must be entried";
                    return false;
                }
                else
                {
                    Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                    if (!reg.IsMatch(time))
                    {
                        validationErrMsg = "Medication time must be entried in correct format (hh:mm)";
                        return false;
                    }
                }
            }
            return (validationErrMsg == string.Empty);
        }

        private bool IsValidTransaction(string lstTimeTransaction, ref string validationErrMsg)
        {

            string[] lstTransactionTime = lstTimeTransaction.Split(',');
            foreach (string time in lstTransactionTime)
            {
                if (string.IsNullOrEmpty(time) || time == "__:__")
                {
                    validationErrMsg = "Transaction Time must be entried";
                    return false;
                }
                else
                {
                    Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                    if (!reg.IsMatch(time))
                    {
                        validationErrMsg = "Transaction time must be entried in correct format (hh:mm)";
                        return false;
                    }
                }
            }
            return (validationErrMsg == string.Empty);
        }


        private bool OnBeforeProcessMedicationSchedule(DateTime physicianDischargedDate)
        {
            if (physicianDischargedDate != null && physicianDischargedDate.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
            {
                DateTime medicationDate = Helper.GetDatePickerValue(txtMedicationDate.Text);

                if (medicationDate > physicianDischargedDate)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        private string ProcessMedicationSchedule(string lstRecordID, string lstScheduleID, string lstTime, string lstMultiDose, ref bool isError, ref string result)
        {
            IDbContext ctx = DbFactory.Configure(true);

            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            PatientChargesHdDao chargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
            ConsultVisitDao cvDao = new ConsultVisitDao(ctx);
            ItemProductDao iProductDao = new ItemProductDao(ctx);

            ConsultVisit entityVisit = cvDao.Get(AppSession.RegisteredPatient.VisitID);
            DateTime physicianDischargedDate;
            if (entityVisit.PhysicianDischargedDate != null && entityVisit.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
            {
                physicianDischargedDate = entityVisit.PhysicianDischargedDate;
            }
            else
            {
                physicianDischargedDate = Helper.GetDatePickerValue(Constant.ConstantDate.DEFAULT_NULL);
            }

            int locationID = Convert.ToInt16(cboLocation.Value);
            DateTime transactionDate = Helper.GetDatePickerValue(txtTransactionDate.Text);
            DateTime medicationDate = Helper.GetDatePickerValue(txtMedicationDate.Text);
            string transactionTime = txtTransactionTime.Text;

            try
            {
                string validationErrMsg = string.Empty;
                bool isAllowProcess = OnBeforeProcessMedicationSchedule(physicianDischargedDate);

                string transactionNo = string.Empty;
                string referenceNo = string.Empty;

                if (isAllowProcess)
                {
                    //Generate Process Reference  Number
                    referenceNo = GenerateReferenceNumber();

                    string filterExpression = string.Empty;
                    if (lstRecordID != "")
                        filterExpression = String.Format("PrescriptionOrderDetailID IN ({0}) OR ParentID IN ({0})", lstRecordID);

                    List<vPrescriptionOrderDt4> lstEntityDt = BusinessLayer.GetvPrescriptionOrderDt4List(filterExpression, ctx);

                    #region PatientChargesHd
                    PatientChargesHd chargesHd = new PatientChargesHd();

                    chargesHd.VisitID = AppSession.RegisteredPatient.VisitID;
                    chargesHd.TransactionDate = transactionDate;
                    chargesHd.TransactionTime = transactionTime;
                    chargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    chargesHd.PrescriptionOrderID = null; //multiple prescription order id
                    chargesHd.HealthcareServiceUnitID = Convert.ToInt16(hdnDispensaryServiceUnitID.Value);
                    chargesHd.CreatedBy = AppSession.UserLogin.UserID;
                    chargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_INPATIENT;
                    chargesHd.ReferenceNo = referenceNo;
                    #endregion

                    List<PatientChargesDt> lstChargesDt = new List<PatientChargesDt>();
                    List<PrescriptionOrderDt> lstOrderDt = new List<PrescriptionOrderDt>();
                    foreach (vPrescriptionOrderDt4 orderDt in lstEntityDt)
                    {
                        PatientChargesDt chargesDt = new PatientChargesDt();

                        #region PatientChargesDt
                        chargesDt.PrescriptionOrderDetailID = orderDt.PrescriptionOrderDetailID;
                        chargesDt.ItemID = (int)orderDt.ItemID;
                        chargesDt.LocationID = locationID;
                        chargesDt.ChargeClassID = AppSession.RegisteredPatient.ChargeClassID;

                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID, chargesDt.ChargeClassID, (int)orderDt.ItemID, 2, transactionDate, ctx);


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

                        chargesDt.BaseTariff = basePrice;
                        chargesDt.Tariff = price;
                        chargesDt.BaseComp1 = basePriceComp1;
                        chargesDt.BaseComp2 = basePriceComp2;
                        chargesDt.BaseComp3 = basePriceComp3;
                        chargesDt.TariffComp1 = priceComp1;
                        chargesDt.TariffComp2 = priceComp2;
                        chargesDt.TariffComp3 = priceComp3;
                        chargesDt.CostAmount = costAmount;

                        //chargesDt.DiscountAmount = discountAmount;
                        //if (chargesDt.ChargedQuantity > 0)
                        //{
                        //    chargesDt.DiscountComp1 = chargesDt.DiscountAmount / chargesDt.ChargedQuantity;
                        //}

                        chargesDt.GCBaseUnit = orderDt.GCItemUnit;
                        chargesDt.GCItemUnit = orderDt.GCDosingUnit;
                        chargesDt.ParamedicID = Convert.ToInt32(orderDt.ParamedicID);

                        chargesDt.IsVariable = false;
                        chargesDt.IsUnbilledItem = false;

                        #region Calculate based on schedule-dosing quantity
                        decimal dosageQty = orderDt.NumberOfDosage;
                        decimal compoundQty = orderDt.CompoundQty;
                        decimal resultQty = 0;
                        decimal chargeQty = 0;
                        decimal baseQty = 0;

                        if (compoundQty > 0)
                        {
                            resultQty = dosageQty * compoundQty;
                            chargeQty = dosageQty * compoundQty;
                        }
                        else
                        {
                            resultQty = dosageQty;
                            chargeQty = dosageQty;
                        }

                        if (hdnUDDRoundingSystem.Value == "1")
                        {
                            if (orderDt.GCConsumptionDeductionType == Constant.InventoryRoundingStatus.ROUND_UP)
                                resultQty = Math.Ceiling(resultQty);
                        }

                        if (hdnUDDRoundingSystem.Value == "1")
                        {
                            if (orderDt.GCStockDeductionType == Constant.InventoryRoundingStatus.ROUND_UP)
                                chargeQty = Math.Ceiling(chargeQty);
                        }

                        baseQty = resultQty;

                        chargesDt.UsedQuantity = resultQty;
                        chargesDt.BaseQuantity = baseQty;
                        chargesDt.ChargedQuantity = chargeQty;
                        #endregion

                        chargesDt.PrescriptionFeeAmount = Convert.ToDecimal(hdnPrescriptionFeeAmount.Value);

                        if (orderDt.EmbalaceID != 0)
                        {
                            EmbalaceDt embalace = BusinessLayer.GetEmbalaceDtList(string.Format("EmbalaceID = {0} AND StartingQty <= {1} AND EndingQty >= {1}", orderDt.EmbalaceID, orderDt.EmbalaceQty)).FirstOrDefault();
                            chargesDt.EmbalaceAmount = Convert.ToDecimal(embalace.Tariff * orderDt.EmbalaceQty);
                        }
                        else
                        {
                            chargesDt.EmbalaceAmount = 0;
                        }

                        decimal grossLineAmount = (chargesDt.ChargedQuantity * price) + chargesDt.EmbalaceAmount + chargesDt.PrescriptionFeeAmount;

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
                                        chargesDt.DiscountPercentageComp1 = discountAmountComp1;
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
                                        chargesDt.DiscountPercentageComp2 = discountAmountComp2;
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
                                        chargesDt.DiscountPercentageComp3 = discountAmountComp3;
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
                                    chargesDt.DiscountPercentageComp1 = discountAmount;
                                }

                                if (priceComp2 > 0)
                                {
                                    totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                    chargesDt.DiscountPercentageComp2 = discountAmount;
                                }

                                if (priceComp3 > 0)
                                {
                                    totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                    chargesDt.DiscountPercentageComp3 = discountAmount;
                                }
                            }

                            if (chargesDt.DiscountPercentageComp1 > 0)
                            {
                                chargesDt.IsDiscountInPercentageComp1 = true;
                            }

                            if (chargesDt.DiscountPercentageComp2 > 0)
                            {
                                chargesDt.IsDiscountInPercentageComp2 = true;
                            }

                            if (chargesDt.DiscountPercentageComp3 > 0)
                            {
                                chargesDt.IsDiscountInPercentageComp3 = true;
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

                        totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (chargesDt.ChargedQuantity);

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

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, chargesDt.ItemID), ctx).FirstOrDefault();
                        chargesDt.AveragePrice = iPlanning.AveragePrice;
                        chargesDt.CostAmount = iPlanning.UnitPrice;

                        if (chargesDt.ItemID != null && chargesDt.ItemID != 0)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            ItemProduct iProduct = iProductDao.Get(chargesDt.ItemID);
                            chargesDt.HETAmount = iProduct.HETAmount;
                        }

                        chargesDt.ConversionFactor = orderDt.ConversionFactor;
                        chargesDt.IsCITO = false;
                        chargesDt.CITOAmount = 0;
                        chargesDt.IsComplication = false;
                        chargesDt.ComplicationAmount = 0;

                        chargesDt.IsDiscount = totalDiscountAmount != 0 ? true : false;
                        chargesDt.DiscountAmount = totalDiscountAmount;
                        chargesDt.DiscountComp1 = totalDiscountAmount1;
                        chargesDt.DiscountComp2 = totalDiscountAmount2;
                        chargesDt.DiscountComp3 = totalDiscountAmount3;

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

                        chargesDt.PatientAmount = oPatientAmount;
                        chargesDt.PayerAmount = oPayerAmount;
                        chargesDt.LineAmount = oLineAmount;

                        chargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                        chargesDt.CreatedBy = chargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;

                        lstChargesDt.Add(chargesDt);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        PrescriptionOrderDt entityOrderDt = orderDtDao.Get(Convert.ToInt32(orderDt.PrescriptionOrderDetailID));
                        if (entityOrderDt.IsRFlag)
                        {
                            decimal takenQty = orderDt.TakenQty + orderDt.NumberOfDosage;
                            entityOrderDt.TakenQty = takenQty;
                            entityOrderDt.ChargeQty = takenQty;
                            entityOrderDt.ResultQty = takenQty;
                            lstOrderDt.Add(entityOrderDt);
                        }
                        #endregion
                    }

                    #region Commit to Database
                    chargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(chargesHd.TransactionCode, chargesHd.TransactionDate, ctx);
                    chargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL; //Proposed
                    chargesHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    int transactionID = chargesHdDao.InsertReturnPrimaryKeyID(chargesHd);
                    transactionNo = chargesHd.TransactionNo;
                    hdnTransactionNo.Value = transactionNo;
                    hdnTransactionDate.Value = transactionDate.ToString(Constant.FormatString.DATE_FORMAT);

                    foreach (PatientChargesDt item in lstChargesDt)
                    {
                        item.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL; //Proposed
                        item.TransactionID = transactionID;
                        item.IsApproved = true;
                        item.CreatedBy = AppSession.UserLogin.UserID;
                        chargesDtDao.Insert(item);
                    }

                    filterExpression = string.Format("ID in ({0}) ORDER BY ID", lstScheduleID);
                    List<MedicationSchedule> lstMedication = BusinessLayer.GetMedicationScheduleList(filterExpression, ctx);

                    int counter = 0;
                    string[] time = lstTime.Split(',');
                    string[] multiDose = lstMultiDose.Split(',');
                    foreach (var medication in lstMedication)
                    {
                        medication.ReferenceNo = referenceNo;
                        medication.TransactionID = transactionID;
                        medication.MedicationTime = time[counter];
                        medication.GCMedicationStatus = Constant.MedicationStatus.DIPROSES_FARMASI;
                        medication.LastUpdatedBy = AppSession.UserLogin.UserID;
                        scheduleDao.Update(medication);

                        if (multiDose[counter] == "1")
                        {
                            int itemID = (int)medication.ItemID;
                            filterExpression = string.Format("ItemID = {0} AND MedicationDate = '{1}' AND SequenceNo <> '{2}'", itemID.ToString(), Helper.GetDatePickerValue(txtMedicationDate).ToString(Constant.FormatString.DATE_FORMAT_112), cboSequence.Value.ToString());
                            List<MedicationSchedule> lstSchedule = BusinessLayer.GetMedicationScheduleList(filterExpression, ctx);
                            foreach (MedicationSchedule oSchedule in lstSchedule)
                            {
                                oSchedule.ReferenceNo = referenceNo;
                                oSchedule.TransactionID = transactionID;
                                oSchedule.MedicationTime = oSchedule.MedicationTime;
                                oSchedule.GCMedicationStatus = Constant.MedicationStatus.DIPROSES_FARMASI;
                                oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                                scheduleDao.Update(oSchedule);
                            }
                        }
                        counter += 1;
                    }

                    foreach (PrescriptionOrderDt orderDt in lstOrderDt)
                    {
                        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        orderDtDao.Update(orderDt);
                    }
                    ctx.CommitTransaction();
                    #endregion

                    string message = string.Format("Transaction was created successfully with Transaction Number <b>{0}</b>", transactionNo);
                    result = string.Format("process|1|{0}|{1}|{2}", message, referenceNo, transactionNo);
                }
                else
                {
                    isError = true;
                    string message = string.Format("Proses dispense tidak bisa dilakukan di tanggal {0} karena pasien sudah diinstruksikan pulang oleh DPJP di tanggal {1}", medicationDate.ToString(Constant.FormatString.DATE_FORMAT), physicianDischargedDate.ToString(Constant.FormatString.DATE_FORMAT));
                    result = string.Format("process|0|{0}|{1}|{2}", message, referenceNo, transactionNo);
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                isError = true;
                result = string.Format("process|0|{0}||", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private string PrintUDDDrugLabel(string lstScheduleID)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                //string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                //    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET);

                //bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}', '{2}','{3}','{4}')",
                    AppSession.UserLogin.HealthcareID,
                    Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET,
                    Constant.SettingParameter.PH_UDD_MEDICATION_RECEIPT,
                    Constant.SettingParameter.SA_LOKASI_PRINTER_IP_ADDR,
                    Constant.SettingParameter.FM_FORMAT_CETAKAN_LABEL_UDD
                    );

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_FORMAT_CETAKAN_ETIKET)).FirstOrDefault().ParameterValue;
                string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.PH_UDD_MEDICATION_RECEIPT)).FirstOrDefault().ParameterValue;
                string SA_LOKASI_PRINTER_IP_ADDR = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.SA_LOKASI_PRINTER_IP_ADDR)).FirstOrDefault().ParameterValue;
                string FM_FORMAT_CETAKAN_LABEL_UDD = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_FORMAT_CETAKAN_LABEL_UDD)).FirstOrDefault().ParameterValue;
                bool isBasedOnIPAddress = Convert.ToBoolean(Convert.ToInt16(SA_LOKASI_PRINTER_IP_ADDR));
            
                if (isBasedOnIPAddress)
                {
                    //Get Printer Address
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}','{2}','{3}') AND IsDeleted=0",
                        ipAddress, Constant.DirectPrintType.LABEL_UDD, Constant.DirectPrintType.ETIKET_OBAT_DALAM, Constant.DirectPrintType.ETIKET_OBAT_LUAR);

                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                    if (lstPrinter.Count > 0)
                    {
                        string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_UDD).FirstOrDefault().PrinterName;
                        string formatPrint = FM_FORMAT_CETAKAN_LABEL_UDD;  /// AppSession.UDDDrugLabelType;

                        if (formatPrint == Constant.PrintFormat.BIXOLON_ETIKET_UDD_RSPKSB)
                        {
                            if (chkIsAutoPrintReceipt.Checked)
                            {
                                if (AppSession.UDDReceiptPrinterType == Constant.PrinterType.THERMAL_RECEIPT_PRINTER)
                                    ZebraPrinting.PrintUDDLabelWithTMPrinter(AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtMedicationDate.Text), cboSequence.Value.ToString(), lstScheduleID, hdnTransactionNo.Value, printerUrl1);
                                else
                                    ZebraPrinting.PrintUDDLabel(AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtMedicationDate.Text), cboSequence.Value.ToString(), lstScheduleID, printerUrl1);
                            }
                            if (chkIsPrintDrugLabel.Checked)
                            {
                                string printerUrl2 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_DALAM).FirstOrDefault().PrinterName;
                                string printerUrl3 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_LUAR).FirstOrDefault().PrinterName;
                                ZebraPrinting.PrintUDDDrugLabelBixolon(AppSession.RegisteredPatient.VisitID, hdnTransactionNo.Value, hdnTransactionDate.Value, formatPrint, lstScheduleID, printerUrl2, printerUrl3);
                            }
                        }
                        else
                        {
                            if (chkIsAutoPrintReceipt.Checked)
                            {
                                if (AppSession.UDDReceiptPrinterType == Constant.PrinterType.THERMAL_RECEIPT_PRINTER)
                                    ZebraPrinting.PrintUDDLabelWithTMPrinter(AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtMedicationDate.Text), cboSequence.Value.ToString(), lstScheduleID, hdnTransactionNo.Value, printerUrl1);
                                else
                                    ZebraPrinting.PrintUDDLabel(AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtMedicationDate.Text), cboSequence.Value.ToString(), lstScheduleID, printerUrl1);
                            }
                            if (chkIsPrintDrugLabel.Checked)
                            {
                                string printerUrl2 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_DALAM).FirstOrDefault().PrinterName;
                                string printerUrl3 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.ETIKET_OBAT_LUAR).FirstOrDefault().PrinterName;
                                ZebraPrinting.PrintUDDDrugLabel(AppSession.RegisteredPatient.VisitID, hdnTransactionNo.Value, hdnTransactionDate.Value, formatPrint, lstScheduleID, printerUrl2, printerUrl3);
                            }
                        }
                        result = string.Format("print|1|||");
                    }
                    else
                    {
                        string message = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                        result = string.Format("print|0|{0}||", message);
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Format("print|0|{0}||", ex.Message);
            }
            return result;
        }

        private string GenerateReferenceNumber()
        {
            string result = string.Empty;
            string part1 = AppSession.RegisteredPatient.MedicalNo.Replace("-", "");
            string part2 = string.Format("{0}-{1}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112), cboSequence.Text);
            string part3 = DateTime.Now.ToString("HH:mm:ss").Replace(":", "");
            //medicalno.yyyymmdd-sequenceno.hhmmss

            result = string.Format("{0}.{1}.{2}", part1, part2, part3);
            return result;
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstSC = new List<StandardCode>();
            for (int a = 1; a <= 6; a++)
            {
                StandardCode sc = new StandardCode()
                {
                    StandardCodeID = a.ToString(),
                    StandardCodeName = a.ToString()
                };
                lstSC.Add(sc);
            }
            Methods.SetComboBoxField<StandardCode>(cboSequence, lstSC, "StandardCodeName", "StandardCodeID");
            cboSequence.SelectedIndex = 0;

            string filterExpression = string.Format("IsInpatientDispensary = 1");
            List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.PHARMACY, filterExpression);
            Methods.SetComboBoxField<GetServiceUnitUserList>(cboDispensary, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboDispensary.Value = hdnDispensaryServiceUnitID.Value;

            BindCboLocation();

            chkIsAutoPrintReceipt.Enabled = AppSession.IsUsedUDDDrugLabel;
            if (!chkIsAutoPrintReceipt.Enabled)
                chkIsAutoPrintReceipt.Checked = false;
            else
                chkIsAutoPrintReceipt.Checked = true;

            BindGridView();
        }


        protected void cboLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            hdnDispensaryServiceUnitID.Value = cboDispensary.Value.ToString();
            BindCboLocation();
            hdnLocationID.Value = cboLocation.Value.ToString();
        }

        private void BindCboLocation()
        {
            if (cboDispensary.Value != null)
            {
                Location location = BusinessLayer.GetLocationList(string.Format("LocationID IN (SELECT LocationID FROM HealthcareServiceUnit WHERE HealthcareServiceUnitID = {0})", cboDispensary.Value)).FirstOrDefault();

                if (location != null)
                {
                    int locationID = location.LocationID;
                    Location loc = BusinessLayer.GetLocation(locationID);
                    List<Location> lstLocation = null;
                    if (loc.IsHeader)
                        lstLocation = BusinessLayer.GetLocationList(string.Format("ParentID = {0}", loc.LocationID));
                    else
                    {
                        lstLocation = new List<Location>();
                        lstLocation.Add(loc);
                    }
                    Methods.SetComboBoxField<Location>(cboLocation, lstLocation, "LocationName", "LocationID");
                    cboLocation.SelectedIndex = 0;
                }
            }
        }

        protected void lvwDispenseView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            //if (e.Item.ItemType == ListViewItemType.DataItem)
            //{
            //    CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsProcessItem");
            //    //HtmlTextInput chkIsSelected = (CheckBox)e.Item.FindControl("chkIsProcessItem");
            //    vMedicationSchedule item = (vMedicationSchedule)e.Item.DataItem;
            //    if (item.GCMedicationStatus != Constant.MedicationStatus.OPEN)
            //    {
            //        chkIsSelected.Visible = false;
            //    }
            //}
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitID = {0} AND MedicationDate = '{1}' AND GCMedicationStatus = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID,
                Helper.GetDatePickerValue(txtMedicationDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.MedicationStatus.OPEN);
            if (cboSequence.Value != null)
            {
                filterExpression += string.Format(" AND SequenceNo = '{0}'", cboSequence.Value);
            }
            filterExpression += " ORDER BY ID";
            List<vMedicationScheduleForDispensing> lstEntity = BusinessLayer.GetvMedicationScheduleForDispensingList(filterExpression);
            lvwDispenseView.DataSource = lstEntity;
            lvwDispenseView.DataBind();
        }

        protected void cbpDispenseView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected void cbpCalculateItem_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = e.Parameter;
            string result = param + "|";
            string retval = "";

            string errMessage = "";
            if (param == "calculate")
            {
                if (OnCalculateItem(ref errMessage, ref retval))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }

        private bool OnCalculateItem(ref string errMessage, ref string retval)
        {
            bool result = true;
            try
            {
                String filterExpression = "1 = 0";
                if (hdnSelectedID.Value != "")
                    filterExpression = String.Format("PrescriptionOrderDetailID IN ({0})", hdnSelectedID.Value);
                List<PrescriptionOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionOrderDtList(filterExpression);

                StringBuilder lstItemBalanceInfo = new StringBuilder();

                foreach (PrescriptionOrderDt objDt in lstEntityDt)
                {
                    #region Get Stock Information
                    string filterExp = string.Format("LocationID = {0} AND ItemID = {1}", Convert.ToInt32(cboLocation.Value), objDt.ItemID);
                    vItemBalanceQuickPick1 objBalanceInfo = BusinessLayer.GetvItemBalanceQuickPick1List(filterExp).FirstOrDefault();
                    if (objBalanceInfo != null)
                    {
                        string itemName = string.Format("<span style='font-weight:bold'>{0}</span> ", objBalanceInfo.ItemName1.TrimEnd());
                        string balanceInfo1 = string.Format(" ,Jumlah tersedia di Lokasi : <span style='font-weight:bold; color: blue'>{0}</span> ", objBalanceInfo.QuantityEND.ToString("G29"));
                        string balanceInfo2 = string.Format(" ,Jumlah seluruh RS : <span style='font-weight:bold; color: blue'>{0}</span> ", objBalanceInfo.TotalQtyOnHand.ToString("G29"));
                        string itemInfo = string.Format("{0}{1}{2}", itemName, balanceInfo1, balanceInfo2);
                        lstItemBalanceInfo.AppendLine(string.Format("{0}", itemInfo));
                    }
                    #endregion
                }
                retval = string.Format("{0}", lstItemBalanceInfo.ToString().Replace(Environment.NewLine, "<br/>"));
                hdnBalanceInfo.Value = lstItemBalanceInfo.ToString().Replace(Environment.NewLine, "<br/>");
                result = true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                retval = string.Format("{0}|{1}|{2}", 0, 0, 0);
                result = false;
            }
            return result;
        }


    }
}