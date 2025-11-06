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
    public partial class InformationPatientPayment : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PAYMENT_INFORMATION;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PAYMENT_INFORMATION;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PAYMENT_INFORMATION;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.PAYMENT_INFORMATION;
                case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.PAYMENT_INFORMATION;
                case Constant.Facility.LABORATORY: return Constant.MenuCode.Laboratory.PAYMENT_INFORMATION;
                case Constant.Facility.IMAGING: return Constant.MenuCode.Imaging.PAYMENT_INFORMATION;
                default: return Constant.MenuCode.Outpatient.PAYMENT_INFORMATION;
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
            string[] param = Page.Request.QueryString["id"].Split('|');
            hdnHealthcareServiceUnitID.Value = param[0];
            hdnVisitID.Value = param[1];

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[1]))[0];

            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value))[0];

            List<SettingParameterDt> lstSetvar = BusinessLayer.GetSettingParameterDtList(string.Format(
                "ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM));

            hdnIS.Value = lstSetvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            hdnLB.Value = lstSetvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;

            EntityToControl(entity);

            BindGridDetail();
        }

        private void BindGridDetail()
        {
            List<vPatientPaymentHd> lst = BusinessLayer.GetvPatientPaymentHdList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value));
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void EntityToControl(vRegistration entity)
        {
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();

            if (hdnHealthcareServiceUnitID.Value == hdnIS.Value)
            {
                hdnDepartmentID.Value = Constant.Facility.IMAGING;
            }
            else if (hdnHealthcareServiceUnitID.Value == hdnLB.Value)
            {
                hdnDepartmentID.Value = Constant.Facility.LABORATORY;
            }
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

    }
}