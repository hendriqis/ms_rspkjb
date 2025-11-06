using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class MyTodayPatientVisitList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.MY_TODAY_PATIENT_VISIT_LIST;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                grdRegisteredPatient.InitializeControl();
                
            }
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            return string.Format("VisitDate = '{0}' AND ParamedicID = {1} AND RegistrationNo LIKE '%{2}%' AND GCVisitStatus NOT IN ('{3}','{4}','{5}','{6}')", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), AppSession.UserLogin.ParamedicID, txtRegistrationNo.Text, Constant.RegistrationStatus.CANCELLED, Constant.RegistrationStatus.PHYSICIAN_DISCHARGE, Constant.RegistrationStatus.CLOSED, Constant.RegistrationStatus.OPEN);
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", transactionNo))[0];
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            AppSession.RegisteredPatient = pt;

            Response.Redirect("~/Program/PatientPage/PatientDataView.aspx");
        }
    }
}