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

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class PatientInformation : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.HOSPITALIZED_PATIENT_LIST;
            
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
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                List<vInpatientServiceUnitLink> lstServiceUnit = BusinessLayer.GetvInpatientServiceUnitLinkList("");
                Methods.SetComboBoxField<vInpatientServiceUnitLink>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "ServiceUnitCode");
                cboServiceUnit.SelectedIndex = 0;

                List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(String.Format("ParentID IN ('{0}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.MEAL_TIME));
                Methods.SetComboBoxField<StandardCode>(cboMealTime, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.MEAL_TIME).ToList(), "StandardCodeName", "StandardCodeID");
                cboMealTime.SelectedIndex = 0;
                
                BindGridView(1, true, ref PageCount);

                Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

               
            }
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
            string id = Page.Request.QueryString["id"];
           // filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')", Constant.RegistrationStatus.CANCELLED, Constant.RegistrationStatus.OPEN, Constant.RegistrationStatus.CLOSED);
            if (cboServiceUnit.Value.ToString() != "0")
                filterExpression += string.Format("ServiceUnitCode = '{0}'", cboServiceUnit.Value);
            else
                filterExpression += string.Format("ServiceUnitCode IN ({0})", hdnLstHealthcareServiceUnitID.Value);
            if (cboMealTime.Value.ToString() != "0")
                filterExpression += string.Format(" AND GCMealTime = '{0}'", cboMealTime.Value);
            else
                filterExpression += string.Format(" AND GCMealTime like '%'");
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            
            filterExpression += " AND DepartmentID = 'INPATIENT'";

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvNutritionOrderDtCustomRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vNutritionOrderDtCustom> lstEntity = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vInpatientPatientListLink entity = e.Item.DataItem as vInpatientPatientListLink;
                Label lblNutritionInformation = (Label)e.Item.FindControl("lblNutritionInformation");
                vNutritionOrderDtCustom entityOrder = BusinessLayer.GetvNutritionOrderDtCustomList(String.Format("NutritionOrderDate = '{0}' ORDER BY NutritionOrderDtID DESC",DateTime.Now)).FirstOrDefault();
                if(entityOrder != null)
                    lblNutritionInformation.Text = String.Format("{0}, {1}",entityOrder.DiagnoseName,entityOrder.MealPlanName);
                else
                    lblNutritionInformation.Text = String.Empty;
            }   
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnTransactionNo.Value != "")
            {
                vInpatientPatientListLink entity = BusinessLayer.GetvInpatientPatientListLinkList(string.Format("RegistrationNo = '{0}'", hdnTransactionNo.Value))[0];
                Response.Redirect(GetResponseRedirectUrl(entity));
            }
        }

        private string GetResponseRedirectUrl(vInpatientPatientListLink entity)
        {
            string url = "";
            string id = Page.Request.QueryString["id"];
           // url = string.Format("~/Program/NutritionOrder/NutritionOrderEntryLink.aspx?id={0}", entity.RegistrationNo);
            return url;
        }

      
    }
}