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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingSummaryAdjustmentEntry : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_SUMMARY_ADJUSTMENT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected string OnGetRevenueSharingFilterExpression()
        {
            return string.Format("ParamedicID = {0} AND GCTransactionStatus = '{1}'", AppSession.ParamedicID, Constant.TransactionStatus.OPEN);
        }

        protected string OnGetAdjustmentGroupPlus()
        {
            return Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnParamedicID.Value = AppSession.ParamedicID.ToString();

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                    AppSession.UserLogin.HealthcareID,
                                                    Constant.SettingParameter.FN_IS_USED_BUTTON_PROCESS_REVENUE_SHARING_ADJUSTMENT);
            List<SettingParameterDt> lstSetVar = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsUsedButtonProcessAdjustment.Value = lstSetVar.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_USED_BUTTON_PROCESS_REVENUE_SHARING_ADJUSTMENT).FirstOrDefault().ParameterValue;

            string filterBP = string.Format("ParamedicID = {0} AND IsDeleted = 0 AND IsActive = 1", AppSession.ParamedicID.ToString());
            List<vCustomer> lstBP = BusinessLayer.GetvCustomerList(filterBP);
            if (lstBP.Count() > 0)
            {
                hdnIsParamedicHasLinkedToCustomer.Value = "1";
            }
            else
            {
                hdnIsParamedicHasLinkedToCustomer.Value = "0";
            }

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_GROUP, Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_TYPE));
            Methods.SetRadioButtonListField(rblAdjustment, lstSc.Where(p => p.ParentID == Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_GROUP).ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField(cboAdjustmentTypeAdd, lstSc.Where(p => p.ParentID == Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_TYPE && p.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboAdjustmentTypeMin, lstSc.Where(p => p.ParentID == Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_TYPE && p.TagProperty == "2").ToList(), "StandardCodeName", "StandardCodeID");

            rblAdjustment.SelectedValue = OnGetAdjustmentGroupPlus();

            Helper.SetControlEntrySetting(hdnRSSummaryID, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(hdnRSSummaryNo, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtRSSummaryNo, new ControlEntrySetting(false, false, false), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboAdjustmentTypeAdd, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboAdjustmentTypeMin, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtAdjustmentAmount, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(chkRevenueSharingFee, new ControlEntrySetting(false, false, false), "mpEntryPopup");

            BindGridDetail();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void BindGridDetail()
        {
            string filterExpression = "";
            int RSSummaryID;

            RSSummaryID = hdnRSSummaryID.Value != "" ? Convert.ToInt32(hdnRSSummaryID.Value) : 0;

            filterExpression = string.Format("IsDeleted = 0 AND ParamedicID = {0} AND RSSummaryID = {1} AND GCRSAdjustmentGroup = '{2}'", AppSession.ParamedicID, RSSummaryID, rblAdjustment.SelectedValue);

            List<vTransRevenueSharingSummaryAdj> lstEntity = BusinessLayer.GetvTransRevenueSharingSummaryAdjList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string retval = "";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage, ref retval))
                        result += string.Format("success|{0}", retval);
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(TransRevenueSharingSummaryAdj entity)
        {
            entity.RSSummaryID = Convert.ToInt32(hdnRSSummaryID.Value);
            entity.GCRSAdjustmentGroup = rblAdjustment.SelectedValue;
            if (rblAdjustment.SelectedValue == OnGetAdjustmentGroupPlus())
            {
                entity.GCRSAdjustmentType = cboAdjustmentTypeAdd.Value.ToString();
            }
            else
            {
                entity.GCRSAdjustmentType = cboAdjustmentTypeMin.Value.ToString();
            }
            entity.AdjustmentAmountBRUTO = Convert.ToDecimal(txtAdjustmentAmountBruto.Text);
            entity.AdjustmentAmount = Convert.ToDecimal(txtAdjustmentAmount.Text);
            entity.IsTaxed = chkRevenueSharingFee.Checked;
            entity.Remarks = txtRemarks.Text;
            if (hdnRevenueSharingID.Value != null && hdnRevenueSharingID.Value != "" && hdnRevenueSharingID.Value != "0")
            {
                entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            }
            else
            {
                entity.RevenueSharingID = null;
            }
            entity.RegistrationNo = txtRegistrationNo.Text;
            entity.RegistrationDate = txtRegistrationDate.Text;
            entity.DischargeDate = txtDischargeDate.Text;
            entity.ReceiptNo = txtReceiptNo.Text;
            entity.InvoiceNo = txtInvoiceNo.Text;
            entity.ReferenceNo = txtReferenceNo.Text;
            entity.BusinessPartnerName = txtBusinessPartnerName.Text;
            entity.MedicalNo = txtMedicalNo.Text;
            entity.PatientName = txtPatientName.Text;
            entity.TransactionNo = txtTransactionNo.Text;
            if (txtTransactionDate.Text != "")
            {
                entity.TransactionDate = Helper.GetDatePickerValue(txtTransactionDate.Text);
            }
            else
            {
                entity.TransactionDate = null;
            }
            entity.ItemName1 = txtItemName1.Text;
            entity.ChargedQty = Convert.ToDecimal(txtChargedQty.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingSummaryHdDao entitySummaryHdDao = new TransRevenueSharingSummaryHdDao(ctx);
            TransRevenueSharingSummaryAdjDao entitySummaryAdjustmentDao = new TransRevenueSharingSummaryAdjDao(ctx);
            ARReceivingHdDao receivingHdDao = new ARReceivingHdDao(ctx);
            ARReceivingDtDao receivingDtDao = new ARReceivingDtDao(ctx);

            try
            {
                int RSSummaryID = (hdnRSSummaryID.Value == "" || hdnRSSummaryID.Value == "0" || hdnRSSummaryID.Value == null) ? 0 : Convert.ToInt32(hdnRSSummaryID.Value);

                if (RSSummaryID == 0)
                {
                    TransRevenueSharingSummaryHd entitySummaryHd = new TransRevenueSharingSummaryHd();
                    entitySummaryHd.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                    entitySummaryHd.RSSummaryDate = DateTime.Now;
                    entitySummaryHd.RSSummaryNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.TRANS_REVENUE_SHARING_SUMMARY_ENTRY, entitySummaryHd.RSSummaryDate, ctx);
                    entitySummaryHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entitySummaryHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    RSSummaryID = entitySummaryHdDao.InsertReturnPrimaryKeyID(entitySummaryHd);

                    hdnRSSummaryID.Value = RSSummaryID.ToString();
                    retval = hdnRSSummaryNo.Value = entitySummaryHd.RSSummaryNo;
                }

                TransRevenueSharingSummaryAdj entity = new TransRevenueSharingSummaryAdj();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entitySummaryAdjustmentDao.Insert(entity);

                #region ARReceiving

                // INSERT ARReceivingHd
                if (entity.GCRSAdjustmentType == Constant.RevenueSharingAdjustmentType.POTONGAN_PIUTANG)
                {
                    string filterBP = string.Format("ParamedicID = {0} AND IsDeleted = 0 AND IsActive = 1", AppSession.ParamedicMasterRevenueSharingProcess.ParamedicID.ToString());
                    List<vCustomer> lstBP = BusinessLayer.GetvCustomerList(filterBP, ctx);

                    if (lstBP.Count() > 0)
                    {
                        vCustomer cust = lstBP.FirstOrDefault();

                        ARReceivingHd rcvHD = new ARReceivingHd();
                        rcvHD.ReceivingDate = DateTime.Now;
                        rcvHD.CustomerGroupID = cust.CustomerGroupID;
                        rcvHD.BusinessPartnerID = cust.BusinessPartnerID;
                        rcvHD.TotalReceivingAmount = entity.AdjustmentAmount;
                        rcvHD.TotalInvoiceAmount = 0;
                        rcvHD.TotalFeeAmount = 0;
                        rcvHD.CashBackAmount = 0;
                        rcvHD.Remarks = string.Format("REKAP JASA MEDIS - {0}", entity.Remarks);
                        rcvHD.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        rcvHD.CreatedBy = AppSession.UserLogin.UserID;

                        if (rcvHD.BusinessPartnerID != 1)
                        {
                            rcvHD.ARReceivingNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.AR_RECEIVE_PAYER, rcvHD.ReceivingDate, ctx);
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        int ARReceivingID = receivingHdDao.InsertReturnPrimaryKeyID(rcvHD);

                        // INSERT ARReceivingDt
                        ARReceivingDt rcvDT = new ARReceivingDt();
                        rcvDT.ARReceivingID = ARReceivingID;
                        rcvDT.GCARPaymentMethod = Constant.AR_PAYMENT_METHODS.POTONGAN_HONOR_DOKTER;
                        rcvDT.PaymentAmount = entity.AdjustmentAmount;
                        rcvDT.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        receivingDtDao.Insert(rcvDT);
                    }

                }

                #endregion

                ctx.CommitTransaction();
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

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingSummaryAdjDao entitySummaryAdjustmentDao = new TransRevenueSharingSummaryAdjDao(ctx);
            ARReceivingHdDao receivingHdDao = new ARReceivingHdDao(ctx);
            ARReceivingDtDao receivingDtDao = new ARReceivingDtDao(ctx);
            TransRevenueSharingAdjustmentDtDao entityAdjDtDao = new TransRevenueSharingAdjustmentDtDao(ctx);

            try
            {
                TransRevenueSharingSummaryAdj entity = entitySummaryAdjustmentDao.Get(Convert.ToInt32(hdnID.Value));
                if (entity.GCRSAdjustmentType == Constant.RevenueSharingAdjustmentType.POTONGAN_PIUTANG)
                {
                    result = false;
                    errMessage = "Maaf, untuk penyesuaian yang berasal dari potongan piutang dokter, tidak dapat diedit, hanya bisa dihapus saja.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                else
                {
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entitySummaryAdjustmentDao.Update(entity);

                    string filterAdjDt = string.Format("RSSummaryAdjustmentID = {0} AND IsDeleted = 0", entity.ID);
                    List<TransRevenueSharingAdjustmentDt> adjDtLst = BusinessLayer.GetTransRevenueSharingAdjustmentDtList(filterAdjDt, ctx);
                    foreach (TransRevenueSharingAdjustmentDt adjDt in adjDtLst)
                    {
                        adjDt.GCRSAdjustmentGroup = rblAdjustment.SelectedValue;
                        if (rblAdjustment.SelectedValue == OnGetAdjustmentGroupPlus())
                        {
                            adjDt.GCRSAdjustmentType = cboAdjustmentTypeAdd.Value.ToString();
                        }
                        else
                        {
                            adjDt.GCRSAdjustmentType = cboAdjustmentTypeMin.Value.ToString();
                        }
                        adjDt.AdjustmentAmountBRUTO = Convert.ToDecimal(txtAdjustmentAmountBruto.Text);
                        adjDt.AdjustmentAmount = Convert.ToDecimal(txtAdjustmentAmount.Text);
                        adjDt.IsTaxed = chkRevenueSharingFee.Checked;
                        adjDt.Remarks = txtRemarks.Text;
                        adjDt.AdjustmentAmountBRUTO = Convert.ToDecimal(txtAdjustmentAmountBruto.Text);
                        adjDt.AdjustmentAmount = Convert.ToDecimal(txtAdjustmentAmount.Text);
                        adjDt.IsTaxed = chkRevenueSharingFee.Checked;
                        adjDt.Remarks = txtRemarks.Text;
                        adjDt.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
                        adjDt.RegistrationNo = txtRegistrationNo.Text;
                        adjDt.RegistrationDate = txtRegistrationDate.Text;
                        adjDt.DischargeDate = txtDischargeDate.Text;
                        adjDt.ReceiptNo = txtReceiptNo.Text;
                        adjDt.InvoiceNo = txtInvoiceNo.Text;
                        adjDt.ReferenceNo = txtReferenceNo.Text;
                        adjDt.BusinessPartnerName = txtBusinessPartnerName.Text;
                        adjDt.MedicalNo = txtMedicalNo.Text;
                        adjDt.PatientName = txtPatientName.Text;
                        adjDt.TransactionNo = txtTransactionNo.Text;
                        if (txtTransactionDate.Text != "")
                        {
                            adjDt.TransactionDate = Helper.GetDatePickerValue(txtTransactionDate.Text);
                        }
                        else
                        {
                            adjDt.TransactionDate = null;
                        }
                        adjDt.ItemName1 = txtItemName1.Text;
                        adjDt.ChargedQty = Convert.ToDecimal(txtChargedQty.Text);
                        adjDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityAdjDtDao.Update(adjDt);
                    }

                    ctx.CommitTransaction();
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingSummaryAdjDao entitySummaryAdjustmentDao = new TransRevenueSharingSummaryAdjDao(ctx);
            ARReceivingHdDao receivingHdDao = new ARReceivingHdDao(ctx);
            ARReceivingDtDao receivingDtDao = new ARReceivingDtDao(ctx);
            ARInvoiceReceivingDao invReceivingDao = new ARInvoiceReceivingDao(ctx);
            ARInvoiceReceivingDtDao invReceivingDtDao = new ARInvoiceReceivingDtDao(ctx);
            ARInvoiceHdDao eInvoiceHd = new ARInvoiceHdDao(ctx);
            ARInvoiceDtDao eInvoiceDt = new ARInvoiceDtDao(ctx);
            TransRevenueSharingAdjustmentHdDao entityAdjHdDao = new TransRevenueSharingAdjustmentHdDao(ctx);
            TransRevenueSharingAdjustmentDtDao entityAdjDtDao = new TransRevenueSharingAdjustmentDtDao(ctx);

            try
            {
                TransRevenueSharingSummaryAdj entity = entitySummaryAdjustmentDao.Get(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entitySummaryAdjustmentDao.Update(entity);

                string filterAdjDt = string.Format("RSSummaryAdjustmentID = {0} AND IsDeleted = 0", entity.ID);
                List<TransRevenueSharingAdjustmentDt> adjDtList = BusinessLayer.GetTransRevenueSharingAdjustmentDtList(filterAdjDt, ctx);
                foreach (TransRevenueSharingAdjustmentDt adjDt in adjDtList)
                {
                    adjDt.RSSummaryAdjustmentID = null;
                    adjDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityAdjDtDao.Update(adjDt);

                    string filterAdjDtCek = string.Format("RSAdjustmentID = {0} AND IsDeleted = 0 AND RSSummaryAdjustmentID IS NOT NULL", adjDt.RSAdjustmentID);
                    List<TransRevenueSharingAdjustmentDt> adjDtLstCek = BusinessLayer.GetTransRevenueSharingAdjustmentDtList(filterAdjDtCek, ctx);
                    if (adjDtLstCek.Count() == 0)
                    {
                        TransRevenueSharingAdjustmentHd adjHd = entityAdjHdDao.Get(adjDt.RSAdjustmentID);
                        adjHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        adjHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityAdjHdDao.Update(adjHd);
                    }
                    else if (adjDtLstCek.Count() > 0)
                    {
                        TransRevenueSharingAdjustmentHd adjHd = entityAdjHdDao.Get(adjDt.RSAdjustmentID);
                        adjHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        adjHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityAdjHdDao.Update(adjHd);
                    }
                }


                if (entity.GCRSAdjustmentType == Constant.RevenueSharingAdjustmentType.POTONGAN_PIUTANG)
                {
                    #region Update ARReceiving

                    string filterARHD = string.Format("RSSummaryAdjustmentID = '{0}' AND GCTransactionStatus = '{1}'", entity.ID, Constant.TransactionStatus.OPEN);
                    List<ARReceivingHd> lstRcvHD = BusinessLayer.GetARReceivingHdList(filterARHD, ctx);
                    if (lstRcvHD.Count() > 0)
                    {
                        ARReceivingHd rcvHD = lstRcvHD.FirstOrDefault();
                        rcvHD.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        rcvHD.GCVoidReason = Constant.DeleteReason.OTHER;
                        rcvHD.VoidReason = "Delete from Revenue Sharing Summary";
                        rcvHD.VoidBy = AppSession.UserLogin.UserID;
                        rcvHD.VoidDate = DateTime.Now;
                        rcvHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                        receivingHdDao.Update(rcvHD);

                        string filterInvRcv = string.Format("ARReceivingID = '{0}' AND IsDeleted = 0", rcvHD.ARReceivingID);
                        List<ARInvoiceReceiving> lstInvRcv = BusinessLayer.GetARInvoiceReceivingList(filterInvRcv, ctx);
                        foreach (ARInvoiceReceiving invRcv in lstInvRcv)
                        {
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            invRcv.IsDeleted = true;
                            invRcv.LastUpdatedBy = AppSession.UserLogin.UserID;
                            invReceivingDao.Update(invRcv);

                            string filterARInvRcvDt = string.Format("ARInvoiceReceivingID = {0} AND IsDeleted = 0", invRcv.ID);
                            List<ARInvoiceReceivingDt> lstARInvRcvDt = BusinessLayer.GetARInvoiceReceivingDtList(filterARInvRcvDt, ctx);
                            foreach (ARInvoiceReceivingDt invrcvDt in lstARInvRcvDt)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                invrcvDt.IsDeleted = true;
                                invrcvDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                invReceivingDtDao.Update(invrcvDt);
                            }

                            #region Update ARInvoice

                            ARInvoiceHd invHd = eInvoiceHd.Get(invRcv.ARInvoiceID);
                            invHd.TotalPaymentAmount -= rcvHD.TotalReceivingAmount;
                            if (invHd.TotalPaymentAmount == 0)
                            {
                                invHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;

                                decimal rcvAll = rcvHD.TotalReceivingAmount;
                                decimal rcvNow = 0;

                                string filterInvDt = string.Format("ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", invRcv.ARInvoiceID, Constant.TransactionStatus.VOID);
                                List<ARInvoiceDt> lstInvoiceDt = BusinessLayer.GetARInvoiceDtList(filterInvDt, ctx);
                                foreach (ARInvoiceDt oInvoiceDt in lstInvoiceDt)
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    if (oInvoiceDt.ClaimedAmount <= rcvAll)
                                    {
                                        rcvNow = oInvoiceDt.ClaimedAmount;
                                    }
                                    else
                                    {
                                        rcvNow = rcvAll;
                                    }

                                    if (oInvoiceDt.PaymentAmount >= 0)
                                    {
                                        oInvoiceDt.PaymentAmount -= rcvNow;
                                    }
                                    rcvAll -= rcvNow;

                                    if (oInvoiceDt.PaymentAmount == 0)
                                    {
                                        oInvoiceDt.GCTransactionDetailStatus = Constant.TransactionStatus.APPROVED;
                                    }
                                    else
                                    {
                                        oInvoiceDt.GCTransactionDetailStatus = Constant.TransactionStatus.PROCESSED;
                                    }
                                    oInvoiceDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    eInvoiceDt.Update(oInvoiceDt);
                                }
                            }
                            else
                            {
                                invHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                            }
                            invHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            eInvoiceHd.Update(invHd);

                            #endregion

                        }
                    }

                    #endregion
                }

                ctx.CommitTransaction();
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            try
            {
                int RSSummaryID = Convert.ToInt32(hdnRSSummaryID.Value);
                TransRevenueSharingSummaryHd summaryHd = BusinessLayer.GetTransRevenueSharingSummaryHd(RSSummaryID);
                if (summaryHd != null)
                {
                    if (summaryHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        retval = BusinessLayer.GenerateRevenueSharingSummaryAdjustment(
                                                    RSSummaryID,
                                                    Convert.ToInt32(hdnParamedicID.Value),
                                                    AppSession.UserLogin.UserID
                                                );
                        return true;
                    }
                    else
                    {
                        errMessage = string.Format("Rekap jasa medis ini sudah diproses. Harap Refresh Halaman Ini");
                        return false;
                    }
                }
                else
                {
                    errMessage = string.Format("Rekap jasa medis tidak ditemukan");
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}