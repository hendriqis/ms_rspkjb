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
    public partial class PatientPerBillDetailReprintListDetail : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"].Split('|')[0];
            hdnRequestID.Value = id;
            switch (id)
            {
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL;
                    return Constant.MenuCode.MedicalDiagnostic.PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL;
                default: return Constant.MenuCode.EmergencyCare.PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        protected override void InitializeDataControl()
        {
            string id = Page.Request.QueryString["id"];
            hdnRequestID.Value = id;
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();

            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            
            BindGridDetail();
        }

        private void BindGridDetail()
        {
            List<PatientBill> lst = BusinessLayer.GetPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID));
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

    }
}