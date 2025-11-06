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
    public partial class GridPatientSatuSehatSudahIntegrasiListCtl : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        public void InitializeControl()
        {
            if (AppSession.LastPagingMR1 != null)
            {
                hdnLastPagging.Value = AppSession.LastPagingMR1.PageID.ToString();
            }
            BindGridView(1, true, ref PageCount);
        }

        protected void cbpViewSudahIntegrasi_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
            string filterExpression = ((BasePagePatientOrderSatuSehatSudahIntegrasi)Page).GetFilterExpressionSatuSehatSudahIntegrasi();

            int rowCount = 0;
            if (isCountPageCount)
            {
                rowCount = BusinessLayer.GetvConsultVisitSatuSehatRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_ITEM);
            }

            LastPagingMR1 mr = new LastPagingMR1();
            mr.PageID = pageIndex;
            AppSession.LastPagingMR1 = mr;

            List<vConsultVisitSatuSehat> lstEntity = BusinessLayer.GetvConsultVisitSatuSehatList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            RowCountTable rc = new RowCountTable()
            {
                TotalRow = lstEntity.Count()
            };

            List<RowCountTable> rcList = new List<RowCountTable>();
            rcList.Add(rc);

            lvwViewCount.DataSource = rcList;
            lvwViewCount.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vConsultVisitSatuSehat entity = e.Item.DataItem as vConsultVisitSatuSehat;
                HtmlGenericControl divDischargeDate = e.Item.FindControl("divDischargeDate") as HtmlGenericControl;
                HtmlGenericControl divChiefComplaint = e.Item.FindControl("divChiefComplaint") as HtmlGenericControl;
                HtmlGenericControl divPDxID = e.Item.FindControl("divPDxID") as HtmlGenericControl;
                HtmlGenericControl divPhysicianDischarge = e.Item.FindControl("divPhysicianDischarge") as HtmlGenericControl;

                if (entity.ChiefComplaint != null && entity.ChiefComplaint != "")
                {
                    divChiefComplaint.InnerHtml = "X";
                    divChiefComplaint.Style.Add("color", "blue");
                }
                else
                {
                    divChiefComplaint.InnerHtml = "O";
                    divChiefComplaint.Style.Add("color", "red");
                }
                if (entity.CheckIsAllCodingRM == 1)
                {
                    divPDxID.InnerHtml = "X";
                    divPDxID.Style.Add("color", "blue");
                }
                else
                {
                    divPDxID.InnerHtml = "O";
                    divPDxID.Style.Add("color", "red");
                }
                //if (entity.FinalDiagnoseID != null && entity.FinalDiagnoseID != "")
                //{
                //    divPDxID.InnerHtml = "X";
                //    divPDxID.Style.Add("color", "blue");
                //}
                //else
                //{
                //    divPDxID.InnerHtml = "O";
                //    divPDxID.Style.Add("color", "red");
                //}

                if (entity.PhysicianDischargedDate != null && entity.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                {
                    divPhysicianDischarge.InnerHtml = "O";
                    divPhysicianDischarge.Style.Add("color", "red");
                }
                else
                {
                    divPhysicianDischarge.InnerHtml = "X";
                    divPhysicianDischarge.Style.Add("color", "blue");
                }
            }
        }

        protected string GetLabel(string code)
        {
            return ((BasePageRegisteredPatient)Page).GetLabel(code);
        }

        protected void btnOpenTransactionDt2_Click(object sender, EventArgs e)
        {
            if (hdnTransactionNo.Value != "")
            {
                ((BasePagePatientOrderSatuSehatSudahIntegrasi)Page).OnGrdRowClickSatuSehatSudahIntegrasi(hdnTransactionNo.Value, "", "");
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