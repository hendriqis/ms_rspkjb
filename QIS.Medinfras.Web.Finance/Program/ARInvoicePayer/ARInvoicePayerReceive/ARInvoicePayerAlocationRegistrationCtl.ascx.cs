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
    public partial class ARInvoicePayerAlocationRegistrationCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string lstARInvoiceIDParam = "";
        private string[] lstSelectedMember = null;
        private string[] lstNilai = null;

        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            hdnARReceivingID.Value = param;

            ARReceivingHd receivingHD = BusinessLayer.GetARReceivingHd(Convert.ToInt32(param));
            txtARReceivingNo.Text = receivingHD.ARReceivingNo;
            txtPaymentAmount.Text = receivingHD.cfReceiveAmount.ToString();

            string filterParam = string.Format("GCTransactionStatus != '{0}' AND ARInvoiceID IN (SELECT ARInvoiceID FROM ARInvoiceReceiving WHERE ARReceivingID = {1} AND IsDeleted = 0)",
                                        Constant.TransactionStatus.VOID, hdnARReceivingID.Value);
            List<ARInvoiceHd> lstARParam = BusinessLayer.GetARInvoiceHdList(filterParam);
            foreach (ARInvoiceHd invHD in lstARParam)
            {
                lstARInvoiceIDParam += string.Format("{0},", invHD.ARInvoiceID);
            }
            if (lstARInvoiceIDParam != "")
            {
                lstARInvoiceIDParam = lstARInvoiceIDParam.Substring(0, lstARInvoiceIDParam.Length - 1);
            }

            BindGridViewInvoice();
            BindGridView();
        }

        private void BindGridViewInvoice()
        {
            ListView lvwARInvoice = (ListView)ddeInvoiceNoDetail.FindControl("lvwInvoice");
            string filter = string.Format("BusinessPartnerID = {0} AND GCTransactionStatus IN ('{1}','{2}') AND ARInvoiceID IN ({3})",
                                        AppSession.BusinessPartnerID, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED, lstARInvoiceIDParam);
            List<ARInvoiceHd> lst = BusinessLayer.GetARInvoiceHdList(filter);

            lvwARInvoice.DataSource = lst;
            lvwARInvoice.DataBind();
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string arInvParam = hdnListInvoiceIDCtl.Value;
            if (arInvParam == "")
            {
                arInvParam = "0";
            }
            lstSelectedMember = hdnListInvoiceIDCtl.Value.Split(',');

            string filterDt = string.Format("ARInvoiceID IN ({0}) AND GCTransactionStatus != '{1}' AND GCTransactionDetailStatus != '{1}' ORDER BY ARInvoiceID, PaymentID, PaymentDetailID", arInvParam, Constant.TransactionStatus.VOID);
            List<vARInvoiceDt> lstarDT = BusinessLayer.GetvARInvoiceDtList(filterDt);
            hdnTotalSelectedDtPaymentAmount.Value = lstarDT.Sum(a => a.PaymentAmount).ToString();
            
            lvwView.DataSource = lstarDT;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vARInvoiceDt entity = e.Item.DataItem as vARInvoiceDt;
                CheckBox chkIsSelectedDetail = (CheckBox)e.Item.FindControl("chkIsSelectedDetail");
                TextBox txtPembayaran = (TextBox)e.Item.FindControl("txtPembayaran");
                txtPembayaran.Text = entity.TotalOutstandingPayment.ToString();

                if (lstSelectedMember.Contains(entity.PaymentDetailID.ToString()))
                {
                    int idx = Array.IndexOf(lstSelectedMember, entity.PaymentDetailID.ToString());
                    chkIsSelectedDetail.Checked = true;
                    txtPembayaran.ReadOnly = false;
                    txtPembayaran.Attributes.Add("hiddenVal", lstNilai[idx].ToString());
                    txtPembayaran.Text = lstNilai[idx].ToString();
                }
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceDtDao invDtDao = new ARInvoiceDtDao(ctx);
            try
            {
                if (hdnSelectedDtARInvoiceDtID.Value.Substring(0, 1) == ",")
                {
                    hdnSelectedDtARInvoiceDtID.Value = hdnSelectedDtARInvoiceDtID.Value.Substring(1);
                }
                if (hdnSelectedDtPaymentDetailID.Value.Substring(0, 1) == ",")
                {
                    hdnSelectedDtPaymentDetailID.Value = hdnSelectedDtPaymentDetailID.Value.Substring(1);
                }
                if (hdnSelectedDtARInvoiceID.Value.Substring(0, 1) == ",")
                {
                    hdnSelectedDtARInvoiceID.Value = hdnSelectedDtARInvoiceID.Value.Substring(1);
                }
                if (hdnSelectedDtRegistrationID.Value.Substring(0, 1) == ",")
                {
                    hdnSelectedDtRegistrationID.Value = hdnSelectedDtRegistrationID.Value.Substring(1);
                }
                if (hdnSelectedDtPaymentID.Value.Substring(0, 1) == ",")
                {
                    hdnSelectedDtPaymentID.Value = hdnSelectedDtPaymentID.Value.Substring(1);
                }
                if (hdnSelectedDtPaymentAmount.Value.Substring(0, 1) == ",")
                {
                    hdnSelectedDtPaymentAmount.Value = hdnSelectedDtPaymentAmount.Value.Substring(1);
                }

                string[] lstSelectedARInvoiceDtID = hdnSelectedDtARInvoiceDtID.Value.Split(',');
                string[] lstSelectedDtPaymentDetailID = hdnSelectedDtPaymentDetailID.Value.Split(',');
                string[] lstSelectedDtARInvoiceID = hdnSelectedDtARInvoiceID.Value.Split(',');
                string[] lstSelectedDtRegistrationID = hdnSelectedDtRegistrationID.Value.Split(',');
                string[] lstSelectedDtPaymentID = hdnSelectedDtPaymentID.Value.Split(',');
                string[] lstSelectedDtPaymentAmount = hdnSelectedDtPaymentAmount.Value.Split(',');

                string arReceive = hdnARReceivingID.Value;
                string arInv = hdnSelectedDtARInvoiceID.Value;
                string paymentDt = hdnSelectedDtPaymentDetailID.Value;

                //List<ARInvoiceReceiving> invRcv = BusinessLayer.GetARInvoiceReceivingList(string.Format(
                //                    "ARReceivingID IN ({0}) AND ARInvoiceID IN ({1}) AND IsDeleted = 0", arReceive, arInv));
                //hdnInvoicePaymentOutstandingAmountDt.Value = invRcv.Sum(a => a.ReceivingAmount).ToString();

                //decimal invrcv = Convert.ToDecimal(hdnInvoicePaymentOutstandingAmountDt.Value);
                decimal rcv = Convert.ToDecimal(hdnTotalInputSelectedDtPaymentAmount.Value);
                decimal outstanding = Convert.ToDecimal(hdnInvoicePaymentOutstandingAmountDt.Value);

                if (rcv <= outstanding)
                {
                    string filterDt = string.Format("ARInvoiceID IN ({0}) AND PaymentDetailID IN ({1})", arInv, paymentDt);
                    List<ARInvoiceDt> lstARInvDt = BusinessLayer.GetARInvoiceDtList(filterDt);

                    for (int i = 0; i < lstARInvDt.Count(); i++)
                    {
                        int ARInvoiceDtID = Convert.ToInt32(lstSelectedARInvoiceDtID[i]);
                        int ARInvoiceID = Convert.ToInt32(lstSelectedDtARInvoiceID[i]);
                        int RegistrationID = Convert.ToInt32(lstSelectedDtRegistrationID[i]);
                        int PaymentID = Convert.ToInt32(lstSelectedDtPaymentID[i]);
                        int PaymentDetailID = Convert.ToInt32(lstSelectedDtPaymentDetailID[i]);

                        ARInvoiceDt invDt = BusinessLayer.GetARInvoiceDt(ARInvoiceDtID);
                        if (invDt != null)
                        {
                            invDt.PaymentAmount = Convert.ToDecimal(lstSelectedDtPaymentAmount[i]);
                            invDtDao.Update(invDt);
                        }
                        else
                        {
                            result = false;
                            errMessage = "Maaf, tidak ditemukan data detail piutang tersebut.";
                            ctx.RollBackTransaction();
                        }
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, nilai alokasi tidak bisa melebihi nilai penerimaan.";
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