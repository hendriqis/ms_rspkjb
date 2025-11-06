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
    public partial class UserRolesEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.USER_ROLES;
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
                String roleID = Request.QueryString["id"];
                hdnID.Value = roleID;
                UserRole entity = BusinessLayer.GetUserRole(Convert.ToInt32(roleID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtRoleName.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtRoleName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDescription, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDefaultPageUrl, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(UserRole entity)
        {
            txtRoleName.Text = entity.RoleName;
            txtDescription.Text = entity.Description;
            txtDefaultPageUrl.Text = entity.DefaultPageUrl;
        }

        private void ControlToEntity(UserRole entity)
        {
            entity.RoleName = txtRoleName.Text;
            entity.Description = txtDescription.Text;
            entity.LoweredRoleName = entity.RoleName.ToLower();
            entity.DefaultPageUrl = txtDefaultPageUrl.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("RoleName = '{0}'", txtRoleName.Text);
            List<UserRole> lst = BusinessLayer.GetUserRoleList(FilterExpression);

            if (lst.Count > 0)
                errMessage = "Role name is already exist";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            UserRoleDao entityDao = new UserRoleDao(ctx);
            bool result = false;
            try
            {
                UserRole entity = new UserRole();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetUserRoleMaxID(ctx).ToString();
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
            try
            {
                UserRole entity = BusinessLayer.GetUserRole(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateUserRole(entity);
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