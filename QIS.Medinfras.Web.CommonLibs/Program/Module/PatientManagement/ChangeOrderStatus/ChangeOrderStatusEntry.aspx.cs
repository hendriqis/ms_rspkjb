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
    public partial class ChangeOrderStatusEntry : BasePageTrx
    {
        protected string laboratoryTransactionCode = "";

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override string OnGetMenuCode()
        {
            switch (hdnModuleID.Value)
            {
                case Constant.Module.OUTPATIENT: return Constant.MenuCode.Outpatient.CHANGE_PATIENT_ORDER_STATUS;
                case Constant.Module.INPATIENT: return Constant.MenuCode.Inpatient.CHANGE_PATIENT_ORDER_STATUS;
                case Constant.Module.EMERGENCY: return Constant.MenuCode.EmergencyCare.CHANGE_PATIENT_ORDER_STATUS;
                case Constant.Module.IMAGING: return Constant.MenuCode.Imaging.CHANGE_PATIENT_ORDER_STATUS;
                case Constant.Module.LABORATORY: return Constant.MenuCode.Laboratory.CHANGE_PATIENT_ORDER_STATUS;
                case Constant.Module.MEDICAL_DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.CHANGE_PATIENT_ORDER_STATUS;
                default: return Constant.MenuCode.Outpatient.CHANGE_PATIENT_ORDER_STATUS;
            }
        }

        protected override void InitializeDataControl()
        {
            string moduleName = Helper.GetModuleName();
            string moduleID = Helper.GetModuleID(moduleName);
            hdnModuleID.Value = moduleID;

            //laboratoryTransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
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
            hdnDepartmentID.Value = entity.DepartmentID;
            //hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            //hdnClassID.Value = entity.ClassID.ToString();
            EntityToControl(entity);
            BindGridView();
            }
        }

        protected override void SetControlProperties()
        {
            String filterExpression = string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vPatientOrderAll WHERE RegistrationID = {0})", hdnRegistrationID.Value);
            List<vHealthcareServiceUnit> lstEntity = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            lstEntity.Insert(0, new vHealthcareServiceUnit() { ServiceUnitName = "", ServiceUnitCode = "" });
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnitPerHealthcare, lstEntity, "ServiceUnitName", "ServiceUnitCode");
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus IN ('{1}')", hdnRegistrationID.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            if (cboServiceUnitPerHealthcare.Value != null && cboServiceUnitPerHealthcare.Value.ToString() != "")
            {
                filterExpression += string.Format(" AND ServiceUnitCode = '{0}'", cboServiceUnitPerHealthcare.Value);
            }
            filterExpression += " ORDER BY OrderDate, OrderTime";
            List<vPatientOrderAll> lstEntity = BusinessLayer.GetvPatientOrderAllList(filterExpression);
            grdView.DataSource = lstEntity;
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
    }
}