using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PaymentBillAllocationEntryCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            IsAdd = true;

            vPatientPaymentHd entity = BusinessLayer.GetvPatientPaymentHdList(string.Format("PaymentID = {0}", param)).FirstOrDefault();
            hdnPaymentID.Value = entity.PaymentID.ToString();
            txtPaymentNo.Text = entity.PaymentNo;
            txtPaymentTotal.Text = entity.TotalPaymentAmount.ToString("N");
            hdnPaymentTotal.Value = entity.TotalPaymentAmount.ToString();
            decimal remainingAmount = entity.TotalPaymentAmount - entity.TotalPatientBillAmount;
            txtRemainingAmount.Text = remainingAmount.ToString();

            lstPatientBillPayment = BusinessLayer.GetPatientBillPaymentList(string.Format("PaymentID = {0}", entity.PaymentID));
            lvwView.DataSource = BusinessLayer.GetvPatientBillList(string.Format("RegistrationID = {0} AND (GCTransactionStatus = '{1}' OR PatientBillingID IN (SELECT PatientBillingID FROM PatientBillPayment WHERE PaymentID = {2}))", entity.RegistrationID, Constant.TransactionStatus.OPEN, entity.PaymentID));
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPatientBill entity = e.Item.DataItem as vPatientBill;
                PatientBillPayment billPayment = lstPatientBillPayment.FirstOrDefault(p => p.PatientBillingID == entity.PatientBillingID);
                if (billPayment != null)
                {
                    CheckBox chkPaymentBillAllocationChecked = e.Item.FindControl("chkPaymentBillAllocationChecked") as CheckBox;
                    HtmlInputText txtPaymentBillAllocation = e.Item.FindControl("txtPaymentBillAllocation") as HtmlInputText;
                    HtmlGenericControl divPatientRemainingAmount = e.Item.FindControl("divPatientRemainingAmount") as HtmlGenericControl;
                    chkPaymentBillAllocationChecked.Checked = true;
                    chkPaymentBillAllocationChecked.Enabled = false;

                    divPatientRemainingAmount.InnerHtml = billPayment.PatientPaymentAmount.ToString("N");
                    txtPaymentBillAllocation.Value = billPayment.PatientPaymentAmount.ToString();
                }
            }
        }

        List<PatientBillPayment> lstPatientBillPayment = null;
        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientPaymentHdDao entityHdDao = new PatientPaymentHdDao(ctx);
            PatientBillPaymentDao patientBillPaymentDao = new PatientBillPaymentDao(ctx);
            PatientBillDao patientBillDao = new PatientBillDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            try
            {
                int paymentID = Convert.ToInt32(hdnPaymentID.Value);
                string[] lstParam = hdnResult.Value.Split('|');
                PatientPaymentHd entityHd = entityHdDao.Get(paymentID);
                List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID IN ({0})", hdnListBillID.Value), ctx);
                foreach (string param in lstParam)
                {
                    string[] temp = param.Split(';');
                    PatientBillPayment patientBillPayment = new PatientBillPayment();
                    patientBillPayment.PaymentID = paymentID;
                    patientBillPayment.PatientBillingID = Convert.ToInt32(temp[0]);
                    patientBillPayment.PatientPaymentAmount = Convert.ToDecimal(temp[1]);
                    patientBillPayment.PayerPaymentAmount = 0;
                    patientBillPaymentDao.Insert(patientBillPayment);

                    string oldBillStatus = null;
                    PatientBill patientBill = patientBillDao.Get(patientBillPayment.PatientBillingID);
                    oldBillStatus = patientBill.GCTransactionStatus;
                    patientBill.PaymentID = paymentID;
                    patientBill.TotalPatientPaymentAmount += patientBillPayment.PatientPaymentAmount;
                    if (patientBill.RemainingAmount < 1)
                    {
                        patientBill.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                        List<PatientChargesHd> lstChargesHd = lstPatientChargesHd.Where(p => p.PatientBillingID == patientBillPayment.PatientBillingID).ToList();
                        foreach (PatientChargesHd patientChargesHd in lstChargesHd)
                        {
                            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                            patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientChargesHdDao.Update(patientChargesHd);
                        }
                    }
                    if (oldBillStatus != patientBill.GCTransactionStatus)
                    {
                        patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientBill.LastUpdatedDate = DateTime.Now;
                    }
                    patientBillDao.Update(patientBill);

                    entityHd.TotalPatientBillAmount += patientBillPayment.PatientPaymentAmount;
                }
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entityHd);
                ctx.CommitTransaction();
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