using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class InformationAplicares : BasePageContent
    {
        private GetUserMenuAccess menu;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.INFORMATION_APLICARES;
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
                hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
                
                string filterClass = string.Format("IsDeleted = 0");
                List<ClassCare> lstCC = BusinessLayer.GetClassCareList(filterClass);
                lstCC.Insert(0, new ClassCare { ClassID = 0, ClassName = "" });
                Methods.SetComboBoxField<ClassCare>(cboAplicaresClass, lstCC, "ClassName", "ClassID");
                cboAplicaresClass.SelectedIndex = 0;

                BindGridView();
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();
            List<vServiceUnitAplicares> lstEntity = BusinessLayer.GetvServiceUnitAplicaresList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        private string GetFilterExpression()
        {
            string filterExpression = "CountIsSendToAplicares > 0";

            if (hdnServiceUnitID.Value != "" && hdnServiceUnitID.Value != "0")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression = string.Format("HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            }

            if (cboAplicaresClass.Value.ToString() != "0" && cboAplicaresClass.Value.ToString() != "")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("ClassID = {0}", cboAplicaresClass.Value);
            }
            
            return filterExpression;
        }

    }
}