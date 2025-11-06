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
    public partial class ARInvoicePayerAlocationCtl : BaseEntryPopupCtl
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

            string paramARInvoiceID = "0";
            if (param.Contains("|"))
            {
                paramARInvoiceID = param.Split('|')[0];
                hdnIsFromAlocation.Value = "1";
            }
            else
            {
                paramARInvoiceID = param;
                hdnIsFromAlocation.Value = "0";
            }

            hdnARInvoiceID.Value = paramARInvoiceID;

            string lstReceiveNo = "";

            string filterReceiving1 = string.Format("GCTransactionStatus != '{0}' AND ARReceivingID IN (SELECT ARReceivingID FROM ARInvoiceReceiving WHERE ARInvoiceID = {1} AND IsDeleted = 0)",
                                                        Constant.TransactionStatus.VOID, hdnARInvoiceID.Value);
            List<ARReceivingHd> lstRcvHD = BusinessLayer.GetARReceivingHdList(filterReceiving1);
            foreach (ARReceivingHd rcvHd in lstRcvHD)
            {
                if (lstReceiveNo != "")
                {
                    lstReceiveNo = lstReceiveNo + ", " + rcvHd.ARReceivingNo;
                }
                else
                {
                    lstReceiveNo = rcvHd.ARReceivingNo;
                }
            }
            txtARReceivingNo.Text = lstReceiveNo;

            string filterParam = string.Format("GCTransactionStatus != '{0}' AND ARInvoiceID IN (SELECT ARInvoiceID FROM ARInvoiceReceiving WHERE ARInvoiceID = {1} AND IsDeleted = 0)",
                                        Constant.TransactionStatus.VOID, hdnARInvoiceID.Value);
            List<vARInvoiceHd> lstARParam = BusinessLayer.GetvARInvoiceHdList(filterParam);
            foreach (vARInvoiceHd invHD in lstARParam)
            {
                lstARInvoiceIDParam += string.Format("{0},", invHD.ARInvoiceID);
            }
            if (lstARInvoiceIDParam != "")
            {
                lstARInvoiceIDParam = lstARInvoiceIDParam.Substring(0, lstARInvoiceIDParam.Length - 1);
            }

            txtCustomerInfo.Text = string.Format("{0} ({1})", lstARParam.FirstOrDefault().BusinessPartnerName, lstARParam.FirstOrDefault().BusinessPartnerCode);
            txtARInvoiceNo.Text = lstARParam.FirstOrDefault().ARInvoiceNo;
            txtInvoiceTotal.Text = lstARParam.Sum(a => a.TotalClaimedAmount).ToString();

            string filterReceiving2 = string.Format("ARInvoiceID = {0} AND IsDeleted = 0", hdnARInvoiceID.Value);
            List<ARInvoiceReceiving> lstInvRcv = BusinessLayer.GetARInvoiceReceivingList(filterReceiving2);
            txtPaymentAmount.Text = lstInvRcv.Sum(a => a.ReceivingAmount).ToString();

            BindGridView();
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
            string arinvTemp = hdnARInvoiceID.Value + ",";
            lstSelectedMember = arinvTemp.Split(',');

            string filterDt = string.Format("ARInvoiceID IN ({0}) AND GCTransactionStatus != '{1}' AND GCTransactionDetailStatus != '{1}' ORDER BY ARInvoiceID, PaymentID, PaymentDetailID", hdnARInvoiceID.Value, Constant.TransactionStatus.VOID);
            List<vARInvoiceDt> lstarDT = BusinessLayer.GetvARInvoiceDtList(filterDt);
            hdnTotalSelectedDtPaymentAmount.Value = lstarDT.Sum(a => a.PaymentAmount).ToString();
            hdnTotalSelectedDtClaimAmount.Value = lstarDT.Sum(a => a.ClaimedAmount).ToString();

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

                if (hdnIsFromAlocation.Value == "1")
                {
                    chkIsSelectedDetail.Enabled = false;
                    txtPembayaran.ReadOnly = true;
                }
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceDtDao invDtDao = new ARInvoiceDtDao(ctx);
            ARInvoiceReceivingDao arRcvInvDao = new ARInvoiceReceivingDao(ctx);
            ARInvoiceReceivingDtDao arRcvInvDtDao = new ARInvoiceReceivingDtDao(ctx);
            try
            {
                if (hdnIsFromAlocation.Value == "0")
                {
                    if (hdnSelectedDtID.Value.Substring(0, 1) == ",")
                    {
                        hdnSelectedDtID.Value = hdnSelectedDtID.Value.Substring(1);
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

                    string[] lstSelectedDtID = hdnSelectedDtID.Value.Split(',');
                    string[] lstSelectedDtPaymentDetailID = hdnSelectedDtPaymentDetailID.Value.Split(',');
                    string[] lstSelectedDtARInvoiceID = hdnSelectedDtARInvoiceID.Value.Split(',');
                    string[] lstSelectedDtRegistrationID = hdnSelectedDtRegistrationID.Value.Split(',');
                    string[] lstSelectedDtPaymentID = hdnSelectedDtPaymentID.Value.Split(',');
                    string[] lstSelectedDtPaymentAmount = hdnSelectedDtPaymentAmount.Value.Split(',');

                    string arReceive = hdnARInvoiceID.Value;
                    string arInvoiceDtID = hdnSelectedDtID.Value;
                    string arInv = hdnSelectedDtARInvoiceID.Value;
                    string paymentDt = hdnSelectedDtPaymentDetailID.Value;

                    decimal rcv = Convert.ToDecimal(hdnTotalInputSelectedDtPaymentAmount.Value);
                    decimal outstanding = Convert.ToDecimal(hdnInvoicePaymentOutstandingAmountDt.Value);

                    if (rcv <= outstanding)
                    {
                        string filterDt = string.Format("ARInvoiceID IN ({0}) AND ID IN ({1}) AND GCTransactionDetailStatus != '{2}'", arInv, arInvoiceDtID, Constant.TransactionStatus.VOID);
                        List<ARInvoiceDt> lstARInvDt = BusinessLayer.GetARInvoiceDtList(filterDt);

                        for (int i = 0; i < lstARInvDt.Count(); i++)
                        {
                            int ARInvoiceDtID = Convert.ToInt32(lstSelectedDtID[i]);
                            int ARInvoiceID = Convert.ToInt32(lstSelectedDtARInvoiceID[i]);
                            int RegistrationID = Convert.ToInt32(lstSelectedDtRegistrationID[i]);
                            int PaymentID = Convert.ToInt32(lstSelectedDtPaymentID[i]);
                            int PaymentDetailID = Convert.ToInt32(lstSelectedDtPaymentDetailID[i]);

                            ARInvoiceDt invDt = invDtDao.Get(ARInvoiceDtID);
                            if (invDt != null)
                            {
                                invDt.PaymentAmount = Convert.ToDecimal(lstSelectedDtPaymentAmount[i]);
                                invDt.LastUpdatedBy = AppSession.UserLogin.UserID;
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
                else
                {
                    result = false;
                    errMessage = "Maaf, alokasi per detail penagihan piutang hanya dapat dilakukan di menu Approval Penagihan Piutang Instansi (right panel Task > Alokasi Penerimaan Piutang Instansi).";
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