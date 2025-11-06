using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GridPatientImagingVisitCtl1 : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        public void InitializeControl()
        {
            BindGridView(1, true, ref PageCount);
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
                int rowCount = BusinessLayer.GetvImagingConsultVisitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vImagingConsultVisit> lstEntity = BusinessLayer.GetvImagingConsultVisitList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "VisitID");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vImagingConsultVisit entity = (vImagingConsultVisit)e.Item.DataItem;
                HtmlInputImage imgOrderStatus = e.Item.FindControl("imgOrderStatus") as HtmlInputImage;
                HtmlGenericControl divOrderStatus = e.Item.FindControl("divOrderStatus") as HtmlGenericControl;

                // Check for Outstanding Order
                string filterExpression = string.Format("RegistrationID = {0}", entity.RegistrationID);
                vRegistrationOutstandingInfo lstInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(filterExpression).FirstOrDefault();
                bool outstanding = (lstInfo.ServiceOrder + lstInfo.PrescriptionOrder + lstInfo.PrescriptionReturnOrder + lstInfo.LaboratoriumOrder + lstInfo.RadiologiOrder + lstInfo.OtherOrder > 0);
                if (!outstanding)
                {
                    divOrderStatus.Style.Add("display", "none");
                }
                else
                {
                    divOrderStatus.Style.Add("display", "");
                }

            }
        }

        protected string GetLabel(string code)
        {
            return ((BasePageRegisteredPatient)Page).GetLabel(code);
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnVisitID.Value != "")
            {
                string transactionInfo = string.Format("{0}|{1}", hdnVisitID.Value, hdnTransactionID.Value);
                ((BasePageRegisteredPatient)Page).OnGrdRowClick(transactionInfo);
            }
        }
    }
}