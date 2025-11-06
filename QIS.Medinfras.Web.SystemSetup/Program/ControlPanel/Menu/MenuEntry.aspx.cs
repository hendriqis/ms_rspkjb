using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Text;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class MenuEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.MENU;
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
                String menuID = Request.QueryString["id"];
                hdnID.Value = menuID;
                SetControlProperties();
                MenuMaster entity = BusinessLayer.GetMenuMaster(Convert.ToInt32(menuID));
                EntityToControl(entity);
                if (entity.ParentID != null && entity.ParentID > 0)
                {
                    MenuMaster entityParent = BusinessLayer.GetMenuMaster((int)entity.ParentID);
                    txtParentCode.Text = entityParent.MenuCode;
                    txtParentName.Text = entityParent.MenuCaption;
                }
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtMenuCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<Module> lstModule = BusinessLayer.GetModuleList("");
            Methods.SetComboBoxField<Module>(cboModule, lstModule, "ModuleName", "ModuleID");
            cboModule.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtMenuCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboModule, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMenuCaption, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtHelpLinkForList, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtHelpLinkForEntry, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMenuToolTip, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(hdnParentID, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtMenuUrl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtImageUrl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMenuLevel, new ControlEntrySetting(true, true, false, 0));
            SetControlEntrySetting(txtMenuIndex, new ControlEntrySetting(true, true, false, 0));
            SetControlEntrySetting(chkIsHeader, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsShowInPullDownMenu, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsVisible, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsBeginGroup, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkRead, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkCreate, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkUpdate, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkDelete, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkPrint, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkApprove, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkVoid, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkVoidWithReason, new ControlEntrySetting(true, true, false));

        }

        private void EntityToControl(MenuMaster entity)
        {
            txtMenuCode.Text = entity.MenuCode;
            cboModule.Value = entity.ModuleID;
            txtMenuCaption.Text = entity.MenuCaption;
            txtHelpLinkForList.Text = entity.HelpLinkIDForList;
            txtHelpLinkForEntry.Text = entity.HelpLinkIDForEntry;
            txtMenuToolTip.Text = entity.MenuTooltip;

            hdnParentID.Value = entity.ParentID.ToString();
            txtMenuUrl.Text = entity.MenuUrl;
            txtImageUrl.Text = entity.ImageUrl;
            txtMenuLevel.Text = entity.MenuLevel.ToString();
            txtMenuIndex.Text = entity.MenuIndex.ToString();
            chkIsHeader.Checked = entity.IsHeader;
            chkIsShowInPullDownMenu.Checked = entity.IsShowInPullDownMenu;
            chkIsVisible.Checked = entity.IsVisible;
            chkIsBeginGroup.Checked = entity.IsBeginGroup;

            CRUDMode = entity.CRUDMode;
            chkCreate.Checked = IsCanCreate;
            chkRead.Checked = IsCanRead;
            chkUpdate.Checked = IsCanUpdate;
            chkDelete.Checked = IsCanDelete;
            chkPrint.Checked = IsCanPrint;
            chkApprove.Checked = IsCanApproval;
            chkVoid.Checked = IsCanView;
            chkVoidWithReason.Checked = IsCanVoidWithReason;
        }

        private void ControlToEntity(MenuMaster entity)
        {
            entity.MenuCode = txtMenuCode.Text;
            entity.ModuleID = cboModule.Value.ToString();
            entity.MenuCaption = txtMenuCaption.Text;
            entity.HelpLinkIDForList = txtHelpLinkForList.Text;
            entity.HelpLinkIDForEntry = txtHelpLinkForEntry.Text;
            entity.MenuTooltip = txtMenuToolTip.Text;
            if (hdnParentID.Value == "" || hdnParentID.Value.ToString() == "0")
                entity.ParentID = null;
            else
                entity.ParentID = Convert.ToInt32(hdnParentID.Value);
            entity.MenuUrl = txtMenuUrl.Text;
            entity.ImageUrl = txtImageUrl.Text;
            entity.MenuLevel = Convert.ToInt16(txtMenuLevel.Text);
            entity.MenuIndex = Convert.ToInt16(txtMenuIndex.Text);
            entity.IsHeader = chkIsHeader.Checked;
            entity.IsShowInPullDownMenu = chkIsShowInPullDownMenu.Checked;
            entity.IsVisible = chkIsVisible.Checked;
            entity.IsBeginGroup = chkIsBeginGroup.Checked;

            entity.CRUDMode = GetCRUDModeControl;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("MenuCode = '{0}'", txtMenuCode.Text);
            List<MenuMaster> lst = BusinessLayer.GetMenuMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Menu with Code " + txtMenuCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 menuID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("MenuCode = '{0}' AND MenuID != {1}", txtMenuCode.Text, menuID);
            List<MenuMaster> lst = BusinessLayer.GetMenuMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Menu with Code " + txtMenuCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            MenuMasterDao entityDao = new MenuMasterDao(ctx);
            bool result = false;
            try
            {
                MenuMaster entity = new MenuMaster();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetMenuMasterMaxID(ctx).ToString();
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
                MenuMaster entity = BusinessLayer.GetMenuMaster(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                SetUserRoleMenuCRUDMode(entity);
                SetUserMenuCRUDMode(entity);

                BusinessLayer.UpdateMenuMaster(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private void SetUserRoleMenuCRUDMode(MenuMaster entity)
        {

            char[] CRUDPAVchar = { 'C', 'R', 'U', 'D', 'P', 'A', 'V', 'X' };
            List<UserRoleMenu> ListUserRoleMenu = BusinessLayer.GetUserRoleMenuList(string.Format("MenuID = {0} AND IsDeleted = 0", entity.MenuID));
            foreach (UserRoleMenu userRoleMenu in ListUserRoleMenu)
            {
                if (!entity.CRUDMode.Contains("R"))
                {
                    userRoleMenu.CRUDMode = "-------";
                    userRoleMenu.IsDeleted = true;
                }
                else
                {
                    string[] arr = userRoleMenu.CRUDMode.Split('-');
                    for (int i = 0; i < CRUDPAVchar.Length; ++i)
                    {
                        char c = CRUDPAVchar[i];
                        if (c == 'R') continue;
                        if (!entity.CRUDMode.Contains(c))
                            arr[i] = "";
                    }
                    userRoleMenu.CRUDMode = arr[0] + "-" + arr[1] + "-" + arr[2] + "-" + arr[3] + "-" + arr[4] + "-" + arr[5] + "-" + arr[6] + "-" + arr[7];
                    userRoleMenu.IsDeleted = false;
                }
                BusinessLayer.UpdateUserRoleMenu(userRoleMenu);
            }
        }

        private void SetUserMenuCRUDMode(MenuMaster entity)
        {
            char[] CRUDPAVchar = { 'C', 'R', 'U', 'D', 'P', 'A', 'V', 'X' };
            List<UserMenu> ListUserMenu = BusinessLayer.GetUserMenuList(string.Format("MenuID = {0} AND IsDeleted = 0", entity.MenuID));
            foreach (UserMenu userMenu in ListUserMenu)
            {
                if (!entity.CRUDMode.Contains("R"))
                {
                    userMenu.CRUDMode = "-------";
                    userMenu.IsDeleted = true;
                }
                else
                {
                    string[] arr = userMenu.CRUDMode.Split('-');
                    for (int i = 0; i < CRUDPAVchar.Length; ++i)
                    {
                        char c = CRUDPAVchar[i];
                        if (c == 'R') continue;
                        if (!entity.CRUDMode.Contains(c))
                            arr[i] = "";
                    }
                    userMenu.CRUDMode = arr[0] + "-" + arr[1] + "-" + arr[2] + "-" + arr[3] + "-" + arr[4] + "-" + arr[5] + "-" + arr[6] + "-" + arr[7];
                    userMenu.IsDeleted = false;
                }
                BusinessLayer.UpdateUserMenu(userMenu);
            }
        }

        #region CRUDMode
        #region EntityToControl
        private static string CRUDMode;
        private bool IsCanCreate
        {
            get
            {
                return CRUDMode.Contains("C");
            }
        }
        private bool IsCanRead
        {
            get
            {
                return CRUDMode.Contains("R");
            }
        }
        private bool IsCanUpdate
        {
            get
            {
                return CRUDMode.Contains("U");
            }
        }
        private bool IsCanDelete
        {
            get
            {
                return CRUDMode.Contains("D");
            }
        }
        private bool IsCanPrint
        {
            get
            {
                return CRUDMode.Contains("P");
            }
        }
        private bool IsCanApproval
        {
            get
            {
                return CRUDMode.Contains("A");
            }
        }
        private bool IsCanView
        {
            get
            {
                return CRUDMode.Contains("V");
            }
        }
        private bool IsCanVoidWithReason
        {
            get
            {
                return CRUDMode.Contains("X");
            }
        }
        #endregion
        private string GetCRUDModeControl
        {
            get
            {
                StringBuilder sbCRUDMode = new StringBuilder();
                sbCRUDMode.Append(chkCreate.Checked ? "C" : String.Empty).Append("-");
                sbCRUDMode.Append(chkRead.Checked ? "R" : String.Empty).Append("-");
                sbCRUDMode.Append(chkUpdate.Checked ? "U" : String.Empty).Append("-");
                sbCRUDMode.Append(chkDelete.Checked ? "D" : String.Empty).Append("-");
                sbCRUDMode.Append(chkPrint.Checked ? "P" : String.Empty).Append("-");
                sbCRUDMode.Append(chkApprove.Checked ? "A" : String.Empty).Append("-");
                sbCRUDMode.Append(chkVoid.Checked ? "V" : String.Empty).Append("-");
                sbCRUDMode.Append(chkVoidWithReason.Checked ? "X" : String.Empty);
                return sbCRUDMode.ToString();
            }
        }
        #endregion
    }
}