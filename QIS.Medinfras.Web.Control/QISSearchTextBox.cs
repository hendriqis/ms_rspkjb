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

namespace QIS.Medinfras.Web.CustomControl
{
    public class QISSearchTextBox : WebControl
    {
        HtmlInputHidden hdnValueText;
        HtmlInputHidden hdnDisplayText;
        HtmlInputHidden hdnSearchText;
        HtmlGenericControl divSearchTextBox = new HtmlGenericControl("div");
        HtmlInputText txtSearch = new HtmlInputText();
        private List<QISSearchTextBoxColumn> _Columns = new List<QISSearchTextBoxColumn>();
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<QISSearchTextBoxColumn> Columns
        {
            get { return _Columns; }
            set { _Columns = value; }
        }

        private String _MethodName;
        public String MethodName
        {
            get { return _MethodName; }
            set { _MethodName = value; }
        }

        private String _FilterExpression;
        public String FilterExpression
        {
            get { return _FilterExpression; }
            set { _FilterExpression = value; }
        }

        private String _ValueText;
        public String ValueText
        {
            get { return _ValueText; }
            set { _ValueText = value; }
        }

        private Boolean _ClientVisible = true;
        public Boolean ClientVisible
        {
            get { return _ClientVisible; }
            set { _ClientVisible = value; }
        }

        private Boolean _IsAllowOtherValue = false;
        public Boolean IsAllowOtherValue
        {
            get { return _IsAllowOtherValue; }
            set { _IsAllowOtherValue = value; }
        }

        private String _DisplayText;
        public String DisplayText
        {
            get { return _DisplayText; }
            set { _DisplayText = value; }
        }

        public String Text
        {
            get { return txtSearch.Value; }
            set { txtSearch.Value = value; }
        }

        public String Value
        {
            get { return txtSearch.Value; }
            set { txtSearch.Value = value; }
        }

        QISSearchTextBoxClientSideEvent _ClientSideEvents = new QISSearchTextBoxClientSideEvent();
        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Styles")]
        [Description("DataControlField_HeaderStyle")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public QISSearchTextBoxClientSideEvent ClientSideEvents
        {
            get { return _ClientSideEvents; }
            set { _ClientSideEvents = value; }
        }

        public string ClientInstanceName { get; set; }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);                       
        }

        private HtmlInputHidden CreateHiddenField(string id, string value)
        {
            HtmlInputHidden hdn = new HtmlInputHidden();
            hdn.ID = this.ID + "_" + id;
            hdn.Attributes.Add("class", id);
            hdn.Value = value;
            return hdn;
        }

        private void SaveDataSourceState()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
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

            divSearchTextBox.ID = this.ID + "_divAutoComplete";
            divSearchTextBox.Attributes.Add("class", "containerAutoComplete");
            if (_ClientVisible)
                divSearchTextBox.Attributes.Add("style", "display:block");
            else
                divSearchTextBox.Attributes.Add("style", "display:none");

            hdnValueText = CreateHiddenField("hdnValueText", "");
            hdnDisplayText = CreateHiddenField("hdnDisplayText", "");
            hdnSearchText = CreateHiddenField("hdnSearchText", "");
            HtmlInputHidden hdnClientInstanceName = CreateHiddenField("hdnClientInstanceName", ClientInstanceName);

            divSearchTextBox.Controls.Add(hdnValueText);
            divSearchTextBox.Controls.Add(hdnDisplayText);
            divSearchTextBox.Controls.Add(hdnSearchText);
            divSearchTextBox.Controls.Add(hdnClientInstanceName);

            WebControl templateScript = new WebControl(HtmlTextWriterTag.Script);
            divSearchTextBox.Controls.Add(templateScript);
            templateScript.Attributes["id"] = this.ClientID + "_tmplAutoComplete";
            templateScript.Attributes["type"] = "text/x-jquery-tmpl";

            StringBuilder templateScriptInnerHtml = new StringBuilder();
            templateScriptInnerHtml.Append("<tr>");
            foreach (QISSearchTextBoxColumn column in Columns)
            {
                templateScriptInnerHtml.Append("<td>${").Append(column.FieldName).Append("}</td>");
            }
            templateScriptInnerHtml.Append("</tr>");

