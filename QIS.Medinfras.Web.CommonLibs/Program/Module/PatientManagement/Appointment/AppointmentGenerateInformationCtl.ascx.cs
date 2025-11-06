using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentGenerateInformationCtl : BaseViewPopupCtl
    {  
        public override void InitializeDataControl(string param)
        {
            AppointmentRequest entity = BusinessLayer.GetAppointmentRequest(Convert.ToInt32(param));
            if (entity != null)
            {
                if (entity.AppointmentID != null)
                {
                    vAppointment app = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", entity.AppointmentID)).FirstOrDefault();
                    txtAppointmentNo.Text = app.AppointmentNo;
                    txtPatientName.Text = string.Format("({0}) {1}", app.MedicalNo, app.PatientName);
                    txtPhoneNo.Text = app.PhoneNo;
                    txtMobilePhoneNo.Text = app.MobilePhoneNo;
                    txtAppointmentDate.Text = string.Format("{0} {1}", app.cfStartDate, app.StartTime);
                    txtParamedicName.Text = app.ParamedicName;
                    txtServiceUnitName.Text = app.ServiceUnitName;
                    txtVisitType.Text = app.VisitTypeName;
                    txtQueueNo.Text = app.cfQueueNo;
                    //txtSession.Text = string.Format("Sesi {0}", app.Session);

                    hdnAppointmentRequestID.Value = entity.AppointmentRequestID.ToString();
                    hdnAppointmentID.Value = entity.AppointmentID.ToString();
                    hdnRegistrationID.Value = entity.RegistrationID.ToString();

                    BusinessPartners entityBP = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = {0}", app.BusinessPartnerID)).FirstOrDefault();
                    txtBusinessPartnerName.Text = entityBP.BusinessPartnerName;
                }
            }
        }

        protected void cbpPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');            
            if (param[0] == "void")
            {
                if (onVoidRecord(ref errMessage))
                {
                    result = "sucess";
                }
                else
                {
                    result += "fail|" + errMessage;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool onVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao appointmentDao = new AppointmentDao(ctx);
            RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
            AppointmentRequestDao appointmentRequestDao = new AppointmentRequestDao(ctx);
            try
            {
                if (!String.IsNullOrEmpty(hdnAppointmentID.Value) && !String.IsNullOrEmpty(hdnAppointmentRequestID.Value))
                {
                    Appointment entity = appointmentDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                    RegistrationBPJS entityRegistration = null;
                    if (!String.IsNullOrEmpty(hdnRegistrationID.Value))
                    {
                        entityRegistration = new RegistrationBPJS();
                        entityRegistration = registrationBPJSDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                    }
                    AppointmentRequest entityRequest = appointmentRequestDao.Get(Convert.ToInt32(hdnAppointmentRequestID.Value));

                    entity.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    appointmentDao.Update(entity);

                    if (entityRegistration != null)
                    {
                        entityRegistration.AppointmentID = null;
                        entityRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                        registrationBPJSDao.Update(entityRegistration);
                    }

                    entityRequest.AppointmentID = null;
                    entityRequest.LastUpdatedBy = AppSession.UserLogin.UserID;
                    appointmentRequestDao.Update(entityRequest);

                    ctx.CommitTransaction();
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