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
    public partial class CheckGridPatientVisitCtl : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        public void InitializeControl()
        {
            BindGridView(1, true, ref PageCount);
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ((BasePageCheckRegisteredPatient)Page).LoadAllWords();
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
            string filterExpression = ((BasePageCheckRegisteredPatient)Page).GetFilterExpression();
            hdnFilterExpressionGridCtl.Value = filterExpression;

            int rowCount = 0;
            if (isCountPageCount)
            {
                rowCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_MR_LIST);
            }

            List<vConsultVisit1> lstEntity = BusinessLayer.GetvConsultVisit1List(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_MR_LIST, pageIndex,"RegistrationNo DESC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            List<vConsultVisit1> lstEntity2 = BusinessLayer.GetvConsultVisit1List(filterExpression);
            RowCountTable rc = new RowCountTable()
            {
                TotalRow = lstEntity.Count()
            };

            List<RowCountTable> rcList = new List<RowCountTable>();
            rcList.Add(rc);

            lvwViewCount.DataSource = rcList;
            lvwViewCount.DataBind();
        }

        protected string GetLabel(string code)
        {
            return ((BasePageCheckRegisteredPatient)Page).GetLabel(code);
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem dataitem = (ListViewDataItem)e.Item;
                if (!Convert.ToBoolean(DataBinder.Eval(dataitem.DataItem, "IsNewPatient")))
                {
                    System.Web.UI.HtmlControls.HtmlTableRow tr = (System.Web.UI.HtmlControls.HtmlTableRow)dataitem.FindControl("trItem");
                    //tr.BgColor = System.Drawing.Color.AliceBlue.ToString();
                    tr.Attributes.Add("class", "LvColor");
                }
                //if (!string.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(dataitem.DataItem, "RegistrationMRFileStatus"))))
                //{
                //    System.Web.UI.HtmlControls.HtmlInputCheckBox chk = (System.Web.UI.HtmlControls.HtmlInputCheckBox)dataitem.FindControl("chkIsHasPhysicianDiagnosis");
                //    chk.Visible = false;
                //}
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