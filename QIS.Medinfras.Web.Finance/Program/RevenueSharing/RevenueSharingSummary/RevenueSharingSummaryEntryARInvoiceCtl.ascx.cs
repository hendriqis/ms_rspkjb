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
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingSummaryEntryARInvoiceCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private RevenueSharingSummaryAdjustmentEntry DetailPage
        {
            get { return (RevenueSharingSummaryAdjustmentEntry)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            string[] paramCtl = param.Split('|');
            hdnRSSummaryIDCtl.Value = paramCtl[0];
            hdnRSSummaryMaxAmountCtl.Value = paramCtl[1];

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format("GCTransactionStatus IN ('{0}','{1}') AND ParamedicID = {2} AND TotalPaymentAmount < TotalClaimedAmount",
                                                        Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED, AppSession.ParamedicID);

            filterExpression += string.Format(" AND ARInvoiceDate BETWEEN '{0}' AND '{1}'",
                                                    Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112),
                                                    Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));

            lstSelectedMember = hdnSelectedARParamedicInvoiceID.Value.Split(',');

            List<vARParamedicInvoiceHd> lstEntity = BusinessLayer.GetvARParamedicInvoiceHdList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vARParamedicInvoiceHd entity = e.Item.DataItem as vARParamedicInvoiceHd;
                TextBox txtCopyRevenueAdjustment = (TextBox)e.Item.FindControl("txtCopyRevenueAdjustment");
                txtCopyRevenueAdjustment.Text = (entity.TotalClaimedAmount - entity.TotalPaymentAmount).ToString();
            }
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Save Entity
        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingSummaryHdDao rsSummaryHdDao = new TransRevenueSharingSummaryHdDao(ctx);
            TransRevenueSharingSummaryAdjDao rsSummaryAdjDao = new TransRevenueSharingSummaryAdjDao(ctx);
            ARInvoiceHdDao arInvoiceHdDao = new ARInvoiceHdDao(ctx);
            ARInvoiceDtDao arInvoiceDtDao = new ARInvoiceDtDao(ctx);
            ARReceivingHdDao arReceivingHdDao = new ARReceivingHdDao(ctx);
            ARReceivingDtDao arReceivingDtDao = new ARReceivingDtDao(ctx);
            ARInvoiceReceivingDao arInvRcvDao = new ARInvoiceReceivingDao(ctx);
            ARInvoiceReceivingDtDao arInvRcvDtDao = new ARInvoiceReceivingDtDao(ctx);

            try
            {
                int RSSummaryID = Convert.ToInt32(hdnRSSummaryIDCtl.Value);

                string filterBP = string.Format("ParamedicID = {0} AND IsDeleted = 0 AND IsActive = 1", AppSession.ParamedicID.ToString());
                List<vCustomer> lstBP = BusinessLayer.GetvCustomerList(filterBP, ctx);
                if (lstBP.Count() > 0)
                {
                    vCustomer oCustomer = lstBP.FirstOrDefault();

                    TransRevenueSharingSummaryHd rsSummaryHd = rsSummaryHdDao.Get(RSSummaryID);
                    if (rsSummaryHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {

                        if (hdnSelectedARParamedicInvoiceID.Value.Substring(0, 1) == ",")
                        {
                            hdnSelectedARParamedicInvoiceID.Value = hdnSelectedARParamedicInvoiceID.Value.Substring(1);
                        }
                        if (hdnSelectedARClaimedAmount.Value.Substring(0, 1) == ",")
                        {
                            hdnSelectedARClaimedAmount.Value = hdnSelectedARClaimedAmount.Value.Substring(1);
                        }

                        string[] lstSelectedInvoiceID = hdnSelectedARParamedicInvoiceID.Value.Split(',');
                        string[] lstSelectedAdjustmentAmount = hdnSelectedARClaimedAmount.Value.Split(',');

                        for (int i = 0; i < lstSelectedInvoiceID.Count(); i++)
                        {
                            ARInvoiceHd entity = arInvoiceHdDao.Get(Convert.ToInt32(lstSelectedInvoiceID[i]));

                            decimal copyAdjAmount = Convert.ToDecimal(lstSelectedAdjustmentAmount[i]);

                            if (copyAdjAmount <= entity.RemainingAmount)
                            {
                                #region Insert RSSummaryAdjustment

                                TransRevenueSharingSummaryAdj adj = new TransRevenueSharingSummaryAdj();
                                adj.RSSummaryID = rsSummaryHd.RSSummaryID;
                                adj.GCRSAdjustmentGroup = Constant.RevenueSharingAdjustmentGroup.PENGURANGAN;
                                adj.GCRSAdjustmentType = Constant.RevenueSharingAdjustmentType.POTONGAN_PIUTANG;
                                adj.AdjustmentAmount = copyAdjAmount;
                                adj.Remarks = string.Format("Potongan piutang dari nomor {0}", entity.ARInvoiceNo);
                                adj.IsTaxed = false;
                                adj.CreatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                int rsSummaryAdjID = rsSummaryAdjDao.InsertReturnPrimaryKeyID(adj);

                                #endregion

                                #region Insert ARReceiving

                                ARReceivingHd rcvHD = new ARReceivingHd();
                                rcvHD.ReceivingDate = DateTime.Now;
                                rcvHD.CustomerGroupID = oCustomer.CustomerGroupID;
                                rcvHD.BusinessPartnerID = oCustomer.BusinessPartnerID;

                                //rcvHD.TotalInvoiceAmount = entity.TotalClaimedAmount;
                                rcvHD.TotalInvoiceAmount = entity.RemainingAmount;
                                rcvHD.TotalReceivingAmount = copyAdjAmount;

                                rcvHD.TotalFeeAmount = 0;
                                rcvHD.CashBackAmount = 0;
                                rcvHD.Remarks = string.Format("Biaya Perawatan Rajal / Ranap dari Jasa Medis dengan nomor rekap {0} ", rsSummaryHd.RSSummaryNo);
                                rcvHD.ARReceivingNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.AR_RECEIVE_PAYER, rcvHD.ReceivingDate, ctx);
                                rcvHD.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                rcvHD.CreatedBy = AppSession.UserLogin.UserID;
                                rcvHD.RSSummaryAdjustmentID = rsSummaryAdjID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                int ARReceivingID = arReceivingHdDao.InsertReturnPrimaryKeyID(rcvHD);

                                ARReceivingDt rcvDT = new ARReceivingDt();
                                rcvDT.ARReceivingID = ARReceivingID;
                                rcvDT.GCARPaymentMethod = Constant.AR_PAYMENT_METHODS.POTONGAN_HONOR_DOKTER;
                                rcvDT.PaymentAmount = copyAdjAmount;
                                rcvDT.CreatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                arReceivingDtDao.Insert(rcvDT);

                                #endregion

                                #region Insert ARInvoiceReceiving

                                ARInvoiceReceiving entityARRcv = new ARInvoiceReceiving();
                                entityARRcv.ARReceivingID = ARReceivingID;
                                entityARRcv.ARInvoiceID = entity.ARInvoiceID;
                                entityARRcv.ReceivingAmount = rcvDT.PaymentAmount;
                                entityARRcv.IsDeleted = false;
                                entityARRcv.CreatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                int oARInvoiceReceivingID = arInvRcvDao.InsertReturnPrimaryKeyID(entityARRcv);

                                #endregion

                                #region Update ARInvoiceHd+Dt & Insert ARInvoiceReceivingDt

                                decimal rcvAll = entityARRcv.ReceivingAmount;
                                decimal rcvNow = 0;

                                List<ARInvoiceDt> lstARInvoiceDT = BusinessLayer.GetARInvoiceDtList(string.Format(
                                                                                    "ARInvoiceID IN ({0}) AND ISNULL(GCTransactionDetailStatus,'') != '{1}' AND PaymentDetailID IN (SELECT ppdi.PaymentDetailID FROM PatientPaymentDtInfo ppdi WHERE ppdi.GCClaimStatus = 'X380^002' AND ppdi.GCFinalStatus = 'X381^002' AND ppdi.GrouperAmountFinal != 0)",
                                                                                    entity.ARInvoiceID, Constant.TransactionStatus.VOID), ctx);
                                foreach (ARInvoiceDt entityDt in lstARInvoiceDT)
                                {
                                    string filterARInvRcvDt = string.Format("ARInvoiceReceivingID = {0} AND ARInvoiceDtID = {1} AND IsDeleted = 0", oARInvoiceReceivingID, entityDt.ID);
                                    List<ARInvoiceReceivingDt> lstARInvRcvDt = BusinessLayer.GetARInvoiceReceivingDtList(filterARInvRcvDt, ctx);
                                    if (lstARInvRcvDt.Count() == 0)
                                    {
                                        ARInvoiceReceivingDt entityARRcvDt = new ARInvoiceReceivingDt();
                                        entityARRcvDt.ARInvoiceReceivingID = oARInvoiceReceivingID;
                                        entityARRcvDt.ARInvoiceDtID = entityDt.ID;

                                        if ((entityDt.ClaimedAmount - entityDt.PaymentAmount) <= rcvAll)
                                        {
                                            rcvNow = (entityDt.ClaimedAmount - entityDt.PaymentAmount);
                                        }
                                        else
                                        {
                                            rcvNow = rcvAll;
                                        }

                                        entityARRcvDt.ReceivingAmount = rcvNow;
                                        rcvAll -= rcvNow;

                                        entityARRcvDt.IsDeleted = false;
                                        entityARRcvDt.CreatedBy = AppSession.UserLogin.UserID;
                                        arInvRcvDtDao.Insert(entityARRcvDt);
                                    }
                                    else
                                    {
                                        ARInvoiceReceivingDt entityARRcvDt = lstARInvRcvDt.FirstOrDefault();

                                        if ((entityDt.ClaimedAmount - entityDt.PaymentAmount) <= rcvAll)
                                        {
                                            rcvNow = (entityDt.ClaimedAmount - entityDt.PaymentAmount);
                                        }
                                        else
                                        {
                                            rcvNow = rcvAll;
                                        }

                                        entityARRcvDt.ReceivingAmount = rcvNow;
                                        rcvAll -= rcvNow;

                                        entityARRcvDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        arInvRcvDtDao.Update(entityARRcvDt);
                                    }

                                    entityDt.PaymentAmount += rcvNow;
                                    if (entityDt.PaymentAmount == entityDt.ClaimedAmount)
                                    {
                                        entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.CLOSED;
                                    }
                                    else
                                    {
                                        entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.PROCESSED;
                                    }
                                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    arInvoiceDtDao.Update(entityDt);

                                }

                                entity.TotalPaymentAmount += rcvDT.PaymentAmount;

                                if (entity.RemainingAmount == 0)
                                {
                                    entity.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                }
                                else
                                {
                                    entity.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                }
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                arInvoiceHdDao.Update(entity);

                                #endregion

                                #region Update RSSummaryAdjustment untuk Remarks

                                TransRevenueSharingSummaryAdj adjUpdate = rsSummaryAdjDao.Get(rsSummaryAdjID);
                                adjUpdate.Remarks = string.Format("Potongan piutang dari nomor tagihan {0} dan nomor penerimaan piutang {1}", entity.ARInvoiceNo, rcvHD.ARReceivingNo);
                                adjUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                rsSummaryAdjDao.Update(adjUpdate);

                                #endregion
                            }
                            else
                            {
                                result = false;
                                errMessage = string.Format("Nilai salin piutang dari nomor tagihan piutang {0} tidak dapat diproses karena melebihi nilai tagihnya.", entity.ARInvoiceNo);
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                                break;
                            }

                        }

                        retval = RSSummaryID.ToString();

                        if (result)
                        {
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Transaksi rekap jasa medis di nomor {0} tidak dapat diubah karena sudah diproses.", rsSummaryHd.RSSummaryNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Maaf, Dokter/Paramedis ini belum memiliki link ke Penjamin Bayar manapun.");
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