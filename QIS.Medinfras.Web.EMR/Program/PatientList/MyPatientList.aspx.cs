using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class MyPatientList : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPageDate = 1;
        protected int PageCountDate = 1;
        protected bool emr = false;

        public class PatientListInfo
        {
            public int HealthcareServiceUnitID { get; set; }
            public string ServiceUnitCode { get; set; }
            public string ServiceUnitName { get; set; }
            public int NoOfPatients { get; set; }
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_LIST;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridViewDate(1, true, ref PageCountDate);
        }

        private void BindGridViewDate(int pageIndexDate, bool isCountPageCount, ref int pageCountDate)
        {
            List<PatientListInfo> lstPatient = new List<PatientListInfo>();
            List<PhysicianPatientListIP> lstEntity = BusinessLayer.GetPhysicianPatientListIP(Convert.ToInt32(AppSession.UserLogin.ParamedicID));
            List<PhysicianPatientListIP> list = lstEntity.GroupBy(lst => lst.HealthcareServiceUnitID).Select(grp => grp.First()).OrderBy(lst => lst.ServiceUnitName).ToList();

            foreach (PhysicianPatientListIP item in list)
            {
                PatientListInfo obj = new PatientListInfo();
                obj.HealthcareServiceUnitID = item.HealthcareServiceUnitID;
                obj.ServiceUnitCode = item.ServiceUnitCode;
                obj.ServiceUnitName = item.ServiceUnitName;
                obj.NoOfPatients = lstEntity.Count(lst => lst.ServiceUnitCode == item.ServiceUnitCode);
                lstPatient.Add(obj);
            }

            grdView.DataSource = lstPatient;
            grdView.DataBind();
        }

        protected void cbpInfoParamedicScheduleDateView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCountDate = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDate(Convert.ToInt32(param[1]), false, ref pageCountDate);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDate(1, true, ref pageCountDate);
                    result = "refresh|" + pageCountDate;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";

            string filterExpression = String.Format("HealthcareServiceUnitID = {0} AND DischargeDate IS NULL AND GCVisitStatus NOT IN ('{1}', '{2}', '{3}') AND DepartmentID = 'INPATIENT' AND ParamedicTeamPhysicianID = {4} ORDER BY BedCode", Convert.ToInt32(hdnID.Value), Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, AppSession.UserLogin.ParamedicID);

            List<vAppointmentRegistration> lstEntityDate = null;
            lstEntityDate = BusinessLayer.GetvAppointmentRegistrationList(filterExpression);
            lvwView.DataSource = lstEntityDate;
            lvwView.DataBind();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}