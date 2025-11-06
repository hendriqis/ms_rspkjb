using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARInvoicePayerReceiveEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AR_INVOICE_PAYER_RECEIVE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowVoid = IsAllowProposed = false;
        }

        protected string OnGetCustomerFilterExpression()
        {
            return string.Format("IsDeleted = 0");
        }

        protected override void InitializeDataControl()
        {
            hdnBusinessPartnerID.Value = AppSession.BusinessPartnerID.ToString();

            bool IsAllowAdd, IsAllowSave, IsAllowVoid, IsAllowNextPrev;
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
            SetToolbarVisibility(ref IsAllowAdd, ref IsAllowSave, ref IsAllowVoid, ref IsAllowNextPrev);

            InitializeControlProperties();
        }

        private void InitializeControlProperties()
        {
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                AppSession.UserLogin.HealthcareID, //0
                                                Constant.SettingParameter.FN_IS_ARI_DATE_ALLOW_BACKDATE //1
                                            ));

            hdnIsAllowBackdateARI.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_ARI_DATE_ALLOW_BACKDATE).ParameterValue;

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoidByReason.Value = CRUDMode.Contains("X") ? "1" : "0";

            hdnCreditCardFeeFilterExpression.Value = string.Format("HealthcareID = '{0}' AND GCCardType = '[GCCardType]' AND GCCardProvider = '[GCCardProvider]' AND EDCMachineID = [EDCMachineID]", AppSession.UserLogin.HealthcareID);

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList("IsDeleted = 0 AND IsActive = 1");
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboCashierGroup, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.CASHIER_GROUP || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboShift, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.SHIFT || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            List<EDCMachine> lstEDCMachine = BusinessLayer.GetEDCMachineList("IsDeleted = 0");
            Methods.SetComboBoxField<EDCMachine>(cboEDCMachine, lstEDCMachine, "EDCMachineName", "EDCMachineID");
            cboEDCMachine.SelectedIndex = 0;

            //List<Bank> lstBank = BusinessLayer.GetBankList(string.Format("IsDeleted = 0 AND GCBankType = '{0}'", Constant.BankType.BANK_PIUTANG));
            //if (lstBank.Count > 0)
            //{
            //    Methods.SetComboBoxField<Bank>(cboBank, lstBank, "BankName", "BankID");
            //}
            //else
            //{
            //    List<Bank> lstBankAll = BusinessLayer.GetBankList(string.Format("IsDeleted = 0 AND GCBankType IS NULL"));
            //    Methods.SetComboBoxField<Bank>(cboBank, lstBankAll, "BankName", "BankID");
            //}
            //cboBank.SelectedIndex = 0;

            List<Bank> lstBank = BusinessLayer.GetBankList(string.Format("IsDeleted = 0 AND (GCBankType = '{0}' OR GCBankType IS NULL)", Constant.BankType.BANK_KASIR));
            Methods.SetComboBoxField<Bank>(cboBank, lstBank, "BankName", "BankID");
            cboBank.SelectedIndex = 0;

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(String.Format(
                "ParentID IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0",
                Constant.StandardCode.PAYMENT_TYPE, Constant.StandardCode.AR_PAYMENT_METHOD, Constant.StandardCode.CARD_TYPE, Constant.StandardCode.CARD_PROVIDER));

            Methods.SetComboBoxField<StandardCode>(cboPaymentMethod, lstSc.Where(p => p.ParentID == Constant.StandardCode.AR_PAYMENT_METHOD).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCardType, lstSc.Where(p => p.ParentID == Constant.StandardCode.CARD_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCardProvider, lstSc.Where(p => p.ParentID == Constant.StandardCode.CARD_PROVIDER).ToList(), "StandardCodeName", "StandardCodeID");

            //tdARPaymentMethod.InnerHtml = lstSc.FirstOrDefault(p => p.StandardCodeID == Constant.PaymentMethod.CREDIT).StandardCodeName;

            cboPaymentMethod.SelectedIndex = 0;
            cboCardType.SelectedIndex = 0;

            cboCardDateMonth.DataSource = Enumerable.Range(1, 12).Select(a => new
            {
                MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                MonthNumber = a
            });
            cboCardDateMonth.TextField = "MonthName";
            cboCardDateMonth.ValueField = "MonthNumber";
            cboCardDateMonth.EnableCallbackMode = false;
            cboCardDateMonth.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboCardDateMonth.DropDownStyle = DropDownStyle.DropDownList;
            cboCardDateMonth.DataBind();

            cboCardDateYear.DataSource = Enumerable.Range(DateTime.Now.Year, 10);
            cboCardDateYear.EnableCallbackMode = false;
            cboCardDateYear.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboCardDateYear.DropDownStyle = DropDownStyle.DropDownList;
            cboCardDateYear.DataBind();

            OnAddRecord();

        }

        protected override void SetControlProperties()
        {
            ListView lvwARInvoice = (ListView)ddeInvoiceNo.FindControl("lvwInvoice");
            string filter = string.Format("BusinessPartnerID = {0} AND (GCTransactionStatus = '{1}' OR GCTransactionStatus = '{2}')", AppSession.BusinessPartnerID, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED);
            List<ARInvoiceHd> lst = BusinessLayer.GetARInvoiceHdList(filter);

            lvwARInvoice.DataSource = lst;
            lvwARInvoice.DataBind();

            Helper.SetControlEntrySetting(cboCardType, new ControlEntrySetting(true, true, true), "vgCardInformation");
            Helper.SetControlEntrySetting(cboCardProvider, new ControlEntrySetting(true, true, true), "vgCardInformation");
            Helper.SetControlEntrySetting(txtCardNumber4, new ControlEntrySetting(true, true, true), "vgCardInformation");
            Helper.SetControlEntrySetting(txtHolderName, new ControlEntrySetting(true, true, true), "vgCardInformation");
            Helper.SetControlEntrySetting(cboCardDateMonth, new ControlEntrySetting(true, true, false), "vgCardInformation");
            Helper.SetControlEntrySetting(cboCardDateYear, new ControlEntrySetting(true, true, false), "vgCardInformation");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnARReceivingID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtARReceivingNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtReceivingDate, new ControlEntrySetting((hdnIsAllowBackdateARI.Value == "1"), false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(hdnTransactionStatus, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(cboCashierGroup, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboShift, new ControlEntrySetting(true, false, false));

            SetControlEntrySetting(txtInvoiceNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(ddeInvoiceNo, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtRemainingTotal, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(txtPaymentAmount, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtCashbackAmount, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(cboCardType, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboCardProvider, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtCardNumber4, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(txtHolderName, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboCardDateMonth, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(cboCardDateYear, new ControlEntrySetting(true, false, false));
        }

        #region Load Entity
        private void BindGrdARReceivingDetail()
        {
            List<vARReceivingDt> lstDt = BusinessLayer.GetvARReceivingDtList(string.Format("ARReceivingID = {0}", hdnARReceivingID.Value));
            lvwARReceivingDt.DataSource = lstDt;
            lvwARReceivingDt.DataBind();

            decimal paymentAmount = lstDt.Select(p => p.PaymentAmount).Sum();
            decimal cardFeeAmount = lstDt.Select(p => p.CardFeeAmount).Sum();

            tdTotalPaymentEdit.InnerHtml = paymentAmount.ToString("N");
            tdTotalCardFeeEdit.InnerHtml = cardFeeAmount.ToString("N");
            tdLineTotalEdit.InnerHtml = (paymentAmount + cardFeeAmount).ToString("N");
            hdnTotalPaymentAmount.Value = paymentAmount.ToString();

        }

        protected void cbpARReceivingDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGrdARReceivingDetail();
        }

        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("BusinessPartnerID = {0}", hdnBusinessPartnerID.Value);
            return BusinessLayer.GetvARReceivingHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("BusinessPartnerID = {0}", hdnBusinessPartnerID.Value);
            vARReceivingHd entity = BusinessLayer.GetvARReceivingHd(filterExpression, PageIndex, "ARReceivingID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("BusinessPartnerID = {0}", hdnBusinessPartnerID.Value);
            PageIndex = BusinessLayer.GetvARReceivingHdRowIndex(filterExpression, keyValue, "ARReceivingID DESC");
            vARReceivingHd entity = BusinessLayer.GetvARReceivingHd(filterExpression, PageIndex, "ARReceivingID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vARReceivingHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            hdnARReceivingID.Value = entity.ARReceivingID.ToString();
            hdnTransactionStatus.Value = entity.GCTransactionStatus;
            txtARReceivingNo.Text = entity.ARReceivingNo;
            //txtReceivingDate.Text = entity.ReceivingDateInString;
            txtReceivingDate.Text = entity.ReceivingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtInvoiceNo.Text = entity.ARInvoiceNo;
            txtRemarks.Text = entity.Remarks;
            cboCashierGroup.Value = entity.GCCashierGroup;
            cboShift.Value = entity.GCShift;

            txtVoucherNo.Text = entity.VoucherNo;
            if (entity.VoucherDate != null && entity.VoucherDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                txtVoucherDate.Text = entity.VoucherDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }

            txtRemainingTotal.Text = entity.TotalInvoiceAmount.ToString();
            txtPaymentAmount.Text = entity.cfReceiveAmount.ToString();
            txtCashbackAmount.Text = entity.CashBackAmount.ToString();

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);

            if (entity.ApprovedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trApprovedBy.Style.Add("display", "none");
                trApprovedDate.Style.Add("display", "none");

                divApprovedBy.InnerHtml = "";
                divApprovedDate.InnerHtml = "";
            }
            else
            {
                trApprovedBy.Style.Remove("display");
                trApprovedDate.Style.Remove("display");

                divApprovedBy.InnerHtml = entity.ApprovedByName;
                divApprovedDate.InnerHtml = entity.ApprovedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.VoidDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trVoidBy.Style.Add("display", "none");
                trVoidDate.Style.Add("display", "none");

                divVoidBy.InnerHtml = "";
                divVoidDate.InnerHtml = "";
            }
            else
            {
                trVoidBy.Style.Remove("display");
                trVoidDate.Style.Remove("display");

                divVoidBy.InnerHtml = entity.VoidByName;
                divVoidDate.InnerHtml = entity.VoidDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedBy.InnerHtml = "";
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            BindGrdARReceivingDetail();

        }
        #endregion

        #region Save
        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            String[] listParam = hdnInlineEditingData.Value.Split('|');
            int countDt = 0;
            int countAmount = 0;

            foreach (String paramCheck in listParam)
            {
                String[] data = paramCheck.Split(';');

                if (data[0] != "0")
                {
                    countDt += 1;
                }

                if (data[7] == "0.00")
                {
                    countAmount += 1;
                }
            }

            if (countDt == 0)
            {
                errMessage = "Pembayaran tidak dapat disimpan karena belum memiliki detail bayar.";
                return false;
            }
            else
            {
                if (countAmount > 0)
                {
                    errMessage = "Pembayaran tidak memiliki nominal.";
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            String[] listParam = hdnInlineEditingData.Value.Split('|');

            IDbContext ctx = DbFactory.Configure(true);

            ARInvoiceHdDao entityInvoiceHdDao = new ARInvoiceHdDao(ctx);
            ARInvoiceDtDao entityInvoiceDtDao = new ARInvoiceDtDao(ctx);
            ARReceivingHdDao entityReceivingHdDao = new ARReceivingHdDao(ctx);
            ARReceivingDtDao entityReceivingDtDao = new ARReceivingDtDao(ctx);
            ARInvoiceReceivingDao entityIRDao = new ARInvoiceReceivingDao(ctx);
            ARInvoiceReceivingDtDao entityIRDTDao = new ARInvoiceReceivingDtDao(ctx);
            PatientPaymentHdDao entityPaymentHdDao = new PatientPaymentHdDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                bool isTransactionLock = false;
                TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.AR_RECEIVE_PAYER);
                if (entityLock.LockedUntilDate != null)
                {
                    DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                    DateTime DateNow = Helper.GetDatePickerValue(txtReceivingDate);
                    if (DateNow > DateLock)
                    {
                        isTransactionLock = false;
                    }
                    else
                    {
                        isTransactionLock = true;
                    }
                }
                else
                {
                    isTransactionLock = false;
                }

                if (!isTransactionLock)
                {
                    #region ARReceivingHd
                    ARReceivingHd entityReceivingHd = new ARReceivingHd();
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<ARInvoiceHd> lstARInvoiceHd = BusinessLayer.GetARInvoiceHdList(string.Format("ARInvoiceID IN ({0})", hdnListInvoiceID.Value), ctx);
                    decimal totalInvoice = 0;

                    entityReceivingHd.BusinessPartnerID = AppSession.BusinessPartnerID;
                    entityReceivingHd.ReceivingDate = Helper.GetDatePickerValue(txtReceivingDate.Text);
                    entityReceivingHd.TotalReceivingAmount = Convert.ToDecimal(hdnTotalPaymentAmount.Value);
                    entityReceivingHd.TotalFeeAmount = Convert.ToDecimal(hdnTotalFeeAmount.Value);
                    //entityReceivingHd.CashBackAmount = Convert.ToDecimal(hdnCashbackAmount.Value);
                    //entityReceivingHd.TotalInvoiceAmount = Convert.ToDecimal(hdnTotalTransactionAmount.Value);
                    entityReceivingHd.Remarks = txtRemarks.Text;
                    if (cboCashierGroup.Value != null && cboCashierGroup.Value.ToString() != "")
                        entityReceivingHd.GCCashierGroup = cboCashierGroup.Value.ToString();
                    else
                        entityReceivingHd.GCCashierGroup = null;
                    if (cboShift.Value != null && cboShift.Value.ToString() != "")
                        entityReceivingHd.GCShift = cboShift.Value.ToString();
                    else
                        entityReceivingHd.GCShift = null;
                    entityReceivingHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityReceivingHd.ARReceivingNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.AR_RECEIVE_PAYER, entityReceivingHd.ReceivingDate, ctx);
                    entityReceivingHd.CreatedBy = entityReceivingHd.LastUpdatedBy = AppSession.UserLogin.UserID;

                    foreach (ARInvoiceHd arinvoicehd in lstARInvoiceHd)
                    {
                        totalInvoice += arinvoicehd.RemainingAmount;
                        if (totalInvoice > entityReceivingHd.TotalReceivingAmount)
                            break;
                    }
                    entityReceivingHd.TotalInvoiceAmount = totalInvoice;

                    //Perbaikan issue NHS-294
                    if (entityReceivingHd.TotalReceivingAmount > entityReceivingHd.TotalInvoiceAmount)
                    {
                        entityReceivingHd.CashBackAmount = Convert.ToDecimal(Request.Form[txtCashbackAmount.UniqueID]);
                    }
                    else
                    {
                        entityReceivingHd.CashBackAmount = 0;
                    }

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityReceivingHd.ARReceivingID = entityReceivingHdDao.InsertReturnPrimaryKeyID(entityReceivingHd);
                    #endregion

                    #region ARReceivingDt
                    foreach (String param in listParam)
                    {
                        String[] data = param.Split(';');
                        bool isChanged = data[0] == "1" ? true : false;
                        int ID = Convert.ToInt32(data[1]);
                        if (isChanged || ID > 0)
                        {
                            ARReceivingDt entityDt = new ARReceivingDt();
                            entityDt.ARReceivingID = entityReceivingHd.ARReceivingID;
                            entityDt.GCARPaymentMethod = data[2];
                            if (entityDt.GCARPaymentMethod != Constant.AR_PAYMENT_METHODS.CASH)
                            {
                                if (data[3] != "")
                                    entityDt.EDCMachineID = Convert.ToInt32(data[3]);
                                else
                                    entityDt.EDCMachineID = null;
                                if (data[5] != "")
                                    entityDt.BankID = Convert.ToInt32(data[5]);
                                else
                                    entityDt.BankID = null;
                                entityDt.ReferenceNo = data[6];
                                entityDt.GCCardType = data[10];
                                if (data[11] != "")
                                    entityDt.CardNumber = string.Format("XXXX-XXXX-XXXX-{0}", data[11]);
                                else
                                    entityDt.CardNumber = "";
                                entityDt.CardHolderName = data[12];
                                if (data[13] != "" && data[14] != "")
                                    entityDt.CardValidThru = string.Format("{0:00}/{1:00}", data[13].PadLeft(2, '0'), data[14].Substring(2));
                                else
                                    entityDt.CardValidThru = "";
                                entityDt.GCCardProvider = data[16];
                            }
                            entityDt.PaymentAmount = Convert.ToDecimal(data[7].Replace(",", ""));
                            entityDt.CardFeeAmount = Convert.ToDecimal(data[8].Replace(",", ""));
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityReceivingDtDao.Insert(entityDt);
                        }
                    }
                    #endregion

                    #region Update InvoiceReceive
                    if (hdnListInvoiceID.Value != "")
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<ARInvoiceHd> lstARInvoiceHD = BusinessLayer.GetARInvoiceHdList(string.Format("ARInvoiceID IN ({0})", hdnListInvoiceID.Value), ctx);
                        decimal totalPaymentAmount = entityReceivingHd.TotalReceivingAmount;
                        foreach (ARInvoiceHd ARInvoiceHdobj in lstARInvoiceHD)
                        {
                            ARInvoiceReceiving ARInvoiceReceivingObj = new ARInvoiceReceiving();
                            ARInvoiceReceivingObj.ARInvoiceID = ARInvoiceHdobj.ARInvoiceID;
                            ARInvoiceReceivingObj.ARReceivingID = entityReceivingHd.ARReceivingID;
                            ARInvoiceReceivingObj.IsDeleted = false;
                            decimal remainingAmount = ARInvoiceHdobj.RemainingAmount;
                            if (remainingAmount < totalPaymentAmount)
                            {
                                ARInvoiceReceivingObj.ReceivingAmount = remainingAmount;
                                ARInvoiceHdobj.TotalPaymentAmount += remainingAmount;
                                totalPaymentAmount -= remainingAmount;
                            }
                            else
                            {
                                ARInvoiceReceivingObj.ReceivingAmount = totalPaymentAmount;
                                ARInvoiceHdobj.TotalPaymentAmount += totalPaymentAmount;
                                totalPaymentAmount = 0;
                            }
                            ARInvoiceHdobj.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ARInvoiceHdobj.LastUpdatedDate = DateTime.Now;
                            ARInvoiceReceivingObj.CreatedBy = AppSession.UserLogin.UserID;
                            int oARInvoiceReceivingID = entityIRDao.InsertReturnPrimaryKeyID(ARInvoiceReceivingObj);

                            if (ARInvoiceHdobj.RemainingAmount == 0)
                            {
                                ARInvoiceHdobj.GCTransactionStatus = Constant.TransactionStatus.CLOSED;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<ARInvoiceDt> lstARInvoiceDT = BusinessLayer.GetARInvoiceDtList(string.Format(
                                                    "ARInvoiceID IN ({0}) AND ISNULL(GCTransactionDetailStatus,'') != '{1}'",
                                                    ARInvoiceHdobj.ARInvoiceID, Constant.TransactionStatus.VOID), ctx);
                                foreach (ARInvoiceDt ARInvoiceDtobj in lstARInvoiceDT)
                                {
                                    ARInvoiceDtobj.PaymentAmount = ARInvoiceDtobj.ClaimedAmount;
                                    ARInvoiceDtobj.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ARInvoiceDtobj.LastUpdatedDate = DateTime.Now;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityInvoiceDtDao.Update(ARInvoiceDtobj);

                                    string filterARInvRcvDt = string.Format("ARInvoiceReceivingID = {0} AND ARInvoiceDtID = {1} AND IsDeleted = 0", oARInvoiceReceivingID, ARInvoiceDtobj.ID);
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    List<ARInvoiceReceivingDt> lstARInvRcvDt = BusinessLayer.GetARInvoiceReceivingDtList(filterARInvRcvDt, ctx);
                                    if (lstARInvRcvDt.Count() == 0)
                                    {
                                        #region agar ReceivingAmount di ARInvoiceReceivingDt sesuai dengan yang dilunasi di ARReceiving saat ini
                                        Decimal pengurang = 0;
                                        string filterARInvRcvMain = string.Format("ID != {0} AND ARInvoiceID = {1} AND IsDeleted = 0", oARInvoiceReceivingID, ARInvoiceDtobj.ARInvoiceID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        List<ARInvoiceReceiving> lstInvoiceReceiving = BusinessLayer.GetARInvoiceReceivingList(filterARInvRcvMain, ctx);
                                        foreach (ARInvoiceReceiving e in lstInvoiceReceiving)
                                        {
                                            string filterARInvRcvDetail = string.Format("ARInvoiceReceivingID = {0} AND ARInvoiceDtID = {1} AND IsDeleted = 0", e.ID, ARInvoiceDtobj.ID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            List<ARInvoiceReceivingDt> lstInvoiceReceivingDt = BusinessLayer.GetARInvoiceReceivingDtList(filterARInvRcvDetail, ctx);
                                            foreach (ARInvoiceReceivingDt k in lstInvoiceReceivingDt)
                                            {
                                                pengurang = pengurang + k.ReceivingAmount;
                                            }
                                        }
                                        #endregion

                                        ARInvoiceReceivingDt entityARRcvDt = new ARInvoiceReceivingDt();
                                        entityARRcvDt.ARInvoiceReceivingID = oARInvoiceReceivingID;
                                        entityARRcvDt.ARInvoiceDtID = ARInvoiceDtobj.ID;
                                        entityARRcvDt.ReceivingAmount = ARInvoiceDtobj.ClaimedAmount - pengurang;
                                        entityARRcvDt.IsDeleted = false;
                                        entityARRcvDt.CreatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityIRDTDao.Insert(entityARRcvDt);
                                    }
                                    else
                                    {
                                        ARInvoiceReceivingDt entityARRcvDt = lstARInvRcvDt.FirstOrDefault();
                                        entityARRcvDt.ReceivingAmount += ARInvoiceDtobj.ClaimedAmount;
                                        entityARRcvDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityIRDTDao.Update(entityARRcvDt);
                                    }
                                }
                            }
                            else
                            {
                                ARInvoiceHdobj.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<ARInvoiceDt> lstARInvoiceDT = BusinessLayer.GetARInvoiceDtList(string.Format(
                                                    "ARInvoiceID IN ({0}) AND ISNULL(GCTransactionDetailStatus,'') != '{1}'",
                                                    ARInvoiceHdobj.ARInvoiceID, Constant.TransactionStatus.VOID), ctx);

                                Decimal AmountForDt = ARInvoiceHdobj.TotalPaymentAmount - lstARInvoiceDT.Where(t => t.PaymentAmount == t.ClaimedAmount).ToList().Sum(x => x.PaymentAmount);
                                foreach (ARInvoiceDt ARInvoiceDtobj in lstARInvoiceDT.Where(t => t.PaymentAmount < t.ClaimedAmount).ToList())
                                {
                                    if (AmountForDt > ARInvoiceDtobj.ClaimedAmount)
                                    {
                                        ARInvoiceDtobj.PaymentAmount = ARInvoiceDtobj.ClaimedAmount;
                                    }
                                    else
                                    {
                                        ARInvoiceDtobj.PaymentAmount = AmountForDt;
                                    }

                                    AmountForDt = AmountForDt - ARInvoiceDtobj.PaymentAmount;

                                    ARInvoiceDtobj.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ARInvoiceDtobj.LastUpdatedDate = DateTime.Now;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityInvoiceDtDao.Update(ARInvoiceDtobj);

                                    string filterARInvRcvDt = string.Format("ARInvoiceReceivingID = {0} AND ARInvoiceDtID = {1} AND IsDeleted = 0", oARInvoiceReceivingID, ARInvoiceDtobj.ID);
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    List<ARInvoiceReceivingDt> lstARInvRcvDt = BusinessLayer.GetARInvoiceReceivingDtList(filterARInvRcvDt, ctx);

                                    string filterARInvRcvDtOther = string.Format("ARInvoiceReceivingID != {0} AND ARInvoiceDtID = {1} AND IsDeleted = 0", oARInvoiceReceivingID, ARInvoiceDtobj.ID);
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    List<ARInvoiceReceivingDt> lstARInvRcvDtOther = BusinessLayer.GetARInvoiceReceivingDtList(filterARInvRcvDtOther, ctx);

                                    if (lstARInvRcvDt.Count() == 0)
                                    {
                                        if (ARInvoiceDtobj.PaymentAmount > 0)
                                        {
                                            ARInvoiceReceivingDt entityARRcvDt = new ARInvoiceReceivingDt();
                                            entityARRcvDt.ARInvoiceReceivingID = oARInvoiceReceivingID;
                                            entityARRcvDt.ARInvoiceDtID = ARInvoiceDtobj.ID;
                                            entityARRcvDt.ReceivingAmount = ARInvoiceDtobj.PaymentAmount - lstARInvRcvDtOther.Sum(t => t.ReceivingAmount);                                            
                                            entityARRcvDt.IsDeleted = false;
                                            entityARRcvDt.CreatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            entityIRDTDao.Insert(entityARRcvDt);
                                        }
                                    }
                                    else
                                    {
                                        ARInvoiceReceivingDt entityARRcvDt = lstARInvRcvDt.FirstOrDefault();
                                        entityARRcvDt.ReceivingAmount += ARInvoiceDtobj.PaymentAmount;
                                        entityARRcvDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        entityIRDTDao.Update(entityARRcvDt);
                                    }
                                }
                            }

                            ARInvoiceHdobj.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityInvoiceHdDao.Update(ARInvoiceHdobj); // UPDATE AR INVOICE HD
                        }
                    }
                    #endregion

                    #region PatientPaymentHD
                    if (hdnListInvoiceID.Value != "")
                    {
                        List<PatientPaymentHd> lstPaymentHD = new List<PatientPaymentHd>();
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<ARInvoiceHd> lstARInvoiceHD = BusinessLayer.GetARInvoiceHdList(string.Format("ARInvoiceID IN ({0}) AND TransactionCode = '{1}'", hdnListInvoiceID.Value, Constant.TransactionCode.AR_INVOICE_PAYER), ctx);
                        foreach (ARInvoiceHd ARInvoiceHdobj in lstARInvoiceHD)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<ARInvoiceDt> lstARInvoiceDt = BusinessLayer.GetARInvoiceDtList(string.Format("ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", ARInvoiceHdobj.ARInvoiceID, Constant.TransactionStatus.VOID), ctx);
                            var lstARInvoiceDtGroupByPaymentID = lstARInvoiceDt.GroupBy(test => test.PaymentID).Select(grp => grp.First()).ToList().OrderBy(x => x.PaymentID);
                            foreach (ARInvoiceDt ARInvoiceDtobj in lstARInvoiceDtGroupByPaymentID)
                            {
                                if (ARInvoiceDtobj.PaymentID != null && ARInvoiceDtobj.PaymentID != 0)
                                {
                                    if (lstPaymentHD.Where(t => t.PaymentID == ARInvoiceDtobj.PaymentID).Count() <= 0)
                                    {
                                        PatientPaymentHd entity = entityPaymentHdDao.Get(Convert.ToInt32(ARInvoiceDtobj.PaymentID));
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        lstPaymentHD.Add(entity);
                                    }
                                }
                            }
                        }

                        if (lstPaymentHD.Count > 0)
                        {
                            foreach (PatientPaymentHd e in lstPaymentHD)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<PatientPaymentDt> lstPaymentDt = BusinessLayer.GetPatientPaymentDtList((string.Format("PaymentID = {0} AND IsDeleted = 0", e.PaymentID)), ctx);
                                if (lstPaymentDt.Count == 1)
                                {
                                    if (lstPaymentDt.FirstOrDefault().PaymentID != 0)
                                    {
                                        string filterExpression = string.Format("PaymentID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", lstPaymentDt.FirstOrDefault().PaymentID, Constant.TransactionStatus.VOID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        List<ARInvoiceDt> lstInvoiceDt = BusinessLayer.GetARInvoiceDtList(filterExpression, ctx);

                                        if (lstInvoiceDt.Count > 0)
                                        {
                                            ARInvoiceDt invoiceDt = lstInvoiceDt.FirstOrDefault();
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            ARInvoiceHd invoiceHd = BusinessLayer.GetARInvoiceHdList(string.Format("ARInvoiceID IN ({0})", invoiceDt.ARInvoiceID), ctx).FirstOrDefault();
                                            if (invoiceHd.GCTransactionStatus == Constant.TransactionStatus.CLOSED)
                                            {
                                                e.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                                e.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                entityPaymentHdDao.Update(e);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    int CountPaymentDt = lstPaymentDt.Count;
                                    int CountInvoiceClosed = 0;
                                    foreach (PatientPaymentDt x in lstPaymentDt)
                                    {
                                        string filtrExpression = string.Format("PaymentDetailID = {0} AND PaymentID = {1} AND ISNULL(GCTransactionDetailStatus,'') != '{2}'", x.PaymentDetailID, x.PaymentID, Constant.TransactionStatus.VOID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        List<ARInvoiceDt> lstInvoiceDt = BusinessLayer.GetARInvoiceDtList(filtrExpression, ctx);
                                        if (lstInvoiceDt.Count > 0)
                                        {
                                            ARInvoiceDt invoiceDt = lstInvoiceDt.FirstOrDefault();
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            ARInvoiceHd invoiceHd = BusinessLayer.GetARInvoiceHdList(string.Format("ARInvoiceID IN ({0})", invoiceDt.ARInvoiceID), ctx).FirstOrDefault();
                                            if (invoiceHd.GCTransactionStatus == Constant.TransactionStatus.CLOSED)
                                            {
                                                CountInvoiceClosed = CountInvoiceClosed + 1;
                                            }
                                        }
                                    }

                                    if (CountInvoiceClosed > 0)
                                    {
                                        if (CountInvoiceClosed == CountPaymentDt)
                                        {
                                            e.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                            e.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            entityPaymentHdDao.Update(e);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }

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

        #endregion

        #region Void
        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            ARReceivingHdDao entityARRHdDao = new ARReceivingHdDao(ctx);
            ARInvoiceHdDao eInvoiceHd = new ARInvoiceHdDao(ctx);
            ARInvoiceDtDao eInvoiceDt = new ARInvoiceDtDao(ctx);
            ARInvoiceReceivingDao arRcvInvDao = new ARInvoiceReceivingDao(ctx);
            ARInvoiceReceivingDtDao arRcvInvDtDao = new ARInvoiceReceivingDtDao(ctx);
            PatientPaymentHdDao entityPaymentDao = new PatientPaymentHdDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                ARReceivingHd entityHD = entityARRHdDao.Get(Convert.ToInt32(hdnARReceivingID.Value));
                if (type.Contains("voidbyreason"))
                {
                    TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.AR_RECEIVE_PATIENT);

                    bool isTransactionLock = false;
                    if (entityLock.LockedUntilDate != null)
                    {
                        DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                        DateTime DateNow = entityHD.ReceivingDate;
                        if (DateNow > DateLock)
                        {
                            isTransactionLock = false;
                        }
                        else
                        {
                            isTransactionLock = true;
                        }
                    }
                    else
                    {
                        isTransactionLock = false;
                    }

                    if (!isTransactionLock)
                    {
                        string filter = string.Format("ARInvoiceID IN (SELECT ARInvoiceID FROM ARInvoiceReceiving WHERE ARReceivingID = {0} AND IsDeleted = 0)", hdnARReceivingID.Value);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<ARInvoiceDt> lstARInvoiceDt = BusinessLayer.GetARInvoiceDtList(filter, ctx);
                        decimal sumAllocation = 0;
                        if (lstARInvoiceDt != null)
                        {
                            sumAllocation = lstARInvoiceDt.Sum(a => a.PaymentAmount);
                        }

                        if (entityHD.RSSummaryAdjustmentID == null || entityHD.RSSummaryAdjustmentID == 0)
                        {
                            if (entityHD.GLTransactionID == null || (entityHD.GLTransactionID != null && sumAllocation == 0))
                            {
                                string[] param = type.Split(';');
                                string gcDeleteReason = param[1];
                                string reason = param[2];

                                if (entityHD.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                                {
                                    entityHD.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    entityHD.GCVoidReason = gcDeleteReason;
                                    entityHD.VoidReason = reason;
                                    entityHD.VoidBy = AppSession.UserLogin.UserID;
                                    entityHD.VoidDate = DateTime.Now;
                                    entityHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityARRHdDao.Update(entityHD);

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    List<ARInvoiceHd> lstARInvoice = BusinessLayer.GetARInvoiceHdList(filter, ctx);
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    List<ARInvoiceReceiving> lstARIR = BusinessLayer.GetARInvoiceReceivingList(string.Format("ARReceivingID = {0}", hdnARReceivingID.Value), ctx);
                                    foreach (ARInvoiceHd enARI in lstARInvoice)
                                    {
                                        string filterPayment = string.Format("PaymentID IN (SELECT PaymentID FROM ARInvoiceDt WHERE ARInvoiceID = {0})", enARI.ARInvoiceID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        List<PatientPaymentHd> lstPaymentHd = BusinessLayer.GetPatientPaymentHdList(filterPayment, ctx);

                                        ARInvoiceReceiving entityIR = lstARIR.FirstOrDefault(p => p.ARInvoiceID == enARI.ARInvoiceID);
                                        entityIR.IsDeleted = true;
                                        entityIR.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        arRcvInvDao.Update(entityIR);

                                        string filterARInvRcvDt = string.Format("ARInvoiceReceivingID = {0} AND IsDeleted = 0", entityIR.ID);
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        List<ARInvoiceReceivingDt> lstARInvRcvDt = BusinessLayer.GetARInvoiceReceivingDtList(filterARInvRcvDt, ctx);
                                        foreach (ARInvoiceReceivingDt entityIRDT in lstARInvRcvDt)
                                        {
                                            ARInvoiceDt invDt = eInvoiceDt.Get(entityIRDT.ARInvoiceDtID);
                                            invDt.PaymentAmount -= entityIRDT.ReceivingAmount;

                                            if (invDt.PaymentAmount == 0)
                                            {
                                                invDt.GCTransactionDetailStatus = Constant.TransactionStatus.APPROVED;
                                            }
                                            else if (invDt.PaymentAmount == invDt.ClaimedAmount)
                                            {
                                                invDt.GCTransactionDetailStatus = Constant.TransactionStatus.CLOSED;
                                            }
                                            else
                                            {
                                                invDt.GCTransactionDetailStatus = Constant.TransactionStatus.PROCESSED;
                                            }
                                            invDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            eInvoiceDt.Update(invDt);

                                            entityIRDT.IsDeleted = true;
                                            entityIRDT.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            arRcvInvDtDao.Update(entityIRDT);
                                        }

                                        enARI.TotalPaymentAmount -= entityIR.ReceivingAmount;
                                        if (enARI.TotalPaymentAmount < 1)
                                        {
                                            enARI.GCTransactionStatus = Constant.TransactionStatus.APPROVED;

                                            foreach (PatientPaymentHd enPayment in lstPaymentHd)
                                            {
                                                enPayment.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                                enPayment.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                entityPaymentDao.Update(enPayment);
                                            }
                                        }
                                        else
                                        {
                                            enARI.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                        }
                                        enARI.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        eInvoiceHd.Update(enARI);
                                    }
                                    retval = entityHD.ARReceivingNo;
                                    ctx.CommitTransaction();
                                }
                                else
                                {
                                    result = false;
                                    errMessage = "Piutang tidak dapat diubah. Harap refresh halaman ini.";
                                    ctx.RollBackTransaction();
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = "Piutang tidak dapat diubah, karena sudah ada alokasi detail pembayaran.";
                                ctx.RollBackTransaction();
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Piutang tidak dapat diubah, karena piutang berasal dari Pembayaran Jasa Medis";
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                        ctx.RollBackTransaction();
                    }
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