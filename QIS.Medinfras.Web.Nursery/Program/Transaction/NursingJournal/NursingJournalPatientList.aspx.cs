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
using System.Text;

namespace QIS.Medinfras.Web.Nursing.Program
{
    public partial class NursingJournalPatientList : BasePageContent
    {
        private string menuType = "";

        public override string OnGetMenuCode()
        {
            string menuCode;
            switch (menuType)
            {
                case "RI": menuCode = Constant.MenuCode.Nursing.NURSING_JOURNAL_INPATIENT; break;
                case "RD": menuCode = Constant.MenuCode.Nursing.NURSING_JOURNAL_EMERGENCY; break;
                default: menuCode = Constant.MenuCode.Nursing.NURSING_JOURNAL_OUTPATIENT; break;
            }
            return menuCode;
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
        private string refreshGridInterval = "";
        protected int PageCount = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                menuType = Request.QueryString["id"];   

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                prepareScreen();

                BindGridView(1, true, ref PageCount);

                Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                txtBarcodeEntry.Focus();
            }
        }

        private string GetDepartmentID()
        {
            string departmentID = "";
            menuType = Request.QueryString["id"];
            switch (menuType)
            {
                case "RI": departmentID = Constant.Facility.INPATIENT; break;
                case "RJ": departmentID = Constant.Facility.OUTPATIENT; break;
                case "RD": departmentID = Constant.Facility.EMERGENCY; break;
            }
            return departmentID;
        }

        private void prepareScreen()
        {
            txtRegistrationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            trRegistrationDate.Attributes.Remove("style");
            trServiceUnit.Attributes.Remove("style");
            string departmentID = GetDepartmentID();

            List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, departmentID, "");
            Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            if(cboServiceUnit.Items.Count > 0)
                cboServiceUnit.SelectedIndex = 0;

            if(departmentID == Constant.Facility.INPATIENT)
                trRegistrationDate.Attributes.Add("style", "display:none");
            else if(departmentID == Constant.Facility.EMERGENCY)
                trServiceUnit.Attributes.Add("style", "display:none");
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

        private string GetFilterExpression()
        {
            string filterExpression = "";
            string departmentID = GetDepartmentID();
            filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.OPEN, Constant.VisitStatus.CLOSED);
            if(departmentID != Constant.Facility.INPATIENT)
                filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRegistrationDate.Text).ToString("yyyy-MM-dd"));
            if (departmentID != Constant.Facility.EMERGENCY)
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            filterExpression += String.Format(" AND DepartmentID = '{0}'", GetDepartmentID());

            return filterExpression;
        }        

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnTransactionNo.Value != "")
            {
                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnTransactionNo.Value))[0];
                Response.Redirect(GetResponseRedirectUrl(entity));
            }
        }

        private string GetResponseRedirectUrl(vConsultVisit entity)
        {
            string url = "";
            string id = Page.Request.QueryString["id"];
            url = string.Format("~/Program/Transaction/NursingJournal/NursingJournalEntry.aspx?id={1}|{0}", entity.VisitID, id);
            return url;
        }

        protected void cbpBarcodeEntryProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = GetFilterExpression();
            filterExpression += string.Format(" AND MedicalNo = '{0}' AND DepartmentID = '{1}'", txtBarcodeEntry.Text,GetDepartmentID());
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(filterExpression).FirstOrDefault();

            string url = "";
            if (entity != null)
                url = GetResponseRedirectUrl(entity);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpUrl"] = url;
        }
    }
}