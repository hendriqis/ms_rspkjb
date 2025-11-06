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
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class TransactionVerificationDtCtl : BaseViewPopupCtl
    {
        String[] lstSelectedMember, lstUnselectedMember = null;
        public override void InitializeDataControl(string param)
        {
            Int32 transactionID = Convert.ToInt32(param);
            PatientChargesHd chargesHd = BusinessLayer.GetPatientChargesHd(transactionID);
            if (chargesHd.GCTransactionStatus == Constant.TransactionStatus.PROCESSED)
                btnMPEntryPopupVerified.Style.Add("display", "none");
            hdnTransactionHdID.Value = chargesHd.TransactionID.ToString();
            txtTransactionNo.Text = chargesHd.TransactionNo;
            txtReferenceNo.Text = chargesHd.ReferenceNo;
            txtTransactionDate.Text = chargesHd.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTransactionTime.Text = chargesHd.TransactionTime;

            BindGrid(transactionID, chargesHd.TransactionCode);
        }

        private void BindGrid(int transactionID, string transactionCode)
        {
            List<vPatientChargesDt> lst = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", transactionID));

            //string itemType = "";
            //if (transactionCode == Constant.TransactionCode.IMAGING_CHARGES)
            //    itemType = Constant.ItemGroupMaster.RADIOLOGY;
            //else if (transactionCode == Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES)
            //    itemType = Constant.ItemGroupMaster.DIAGNOSTIC;
            //else
            //    itemType = Constant.ItemGroupMaster.SERVICE;
            List<vPatientChargesDt> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE || p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY || p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            ctlService1.BindGrid(lstService);

            List<vPatientChargesDt> lstDrugMS = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ctlDrugMS1.BindGrid(lstDrugMS);

            List<vPatientChargesDt> lstLogistic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ctlLogistic1.BindGrid(lstLogistic);

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
                ctlService1.VerifyProcess(ctx, lstSelectedMember, lstUnselectedMember, hdnTransactionHdID.Value);
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