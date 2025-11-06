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
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class NutritionOrderList : BasePageContent
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

                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.INPATIENT, "");
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
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
                filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.OPEN, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED);
            if (cboServiceUnit.Value.ToString() != "0")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            else
                filterExpression += string.Format(" AND HealthcareServiceUnitID IN ({0})", hdnLstHealthcareServiceUnitID.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex);
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
            url = string.Format("~/Program/NutritionOrder/NutritionOrderEntry.aspx?id={0}", entity.VisitID);
            return url;
        }

        protected void cbpBarcodeEntryProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = GetFilterExpression();
            filterExpression += string.Format(" AND MedicalNo = '{0}'", txtBarcodeEntry.Text);
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(filterExpression).FirstOrDefault();

            string url = "";
            if (entity != null)
                url = GetResponseRedirectUrl(entity);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpUrl"] = url;
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vConsultVisit entity = e.Item.DataItem as vConsultVisit;
                HtmlGenericControl divDischargeDate = e.Item.FindControl("divDischargeDate") as HtmlGenericControl;

                if (entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                    divDischargeDate.Style.Add("display", "none");
                else
                    divDischargeDate.InnerHtml = string.Format("{0} : {1} {2}", GetLabel("Pulang"), entity.DischargeDateInString, entity.DischargeTime);

                if (entity.DepartmentID == Constant.Facility.EMERGENCY)
                {
                    HtmlTableCell tdIndicator = e.Item.FindControl("tdIndicator") as HtmlTableCell;
                    if (!String.IsNullOrEmpty(entity.TriageColor))
                    {
                        tdIndicator.Style.Add("background-color", entity.TriageColor);
                    }
                }

                if (entity.GCVisitStatus != Constant.VisitStatus.OPEN)
                {
                    HtmlGenericControl divChiefComplaint = e.Item.FindControl("divChiefComplaint") as HtmlGenericControl;
                    vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID)).FirstOrDefault();
                    divChiefComplaint.InnerHtml = oChiefComplaint != null ? "X" : "";

                    HtmlGenericControl divDiagnosis = e.Item.FindControl("divDiagnosis") as HtmlGenericControl;
                    vPatientDiagnosis oDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID)).FirstOrDefault();
                    divDiagnosis.InnerHtml = oDiagnosis != null ? "X" : "";
                }

                if (entity.DepartmentID == Constant.Facility.OUTPATIENT)
                {
                    HtmlImage imgPatientSatisfactionLevelImageUri = (HtmlImage)e.Item.FindControl("imgPatientSatisfactionLevelImageUri");
                    imgPatientSatisfactionLevelImageUri.Src = ResolveUrl(string.Format("~/Libs/Images/Status/PatientStatus_{0}.png", entity.PatientSatisfactionLevel));

                }
            }
        }
    }
}