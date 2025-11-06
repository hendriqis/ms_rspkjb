using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientPaymentReceiptCombine : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_RECEIPT_PRINT_COMBINE;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_RECEIPT_PRINT_COMBINE;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_RECEIPT_PRINT_COMBINE;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_RECEIPT_PRINT_COMBINE;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_RECEIPT_PRINT_COMBINE;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_RECEIPT_PRINT_COMBINE;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_RECEIPT_PRINT_COMBINE;
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_RECEIPT_PRINT_COMBINE;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_RECEIPT_PRINT_COMBINE;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnRegistrationID.Value = hdnRegistrationIDCurrent.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            vRegistration1 entity = BusinessLayer.GetvRegistration1List(string.Format("RegistrationID = {0}", hdnRegistrationID.Value)).FirstOrDefault();
            hdnMRN.Value = entity.MRN.ToString();
            hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
            txtRegNoFilter.Text = entity.RegistrationNo;
            EntityToControl(entity);
            BindGridPaymentDetail();
            BindGridReceiptDetail();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void EntityToControl(vRegistration1 entity)
        {
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnPatientName.Value = entity.PatientName;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
        }

        private void BindGridPaymentDetail()
        {
            string filter = string.Format("MRN = {0} AND PaymentReceiptID IS NULL AND GCTransactionStatus != '{1}'", hdnMRN.Value, Constant.TransactionStatus.VOID);

            string IsARAllowPaymentReceipt = "1";
            SettingParameterDt setvarDt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_AR_IS_ALLOW_PAYMENT_RECEIPT);
            IsARAllowPaymentReceipt = setvarDt.ParameterValue;

            if (IsARAllowPaymentReceipt == "0")
            {
                filter += string.Format(" AND GCPaymentType NOT IN ('{0}','{1}')", Constant.PaymentType.AR_PAYER, Constant.PaymentType.AR_PATIENT);
            }

            List<vPatientPaymentHd> lst_payment = BusinessLayer.GetvPatientPaymentHdList(filter);
            lvwView_payment.DataSource = lst_payment;
            lvwView_payment.DataBind();
        }

        private void BindGridReceiptDetail()
        {
            string filterExp = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            if (String.IsNullOrEmpty(txtRegNoFilter.Text))
            {
                filterExp = string.Format("MRN = '{0}'" , hdnMRN.Value);
            }
            filterExp += string.Format(" ORDER BY RegistrationID");

            List<vPaymentReceipt> lst_receipt = BusinessLayer.GetvPaymentReceiptList(filterExp);
            lvwView_receipt.DataSource = lst_receipt;
            lvwView_receipt.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridPaymentDetail();
        }

        protected void cbpView2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridReceiptDetail();
        }

        protected void cbpViewPrint_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            string errMessage = "";
            string result = "";

            if (param[0] == "copyKwitansi")
            {
                result = printCopyKwitansi(ref errMessage, "copyKwitansi");
            }
            else if (param[0] == "copyTransaksi")
            {
                result = printCopyKwitansi(ref errMessage, "copyTransaksi");
            }
            else if (param[0] == "halodocreceipt")
            {
                result = printHalodocReceipt(ref errMessage, "halodocreceipt");
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        private string printCopyKwitansi(ref string errMessage, string type)
        {
            string result = "";
            try
            {
                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;
                string id = hdnPrintCopy.Value;
                if (type == "copyKwitansi")
                {
                    result = ZebraPrinting.PrintCopyKwitansi(id, "");
                }
                else if (type == "copyTransaksi")
                {
                    result = ZebraPrinting.PrintCopyBuktiTransaksi(id, "");
                }
            }
            catch (Exception ex)
            {
                result = "";
                errMessage = ex.Message;
            }
            return result;
        }

        private string printHalodocReceipt(ref string errMessage, string type)
        {
            string result = "";
            try
            {
                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;
                string id = hdnPrintCopy.Value;
                result = ZebraPrinting.PrintKwitansiHaloDoc(id);
            }
            catch (Exception ex)
            {
                result = "";
                errMessage = ex.Message;
            }
            return result;
        }
    }
}