using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class OpenBillingCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            vRegistration8 entity = BusinessLayer.GetvRegistration8List(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            
            txtLockStatusCtl.Text = entity.IsLockDownInText;
            txtLockByCtl.Text = entity.LockDownByName;
            txtLockDateCtl.Text = entity.cfLockDownDateInString;
            txtBillingStatusCtl.Text = entity.IsBillingClosedInText;
            txtBillingClosedByCtl.Text = entity.BillingClosedByName;
            txtBillingClosedDateCtl.Text = entity.cfBillingClosedDateInString;

            hdnRegistrationIDCtl.Value = entity.RegistrationID.ToString();
            hdnRegistrationNoCtl.Value = entity.RegistrationNo;
        }

        protected void cbpOpenBillingCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            if (param[0] == "update")
            {
                if (OnUpdateRecord(ref errMessage))
                    result = "success";
                else
                    result = "fail|" + errMessage;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnUpdateRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao registrationDao = new RegistrationDao(ctx);

            try
            {
                Registration entityReg = registrationDao.Get(Convert.ToInt32(hdnRegistrationIDCtl.Value));
                if (entityReg.IsBillingClosed)
                {
                    entityReg.IsBillingClosed = false;
                    entityReg.BillingClosedBy = null;
                    entityReg.BillingClosedDate = null;
                    entityReg.LastUpdatedBy = AppSession.UserLogin.UserID;
                    registrationDao.Update(entityReg);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, registrasi " + hdnRegistrationNoCtl.Value + " ini proses tagihannya sudah BUKA kembali.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
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
    }
}