            templateScript.Controls.Add(new LiteralControl(templateScriptInnerHtml.ToString()));

            #region Text Search
            HtmlTable tbl = new HtmlTable();
            tbl.Attributes.Add("style", string.Format("height:24px;width:{0};border-collapse:collapse;", Width));
            tbl.Attributes.Add("class", "tblContainerTextBox");
            tbl.CellPadding = 0;
            tbl.CellSpacing = 0;

            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();
            cell.Attributes.Add("style", "width: 100%");
            cell.Attributes.Add("class", "dxic");

            HtmlGenericControl divInput = new HtmlGenericControl("div");
            divInput.Attributes.Add("style", "position:relative;height:20px");

            txtSearch.Attributes.Add("autocomplete", "off");
            txtSearch.Attributes.Add("class", "txtIntellisense");
            txtSearch.Attributes.Add("style", String.Format("border:0px;font-family:Segoe UI;font-size:9pt;margin-left:1px;margin-right:1px;margin-top:1px;margin-bottom:2px;color:Black;width:{0}", Width));

            HtmlImage imgSearch = new HtmlImage();
            imgSearch.Attributes.Add("class", "imgLoadingAutoComplete");
            imgSearch.Attributes.Add("style", "width:22px;height:22px;position:absolute;cursor:pointer;top:-2px;right:-2px;z-index:1100;");
            imgSearch.Alt = "S";
            imgSearch.Src = Page.ResolveUrl("~/Libs/Images/loading_small.gif");

            divInput.Controls.Add(txtSearch);
            divInput.Controls.Add(imgSearch);
            cell.Controls.Add(divInput);
            row.Cells.Add(cell);
            tbl.Rows.Add(row);

            divSearchTextBox.Controls.Add(tbl);
            #endregion

            #region Auto Complete Box

            HtmlGenericControl containerAutoCompleteIntellisenseBox = new HtmlGenericControl("span");
            containerAutoCompleteIntellisenseBox.Attributes.Add("class", "containerAutoCompleteIntellisenseBox");

            HtmlGenericControl autoCompleteIntellisenseBox = new HtmlGenericControl("div");
            autoCompleteIntellisenseBox.Attributes.Add("class", "autoCompleteIntellisenseBox");

            HtmlGenericControl autoCompleteTitleParameter = new HtmlGenericControl("div");
            autoCompleteTitleParameter.Attributes.Add("class", "autoCompleteTitle");
            autoCompleteTitleParameter.InnerHtml = "Search Parameter";

            HtmlGenericControl autoCompleteIntellisenseContent = new HtmlGenericControl("div");
            autoCompleteIntellisenseContent.Attributes.Add("class", "autoCompleteIntellisenseContent");

            HtmlGenericControl autoCompleteIntellisenseContentText = new HtmlGenericControl("div");
            autoCompleteIntellisenseContentText.Attributes.Add("class", "autoCompleteIntellisenseContentText");
            //autoCompleteIntellisenseContentText.InnerHtml = "<b>FirstName</b>;LastName";

            HtmlGenericControl divIntellisenseDescription = new HtmlGenericControl("div");
            divIntellisenseDescription.Attributes.Add("class", "intellisenseDescription");

            containerAutoCompleteIntellisenseBox.Controls.Add(autoCompleteIntellisenseBox);
            autoCompleteIntellisenseBox.Controls.Add(autoCompleteTitleParameter);
            autoCompleteIntellisenseBox.Controls.Add(autoCompleteIntellisenseContent);
            autoCompleteIntellisenseContent.Controls.Add(autoCompleteIntellisenseContentText);
            autoCompleteIntellisenseContent.Controls.Add(divIntellisenseDescription);


            HtmlGenericControl autoCompleteBox = new HtmlGenericControl("div");
            autoCompleteBox.Attributes.Add("class", "autoCompleteBox");

            HtmlGenericControl autoCompleteTitleResult = new HtmlGenericControl("div");
            autoCompleteTitleResult.Attributes.Add("class", "autoCompleteTitle");
            autoCompleteTitleResult.InnerHtml = "Search Result";

