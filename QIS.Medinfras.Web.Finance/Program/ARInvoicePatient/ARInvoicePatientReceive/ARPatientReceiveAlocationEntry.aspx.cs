using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARPatientReceiveAlocationEntry : BasePageTrx
    {
        private Decimal amountMain = 0;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AR_PATIENT_RECEIVE_ALOCATION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowProposed = false;
        }

        protected override void InitializeDataControl()
        {
            hdnMRN.Value = AppSession.PatientDetail.MRN.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            bool IsAllowAdd, IsAllowSave, IsAllowVoid, IsAllowNextPrev;
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
            SetToolbarVisibility(ref IsAllowAdd, ref IsAllowSave, ref IsAllowVoid, ref IsAllowNextPrev);
            BindGridView();
        }


        #region Load Entity

        public override int OnGetRowCount()
        {
            string filterExpression = string.Format("GCTransactionStatus = '{0}' AND MRN = {1}", Constant.TransactionStatus.APPROVED, hdnMRN.Value);
            return BusinessLayer.GetvARReceivingHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("GCTransactionStatus = '{0}' AND MRN = {1}", Constant.TransactionStatus.APPROVED, hdnMRN.Value);
            vARReceivingHd entity = BusinessLayer.GetvARReceivingHd(filterExpression, PageIndex, "ARReceivingID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = string.Format("GCTransactionStatus = '{0}' AND MRN = {1}", Constant.TransactionStatus.APPROVED, hdnMRN.Value);
            PageIndex = BusinessLayer.GetvARReceivingHdRowIndex(filterExpression, keyValue, "ARReceivingID DESC");
            vARReceivingHd entity = BusinessLayer.GetvARReceivingHd(filterExpression, PageIndex, "ARReceivingID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";

            if (hdnARReceivingID.Value != "" && hdnARReceivingID.Value != "0")
            {
                filterExpression = string.Format("ARReceivingID = {0}", hdnARReceivingID.Value);
            }

            List<vARReceivingInvoice> lstInvoiceDt = BusinessLayer.GetvARReceivingInvoiceList(filterExpression);
            lvwView.DataSource = lstInvoiceDt;
            lvwView.DataBind();
        }

        private void EntityToControl(vARReceivingHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            hdnARReceivingID.Value = entity.ARReceivingID.ToString();
            hdnTransactionStatus.Value = entity.GCTransactionStatus;
            txtARReceivingNo.Text = entity.ARReceivingNo;
            txtReceivingDate.Text = entity.ReceivingDateInString;
            amountMain = entity.cfReceiveAmount;
            txtPaymentAmount.Text = entity.cfReceiveAmount.ToString(Constant.FormatString.NUMERIC_2);
            txtRemarks.Text = entity.Remarks;

            txtVoucherNo.Text = entity.VoucherNo;
            if (entity.VoucherDate != null && entity.VoucherDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                txtVoucherDate.Text = entity.VoucherDate.ToString(Constant.FormatString.DATE_FORMAT);
            }

            string filterExpression = "1 = 0";

            if (hdnARReceivingID.Value != "" && hdnARReceivingID.Value != "0")
            {
                filterExpression = string.Format("ARReceivingID = {0} AND IsDeleted = 0", hdnARReceivingID.Value);
            }

            List<ARInvoiceReceiving> lstInvoiceDt = BusinessLayer.GetARInvoiceReceivingList(filterExpression);
            Decimal amountDetail = lstInvoiceDt.Sum(a => a.ReceivingAmount);
            Decimal totalAmount = entity.cfReceiveAmount;
            Decimal remainingAmount = totalAmount - amountDetail;

            txtAlocationAmount.Text = amountDetail.ToString(Constant.FormatString.NUMERIC_2);
            txtRemainingAmount.Text = remainingAmount.ToString(Constant.FormatString.NUMERIC_2);

            if (amountMain == amountDetail)
            {
                hdnIsFinish.Value = "1";
            }
            else
            {
                hdnIsFinish.Value = "0";
            }

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DATE_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_FORMAT);
            }

            BindGridView();
        }

        #endregion

        #region Callback

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int ARReceivingID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            if (param[0] == "delete")
            {
                ARReceivingID = Convert.ToInt32(hdnARReceivingID.Value);
                if (OnDeleteEntityDt(ref errMessage, ARReceivingID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpARReceivingID"] = ARReceivingID.ToString();
        }
        #endregion

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARReceivingHdDao eReceivingHd = new ARReceivingHdDao(ctx);
            ARInvoiceReceivingDao arRcvInvDao = new ARInvoiceReceivingDao(ctx);
            ARInvoiceReceivingDtDao arRcvInvDtDao = new ARInvoiceReceivingDtDao(ctx);
            ARInvoiceHdDao eInvoiceHd = new ARInvoiceHdDao(ctx);
            ARInvoiceDtDao eInvoiceDt = new ARInvoiceDtDao(ctx);

            try
            {
                int ARReceivingID = Convert.ToInt32(hdnARReceivingID.Value);
                int ARInvoiceID = Convert.ToInt32(hdnARInvoiceID.Value);
                int ARInvoiceReceivingID = Convert.ToInt32(hdnDtID.Value);

                ARInvoiceReceiving entity = arRcvInvDao.Get(ARInvoiceReceivingID);

                if (!entity.IsDeleted)
                {
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    arRcvInvDao.Update(entity);

                    string filterARInvRcvDt = string.Format("ARInvoiceReceivingID = {0} AND IsDeleted = 0", entity.ID);
                    List<ARInvoiceReceivingDt> lstARInvRcvDt = BusinessLayer.GetARInvoiceReceivingDtList(filterARInvRcvDt, ctx);
                    foreach (ARInvoiceReceivingDt arInvRcvDt in lstARInvRcvDt)
                    {
                        ARInvoiceDt invDt = eInvoiceDt.Get(arInvRcvDt.ARInvoiceDtID);
                        invDt.PaymentAmount -= arInvRcvDt.ReceivingAmount;

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
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        eInvoiceDt.Update(invDt);

                        arInvRcvDt.IsDeleted = true;
                        arInvRcvDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        arRcvInvDtDao.Update(arInvRcvDt);
                    }

                    ARInvoiceHd invHd = eInvoiceHd.Get(ARInvoiceID);
                    invHd.TotalPaymentAmount -= entity.ReceivingAmount;
                    if (invHd.TotalPaymentAmount == 0)
                    {
                        invHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    }
                    else
                    {
                        invHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                    }
                    invHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    eInvoiceHd.Update(invHd);

                    ARReceivingHd rcvHD = eReceivingHd.Get(ARReceivingID);
                    rcvHD.TotalInvoiceAmount -= invHd.TotalClaimedAmount;
                    rcvHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    eReceivingHd.Update(rcvHD);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Piutang tidak dapat diubah. Harap refresh halaman ini.";
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
    }
}