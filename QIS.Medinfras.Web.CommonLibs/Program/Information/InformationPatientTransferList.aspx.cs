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
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InformationPatientTransferList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                {
                    return Constant.MenuCode.EmergencyCare.INFORMATION_PATIENT_TRANSFER;
                }
                else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                {
                    return Constant.MenuCode.MedicalDiagnostic.INFORMATION_PATIENT_TRANSFER;
                }
                else if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                {
                    return Constant.MenuCode.Outpatient.INFORMATION_PATIENT_TRANSFER;
                }
                else
                {
                    return Constant.MenuCode.EMR.INFORMATION_PATIENT_TRANSFER;
                }
            }
            return "";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                #region Menu ID
                if (Page.Request.QueryString["id"] == Constant.Facility.EMERGENCY)
                {
                    List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.EMERGENCY, "IsUsingRegistration = 1");
                    lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboServiceUnit.SelectedIndex = 0;

                    List<GetServiceUnitUserList> lstServiceUnit2 = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.EMERGENCY, "IsUsingRegistration = 1");
                    lstServiceUnit2.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit2, lstServiceUnit2, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboServiceUnit2.SelectedIndex = 0;
                }
                else if (Page.Request.QueryString["id"] == Constant.Facility.DIAGNOSTIC)
                {
                    List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.DIAGNOSTIC, string.Format("IsUsingRegistration = 1 AND HealthcareServiceUnitID NOT IN (SELECT ParameterValue FROM SettingParameterDt WHERE ParameterCode IN ('{0}','{1}'))", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM));
                    lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboServiceUnit.SelectedIndex = 0;

                    List<GetServiceUnitUserList> lstServiceUnit2 = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.DIAGNOSTIC, string.Format("IsUsingRegistration = 1 AND HealthcareServiceUnitID NOT IN (SELECT ParameterValue FROM SettingParameterDt WHERE ParameterCode IN ('{0}','{1}'))", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM));
                    lstServiceUnit2.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit2, lstServiceUnit2, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboServiceUnit2.SelectedIndex = 0;
                }
                else if (Page.Request.QueryString["id"] == Constant.Facility.OUTPATIENT)
                {
                    List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.OUTPATIENT, "IsUsingRegistration = 1");
                    lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboServiceUnit.SelectedIndex = 0;

                    List<GetServiceUnitUserList> lstServiceUnit2 = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.OUTPATIENT, "IsUsingRegistration = 1");
                    lstServiceUnit2.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit2, lstServiceUnit2, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboServiceUnit2.SelectedIndex = 0;
                }
                else
                {
                    List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID IN ('{0}', '{1}', '{2}') AND IsDeleted = 0 ORDER BY ServiceUnitCode, ServiceUnitName", Constant.Facility.EMERGENCY, Constant.Facility.DIAGNOSTIC, Constant.Facility.OUTPATIENT));
                    lstServiceUnit.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                    Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboServiceUnit.SelectedIndex = 0;

                    List<vHealthcareServiceUnit> lstServiceUnit2 = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID IN ('{0}', '{1}', '{2}') AND IsDeleted = 0 ORDER BY ServiceUnitCode, ServiceUnitName", Constant.Facility.EMERGENCY, Constant.Facility.DIAGNOSTIC, Constant.Facility.OUTPATIENT));
                    lstServiceUnit2.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                    Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit2, lstServiceUnit2, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboServiceUnit2.SelectedIndex = 0;
                }
                #endregion

                Helper.SetControlEntrySetting(txtFromRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");
                Helper.SetControlEntrySetting(txtToRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");

                txtFromRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtToRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                Helper.SetControlEntrySetting(txtFromRegistrationDate2, new ControlEntrySetting(true, true, true), "mpPatientList");
                Helper.SetControlEntrySetting(txtToRegistrationDate2, new ControlEntrySetting(true, true, true), "mpPatientList");

                txtFromRegistrationDate2.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtToRegistrationDate2.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                ((GridPatientRegistrationTransferCtl)grdInpatientReg).InitializeControl();

                BindingPatientDischargeTransferInpatient();
            }
        }

        public override string GetFilterExpression()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnDepartmentID.Value = Page.Request.QueryString["id"];
            }

            string filterExpression = "";

            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.EMERGENCY: filterExpression = string.Format("DepartmentID = '{0}'", Constant.Facility.EMERGENCY); break;
                case Constant.Facility.DIAGNOSTIC: filterExpression = string.Format("DepartmentID = '{0}'", Constant.Facility.DIAGNOSTIC); break;
                case Constant.Facility.OUTPATIENT: filterExpression = string.Format("DepartmentID = '{0}'", Constant.Facility.OUTPATIENT); break;
                default: filterExpression = string.Format("DepartmentID IN ('{0}', '{1}', '{2}')", Constant.Facility.EMERGENCY, Constant.Facility.DIAGNOSTIC, Constant.Facility.OUTPATIENT); break;
            }

            //Cek Combo Box Service Unit
            if (cboServiceUnit.Value != null && cboServiceUnit.Value.ToString() != "0")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            }

            //Cek Quick Search
            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            filterExpression += string.Format(" AND (RegistrationDate BETWEEN '{0}' AND '{1}')", Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            return filterExpression;
        }

        protected void cbpViewPatientDischargeTransferInpatient_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindingPatientDischargeTransferInpatient();
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindingPatientDischargeTransferInpatient()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnDepartmentID.Value = Page.Request.QueryString["id"];
            }

            string filterExpression2 = "";

            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.EMERGENCY: filterExpression2 = string.Format("DepartmentID = '{0}'", Constant.Facility.EMERGENCY); break;
                case Constant.Facility.DIAGNOSTIC: filterExpression2 = string.Format("DepartmentID = '{0}'", Constant.Facility.DIAGNOSTIC); break;
                case Constant.Facility.OUTPATIENT: filterExpression2 = string.Format("DepartmentID = '{0}'", Constant.Facility.OUTPATIENT); break;
                default: filterExpression2 = string.Format("DepartmentID IN ('{0}', '{1}', '{2}')", Constant.Facility.EMERGENCY, Constant.Facility.DIAGNOSTIC, Constant.Facility.OUTPATIENT); break;
            }

            //Cek Combo Box Service Unit
            if (hdnFilterCboServiceUnit2.Value != null && hdnFilterCboServiceUnit2.Value != "" && hdnFilterCboServiceUnit2.Value != "0")
            {
                filterExpression2 += string.Format(" AND HealthcareServiceUnitID = {0}", hdnFilterCboServiceUnit2.Value);
            }

            //Cek Quick Search
            if (hdnFilterExpressionQuickSearch2.Value != "")
            {
                filterExpression2 += string.Format(" AND {0}", hdnFilterExpressionQuickSearch2.Value);
            }

            filterExpression2 += string.Format(" AND RegistrationDate BETWEEN '{0}' AND '{1}' AND GCDischargeMethod = '{2}' AND GCRegistrationStatus != '{3}' AND GCVisitStatus != '{4}' AND LinkedToRegistrationID IS NULL ORDER BY PhysicianDischargedDate",
                                                 Helper.GetDatePickerValue(txtFromRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate2).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.DischargeMethod.DISCHARGED_TO_WARD,
                                                 Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CANCELLED);
            List<vPatientVisitInformation> lstPatientDischargeTransferInpatient = BusinessLayer.GetvPatientVisitInformationList(filterExpression2);
            lvwPatientDischargeTransferInpatient.DataSource = lstPatientDischargeTransferInpatient;
            lvwPatientDischargeTransferInpatient.DataBind();
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