using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class InfoRegistrationCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            string departmentID = Page.Request.QueryString["id"];

            if (departmentID == Constant.Facility.EMERGENCY)
            {
                int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format(
                                            "DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}",
                                            Constant.Facility.EMERGENCY, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));

                string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsUsingRegistration = 1 AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.EMERGENCY);
                if (serviceUnitUserCount > 0)
                {
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2})", Constant.Facility.EMERGENCY, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
                }

                List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboInfoRegistrationServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboInfoRegistrationServiceUnit.SelectedIndex = 0;
            }
            else if (departmentID == Constant.Facility.OUTPATIENT)
            {
                int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format(
                                            "DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}",
                                            Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));

                string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT);
                if (serviceUnitUserCount > 0)
                {
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2})", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
                }

                List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboInfoRegistrationServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboInfoRegistrationServiceUnit.SelectedIndex = 0;
            }
            else if (departmentID == Constant.Facility.INPATIENT)
            {
                int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format(
                                            "DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}",
                                            Constant.Facility.INPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));

                string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.INPATIENT);
                if (serviceUnitUserCount > 0)
                {
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2})", Constant.Facility.INPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
                }

                List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboInfoRegistrationServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboInfoRegistrationServiceUnit.SelectedIndex = 0;
            }
            else if (departmentID == Constant.Facility.DIAGNOSTIC)
            {
                int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format(
                        "DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND HealthcareServiceUnitID NOT IN ({3},{4}) AND IsLaboratoryUnit = 0",
                        Constant.Facility.DIAGNOSTIC, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID));

                string filterExpression = string.Format(
                        "HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ({2},{3}) AND IsLaboratoryUnit = 0 AND IsUsingRegistration = 1",
                        AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                if (serviceUnitUserCount > 0)
                {
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}) AND HealthcareServiceUnitID NOT IN ({3},{4}) AND IsLaboratoryUnit = 0", Constant.Facility.DIAGNOSTIC, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                }

                List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboInfoRegistrationServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboInfoRegistrationServiceUnit.SelectedIndex = 0;
            }
            else if (departmentID == Constant.Facility.IMAGING)
            {
                int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format(
                        "DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND HealthcareServiceUnitID IN ({3})",
                        Constant.Facility.DIAGNOSTIC, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, AppSession.MedicalDiagnostic.HealthcareServiceUnitID));

                string filterExpression = string.Format(
                        "HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID IN ({2}) AND IsUsingRegistration = 1",
                        AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, AppSession.MedicalDiagnostic.HealthcareServiceUnitID);
                if (serviceUnitUserCount > 0)
                {
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}) AND HealthcareServiceUnitID IN ({3})", Constant.Facility.DIAGNOSTIC, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID);
                }

                List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboInfoRegistrationServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboInfoRegistrationServiceUnit.SelectedIndex = 0;
            }
            else if (departmentID == Constant.Facility.LABORATORY)
            {
                int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format(
                        "DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsLaboratoryUnit = 1",
                        Constant.Facility.DIAGNOSTIC, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));

                string filterExpression = string.Format(
                        "HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsLaboratoryUnit = 1 AND IsUsingRegistration = 1",
                        AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC);
                if (serviceUnitUserCount > 0)
                {
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}) AND IsLaboratoryUnit = 1", Constant.Facility.DIAGNOSTIC, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
                }

                List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboInfoRegistrationServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboInfoRegistrationServiceUnit.SelectedIndex = 0;
            }
            else if (departmentID == Constant.Facility.MEDICAL_CHECKUP)
            {
                int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format(
                                            "DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}",
                                            Constant.Facility.MEDICAL_CHECKUP, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));

                string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.MEDICAL_CHECKUP);
                if (serviceUnitUserCount > 0)
                {
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2})", Constant.Facility.MEDICAL_CHECKUP, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
                }

                List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboInfoRegistrationServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboInfoRegistrationServiceUnit.SelectedIndex = 0;
            }
            else if (departmentID == Constant.Facility.PHARMACY)
            {
                int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format(
                                            "DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}",
                                            Constant.Facility.PHARMACY, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));

                string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsUsingRegistration = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.PHARMACY);
                if (serviceUnitUserCount > 0)
                {
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2})", Constant.Facility.PHARMACY, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
                }

                List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboInfoRegistrationServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboInfoRegistrationServiceUnit.SelectedIndex = 0;
            }

            // comment by RN : 20181130
            //if (departmentID != "DIAGNOSTIC")
            //{
            //    trServiceUnit.Style.Add("display", "none");
            //    if (departmentID != "EMERGENCY")
            //    {
            //        hdnHealthcareServiceUnitID.Value = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
            //    }
            //    else
            //    {
            //        hdnHealthcareServiceUnitID.Value = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.EMERGENCY))[0].HealthcareServiceUnitID.ToString();
            //    }
            //}
            //else
            //{
            //    int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND HealthcareServiceUnitID NOT IN ({3},{4})", Constant.Facility.DIAGNOSTIC, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID));
            //    string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ({2},{3})", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
            //    if (serviceUnitUserCount > 0)
            //    {
            //        filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}) AND HealthcareServiceUnitID NOT IN ({3},{4})", Constant.Facility.DIAGNOSTIC, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
            //    }

            //    List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            //    Methods.SetComboBoxField<vHealthcareServiceUnit>(cboInfoRegistrationServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            //    cboInfoRegistrationServiceUnit.SelectedIndex = 0;
            //}


            txtInfoRegistrationRegistrationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            Helper.SetControlEntrySetting(txtInfoRegistrationRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");
            Helper.SetControlEntrySetting(cboInfoRegistrationServiceUnit, new ControlEntrySetting(true, true, true), "mpPatientList");
           

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string healthcareServiceUnitID = cboInfoRegistrationServiceUnit.Value.ToString();

            string filterExpression = string.Format("HealthcareServiceUnitID = {0} AND RegistrationDate = '{1}'", healthcareServiceUnitID, Helper.GetDatePickerValue(txtInfoRegistrationRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRegistrationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vRegistration> lstEntity = BusinessLayer.GetvRegistrationList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpInfoRegistrationView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}