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
using System.Text.RegularExpressions;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class QuestionEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnQuestionID.Value = param;
            Question entity = BusinessLayer.GetQuestion(Convert.ToInt32(hdnQuestionID.Value));
            txtQuestionCode.Text = entity.QuestionCode.ToString();
            txtQuestionName.Text = entity.QuestionText1;
            BindGridView();
            hdnAnswerID.Attributes.Add("validationgroup", "mpEntryPopup");
            txtDisplayOrder.Attributes.Add("validationgroup", "mpEntryPopup");

        }

        private string GetFilterExpressionQuestionAnswer()
        {
            string filterExpression = String.Format("QuestionID = {0}", hdnQuestionID.Value); //hdnFilterExpression.Value;
            filterExpression += " AND IsDeleted = 0";
            return filterExpression;
        }


        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetQuestionAnswerList(string.Format("AnswerID = '{0}' AND IsDeleted = 0 ORDER BY QuestionAnswerID ASC", hdnQuestionID.Value));
            grdView.DataBind();


            string filterExpression = GetFilterExpressionQuestionAnswer();

            List<vQuestionAnswer> lstEntity = BusinessLayer.GetvQuestionAnswerList(filterExpression);
            grdView.DataSource = lstEntity;
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
                if (hdnQuestionAnswerID.Value.ToString() != "")
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

        private void ControlToEntity(QuestionAnswer entity)
        {
            //entity.QuestionAnswerID = Convert.ToInt32(txtQuestionAnswerCode.Text);
            //entity.QuestionID = Convert.ToInt32(txtQuestionID.Text);
            //entity.AnswerID = Convert.ToInt32(txtAnswerID.Text);
            entity.AnswerID = Convert.ToInt32(hdnAnswerID.Value);
            entity.DisplayOrder = txtDisplayOrder.Text;

        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            //bool result = true;
            //IDbContext ctx = DbFactory.Configure(true);
            //QuestionAnswerDao entityDao = new QuestionAnswerDao(ctx);
            try
            {
                QuestionAnswer entity = new QuestionAnswer();
                ControlToEntity(entity);
                entity.QuestionID =Convert.ToInt32(hdnQuestionID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertQuestionAnswer(entity);
                return true;
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

            //bool result = true;
            //IDbContext ctx = DbFactory.Configure(true);
            //QuestionAnswerDao entityDao = new QuestionAnswerDao(ctx);
            try
            {
                QuestionAnswer entity = BusinessLayer.GetQuestionAnswer(Convert.ToInt32(hdnQuestionAnswerID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateQuestionAnswer(entity);
                return true;
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
                QuestionAnswer entity = BusinessLayer.GetQuestionAnswer(Convert.ToInt32(hdnQuestionAnswerID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateQuestionAnswer(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private int InsertQuestionAnswer(IDbContext ctx)
        {
            QuestionAnswerDao entityDao = new QuestionAnswerDao(ctx);
            int result = 0;
            try
            {
                QuestionAnswer entity = BusinessLayer.GetQuestionAnswer(Convert.ToInt32(hdnQuestionID.Value));
                if (entity != null)
                {
                    result = entity.QuestionID;
                }
                else
                {
                    entity = new QuestionAnswer();
                    entity.QuestionID = Convert.ToInt32(txtQuestionID.Text);
                    entity.IsDeleted = false;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entityDao.Insert(entity);
                    result = BusinessLayer.GetQuestionAnswerMaxID(ctx);
                }
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;

        }
    }
}