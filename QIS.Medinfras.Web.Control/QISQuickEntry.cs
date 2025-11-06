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
    public class QISQuickEntry : WebControl
    {
        HtmlGenericControl divIntellisense = new HtmlGenericControl("div");
        HtmlInputText txtSearch = new HtmlInputText();
        private List<QISQuickEntryHint> _QuickEntryHints = new List<QISQuickEntryHint>();
        private string _AdditionalFilterExpression = string.Empty;

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public List<QISQuickEntryHint> QuickEntryHints
        {
            get { return _QuickEntryHints; }
            set { _QuickEntryHints = value; }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string AdditionalFilterExpression 
        { 
            get { return _AdditionalFilterExpression;}
            set { _AdditionalFilterExpression = value;} 
        }

        public String GenerateFilterExpression()
        {
            String[] textValue = txtSearch.Value.Split(';');
            int i = 0;
            StringBuilder result = new StringBuilder();
            while (true) {
                if (i == textValue.Length || i == QuickEntryHints.Count)
                    break;
                if (textValue[i] != "*") {
                    if (result.ToString() != "")
                        result.Append(" AND ");
                    result.Append(QuickEntryHints[i].ValueField).Append(" LIKE '%").Append(textValue[i]).Append("%'");
                }
                i++;
            }
            if (!string.IsNullOrEmpty(_AdditionalFilterExpression))
            {
                result.Append(string.Format(" AND {0}",_AdditionalFilterExpression));
            }
            return result.ToString();
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

        QISQuickEntryClientSideEvent _ClientSideEvents = new QISQuickEntryClientSideEvent();
        [DefaultValue("")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Styles")]
        [Description("DataControlField_HeaderStyle")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public QISQuickEntryClientSideEvent ClientSideEvents
        {
            get { return _ClientSideEvents; }
            set { _ClientSideEvents = value; }
        }

        public string ClientInstanceName { get; set; }


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

            //Control currControl = this;
            //string name = currControl.UniqueID;

            divIntellisense.ID = this.ID + "_divAutoComplete";
            divIntellisense.Attributes.Add("class", "containerAutoComplete");
            divIntellisense.Attributes.Add("style", "width:" + Width);

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
            txtSearch.Attributes.Add("class", "txtAutoComplete"); 
            txtSearch.Attributes.Add("style", String.Format("border:0px;font-family:Segoe UI;font-size:9pt;margin-left:1px;margin-right:1px;margin-top:1px;margin-bottom:2px;color:Black;width:{0}", Width));

            HtmlInputHidden hdnAutoCompleteValue = new HtmlInputHidden();
            hdnAutoCompleteValue.Attributes.Add("class", "hdnAutoCompleteValue");

            HtmlImage imgSearch = new HtmlImage();
            imgSearch.Attributes.Add("class", "imgLoadingAutoComplete");
            imgSearch.Attributes.Add("style", "width:22px;height:22px;position:absolute;cursor:pointer;top:-2px;right:-2px;z-index:1100;");
            imgSearch.Alt = "S";
            imgSearch.Src = Page.ResolveUrl("~/Libs/Images/loading_small.gif");

            divInput.Controls.Add(txtSearch);
            divInput.Controls.Add(hdnAutoCompleteValue);
            divInput.Controls.Add(imgSearch);
            cell.Controls.Add(divInput);
            row.Cells.Add(cell);
            tbl.Rows.Add(row);

            divIntellisense.Controls.Add(tbl);
            #endregion

            #region Intellisense Box
            #region Search Parameter
            HtmlGenericControl divContainerAutoCompleteIntellisenseBox = new HtmlGenericControl("div");
            divContainerAutoCompleteIntellisenseBox.Attributes.Add("class", "containerAutoCompleteIntellisenseBox");

            HtmlGenericControl divAutoCompleteIntellisenseBox = new HtmlGenericControl("div");
            divAutoCompleteIntellisenseBox.Attributes.Add("class", "autoCompleteIntellisenseBox");

            HtmlGenericControl divAutoCompleteTitle = new HtmlGenericControl("div");
            divAutoCompleteTitle.Attributes.Add("class", "autoCompleteTitle");
            divAutoCompleteTitle.InnerHtml = "Search Parameter";

            HtmlGenericControl divIntellisenseBox = new HtmlGenericControl("div");
            divIntellisenseBox.Attributes.Add("class", "intellisenseBox");

            HtmlGenericControl divIntellisenseContent = new HtmlGenericControl("div");
            divIntellisenseContent.Attributes.Add("class", "intellisenseContent");

            HtmlGenericControl divAutoCompleteIntellisenseContentText = new HtmlGenericControl("div");
            divAutoCompleteIntellisenseContentText.Attributes.Add("class", "autoCompleteIntellisenseContentText");

            HtmlGenericControl divIntellisenseDescription = new HtmlGenericControl("div");
            divIntellisenseDescription.Attributes.Add("class", "intellisenseDescription");


            divIntellisenseContent.Controls.Add(divAutoCompleteIntellisenseContentText);
            divIntellisenseContent.Controls.Add(divIntellisenseDescription);

            divIntellisenseBox.Controls.Add(divIntellisenseContent);

            divAutoCompleteIntellisenseBox.Controls.Add(divAutoCompleteTitle);
            divAutoCompleteIntellisenseBox.Controls.Add(divIntellisenseBox);

            divContainerAutoCompleteIntellisenseBox.Controls.Add(divAutoCompleteIntellisenseBox);

            divIntellisense.Controls.Add(divContainerAutoCompleteIntellisenseBox);
            #endregion

            #region Search Parameter
            HtmlGenericControl divAutoCompleteBox = new HtmlGenericControl("div");
            divAutoCompleteBox.Attributes.Add("class", "autoCompleteBox");

            HtmlGenericControl divAutoCompleteTitle2 = new HtmlGenericControl("div");
            divAutoCompleteTitle2.Attributes.Add("class", "autoCompleteTitle");
            divAutoCompleteTitle2.InnerHtml = "Search Result";

            HtmlGenericControl divAutoCompleteContent = new HtmlGenericControl("div");
            divAutoCompleteContent.Attributes.Add("class", "autoCompleteContent");

            HtmlTable tblAutoCompleteContent = new HtmlTable();
            tblAutoCompleteContent.Attributes.Add("class", "tblAutoCompleteContent");
            tblAutoCompleteContent.CellPadding = 0;
            tblAutoCompleteContent.CellSpacing = 0;


            divAutoCompleteContent.Controls.Add(tblAutoCompleteContent);

            divAutoCompleteBox.Controls.Add(divAutoCompleteTitle2);
            divAutoCompleteBox.Controls.Add(divAutoCompleteContent);

            divContainerAutoCompleteIntellisenseBox.Controls.Add(divAutoCompleteBox);
            #endregion
            #endregion

            #region Template
            int ctr = 1;
            foreach (QISQuickEntryHint hint in _QuickEntryHints)
            {
                StringBuilder sbHeaderTemplate = new StringBuilder();
                StringBuilder sbItemTemplate = new StringBuilder();
                sbHeaderTemplate.Append("<tr>");
                sbItemTemplate.Append("<tr>");
                foreach (QISQuickEntryHintColumn column in hint.Columns)
                {
                    sbHeaderTemplate.Append(string.Format("<th style='width:{0}'>{1}</th>", column.Width, column.Caption));
                    sbItemTemplate.Append(string.Format("<td>${{{0}}}</th>", column.FieldName));
                }
                sbHeaderTemplate.Append("</tr>");
                sbItemTemplate.Append("</tr>");

                HtmlGenericControl scriptHeader = new HtmlGenericControl("script");
                scriptHeader.Attributes["class"] = "headerAutoComplete" + ctr;
                scriptHeader.Attributes["type"] = "text/x-jquery-tmpl";
                scriptHeader.InnerHtml = sbHeaderTemplate.ToString();

                HtmlGenericControl scriptItem = new HtmlGenericControl("script");
                scriptItem.Attributes["class"] = "tmplAutoComplete" + ctr;
                scriptItem.Attributes["type"] = "text/x-jquery-tmpl";
                scriptItem.InnerHtml = sbItemTemplate.ToString();

                divIntellisense.Controls.Add(scriptHeader);
                divIntellisense.Controls.Add(scriptItem);
                ctr++;
            }
            #endregion

            container.Controls.Add(divIntellisense);
            Controls.Add(parent);
            #endregion

            //if (SaveState == "")
            RegisterJavaScript();
        }

        private void RegisterJavaScript()
        {
            WebControl script = new WebControl(HtmlTextWriterTag.Script);
            divIntellisense.Controls.Add(script);
            script.Attributes["id"] = string.Format("dxss_{0}", this.ClientID);
            script.Attributes["type"] = "text/javascript";

            if (_ClientSideEvents.SearchClick == "")
                _ClientSideEvents.SearchClick = "function(s){}";
            script.Controls.Add(new LiteralControl("$(function () {"));
            script.Controls.Add(new LiteralControl(string.Format("var {0}_hints = [];", this.ClientID)));
            foreach (QISQuickEntryHint hint in QuickEntryHints)
            {
                StringBuilder sbColumn = new StringBuilder();
                sbColumn.Append("'column': [");
                StringBuilder listColumn = new StringBuilder();
                foreach (QISQuickEntryHintColumn column in hint.Columns)
                {
                    if (listColumn.ToString() != "")
                        listColumn.Append(", ");
                    listColumn.Append(string.Format("{{ 'name': \"{0}\" }}", column.FieldName));
                }
                sbColumn.Append(listColumn.ToString());
                sbColumn.Append("]");
                script.Controls.Add(new LiteralControl(string.Format("{0}_hints.push({{ 'text':\"{1}\",'fieldName':\"{2}\",'valueField':\"{3}\",'filterExpression':\"{4}\",'description':\"{5}\",'methodName':\"{6}\", {7} }});", this.ClientID, hint.Text, hint.TextField, hint.ValueField, hint.FilterExpression, hint.Description, hint.MethodName, sbColumn.ToString())));
            }

            script.Controls.Add(new LiteralControl(string.Format("window.{0} = new QISClientQuickEntry();", ClientInstanceName)));
            script.Controls.Add(new LiteralControl(string.Format("{0}.init('{1}', {1}_hints);", ClientInstanceName, this.ClientID)));

            script.Controls.Add(new LiteralControl(string.Format("window.{0}Helper = new QISClientQuickEntryHelper();", this.ClientID)));
            script.Controls.Add(new LiteralControl(string.Format("{0}Helper.setParam({1}, {2});", this.ClientID, ClientInstanceName, _ClientSideEvents.SearchClick)));
            script.Controls.Add(new LiteralControl(string.Format("{0}Helper.init('{0}');", this.ClientID)));
            script.Controls.Add(new LiteralControl("});"));
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
        }
    }

}