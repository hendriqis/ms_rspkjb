using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using System.Web.Security;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QISEncryptionPasswordUser;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class ChangePasswordCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            txtOldPassword.Attributes.Add("validationgroup", "mpChangePassword");
            txtNewPassword.Attributes.Add("validationgroup", "mpChangePassword");
            txtConfirmPassword.Attributes.Add("validationgroup", "mpChangePassword");
        }

        protected void cbpChangePasswordProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string oldPassword = txtOldPassword.Text;
            User user = BusinessLayer.GetUser(AppSession.UserLogin.UserID);
            if (user.Password.Trim() == FormsAuthentication.HashPasswordForStoringInConfigFile(oldPassword, "sha1"))
            {
                #region CheckPassword
                if (txtConfirmPassword.Text.Length < 12)
                {
                    errMessage = "Password minimal harus 12 karakter dan terdiri dari angka, huruf (besar dan kecil) dan symbol";
                }
                else if (txtConfirmPassword.Text.Length > 128)
                {
                    errMessage = "Password maksimal 128 karakter dan terdiri dari angka, huruf (besar dan kecil) dan symbol";
                }

                if (txtConfirmPassword.Text.Where(char.IsLower).Count() <= 0 || txtConfirmPassword.Text.Where(char.IsUpper).Count() <= 0 || txtConfirmPassword.Text.Where(char.IsDigit).Count() <= 0 || txtConfirmPassword.Text.Where(char.IsPunctuation).Count() <= 0)
                {
                    errMessage = "Password minimal harus 12 karakter dan harus terdiri dari angka, huruf (besar dan kecil) dan symbol";
                }
                #endregion

                if (String.IsNullOrEmpty(errMessage))
                {
                    user.Password = EncryptionPS.EncryptString(FormsAuthentication.HashPasswordForStoringInConfigFile(txtConfirmPassword.Text, "sha1"));
                    user.LastPasswordChangedDate = DateTime.Now;
                    BusinessLayer.UpdateUser(user);

                    UserAttribute oUserA = BusinessLayer.GetUserAttribute(user.UserID);
                    if (oUserA != null)
                    {
                        oUserA.IsResetPassword = false;
                        oUserA.LastUpdatedBy = AppSession.UserLogin.UserID;
                        oUserA.LastUpdatedDate = DateTime.Now;
                        BusinessLayer.UpdateUserAttribute(oUserA);
                    }

                    result = "success";
                }
                else
                {
                    result = string.Format("fail|{0}", errMessage);
                }

            }
            else
                result = "fail|Old Password is invalid, try again";
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}