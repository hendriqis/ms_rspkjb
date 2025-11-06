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
    public partial class PatientBillSummaryPayReceipt : BasePageTrx
    {
        public string GetMenuCaption() 
        {
            String id = hdnKode.Value;
            if (id == "r")
                return "Cetak Ulang Kwitansi";
            else
                return "Pembatalan Kwitansi";
        }
        
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"].Split('|')[0];
            hdnKode.Value = id;
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: 
                    if(id == "r")
                        return Constant.MenuCode.Inpatient.BILL_SUMMARY_RECEIPT_REPRINT;
                    else
                        return Constant.MenuCode.Inpatient.BILL_SUMMARY_RECEIPT_VOID;
                case Constant.Facility.MEDICAL_CHECKUP:
                    if (id == "r")
                        return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_RECEIPT_REPRINT;
                    else
                        return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_RECEIPT_VOID;
                case Constant.Facility.EMERGENCY:
                    if (id == "r")
                        return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_RECEIPT_REPRINT;
                    else
                        return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_RECEIPT_VOID;
                case Constant.Facility.PHARMACY:
                    if (id == "r")
                        return Constant.MenuCode.Pharmacy.BILL_SUMMARY_RECEIPT_REPRINT;
                    else
                        return Constant.MenuCode.Pharmacy.BILL_SUMMARY_RECEIPT_VOID;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        if (id == "r")
                            return Constant.MenuCode.Laboratory.BILL_SUMMARY_RECEIPT_REPRINT;
                        else
                            return Constant.MenuCode.Laboratory.BILL_SUMMARY_RECEIPT_VOID;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        if (id == "r")
                            return Constant.MenuCode.Imaging.BILL_SUMMARY_RECEIPT_REPRINT;
                        else
                            return Constant.MenuCode.Imaging.BILL_SUMMARY_RECEIPT_VOID;
                    if (id == "r")
                        return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_RECEIPT_REPRINT;
                    else
                        return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_RECEIPT_VOID;
                default: 
                    if(id == "r")
                        return Constant.MenuCode.Outpatient.BILL_SUMMARY_RECEIPT_REPRINT;
                    else
                        return Constant.MenuCode.Outpatient.BILL_SUMMARY_RECEIPT_VOID;
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
            List<vPaymentReceipt> lst = BusinessLayer.GetvPaymentReceiptList(string.Format("RegistrationID = {0} AND IsDeleted = 0", hdnRegistrationID.Value));
            grdView.DataSource = lst;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void EntityToControl(vRegistration entity)
        {
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