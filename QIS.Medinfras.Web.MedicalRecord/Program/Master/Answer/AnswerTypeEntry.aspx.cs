using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class AnswerTypeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.ANSWER_TYPE;
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
                AnswerType entity = BusinessLayer.GetAnswerTypeList(String.Format("AnswerTypeID = {0}", Convert.ToInt32(ID))).FirstOrDefault();
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtAnswerTypeCode.Focus();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtAnswerTypeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAnswerTypeText1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAnswerTypeText2, new ControlEntrySetting(true, true, false));

        }

        private void EntityToControl(AnswerType entity)
        {
            txtAnswerTypeCode.Text = entity.AnswerTypeCode;
            txtAnswerTypeText1.Text = entity.AnswerTypeText1;
            txtAnswerTypeText2.Text = entity.AnswerTypeText2;
        }

        private void ControlToEntity(AnswerType entity)
        {
            entity.AnswerTypeCode = txtAnswerTypeCode.Text;
            entity.AnswerTypeText1 = txtAnswerTypeText1.Text;
            entity.AnswerTypeText2 = txtAnswerTypeText2.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("AnswerTypeCode = '{0}' AND IsDeleted = 0", txtAnswerTypeCode.Text);
            List<AnswerType> lst = BusinessLayer.GetAnswerTypeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Answer Type with Code " + txtAnswerTypeCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("AnswerTypeCode = '{0}' AND IsDeleted = 0 AND AnswerTypeID != {1}", txtAnswerTypeCode.Text, hdnID.Value);
            List<AnswerType> lst = BusinessLayer.GetAnswerTypeList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Answer Type with Code " + txtAnswerTypeCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            AnswerTypeDao entityDao = new AnswerTypeDao(ctx);
            bool result = false;

            try
            {
                AnswerType entity = new AnswerType();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetAnswerTypeMaxID(ctx).ToString();
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
                AnswerType entity = BusinessLayer.GetAnswerType(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateAnswerType(entity);
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