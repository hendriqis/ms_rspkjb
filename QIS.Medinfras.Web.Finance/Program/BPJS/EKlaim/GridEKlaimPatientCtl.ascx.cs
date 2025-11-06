using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class GridEKlaimPatientCtl : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        public void InitializeControl()
        {
            BindGridView(1, true, ref PageCount);
        }

        protected void cbpViewEKlaim_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
            string[] paramFilter = ((BasePageRegisteredPatient)Page).GetFilterExpression().Split('|');
            string filterExpression = paramFilter[0];
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRegistrationBPJS5RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vRegistrationBPJS5> lstEntity = BusinessLayer.GetvRegistrationBPJS5List(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, paramFilter[1]);
            lvwViewEKlaim.DataSource = lstEntity;
            lvwViewEKlaim.DataBind();
        }

        protected void lvwViewEKlaim_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vRegistrationBPJS5 entity = e.Item.DataItem as vRegistrationBPJS5;

                HtmlGenericControl divHasDiagnoseClaim = e.Item.FindControl("divHasDiagnoseClaim") as HtmlGenericControl;
                HtmlGenericControl divHasGrouperStage1 = e.Item.FindControl("divHasGrouperStage1") as HtmlGenericControl;
                HtmlGenericControl divHasFinalClaim = e.Item.FindControl("divHasFinalClaim") as HtmlGenericControl;
                HtmlGenericControl divHasSentClaim = e.Item.FindControl("divHasSentClaim") as HtmlGenericControl;

                if (entity.CountClaimDiagnose > 0)
                {
                    divHasDiagnoseClaim.InnerHtml = "X";
                    divHasDiagnoseClaim.Style.Add("color", "blue");
                }
                else
                {
                    divHasDiagnoseClaim.InnerHtml = "O";
                    divHasDiagnoseClaim.Style.Add("color", "red");
                }

                if (entity.GrouperTypeClaim != null && entity.GrouperTypeClaim != "")
                {
                    divHasGrouperStage1.InnerHtml = "X";
                    divHasGrouperStage1.Style.Add("color", "blue");
                }
                else
                {
                    divHasGrouperStage1.InnerHtml = "O";
                    divHasGrouperStage1.Style.Add("color", "red");
                }

                if (entity.EKlaimFinalBy != null && entity.EKlaimFinalBy != 0)
                {
                    divHasFinalClaim.InnerHtml = "X";
                    divHasFinalClaim.Style.Add("color", "blue");
                }
                else
                {
                    divHasFinalClaim.InnerHtml = "O";
                    divHasFinalClaim.Style.Add("color", "red");
                }

                if (entity.EKlaimSendOnlineBy != null && entity.EKlaimSendOnlineBy != 0)
                {
                    divHasSentClaim.InnerHtml = "X";
                    divHasSentClaim.Style.Add("color", "blue");
                }
                else
                {
                    divHasSentClaim.InnerHtml = "O";
                    divHasSentClaim.Style.Add("color", "red");
                }
            }
        }

        protected string GetLabel(string code)
        {
            return ((BasePageRegisteredPatient)Page).GetLabel(code);
        }

        protected void btnOpenEKlaim_Click(object sender, EventArgs e)
        {
            if (hdnRegistrationID.Value != "")
            {
                ((BasePageRegisteredPatient)Page).OnGrdRowClick(hdnRegistrationID.Value);
            }
        }
    }
}