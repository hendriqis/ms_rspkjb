using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class RecalculationPatientBillDetail : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            //return Constant.MenuCode.Laboratory.RECALCULATION_PATIENT_BILL;
            return "";
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnRegistrationID.Value = Page.Request.QueryString["id"];
                vRegistration3 entity = BusinessLayer.GetvRegistration3List(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
                hdnDepartmentID.Value = entity.DepartmentID;
                ctlPatientBanner.InitializePatientBanner(entity);
                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(string.Format("RegistrationID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}') AND IsDeleted = 0", registrationID, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID));
                BindGrid(lst);
            }   
        }

        private void BindGrid(List<vPatientChargesDt8> lst)
        {
            List<vPatientChargesDt8> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY).ToList();
            ctlService.BindGrid(lstService);

            txtTotalPayer.Text = lst.Sum(p => p.PayerAmount).ToString("N");
            txtTotalPatient.Text = lst.Sum(p => p.PatientAmount).ToString("N");
            txtTotal.Text = lst.Sum(p => p.LineAmount).ToString("N");

            divWarningPendingRecalculated.Attributes.Remove("style");
            int countPendingRecalculated = BusinessLayer.GetvPatientChargesHdRowCount(string.Format("RegistrationID = {0} AND GCTransactionStatus = '{1}' AND IsPendingRecalculated = 1", hdnRegistrationID.Value, Constant.TransactionStatus.OPEN));
            if (countPendingRecalculated < 1)
                divWarningPendingRecalculated.Attributes.Add("style", "display:none");
        }

        protected void cbpRecalculationPatientBill_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(string.Format("RegistrationID = {0} AND GCTransactionStatus = '{1}' AND IsDeleted = 0", registrationID, Constant.TransactionStatus.OPEN));
            BindGrid(lst);
        }
    }
}