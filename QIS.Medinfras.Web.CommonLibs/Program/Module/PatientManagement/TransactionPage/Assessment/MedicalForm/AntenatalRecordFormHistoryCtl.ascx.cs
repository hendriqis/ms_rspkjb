using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AntenatalRecordFormHistoryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnAntenatalRecordID.Value = param;
                BindGridView(1, true, ref PageCount);
            }
        }

        #region Antenatal History
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("AntenatalID = '{0}'", hdnAntenatalRecordID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvAntenatalRecordHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            filterExpression += " ORDER BY HistoryID DESC";

            List<vAntenatalRecordHistory> lstEntity = BusinessLayer.GetvAntenatalRecordHistoryList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex);
            grdAntenatalRecordHistory.DataSource = lstEntity;
            grdAntenatalRecordHistory.DataBind();
        }

        protected void cbpViewAntenatalHistory_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
        #endregion
    }
}