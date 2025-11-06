using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PaymentReceiptReprintDetail : BasePageTrx
    {
        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"].Split('|')[0];
            hdnRequestID.Value = id;
            switch (id)
            {
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PAYMENT_RECEIPT_REPRINT_DETAIL;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PAYMENT_RECEIPT_REPRINT_DETAIL;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.PAYMENT_RECEIPT_REPRINT_DETAIL;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.PAYMENT_RECEIPT_REPRINT_DETAIL;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.PAYMENT_RECEIPT_REPRINT_DETAIL;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.PAYMENT_RECEIPT_REPRINT_DETAIL;
                    return Constant.MenuCode.MedicalDiagnostic.PAYMENT_RECEIPT_REPRINT_DETAIL;
                default: return Constant.MenuCode.EmergencyCare.PAYMENT_RECEIPT_REPRINT_DETAIL;
            }
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnRegistrationID.Value = param[1];

                vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value)).FirstOrDefault();
                hdnLinkedRegistrationID.Value = entityReg.LinkedRegistrationID.ToString();
                hdnHealthcareServiceUnitID.Value = entityReg.HealthcareServiceUnitID.ToString();
                hdnDepartmentID.Value = entityReg.DepartmentID;

                BindGridReceiptDetail();
            }
        }

        private void BindGridReceiptDetail()
        {
            List<vPaymentReceipt> lst_receipt = BusinessLayer.GetvPaymentReceiptList(string.Format("RegistrationID = {0} AND IsDeleted = 0", hdnRegistrationID.Value));
            lvwView_receipt.DataSource = lst_receipt;
            lvwView_receipt.DataBind();
        }

        protected void cbpViewReceipt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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