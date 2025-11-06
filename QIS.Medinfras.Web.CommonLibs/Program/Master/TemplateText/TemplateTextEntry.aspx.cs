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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TemplateTextEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            String TemplateType = hdnTemplateType.Value;
            switch (TemplateType)
            {
                case Constant.TemplateTextType.IMAGING: return Constant.MenuCode.Imaging.TEMPLATE_GROUP;
                case Constant.TemplateTextType.LABORATORY: return Constant.MenuCode.Laboratory.TEMPLATE_GROUP;
                default: return Constant.MenuCode.MedicalDiagnostic.TEMPLATE_GROUP;
            }
        }

        protected String GetPageTitle()
        {
            String TemplateType = hdnTemplateType.Value;
            switch (TemplateType)
            {
                case Constant.TemplateTextType.IMAGING: return GetLabel("Imaging Test Result Template");
                case Constant.TemplateTextType.LABORATORY: return GetLabel("Laboratory Test Result Template");
                default: return GetLabel("Diagnostic Test Result Template");
            }
        }

        protected override void InitializeDataControl()
        {
            String[] param = Request.QueryString["id"].Split('|');
            hdnTemplateType.Value = param[0];
            if (param.Length > 1)
            {
                IsAdd = false;
                Int32 ID = Convert.ToInt32(param[1]);
                hdnID.Value = ID.ToString();
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

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            string GCTemplateGroup = "";

            if (hdnTemplateType.Value == "IMAGING")
                GCTemplateGroup = (string.Format("StandardCodeID = '{0}'", Constant.TemplateGroup.IMAGING));
            else if (hdnTemplateType.Value == "LABORATORY")
                GCTemplateGroup = (string.Format("StandardCodeID = '{0}'", Constant.TemplateGroup.LABORATORY));
            else if (hdnTemplateType.Value == "DIAGNOSTIC")
                GCTemplateGroup = (string.Format("StandardCodeID = '{0}'", Constant.TemplateGroup.DIAGNOSTIC));

            if (GCTemplateGroup != "") trTemplateGroup.Style.Add("Display", "None");

            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(String.Format("{0} AND IsDeleted = 0", GCTemplateGroup));
            Methods.SetComboBoxField<StandardCode>(cboTemplateGroup, lst, "StandardCodeName", "StandardCodeID");
            cboTemplateGroup.SelectedIndex = 0;
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
                errMessage = " Template Text With Code " + txtTemplateCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("TemplateCode = '{0}' AND TemplateID != {1}", txtTemplateCode.Text, hdnID.Value);
            List<TemplateText> lst = BusinessLayer.GetTemplateTextList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Template Text With Code " + txtTemplateCode.Text + " is already exist!";

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