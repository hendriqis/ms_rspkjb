using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Web.Security;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs
{
    public partial class Login : BasePage
    {
        protected string moduleName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdnHealthcareID.Value = BusinessLayer.GetHealthcareList("").FirstOrDefault().HealthcareID;

                moduleName = Helper.GetModuleName();
                hdnModuleID.Value = Helper.GetModuleID(moduleName);
                if (AppSession.UserLogin == null)
                {
                    pnlUserLoginInformation.Style.Add("display", "none");
                    loginContainerLoginInfo.Style.Add("display", "block");
                    pnlOpenWindowPopup.Style.Add("display", "none");
                    txtUserName.Focus();
                }
                else
                {
                    lblUserLoginInfo.InnerHtml = AppSession.UserLogin.UserFullName;
                    pnlUserLoginInformation.Style.Add("display", "block");
                    loginContainerLoginInfo.Style.Add("display", "none");
                    pnlOpenWindowPopup.Style.Remove("display");
                }

                txtUserName.Attributes.Add("validationgroup", "mpLogin");
                txtPassword.Attributes.Add("validationgroup", "mpLogin");
                Helper.AddCssClass(txtUserName, "required");
                Helper.AddCssClass(txtPassword, "required");
            }
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = e.Parameter;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpParam"] = param;
            if (param == "login")
            {
                string result = "";
                string loginData = "";
                string userName = txtUserName.Text;
                string password = txtPassword.Text;
                List<vUser> lstUser = BusinessLayer.GetvUserList(string.Format("UserName = '{0}' AND IsDeleted = 0", userName));
                if (lstUser.Count > 0)
                {
                    vUser user = lstUser[0];
                    if (user.Password.Trim() == FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1"))
                    {
                        loginData = string.Format("{0}|{1}", userName, user.Password);

                        UserLogin userLogin = new UserLogin();
                        userLogin.UserID = user.UserID;
                        userLogin.UserName = user.UserName;
                        userLogin.ParamedicID = user.ParamedicID;
                        if (userLogin.ParamedicID != null && userLogin.ParamedicID > 0)
                        {
                            vParamedicMaster userParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", userLogin.ParamedicID))[0];
                            userLogin.UserFullName = userParamedic.ParamedicName;
                            userLogin.DepartmentID = userParamedic.DepartmentID;
                            userLogin.SpecialtyID = userParamedic.SpecialtyID;
                            userLogin.IsSpecialist = userParamedic.IsSpecialist;
                            userLogin.IsRMO = userParamedic.IsRMO;
                            userLogin.GCParamedicMasterType = userParamedic.GCParamedicMasterType;
                            userLogin.IsPrimaryNurse = userParamedic.IsPrimaryNurse;
                        }
                        else
                            userLogin.UserFullName = user.FullName;

                        AppSession.UserLogin = userLogin;
                        result = string.Format("success|{0}", userLogin.UserFullName);
                    }
                    else
                        result = "fail|UserID And Password Doesn't match";
                }
                else
                    result = "fail|User Doesn't exist";
                panel.JSProperties["cpResult"] = result;
                panel.JSProperties["cpLoginData"] = loginData;
            }
            else // Get Data Login
            {
                User user = BusinessLayer.GetUser(AppSession.UserLogin.UserID);
                string loginData = string.Format("{0}|{1}", user.UserName, user.Password);
                panel.JSProperties["cpLoginData"] = loginData;
                panel.JSProperties["cpLink"] = param.Split('|')[1];
                panel.JSProperties["cpModuleID"] = param.Split('|')[2];
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            AppSession.ClearSession();
            HttpContext.Current.Response.Redirect("~/Libs/Login.aspx", true);
        }
    }
}