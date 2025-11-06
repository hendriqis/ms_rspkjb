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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ViewErrorLogList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.VIEW_ERROR_LOG;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            txtLogDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            BindGridView(CurrPage, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string date = Helper.GetDatePickerValue(txtLogDate).ToString("yyyyMMdd");
            string[] text = Helper.LoadTextFile(this, string.Format("log/{0}.txt", date));
            List<CLog> lstLog = new List<CLog>();

            int numRows = int.MaxValue;
            int startIndex = 0;
            int endIndex = startIndex + numRows;

            int textCount = text.Count();
            if (endIndex > textCount)
                endIndex = textCount;

            for (int i = 0; i < endIndex; ++i)
                lstLog.Add(new CLog(text[textCount - i - 1]));

            //if (isCountPageCount)
            //    pageCount = Helper.GetPageCount(textCount, Constant.GridViewPageSize.GRID_TEMP_MAX_FOR_ORDERING);

            grdView.DataSource = lstLog;
            grdView.DataBind();
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

        class CLog
        {
            public String Time { get; set; }
            public String ModuleID { get; set; }
            public String IPAddress { get; set; }
            public String ErrorMessage { get; set; }
            public String PageUrl { get; set; }
            public String ErrorDetail { get; set; }

            public CLog(string logData)
            {
                string replaceChar = "%^%";
                string[] param = logData.Split('|');
                Time = param[0];
                ModuleID = param[1];
                IPAddress = param[2];
                PageUrl = param[3];
                ErrorMessage = param[4];
                ErrorDetail = "<div>" + param[5].Replace(replaceChar, "</div><div>") + "</div>";
            }
        }
    }
}