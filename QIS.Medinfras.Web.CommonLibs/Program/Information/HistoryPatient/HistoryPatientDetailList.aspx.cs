using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class HistoryPatientDetailList : BasePageTrx
    {
        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override string OnGetMenuCode()
        {
            //switch (hdnModuleID.Value)
            //{
            //    case Constant.Module.INPATIENT: return Constant.MenuCode.Inpatient.CHANGE_PATIENT_TRANSACTION_STATUS;
            //    case Constant.Module.EMERGENCY: return Constant.MenuCode.EmergencyCare.CHANGE_PATIENT_TRANSACTION_STATUS;
            //    case Constant.Module.PHARMACY: return Constant.MenuCode.Pharmacy.CHANGE_PATIENT_TRANSACTION_STATUS;
            //    case Constant.Module.MEDICAL_DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.CHANGE_PATIENT_TRANSACTION_STATUS;
            //    case Constant.Module.LABORATORY: return Constant.MenuCode.Laboratory.CHANGE_PATIENT_TRANSACTION_STATUS;
            //    case Constant.Module.IMAGING: return Constant.MenuCode.Imaging.CHANGE_PATIENT_TRANSACTION_STATUS;
            //    default: return Constant.MenuCode.Outpatient.CHANGE_PATIENT_TRANSACTION_STATUS;
            //}
            return Constant.MenuCode.Imaging.HISTORY_PATIENT_INFORMATION;
        }

        protected override void InitializeDataControl()
        {
            string moduleName = Helper.GetModuleName();
            string moduleID = Helper.GetModuleID(moduleName);
            hdnModuleID.Value = moduleID;

            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnHealthcareServiceUnitID.Value = param[0];
                hdnVisitID.Value = param[1];

                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                        "HealthcareServiceUnitID = {0} AND IsDeleted = 0 AND IsUsingRegistration = 1", hdnHealthcareServiceUnitID.Value)).FirstOrDefault();
                hdnDepartmentID.Value = hsu.DepartmentID;
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                EntityToControl(entity);

                BindGridView();
            }
        }

        private void BindGridView()
        {

            string filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
            {
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1)) AND GCTransactionStatus != '{2}'", hdnVisitID.Value, hdnLinkedRegistrationID.Value, Constant.TransactionStatus.VOID);
            }
            else
            {
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
                filterExpression += string.Format(" AND GCTransactionStatus != '{0}'", Constant.TransactionStatus.VOID);
            }

            SettingParameterDt setvar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI);
            filterExpression += string.Format(" AND HealthcareServiceUnitID = {0} ORDER BY TransactionID DESC", setvar.ParameterValue);

            List<vPatientChargesHDPerParamedicTestOrder> lst = BusinessLayer.GetvPatientChargesHDPerParamedicTestOrderList(filterExpression);
            grdView.DataSource = lst;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void EntityToControl(vConsultVisit2 entity)
        {
            ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<vPatientChargesDtWithImagingResultStatus> lstDetail = BusinessLayer.GetvPatientChargesDtWithImagingResultStatusList(string.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", hdnCollapseID.Value, Constant.TransactionStatus.VOID));
            grdDetail.DataSource = lstDetail;
            grdDetail.DataBind();
        }
    }
}