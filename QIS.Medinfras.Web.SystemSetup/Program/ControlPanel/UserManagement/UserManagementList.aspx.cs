using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QISEncryptionPasswordUser;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class UserManagementList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.USER_ACCOUNTS;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvUserRowIndex(filterExpression, keyValue, "UserName") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
            {
                CurrPage = 1;
            }

            BindGridView(CurrPage, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            if (!AppSession.UserLogin.IsSysAdmin)
                filterExpression += "UserID NOT IN (SELECT UserID FROM UserInRole WHERE RoleID = 1) AND IsDeleted = 0";
            else if (AppSession.UserLogin.UserID != 1)
                filterExpression += "UserID != 1 AND IsDeleted = 0";
            else
                filterExpression += "IsDeleted = 0";
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvUserRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vUser> lstEntity = BusinessLayer.GetvUserList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "UserName");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "UserName", "Full Name", "Email" };
            fieldListValue = new string[] { "UserName", "FullName", "Email" };
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl("~/Program/ControlPanel/UserManagement/UserManagementEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/ControlPanel/UserManagement/UserManagementEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                int deletedID = Convert.ToInt32(hdnID.Value);
                if (AppSession.UserLogin.UserID == deletedID)
                {
                    errMessage = "Cannot Delete Your Own Account";
                    return false;
                }
                else if (deletedID == 1)
                {
                    errMessage = "Cannot Delete SysAdmin. This account is preloaded by system";
                    return false;
                }
                else
                {
                    UserAttribute ua = BusinessLayer.GetUserAttribute(deletedID);
                    ua.IsDeleted = true;
                    ua.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateUserAttribute(ua);
                    return true;
                }
            }
            return false;
        }

        protected override bool OnCustomButtonClick(string type, ref string retval, ref string errMessage)
        {
            bool result = true;
            if (type == "resetpassword")
            {
                if (hdnID.Value != "")
                {
                    String newPassword = BusinessLayer.GetSettingParameter(Constant.SettingParameter.DEFAULT_PASSWORD).ParameterValue;

                    if (newPassword.Length < 12)
                    {
                        result = false;
                        errMessage = "Password minimal harus 12 karakter dan terdiri dari angka, huruf (besar dan kecil) dan symbol";
                    }
                    else if (newPassword.Length > 128)
                    {
                        result = false;
                        errMessage = "Password maksimal 128 karakter dan terdiri dari angka, huruf (besar dan kecil) dan symbol";
                    }

                    if (newPassword.Where(char.IsUpper).Count() <= 0 || newPassword.Where(char.IsDigit).Count() <= 0 || newPassword.Where(char.IsPunctuation).Count() <= 0)
                    {
                        result = false;
                        errMessage = "Password minimal harus 12 karakter dan harus terdiri dari angka, huruf (besar dan kecil) dan symbol";
                    }

                    if (result)
                    {
                        User entity = BusinessLayer.GetUser(Convert.ToInt32(hdnID.Value));
                        entity.Password = EncryptionPS.EncryptString(FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "sha1"));
                        BusinessLayer.UpdateUser(entity);
                        return result;
                    }
                    else
                    {
                        return result;
                    }
                }
                else
                {
                    return result;
                }
            }
            return result;
        }
    }
}