using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class QuestionEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.QUESTION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                Question entity = BusinessLayer.GetQuestionList(String.Format("QuestionID = {0}", Convert.ToInt32(ID))).FirstOrDefault();
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtQuestionCode.Focus();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtQuestionCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtQuestionText1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtQuestionText2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSummarizeDisplayText1, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSummarizeDisplayText2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHeader, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(chkIsUsinginlineAnswer, new ControlEntrySetting(true, true, false, true));

        }

        private void EntityToControl(Question entity)
        {
            txtQuestionCode.Text = entity.QuestionCode;
            txtQuestionText1.Text = entity.QuestionText1;
            txtQuestionText2.Text = entity.QuestionText2;
            hdnParentID.Value = entity.ParentID.ToString();
            txtParentCode.Text = entity.QuestionCode;
            txtParentName.Text = entity.QuestionText1;
            txtSummarizeDisplayText1.Text = entity.SummarizeDisplayText1;
            txtSummarizeDisplayText2.Text = entity.SummarizeDisplayText2;
            chkIsHeader.Checked = Convert.ToBoolean(entity.IsHeader);
            chkIsUsinginlineAnswer.Checked = Convert.ToBoolean(entity.IsUsingInlineAnswer);
        }

        private void ControlToEntity(Question entity)
        {
            entity.QuestionCode = txtQuestionCode.Text;
            entity.QuestionText1 = txtQuestionText1.Text;
            entity.QuestionText2 = txtQuestionText2.Text;
            if (hdnParentID.Value != null && hdnParentID.Value != "")
            {
                entity.ParentID = Convert.ToInt32(hdnParentID.Value);
            }
            else
            {
                entity.ParentID = null;
            }
            entity.SummarizeDisplayText1 = txtSummarizeDisplayText1.Text;
            entity.SummarizeDisplayText2 = txtSummarizeDisplayText2.Text;
            entity.IsHeader = Convert.ToInt32(chkIsHeader.Checked);
            entity.IsUsingInlineAnswer = chkIsUsinginlineAnswer.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("QuestionCode = '{0}'", txtQuestionCode.Text);
            List<Question> lst = BusinessLayer.GetQuestionList(FilterExpression);

            if (!chkIsHeader.Checked)
            {
                if (String.IsNullOrEmpty(hdnParentID.Value))
                {
                    errMessage = "Parent ID Belum Diisi";
                }
            }

            if (lst.Count > 0)
                errMessage = " Question with Code " + txtQuestionCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("QuestionCode = '{0}' AND QuestionID != {1}", txtQuestionCode.Text, hdnID.Value);
            List<Question> lst = BusinessLayer.GetQuestionList(FilterExpression);

            if (!chkIsHeader.Checked)
            {
                if (String.IsNullOrEmpty(hdnParentID.Value))
                {
                    errMessage = "Parent ID Belum Diisi";
                }
            }

            if (lst.Count > 0)
                errMessage = " Question Group with Code " + txtQuestionCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            QuestionDao entityDao = new QuestionDao(ctx);
            bool result = false;

            try
            {
                Question entity = new Question();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetQuestionMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                Question entity = BusinessLayer.GetQuestion(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateQuestion(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}