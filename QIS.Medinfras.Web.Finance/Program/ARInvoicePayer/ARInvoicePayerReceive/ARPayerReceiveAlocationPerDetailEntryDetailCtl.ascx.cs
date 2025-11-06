using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARPayerReceiveAlocationPerDetailEntryDetailCtl : BaseEntryPopupCtl
    {
        private string[] lstSelectedMember = null;

        private ARPayerReceiveAlocationPerDetailEntry DetailPage
        {
            get { return (ARPayerReceiveAlocationPerDetailEntry)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnARReceivingIDCtl.Value = param;

            List<Variable> lstFilterBy = new List<Variable>();
            lstFilterBy.Add(new Variable { Code = "0", Value = "--ALL--" });
            lstFilterBy.Add(new Variable { Code = "1", Value = "Full Outstanding" });
            lstFilterBy.Add(new Variable { Code = "2", Value = "Partial Outstanding" });
            Methods.SetComboBoxField<Variable>(cboFilterBy, lstFilterBy, "Value", "Code");
            cboFilterBy.Value = "1";

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format("BusinessPartnerID = {0} AND GCTransactionStatus IN ('{1}','{2}')",
                                        AppSession.BusinessPartnerID, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED);

            if (cboFilterBy.Value.ToString() == "1")
            {
                filterExpression += " AND PaymentAmount = 0";
            }
            else if (cboFilterBy.Value.ToString() == "2")
            {
                filterExpression += " AND PaymentAmount < ClaimedAmount AND PaymentAmount != 0";
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            filterExpression += " ORDER BY ARInvoiceNo, ID, PaymentNo";

            lstSelectedMember = hdnSelectedARInvoiceDtID.Value.Split(',');

            List<vARInvoiceDt2> lstEntity = BusinessLayer.GetvARInvoiceDt2List(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vARInvoiceDt2 entity = e.Item.DataItem as vARInvoiceDt2;

                TextBox txtAlocationAmountCtl = e.Item.FindControl("txtAlocationAmountCtl") as TextBox;
                txtAlocationAmountCtl.Text = entity.OutstandingPayment.ToString();
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
            ARReceivingHdDao eReceivingHd = new ARReceivingHdDao(ctx);
            ARInvoiceReceivingDao entityARRcvDao = new ARInvoiceReceivingDao(ctx);
            ARInvoiceReceivingDtDao entityARRcvDtDao = new ARInvoiceReceivingDtDao(ctx);
            ARInvoiceHdDao eInvoiceHd = new ARInvoiceHdDao(ctx);
            ARInvoiceDtDao eInvoiceDt = new ARInvoiceDtDao(ctx);

            try
            {
                string filterExpression = string.Format("ID IN ({0})", hdnSelectedARInvoiceDtID.Value.Substring(1));
                List<ARInvoiceDt> lstAR = BusinessLayer.GetARInvoiceDtList(filterExpression, ctx);

                List<String> lstSelectedARInv = hdnSelectedARInvoiceDtID.Value.Split(',').ToList();
                List<String> lstSelectedRcvAmount = hdnSelectedARReceivingAmount.Value.Split(',').ToList();
                lstSelectedARInv.RemoveAt(0);
                lstSelectedRcvAmount.RemoveAt(0);

                ARReceivingHd rcvHD = eReceivingHd.Get(Convert.ToInt32(hdnARReceivingIDCtl.Value));

                string filterInvRcvList = string.Format("ARReceivingID = {0} AND IsDeleted = 0", hdnARReceivingIDCtl.Value);
                List<ARInvoiceReceiving> lstInvoiceDt = BusinessLayer.GetARInvoiceReceivingList(filterInvRcvList);
                decimal tempRcv = 0;
                decimal remainingRcv = rcvHD.cfReceiveAmount - lstInvoiceDt.Sum(a => a.ReceivingAmount);
                decimal invAmount = 0;

                int count = 0;
                foreach (ARInvoiceDt entity in lstAR)
                {
                    ARInvoiceHd entityHd = eInvoiceHd.Get(entity.ARInvoiceID);

                    decimal paymentAmount = Convert.ToDecimal(lstSelectedRcvAmount[count]);
                    decimal paymentAmountForDetail = Convert.ToDecimal(lstSelectedRcvAmount[count]);

                    string filterARInvRcv = string.Format("ARInvoiceID = {0} AND ARReceivingID = {1} AND IsDeleted = 0", entity.ARInvoiceID, rcvHD.ARReceivingID);
                    List<ARInvoiceReceiving> lstARInvRcv = BusinessLayer.GetARInvoiceReceivingList(filterARInvRcv, ctx);
                    if (lstARInvRcv.Count() == 0)
                    {
                        ARInvoiceReceiving entityARRcv = new ARInvoiceReceiving();
                        entityARRcv.ARReceivingID = rcvHD.ARReceivingID;
                        entityARRcv.ARInvoiceID = Convert.ToInt32(entity.ARInvoiceID);

                        decimal invRemaining = (entity.ClaimedAmount - entity.PaymentAmount);

                        if (paymentAmount <= remainingRcv)
                        {
                            if (paymentAmount <= invRemaining)
                            {
                                paymentAmountForDetail = paymentAmount;
                                entityARRcv.ReceivingAmount += paymentAmount;
                                remainingRcv -= paymentAmount;
                            }
                            else
                            {
                                paymentAmountForDetail = invRemaining;
                                entityARRcv.ReceivingAmount += invRemaining;
                                remainingRcv -= invRemaining;
                            }
                        }
                        else
                        {
                            if (remainingRcv <= invRemaining)
                            {
                                paymentAmountForDetail = remainingRcv;
                                entityARRcv.ReceivingAmount += remainingRcv;
                                remainingRcv -= remainingRcv;
                            }
                            else
                            {
                                paymentAmountForDetail = invRemaining;
                                entityARRcv.ReceivingAmount += invRemaining;
                                remainingRcv -= invRemaining;
                            }
                        }

                        entityARRcv.IsDeleted = false;
                        entityARRcv.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        int oARInvoiceReceivingID = entityARRcvDao.InsertReturnPrimaryKeyID(entityARRcv);

                        string filterARInvRcvDt = string.Format("ARInvoiceReceivingID = {0} AND ARInvoiceDtID = {1} AND IsDeleted = 0", oARInvoiceReceivingID, entity.ID);
                        List<ARInvoiceReceivingDt> lstARInvRcvDt = BusinessLayer.GetARInvoiceReceivingDtList(filterARInvRcvDt, ctx);
                        if (lstARInvRcvDt.Count() == 0)
                        {
                            ARInvoiceReceivingDt entityARRcvDt = new ARInvoiceReceivingDt();
                            entityARRcvDt.ARInvoiceReceivingID = oARInvoiceReceivingID;
                            entityARRcvDt.ARInvoiceDtID = entity.ID;
                            entityARRcvDt.ReceivingAmount = paymentAmountForDetail;
                            entityARRcvDt.IsDeleted = false;
                            entityARRcvDt.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityARRcvDtDao.Insert(entityARRcvDt);
                        }
                        else
                        {
                            ARInvoiceReceivingDt entityARRcvDt = lstARInvRcvDt.FirstOrDefault();
                            entityARRcvDt.ReceivingAmount += paymentAmountForDetail;
                            entityARRcvDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityARRcvDtDao.Update(entityARRcvDt);
                        }

                        invAmount += paymentAmountForDetail;
                        entity.PaymentAmount += paymentAmountForDetail;
                        entityHd.TotalPaymentAmount += paymentAmountForDetail;
                        if (entity.PaymentAmount == entity.ClaimedAmount)
                        {
                            entity.GCTransactionDetailStatus = Constant.TransactionStatus.CLOSED;
                        }
                        else
                        {
                            entity.GCTransactionDetailStatus = Constant.TransactionStatus.PROCESSED;
                        }
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        eInvoiceDt.Update(entity);
                    }
                    else
                    {
                        ARInvoiceReceiving entityARRcv = lstARInvRcv.FirstOrDefault();
                        int oARInvoiceReceivingID = entityARRcv.ID;

                        decimal invRemaining = (entity.ClaimedAmount - entity.PaymentAmount);

                        if (paymentAmount <= remainingRcv)
                        {
                            if (paymentAmount <= invRemaining)
                            {
                                paymentAmountForDetail = paymentAmount;
                                entityARRcv.ReceivingAmount += paymentAmount;
                                remainingRcv -= paymentAmount;
                            }
                            else
                            {
                                paymentAmountForDetail = invRemaining;
                                entityARRcv.ReceivingAmount += invRemaining;
                                remainingRcv -= invRemaining;
                            }
                        }
                        else
                        {
                            if (remainingRcv <= invRemaining)
                            {
                                paymentAmountForDetail = remainingRcv;
                                entityARRcv.ReceivingAmount += remainingRcv;
                                remainingRcv -= remainingRcv;
                            }
                            else
                            {
                                paymentAmountForDetail = invRemaining;
                                entityARRcv.ReceivingAmount += invRemaining;
                                remainingRcv -= invRemaining;
                            }
                        }

                        entityARRcv.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityARRcvDao.Update(entityARRcv);

                        string filterARInvRcvDt = string.Format("ARInvoiceReceivingID = {0} AND ARInvoiceDtID = {1} AND IsDeleted = 0", oARInvoiceReceivingID, entity.ID);
                        List<ARInvoiceReceivingDt> lstARInvRcvDt = BusinessLayer.GetARInvoiceReceivingDtList(filterARInvRcvDt, ctx);
                        if (lstARInvRcvDt.Count() == 0)
                        {
                            ARInvoiceReceivingDt entityARRcvDt = new ARInvoiceReceivingDt();
                            entityARRcvDt.ARInvoiceReceivingID = oARInvoiceReceivingID;
                            entityARRcvDt.ARInvoiceDtID = entity.ID;
                            entityARRcvDt.ReceivingAmount = paymentAmountForDetail;
                            entityARRcvDt.IsDeleted = false;
                            entityARRcvDt.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityARRcvDtDao.Insert(entityARRcvDt);
                        }
                        else
                        {
                            ARInvoiceReceivingDt entityARRcvDt = lstARInvRcvDt.FirstOrDefault();
                            entityARRcvDt.ReceivingAmount += paymentAmountForDetail;
                            entityARRcvDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityARRcvDtDao.Update(entityARRcvDt);
                        }

                        invAmount += paymentAmountForDetail;
                        entity.PaymentAmount += paymentAmountForDetail;
                        entityHd.TotalPaymentAmount += paymentAmountForDetail;
                        if (entity.PaymentAmount == entity.ClaimedAmount)
                        {
                            entity.GCTransactionDetailStatus = Constant.TransactionStatus.CLOSED;
                        }
                        else
                        {
                            entity.GCTransactionDetailStatus = Constant.TransactionStatus.PROCESSED;
                        }
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        eInvoiceDt.Update(entity);
                    }

                    if (entityHd.TotalPaymentAmount == entityHd.TotalClaimedAmount)
                    {
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                    }
                    else
                    {
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                    }
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    eInvoiceHd.Update(entityHd);

                    count++;
                }


                rcvHD.TotalInvoiceAmount += invAmount;
                rcvHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                eReceivingHd.Update(rcvHD); // UPDATE AR RECEIVING HD

                retval = hdnARReceivingIDCtl.Value;
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
        #endregion
    }
}