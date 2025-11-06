using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Dashboard.Program
{
    public partial class Example3 : BasePageTrx
    {
        private const string STATUS_IMAGE_PATH = "~/libs/Images/Dashboard/";
        List<ParamedicMaster> lstDoctor;
        List<ParamedicMaster> lstNurse;
        List<vHealthcareServiceUnit> lstClinic;
        List<Patient> lstPatient;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Dashboard.Example3;
        }
        protected override void InitializeDataControl()
        {
            string filterExpressionDoctor = string.Format("GCParamedicMasterType = '{0}' AND IsDeleted = 0", Constant.ParamedicType.Physician);
            string filterExpressionNurse = string.Format("GCParamedicMasterType = '{0}' AND IsDeleted = 0", Constant.ParamedicType.Nurse);
            string filterExpressionClinic = string.Format("DepartmentID = '{0}' AND IsDeleted = 0", Constant.Facility.OUTPATIENT);
            lstDoctor = BusinessLayer.GetParamedicMasterList(filterExpressionDoctor);
            lstNurse = BusinessLayer.GetParamedicMasterList(filterExpressionNurse);
            lstClinic = BusinessLayer.GetvHealthcareServiceUnitList(filterExpressionClinic);
            List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));
            Methods.SetComboBoxField<Department>(cboDepartment, lst, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;
            lblDateTime.InnerText = DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT);

            #region patient
            var resultdoctor = lstDoctor.GroupBy(doctor => doctor.ParamedicID).Select(grp => grp.First()).ToList().OrderBy(x => x.ParamedicID);
            var resultnurse = lstNurse.GroupBy(nurse => nurse.ParamedicID).Select(grp => grp.First()).ToList().OrderBy(x => x.ParamedicID);
            var resultclinic = lstClinic.GroupBy(clinic => clinic.ServiceUnitID).Select(grp => grp.First()).ToList().OrderBy(x => x.ServiceUnitID);
            lblParamedicCount.InnerText = string.Format("{0}", resultdoctor.Count());
            lblNurseCount.InnerText = string.Format("{0}", resultnurse.Count());
            lblClinicCount.InnerText = string.Format("{0}", resultclinic.Count());
            lblHeader.InnerText = string.Format("Hello, {0}", AppSession.UserLogin.UserFullName);
            imgDoctor.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "doctor.png");
            imgNurse.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "nurse.png");
            imgClinic.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "clinic.png");
            #endregion

            BindGridView();
            BindGridDetail();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
                result = string.Format("refresh");
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewTime_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter == "refreshHour")
            {
                hdnTimeNow.Value = DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT);
                result = string.Format("refreshHour");
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            List<GetCountVisitPerDepartmentDashboard> lstEntity = BusinessLayer.GetCountVisitPerDepartmentDashboard(DateTime.Now.Year, DateTime.Now.Month, Convert.ToInt32(AppSession.UserLogin.ParamedicID), cboDepartment.Value.ToString());
            if (lstEntity.Count > 0)
            {
                List<ChartGraphV1> lstChart = new List<ChartGraphV1>();
                foreach (GetCountVisitPerDepartmentDashboard row in lstEntity)
                {
                    ChartGraphV1 entity = new ChartGraphV1();
                    entity.ID = row.ServiceUnitName;
                    entity.Value = row.CountVisit.ToString();
                    lstChart.Add(entity);
                }

                JsonChartData.Value = JsonConvert.SerializeObject(lstChart, Formatting.Indented);
            }
        }

        private void BindGridDetail()
        {
            List<vAppointment> lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("ParamedicID = {0} AND StartDate = '{1}' AND GCAppointmentStatus NOT IN ('{2}','{3}')", AppSession.UserLogin.ParamedicID, DateTime.Now.ToString(), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED));
            lvwView.DataSource = lstAppointment;
            lvwView.DataBind();
        }

        protected void cbpViewAppointment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        protected void cboDepartment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));
            Methods.SetComboBoxField<Department>(cboDepartment, lst, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = -1;
        }

    }
}