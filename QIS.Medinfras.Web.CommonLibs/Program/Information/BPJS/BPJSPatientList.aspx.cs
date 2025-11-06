using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class BPJSPatientList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.IP_DATA_KUNJUNGAN_PASIEN_BPJS;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.ER_DATA_KUNJUNGAN_PASIEN_BPJS;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.PH_DATA_KUNJUNGAN_PASIEN_BPJS;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.LB_DATA_KUNJUNGAN_PASIEN_BPJS;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.IS_DATA_KUNJUNGAN_PASIEN_BPJS;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.DATA_KUNJUNGAN_PASIEN_BPJS;
                    return Constant.MenuCode.MedicalDiagnostic.MD_DATA_KUNJUNGAN_PASIEN_BPJS;
                default: return Constant.MenuCode.Outpatient.OP_DATA_KUNJUNGAN_PASIEN_BPJS;
            }
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }

        private string refreshGridInterval = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<GetServiceUnitUserList> lstServiceUnit = null;
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
                if (Page.Request.QueryString["id"] == "ER")
                {
                    lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.EMERGENCY, "IsUsingRegistration = 1");
                }
                else
                {
                    lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.OUTPATIENT, "IsUsingRegistration = 1");
                }
                lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;
                ((GridHospitalizedPatientRegistrationCtl)grdInpatientReg).InitializeControl();
            }
        }

        public override string GetFilterExpression()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnDepartmentID.Value = Page.Request.QueryString["id"];
            }

            string filterExpression = "";

            //switch (hdnDepartmentID.Value)
            //{
            //    case "ER": filterExpression = string.Format("DepartmentID = '{0}'", Constant.Facility.EMERGENCY); break;
            //    case "OP": filterExpression = string.Format("DepartmentID = '{0}'", Constant.Facility.OUTPATIENT); break;
            //}

            ////Cek Combo Box Service Unit
            //if (cboServiceUnit.Value != null && cboServiceUnit.Value.ToString() != "0")
            //{
            //    filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            //}

            ////Cek Visit Status
            //if (hdnDepartmentID.Value == "OP")
            //{
            //    SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format(
            //        "ParameterCode = '{0}'", Constant.SettingParameter.IS_OUTPATIENT_REGISTRATION_AUTOMATICALLY_CHECKED_IN)).FirstOrDefault();
            //    string setvarStr = setvar.ParameterValue;
            //    if (setvarStr != "1")
            //    {
            //        filterExpression += string.Format(" AND GCRegistrationStatus = '{0}'", Constant.VisitStatus.OPEN);
            //    }
            //    else
            //    {
            //        filterExpression += string.Format(" AND GCRegistrationStatus = '{0}'", Constant.VisitStatus.CHECKED_IN);
            //    }
            //}
            //else
            //{
            //    filterExpression += string.Format(" AND GCRegistrationStatus = '{0}'", Constant.VisitStatus.CHECKED_IN);
            //}

            ////Cek Quick Search
            //if (hdnFilterExpressionQuickSearch.Value != "")
            //{
            //    filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            //}

            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }
    }
}