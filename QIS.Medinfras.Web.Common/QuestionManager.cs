using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using QIS.Medinfras.Data.Service;
using System.Drawing;

namespace QIS.Medinfras.Web.Common
{
	public static class QuestionManager
	{
		/*public static StringBuilder HtmlQuestion(Question question)
		{
			StringBuilder sb = new StringBuilder();
			return sb;
		}

		public static void PopulateQuestionByForm(List<string> questionList, string formID)
		{
			questionList.Clear();
			List<QuestionGroupInForm> questionGroup = BusinessLayer.ListQuestionGroupInForm(string.Format("FormID = '{0}'", formID));
			foreach (QuestionGroupInForm group in questionGroup)
			{
				List<UspPopulateQuestionByGroup> lstQuestion = BusinessLayer.PopulateQuestionByGroup(group.QuestionGroupID);
				foreach (UspPopulateQuestionByGroup question in lstQuestion)
				{
					string questionTag = string.Format("<b>{0}. {1} </b> <br/>", question.QuestionOrder, question.QuestionText);
					questionTag += PopulateQuestionAnswer(question.QuestionID);
					questionList.Add(questionTag);
				}
			}
		}

		public static string PopulateQuestionAnswer(string questionID)
		{
			string resultTag = string.Empty;
			List<UspPopulateQuestionAnswer> lstAnswer = BusinessLayer.PopulateQuestionAnswer(questionID);
			string style = string.Format("style=\"{0}\"", "font-size: 8pt; font-family: Tahoma");
			foreach (UspPopulateQuestionAnswer answer in lstAnswer)
			{
				string indent = htmlInsertSpace(answer.AnswerLevel);

				switch (answer.AnswerType.ToLower())
				{
					case Constant.QuestionaireAnswerType.LABEL:
						resultTag += string.Format("{0} {1} <br/>", indent, answer.AnswerText);
						break;
					case Constant.QuestionaireAnswerType.TEXT:
						if (answer.PrefixText != string.Empty && answer.SuffixText != string.Empty)
							resultTag += ("<table border=0 cellpadding=0 cellspacing=0 width=100%><tr><td valign=middle width=30%>" + string.Format("{0} {1}", indent, answer.PrefixText) + "</td><td valign=middle width=30%>" + string.Format("<input id=\"{0}\" type=\"{1}\" value=0 style=\"{2}\" />", answer.AnswerID, answer.AnswerType, style)) + "</td><td valign=middle width=40%>" + answer.SuffixText + "</td></tr></table>";
						else
							if (answer.PrefixText != string.Empty)
								resultTag += ("<table border=0 cellpadding=0 cellspacing=0 width=100%><tr><td valign=middle colspan=2>" + string.Format("{0} {1}", indent, answer.PrefixText) + "</td><td valign=middle>" + string.Format("<input id=\"{0}\" type=\"{1}\" value=0 style=\"{2}\" />", answer.AnswerID, answer.AnswerType, style)) + "</td></tr></table>";
							else
								resultTag += ("<tr><td align = center valign=middle>" + string.Format("<input id=\"{0}\" type=\"{1}\" value=0 style=\"{2}\" size=10 /> {3}", answer.AnswerID, answer.AnswerType, style, answer.AnswerText) + "</td></tr>");
						break;
					case Constant.QuestionaireAnswerType.RADIO:
						resultTag += string.Format("{0} <input id=\"{1}\" name=\"{2}\" type=\"radio\" value=\"{3}\"/> {4} <br/>", indent, answer.AnswerID, answer.QuestionID, answer.AnswerText, answer.AnswerText);
						break;
					default:
						resultTag += string.Format("{0} <input id=\"{1}\" type=\"{2}\" value=\"{3}\"/> {4} <br/>", indent, answer.AnswerID, answer.AnswerType, answer.AnswerText, answer.AnswerText);
						break;
				}
				if (answer.IsHasChild)
				{
					resultTag += PopulateDetailAnswer(answer.QuestionID, answer.AnswerID);
				}
			}
			return resultTag;
		}

		private static string PopulateDetailAnswer(string questionID, string parentID)
		{
			string resultTag = string.Empty;
			List<UspPopulateQuestionAnswerByParentID> lstAnswer = BusinessLayer.PopulateQuestionAnswerByParentID(questionID, parentID);
			string style = string.Format("style=\"{0}\"", "font-size: 8pt; font-family: Tahoma");
			foreach (UspPopulateQuestionAnswerByParentID answer in lstAnswer)
			{
				string indent = htmlInsertSpace(answer.AnswerLevel);
				switch (answer.AnswerType.ToLower())
				{
					case Constant.QuestionaireAnswerType.LABEL:
						resultTag += string.Format("{0} {1} <br/>", indent, answer.AnswerText);
						break;
					case Constant.QuestionaireAnswerType.TEXT:
						if (answer.PrefixText != string.Empty && answer.SuffixText != string.Empty)
							resultTag += ("<table border=0 cellpadding=0 cellspacing=0 width=100%><tr><td valign=middle width=30%>" + string.Format("{0} {1}", indent, answer.PrefixText) + "</td><td valign=middle width=30%>" + string.Format("<input id=\"{0}\" type=\"{1}\" value=0 style=\"{2}\" />", answer.AnswerID, answer.AnswerType, style)) + "</td><td valign=middle width=40%>" + answer.SuffixText + "</td></tr></table>";
						else
							if (answer.PrefixText != string.Empty)
								resultTag += ("<table border=0 cellpadding=0 cellspacing=0 width=100%><tr><td valign=middle colspan=2>" + string.Format("{0} {1}", indent, answer.PrefixText) + "</td><td valign=middle>" + string.Format("<input id=\"{0}\" type=\"{1}\" value=0 style=\"{2}\" />", answer.AnswerID, answer.AnswerType, style)) + "</td></tr></table>";
							else
								resultTag += ("<tr><td align = center valign=middle>" + string.Format("<input id=\"{0}\" type=\"{1}\" value=0 style=\"{2}\" size=10 /> {3}", answer.AnswerID, answer.AnswerType, style, answer.AnswerText) + "</td></tr>");
						break;
					case Constant.QuestionaireAnswerType.RADIO:
						resultTag += string.Format("{0} <input id=\"{1}\" name=\"{2}\" type=\"radio\" value=\"{3}\"/> {4} <br/>", indent, answer.AnswerID, answer.ParentID, answer.AnswerText, answer.AnswerText);
						break;
					default:
						resultTag += string.Format("{0} <input id=\"{1}\" type=\"{2}\" value=1/> {3}", indent, answer.AnswerID, answer.AnswerType, answer.AnswerText);
						break;
				}
				if (answer.IsHasChild)
				{
					resultTag += PopulateDetailAnswer(answer.QuestionID, answer.AnswerID);
				}
			}
			return resultTag;
		}

		private static string htmlInsertSpace(int level)
		{
			string result = string.Empty;
			for (int i = 0; i < level; i++)
			{
				result += "&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp";
			}
			return result;
        }

        #region BuatPopulate Question New
        //new
        public static void PopulateQuestionaireNEW(string questionaireForm, ref HtmlTable table)
        {
            //Populate Questionaire Question
            List<QuestionGroupInForm> list = BusinessLayer.ListQuestionGroupInForm(string.Format(" FormID = '{0}' and IsActive = 1 ORDER BY GroupOrder", questionaireForm));
            foreach (QuestionGroupInForm group in list)
            {
                int sequenceNo = 1;
                HtmlTableRow row;

                HtmlTableCell cell;
                if (list.Count > 1)
                {
                    //Question Group
                    row = new HtmlTableRow();
                    table.Rows.Add(row);

                    cell = new HtmlTableCell();
                    cell.Align = "left";
                    cell.VAlign = "top";
                    cell.ColSpan = 8;
                    cell.Attributes["style"] = "font-weight:bold;";

                    ASPxLabel groupText = new ASPxLabel();
                    groupText.ID = string.Format("lbl{0}", group.QuestionGroupID);
                    groupText.Text = string.Format("{0}", BusinessLayer.GetQuestionGroup(group.QuestionGroupID).QuestionGroupName);
                    groupText.SkinID = "Important";
                    groupText.ForeColor = Color.Blue;
                    cell.Controls.Add(groupText);
                    row.Cells.Add(cell);

                    row = new HtmlTableRow();
                    table.Rows.Add(row);
                    cell = new HtmlTableCell();
                    cell.Align = "left";
                    cell.VAlign = "top";
                    cell.ColSpan = 8;
                    cell.Attributes["style"] = "font-weight:bold;";
                    cell.InnerHtml = "<hr/>";
                    row.Cells.Add(cell);

                }

                List<Question> question = BusinessLayer.ListQuestion(string.Format(" QuestionGroupID = '{0}' and IsActive = 1 ORDER BY QuestionOrder", group.QuestionGroupID));
                foreach (Question item in question)
                {
                    Int32 questionID = item.QuestionID;

                    row = new HtmlTableRow();
                    table.Rows.Add(row);

                    //Sequence Column
                    cell = new HtmlTableCell();
                    cell.Align = "center";
                    cell.VAlign = "top";
                    cell.Attributes["style"] = "font-weight:bold;width:5%;";

                    ASPxLabel questionText = new ASPxLabel();
                    questionText.ID = string.Format("lbl{0}_{1}", group.QuestionGroupID, Convert.ToString(sequenceNo).PadLeft(2, '0'));
                    questionText.Text = string.Format("{0}. ", Convert.ToString(sequenceNo).PadLeft(2, '0'));
                    cell.Controls.Add(questionText);
                    row.Cells.Add(cell);

                    //Question Column
                    cell = new HtmlTableCell();
                    cell.Align = "left";
                    //if (!item.IsSingleAnswer)
                    //{
                    //    cell.ColSpan = 7;
                    //    cell.VAlign = "middle";
                    //    cell.Attributes["style"] = "font-weight:bold;";
                    //}
                    //else
                    //{
                    //    cell.VAlign = "top";
                    //    cell.Attributes["style"] = "font-weight:bold;width:35%;";
                    //}


                    questionText = new ASPxLabel();
                    questionText.ID = "lbl" + questionID;
                    questionText.Text = string.Format("{0}", item.QuestionText);

                    questionText.Width = Unit.Percentage(100);
                    questionText.EnableTheming = true;

                    cell.Controls.Add(questionText);
                    row.Cells.Add(cell);

                    PopulateQuestionAnswerNEW(table, item, row);

                    sequenceNo += 1;
                }
            }
        }

        private static void PopulateQuestionAnswerNEW(HtmlTable table, Question question, HtmlTableRow currentRow)
        {
            List<UspPopulateQuestionAnswerNEW> answer = BusinessLayer.PopulateQuestionAnswerNEW(question.QuestionID);
            Int32 questionID = question.QuestionID;

            foreach (UspPopulateQuestionAnswerNEW answerItem in answer)
            {
                string answerID = answerItem.AnswerID.Replace('.', '_');

                HtmlTableRow row;
                bool isCreateOnSingleRow = false;
                if (answer.Count > 1)
                    isCreateOnSingleRow = false;
                else
                    isCreateOnSingleRow = !answerItem.IsHasChild;

                if (!isCreateOnSingleRow)
                {
                    row = new HtmlTableRow();
                    table.Rows.Add(row);
                    CreateIndentColumn(row, answerItem.AnswerLevel);
                }
                else
                {
                    row = currentRow;
                }

                Control ctl = null;
                CreateAnswer(answerItem.AnswerType, answerItem.AnswerText, answerItem.PrefixText, answerItem.SuffixText, answerItem.AnswerLevel, answerItem.IsHasChild, questionID, answerID, ctl, row);

                if (answerItem.IsHasChild)
                {
                    PopulateDetailAnswerNEW(table, question.QuestionID, answerItem.AnswerID);
                }
            }
        }

        private static void PopulateDetailAnswerNEW(HtmlTable table, Int32 questionID, string parentID)
        {
            List<UspPopulateQuestionAnswerNEWByParentID> lstAnswer = BusinessLayer.PopulateQuestionAnswerNEWByParentID(questionID, parentID);
            foreach (UspPopulateQuestionAnswerNEWByParentID answer in lstAnswer)
            {
                string answerID = answer.AnswerID.Replace('.', '_');
                HtmlTableRow row = new HtmlTableRow();
                table.Rows.Add(row);

                CreateIndentColumn(row, answer.AnswerLevel);

                Control ctl = null;
                CreateAnswer(answer.AnswerType, answer.AnswerText, answer.PrefixText, answer.SuffixText, answer.AnswerLevel, answer.IsHasChild, Convert.ToInt32(parentID), answerID, ctl, row);


                if (answer.IsHasChild)
                {
                    PopulateDetailAnswerNEW(table,Convert.ToInt32(answer.QuestionID), answer.AnswerID);
                }
            }
        }
        //
        #endregion

        public static void PopulateQuestionaire(string questionaireForm, ref HtmlTable table, bool isAdd, List<QuestionGroupInForm> list, List<Question> questionAll, List<QuestionAnswer> listQA)
		{
			//Populate Questionaire Question
            //bool isAdd = false;            
            if (isAdd)
            {
                list = BusinessLayer.ListQuestionGroupInForm(string.Format(" FormID = '{0}' and IsActive = 1 ORDER BY GroupOrder", questionaireForm));
            }
            //else
            //{
            //    list = BusinessLayer.ListQuestionGroupInForm(string.Format(" FormID = '{0}' ORDER BY GroupOrder", questionaireForm));
            //}

			foreach (QuestionGroupInForm group in list)
			{
				int sequenceNo = 1;
				HtmlTableRow row;

				HtmlTableCell cell;
				if (list.Count > 1)
				{
					//Question Group
					row = new HtmlTableRow();
					table.Rows.Add(row);

					cell = new HtmlTableCell();
					cell.Align = "left";
					cell.VAlign = "top";
					cell.ColSpan = 8;
					cell.Attributes["style"] = "font-weight:bold;";

					ASPxLabel groupText = new ASPxLabel();
					groupText.ID = string.Format("lbl{0}", group.QuestionGroupID);
					groupText.Text = string.Format("{0}", BusinessLayer.GetQuestionGroup(group.QuestionGroupID).QuestionGroupName);
					groupText.SkinID = "Important";
					groupText.ForeColor = Color.Blue;
					cell.Controls.Add(groupText);
					row.Cells.Add(cell);

					row = new HtmlTableRow();
					table.Rows.Add(row);
					cell = new HtmlTableCell();
					cell.Align = "left";
					cell.VAlign = "top";
					cell.ColSpan = 8;
					cell.Attributes["style"] = "font-weight:bold;";
					cell.InnerHtml = "<hr/>";
					row.Cells.Add(cell);

				}

                List<Question> question = new List<Question>();
                if (isAdd)
                    question = BusinessLayer.ListQuestion(string.Format(" QuestionGroupID = '{0}' and IsActive = 1 ORDER BY QuestionOrder", group.QuestionGroupID));
                else
                {
                    foreach(Question rowquestionAll in questionAll)
                    {
                        //if (Convert.ToInt32(rowquestionAll.QuestionGroupID) == group.QuestionGroupID)
                          //  question.Add(rowquestionAll);
                    }
                }
                //    question = BusinessLayer.ListQuestion(string.Format(" QuestionGroupID = '{0}' ORDER BY QuestionOrder", group.QuestionGroupID));

				foreach (Question item in question)
				{
                    Int32 questionID = item.QuestionID;

                    row = new HtmlTableRow();
                    table.Rows.Add(row);

                    //Sequence Column
                    cell = new HtmlTableCell();
                    cell.Align = "center";
                    cell.VAlign = "top";
                    cell.Attributes["style"] = "font-weight:bold;width:5%;";

                    ASPxLabel questionText = new ASPxLabel();
                    questionText.ID = string.Format("lbl{0}_{1}", group.QuestionGroupID, Convert.ToString(sequenceNo).PadLeft(2, '0'));
                    questionText.Text = string.Format("{0}. ", Convert.ToString(sequenceNo).PadLeft(2, '0'));
                    cell.Controls.Add(questionText);
                    row.Cells.Add(cell);

                    //Question Column
                    cell = new HtmlTableCell();
                    cell.Align = "left";
                    //if (!item.IsSingleAnswer)
                    //{
                    //    cell.ColSpan = 7;
                    //    cell.VAlign = "middle";
                    //    cell.Attributes["style"] = "font-weight:bold;";
                    //}
                    //else
                    //{
                    //    cell.VAlign = "top";
                    //    cell.Attributes["style"] = "font-weight:bold;width:35%;";
                    //}


                    questionText = new ASPxLabel();
                    questionText.ID = "lbl" + questionID;
                    questionText.Text = string.Format("{0}", item.QuestionText);

                    questionText.Width = Unit.Percentage(100);
                    questionText.EnableTheming = true;

                    cell.Controls.Add(questionText);
                    row.Cells.Add(cell);

                    PopulateQuestionAnswer(table, item, row, isAdd, listQA);

                    sequenceNo += 1;
				}
			}
		}

		//private static Control InitializeControl(string answerType)
		//{
		//    //Control ctl;
		//    //switch (answerType.ToUpper())
		//    //{
		//    //    case Constant.QuestionaireAnswerType.COMBOBOX:
		//    //        ctl = new ASPxComboBox();
		//    //        (ctl as ASPxComboBox).ImageFolder = "~/App_Themes/Glass/{0}/";
		//    //        (ctl as ASPxComboBox).CssFilePath = "~/App_Themes/Glass/{0}/styles.css";
		//    //        (ctl as ASPxComboBox).CssPostfix = "Glass";
		//    //        break;
		//    //    case Constant.QuestionaireAnswerType.LABEL:
		//    //        ctl = new ASPxLabel();
		//    //        break;
		//    //    case Constant.QuestionaireAnswerType.MEMO:
		//    //        ctl = new ASPxMemo();
		//    //        ((ASPxEdit)ctl).Height = Unit.Pixel(71);
		//    //        break;
		//    //    case Constant.QuestionaireAnswerType.RADIO:
		//    //        ctl = new ASPxRadioButton();
		//    //        ((ASPxRadioButton)ctl).Text = answerItem.AnswerText;
		//    //        ((ASPxRadioButton)ctl).GroupName = questionID;
		//    //        isRadioButton = true;
		//    //        break;
		//    //    case Constant.QuestionaireAnswerType.TEXT:
		//    //        ctl = new ASPxTextBox();
		//    //        ((ASPxTextBox)ctl).Text = answerItem.AnswerText;
		//    //        break;
		//    //}
		//    //return ctl;
		//}

        private static void PopulateQuestionAnswer(HtmlTable table, Question question, HtmlTableRow currentRow, bool isAdd, List<QuestionAnswer> listQA)
		{            
            List<QuestionAnswer> answer = new List<QuestionAnswer>();
            if (isAdd)
                answer = BusinessLayer.ListQuestionAnswer("QuestionID = '" + question.QuestionID + "' AND AnswerLevel = 1 and IsActive = 1");
            else
            {
                foreach (QuestionAnswer row in listQA)
                {
                    if (Convert.ToInt32(row.QuestionID) == question.QuestionID && row.AnswerLevel == 1)
                        answer.Add(row);
                }
            }
			//List<UspPopulateQuestionAnswer> answer = BusinessLayer.PopulateQuestionAnswer(question.QuestionID);
			Int32 questionID = question.QuestionID;

            foreach (QuestionAnswer answerItem in answer)
            {

                string answerID = answerItem.AnswerID.Replace('.', '_');

                HtmlTableRow row;
                bool isCreateOnSingleRow = false;
                if (answer.Count > 1)
                    isCreateOnSingleRow = false;
                else
                    isCreateOnSingleRow = !answerItem.IsHasChild;

                if (!isCreateOnSingleRow)
                {
                    row = new HtmlTableRow();
                    table.Rows.Add(row);
                    CreateIndentColumn(row, answerItem.AnswerLevel);
                }
                else
                {
                    row = currentRow;
                }

                Control ctl = null;
                CreateAnswer(answerItem.AnswerType, answerItem.AnswerText, answerItem.PrefixText, answerItem.SuffixText, answerItem.AnswerLevel, answerItem.IsHasChild, questionID, answerID, ctl, row);

                if (answerItem.IsHasChild)
                {
                    PopulateDetailAnswer(table, question.QuestionID, answerItem.AnswerID, isAdd, listQA);
                }
            }

		}

		private static void PopulateDetailAnswer(HtmlTable table, Int32 questionID, Int32 parentID, bool isAdd, List<QuestionAnswer> listQA)
		{
            List<QuestionAnswer> lstAnswer = new List<QuestionAnswer>();
            if (isAdd)
                lstAnswer = BusinessLayer.ListQuestionAnswer("QuestionID = '" + questionID + "' AND ParentID = '" + parentID + "' and IsActive = 1");
            else
            {
                foreach (QuestionAnswer row in listQA)
                {
                    if (Convert.ToInt32(row.QuestionID) == questionID && row.ParentID == parentID)
                        lstAnswer.Add(row);
                }
            }
			//List<UspPopulateQuestionAnswerByParentID> lstAnswer = BusinessLayer.PopulateQuestionAnswerByParentID(questionID, parentID);
            foreach (QuestionAnswer answer in lstAnswer)
			{
                if (!isAdd || (isAdd && answer.IsActive))
                {
                    string answerID = answer.AnswerID.Replace('.', '_');
                    HtmlTableRow row = new HtmlTableRow();
                    table.Rows.Add(row);

                    CreateIndentColumn(row, answer.AnswerLevel);

                    Control ctl = null;
                    CreateAnswer(answer.AnswerType, answer.AnswerText, answer.PrefixText, answer.SuffixText, answer.AnswerLevel, answer.IsHasChild,Convert.ToInt32(parentID), answerID, ctl, row);


                    if (answer.IsHasChild)
                    {
                        PopulateDetailAnswer(table, Convert.ToInt32(answer.QuestionID), answer.AnswerID, isAdd, listQA);
                    }
                }
			}
		}

        //private static void CreateAnswer(string answerType,string answerText,string prefixText,string suffixText,byte answerLevel,bool isHasChild,Int32 questionID, string answerID, Control ctl, HtmlTableRow row)
        //{
        //    bool isLabel = false;
        //    bool isRadioButton = false;
        //    switch (answerType.ToUpper())
        //    {
        //        case Constant.QuestionaireAnswerType.CHECKBOX:
        //            ctl = new ASPxCheckBox();
        //            ((ASPxCheckBox)ctl).Text = answerText;
        //            break;
        //        case Constant.QuestionaireAnswerType.COMBOBOX:
        //            ctl = new ASPxComboBox();
        //            break;
        //        case Constant.QuestionaireAnswerType.DATE:
        //            ctl = new ASPxDateEdit();
        //            ((ASPxDateEdit)ctl).HorizontalAlign = HorizontalAlign.Left;
        //            ((ASPxDateEdit)ctl).EditFormatString = Constant.FormatString.DATE_FORMAT;
        //            ((ASPxDateEdit)ctl).Width = Unit.Pixel(110);
        //            break;
        //        case Constant.QuestionaireAnswerType.LABEL:
        //            ctl = new ASPxLabel();
        //            ((ASPxLabel)ctl).Text = answerText;
        //            isLabel = true;
        //            break;
        //        case Constant.QuestionaireAnswerType.MEMO:
        //            ctl = new ASPxMemo();       
        //            ((ASPxEdit)ctl).Height = Unit.Pixel(50);
        //            break;
        //        case Constant.QuestionaireAnswerType.RADIO:
        //            ctl = new ASPxRadioButton();
        //            ((ASPxRadioButton)ctl).Text = answerText;
        //            ((ASPxRadioButton)ctl).GroupName = questionID.ToString();
        //            isRadioButton = true;
        //            break;
        //        case Constant.QuestionaireAnswerType.TEXT:
        //            ctl = new ASPxTextBox();
        //            ((ASPxTextBox)ctl).Text = answerText;
        //            break;
        //    }

        //    if (ctl != null)
        //    {
        //        HtmlTableCell cell;
        //        if (prefixText != string.Empty)
        //        {
        //            cell = new HtmlTableCell();
        //            cell.Align = "left";
        //            cell.VAlign = "middle";
        //            cell.Attributes["style"] = string.Format("width:35%;");

        //            ASPxLabel lblprefixText = new ASPxLabel();
        //            lblprefixText.ID = "lbp" + answerID;
        //            lblprefixText.Text = prefixText;
        //            cell.Controls.Add(lblprefixText);
        //            row.Cells.Add(cell);
        //        }

        //        cell = new HtmlTableCell();
        //        cell.VAlign = "middle";
        //        cell.Align = "left";
        //        if (suffixText == string.Empty)
        //            cell.Attributes["style"] = string.Format("width:35%;");
        //        else
        //            cell.Attributes["style"] = string.Format("width:10%;");

        //        if (isHasChild || isRadioButton)
        //            cell.ColSpan = prefixText == string.Empty ? 7 - answerLevel : (7 - answerLevel) - 1;


        //        ctl.ID = answerID;
        //        if (!isLabel)
        //        {
        //            ((ASPxEdit)ctl).ClientInstanceName = ctl.ID;
        //            ((ASPxEdit)ctl).Width = Unit.Percentage(100);

        //            if (ctl is ASPxComboBox)
        //            {
        //                QuestionAnswer questionAnswer = BusinessLayer.GetQuestionAnswer(questionID, answerID);
        //                if (questionAnswer != null)
        //                    Methods.AssignGeneralCodeComboBox((ASPxComboBox)ctl, questionAnswer.GCValueCodeID);
        //            }
        //        }
        //        ctl.EnableTheming = true;
        //        ctl.EnableViewState = true;

        //        //Add the Title and Edit Control to Cell
        //        cell.Controls.Add(ctl);
        //        row.Cells.Add(cell);

        //        if (suffixText != string.Empty)
        //        {
        //            cell = new HtmlTableCell();
        //            cell.Align = "left";
        //            cell.VAlign = "middle";
        //            cell.Attributes["style"] = string.Format("width:10%;");

        //            ASPxLabel lblsuffixText = new ASPxLabel();
        //            lblsuffixText.ID = "lbs" + answerID;
        //            lblsuffixText.Text = suffixText;
        //            cell.Controls.Add(lblsuffixText);
        //            row.Cells.Add(cell);
        //        }
        //    }
        //}


        //private static void CreateIndentColumn(HtmlTableRow row, int level)
        //{
        //    for (int i = 1; i <= level; i++)
        //    {	
        //        HtmlTableCell cell = new HtmlTableCell();
        //        cell.Align = "left";
        //        cell.VAlign = "middle";
        //        cell.Attributes["style"] = string.Format("width:5px;");
        //        row.Cells.Add(cell);
        //    }
        //}
     */    
	}
}
