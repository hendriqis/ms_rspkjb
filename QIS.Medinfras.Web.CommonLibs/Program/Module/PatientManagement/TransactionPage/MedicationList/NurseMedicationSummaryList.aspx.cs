using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using System.Drawing;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NurseMedicationSummaryList : BasePageTrx
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;
        private string refreshGridInterval = "10";

        protected override void InitializeDataControl()
        {
            string[] paramInfo = Page.Request.QueryString["id"].Split('|');
            if (paramInfo[0] != null)
            {
                deptType = paramInfo[0];
                hdnDisplayMode.Value = paramInfo[1];
                if (paramInfo.Length >= 3)
                    menuType = paramInfo[2];
                else
                    menuType = string.Empty;
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnPhysicianID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
            hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();

            List<Variable> lstDisplay = new List<Variable>() { new Variable() { Code = "All", Value = "0" }
                , new Variable() { Code = "Obat Oral/Supposituria/Topikal", Value = "1" }, new Variable() { Code = "Obat Injeksi", Value = "2" }};
            Methods.SetComboBoxField(cboDisplay, lstDisplay, "Code", "Value");
            cboDisplay.Value = hdnDisplayMode.Value;


            hdnUserParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnUserParamedicType.Value = AppSession.UserLogin.GCParamedicMasterType;


            List<Variable> lstStatus = new List<Variable>() { 
                new Variable() { Code = "All", Value = "0" }, 
            new Variable() { Code = "Active", Value = "1" }, 
            new Variable() { Code = "Stop", Value = "2" }};
            Methods.SetComboBoxField(cboMedicationStatus, lstStatus, "Code", "Value");
            cboMedicationStatus.Value = "1";

            BindGridView(1, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            try
            {
                string filterExpression = "";//hdnFilterExpression.Value;
                if (filterExpression != "")
                    filterExpression += " AND ";
                String TransactionStatus = String.Format("'{0}','{1}'", Constant.TestOrderStatus.OPEN, Constant.TestOrderStatus.CANCELLED);
                filterExpression += string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID);

                string displayMode = cboDisplay.Value.ToString();
                List<PatientMedicationSummary> lstEntity = BusinessLayer.GetPatientMedicationSummaryList(AppSession.RegisteredPatient.RegistrationID, displayMode,cboMedicationStatus.Value.ToString());

                lvwView.DataSource = lstEntity;
                lvwView.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);                
            }
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                PatientMedicationSummary entity = e.Item.DataItem as PatientMedicationSummary;
                if (entity.IsUsingUDD)
                {
                    HtmlImage imgStatusImageUri = (HtmlImage)e.Item.FindControl("imgStatusImageUri");
                    if (Convert.ToDecimal(entity.cfRemainingQuantity)>0)
                        imgStatusImageUri.Src = ResolveUrl(string.Format("~/Libs/Images/Status/udd_wip.png"));
                    else
                        imgStatusImageUri.Src = ResolveUrl(string.Format("~/Libs/Images/Status/udd_finish.png"));
                }

                if (cboMedicationStatus.Value.ToString() != "2")
                {

                    HtmlTableRow row = (HtmlTableRow)e.Item.FindControl("trRow");

                    if (row != null)
                    {
                        if (entity.EndDate.ToString(Constant.FormatString.DATE_FORMAT_112) == DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112))
                        {
                            HtmlControl control = row.FindControl("lblEndDate") as HtmlControl;
                            if (control != null)
                            {
                                control.Attributes.Add("class", "blink-alert");
                            }
                        }
                    }
                }
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            if (string.IsNullOrEmpty(menuType))
            {
                return Constant.MenuCode.Inpatient.MEDICATION_SUMMARY_WITH_PROCESS;
            }
            else
            {
                return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_MEDICATION_SUMMARY_WITH_PROCESS;
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;

            try
            {

            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            finally
            {

            }
            return result;
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