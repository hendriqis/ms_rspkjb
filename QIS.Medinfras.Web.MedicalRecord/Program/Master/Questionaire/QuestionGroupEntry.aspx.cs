using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class QuestionGroupEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.QUESTION_GROUP;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            String param = Request.QueryString["id"];
            chkIsHeader.Checked = true;

            if (!string.IsNullOrEmpty(param))
            {
                IsAdd = false;
                hdnID.Value = param;
                vQuestionGroup entity = BusinessLayer.GetvQuestionGroupList(string.Format("QuestionGroupID = {0}", hdnID.Value))[0];
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtQuestionGroupText1.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtQuestionGroupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtQuestionGroupText1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtQuestionGroupText2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPrintOrder, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsHeader, new ControlEntrySetting(false, false, false, true));
        }

        private void EntityToControl(vQuestionGroup entity)
        {
            txtQuestionGroupCode.Text = entity.QuestionGroupCode;
            txtQuestionGroupText1.Text = entity.QuestionGroupText1;
            txtQuestionGroupText2.Text = entity.QuestionGroupText2;
            txtPrintOrder.Text = entity.DisplayOrder;
            chkIsHeader.Checked = entity.IsHeader;
            txtNotes.Text = entity.Remarks;
        }

        private void ControlToEntity(QuestionGroup entity)
        {
            entity.QuestionGroupCode = txtQuestionGroupCode.Text;
            entity.QuestionGroupText1 = txtQuestionGroupText1.Text;
            entity.QuestionGroupText2 = txtQuestionGroupText2.Text;
            entity.DisplayOrder = txtPrintOrder.Text;
            entity.ParentID = null;
            entity.IsHeader = chkIsHeader.Checked;
            entity.Remarks = txtNotes.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("QuestionGroupCode = '{0}'", txtQuestionGroupCode.Text);
            List<QuestionGroup> lst = BusinessLayer.GetQuestionGroupList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Question Group with Code " + txtQuestionGroupCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("QuestionGroupCode = '{0}' AND QuestionGroupID != {1}", txtQuestionGroupCode.Text, hdnID.Value);
            List<QuestionGroup> lst = BusinessLayer.GetQuestionGroupList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Question Group with Code " + txtQuestionGroupCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            QuestionGroupDao entityDao = new QuestionGroupDao(ctx);
            bool result = false;
            try
            {
                QuestionGroup entity = new QuestionGroup();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();

                ctx.CommitTransaction();

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            QuestionGroupDao entityDao = new QuestionGroupDao(ctx);
            bool result = false;
            try
            {
                QuestionGroup entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                ctx.CommitTransaction();

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}