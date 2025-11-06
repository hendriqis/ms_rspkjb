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
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RecalculationPatientBillDetail : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            //switch (hdnDepartmentID.Value)
            //{
            //    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.RECALCULATION_PATIENT_BILL;
            //    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.RECALCULATION_PATIENT_BILL;
            //    case Constant.Facility.DIAGNOSTIC:
            //        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
            //            return Constant.MenuCode.Laboratory.RECALCULATION_PATIENT_BILL;
            //        if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
            //            return Constant.MenuCode.Imaging.RECALCULATION_PATIENT_BILL;
            //        return Constant.MenuCode.MedicalDiagnostic.RECALCULATION_PATIENT_BILL;
            //    default: return Constant.MenuCode.Outpatient.RECALCULATION_PATIENT_BILL;
            //} 
            return "";
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnRegistrationID.Value = Page.Request.QueryString["id"];
                vRegistration3 entity = BusinessLayer.GetvRegistration3List(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];

                hdnDepartmentID.Value = entity.DepartmentID;
                ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);

                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(string.Format("RegistrationID = {0} AND GCTransactionStatus NOT IN ('{1}','{2}') AND IsDeleted = 0", registrationID, Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID));
                BindGrid(lst);
            }   
        }

        private void BindGrid(List<vPatientChargesDt8> lst)
        {            
            List<vPatientChargesDt8> lstService = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE).ToList();
            ((TransactionDtServiceViewCtl)ctlService).BindGrid(lstService);

            List<vPatientChargesDt8> lstDrugMS = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ((TransactionDtProductViewCtl)ctlDrugMS).BindGrid(lstDrugMS);

            List<vPatientChargesDt8> lstLogistic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ((TransactionDtProductViewCtl)ctlLogistic).BindGrid(lstLogistic);

            List<vPatientChargesDt8> lstLaboratory = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.LABORATORY).ToList();
            ((TransactionDtServiceViewCtl)ctlLaboratory).BindGrid(lstLaboratory);

            List<vPatientChargesDt8> lstImaging = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY).ToList();
            ((TransactionDtServiceViewCtl)ctlImaging).BindGrid(lstImaging);

            List<vPatientChargesDt8> lstMedicalDiagnostic = lst.Where(p => p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC).ToList();
            ((TransactionDtServiceViewCtl)ctlMedicalDiagnostic).BindGrid(lstMedicalDiagnostic);

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
            List<vPatientChargesDt8> lst = BusinessLayer.GetvPatientChargesDt8List(string.Format("RegistrationID = {0} AND GCTransactionStatus = '{1}' AND IsDeleted = 0", registrationID, Constant.TransactionStatus.OPEN));
            BindGrid(lst);
        }
    }
}