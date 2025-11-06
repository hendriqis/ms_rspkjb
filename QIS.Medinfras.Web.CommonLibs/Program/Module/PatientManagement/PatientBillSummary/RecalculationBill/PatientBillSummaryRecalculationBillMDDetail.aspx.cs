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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryRecalculationBillMDDetail : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                return Constant.MenuCode.Imaging.BILL_SUMMARY_RECALCULATION_BILL;
            return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_RECALCULATION_BILL;
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
            string itemType = "";
            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                itemType = Constant.ItemGroupMaster.RADIOLOGY;
            else
                itemType = Constant.ItemGroupMaster.DIAGNOSTIC;

            List<vPatientChargesDt8> lstService = lst.Where(p => p.GCItemType == itemType).ToList();
            ((TransactionDtServiceViewCtl)ctlService).BindGrid(lstService);

            List<vPatientChargesDt8> lstDrugMS = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ((TransactionDtProductViewCtl)ctlDrugMS).BindGrid(lstDrugMS);

            List<vPatientChargesDt8> lstLogistic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ((TransactionDtProductViewCtl)ctlLogistic).BindGrid(lstLogistic);

            txtTotalPayer.Text = lst.Sum(p => p.PayerAmount).ToString("N");
            txtTotalPatient.Text = lst.Sum(p => p.PatientAmount).ToString("N");
            txtTotal.Text = lst.Sum(p => p.LineAmount).ToString("N");

            divWarningPendingRecalculated.Attributes.Remove("style");
            int countPendingRecalculated = BusinessLayer.GetvPatientChargesHdRowCount(string.Format("RegistrationID = {0} AND GCTransactionStatus IN ('{1}','{2}') AND IsPendingRecalculated = 1", hdnRegistrationID.Value, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL));
            if (countPendingRecalculated < 1)
                divWarningPendingRecalculated.Attributes.Add("style", "display:none");
        }

        protected void cbpRecalculationPatientBill_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(string.Format("RegistrationID = {0} AND GCTransactionStatus = IN ('{1}','{2}') AND IsDeleted = 0", registrationID, Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL));
            BindGrid(lst);
        }
    }
}