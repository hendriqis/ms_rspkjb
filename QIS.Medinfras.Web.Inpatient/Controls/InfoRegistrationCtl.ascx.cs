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

namespace QIS.Medinfras.Web.Inpatient.Controls
{
    public partial class InfoRegistrationCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            string departmentID = Page.Request.QueryString["id"];
           
            txtInfoRegistrationRegistrationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            Helper.SetControlEntrySetting(txtInfoRegistrationRegistrationDate, new ControlEntrySetting(true, true, true),"mpPatientList");

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("DepartmentID = 'INPATIENT' AND RegistrationDate = '{0}'", Helper.GetDatePickerValue(txtInfoRegistrationRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
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