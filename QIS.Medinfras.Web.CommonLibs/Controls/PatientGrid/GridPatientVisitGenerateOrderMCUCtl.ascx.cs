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
    public partial class GridPatientVisitGenerateOrderMCUCtl : System.Web.UI.UserControl
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
                int rowCount = BusinessLayer.GetvConsultVisitGenerateOrderMCURowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST_2);
            }

            string orderBy = "VisitID";

            switch (AppSession.UserLogin.DepartmentID)
            {
                case Constant.Facility.EMERGENCY:
                    orderBy = AppSession.RegistrationOrderBy_ER == "1" ? "VisitID" : "QueueNo";
                    break;
                case Constant.Facility.MEDICAL_CHECKUP:
                    orderBy = AppSession.RegistrationOrderBy_MC == "1" ? "VisitID" : "QueueNo";
                    break;
                case Constant.Facility.DIAGNOSTIC:
                    orderBy = AppSession.RegistrationOrderBy_MD == "1" ? "VisitID" : "QueueNo";
                    break;
                case Constant.Facility.PHARMACY:
                    orderBy = AppSession.RegistrationOrderBy_PH == "1" ? "VisitID" : "QueueNo";
                    break;
                default: 
                    break;
            }

            orderBy += ", IsMainPackage DESC, ConsultVisitItemPackageID";

            List<vConsultVisitGenerateOrderMCU> lstEntity = BusinessLayer.GetvConsultVisitGenerateOrderMCUList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST_2, pageIndex, orderBy);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vConsultVisitGenerateOrderMCU entity = e.Item.DataItem as vConsultVisitGenerateOrderMCU;

            }
        }

        protected string GetLabel(string code)
        {
            return ((BasePageRegisteredPatient)Page).GetLabel(code);
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnParamClick.Value != "")
            {
                ((BasePageRegisteredPatient)Page).OnGrdRowClick(hdnParamClick.Value);
            }
        }
    }
}