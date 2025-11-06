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
    public partial class SubLedgerDtViewCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        private void CreateGridColumn()
        {
            grdView.Columns.Add(CreateColumn("ID", hdnIDFieldName.Value, "keyField"));
            grdView.Columns.Add(CreateColumn("Code", hdnCodeFieldName.Value, "", 150));
            grdView.Columns.Add(CreateColumn("Name", hdnDisplayFieldName.Value, ""));

        }

        private BoundField CreateColumn(string title, string dataField, string cssClass, int width = 0)
        {
            BoundField field = new BoundField();
            field.HeaderText = title;
            field.DataField = dataField;
            if (cssClass != "")
                field.HeaderStyle.CssClass = field.ItemStyle.CssClass = cssClass;
            if (width > 0)
                field.HeaderStyle.Width = new Unit(width);
            return field;
        }

        public override void InitializeDataControl(string param)
        {
            hdnSubLedgerID.Value = param;

            vSubLedgerHd entity = BusinessLayer.GetvSubLedgerHdList(string.Format("SubLedgerID = {0}", param))[0];
            txtSubLedgerName.Text = string.Format("{0} - {1}", entity.SubLedgerCode, entity.SubLedgerName);
            hdnSubLedgerTypeID.Value = entity.SubLedgerTypeID.ToString();
            hdnTableName.Value = entity.TableName;
            hdnFilterExpression.Value = entity.FilterExpression.Replace("@SubLedgerID", param);
            hdnIDFieldName.Value = entity.IDFieldName;
            hdnCodeFieldName.Value = entity.CodeFieldName;
            hdnDisplayFieldName.Value = entity.DisplayFieldName;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                CreateGridColumn();
                string filterExpression = hdnFilterExpression.Value;
                if (isCountPageCount)
                {
                    string result = string.Format("SELECT COUNT(*) FROM {0} ", hdnTableName.Value);
                    if (filterExpression != null && filterExpression.Trim().Length > 0)
                        result += string.Format("WHERE {0}", string.Format(filterExpression));

                    ctx.CommandText = result;
                    DataRow row = DaoBase.GetDataRow(ctx);
                    int rowCount = Convert.ToInt32(row.ItemArray.GetValue(0));
                    pageCount = Helper.GetPageCount(rowCount, 8);
                }
                ctx.CommandText = Select(hdnTableName.Value, filterExpression, 8, pageIndex, "");
                DataTable dataTable = DaoBase.GetDataTable(ctx);
                grdView.DataSource = dataTable;
                grdView.DataBind();
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
            return string.Format("SELECT * FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY {0}) - 1 as row FROM {1}{4}) a WHERE a.row >= {2} and a.row < {3}", orderByExpression, tableName, startIndex, endIndex, filterExpression);
            //return string.Format("SELECT * FROM {0} ", _tableName);
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