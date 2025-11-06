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
    public partial class TemplatePrescriptionEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.MASTER_PHYSICIAN_PRESCRIPTION_TEMPLATE;
        }

        protected String GetPageTitle()
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
                PhysicianText entity = BusinessLayer.GetPhysicianText(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtTemplateCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;           
        }


        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTemplateCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTemplateName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboTemplateGroup, new ControlEntrySetting(true, true, true));

            List<StandardCode> lstLocationGroup = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.PHYSICIAN_TEXT_TEMPLATE));
            Methods.SetComboBoxField<StandardCode>(cboTemplateGroup, lstLocationGroup, "StandardCodeName", "StandardCodeID", DevExpress.Web.ASPxEditors.DropDownStyle.DropDown);
        }

        private void EntityToControl(PhysicianText  entity)
        {
            txtTemplateCode.Text = entity.TemplateCode;
            txtTemplateName.Text = entity.TemplateText;
            cboTemplateGroup.Value = entity.GCTextTemplateGroup;
        }

        private void ControlToEntity(PhysicianText entity)
        {
            entity.TemplateCode = txtTemplateCode.Text;
            entity.TemplateText = txtTemplateName.Text;
            entity.GCTextTemplateGroup = cboTemplateGroup.Value.ToString();
            entity.UserID = AppSession.UserLogin.UserID;           
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("TemplateCode = '{0}' AND UserID = {1} AND IsDeleted = 0 ", txtTemplateCode.Text, AppSession.UserLogin.UserID);
            List<PhysicianText> lst = BusinessLayer.GetPhysicianTextList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Template Text With Code " + txtTemplateCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("TemplateCode = '{0}' AND ID != {1} AND UserID = {2} AND IsDeleted = 0 ", txtTemplateCode.Text, hdnID.Value, AppSession.UserLogin.UserID);
            List<PhysicianText> lst = BusinessLayer.GetPhysicianTextList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Template Text With Code " + txtTemplateCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            PhysicianTextDao entityDao = new PhysicianTextDao(ctx);
            bool result = false;
            try
            {
                PhysicianText entity = new PhysicianText();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetPhysicianTextMaxID(ctx).ToString();
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
                PhysicianText entity = BusinessLayer.GetPhysicianText(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePhysicianText(entity);
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