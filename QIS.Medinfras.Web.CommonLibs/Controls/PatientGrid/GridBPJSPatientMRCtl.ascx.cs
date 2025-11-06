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
    public partial class GridBPJSPatientMRCtl : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        public void  InitializeControl()
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
                int rowCount = BusinessLayer.GetvConsultVisitBPJSRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vConsultVisitBPJS> lstEntity = BusinessLayer.GetvConsultVisitBPJSList(filterExpression, 17, pageIndex);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            //if (e.Item.ItemType == ListViewItemType.DataItem)
            //{
            //    vConsultVisitBPJS entity = e.Item.DataItem as vConsultVisitBPJS;
            //    HtmlGenericControl divDischargeDate = e.Item.FindControl("divDischargeDate") as HtmlGenericControl;
            //    HtmlGenericControl divDischarge = e.Item.FindControl("divDischarge") as HtmlGenericControl;
            //    if (entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            //    {
            //        divDischargeDate.Style.Add("display", "none");
            //        divDischarge.InnerHtml = "O";
            //        divDischarge.Style.Add("color", "red");
            //    }
            //    else
            //    {
            //        divDischargeDate.InnerHtml = string.Format("{0} : {1} {2}", GetLabel("Pulang"), entity.DischargeDateInString, entity.DischargeTime);
            //        divDischarge.InnerHtml = "X";
            //        divDischarge.Style.Add("color", "blue");
            //    }
            //}
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