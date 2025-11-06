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
    public partial class PrescriptionReturnOrderNotesCtl : BaseContentPopupCtl
    {
        public override void InitializeControl(string param)
        {
            string[] temp = param.Split('|');
            hdnPrescriptionReturnOrderID.Value = temp[0];
            hdnDepartmentID.Value = temp[1];
            hdnImagingServiceUnitID.Value = temp[2];
            hdnLaboratoryServiceUnitID.Value = temp[3];
            hdnChargesTransactionID.Value = temp[4];

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //1
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1 //2
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsEndingAmountRoundingTo100.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;

            String filterExpression = string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ALASAN_PEMBATALAN_OBAT_RESEP);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);

            List<StandardCode> lstVoidReason = lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.ALASAN_PEMBATALAN_OBAT_RESEP).ToList();
            Methods.SetComboBoxField<StandardCode>(cboVoidReason, lstVoidReason, "StandardCodeName", "StandardCodeID");
            cboVoidReason.SelectedIndex = 0;

            vPrescriptionReturnOrderHd entityOrderHd = BusinessLayer.GetvPrescriptionReturnOrderHdList(string.Format("PrescriptionReturnOrderID = {0}", hdnPrescriptionReturnOrderID.Value)).FirstOrDefault();
            hdnChargesTransactionID.Value = entityOrderHd.TransactionID.ToString();
            txtPrescriptionReturnOrderNo.Text = entityOrderHd.PrescriptionReturnOrderNo;
            txtOrderDate.Text = entityOrderHd.OrderDate.ToString(Constant.FormatString.DATE_FORMAT);
            txtOrderTime.Text = entityOrderHd.OrderTime;
            txtParamedic.Text = string.Format("{0} ({1})", entityOrderHd.ParamedicName, entityOrderHd.ParamedicCode);
            txtTransactionStatus.Text = entityOrderHd.TransactionStatus;
            hdnVisitID.Value = entityOrderHd.VisitID.ToString();

            vConsultVisit2 consultVisit = BusinessLayer.GetvConsultVisit2List(String.Format("VisitID = {0}", hdnVisitID.Value))[0];
            hdnRegistrationID.Value = consultVisit.RegistrationID.ToString();
            hdnChargeClassID.Value = consultVisit.ChargeClassID.ToString();
            BindGrid();

            int locationID = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", entityOrderHd.HealthcareServiceUnitID)).FirstOrDefault().LocationID;
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
                Methods.SetComboBoxField<Location>(cboPrescriptionReturnOrderLocation, lstLocation, "LocationName", "LocationID");
                cboPrescriptionReturnOrderLocation.SelectedIndex = 0;
            }
        }

        private void BindGrid()
        {
            String filterExpression = String.Format("PrescriptionReturnOrderID = {0} AND IsDeleted = 0", hdnPrescriptionReturnOrderID.Value);
            List<vPrescriptionReturnOrderDt> lstEntity = BusinessLayer.GetvPrescriptionReturnOrderDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");
                vPrescriptionReturnOrderDt item = (vPrescriptionReturnOrderDt)e.Item.DataItem;
                if (item.GCPrescriptionReturnOrderStatus != Constant.TestOrderStatus.RECEIVED)
                    chkIsSelected.Visible = false;
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
            else if (param == "decline")
            {
                if (OnDeclineRecord(ref errMessage, ref retval))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }

        private bool OnApproveRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionReturnOrderHdDao PrescriptionReturnOrderHdDao = new PrescriptionReturnOrderHdDao(ctx);
            try
            {
                PrescriptionReturnOrderHd returnOrderHd = PrescriptionReturnOrderHdDao.Get(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value));

                if (hdnChargesTransactionID.Value == "" || hdnChargesTransactionID.Value == "0" || BusinessLayer.GetPatientChargesHd(Convert.ToInt32(hdnChargesTransactionID.Value)).GCTransactionStatus != Constant.TransactionStatus.OPEN)
                {
                    SaveAddRecord(ctx, returnOrderHd, ref retval, ref errMessage);
                }
                else
                {
                    SaveEditRecord(ctx, returnOrderHd, ref retval);
                }

                returnOrderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                returnOrderHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                returnOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                PrescriptionReturnOrderHdDao.Update(returnOrderHd);

                if (String.IsNullOrEmpty(errMessage))
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    string err = string.Format("Order Sudah Di Proses Oleh User lain di nomor transaksi {0}, Silahkan Merefresh halaman ini.", errMessage);
                    errMessage = err;
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
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

        private bool OnDeclineRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            if (String.IsNullOrEmpty(cboVoidReason.Value.ToString()))
            {
                result = false;
                errMessage = "Alasan Pembatalan harus diisi";
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionReturnOrderDtDao oDetailDao = new PrescriptionReturnOrderDtDao(ctx);
            PrescriptionReturnOrderHdDao oHeaderDao = new PrescriptionReturnOrderHdDao(ctx);
            try
            {
                String filterExpressionHd = String.Format("PrescriptionReturnOrderID = {0} AND PrescriptionReturnOrderDtID IN ({1})", hdnPrescriptionReturnOrderID.Value, hdnLstSelected.Value);

                List<PrescriptionReturnOrderDt> lstOrderDt = BusinessLayer.GetPrescriptionReturnOrderDtList(filterExpressionHd);
                if (lstOrderDt.Count > 0)
                {
                    foreach (PrescriptionReturnOrderDt dt in lstOrderDt)
                    {
                        if (dt.GCPrescriptionReturnOrderStatus == Constant.TestOrderStatus.CANCELLED || dt.GCPrescriptionReturnOrderStatus == Constant.TestOrderStatus.IN_PROGRESS)
                        {
                            result = false;
                            errMessage = "Order Sudah Di Proses Oleh User lain, Silahkan Merefresh halaman ini.";
                            break;
                        }
                        dt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.CANCELLED;
                        dt.GCDeleteReason = cboVoidReason.Value.ToString();
                        if (dt.GCDeleteReason.Contains("999")) //Other
                            dt.DeleteReason = txtVoidReason.Text;
                        dt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oDetailDao.Update(dt);
                    }
                }
                else
                {
                    result = false;
                }

                if (result)
                {
                    int dtAllCount = BusinessLayer.GetPrescriptionReturnOrderDtRowCount(string.Format("PrescriptionReturnOrderID = {0} AND IsDeleted = 0", hdnPrescriptionReturnOrderID.Value), ctx);
                    int dtOpenCount = BusinessLayer.GetPrescriptionReturnOrderDtRowCount(string.Format("PrescriptionReturnOrderID = {0} AND GCPrescriptionReturnOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionReturnOrderID.Value, Constant.OrderStatus.RECEIVED), ctx);
                    int dtProcessedCount = BusinessLayer.GetPrescriptionReturnOrderDtRowCount(string.Format("PrescriptionReturnOrderID = {0} AND GCPrescriptionReturnOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionReturnOrderID.Value, Constant.OrderStatus.IN_PROGRESS), ctx);
                    int dtVoidCount = BusinessLayer.GetPrescriptionReturnOrderDtRowCount(string.Format("PrescriptionReturnOrderID = {0} AND GCPrescriptionReturnOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionReturnOrderID.Value, Constant.OrderStatus.CANCELLED), ctx);
                    if (dtProcessedCount > 0)
                    {
                        PrescriptionReturnOrderHd orderHd = oHeaderDao.Get(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value));
                        orderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        orderHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oHeaderDao.Update(orderHd);
                    }
                    else
                    {
                        if (dtVoidCount == dtAllCount)
                        {
                            PrescriptionReturnOrderHd orderHd = oHeaderDao.Get(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value));
                            orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            orderHd.GCOrderStatus = Constant.TestOrderStatus.CANCELLED;
                            orderHd.GCVoidReason = cboVoidReason.Value.ToString();
                            if (orderHd.GCVoidReason.Contains("999")) //Other
                                orderHd.VoidReason = txtVoidReason.Text;
                            orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            oHeaderDao.Update(orderHd);
                        }
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

        private void SaveAddRecord(IDbContext ctx, PrescriptionReturnOrderHd returnOrderHd, ref string retval, ref string errMessage)
        {
            String filterExpression = String.Format("PrescriptionReturnOrderDtID IN ({0}) AND IsDeleted = 0", hdnLstSelected.Value);
            List<vPrescriptionReturnOrderDt> lstEntityDt = BusinessLayer.GetvPrescriptionReturnOrderDtList(filterExpression, ctx);
            PrescriptionReturnOrderHdDao orderReturnHdDao = new PrescriptionReturnOrderHdDao(ctx);
            PrescriptionReturnOrderDtDao orderReturnDtDao = new PrescriptionReturnOrderDtDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesHdDao entityChargesHdDao = new PatientChargesHdDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);

            #region PatientChargesHd
            PatientChargesHd entityPatientChargesHd = new PatientChargesHd();

            entityPatientChargesHd.VisitID = returnOrderHd.VisitID;
            entityPatientChargesHd.TransactionDate = returnOrderHd.OrderDate;
            entityPatientChargesHd.TransactionTime = returnOrderHd.OrderTime;
            entityPatientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
            entityPatientChargesHd.PrescriptionReturnOrderID = returnOrderHd.PrescriptionReturnOrderID;
            entityPatientChargesHd.HealthcareServiceUnitID = returnOrderHd.HealthcareServiceUnitID;
            entityPatientChargesHd.TransactionCode = Constant.TransactionCode.PH_CHARGES;
            entityPatientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(entityPatientChargesHd.TransactionCode, entityPatientChargesHd.TransactionDate, ctx);
            entityPatientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            int transactionID = entityChargesHdDao.InsertReturnPrimaryKeyID(entityPatientChargesHd);
            #endregion

            retval = entityPatientChargesHd.TransactionNo;

            foreach (vPrescriptionReturnOrderDt objDt in lstEntityDt)
            {
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                //string filterCheck = string.Format("PrescriptionReturnOrderDtID = {0} AND IsDeleted = 0 AND GCTransactionStatus != '{1}' AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", objDt.PrescriptionReturnOrderDtID, Constant.TransactionStatus.VOID);
                //List<vPatientChargesDt8> lstCheck = BusinessLayer.GetvPatientChargesDt8List(filterCheck, ctx);

                string filterCheck = string.Format("PrescriptionReturnOrderID = {0} AND GCTransactionStatus != '{1}' AND TransactionID != '{2}'", objDt.PrescriptionReturnOrderID, Constant.TransactionStatus.VOID, transactionID);
                List<PatientChargesHd> lstCheck = BusinessLayer.GetPatientChargesHdList(filterCheck, ctx);
                if (lstCheck.Count <= 0)
                {
                    PatientChargesDt oldChargesDt = entityChargesDtDao.Get(Convert.ToInt32(objDt.PatientChargesDtId));
                    if (oldChargesDt != null)
                    {
                        #region PatientChargesDt
                        PatientChargesDt patientChargesDt = new PatientChargesDt();
                        patientChargesDt.TransactionID = transactionID;
                        patientChargesDt.ItemID = Convert.ToInt32(objDt.ItemID);
                        patientChargesDt.ChargeClassID = oldChargesDt.ChargeClassID;
                        patientChargesDt.PrescriptionOrderDetailID = null;

                        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, returnOrderHd.VisitID, Convert.ToInt32(hdnChargeClassID.Value), Convert.ToInt32(objDt.ItemID), 2, oldChargesDt.CreatedDate, ctx);
                        decimal discountAmount = 0;
                        decimal coverageAmount = 0;
                        decimal price = 0;
                        decimal basePrice = 0;
                        bool isCoverageInPercentage = false;
                        bool isDiscountInPercentage = false;
                        if (list.Count > 0)
                        {
                            GetCurrentItemTariff obj = list[0];
                            discountAmount = obj.DiscountAmount;
                            coverageAmount = obj.CoverageAmount;
                            price = obj.Price;
                            basePrice = obj.BasePrice;
                            isCoverageInPercentage = obj.IsCoverageInPercentage;
                            isDiscountInPercentage = obj.IsDiscountInPercentage;
                        }

                        decimal persentaseRetur = 0;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        string filterSetVarDt = string.Format("HealthcareID = '{0}' AND ParameterCode = '{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_PERSENTASE_RETUR_RESEP);
                        List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVarDt, ctx);
                        if (lstSetVarDt.Count() > 0)
                        {
                            if (lstSetVarDt.FirstOrDefault().ParameterValue != "" && lstSetVarDt.FirstOrDefault().ParameterValue != null)
                            {
                                persentaseRetur = Convert.ToDecimal(lstSetVarDt.FirstOrDefault().ParameterValue);
                            }
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        patientChargesDt.BaseTariff = oldChargesDt.BaseTariff;
                        patientChargesDt.BaseComp1 = oldChargesDt.BaseComp1;
                        patientChargesDt.BaseComp2 = oldChargesDt.BaseComp2;
                        patientChargesDt.BaseComp3 = oldChargesDt.BaseComp3;
                        patientChargesDt.Tariff = oldChargesDt.Tariff * persentaseRetur / 100;
                        patientChargesDt.TariffComp1 = oldChargesDt.TariffComp1 * persentaseRetur / 100;
                        patientChargesDt.TariffComp2 = oldChargesDt.TariffComp2 * persentaseRetur / 100;
                        patientChargesDt.TariffComp3 = oldChargesDt.TariffComp3 * persentaseRetur / 100;
                        patientChargesDt.GCBaseUnit = oldChargesDt.GCBaseUnit;
                        patientChargesDt.GCItemUnit = oldChargesDt.GCItemUnit;

                        patientChargesDt.ParamedicID = Convert.ToInt32(oldChargesDt.ParamedicID);
                        patientChargesDt.IsVariable = false;
                        patientChargesDt.IsUnbilledItem = false;

                        decimal qty = (Convert.ToDecimal(objDt.ItemQty) * -1);
                        decimal totalDiscountAmount = qty * ((oldChargesDt.DiscountAmount * persentaseRetur / 100) / oldChargesDt.ChargedQuantity);
                        decimal grossLineAmount = qty * patientChargesDt.Tariff;

                        decimal total = grossLineAmount - totalDiscountAmount;
                        decimal totalPayer = 0;

                        if (isCoverageInPercentage)
                            totalPayer = total * coverageAmount / 100;
                        else
                            totalPayer = coverageAmount * qty;

                        if (total > 0 && totalPayer > total)
                            totalPayer = total;

                        patientChargesDt.ConversionFactor = 1;

                        patientChargesDt.CostAmount = oldChargesDt.CostAmount;
                        patientChargesDt.AveragePrice = oldChargesDt.AveragePrice;

                        patientChargesDt.IsCITO = false;
                        patientChargesDt.CITOAmount = 0;
                        patientChargesDt.IsComplication = false;
                        patientChargesDt.ComplicationAmount = 0;
                        patientChargesDt.IsDiscount = oldChargesDt.IsDiscount;
                        patientChargesDt.DiscountAmount = totalDiscountAmount * persentaseRetur / 100;
                        patientChargesDt.DiscountComp1 = oldChargesDt.DiscountComp1 * persentaseRetur / 100;
                        patientChargesDt.DiscountComp2 = oldChargesDt.DiscountComp2 * persentaseRetur / 100;
                        patientChargesDt.DiscountComp3 = oldChargesDt.DiscountComp3 * persentaseRetur / 100;
                        patientChargesDt.IsDiscountInPercentageComp1 = oldChargesDt.IsDiscountInPercentageComp1;
                        patientChargesDt.IsDiscountInPercentageComp2 = oldChargesDt.IsDiscountInPercentageComp2;
                        patientChargesDt.IsDiscountInPercentageComp3 = oldChargesDt.IsDiscountInPercentageComp3;
                        patientChargesDt.DiscountPercentageComp1 = oldChargesDt.DiscountPercentageComp1;
                        patientChargesDt.DiscountPercentageComp2 = oldChargesDt.DiscountPercentageComp2;
                        patientChargesDt.DiscountPercentageComp3 = oldChargesDt.DiscountPercentageComp3;

                        patientChargesDt.ReturnChargesTariffPercent = persentaseRetur;

                        patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = qty * -1;

                        decimal oPatientAmount = (total - totalPayer);
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

                        patientChargesDt.PatientAmount = oPatientAmount * -1;
                        patientChargesDt.PayerAmount = oPayerAmount * -1;
                        patientChargesDt.LineAmount = oLineAmount * -1;

                        patientChargesDt.PrescriptionReturnOrderDtID = objDt.PrescriptionReturnOrderDtID;
                        patientChargesDt.LocationID = Convert.ToInt32(cboPrescriptionReturnOrderLocation.Value);
                        patientChargesDt.IsApproved = false;
                        patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                        entityChargesDtDao.Insert(patientChargesDt);
                        #endregion

                        #region PrescriptionReturnOrderDt
                        PrescriptionReturnOrderDt orderDt = orderReturnDtDao.Get(objDt.PrescriptionReturnOrderDtID);
                        orderDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        orderReturnDtDao.Update(orderDt);
                        #endregion
                    }
                }
                else
                {
                    errMessage = lstCheck.FirstOrDefault().TransactionNo;
                    break;
                }
            }

            int orderDtCount = BusinessLayer.GetPrescriptionReturnOrderDtRowCount(string.Format("PrescriptionReturnOrderID = {0} AND GCPrescriptionReturnOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionReturnOrderID.Value, Constant.TestOrderStatus.RECEIVED), ctx);
            if (orderDtCount < 1)
            {
                PrescriptionReturnOrderHd orderHd = orderReturnHdDao.Get(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value));
                orderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                orderHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                orderReturnHdDao.Update(orderHd);
            }
        }

        private void SaveEditRecord(IDbContext ctx, PrescriptionReturnOrderHd returnOrderHd, ref string retval)
        {
            String filterExpression = String.Format("PrescriptionReturnOrderDtID IN ({0}) AND IsDeleted = 0", hdnLstSelected.Value);
            List<vPrescriptionReturnOrderDt> lstEntityDt = BusinessLayer.GetvPrescriptionReturnOrderDtList(filterExpression, ctx);
            PrescriptionReturnOrderHdDao orderReturnHdDao = new PrescriptionReturnOrderHdDao(ctx);
            PrescriptionReturnOrderDtDao orderReturnDtDao = new PrescriptionReturnOrderDtDao(ctx);
            PatientChargesDtDao entityChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesHdDao entityChargesHdDao = new PatientChargesHdDao(ctx);
            ItemMasterDao itemDao = new ItemMasterDao(ctx);
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);

            #region PatientChargesHd
            PatientChargesHd entityPatientChargesHd = entityChargesHdDao.Get(Convert.ToInt32(hdnChargesTransactionID.Value));
            int transactionID = entityPatientChargesHd.TransactionID;
            #endregion

            retval = entityPatientChargesHd.TransactionNo;

            foreach (vPrescriptionReturnOrderDt objDt in lstEntityDt)
            {
                PatientChargesDt oldChargesDt = entityChargesDtDao.Get(Convert.ToInt32(objDt.PatientChargesDtId));
                if (oldChargesDt != null)
                {
                    #region PatientChargesDt
                    PatientChargesDt patientChargesDt = new PatientChargesDt();
                    patientChargesDt.TransactionID = transactionID;
                    patientChargesDt.ItemID = Convert.ToInt32(objDt.ItemID);
                    patientChargesDt.ChargeClassID = Convert.ToInt32(hdnChargeClassID.Value);
                    patientChargesDt.PrescriptionOrderDetailID = null;

                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(registrationID, returnOrderHd.VisitID, Convert.ToInt32(hdnChargeClassID.Value), Convert.ToInt32(objDt.ItemID), 2, oldChargesDt.CreatedDate, ctx);
                    decimal discountAmount = 0;
                    decimal coverageAmount = 0;
                    decimal price = 0;
                    decimal basePrice = 0;
                    bool isCoverageInPercentage = false;
                    bool isDiscountInPercentage = false;
                    if (list.Count > 0)
                    {
                        GetCurrentItemTariff obj = list[0];
                        discountAmount = obj.DiscountAmount;
                        coverageAmount = obj.CoverageAmount;
                        price = obj.Price;
                        basePrice = obj.BasePrice;
                        isCoverageInPercentage = obj.IsCoverageInPercentage;
                        isDiscountInPercentage = obj.IsDiscountInPercentage;
                    }

                    decimal persentaseRetur = 0;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    string filterSetVarDt = string.Format("HealthcareID = '{0}' AND ParameterCode = '{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_PERSENTASE_RETUR_RESEP);
                    List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVarDt, ctx);
                    if (lstSetVarDt.Count() > 0)
                    {
                        if (lstSetVarDt.FirstOrDefault().ParameterValue != "" && lstSetVarDt.FirstOrDefault().ParameterValue != null)
                        {
                            persentaseRetur = Convert.ToDecimal(lstSetVarDt.FirstOrDefault().ParameterValue);
                        }
                    }

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    patientChargesDt.BaseTariff = oldChargesDt.BaseTariff;
                    patientChargesDt.BaseComp1 = oldChargesDt.BaseComp1;
                    patientChargesDt.BaseComp2 = oldChargesDt.BaseComp2;
                    patientChargesDt.BaseComp3 = oldChargesDt.BaseComp3;
                    patientChargesDt.Tariff = oldChargesDt.Tariff * persentaseRetur / 100;
                    patientChargesDt.TariffComp1 = oldChargesDt.TariffComp1 * persentaseRetur / 100;
                    patientChargesDt.TariffComp2 = oldChargesDt.TariffComp2 * persentaseRetur / 100;
                    patientChargesDt.TariffComp3 = oldChargesDt.TariffComp3 * persentaseRetur / 100;
                    patientChargesDt.GCBaseUnit = oldChargesDt.GCBaseUnit;
                    patientChargesDt.GCItemUnit = oldChargesDt.GCItemUnit;

                    patientChargesDt.ParamedicID = Convert.ToInt32(oldChargesDt.ParamedicID);
                    patientChargesDt.IsVariable = false;
                    patientChargesDt.IsUnbilledItem = false;

                    decimal qty = (Convert.ToDecimal(objDt.ItemQty) * -1);
                    decimal totalDiscountAmount = qty * ((oldChargesDt.DiscountAmount * persentaseRetur / 100) / oldChargesDt.ChargedQuantity);
                    decimal grossLineAmount = qty * patientChargesDt.Tariff;

                    decimal total = grossLineAmount - totalDiscountAmount;
                    decimal totalPayer = 0;

                    if (isCoverageInPercentage)
                        totalPayer = total * coverageAmount / 100;
                    else
                        totalPayer = coverageAmount * qty;

                    if (total > 0 && totalPayer > total)
                        totalPayer = total;

                    patientChargesDt.ConversionFactor = 1;

                    patientChargesDt.CostAmount = oldChargesDt.CostAmount;
                    patientChargesDt.AveragePrice = oldChargesDt.AveragePrice;

                    patientChargesDt.IsCITO = false;
                    patientChargesDt.CITOAmount = 0;
                    patientChargesDt.IsComplication = false;
                    patientChargesDt.ComplicationAmount = 0;
                    patientChargesDt.IsDiscount = oldChargesDt.IsDiscount;
                    patientChargesDt.DiscountAmount = totalDiscountAmount * persentaseRetur / 100;
                    patientChargesDt.DiscountComp1 = oldChargesDt.DiscountComp1 * persentaseRetur / 100;
                    patientChargesDt.DiscountComp2 = oldChargesDt.DiscountComp2 * persentaseRetur / 100;
                    patientChargesDt.DiscountComp3 = oldChargesDt.DiscountComp3 * persentaseRetur / 100;
                    patientChargesDt.IsDiscountInPercentageComp1 = oldChargesDt.IsDiscountInPercentageComp1;
                    patientChargesDt.IsDiscountInPercentageComp2 = oldChargesDt.IsDiscountInPercentageComp2;
                    patientChargesDt.IsDiscountInPercentageComp3 = oldChargesDt.IsDiscountInPercentageComp3;
                    patientChargesDt.DiscountPercentageComp1 = oldChargesDt.DiscountPercentageComp1;
                    patientChargesDt.DiscountPercentageComp2 = oldChargesDt.DiscountPercentageComp2;
                    patientChargesDt.DiscountPercentageComp3 = oldChargesDt.DiscountPercentageComp3;

                    patientChargesDt.ReturnChargesTariffPercent = persentaseRetur;

                    patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = qty * -1;

                    decimal oPatientAmount = (total - totalPayer);
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

                    patientChargesDt.PatientAmount = oPatientAmount * -1;
                    patientChargesDt.PayerAmount = oPayerAmount * -1;
                    patientChargesDt.LineAmount = oLineAmount * -1;

                    patientChargesDt.PrescriptionReturnOrderDtID = objDt.PrescriptionReturnOrderDtID;
                    patientChargesDt.LocationID = Convert.ToInt32(cboPrescriptionReturnOrderLocation.Value);
                    patientChargesDt.IsApproved = true;
                    patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                    entityChargesDtDao.Insert(patientChargesDt);
                    #endregion

                    #region PrescriptionReturnOrderDt
                    PrescriptionReturnOrderDt orderDt = orderReturnDtDao.Get(objDt.PrescriptionReturnOrderDtID);
                    orderDt.GCPrescriptionReturnOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                    orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    orderReturnDtDao.Update(orderDt);
                    #endregion
                }
            }

            int orderDtCount = BusinessLayer.GetPrescriptionReturnOrderDtRowCount(string.Format("PrescriptionReturnOrderID = {0} AND GCPrescriptionReturnOrderStatus = '{1}' AND IsDeleted = 0", hdnPrescriptionReturnOrderID.Value, Constant.TestOrderStatus.RECEIVED), ctx);
            if (orderDtCount < 1)
            {
                PrescriptionReturnOrderHd orderHd = orderReturnHdDao.Get(Convert.ToInt32(hdnPrescriptionReturnOrderID.Value));
                orderHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                orderHd.GCOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                orderReturnHdDao.Update(orderHd);
            }
        }
    }
}