using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ModuleEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.MODULE;
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
                String moduleID = Request.QueryString["id"];
                hdnID.Value = moduleID;
                Module entity = BusinessLayer.GetModule(moduleID);
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtModuleCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtModuleCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtModuleName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtShortName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtModuleIndex, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtDescription, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtImageUrl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDisabledImageUrl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDefaultUrl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsVisible, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(Module entity)
        {
            txtModuleCode.Text = entity.ModuleID;
            txtModuleName.Text = entity.ModuleName;
            txtShortName.Text = entity.ModuleShortName;
            txtModuleIndex.Text = entity.ModuleIndex.ToString();
            txtDescription.Text = entity.Description;
            txtImageUrl.Text = entity.ImageUrl;
            txtDisabledImageUrl.Text = entity.DisabledImageUrl;
            txtDefaultUrl.Text = entity.DefaultUrl;
            chkIsVisible.Checked = entity.IsVisible;
        }

        private void ControlToEntity(Module entity)
        {
            entity.ModuleID = txtModuleCode.Text;
            entity.ModuleName = txtModuleName.Text;
            entity.ModuleShortName = txtShortName.Text;
            entity.ModuleIndex = Convert.ToInt16(txtModuleIndex.Text);
            entity.Description = txtDescription.Text;
            entity.ImageUrl = txtImageUrl.Text;
            entity.DisabledImageUrl = txtDisabledImageUrl.Text;
            entity.DefaultUrl = txtDefaultUrl.Text;
            entity.IsVisible = chkIsVisible.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ModuleID = '{0}'", Request.Form[txtModuleCode.UniqueID]);
            List<Module> lst = BusinessLayer.GetModuleList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Module with Code " + txtModuleCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                Module entity = new Module();
                ControlToEntity(entity);
                BusinessLayer.InsertModule(entity);
                retval = entity.ModuleID;
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                Module entity = BusinessLayer.GetModule(hdnID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateModule(entity);
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