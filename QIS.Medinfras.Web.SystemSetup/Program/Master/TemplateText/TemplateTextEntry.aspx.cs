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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class TemplateTextEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.CANNED_TEXT;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                SetControlProperties();
                TemplateText entity = BusinessLayer.GetTemplateText(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtTemplateCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.TEMPLATE_TEXT_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboTemplateGroup, lst, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTemplateCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTemplateName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboTemplateGroup, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(TemplateText entity)
        {
            txtTemplateCode.Text = entity.TemplateCode;
            txtTemplateName.Text = entity.TemplateName;
            cboTemplateGroup.Value = entity.GCTemplateGroup;
            txtTemplateContent.Text = entity.TemplateContent;
        }

        private void ControlToEntity(TemplateText entity)
        {
            entity.TemplateCode = txtTemplateCode.Text;
            entity.TemplateName = txtTemplateName.Text;
            entity.GCTemplateGroup = cboTemplateGroup.Value.ToString();
            entity.TemplateContent = Helper.GetHTMLEditorText(txtTemplateContent);
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("TemplateCode = '{0}'", txtTemplateCode.Text);
            List<TemplateText> lst = BusinessLayer.GetTemplateTextList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Canned Text With Code " + txtTemplateCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("TemplateCode = '{0}' AND TemplateID != {1}", txtTemplateCode.Text, hdnID.Value);
            List<TemplateText> lst = BusinessLayer.GetTemplateTextList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Canned Text With Code " + txtTemplateCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            TemplateTextDao entityDao = new TemplateTextDao(ctx);
            bool result = false;
            try
            {
                TemplateText entity = new TemplateText();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetTemplateTextMaxID(ctx).ToString();
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
                TemplateText entity = BusinessLayer.GetTemplateText(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateTemplateText(entity);
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