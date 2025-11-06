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
using System.Text;
using System.Globalization;
using Newtonsoft.Json;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ExtendODDOrderDurationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnDispensaryServiceUnitID.Value = paramInfo[0];
            txtDefaultStartDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDefaultDuration.Text = "1";
            SetControlProperties();
        }

        private bool IsValidated(string lstDuration, string lstStartDate, string lstTime1, string lstTime2, string lstTime3, string lstTime4, string lstTime5, string lstTime6,ref string result)
        {
            string message = string.Empty;

            if (string.IsNullOrEmpty(message))
            {
                #region Validate Duration
                string[] selectedDuration = lstDuration.Split(',');
                foreach (string duration in selectedDuration)
                {
                    if (string.IsNullOrEmpty(duration))
                    {
                        message = "Ada obat dengan jumlah hari extend yang belum diisi";
                        break;
                    }
                    else
                    {
                        Decimal value;
                        if (!Decimal.TryParse(duration, out value))
                        {
                            message = string.Format("Ada obat dengan jumlah hari extend yang tidak sesuai ({0})", duration);
                            break;
                        }
                        else
                        {
                            if (value == 0)
                            {
                                message = string.Format("Ada obat dengan jumlah hari extend yang tidak sesuai ({0})", duration);
                                break;
                            }
                        }
                    }
                }
                #endregion

                #region Validate Start Date
                string[] selectedDate = lstStartDate.Split(',');
                foreach (string date in selectedDate)
                {
                    if (string.IsNullOrEmpty(date))
                    {
                        message = "Ada obat dengan tanggal mulai extend yang belum diisi";
                        break;
                    }
                    else
                    {
                        DateTime startDate;
                        string format = Constant.FormatString.DATE_PICKER_FORMAT;
                        try
                        {
                            startDate = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
                        }
                        catch (FormatException)
                        {
                            message = string.Format("Ada obat dengan tanggal mulai extend {0} yang tidak benar", date);
                            break;
                        }
                        DateTime sdate = Helper.GetDatePickerValue(date);
                        if (DateTime.Compare(sdate, Helper.GetDatePickerValue(txtDefaultStartDate)) == 0)
                        {
                            message = "Tanggal mulai extend harus lebih besar dari tanggal akhir pemberian saat ini.";
                            break;
                        }
                    }
                }
                #endregion

                #region Validate Medication Time
                string[] lstMedicationTime = lstTime1.Split(',');
                foreach (string time in lstMedicationTime)
                {
                    if (time != "-")
                    {
                        if (!string.IsNullOrEmpty(time) || time == "__:__")
                        {
                            Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                            if (!reg.IsMatch(time))
                            {
                                message = "Waktu pemberian obat sequence #1 harus sesuai dengan format jam (00:00 s/d 23:59)";
                                return false;
                            }
                        }
                    }
                }

                lstMedicationTime = lstTime2.Split(',');
                foreach (string time in lstMedicationTime)
                {
                    if (time != "-")
                    {
                        if (!string.IsNullOrEmpty(time) || time == "__:__")
                        {
                            Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                            if (!reg.IsMatch(time))
                            {
                                message = "Waktu pemberian obat sequence #2 harus sesuai dengan format jam (00:00 s/d 23:59)";
                                return false;
                            }
                        }
                    }
                }

                lstMedicationTime = lstTime3.Split(',');
                foreach (string time in lstMedicationTime)
                {
                    if (time != "-")
                    {
                        if (!string.IsNullOrEmpty(time) || time == "__:__")
                        {
                            Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                            if (!reg.IsMatch(time))
                            {
                                message = "Waktu pemberian obat sequence #3 harus sesuai dengan format jam (00:00 s/d 23:59)";
                                return false;
                            }
                        }
                    }
                }

                lstMedicationTime = lstTime4.Split(',');
                foreach (string time in lstMedicationTime)
                {
                    if (time != "-")
                    {
                        if (!string.IsNullOrEmpty(time) || time == "__:__")
                        {
                            Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                            if (!reg.IsMatch(time))
                            {
                                message = "Waktu pemberian obat sequence #4 harus sesuai dengan format jam (00:00 s/d 23:59)";
                                return false;
                            }
                        }
                    }
                }

                lstMedicationTime = lstTime5.Split(',');
                foreach (string time in lstMedicationTime)
                {
                    if (time != "-")
                    {
                        if (!string.IsNullOrEmpty(time) || time == "__:__")
                        {
                            Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                            if (!reg.IsMatch(time))
                            {
                                message = "Waktu pemberian obat sequence #5 harus sesuai dengan format jam (00:00 s/d 23:59)";
                                return false;
                            }
                        }
                    }
                }

                lstMedicationTime = lstTime6.Split(',');
                foreach (string time in lstMedicationTime)
                {
                    if (time != "-")
                    {
                        if (!string.IsNullOrEmpty(time) || time == "__:__")
                        {
                            Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                            if (!reg.IsMatch(time))
                            {
                                message = "Waktu pemberian obat sequence #6 harus sesuai dengan format jam (00:00 s/d 23:59)";
                                return false;
                            }
                        }
                    }
                }
                #endregion
            }

            if (!string.IsNullOrEmpty(message))
            {
                result = message;
            }
            return message == string.Empty;
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string lstRecordID = hdnSelectedID.Value;
            string lstStartDate = hdnSelectedStartDate.Value;
            string lstDuration = hdnSelectedDuration.Value;
            string lstSelectedTime1 = hdnSelectedTime1.Value;
            string lstSelectedTime2 = hdnSelectedTime2.Value;
            string lstSelectedTime3 = hdnSelectedTime3.Value;
            string lstSelectedTime4 = hdnSelectedTime4.Value;
            string lstSelectedTime5 = hdnSelectedTime5.Value;
            string lstSelectedTime6 = hdnSelectedTime6.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            string errorMessage = string.Empty;
            bool isError = false;
            if (!IsValidated(lstDuration,lstStartDate, lstSelectedTime1, lstSelectedTime2, lstSelectedTime3, lstSelectedTime4, lstSelectedTime5, lstSelectedTime6, ref errorMessage))
            {
                isError = true;
                result = string.Format("process|0|{0}", errorMessage);
            }

            if (!isError)
            {
                result = GenerateMedicationSchedule(lstRecordID, lstStartDate, lstDuration, lstSelectedTime1, lstSelectedTime2, lstSelectedTime3, lstSelectedTime4, lstSelectedTime5, lstSelectedTime6); 
            }

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        //private bool CreateCharges(IDbContext ctx, PrescriptionOrderHd orderHd, List<PrescriptionOrderDt> lstSelectedOrderDt, ref string errMessage)
        //{
        //    bool result = true;
        //    String filterExpression = "1 = 0";
        //    DateTime transactionDate = DateTime.Now.Date;
        //    string transactionNo = string.Empty;

        //    PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
        //    PatientChargesHdDao chargesHdDao = new PatientChargesHdDao(ctx);
        //    PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
        //    PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);

        //    #region PatientChargesHd
        //    PatientChargesHd chargesHd = new PatientChargesHd();

        //    chargesHd.VisitID = orderHd.VisitID;
        //    chargesHd.TransactionDate = DateTime.Now.Date;
        //    chargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        //    chargesHd.PrescriptionOrderID = orderHd.PrescriptionOrderID;
        //    chargesHd.HealthcareServiceUnitID = orderHd.DispensaryServiceUnitID;
        //    chargesHd.CreatedBy = AppSession.UserLogin.UserID;
        //    chargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_INPATIENT;
        //    #endregion

        //    List<PatientChargesDt> lstChargesDt = new List<PatientChargesDt>();
        //    List<PrescriptionOrderDt> lstOrderDt = new List<PrescriptionOrderDt>();

        //    foreach (PrescriptionOrderDt orderDt in lstSelectedOrderDt)
        //    {
        //        if (orderDt.GCPrescriptionOrderStatus == Constant.TestOrderStatus.CANCELLED || orderDt.GCPrescriptionOrderStatus == Constant.TestOrderStatus.IN_PROGRESS)
        //        {
        //            result = false;
        //            errMessage = "Order Sudah Di Proses Oleh User lain, Silahkan Merefresh halaman ini.";
        //            break;
        //        }

        //        PatientChargesDt chargesDt = new PatientChargesDt();

        //        filterExpression = string.Format("ItemID = {0}", orderDt.ItemID);
        //        vItemProduct item = BusinessLayer.GetvItemProductList(filterExpression, ctx).FirstOrDefault();

        //        if (orderDt == null)
        //        {
        //            result = false;
        //            errMessage = string.Format("Cannot found item product information for item {0}", orderDt.DrugName);
        //            break;
        //        }

        //        #region PatientChargesDt
        //        chargesDt.PrescriptionOrderDetailID = orderDt.PrescriptionOrderDetailID;
        //        chargesDt.ItemID = (int)orderDt.ItemID;
        //        chargesDt.LocationID = Convert.ToInt32(cboLocation.Value);
        //        chargesDt.ChargeClassID = AppSession.RegisteredPatient.ChargeClassID;

        //        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.VisitID, chargesDt.ChargeClassID, (int)orderDt.ItemID, 2, transactionDate, ctx);

        //        decimal basePrice = 0;
        //        decimal basePriceComp1 = 0;
        //        decimal basePriceComp2 = 0;
        //        decimal basePriceComp3 = 0;
        //        decimal price = 0;
        //        decimal priceComp1 = 0;
        //        decimal priceComp2 = 0;
        //        decimal priceComp3 = 0;
        //        bool isDiscountUsedComp = false;
        //        decimal discountAmount = 0;
        //        decimal discountAmountComp1 = 0;
        //        decimal discountAmountComp2 = 0;
        //        decimal discountAmountComp3 = 0;
        //        decimal coverageAmount = 0;
        //        bool isDiscountInPercentage = false;
        //        bool isDiscountInPercentageComp1 = false;
        //        bool isDiscountInPercentageComp2 = false;
        //        bool isDiscountInPercentageComp3 = false;
        //        bool isCoverageInPercentage = false;
        //        decimal costAmount = 0;

        //        if (list.Count > 0)
        //        {
        //            GetCurrentItemTariff obj = list[0];
        //            basePrice = obj.BasePrice;
        //            basePriceComp1 = obj.BasePriceComp1;
        //            basePriceComp2 = obj.BasePriceComp2;
        //            basePriceComp3 = obj.BasePriceComp3;
        //            price = obj.Price;
        //            priceComp1 = obj.PriceComp1;
        //            priceComp2 = obj.PriceComp2;
        //            priceComp3 = obj.PriceComp3;
        //            isDiscountUsedComp = obj.IsDiscountUsedComp;
        //            discountAmount = obj.DiscountAmount;
        //            discountAmountComp1 = obj.DiscountAmountComp1;
        //            discountAmountComp2 = obj.DiscountAmountComp2;
        //            discountAmountComp3 = obj.DiscountAmountComp3;
        //            coverageAmount = obj.CoverageAmount;
        //            isDiscountInPercentage = obj.IsDiscountInPercentage;
        //            isDiscountInPercentageComp1 = obj.IsDiscountInPercentageComp1;
        //            isDiscountInPercentageComp2 = obj.IsDiscountInPercentageComp2;
        //            isDiscountInPercentageComp3 = obj.IsDiscountInPercentageComp3;
        //            isCoverageInPercentage = obj.IsCoverageInPercentage;
        //            costAmount = obj.CostAmount;
        //        }

        //        chargesDt.BaseTariff = basePrice;
        //        chargesDt.Tariff = price;
        //        chargesDt.BaseComp1 = basePriceComp1;
        //        chargesDt.BaseComp2 = basePriceComp2;
        //        chargesDt.BaseComp3 = basePriceComp3;
        //        chargesDt.TariffComp1 = priceComp1;
        //        chargesDt.TariffComp2 = priceComp2;
        //        chargesDt.TariffComp3 = priceComp3;
        //        chargesDt.CostAmount = costAmount;

        //        chargesDt.GCBaseUnit = item.GCItemUnit;
        //        chargesDt.GCItemUnit = item.GCItemUnit;
        //        chargesDt.ParamedicID = Convert.ToInt32(orderHd.ParamedicID);

        //        chargesDt.IsVariable = false;
        //        chargesDt.IsUnbilledItem = false;

        //        #region dispense full since it is not UDD

        //        decimal dosageQty = orderDt.NumberOfDosage;
        //        decimal compoundQty = orderDt.CompoundQty;
        //        decimal resultQty = orderDt.ResultQty;
        //        decimal chargeQty = orderDt.ChargeQty;
        //        decimal baseQty = orderDt.ResultQty;

        //        if (item.GCConsumptionDeductionType == Constant.InventoryRoundingStatus.ROUND_UP)
        //            resultQty = Math.Ceiling(resultQty);

        //        if (item.GCStockDeductionType == Constant.InventoryRoundingStatus.ROUND_UP)
        //            chargeQty = Math.Ceiling(chargeQty);

        //        baseQty = resultQty;

        //        chargesDt.UsedQuantity = resultQty;
        //        chargesDt.BaseQuantity = baseQty;
        //        chargesDt.ChargedQuantity = chargeQty;

        //        if (chargesDt.ChargedQuantity > 0)
        //        {
        //            chargesDt.DiscountAmount = discountAmount;
        //            chargesDt.DiscountComp1 = chargesDt.DiscountAmount / chargesDt.ChargedQuantity;
        //            chargesDt.PrescriptionFeeAmount = Convert.ToDecimal(hdnPrescriptionFeeAmount.Value);

        //            if (orderDt.EmbalaceID != null && orderDt.IsRFlag)
        //            {
        //                EmbalaceHd embalaceHd = BusinessLayer.GetEmbalaceHdList(string.Format("EmbalaceID = {0}", orderDt.EmbalaceID)).FirstOrDefault();
        //                if (embalaceHd != null)
        //                {
        //                    if (embalaceHd.IsUsingRangePricing)
        //                    {
        //                        EmbalaceDt embalaceDt = BusinessLayer.GetEmbalaceDtList(string.Format("EmbalaceID = {0} AND StartingQty <= {1} AND EndingQty >= {1}", orderDt.EmbalaceID, orderDt.EmbalaceQty)).FirstOrDefault();
        //                        if (embalaceDt != null)
        //                            chargesDt.EmbalaceAmount = Convert.ToDecimal(embalaceDt.Tariff * orderDt.EmbalaceQty);
        //                        else
        //                            chargesDt.EmbalaceAmount = 0;
        //                    }
        //                    else
        //                    {
        //                        chargesDt.EmbalaceAmount = Convert.ToDecimal(embalaceHd.Tariff * orderDt.EmbalaceQty);
        //                    }
        //                }

        //            }
        //            else
        //            {
        //                chargesDt.EmbalaceAmount = 0;
        //            }
        //        }
        //        else
        //        {
        //            chargesDt.PrescriptionFeeAmount = 0;
        //            chargesDt.EmbalaceAmount = 0;
        //        }
        //        #endregion

        //        decimal grossLineAmount = (chargesDt.ChargedQuantity * price) + chargesDt.EmbalaceAmount + chargesDt.PrescriptionFeeAmount;

        //        decimal totalDiscountAmount = 0;
        //        decimal totalDiscountAmount1 = 0;
        //        decimal totalDiscountAmount2 = 0;
        //        decimal totalDiscountAmount3 = 0;

        //        if (isDiscountInPercentage || isDiscountInPercentageComp1 || isDiscountInPercentageComp2 || isDiscountInPercentageComp3)
        //        {
        //            if (isDiscountUsedComp)
        //            {
        //                if (priceComp1 > 0)
        //                {
        //                    if (isDiscountInPercentageComp1)
        //                    {
        //                        totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
        //                        chargesDt.DiscountPercentageComp1 = discountAmountComp1;
        //                    }
        //                    else
        //                    {
        //                        totalDiscountAmount1 = discountAmountComp1;
        //                    }
        //                }

        //                if (priceComp2 > 0)
        //                {
        //                    if (isDiscountInPercentageComp2)
        //                    {
        //                        totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
        //                        chargesDt.DiscountPercentageComp2 = discountAmountComp2;
        //                    }
        //                    else
        //                    {
        //                        totalDiscountAmount2 = discountAmountComp2;
        //                    }
        //                }

        //                if (priceComp3 > 0)
        //                {
        //                    if (isDiscountInPercentageComp3)
        //                    {
        //                        totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
        //                        chargesDt.DiscountPercentageComp3 = discountAmountComp3;
        //                    }
        //                    else
        //                    {
        //                        totalDiscountAmount3 = discountAmountComp3;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (priceComp1 > 0)
        //                {
        //                    totalDiscountAmount1 = priceComp1 * discountAmount / 100;
        //                    chargesDt.DiscountPercentageComp1 = discountAmount;
        //                }

        //                if (priceComp2 > 0)
        //                {
        //                    totalDiscountAmount2 = priceComp2 * discountAmount / 100;
        //                    chargesDt.DiscountPercentageComp2 = discountAmount;
        //                }

        //                if (priceComp3 > 0)
        //                {
        //                    totalDiscountAmount3 = priceComp3 * discountAmount / 100;
        //                    chargesDt.DiscountPercentageComp3 = discountAmount;
        //                }
        //            }

        //            if (chargesDt.DiscountPercentageComp1 > 0)
        //            {
        //                chargesDt.IsDiscountInPercentageComp1 = true;
        //            }

        //            if (chargesDt.DiscountPercentageComp2 > 0)
        //            {
        //                chargesDt.IsDiscountInPercentageComp2 = true;
        //            }

        //            if (chargesDt.DiscountPercentageComp3 > 0)
        //            {
        //                chargesDt.IsDiscountInPercentageComp3 = true;
        //            }
        //        }
        //        else
        //        {
        //            if (isDiscountUsedComp)
        //            {
        //                if (priceComp1 > 0)
        //                    totalDiscountAmount1 = discountAmountComp1;
        //                if (priceComp2 > 0)
        //                    totalDiscountAmount2 = discountAmountComp2;
        //                if (priceComp3 > 0)
        //                    totalDiscountAmount3 = discountAmountComp3;
        //            }
        //            else
        //            {
        //                if (priceComp1 > 0)
        //                    totalDiscountAmount1 = discountAmount;
        //                if (priceComp2 > 0)
        //                    totalDiscountAmount2 = discountAmount;
        //                if (priceComp3 > 0)
        //                    totalDiscountAmount3 = discountAmount;
        //            }
        //        }

        //        totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (chargesDt.ChargedQuantity);

        //        if (grossLineAmount > 0)
        //        {
        //            if (totalDiscountAmount > grossLineAmount)
        //            {
        //                totalDiscountAmount = grossLineAmount;
        //            }
        //        }

        //        decimal total = grossLineAmount - totalDiscountAmount;
        //        decimal totalPayer = 0;
        //        if (isCoverageInPercentage)
        //        {
        //            totalPayer = total * coverageAmount / 100;
        //        }
        //        else
        //        {
        //            totalPayer = coverageAmount * chargesDt.ChargedQuantity;
        //        }

        //        if (total == 0)
        //        {
        //            totalPayer = total;
        //        }
        //        else
        //        {
        //            if (totalPayer < 0 && totalPayer < total)
        //            {
        //                totalPayer = total;
        //            }
        //            else if (totalPayer > 0 & totalPayer > total)
        //            {
        //                totalPayer = total;
        //            }
        //        }

        //        ctx.CommandType = CommandType.Text;
        //        ctx.Command.Parameters.Clear();
        //        ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, chargesDt.ItemID), ctx).FirstOrDefault();
        //        chargesDt.AveragePrice = iPlanning.AveragePrice;
        //        chargesDt.CostAmount = iPlanning.UnitPrice;

        //        chargesDt.ConversionFactor = orderDt.ConversionFactor;
        //        chargesDt.IsCITO = false;
        //        chargesDt.CITOAmount = 0;
        //        chargesDt.IsComplication = false;
        //        chargesDt.ComplicationAmount = 0;

        //        chargesDt.IsDiscount = totalDiscountAmount != 0 ? true : false;
        //        chargesDt.DiscountAmount = totalDiscountAmount;
        //        chargesDt.DiscountComp1 = totalDiscountAmount1;
        //        chargesDt.DiscountComp2 = totalDiscountAmount2;
        //        chargesDt.DiscountComp3 = totalDiscountAmount3;

        //        decimal oPatientAmount = total - totalPayer;
        //        decimal oPayerAmount = totalPayer;
        //        decimal oLineAmount = total;

        //        if (hdnIsEndingAmountRoundingTo100.Value == "1")
        //        {
        //            oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
        //            oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
        //            oLineAmount = oPatientAmount + oPayerAmount;
        //        }

        //        chargesDt.PatientAmount = oPatientAmount;
        //        chargesDt.PayerAmount = oPayerAmount;
        //        chargesDt.LineAmount = oLineAmount;

        //        chargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
        //        chargesDt.CreatedBy = AppSession.UserLogin.UserID;
        //        lstChargesDt.Add(chargesDt);

        //        ctx.CommandType = CommandType.Text;
        //        ctx.Command.Parameters.Clear();
        //        PrescriptionOrderDt entityOrderDt = orderDtDao.Get(Convert.ToInt32(orderDt.PrescriptionOrderDetailID));
        //        entityOrderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.IN_PROGRESS;
        //        lstOrderDt.Add(entityOrderDt);

        //        #endregion
        //    }

        //    #region Commit to Database
        //    chargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(chargesHd.TransactionCode, chargesHd.TransactionDate, ctx);
        //    chargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN; //OPEN
        //    chargesHd.CreatedBy = AppSession.UserLogin.UserID;
        //    ctx.CommandType = CommandType.Text;
        //    ctx.Command.Parameters.Clear();
        //    int transactionID = chargesHdDao.InsertReturnPrimaryKeyID(chargesHd);
        //    transactionNo = chargesHd.TransactionNo;

        //    foreach (PatientChargesDt item in lstChargesDt)
        //    {
        //        item.TransactionID = transactionID;
        //        item.IsApproved = false;
        //        item.CreatedBy = AppSession.UserLogin.UserID;
        //        chargesDtDao.Insert(item);
        //    }

        //    foreach (PrescriptionOrderDt orderDt in lstOrderDt)
        //    {
        //        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
        //        orderDtDao.Update(orderDt);
        //    }
        //    #endregion

        //    return result;
        //}

        private string GenerateMedicationSchedule(string lstRecordID, string lstStartDate, string lstDuration, string lstSelectedTime1, string lstSelectedTime2,
            string lstSelectedTime3, string lstSelectedTime4, string lstSelectedTime5, string lstSelectedTime6)
        {
            string result = string.Empty;
            String[] selectedID = lstRecordID.Split(',');
            String[] selectedStartDate = lstStartDate.Split(',');
            String[] selectedDuration = lstDuration.Split(',');
            String[] selectedTime1 = lstSelectedTime1.Split(',');
            String[] selectedTime2 = lstSelectedTime2.Split(',');
            String[] selectedTime3 = lstSelectedTime3.Split(',');
            String[] selectedTime4 = lstSelectedTime4.Split(',');
            String[] selectedTime5 = lstSelectedTime5.Split(',');
            String[] selectedTime6 = lstSelectedTime6.Split(',');

            IDbContext ctx = DbFactory.Configure(true);
            PatientVisitNoteDao visitNoteDao = new PatientVisitNoteDao(ctx);
            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderChangesLogDao changesLogDao = new PrescriptionOrderChangesLogDao(ctx);
            PharmacyJournalDao journalDao = new PharmacyJournalDao(ctx);

            try
            {
                StringBuilder pharmacistNoteText = new StringBuilder();

                if (!string.IsNullOrEmpty(txtPharmacyNoteText.Text))
                    pharmacistNoteText.AppendLine(txtPharmacyNoteText.Text);
                else
                    pharmacistNoteText.AppendLine("Extend Medication Duration :");

                for (int ct = 0; ct < selectedID.Length; ct++)
                {
                    PrescriptionOrderDt orderDt = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderDetailID = {0}", selectedID[ct]),ctx).FirstOrDefault();
                    DateTime startDate = Helper.GetDatePickerValue(selectedStartDate[ct]);
                    int duration = Convert.ToInt16(selectedDuration[ct]);

                    #region Pharmacist Notes Detail
                    string signa = string.Format("S {0} x {1} {2}", orderDt.Frequency.ToString(), orderDt.NumberOfDosage.ToString(), orderDt.GCDosingUnit.Substring(5));
                    string noteDetail = string.Format("- {0} {1}, Start Date : {2}, Duration : {3}", orderDt.DrugName, signa, startDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), duration.ToString());
                    pharmacistNoteText.AppendLine(noteDetail);
                    #endregion

                    #region Log Prescription Order Changes
                    StringBuilder logText = new StringBuilder();

                    if (!string.IsNullOrEmpty(txtPharmacyNoteText.Text))
                        logText.AppendLine(txtPharmacyNoteText.Text);

                    logText.AppendLine("Extend Medication Duration :");
                    logText.AppendLine(noteDetail);

                    PrescriptionOrderChangesLog changesLog = new PrescriptionOrderChangesLog();
                    changesLog.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    changesLog.LogDate = DateTime.Now.Date;
                    changesLog.LogTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    changesLog.GCPrescriptionOrderChangeType = Constant.PrescriptionOrderChangesType.QUANTITY;
                    changesLog.PrescriptionOrderID = orderDt.PrescriptionOrderID;
                    changesLog.HealthcareServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                    changesLog.NoteText = logText.ToString();
                    changesLog.IsAutoGeneratedBySystem = true;
                    changesLog.CreatedBy = AppSession.UserLogin.UserID;
                    changesLogDao.Insert(changesLog);
                    #endregion
                    

                    for (int i = 0; i < duration; i++)
                    {
                        DateTime date = startDate.AddDays(i);
                        int sequenceNo = 1;
                        for (int j = 0; j < orderDt.Frequency; j++)
                        {
                            MedicationSchedule oSchedule = new MedicationSchedule();
                            oSchedule.VisitID = Convert.ToInt32(hdnVisitID.Value);
                            oSchedule.PrescriptionOrderID = orderDt.PrescriptionOrderID;
                            oSchedule.PrescriptionOrderDetailID = orderDt.PrescriptionOrderDetailID;
                            oSchedule.ItemID = orderDt.ItemID;
                            oSchedule.ItemName = orderDt.DrugName;
                            oSchedule.MedicationDate = date;
                            oSchedule.SequenceNo = sequenceNo.ToString();
                            oSchedule.MedicationTime = GetSequenceMedicationTime(sequenceNo, selectedTime1[ct], selectedTime2[ct],
                                selectedTime3[ct], selectedTime4[ct], selectedTime5[ct], selectedTime6[ct]);
                            oSchedule.NumberOfDosage = orderDt.NumberOfDosage;
                            oSchedule.NumberOfDosageInString = orderDt.NumberOfDosageInString;
                            oSchedule.GCDosingUnit = orderDt.GCDosingUnit;
                            oSchedule.ConversionFactor = orderDt.ConversionFactor;
                            oSchedule.ResultQuantity = 0;
                            oSchedule.ChargeQuantity = 0;
                            oSchedule.IsAsRequired = orderDt.IsAsRequired;
                            oSchedule.GCRoute = orderDt.GCRoute;
                            oSchedule.GCCoenamRule = orderDt.GCCoenamRule;
                            oSchedule.MedicationAdministration = orderDt.MedicationAdministration;
                            oSchedule.GCMedicationStatus = Constant.MedicationStatus.OPEN;
                            oSchedule.IsInternalMedication = true;
                            oSchedule.IsUsingUDD = orderDt.IsUsingUDD;
                            oSchedule.CreatedBy = AppSession.UserLogin.UserID;
                            sequenceNo++;
                            scheduleDao.Insert(oSchedule);
                        }                   
                    }

                    //Update Prescription Order Duration     
                    orderDt.DosingDuration = orderDt.DosingDuration + duration;
                    orderDtDao.Update(orderDt);
                }

                #region Pharmacist Notes


                int linkedNoteID = 0;
                if (hdnPlanningNoteID.Value != "" && hdnPlanningNoteID.Value != "0")
                    linkedNoteID = Convert.ToInt32(hdnPlanningNoteID.Value);

                if (chkIsGenerateCPPT.Checked)
                {
                    PatientVisitNote note = new PatientVisitNote();
                    note.VisitID = Convert.ToInt32(hdnVisitID.Value);
                    note.GCPatientNoteType = Constant.PatientVisitNotes.PHARMACY_NOTES;
                    note.NoteDate = DateTime.Now.Date;
                    note.NoteTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    note.HealthcareServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                    note.ParamedicID = AppSession.UserLogin.ParamedicID;
                    note.NoteText = pharmacistNoteText.ToString();

                    if (cboPhysicianInstructionSource.Value != null)
                        note.GCPhysicianInstructionSource = cboPhysicianInstructionSource.Value.ToString();
                    if (linkedNoteID != 0)
                        note.LinkedNoteID = linkedNoteID;

                    if (chkIsNeedConfirmation.Checked)
                    {
                        note.IsNeedConfirmation = chkIsNeedConfirmation.Checked;
                        if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
                            note.ConfirmationPhysicianID = Convert.ToInt32(hdnParamedicID.Value);
                    }
                    note.CreatedBy = AppSession.UserLogin.UserID;
                    visitNoteDao.Insert(note); 
                }

                //Confirm Notification Note
                if (linkedNoteID != 0)
                {
                    PatientVisitNote oNote = BusinessLayer.GetPatientVisitNote(linkedNoteID);
                    if (oNote.NotificationParamedicID == null)
                    {
                        oNote.NotificationDateTime = DateTime.Now;
                        oNote.NotificationParamedicID = AppSession.UserLogin.ParamedicID;
                        oNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                        visitNoteDao.Update(oNote);
                    }
                }
                #endregion

                #region Pharmacy Journal
                if (chkIsGenerateJournal.Checked)
                {
                    PharmacyJournal oJournal = new PharmacyJournal();
                    oJournal.JournalDate = DateTime.Now.Date;
                    oJournal.JournalTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    oJournal.VisitID = AppSession.RegisteredPatient.VisitID;
                    oJournal.HealthcareServiceUnitID = Convert.ToInt32(hdnDispensaryServiceUnitID.Value);
                    oJournal.JournalText = pharmacistNoteText.ToString();
                    if (linkedNoteID != 0)
                    {
                        oJournal.PatientInstructionID = linkedNoteID;
                    }
                    oJournal.CreatedBy = AppSession.UserLogin.UserID;
                    journalDao.Insert(oJournal);
                }
                #endregion

                ctx.CommitTransaction();

                string message = string.Format("Medication schedule was created successfully");
                result = string.Format("process|1|{0}", message);
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private string GetSequenceMedicationTime(int sequence, string time1, string time2, string time3, string time4, string time5, string time6)
        {
            string medicationTime = "";
            switch (sequence)
            {
                case 1:
                    medicationTime = time1;
                    break;
                case 2:
                    medicationTime = time2;
                    break;
                case 3:
                    medicationTime = time3;
                    break;
                case 4:
                    medicationTime = time4;
                    break;
                case 5:
                    medicationTime = time5;
                    break;
                case 6:
                    medicationTime = time6;
                    break;
                default:
                    medicationTime = "00:00";
                    break;
            }
            if (string.IsNullOrEmpty(medicationTime))
            {
                medicationTime = "00:00";
            }
            return medicationTime;
        }

        private void SetControlProperties()
        {
            //List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
            //                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
            //                                                    AppSession.UserLogin.HealthcareID,
            //                                                    Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT,
            //                                                    Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100
            //                                                ));

            //hdnPrescriptionFeeAmount.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT).ParameterValue;
            //hdnIsEndingAmountRoundingTo100.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;

            //Load Physician
            string filterExpression = string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID);
            List<vParamedicTeam> lstPhysician = BusinessLayer.GetvParamedicTeamList(filterExpression);
            Methods.SetComboBoxField(cboPhysician, lstPhysician, "ParamedicName", "ParamedicID");
            cboPhysician.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            filterExpression = string.Format("ParentID = '{0}'", Constant.StandardCode.PHYSICIAN_INSTRUCTION_SOURCE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField(cboPhysicianInstructionSource, lstStandardCode, "StandardCodeName", "StandardCodeID");

            if (AppSession.UserLogin.ParamedicID != null)
            {
                chkIsGenerateCPPT.Enabled = true;
            }
            else
            {
                chkIsGenerateCPPT.Checked = false;
                chkIsGenerateCPPT.Enabled = false;
            }

            BindGridView();
        }

        protected void lvwExtendView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vODDMedicationScheduleForExtend item = (vODDMedicationScheduleForExtend)e.Item.DataItem;
                TextBox txtStartDate = e.Item.FindControl("txtStartDate") as TextBox;
                TextBox txtDuration = e.Item.FindControl("txtDuration") as TextBox;
                int duration = 1;
                if (!string.IsNullOrEmpty(txtDefaultDuration.Text))
                {
                    duration = Convert.ToInt16(txtDefaultDuration.Text);
                }
                txtStartDate.Text = Helper.GetDatePickerValue(txtDefaultStartDate.Text).AddDays(1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDuration.Text = txtDefaultDuration.Text;
            }
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitID = {0} AND (EndDate <= '{1}' OR EndDate IS NULL) AND IsInternalMedication = 1 AND IsUsingUDD = 0", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtDefaultStartDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
            List<vODDMedicationScheduleForExtend> lstEntity = BusinessLayer.GetvODDMedicationScheduleForExtendList(filterExpression);
            lvwExtendView.DataSource = lstEntity;
            lvwExtendView.DataBind();
        }

        protected void cbpExtendView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}