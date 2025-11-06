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
    public partial class SummaryRecalculationPatientBillDetail : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.BILL_SUMMARY_RECALCULATION_BILL;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected override void InitializeDataControl()
        {
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];

            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];

            hdnDepartmentID.Value = entity.DepartmentID;

            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(string.Format("RegistrationID = {0} AND GCTransactionStatus IN ('{1}','{2}') AND IsDeleted = 0", registrationID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL));
            BindGrid(lst);
        }

        private void BindGrid(List<vPatientChargesDt8> lst)
        {
            List<vPatientChargesDt8> lstLaboratory = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY).ToList();
            ctlLaboratory.BindGrid(lstLaboratory);

            List<vPatientChargesDt8> lstImaging = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY).ToList();
            ctlImaging.BindGrid(lstImaging);

            List<vPatientChargesDt8> lstMedicalDiagnostic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            ctlMedicalDiagnostic.BindGrid(lstMedicalDiagnostic);

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
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(string.Format("RegistrationID = {0} AND GCTransactionStatus IN ('{1}','{2}') AND IsDeleted = 0", registrationID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL));
            BindGrid(lst);
        }
    }
}