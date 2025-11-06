using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class UserRolesMenuAccessEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            ListStateMenuAccess.Clear();

            hdnRoleID.Value = param;

            UserRole ur = BusinessLayer.GetUserRole(Convert.ToInt32(param));
            txtUserRoleName.Text = ur.RoleName;

            List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("");
            if (lstHealthcare.Count > 0)
            {
                Methods.SetComboBoxField<Healthcare>(ddlHealthcare, lstHealthcare, "HealthcareName", "HealthcareID");
                hdnSelectedHealthcare.Value = lstHealthcare[0].HealthcareID.ToString();

                List<Module> lstModule = BusinessLayer.GetModuleList("IsVisible = 1");
                Methods.SetComboBoxField<Module>(ddlModule, lstModule, "ModuleName", "ModuleID");
                hdnSelectedModule.Value = lstModule[0].ModuleID;
                List<UserRole> lstUserRole = BusinessLayer.GetUserRoleList(GetFilterExpression());
                //TO-DO
                //Methods.SetComboBoxField<UserRole>(ddlProfile, lstUserRole, "RoleName", "RoleID");
                ListUserRoleMenuAccess = BusinessLayer.GetUserRoleMenuList(hdnSelectedModule.Value, hdnSelectedHealthcare.Value, Convert.ToInt32(hdnRoleID.Value), AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);

                BindGridView(1, true, ref PageCount);
            }
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            if (AppSession.UserLogin.UserID != 1)
                filterExpression += "RoleID != 1 AND IsDeleted = 0";
            else
                filterExpression += "IsDeleted = 0";
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<GetUserRoleMenuList> lstEntity = ListUserRoleMenuAccess.Where(p => p.MenuCaption.ToLower().Contains(hdnFilterMenuCaption.Value.ToLower())).ToList();
            if (hdnFilterRead.Value != "")
            {
                bool isReadChecked = (hdnFilterRead.Value == "1");
                lstEntity = lstEntity.Where(p => p.ENABLED == isReadChecked).ToList();
            }
            if (isCountPageCount)
            {
                pageCount = Helper.GetPageCount(lstEntity.Count, Constant.GridViewPageSize.GRID_MATRIX);
            }
            List<GetUserRoleMenuList> lst = lstEntity.Skip((pageIndex - 1) * 10).Take(10).ToList();
            foreach (GetUserRoleMenuList entity in lst)
            {
                CUserRoleMenuAccessState obj = ListStateMenuAccess.FirstOrDefault(p => p.HealthcareID == entity.HealthcareID && p.MenuID == entity.MenuID);
                if (obj != null)
                    entity.CRUDModeUserRole = obj.CRUDMode;
            }

            grdView.DataSource = lst;
            grdView.DataBind();
        }

        protected void cbpMenuAccess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                SetStateMenuAccess();

                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "changemodule")
                {
                    ListUserRoleMenuAccess = BusinessLayer.GetUserRoleMenuList(hdnSelectedModule.Value, hdnSelectedHealthcare.Value, Convert.ToInt32(hdnRoleID.Value), AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
                else
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpMenuAccessProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            SetStateMenuAccess();
            string errMessage = "";
            string result = "";
            if (SaveMenuAccess(ref errMessage))
                result += "success";
            else
                result += "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool SaveMenuAccess(ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                UserRoleMenuDao entityDao = new UserRoleMenuDao(ctx);

                List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("", ctx);
                foreach (Healthcare healthcare in lstHealthcare)
                {
                    StringBuilder listMenuAccessID = new StringBuilder();
                    List<CUserRoleMenuAccessState> ListState = ListStateMenuAccess.Where(p => p.HealthcareID == healthcare.HealthcareID).ToList();
                    if (ListState.Count > 0)
                    {
                        foreach (CUserRoleMenuAccessState row in ListState)
                        {
                            if (listMenuAccessID.ToString() != "")
                                listMenuAccessID.Append(",");
                            listMenuAccessID.Append(row.MenuID);
                        }
                        List<UserRoleMenu> lstUserRoleMenu = BusinessLayer.GetUserRoleMenuList(string.Format("RoleID = {0} AND HealthcareID = {1} AND MenuID IN ({2})", hdnRoleID.Value, healthcare.HealthcareID, listMenuAccessID.ToString()), ctx);

                        Int32 RoleID = Convert.ToInt32(hdnRoleID.Value);
                        foreach (CUserRoleMenuAccessState row in ListState)
                        {
                            UserRoleMenu obj = lstUserRoleMenu.FirstOrDefault(p => p.MenuID == row.MenuID);
                            if (obj != null)
                            {
                                if (obj.CRUDMode != row.CRUDMode)
                                {
                                    obj.CRUDMode = row.CRUDMode;
                                    obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    if (obj.CRUDMode == "-------")
                                    {
                                        obj.IsDeleted = true;
                                    }
                                    else
                                    {
                                        obj.IsDeleted = false;
                                    }
                                    entityDao.Update(obj);
                                    SetUserMenuCRUDMode(obj, ctx);
                                }
                            }
                            else
                            {
                                if (row.CRUDMode != "-------")
                                {
                                    obj = new UserRoleMenu();
                                    obj.RoleID = RoleID;
                                    obj.HealthcareID = healthcare.HealthcareID;
                                    obj.MenuID = row.MenuID;
                                    obj.CRUDMode = row.CRUDMode;
                                    obj.CreatedBy = AppSession.UserLogin.UserID;
                                    if (obj.CRUDMode == "-------")
                                    {
                                        obj.IsDeleted = true;
                                    }
                                    else
                                    {
                                        obj.IsDeleted = false;
                                    }
                                    entityDao.Insert(obj);
                                }
                            }
                        }
                    }
                }
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

        private void SetStateMenuAccess()
        {
            string[] listCRUDMode = hdnCRUDMode.Value.Split('|');
            string[] listMenuID = hdnListMenuID.Value.Split('|');

            for (int i = 0; i < listMenuID.Length; ++i)
            {
                if (listMenuID[i] != "")
                {
                    GetUserRoleMenuList lst = ListUserRoleMenuAccess.FirstOrDefault(p => p.HealthcareID == hdnPrevSelectedHealthcare.Value && p.MenuID == Convert.ToInt32(listMenuID[i]));
                    if (lst.CRUDModeUserRole != listCRUDMode[i])
                    {
                        CUserRoleMenuAccessState obj = ListStateMenuAccess.FirstOrDefault(p => p.HealthcareID == hdnPrevSelectedHealthcare.Value && p.MenuID == Convert.ToInt32(listMenuID[i]));
                        if (obj == null)
                        {
                            obj = new CUserRoleMenuAccessState();
                            obj.HealthcareID = hdnPrevSelectedHealthcare.Value;
                            obj.MenuID = Convert.ToInt32(listMenuID[i]);
                            ListStateMenuAccess.Add(obj);
                        }
                        obj.CRUDMode = listCRUDMode[i];
                    }
                }
            }
        }

        private void SetUserMenuCRUDMode(UserRoleMenu entity, IDbContext ctx)
        {
            UserMenuDao userMenuDao = new UserMenuDao(ctx);
            List<UserMenu> ListUserMenu = BusinessLayer.GetUserMenuList(string.Format("MenuID = {0} AND HealthcareID = '{1}' AND IsDeleted = 0", entity.MenuID, entity.HealthcareID), ctx);
            List<User> lstUser = BusinessLayer.GetUserList(string.Format("UserID IN (SELECT UserID FROM UserInRole WHERE RoleID = {0} AND HealthcareID = '{1}')", entity.RoleID, entity.HealthcareID), ctx);
            char[] CRUDPAVchar = { 'C', 'R', 'U', 'D', 'P', 'A', 'V', 'X' };
            foreach (User user in lstUser)
            {
                UserMenu userMenu = ListUserMenu.FirstOrDefault(p => p.UserID == user.UserID);
                if (userMenu != null)
                {
                    List<UserRoleMenu> lstUserRoleMenu = BusinessLayer.GetUserRoleMenuList(string.Format("MenuID = '{0}' AND RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {1} AND HealthcareID = '{2}') AND RoleID != {3}", entity.MenuID, user.UserID, entity.HealthcareID, entity.RoleID), ctx);
                    lstUserRoleMenu.Add(entity);

                    if (lstUserRoleMenu.Where(p => p.CRUDMode.Contains("R")).Count() < 1)
                        userMenu.CRUDMode = "-------";
                    else
                    {
                        string[] arr = userMenu.CRUDMode.Split('-');
                        for (int i = 0; i < CRUDPAVchar.Length; ++i)
                        {
                            char c = CRUDPAVchar[i];
                            if (c == 'R') continue;
                            if (lstUserRoleMenu.Where(p => p.CRUDMode.Contains(c)).Count() < 1)
                                arr[i] = "";
                        }
                        userMenu.CRUDMode = arr[0] + "-" + arr[1] + "-" + arr[2] + "-" + arr[3] + "-" + arr[4] + "-" + arr[5] + "-" + arr[6] + "-" + arr[7];
                    }
                    userMenuDao.Update(userMenu);
                }
            }
        }

        private const string SESSION_NAME_ROLE_MENU_ACCESS = "UserRoleMenuAccess";
        private const string SESSION_NAME_STATE_MENU_ACCESS = "SelectedUserRoleMenuAccess";
        private static List<CUserRoleMenuAccessState> ListStateMenuAccess
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_NAME_STATE_MENU_ACCESS] == null) HttpContext.Current.Session[SESSION_NAME_STATE_MENU_ACCESS] = new List<CUserRoleMenuAccessState>();
                return (List<CUserRoleMenuAccessState>)HttpContext.Current.Session[SESSION_NAME_STATE_MENU_ACCESS];
            }
            set
            {
                HttpContext.Current.Session[SESSION_NAME_STATE_MENU_ACCESS] = value;
            }
        }
        public static List<GetUserRoleMenuList> ListUserRoleMenuAccess
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_NAME_ROLE_MENU_ACCESS] == null) HttpContext.Current.Session[SESSION_NAME_ROLE_MENU_ACCESS] = new List<GetUserRoleMenuList>();
                return (List<GetUserRoleMenuList>)HttpContext.Current.Session[SESSION_NAME_ROLE_MENU_ACCESS];
            }
            set
            {
                HttpContext.Current.Session[SESSION_NAME_ROLE_MENU_ACCESS] = value;
            }
        }

        private class CUserRoleMenuAccessState
        {
            public string HealthcareID { get; set; }
            public int MenuID { get; set; }
            public string CRUDMode { get; set; }
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
        }
    }
}