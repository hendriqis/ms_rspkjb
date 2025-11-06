using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Outpatient.Program
{
    public partial class PatientQueueInformation : BasePageRegisteredPatient
    {
        protected int pageC = 1;
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Outpatient.PATIENT_QUEUE_INFORMATION;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string filterExpression = string.Format("DepartmentID = '{0}' AND IsUsingRegistration = 1 AND IsDeleted = 0", Constant.Facility.OUTPATIENT);
                List<vHealthcareServiceUnit> lstWard = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboClinic, lstWard, "ServiceUnitName", "HealthcareServiceUnitID");
                cboClinic.SelectedIndex = 0;

                BindingRptParamedic();

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        private void BindGridViewParamedic(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            BindingRptParamedic();
        }

        private void BindGridViewPatient(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            //BindingRptPatient();
        }

        protected void cbpViewParamedic_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindingRptParamedic();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewPatient_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindingRptPatient(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindingRptPatient(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindingRptParamedic()
        {
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID IN (SELECT ParamedicID FROM vConsultVisit WHERE HealthcareServiceUnitID = {0} AND GCVisitStatus != '{1}')", cboClinic.Value, Constant.VisitStatus.CANCELLED));
            rptParamedic.DataSource = lstParamedic;
            rptParamedic.DataBind();
        }


        private void BindingRptPatient(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format(
                                            "ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND QueueNo IS NOT NULL AND VisitDate = '{2}' AND GCRegistrationStatus != '{3}' AND GCVisitStatus != '{3}'",
                                            hdnParamedicID.Value, cboClinic.Value, DateTime.Now.ToString("yyyy-MM-dd"), Constant.VisitStatus.CANCELLED);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_DEFAULT);
            }
            List<vConsultVisit> lstPatient = BusinessLayer.GetvConsultVisitList(filterExpression, Constant.GridViewPageSize.GRID_DEFAULT, pageIndex, "Session, QueueNo");
            rptPatient.DataSource = lstPatient;
            rptPatient.DataBind();
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string filterExpression = "";
            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
        }
    }
}