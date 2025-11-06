using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;
using System.Reflection;
using System.Collections;
using QIS.Data.Core.Dal;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;

namespace QIS.Medinfras.Web.CommonLibs.Tools
{
    public partial class DataMigration : BasePage
    {
        MigrationConfigurationHd hd = null;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                hdnHeaderID.Value = Request.QueryString["id"];
            }

            hd = BusinessLayer.GetMigrationConfigurationHdList(string.Format("ID = {0}", Request.QueryString["id"]))[0];
            string[] gridColumns = hd.GridColumns.Split('|');
            for (int i = 0; i < gridColumns.Length; ++i)
            {
                BoundField field = new BoundField();
                field.DataField = field.HeaderText = gridColumns[i];
                if (i == 0)
                    field.HeaderStyle.CssClass = field.ItemStyle.CssClass = "keyField";
                else
                    txtSearchView.IntellisenseHints.Add(new CustomControl.QISIntellisenseHint { FieldName = gridColumns[i], Text = gridColumns[i] });
                grdView.Columns.Add(field);
            }
        }

        protected void SetControlEntrySetting(Control ctrl, ControlEntrySetting setting)
        {
            if (ctrl is ASPxEdit)
            {
                ASPxEdit ctl = ctrl as ASPxEdit;
                ctl.ValidationSettings.RequiredField.IsRequired = setting.IsRequired;
                ctl.ValidationSettings.RequiredField.ErrorText = "";
                ctl.ValidationSettings.CausesValidation = true;
                ctl.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.None;
                ctl.ValidationSettings.ErrorFrameStyle.Paddings.Padding = new System.Web.UI.WebControls.Unit(0);

                //if (setting.IsRequired)
                ctl.ValidationSettings.ValidationGroup = "mpEntry";
            }
            else if (ctrl is WebControl)
            {
                if (setting.IsRequired)
                {
                    Helper.AddCssClass(((WebControl)ctrl), "required");
                }
                ((WebControl)ctrl).Attributes.Add("validationgroup", "mpEntry");
            }
        }

        protected int PageCount = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (hdnIsSave.Value != "1" && hdnID.Value != "")
            {
                IDbContext ctx = DbFactory.Configure(true);
                try
                {
                    ctx.CommandText = string.Format("SELECT * FROM {0} WHERE ID = {1}", hd.FromTable, Request.Form[hdnEntryID.UniqueID]);
                    DataTable dataTable = DaoBase.GetDataTable(ctx);
                    dataRow = dataTable.Rows[0];
                    ctx.CommitTransaction();
                }
                catch
                {
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }

            List<MigrationConfigurationTableLink> lstTempMigrationTableLink = BusinessLayer.GetMigrationConfigurationTableLinkList(string.Format("HeaderID = {0}", hdnHeaderID.Value));

            List<MigrationConfigurationTableLink> lstTableLink = new List<MigrationConfigurationTableLink>();
            lstTableLink.Add(new MigrationConfigurationTableLink { LinkTableName = hd.ToTable, ColumnName = "" });

            List<MigrationConfigurationTableLink> lstTemp = lstTempMigrationTableLink.Where(p => p.TableName == hd.ToTable).ToList();
            foreach (MigrationConfigurationTableLink temp in lstTemp)
                lstTableLink.Add(temp);

            lstTemp = lstTempMigrationTableLink.Where(p => p.LinkTableName == hd.ToTable).ToList();
            foreach (MigrationConfigurationTableLink temp in lstTemp)
            {
                lstTableLink.Add(new MigrationConfigurationTableLink
                {
                    LinkTableName = temp.TableName,
                    ColumnName = temp.LinkTableColumn,
                    RepeaterFilterExpression = temp.RepeaterFilterExpression,
                    RepeaterTable = temp.RepeaterTable,
                    RepeaterIDValue = temp.RepeaterIDValue,
                    RepeaterLabelValue = temp.RepeaterLabelValue,
                    IsOneToMany = temp.IsOneToMany
                });
                List<MigrationConfigurationTableLink> lstTemp2 = lstTempMigrationTableLink.Where(p => p.LinkTableName == temp.TableName).ToList();
                foreach (MigrationConfigurationTableLink temp2 in lstTemp2)
                {
                    lstTableLink.Add(new MigrationConfigurationTableLink
                    {
                        LinkTableName = temp2.TableName,
                        ColumnName = temp2.LinkTableColumn,
                        RepeaterFilterExpression = temp2.RepeaterFilterExpression,
                        RepeaterTable = temp2.RepeaterTable,
                        RepeaterIDValue = temp2.RepeaterIDValue,
                        RepeaterLabelValue = temp2.RepeaterLabelValue,
                        IsOneToMany = temp2.IsOneToMany
                    });
                }
            }


            //List<MigrationConfigurationTableLink> lstTableLink = BusinessLayer.GetMigrationConfigurationTableLinkList(string.Format("HeaderID = {0}", hdnHeaderID.Value));
            //lstTableLink.Insert(0, new MigrationConfigurationTableLink { LinkTableName = hd.ToTable, ColumnName = "" });

            lstMigrationDt = BusinessLayer.GetMigrationConfigurationDtList(string.Format("HeaderID = {0}", hdnHeaderID.Value));

            rptEntry.DataSource = lstTableLink;
            rptEntry.DataBind();

            if (!Page.IsPostBack)
            {
                BindGridView(1, true, ref PageCount);
                /*List<MigrationConfigurationDt> lst = BusinessLayer.GetMigrationConfigurationDtList("HeaderID = 1");
                rptEntry.DataSource = lst;
                rptEntry.DataBind();*/
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                string filterExpression = hdnFilterExpression.Value;
                if (filterExpression != "")
                    filterExpression += " AND ";
                filterExpression += "MigrationStatus = 0";
                if (isCountPageCount)
                {
                    string result = string.Format("SELECT COUNT(*) FROM {0} ", hd.FromTable);
                    if (filterExpression != null && filterExpression.Trim().Length > 0)
                        result += string.Format("WHERE {0}", string.Format(filterExpression));

                    ctx.CommandText = result;
                    DataRow row = DaoBase.GetDataRow(ctx);
                    int rowCount = Convert.ToInt32(row.ItemArray.GetValue(0));
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
                ctx.CommandText = Select(hd.FromTable, filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "");
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

        public string Select(string tableName, string filterExpression)
        {
            string result = string.Format("SELECT * FROM {0} ", tableName);
            if (filterExpression != null && filterExpression.Trim().Length > 0)
            {
                result += string.Format("WHERE {0}", filterExpression);
            }
            return result;
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

        private DataRow dataRow = null;
        protected void cbpEntry_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {

        }

        private RepeaterItem GetRepeaterHd(string tableName, string linkTableColumn)
        {
            int count = rptEntry.Items.Count;
            for (int i = 0; i < count; ++i)
            {
                RepeaterItem itemHd = rptEntry.Items[i];
                HtmlInputHidden hdnCode = (HtmlInputHidden)itemHd.FindControl("hdnCode");
                HtmlInputHidden hdnLinkTableColumn = (HtmlInputHidden)itemHd.FindControl("hdnLinkTableColumn");
                if (hdnCode.Value == tableName && hdnLinkTableColumn.Value == linkTableColumn)
                    return itemHd;
            }
            return null;
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                List<MigrationConfigurationTableLink> lstTempMigrationTableLink = BusinessLayer.GetMigrationConfigurationTableLinkList(string.Format("HeaderID = {0}", hdnHeaderID.Value));

                List<MigrationConfigurationTableLink> lstTableLink = new List<MigrationConfigurationTableLink>();
                lstTableLink.Add(new MigrationConfigurationTableLink { LinkTableName = hd.ToTable, ColumnName = "" });

                List<MigrationConfigurationTableLink> lstTemp = lstTempMigrationTableLink.Where(p => p.TableName == hd.ToTable).ToList();
                foreach (MigrationConfigurationTableLink temp in lstTemp)
                    lstTableLink.Insert(0, temp);

                lstTemp = lstTempMigrationTableLink.Where(p => p.LinkTableName == hd.ToTable).ToList();
                foreach (MigrationConfigurationTableLink temp in lstTemp)
                {
                    lstTableLink.Add(new MigrationConfigurationTableLink
                    {
                        HeaderID = 0,
                        LinkTableName = temp.TableName,
                        TableName = temp.LinkTableName,
                        LinkTableColumn = temp.ColumnName,
                        ColumnName = temp.LinkTableColumn,
                        RepeaterFilterExpression = temp.RepeaterFilterExpression,
                        RepeaterTable = temp.RepeaterTable,
                        RepeaterIDValue = temp.RepeaterIDValue,
                        RepeaterLabelValue = temp.RepeaterLabelValue,
                        IsOneToMany = temp.IsOneToMany,
                        DtColumnID = temp.DtColumnID,
                        DtColumnValue = temp.DtColumnValue
                    });
                    List<MigrationConfigurationTableLink> lstTemp2 = lstTempMigrationTableLink.Where(p => p.LinkTableName == temp.TableName).ToList();
                    foreach (MigrationConfigurationTableLink temp2 in lstTemp2)
                    {
                        lstTableLink.Add(new MigrationConfigurationTableLink
                        {
                            HeaderID = 0,
                            LinkTableName = temp2.TableName,
                            TableName = temp2.LinkTableName,
                            LinkTableColumn = temp2.ColumnName,
                            ColumnName = temp2.LinkTableColumn,
                            RepeaterFilterExpression = temp2.RepeaterFilterExpression,
                            RepeaterTable = temp2.RepeaterTable,
                            RepeaterIDValue = temp2.RepeaterIDValue,
                            RepeaterLabelValue = temp2.RepeaterLabelValue,
                            IsOneToMany = temp2.IsOneToMany
                        });
                    }
                }

                IDbContext ctx = DbFactory.Configure(true);
                try
                {
                    List<LinkOtherTable> lstLinkOtherTable = new List<LinkOtherTable>();
                    int count = rptEntry.Items.Count;
                    int i = 0;
                    foreach (MigrationConfigurationTableLink tableLink in lstTableLink)
                    {
                        List<CTableCode> lstTableCode = new List<CTableCode>();
                        RepeaterItem itemHd = GetRepeaterHd(tableLink.LinkTableName, tableLink.ColumnName);
                        HtmlInputHidden hdnCode = (HtmlInputHidden)itemHd.FindControl("hdnCode");
                        HtmlInputHidden hdnLinkTableColumn = (HtmlInputHidden)itemHd.FindControl("hdnLinkTableColumn");
                        string tableName = hdnCode.Value;
                        string linkTableColumn = hdnLinkTableColumn.Value;

                        Repeater rptEntryDt = (Repeater)itemHd.FindControl("rptEntryDt");
                        if (tableLink.IsOneToMany)
                        {
                            foreach (RepeaterItem itemDt in rptEntryDt.Items)
                            {
                                StringBuilder sbInsert = new StringBuilder();
                                StringBuilder sbInsertColumn = new StringBuilder();
                                StringBuilder sbInsertValue = new StringBuilder();

                                HtmlInputHidden hdnIsRequired = (HtmlInputHidden)itemDt.FindControl("hdnIsRequired");
                                HtmlInputHidden hdnType = (HtmlInputHidden)itemDt.FindControl("hdnType");
                                HtmlInputHidden hdnColumn = (HtmlInputHidden)itemDt.FindControl("hdnColumn");
                                if (hdnType.Value == "6")
                                {
                                    CTableCode tableCode = new CTableCode();
                                    HtmlInputHidden hdnFormatCode = (HtmlInputHidden)itemDt.FindControl("hdnFormatCode");
                                    HtmlInputHidden hdnIDColumn = (HtmlInputHidden)itemDt.FindControl("hdnIDColumn");

                                    tableCode.FormatCode = hdnFormatCode.Value;
                                    tableCode.IDColumn = hdnIDColumn.Value;
                                    tableCode.ColumnName = hdnColumn.Value;

                                    lstTableCode.Add(tableCode);
                                }
                                else
                                {
                                    string text = "";
                                    if (hdnType.Value == "1")
                                    {
                                        TextBox txt = (TextBox)itemDt.FindControl("txtValue");
                                        text = Request.Form[txt.UniqueID];
                                        if (hdnIsRequired.Value == "False" && text == "")
                                            text = "NULL";
                                        else
                                            text = string.Format("\'{0}\'", text);
                                    }
                                    else if (hdnType.Value == "2")
                                    {
                                        ASPxComboBox cbo = (ASPxComboBox)itemDt.FindControl("cboNewValue");
                                        if (Request.Form[cbo.UniqueID] != "")
                                        {
                                            cbo.Text = Request.Form[cbo.UniqueID];
                                            text = string.Format("\'{0}\'", cbo.Value.ToString());
                                        }
                                        else
                                            text = "NULL";
                                    }
                                    else if (hdnType.Value == "3")
                                    {
                                        CheckBox chk = (CheckBox)itemDt.FindControl("chkValue");
                                        text = Request.Form[chk.UniqueID];
                                        if (text == "on")
                                            text = "1";
                                        else
                                            text = "0";
                                    }
                                    else if (hdnType.Value == "4")
                                    {
                                        TextBox txt = (TextBox)itemDt.FindControl("txtDteValue");
                                        text = Request.Form[txt.UniqueID];
                                        if (hdnIsRequired.Value == "False" && text == "")
                                            text = "NULL";
                                        else
                                        {
                                            DateTime dte = Helper.GetDatePickerValue(text);
                                            text = string.Format("\'{0}\'", dte.ToString("yyyyMMdd"));
                                        }
                                    }
                                    else if (hdnType.Value == "5")
                                    {
                                        HtmlInputHidden hdnSdNewID = (HtmlInputHidden)itemDt.FindControl("hdnSdNewID");
                                        text = Request.Form[hdnSdNewID.UniqueID];
                                        if (hdnIsRequired.Value == "False" && text == "")
                                            text = "NULL";
                                        else
                                            text = string.Format("\'{0}\'", text);
                                    }
                                    if (text != "NULL")
                                    {
                                        sbInsertColumn.Append(tableLink.DtColumnID).Append(", ").Append(tableLink.DtColumnValue);
                                        sbInsertValue.Append(string.Format("\'{0}\'", hdnColumn.Value));

                                        if (sbInsertValue.ToString() != "")
                                            sbInsertValue.Append(", ");
                                        sbInsertValue.Append(text);

                                        List<LinkOtherTable> lst = lstLinkOtherTable.Where(p => p.TableName == tableName).ToList();
                                        foreach (LinkOtherTable lnk in lst)
                                        {
                                            if (sbInsertColumn.ToString() != "")
                                                sbInsertColumn.Append(", ");
                                            sbInsertColumn.Append(lnk.ColumnName);

                                            if (sbInsertValue.ToString() != "")
                                                sbInsertValue.Append(", ");
                                            sbInsertValue.Append(lnk.Value);
                                        }


                                        sbInsert.Append("INSERT INTO ").Append(tableName).Append(" (").Append(sbInsertColumn).Append(") VALUES (").Append(sbInsertValue).Append(")");
                                        ctx.CommandText = sbInsert.ToString();
                                        DaoBase.ExecuteNonQuery(ctx);
                                    }
                                }
                            }
                        }
                        else
                        {
                            StringBuilder sbInsert = new StringBuilder();
                            StringBuilder sbInsertColumn = new StringBuilder();
                            StringBuilder sbInsertValue = new StringBuilder();
                            foreach (RepeaterItem itemDt in rptEntryDt.Items)
                            {
                                HtmlInputHidden hdnIsRequired = (HtmlInputHidden)itemDt.FindControl("hdnIsRequired");
                                HtmlInputHidden hdnType = (HtmlInputHidden)itemDt.FindControl("hdnType");
                                HtmlInputHidden hdnColumn = (HtmlInputHidden)itemDt.FindControl("hdnColumn");
                                if (hdnType.Value == "6")
                                {
                                    CTableCode tableCode = new CTableCode();
                                    HtmlInputHidden hdnFormatCode = (HtmlInputHidden)itemDt.FindControl("hdnFormatCode");
                                    HtmlInputHidden hdnIDColumn = (HtmlInputHidden)itemDt.FindControl("hdnIDColumn");

                                    tableCode.FormatCode = hdnFormatCode.Value;
                                    tableCode.IDColumn = hdnIDColumn.Value;
                                    tableCode.ColumnName = hdnColumn.Value;

                                    lstTableCode.Add(tableCode);
                                }
                                else
                                {
                                    if (sbInsertColumn.ToString() != "")
                                        sbInsertColumn.Append(", ");
                                    sbInsertColumn.Append(hdnColumn.Value);

                                    string text = "";
                                    if (hdnType.Value == "1")
                                    {
                                        TextBox txt = (TextBox)itemDt.FindControl("txtValue");
                                        text = Request.Form[txt.UniqueID];
                                        if (hdnIsRequired.Value == "False" && text == "")
                                            text = "NULL";
                                        else
                                            text = string.Format("\'{0}\'", text);
                                    }
                                    else if (hdnType.Value == "2")
                                    {
                                        ASPxComboBox cbo = (ASPxComboBox)itemDt.FindControl("cboNewValue");
                                        if (Request.Form[cbo.UniqueID] != "")
                                        {
                                            cbo.Text = Request.Form[cbo.UniqueID];
                                            text = string.Format("\'{0}\'", cbo.Value.ToString());
                                        }
                                        else
                                            text = "NULL";
                                    }
                                    else if (hdnType.Value == "3")
                                    {
                                        CheckBox chk = (CheckBox)itemDt.FindControl("chkValue");
                                        text = Request.Form[chk.UniqueID];
                                        if (text == "on")
                                            text = "1";
                                        else
                                            text = "0";
                                    }
                                    else if (hdnType.Value == "4")
                                    {
                                        TextBox txt = (TextBox)itemDt.FindControl("txtDteValue");
                                        text = Request.Form[txt.UniqueID];
                                        if (hdnIsRequired.Value == "False" && text == "")
                                            text = "NULL";
                                        else
                                        {
                                            DateTime dte = Helper.GetDatePickerValue(text);
                                            text = string.Format("\'{0}\'", dte.ToString("yyyyMMdd"));
                                        }
                                    }
                                    else if (hdnType.Value == "5")
                                    {
                                        HtmlInputHidden hdnSdNewID = (HtmlInputHidden)itemDt.FindControl("hdnSdNewID");
                                        text = Request.Form[hdnSdNewID.UniqueID];
                                        if (hdnIsRequired.Value == "False" && text == "")
                                            text = "NULL";
                                        else
                                            text = string.Format("\'{0}\'", text);
                                    }
                                    if (sbInsertValue.ToString() != "")
                                        sbInsertValue.Append(", ");
                                    sbInsertValue.Append(text);
                                }
                            }
                            List<LinkOtherTable> lst = lstLinkOtherTable.Where(p => p.TableName == tableName).ToList();
                            foreach (LinkOtherTable lnk in lst)
                            {
                                if (sbInsertColumn.ToString() != "")
                                    sbInsertColumn.Append(", ");
                                sbInsertColumn.Append(lnk.ColumnName);

                                if (sbInsertValue.ToString() != "")
                                    sbInsertValue.Append(", ");
                                sbInsertValue.Append(lnk.Value);
                            }

                            sbInsert.Append("INSERT INTO ").Append(tableName).Append(" (").Append(sbInsertColumn).Append(") VALUES (").Append(sbInsertValue).Append(")");
                            ctx.CommandText = sbInsert.ToString();
                            DaoBase.ExecuteNonQuery(ctx);

                            foreach (CTableCode tableCode in lstTableCode)
                            {
                                string command = string.Format("SELECT TOP(1) {0} FROM {1} ORDER BY {0} DESC", tableCode.IDColumn, tableName);
                                ctx.CommandText = command;
                                DataRow row = DaoBase.GetDataRow(ctx);
                                Int32 ID = Convert.ToInt32(row.ItemArray.GetValue(0));
                                String Code = Helper.GenerateCode(tableCode.FormatCode, ID);

                                StringBuilder sbUpdate = new StringBuilder();
                                ctx.CommandText = string.Format("UPDATE {0} SET {1} = '{2}' WHERE {3} = {4}", tableName, tableCode.ColumnName, Code, tableCode.IDColumn, ID);
                                DaoBase.ExecuteNonQuery(ctx);
                            }

                            if (i < count - 1)
                            {
                                LinkOtherTable lnk = new LinkOtherTable();

                                MigrationConfigurationTableLink tblLink = null;
                                if (linkTableColumn != "")
                                {
                                    tblLink = lstTableLink.FirstOrDefault(p => p.LinkTableName == tableName && p.ColumnName == linkTableColumn);
                                    string command = string.Format("SELECT TOP(1) {0} FROM {1} ORDER BY {0} DESC", tblLink.LinkTableColumn, tblLink.LinkTableName);
                                    ctx.CommandText = command;
                                    DataRow row = DaoBase.GetDataRow(ctx);
                                    lnk.Value = row.ItemArray.GetValue(0).ToString();
                                    lnk.TableName = tblLink.TableName;
                                    lnk.ColumnName = tblLink.ColumnName;
                                    lstLinkOtherTable.Add(lnk);

                                    tblLink = lstTableLink.FirstOrDefault(p => p.TableName == tableName && p.HeaderID == 0);
                                    if (tblLink != null)
                                    {
                                        lnk = new LinkOtherTable();
                                        command = string.Format("SELECT TOP(1) {0} FROM {1} ORDER BY {0} DESC", tblLink.ColumnName, tblLink.TableName);
                                        ctx.CommandText = command;
                                        row = DaoBase.GetDataRow(ctx);
                                        lnk.Value = row.ItemArray.GetValue(0).ToString();
                                        lnk.TableName = tblLink.LinkTableName;
                                        lnk.ColumnName = tblLink.LinkTableColumn;
                                        lstLinkOtherTable.Add(lnk);
                                    }
                                }
                                else
                                {
                                    tblLink = lstTableLink.FirstOrDefault(p => p.TableName == tableName && p.HeaderID == 0);
                                    string command = string.Format("SELECT TOP(1) {0} FROM {1} ORDER BY {0} DESC", tblLink.ColumnName, tblLink.TableName);
                                    ctx.CommandText = command;
                                    DataRow row = DaoBase.GetDataRow(ctx);
                                    lnk.Value = row.ItemArray.GetValue(0).ToString();
                                    lnk.TableName = tblLink.LinkTableName;
                                    lnk.ColumnName = tblLink.LinkTableColumn;
                                    lstLinkOtherTable.Add(lnk);
                                }

                            }
                        }
                        i++;
                    }

                    ctx.CommandText = string.Format("UPDATE {0} SET MigrationStatus = 1 WHERE ID = {1}", hd.FromTable, Request.Form[hdnEntryID.UniqueID]);
                    DaoBase.ExecuteNonQuery(ctx);

                    result += "success";
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    ctx.RollBackTransaction();
                    if (hdnIsSaveAll.Value == "0")
                    {
                        result += "fail|" + ex.Message;
                    }
                    else
                    {
                        ctx.CommandText = string.Format("UPDATE {0} SET MigrationStatus = 3 WHERE ID = {1}", hd.FromTable, Request.Form[hdnEntryID.UniqueID]);
                        DaoBase.ExecuteNonQuery(ctx);

                        result += "success";
                        ctx.CommitTransaction();
                    }
                }
                finally
                {
                    ctx.Close();
                }
            }
            else if (e.Parameter == "savefailed")
            {
                result = "save|";
                IDbContext ctx = DbFactory.Configure(true);
                try
                {

                    ctx.CommandText = string.Format("UPDATE {0} SET MigrationStatus = 3 WHERE ID = {1}", hd.FromTable, Request.Form[hdnEntryID.UniqueID]);
                    DaoBase.ExecuteNonQuery(ctx);

                    result += "success";
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    result += "fail|" + ex.Message;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            else
            {
                result = "delete|";
                IDbContext ctx = DbFactory.Configure(true);
                try
                {

                    ctx.CommandText = string.Format("UPDATE {0} SET MigrationStatus = 2 WHERE ID = {1}", hd.FromTable, Request.Form[hdnEntryID.UniqueID]);
                    DaoBase.ExecuteNonQuery(ctx);

                    result += "success";
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    result += "fail|" + ex.Message;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        class LinkOtherTable
        {
            public string TableName;
            public string ColumnName;
            public string Value;
        }

        class CTableCode
        {
            public string FormatCode;
            public string IDColumn;
            public string ColumnName;
        }

        private List<MigrationConfigurationDt> lstMigrationDt = null;
        protected void rptEntry_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                MigrationConfigurationTableLink entity = (MigrationConfigurationTableLink)e.Item.DataItem;

                if (entity.IsOneToMany)
                {
                    List<MigrationConfigurationDt> lstBinding = new List<MigrationConfigurationDt>();

                    IDbContext ctx = DbFactory.Configure(true);
                    try
                    {
                        ctx.CommandText = Select(entity.RepeaterTable, entity.RepeaterFilterExpression);
                        DataTable dataTable = DaoBase.GetDataTable(ctx);
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string colName = row[entity.RepeaterIDValue].ToString();
                            MigrationConfigurationDt dt = lstMigrationDt.FirstOrDefault(p => p.ColumnName == colName && p.TableName == entity.LinkTableName && p.LinkColumn == entity.ColumnName);
                            if (dt == null)
                            {
                                dt = new MigrationConfigurationDt();
                                dt.Type = "1";
                                dt.FromColumn = "";
                                dt.DefaultValue = "";
                                dt.TableName = entity.LinkTableName;
                            }
                            else
                                if (!dt.IsVisible)
                                    continue;
                            dt.ColumnCaption = row[entity.RepeaterLabelValue].ToString();
                            lstBinding.Add(dt);
                        }
                    }
                    finally
                    {
                        ctx.Close();
                    }
                    Repeater rptEntryDt = (Repeater)e.Item.FindControl("rptEntryDt");
                    rptEntryDt.DataSource = lstBinding;
                    rptEntryDt.DataBind();
                }
                else
                {
                    List<SysColumns> lstColumns = BusinessLayer.GetSysColumnsList(string.Format("OBJECT_ID = (SELECT object_id FROM Sys.objects WHERE name = '{0}') AND name NOT IN (SELECT ColumnName FROM MigrationConfigurationTableLink WHERE HeaderID = {1})", entity.LinkTableName, hdnHeaderID.Value));

                    List<MigrationConfigurationDt> lstBinding = new List<MigrationConfigurationDt>();
                    foreach (SysColumns col in lstColumns)
                    {
                        if (col.Name != "IsDeleted" && col.Name != "CreatedBy" && col.Name != "CreatedDate" && col.Name != "LastUpdatedDate" && col.Name != "LastUpdatedBy")
                        {
                            if (!col.IsIdentity)
                            {
                                MigrationConfigurationDt dt = lstMigrationDt.FirstOrDefault(p => p.ColumnName == col.Name && p.TableName == entity.LinkTableName && p.LinkColumn == entity.ColumnName);
                                if (dt == null)
                                {
                                    dt = new MigrationConfigurationDt();
                                    dt.ColumnCaption = dt.ColumnName = col.Name;
                                    if (col.Type == "Boolean")
                                        dt.Type = "3";
                                    else
                                        dt.Type = "1";
                                    dt.TableName = entity.LinkTableName;
                                    dt.FromColumn = "";
                                    dt.DefaultValue = "";
                                }
                                else
                                    if (!dt.IsVisible)
                                        continue;
                                dt.IsRequired = !col.IsNullable;
                                lstBinding.Add(dt);
                            }
                        }
                    }
                    Repeater rptEntryDt = (Repeater)e.Item.FindControl("rptEntryDt");
                    rptEntryDt.DataSource = lstBinding;
                    rptEntryDt.DataBind();
                }
            }
        }

        protected void rptEntryDt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                MigrationConfigurationDt entity = (MigrationConfigurationDt)e.Item.DataItem;

                if (entity.Type != "3")
                {
                    HtmlGenericControl lbl = (HtmlGenericControl)e.Item.FindControl("lblColumn");
                    if (entity.IsRequired)
                        lbl.Attributes.Add("class", "lblMandatory");
                }
                HtmlGenericControl div = null;
                string text = "";
                if (dataRow != null)
                {
                    text = entity.FromColumn;
                    Regex regex = new Regex("\\[([a-zA-Z0-9_]*)\\]");
                    MatchCollection matches = regex.Matches(text);
                    List<string> myResultList = new List<string>();
                    foreach (Match match in matches)
                    {
                        string value = match.Value.Replace("[", "").Replace("]", "");
                        text = text.Replace(match.Value, dataRow[value].ToString());
                    }
                }
                TextBox txtOldValue = (TextBox)e.Item.FindControl("txtOldValue");
                txtOldValue.Text = text.Trim();
                txtOldValue.Attributes.Add("disabled", "disabled");

                if (text == "")
                    text = entity.DefaultValue;
                text = text.TrimStart();

                if (entity.Type == "1")
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divTxt");
                    TextBox txt = (TextBox)e.Item.FindControl("txtValue");
                    txt.Text = text;
                    SetControlEntrySetting(txt, new ControlEntrySetting(true, true, entity.IsRequired));
                    //ctl = (TextBox)e.Item.FindControl("txtVitalSignType");
                }
                else if (entity.Type == "2")
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divDdl");
                    ASPxComboBox ddl = (ASPxComboBox)e.Item.FindControl("cboNewValue");

                    MethodInfo method = typeof(BusinessLayer).GetMethod(entity.MethodName, new[] { typeof(string) });
                    object obj = method.Invoke(null, new string[] { entity.FilterExpression });
                    IList list = (IList)obj;

                    ddl.DataSource = list;
                    ddl.TextField = entity.TextField;
                    ddl.ValueField = entity.ValueField;
                    ddl.CallbackPageSize = 50;
                    ddl.EnableCallbackMode = false;
                    ddl.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                    ddl.DropDownStyle = DropDownStyle.DropDownList;
                    ddl.DataBind();

                    if (!entity.IsRequired)
                    {
                        ddl.Items.Insert(0, new ListEditItem { Text = "", Value = "" });
                    }

                    ddl.Text = text.Trim();
                    if (ddl.Value != null && ddl.Text == ddl.Value.ToString())
                        ddl.SelectedIndex = -1;

                    SetControlEntrySetting(ddl, new ControlEntrySetting(true, true, entity.IsRequired));
                }
                else if (entity.Type == "3")
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divChk");
                    CheckBox chk = (CheckBox)e.Item.FindControl("chkValue");
                    chk.Checked = (text == entity.ValueChecked);
                }
                else if (entity.Type == "4")
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divDte");
                    TextBox txt = (TextBox)e.Item.FindControl("txtDteValue");
                    if (text != "")
                    {
                        DateTime date = DateTime.ParseExact(text,
                                           entity.FormatDate,
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None);
                        if (date != null)
                            txt.Text = date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    }
                }
                else if (entity.Type == "5")
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divSd");
                    HtmlGenericControl lbl = (HtmlGenericControl)e.Item.FindControl("lblColumn");
                    if (entity.IsRequired)
                        lbl.Attributes.Add("class", "lblLink lblMandatory");
                    else
                        lbl.Attributes.Add("class", "lblLink");

                    HtmlInputHidden hdnSdNewID = (HtmlInputHidden)e.Item.FindControl("hdnSdNewID");
                    TextBox txtSdNewCode = (TextBox)e.Item.FindControl("txtSdNewCode");
                    TextBox txtSdNewText = (TextBox)e.Item.FindControl("txtSdNewText");
                    if (text == "-")
                        text = "";
                    if (text.Trim() != "")
                    {
                        MethodInfo method = typeof(BusinessLayer).GetMethod(entity.SearchDialogMethodName, new[] { typeof(string) });
                        string filterExpression = string.Format("{0} LIKE '%{1}%'", entity.SearchDialogNameField, text.Trim());
                        object tempObj = method.Invoke(null, new string[] { filterExpression });
                        IList list = (IList)tempObj;
                        if (list.Count > 0)
                        {
                            object obj = list[0];
                            hdnSdNewID.Value = obj.GetType().GetProperty(entity.SearchDialogIDField).GetValue(obj, null).ToString();
                            txtSdNewCode.Text = obj.GetType().GetProperty(entity.SearchDialogCodeField).GetValue(obj, null).ToString();
                            txtSdNewText.Text = obj.GetType().GetProperty(entity.SearchDialogNameField).GetValue(obj, null).ToString();
                        }
                    }
                    if (entity.SearchDialogCodeField == entity.SearchDialogNameField)
                        txtSdNewText.Visible = false;
                    SetControlEntrySetting(txtSdNewCode, new ControlEntrySetting(true, true, entity.IsRequired));
                    //CheckBox chk = (CheckBox)e.Item.FindControl("chkValue");
                    //chk.Checked = (text == entity.ValueChecked);
                }
                else if (entity.Type == "6")
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divCode");
                    TextBox txt = (TextBox)e.Item.FindControl("txtCode");
                    HtmlInputHidden hdnFormatCode = (HtmlInputHidden)e.Item.FindControl("hdnFormatCode");
                    HtmlInputHidden hdnIDColumn = (HtmlInputHidden)e.Item.FindControl("hdnIDColumn");

                    hdnFormatCode.Value = entity.FormatCode;
                    hdnIDColumn.Value = entity.IDColumn;
                    SetControlEntrySetting(txt, new ControlEntrySetting(false, false, false));
                }
                if (div != null)
                    div.Visible = true;
            }
        }
    }
}