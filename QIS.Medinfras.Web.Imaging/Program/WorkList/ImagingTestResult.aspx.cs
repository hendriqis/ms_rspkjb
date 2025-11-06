using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Imaging.Program.WorkList
{
    public partial class ImagingTestResult : BasePageRegisteredPatient
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}'", Constant.Facility.DIAGNOSTIC));
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboMedicSupport, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
                cboMedicSupport.SelectedIndex = 0;
                List<Department> lstDept = BusinessLayer.GetDepartmentList("IsActive='1'");
                Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
                txtTransactionDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                
                Helper.SetControlEntrySetting(txtTransactionDate, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboMedicSupport, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboPatientFrom, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(true, true, false), "mpPatientList");

                grdPatientResult.InitializeControl();
            }
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Imaging.IMAGING_TEST_ITEM;
        }
        
        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format("TransactionDate = '{0}' AND HealthcareServiceUnitID= '{1}' AND GCTransactionStatus='{2}'", Helper.GetDatePickerValue(txtTransactionDate).ToString(Constant.FormatString.DATE_FORMAT_112), cboMedicSupport.Value ,Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            if (cboPatientFrom.Value != null) {
                filterExpression += string.Format(" AND DepartmentID='{0}'", cboPatientFrom.Value);
            }
            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
            {
                filterExpression += string.Format("AND VisitHealthcareServiceUnitID = '{0}'", hdnServiceUnitID.Value);
                return filterExpression;
            }
            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            string url = "";
            url = string.Format("~/Program/Worklist/ImagingTestResultDetail.aspx?id={0}", transactionNo);
            Response.Redirect(url);
        }
        
    }
}