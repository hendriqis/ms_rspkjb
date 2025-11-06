using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Data.Core.Dal;
using System.Data;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Tools
{
    public partial class DataMigrationProcessedDataCtl : BaseViewPopupCtl
    {
        MigrationConfigurationHd hd = null;
        protected int PageCount = 1;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            hd = BusinessLayer.GetMigrationConfigurationHdList(string.Format("ID = {0}", Request.QueryString["id"]))[0];
            string[] gridColumns = hd.GridColumns.Split('|');
            for (int i = 0; i < gridColumns.Length; ++i)
            {
                BoundField field = new BoundField();
                field.DataField = field.HeaderText = gridColumns[i];
                if (i == 0)
                    field.HeaderStyle.CssClass = field.ItemStyle.CssClass = "keyField";
                grdViewPopup.Columns.Add(field);
            }
            BoundField tf = new BoundField();
            tf.DataField = "MigrationStatusText";
            tf.HeaderText = "Migration Status";
            grdViewPopup.Columns.Add(tf);
        }

        public override void InitializeDataControl(string param)
        {
            cboMigrationStatus.SelectedIndex = 0;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                //string filterExpression = hdnFilterExpression.Value;
                //if (filterExpression != "")
                //    filterExpression += " AND ";
                //filterExpression += "MigrationStatus > 0";
                string filterExpression  = "";
                if (cboMigrationStatus.Value.ToString() == "-1")
                    filterExpression = "MigrationStatus > 0";
                else
                    filterExpression = string.Format("MigrationStatus = {0}", cboMigrationStatus.Value);
                if (isCountPageCount)
                {
                    string result = string.Format("SELECT COUNT(*) FROM {0} ", hd.FromTable);
                    if (filterExpression != null && filterExpression.Trim().Length > 0)
                        result += string.Format("WHERE {0}", string.Format(filterExpression));

                    ctx.CommandText = result;
                    DataRow row = DaoBase.GetDataRow(ctx);
                    int rowCount = Convert.ToInt32(row.ItemArray.GetValue(0));
                    pageCount = Helper.GetPageCount(rowCount, 14);
                }
                ctx.CommandText = Select(hd.FromTable, filterExpression, 14, pageIndex, "");
                DataTable dataTable = DaoBase.GetDataTable(ctx);
                grdViewPopup.DataSource = dataTable;
                grdViewPopup.DataBind();
            }
            finally
            {
                ctx.Close();
            }
        }

        public string Select(string tableName, string filterExpression, int numRows, int pageIndex, string orderByExpression)
        {
            if (filterExpression != "")
                filterExpression = " WHERE " + filterExpression;
            int startIndex = (pageIndex - 1) * numRows;
            int endIndex = pageIndex * numRows;
            if (orderByExpression == null || orderByExpression == "")
                orderByExpression = "(SELECT 0)";
            return string.Format("SELECT *, MigrationStatusText = CASE MigrationStatus WHEN 0 THEN 'Open' WHEN 1 THEN 'Transferred' WHEN 2 THEN 'Trashed' ELSE 'Failed' END FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY {0}) - 1 as row FROM {1}{4}) a WHERE a.row >= {2} and a.row < {3}", orderByExpression, tableName, startIndex, endIndex, filterExpression);
            //return string.Format("SELECT * FROM {0} ", _tableName);
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
    }
}