using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Common;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AntrolMessageLog : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            switch (hdnModuleID.Value)
            {
                case Constant.Module.SYSTEM_SETUP:
                    return Constant.MenuCode.SystemSetup.BPJS_AntrianOnline;
                default:
                    return Constant.MenuCode.SystemSetup.BPJS_AntrianOnline;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetRefreshGridInterval()
        {
            return AppSession.RefreshGridInterval;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnModuleID.Value = Page.Request.QueryString["id"];
            }

            txtFromDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(CurrPage, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string fromDate = Helper.GetDatePickerValue(txtFromDate).ToString("yyyyMMdd");
            string toDate = Helper.GetDatePickerValue(txtToDate).ToString("yyyyMMdd");
            String filterExpression = String.Format("(RegistrationDate BETWEEN '{0}' AND '{1}') AND GCRegistrationStatus != '{2}'", fromDate, toDate, Constant.VisitStatus.CANCELLED);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvBPJSTaskLogRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += String.Format(" AND {0} ", hdnFilterExpressionQuickSearch.Value);
            }

            List<vBPJSTaskLog> lstEntity = BusinessLayer.GetvBPJSTaskLogList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "RegistrationNo").GroupBy(g => g.RegistrationID).Select(s => s.FirstOrDefault()).ToList();
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vBPJSTaskLog obj = (vBPJSTaskLog)e.Row.DataItem;
                Repeater rptLaboratoryDt = (Repeater)e.Row.FindControl("rptTaskID");
                List<vBPJSTaskLog> lstObj = BusinessLayer.GetvBPJSTaskLogList(string.Format("RegistrationID = {0} ORDER BY LogDateTime, RegistrationID, TaskID", obj.RegistrationID));
                rptLaboratoryDt.DataSource = lstObj;
                rptLaboratoryDt.DataBind();
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

        protected void cbpSendToJKN_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');

            OnResendToMedinfrasAPI_MobileJKN(param[0], param[1], ref errMessage);
            if (!string.IsNullOrEmpty(errMessage))
            {
                result = "failed|" + errMessage;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = hdnMRN.Value;
        }

        private void OnResendToMedinfrasAPI_MobileJKN(string registrationID, string taskID, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            BPJSTaskLogDao entityDao = new BPJSTaskLogDao(ctx);
            try
            {
                List<BPJSTaskLog> lst = BusinessLayer.GetBPJSTaskLogList(String.Format("RegistrationID = {0}", registrationID));

                foreach (BPJSTaskLog b in lst.Where(w => w.GCTaskLogStatus == "X501^003").ToList())
                {
                    b.GCTaskLogStatus = "X501^001";
                    entityDao.Update(b);
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.ToString();
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
        }
    }
}