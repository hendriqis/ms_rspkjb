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
using QIS.Medinfras.Web.CommonLibs.Program;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class TransactionVerificationDtLaboratoryCtl : BaseViewPopupCtl
    {
        String[] lstSelectedMember, lstUnselectedMember = null;
        public override void InitializeDataControl(string param)
        {
            Int32 transactionID = Convert.ToInt32(param);
            PatientChargesHd chargesHd = BusinessLayer.GetPatientChargesHd(transactionID);
            if (chargesHd.GCTransactionStatus == Constant.TransactionStatus.PROCESSED)
                btnMPEntryPopupVerified.Style.Add("display", "none");
            hdnTransactionHdID.Value = param;
            txtTransactionNo.Text = chargesHd.TransactionNo;
            txtReferenceNo.Text = chargesHd.ReferenceNo;
            txtTransactionDate.Text = chargesHd.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = chargesHd.TransactionTime;

            BindGrid(transactionID);
        }

        private void BindGrid(int transactionID)
        {
            List<vPatientChargesDt> lst = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", transactionID));
            List<vPatientChargesDt> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY).ToList();
            ((TransactionVerificationServiceCtl)ctlService).BindGrid(lstService);

            txtTotalPayer.Text = lst.Sum(p => p.PayerAmount).ToString("N");
            txtTotalPatient.Text = lst.Sum(p => p.PatientAmount).ToString("N");
            txtTotal.Text = lst.Sum(p => p.LineAmount).ToString("N");
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

        private bool OnProcessRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            lstSelectedMember = hdnSelectedMember.Value.Substring(0).Split(',');
            lstUnselectedMember = hdnUnselectedMember.Value.Substring(0).Split(',');
            try
            {
                ctlService.VerifyProcess(ctx, lstSelectedMember, lstUnselectedMember, hdnTransactionHdID.Value);
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
    }
}