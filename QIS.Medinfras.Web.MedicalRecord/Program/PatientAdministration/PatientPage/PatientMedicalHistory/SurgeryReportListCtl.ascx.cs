using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class SurgeryReportListCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnVisitID.Value = param;

            Healthcare oHc = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            if (oHc != null)
            {
                if (oHc.Initial == "RSSEBS" || oHc.Initial == "RSSEB" || oHc.Initial == "RSSEBK" || oHc.Initial == "RSPR" || oHc.Initial == "RSDI")
                {
                    hdnRptCodeSurgery.Value = "PM-00588";
                }
                else if (oHc.Initial == "RSBL")
                {
                    hdnRptCodeSurgery.Value = "PM-00673";
                }
                else if (oHc.Initial == "RSSA")
                {
                    hdnRptCodeSurgery.Value = "PM-00731";
                }
                else if (oHc.Initial == "RSSES")
                {
                    hdnRptCodeSurgery.Value = "PM-005671";
                }
                else if (oHc.Initial == "RSSK")
                {
                    hdnRptCodeSurgery.Value = "PM-90086";
                }
                else
                {
                    hdnRptCodeSurgery.Value = "PM-00567";
                }
            }
            else
            {
                hdnRptCodeSurgery.Value = "PM-00567";
            }
            
            BindGridDetail();
        }

        private void BindGridDetail()
        {
            List<vPatientSurgery> lst = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID = {0} AND IsDeleted = 0  ORDER BY PatientSurgeryID DESC", hdnVisitID.Value));
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }     
    }
}