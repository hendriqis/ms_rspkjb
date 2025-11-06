using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class DraftAppointmentInfoCtl : BasePatientManagementDraftAppointmentPage
    {

        List<vDraftPatientChargesDt> lstDraftChargesDt = new List<vDraftPatientChargesDt>();
        List<vDraftTestOrderDt> lstDraftTestOrderDt = new List<vDraftTestOrderDt>();
        List<vDraftPrescriptionOrderDt> lstDraftPrescriptionOrderDt = new List<vDraftPrescriptionOrderDt>();

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
           
                IsAdd = false;
                hdnAppointmentCtlID.Value = param;
               // vRegistration entity = BusinessLayer.GetvRegistrationList(String.Format("AppointmentID = {0} AND GCRegistrationStatus != '{1}'",hdnAppointmentCtlID.Value, Constant.VisitStatus.CANCELLED))[0];
               // hdnRegistrationID.Value = Convert.ToString(entity.RegistrationID);
               // hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
               /// hdnHealthcareServiceUnitID.Value = Convert.ToString(entity.HealthcareServiceUnitID);
                List<SettingParameterDt> lstSetVar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}', '{1}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM));
                hdnHealthcareServiceUnitIDIS.Value = lstSetVar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
                hdnHealthcareServiceUnitIDLB.Value = lstSetVar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;

                string filterExpressionDraftCharges = string.Format("AppointmentID = {0} AND GCTransactionStatus != '{1}' AND GCTransactionDetailStatus != '{1}' AND IsDeleted = 0", hdnAppointmentCtlID.Value, Constant.TransactionStatus.VOID);
                lstDraftChargesDt = BusinessLayer.GetvDraftPatientChargesDtList(filterExpressionDraftCharges);

                string filterExpressionDraftTestOrder = String.Format("AppointmentID = {0} AND GCTransactionStatus != '{1}' AND GCDraftTestOrderStatus != '{2}' AND IsDeleted = 0", hdnAppointmentCtlID.Value, Constant.TransactionStatus.VOID, Constant.TestOrderStatus.CANCELLED);
                lstDraftTestOrderDt = BusinessLayer.GetvDraftTestOrderDtList(filterExpressionDraftTestOrder);

                string filterExpressionDraftPrescriptionOrder = String.Format("AppointmentID = {0} AND GCTransactionStatus != '{1}' AND GCDraftPrescriptionOrderStatus != '{2}' AND IsDeleted = 0", hdnAppointmentCtlID.Value, Constant.TransactionStatus.VOID, Constant.TestOrderStatus.CANCELLED);
                lstDraftPrescriptionOrderDt = BusinessLayer.GetvDraftPrescriptionOrderDtList(filterExpressionDraftPrescriptionOrder);
               
                BindGrid();
            }
        }

        protected override void BindGrid()
        {
            List<vDraftPatientChargesDt> lstService = lstDraftChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.SERVICE || p.GCItemType == Constant.ItemGroupMaster.LABORATORY ||  p.GCItemType == Constant.ItemGroupMaster.RADIOLOGY || p.GCItemType == Constant.ItemGroupMaster.DIAGNOSTIC || p.GCItemType == Constant.ItemGroupMaster.MEDICAL_CHECKUP).ToList();
            ((DraftTransactionDtServiceViewCtl)ctlService).BindGrid(lstService);

            List<vDraftPatientChargesDt> lstDrugMS = lstDraftChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.DRUGS || p.GCItemType == Constant.ItemGroupMaster.SUPPLIES).ToList();
            ((DraftTransactionDtProductViewCtl)ctlDrugMS).BindGrid(lstDrugMS);

            List<vDraftPatientChargesDt> lstLogistic = lstDraftChargesDt.Where(p => p.GCItemType == Constant.ItemGroupMaster.LOGISTIC).ToList();
            ((DraftTransactionDtProductViewCtl)ctlLogistic).BindGrid(lstLogistic);

            List<vDraftTestOrderDt> lstLaboratory = lstDraftTestOrderDt.Where(p => p.HealthcareServiceUnitID == Convert.ToInt32(hdnHealthcareServiceUnitIDLB.Value)).ToList();
            ((DraftTestOrderDtServiceViewCtl)ctlLaboratory).BindGrid(lstLaboratory);

            List<vDraftTestOrderDt> lstImaging = lstDraftTestOrderDt.Where(p => p.HealthcareServiceUnitID == Convert.ToInt32(hdnHealthcareServiceUnitIDIS.Value)).ToList();
            ((DraftTestOrderDtServiceViewCtl)ctlImaging).BindGrid(lstImaging);

            List<vDraftTestOrderDt> lstMedicalDiagnostic = lstDraftTestOrderDt.Where(p => p.HealthcareServiceUnitID != Convert.ToInt32(hdnHealthcareServiceUnitIDIS.Value) && p.HealthcareServiceUnitID != Convert.ToInt32(hdnHealthcareServiceUnitIDLB.Value)).ToList();
            ((DraftTestOrderDtServiceViewCtl)ctlMedicalDiagnostic).BindGrid(lstMedicalDiagnostic);

            ((DraftPrescriptionOrderDtServiceViewCtl)ctlPharmacy).BindGrid(lstDraftPrescriptionOrderDt);

            txtTotalPayer.Text = lstDraftChargesDt.Sum(p => p.PayerAmount).ToString("N");
            txtTotalPatient.Text = lstDraftChargesDt.Sum(p => p.PatientAmount).ToString("N");
            txtTotal.Text = lstDraftChargesDt.Sum(p => p.LineAmount).ToString("N");
        }

        protected void cbpRecalculationPatientBillProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            if (hdnParamCharges.Value != "")
            {
                string[] listParam = hdnParamCharges.Value.Split('|');
                int[] listParamTemp = Array.ConvertAll(listParam, int.Parse);
//                OnProcessRecalculation(registrationID, chkIsIncludeVariableTariff.Checked, chkIsResetItemTariff.Checked, listParamTemp);
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
            return false; ///OnSaveRecord(ref errMessage, ref retval, registrationID, hdnParamCharges.Value, hdnParamTestOrder.Value, hdnParamPrescriptionOrder.Value, hdnHealthcareServiceUnitID.Value);
        }
    }
}