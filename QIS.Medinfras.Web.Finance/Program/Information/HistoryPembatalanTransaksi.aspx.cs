using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class HistoryPembatalanTransaksi : BasePageRegisteredPatient
    {
        private GetUserMenuAccess menu;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.HISTORY_PEMBATALAN_TRANSAKSI;
        }

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected int PageCount = 1;
        protected int CurrPage = 1;
        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsActive = 1 AND IsHasRegistration = 1"));
                lstDept = lstDept.OrderBy(lst => lst.TabOrder).ToList();
                Methods.SetComboBoxField<Department>(cboDepartment, lstDept, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                ((GridPatientRegOrderCtl)grdPatient).InitializeControl();
            }
        }

        public override string GetFilterExpression()
        {
            string departmentID = cboDepartment.Value.ToString();
            string filterExpression = string.Format("GCVisitStatus NOT IN ('{0}') AND DepartmentID = '{1}'", Constant.VisitStatus.CANCELLED, departmentID);

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

        public override void OnGrdRowClick(string VisitID)
        {
            string healthcareServiceUnitID = "";
            if (hdnModuleID.Value == Constant.Module.LABORATORY || hdnModuleID.Value == Constant.Module.IMAGING)
                healthcareServiceUnitID = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
            else
                healthcareServiceUnitID = hdnServiceUnitID.Value.ToString();
            if (String.IsNullOrEmpty(healthcareServiceUnitID))
            {
                ConsultVisit oVisit = BusinessLayer.GetConsultVisit(Convert.ToInt32(VisitID));
                if (oVisit != null)
                {
                    healthcareServiceUnitID = oVisit.HealthcareServiceUnitID.ToString();
                }
            }
            if (healthcareServiceUnitID != "" || healthcareServiceUnitID != "0")
            {
                string url = "";
                url = string.Format("~/Program/Information/HistoryPembatalanTransaksiDetail.aspx?id={0}|{1}", healthcareServiceUnitID, VisitID);
                Response.Redirect(url);
            }
        }
    }
}