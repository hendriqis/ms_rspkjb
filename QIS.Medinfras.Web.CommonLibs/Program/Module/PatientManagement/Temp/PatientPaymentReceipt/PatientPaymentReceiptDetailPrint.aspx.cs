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
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientPaymentReceiptDetailPrint : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            //switch (hdnDepartmentID.Value)
            //{
            //    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_PAYMENT_RECEIPT_PRINT;
            //    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_PAYMENT_RECEIPT_PRINT;
            //    case Constant.Facility.DIAGNOSTIC:
            //        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
            //            return Constant.MenuCode.Laboratory.PATIENT_PAYMENT_RECEIPT_PRINT;
            //        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
            //            return Constant.MenuCode.Imaging.PATIENT_PAYMENT_RECEIPT_PRINT;
            //        return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAYMENT_RECEIPT_PRINT;
            //    default: return Constant.MenuCode.Outpatient.PATIENT_PAYMENT_RECEIPT_PRINT;
            //}
            return "";
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnRegistrationID.Value = Page.Request.QueryString["id"];
                vRegistration3 entity = BusinessLayer.GetvRegistration3List(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
                EntityToControl(entity);
                BindGridDetail();
            }
        }

        private void BindGridDetail()
        {
            List<vPatientPaymentHd> lst = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID = {0} AND PaymentID NOT IN (SELECT PaymentID FROM PaymentReceipt WHERE IsDeleted = 0) AND GCTransactionStatus != '{1}' AND GCPaymentType = '{2}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID, Constant.PaymentType.SETTLEMENT));
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void EntityToControl(vRegistration3 entity)
        {
            ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
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