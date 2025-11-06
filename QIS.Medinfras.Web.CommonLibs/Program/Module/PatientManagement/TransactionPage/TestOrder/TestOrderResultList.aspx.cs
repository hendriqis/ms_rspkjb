using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TestOrderResultList : BasePageTrx
    {
        string menuType = string.Empty;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                    return Constant.MenuCode.Inpatient.FOLLOWUP_TEST_ORDER_RESULT_LIST;
                else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                    return Constant.MenuCode.EmergencyCare.FOLLOWUP_TEST_ORDER_RESULT_LIST;
                else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                    return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_TEST_ORDER_LB_IS_RESULT_LIST;
                else return Constant.MenuCode.Outpatient.FOLLOWUP_TEST_ORDER_RESULT_LIST;
                #endregion
            }
            else if (menuType == "dp")
            {
                #region Data Pemeriksaan Pasien
                if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                {
                    return Constant.MenuCode.Outpatient.DATA_PATIENT_TEST_ORDER_LB_IS_RESULT_LIST;
                }
                else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                {
                    return Constant.MenuCode.EmergencyCare.DATA_PATIENT_TEST_ORDER_LB_IS_RESULT_LIST;
                }
                else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                {
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.TEST_ORDER_RESULT_LIST;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.TEST_ORDER_LB_IS_RESULT_LIST;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.OtherMedicalDiagnostic)
                        return Constant.MenuCode.MedicalDiagnostic.TEST_ORDER_LB_IS_RESULT_LIST;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Nutrition)
                    {
                        if (menuType == "nt")
                        {
                            return Constant.MenuCode.Nutrition.NUTRITION_DIAGNOSTIC_RESULT;
                        }
                    }
                    return Constant.MenuCode.Outpatient.TEST_ORDER_RESULT_LIST;
                }
                else return Constant.MenuCode.Outpatient.DATA_PATIENT_TEST_ORDER_LB_IS_RESULT_LIST;
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                    return Constant.MenuCode.Inpatient.TEST_ORDER_RESULT_LIST;
                else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                    return Constant.MenuCode.EmergencyCare.TEST_ORDER_RESULT_LIST;
                else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC) {
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.TEST_ORDER_RESULT_LIST;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.TEST_ORDER_RESULT_LIST;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.OtherMedicalDiagnostic)
                        return Constant.MenuCode.MedicalDiagnostic.TEST_ORDER_LB_IS_RESULT_LIST;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Nutrition)
                    {
                        if (menuType == "nt")
                        {
                                return Constant.MenuCode.Nutrition.NUTRITION_DIAGNOSTIC_RESULT;
                        }
                    }
                    return Constant.MenuCode.Outpatient.TEST_ORDER_RESULT_LIST;
                }
                else if (hdnDepartmentID.Value == Constant.Module.RADIOTHERAPHY)
                    return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_TEST_ORDER_RESULT_LIST;
                else if (hdnDepartmentID.Value == Constant.Facility.PHARMACY) {
                    if (menuType == "cp")
                    {
                        return Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_TEST_ORDER_RESULTS;
                    }
                    return Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_TEST_ORDER_RESULTS;
                }
                else if (hdnDepartmentID.Value == Constant.Facility.LABORATORY)
                    return Constant.MenuCode.Laboratory.TEST_ORDER_LB_IS_RESULT_LIST;
                else return Constant.MenuCode.Outpatient.TEST_ORDER_RESULT_LIST;
                #endregion
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void SetControlProperties()
        {
            String filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID IN ('{2}','{3}') AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, hdnLaboratoryServiceUnitID.Value, hdnImagingServiceUnitID.Value);
            List<vHealthcareServiceUnitCustom> lstEntity = BusinessLayer.GetvHealthcareServiceUnitCustomList(filterExpression);
            lstEntity.Insert(0, new vHealthcareServiceUnitCustom() { ServiceUnitName = "", ServiceUnitCode = "" });
            Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnitPerHealthcare, lstEntity, "ServiceUnitName", "ServiceUnitCode");
            cboServiceUnitPerHealthcare.SelectedIndex = 0;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnDepartmentID.Value = param[0];
                if (param.Length > 1)
                {
                    menuType = param[1];
                }
            }
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}','{2}') AND HealthcareID = '{3}'", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.LB_BRIDGING_LIS, AppSession.UserLogin.HealthcareID));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnIsBridgingWithLIS.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_BRIDGING_LIS).ParameterValue;


            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnGCRegistrationStatus.Value = AppSession.RegisteredPatient.GCRegistrationStatus;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnLinkedRegistrationID.Value = AppSession.RegisteredPatient.LinkedRegistrationID.ToString();
            hdnClassID.Value = AppSession.RegisteredPatient.ClassID.ToString();

            txtFromDate.Text = DateTime.Today.AddDays(-7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            //hdnListVisitID.Value = hdnVisitID.Value;

            //string filterCVL = string.Format("RegistrationID = {0} AND GCVisitStatus != '{1}'", hdnLinkedRegistrationID.Value, Constant.VisitStatus.CANCELLED);
            //List<ConsultVisit> lstCVL = BusinessLayer.GetConsultVisitList(filterCVL);
            //foreach (ConsultVisit cv in lstCVL)
            //{
            //    hdnListVisitID.Value += "," + cv.VisitID.ToString();
            //}

            //BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND HealthcareServiceUnitID IN ('{1}','{2}')", hdnVisitID.Value, hdnLaboratoryServiceUnitID.Value, hdnImagingServiceUnitID.Value);
            if (!chkIsIgnoreDate.Checked)
            {
                filterExpression += string.Format(" AND TransactionDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtFromDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }
            if (cboServiceUnitPerHealthcare.Value != null && cboServiceUnitPerHealthcare.Value.ToString() != "")
            {
                filterExpression += string.Format(" AND ServiceUnitCode = '{0}'", cboServiceUnitPerHealthcare.Value);
            }
            //else
            //{
            //    filterExpression += string.Format("and ServiceUnitCode in ('{0}','{1}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM);
            //}

            filterExpression += " ORDER BY TransactionDate DESC, TransactionID DESC";

            List<vPatientChargesTestOrder> lstEntity = BusinessLayer.GetvPatientChargesTestOrderList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }


        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}