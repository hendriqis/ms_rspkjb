using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PaymentCloseRegistrationCtl : BaseContentPopupCtl
    {
        public override void InitializeControl(string param)
        {
            hdnRegistrationID.Value = param;
        }

        protected void cbpProcessPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnProcessRecord(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnProcessRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityDao = new RegistrationDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            try
            {
                Registration registration = entityDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                registration.GCRegistrationStatus = Constant.VisitStatus.CLOSED;
                registration.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(registration);

                List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", registration.RegistrationID), ctx);
                foreach (ConsultVisit consultVisit in lstConsultVisit)
                {
                    consultVisit.GCVisitStatus = Constant.VisitStatus.CLOSED;
                    consultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                    consultVisitDao.Update(consultVisit);
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}