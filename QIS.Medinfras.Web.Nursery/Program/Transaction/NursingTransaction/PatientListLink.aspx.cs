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
    public partial class PatientListLink : BasePageContent
    {
        private string menuType = "";

        public override string OnGetMenuCode()
        {
            string menuCode;
            switch (menuType)
            {
                case "RI": menuCode = Constant.MenuCode.Nursing.NURSING_TRANSACTION_INPATIENT;  break;
                case "RD": menuCode = Constant.MenuCode.Nursing.NURSING_TRANSACTION_EMERGENCY; break;
                default : menuCode = Constant.MenuCode.Nursing.NURSING_TRANSACTION_OUTPATIENT; break;
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

        private void prepareScreen()
        {
            txtRegistrationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            trRegistrationDate.Attributes.Remove("style");
            trServiceUnit.Attributes.Remove("style");
            if (menuType == "RI")
            {
                trRegistrationDate.Attributes.Add("style", "display:none");
                List<vServiceUnitLinkPerUser> lstServiceUnit = BusinessLayer.GetvServiceUnitLinkPerUserList(String.Format("UserID = '{0}' AND Tipe = '{1}'", AppSession.UserLogin.UserName,Constant.ServiceUnitLinkType.INPATIENT));
                Methods.SetComboBoxField<vServiceUnitLinkPerUser>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "ServiceUnitCode");
                cboServiceUnit.SelectedIndex = 0;
            }
            else if (menuType == "RD")
            {
                trServiceUnit.Attributes.Add("style", "display:none");
            }
            else 
            {
                List<vServiceUnitLinkPerUser> lstServiceUnit = BusinessLayer.GetvServiceUnitLinkPerUserList(String.Format("UserID = '{0}' AND Tipe = '{1}'", AppSession.UserLogin.UserName, Constant.ServiceUnitLinkType.OUTPATIENT));
                Methods.SetComboBoxField<vServiceUnitLinkPerUser>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "ServiceUnitCode");
                cboServiceUnit.SelectedIndex = 0;
            }
        }

        protected string OnGetRegistrationFilterExpression()
        {
            return String.Format("App = '{0}'", Request.QueryString["id"]);
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

        private string GetFilterExpressionInpatient()
        {
            string filterExpression = "";
            string id = Page.Request.QueryString["id"];
                filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.OPEN, Constant.VisitStatus.CLOSED);
                if (cboServiceUnit.Value.ToString() != "0")
                    filterExpression += string.Format(" AND ServiceUnitCode = '{0}'", cboServiceUnit.Value);
                else
                    filterExpression += string.Format(" AND ServiceUnitCode IN ({0})", hdnLstHealthcareServiceUnitID.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            filterExpression += " AND DepartmentID = 'INPATIENT'";
            
            return filterExpression;
        }

        private string GetFilterExpressionEmergency()
        {
            string filterExpression = "";
            string id = Page.Request.QueryString["id"];
            filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}') AND VisitDate = '{3}'", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.OPEN, Constant.VisitStatus.CLOSED,Helper.GetDatePickerValue(txtRegistrationDate.Text).ToString("yyyy-MM-dd"));
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            filterExpression += " AND DepartmentID = 'EMERGENCY'";

            return filterExpression;
        }

        private string GetFilterExpressionOutpatient()
        {
            string filterExpression = "";
            string id = Page.Request.QueryString["id"];
            filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}') AND VisitDate = '{3}'", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.OPEN, Constant.VisitStatus.CLOSED, Helper.GetDatePickerValue(txtRegistrationDate.Text).ToString("yyyy-MM-dd"));
            if (cboServiceUnit.Value.ToString() != "0")
                filterExpression += string.Format(" AND ServiceUnitCode = '{0}'", cboServiceUnit.Value);
            else
                filterExpression += string.Format(" AND ServiceUnitCode IN ({0})", hdnLstHealthcareServiceUnitID.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            filterExpression += " AND DepartmentID = 'OUTPATIENT'";

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            menuType = Page.Request.QueryString["id"];
            if (menuType == "RI")
            {
                string filterExpression = GetFilterExpressionInpatient();

                List<vInpatientPatientListLink> lstEntity = BusinessLayer.GetvInpatientPatientListLinkList(filterExpression);
                lvwView.DataSource = lstEntity;
                lvwView.DataBind();
            }
            else if (menuType == "RD")
            {
                string filterExpression = GetFilterExpressionEmergency();

                List<vEmergencyPatientListLink> lstEntity = BusinessLayer.GetvEmergencyPatientListLinkList(filterExpression);
                lvwView.DataSource = lstEntity;
                lvwView.DataBind();
            }
            else 
            {
                string filterExpression = GetFilterExpressionOutpatient();

                List<vOutPatientPatientListLink> lstEntity = BusinessLayer.GetvOutPatientPatientListLinkList(filterExpression);
                lvwView.DataSource = lstEntity;
                lvwView.DataBind();
            }
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnTransactionNo.Value != "")
            {
                Response.Redirect(GetResponseRedirectUrl(hdnTransactionNo.Value));
            }
        }

        private string GetResponseRedirectUrl(string registrationNo)
        {
            string url = "";
            string id = Page.Request.QueryString["id"];
            url = string.Format("~/Program/Transaction/NursingTransaction/NursingTransactionEntryLink.aspx?id={1}|{0}", registrationNo, id);
            return url;
        }

        protected void cbpBarcodeEntryProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            menuType = Page.Request.QueryString["id"];
            string filterExpression = ""; 
            string registrationNo = "";
            if (menuType == "RI")
            {
                filterExpression = GetFilterExpressionInpatient();
                filterExpression += string.Format(" AND MedicalNo = '{0}' ORDER BY RegistrationNo", txtBarcodeEntry.Text);
                vInpatientPatientListLink entity = BusinessLayer.GetvInpatientPatientListLinkList(filterExpression).FirstOrDefault();
                if (entity != null) registrationNo = entity.RegistrationNo;
            }
            else if (menuType == "RD")
            {
                filterExpression = GetFilterExpressionEmergency();
                filterExpression += string.Format(" AND MedicalNo = '{0}' ORDER BY RegistrationNo", txtBarcodeEntry.Text);
                vEmergencyPatientListLink entity = BusinessLayer.GetvEmergencyPatientListLinkList(filterExpression).FirstOrDefault();
                if (entity != null) registrationNo = entity.RegistrationNo;
            }
            else
            {
                filterExpression = GetFilterExpressionOutpatient();
                filterExpression += string.Format(" AND MedicalNo = '{0}' ORDER BY RegistrationNo", txtBarcodeEntry.Text);
                vOutPatientPatientListLink entity = BusinessLayer.GetvOutPatientPatientListLinkList(filterExpression).FirstOrDefault();
                if (entity != null) registrationNo = entity.RegistrationNo;
            }
            

            string url = "";
            
            url = GetResponseRedirectUrl(registrationNo);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpUrl"] = url;
        }
    }
}