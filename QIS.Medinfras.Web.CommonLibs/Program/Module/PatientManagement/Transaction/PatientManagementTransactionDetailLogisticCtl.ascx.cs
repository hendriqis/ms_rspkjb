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
    public partial class PatientManagementTransactionDetailLogisticCtl : BaseUserControlCtl
    {
        protected bool IsShowSwitchIcon = false;
        private BasePageTrxPatientManagement DetailPage
        {
            get { return (BasePageTrxPatientManagement)Page; }
        }
        public void InitializeTransactionControl(bool flagHaveCharges)
        {
            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //1
                                                        Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1, //2
                                                        Constant.SettingParameter.FN_IS_CHARGES_QTY_USING_VALIDATE_2DIGIT //3
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsEndingAmountRoundingTo100.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;
            hdnIsUsingValidateDigitDecimal.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_CHARGES_QTY_USING_VALIDATE_2DIGIT).FirstOrDefault().ParameterValue;

            if (flagHaveCharges)
            {
                BindGridLogistic();
                hdnLedLogisticItemFilterExpression.Value = string.Format("LocationID = [LocationID] AND GCItemType IN ('{1}','{2}') AND ItemID NOT IN (SELECT ItemID FROM PatientChargesDt WHERE TransactionID = {0} AND IsDeleted = 0) AND IsChargeToPatient = 1 AND IsDeleted = 0", DetailPage.GetTransactionHdID(), Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
            }
            else
            {
                hdnLedLogisticItemFilterExpression.Value = string.Format("LocationID = [LocationID] AND GCItemType IN ('{1}','{2}') AND IsChargeToPatient = 1 AND IsDeleted = 0", Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
            }

            Helper.SetControlEntrySetting(txtLogisticBaseQty, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(txtLogisticConversion, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(txtLogisticPatient, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(txtLogisticPayer, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(txtLogisticPriceDiscount, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(txtLogisticPriceTariff, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(txtLogisticQtyCharged, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(txtLogisticQtyUsed, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(txtLogisticTotal, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(txtLogisticUnitTariff, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(cboLogisticChargeClassID, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(hdnLogisticItemID, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(txtLogisticItemCode, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
            Helper.SetControlEntrySetting(txtLogisticItemName, new ControlEntrySetting(true, true, true), "mpTrxLogistic");
        }

        public void SetControlProperties()
        {
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0");
            Methods.SetComboBoxField(cboLogisticChargeClassID, lstClassCare, "ClassName", "ClassID");

            BindCboLocation();
        }

        public void OnAddRecord()
        {
            IsEditable = true;
            hdnLedLogisticItemFilterExpression.Value = string.Format("LocationID = [LocationID] AND GCItemType IN ('{0}','{1}') AND IsDeleted = 0 AND IsChargeToPatient = 1", Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
            BindGridLogistic();
            IsEditable = true;
            hdnIsEditable.Value = "1";
        }

        protected void cboLogisticLocation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindCboLocation();
        }

        private void BindCboLocation()
        {
            int locationID = DetailPage.GetLogisticLocationID();
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

                        Methods.SetComboBoxField<Location>(cboLogisticLocation, lstLocationSelected, "LocationName", "LocationID");
                    }
                    else
                    {
                        lstLocation = BusinessLayer.GetLocationList(filterLocation);
                        Methods.SetComboBoxField<Location>(cboLogisticLocation, lstLocation, "LocationName", "LocationID");
                    }

                    cboLogisticLocation.SelectedIndex = 0;
                }
                else
                {
                    lstLocation = new List<Location>();
                    lstLocation.Add(loc);
                    Methods.SetComboBoxField<Location>(cboLogisticLocation, lstLocation, "LocationName", "LocationID");
                    cboLogisticLocation.SelectedIndex = 0;
                }
            }
        }

        protected void cboLogisticUoM_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnLogisticItemID.Value));
            Methods.SetComboBoxField<StandardCode>(cboLogisticUoM, lst, "StandardCodeName", "StandardCodeID");
            cboLogisticUoM.SelectedIndex = -1;
        }

        protected bool IsEditable = true;
        private void BindGridLogistic()
        {
            string GCTransactionStatus = DetailPage.GetGCTransactionStatus();

            Registration entity = BusinessLayer.GetRegistration(Convert.ToInt32(AppSession.RegisteredPatient.RegistrationID));

            if (DetailPage.IsPatientBillSummaryPage() && AppSession.IsAdminCanCancelAllTransaction)
            {
                IsEditable = (GCTransactionStatus == "" || GCTransactionStatus == Constant.TransactionStatus.OPEN || GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            }
            else
            {
                IsEditable = (GCTransactionStatus == "" || GCTransactionStatus == Constant.TransactionStatus.OPEN);
            }

            hdnIsEditable.Value = IsEditable && !entity.IsLockDown ? "1" : "0";
            IsEditable = entity.IsLockDown ? false : IsEditable;

            string filterExpression = "1 = 0";
            string transactionID = DetailPage.GetTransactionHdID();
            if (transactionID != "")
            {
                filterExpression = string.Format("TransactionID = {0} AND UsedQuantity >= 0 AND GCItemType IN ('{1}','{2}') AND IsDeleted = 0", transactionID, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
            }
            List<vPatientChargesDt1> lst = BusinessLayer.GetvPatientChargesDt1List(filterExpression);
            IsShowSwitchIcon = DetailPage.GetGCCustomerType() != Constant.CustomerType.PERSONAL;
            lvwLogistic.DataSource = lst;
            lvwLogistic.DataBind();

            decimal totalPatientAmount = lst.Sum(p => p.PatientAmount);
            decimal totalPayerAmount = lst.Sum(p => p.PayerAmount);
            decimal totalLineAmount = lst.Sum(p => p.LineAmount);
            if (lst.Count > 0)
            {
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotalPayer")).InnerHtml = totalPayerAmount.ToString("N");
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotalPatient")).InnerHtml = totalPatientAmount.ToString("N");
                ((HtmlTableCell)lvwLogistic.FindControl("tdLogisticTotal")).InnerHtml = totalLineAmount.ToString("N");
            }
            hdnLogisticAllTotalPatient.Value = totalPatientAmount.ToString();
            hdnLogisticAllTotalPayer.Value = totalPayerAmount.ToString();

            PatientChargesHd entityHd = BusinessLayer.GetPatientChargesHdList(string.Format("TransactionID = {0}", Convert.ToInt32(DetailPage.GetTransactionHdID()))).FirstOrDefault();
            if (entityHd != null)
            {
                if (entityHd.IsAIOTransaction)
                {
                    hdnIsAIOTransactionLogisticCtl.Value = "1";
                }
                else
                {
                    hdnIsAIOTransactionLogisticCtl.Value = "0";
                }

                if (entityHd.ConsultVisitItemPackageID != null && entityHd.ConsultVisitItemPackageID != 0)
                {
                    hdnIsChargesGenerateMCULogisticCtl.Value = "1";
                }
                else
                {
                    hdnIsChargesGenerateMCULogisticCtl.Value = "0";
                }
            }
            else
            {
                hdnIsAIOTransactionLogisticCtl.Value = "0";
                hdnIsChargesGenerateMCULogisticCtl.Value = "0";
            }

        }

        #region Logistic
        protected void cbpLogistic_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            int transactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnLogisticTransactionDtID.Value.ToString() != "")
                {
                    transactionID = Convert.ToInt32(DetailPage.GetTransactionHdID());
                    if (OnSaveEditRecordLogistic(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordLogistic(ref errMessage, ref transactionID))
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
            BindGridLogistic();

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
                List<vPatientChargesDt1> lstPatientChargesDt = BusinessLayer.GetvPatientChargesDt1List(string.Format("TransactionID = {0} AND GCItemType IN ('{1}','{2}','{3}') AND IsDeleted = 0", transactionHdID, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM), ctx);
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

        private void LogisticControlToEntity(PatientChargesDt entity)
        {
            entity.ParamedicID = DetailPage.GetRegistrationPhysicianID();
            entity.ChargeClassID = Convert.ToInt32(cboLogisticChargeClassID.Value);
            entity.IsVariable = false;
            entity.TariffComp1 = entity.Tariff = Convert.ToDecimal(Request.Form[txtLogisticUnitTariff.UniqueID]);
            entity.TariffComp2 = entity.TariffComp3 = 0;
            //entity.CostAmount = Convert.ToDecimal(hdnLogisticCostAmount.Value); -> ditutup pada 20190426 karna akan diisi harga satuan kecil dari ItemPlanning di bawah
            entity.IsDiscount = false;
            entity.DiscountAmount = Convert.ToDecimal(Request.Form[txtLogisticPriceDiscount.UniqueID]);
            entity.GCItemUnit = cboLogisticUoM.Value.ToString();
            entity.IsCITO = false;
            entity.CITOAmount = 0;
            entity.IsComplication = false;
            entity.ComplicationAmount = 0;
            entity.BaseQuantity = Convert.ToDecimal(Request.Form[txtLogisticBaseQty.UniqueID]);
            entity.UsedQuantity = Convert.ToDecimal(txtLogisticQtyUsed.Text);
            entity.ChargedQuantity = Convert.ToDecimal(txtLogisticQtyCharged.Text);

            if (hdnLogisticConversionValue.Value != "0")
            {
                entity.ConversionFactor = Convert.ToDecimal(hdnLogisticConversionValue.Value);
            }
            else
            {
                entity.ConversionFactor = 1;
            }

            decimal oPatientAmount = Convert.ToDecimal(Request.Form[txtLogisticPatient.UniqueID]);
            decimal oPayerAmount = Convert.ToDecimal(Request.Form[txtLogisticPayer.UniqueID]);
            decimal oLineAmount = Convert.ToDecimal(Request.Form[txtLogisticTotal.UniqueID]);

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

        private bool OnSaveAddRecordLogistic(ref string errMessage, ref int transactionID)
        {
            bool result = true;
            decimal itemQty = Convert.ToDecimal(Request.Form[txtLogisticQtyUsed.UniqueID]);
            decimal qtyOnHand = 0;

            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            ItemProductDao iProductDao = new ItemProductDao(ctx);
            try
            {
                string filterExpItemBalance = string.Format("LocationID = {0} AND ItemID = {1} AND IsDeleted = 0", cboLogisticLocation.Value.ToString(), hdnLogisticItemID.Value);
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
                            LogisticControlToEntity(entityDt);
                            entityDt.ItemID = Convert.ToInt32(hdnLogisticItemID.Value);
                            entityDt.LocationID = Convert.ToInt32(cboLogisticLocation.Value);
                            entityDt.BaseComp1 = entityDt.BaseTariff = Convert.ToDecimal(hdnLogisticBaseTariff.Value);
                            entityDt.BaseComp2 = entityDt.BaseComp3 = 0;
                            entityDt.GCBaseUnit = hdnLogisticDefaultUoM.Value;

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
                        string itemName = Request.Form[txtLogisticItemName.UniqueID];
                        errMessage = string.Format("Quantity tersedia untuk item {0} tidak mencukupi (Saldo = {1} {2}, Qty Transaksi = {3} {4})", itemName, qtyOnHand.ToString(Constant.FormatString.NUMERIC_2), cboLogisticUoM.Text, itemQty.ToString(Constant.FormatString.NUMERIC_2), cboLogisticUoM.Text);
                    }
                }
                else
                {
                    result = false;
                    string itemName = Request.Form[txtLogisticItemName.UniqueID];
                    errMessage = string.Format("Item {0} belum terdaftar di Lokasi {1}", itemName, cboLogisticLocation.Text);
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

        private bool OnSaveEditRecordLogistic(ref string errMessage)
        {
            bool result = true;
            decimal itemQty = Convert.ToDecimal(Request.Form[txtLogisticBaseQty.UniqueID]);
            decimal qtyOnHand = 0;

            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                string filterExpItemBalance = string.Format("LocationID = {0} AND ItemID = {1} AND IsDeleted = 0", cboLogisticLocation.Value.ToString(), hdnLogisticItemID.Value);
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
                                PatientChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnLogisticTransactionDtID.Value));
                                if (!entityDt.IsDeleted && !entityDt.IsApproved)
                                {
                                    LogisticControlToEntity(entityDt);
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
                        string itemName = Request.Form[txtLogisticItemName.UniqueID];
                        errMessage = string.Format("Quantity tersedia untuk item {0} tidak mencukupi (Saldo = {1} {2}, Qty Transaksi = {3} {4})", itemName, qtyOnHand.ToString(Constant.FormatString.NUMERIC_2), cboLogisticUoM.Text, itemQty.ToString(Constant.FormatString.NUMERIC_2), cboLogisticUoM.Text);
                    }
                }
                else
                {
                    result = false;
                    string itemName = Request.Form[txtLogisticItemName.UniqueID];
                    errMessage = string.Format("Item {0} belum terdaftar di Lokasi {1}", itemName, cboLogisticLocation.Text);
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