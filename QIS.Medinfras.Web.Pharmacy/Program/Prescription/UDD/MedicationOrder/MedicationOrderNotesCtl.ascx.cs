using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class MedicationOrderDtCtl : BaseContentPopupCtl
    {

        public override void InitializeControl(string param)
        {
            string[] temp = param.Split('|');
            hdnPrescriptionOrderID.Value = temp[0];
            hdnDepartmentID.Value = temp[1];
            hdnImagingServiceUnitID.Value = temp[2];
            hdnLaboratoryServiceUnitID.Value = temp[3];
            hdnIsEntryMode.Value = temp[4];

            divProcessButton.Visible = hdnIsEntryMode.Value == "0";

            txtTransactionDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            vPrescriptionOrderHd1 entityOrderHd = BusinessLayer.GetvPrescriptionOrderHd1List(string.Format("PrescriptionOrderID = {0}", hdnPrescriptionOrderID.Value)).FirstOrDefault();
            txtTransactionDate.Enabled = entityOrderHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL;
            txtTransactionTime.Enabled = txtTransactionDate.Enabled;

            txtPrescriptionOrderNo.Text = entityOrderHd.PrescriptionOrderNo;
            txtPrescriptionOrderDate.Text = entityOrderHd.PrescriptionDate.ToString(Constant.FormatString.DATE_FORMAT);
            txtPrescriptionOrderTime.Text = entityOrderHd.PrescriptionTime;
            txtParamedic.Text = string.Format("{0} ({1})", entityOrderHd.ParamedicName, entityOrderHd.ParamedicCode);
            hdnVisitID.Value = entityOrderHd.VisitID.ToString();

            txtNotes.Text = entityOrderHd.Remarks;
            BindGrid();

            int locationID = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", entityOrderHd.DispensaryServiceUnitID)).FirstOrDefault().LocationID;
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
                Methods.SetComboBoxField<Location>(cboPrescriptionOrderLocation, lstLocation, "LocationName", "LocationID");
                cboPrescriptionOrderLocation.SelectedIndex = 0;
            }

            List<vPrescriptionOrderDt1> orderDtList = BusinessLayer.GetvPrescriptionOrderDt1List(string.Format("PrescriptionOrderID = {0}", hdnPrescriptionOrderID.Value));
            lvwView.Visible = orderDtList.Count > 0;
            hdnIsOrderDetailExists.Value = orderDtList.Count > 0 ? "1" : "0";

            String filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ALASAN_PEMBATALAN_OBAT_RESEP);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboVoidReason, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboVoidReason.SelectedIndex = 0;

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT));
            hdnPrescriptionFeeAmount.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT).ParameterValue;
        }
        private void BindGrid()
        {
            String filterExpression = String.Format("PrescriptionOrderID = {0} AND OrderIsDeleted = 0", hdnPrescriptionOrderID.Value);
            List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            bool isEntryMode = false;
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");
                CheckBox chkIsUsingUDD = (CheckBox)e.Item.FindControl("chkIsUsingUDD");
                vPrescriptionOrderDt1 item = (vPrescriptionOrderDt1)e.Item.DataItem;
                if (item.GCPrescriptionOrderStatus != Constant.OrderStatus.OPEN)
                {
                    chkIsSelected.Visible = false;
                    chkIsUsingUDD.Visible = false;
                }
                else
                {
                    isEntryMode = true;
                }

                if (item.IsCompound || item.IsAsRequired)
                {
                    chkIsUsingUDD.Visible = false;
                    hdnIsHasCompound.Value = item.IsCompound ? "1" : "0";
                }
            }
            divProcessButton.Visible = isEntryMode;
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;
            string result = param + "|";
            string retval = "";

            string errMessage = "";
            if (param == "approve")
            {
                if (OnApproveRecord(ref errMessage, ref retval))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            else if (param == "decline")
            {
                if (OnDeclineRecord(ref errMessage))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            else if (param == "close")
            {
                //if (OnCloseRecord(ref errMessage))
                result += "success";
                //else
                //    result += "fail|" + errMessage;
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }

        private bool OnApproveRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao prescriptionOrderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao prescriptionOrderDtDao = new PrescriptionOrderDtDao(ctx);
            try
            {
                #region PrescriptionOrderHd
                PrescriptionOrderHd orderHd = prescriptionOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                String filterExpression = "1 = 0";
                if (hdnLstSelected.Value != "")
                    filterExpression = String.Format("PrescriptionOrderDetailID IN ({0}) OR ParentID IN ({0})", hdnLstSelected.Value);
                List<PrescriptionOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionOrderDtList(filterExpression, ctx);
                orderHd.LocationID = Convert.ToInt32(cboPrescriptionOrderLocation.Value);
                orderHd.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                orderHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                #endregion

                #region Split Selected Items into UDD and Non-UDD staging list
                string[] selectedUDD = hdnLstSelectedUDD.Value.Split(',');
                int ct = 0;
                List<PrescriptionOrderDt> lstProcessAsUDD = new List<PrescriptionOrderDt>();
                List<PrescriptionOrderDt> lstProcessToCharges = new List<PrescriptionOrderDt>();

                foreach (PrescriptionOrderDt objDt in lstEntityDt)
                {
                    if (objDt.GCPrescriptionOrderStatus != Constant.OrderStatus.OPEN)
                    {
                        result = false;
                        errMessage = "Order Sudah Di Proses Oleh User lain, Silahkan direfresh halaman ini.";
                        break;
                    }

                    if (objDt.IsCompound)
                    {
                        lstProcessToCharges.Add(objDt);
                        if (objDt.IsRFlag)
                            ct += 1;
                    }
                    else
                    {
                        if (selectedUDD[ct] == "1")
                        {
                            lstProcessAsUDD.Add(objDt);
                            ct += 1;
                        }
                        else
                        {
                            lstProcessToCharges.Add(objDt);
                        }
                    }
                }
                #endregion

                #region Process As UDD
                if (lstProcessAsUDD.Count > 0)
                {
                    foreach (PrescriptionOrderDt orderDt in lstProcessAsUDD)
                    {
                        orderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.RECEIVED;
                        orderDt.IsUsingUDD = true;
                        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        prescriptionOrderDtDao.Update(orderDt);
                    }
                }
                #endregion

                #region Process As Charges
                if (lstProcessToCharges.Count > 0)
                {
                    CreateCharges(ctx, orderHd, lstProcessToCharges, ref errMessage);
                }

                #endregion

                #region PrescriptionOrderHd
                prescriptionOrderHdDao.Update(orderHd);
                retval = orderHd.PrescriptionOrderNo;
                #endregion


                if (result)
                    ctx.CommitTransaction();
                else
                    ctx.RollBackTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnDeclineRecord(ref string errMessage)
        {
            bool result = true;

            if (String.IsNullOrEmpty(cboVoidReason.Value.ToString()))
            {
                result = false;
                errMessage = "Alasan Pembatalan harus diisi";
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderDtDao oDetailDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHdDao oHeaderDao = new PrescriptionOrderHdDao(ctx);
            try
            {
                if (hdnIsOrderDetailExists.Value == "1")
                {
                    String filterExpressionHd = String.Format("PrescriptionOrderID = {0} AND PrescriptionOrderDetailID IN ({1})", hdnPrescriptionOrderID.Value, hdnLstSelected.Value);

                    List<PrescriptionOrderDt> lstOrderDt = BusinessLayer.GetPrescriptionOrderDtList(filterExpressionHd);
                    if (lstOrderDt.Count > 0)
                    {
                        foreach (PrescriptionOrderDt dt in lstOrderDt)
                        {
                            if (dt.GCPrescriptionOrderStatus == Constant.TestOrderStatus.CANCELLED || dt.GCPrescriptionOrderStatus == Constant.TestOrderStatus.IN_PROGRESS)
                            {
                                result = false;
                                errMessage = "Order Sudah Di Proses Oleh User lain, Silahkan Merefresh halaman ini.";
                                break;
                            }
                            dt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.CANCELLED;
                            dt.GCVoidReason = cboVoidReason.Value.ToString();
                            if (dt.GCVoidReason.Contains("999")) //Other
                                dt.VoidReason = txtVoidReason.Text;
                            dt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            oDetailDao.Update(dt);
                        }
                    }
                    if (result)
                    {
                        int dtOpenCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.TransactionStatus.OPEN), ctx);
                        int dtProcessedCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.TransactionStatus.PROCESSED), ctx);
                        if (dtProcessedCount > 0)
                        {
                            PrescriptionOrderHd orderHd = oHeaderDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                            orderHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            oHeaderDao.Update(orderHd);
                        }
                        else
                        {
                            if (dtOpenCount == 0 && dtProcessedCount == 0)
                            {
                                PrescriptionOrderHd orderHd = oHeaderDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                                orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                orderHd.GCVoidReason = cboVoidReason.Value.ToString();
                                if (orderHd.GCVoidReason == Constant.DeleteReason.OTHER) //Other
                                    orderHd.VoidReason = txtVoidReason.Text;
                                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                oHeaderDao.Update(orderHd);
                            }
                        }
                    }
                }
                else
                {
                    PrescriptionOrderHd orderHd = oHeaderDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                    orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    orderHd.GCVoidReason = cboVoidReason.Value.ToString();
                    if (orderHd.GCVoidReason == Constant.DeleteReason.OTHER)
                        orderHd.VoidReason = txtVoidReason.Text;
                    orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    oHeaderDao.Update(orderHd);
                }
                if (result) ctx.CommitTransaction();
                else ctx.RollBackTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnCloseRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            return result;
        }

        private bool CreateCharges(IDbContext ctx, PrescriptionOrderHd orderHd, List<PrescriptionOrderDt> lstSelectedOrderDt, ref string errMessage)
        {
            bool result = true;
            String filterExpression = "1 = 0";
            DateTime transactionDate = Helper.GetDatePickerValue(txtTransactionDate.Text);
            string transactionNo = string.Empty;

            PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesHdDao chargesHdDao = new PatientChargesHdDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);

            #region PatientChargesHd
            PatientChargesHd chargesHd = new PatientChargesHd();

            chargesHd.VisitID = orderHd.VisitID;
            chargesHd.TransactionDate = transactionDate;
            chargesHd.TransactionTime = txtTransactionTime.Text;
            chargesHd.PrescriptionOrderID = orderHd.PrescriptionOrderID;
            chargesHd.HealthcareServiceUnitID = orderHd.DispensaryServiceUnitID;
            chargesHd.CreatedBy = AppSession.UserLogin.UserID;
            chargesHd.TransactionCode = Constant.TransactionCode.PRESCRIPTION_INPATIENT;
            #endregion

            //if (hdnLstSelected.Value != "")
            //    filterExpression = String.Format("PrescriptionOrderDetailID IN ({0}) OR ParentID IN ({0})", hdnLstSelected.Value);
            //List<vPrescriptionOrderDt4> lstEntityDt = BusinessLayer.GetvPrescriptionOrderDt4List(filterExpression, ctx);

            List<PatientChargesDt> lstChargesDt = new List<PatientChargesDt>();
            List<PrescriptionOrderDt> lstOrderDt = new List<PrescriptionOrderDt>();

            foreach (PrescriptionOrderDt orderDt in lstSelectedOrderDt)
            {
                if (orderDt.GCPrescriptionOrderStatus == Constant.TestOrderStatus.CANCELLED || orderDt.GCPrescriptionOrderStatus == Constant.TestOrderStatus.IN_PROGRESS)
                {
                    result = false;
                    errMessage = "Order Sudah Di Proses Oleh User lain, Silahkan Merefresh halaman ini.";
                    break;
                }

                PatientChargesDt chargesDt = new PatientChargesDt();

                filterExpression = string.Format("ItemID = {0}", orderDt.ItemID);
                vItemProduct item = BusinessLayer.GetvItemProductList(filterExpression, ctx).FirstOrDefault();

                if (orderDt == null)
                {
                    result = false;
                    errMessage = string.Format("Cannot found item product information for item {0}", orderDt.DrugName);
                    break;
                }

                #region PatientChargesDt
                chargesDt.PrescriptionOrderDetailID = orderDt.PrescriptionOrderDetailID;
                chargesDt.ItemID = (int)orderDt.ItemID;
                chargesDt.LocationID = Convert.ToInt32(cboPrescriptionOrderLocation.Value);
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

                chargesDt.GCBaseUnit = item.GCItemUnit;
                chargesDt.GCItemUnit = orderDt.GCDosingUnit;
                chargesDt.ParamedicID = Convert.ToInt32(orderHd.ParamedicID);

                chargesDt.IsVariable = false;
                chargesDt.IsUnbilledItem = false;

                #region dispense full since it is not UDD

                decimal dosageQty = orderDt.NumberOfDosage;
                decimal compoundQty = orderDt.CompoundQty;
                decimal resultQty = orderDt.ResultQty;
                decimal chargeQty = orderDt.ChargeQty;
                decimal baseQty = orderDt.ResultQty;

                if (item.GCConsumptionDeductionType == Constant.InventoryRoundingStatus.ROUND_UP)
                    resultQty = Math.Ceiling(resultQty);

                if (item.GCStockDeductionType == Constant.InventoryRoundingStatus.ROUND_UP)
                    chargeQty = Math.Ceiling(chargeQty);

                baseQty = resultQty;

                chargesDt.UsedQuantity = resultQty;
                chargesDt.BaseQuantity = baseQty;
                chargesDt.ChargedQuantity = chargeQty;
                #endregion

                chargesDt.PrescriptionFeeAmount = Convert.ToDecimal(hdnPrescriptionFeeAmount.Value);

                if (orderDt.EmbalaceID != null && orderDt.IsRFlag)
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

                if (isDiscountInPercentage)
                {
                    //totalDiscountAmount = grossLineAmount * discountAmount / 100;

                    if (isDiscountUsedComp)
                    {
                        if (priceComp1 > 0)
                        {
                            totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                            chargesDt.DiscountPercentageComp1 = discountAmountComp1;
                        }

                        if (priceComp2 > 0)
                        {
                            totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                            chargesDt.DiscountPercentageComp2 = discountAmountComp2;
                        }

                        if (priceComp3 > 0)
                        {
                            totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                            chargesDt.DiscountPercentageComp3 = discountAmountComp3;
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

                    if (totalDiscountAmount1 > 0)
                    {
                        chargesDt.IsDiscountInPercentageComp1 = true;
                    }

                    if (totalDiscountAmount2 > 0)
                    {
                        chargesDt.IsDiscountInPercentageComp2 = true;
                    }

                    if (totalDiscountAmount3 > 0)
                    {
                        chargesDt.IsDiscountInPercentageComp3 = true;
                    }
                }
                else
                {
                    //totalDiscountAmount = discountAmount * 1;

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

                chargesDt.ConversionFactor = orderDt.ConversionFactor;
                chargesDt.IsCITO = false;
                chargesDt.CITOAmount = 0;
                chargesDt.IsComplication = false;
                chargesDt.ComplicationAmount = 0;

                chargesDt.IsDiscount = totalDiscountAmount != 0;
                chargesDt.DiscountAmount = totalDiscountAmount;
                chargesDt.DiscountComp1 = totalDiscountAmount1;
                chargesDt.DiscountComp2 = totalDiscountAmount2;
                chargesDt.DiscountComp3 = totalDiscountAmount3;

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
                    entityOrderDt.TakenQty = orderDt.DispenseQty;
                }
                entityOrderDt.GCPrescriptionOrderStatus = Constant.OrderStatus.IN_PROGRESS;
                entityOrderDt.IsUsingUDD = false;
                lstOrderDt.Add(entityOrderDt);
                #endregion
            }

            #region Commit to Database
            chargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(chargesHd.TransactionCode, chargesHd.TransactionDate, ctx);
            chargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN; //OPEN
            chargesHd.CreatedBy = AppSession.UserLogin.UserID;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            int transactionID = chargesHdDao.InsertReturnPrimaryKeyID(chargesHd);
            transactionNo = chargesHd.TransactionNo;

            foreach (PatientChargesDt item in lstChargesDt)
            {
                item.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL; //PROPOSED
                item.TransactionID = transactionID;
                item.IsApproved = false;
                chargesDtDao.Insert(item);
            }

            foreach (PrescriptionOrderDt orderDt in lstOrderDt)
            {
                orderDtDao.Update(orderDt);
            }
            #endregion

            return result;
        }
    }
}