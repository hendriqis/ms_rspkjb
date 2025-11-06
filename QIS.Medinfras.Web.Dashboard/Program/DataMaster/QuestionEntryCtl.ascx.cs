using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class QuestionEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnSurveyID.Value = param;
            DashboardSurvey entity = BusinessLayer.GetDashboardSurvey(Convert.ToInt32(hdnSurveyID.Value));
            txtSurveyCode.Text = entity.SurveyCode;
            txtSurveyName.Text = entity.SurveyName;

            string filterExpression = string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.TIPE_PERTANYAAN_SURVEY);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboQuestionType, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboQuestionType.SelectedIndex = 0;
            BindGridView();

            txtQuestionCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtQuestionName.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        protected string OnGetItemBedChargesFilterExpression()
        {
            return string.Format("GCItemType = '{0}' AND IsDeleted = 0 AND GCItemStatus != '{1}'", Constant.ItemGroupMaster.SERVICE, Constant.ItemStatus.IN_ACTIVE);
        }

        protected string onGetNursingServiceItemFilterExpression()
        {
            return string.Format("GCItemType = '{0}' AND IsDeleted = 0 AND GCItemStatus != '{1}'", Constant.ItemGroupMaster.SERVICE, Constant.ItemStatus.IN_ACTIVE);
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetDashboardQuestionList(string.Format("SurveyID = '{0}' AND IsDeleted = 0", hdnSurveyID.Value));
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnQuestionID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(DashboardQuestion entity)
        {
            entity.QuestionCode = txtQuestionCode.Text;
            entity.QuestionName = txtQuestionName.Text;
            entity.QuestionNotes = txtQuestionNotes.Text;
            entity.GCQuestionType = cboQuestionType.Value.ToString();
            entity.Answer01 = txtAnswer1.Text;
            entity.Answer02 = txtAnswer2.Text;
            entity.Answer03 = txtAnswer3.Text;
            entity.Answer04 = txtAnswer4.Text;
            entity.Answer05 = txtAnswer5.Text;
        }


        private bool IsValidIPAddress(string ipAddress)
        {
            //create our match pattern
            string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            //create our Regular Expression object
            Regex check = new Regex(pattern);
            //boolean variable to hold the status
            bool valid = false;
            //check to make sure an ip address was provided
            if (ipAddress == "")
            {
                //no address provided   so return false
                valid = false;
            }
            else
            {
                //address provided so use the IsMatch Method
                //of the Regular Expression object
                valid = check.IsMatch(ipAddress, 0);
            }
            //return the results
            return valid;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                errMessage = string.Empty;
                DashboardQuestion entity = new DashboardQuestion();
                
                string FilterExpression = string.Format("QuestionCode = '{0}'", txtQuestionCode.Text);
                List<DashboardQuestion> lst = BusinessLayer.GetDashboardQuestionList(FilterExpression);

                if (lst.Count > 0)
                {
                    errMessage = " Survey with Code " + txtQuestionCode.Text + " already exist!";
                    return (errMessage == string.Empty);
                }
                else
                {
                    ControlToEntity(entity);
                    entity.SurveyID = Convert.ToInt32(hdnSurveyID.Value);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.InsertDashboardQuestion(entity);
                    return true;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                errMessage = string.Empty;
                Int32 ID = Convert.ToInt32(hdnQuestionID.Value);
                DashboardQuestion entity = BusinessLayer.GetDashboardQuestion(Convert.ToInt32(hdnQuestionID.Value));
                ControlToEntity(entity);
                string FilterExpression = string.Format("QuestionCode = '{0}' AND QuestionID != {1}", txtQuestionCode.Text, ID);
                List<DashboardQuestion> lst = BusinessLayer.GetDashboardQuestionList(FilterExpression);

                if (lst.Count > 0)
                {
                    errMessage = " Survey with Code " + txtQuestionCode.Text + " already exist!";

                    return (errMessage == string.Empty);
                }
                else
                {
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateDashboardQuestion(entity);
                    return true;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                DashboardQuestion entity = BusinessLayer.GetDashboardQuestion(Convert.ToInt32(hdnQuestionID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateDashboardQuestion(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}