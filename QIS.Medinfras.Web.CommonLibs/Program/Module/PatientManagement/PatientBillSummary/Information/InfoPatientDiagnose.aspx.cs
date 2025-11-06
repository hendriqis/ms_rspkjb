using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InfoPatientDiagnose : BasePageTrx
    {
        String filterExpression = "";
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_DIAGNOSE_INPATIENT;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_DIAGNOSE_EMERGENCY;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.PATIENT_DIAGNOSE_EMERGENCY;
                default: return Constant.MenuCode.Outpatient.PATIENT_DIAGNOSE_OUTPATIENT;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;            
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();

            BindGridDetail();

            if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                BindGridDetail2();
            else
                divPlanningSummary.Style.Add("display", "none");
        }

        private void BindGridDetail()
        {
            filterExpression = (string.Format("VisitID = {0}", hdnVisitID.Value));
            List<vPatientDiagnosis1> lst = BusinessLayer.GetvPatientDiagnosis1List(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void BindGridDetail2()
        {
            filterExpression = (string.Format("VisitID = {0} AND PlanningSummary IS NOT NULL AND PlanningSummary != ''", hdnVisitID.Value));
            List<vChiefComplaint> lst = BusinessLayer.GetvChiefComplaintList(filterExpression);
            lvwView2.DataSource = lst;
            lvwView2.DataBind();
        }

        protected void cbpView2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail2();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

    }
}