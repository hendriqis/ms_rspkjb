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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARInvoicePatientAlocationCtlX : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        private decimal totalAmountHd = 0, totalAmountDt = 0, remainingAmountHd = 0;

        public override void InitializeDataControl(string param)
        {
            hdnARInvoiceID.Value = param;

            ARInvoiceHd arHD = BusinessLayer.GetARInvoiceHd(Convert.ToInt32(hdnARInvoiceID.Value));
            txtARInvoiceNo.Text = arHD.ARInvoiceNo;
            totalAmountHd = arHD.TotalPaymentAmount;
            txtPaymentAmount.Text = string.Format("Rp {0}", totalAmountHd.ToString("N2"));

            List<ARInvoiceDt> lstDt = BusinessLayer.GetARInvoiceDtList(string.Format("ARInvoiceID = {0}", hdnARInvoiceID.Value));
            totalAmountDt = lstDt.Sum(a => a.PaymentAmount);
            txtTotalPaymentDt.Text = string.Format("Rp {0}", totalAmountDt.ToString("N2"));

            remainingAmountHd = totalAmountHd - totalAmountDt;
            txtRemainingPaymentDt.Text = string.Format("Rp {0}", remainingAmountHd.ToString("N2"));

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<vARInvoiceDt> lstarDT = BusinessLayer.GetvARInvoiceDtList(string.Format("ARInvoiceID = {0}", hdnARInvoiceID.Value), 8, pageCount, "RegistrationID ASC");
            grdView.DataSource = lstarDT;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string param = e.Parameter;
            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnARInvoiceID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                    {
                        string totalAmountString = "Rp 0", totalRemainingString = "Rp 0";
                        ARInvoiceHd arHD = BusinessLayer.GetARInvoiceHd(Convert.ToInt32(hdnARInvoiceID.Value));
                        totalAmountHd = arHD.TotalPaymentAmount;
                        List<ARInvoiceDt> lstDt = BusinessLayer.GetARInvoiceDtList(string.Format("ARInvoiceID = {0}", hdnARInvoiceID.Value));
                        totalAmountDt = lstDt.Sum(a => a.PaymentAmount);
                        totalAmountString = string.Format("Rp {0}", totalAmountDt.ToString("N2"));
                        remainingAmountHd = totalAmountHd - totalAmountDt;
                        totalRemainingString = string.Format("Rp {0}", remainingAmountHd.ToString("N2"));

                        result += string.Format("success|{0}|{1}", totalAmountString, totalRemainingString);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }

            BindGridView(1, true, ref PageCount);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceDtDao entityDtDao = new ARInvoiceDtDao(ctx);
            try
            {
                decimal paymentDt = Convert.ToDecimal(txtPaymentDtAmount.Text);
                decimal claimedDt = Convert.ToDecimal(hdnClaimedAmountDt.Value);
                if (paymentDt <= claimedDt)
                {
                    ARInvoiceHd arHD = BusinessLayer.GetARInvoiceHd(Convert.ToInt32(hdnARInvoiceID.Value));
                    totalAmountHd = arHD.TotalPaymentAmount;
                    List<ARInvoiceDt> lstDt = BusinessLayer.GetARInvoiceDtList(string.Format("ARInvoiceID = {0}", hdnARInvoiceID.Value));
                    totalAmountDt = lstDt.Sum(a => a.PaymentAmount);
                    remainingAmountHd = totalAmountHd - totalAmountDt;

                    if (paymentDt <= remainingAmountHd)
                    {
                        ARInvoiceDt entityDt = BusinessLayer.GetARInvoiceDtList(string.Format(
                            "ARInvoiceID = {0} AND RegistrationID = {1} AND PaymentID = {2}", hdnARInvoiceID.Value, hdnARDtRegistrationID.Value, hdnARDtPaymentID.Value)).FirstOrDefault();
                        entityDt.PaymentAmount = Convert.ToDecimal(txtPaymentDtAmount.Text);
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, jumlah bayar tidak bisa melebihi jumlah bayar invoice";
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, jumlah bayar tidak bisa melebihi jumlah klaim";
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
    }
}