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
    public partial class AnswerEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.ANSWER;
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
                Answer entity = BusinessLayer.GetAnswerList(String.Format("AnswerID = {0}", Convert.ToInt32(ID))).FirstOrDefault();
                SetControlProperties();
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtAnswerCode.Focus();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtAnswerCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAnswerText1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAnswerText2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPrefixText, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSuffixText, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHasChild, new ControlEntrySetting(true, true, false, true));

        }

        private void EntityToControl(Answer entity)
        {
            txtAnswerCode.Text = entity.AnswerCode;
            txtAnswerText1.Text = entity.AnswerText1;
            txtAnswerText2.Text = entity.AnswerText2;
            hdnParentID.Value = entity.ParentID.ToString();
            txtParentCode.Text = entity.AnswerCode;
            txtParentName.Text = entity.AnswerText1;
            chkIsUsingPrefix.Checked = entity.IsUsingPrefix;
            if (!string.IsNullOrEmpty(entity.PrefixText))
            {
                txtPrefixText.Text = entity.PrefixText;
            }
            chkIsUsingSuffix.Checked = entity.IsUsingSuffix;
            if (!string.IsNullOrEmpty(entity.SuffixText))
            {
                txtSuffixText.Text = entity.SuffixText;
            }
            txtPrefixText.Text = entity.PrefixText;
            txtSuffixText.Text = entity.SuffixText;
            chkIsHasChild.Checked = Convert.ToBoolean(entity.IsHasChild);
        }

        private void ControlToEntity(Answer entity)
        {
            entity.AnswerCode = txtAnswerCode.Text;
            entity.AnswerText1 = txtAnswerText1.Text;
            entity.AnswerText2 = txtAnswerText2.Text;
            entity.IsUsingPrefix = chkIsUsingPrefix.Checked;
            entity.IsUsingSuffix = chkIsUsingSuffix.Checked;
            entity.PrefixText = txtPrefixText.Text;
            entity.SuffixText = txtSuffixText.Text;
            if (hdnParentID.Value != null && hdnParentID.Value != "")
            {
                entity.ParentID = Convert.ToInt32(hdnParentID.Value);
            }
            else
            {
                entity.ParentID = null;
            }
            entity.IsHasChild = Convert.ToInt32(chkIsHasChild.Checked);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("AnswerCode = '{0}' AND IsDeleted = 0", txtAnswerCode.Text);
            List<Answer> lst = BusinessLayer.GetAnswerList(FilterExpression);

            if (!chkIsHasChild.Checked)
            {
                if (String.IsNullOrEmpty(hdnParentID.Value))
                {
                    errMessage = "Parent ID Belum Diisi";
                }
            }

            if (lst.Count > 0)
                errMessage = " Answer with Code " + txtAnswerCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("AnswerCode = '{0}' AND IsDeleted = 0 AND AnswerID != {1}", txtAnswerCode.Text, hdnID.Value);
            List<Answer> lst = BusinessLayer.GetAnswerList(FilterExpression);

            if (!chkIsHasChild.Checked)
            {
                if (String.IsNullOrEmpty(hdnParentID.Value))
                {
                    errMessage = "Parent ID Belum Diisi";
                }
            }

            if (lst.Count > 0)
                errMessage = " Answer with Code " + txtAnswerCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            AnswerDao entityDao = new AnswerDao(ctx);
            bool result = false;

            try
            {
                Answer entity = new Answer();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetAnswerMaxID(ctx).ToString();
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
                Answer entity = BusinessLayer.GetAnswer(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateAnswer(entity);
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