            HtmlGenericControl divSearchContent = new HtmlGenericControl("div");
            divSearchContent.Attributes.Add("class", "autoCompleteContent");

            HtmlTable tblSearch = new HtmlTable();
            tblSearch.CellPadding = 0;
            tblSearch.CellSpacing = 0;
            tblSearch.Attributes.Add("class", "tblAutoCompleteContent");

            HtmlTableRow rowSearchContent = new HtmlTableRow();
            tblSearch.Rows.Add(rowSearchContent);

            foreach (QISSearchTextBoxColumn column in Columns)
            {
                HtmlTableCell cellSearchContent = new HtmlTableCell("th");
                cellSearchContent.InnerHtml = column.Caption;
                cellSearchContent.Attributes.Add("style", string.Format("width:{0}", column.Width));
                rowSearchContent.Cells.Add(cellSearchContent);
            }

            divSearchContent.Controls.Add(tblSearch);
            autoCompleteBox.Controls.Add(autoCompleteTitleResult);
            autoCompleteBox.Controls.Add(divSearchContent);
            containerAutoCompleteIntellisenseBox.Controls.Add(autoCompleteBox);
            divSearchTextBox.Controls.Add(containerAutoCompleteIntellisenseBox);
            #endregion

            container.Controls.Add(divSearchTextBox);
            Controls.Add(parent);
            #endregion
            RegisterJavaScript();
        }

        private void RegisterJavaScript()
        {
            WebControl script = new WebControl(HtmlTextWriterTag.Script);
            divSearchTextBox.Controls.Add(script);
            script.Attributes["id"] = string.Format("dxss_{0}", this.ClientID);
            script.Attributes["type"] = "text/javascript";

            if (_ClientSideEvents.LostFocus == "")
                _ClientSideEvents.LostFocus = "function(s){}";
            if (_ClientSideEvents.Init == "")
                _ClientSideEvents.Init = "function(s){}";
            if (_ClientSideEvents.ValueChanged == "")
                _ClientSideEvents.ValueChanged = "function(s){}";
            if (_FilterExpression == "")
                _FilterExpression = "1 = 1";

            script.Controls.Add(new LiteralControl("$(function () {"));
            script.Controls.Add(new LiteralControl(string.Format("var {0}_columns = [];", this.ClientID)));
            foreach (QISSearchTextBoxColumn col in Columns)
            {
                script.Controls.Add(new LiteralControl(string.Format("{0}_columns.push({{ 'text':'{1}','fieldName':'{2}','description':'{3}' }});", this.ClientID, col.Caption, col.FieldName, col.Description)));
            }

            script.Controls.Add(new LiteralControl(string.Format("window.{0}Helper = new QISClientSearchTextBoxHelper();", this.ClientID)));
            script.Controls.Add(new LiteralControl(string.Format("window.{0} = new QISClientSearchTextBox();", ClientInstanceName)));

            string jsIsAllowOtherValue = "";
            if (_IsAllowOtherValue)
                jsIsAllowOtherValue = "true";
            else
                jsIsAllowOtherValue = "false";
            script.Controls.Add(new LiteralControl(string.Format("{0}Helper.setParam({1},\"{2}\",\"{3}\",\"{4}\",\"{5}\",{6},{7},{8});", this.ClientID, ClientInstanceName, MethodName, _FilterExpression, ValueText, DisplayText, jsIsAllowOtherValue, _ClientSideEvents.LostFocus, _ClientSideEvents.ValueChanged)));
            script.Controls.Add(new LiteralControl(string.Format("{0}Helper.init('{0}', {0}_columns);", this.ClientID)));

            script.Controls.Add(new LiteralControl(string.Format("{0}.init({1}Helper, \"{1}\");", ClientInstanceName, this.ClientID)));
            //script.Controls.Add(new LiteralControl(string.Format("{0}({1});", _ClientSideEvents.Init, ClientInstanceName)));
            script.Controls.Add(new LiteralControl(string.Format("{0}Helper.execInitHandler({1});", this.ClientID, _ClientSideEvents.Init)));

            script.Controls.Add(new LiteralControl("});"));
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
        }
    }

}