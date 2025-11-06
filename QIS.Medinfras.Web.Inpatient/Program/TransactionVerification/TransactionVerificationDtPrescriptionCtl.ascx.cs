using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionVerificationDtPrescriptionCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');
            hdnTransactionHdID.Value = temp[0];
            Int32 PrescriptionOrderID = Convert.ToInt32(temp[1]);
            vPrescriptionOrderHd entity = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("PrescriptionOrderID = {0}", PrescriptionOrderID)).FirstOrDefault();
            if (entity.GCChargesTransactionStatus == Constant.TransactionStatus.PROCESSED)
                btnMPEntryPopupVerified.Style.Add("display", "none");
            txtTransactionNo.Text = entity.ChargesTransactionNo;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtTransactionDate.Text = entity.PrescriptionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = entity.PrescriptionTime;

            BindGrid(PrescriptionOrderID);
        }

        public void BindGrid(Int32 PrescriptionOrderID) 
        {
            String filterExpression = String.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", PrescriptionOrderID);
            List<vPrescriptionOrderDtCustom> lstPrescriptionDt = BusinessLayer.GetvPrescriptionOrderDtCustomList(filterExpression);
            lvwPrescription.DataSource = lstPrescriptionDt;
            lvwPrescription.DataBind();

            txtTotal.Text = lstPrescriptionDt.Sum(x => x.LineAmount).ToString("N");
            txtTotalPatient.Text = lstPrescriptionDt.Sum(x => x.PatientAmount).ToString("N");
            txtTotalPayer.Text = lstPrescriptionDt.Sum(x => x.PayerAmount).ToString("N");
        }

        protected void cbpProcessVerification_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnProcessRecord(ref errMessage))
                result += "success";
            else
                result += string.Format("fail|{0}", errMessage);
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        String[] lstSelectedMember, lstUnselectedMember = null;
        private bool OnProcessRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            lstSelectedMember = hdnSelectedMember.Value.Substring(0).Split(',');
            lstUnselectedMember = hdnUnselectedMember.Value.Substring(0).Split(',');
            try
            {
                VerifyProcess(ctx, lstSelectedMember, lstUnselectedMember, hdnTransactionHdID.Value);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private void VerifyProcess(IDbContext ctx, String[] lstSelectedMember, String[] lstUnselectedMember, String transactionID)
        {
            PatientChargesHdDao patientHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientDtDao = new PatientChargesDtDao(ctx);
            string filterExpression = String.Format("TransactionID = {0} AND IsDeleted = 0", transactionID);
            List<PatientChargesDt> lstPatientDt = BusinessLayer.GetPatientChargesDtList(filterExpression, ctx);
            foreach (PatientChargesDt patientDt in lstPatientDt)
            {
                if (lstSelectedMember.Contains(patientDt.ID.ToString()) && patientDt.IsVerified == false)
                {
                    patientDt.IsVerified = true;
                    patientDt.VerifiedBy = AppSession.UserLogin.UserID;
                    patientDt.VerifiedDate = DateTime.Now;
                    patientDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    patientDt.LastUpdatedDate = DateTime.Now;
                    patientDtDao.Update(patientDt);
                }
                if (lstUnselectedMember.Contains(patientDt.ID.ToString()) && patientDt.IsVerified == true)
                {
                    patientDt.IsVerified = false;
                    patientDt.VerifiedBy = null;
                    patientDt.VerifiedDate = new DateTime(1900, 1, 1);
                    patientDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    patientDt.LastUpdatedDate = DateTime.Now;
                    patientDtDao.Update(patientDt);
                }
            }
            PatientChargesHd entityHd = patientHdDao.Get(Convert.ToInt32(transactionID));
            if (lstPatientDt.Where(p => p.IsVerified == false).Count() > 0)
                entityHd.IsVerified = false;
            else
                entityHd.IsVerified = true;
            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
            patientHdDao.Update(entityHd);
            
        }
    }
}