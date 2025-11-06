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
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Dashboard.Program
{
    public partial class SurveyInformationCtl : BaseViewPopupCtl
    {
        List<DashboardSurvey> lstData;
        List<DashboardQuestion> lstDataPertanyaan;
        List<DashboardQuestionAnswer> lstDataJawaban;
        public override void InitializeDataControl(string param)
        {
            string filterExpression = string.Format("IsDeleted = 0 ORDER BY SurveyName ASC");
            lstData = BusinessLayer.GetDashboardSurveyList(filterExpression);
            Methods.SetComboBoxField<DashboardSurvey>(surveyGraphSelection, lstData, "SurveyName", "SurveyID");
            surveyGraphSelection.SelectedIndex = 0;

        }

        private void getSurveyInfo()
        {
            var selectedsurveyName = hdnSurveyName.Value;
            var selectedSurveyIDString = hdnSurveyID.Value;
            var selectedSurveyID = int.Parse(hdnSurveyID.Value);
            DateTime dateFrom = DateTime.Parse(hdnDateFrom.Value);
            TimeSpan beforeTmr = new TimeSpan(23, 59, 59);
            DateTime dateTo = DateTime.Parse(hdnDateTo.Value).Add(beforeTmr);


            string filterExpression = string.Format("(CONVERT(DATE,CreatedDate) BETWEEN '{0}' AND '{1}') AND IsDeleted = 0", dateFrom, dateTo);
            string filterSurvey = string.Format("IsDeleted = 0 AND SurveyID = {0}", selectedSurveyIDString);
            string filterPertanyaan = string.Format("IsDeleted = 0 AND SurveyID = {0} AND GCQuestionType != 'X553^003'", selectedSurveyIDString);
            lstData = BusinessLayer.GetDashboardSurveyList(filterSurvey);
            lstDataPertanyaan = BusinessLayer.GetDashboardQuestionList(filterPertanyaan);
            lstDataJawaban = BusinessLayer.GetDashboardQuestionAnswerList(filterExpression);


            List<DataJawaban> lstJmlJawaban = new List<DataJawaban>();
            string jawabanSTR = "";

            if (lstDataPertanyaan.Count == 0)
            {
                hdnData.Value = "0";
                return;
            }
            else
            {
                foreach (DashboardQuestion e in lstDataPertanyaan)
                {
                    var jmlDataJawaban = lstDataJawaban.Where(t => t.QuestionID == e.QuestionID).Count();
                    var jmlDataPertanyaan = lstDataPertanyaan.Count();
                    if (jmlDataJawaban == 0)
                    {
                        hdnData.Value = "0";
                        return;
                    }
                    DataJawaban obj = new DataJawaban();
                    obj.QuestionID = e.QuestionID;
                    obj.QuestionText = e.QuestionName;
                    obj.txtAnswer01 = e.Answer01;
                    obj.txtAnswer02 = e.Answer02;
                    obj.txtAnswer03 = e.Answer03;
                    obj.txtAnswer04 = e.Answer04;
                    obj.txtAnswer05 = e.Answer05;
                    obj.jmlAnswer01 = lstDataJawaban.Where(t => t.QuestionID == e.QuestionID && t.Answer01 != "null" && t.Answer01 != "" && t.Answer01 != null).Count();
                    obj.jmlAnswer02 = lstDataJawaban.Where(t => t.QuestionID == e.QuestionID && t.Answer02 != "null" && t.Answer02 != "" && t.Answer02 != null).Count();
                    obj.jmlAnswer03 = lstDataJawaban.Where(t => t.QuestionID == e.QuestionID && t.Answer03 != "null" && t.Answer03 != "" && t.Answer03 != null).Count();
                    obj.jmlAnswer04 = lstDataJawaban.Where(t => t.QuestionID == e.QuestionID && t.Answer04 != "null" && t.Answer04 != "" && t.Answer04 != null).Count();
                    obj.jmlAnswer05 = lstDataJawaban.Where(t => t.QuestionID == e.QuestionID && t.Answer05 != "null" && t.Answer05 != "" && t.Answer05 != null).Count();
                    lstJmlJawaban.Add(obj);

                    if (jawabanSTR != "")
                    {
                        jawabanSTR += "[]sAnswer|" + obj.QuestionText + '|' + obj.txtAnswer01 + '|' + obj.txtAnswer02 + '|' +
                            obj.txtAnswer03 + '|' + obj.txtAnswer04 + '|' + obj.txtAnswer05 + '|' + obj.jmlAnswer01 + '|' +
                            obj.jmlAnswer02 + '|' + obj.jmlAnswer03 + '|' + obj.jmlAnswer04 + '|' + obj.jmlAnswer05;
                    }
                    else
                    {
                        jawabanSTR += "sAnswer|" + obj.QuestionText + '|' + obj.txtAnswer01 + '|' + obj.txtAnswer02 + '|' +
                            obj.txtAnswer03 + '|' + obj.txtAnswer04 + '|' + obj.txtAnswer05 + '|' + obj.jmlAnswer01 + '|' +
                            obj.jmlAnswer02 + '|' + obj.jmlAnswer03 + '|' + obj.jmlAnswer04 + '|' + obj.jmlAnswer05;
                    }
                }
            }
            hdnData.Value = jawabanSTR;
        }

        protected void cbpSurveyInformation_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string action = e.Parameter;

            string result = action + "|";
            string errMessage = "";
            if (action == "save")
            {
                getSurveyInfo();
            }
            else
            {
                result += string.Format("fail|{0}", errMessage);
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public class DataJawaban
        {
            public int QuestionID { get; set; }
            public string QuestionText { get; set; }
            public string txtAnswer01 { get; set; }
            public string txtAnswer02 { get; set; }
            public string txtAnswer03 { get; set; }
            public string txtAnswer04 { get; set; }
            public string txtAnswer05 { get; set; }
            public int jmlAnswer01 { get; set; }
            public int jmlAnswer02 { get; set; }
            public int jmlAnswer03 { get; set; }
            public int jmlAnswer04 { get; set; }
            public int jmlAnswer05 { get; set; }
        }
    }
}