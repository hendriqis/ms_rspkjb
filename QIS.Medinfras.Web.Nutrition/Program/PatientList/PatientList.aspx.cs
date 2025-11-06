using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class PatientList : BasePagePatientOrder
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.NUTRITION_PATIENT_PAGE;
        }
        private GetUserMenuAccess menu;
        public override bool IsShowRightPanel()
        {
            return false;
        }
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "10";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string paramFilter = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL);

                List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(paramFilter);
                if (lstParam.Count > 0)
                {
                    refreshGridInterval = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).FirstOrDefault().ParameterValue;
                }

                string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsNutritionUnit = 1 AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC);

                List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                string deptFilter = string.Format("IsActive = 1 AND DepartmentID != '{0}' AND IsHasRegistration = 1", Constant.Facility.PHARMACY);

                List<Department> lstDepartment = BusinessLayer.GetDepartmentList(deptFilter);
                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
                cboDepartment.Value = Constant.Facility.INPATIENT.ToString();

                SettingControlProperties();
                
                txtOrderDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                grdOrderPatient.InitializeControl();
                grdRegisteredPatient.InitializeControl();

                Helper.SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, false), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        private void SettingControlProperties()
        {
            if (AppSession.LastContentUDDList != null)
            {
                LastContentPrescriptionEntry lc = AppSession.LastContentUDDList;
                hdnLastContentID.Value = lc.ContentID;
                cboServiceUnit.Value = lc.HealthcareServiceUnitID.ToString();
                cboDepartment.Value = lc.FromDepartmentID;
                hdnServiceUnitID.Value = lc.FromHealthcareServiceUnitID.ToString();
                vHealthcareServiceUnit entityHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value)).FirstOrDefault();
                txtServiceUnitCode.Text = entityHealthcareServiceUnit == null ? "" : entityHealthcareServiceUnit.ServiceUnitCode;
                txtServiceUnitName.Text = entityHealthcareServiceUnit == null ? "" : entityHealthcareServiceUnit.ServiceUnitName;
                hdnQuickTextReg.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                hdnFilterExpressionQuickSearchReg.Value = lc.QuickFilterExpression;
            }
            else
            {
                cboDepartment.Value = Constant.Facility.INPATIENT;
            }

            List<Variable> lstVariable = new List<Variable>();
            lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
            lstVariable.Add(new Variable { Code = "1", Value = "Baru" });
            lstVariable.Add(new Variable { Code = "2", Value = "Sudah Dikonfirmasi/Diterima" });
            lstVariable.Add(new Variable { Code = "3", Value = "Sudah Diproses" });
            Methods.SetComboBoxField<Variable>(cboOrderResultType, lstVariable, "Value", "Code");
            cboOrderResultType.Value = "1";
        }

        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
                filterExpression += " AND ";

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() == "INPATIENT")
                filterExpression += string.Format("DepartmentID = '{0}' AND GCVisitStatus NOT IN ('{1}','{2}','{3}','{4}')", cboDepartment.Value, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            if (hdnFilterExpressionQuickSearchReg.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchReg.Value);

            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo)).FirstOrDefault();
            if (entity != null)
            {
                RegisteredPatient pt = new RegisteredPatient();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
                pt.RegistrationID = entity.RegistrationID;
                pt.VisitID = entity.VisitID;
                pt.VisitDate = entity.VisitDate;
                pt.VisitTime = entity.VisitTime;
                pt.ParamedicID = entity.ParamedicID;
                pt.SpecialtyID = entity.SpecialtyID;
                pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                pt.DepartmentID = entity.DepartmentID;
                pt.ClassID = entity.ClassID;
                pt.ChargeClassID = entity.ChargeClassID;
                AppSession.RegisteredPatient = pt;
                LastContentPrescriptionEntry lc = new LastContentPrescriptionEntry()
                {
                    ContentID = hdnLastContentID.Value,
                    HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value),
                    FromDepartmentID = cboDepartment.Value.ToString(),
                    FromHealthcareServiceUnitID = hdnServiceUnitID.Value == "" ? 0 : Convert.ToInt32(hdnServiceUnitID.Value),
                    QuickText = txtSearchViewReg.Text,
                    QuickFilterExpression = hdnFilterExpressionQuickSearchReg.Value,
                    VariableDisplay = "0"
                };
                AppSession.LastContentUDDList = lc;
                string url = "";
                url = string.Format("~/Program/Prescription/UDD/MedicationOrder/UDDMedicationOrderEntry.aspx");
                Response.Redirect(url);
            }
        }

        public override void OnGrdRowClickTestOrder(string transactionNo, string prescriptionOrderID)
        {
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo)).FirstOrDefault();
            if (entity != null)
            {
                RegisteredPatient pt = new RegisteredPatient();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
                pt.RegistrationID = entity.RegistrationID;
                pt.VisitID = entity.VisitID;
                pt.VisitDate = entity.VisitDate;
                pt.VisitTime = entity.VisitTime;
                pt.ParamedicID = entity.ParamedicID;
                pt.SpecialtyID = entity.SpecialtyID;
                pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                pt.DepartmentID = entity.DepartmentID;
                pt.ClassID = entity.ClassID;
                pt.ChargeClassID = entity.ChargeClassID;
                AppSession.RegisteredPatient = pt;
                LastContentPrescriptionEntry lc = new LastContentPrescriptionEntry()
                {
                    ContentID = hdnLastContentID.Value,
                    HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value),
                    FromDepartmentID = cboDepartment.Value.ToString(),
                    FromHealthcareServiceUnitID = hdnServiceUnitID.Value == "" ? 0 : Convert.ToInt32(hdnServiceUnitID.Value),
                    QuickText = txtSearchViewReg.Text,
                    QuickFilterExpression = hdnFilterExpressionQuickSearchReg.Value,
                    VariableDisplay = "0"
                };
                AppSession.LastContentUDDList = lc;
                string url = "";
                string[] orderInfo = prescriptionOrderID.Split('|');
                url = string.Format("~/Program/Prescription/UDD/MedicationOrder/UDDMedicationOrderEntry.aspx?id=to|{0}|{1}",transactionNo,orderInfo[0]);
                Response.Redirect(url);
            }
        }

        public override string GetFilterExpressionTestOrder()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
                filterExpression += " AND ";

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() == "INPATIENT")
                filterExpression += string.Format("DepartmentID = '{0}' AND GCVisitStatus NOT IN ('{1}','{2}','{3}','{4}')", cboDepartment.Value, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);

            filterExpression += string.Format(" AND DispensaryServiceUnitID = {0}", cboServiceUnit.Value);

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND VisitHSUID = {0}", hdnServiceUnitID.Value);
            if (hdnFilterExpressionQuickSearchReg.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchReg.Value);


            if (cboOrderResultType.Value.ToString() == "0")
            {
                filterExpression += string.Format(" AND PrescriptionDate = '{0}' AND GCTransactionStatus IN ('{1}','{2}')",
                    Helper.GetDatePickerValue(txtOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED);
            }
            else if (cboOrderResultType.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND PrescriptionDate = '{0}' AND GCTransactionStatus = '{1}' AND GCOrderStatus = '{2}'",
                    Helper.GetDatePickerValue(txtOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112),Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.OrderStatus.OPEN);
            }
            else if (cboOrderResultType.Value.ToString() == "2")
            {
                filterExpression += string.Format(" AND PrescriptionDate = '{0}' AND GCTransactionStatus = '{1}' AND GCOrderStatus = '{2}'",
                    Helper.GetDatePickerValue(txtOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.OrderStatus.RECEIVED);
            }
            else
            {
                filterExpression += string.Format(" AND PrescriptionDate = '{0}' AND GCTransactionStatus IN ('{1}')",
                    Helper.GetDatePickerValue(txtOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112),Constant.TransactionStatus.PROCESSED);
            }

            return filterExpression;
        }
    }
}