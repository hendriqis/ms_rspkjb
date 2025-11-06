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
    public partial class NutritionOrderListLink : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.NUTRITION_ORDER_INPATIENT;
            
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

                List<vInpatientServiceUnitLinkPerUser> lstServiceUnit = BusinessLayer.GetvInpatientServiceUnitLinkPerUserList(String.Format("UserID = '{0}' AND IsDeleted = 0", AppSession.UserLogin.UserName));
                Methods.SetComboBoxField<vInpatientServiceUnitLinkPerUser>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "ServiceUnitCode");
                cboServiceUnit.SelectedIndex = 0;

                BindGridView(1, true, ref PageCount);

                Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                txtBarcodeEntry.Focus();
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            List<vInpatientPatientListLink> lstEntity = BusinessLayer.GetvInpatientPatientListLinkList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vInpatientPatientListLink entity = e.Item.DataItem as vInpatientPatientListLink;
                Label lblNutritionInformation1 = (Label)e.Item.FindControl("lblNutritionInformation1");
                Label lblNutritionInformation2 = (Label)e.Item.FindControl("lblNutritionInformation2");
                Label lblNutritionInformation3 = (Label)e.Item.FindControl("lblNutritionInformation3");
                Label lblNutritionInformation4 = (Label)e.Item.FindControl("lblNutritionInformation4");
                vNutritionOrderDt entityOrder = BusinessLayer.GetvNutritionOrderDtList(String.Format("RegistrationNo = '{0}' ORDER BY NutritionOrderDtID DESC",entity.RegistrationNo)).FirstOrDefault();
                if (entityOrder != null)
                {
                    lblNutritionInformation1.Text = String.Format("Jadwal Makan ({0})", entityOrder.MealTime);
                    lblNutritionInformation2.Text = String.Format("Diit ({0} {1})", entityOrder.MealPlanCode, entityOrder.MealPlanName);
                    lblNutritionInformation3.Text = String.Format("Diagnosa ({0}-{1}, {2})", entityOrder.DiagnoseID, entityOrder.DiagnoseName, entityOrder.DiagnoseText);
                    lblNutritionInformation4.Text = String.Format("{0} ({1}) {2}", entityOrder.NutritionOrderDate.ToString("dd-MMM-yyyy"), entityOrder.NutritionOrderTime, entityOrder.CreatedByName);
                }
                else
                {
                    lblNutritionInformation1.Text = String.Empty;
                    lblNutritionInformation2.Text = String.Empty;
                    lblNutritionInformation3.Text = String.Empty;
                    lblNutritionInformation4.Text = String.Empty;
                }
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
            url = string.Format("~/Program/NutritionOrder/NutritionOrderEntryLink.aspx?id={0}", entity.RegistrationNo);
            return url;
        }

        protected void cbpBarcodeEntryProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = GetFilterExpression();
            filterExpression += string.Format(" AND MedicalNo = '{0}'", txtBarcodeEntry.Text);
            vInpatientPatientListLink entity = BusinessLayer.GetvInpatientPatientListLinkList(filterExpression).FirstOrDefault();

            string url = "";
            if (entity != null)
                url = GetResponseRedirectUrl(entity);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpUrl"] = url;
        }
    }
}