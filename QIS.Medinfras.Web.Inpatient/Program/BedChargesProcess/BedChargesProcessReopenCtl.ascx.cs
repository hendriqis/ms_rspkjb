using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class BedChargesProcessReopenCtl : BaseViewPopupCtl
    {
        protected string filterExpressionSupplier = "";

        public override void InitializeDataControl(string param)
        {
            hdnRegistrationIDCtl.Value = param;
        }

        protected void cbpReopenJobBedCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnReopenJobBed(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnReopenJobBed(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            RegistrationBedChargesDao bedChargesDao = new RegistrationBedChargesDao(ctx);

            try
            {
                int registrationID = Convert.ToInt32(hdnRegistrationIDCtl.Value);

                Registration oReg = entityRegistrationDao.Get(registrationID);
                if (!oReg.IsJobBedReopen)
                {
                    oReg.IsJobBedClosed = false;
                    oReg.JobBedClosedBy = null;
                    oReg.JobBedClosedDate = null;
                    oReg.IsJobBedReopen = true;
                    oReg.JobBedReopenBy = AppSession.UserLogin.UserID;
                    oReg.JobBedReopenDate = DateTime.Now;
                    oReg.JobBedReopenReason = txtReason.Text;
                    oReg.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityRegistrationDao.Update(oReg);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Maaf, job bed untuk registrasi <b>{0}</b> sudah dibuka kembali pada <b>{1}</b>.", oReg.RegistrationNo, Convert.ToDateTime(oReg.JobBedReopenDate).ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT));
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