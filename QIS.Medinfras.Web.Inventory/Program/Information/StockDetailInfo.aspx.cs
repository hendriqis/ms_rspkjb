using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxPivotGrid;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class StockDetailInfo : BasePageTrx
    {
        protected int PageCount = 0;
        protected string filterExpressionLocation = "";   
     
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.STOCK_DETAIL_INFO;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            filterExpressionLocation = string.Format("{0};{1};;", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
            txtDateFrom.Text = DateTime.Now.AddDays(-7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            txtStartTime.Text = "00:00";
            txtEndTime.Text = "23:59";

            BindGridView(1, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref PageCount);
                    result = "refresh|" + PageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public string OnGetFilterExpression()
        {
            return Request.Form[hdnFilterExpression.UniqueID];
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (hdnLocationID.Value != "")
            {
                string startTime = "00:00:00";
                string endTime = "23:59:59";
                if (chkIsFilterByTime.Checked)
                {
                    startTime = string.Format("{0}:00", txtStartTime.Text);
                    endTime = string.Format("{0}:59", txtEndTime.Text);
                }

                string startDateTime = string.Format("{0} {1}:00", Helper.GetDatePickerValue(txtDateFrom.Text).ToString("yyyyMMdd"), txtStartTime.Text);
                string endDateTime = string.Format("{0} {1}:59", Helper.GetDatePickerValue(txtDateTo.Text).ToString("yyyyMMdd"), txtEndTime.Text);

                hdnFilterExpression.Value = string.Format("LocationID = {0} AND CreatedDate BETWEEN Convert(DATETIME,'{1}',112) AND Convert(DATETIME,'{2}',112)", hdnLocationID.Value, startDateTime, endDateTime);

                if (isCountPageCount)
                {
                    string filterExpression = string.Format("LocationID = {0} AND ItemName1 LIKE '%{1}%' AND IsDeleted = 0", hdnLocationID.Value, txtItemName.Text);
                    int rowCount = BusinessLayer.GetvItemBalanceRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_ITEM);
                }

                List<GetItemMovementPerPeriodeDetail> lstEntity = BusinessLayer.GetItemMovementPerPeriodeDetail(string.Format("{0}|{1}", startDateTime, endDateTime), Convert.ToInt32(hdnLocationID.Value), txtItemName.Text, pageIndex, Constant.GridViewPageSize.GRID_ITEM);
                lvwView.DataSource = lstEntity;
                lvwView.DataBind();
            }
        }
    }
}