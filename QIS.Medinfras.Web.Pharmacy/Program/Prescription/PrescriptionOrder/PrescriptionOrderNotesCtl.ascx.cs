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
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrescriptionOrderDtCtl : BaseContentPopupCtl
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

            vPrescriptionOrderHd entityOrderHd = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("PrescriptionOrderID = {0}", hdnPrescriptionOrderID.Value)).FirstOrDefault();
            hdnChargesTransactionID.Value = entityOrderHd.ChargesTransactionID.ToString();

            txtTransactionDate.Enabled = (hdnChargesTransactionID.Value == "" || hdnChargesTransactionID.Value == "0");
            txtTransactionTime.Enabled = txtTransactionDate.Enabled;

            txtPrescriptionOrderNo.Text = entityOrderHd.PrescriptionOrderNo;
            txtPrescriptionOrderDate.Text = entityOrderHd.PrescriptionDate.ToString(Constant.FormatString.DATE_FORMAT);
            txtPrescriptionOrderTime.Text = entityOrderHd.PrescriptionTime;
            txtSendOrderDate.Text = entityOrderHd.SendOrderDateTime.ToString(Constant.FormatString.DATE_FORMAT);
            txtSendOrderTime.Text = entityOrderHd.SendOrderDateTime.ToString(Constant.FormatString.TIME_FORMAT);
            txtParamedic.Text = string.Format("{0} ({1})", entityOrderHd.ParamedicName, entityOrderHd.ParamedicCode);
            hdnVisitID.Value = entityOrderHd.VisitID.ToString();
            vConsultVisit consultVisit = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0}", hdnVisitID.Value))[0];
            hdnRegistrationID.Value = consultVisit.RegistrationID.ToString();
            hdnChargeClassID.Value = consultVisit.ChargeClassID.ToString();
            txtNotes.Text = entityOrderHd.Remarks;
            BindGrid();

            vHealthcareServiceUnit oServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", entityOrderHd.DispensaryServiceUnitID)).FirstOrDefault();
            int locationID = oServiceUnit.LocationID;
            hdnDispensaryInitial.Value = oServiceUnit.ShortName;
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

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                                                                AppSession.UserLogin.HealthcareID, //0
                                                                Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, //1
                                                                Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, //2
                                                                Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT, //3
                                                                Constant.SettingParameter.PH0037, //4
                                                                Constant.SettingParameter.PH_DEFAULT_EMBALACE_CODE_PRESCRIPTION, //5
                                                                Constant.SettingParameter.PH_AUTO_INSERT_EMBALACE_PRESCRIPTION, //6
                                                                Constant.SettingParameter.PH_CREATE_QUEUE_LABEL, //7
                                                                Constant.SettingParameter.PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX, //8
                                                                Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //9
                                                                Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1 //10
                                                            ));

            hdnImagingServiceUnitID.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnPrescriptionFeeAmount.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT).ParameterValue;
            hdnIsAutoGenerateReferenceNo.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH0037).ParameterValue;
            hdnDefaultEmbalaceIDPopUp.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_DEFAULT_EMBALACE_CODE_PRESCRIPTION).ParameterValue;
            hdnIsAutoInsertEmbalacePopUp.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_AUTO_INSERT_EMBALACE_PRESCRIPTION).ParameterValue;
            hdnIsGenerateQueueLabel.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_CREATE_QUEUE_LABEL).ParameterValue;
            hdnItemQtyWithSpecialQueuePrefix.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX).ParameterValue;

            hdnIsEndingAmountRoundingTo100.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;

            List<vPrescriptionOrderDt> orderDtList = BusinessLayer.GetvPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0} AND IsDeleted=0", hdnPrescriptionOrderID.Value));
            lvwView.Visible = orderDtList.Count > 0;
            hdnIsOrderDetailExists.Value = orderDtList.Count > 0 ? "1" : "0";

            String filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ALASAN_PEMBATALAN_OBAT_RESEP);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboVoidReason, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboVoidReason.SelectedIndex = 0;

            if (AppSession.IsBridgingToQueue)
            {
                hdnOrderHdInfo.Value = ConvertOrderHdToDTO(entityOrderHd);
                hdnVisitInfo.Value = ConvertVisitToDTO(consultVisit);
            }
        }
        private void BindGrid()
        {
            String filterExpression = String.Format("PrescriptionOrderID = {0} AND ParentID IS NULL AND OrderIsDeleted = 0", hdnPrescriptionOrderID.Value);
            List<vPrescriptionOrderDt1> lstEntity = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");
                vPrescriptionOrderDt1 item = (vPrescriptionOrderDt1)e.Item.DataItem;
                if (item.GCPrescriptionOrderStatus != Constant.TestOrderStatus.RECEIVED)
                    chkIsSelected.Visible = false;
                if (item.IsCompound)
                    hdnIsHasCompound.Value = "1";
            }
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
            else if (param == "nottaken")
            {
                hdnIsNotTakenByPatient.Value = "1";
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
            try
            {
                #region PrescriptionOrderHd
                PrescriptionOrderHd orderHd = prescriptionOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                orderHd.LocationID = Convert.ToInt32(cboPrescriptionOrderLocation.Value);
                orderHd.GCOrderStatus = Constant.OrderStatus.IN_PROGRESS;
                orderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                orderHd.ReferenceNo = string.Format("{0}|{1}", txtReferenceNo1.Text, txtReferenceNo2.Text, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT2));
                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                prescriptionOrderHdDao.Update(orderHd);
                #endregion

                #region PatientChargesHd+Dt
                if (hdnChargesTransactionID.Value == "" || hdnChargesTransactionID.Value == "0" || BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnChargesTransactionID.Value)).GCTransactionStatus != Constant.TransactionStatus.OPEN)
                {
                    result = SaveAddRecord(ctx, orderHd, ref retval, ref errMessage);
                }
                else
                {
                    result = SaveEditRecord(ctx, orderHd, ref retval, ref errMessage);
                }
                #endregion

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
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
                    String filterExpressionHd = String.Format("PrescriptionOrderID = {0} AND (PrescriptionOrderDetailID IN ({1}) OR ParentID IN ({1}))", hdnPrescriptionOrderID.Value, hdnLstSelected.Value);

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
                            //dt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            oDetailDao.Update(dt);
                        }
                    }
                    if (result)
                    {
                        int dtAllCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND IsDeleted = 0", hdnPrescriptionOrderID.Value), ctx);
                        int dtOpenCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.OrderStatus.RECEIVED), ctx);
                        int dtProcessedCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.OrderStatus.IN_PROGRESS), ctx);
                        int dtVoidCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.OrderStatus.CANCELLED), ctx);
                        if (dtProcessedCount > 0)
                        {
                            PrescriptionOrderHd orderHd = oHeaderDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                            orderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                            orderHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                            orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            oHeaderDao.Update(orderHd);
                        }
                        else
                        {
                            if (dtVoidCount == dtAllCount)
                            {
                                PrescriptionOrderHd orderHd = oHeaderDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                                orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                orderHd.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                oHeaderDao.Update(orderHd);
                            }
                        }
                    }
                }
                else
                {
                    int dtAllCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND IsDeleted = 0", hdnPrescriptionOrderID.Value), ctx);
                    int dtVoidCount = BusinessLayer.GetPrescriptionOrderDtRowCount(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1 AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionOrderID.Value, Constant.OrderStatus.CANCELLED), ctx);

                    if (dtVoidCount == dtAllCount)
                    {
                        PrescriptionOrderHd orderHd = oHeaderDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
                        orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        orderHd.GCVoidReason = cboVoidReason.Value.ToString();
                        if (orderHd.GCVoidReason == Constant.DeleteReason.OTHER)
                            orderHd.VoidReason = txtVoidReason.Text;
                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oHeaderDao.Update(orderHd);
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

        private bool OnCloseRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            //IDbContext ctx = DbFactory.Configure(true);
            //PrescriptionOrderHdDao prescriptionOrderHdDao = new PrescriptionOrderHdDao(ctx);
            //try
            //{
            //    PrescriptionOrderHd testOrderHd = prescriptionOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionOrderID.Value));
            //    testOrderHd.LocationID = Convert.ToInt32(cboPrescriptionOrderLocation.Value);
            //    testOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
            //    testOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
            //    prescriptionOrderHdDao.Update(testOrderHd);
            //    SaveAddRecord(ctx, testOrderHd, ref retval);
            //    ctx.CommitTransaction();
            //}
            //catch (Exception ex)
            //{
            //    result = false;
            //    errMessage = ex.Message;
            //    ctx.RollBackTransaction();
            //}
            //finally
            //{
            //    ctx.Close();
            //}
            return result;
        }

        private bool SaveAddRecord(IDbContext ctx, PrescriptionOrderHd testOrderHd, ref string retval, ref string errMessage)
        {
            bool result = true;
            String filterExpression = "1 = 0";
            if (hdnLstSelected.Value != "")
                filterExpression = String.Format("PrescriptionOrderDetailID IN ({0}) OR ParentID IN ({0})", hdnLstSelected.Value);
            List<PrescriptionOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionOrderDtList(filterExpression, ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesHdDao entityChargesHdDao = new PatientChargesHdDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            ItemProductDao iProductDao = new ItemProductDao(ctx);

            #region PatientChargesHd
            PatientChargesHd entityPatientChargesHd = new PatientChargesHd();

            entityPatientChargesHd.VisitID = testOrderHd.VisitID;
            entityPatientChargesHd.TransactionDate = Helper.GetDatePickerValue(txtTransactionDate.Text);
            entityPatientChargesHd.TransactionTime = txtTransactionTime.Text;
            entityPatientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            entityPatientChargesHd.PrescriptionOrderID = testOrderHd.PrescriptionOrderID;
            entityPatientChargesHd.HealthcareServiceUnitID = testOrderHd.DispensaryServiceUnitID;
            switch (hdnDepartmentID.Value)
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
            #endregion

            List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();

            bool isSpecialTrx = lstEntityDt.Count(lst => lst.IsCompound) > 0;
            if (!isSpecialTrx)
            {
                if (hdnItemQtyWithSpecialQueuePrefix.Value != "" && hdnItemQtyWithSpecialQueuePrefix.Value != "0")
                {
                    if (lstEntityDt.Count > Convert.ToInt32(hdnItemQtyWithSpecialQueuePrefix.Value))
                    {
                        isSpecialTrx = true;
                    }
                }
            }

            foreach (PrescriptionOrderDt objDt in lstEntityDt)
            {

                if (objDt.GCPrescriptionOrderStatus == Constant.OrderStatus.CANCELLED || objDt.GCPrescriptionOrderStatus == Constant.OrderStatus.IN_PROGRESS)
                {
                    result = false;
                    errMessage = "Order Sudah Di Proses Oleh User lain, Silahkan Merefresh halaman ini.";
                    break;
                }
                PatientChargesDt entityChargesDt = new PatientChargesDt();

                #region PrescriptionOrderDt
                objDt.GCPrescriptionOrderStatus = Constant.OrderStatus.IN_PROGRESS;
                if (hdnIsNotTakenByPatient.Value == "1")
                {
                    objDt.TakenQty = 0;
                    objDt.ResultQty = 0;
                    objDt.ChargeQty = 0;
                }

                if (!String.IsNullOrEmpty(hdnIsAutoInsertEmbalacePopUp.Value) && hdnIsAutoInsertEmbalacePopUp.Value == "1")
                {
                    if (!String.IsNullOrEmpty(hdnDefaultEmbalaceIDPopUp.Value) && hdnDefaultEmbalaceIDPopUp.Value != "0")
                    {
                        if (hdnIsNotTakenByPatient.Value != "1")
                        {
                            if (!objDt.IsCompound)
                            {
                                objDt.EmbalaceID = Convert.ToInt32(hdnDefaultEmbalaceIDPopUp.Value);
                                objDt.EmbalaceQty = 1;
                            }
                        }
                    }
                }

                //objDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                //objDt.LastUpdatedDate = DateTime.Now;
                orderDtDao.Update(objDt);
                #endregion

                #region PatientChargesDt
                entityChargesDt.PrescriptionOrderDetailID = objDt.PrescriptionOrderDetailID;
                entityChargesDt.ItemID = (int)objDt.ItemID;
                entityChargesDt.LocationID = testOrderHd.LocationID;
                entityChargesDt.ChargeClassID = Convert.ToInt32(hdnChargeClassID.Value);

                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityChargesDt.ChargeClassID, (int)objDt.ItemID, 2, DateTime.Now, ctx);
                decimal discountAmount = 0;
                decimal coverageAmount = 0;
                decimal price = 0;
                decimal tariffComp1 = 0;
                decimal basePrice = 0;
                decimal baseComp1 = 0;
                bool isCoverageInPercentage = false;
                bool isDiscountInPercentage = false;
                if (list.Count > 0)
                {
                    GetCurrentItemTariff objItemTariff = list[0];
                    discountAmount = objItemTariff.DiscountAmount;
                    coverageAmount = objItemTariff.CoverageAmount;
                    price = objItemTariff.Price;
                    tariffComp1 = objItemTariff.PriceComp1;
                    basePrice = objItemTariff.BasePrice;
                    baseComp1 = objItemTariff.BasePriceComp1;
                    isCoverageInPercentage = objItemTariff.IsCoverageInPercentage;
                    isDiscountInPercentage = objItemTariff.IsDiscountInPercentage;
                }

                entityChargesDt.BaseTariff = basePrice;
                entityChargesDt.BaseComp1 = baseComp1;
                entityChargesDt.Tariff = price;
                entityChargesDt.TariffComp1 = tariffComp1;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                ItemMaster entityItemMaster = itemDao.Get((int)objDt.ItemID);
                entityChargesDt.GCBaseUnit = entityItemMaster.GCItemUnit;
                entityChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                entityChargesDt.ParamedicID = Convert.ToInt32(testOrderHd.ParamedicID);

                entityChargesDt.IsVariable = false;
                entityChargesDt.IsUnbilledItem = false;

                entityChargesDt.UsedQuantity = objDt.ResultQty;
                entityChargesDt.ChargedQuantity = objDt.ChargeQty;
                entityChargesDt.BaseQuantity = objDt.ResultQty;

                if (objDt.IsRFlag)
                {
                    entityChargesDt.PrescriptionFeeAmount = Convert.ToDecimal(hdnPrescriptionFeeAmount.Value);

                    if (objDt.EmbalaceID != null)
                    {
                        EmbalaceDt embalace = BusinessLayer.GetEmbalaceDtList(string.Format("EmbalaceID = {0} AND StartingQty <= {1} AND EndingQty >= {1}", objDt.EmbalaceID, objDt.EmbalaceQty)).FirstOrDefault();
                        decimal tariff = 0;

                        if (embalace != null)
                        {
                            tariff = embalace.Tariff;
                        }

                        entityChargesDt.EmbalaceAmount = Convert.ToDecimal(tariff * objDt.EmbalaceQty);
                    }
                    else
                    {
                        entityChargesDt.EmbalaceAmount = 0;
                    } 
                }
                decimal grossLineAmount = (entityChargesDt.ChargedQuantity * price) + entityChargesDt.EmbalaceAmount + entityChargesDt.PrescriptionFeeAmount;
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
                    totalPayer = coverageAmount * entityChargesDt.ChargedQuantity;

                if (total > 0 && totalPayer > total)
                    totalPayer = total;

                if (objDt.ConversionFactor != 0)
                {
                    entityChargesDt.ConversionFactor = objDt.ConversionFactor;
                }
                else
                {
                    entityChargesDt.ConversionFactor = 1;
                }

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

                entityChargesDt.IsCITO = false;
                entityChargesDt.CITOAmount = 0;
                entityChargesDt.IsComplication = false;
                entityChargesDt.ComplicationAmount = 0;
                entityChargesDt.DiscountAmount = totalDiscountAmount;
                if (entityChargesDt.ChargedQuantity > 0)
                {
                    entityChargesDt.DiscountComp1 = totalDiscountAmount / entityChargesDt.ChargedQuantity;
                }
                entityChargesDt.IsDiscount = totalDiscountAmount != 0 ? true : false;

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
                entityChargesDt.IsCreatedBySystem = false;
                entityChargesDt.CreatedBy = entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;


                lstPatientChargesDt.Add(entityChargesDt);
                #endregion
            }


            entityPatientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityPatientChargesHd.TransactionCode, entityPatientChargesHd.TransactionDate, ctx);
            if (hdnIsAutoGenerateReferenceNo.Value == "1")
            {
                entityPatientChargesHd.ReferenceNo = BusinessLayer.GeneratePrescriptionReferenceNo(entityPatientChargesHd.HealthcareServiceUnitID, entityPatientChargesHd.TransactionDate, ctx);
            }
            if (hdnIsGenerateQueueLabel.Value == "1")
            {
                entityPatientChargesHd.QueueNoLabel = BusinessLayer.GenerateChargesQueueNoLabel(entityPatientChargesHd.HealthcareServiceUnitID, entityPatientChargesHd.TransactionDate, isSpecialTrx, ctx);
            }
            entityPatientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            int transactionID = entityChargesHdDao.InsertReturnPrimaryKeyID(entityPatientChargesHd);

            foreach (PatientChargesDt item in lstPatientChargesDt)
            {
                item.TransactionID = transactionID;
                entityChargesDtDao.Insert(item);
            }

            retval = entityPatientChargesHd.TransactionNo;

            return result;
        }

        private bool SaveEditRecord(IDbContext ctx, PrescriptionOrderHd testOrderHd, ref string retval, ref string errMessage)
        {
            bool result = true;
            String filterExpression = "1 = 0";
            if (hdnLstSelected.Value != "")
                filterExpression = String.Format("PrescriptionOrderDetailID IN ({0}) OR ParentID IN ({0})", hdnLstSelected.Value);
            List<PrescriptionOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionOrderDtList(filterExpression, ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesHdDao entityChargesHdDao = new PatientChargesHdDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            ItemProductDao iProductDao = new ItemProductDao(ctx);

            #region PatientChargesHd
            PatientChargesHd entityPatientChargesHd = entityChargesHdDao.Get(Convert.ToInt32(hdnChargesTransactionID.Value));
            int transactionID = entityPatientChargesHd.TransactionID;
            #endregion

            List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();

            foreach (PrescriptionOrderDt objDt in lstEntityDt)
            {
                if (objDt.GCPrescriptionOrderStatus == Constant.TestOrderStatus.CANCELLED || objDt.GCPrescriptionOrderStatus == Constant.TestOrderStatus.IN_PROGRESS)
                {
                    result = false;
                    errMessage = "Order Sudah Di Proses Oleh User lain, Silahkan Merefresh halaman ini.";
                    break;
                }
                PatientChargesDt entityChargesDt = new PatientChargesDt();

                #region PrescriptionOrderDt
                objDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                if (hdnIsNotTakenByPatient.Value == "1")
                {
                    objDt.TakenQty = 0;
                    objDt.ResultQty = 0;
                    objDt.ChargeQty = 0;
                }
                //objDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                //objDt.LastUpdatedDate = DateTime.Now;

                if (!String.IsNullOrEmpty(hdnIsAutoInsertEmbalacePopUp.Value) && hdnIsAutoInsertEmbalacePopUp.Value == "1")
                {
                    if (!String.IsNullOrEmpty(hdnDefaultEmbalaceIDPopUp.Value) && hdnDefaultEmbalaceIDPopUp.Value != "0")
                    {
                        if (hdnIsNotTakenByPatient.Value != "1")
                        {
                            if (!objDt.IsCompound)
                            {
                                objDt.EmbalaceID = Convert.ToInt32(hdnDefaultEmbalaceIDPopUp.Value);
                                objDt.EmbalaceQty = 1;
                            }
                        }
                    }
                }

                orderDtDao.Update(objDt);
                #endregion

                #region PatientChargesDt
                entityChargesDt.PrescriptionOrderDetailID = objDt.PrescriptionOrderDetailID;
                entityChargesDt.TransactionID = transactionID;
                entityChargesDt.ItemID = (int)objDt.ItemID;
                entityChargesDt.LocationID = testOrderHd.LocationID;
                entityChargesDt.ChargeClassID = Convert.ToInt32(hdnChargeClassID.Value);

                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityChargesDt.ChargeClassID, (int)objDt.ItemID, 2, DateTime.Now, ctx);

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

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                ItemMaster entityItemMaster = itemDao.Get((int)objDt.ItemID);
                entityChargesDt.GCBaseUnit = entityItemMaster.GCItemUnit;
                entityChargesDt.GCItemUnit = objDt.GCDosingUnit;
                entityChargesDt.ParamedicID = Convert.ToInt32(testOrderHd.ParamedicID);

                entityChargesDt.IsVariable = false;
                entityChargesDt.IsUnbilledItem = false;

                entityChargesDt.UsedQuantity = objDt.ResultQty;
                entityChargesDt.ChargedQuantity = objDt.ChargeQty;
                entityChargesDt.BaseQuantity = objDt.ResultQty;

                entityChargesDt.PrescriptionFeeAmount = Convert.ToDecimal(hdnPrescriptionFeeAmount.Value);

                if (objDt.EmbalaceID != null)
                {
                    EmbalaceDt embalace = BusinessLayer.GetEmbalaceDtList(string.Format("EmbalaceID = {0} AND StartingQty <= {1} AND EndingQty >= {1}", objDt.EmbalaceID, objDt.EmbalaceQty)).FirstOrDefault();
                    decimal tariff = 0;

                    if (embalace != null)
                    {
                        tariff = embalace.Tariff;
                    }

                    entityChargesDt.EmbalaceAmount = Convert.ToDecimal(tariff * objDt.EmbalaceQty);
                }
                else
                {
                    entityChargesDt.EmbalaceAmount = 0;
                }
                decimal grossLineAmount = (entityChargesDt.ChargedQuantity * price) + entityChargesDt.EmbalaceAmount + entityChargesDt.PrescriptionFeeAmount;

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
                            entityChargesDt.DiscountPercentageComp1 = discountAmountComp1;
                        }

                        if (priceComp2 > 0)
                        {
                            totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                            entityChargesDt.DiscountPercentageComp2 = discountAmountComp2;
                        }

                        if (priceComp3 > 0)
                        {
                            totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                            entityChargesDt.DiscountPercentageComp3 = discountAmountComp3;
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

                    if (totalDiscountAmount1 > 0)
                    {
                        entityChargesDt.IsDiscountInPercentageComp1 = true;
                    }

                    if (totalDiscountAmount2 > 0)
                    {
                        entityChargesDt.IsDiscountInPercentageComp2 = true;
                    }

                    if (totalDiscountAmount3 > 0)
                    {
                        entityChargesDt.IsDiscountInPercentageComp3 = true;
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
                    totalPayer = total * coverageAmount / 100;
                else
                    totalPayer = coverageAmount * entityChargesDt.ChargedQuantity;
                //                    totalPayer = (coverageAmount * entityChargesDt.ChargedQuantity) + entityChargesDt.EmbalaceAmount + entityChargesDt.PrescriptionFeeAmount;
                //                if (totalPayer > total)
                if (total > 0 && totalPayer > total)
                    totalPayer = total;

                if (objDt.ConversionFactor != 0)
                {
                    entityChargesDt.ConversionFactor = objDt.ConversionFactor;
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

                entityChargesDt.CreatedBy = entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityChargesDtDao.Insert(entityChargesDt);
                #endregion
            }

            retval = entityPatientChargesHd.TransactionNo;

            return result;
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
                if (hdnLstSelected.Value != "")
                    filterExpression = String.Format("PrescriptionOrderDetailID IN ({0}) OR ParentID IN ({0})", hdnLstSelected.Value);
                List<PrescriptionOrderDt> lstEntityDt = BusinessLayer.GetPrescriptionOrderDtList(filterExpression);

                decimal resultTotalAmount = 0;
                decimal resultTotalPatient = 0;
                decimal resultTotalPayer = 0;

                foreach (PrescriptionOrderDt objDt in lstEntityDt)
                {
                    PatientChargesDt entityChargesDt = new PatientChargesDt();

                    #region PatientChargesDt
                    entityChargesDt.ItemID = (int)objDt.ItemID;
                    entityChargesDt.ChargeClassID = Convert.ToInt32(hdnChargeClassID.Value);

                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnVisitID.Value), entityChargesDt.ChargeClassID, (int)objDt.ItemID, 2, DateTime.Now);

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

                    entityChargesDt.IsVariable = false;
                    entityChargesDt.IsUnbilledItem = false;

                    entityChargesDt.UsedQuantity = objDt.ResultQty;
                    entityChargesDt.ChargedQuantity = objDt.ChargeQty;
                    entityChargesDt.BaseQuantity = objDt.ResultQty;

                    entityChargesDt.PrescriptionFeeAmount = Convert.ToDecimal(hdnPrescriptionFeeAmount.Value);

                    if (objDt.EmbalaceID != null)
                    {
                        EmbalaceDt embalace = BusinessLayer.GetEmbalaceDtList(string.Format("EmbalaceID = {0} AND StartingQty <= {1} AND EndingQty >= {1}", objDt.EmbalaceID, objDt.EmbalaceQty)).FirstOrDefault();
                        entityChargesDt.EmbalaceAmount = Convert.ToDecimal(embalace.Tariff * objDt.EmbalaceQty);
                    }
                    else
                    {
                        entityChargesDt.EmbalaceAmount = 0;
                    }
                    decimal grossLineAmount = (entityChargesDt.ChargedQuantity * price) + entityChargesDt.EmbalaceAmount + entityChargesDt.PrescriptionFeeAmount;

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
                                entityChargesDt.DiscountPercentageComp1 = discountAmountComp1;
                            }

                            if (priceComp2 > 0)
                            {
                                totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                entityChargesDt.DiscountPercentageComp2 = discountAmountComp2;
                            }

                            if (priceComp3 > 0)
                            {
                                totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                entityChargesDt.DiscountPercentageComp3 = discountAmountComp3;
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

                        if (totalDiscountAmount1 > 0)
                        {
                            entityChargesDt.IsDiscountInPercentageComp1 = true;
                        }

                        if (totalDiscountAmount2 > 0)
                        {
                            entityChargesDt.IsDiscountInPercentageComp2 = true;
                        }

                        if (totalDiscountAmount3 > 0)
                        {
                            entityChargesDt.IsDiscountInPercentageComp3 = true;
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
                        totalPayer = total * coverageAmount / 100;
                    else
                        totalPayer = (coverageAmount * entityChargesDt.ChargedQuantity) + entityChargesDt.EmbalaceAmount + entityChargesDt.PrescriptionFeeAmount;
                    if (totalPayer > total)
                        totalPayer = total;

                    if (objDt.ConversionFactor != 0)
                    {
                        entityChargesDt.ConversionFactor = objDt.ConversionFactor;
                    }
                    else
                    {
                        entityChargesDt.ConversionFactor = 1;
                    }
                    entityChargesDt.IsCITO = false;
                    entityChargesDt.CITOAmount = 0;
                    entityChargesDt.IsComplication = false;
                    entityChargesDt.ComplicationAmount = 0;
                    //entityChargesDt.IsDiscount = false;
                    //entityChargesDt.DiscountAmount = totalDiscountAmount;
                    //if (entityChargesDt.ChargedQuantity > 0)
                    //{
                    //    entityChargesDt.DiscountComp1 = totalDiscountAmount / entityChargesDt.ChargedQuantity;
                    //}

                    entityChargesDt.IsDiscount = totalDiscountAmount != 0;
                    entityChargesDt.DiscountAmount = totalDiscountAmount;
                    entityChargesDt.DiscountComp1 = totalDiscountAmount1;
                    entityChargesDt.DiscountComp2 = totalDiscountAmount2;
                    entityChargesDt.DiscountComp3 = totalDiscountAmount3;

                    entityChargesDt.PayerAmount = totalPayer;
                    entityChargesDt.PatientAmount = total - totalPayer;

                    entityChargesDt.LineAmount = total;
                    entityChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                    entityChargesDt.IsCreatedBySystem = false;
                    entityChargesDt.CreatedBy = entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    #endregion

                    resultTotalAmount += entityChargesDt.LineAmount;
                    resultTotalPatient += entityChargesDt.PatientAmount;
                    resultTotalPayer += entityChargesDt.PayerAmount;
                }
                retval = string.Format("{0}|{1}|{2}", resultTotalAmount, resultTotalPayer, resultTotalPatient);
                result = true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                retval = string.Format("{0}|{1}|{2}", 0, 0, 0);
                result = false;
            }
            return result;
        }

        #region Bridging to Queue - Methods
        private PrescriptionOrderDTO ConvertOrderToDTO(PrescriptionOrderHd orderHd)
        {
            PrescriptionOrderDTO oData = new PrescriptionOrderDTO();

            oData.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
            oData.PatientID = AppSession.RegisteredPatient.MRN;
            oData.PatientInfo = new PatientData() { PatientID = oData.PatientID, MedicalNo = AppSession.RegisteredPatient.MedicalNo, FullName = AppSession.RegisteredPatient.PatientName };
            oData.VisitID = AppSession.RegisteredPatient.VisitID;
            if (!string.IsNullOrEmpty(hdnVisitInfo.Value))
            {
                string[] visitInfo = hdnVisitInfo.Value.Split(';');
                oData.VisitInformation = new VisitInfo()
                {
                    VisitID = Convert.ToInt32(visitInfo[0]),
                    VisitDate = visitInfo[1],
                    VisitTime = visitInfo[2],
                    PhysicianID = Convert.ToInt32(visitInfo[3]),
                    PhysicianCode = visitInfo[4],
                    PhysicianName = visitInfo[5]
                };
            }
            else
            {
                oData.VisitInformation = new VisitInfo()
                {
                    VisitID = AppSession.RegisteredPatient.VisitID,
                    VisitDate = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112)
                };
            }
            oData.PrescriptionOrderNo = Request.Form[txtPrescriptionOrderNo.UniqueID];
            oData.PrescriptionOrderDate = DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112);
            oData.IsCompound = hdnIsHasCompound.Value == "1" ? true : false;
            return oData;
        }

        private string ConvertVisitToDTO(vConsultVisit consultVisit)
        {
            string strData = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21}",
                consultVisit.VisitID, //0
                consultVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112), //1
                consultVisit.VisitTime, //2
                consultVisit.ParamedicID, //3
                consultVisit.ParamedicCode, //4 
                consultVisit.ParamedicName, //5
                consultVisit.DepartmentID, //6
                consultVisit.ServiceUnitCode, //7
                consultVisit.ServiceUnitName, //8
                consultVisit.RoomID, //9
                consultVisit.RoomCode, //10
                consultVisit.RoomName, //11
                consultVisit.BedCode, //12
                consultVisit.ClassID, //13
                consultVisit.ClassCode, //14
                consultVisit.ClassName, //15
                consultVisit.ChargeClassID, //16
                consultVisit.ChargeClassCode, //17
                consultVisit.ChargeClassName, //18
                consultVisit.VisitTypeID, //19
                consultVisit.VisitTypeCode, //20
                consultVisit.VisitTypeName //21
                );
            return strData;
        }

        private string ConvertOrderHdToDTO(vPrescriptionOrderHd orderHd)
        {
            string strData = string.Format("{0};{1};{2}",
               orderHd.PrescriptionOrderID, //0
               orderHd.PrescriptionOrderNo, //1
               orderHd.PrescriptionDate.ToString(Constant.FormatString.DATE_FORMAT_112) //2
               );
            return strData;
        }

        //private PatientData ConvertPatientToDTO(Patient oPatient)
        //{
        //    PatientData oData = new PatientData();
        //    oData.PatientID = oPatient.MRN;
        //    oData.MedicalNo = oPatient.MedicalNo;
        //    oData.FirstName = oPatient.FirstName;
        //    oData.MiddleName = oPatient.MiddleName;
        //    oData.LastName = oPatient.LastName;
        //    oData.PrefferedName = oPatient.PreferredName;
        //    oData.Gender = string.Format("{0}^{1}", oPatient.GCGender.Substring(5), Request.Form[txtGender.UniqueID]);
        //    oData.Religion = string.Empty;
        //    oData.MaritalStatus = string.Empty;
        //    oData.Nationality = string.Empty;

        //    oData.DateOfBirth = oPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112);
        //    oData.CityOfBirth = oPatient.CityOfBirth;
        //    oData.HomeAddress = Request.Form[txtAddress.UniqueID];
        //    oData.MobileNo1 = oPatient.MobilePhoneNo1;
        //    oData.MobileNo2 = oPatient.MobilePhoneNo2;
        //    oData.EmailAddress = oPatient.EmailAddress;
        //    return oData;
        //}
        #endregion
    }
}