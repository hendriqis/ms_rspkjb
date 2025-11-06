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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ProcessMedicationScheduleCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnDispensaryServiceUnitID.Value = paramInfo[0];
            hdnPrescriptionFeeAmount.Value = paramInfo[1];
            txtMedicationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            SetControlProperties();
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            string lstRecordID = hdnSelectedID.Value;
            string lstScheduleID = hdnSelectedScheduleID.Value;
            string lstTime = hdnSelectedTime.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            //result = PrintUDDDrugLabel(lstRecordID);
            result = ProcessMedicationSchedule(lstRecordID, lstScheduleID,lstTime);

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string ProcessMedicationSchedule(string lstRecordID, string lstScheduleID,string lstTime)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);

            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            PatientChargesHdDao chargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);

            int locationID = Convert.ToInt16(cboLocation.Value);
            DateTime transactionDate = Helper.GetDatePickerValue(txtMedicationDate.Text);

            try
            {
                string transactionNo = string.Empty;
                string referenceNo = string.Empty;

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
                chargesHd.TransactionTime = txtDefaultTime.Text;
                chargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                chargesHd.PrescriptionOrderID = null; //multiple prescription order id
                chargesHd.HealthcareServiceUnitID = Convert.ToInt16(hdnDispensaryServiceUnitID.Value);
                chargesHd.CreatedBy = AppSession.UserLogin.UserID;
                chargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_INPATIENT;
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
                    decimal discountAmount = 0;
                    decimal coverageAmount = 0;
                    decimal price = 0;
                    decimal basePrice = 0;
                    bool isCoverageInPercentage = false;
                    bool isDiscountInPercentage = false;
                    if (list.Count > 0)
                    {
                        GetCurrentItemTariff objItemTariff = list[0];
                        discountAmount = objItemTariff.DiscountAmount;
                        coverageAmount = objItemTariff.CoverageAmount;
                        price = objItemTariff.Price;
                        basePrice = objItemTariff.BasePrice;
                        isCoverageInPercentage = objItemTariff.IsCoverageInPercentage;
                        isDiscountInPercentage = objItemTariff.IsDiscountInPercentage;
                    }

                    chargesDt.BaseTariff = basePrice;
                    chargesDt.Tariff = price;

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

                    if (orderDt.GCConsumptionDeductionType == Constant.InventoryRoundingStatus.ROUND_UP)
                        resultQty = Math.Ceiling(resultQty);

                    if (orderDt.GCStockDeductionType == Constant.InventoryRoundingStatus.ROUND_UP)
                        chargeQty = Math.Ceiling(chargeQty);

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
                    if (isDiscountInPercentage)
                        totalDiscountAmount = grossLineAmount * discountAmount / 100;
                    else
                        totalDiscountAmount = discountAmount * 1;
                    if (totalDiscountAmount > grossLineAmount)
                        totalDiscountAmount = grossLineAmount;

                    decimal total = grossLineAmount - totalDiscountAmount;
                    decimal totalPayer = 0;
                    if (isCoverageInPercentage)
                        totalPayer = total * coverageAmount / 100;
                    else
                        totalPayer = coverageAmount * 1;
                    if (totalPayer > total)
                        totalPayer = total;

                    chargesDt.ConversionFactor = orderDt.ConversionFactor;
                    chargesDt.IsCITO = false;
                    chargesDt.CITOAmount = 0;
                    chargesDt.IsComplication = false;
                    chargesDt.ComplicationAmount = 0;
                    chargesDt.IsDiscount = false;
                    chargesDt.DiscountAmount = totalDiscountAmount;
                    chargesDt.PatientAmount = total - totalPayer;
                    chargesDt.PayerAmount = totalPayer;
                    chargesDt.LineAmount = total;

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

                foreach (PatientChargesDt item in lstChargesDt)
                {
                    item.TransactionID = transactionID;
                    item.IsApproved = true;
                    chargesDtDao.Insert(item);
                }

                filterExpression = string.Format("ID in ({0})", lstScheduleID);
                List<MedicationSchedule> lstMedication = BusinessLayer.GetMedicationScheduleList(filterExpression, ctx);

                int counter = 0;
                string[] time = lstTime.Split(',');
                foreach (var medication in lstMedication)
                {
                    medication.ReferenceNo = referenceNo;
                    medication.TransactionID = transactionID;
                    medication.MedicationTime = time[counter];
                    medication.GCMedicationStatus = Constant.MedicationStatus.DIPROSES_FARMASI;
                    medication.LastUpdatedBy = AppSession.UserLogin.UserID;
                    scheduleDao.Update(medication);
                    counter += 1;
                }

                foreach (PrescriptionOrderDt orderDt in lstOrderDt)
                {
                    orderDtDao.Update(orderDt);
                }
                ctx.CommitTransaction();
                #endregion

                string message = string.Format("Transaction was created successfully with Transaction Number <b>{0}</b>", transactionNo);
                result = string.Format("process|1|{0}|{1}|{2}", message, referenceNo, transactionNo);
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}||", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
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

        private string PrintUDDDrugLabel(string lstRecordID)
        {
            string result = string.Empty;
            //try
            //{
            //    //Get Printing Configuration
            //    string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
            //        AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FM_FORMAT_CETAKAN_LABEL_UDD);
            //    List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
            //    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.FM_FORMAT_CETAKAN_LABEL_UDD)).FirstOrDefault().ParameterValue;

            //    bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

            //    if (isBasedOnIPAddress)
            //    {
            //        //Get Printer Address
            //        string ipAddress = HttpContext.Current.Request.UserHostAddress;

            //        filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType = '{1}' AND IsDeleted=0",
            //            ipAddress, Constant.DirectPrintType.LABEL_UDD);

            //        List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            //        if (lstPrinter.Count > 0)
            //        {
            //            string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_UDD).FirstOrDefault().PrinterName;
            //            string printerUrl2 = string.Empty;

            //            PrescriptionOrderHd oHeader = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));
            //            ZebraPrinting.PrintDrugLabel(oHeader, printFormat, lstPrescriptionOrderDetailID, printerUrl1, printerUrl2);
            //        }
            //        else
            //        {
            //            result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
            //        }
            //    }
            //    else
            //    {
            //        PrescriptionOrderHd oHeader = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(hdnPrescriptionOrderID.Value));
            //        ZebraPrinting.PrintDrugLabel(oHeader, printFormat, lstPrescriptionOrderDetailID);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    result = ex.Message;
            //}
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

            int locationID = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnDispensaryServiceUnitID.Value)).FirstOrDefault().LocationID;
            hdnLocationID.Value = locationID.ToString();
            if (locationID > 0)
            {
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
            BindGridView();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
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
            string filterExpression = string.Format("VisitID = {0} AND MedicationDate = '{1}' AND GCMedicationStatus = '{2}'", AppSession.RegisteredPatient.VisitID, 
                Helper.GetDatePickerValue(txtMedicationDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.MedicationStatus.OPEN);
            if (cboSequence.Value != null)
            {
                filterExpression += string.Format(" AND SequenceNo = '{0}'", cboSequence.Value);
            }
            List<vMedicationSchedule> lstEntity = BusinessLayer.GetvMedicationScheduleList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}