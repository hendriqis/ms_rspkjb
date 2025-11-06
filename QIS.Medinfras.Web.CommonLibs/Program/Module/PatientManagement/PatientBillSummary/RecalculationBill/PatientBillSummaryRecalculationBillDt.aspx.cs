using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryRecalculationBillDt : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BILL_SUMMARY_RECALCULATION_BILL;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_RECALCULATION_BILL;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BILL_SUMMARY_RECALCULATION_BILL;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.BILL_SUMMARY_RECALCULATION_BILL;
                case Constant.Facility.DIAGNOSTIC :
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.BILL_SUMMARY_RECALCULATION_BILL;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.BILL_SUMMARY_RECALCULATION_BILL;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.BILL_SUMMARY_RECALCULATION_BILL;                    
                    return Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY_RECALCULATION_BILL;
                default: return Constant.MenuCode.Outpatient.BILL_SUMMARY_RECALCULATION_BILL;
            }
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnLinkedRegistrationID.Value = AppSession.RegisteredPatient.LinkedRegistrationID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            Registration entityReg = BusinessLayer.GetRegistration(Convert.ToInt32(AppSession.RegisteredPatient.RegistrationID));
            hdnRegistrationNo.Value = entityReg.RegistrationNo;
            hdnGCCustomerType.Value = entityReg.GCCustomerType;
                
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);

            String filterExpression = GetFilterExpression() + " AND IsDeleted = 0 AND IsAIOTransaction = 0 AND IsControlAmount = 0";
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(filterExpression);
            BindGrid(lst);
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(RegistrationID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnRegistrationID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);

            filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}')", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            return filterExpression;
        }

        private void BindGrid(List<vPatientChargesDt8> lst)
        {            
            List<vPatientChargesDt8> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE).ToList();
            ctlService.HideCheckBox();
            ctlService.BindGrid(lstService);

            List<vPatientChargesDt8> lstDrugMS = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ctlService.HideCheckBox();
            ctlDrugMS.BindGrid(lstDrugMS);

            List<vPatientChargesDt8> lstLogistic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ctlService.HideCheckBox();
            ctlLogistic.BindGrid(lstLogistic);

            List<vPatientChargesDt8> lstLaboratory = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY).ToList();
            ctlService.HideCheckBox();
            ctlLaboratory.BindGrid(lstLaboratory);

            List<vPatientChargesDt8> lstImaging = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY).ToList();
            ctlService.HideCheckBox();
            ctlImaging.BindGrid(lstImaging);

            List<vPatientChargesDt8> lstMedicalDiagnostic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            ctlMedicalDiagnostic.BindGrid(lstMedicalDiagnostic);

            txtTotalPayer.Text = lst.Sum(p => p.PayerAmount).ToString("N");
            txtTotalPatient.Text = lst.Sum(p => p.PatientAmount).ToString("N");
            txtTotal.Text = lst.Sum(p => p.LineAmount).ToString("N");

            divWarningPendingRecalculated.Attributes.Remove("style");
            string filterExpression = GetFilterExpression() + " AND IsPendingRecalculated = 1";
            int countPendingRecalculated = BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
            if (countPendingRecalculated < 1)
                divWarningPendingRecalculated.Attributes.Add("style", "display:none");
        }

        protected void cbpRecalculationPatientBill_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            String filterExpression = GetFilterExpression() + "AND IsDeleted = 0";
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(filterExpression);
            BindGrid(lst);
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}