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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryPayReceiptPrint : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_RECEIPT_PRINT;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_RECEIPT_PRINT;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_RECEIPT_PRINT;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_RECEIPT_PRINT;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_RECEIPT_PRINT;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_RECEIPT_PRINT;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_RECEIPT_PRINT;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_RECEIPT_PRINT;
            }
        }

        protected override void InitializeDataControl()
        {
            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];

            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
            EntityToControl(entity);

            BindGridDetail();
        }

        private void BindGridDetail()
        {
            List<vPatientPaymentHd> lst = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID = {0} AND PaymentReceiptID IS NULL AND GCTransactionStatus != '{1}' AND GCPaymentType IN ('{2}','{3}')", hdnRegistrationID.Value, Constant.TransactionStatus.VOID, Constant.PaymentType.SETTLEMENT, Constant.PaymentType.CUSTOM));
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void EntityToControl(vRegistration entity)
        {
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnPatientName.Value = entity.PatientName;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }
    }
}