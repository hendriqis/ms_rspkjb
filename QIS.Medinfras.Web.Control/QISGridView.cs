using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Reflection;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CustomControl
{
    public class QISGridView : WebControl
    {
        HtmlInputHidden hdnRowCount;
        HtmlInputHidden hdnPageCount;
        HtmlInputHidden hdnPageIndex;
        HtmlInputHidden hdnCallbackParam;
        HtmlInputHidden hdnSelected;
        HtmlInputHidden hdnFocusedRowIndex;
        HtmlInputHidden hdnSaveState;
        ASPxCallbackPanel updatePanel = new ASPxCallbackPanel();
        HtmlGenericControl divPaging = new HtmlGenericControl("div");
        HtmlGenericControl divPageInformation = new HtmlGenericControl("div");
        GridView grdView = new GridView();
        GridView grdViewHeader = new GridView();

        public event QISGridViewPageChangedEventHandler PageChanged;
        public event QISGridViewCustomCallbackEventHandler CustomCallback;

        QISGridViewSetting _Setting = new QISGridViewSetting();
        QISDataSourceBusinessLayer _DataSourceBusinessLayer = new QISDataSourceBusinessLayer();
        QISGridViewClientSideEvent _ClientSideEvents = new QISGridViewClientSideEvent();

        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Styles")]
        [Description("DataControlField_HeaderStyle")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public QISDataSourceBusinessLayer DataSourceBusinessLayer
        {
            get { return _DataSourceBusinessLayer; }
            set { _DataSourceBusinessLayer = value; }
        }

        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Styles")]
        [Description("DataControlField_HeaderStyle")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public QISGridViewClientSideEvent ClientSideEvents
        {
            get { return _ClientSideEvents; }
            set { _ClientSideEvents = value; }
        }

        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Styles")]
        [Description("DataControlField_HeaderStyle")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public QISGridViewSetting Setting
        {
            get { return _Setting; }
            set { _Setting = value; }
        }

        private List<QISGridViewColumn> _Columns = new List<QISGridViewColumn>();
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<QISGridViewColumn> Columns
        {
            get { return _Columns; }
            set { _Columns = value; }
        }

        private String _EmptyRowText;
        public String EmptyRowText
        {
            get { return (_EmptyRowText == null ? "" : _EmptyRowText); }
            set { _EmptyRowText = value; }
        }

        public string ClientInstanceName { get; set; }

        public int FocusedRowIndex
        {
            get
            {
                if (hdnFocusedRowIndex.Value == null || hdnFocusedRowIndex.Value == "")
                    return -1;
                return Convert.ToInt32(hdnFocusedRowIndex.Value);
            }
        }

        public object GetRow(int index)
        {
            if (grdView.DataSource == null)
                return null;
            IList list = (IList)grdView.DataSource;
            return list[index];
        }

        private String _KeyFieldName = string.Empty;
        public String KeyFieldName
        {
            get { return _KeyFieldName; }
            set { _KeyFieldName = value; }
        }

        public object DataSource
        {
            get { return grdView.DataSource; }
            set { grdView.DataSource = value; }
        }

        public override void DataBind()
        {
            MethodInfo method = typeof(BusinessLayer).GetMethod(DataSourceBusinessLayer.MethodName, new[] { typeof(string), typeof(int), typeof(int), typeof(string) });
            if (DataSourceBusinessLayer.IsFilterExpressionChanged)
            {
                int rowCount = Convert.ToInt32(typeof(BusinessLayer).GetMethod(DataSourceBusinessLayer.RowCountMethodName, BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[] { DataSourceBusinessLayer.FilterExpression }));
                decimal pageCount = Math.Ceiling((decimal)rowCount / Setting.PageCount);
                hdnPageCount.Value = pageCount.ToString();
                hdnRowCount.Value = rowCount.ToString();
                hdnPageIndex.Value = "1";
                DataSourceBusinessLayer.IsFilterExpressionChanged = false;
            }

            grdView.DataSource = method.Invoke(null, new object[] { DataSourceBusinessLayer.FilterExpression, Setting.PageCount, Convert.ToInt32(hdnPageIndex.Value), "" });
            grdView.DataBind();
            
            MethodInfo method2 = typeof(BusinessLayer).GetMethod(DataSourceBusinessLayer.MethodName, new[] { typeof(string) });
            grdViewHeader.DataSource = method2.Invoke(null, new object[] { "1 = 0" });
            grdViewHeader.DataBind();
            
            SaveDataSourceState();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            #region Create Control
            System.Web.UI.Control parent;
            System.Web.UI.Control container;

            // Get a reference to the ScriptManager object for the page
            // if one exists.
            ScriptManager sm = ScriptManager.GetCurrent(Page);


            if (sm == null || !sm.EnablePartialRendering)
            {
                // If partial rendering is not enabled, set the parent
                // and container as a basic control.
                container = new System.Web.UI.Control();
                parent = container;
            }
            else
            {
                // If partial rendering is enabled, set the parent as
                // a new UpdatePanel object and the container to the
                // content template of the UpdatePanel object.
                UpdatePanel up = new UpdatePanel();
                container = up.ContentTemplateContainer;
                parent = up;
            }

            Control currControl = this;
            string name = currControl.UniqueID;
            //while (true)
            //{
            //    if (currControl == null || currControl.ID == "__Page")
            //        break;

            //    string addName = "$" + name;
            //    if (name == "")
            //        addName = "";

            //    if (currControl.ID == null)
            //        name = currControl.ClientID + addName;
            //    else
            //        name = currControl.ID + addName;

            //    currControl = currControl.NamingContainer;

            //}

            string SaveState = Page.Request.Form[name + "_updPnlGridView$hdnSaveState"] ?? "";
            if (SaveState != "")
            {
                string[] param = SaveState.Split('|');
                DataSourceBusinessLayer.FilterExpression = param[0];
                DataSourceBusinessLayer.IsFilterExpressionChanged = (param[1] == "1");
            }

            HtmlGenericControl divContainer = new HtmlGenericControl("div");
            divContainer.ID = this.ID + "_divContainer";
            divContainer.Attributes.Add("class", "divContainerQISGridView");
            divContainer.Attributes.Add("style", string.Format("position:relative;width:{0};height:{1}", Width, Height));

            #region Blanket & image Loading
            HtmlGenericControl divBlanket = new HtmlGenericControl("div");
            divBlanket.Attributes.Add("class", "grdBlanket");

            HtmlGenericControl divImageLoading = new HtmlGenericControl("div");
            divImageLoading.Attributes.Add("class", "imgLoadingRpt");

            HtmlImage imgLoading = new HtmlImage();
            imgLoading.Src = Page.ResolveUrl("~/Images/Loading.gif");
            divImageLoading.Controls.Add(imgLoading);

            divContainer.Controls.Add(divBlanket);
            divContainer.Controls.Add(divImageLoading);
            #endregion

            HtmlGenericControl divContainerGridView = new HtmlGenericControl("div");
            divContainerGridView.Attributes.Add("style", "width:100%");

            #region Panel GridView
            updatePanel.ID = this.ID + "_updPnlGridView";
            updatePanel.ClientInstanceName = this.ClientID + "_updPnlGridView";
            updatePanel.Callback += new DevExpress.Web.ASPxClasses.CallbackEventHandlerBase(updatePanel_Callback);
            updatePanel.ShowLoadingPanel = false;
            updatePanel.ClientSideEvents.BeginCallback = String.Format("function(s,e){{ {0}grdHelper.onBeginCallback('{0}');}}", this.ClientID);
            updatePanel.ClientSideEvents.EndCallback = String.Format("function(s,e){{ {0}grdHelper.onEndCallback('{0}');}}", this.ClientID);

            hdnSaveState = CreateHiddenField("hdnSaveState", "");
            hdnRowCount = CreateHiddenField("hdnRowCount", "0");
            hdnPageCount = CreateHiddenField("hdnPageCount", "0");
            hdnPageIndex = CreateHiddenField("hdnPageIndex", "1");
            hdnCallbackParam = CreateHiddenField("hdnCallbackParam", "");
            hdnSelected = CreateHiddenField("hdnSelected", "0");
            hdnFocusedRowIndex = CreateHiddenField("hdnFocusedRowIndex", "0");

            grdViewHeader.ShowHeaderWhenEmpty = true;
            grdViewHeader.Width = new Unit("100%");
            grdViewHeader.AutoGenerateColumns = false;
            grdViewHeader.GridLines = GridLines.None;
            grdViewHeader.CssClass = "mGrid";
            grdViewHeader.ID = "grdViewHeader";

            HtmlGenericControl divGridViewScroller = new HtmlGenericControl("div");
            divGridViewScroller.ID = "divGridViewScroller";
            divGridViewScroller.Attributes.Add("class", "divGridViewScroller");

            grdView.Width = new Unit("100%");
            grdView.ShowHeader = false;
            grdView.ID = "grdView";
            grdView.AutoGenerateColumns = false;
            grdView.GridLines = GridLines.None;
            grdView.CssClass = "mGrid";

            divGridViewScroller.Controls.Add(grdView);

            updatePanel.Controls.Add(hdnSaveState);
            updatePanel.Controls.Add(hdnRowCount);
            updatePanel.Controls.Add(hdnPageCount);
            updatePanel.Controls.Add(hdnPageIndex);
            updatePanel.Controls.Add(hdnCallbackParam);
            updatePanel.Controls.Add(hdnSelected);
            updatePanel.Controls.Add(hdnFocusedRowIndex);
            updatePanel.Controls.Add(grdViewHeader);
            updatePanel.Controls.Add(divGridViewScroller);

            divContainerGridView.Controls.Add(updatePanel);
            #endregion

            #region Paging
            HtmlGenericControl divContainerPaging = new HtmlGenericControl("div");
            divContainerPaging.Attributes.Add("class", "containerPaging");
            divContainerPaging.ID = this.ID + "_containerPaging";

            HtmlGenericControl divWrapperPaging = new HtmlGenericControl("div");
            divWrapperPaging.Attributes.Add("class", "wrapperPaging");

            HtmlTable tblPaging = new HtmlTable();
            tblPaging.Attributes.Add("style", "width:100%;");
            HtmlTableRow tblPagingRow = new HtmlTableRow();

            #region Cell1
            HtmlTableCell cell1 = new HtmlTableCell();
            cell1.Attributes.Add("style", "width:180px");

            divPageInformation.ID = this.ID + "_PageInformation";
            divPageInformation.Attributes.Add("class", "pageInformation");

            cell1.Controls.Add(divPageInformation);
            #endregion
            #region Cell2
            HtmlTableCell cell2 = new HtmlTableCell();
            divPaging.ID = this.ID + "_Paging";
            divPaging.Attributes.Add("class", "paging");
            cell2.Controls.Add(divPaging);
            #endregion
            tblPagingRow.Cells.Add(cell1);
            tblPagingRow.Cells.Add(cell2);

            tblPaging.Rows.Add(tblPagingRow);

            divWrapperPaging.Controls.Add(tblPaging);
            divContainerPaging.Controls.Add(divWrapperPaging);
            #endregion

            #region Grid Empty
            HtmlGenericControl divGrdEmpty = new HtmlGenericControl("div");
            divGrdEmpty.Attributes.Add("class", "divGrdEmpty");
            divGrdEmpty.Attributes.Add("style", "display:none");
            if (EmptyRowText == "")
                EmptyRowText = "No Data To Display";
            divGrdEmpty.InnerHtml = EmptyRowText;
            #endregion

            divContainer.Controls.Add(divContainerGridView);
            divContainer.Controls.Add(divContainerPaging);
            divContainer.Controls.Add(divGrdEmpty);
            container.Controls.Add(divContainer);

            Controls.Add(parent);

            #endregion

            BoundField hdnKeyField = new BoundField();
            hdnKeyField.HeaderStyle.CssClass = "hdnValue";
            hdnKeyField.ItemStyle.CssClass = "hdnValue";
            hdnKeyField.DataField = _KeyFieldName;
            grdView.Columns.Add(hdnKeyField);

            foreach (QISGridViewColumn col in Columns)
            {
                if (col is QISGridViewDataColumn)
                {
                    BoundField field = new BoundField();
                    field.DataField = ((QISGridViewDataColumn)col).FieldName;
                    SetCommonProperties(field, col);
                    grdView.Columns.Add(field);

                    BoundField field2 = new BoundField();
                    field2.DataField = ((QISGridViewDataColumn)col).FieldName;
                    SetCommonProperties(field2, col);
                    grdViewHeader.Columns.Add(field2);
                }
                else if (col is QISGridViewTemplateColumn)
                {
                    TemplateField field = new TemplateField();
                    field.ItemTemplate = ((QISGridViewTemplateColumn)col).ItemTemplate;
                    SetCommonProperties(field, col);
                    grdView.Columns.Add(field);

                    TemplateField field2 = new TemplateField();
                    field2.HeaderTemplate = ((QISGridViewTemplateColumn)col).HeaderTemplate;
                    SetCommonProperties(field2, col);
                    grdViewHeader.Columns.Add(field2);
                }
            }

            if (SaveState == "")
                RegisterJavaScript();
        }

        protected void updatePanel_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (hdnCallbackParam.Value != "")
            {
                string[] param = hdnCallbackParam.Value.Split('|');
                if (param[0] == "pagechanged")
                {
                    int pageIndex = Convert.ToInt32(param[1]);
                    if (PageChanged != null)
                        PageChanged(this, new QISGridViewPageChangedEventArgs { PageIndex = pageIndex });
                    else
                        this.DataBind();
                }
                else if (param[0] == "performcallback")
                {
                    if (CustomCallback != null)
                        CustomCallback(this, new QISGridViewCustomCallbackEventArgs { Parameter = param[1] });
                    else
                        this.DataBind();
                }
            }
        }

        private void SetCommonProperties(DataControlField field, QISGridViewColumn col)
        {
            field.HeaderText = col.Caption;
            field.HeaderStyle.HorizontalAlign = col.HeaderStyle.HorizontalAlign;
            field.HeaderStyle.VerticalAlign = col.HeaderStyle.VerticalAlign;
            field.ItemStyle.HorizontalAlign = col.ItemStyle.HorizontalAlign;
            field.ItemStyle.VerticalAlign = col.ItemStyle.VerticalAlign;
            if (col.Width != "")
            {
                field.ItemStyle.Width = new Unit(col.Width);
                field.HeaderStyle.Width = new Unit(col.Width);
            }
        }

        private void SaveDataSourceState()
        {
            StringBuilder saveState = new StringBuilder();
            saveState.Append(DataSourceBusinessLayer.FilterExpression).Append("|");
            if (DataSourceBusinessLayer.IsFilterExpressionChanged)
                saveState.Append("1");
            else
                saveState.Append("0");
            hdnSaveState.Value = saveState.ToString();
        }

        private void RegisterJavaScript()
        {
            WebControl script = new WebControl(HtmlTextWriterTag.Script);
            updatePanel.Controls.Add(script);
            script.Attributes["id"] = string.Format("dxss_{0}", this.ClientID);
            script.Attributes["type"] = "text/javascript";

            if (_ClientSideEvents.RowClick != "")
            {
                script.Controls.Add(new LiteralControl(string.Format("window.{0}RowClickVar = {1};", this.ClientID, _ClientSideEvents.RowClick)));
                script.Controls.Add(new LiteralControl(string.Format("eval({0}RowClickVar);", this.ClientID)));
            }
            if (_ClientSideEvents.RowDblClick != "")
            {
                script.Controls.Add(new LiteralControl(string.Format("window.{0}RowDblClickVar = {1};", this.ClientID, _ClientSideEvents.RowDblClick)));
                script.Controls.Add(new LiteralControl(string.Format("eval({0}RowDblClickVar);", this.ClientID)));
            }

            script.Controls.Add(new LiteralControl("$(function () {"));
            script.Controls.Add(new LiteralControl(string.Format("window.{0} = new QISClientGridView();", ClientInstanceName)));
            script.Controls.Add(new LiteralControl(string.Format("{0}.init('{1}');", ClientInstanceName, this.ClientID)));

            script.Controls.Add(new LiteralControl(string.Format("window.{0}grdHelper = new QISClientGridViewHelper();", this.ClientID)));
            script.Controls.Add(new LiteralControl(string.Format("{0}grdHelper.setParam('{1}','{2}','{3}');", this.ClientID, ClientInstanceName, _ClientSideEvents.RowClick, _ClientSideEvents.RowDblClick)));
            script.Controls.Add(new LiteralControl(string.Format("{0}grdHelper.init('{0}','{1}');", this.ClientID, _ClientSideEvents.RowClick)));
            script.Controls.Add(new LiteralControl("});"));            
        }

        private HtmlInputHidden CreateHiddenField(string id, string value)
        {
            HtmlInputHidden hdn = new HtmlInputHidden();
            hdn.ID = id;
            hdn.Name = id;
            hdn.Value = value;
            return hdn;
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
        }
    }

    public class QISGridViewPageChangedEventArgs : EventArgs
    {
        public int PageIndex { get; set; }
    }

    public class QISGridViewCustomCallbackEventArgs : EventArgs
    {
        public string Parameter { get; set; }
    }

    public delegate void QISGridViewPageChangedEventHandler(object sender, QISGridViewPageChangedEventArgs e);
    public delegate void QISGridViewCustomCallbackEventHandler(object sender, QISGridViewCustomCallbackEventArgs e);

}