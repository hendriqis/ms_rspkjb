using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Dashboard.Program
{
    public partial class SurveyPasien : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Dashboard.Survey;
        }

        List<DashboardSurvey> lstSurvey;
        List<DashboardQuestion> lstQuestion;
        List<DashboardQuestionAnswer> lstQA;
        string urlSurveyID;
        protected List<DataTemp> lstTemp = new List<DataTemp>();

        protected override void InitializeDataControl()
        {
            string filterExp = string.Format("IsDeleted = 0");
            lstSurvey = BusinessLayer.GetDashboardSurveyList(filterExp);
            lstQuestion = BusinessLayer.GetDashboardQuestionList(filterExp);
            lstQA = BusinessLayer.GetDashboardQuestionAnswerList(filterExp);
            Uri url = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
            urlSurveyID = HttpUtility.ParseQueryString(url.Query).Get("SurveyID");
            DashboardQuestionAnswer entity = BusinessLayer.GetDashboardQuestionAnswer(Convert.ToInt32(urlSurveyID));

            getQuestion();
        }

        private void getQuestion()
        {
            int surveySelection = Convert.ToInt32(urlSurveyID);
            var surveyQuestion = lstQuestion.Where(x => x.SurveyID == surveySelection).Select(y => new { y.QuestionID, y.QuestionCode, y.GCQuestionType, y.QuestionName, y.Answer01, y.Answer02, y.Answer03, y.Answer04, y.Answer05 }).ToList();
            JsonChartListPertanyaan.Value = JsonConvert.SerializeObject(surveyQuestion, Formatting.Indented);
        }

        private bool insertAnswerSurvey(ref string errMessage)
        {
            try
            {
                errMessage = string.Empty;
                DashboardQuestionAnswer entity = new DashboardQuestionAnswer();

                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertDashboardQuestionAnswer(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        protected void setDataBeforeSave()
        {
            string[] paramNew = hdnDataAnswerID.Value.Split('$');
            lstTemp = new List<DataTemp>();

            for (int i = 0; i < paramNew.Length; i++)
            {
                string Answer01 = null;
                string Answer02 = null;
                string Answer03 = null;
                string Answer04 = null;
                string Answer05 = null;
                DataTemp oData = new DataTemp();
                if (!String.IsNullOrEmpty(paramNew[i]))
                {
                    string[] paramNewSplit = paramNew[i].Split('|');
                    int QuestionID = Convert.ToInt32(paramNewSplit[1]);
                    string ParticipantName = paramNewSplit[2];
                    Answer01 = paramNewSplit[3];
                    Answer02 = paramNewSplit[4];
                    Answer03 = paramNewSplit[5];
                    Answer04 = paramNewSplit[6];
                    Answer05 = paramNewSplit[7];

                    
                    oData.QuestionID = QuestionID;
                    oData.ParticipantName = ParticipantName;
                    oData.Answer01 = Answer01;
                    oData.Answer02 = Answer02;
                    oData.Answer03 = Answer03;
                    oData.Answer04 = Answer04;
                    oData.Answer05 = Answer05;
                    
                    lstTemp.Add(oData);
                }
                
            }
        }

        private void ControlToEntity(DashboardQuestionAnswer entity)
        {
            setDataBeforeSave();
            foreach (DataTemp e in lstTemp) {
                DashboardQuestionAnswer entityQA = new DashboardQuestionAnswer();
                entityQA.QuestionID = e.QuestionID;
                entityQA.ParticipantName = e.ParticipantName;
                entityQA.Answer01 = e.Answer01;
                entityQA.Answer02 = e.Answer02;
                entityQA.Answer03 = e.Answer03;
                entityQA.Answer04 = e.Answer04;
                entityQA.Answer05 = e.Answer05;
                BusinessLayer.InsertDashboardQuestionAnswer(entityQA);
            }
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                errMessage = string.Empty;
                DashboardQuestionAnswer entity = new DashboardQuestionAnswer();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string action = e.Parameter;

            string result = action + "|";
            string errMessage = "";
            string answer = hdnDataAnswerID.Value;
            if (action == "save")
            {
                if (answer == "")
                {
                    result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    OnSaveAddRecord(ref errMessage);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public class DataTemp
        {
            public int QuestionID { get; set; }
            public string ParticipantName { get; set; }
            public string Answer01 { get; set; }
            public string Answer02 { get; set; }
            public string Answer03 { get; set; }
            public string Answer04 { get; set; }
            public string Answer05 { get; set; }
        }

        public class DataValid
        {
            public int QuestionID { get; set; }
            public string ParticipantName { get; set; }
            public string Answer01 { get; set; }
            public string Answer02 { get; set; }
            public string Answer03 { get; set; }
            public string Answer04 { get; set; }
            public string Answer05 { get; set; }
        }
    }
}