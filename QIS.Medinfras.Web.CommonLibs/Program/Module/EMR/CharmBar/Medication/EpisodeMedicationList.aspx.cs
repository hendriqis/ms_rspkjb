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
    public partial class EpisodeMedicationList : BasePage
    {
        protected int PageCount = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdnVisitIDCBCtl.Value = AppSession.RegisteredPatient.VisitID.ToString();
                hdnRegistrationIDCBCtl.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
                hdnPhysicianIDCBCtl.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

                List<Variable> lstDisplay = new List<Variable>() { new Variable() { Code = "All", Value = "0" }
                , new Variable() { Code = "Obat Oral/Supposituria/Topikal", Value = "1" }, new Variable() { Code = "Obat Injeksi", Value = "2" }};
                Methods.SetComboBoxField(cboDisplay, lstDisplay, "Code", "Value");
                cboDisplay.Value = hdnDisplayMode.Value;

                List<Variable> lstStatus = new List<Variable>() { 
                new Variable() { Code = "All", Value = "0" }, 
            new Variable() { Code = "Active", Value = "1" }, 
            new Variable() { Code = "Stop", Value = "2" }};
                Methods.SetComboBoxField(cboMedicationStatus, lstStatus, "Code", "Value");
                cboMedicationStatus.Value = hdnMedicationStatus.Value;

                BindGridView(1, true, ref PageCount);
            }
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

                List<PatientMedicationSummary> lstEntity = BusinessLayer.GetPatientMedicationSummaryList(AppSession.RegisteredPatient.RegistrationID, displayMode, cboMedicationStatus.Value.ToString());

                lvwView.DataSource = lstEntity;
                lvwView.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);                
            }
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
    }
}