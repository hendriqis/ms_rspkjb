using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentDraftTransactionDetailDrugMSCtl : BaseUserControlCtl
    {
        protected bool IsShowSwitchIcon = false;
        private AppointmentBasePageTrxPatientManagement DetailPage
        {
            get { return (AppointmentBasePageTrxPatientManagement)Page; }
        }
        public void InitializeTransactionControl(bool flagHaveCharges)
        {
            if (flagHaveCharges)
            {
                BindGridDrugMS();
                hdnLedDrugMSItemFilterExpression.Value = string.Format("LocationID = [LocationID] AND GCItemType IN ('{1}','{2}') AND ItemID NOT IN (SELECT ItemID FROM PatientChargesDt WHERE TransactionID = {0} AND IsDeleted = 0) AND IsDeleted = 0", DetailPage.GetTransactionHdID(), Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);
            }
            else
                hdnLedDrugMSItemFilterExpression.Value = string.Format("LocationID = [LocationID] AND GCItemType IN ('{0}','{1}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);

            Helper.SetControlEntrySetting(txtDrugMSBaseQty, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSConversion, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSPatient, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSPayer, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSPriceDiscount, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSPriceTariff, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSQtyCharged, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSQtyUsed, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSTotal, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSUnitTariff, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(cboDrugMSChargeClassID, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");

            Registration reg = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
            hdnBusinessPartnerID.Value = reg.BusinessPartnerID.ToString();
        }

        public void SetControlProperties()
        {
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0");
            Methods.SetComboBoxField(cboDrugMSChargeClassID, lstClassCare, "ClassName", "ClassID");

            BindCboLocation();
        }

        public void OnAddRecord()
        {
            IsEditable = true;
            hdnLedDrugMSItemFilterExpression.Value = string.Format("LocationID = [LocationID] AND GCItemType IN ('{0}','{1}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);
            BindGridDrugMS();
            IsEditable = true;
            hdnIsEditable.Value = "1";
        }

        protected void cboDrugMSUoM_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnDrugMSItemID.Value));
            Methods.SetComboBoxField<StandardCode>(cboDrugMSUoM, lst, "StandardCodeName", "StandardCodeID");
            cboDrugMSUoM.SelectedIndex = -1;
        }

        private void BindCboLocation()
        {
            int locationID = DetailPage.GetLocationID();
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
                hdnIsAllowOverIssued.Value = loc.IsAllowOverIssued ? "1" : "0";
                Methods.SetComboBoxField<Location>(cboDrugMSLocation, lstLocation, "LocationName", "LocationID");
                cboDrugMSLocation.SelectedIndex = 0;
            }
        }

        protected void cboDrugMSLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
        }

        protected bool IsEditable = true;
        //        protected int oBusinessPartnerID = 0;
        private void BindGridDrugMS()
        {
            string GCAppointmentStatus = DetailPage.GetAppointmentStatus();
            Appointment entity = BusinessLayer.GetAppointment(DetailPage.GetAppointmentID());
            //            oBusinessPartnerID = entity.BusinessPartnerID;
            IsEditable = (GCAppointmentStatus == "" || GCAppointmentStatus == Constant.AppointmentStatus.STARTED);
            hdnIsEditable.Value = IsEditable ? "1" : "0";

            string filterExpression = "1 = 0";
            string transactionID = DetailPage.GetTransactionHdID();
            if (transactionID != "")
                filterExpression = string.Format("TransactionID = {0} AND GCItemType IN ('{1}','{2}') AND UsedQuantity > -1 AND IsDeleted = 0", transactionID, Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);
            List<vPatientChargesDt1> lst = BusinessLayer.GetvPatientChargesDt1List(filterExpression);
            IsShowSwitchIcon = DetailPage.GetGCCustomerType() != Constant.CustomerType.PERSONAL;
            lvwDrugMS.DataSource = lst;
            lvwDrugMS.DataBind();

            decimal totalPatientAmount = lst.Sum(p => p.PatientAmount);
            decimal totalPayerAmount = lst.Sum(p => p.PayerAmount);
            decimal totalLineAmount = lst.Sum(p => p.LineAmount);
            if (lst.Count > 0)
            {
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotalPayer")).InnerHtml = totalPayerAmount.ToString("N");
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotalPatient")).InnerHtml = totalPatientAmount.ToString("N");
                ((HtmlTableCell)lvwDrugMS.FindControl("tdDrugMSTotal")).InnerHtml = totalLineAmount.ToString("N");
            }
            hdnDrugMSAllTotalPatient.Value = totalPatientAmount.ToString();
            hdnDrugMSAllTotalPayer.Value = totalPayerAmount.ToString();
        }

        #region Drug MS
        protected void cbpDrugMS_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            int transactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnDrugMSTransactionDtID.Value.ToString() != "")
                {
                    transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                    if (OnSaveEditRecordDrugMS(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordDrugMS(ref errMessage, ref transactionID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                    DetailPage.SetTransactionHdID(transactionID.ToString());
                }
            }
            else if (param[0] == "approve")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnApproveChargesDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "void")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnVoidChargesDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteChargesDt(ref errMessage, param[1]))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "switch")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnSwitchChargesDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            BindGridDrugMS();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID.ToString();
        }

        private bool OnSwitchChargesDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                if (transactionID > 0)
                {
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted && !entity.IsApproved)
                        {
                            decimal temp = entity.PayerAmount;
                            entity.PayerAmount = entity.PatientAmount;
                            entity.PatientAmount = temp;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                        result = false;
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnApproveChargesDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                if (transactionID > 0)
                {
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted && !entity.IsApproved)
                        {
                            entity.IsApproved = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                        result = false;
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        private bool OnVoidChargesDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                if (transactionID > 0)
                {
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted && entity.IsApproved)
                        {
                            entity.IsApproved = false;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                        result = false;
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        private bool OnDeleteChargesDt(ref string errMessage, string param)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            string[] paramDelete = param.Split(';');
            int ID = Convert.ToInt32(paramDelete[0]);
            string gcDeleteReason = paramDelete[1];
            string reason = paramDelete[2];
            try
            {
                int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                if (transactionID > 0)
                {
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted && !entity.IsApproved)
                        {
                            entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            entity.IsDeleted = true;
                            entity.GCDeleteReason = gcDeleteReason;
                            entity.DeleteReason = reason;
                            entity.DeleteDate = DateTime.Now;
                            entity.DeleteBy = AppSession.UserLogin.UserID;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                    else
                    {
                        errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                        result = false;
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        public void OnVoidAllChargesDt(IDbContext ctx, int transactionHdID)
        {
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientChargesHd entityHd = entityHdDao.Get(transactionHdID);
            if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                List<vPatientChargesDt> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0} AND GCItemType IN ('{1}','{2}') AND IsDeleted = 0", transactionHdID, Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES), ctx);
                foreach (vPatientChargesDt patientChargesDt in lstPatientChargesDt)
                {
                    PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
                    entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                    entity.IsApproved = false;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entity);
                }
            }
        }

        private void DrugMSControlToEntity(PatientChargesDt entity)
        {
            entity.ParamedicID = DetailPage.GetAppointmentPhysicianID();
            entity.ChargeClassID = Convert.ToInt32(cboDrugMSChargeClassID.Value);
            entity.IsVariable = false;
            entity.TariffComp1 = entity.Tariff = Convert.ToDecimal(Request.Form[txtDrugMSUnitTariff.UniqueID]);
            entity.TariffComp2 = entity.TariffComp3 = 0;
            entity.CostAmount = Convert.ToDecimal(hdnDrugMSCostAmount.Value);
            entity.IsDiscount = false;
            entity.DiscountAmount = Convert.ToDecimal(Request.Form[txtDrugMSPriceDiscount.UniqueID]);
            entity.GCItemUnit = cboDrugMSUoM.Value.ToString();
            entity.IsCITO = false;
            entity.CITOAmount = 0;
            entity.IsComplication = false;
            entity.ComplicationAmount = 0;
            entity.BaseQuantity = Convert.ToDecimal(Request.Form[txtDrugMSBaseQty.UniqueID]);
            entity.UsedQuantity = Convert.ToDecimal(txtDrugMSQtyUsed.Text);
            entity.ChargedQuantity = Convert.ToDecimal(txtDrugMSQtyCharged.Text);
            entity.DiscountComp1 = entity.DiscountAmount / entity.ChargedQuantity;
            entity.PatientAmount = Convert.ToDecimal(Request.Form[txtDrugMSPatient.UniqueID]);
            entity.PayerAmount = Convert.ToDecimal(Request.Form[txtDrugMSPayer.UniqueID]);
            entity.LineAmount = Convert.ToDecimal(Request.Form[txtDrugMSTotal.UniqueID]);
            if (hdnDrugMSConversionValue.Value != "0")
            {
                entity.ConversionFactor = Convert.ToDecimal(hdnDrugMSConversionValue.Value);
            }
            else
            {
                entity.ConversionFactor = 1;
            }
            entity.IsApproved = true;
        }

        private bool OnSaveAddRecordDrugMS(ref string errMessage, ref int transactionID)
        {
            bool result = true;
            decimal itemQty = Convert.ToDecimal(Request.Form[txtDrugMSBaseQty.UniqueID]);
            decimal qtyOnHand = 0;
            string filterExpItemBalance = string.Format("HealthcareID = '{0}' AND LocationID = {1} AND ItemID = {2}", AppSession.UserLogin.HealthcareID, cboDrugMSLocation.Value.ToString(), hdnDrugMSItemID.Value);
            vItemBalance itemBalance = BusinessLayer.GetvItemBalanceList(filterExpItemBalance).FirstOrDefault();
            if (itemBalance != null)
            {
                qtyOnHand = itemBalance.QuantityEND;
                if (qtyOnHand >= itemQty)
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
                    PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
                    try
                    {
                        DetailPage.SaveTransactionHeader(ctx, ref transactionID);
                        PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            PatientChargesDt entityDt = new PatientChargesDt();
                            DrugMSControlToEntity(entityDt);
                            entityDt.ItemID = Convert.ToInt32(hdnDrugMSItemID.Value);
                            entityDt.LocationID = Convert.ToInt32(cboDrugMSLocation.Value);
                            entityDt.BaseComp1 = entityDt.BaseTariff = Convert.ToDecimal(hdnDrugMSBaseTariff.Value);
                            entityDt.BaseComp2 = entityDt.BaseComp3 = 0;
                            entityDt.GCBaseUnit = hdnDrugMSDefaultUoM.Value;
                            entityDt.AveragePrice = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDt.ItemID), ctx).FirstOrDefault().AveragePrice;
                            entityDt.TransactionID = transactionID;
                            entityDt.CreatedBy = entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDt.GCTransactionDetailStatus = entityHd.GCTransactionStatus;
                            entityDtDao.Insert(entityDt);
                        }
                        else
                        {
                            errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                            result = false;
                        }
                        ctx.CommitTransaction();
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
                }
                else
                {
                    result = false;
                    string itemName = Request.Form[txtDrugMSItemName.UniqueID];
                    errMessage = string.Format("Quantity tersedia untuk item {0} tidak mencukupi (Saldo = {1} {2}, Qty Transaksi = {3} {4})", itemName, qtyOnHand.ToString("N2"), cboDrugMSUoM.Text, itemQty.ToString("N2"), cboDrugMSUoM.Text);
                }
            }
            else
            {
                result = false;
                string itemName = Request.Form[txtDrugMSItemName.UniqueID];
                errMessage = string.Format("Item {0} belum terdaftar di Lokasi {1}", itemName, cboDrugMSLocation.Text);
            }
            return result;
        }

        private bool OnSaveEditRecordDrugMS(ref string errMessage)
        {
            bool result = true;
            decimal itemQty = Convert.ToDecimal(Request.Form[txtDrugMSBaseQty.UniqueID]);
            decimal qtyOnHand = 0;
            string filterExpItemBalance = string.Format("HealthcareID = '{0}' AND LocationID = {1} AND ItemID = {2}", AppSession.UserLogin.HealthcareID, cboDrugMSLocation.Value.ToString(), hdnDrugMSItemID.Value);
            vItemBalance itemBalance = BusinessLayer.GetvItemBalanceList(filterExpItemBalance).FirstOrDefault();
            if (itemBalance != null)
            {
                qtyOnHand = itemBalance.QuantityEND;
                if (qtyOnHand >= itemQty)
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
                    PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
                    try
                    {
                        int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                        if (transactionID > 0)
                        {
                            PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                            if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                            {
                                PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnDrugMSTransactionDtID.Value));
                                if (!entityDt.IsDeleted && !entityDt.IsApproved)
                                {
                                    DrugMSControlToEntity(entityDt);
                                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityDtDao.Update(entityDt);
                                }
                            }
                            else
                            {
                                errMessage = "Transaksi Sudah Diproses. Tidak Bisa Diubah";
                                result = false;
                            }
                        }
                        ctx.CommitTransaction();
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
                }
                else
                {
                    result = false;
                    string itemName = Request.Form[txtDrugMSItemName.UniqueID];
                    errMessage = string.Format("Quantity tersedia untuk item {0} tidak mencukupi (Saldo = {1} {2}, Qty Transaksi = {3} {4})", itemName, qtyOnHand.ToString("N2"), cboDrugMSUoM.Text, itemQty.ToString("N2"), cboDrugMSUoM.Text);
                }
            }
            else
            {
                result = false;
                string itemName = Request.Form[txtDrugMSItemName.UniqueID];
                errMessage = string.Format("Item {0} belum terdaftar di Lokasi {1}", itemName, cboDrugMSLocation.Text);
            }
            return result;
        }
        #endregion
    }
}