using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class InterfaceJournalDtViewCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnTransactionType.Value = param;

            TransactionType entity = BusinessLayer.GetTransactionType(param);
            txtTransactionName.Text = entity.TransactionName;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("TransactionCode = '{0}'", hdnTransactionType.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvInterfaceJournalSettingDtCustomRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vInterfaceJournalSettingDtCustom> lstEntity = BusinessLayer.GetvInterfaceJournalSettingDtCustomList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "Position");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}