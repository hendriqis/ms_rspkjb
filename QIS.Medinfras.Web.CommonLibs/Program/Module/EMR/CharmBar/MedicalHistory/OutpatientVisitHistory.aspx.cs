using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class OutpatientVisitHistory : BasePage
    {
        protected int PageCount = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SettingParameterDt oParam = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.DEFAULT_DAYS_PRMRJ);
                double days = -30;
                if (oParam != null)
                {
                    if (!string.IsNullOrEmpty(oParam.ParameterValue))
                    {
                        days = Convert.ToDouble(oParam.ParameterValue) * -1;
                    }
                }

                txtFromDate.Text = DateTime.Today.AddDays(days).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtToDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                BindGridView(1, true, ref PageCount);
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND VisitID != {1} AND VisitDate BETWEEN '{2}' AND '{3}'", AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.VisitID,Helper.GetDatePickerValue(txtFromDate).ToString(Constant.FormatString.DATE_FORMAT_112),Helper.GetDatePickerValue(txtToDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            if (chkIsComplexVisit.Checked)
            {
                filterExpression += string.Format(" AND IsComplexVisit = 1");
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvOutpatientVisitHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vOutpatientVisitHistory> lstEntity = BusinessLayer.GetvOutpatientVisitHistoryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "VisitDate DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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