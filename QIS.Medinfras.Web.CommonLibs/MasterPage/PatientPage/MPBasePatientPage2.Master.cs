using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MPBasePatientPage2 : BaseMP
    {
        protected string moduleName = "";
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                if (AppSession.UserLogin == null)
                    Response.Redirect("~/../SystemSetup/Login.aspx");
                
                //vAppointment1 entity = BusinessLayer.GetvAppointment1List(string.Format("AppointmentID = {0}", AppSession.RegisteredPatient.VisitID))[0];
                //EntityToControl(entity);
            }
        }

        private void EntityToControl(vAppointment1 entity)
        {
//            ((AppointmentBannerCtl)ctlAppointmentBanner).InitializePatientBanner(entity);
        }

        protected string GetModuleImage()
        {
            return Helper.GetModuleImage(this, Helper.GetModuleName());
        }

        protected string GetHospitalName()
        {
            return AppSession.UserLogin.HealthcareName;
        }

        protected string GetUserInfo()
        {
            return string.Format("{0}", AppSession.UserLogin.UserFullName);
        }

        protected void cbpCloseWindow_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            moduleName = Helper.GetModuleName().ToLower();
            if (moduleName != "systemsetup")
                HttpContext.Current.Session.Clear();
        }
    }
}