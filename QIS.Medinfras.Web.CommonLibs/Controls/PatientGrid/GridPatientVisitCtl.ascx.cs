using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GridPatientVisitCtl : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        public void InitializeControl()
        {
            loadSetpar(); 
            BindGridView(1, true, ref PageCount);
        }
        private void loadSetpar()
        {
            List<SettingParameterDt> setPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN('{0}') AND HealthcareID='001' ", Constant.SettingParameter.MC_MENGGUNAKAN_DEKSTOP_MCU));
            if (setPar.Count > 0)
            {
                string MCUDesktop = setPar.Where(p => p.ParameterCode == Constant.SettingParameter.MC_MENGGUNAKAN_DEKSTOP_MCU).FirstOrDefault().ParameterValue;
                hdnIsDesktopMCU.Value = MCUDesktop; 

            }
        }
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ((BasePageRegisteredPatient)Page).LoadAllWords();
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
        
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ((BasePageRegisteredPatient)Page).GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisit9RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST_2);
            }

            string orderBy = "VisitID";

            switch (AppSession.UserLogin.ModuleID)
            {
                case Constant.Module.EMERGENCY:
                    orderBy = AppSession.RegistrationOrderBy_ER == "1" ? "VisitID" : "QueueNo";
                    break;
                case Constant.Module.MEDICAL_CHECKUP:
                    orderBy = AppSession.RegistrationOrderBy_MC == "1" ? "VisitID" : "QueueNo";
                    break;
                case Constant.Module.LABORATORY:
                    orderBy = AppSession.RegistrationOrderBy_MD == "1" ? "VisitID" : "QueueNo";
                    break;
                case Constant.Module.IMAGING:
                    orderBy = AppSession.RegistrationOrderBy_MD == "1" ? "VisitID" : "QueueNo";
                    break;
                case Constant.Module.MEDICAL_DIAGNOSTIC:
                    orderBy = AppSession.RegistrationOrderBy_MD == "1" ? "VisitID" : "QueueNo";
                    break;
                case Constant.Module.PHARMACY:
                    orderBy = AppSession.RegistrationOrderBy_PH == "1" ? "VisitID" : "QueueNo";
                    break;
                default: 
                    break;
            }

            List<vConsultVisit9> lstEntity = BusinessLayer.GetvConsultVisit9List(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST_2, pageIndex, orderBy);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vConsultVisit9 entity = e.Item.DataItem as vConsultVisit9;
                Healthcare entityH = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
                HtmlImage imgPatientSatisfactionLevelImageUri = (HtmlImage)e.Item.FindControl("imgPatientSatisfactionLevelImageUri");
                HtmlInputImage imgOrderStatus = e.Item.FindControl("imgOrderStatus") as HtmlInputImage;
                HtmlGenericControl divOrderStatus = e.Item.FindControl("divOrderStatus") as HtmlGenericControl;
                HtmlGenericControl tdIsHasBillingNotes1 = e.Item.FindControl("tdIsHasBillingNotes1") as HtmlGenericControl;
                HtmlGenericControl divBusinessPartnerName = e.Item.FindControl("divBusinessPartnerName") as HtmlGenericControl;

                if (entity.DepartmentID == Constant.Facility.EMERGENCY)
                {
                    HtmlTableCell tdIndicator = e.Item.FindControl("tdIndicator") as HtmlTableCell;
                    if (!String.IsNullOrEmpty(entity.TriageColor))
                    {
                        tdIndicator.Style.Add("background-color", entity.TriageColor);
                    }
                }

        
                if (hdnIsDesktopMCU.Value == "1") {
                    tdIsHasBillingNotes1.Style["display"] = "block";

                }

                if (entity.CoverageTypeID != null && entity.CoverageTypeID != 0)
                {
                    if (entity.CoverageTypeCode == "R001" && entityH.Initial == "NHS")
                    {
                        divBusinessPartnerName.InnerHtml = string.Format("{0} ({1})", entity.BusinessPartnerName, entity.CoverageTypeName);
                    }
                    else
                    {
                        divBusinessPartnerName.InnerHtml = string.Format("{0}", entity.BusinessPartnerName);
                    }
                }
                else
                {
                    divBusinessPartnerName.InnerHtml = string.Format("{0}", entity.BusinessPartnerName);
                }
                // Check for Outstanding Order
                //string filterExpression = string.Format("RegistrationID = {0}", entity.RegistrationID);
                //vRegistrationOutstandingInfo lstInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(filterExpression).FirstOrDefault();
                //bool outstanding = (lstInfo.ServiceOrder + lstInfo.PrescriptionOrder + lstInfo.PrescriptionReturnOrder + lstInfo.LaboratoriumOrder + lstInfo.RadiologiOrder + lstInfo.OtherOrder > 0);
                //if (!outstanding)
                //{
                //    divOrderStatus.Style.Add("display", "none");
                //}
                //else
                //{
                //    divOrderStatus.Style.Add("display", "");
                //}
            }
        }

        protected string GetLabel(string code)
        {
            return ((BasePageRegisteredPatient)Page).GetLabel(code);
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnTransactionNo.Value != "")
            {
                ((BasePageRegisteredPatient)Page).OnGrdRowClick(hdnTransactionNo.Value);
            }
        }
    }
}