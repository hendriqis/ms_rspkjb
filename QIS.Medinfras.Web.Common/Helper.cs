using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web;
using QISEncryption;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxEditors;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace QIS.Medinfras.Web.Common
{
    public partial class Helper
    {
        public static DateTime GetCurrentDate()
        {

            DateTime Date = Methods.SetCurrentDate();
            return Date;
        }

        public static String GenerateMRN(Int32 MRN, IDbContext ctx = null)
        {
            string result = string.Empty;

            if (ctx == null)
            {
                ctx = DbFactory.Configure(true);
            }

            HealthcareLastNo oLastNo = BusinessLayer.GetHealthcareLastNoList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID), ctx).FirstOrDefault();
            if (oLastNo != null)
            {
                double lastNo = Convert.ToDouble(oLastNo.MedicalNo.Replace("-", ""));
                result = GenerateCode(AppConfigManager.QISMRNFormat, lastNo + 1);
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }

        public static String GetDateIndonesiaFormat(DateTime date)
        {
            string[] hari = { "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu" };
            string hariIni = hari[(int)date.DayOfWeek];

            string TglIndonesia = string.Format("{0}, {1}", hariIni, date.ToString(Constant.FormatString.DATE_FORMAT_1));
            return TglIndonesia; 
        }

        #region Parsing Form Elektronik
        public static string ParsingHtmlRemoveLink(String HtmlForm, String TitleDocument = "")
        {
            string oResult = string.Empty;
            string Content = string.Empty;
            string FinalContent = string.Empty;
            string FormLayout = HtmlForm.ToString();
            //HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["input"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["textarea"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["select"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["option"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["col"] = HtmlElementFlag.Closed;

            var html = new HtmlDocument();
            html.LoadHtml(FormLayout);

            //remove script 
            var RmScriptJS = html.DocumentNode.SelectNodes("//script");
            // Make sure not Null:
            if (RmScriptJS != null)
            {
                // Remove all Nodes:
                foreach (HtmlNode node in RmScriptJS)
                { node.RemoveAll(); }
                //{
                //    var newNodeStr = "<foo>bar</foo>";
                //    var newNode = HtmlNode.CreateNode(newNodeStr);
                //    node.ParentNode.ReplaceChild(newNode, node);
                //    node.RemoveAll();
                //}
            }

            if (!string.IsNullOrEmpty(TitleDocument))
            {
                // title Document
                string newContent = "<div class='TitleDocument'>" + TitleDocument + "</div>";
                HtmlNode newNode = HtmlNode.CreateNode(newContent);
                // Get body node
                HtmlNode body = html.DocumentNode.SelectSingleNode("//body");
                // Add new node as first child of body
                body.PrependChild(newNode);
            }

            var findiv = html.DocumentNode.SelectNodes("//div");
            if (findiv != null)
            {

                foreach (HtmlNode nodeDiv in findiv)
                {

                    //type input txt
                    var links = nodeDiv.SelectNodes("//a");
                    if (links != null)
                    {
                         
                        foreach (HtmlNode tb in links)
                        {
                            HtmlNode lbl = html.CreateElement("p");
                            lbl.InnerHtml = tb.InnerHtml;
                            tb.ParentNode.ReplaceChild(lbl, tb);
                        }

                    }
                      

                }

                Content = html.DocumentNode.OuterHtml;
                Content = Regex.Replace(Content, "<html>", "");
                Content = Regex.Replace(Content, "</html>", "");
                Content = Regex.Replace(Content, "<body>", "");
                Content = Regex.Replace(Content, "</body>", "");
                Content = Regex.Replace(Content, "<head>", "");
                Content = Regex.Replace(Content, "</head>", "");
                Content = Regex.Replace(Content, "<script(.+?)*</script>", "");
                FinalContent = Content;
                ////FinalContent = string.Format("<section class='sheet padding-10mm'><article>{0}</article> </section>", Content);
            }


            return FinalContent;

        }
        public static string ParsingFormElektronik(String HtmlForm, String ValueForm, String TitleDocument = "")
        {
            string oResult = string.Empty;
            string Content = string.Empty;
            string FinalContent = string.Empty;
            string FormLayout = HtmlForm.ToString();
            string[] ParamValue = ValueForm.Split(';');

            //HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["input"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["textarea"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["select"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["option"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["col"] = HtmlElementFlag.Closed;

            var html = new HtmlDocument();
            html.LoadHtml(FormLayout);

            //remove script 
            var RmScriptJS = html.DocumentNode.SelectNodes("//script");
            // Make sure not Null:
            if (RmScriptJS != null)
            {
                // Remove all Nodes:
                foreach (HtmlNode node in RmScriptJS)
                { node.RemoveAll(); }
                //{
                //    var newNodeStr = "<foo>bar</foo>";
                //    var newNode = HtmlNode.CreateNode(newNodeStr);
                //    node.ParentNode.ReplaceChild(newNode, node);
                //    node.RemoveAll();
                //}
            }

            if (!string.IsNullOrEmpty(TitleDocument))
            {
                // title Document
                string newContent = "<div class='TitleDocument'>" + TitleDocument + "</div>";
                HtmlNode newNode = HtmlNode.CreateNode(newContent);
                // Get body node
                HtmlNode body = html.DocumentNode.SelectSingleNode("//body");
                // Add new node as first child of body
                body.PrependChild(newNode);
            }

            var findiv = html.DocumentNode.SelectNodes("//div");
            if (findiv != null)
            {

                foreach (HtmlNode nodeDiv in findiv)
                {

                    //type input txt
                    var findText = nodeDiv.SelectNodes("//input");
                    if (findText != null)
                    {
                        if (findText.Count > 0)
                        {
                            foreach (HtmlNode node in findText)
                            {
                                if (node.Attributes.Count > 0)
                                {
                                    string ClassAtr = node.Attributes["class"].Value;
                                    string controlID = node.Attributes["controlID"].Value;
                                    if (ClassAtr == "txtForm")
                                    {

                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    HtmlAttribute att = node.Attributes["Value"];
                                                    if (att != null)
                                                    {
                                                        att.Value = Value[1];
                                                    }
                                                    else
                                                    {
                                                        node.Attributes.Append("value", Value[1]);
                                                    }

                                                }
                                            }

                                        }

                                    }
                                    else if (ClassAtr == "optForm")
                                    {

                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    if (Value[1] == "1")
                                                    {
                                                        node.SetAttributeValue("checked", "true");
                                                    }

                                                }
                                                node.SetAttributeValue("disabled", "disabled");
                                            }


                                        }

                                    }
                                    else if (ClassAtr == "chkForm")
                                    {
                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    if (Value[1] == "1")
                                                    {
                                                        node.SetAttributeValue("checked", "true");

                                                    }
                                                    node.SetAttributeValue("disabled", "disabled");
                                                }
                                            }

                                        }
                                    }
                                }
                                node.SetAttributeValue("readonly", "true");
                            }
                        }

                    }
                    //text area
                    var findTextArea = nodeDiv.SelectNodes("//textarea");
                    if (findTextArea != null)
                    {
                        if (findTextArea.Count > 0)
                        {
                            foreach (HtmlNode node in findTextArea)
                            {
                                if (node.Attributes.Count > 0)
                                {
                                    string ClassAtr = node.Attributes["class"].Value;
                                    string controlID = node.Attributes["controlID"].Value;
                                    if (ClassAtr == "txtForm")
                                    {

                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    node.InnerHtml = Value[1];
                                                }
                                            }

                                        }

                                    }
                                }

                                node.SetAttributeValue("readonly", "true");
                            }
                        }
                    }

                    // Select Option
                    var FindSelectOption = nodeDiv.SelectNodes("//select");
                    if (FindSelectOption != null)
                    {
                        if (FindSelectOption.Count > 0)
                        {
                            foreach (HtmlNode node in FindSelectOption)
                            {
                                string ClassAtr = node.Attributes["class"].Value;
                                string controlID = node.Attributes["controlID"].Value;
                                if (ClassAtr == "ddlForm")
                                {
                                    if (ParamValue.Length > 0)
                                    {
                                        for (int i = 0; i < ParamValue.Length; i++)
                                        {
                                            string[] Value = ParamValue[i].Split('=');
                                            if (controlID == Value[0])
                                            {
                                                foreach (HtmlNode opt in node.SelectNodes("option"))
                                                {
                                                    var optValue = opt.Attributes["value"];
                                                    var optSelected = opt.Attributes["selected"];
                                                    if (optSelected != null)
                                                    {
                                                        if (string.IsNullOrWhiteSpace(optSelected.Value))
                                                        {
                                                            opt.Attributes.Remove(optSelected);
                                                        }
                                                    }
                                                    if (optValue != null)
                                                    {
                                                        string optResult = optValue.Value;
                                                        if (optResult == Value[1])
                                                        {
                                                            opt.SetAttributeValue("selected", "selected");
                                                        }
                                                        else
                                                        {
                                                            opt.SetAttributeValue("disabled", "disabled");
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }

                                }

                            }
                        }
                    }

                }

                Content = html.DocumentNode.OuterHtml;
                Content = Regex.Replace(Content, "<html>", "");
                Content = Regex.Replace(Content, "</html>", "");
                Content = Regex.Replace(Content, "<body>", "");
                Content = Regex.Replace(Content, "</body>", "");
                Content = Regex.Replace(Content, "<head>", "");
                Content = Regex.Replace(Content, "</head>", "");
                Content = Regex.Replace(Content, "<script(.+?)*</script>", "");
                FinalContent = Content;
                ////FinalContent = string.Format("<section class='sheet padding-10mm'><article>{0}</article> </section>", Content);
            }


            return FinalContent;

        }
        public static string ParsingFormElektronikLabelWithValue(String HtmlForm, String ValueForm, String TitleDocument = "")
        {
            string oResult = string.Empty;
            string Content = string.Empty;
            string FinalContent = string.Empty;
            string FormLayout = HtmlForm.ToString();
            string[] ParamValue = ValueForm.Split(';');

            //HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["input"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["textarea"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["select"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["option"] = HtmlElementFlag.Closed;
            //HtmlNode.ElementsFlags["col"] = HtmlElementFlag.Closed;

            var html = new HtmlDocument();
            html.LoadHtml(FormLayout);

            //remove script 
            var RmScriptJS = html.DocumentNode.SelectNodes("//script");
            // Make sure not Null:
            if (RmScriptJS != null)
            {
                // Remove all Nodes:
                foreach (HtmlNode node in RmScriptJS)
                { node.RemoveAll(); }
                //{
                //    var newNodeStr = "<foo>bar</foo>";
                //    var newNode = HtmlNode.CreateNode(newNodeStr);
                //    node.ParentNode.ReplaceChild(newNode, node);
                //    node.RemoveAll();
                //}
            }

            if (!string.IsNullOrEmpty(TitleDocument))
            {
                // title Document
                string newContent = "<div class='TitleDocument'>" + TitleDocument + "</div>";
                HtmlNode newNode = HtmlNode.CreateNode(newContent);
                // Get body node
                HtmlNode body = html.DocumentNode.SelectSingleNode("//body");
                // Add new node as first child of body
                body.PrependChild(newNode);
            }

            var findiv = html.DocumentNode.SelectNodes("//div");
            if (findiv != null)
            {

                foreach (HtmlNode nodeDiv in findiv)
                {

                    //type input txt
                    var findText = nodeDiv.SelectNodes("//input");
                    if (findText != null)
                    {
                        if (findText.Count > 0)
                        {
                            foreach (HtmlNode node in findText)
                            {
                                if (node.Attributes.Count > 0)
                                {
                                    string ClassAtr = string.Empty;
                                    if (node.Attributes["class"] != null)
                                    {
                                        ClassAtr = node.Attributes["class"].Value;
                                    }
                                    string controlID = string.Empty;
                                    if (node.Attributes["controlID"] != null)
                                    {
                                        controlID = node.Attributes["controlID"].Value;
                                    }

                                    if (ClassAtr == "txtForm")
                                    {

                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    string a = Value[1];
                                                    //node.InnerHtml = Value[1];
                                                    HtmlNode b = html.CreateElement("b");
                                                    b.InnerHtml = HtmlDocument.HtmlEncode(Value[1]);
                                                    node.ParentNode.InsertAfter(b, node);
                                                    node.RemoveAll();
                                                }
                                            }

                                        }

                                    }
                                    else if (ClassAtr == "optForm")
                                    {

                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    if (Value[1] == "1")
                                                    {
                                                        //node.SetAttributeValue("checked", "true");
                                                        //node.InnerHtml = Value[1];
                                                        HtmlNode b = html.CreateElement("b");
                                                        b.InnerHtml = HtmlDocument.HtmlEncode("√");
                                                        node.ParentNode.InsertAfter(b, node);
                                                        node.RemoveAll();
                                                    }

                                                }
                                                // node.SetAttributeValue("disabled", "disabled");
                                            }


                                        }

                                    }
                                    else if (ClassAtr == "chkForm")
                                    {
                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    if (Value[1] == "1")
                                                    {
                                                        //node.SetAttributeValue("checked", "true");
                                                        string a = Value[1];
                                                        HtmlNode b = html.CreateElement("b");
                                                        b.InnerHtml = HtmlDocument.HtmlEncode("√");
                                                        node.ParentNode.AppendChild(b);
                                                        node.ParentNode.RemoveChild(node, true);

                                                    }
                                                    //node.SetAttributeValue("disabled", "disabled");
                                                }
                                            }

                                        }
                                    }
                                }
                                node.SetAttributeValue("readonly", "true");
                            }
                        }

                    }
                    //text area
                    var findTextArea = nodeDiv.SelectNodes("//textarea");
                    if (findTextArea != null)
                    {
                        if (findTextArea.Count > 0)
                        {
                            foreach (HtmlNode node in findTextArea)
                            {
                                if (node.Attributes.Count > 0)
                                {
                                    string ClassAtr = node.Attributes["class"].Value;
                                    string controlID = node.Attributes["controlID"].Value;
                                    if (ClassAtr == "txtForm")
                                    {

                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    //node.InnerHtml = Value[1];
                                                    string a = Value[1];
                                                    HtmlNode b = html.CreateElement("b");
                                                    b.InnerHtml = HtmlDocument.HtmlEncode(Value[1]);
                                                    node.ParentNode.AppendChild(b);
                                                    node.ParentNode.RemoveChild(node, true);

                                                }
                                            }

                                        }

                                    }
                                }

                                node.SetAttributeValue("readonly", "true");
                            }
                        }
                    }

                    // Select Option
                    var FindTd = nodeDiv.SelectNodes("//td");
                    if (FindTd.Count > 0)
                    {
                        foreach (HtmlNode nodeTd in FindTd)
                        {
                            var FindSelectOption = nodeTd.SelectNodes("//select");
                            if (FindSelectOption != null)
                            {
                                if (FindSelectOption.Count > 0)
                                {
                                    foreach (HtmlNode node in FindSelectOption)
                                    {
                                        string ClassAtr = string.Empty;
                                        if (node.Attributes["class"] != null)
                                        {
                                            ClassAtr = node.Attributes["class"].Value;
                                        }

                                        string controlID = string.Empty;
                                        if (node.Attributes["controlID"] != null)
                                        {
                                            controlID = node.Attributes["controlID"].Value;
                                        }
                                        if (ClassAtr == "ddlForm")
                                        {
                                            if (ParamValue.Length > 0)
                                            {
                                                for (int i = 0; i < ParamValue.Length; i++)
                                                {
                                                    string[] Value = ParamValue[i].Split('=');
                                                    if (controlID == Value[0])
                                                    {
                                                        string valueText = string.Empty;

                                                        foreach (HtmlNode opt in node.SelectNodes("option"))
                                                        {

                                                            var optValue = opt.Attributes["value"];
                                                            var optSelected = opt.Attributes["selected"];
                                                            if (optSelected != null)
                                                            {
                                                                if (string.IsNullOrWhiteSpace(optSelected.Value))
                                                                {
                                                                    opt.Attributes.Remove(optSelected);
                                                                }
                                                            }
                                                            if (optValue != null)
                                                            {
                                                                string optResult = optValue.Value;

                                                                if (optResult == Value[1])
                                                                {
                                                                    valueText = opt.NextSibling.InnerHtml;
                                                                    //opt.SetAttributeValue("selected", "selected");
                                                                }

                                                            }

                                                            opt.RemoveAll();

                                                        }

                                                        if (!string.IsNullOrWhiteSpace(valueText))
                                                        {
                                                            HtmlNode b = html.CreateElement("b");
                                                            b.InnerHtml = HtmlDocument.HtmlEncode(valueText);
                                                            node.ParentNode.InsertAfter(b, node);
                                                            node.RemoveAll();
                                                        }
                                                    }
                                                }

                                            }

                                        }

                                    }

                                }
                            }
                        }
                    }


                }

                Content = html.DocumentNode.OuterHtml;
                Content = Regex.Replace(Content, "<html>", "");
                Content = Regex.Replace(Content, "</html>", "");
                Content = Regex.Replace(Content, "<body>", "");
                Content = Regex.Replace(Content, "</body>", "");
                Content = Regex.Replace(Content, "<head>", "");
                Content = Regex.Replace(Content, "</head>", "");
                Content = Regex.Replace(Content, "<script(.+?)*</script>", "");

                FinalContent = string.Format("<section class='sheet padding-10mm'><article>{0}</article> </section>", Content);
            }


            return FinalContent;

        }

        public static string ParsingFormElektronikGetTagResult(String HtmlForm, String ValueForm, String TitleDocument = "")
        {
            List<FormQuestionModel> dataForm = new List<FormQuestionModel>();
            string oResult = string.Empty;
            string Content = string.Empty;
            string FinalContent = string.Empty;
            string FormLayout = HtmlForm.ToString();
            string[] ParamValue = ValueForm.Split(';');
            
            StringBuilder sb = new StringBuilder();

            var html = new HtmlDocument();
            html.LoadHtml(FormLayout);

            //remove script 
            var RmScriptJS = html.DocumentNode.SelectNodes("//script");
            // Make sure not Null:
            if (RmScriptJS != null)
            {
                // Remove all Nodes:
                foreach (HtmlNode node in RmScriptJS)
                { node.RemoveAll(); }
            }

            if (!string.IsNullOrEmpty(TitleDocument))
            {
                // title Document
                string newContent = "<div class='TitleDocument'>" + TitleDocument + "</div>";
                HtmlNode newNode = HtmlNode.CreateNode(newContent);
                // Get body node
                HtmlNode body = html.DocumentNode.SelectSingleNode("//body");
                // Add new node as first child of body
                body.PrependChild(newNode);
            }
            var findiv = html.DocumentNode.SelectNodes("//div");
            if (findiv != null)
            {

                foreach (HtmlNode nodeDiv in findiv)
                {

                    //type input txt
                    var findText = nodeDiv.SelectNodes("//input");
                    if (findText != null)
                    {
                        if (findText.Count > 0)
                        {
                            foreach (HtmlNode node in findText)
                            {
                                if (node.Attributes.Count > 0)
                                {
                                    string ClassAtr = string.Empty;
                                    if (node.Attributes["class"] != null)
                                    {
                                        ClassAtr = node.Attributes["class"].Value;
                                    }
                                    string controlID = string.Empty;
                                    if (node.Attributes["controlID"] != null)
                                    {
                                        controlID = node.Attributes["controlID"].Value;
                                    }
                                    string labelname = string.Empty;
                                    if (node.Attributes["labelname"] != null)
                                    {
                                        labelname = node.Attributes["labelname"].Value;

                                    }
                                    string sortid = string.Empty; 
                                    if (node.Attributes["sortid"] != null) {
                                        sortid = node.Attributes["sortid"].Value; 
                                    }
                                    if (ClassAtr == "txtForm")
                                    {

                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    if (!string.IsNullOrEmpty(labelname)) {

                                                       FormQuestionModel entityData  =  dataForm.Where(p => p.QuestionName == labelname).FirstOrDefault();
                                                       if (entityData == null) {
                                                           sb.Append(string.Format("{0}^{1}={2}|", sortid, labelname, Value[1]));
                                                           FormQuestionModel entity = new FormQuestionModel();
                                                           entity.QuestionID = sortid;
                                                           entity.QuestionName = labelname.ToString().Trim();
                                                           entity.QuestionValue = Value[1];
                                                           entity.SortID = sortid;
                                                           entity.ShortName = Value[1];
                                                           dataForm.Add(entity);
                                                       }
                                                       
                                                    }
                                                    //if (!string.IsNullOrEmpty(controlLabelID))
                                                    //{
                                                    //    HtmlNode lblNode = nodeDiv.SelectSingleNode(string.Format("//label[@id=\"{0}\"]", controlLabelID));
                                                    //    if (lblNode != null)
                                                    //    {

                                                    //        string sortID = string.Empty;
                                                    //        string shortName = string.Empty;
                                                    //        if (node.Attributes != null)
                                                    //        {
                                                    //            sortID = lblNode.Attributes["sortid"].Value;
                                                    //            shortName = lblNode.Attributes["shortname"].Value;
                                                                
                                                    //        }

                                                    //        FormQuestionModel lstDataForm = dataForm.Where(p => p.QuestionID == controlLabelID).FirstOrDefault();
                                                    //        if (lstDataForm == null)
                                                    //        {

                                                    //            FormQuestionModel entity = new FormQuestionModel();
                                                    //            entity.QuestionID = controlLabelID;
                                                    //            entity.QuestionName = lblNode.InnerHtml.ToString().Trim();
                                                    //            entity.QuestionValue = Value[1];
                                                    //            entity.SortID = sortID;
                                                    //            entity.ShortName = shortName;
                                                    //            dataForm.Add(entity);

                                                    //            sb.Append(string.Format("{0}={1}|", entity.ShortName, entity.QuestionValue));
                                                    //        }

                                                    //    }
                                                    //}
                                                }
                                            }

                                        }

                                    }
                                    else if (ClassAtr == "optForm")
                                    {

                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    if (Value[1] == "1")
                                                    {
                                                        if (!string.IsNullOrEmpty(labelname))
                                                        {
                                                            if (node.Attributes != null)
                                                            {
                                                               
                                                                FormQuestionModel entityData = dataForm.Where(p => p.QuestionName == labelname).FirstOrDefault();
                                                                if (entityData == null)
                                                                {
                                                                    string tagvalue = node.Attributes["tagvalue"].Value;
                                                                    sb.Append(string.Format("{0}^{1}={2}|", sortid, labelname, tagvalue));

                                                                    FormQuestionModel entity = new FormQuestionModel();
                                                                    entity.QuestionID = sortid;
                                                                    entity.QuestionName = labelname.ToString().Trim();
                                                                    entity.QuestionValue = Value[1];
                                                                    entity.SortID = sortid;
                                                                    entity.ShortName = Value[1];
                                                                    dataForm.Add(entity);
                                                                }
                                                            }
                                                           
                                                        }
                                                        //if (!string.IsNullOrEmpty(controlLabelID))
                                                        //{
                                                        //    HtmlNode lblNode = nodeDiv.SelectSingleNode(string.Format("//label[@id=\"{0}\"]", controlLabelID));
                                                        //    if (lblNode != null)
                                                        //    {
                                                        //        string sortID = string.Empty;
                                                        //        string shortName = string.Empty;
                                                        //        if (node.Attributes != null)
                                                        //        {
                                                        //            sortID = lblNode.Attributes["sortid"].Value;
                                                        //            shortName = lblNode.Attributes["shortname"].Value;
                                                        //        }
                                                        //        string tagvalue = string.Empty;
                                                        //        if (node.Attributes != null)
                                                        //        {
                                                        //            tagvalue = node.Attributes["tagvalue"].Value;
                                                        //        }
                                                        //        FormQuestionModel lstDataForm = dataForm.Where(p => p.QuestionID == controlLabelID).FirstOrDefault();
                                                        //        if (lstDataForm == null)
                                                        //        {

                                                        //            FormQuestionModel entity = new FormQuestionModel();
                                                        //            entity.QuestionID = controlLabelID;
                                                        //            entity.QuestionName = lblNode.InnerHtml.ToString().Trim();
                                                        //            entity.QuestionValue = tagvalue;
                                                        //            entity.SortID = sortID;
                                                        //            entity.ShortName = shortName;
                                                        //            dataForm.Add(entity);

                                                        //            sb.Append(string.Format("{0}={1}|", entity.ShortName, entity.QuestionValue));
                                                        //        }

                                                        //    }
                                                        //}


                                                    }

                                                }

                                            }
                                        }

                                    }
                                    else if (ClassAtr == "chkForm")
                                    {
                                        if (ParamValue.Length > 0)
                                        {
                                            string value1 = string.Empty;
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    if (Value[1] == "1")
                                                    {
                                                        string tagvalue = string.Empty;
                                                        if (node.Attributes != null)
                                                        {
                                                          

                                                            FormQuestionModel entityData = dataForm.Where(p => p.QuestionName == labelname).FirstOrDefault();
                                                            if (entityData == null)
                                                            {
                                                                tagvalue = node.Attributes["tagvalue"].Value;
                                                                sb.Append(string.Format("{0}^{1}={2}|", sortid, labelname, tagvalue));

                                                                FormQuestionModel entity = new FormQuestionModel();
                                                                entity.QuestionID = sortid;
                                                                entity.QuestionName = labelname.ToString().Trim();
                                                                entity.QuestionValue = Value[1];
                                                                entity.SortID = sortid;
                                                                entity.ShortName = Value[1];
                                                                dataForm.Add(entity);
                                                            }
                                                        }

                                                      
                                                        //if (!string.IsNullOrEmpty(controlLabelID))
                                                        //{
                                                        //    HtmlNode lblNode = nodeDiv.SelectSingleNode(string.Format("//label[@id=\"{0}\"]", controlLabelID));
                                                        //    if (lblNode != null)
                                                        //    {
                                                        //        string sortID = string.Empty;
                                                        //        string shortName = string.Empty;
                                                        //        if (node.Attributes != null)
                                                        //        {
                                                        //            sortID = lblNode.Attributes["sortid"].Value;
                                                        //            shortName = lblNode.Attributes["shortname"].Value;
                                                        //        }
                                                        //        string tagvalue = string.Empty;
                                                        //        if (node.Attributes != null)
                                                        //        {
                                                        //            tagvalue = node.Attributes["tagvalue"].Value;
                                                        //        }

                                                        //        FormQuestionModel lstDataForm = dataForm.Where(p => p.QuestionID == controlLabelID).FirstOrDefault();
                                                        //        if (lstDataForm == null)
                                                        //        {

                                                        //            FormQuestionModel entity = new FormQuestionModel();
                                                        //            entity.QuestionID = controlLabelID;
                                                        //            entity.QuestionName = lblNode.InnerHtml.ToString().Trim();
                                                        //            entity.QuestionValue = tagvalue;
                                                        //            entity.SortID = sortID;
                                                        //            entity.ShortName = shortName;
                                                        //            dataForm.Add(entity);
                                                        //            value1 = tagvalue; 
                                                        //           sb.Append(string.Format("{0}={1}|", entity.ShortName, entity.QuestionValue));
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            string value = string.Format("{0},{1}", tagvalue, lstDataForm.QuestionValue);
                                                        //            value1 = value;
                                                        //            string currentValue = lstDataForm.QuestionValue;
                                                        //            bool containsSearchResult = currentValue.Contains(tagvalue);
                                                        //            if (containsSearchResult == false)
                                                        //            {
                                                        //                lstDataForm.QuestionValue = value;
                                                        //                dataForm.Add(lstDataForm);
                                                        //                sb.Append(string.Format("{0}={1}|", lstDataForm.ShortName, lstDataForm.QuestionValue));

                                                        //            }
                                                        //        }

                                                        //    }
                                                        //}

                                                    }

                                                }
                                            }
                                            
                                        }
                                    }
                                }
                                //node.SetAttributeValue("readonly", "true");
                            }
                        }

                    }
                    //text area
                    var findTextArea = nodeDiv.SelectNodes("//textarea");
                    if (findTextArea != null)
                    {
                        if (findTextArea.Count > 0)
                        {
                            foreach (HtmlNode node in findTextArea)
                            {
                                if (node.Attributes.Count > 0)
                                {
                                    string ClassAtr = node.Attributes["class"].Value;
                                    string controlID = node.Attributes["controlID"].Value;

                                    string labelname = string.Empty;

                                    if (node.Attributes["labelname"] != null)
                                    {
                                        labelname = node.Attributes["labelname"].Value;

                                    }
                                    string sortid = string.Empty;
                                    if (node.Attributes["sortid"] != null)
                                    {
                                        sortid = node.Attributes["sortid"].Value;
                                    }
                                    if (ClassAtr == "txtForm")
                                    {

                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    ////sb.Append(string.Format("{0}={1}|", labelname, Value[1]));
                                                   

                                                    FormQuestionModel entityData = dataForm.Where(p => p.QuestionName == labelname).FirstOrDefault();
                                                    if (entityData == null)
                                                    {
                                                        
                                                        sb.Append(string.Format("{0}^{1}={2}|", sortid, labelname, Value[1]));

                                                        FormQuestionModel entity = new FormQuestionModel();
                                                        entity.QuestionID = sortid;
                                                        entity.QuestionName = labelname.ToString().Trim();
                                                        entity.QuestionValue = Value[1];
                                                        entity.SortID = sortid;
                                                        entity.ShortName = Value[1];
                                                        dataForm.Add(entity);
                                                    }
                                                    //if (!string.IsNullOrEmpty(controlLabelID))
                                                    //{
                                                    //    HtmlNode lblNode = nodeDiv.SelectSingleNode(string.Format("//label[@id=\"{0}\"]", controlLabelID));

                                                    //    if (lblNode != null)
                                                    //    {
                                                    //        string sortID = string.Empty;
                                                    //        string shortName = string.Empty;
                                                    //        if (node.Attributes != null)
                                                    //        {
                                                    //            sortID = lblNode.Attributes["sortid"].Value;
                                                    //            shortName = lblNode.Attributes["shortname"].Value;
                                                    //        }

                                                    //        List<FormQuestionModel> lstDataForm = dataForm.Where(p => p.QuestionID == controlID).ToList();
                                                    //        if (lstDataForm.Count == 0)
                                                    //        {

                                                    //            FormQuestionModel entity = new FormQuestionModel();
                                                    //            entity.QuestionID = controlID;
                                                    //            entity.QuestionName = lblNode.InnerHtml.ToString().Trim();
                                                    //            entity.QuestionValue = Value[1];
                                                    //            entity.SortID = sortID;
                                                    //            entity.ShortName = shortName;
                                                    //            dataForm.Add(entity);

                                                    //            sb.Append(string.Format("{0}={1}|", entity.ShortName, entity.QuestionValue));
                                                    //        }
                                                    //    }
                                                    //}


                                                }
                                            }

                                        }

                                    }
                                }

                                node.SetAttributeValue("readonly", "true");
                            }
                        }
                    }

                    // Select Option
                    var FindTd = nodeDiv.SelectNodes("//td");
                    if (FindTd.Count > 0)
                    {
                        foreach (HtmlNode nodeTd in FindTd)
                        {
                            var FindSelectOption = nodeTd.SelectNodes("//select");
                            if (FindSelectOption != null)
                            {
                                if (FindSelectOption.Count > 0)
                                {
                                    foreach (HtmlNode node in FindSelectOption)
                                    {
                                        string ClassAtr = string.Empty;
                                        if (node.Attributes["class"] != null)
                                        {
                                            ClassAtr = node.Attributes["class"].Value;
                                        }

                                        string controlID = string.Empty;
                                        if (node.Attributes["controlID"] != null)
                                        {
                                            controlID = node.Attributes["controlID"].Value;
                                        }
                                        string labelname = string.Empty;
                                        if (node.Attributes["labelname"] != null)
                                        {
                                            labelname = node.Attributes["labelname"].Value;
                                        }
                                        string sortid = string.Empty;
                                        if (node.Attributes["sortid"] != null)
                                        {
                                            sortid = node.Attributes["sortid"].Value;
                                        }
                                        if (ClassAtr == "ddlForm")
                                        {
                                            if (ParamValue.Length > 0)
                                            {
                                                for (int i = 0; i < ParamValue.Length; i++)
                                                {
                                                    string[] Value = ParamValue[i].Split('=');
                                                    if (controlID == Value[0])
                                                    {
                                                        string valueText = string.Empty;

                                                        foreach (HtmlNode opt in node.SelectNodes("option"))
                                                        {

                                                            var optValue = opt.Attributes["value"];
                                                            var optSelected = opt.Attributes["selected"];
                                                            if (optSelected != null)
                                                            {
                                                                if (string.IsNullOrWhiteSpace(optSelected.Value))
                                                                {
                                                                    opt.Attributes.Remove(optSelected);
                                                                }
                                                            }
                                                            if (optValue != null)
                                                            {
                                                                string optResult = optValue.Value;

                                                                if (optResult == Value[1])
                                                                {
                                                                   // valueText = opt.NextSibling.InnerHtml;
                                                                    valueText = optResult;
                                                                    //opt.SetAttributeValue("selected", "selected");
                                                                }

                                                            }

                                                            opt.RemoveAll();

                                                        }

                                                        if (!string.IsNullOrWhiteSpace(valueText))
                                                        {
                                                           
                                                            FormQuestionModel entityData = dataForm.Where(p => p.QuestionName == labelname).FirstOrDefault();
                                                            if (entityData == null)
                                                            {

                                                                sb.Append(string.Format("{0}^{1}={2}|", sortid, labelname, Value[1]));

                                                                FormQuestionModel entity = new FormQuestionModel();
                                                                entity.QuestionID = sortid;
                                                                entity.QuestionName = labelname.ToString().Trim();
                                                                entity.QuestionValue = Value[1];
                                                                entity.SortID = sortid;
                                                                entity.ShortName = Value[1];
                                                                dataForm.Add(entity);
                                                            }

                                                            //FormQuestionModel entity = new FormQuestionModel();
                                                            //entity.QuestionID = sortid;
                                                            //entity.QuestionName = labelname.ToString().Trim();
                                                            //entity.QuestionValue = Value[1];
                                                            //entity.SortID = sortid;
                                                            //entity.ShortName = Value[1];
                                                            //dataForm.Add(entity);

                                                            //if (!string.IsNullOrEmpty(controlLabelID))
                                                            //{
                                                            //    HtmlNode lblNode = nodeDiv.SelectSingleNode(string.Format("//label[@id=\"{0}\"]", controlLabelID));
                                                            //    if (lblNode != null)
                                                            //    {
                                                            //        string sortID = string.Empty;
                                                            //        string shortName = string.Empty;
                                                            //        if (node.Attributes != null)
                                                            //        {
                                                            //            sortID = lblNode.Attributes["sortid"].Value;
                                                            //            shortName = lblNode.Attributes["shortname"].Value;
                                                            //        }
                                                            //        List<FormQuestionModel> lstDataForm = dataForm.Where(p => p.QuestionID == controlID).ToList();
                                                            //        if (lstDataForm.Count == 0)
                                                            //        {

                                                            //            FormQuestionModel entity = new FormQuestionModel();
                                                            //            entity.QuestionID = controlID;
                                                            //            entity.QuestionName = lblNode.InnerHtml.ToString().Trim();
                                                            //            entity.QuestionValue = Value[1];
                                                            //            entity.SortID = sortID;
                                                            //            entity.ShortName = shortName;
                                                            //            dataForm.Add(entity);

                                                            //            sb.Append(string.Format("{0}={1}|", entity.ShortName, entity.QuestionValue));
                                                            //        }

                                                            //    }
                                                            //}

                                                        }
                                                    }
                                                }

                                            }

                                        }

                                    }
                                }
                            }
                        }
                    }
                }


            }
            
            
            string json = JsonConvert.SerializeObject(dataForm, Formatting.Indented);


            return  sb.ToString() ;

        }


        public static string ParsingFormElektronikGetTagField(String HtmlForm, String ValueForm, String TitleDocument = "")
        {
            List<FormQuestionModel> dataForm = new List<FormQuestionModel>();
            string oResult = string.Empty;
            string Content = string.Empty;
            string FinalContent = string.Empty;
            string FormLayout = HtmlForm.ToString();
            string[] ParamValue = ValueForm.Split(';');

            var html = new HtmlDocument();
            html.LoadHtml(FormLayout);

            //remove script 
            var RmScriptJS = html.DocumentNode.SelectNodes("//script");
            // Make sure not Null:
            if (RmScriptJS != null)
            {
                // Remove all Nodes:
                foreach (HtmlNode node in RmScriptJS)
                { node.RemoveAll(); }
            }

            if (!string.IsNullOrEmpty(TitleDocument))
            {
                // title Document
                string newContent = "<div class='TitleDocument'>" + TitleDocument + "</div>";
                HtmlNode newNode = HtmlNode.CreateNode(newContent);
                // Get body node
                HtmlNode body = html.DocumentNode.SelectSingleNode("//body");
                // Add new node as first child of body
                body.PrependChild(newNode);
            }
            var findiv = html.DocumentNode.SelectNodes("//div");
            if (findiv != null)
            {

                foreach (HtmlNode nodeDiv in findiv)
                {

                    //type input txt
                    var findText = nodeDiv.SelectNodes("//input");
                    if (findText != null)
                    {
                        if (findText.Count > 0)
                        {
                            foreach (HtmlNode node in findText)
                            {
                                if (node.Attributes.Count > 0)
                                {
                                    string ClassAtr = string.Empty;
                                    if (node.Attributes["class"] != null)
                                    {
                                        ClassAtr = node.Attributes["class"].Value;
                                    }
                                    string controlID = string.Empty;
                                    if (node.Attributes["controlID"] != null)
                                    {
                                        controlID = node.Attributes["controlID"].Value;
                                    }
                                    string controlLabelID = string.Empty;
                                    if (node.Attributes["labelControlID"] != null)
                                    {
                                        controlLabelID = node.Attributes["labelControlID"].Value;

                                    }
                                    if (ClassAtr == "txtForm")
                                    {

                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    if (!string.IsNullOrEmpty(controlLabelID))
                                                    {
                                                        HtmlNode lblNode = nodeDiv.SelectSingleNode(string.Format("//label[@id=\"{0}\"]", controlLabelID));
                                                        if (lblNode != null)
                                                        {

                                                            string sortID = string.Empty;
                                                            string shortName = string.Empty;
                                                            if (node.Attributes != null)
                                                            {
                                                                sortID = lblNode.Attributes["sortid"].Value;
                                                                shortName = lblNode.Attributes["shortname"].Value;
                                                            }

                                                            FormQuestionModel lstDataForm = dataForm.Where(p => p.QuestionID == controlLabelID).FirstOrDefault();
                                                            if (lstDataForm == null)
                                                            {

                                                                FormQuestionModel entity = new FormQuestionModel();
                                                                entity.QuestionID = controlLabelID;
                                                                entity.QuestionName = lblNode.InnerHtml.ToString().Trim();
                                                                entity.QuestionValue = Value[1];
                                                                entity.SortID = sortID;
                                                                entity.ShortName = shortName;
                                                                dataForm.Add(entity);
                                                            }

                                                        }
                                                    }
                                                }
                                            }

                                        }

                                    }
                                    else if (ClassAtr == "optForm")
                                    {

                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    if (Value[1] == "1")
                                                    {
                                                        if (!string.IsNullOrEmpty(controlLabelID))
                                                        {
                                                            HtmlNode lblNode = nodeDiv.SelectSingleNode(string.Format("//label[@id=\"{0}\"]", controlLabelID));
                                                            if (lblNode != null)
                                                            {
                                                                string sortID = string.Empty;
                                                                string shortName = string.Empty;
                                                                if (node.Attributes != null)
                                                                {
                                                                    sortID = lblNode.Attributes["sortid"].Value;
                                                                    shortName = lblNode.Attributes["shortname"].Value;
                                                                }
                                                                string tagvalue = string.Empty;
                                                                if (node.Attributes != null)
                                                                {
                                                                    tagvalue = node.Attributes["tagvalue"].Value;
                                                                }
                                                                FormQuestionModel lstDataForm = dataForm.Where(p => p.QuestionID == controlLabelID).FirstOrDefault();
                                                                if (lstDataForm == null)
                                                                {

                                                                    FormQuestionModel entity = new FormQuestionModel();
                                                                    entity.QuestionID = controlLabelID;
                                                                    entity.QuestionName = lblNode.InnerHtml.ToString().Trim();
                                                                    entity.QuestionValue = tagvalue;
                                                                    entity.SortID = sortID;
                                                                    entity.ShortName = shortName;
                                                                    dataForm.Add(entity);
                                                                }

                                                            }
                                                        }


                                                    }

                                                }

                                            }
                                        }

                                    }
                                    else if (ClassAtr == "chkForm")
                                    {
                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    if (Value[1] == "1")
                                                    {
                                                        if (!string.IsNullOrEmpty(controlLabelID))
                                                        {
                                                            HtmlNode lblNode = nodeDiv.SelectSingleNode(string.Format("//label[@id=\"{0}\"]", controlLabelID));
                                                            if (lblNode != null)
                                                            {
                                                                string sortID = string.Empty;
                                                                string shortName = string.Empty;
                                                                if (node.Attributes != null)
                                                                {
                                                                    sortID = lblNode.Attributes["sortid"].Value;
                                                                    shortName = lblNode.Attributes["shortname"].Value;
                                                                }
                                                                string tagvalue = string.Empty;
                                                                if (node.Attributes != null)
                                                                {
                                                                    tagvalue = node.Attributes["tagvalue"].Value;
                                                                }

                                                                FormQuestionModel lstDataForm = dataForm.Where(p => p.QuestionID == controlLabelID).FirstOrDefault();
                                                                if (lstDataForm == null)
                                                                {

                                                                    FormQuestionModel entity = new FormQuestionModel();
                                                                    entity.QuestionID = controlLabelID;
                                                                    entity.QuestionName = lblNode.InnerHtml.ToString().Trim();
                                                                    entity.QuestionValue = tagvalue;
                                                                    entity.SortID = sortID;
                                                                    entity.ShortName = shortName;
                                                                    dataForm.Add(entity);
                                                                }
                                                                else
                                                                {
                                                                    string value = string.Format("{0},{1}", tagvalue, lstDataForm.QuestionValue);
                                                                    string currentValue = lstDataForm.QuestionValue;
                                                                    bool containsSearchResult = currentValue.Contains(tagvalue);
                                                                    if (containsSearchResult == false)
                                                                    {
                                                                        lstDataForm.QuestionValue = value;
                                                                        dataForm.Add(lstDataForm);
                                                                    }
                                                                }

                                                            }
                                                        }

                                                    }

                                                }
                                            }

                                        }
                                    }
                                }
                                //node.SetAttributeValue("readonly", "true");
                            }
                        }

                    }
                    //text area
                    var findTextArea = nodeDiv.SelectNodes("//textarea");
                    if (findTextArea != null)
                    {
                        if (findTextArea.Count > 0)
                        {
                            foreach (HtmlNode node in findTextArea)
                            {
                                if (node.Attributes.Count > 0)
                                {
                                    string ClassAtr = node.Attributes["class"].Value;
                                    string controlID = node.Attributes["controlID"].Value;

                                    string controlLabelID = string.Empty;

                                    if (node.Attributes["labelControlID"] != null)
                                    {
                                        controlLabelID = node.Attributes["labelControlID"].Value;

                                    }

                                    if (ClassAtr == "txtForm")
                                    {

                                        if (ParamValue.Length > 0)
                                        {
                                            for (int i = 0; i < ParamValue.Length; i++)
                                            {
                                                string[] Value = ParamValue[i].Split('=');
                                                if (controlID == Value[0])
                                                {
                                                    if (!string.IsNullOrEmpty(controlLabelID))
                                                    {
                                                        HtmlNode lblNode = nodeDiv.SelectSingleNode(string.Format("//label[@id=\"{0}\"]", controlLabelID));

                                                        if (lblNode != null)
                                                        {
                                                            string sortID = string.Empty;
                                                            string shortName = string.Empty;
                                                            if (node.Attributes != null)
                                                            {
                                                                sortID = lblNode.Attributes["sortid"].Value;
                                                                shortName = lblNode.Attributes["shortname"].Value;
                                                            }

                                                            List<FormQuestionModel> lstDataForm = dataForm.Where(p => p.QuestionID == controlID).ToList();
                                                            if (lstDataForm.Count == 0)
                                                            {

                                                                FormQuestionModel entity = new FormQuestionModel();
                                                                entity.QuestionID = controlID;
                                                                entity.QuestionName = lblNode.InnerHtml.ToString().Trim();
                                                                entity.QuestionValue = Value[1];
                                                                entity.SortID = sortID;
                                                                entity.ShortName = shortName;
                                                                dataForm.Add(entity);
                                                            }
                                                        }
                                                    }


                                                }
                                            }

                                        }

                                    }
                                }

                                node.SetAttributeValue("readonly", "true");
                            }
                        }
                    }

                    // Select Option
                    var FindTd = nodeDiv.SelectNodes("//td");
                    if (FindTd.Count > 0)
                    {
                        foreach (HtmlNode nodeTd in FindTd)
                        {
                            var FindSelectOption = nodeTd.SelectNodes("//select");
                            if (FindSelectOption != null)
                            {
                                if (FindSelectOption.Count > 0)
                                {
                                    foreach (HtmlNode node in FindSelectOption)
                                    {
                                        string ClassAtr = string.Empty;
                                        if (node.Attributes["class"] != null)
                                        {
                                            ClassAtr = node.Attributes["class"].Value;
                                        }

                                        string controlID = string.Empty;
                                        if (node.Attributes["controlID"] != null)
                                        {
                                            controlID = node.Attributes["controlID"].Value;
                                        }
                                        string controlLabelID = string.Empty;
                                        if (node.Attributes["labelControlID"] != null)
                                        {
                                            controlLabelID = node.Attributes["labelControlID"].Value;
                                        }
                                        if (ClassAtr == "ddlForm")
                                        {
                                            if (ParamValue.Length > 0)
                                            {
                                                for (int i = 0; i < ParamValue.Length; i++)
                                                {
                                                    string[] Value = ParamValue[i].Split('=');
                                                    if (controlID == Value[0])
                                                    {
                                                        string valueText = string.Empty;

                                                        foreach (HtmlNode opt in node.SelectNodes("option"))
                                                        {

                                                            var optValue = opt.Attributes["value"];
                                                            var optSelected = opt.Attributes["selected"];
                                                            if (optSelected != null)
                                                            {
                                                                if (string.IsNullOrWhiteSpace(optSelected.Value))
                                                                {
                                                                    opt.Attributes.Remove(optSelected);
                                                                }
                                                            }
                                                            if (optValue != null)
                                                            {
                                                                string optResult = optValue.Value;

                                                                if (optResult == Value[1])
                                                                {
                                                                    valueText = opt.NextSibling.InnerHtml;
                                                                    //opt.SetAttributeValue("selected", "selected");
                                                                }

                                                            }

                                                            opt.RemoveAll();

                                                        }

                                                        if (!string.IsNullOrWhiteSpace(valueText))
                                                        {
                                                            if (!string.IsNullOrEmpty(controlLabelID))
                                                            {
                                                                HtmlNode lblNode = nodeDiv.SelectSingleNode(string.Format("//label[@id=\"{0}\"]", controlLabelID));
                                                                if (lblNode != null)
                                                                {
                                                                    string sortID = string.Empty;
                                                                    string shortName = string.Empty;
                                                                    if (node.Attributes != null)
                                                                    {
                                                                        sortID = lblNode.Attributes["sortid"].Value;
                                                                        shortName = lblNode.Attributes["shortname"].Value;
                                                                    }
                                                                    List<FormQuestionModel> lstDataForm = dataForm.Where(p => p.QuestionID == controlID).ToList();
                                                                    if (lstDataForm.Count == 0)
                                                                    {

                                                                        FormQuestionModel entity = new FormQuestionModel();
                                                                        entity.QuestionID = controlID;
                                                                        entity.QuestionName = lblNode.InnerHtml.ToString().Trim();
                                                                        entity.QuestionValue = Value[1];
                                                                        entity.SortID = sortID;
                                                                        entity.ShortName = shortName;
                                                                        dataForm.Add(entity);
                                                                    }

                                                                }
                                                            }

                                                        }
                                                    }
                                                }

                                            }

                                        }

                                    }
                                }
                            }
                        }
                    }
                }


            }
            string json = JsonConvert.SerializeObject(dataForm, Formatting.Indented);

            return json;

        }

        #endregion

        //public static String GenerateMRN(Int32 MRN, IDbContext ctx)
        //{
        //    string result = string.Empty;

        //    HealthcareLastNoDao entityDao = new HealthcareLastNoDao(ctx);

        //    HealthcareLastNo oLastNo = entityDao.Get(AppSession.UserLogin.HealthcareID);
        //    if (oLastNo != null)
        //    {
        //        double lastNo = Convert.ToDouble(oLastNo.MedicalNo.Replace("-", ""));
        //        result = GenerateCode(AppConfigManager.QISMRNFormat, lastNo + 1);
        //    }
        //    else
        //    {
        //        result = string.Empty;
        //    }

        //    return result;
        //}

        public static String GenerateItemCode(IDbContext ctx, String ItemName)
        {
            string itemName2Char = ItemName.Trim().Substring(0, 2).ToUpper();
            ItemMaster im = BusinessLayer.GetItemMasterList(string.Format("ItemCode LIKE '{0}%'", itemName2Char), 1, 1, "ItemCode DESC", ctx).FirstOrDefault();
            int newNumber = 1;
            if (im != null)
                newNumber = Convert.ToInt32(im.ItemCode.Substring(itemName2Char.Length)) + 1;
            return string.Format("{0}{1}", itemName2Char, newNumber.ToString().PadLeft(4, '0'));
        }

        public static String GenerateCustomerGroupCode(IDbContext ctx, String CustomerGroupName)
        {
            string customerGroupName2Char = CustomerGroupName.Trim().Substring(0, 2).ToUpper();
            CustomerGroup cg = BusinessLayer.GetCustomerGroupList(string.Format("CustomerGroupCode LIKE '{0}%'", customerGroupName2Char), 1, 1, "CustomerGroupCode DESC", ctx).FirstOrDefault();
            int newNumber = 1;
            if (cg != null)
                newNumber = Convert.ToInt32(cg.CustomerGroupCode.Substring(customerGroupName2Char.Length)) + 1;
            return string.Format("{0}{1}", customerGroupName2Char, newNumber.ToString().PadLeft(5, '0'));
        }

        public static String GeneratePartnerCode(IDbContext ctx, String BusinessPartnerName)
        {
            string businessPartnerName2Char = BusinessPartnerName.Trim().Substring(0, 2).ToUpper();
            BusinessPartners bp = BusinessLayer.GetBusinessPartnersList2(string.Format("BusinessPartnerCode LIKE '{0}%' AND BusinessPartnerCode != 'PERSONAL'", businessPartnerName2Char), 1, 1, "BusinessPartnerCode DESC", ctx).FirstOrDefault();
            int newNumber = 1;
            if (bp != null)
                newNumber = Convert.ToInt32(bp.BusinessPartnerCode.Substring(businessPartnerName2Char.Length)) + 1;
            return string.Format("{0}{1}", businessPartnerName2Char, newNumber.ToString().PadLeft(5, '0'));
        }

        public static String GenerateTestTemplateCode(IDbContext ctx, String TestTemplateName)
        {
            string TestTemplateName2Char = TestTemplateName.Trim().Substring(0, 2).ToUpper();
            TestTemplateHd bp = BusinessLayer.GetTestTemplateHdList2(string.Format("TestTemplateCode LIKE '{0}%'", TestTemplateName2Char), 1, 1, "TestTemplateCode DESC", ctx).FirstOrDefault();
            int newNumber = 1;
            if (bp != null)
                newNumber = Convert.ToInt32(bp.TestTemplateCode.Substring(TestTemplateName2Char.Length)) + 1;
            return string.Format("{0}{1}", TestTemplateName2Char, newNumber.ToString().PadLeft(5, '0'));
        }

        public static String GenerateChargesTemplateCode(IDbContext ctx, String ChargesTemplateName)
        {
            string ChargesTemplateName2Char = ChargesTemplateName.Trim().Substring(0, 2).ToUpper();
            ChargesTemplateHd bp = BusinessLayer.GetChargesTemplateHdList2(string.Format("ChargesTemplateCode LIKE '{0}%'", ChargesTemplateName2Char), 1, 1, "ChargesTemplateCode DESC", ctx).FirstOrDefault();
            int newNumber = 1;
            if (bp != null)
                newNumber = Convert.ToInt32(bp.ChargesTemplateCode.Substring(ChargesTemplateName2Char.Length)) + 1;
            return string.Format("{0}{1}", ChargesTemplateName2Char, newNumber.ToString().PadLeft(5, '0'));
        }

        public static String GenerateProcedureGroupCode(IDbContext ctx, String ProcedureGroupName)
        {
            string ProcedureGroupName2Char = ProcedureGroupName.Trim().Substring(0, 2).ToUpper();
            ProcedureGroup bp = BusinessLayer.GetProcedureGroupList2(string.Format("ProcedureGroupCode LIKE '{0}%'", ProcedureGroupName2Char), 1, 1, "ProcedureGroupCode DESC", ctx).FirstOrDefault();
            int newNumber = 1;
            if (bp != null)
                newNumber = Convert.ToInt32(bp.ProcedureGroupCode.Substring(ProcedureGroupName2Char.Length)) + 1;
            return string.Format("{0}{1}", ProcedureGroupName2Char, newNumber.ToString().PadLeft(3, '0'));
        }

        public static String GenerateProcedurePanelCode(IDbContext ctx, String ProcedurePanelName)
        {
            string ProcedurePanelName2Char = ProcedurePanelName.Trim().Substring(0, 2).ToUpper();
            ProcedurePanelHd bp = BusinessLayer.GetProcedurePanelHdList2(string.Format("ProcedurePanelCode LIKE '{0}%'", ProcedurePanelName2Char), 1, 1, "ProcedurePanelCode DESC", ctx).FirstOrDefault();
            int newNumber = 1;
            if (bp != null)
                newNumber = Convert.ToInt32(bp.ProcedurePanelCode.Substring(ProcedurePanelName2Char.Length)) + 1;
            return string.Format("{0}{1}", ProcedurePanelName2Char, newNumber.ToString().PadLeft(3, '0'));
        }

        public static String GenerateProductBrandCode(IDbContext ctx, String ProductBrandName)
        {
            string ProductBrandName2Char = ProductBrandName.Trim().Substring(0, 2).ToUpper();
            ProductBrand bp = BusinessLayer.GetProductBrandList2(string.Format("ProductBrandCode LIKE '{0}%'", ProductBrandName2Char), 1, 1, "ProductBrandCode DESC", ctx).FirstOrDefault();
            int newNumber = 1;
            if (bp != null)
                newNumber = Convert.ToInt32(bp.ProductBrandCode.Substring(ProductBrandName2Char.Length)) + 1;
            return string.Format("{0}{1}", ProductBrandName2Char, newNumber.ToString().PadLeft(5, '0'));
        }

        public static String GenerateCode(String formatCode, double ID)
        {
            int count = 0;
            for (int i = 0; i < formatCode.Length; ++i)
                if (formatCode[i] == '*')
                    count++;

            string tempCode = ID.ToString().PadLeft(count, '0');
            StringBuilder result = new StringBuilder();

            int ctrTempCode = 0;
            for (int i = 0; i < formatCode.Length; ++i)
            {
                char c = formatCode[i];
                if (c == '*')
                    result.Append(tempCode[ctrTempCode++]);
                else
                    result.Append(c);
            }
            return result.ToString();
        }

        public static String NumberInWords(Int64 amount, Boolean isMoney = false)
        {
            StringBuilder strbuild;
            if (isMoney)
                strbuild = new StringBuilder("RUPIAH");
            else
                strbuild = new StringBuilder();

            String[] arrBil = { "", "SATU ", "DUA ", "TIGA ", "EMPAT ", "LIMA ", "ENAM ", "TUJUH ", "DELAPAN ", "SEMBILAN ", "SE" };
            String[] arrSatKecil = { "", "PULUH ", "RATUS " };
            String[] arrSatBesar = { "", "RIBU ", "JUTA ", "MILYAR " };
            int ctrKecil = 0;
            int ctrBesar = 0;
            if (amount == 0)
            {
                if (isMoney)
                    return "NOL RUPIAH";
                else
                    return "NOL";
            }
            else
            {
                while (amount > 0)
                {
                    long a = amount % 10;
                    amount /= 10;

                    if (a > 0)
                        strbuild.Insert(0, arrSatKecil[ctrKecil]);

                    if (a == 1 && ctrKecil > 0)
                        strbuild.Insert(0, arrBil[10]);
                    else if (ctrKecil == 0 && amount % 10 == 1 && a > 0)
                    {
                        strbuild.Insert(0, "BELAS ");
                        if (a == 1)
                            a = 10;
                        strbuild.Insert(0, arrBil[a]);
                        amount /= 10;
                        ctrKecil++;
                    }
                    else
                        strbuild.Insert(0, arrBil[a]);

                    ctrKecil++;
                    if (ctrKecil % 3 == 0)
                    {
                        ctrBesar++;
                        ctrKecil = 0;
                        if (amount > 0 && amount % 1000 > 0)
                        {
                            strbuild.Insert(0, arrSatBesar[ctrBesar]);
                        }
                    }

                }
                return strbuild.ToString();
            }
        }

        public static String NumberInWordsInEnglish(Int64 amount, Boolean isMoney = false)
        {
            if (amount == 0)
                return "ZERO";

            if (amount < 0)
                return "MINUS " + NumberInWordsInEnglish(Math.Abs(amount));

            string words = "";

            if ((amount / 1000000000) > 0)
            {
                words += NumberInWordsInEnglish(amount / 1000000000) + " BILLION ";
                amount %= 1000000;
            }

            if ((amount / 1000000) > 0)
            {
                words += NumberInWordsInEnglish(amount / 1000000) + " MILLION ";
                amount %= 1000000;
            }

            if ((amount / 1000) > 0)
            {
                words += NumberInWordsInEnglish(amount / 1000) + " THOUSAND ";
                amount %= 1000;
            }

            if ((amount / 100) > 0)
            {
                words += NumberInWordsInEnglish(amount / 100) + " HUNDRED ";
                amount %= 100;
            }

            if (amount > 0)
            {
                if (words != "")
                    words += "AND ";

                var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", 
                        "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
                var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

                if (amount < 20)
                    words += unitsMap[amount];
                else
                {
                    words += tensMap[amount / 10];
                    if ((amount % 10) > 0)
                        words += "-" + unitsMap[amount % 10];
                }
            }

            if (isMoney)
            {
                words += " RUPIAH";
            }

            return words;
        }

        #region New Convert Number In Indonesian Words

        public static String NumberWithPointInWordsInIndonesian(Double amount, Boolean isMoney = false)
        {
            StringBuilder strbuild = new StringBuilder();

            string[] amountInSplit = amount.ToString().Split('.');


            String[] arrBil = { "", "SATU ", "DUA ", "TIGA ", "EMPAT ", "LIMA ", "ENAM ", "TUJUH ", "DELAPAN ", "SEMBILAN ", "SE" };
            String[] arrSatKecil = { "", "PULUH ", "RATUS " };
            String[] arrSatBesar = { "", "RIBU ", "JUTA ", "MILYAR ", "TRILIUN ", "KUADRILIUN " };
            int ctrKecil = 0;
            int ctrBesar = 0;

            Int64 amountNew = Convert.ToInt64(amountInSplit[0]); // angka di depan koma desimal

            bool flagMinus = false;

            if (amountNew < 0)
            {
                amountNew = amountNew * -1;
                flagMinus = true;
            }

            if (amountNew == 0)
            {
                if (isMoney)
                    return "NOL RUPIAH";
                else
                    return "NOL";
            }
            else
            {
                while (amountNew > 0)
                {
                    long a = amountNew % 10;
                    amountNew /= 10;

                    if (a > 0)
                    {
                        strbuild.Insert(0, arrSatKecil[ctrKecil]);
                    }

                    if (a == 1 && ctrKecil > 0)
                    {
                        strbuild.Insert(0, arrBil[10]);
                    }
                    else if (ctrKecil == 0 && amountNew % 10 == 1 && a > 0)
                    {
                        strbuild.Insert(0, "BELAS ");
                        if (a == 1)
                            a = 10;
                        strbuild.Insert(0, arrBil[a]);
                        amountNew /= 10;
                        ctrKecil++;
                    }
                    else
                    {
                        strbuild.Insert(0, arrBil[a]);
                    }

                    ctrKecil++;
                    if (ctrKecil % 3 == 0)
                    {
                        ctrBesar++;
                        ctrKecil = 0;
                        if (amountNew > 0 && amountNew % 1000 > 0)
                        {
                            strbuild.Insert(0, arrSatBesar[ctrBesar]);
                        }
                    }
                }

                if (flagMinus)
                {
                    strbuild.Insert(0, "MINUS ");
                }

                #region Point
                Int64 amountPointNew = 0;
                string amountDecimal1 = "0";
                string amountDecimal2 = "0";

                if (amountInSplit.Count() > 1)
                {
                    amountPointNew = Convert.ToInt64(amountInSplit[1]); // angka di belakang koma desimal

                    if (amountInSplit[1].Length > 1)
                    {
                        amountDecimal1 = amountInSplit[1].Substring(0, 1);
                        amountDecimal2 = amountInSplit[1].Substring(1, 1);
                    }
                    else
                    {
                        amountDecimal1 = amountInSplit[1].Substring(0, 1);
                    }

                    if (amountPointNew > 0)
                    {
                        int point1 = Convert.ToInt32(amountDecimal1);
                        int point2 = Convert.ToInt32(amountDecimal2);

                        strbuild.Append(" KOMA ");

                        if (point1 == 0)
                        {
                            strbuild.Append("NOL ");
                        }
                        else
                        {
                            strbuild.Append(arrBil[point1]);
                        }

                        if (point2 == 0)
                        {
                            //strbuild.Append("NOL");
                        }
                        else
                        {
                            strbuild.Append(arrBil[point2]);
                        }
                    }
                }
                #endregion

                strbuild.Append(" RUPIAH");

                return strbuild.ToString();
            }
        }

        #endregion

        #region New Convert Number In English Words (from https://www.codeproject.com/Lounge.aspx?msg=4667570#xx4667570xx)

        private enum Digit
        {
            ZERO = 0, ONE = 1, TWO = 2, THREE = 3, FOUR = 4, FIVE = 5, SIX = 6, SEVEN = 7, EIGHT = 8, NINE = 9
        }

        private enum Teen
        {
            TEN = 10, ELEVEN = 11, TWELVE = 12, THRITEEN = 13, FOURTEEN = 14, FIFTEEN = 15, SIXTEEN = 16, SEVENTEEN = 17, EIGHTEEN = 18, NINETEEN = 19
        }

        private enum Ten
        {
            TWENTY = 2, THIRTY = 3, FORTY = 4, FIFTY = 5, SIXTY = 6, SEVENTY = 7, EIGHTY = 8, NINETY = 9
        }

        private enum PowerOfTen
        {
            HUNDRED = 0, THOUSAND = 1, MILLION = 2, BILLION = 3, TRILLION = 4, QUADRILLION = 5, QUINTILLION = 6
        }

        /// <summary>
        /// How many powers of ten there are; faster to work this out ahead of time,
        /// and I didn't want to hard-code it into the algorithm...
        /// </summary>
        private static int PowersOfTen = Enum.GetValues(typeof(PowerOfTen)).Length;

        /// <summary>
        /// Converts a number to English words
        /// </summary>
        /// <param name="N">The number</param>
        /// <returns>The number, in English</returns>
        public static string NumberWithPointInWordsInEnglish(Double N, Boolean isMoney = false)
        {
            string Prefix = N < 0 ? "MINUS " : "";
            string Significand = Digit.ZERO.ToString();
            string Mantissa = "";
            if ((N = Math.Abs(N)) > 0)
            {
                // Do the Mantissa
                if (N != Math.Floor(N))
                {
                    Mantissa = " POINT";
                    foreach (char C in N.ToString().Substring(N.ToString().IndexOf('.') + 1))
                        Mantissa += " " + ((Digit)(int.Parse(C.ToString())));
                }

                // Figure out the bit of the Significand less than 100
                long n = Convert.ToInt64(N = Math.Floor(N)) % 100;
                Significand = n == 0 ? ""
                  : n < 10 ? ((Digit)n).ToString()
                  : n < 20 ? ((Teen)n).ToString()
                  : ((Digit)(n % 10)) != 0 ? ((Ten)(n / 10) + "-" + (Digit)(n % 10)).ToString()
                  : ((Ten)(n / 10)).ToString();

                // Do the other powers of 10, if there are any
                if ((N = Math.Floor(N / 100D)) > 0)
                {
                    string EW = "";
                    for (int i = 0; (N > 0) && (i < PowersOfTen); i++)
                    {
                        double p = Math.Pow(10, (i << 1) + 1);
                        n = Convert.ToInt64(N % p);
                        if (n > 0)
                            EW = NumberWithPointInWordsInEnglish(n) + " " + (PowerOfTen)i + (EW.Length == 0 ? "" : ", " + EW);
                        N = Math.Floor(N / p);
                    }
                    if (EW.Length > 0)
                        Significand = EW + (Significand.Length == 0 ? "" : " AND " + Significand);
                }
            }

            if (isMoney)
            {
                return Prefix + (Significand + Mantissa).Trim() + " RUPIAH";
            }
            else
            {
                return Prefix + (Significand + Mantissa).Trim();
            }
        }

        #endregion

        /*public static String FormatMRN(Int32 MRN)
        {
            String DefaultMRN = "00-00-00-00";
            char SplitChar = '-';
            String MedicalNo = MRN.ToString();
            int ctr = MedicalNo.Length - 1;
            for (int i = DefaultMRN.Length - 1; i >= 0; i--)
            {
                if (DefaultMRN[i] == SplitChar)
                    continue;
                else
                {
                    if (ctr >= 0)
                    {
                        DefaultMRN = DefaultMRN.Remove(i, 1);
                        DefaultMRN = DefaultMRN.Insert(i, MedicalNo[ctr].ToString());
                        ctr--;
                    }
                    else
                        break;
                }
            }

            return DefaultMRN;
        }*/

        public static String GetComboBoxValue(ASPxComboBox cbo, bool IsNullable)
        {
            if (IsNullable)
            {
                if (cbo.Value != null && cbo.Value.ToString() != "")
                    return cbo.Value.ToString();
                else
                    return null;
            }
            return cbo.Value.ToString();
        }

        public static DateTime InitializeDateTimeNull()
        {
            return new DateTime(1900, 1, 1);
        }

        public static XDocument LoadXMLFile(TemplateControl page, string xmlFileName)
        {
            //string myXml = string.Format("{0}\\App_Data\\{1}", HttpContext.Current.Request.MapPath("~"), xmlFileName);
            //string myXml = string.Format("{0}\\QIS.Medinfras.Web.CommonLibs\\App_Data\\{1}", string.Join("\\", remStrings), xmlFileName);
            string myXml = page.ResolveUrl("~/Libs/App_Data/") + xmlFileName;
            string physicalPath = HttpContext.Current.Request.MapPath(myXml);
            if (File.Exists(physicalPath))
            {
                XDocument xdoc = XDocument.Load(physicalPath);
                return xdoc;
            }
            return null;
        }

        public static string[] LoadTextFile(TemplateControl page, string textFileName)
        {
            string myText = page.ResolveUrl("~/Libs/App_Data/") + textFileName;
            string path = HttpContext.Current.Request.MapPath(myText);
            if (File.Exists(path))
                return System.IO.File.ReadAllLines(HttpContext.Current.Request.MapPath(myText), Encoding.GetEncoding("windows-1250"));
            else
            {
                string[] str = { };
                return str;
            }
        }

        #region Language
        public static string GetWordsLabel(List<Words> words, string code)
        {
            if (words == null)
                return code;
            Words word = words.FirstOrDefault(w => w.Code == code);
            return word == null ? code : word.Text;
        }

        public static List<Words> LoadWords(TemplateControl page)
        {
            XDocument xdoc = LoadXMLFile(page, "config.xml");
            var config = (from pg in xdoc.Descendants("page")
                          select new
                          {
                              Lang = pg.Attribute("lang").Value
                          }).FirstOrDefault();

            List<Words> words = new List<Words>();
            string[] tempWords = Helper.LoadTextFile(page, string.Format("lang/{0}.txt", config.Lang));
            foreach (string word in tempWords)
            {
                string[] param = word.Split(';');
                words.Add(new Words { Code = param[0], Text = param[1] });
            }
            return words;
        }
        #endregion

        public static Control FindControlRecursive(Control Root, string Id)
        {
            if (Root.ID == Id)
                return Root;

            foreach (Control Ctl in Root.Controls)
            {
                Control FoundCtl = FindControlRecursive(Ctl, Id);
                if (FoundCtl != null)
                    return FoundCtl;
            }

            return null;
        }

        public static void AddCssClass(WebControl ctrl, string classname)
        {
            ctrl.CssClass = String.Join(" ", ctrl.CssClass.Split(' ').Except(new string[] { "", classname }).Concat(new string[] { classname }).ToArray());
        }

        public static void AddCssClass(HtmlGenericControl ctrl, string classname)
        {
            string cssClass = ctrl.Attributes["class"];
            ctrl.Attributes.Add("class", String.Join(" ", cssClass.Split(' ').Except(new string[] { "", classname }).Concat(new string[] { classname }).ToArray()));
        }

        public static void SetDropDownListValue(DropDownList ddl, object value)
        {
            if (value != null)
            {
                if (ddl.Items.FindByValue(value.ToString()) != null)
                {
                    ddl.ClearSelection();
                    ddl.Items.FindByValue(value.ToString()).Selected = true;
                }
            }
        }

        public static String GetPatientAge(List<Words> words, DateTime DoB)
        {
            int ageInYear = Function.GetPatientAgeInYear(DoB, DateTime.Now);
            int ageInMonth = Function.GetPatientAgeInMonth(DoB, DateTime.Now);
            int ageInDay = Function.GetPatientAgeInDay(DoB, DateTime.Now);

            return string.Format("{0} {3}  {1} {4}  {2} {5}", ageInYear, ageInMonth, ageInDay, GetWordsLabel(words, "thn"), GetWordsLabel(words, "bln"), GetWordsLabel(words, "hr"));
        }

        public static String GetPatientAgeOnDischarge(List<Words> words, DateTime DoB, DateTime DischargeDate)
        {
            int ageInYear = Function.GetPatientAgeInYear(DoB, DischargeDate);
            int ageInMonth = Function.GetPatientAgeInMonth(DoB, DischargeDate);
            int ageInDay = Function.GetPatientAgeInDay(DoB, DischargeDate);

            return string.Format("{0} {3}  {1} {4}  {2} {5}", ageInYear, ageInMonth, ageInDay, GetWordsLabel(words, "thn"), GetWordsLabel(words, "bln"), GetWordsLabel(words, "hr"));
        }

        public static String GetPatientAgeOnDeath(List<Words> words, DateTime DoB, DateTime DateOfDeath)
        {
            int ageInYear = Function.GetPatientAgeInYear(DoB, DateOfDeath);
            int ageInMonth = Function.GetPatientAgeInMonth(DoB, DateOfDeath);
            int ageInDay = Function.GetPatientAgeInDay(DoB, DateOfDeath);

            return string.Format("{0} {3}  {1} {4}  {2} {5}", ageInYear, ageInMonth, ageInDay, GetWordsLabel(words, "thn"), GetWordsLabel(words, "bln"), GetWordsLabel(words, "hr"));
        }

        #region Date
        public static DateTime GetDatePickerValue(TextBox txt)
        {
            return GetDatePickerValue(txt.Text);
        }

        public static DateTime GetDatePickerValue(String text)
        {
            if (text != "")
            {
                var culture = System.Globalization.CultureInfo.CurrentCulture;
                return DateTime.ParseExact(text, "dd-MM-yyyy", culture);
            }
            return new DateTime(1900, 1, 1);
        }

        public static DateTime YYYYMMDDToDate(String text)
        {
            if (text != "")
            {
                var culture = System.Globalization.CultureInfo.CurrentCulture;
                return DateTime.ParseExact(text, "yyyyMMdd", culture);
            }
            return new DateTime(1900, 1, 1);
        }

        public static DateTime YYYYMMDDHourToDate(String text)
        {
            if (text != "")
            {
                var culture = System.Globalization.CultureInfo.CurrentCulture;
                return DateTime.ParseExact(text, "yyyyMMdd HH:mm:ss.fff", culture);
            }
            return new DateTime(1900, 1, 1);
        }

        public static DateTime ConvertDateToString(string val, string format)
        {
            if (val != "")
            {
                var culture = System.Globalization.CultureInfo.CurrentCulture;
                return DateTime.ParseExact(val, format, culture);
            }
            return new DateTime(1900, 1, 1);
        }
        #endregion

        #region Module
        public static string GetModuleName()
        {
            string[] param = HttpContext.Current.Request.ApplicationPath.Split('/');
            return param.Last();
        }

        public static string GetModuleImage(TemplateControl page, string moduleName)
        {
            string img = "";
            moduleName = moduleName.ToLower();
            switch (moduleName)
            {
                case "outpatient": img = "outpatient"; break;
                case "systemsetup": img = "systemsetup"; break;
                case "inpatient": img = "inpatient"; break;
                case "emergencycare": img = "emergencycare"; break;
                case "medicalrecord": img = "medicalrecord"; break;
                case "emr": img = "emr"; break;
                case "pharmacy": img = "pharmacy"; break;
                case "laboratory": img = "laboratory"; break;
                case "medicaldiagnostic": img = "medicaldiagnostic"; break;
                case "inventory": img = "inventorymanagement"; break;
                case "imaging": img = "imaging"; break;
                case "finance": img = "finance"; break;
                case "billing": img = "billingmanagement"; break;
                case "medicalcheckup": img = "medicalcheckup"; break;
                case "accounting": img = "accounting"; break;
                case "nutrition": img = "nutrition"; break;
                case "nursing": img = "nursing"; break;
                case "dashboard": img = "dashboard"; break;
                case "reporting": img = "reporting"; break;
                case "radiotheraphy": img = "radiotheraphy"; break;
            }
            return page.ResolveUrl(string.Format("~/Libs/Images/Module/{0}small.png", img));
        }

        public static string GetModuleID(string moduleName)
        {
            string result = "";
            moduleName = moduleName.ToLower();
            switch (moduleName)
            {
                case "systemsetup": result = "SA"; break;
                case "outpatient": result = "OP"; break;
                case "medicalrecord": result = "RM"; break;
                case "emergencycare": result = "ER"; break;
                case "inpatient": result = "IP"; break;
                case "emr": result = "EM"; break;
                case "pharmacy": result = "PH"; break;
                case "medicaldiagnostic": result = "MD"; break;
                case "laboratory": result = "LB"; break;
                case "inventory": result = "IM"; break;
                case "imaging": result = "IS"; break;
                case "medicalcheckup": result = "MC"; break;
                case "finance": result = "FN"; break;
                case "accounting": result = "AC"; break;
                case "billing": result = "BM"; break;
                case "nutrition": result = "NT"; break;
                case "nursing": result = "NR"; break;
                case "dashboard": result = "DB"; break;
                case "reporting": result = "RP"; break;
                case "radiotheraphy": result = "RT"; break;
            }
            return result;
        }
        #endregion

        public static void SetControlEntrySetting(Control ctrl, ControlEntrySetting setting, string ValidationGroup)
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
                ctl.ValidationSettings.ValidationGroup = ValidationGroup;
            }
            else if (ctrl is WebControl)
            {
                if (setting.IsRequired)
                    Helper.AddCssClass(((WebControl)ctrl), "required");
                ((WebControl)ctrl).Attributes.Add("validationgroup", ValidationGroup);
                if (setting.IsEditAbleInEditMode)
                    ((WebControl)ctrl).Attributes.Add("IsEditAbleInEditMode", "1");
                else
                    ((WebControl)ctrl).Attributes.Add("IsEditAbleInEditMode", "0");
            }
            else if (ctrl is HtmlGenericControl)
            {
                if (setting.IsEditAbleInEditMode)
                    ((HtmlGenericControl)ctrl).Attributes.Add("IsEditAbleInEditMode", "1");
                else
                    ((HtmlGenericControl)ctrl).Attributes.Add("IsEditAbleInEditMode", "0");
            }
        }

        public static void SetControlAttribute(Control ctrl, bool isEnabled)
        {
            if (ctrl is ASPxEdit)
            {
                ((ASPxEdit)ctrl).ClientEnabled = isEnabled;
            }
            else if (ctrl is TextBox)
            {
                if (isEnabled)
                    ((TextBox)ctrl).ReadOnly = false;
                else
                    ((TextBox)ctrl).ReadOnly = true;
            }
            else if (ctrl is DropDownList)
            {
                ((DropDownList)ctrl).Enabled = isEnabled;
            }
            else if (ctrl is CheckBox)
            {
                ((CheckBox)ctrl).Enabled = isEnabled;
            }
            else if (ctrl is HtmlGenericControl)
            {
                HtmlGenericControl lbl = ctrl as HtmlGenericControl;
                if (!isEnabled)
                    lbl.Attributes.Add("class", "lblDisabled");
            }
        }

        public static String GetHTMLEditorText(TextBox txt)
        {
            return HttpUtility.HtmlDecode(txt.Text);
        }

        public static int GetPageCount(int RowCount, double pageSize = 16.0)
        {
            double pageCount = RowCount / pageSize;
            return (int)Math.Ceiling(pageCount);
        }

        public static DateTime DateInStringToDateTime(string value)
        {
            DateTime theTime = DateTime.ParseExact(value,
                                        "yyyyMMdd",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None);
            return theTime;
        }

        public static DateTime DateTimeInStringToDateTime(string value)
        {
            DateTime theTime = DateTime.ParseExact(value,
                                        "yyyyMMdd HH:mm",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None);
            return theTime;
        }

        public static String GetErrorMessageText(TemplateControl page, string code)
        {
            XDocument xdoc = LoadXMLFile(page, "config.xml");
            var config = (from pg in xdoc.Descendants("page")
                          select new
                          {
                              Lang = pg.Attribute("lang").Value
                          }).FirstOrDefault();

            string[] tempWords = Helper.LoadTextFile(page, string.Format("err_message/{0}.txt", config.Lang));
            foreach (string word in tempWords)
            {
                string[] param = word.Split(';');
                if (param[0] == code)
                    return param[1];
            }
            return "";
        }

        public static String GetTextFormatText(TemplateControl page, string code)
        {
            XDocument xdoc = LoadXMLFile(page, "config.xml");
            var config = (from pg in xdoc.Descendants("page")
                          select new
                          {
                              Lang = pg.Attribute("lang").Value
                          }).FirstOrDefault();

            string[] tempWords = Helper.LoadTextFile(page, string.Format("text_format/{0}.txt", config.Lang));
            foreach (string word in tempWords)
            {
                string[] param = word.Split(';');
                if (param[0] == code)
                    return param[1];
            }
            return "";
        }

        public static Boolean GetIsNeedRenewalOPInitialAssessment(DateTime lastDate, Int16 renewalDays)
        {
            return ((DateTime.Now.Date - lastDate).TotalDays >= renewalDays);
        }


        #region Load From Desktop
        public static XDocument LoadXMLFile(string xmlFileName)
        {
            string physicalPath = string.Format(@"{0}\App_Data\{1}", AppConfigManager.QISLibsPhysicalDirectory, xmlFileName);
            if (File.Exists(physicalPath))
            {
                XDocument xdoc = XDocument.Load(physicalPath);
                return xdoc;
            }
            return null;
        }

        public static string[] LoadTextFile(string textFileName)
        {
            string physicalPath = string.Format(@"{0}\App_Data\{1}", AppConfigManager.QISLibsPhysicalDirectory, textFileName);
            if (File.Exists(physicalPath))
                return System.IO.File.ReadAllLines(physicalPath, Encoding.GetEncoding("windows-1250"));
            else
            {
                string[] str = { };
                return str;
            }
        }

        public static String GetTextFormatText(string code)
        {
            XDocument xdoc = LoadXMLFile("config.xml");
            var config = (from pg in xdoc.Descendants("page")
                          select new
                          {
                              Lang = pg.Attribute("lang").Value
                          }).FirstOrDefault();

            string[] tempWords = Helper.LoadTextFile(string.Format("text_format/{0}.txt", config.Lang));
            foreach (string word in tempWords)
            {
                string[] param = word.Split(';');
                if (param[0] == code)
                    return param[1];
            }
            return "";
        }
        #endregion

        #region Name
        public static String GenerateFullName(String _Name, String _Title, String _Suffix)
        {
            StringBuilder result = new StringBuilder("Title Name, Suffix");
            result = result.Replace("Title", _Title).
                Replace("Name", _Name).
                Replace("Suffix", _Suffix).
                Replace(",  ", "").
                Replace("  ", " ");
            return result.ToString().TrimStart(new char[] { ' ' }).TrimEnd(new char[] { ',', ' ' });
        }

        public static String GenerateName(String _LastName, String _MiddleName, String _FirstName)
        {
            StringBuilder result = new StringBuilder(AppConfigManager.QISNameFormat);
            result = result.Replace("LastName", _LastName).
                Replace("LASTNAME", _LastName.ToUpper()).
                Replace("MiddleName", _MiddleName).
                Replace("MIDDLENAME", _MiddleName.ToUpper()).
                Replace("FirstName", _FirstName).
                Replace("FIRSTNAME", _FirstName.ToUpper()).
                Replace(",  ", "").
                Replace("  ", " ");
            return result.ToString().TrimStart(new char[] { ' ' }).TrimEnd(new char[] { ',', ' ' });
        }
        #endregion

        #region Error Log
        public static void InsertErrorLog()
        {
            // Code that runs when an unhandled error occurs
            HttpServerUtility server = HttpContext.Current.Server;
            Exception exception = server.GetLastError();
            InsertErrorLog(exception);
        }

        public static void InsertErrorLog(Exception exception)
        {
            // Code that runs when an unhandled error occurs
            Exception baseException = exception.GetBaseException();
            if (baseException != null)
                exception = baseException;
            string userIP = HttpContext.Current.Request.UserHostAddress;
            string appPath = HttpContext.Current.Request.Url.AbsolutePath;
            string trace = RemoveLineEndings(exception.StackTrace);
            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);

            string message = string.Format("{0}|{1}|{2}|{3}|{4}|{5}{6}", DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT), ModuleID, userIP, appPath, RemoveLineEndings1(exception.Message), trace, Environment.NewLine);

            string path = VirtualPathUtility.ToAbsolute("~/Libs/App_Data/log");
            string physicalPath = HttpContext.Current.Request.MapPath(path);
            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);

            string myFile = string.Format("{0}\\{1}.txt", physicalPath, DateTime.Now.ToString("yyyyMMdd"));

            if (!File.Exists(myFile))
                File.WriteAllText(myFile, message);
            else
                File.AppendAllText(myFile, message);
        }

        private static string RemoveLineEndings(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            string replaceChar = "%^%";
            return value.Replace("\r\n", replaceChar).Replace("\n", replaceChar).Replace("\r", replaceChar).Replace(lineSeparator, replaceChar).Replace(paragraphSeparator, replaceChar);
        }

        private static string RemoveLineEndings1(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            string lineSeparator = ((char)0x2028).ToString();
            string paragraphSeparator = ((char)0x2029).ToString();

            string replaceChar = " ";
            return value.Replace("\r\n", replaceChar).Replace("\n", replaceChar).Replace("\r", replaceChar).Replace(lineSeparator, replaceChar).Replace(paragraphSeparator, replaceChar);
        }
        #endregion

        public static bool SendHL7ImagingOrderToRIS(int testOrderID, int transactionID)
        {
            string messageDateTime = string.Format("{0}{1}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.Date.ToString("HH:mm:ss").Replace(":", ""));
            StringBuilder command = new StringBuilder();

            string filterExpression = string.Format("TransactionID = {0}", transactionID);
            vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
            if (oHeader != null)
            {
                command.AppendLine(string.Format(@"MSH|^~\&|MEDINFRAS||||20170303124841||ORM^001|US23148212000000C2D9|P|2.3.1"));
                command.AppendLine(string.Format(@"PID|{0}||{1}||{2}^{3}^{4}||{5}|{6}|||{7}^{8}^{9}^{10}^|||||||||"));
                command.AppendLine(string.Format(@"PV1||{0}|{1}||||||||||||||||{2}", AppSession.RegisteredPatient.DepartmentID, AppSession.RegisteredPatient.RegistrationNo));
                command.AppendLine(string.Format(@"ORC|"));
                command.AppendLine(string.Format(@"OBR|"));
                //AppSession.RegisteredPatient.
            }

            //if (oHeader != null)
            //{
            //    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
            //    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
            //    TestOrderDTO oData = new TestOrderDTO();
            //    if (oList.Count > 0)
            //    {
            //        string orderPriority = "NORMAL";
            //        string orderParamedicCode = oVisit.ParamedicCode;
            //        string orderParamedicName = oVisit.ParamedicName;
            //        DateTime orderDate = DateTime.Now.Date;
            //        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            //        if (testOrderID > 0)
            //        {
            //            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
            //            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";
            //            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : "";
            //            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : "";
            //            orderDate = oOrderHd.TestOrderDate;
            //            orderTime = oOrderHd.TestOrderTime;
            //        }

            //        oData.placerOrderNumber = oHeader.TransactionNo;
            //        oData.visitNumber = oVisit.RegistrationNo;
            //        oData.pointOfCare = oHeader.ServiceUnitName;
            //        oData.room = oVisit.RoomName;
            //        oData.bed = oVisit.BedCode;
            //        oData.orderDateTime = string.Format("{0} {1}:00", orderDate.ToString("yyyy-MM-dd"), orderTime);
            //        oData.imagingOrderPriority = orderPriority;
            //        oData.reportingPriority = orderPriority;

            //        List<TestOrderDtDTO> lstDetail = new List<TestOrderDtDTO>();

            //        foreach (vPatientChargesDt item in oList)
            //        {
            //            TestOrderDtDTO oDetail = new TestOrderDtDTO();
            //            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);
            //            procedure oProcedure = new procedure() { procedureCode = item.ItemCode, procedureName = item.ItemName1, modalityCode = modality, procedureFee = 0 };
            //            readingPhysician oPhysician = new readingPhysician() { radStaffCode = item.ParamedicCode, radStaffName = item.ParamedicName };
            //            List<readingPhysician> lst = new List<readingPhysician>();
            //            lst.Add(oPhysician);

            //            oDetail.procedure = oProcedure;
            //            oDetail.readingPhysician = lst;
            //            lstDetail.Add(oDetail);
            //        }
            //        oData.orderDetail = lstDetail;

            //        patient oPatient = new patient();

            //        oPatient.patientID = oVisit.MRN.ToString();
            //        oPatient.mrn = oVisit.MedicalNo;
            //        oPatient.patientName = oVisit.PatientName;
            //        oPatient.sex = oVisit.GCGender.Substring(5);
            //        oPatient.address = oVisit.HomeAddress;
            //        oPatient.dateOfBirth = oVisit.DateOfBirth.ToString("yyyy-MM-dd");
            //        oPatient.size = "0";
            //        oPatient.weight = "0";
            //        oPatient.maritalStatus = string.IsNullOrEmpty(oVisit.GCMaritalStatus) ? "U" : oVisit.GCMaritalStatus.Substring(5);

            //        oData.patient = oPatient;

            //        List<referringPhysician> lstReferringPhysician = new List<referringPhysician>();

            //        if (testOrderID > 0)
            //        {
            //            lstReferringPhysician.Add(new referringPhysician() { refPhyCode = orderParamedicCode, refPhyName = orderParamedicName });
            //        }
            //        else
            //        {
            //            if (!String.IsNullOrEmpty(oVisit.ReferralPhysicianCode))
            //                lstReferringPhysician.Add(new referringPhysician() { refPhyCode = oVisit.ReferralPhysicianCode, refPhyName = oVisit.ReferralPhysicianName });
            //            else
            //                lstReferringPhysician.Add(new referringPhysician() { refPhyCode = oVisit.ParamedicCode, refPhyName = oVisit.ParamedicName });
            //        }

            //        oData.referringPhysician = lstReferringPhysician;
            return false;
        }

        public static string GenerateRegistrationTicketNo(int queueNo, int roomID)
        {
            string roomPrefix = BusinessLayer.GetRoom(roomID).RoomQueuePrefix;

            StringBuilder result = new StringBuilder();

            result = result.Append(roomPrefix).Append(queueNo.ToString().PadLeft(3, '0'));

            return result.ToString();
        }

        public static String ReplaceLineBreak(string sentence)
        {
            string result = string.Empty;

            if (sentence.Contains("\n") || sentence.Contains("\r") || sentence.Contains("\t"))
            {
                result = sentence.Replace("\n", " ");
                result = result.Replace("\r", " ");
                result = result.Replace("\t", " ");
            }
            else
            {
                result = sentence;
            }

            return result;
        }

        #region Excel Processes
        private static void GetExcelProvider(string fullPath, string fileName, ref string connString)
        {
            if (fileName.Trim() == Constant.ExtensionFile.XLS)
            {
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fullPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
            }
            else if (fileName.Trim() == Constant.ExtensionFile.XLSX)
            {
                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fullPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
            }
        }

        private static void ExcelConnection(string fullPath, string fileName, string sheetName, ref string connString, ref string query, ref StringBuilder error)
        {
            //Connection String to Excel Workbook
            GetExcelProvider(fullPath, fileName, ref connString);

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        try
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            DataTable dtExcelSheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            //sheetName = dtExcelSheets.Rows[0]["TABLE_NAME"].ToString();
                            query = string.Format("SELECT * FROM [{0}]", sheetName);
                            conn.Close();
                        }
                        catch (Exception ex)
                        {
                            error.AppendLine(ex.Message.ToString());
                        }
                    }
                }
            }
        }

        public static string ConvertExcelFileToJSON(string fullPath, string fileName, ref string errMessage, string sheetName = "TABLE_NAME")
        {
            string result = string.Empty;
            StringBuilder error = new StringBuilder();
            string connString = string.Empty;
            string query = string.Empty;
            #region read the excel file
            ExcelConnection(fullPath, fileName, sheetName, ref connString, ref query, ref error);

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        try
                        {
                            cmd.CommandText = query;
                            cmd.Connection = conn;
                            conn.Open();
                            da.SelectCommand = cmd;
                            string test = string.Empty;
                            using (var rdr = cmd.ExecuteReader())
                            {
                                var queryString = (from DbDataRecord row in rdr select row).Select(s =>
                                {
                                    Dictionary<string, object> item = new Dictionary<string, object>();
                                    for (int i = 0; i < s.FieldCount; i++)
                                    {
                                        test += string.Format("{0},", rdr.GetValue(i));
                                        item.Add(rdr.GetName(i), s[i]);
                                    }
                                    return item;
                                });

                                result = JsonConvert.SerializeObject(queryString).ToString();

                            }
                            da.Dispose();
                            conn.Close();
                            conn.Dispose();
                        }
                        catch (Exception ex)
                        {
                            error.AppendLine(ex.Message.ToString());
                        }
                    }
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(error.ToString()))
            {
                errMessage = error.ToString().Replace(Environment.NewLine, "<br />");
            }

            return result;
        }

        public static T ConvertExcelFileToJSONToModel<T>(string fullPath, string fileName, ref string errMessage, string sheetName = "TABLE_NAME")
        {
            string result = string.Empty;
            StringBuilder error = new StringBuilder();
            string connString = string.Empty;
            string query = string.Empty;

            var jsonSettings = new JsonSerializerSettings();

            jsonSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSettings.DefaultValueHandling = DefaultValueHandling.Ignore;

            #region read the excel file
            ExcelConnection(fullPath, fileName, sheetName, ref connString, ref query, ref error);

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        try
                        {
                            cmd.CommandText = query;
                            cmd.Connection = conn;
                            conn.Open();
                            da.SelectCommand = cmd;
                            string test = string.Empty;
                            using (var rdr = cmd.ExecuteReader())
                            {
                                var queryString = (from DbDataRecord row in rdr select row).Select(s =>
                                {
                                    Dictionary<string, object> item = new Dictionary<string, object>();
                                    for (int i = 0; i < s.FieldCount; i++)
                                    {
                                        test += string.Format("{0},", rdr.GetValue(i));
                                        item.Add(rdr.GetName(i), s[i]);
                                    }
                                    item = item.Where(w => !string.IsNullOrEmpty(w.Value.ToString())).ToDictionary(d => d.Key, d => d.Value);
                                    return item;
                                });

                                result = JsonConvert.SerializeObject(queryString, jsonSettings).ToString().Replace(",{}", "").Replace("{}", "");

                            }
                            da.Dispose();
                            conn.Close();
                            conn.Dispose();
                        }
                        catch (Exception ex)
                        {
                            error.AppendLine(ex.Message.ToString());
                        }
                    }
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(error.ToString()))
            {
                errMessage = error.ToString().Replace(Environment.NewLine, "<br />");
            }

            return JsonConvert.DeserializeObject<T>(result);
        }

        public static string ReadExcelFile_GetSheetsName(string fullPath, string fileName, ref List<string> lstSheetName)
        {
            string result = string.Empty;
            string connString = string.Empty;
            string sheetName = string.Empty;
            string query = string.Empty;
            #region read the excel file
            //Connection String to Excel Workbook
            GetExcelProvider(fullPath, fileName, ref connString);

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();

                    DataTable lstSheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    if (lstSheets == null)
                    {
                        return null;
                    }

                    String[] excelSheets = new String[lstSheets.Rows.Count];
                    int i = 0;

                    foreach (DataRow row in lstSheets.Rows)
                    {
                        excelSheets[i] = row["TABLE_NAME"].ToString();
                        lstSheetName.Add(excelSheets[i].ToString());
                        i++;
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
                finally
                {
                    conn.Close();
                }
            }
            #endregion

            return result;
        }

        public static string ConvertExcelFileToGridView(string fullPath, string fileName, string sheetName, GridView grdView, ref string errMessage)
        {
            string result = string.Empty;
            StringBuilder error = new StringBuilder();
            string connString = string.Empty;
            string query = string.Empty;
            #region read the excel file
            ExcelConnection(fullPath, fileName, sheetName, ref connString, ref query, ref error);

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        try
                        {
                            cmd.CommandText = query;
                            cmd.Connection = conn;
                            conn.Open();
                            da.SelectCommand = cmd;
                            string test = string.Empty;
                            using (var rdr = cmd.ExecuteReader())
                            {
                                var queryString = (from DbDataRecord row in rdr select row).Select(s =>
                                {
                                    Dictionary<string, object> item = new Dictionary<string, object>();
                                    for (int i = 0; i < s.FieldCount; i++)
                                    {
                                        test += string.Format("{0},", rdr.GetValue(i));
                                        item.Add(rdr.GetName(i), s[i]);
                                    }
                                    return item;
                                });

                                result = JsonConvert.SerializeObject(queryString).ToString();

                            }

                            DataSet ds = new DataSet();
                            da.Fill(ds);

                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                                {
                                    if (Convert.ToString(ds.Tables[i].Rows[j]["No"]) == string.Empty)
                                    {
                                        ds.Tables[i].Rows[j].Delete();
                                    }
                                }
                            }

                            grdView.DataSource = ds.Tables[0];
                            grdView.DataBind();
                            da.Dispose();
                            conn.Close();
                            conn.Dispose();
                        }
                        catch (Exception ex)
                        {
                            error.AppendLine(ex.Message.ToString());
                        }
                    }
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(error.ToString()))
            {
                errMessage = error.ToString().Replace(Environment.NewLine, "<br />");
            }

            return result;
        }

        public static string[] GetExcelColumnNameInArray(string fullPath, string fileName, string sheetName, ref string errMessage)
        {
            string result = string.Empty;
            StringBuilder error = new StringBuilder();
            string connString = string.Empty;
            string query = string.Empty;
            List<String> lstColumns = new List<String>();
            #region read the excel file
            ExcelConnection(fullPath, fileName, sheetName, ref connString, ref query, ref error);

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    using (OleDbDataAdapter da = new OleDbDataAdapter())
                    {
                        try
                        {
                            cmd.CommandText = query;
                            cmd.Connection = conn;
                            conn.Open();
                            da.SelectCommand = cmd;
                            string test = string.Empty;
                            using (var rdr = cmd.ExecuteReader())
                            {
                                var queryString = (from DbDataRecord row in rdr select row).Select(s =>
                                {
                                    Dictionary<string, object> item = new Dictionary<string, object>();
                                    for (int i = 0; i < s.FieldCount; i++)
                                    {
                                        test += string.Format("{0},", rdr.GetValue(i));
                                        item.Add(rdr.GetName(i), s[i]);
                                    }
                                    return item;
                                });

                                result = JsonConvert.SerializeObject(queryString).ToString();

                            }

                            DataSet ds = new DataSet();
                            da.Fill(ds);

                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                var a = ds.Tables[i].Columns.Count;

                                for (int k = 0; k < ds.Tables[i].Columns.Count; k++)
                                {
                                    lstColumns.Add(ds.Tables[i].Columns[k].ToString());
                                }
                            }

                            if (string.IsNullOrEmpty(error.ToString()))
                            {
                                error.AppendLine(string.Format("No Data Found"));
                            }
                            da.Dispose();
                            conn.Close();
                            conn.Dispose();
                        }
                        catch (Exception ex)
                        {
                            error.AppendLine(ex.Message.ToString());
                        }
                    }
                }
            }
            #endregion

            if (!string.IsNullOrEmpty(error.ToString()))
            {
                errMessage = error.ToString().Replace(Environment.NewLine, "<br />");
            }

            return lstColumns.ToArray();
        }
        #endregion

        public static string ConvertDate112ToDatePickerFormat(string date)
        {
            String year = date.Substring(0, 4);
            String month = date.Substring(4, 2);
            String day = date.Substring(6, 2);

            return string.Format("{0}-{1}-{2}", day, month, year);
        }

        public static List<GetUserRoleTabMenuList> OnGetMenuTabAccess(string moduleID, string pageMenuCode)
        {
            List<UserInRole> lstRole = BusinessLayer.GetUserInRoleList(string.Format("UserID = {0}", AppSession.UserLogin.UserID));
            string lstRoleID = string.Empty;
            foreach (UserInRole r in lstRole)
            {
                lstRoleID += string.Format("{0},", r.RoleID.ToString());
            }

            lstRoleID = lstRoleID.Remove(lstRoleID.Length - 1, 1);

            List<GetUserRoleTabMenuList> lstMenuTab = BusinessLayer.GetUserRoleMenuTabList(moduleID, AppSession.UserLogin.HealthcareID, lstRoleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, pageMenuCode);

            return lstMenuTab;
        }

        public static bool IsValidCheckSqlInjectionFilterParameter(string filterParameter)
        {
            string text = filterParameter.ToLower();
            if (text.Contains("insert ") || text.Contains("update ") || text.Contains("delete ") || text.Contains("create ") || text.Contains("drop ")
                || text.Contains("enable ") || text.Contains("disable ") || text.Contains("user_name") || text.Contains("db_name")
                || text.Contains("xp_cmdshell") || text.Contains("collate") || text.Contains("sql_latin1_general_cp1254_cs_as")
                || text.Contains("master..sysmessages") || text.Contains("master..sysservers") || text.Contains("sys.sql_logins")
                || text.Contains("xp_regread") || text.Contains("xp_regaddmultistring") || text.Contains("xp_regdeletekey")
                || text.Contains("xp_regdeletevalue") || text.Contains("xp_regenumkeys") || text.Contains("xp_regenumvalues")
                || text.Contains("xp_regremovemultistring") || text.Contains("xp_regwrite") || text.Contains("xp_servicecontrol")
                || text.Contains("xp_availablemedia") || text.Contains("xp_enumdsn") || text.Contains("xp_loginconfig") || text.Contains("xp_makecab")
                || text.Contains("xp_ntsec_enumdomains") || text.Contains("xp_terminate_process") || text.Contains("limit") || text.Contains("shutdown")
                || text.Contains("sp_configure") || text.Contains("reconfigure") || text.Contains("sysobjects") || text.Contains("syscolumns") || text.Contains("begin")
                || text.Contains("declare ") || text.Contains("waitfor delay") || text.Contains("bulk") || text.Contains(" union "))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region AgePatient
        public static string CalculateAge(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime PastYearDate = Dob.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(PastYearDate.AddMonths(Months)).Days;
            int Hours = Now.Subtract(PastYearDate).Hours;
            int Minutes = Now.Subtract(PastYearDate).Minutes;
            int Seconds = Now.Subtract(PastYearDate).Seconds;
            return string.Format("{0}|{1}|{2}", Years, Months, Days);
        }
        #endregion

        #region Direct Printing Tool
        public static void MedinfrasDirectPrintingTools(string ipAndPort, string printingCommand)
        {
            try
            {
                string[] ipAndPortArr = ipAndPort.Split(':');
                string ipAddress = ipAndPortArr[0];
                string port = ipAndPortArr[1];
                TcpClient client = new TcpClient();
                client.Connect(ipAddress, Convert.ToInt16(port));
                // Retrieve the network stream. 
                NetworkStream stream = client.GetStream();
                // Create a BinaryWriter for writing to the stream. 
                using (BinaryWriter w = new BinaryWriter(stream, Encoding.UTF8))
                { // Create a BinaryReader for reading from the stream. 
                    using (BinaryReader r = new BinaryReader(stream))
                    { // Start a dialogue. 
                        w.Write(string.Format("{0}", printingCommand).ToCharArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
            }
        }
        #endregion
    }
}
