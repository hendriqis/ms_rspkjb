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

namespace QIS.Medinfras.Web.EmergencyCare.Program
{
    public partial class RegisteredPatientList : BasePageRegisteredPatient
    {

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EmergencyCare.REGISTERED_PATIENT_LIST;
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

                txtRegistrationDateFrom.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtRegistrationDateTo.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                grdInpatientReg.InitializeControl();
            }
        }
        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("DepartmentID = '{0}' AND ActualVisitDate >= '{1}' AND ActualVisitDate <= '{2}'", 
                                                    Constant.Facility.EMERGENCY,
                                                    Helper.GetDatePickerValue(txtRegistrationDateFrom).ToString(Constant.FormatString.DATE_FORMAT_112),
                                                    Helper.GetDatePickerValue(txtRegistrationDateTo).ToString(Constant.FormatString.DATE_FORMAT_112)
                                                    );
            if (hdnFilterExpressionQuickSearch.Value == "Search")
                hdnFilterExpressionQuickSearch.Value = " ";

            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
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