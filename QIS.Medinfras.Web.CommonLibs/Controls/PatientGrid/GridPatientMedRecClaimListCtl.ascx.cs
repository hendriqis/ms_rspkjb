using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Text;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GridPatientMedRecClaimListCtl : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        public void  InitializeControl()
        {
            BindGridView(1, true, ref PageCount);
        }

        protected void cbpView2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
            string filterExpression = ((BasePagePatientOrder)Page).GetFilterExpressionTestOrder();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisitCasemixRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_REORDER_ITEM);
            }

            List<vConsultVisitCasemix> lstEntity = BusinessLayer.GetvConsultVisitCasemixList(filterExpression, Constant.GridViewPageSize.GRID_REORDER_ITEM, pageIndex, "RegistrationDate, RegistrationNo");
            lvwView2.DataSource = lstEntity;
            lvwView2.DataBind();

            RowCountTable rc = new RowCountTable()
            {
                TotalRow = lstEntity.Count()
            };

            List<RowCountTable> rcList = new List<RowCountTable>();
            rcList.Add(rc);
            lvwViewCount.DataSource = rcList;
            lvwViewCount.DataBind();
        }

        protected void lvwView2_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vConsultVisitCasemix entity = e.Item.DataItem as vConsultVisitCasemix;

                HtmlGenericControl divDiagnosis = e.Item.FindControl("divDiagnosis") as HtmlGenericControl;
                if (entity.ClaimDiagnosisID != null && entity.ClaimDiagnosisID != "")
                {
                    divDiagnosis.InnerHtml = "X";
                    divDiagnosis.Style.Add("color", "blue");
                }
                else
                {
                    divDiagnosis.InnerHtml = "O";
                    divDiagnosis.Style.Add("color", "red");
                }
            }
        }

        protected string GetLabel(string code)
        {
            return ((BasePageRegisteredPatient)Page).GetLabel(code);
        }

        protected void btnOpenTransactionDt2_Click(object sender, EventArgs e)
        {
            if (hdnVisitID.Value != "")
            {
                ((BasePagePatientOrder)Page).OnGrdRowClickTestOrder(hdnVisitID.Value, "");
            }
        }

        private class RowCountTable
        {
            private Int32 totalRow;

            public Int32 TotalRow
            {
                get { return totalRow; }
                set { totalRow = value; }
            }
        }
    }
}