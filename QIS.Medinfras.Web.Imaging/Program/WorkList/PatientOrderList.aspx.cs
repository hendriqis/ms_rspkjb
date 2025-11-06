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
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.Imaging.Program.WorkList
{
    public partial class PatientOrderList : BasePagePatientOrder
    {
        private GetUserMenuAccess menu;
        protected int PageCount = 1;
        protected int CurrPage = 1;
        protected int pageCountOrder = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Imaging.IMAGING_TEST_ITEM;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string ImagingID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                hdnID.Value = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, ImagingID))[0].HealthcareServiceUnitID.ToString();

                #region Region Registrasi
                txtRealisationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                cboMedicSupport.SelectedIndex = 0;
                Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false), "mpServiceUnit");

                List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}'", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC));
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboMedicSupport, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");

                List<Department> lstDept = BusinessLayer.GetDepartmentList("IsActive='1'");
                Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
                #endregion

                #region Region Order
                txtOrderDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                cboMedicSupportOrder.SelectedIndex = 0;
                List<vHealthcareServiceUnit> lstHSUOrder = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}'", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC));
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboMedicSupportOrder, lstHSUOrder, "ServiceUnitName", "HealthcareServiceUnitID");

                List<Department> lstDeptOrder = BusinessLayer.GetDepartmentList("IsActive = 1");
                Methods.SetComboBoxField<Department>(cboPatientFromOrder, lstDeptOrder, "DepartmentName", "DepartmentID");
                #endregion
                grdRegisteredPatient.InitializeControl();
                grdOrderedPatient.InitializeControl();

                Helper.SetControlEntrySetting(cboPatientFrom, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboPatientFromOrder, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtRealisationDate, new ControlEntrySetting(true, true, true), "mpPatientList");
                Helper.SetControlEntrySetting(txtOrderDate, new ControlEntrySetting(true, true, true), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

               // if(AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                    //AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("VisitDate = '{0}' AND GCRegistrationStatus NOT IN ('{1}','{2}','{3}')", Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.RegistrationStatus.CANCELLED, Constant.RegistrationStatus.CLOSED, Constant.RegistrationStatus.OPEN);
            if (cboPatientFrom.Value!=null)
            {
                filterExpression += string.Format(" AND DepartmentID= '{0}'", cboPatientFrom.Value);
            }
            else { }
            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
            {
                filterExpression += string.Format("AND HealthcareServiceUnitID = '{0}'", hdnServiceUnitID.Value);
                return filterExpression;
            }
            else
                return filterExpression;

        }
        
        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            string url = string.Format("~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetail.aspx?id={0}|{1}", transactionNo, cboMedicSupport.Value);
            Response.Redirect(url);
        }

        public override void OnGrdRowClickTestOrder(string transactionNo, string TestOrderID)
        {
            string url = string.Format("~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetail.aspx?id=to|{0}|{1}", TestOrderID, transactionNo);
            Response.Redirect(url);
        }

        public override string GetFilterExpressionTestOrder()
        {
            string filterExpression = string.Format("TestOrderDate = '{0}' AND GCRegistrationStatus NOT IN ('{1}','{2}','{3}') AND GCTransactionStatus='{4}'", Helper.GetDatePickerValue(txtOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.RegistrationStatus.CANCELLED, Constant.RegistrationStatus.CLOSED, Constant.RegistrationStatus.OPEN, Constant.TransactionStatus.OPEN);
            if (cboPatientFromOrder.Value != null)
                filterExpression += string.Format(" AND DepartmentID= '{0}'", cboPatientFromOrder.Value);
            if (hdnServiceUnitOrderCode.Value != "0" && hdnServiceUnitOrderCode.Value != "")
                filterExpression += string.Format("AND VisitHSUID = '{0}'", hdnServiceUnitOrderCode.Value);
            return filterExpression;
        }
    }
}