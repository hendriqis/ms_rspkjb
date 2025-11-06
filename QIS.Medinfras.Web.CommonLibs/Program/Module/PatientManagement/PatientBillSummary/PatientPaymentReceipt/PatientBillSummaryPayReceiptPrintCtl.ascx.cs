using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using System.Data;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryPayReceiptPrintCtl : BaseViewPopupCtl
    {
        private string[] lstSelectedPayment = null;
        public override void InitializeDataControl(string param)
        {
            hdnParam1.Value = param.Split('-')[0];
            hdnParam2.Value = param.Split('-')[1];
            hdnRegistrationID.Value = hdnParam1.Value.Split('|')[0];
            //hdnPaymentID.Value = hdnParam.Value.Split('|')[1];
            hdnDepartmentID.Value = hdnParam1.Value.Split('|')[1];
            hdnPatientName.Value = hdnParam1.Value.Split('|')[3];
            lstSelectedPayment = hdnParam2.Value.Split('|');
            txtReceiptName.Text = hdnPatientName.Value;
            Helper.SetControlEntrySetting(txtReceiptName, new ControlEntrySetting(true, true, true), "mpTrxPopup");
        }

        protected void cbpPatientPaymentReceiptProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnSaveRecord(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PaymentReceiptDao entityDao = new PaymentReceiptDao();
            PatientPaymentHdDao entityPaymentDao = new PatientPaymentHdDao();

            try
            {
                string transactionCode = "";
                PaymentReceipt entity = null;
                entity = new PaymentReceipt();
                entity.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                    transactionCode = Constant.TransactionCode.ER_PAYMENT_RECEIPT;
                else if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                    transactionCode = Constant.TransactionCode.IP_PAYMENT_RECEIPT;
                else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    transactionCode = Constant.TransactionCode.OP_PAYMENT_RECEIPT;
                else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                {
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        transactionCode = Constant.TransactionCode.LABORATORY_PAYMENT_RECEIPT;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        transactionCode = Constant.TransactionCode.IMAGING_PAYMENT_RECEIPT;
                    else
                        transactionCode = Constant.TransactionCode.OTHER_PAYMENT_RECEIPT;
                }
                entity.PaymentReceiptDateTime = entity.LastPrintedDate = DateTime.Now;
                entity.PaymentReceiptNo = BusinessLayer.GenerateTransactionNo(transactionCode, entity.PaymentReceiptDateTime);
                //entity.PaymentID = Convert.ToInt32(hdnPaymentID.Value);
                entity.PrintAsName = txtReceiptName.Text;
                entity.PrintNumber = 1;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.ReceiptAmount = Convert.ToDecimal(hdnParam1.Value.Split('|')[2]);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityDao.Insert(entity);
                entity.PaymentReceiptID = BusinessLayer.GetPaymentReceiptMaxID(ctx);
                hdnPaymentReceipt.Value = entity.PaymentReceiptID.ToString();
                foreach (string param in lstSelectedPayment)
                {
                    int paymentID = Convert.ToInt32(param);

                    PatientPaymentHd entityPayment = entityPaymentDao.Get(paymentID);
                    entityPayment.PaymentReceiptID = entity.PaymentReceiptID;
                    entityPayment.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityPaymentDao.Update(entityPayment);
                }

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