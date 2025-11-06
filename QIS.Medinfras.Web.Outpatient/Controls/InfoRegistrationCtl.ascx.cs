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
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Outpatient.Controls
{
    public partial class InfoRegistrationCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
            string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}'", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT);
            if (serviceUnitUserCount > 0)
                filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2})", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboInfoRegistrationServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboInfoRegistrationServiceUnit.SelectedIndex = 0;

            txtInfoRegistrationRegistrationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("HealthcareServiceUnitID = {0} AND RegistrationDate = '{1}'", cboInfoRegistrationServiceUnit.Value, Helper.GetDatePickerValue(txtInfoRegistrationRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
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