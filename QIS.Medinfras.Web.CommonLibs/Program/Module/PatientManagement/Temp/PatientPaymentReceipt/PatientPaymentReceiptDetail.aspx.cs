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
    public partial class PatientPaymentReceiptDetail : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            //string id = Page.Request.QueryString["id"].Split('|')[0];
            //hdnKode.Value = id;
            //switch (hdnDepartmentID.Value)
            //{
            //    case Constant.Facility.INPATIENT: 
            //        if(id == "r")
            //            return Constant.MenuCode.Inpatient.PATIENT_PAYMENT_RECEIPT_REPRINT;
            //        else
            //            return Constant.MenuCode.Inpatient.VOID_PATIENT_PAYMENT_RECEIPT;
            //    case Constant.Facility.EMERGENCY:
            //        if (id == "r")
            //            return Constant.MenuCode.EmergencyCare.PATIENT_PAYMENT_RECEIPT_REPRINT;
            //        else
            //            return Constant.MenuCode.EmergencyCare.VOID_PATIENT_PAYMENT_RECEIPT;
            //    case Constant.Facility.DIAGNOSTIC:
            //        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
            //            if (id == "r")
            //                return Constant.MenuCode.Laboratory.PATIENT_PAYMENT_RECEIPT_REPRINT;
            //            else
            //                return Constant.MenuCode.Laboratory.VOID_PATIENT_PAYMENT_RECEIPT;
            //        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
            //            if (id == "r")
            //                return Constant.MenuCode.Imaging.PATIENT_PAYMENT_RECEIPT_REPRINT;
            //            else
            //                return Constant.MenuCode.Imaging.VOID_PATIENT_PAYMENT_RECEIPT;
            //        if (id == "r")
            //            return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAYMENT_RECEIPT_REPRINT;
            //        else
            //            return Constant.MenuCode.MedicalDiagnostic.VOID_PATIENT_PAYMENT_RECEIPT;
            //    default: 
            //        if(id == "r")
            //            return Constant.MenuCode.Outpatient.PATIENT_PAYMENT_RECEIPT_REPRINT;
            //        else
            //            return Constant.MenuCode.Outpatient.VOID_PATIENT_PAYMENT_RECEIPT;
            //}
            return "";
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnRegistrationID.Value = Page.Request.QueryString["id"].Split('|')[1];
                vRegistration3 entity = BusinessLayer.GetvRegistration3List(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];
                EntityToControl(entity);
                BindGridDetail();
            }
        }

        private void BindGridDetail()
        {
            List<vPaymentReceipt> lst = BusinessLayer.GetvPaymentReceiptList(string.Format("RegistrationID = {0} AND PaymentReceiptID IN (SELECT PaymentReceiptID FROM PatientPaymentHd WHERE RegistrationID = {1} AND PaymentReceiptID IS NOT NULL AND GCTransactionStatus != '{2}' AND GCPaymentType = '{3}')", 
                hdnRegistrationID.Value, hdnRegistrationID.Value, Constant.TransactionStatus.VOID, Constant.PaymentType.SETTLEMENT));
            grdView.DataSource = lst;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void EntityToControl(vRegistration3 entity)
        {
            ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            PaymentReceiptDao entityDao = new PaymentReceiptDao();

            if (type == "reprint")
            {
                try
                {
                    PaymentReceipt entity = BusinessLayer.GetPaymentReceipt(Convert.ToInt32(hdnPaymentReceiptID.Value));
                    entity.PrintNumber += 1;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                }
            }
            return result;
        }
    }
}