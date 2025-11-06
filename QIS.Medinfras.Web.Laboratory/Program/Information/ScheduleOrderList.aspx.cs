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
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class ScheduleOrderList : BasePagePatientOrder
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.LAB_SCHEDULED_LIST;
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
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                string filterExpression = string.Format("HealthcareServiceUnitID IN ({0})", AppSession.MedicalDiagnostic.HealthcareServiceUnitID);

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID != '{0}' AND IsActive = 1 AND IsHasRegistration = 1", Constant.Facility.PHARMACY));

                #region Region Order
                txtOrderDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                lstDept.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
                Methods.SetComboBoxField<Department>(cboPatientFromOrder, lstDept, "DepartmentName", "DepartmentID");

                //lstHealthcareServiceUnit.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = string.Format(" - {0} - ", GetLabel("All")) });
                cboPatientFromOrder.SelectedIndex = 0;
                #endregion

                ((GridPatientScheduledOrderCtl)grdOrderedPatient).InitializeControl();

                Helper.SetControlEntrySetting(cboPatientFromOrder, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtOrderDate, new ControlEntrySetting(true, true, true), "mpPatientList");
            }
        }
        public override bool IsShowRightPanel()
        {
            return false;
        }


        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            return string.Empty;
        }

        public override string GetFilterExpressionTestOrder()
        {
            string filterExpression = "";
            string healthcareServiceUnitID = "";

            healthcareServiceUnitID = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();

            filterExpression += string.Format("ScheduledDate = '{0}' AND HealthcareServiceUnitID = {1} AND GCVisitStatus NOT IN ('{2}','{3}','{4}') AND ",
                    Helper.GetDatePickerValue(txtOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                    healthcareServiceUnitID, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);

            filterExpression += string.Format("GCTransactionStatus = '{0}'", Constant.TransactionStatus.WAIT_FOR_APPROVAL);

            if (cboPatientFromOrder.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFromOrder.Value);
            if (hdnServiceUnitOrderID.Value != "0" && hdnServiceUnitOrderID.Value != "")
                filterExpression += string.Format(" AND VisitHSUID = {0}", hdnServiceUnitOrderID.Value);
            if (hdnFilterExpressionQuickSearchOrder.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchOrder.Value);
            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
        }

        public override void OnGrdRowClickTestOrder(string transactionNo, string TestOrderID)
        {
        }

        public override string GetSortingTestOrder()
        {
            string sortBy = "";

            return sortBy;
        }
    }
}