using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class MatrixWithParameterCtl : BaseViewPopupCtl
    {
        #region UserSelectUserRole
        private void InitializeUserSelectUserRole(string queryString)
        {
            lblHeader.InnerText = GetLabel("UserName");
            lblHeader2.InnerText = GetLabel("Healthcare");

            User ur = BusinessLayer.GetUser(Convert.ToInt32(queryString));
            txtHeader.Text = ur.UserName;

            List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("");
            Methods.SetComboBoxField<Healthcare>(ddlDropDown, lstHealthcare, "HealthcareName", "HealthcareID");

            string filterExpression = "RoleID != 1 AND IsDeleted = 0";
            List<UserRole> lstAllAvailable = BusinessLayer.GetUserRoleList(filterExpression);
            ListAllAvailable = (from p in lstAllAvailable.OrderBy(p => p.RoleName)
                                select new Variable { Code = p.RoleID.ToString(), Value = p.RoleName }).ToList();

            filterExpression = string.Format("UserID = {0}", queryString);
            if (AppSession.UserLogin.UserID > 1)
                filterExpression += " AND RoleID != 1";
            List<vUserInRole> lstSelected = BusinessLayer.GetvUserInRoleList(filterExpression);
            ListSelected = (from p in lstSelected
                            select new CMatrixParameter { IsChecked = false, ID = p.RoleID.ToString(), Name = p.RoleName, DropDownValue = p.HealthcareID }).ToList();
        }

        private bool SaveUserSelectUserRole(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int UserID = Convert.ToInt32(queryString);
                UserInRoleDao entityDao = new UserInRoleDao(ctx);
                foreach (ProceedEntityParameter row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntityParameter.ProceedEntityStatus.Add)
                    {
                        UserInRole entity = new UserInRole();
                        entity.UserID = UserID;
                        entity.RoleID = Convert.ToInt32(row.ID);
                        entity.HealthcareID = row.DropDownValue;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        int roleID = Convert.ToInt32(row.ID);
                        string healthcareID = row.DropDownValue;
                        entityDao.Delete(UserID, healthcareID, roleID);

                        SetUserMenuCRUDMode(UserID, healthcareID, ctx);
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
        #endregion
        #region UserRoleSelectUser
        private void InitializeUserRoleSelectUser(string queryString)
        {
            lblHeader.InnerText = GetLabel("UserRole");
            lblHeader2.InnerText = GetLabel("Healthcare");

            UserRole ur = BusinessLayer.GetUserRole(Convert.ToInt32(queryString));
            txtHeader.Text = ur.RoleName;

            List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("");
            Methods.SetComboBoxField<Healthcare>(ddlDropDown, lstHealthcare, "HealthcareName", "HealthcareID");

            string filterExpression = "IsDeleted = 0";
            if (!AppSession.UserLogin.IsSysAdmin)
                filterExpression = "UserID NOT IN (SELECT UserID FROM UserInRole WHERE RoleID = 1) AND IsDeleted = 0";
            else if (AppSession.UserLogin.UserID > 1)
                filterExpression = "UserID != 1 AND IsDeleted = 0";
            
            List<vUser> lstAllAvailable = BusinessLayer.GetvUserList(filterExpression);
            ListAllAvailable = (from p in lstAllAvailable.OrderBy(p => p.UserName)
                                select new Variable { Code = p.UserID.ToString(), Value = string.Format("{0} ({1})",p.FullName,p.UserName) }).ToList();

            filterExpression = string.Format("RoleID = {0}", queryString);
            if (!AppSession.UserLogin.IsSysAdmin)
                filterExpression += " AND UserID NOT IN (SELECT UserID FROM UserInRole WHERE RoleID = 1)";
            else if(AppSession.UserLogin.UserID > 1)
                filterExpression += " AND UserID != 1";
            List<vUserInRole> lstSelected = BusinessLayer.GetvUserInRoleList(filterExpression);
            ListSelected = (from p in lstSelected
                            select new CMatrixParameter { IsChecked = false, ID = p.UserID.ToString(), Name = string.Format("{0} ({1})", p.FullName, p.UserName), DropDownValue = p.HealthcareID }).ToList();
        }

        private bool SaveUserRoleSelectUser(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int RoleID = Convert.ToInt32(queryString);
                UserInRoleDao entityDao = new UserInRoleDao(ctx);
                foreach (ProceedEntityParameter row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntityParameter.ProceedEntityStatus.Add)
                    {
                        UserInRole entity = new UserInRole();
                        entity.RoleID = RoleID;
                        entity.UserID = Convert.ToInt32(row.ID);
                        entity.HealthcareID = row.DropDownValue;
                        entityDao.Insert(entity);
                    }
                    else
                    {
                        int userID = Convert.ToInt32(row.ID);
                        string healthcareID = row.DropDownValue;
                        entityDao.Delete(userID, healthcareID, RoleID);

                        SetUserMenuCRUDMode(userID, healthcareID, ctx);
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

        private void SetUserMenuCRUDMode(int userID, string healthcareID, IDbContext ctx)
        {
            UserMenuDao userMenuDao = new UserMenuDao(ctx);
            List<UserMenu> ListUserMenu = BusinessLayer.GetUserMenuList(string.Format("UserID = {0} AND HealthcareID = '{1}' AND IsDeleted = 0", userID, healthcareID), ctx);
            List<UserRoleMenu> lstAllUserRoleMenu = BusinessLayer.GetUserRoleMenuList(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}')", userID, healthcareID), ctx);

            char[] CRUDPAVchar = { 'C', 'R', 'U', 'D', 'P', 'A', 'V' };
            foreach (UserMenu userMenu in ListUserMenu)
            {
                List<UserRoleMenu> lstUserRoleMenu = lstAllUserRoleMenu.Where(p => p.MenuID == userMenu.MenuID).ToList();

                if (lstUserRoleMenu.Where(p => p.CRUDMode.Contains("R")).Count() < 1)
                    userMenu.CRUDMode = "------";
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
                    userMenu.CRUDMode = arr[0] + "-" + arr[1] + "-" + arr[2] + "-" + arr[3] + "-" + arr[4] + "-" + arr[5] + "-" + arr[6];
                }
                userMenuDao.Update(userMenu);
            }
        }
        #endregion

        private void InitializeListMatrix(string type, string queryString)
        {
            switch (type)
            {
                case "UserRoleSelectUser": InitializeUserRoleSelectUser(queryString); break;
                case "UserSelectUserRole": InitializeUserSelectUserRole(queryString); break;
            }
        }

        private bool SaveMatrix(string type, string queryString, ref string errMessage)
        {
            switch (type)
            {
                case "UserRoleSelectUser": return SaveUserRoleSelectUser(queryString, ref errMessage);
                case "UserSelectUserRole": return SaveUserSelectUserRole(queryString, ref errMessage);
            }
            return false;
        }



        protected int PageCountAvailable = 1;
        protected int PageCountSelected = 1;
        public override void InitializeDataControl(string param)
        {
            ListProceedEntity.Clear();
            hdnParam.Value = param;

            string[] temp = param.Split('|');

            InitializeListMatrix(temp[0], temp[1]);

            hdnSelectedDropDown.Value = ddlDropDown.SelectedValue;

            BindGridAvailable(1, true, ref PageCountAvailable);
            BindGridSelected(1, true, ref PageCountSelected);
        }

        #region Available
        private void BindGridAvailable(int pageIndex, bool isCountPageCount, ref int pageCount, List<string> listCheckedAvailable = null)
        {
            ListAvailable = (from item1 in ListAllAvailable
                             where !(ListSelected.Any(item2 => item2.ID == item1.Code && item2.DropDownValue == hdnSelectedDropDown.Value))
                             select new CMatrixParameter { IsChecked = false, ID = item1.Code, Name = item1.Value, DropDownValue = hdnSelectedDropDown.Value }).ToList();

            List<CMatrixParameter> lstEntity = ListAvailable.Where(p => p.Name.ToUpper().Contains(hdnAvailableSearchText.Value.ToUpper())).ToList();
            if (isCountPageCount)
            {
                pageCount = Helper.GetPageCount(lstEntity.Count, Constant.GridViewPageSize.GRID_MATRIX);
            }
            List<CMatrixParameter> lst = lstEntity.Skip((pageIndex - 1) * 10).Take(10).ToList();
            foreach (CMatrixParameter mtx in lst)
            {
                if (listCheckedAvailable != null && listCheckedAvailable.Contains(mtx.ID))
                {
                    mtx.IsChecked = true;
                    listCheckedAvailable.Remove(mtx.ID);
                }
                else
                    mtx.IsChecked = false;
            }

            grdAvailable.DataSource = lst;
            grdAvailable.DataBind();
        }

        protected void cbpMatrixAvailable_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            List<string> listCheckedAvailable = hdnCheckedAvailable.Value.Split(';').ToList();
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    string[] newCheckedAvailable = param[2].Split(';');
                    foreach (string a in newCheckedAvailable)
                    {
                        if (a != "")
                            listCheckedAvailable.Add(a);
                    }

                    BindGridAvailable(Convert.ToInt32(param[1]), false, ref pageCount, listCheckedAvailable);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridAvailable(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpCheckedAvailable"] = string.Join(";", listCheckedAvailable.ToArray());
        }
        #endregion

        #region Selected
        private void BindGridSelected(int pageIndex, bool isCountPageCount, ref int pageCount, List<string> listCheckedSelected = null)
        {
            List<CMatrixParameter> lstEntity = ListSelected.Where(p => p.DropDownValue == hdnSelectedDropDown.Value && p.Name.ToUpper().Contains(hdnSelectedSearchText.Value.ToUpper())).ToList();
            if (isCountPageCount)
            {
                pageCount = Helper.GetPageCount(lstEntity.Count, Constant.GridViewPageSize.GRID_MATRIX);
            }
            List<CMatrixParameter> lst = lstEntity.Skip((pageIndex - 1) * 10).Take(10).ToList();
            foreach (CMatrixParameter mtx in lst)
            {
                if (listCheckedSelected != null && listCheckedSelected.Contains(mtx.ID))
                {
                    mtx.IsChecked = true;
                    listCheckedSelected.Remove(mtx.ID);
                }
                else
                    mtx.IsChecked = false;
            }

            grdSelected.DataSource = lst;
            grdSelected.DataBind();
        }

        protected void cbpMatrixSelected_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            List<string> listCheckedSelected = hdnCheckedSelected.Value.Split(';').ToList();
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    string[] newCheckedSelected = param[2].Split(';');
                    foreach (string a in newCheckedSelected)
                    {
                        if (a != "")
                            listCheckedSelected.Add(a);
                    }

                    BindGridSelected(Convert.ToInt32(param[1]), false, ref pageCount, listCheckedSelected);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridSelected(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpCheckedSelected"] = string.Join(";", listCheckedSelected.ToArray());

        }
        #endregion


        protected void cbpMatrixProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            if (param[0] == "rightAll")
            {
                List<CMatrixParameter> lst = ListAvailable.Where(p => p.DropDownValue == hdnSelectedDropDown.Value && p.Name.Contains(hdnAvailableSearchText.Value)).ToList();
                foreach (CMatrixParameter row in lst)
                {
                    ListSelected.Add(row);

                    ProceedEntityParameter obj = ListProceedEntity.FirstOrDefault(p => p.ID == row.ID);
                    if (obj != null)
                        ListProceedEntity.Remove(obj);
                    else
                    {
                        ProceedEntityParameter proceedEntity = new ProceedEntityParameter();
                        proceedEntity.ID = row.ID;
                        proceedEntity.DropDownValue = hdnSelectedDropDown.Value;
                        proceedEntity.Status = ProceedEntityParameter.ProceedEntityStatus.Add;
                        ListProceedEntity.Add(proceedEntity);
                    }
                }
                ListSelected = ListSelected.ToList();
            }
            else if (param[0] == "right")
            {
                List<string> listCheckedAvailable = hdnCheckedAvailable.Value.Split(';').ToList();
                string[] newCheckedAvailable = param[1].Split(';');
                foreach (string a in newCheckedAvailable)
                {
                    if (a != "")
                        listCheckedAvailable.Add(a);
                }

                foreach (string value in listCheckedAvailable)
                {
                    if (value != "")
                    {
                        ProceedEntityParameter obj = ListProceedEntity.FirstOrDefault(p => p.ID == value && p.DropDownValue == hdnSelectedDropDown.Value);
                        if (obj != null)
                            ListProceedEntity.Remove(obj);
                        else
                        {
                            ProceedEntityParameter proceedEntity = new ProceedEntityParameter();
                            proceedEntity.ID = value;
                            proceedEntity.DropDownValue = hdnSelectedDropDown.Value;
                            proceedEntity.Status = ProceedEntityParameter.ProceedEntityStatus.Add;
                            ListProceedEntity.Add(proceedEntity);
                        }

                        CMatrixParameter removeObj = ListAvailable.FirstOrDefault(p => p.ID == value && p.DropDownValue == hdnSelectedDropDown.Value);
                        if (removeObj != null)
                            ListSelected.Add(removeObj);
                    }
                }

                ListSelected = ListSelected.ToList();
            }
            else if (param[0] == "left")
            {
                List<string> listCheckedSelected = hdnCheckedSelected.Value.Split(';').ToList();
                string[] newCheckedSelected = param[1].Split(';');
                foreach (string a in newCheckedSelected)
                {
                    if (a != "")
                        listCheckedSelected.Add(a);
                }

                foreach (string value in listCheckedSelected)
                {
                    if (value != "")
                    {
                        ProceedEntityParameter obj = ListProceedEntity.FirstOrDefault(p => p.ID == value && p.DropDownValue == hdnSelectedDropDown.Value);
                        if (obj != null)
                            ListProceedEntity.Remove(obj);
                        else
                        {
                            ProceedEntityParameter proceedEntity = new ProceedEntityParameter();
                            proceedEntity.ID = value;
                            proceedEntity.DropDownValue = hdnSelectedDropDown.Value;
                            proceedEntity.Status = ProceedEntityParameter.ProceedEntityStatus.Remove;
                            ListProceedEntity.Add(proceedEntity);
                        }

                        CMatrixParameter removeObj = ListSelected.FirstOrDefault(p => p.ID == value);
                        if (removeObj != null)
                            ListSelected.Remove(removeObj);
                    }
                }

                ListAvailable = ListAvailable.ToList();
            }
            else if (param[0] == "leftAll")
            {
                List<CMatrixParameter> lst = ListSelected.Where(p => p.DropDownValue == hdnSelectedDropDown.Value && p.Name.Contains(hdnSelectedSearchText.Value)).ToList();
                foreach (CMatrixParameter row in lst)
                {
                    ProceedEntityParameter obj = ListProceedEntity.FirstOrDefault(p => p.ID == row.ID);
                    if (obj != null)
                        ListProceedEntity.Remove(obj);
                    else
                    {
                        ProceedEntityParameter proceedEntity = new ProceedEntityParameter();
                        proceedEntity.ID = row.ID;
                        proceedEntity.DropDownValue = hdnSelectedDropDown.Value;
                        proceedEntity.Status = ProceedEntityParameter.ProceedEntityStatus.Remove;
                        ListProceedEntity.Add(proceedEntity);
                    }
                }
                ListSelected.RemoveAll(x => x.DropDownValue == hdnSelectedDropDown.Value && x.Name.Contains(hdnSelectedSearchText.Value));
            }
            else if (param[0] == "save")
            {
                string errMessage = "";
                string[] temp = hdnParam.Value.Split('|');
                if (SaveMatrix(temp[0], temp[1], ref errMessage))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public class CMatrixParameter
        {
            public bool IsChecked { get; set; }
            public String ID { get; set; }
            public String Name { get; set; }
            public String DropDownValue { get; set; }
        }

        private const string SESSION_NAME_SELECTED_ENTITY = "MtxWithParameterSelectedEntityUserRole";
        private const string SESSION_NAME_AVAILABLE_ENTITY = "MtxWithParameterAvailableEntityUserRole";
        private const string SESSION_NAME_ALL_AVAILABLE_ENTITY = "MtxWithParameterAllAvailableEntityUserRole";
        private const string SESSION_PROCEED_ENTITY = "MtxWithParameterProceedEntity";

        #region Matrix
        private class ProceedEntityParameter
        {
            private String _ID;
            private String _DropDownValue;
            private ProceedEntityStatus _Status;

            public String ID
            {
                get { return _ID; }
                set { _ID = value; }
            }

            public String DropDownValue
            {
                get { return _DropDownValue; }
                set { _DropDownValue = value; }
            }

            public ProceedEntityStatus Status
            {
                get { return _Status; }
                set { _Status = value; }
            }

            public enum ProceedEntityStatus
            {
                Remove = 0,
                Add = 1
            }
        }

        public static List<CMatrixParameter> ListSelected
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_NAME_SELECTED_ENTITY] == null) HttpContext.Current.Session[SESSION_NAME_SELECTED_ENTITY] = new List<CMatrixParameter>();
                return (List<CMatrixParameter>)HttpContext.Current.Session[SESSION_NAME_SELECTED_ENTITY];
            }
            set
            {
                HttpContext.Current.Session[SESSION_NAME_SELECTED_ENTITY] = value;
            }
        }
        public static List<CMatrixParameter> ListAvailable
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_NAME_AVAILABLE_ENTITY] == null) HttpContext.Current.Session[SESSION_NAME_AVAILABLE_ENTITY] = new List<CMatrixParameter>();
                return (List<CMatrixParameter>)HttpContext.Current.Session[SESSION_NAME_AVAILABLE_ENTITY];
            }
            set
            {
                HttpContext.Current.Session[SESSION_NAME_AVAILABLE_ENTITY] = value;
            }
        }
        public static List<Variable> ListAllAvailable
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_NAME_ALL_AVAILABLE_ENTITY] == null) HttpContext.Current.Session[SESSION_NAME_ALL_AVAILABLE_ENTITY] = new List<Variable>();
                return (List<Variable>)HttpContext.Current.Session[SESSION_NAME_ALL_AVAILABLE_ENTITY];
            }
            set
            {
                HttpContext.Current.Session[SESSION_NAME_ALL_AVAILABLE_ENTITY] = value;
            }
        }

        private static List<ProceedEntityParameter> ListProceedEntity
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_PROCEED_ENTITY] == null) HttpContext.Current.Session[SESSION_PROCEED_ENTITY] = new List<ProceedEntityParameter>();
                return (List<ProceedEntityParameter>)HttpContext.Current.Session[SESSION_PROCEED_ENTITY];
            }
            set
            {
                HttpContext.Current.Session[SESSION_PROCEED_ENTITY] = value;
            }
        }
        #endregion
    }
}