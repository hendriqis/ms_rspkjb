using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ProcessVoidTransferDownPayment : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.PROCESS_VOID_TRANSFER_DOWN_PAYMENT;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vConsultVisit9 entityVisit = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            hdnDepartmentID.Value = entityVisit.DepartmentID.ToString();

            vConsultVisit9 entityVisitLinked = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0}", entityVisit.LinkedRegistrationID)).FirstOrDefault();
            hdnLinkedRegistrationID.Value = entityVisitLinked.RegistrationID.ToString();
            hdnLinkedDepartmentID.Value = entityVisitLinked.DepartmentID.ToString();

            BindGridDetail();
        }

        private void BindGridDetail()
        {
            string filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}' AND GCPaymentType = '{2}' AND IsTransfered = 1 AND SourcePaymentID IS NULL", hdnLinkedRegistrationID.Value, Constant.TransactionStatus.VOID, Constant.PaymentType.DOWN_PAYMENT);
            List<vPatientPaymentHd> lst = BusinessLayer.GetvPatientPaymentHdList(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            if (param[0] == "voidtransfer")
            {
                if (VoidTransferDownPayment(param, ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else
            {
                BindGridDetail();
            }


            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool VoidTransferDownPayment(string[] param, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientPaymentHdDao paymentHdDao = new PatientPaymentHdDao(ctx);

            try
            {
                PatientPaymentHd paymentHd = paymentHdDao.Get(Convert.ToInt32(param[1]));
                if (paymentHd.IsTransfered)
                {
                    #region Update PaymentHD
                    paymentHd.IsTransfered = false;
                    paymentHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    paymentHd.LastUpdatedDate = DateTime.Now;

                    paymentHdDao.Update(paymentHd);
                    #endregion

                    string filterSourcePayment = string.Format("SourcePaymentID = {0} AND IsTransfered = 0 AND GCTransactionStatus != '{1}'", paymentHd.PaymentID, Constant.TransactionStatus.VOID);
                    List<PatientPaymentHd> lstPayment = BusinessLayer.GetPatientPaymentHdList(filterSourcePayment, ctx);
                    foreach (PatientPaymentHd entity in lstPayment)
                    {
                        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entity.GCVoidReason = Constant.DeleteReason.OTHER;
                        entity.VoidReason = "Proses batal transfer pembayaran uang muka dari nomor pembayaran " + paymentHd.PaymentNo;
                        entity.VoidBy = AppSession.UserLogin.UserID;
                        entity.VoidDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        paymentHdDao.Update(entity);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pembayaran dengan nomor " + paymentHd.PaymentNo + " belum pernah ditransfer sebelumnya.";
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
            return result;
        }

    }
}