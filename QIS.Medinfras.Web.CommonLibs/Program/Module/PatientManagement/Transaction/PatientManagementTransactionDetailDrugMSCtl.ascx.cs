using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientManagementTransactionDetailDrugMSCtl : BaseUserControlCtl
    {
        protected bool IsShowSwitchIcon = false;
        private BasePageTrxPatientManagement DetailPage
        {
            get { return (BasePageTrxPatientManagement)Page; }
        }
        public void InitializeTransactionControl(bool flagHaveCharges)
        {
            Registration reg = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
            hdnBusinessPartnerID.Value = reg.BusinessPartnerID.ToString();

            if (flagHaveCharges)
            {
                BindGridDrugMS();
                hdnLedDrugMSItemFilterExpression.Value = string.Format("LocationID = [LocationID] AND GCItemType IN ('{1}','{2}') AND ItemID NOT IN (SELECT ItemID FROM PatientChargesDt WHERE TransactionID = {0} AND IsDeleted = 0) AND IsDeleted = 0", DetailPage.GetTransactionHdID(), Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);
            }
            else
            {
                hdnLedDrugMSItemFilterExpression.Value = string.Format("LocationID = [LocationID] AND GCItemType IN ('{0}','{1}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);
            }

            Helper.SetControlEntrySetting(txtDrugMSBaseQty, new ControlEntrySetting(false, false, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSConversion, new ControlEntrySetting(false, false, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSPatient, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSPayer, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSPriceDiscount, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSPriceTariff, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSQtyCharged, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSQtyUsed, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSTotal, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSUnitTariff, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(cboDrugMSChargeClassID, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(hdnDrugMSItemID, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSItemCode, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");
            Helper.SetControlEntrySetting(txtDrugMSItemName, new ControlEntrySetting(true, true, true), "mpTrxDrugMS");

        }

        public void SetControlProperties()
        {
            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.IM_TRANSAKSI_OBAT_HANYA_TIPE_DISTRIBUSI, //1
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //2
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1, //3
                                                        Constant.SettingParameter.FN_IS_CHARGES_QTY_USING_VALIDATE_2DIGIT //4
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsDrugChargesJustDistribution.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.IM_TRANSAKSI_OBAT_HANYA_TIPE_DISTRIBUSI).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo100.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;
            hdnIsUsingValidateDigitDecimal.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_CHARGES_QTY_USING_VALIDATE_2DIGIT).FirstOrDefault().ParameterValue;

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
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE ItemID = {1} AND IsDeleted = 0) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnDrugMSItemID.Value));
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
                List<Location> lstLocationSelected = new List<Location>();

                if (loc.IsHeader)
                {
                    string filterLocation = string.Format("ParentID = {0} AND IsDeleted = 0 AND LocationID NOT IN (SELECT ISNULL(a.LocationID,0) FROM vLocationFromIM0008 a)", loc.LocationID);
                    lstLocation = BusinessLayer.GetLocationList(filterLocation);

                    List<GetLocationUserList> lstLocationByUser = BusinessLayer.GetLocationUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Empty, string.Empty);
                    if (lstLocationByUser.Count > 0)
                    {
                        foreach (GetLocationUserList locUser in lstLocationByUser)
                        {
                            foreach (Location lc in lstLocation)
                            {
                                if (locUser.LocationID == lc.LocationID)
                                {
                                    lstLocationSelected.Add(lc);
                                }
                            }
                        }

                        Methods.SetComboBoxField<Location>(cboDrugMSLocation, lstLocationSelected, "LocationName", "LocationID");
                    }
                    else
                    {
                        lstLocation = BusinessLayer.GetLocationList(filterLocation);
                        Methods.SetComboBoxField<Location>(cboDrugMSLocation, lstLocation, "LocationName", "LocationID");
                    }

                    cboDrugMSLocation.SelectedIndex = 0;
                }
                else
                {
                    lstLocation = new List<Location>();
                    lstLocation.Add(loc);
                    Methods.SetComboBoxField<Location>(cboDrugMSLocation, lstLocation, "LocationName", "LocationID");
                    cboDrugMSLocation.SelectedIndex = 0;
                }
                hdnIsAllowOverIssued.Value = loc.IsAllowOverIssued ? "1" : "0";
            }
        }

        protected void cboDrugMSLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
        }

        protected bool IsEditable = true;
        protected int oBusinessPartnerID = 0;
        private void BindGridDrugMS()
        {
            string GCTransactionStatus = DetailPage.GetGCTransactionStatus();
            Registration entity = BusinessLayer.GetRegistration(Convert.ToInt32(AppSession.RegisteredPatient.RegistrationID));
            oBusinessPartnerID = entity.BusinessPartnerID;
            if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
            {
                IsEditable = (GCTransactionStatus == "" || GCTransactionStatus == Constant.TransactionStatus.OPEN || GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            }
            else
            {
                IsEditable = (GCTransactionStatus == "" || GCTransactionStatus == Constant.TransactionStatus.OPEN);
            }
            //IsEditable = (GCTransactionStatus == "" || GCTransactionStatus == Constant.TransactionStatus.OPEN);
            hdnIsEditable.Value = IsEditable && !entity.IsLockDown ? "1" : "0";
            IsEditable = entity.IsLockDown ? false : IsEditable;

            string filterExpression = "1 = 0";
            string transactionID = DetailPage.GetTransactionHdID();
            if (transactionID != "")
                filterExpression = string.Format("TransactionID = {0} AND GCItemType IN ('{1}','{2}') AND UsedQuantity > 0 AND IsDeleted = 0", transactionID, Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);
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

            PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID = {0}", Convert.ToInt32(DetailPage.GetTransactionHdID()))).FirstOrDefault();
            if (entityHd != null)
            {
                if (entityHd.IsAIOTransaction)
                {
                    hdnIsAIOTransactionDrugCtl.Value = "1";
                }
                else
                {
                    hdnIsAIOTransactionDrugCtl.Value = "0";
                }

                if (entityHd.ConsultVisitItemPackageID != null && entityHd.ConsultVisitItemPackageID != 0)
                {
                    hdnIsChargesGenerateMCUDrugCtl.Value = "1";
                }
                else
                {
                    hdnIsChargesGenerateMCUDrugCtl.Value = "0";
                }
            }
            else
            {
                hdnIsAIOTransactionDrugCtl.Value = "0";
                hdnIsChargesGenerateMCUDrugCtl.Value = "0";
            }

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
                    bool isAllowSaveDt = false;
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }

                    if (isAllowSaveDt)
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
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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
                    bool isAllowSaveDt = false;
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }

                    if (isAllowSaveDt)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted && !entity.IsApproved)
                        {
                            entity.IsApproved = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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
                    bool isAllowSaveDt = false;
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }

                    if (isAllowSaveDt)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted && entity.IsApproved)
                        {
                            entity.IsApproved = false;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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
                    bool isAllowSaveDt = false;
                    PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                    if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat dihapus lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }
                    else
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            isAllowSaveDt = true;
                        }
                        else
                        {
                            isAllowSaveDt = false;
                            errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat dihapus lagi.", entityHd.TransactionNo);
                            result = false;
                        }
                    }

                    if (isAllowSaveDt)
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
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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

        public void OnVoidAllChargesDt(IDbContext ctx, int transactionHdID)
        {
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientChargesHd entityHd = entityHdDao.Get(transactionHdID);
            bool isAllowSaveDt = false;
            if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
            {
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    isAllowSaveDt = true;
                }
            }
            else
            {
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    isAllowSaveDt = true;
                }
            }

            if (isAllowSaveDt)
            {
                List<vPatientChargesDt1> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDt1List(string.Format("TransactionID = {0} AND GCItemType IN ('{1}','{2}') AND IsDeleted = 0", transactionHdID, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS), ctx);
                foreach (vPatientChargesDt1 patientChargesDt in lstPatientChargesDt)
                {
                    PatientChargesDt entity = entityDtDao.Get(patientChargesDt.ID);
                    if (!entity.IsDeleted)
                    {
                        entity.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                        entity.IsApproved = false;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Update(entity);
                    }
                }
            }
        }

        private void DrugMSControlToEntity(PatientChargesDt entity)
        {
            entity.ParamedicID = DetailPage.GetRegistrationPhysicianID();
            entity.ChargeClassID = Convert.ToInt32(cboDrugMSChargeClassID.Value);
            entity.IsVariable = false;
            entity.TariffComp1 = entity.Tariff = Convert.ToDecimal(Request.Form[txtDrugMSUnitTariff.UniqueID]);
            entity.TariffComp2 = entity.TariffComp3 = 0;
            entity.GCItemUnit = cboDrugMSUoM.Value.ToString();
            entity.IsCITO = false;
            entity.CITOAmount = 0;
            entity.IsComplication = false;
            entity.ComplicationAmount = 0;
            entity.BaseQuantity = Convert.ToDecimal(Request.Form[txtDrugMSBaseQty.UniqueID]);
            entity.UsedQuantity = Convert.ToDecimal(txtDrugMSQtyUsed.Text);
            entity.ChargedQuantity = Convert.ToDecimal(txtDrugMSQtyCharged.Text);
            entity.DiscountAmount = Convert.ToDecimal(Request.Form[txtDrugMSPriceDiscount.UniqueID]);
            entity.DiscountComp1 = entity.DiscountAmount / entity.ChargedQuantity;
            entity.IsDiscount = entity.DiscountAmount != 0 ? true : false;

            if (hdnDrugMSConversionValue.Value != "0")
            {
                entity.ConversionFactor = Convert.ToDecimal(hdnDrugMSConversionValue.Value);
            }
            else
            {
                entity.ConversionFactor = 1;
            }

            decimal oPatientAmount = Convert.ToDecimal(Request.Form[txtDrugMSPatient.UniqueID]);
            decimal oPayerAmount = Convert.ToDecimal(Request.Form[txtDrugMSPayer.UniqueID]);
            decimal oLineAmount = Convert.ToDecimal(Request.Form[txtDrugMSTotal.UniqueID]);

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

            entity.PatientAmount = oPatientAmount;
            entity.PayerAmount = oPayerAmount;
            entity.LineAmount = oLineAmount;

            entity.IsApproved = true;
        }

        private bool OnSaveAddRecordDrugMS(ref string errMessage, ref int transactionID)
        {
            bool result = true;
            decimal itemQty = Convert.ToDecimal(Request.Form[txtDrugMSQtyUsed.UniqueID]);
            decimal qtyOnHand = 0;

            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            ItemProductDao iProductDao = new ItemProductDao(ctx);
            try
            {
                string filterExpItemBalance = string.Format("LocationID = {0} AND ItemID = {1} AND IsDeleted = 0", cboDrugMSLocation.Value.ToString(), hdnDrugMSItemID.Value);
                ItemBalance itemBalance = BusinessLayer.GetItemBalanceList(filterExpItemBalance, ctx).FirstOrDefault();
                if (itemBalance != null)
                {
                    qtyOnHand = itemBalance.QuantityEND;
                    if (qtyOnHand >= itemQty)
                    {
                        DetailPage.SaveTransactionHeader(ctx, ref transactionID);
                        PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                        bool isAllowSaveDt = false;
                        if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                        {
                            if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                            {
                                isAllowSaveDt = true;
                            }
                            else
                            {
                                isAllowSaveDt = false;
                                errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                                result = false;
                            }
                        }
                        else
                        {
                            if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                            {
                                isAllowSaveDt = true;
                            }
                            else
                            {
                                isAllowSaveDt = false;
                                errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                                result = false;
                            }
                        }

                        if (isAllowSaveDt)
                        {
                            PatientChargesDt entityDt = new PatientChargesDt();
                            DrugMSControlToEntity(entityDt);
                            entityDt.ItemID = Convert.ToInt32(hdnDrugMSItemID.Value);
                            entityDt.LocationID = Convert.ToInt32(cboDrugMSLocation.Value);
                            entityDt.BaseComp1 = entityDt.BaseTariff = Convert.ToDecimal(hdnDrugMSBaseTariff.Value);
                            entityDt.BaseComp2 = entityDt.BaseComp3 = 0;
                            entityDt.GCBaseUnit = hdnDrugMSDefaultUoM.Value;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            ItemPlanning iPlanning = BusinessLayer.GetItemPlanningList(string.Format("HealthcareID = '{0}' AND ItemID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDt.ItemID), ctx).FirstOrDefault();
                            entityDt.AveragePrice = iPlanning.AveragePrice;
                            entityDt.CostAmount = iPlanning.UnitPrice;

                            if (entityDt.ItemID != null && entityDt.ItemID != 0)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                ItemProduct iProduct = iProductDao.Get(entityDt.ItemID);
                                entityDt.HETAmount = iProduct.HETAmount;
                            }

                            entityDt.TransactionID = transactionID;
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                            entityDt.GCTransactionDetailStatus = entityHd.GCTransactionStatus;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityDtDao.Insert(entityDt);
                        }
                    }
                    else
                    {
                        result = false;
                        string itemName = Request.Form[txtDrugMSItemName.UniqueID];
                        errMessage = string.Format("Quantity tersedia untuk item {0} tidak mencukupi (Saldo = {1} {2}, Qty Transaksi = {3} {4})", itemName, qtyOnHand.ToString(Constant.FormatString.NUMERIC_2), cboDrugMSUoM.Text, itemQty.ToString(Constant.FormatString.NUMERIC_2), cboDrugMSUoM.Text);
                    }
                }
                else
                {
                    result = false;
                    string itemName = Request.Form[txtDrugMSItemName.UniqueID];
                    errMessage = string.Format("Item {0} belum terdaftar di Lokasi {1}", itemName, cboDrugMSLocation.Text);
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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

        private bool OnSaveEditRecordDrugMS(ref string errMessage)
        {
            bool result = true;
            decimal itemQty = Convert.ToDecimal(Request.Form[txtDrugMSBaseQty.UniqueID]);
            decimal qtyOnHand = 0;

            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                string filterExpItemBalance = string.Format("LocationID = {0} AND ItemID = {1} AND IsDeleted = 0", cboDrugMSLocation.Value.ToString(), hdnDrugMSItemID.Value);
                ItemBalance itemBalance = BusinessLayer.GetItemBalanceList(filterExpItemBalance, ctx).FirstOrDefault();
                if (itemBalance != null)
                {
                    qtyOnHand = itemBalance.QuantityEND;
                    if (qtyOnHand >= itemQty)
                    {
                        int transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                        if (transactionID > 0)
                        {
                            PatientChargesHd entityHd = entityHdDao.Get(transactionID);
                            bool isAllowSaveDt = false;
                            if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
                            {
                                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                                {
                                    isAllowSaveDt = true;
                                }
                                else
                                {
                                    isAllowSaveDt = false;
                                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                                    result = false;
                                }
                            }
                            else
                            {
                                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    isAllowSaveDt = true;
                                }
                                else
                                {
                                    isAllowSaveDt = false;
                                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak dapat diubah lagi.", entityHd.TransactionNo);
                                    result = false;
                                }
                            }

                            if (isAllowSaveDt)
                            {
                                PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnDrugMSTransactionDtID.Value));
                                if (!entityDt.IsDeleted && !entityDt.IsApproved)
                                {
                                    DrugMSControlToEntity(entityDt);
                                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityDtDao.Update(entityDt);
                                }
                            }
                        }
                    }
                    else
                    {
                        result = false;
                        string itemName = Request.Form[txtDrugMSItemName.UniqueID];
                        errMessage = string.Format("Quantity tersedia untuk item {0} tidak mencukupi (Saldo = {1} {2}, Qty Transaksi = {3} {4})", itemName, qtyOnHand.ToString(Constant.FormatString.NUMERIC_2), cboDrugMSUoM.Text, itemQty.ToString(Constant.FormatString.NUMERIC_2), cboDrugMSUoM.Text);
                    }
                }
                else
                {
                    result = false;
                    string itemName = Request.Form[txtDrugMSItemName.UniqueID];
                    errMessage = string.Format("Item {0} belum terdaftar di Lokasi {1}", itemName, cboDrugMSLocation.Text);
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
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
        #endregion
    }
}