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
using System.Globalization;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ResendLaboratoryOrderList : BasePagePatientOrder
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.RESEND_ORDER;
        }
        private GetUserMenuAccess menu;
        public override bool IsShowRightPanel()
        {
            return true;
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
        private string refreshGridInterval = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LB_INTERVAL_AUTO_REFRESH).ParameterValue;
                hdnID.Value = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
                hdnIDReg.Value = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();

                hdnLastContentID.Value = "containerByOrder";

                string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND IsLaboratoryUnit = 1", AppSession.UserLogin.HealthcareID);

                List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnitOrder, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = cboServiceUnitOrder.SelectedIndex = 0;

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Diproses" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diproses" });
                Methods.SetComboBoxField<Variable>(cboOrderResultType, lstVariable, "Value", "Code");
                cboOrderResultType.Value = "1";

                txtSearchViewReg.IntellisenseHints = Methods.LoadRegistrationWorklistQuickFilterHints("1");
                txtSearchViewOrder.IntellisenseHints = Methods.LoadLaboratoryWorklistQuickFilterHints("1");

                List<Department> lstDepartment = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration = 1 AND IsActive = 1 AND DepartmentID != '{0}'", Constant.Facility.PHARMACY));
                List<Department> lstDepartmentOrder = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration = 1 AND IsActive = 1 AND DepartmentID != '{0}'", Constant.Facility.PHARMACY));
                lstDepartmentOrder.Insert(0, new Department { DepartmentName = string.Format("{0}", GetLabel(" ")) });

                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
                Methods.SetComboBoxField<Department>(cboDepartmentOrder, lstDepartmentOrder, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;
                cboDepartmentOrder.SelectedIndex = 0;

                txtDate.Text = txtTestOrderDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                SettingControlProperties();

                grdRegisteredPatient.InitializeControl();
                grdOrderPatient.InitializeControl();

                Helper.SetControlEntrySetting(txtDate, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtTestOrderDate, new ControlEntrySetting(true, true, false), "mpPatientListOrder");
                Helper.SetControlEntrySetting(cboDepartmentOrder, new ControlEntrySetting(true, true, false), "mpPatientListOrder");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        private void SettingControlProperties()
        {
            if (AppSession.LastContentLaboratoryRealization != null)
            {
                LastContentLaboratoryRealization lc = AppSession.LastContentLaboratoryRealization;
                if (lc.LabContentID.Equals("containerByOrder"))
                {
                    hdnLastContentID.Value = "containerByOrder";
                    cboDepartmentOrder.Value = lc.LabPatientFrom;
                    txtTestOrderDate.Text = DateTime.ParseExact(lc.LabDate,
                              Constant.FormatString.DATE_FORMAT_112,
                               CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    hdnQuickTextOrder.Value = lc.LabQuickFilter.Equals("Search") ? "" : lc.LabQuickFilter;
                    hdnFilterExpressionQuickSearchOrder.Value = lc.LabQuickFilterExpression;
                    cboOrderResultType.Value = lc.LabDisplayResult;
                }
                else
                {
                    hdnLastContentID.Value = "containerDaftar";
                    cboDepartment.Value = lc.LabPatientFrom;
                    txtDate.Text = DateTime.ParseExact(lc.LabDate,
                              Constant.FormatString.DATE_FORMAT_112,
                               CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    hdnQuickTextReg.Value = lc.LabQuickFilter.Equals("Search") ? "" : lc.LabQuickFilter;
                    hdnFilterExpressionQuickSearchReg.Value = lc.LabQuickFilterExpression;
                }
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression = string.Format("GCVisitStatus IN ('{0}')", Constant.VisitStatus.CLOSED);

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() != "INPATIENT")
            {
                if (!chkIsIgnoreDate.Checked)
                {
                    filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtDate).ToString(Constant.FormatString.DATE_FORMAT_112));
                }
            }

            if (cboDepartment.Value != null)
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);
            }

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
            string cboDepartmentOrderValue = "";

            if (cboDepartmentOrder.Value != null)
            {
                cboDepartmentOrderValue = cboDepartmentOrder.Value.ToString();
            }
            else
            {
                cboDepartmentOrderValue = "";
            }

            LastContentLaboratoryRealization lc = new LastContentLaboratoryRealization()
            {
                LabContentID = hdnLastContentID.Value,
                LabDate = Helper.GetDatePickerValue(txtTestOrderDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112),
                LabDisplayResult = cboOrderResultType.Value.ToString(),
                LabPatientFrom = cboDepartmentOrderValue,
                LabQuickFilter = txtSearchViewOrder.Text,
                LabQuickFilterExpression = hdnFilterExpressionQuickSearchOrder.Value
            };
            AppSession.LastContentLaboratoryRealization = lc;

            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo)).FirstOrDefault();
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.DischargeDate = entity.DischargeDate;
            pt.DischargeTime = entity.DischargeTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.ParamedicCode = entity.ParamedicCode;
            pt.ParamedicName = entity.ParamedicName;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            pt.ChargeClassID = entity.ChargeClassID;
            pt.IsBillingReopen = entity.IsBillingReopen;
            pt.IsLockDown = entity.IsLockDown;
            AppSession.RegisteredPatient = pt;

            string url = "";
            url = string.Format("~/Libs/Program/Module/PatientManagement/Process/ResendLaboratoryOrderDetail.aspx?id=pr|{0}|{1}|{2}", transactionNo, hdnServiceUnitID.Value, entity.RegistrationID);
            Response.Redirect(url);
        }

        public override void OnGrdRowClickTestOrder(string transactionNo, string TestOrderID)
        {
            string cboDepartmentOrderValue = "";

            if (cboDepartmentOrder.Value != null)
            {
                cboDepartmentOrderValue = cboDepartmentOrder.Value.ToString();
            }
            else
            {
                cboDepartmentOrderValue = "";
            }

            LastContentLaboratoryRealization lc = new LastContentLaboratoryRealization()
            {
                LabContentID = hdnLastContentID.Value,
                LabDate = Helper.GetDatePickerValue(txtTestOrderDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112),
                LabDisplayResult = cboOrderResultType.Value.ToString(),
                LabPatientFrom = cboDepartmentOrderValue,
                LabQuickFilter = txtSearchViewOrder.Text,
                LabQuickFilterExpression = hdnFilterExpressionQuickSearchOrder.Value
            };
            AppSession.LastContentLaboratoryRealization = lc;

            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo)).FirstOrDefault();
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.DischargeDate = entity.DischargeDate;
            pt.DischargeTime = entity.DischargeTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.ParamedicCode = entity.ParamedicCode;
            pt.ParamedicName = entity.ParamedicName;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            pt.ChargeClassID = entity.ChargeClassID;
            pt.IsBillingReopen = entity.IsBillingReopen;
            pt.IsLockDown = entity.IsLockDown;
            AppSession.RegisteredPatient = pt;

            string url = "";
            url = string.Format("~/Libs/Program/Module/PatientManagement/Process/ResendLaboratoryOrderDetail.aspx?id=to|{0}|{1}|{2}", transactionNo, TestOrderID, entity.RegistrationID);
            Response.Redirect(url);
        }

        public override string GetFilterExpressionTestOrder()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format("GCVisitStatus IN ('{0}')", Constant.VisitStatus.CLOSED);

            if (cboOrderResultType.Value.ToString() == "0")
            {
                filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}','{2}')",
                    Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
            }
            else if (cboOrderResultType.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND GCTransactionStatus = '{0}'",
                    Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            }
            else
            {
                filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}')",
                    Constant.TransactionStatus.PROCESSED, Constant.TransactionStatus.CLOSED);
            }

            if (hdnID.Value != "0" && hdnID.Value != "") filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}", hdnID.Value);

            if (cboDepartmentOrder.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartmentOrder.Value);

            if (hdnServiceUnitIDOrder.Value != "0" && hdnServiceUnitIDOrder.Value != "")
                filterExpression += string.Format(" AND VisitHSUID = '{0}'", hdnServiceUnitIDOrder.Value);

            if (hdnFilterExpressionQuickSearchOrder.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchOrder.Value);

            if (!chkIgnoreDateOrder.Checked)
            {
                filterExpression += string.Format(" AND ScheduledDate = '{0}'", Helper.GetDatePickerValue(txtTestOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            filterExpression += string.Format(" AND IsAIOTransaction = 0");

            return filterExpression;
        }

        public override string GetSortingTestOrder()
        {
            string sortBy = "";

            return sortBy;
        }
    }
}