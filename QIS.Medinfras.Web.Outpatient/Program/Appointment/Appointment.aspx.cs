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

namespace QIS.Medinfras.Web.Outpatient.Program
{
    public partial class Appointment : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Outpatient.APPOINTMENT;
        }

        protected int PageCount = 1;
        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowSave = false;
            IsAllowVoid = false;
            IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            InitializeControlProperties();
            BindGridPhysician(1, true, ref PageCount);
        }

        private void InitializeControlProperties()
        {
            int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
            string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}'", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT);
            if (serviceUnitUserCount > 0)
                filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2})", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            txtServiceUnit.Text = cboServiceUnit.Text;
            txtAppointmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnCalAppointmentSelectedDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.TITLE));
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboSalutation, lstStandardCode, "StandardCodeName", "StandardCodeID");
        }

        private void BindGridPhysician(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
            string filterExpression = string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = {0} AND DayNumber = {1}) AND IsDeleted = 0", cboServiceUnit.Value, (int)selectedDate.DayOfWeek);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvParamedicMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vParamedicMaster> lstEntity = BusinessLayer.GetvParamedicMasterList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdPhysician.DataSource = lstEntity;
            grdPhysician.DataBind();
        }

        protected void cbpPhysician_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridPhysician(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridPhysician(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }



        private void SettingScheduleTable(List<AppointmentCustomClass> ListApp, List<vAppointment> lstAppointment, String start, String end, Int32 interval, ref int id)
        {
            DateTime startTime = DateTime.Parse(String.Format("2012-01-28 {0}:00", start));
            DateTime endTime = DateTime.Parse(String.Format("2012-01-28 {0}:00", end));

            int minuteStartTime = startTime.Minute;
            int parentID = 0;
            while (startTime < endTime)
            {
                vAppointment Patient = lstAppointment.FirstOrDefault(p => p.StartTime == startTime.ToString("HH:mm"));
                AppointmentCustomClass entity = new AppointmentCustomClass();
                entity.ID = ++id;

                entity.Time = startTime.ToString("HH:mm");
                if (startTime.Minute == minuteStartTime)
                {
                    parentID = entity.ID;
                    entity.ParentID = -1;
                }
                else
                    entity.ParentID = parentID;
                
                if (Patient != null)
                {
                    //entity.ID = Patient.AppointmentID.ToString();
                    //entity.Patient = PersonNameFormat.Format(Patient.LastName, Patient.FirstName, Patient.MiddleName, Patient.Salutation);
                    //entity.MedicalNo = Patient.cfMedicalNo;
                    //entity.PictureFileName = Patient.PictureFileName;
                    entity.VisitType = Patient.VisitTypeName;
                }
                else
                {
                    entity.Patient = "";
                }
                startTime = startTime.AddMinutes(interval);

                ListApp.Add(entity);
            }
            AppointmentCustomClass temp = new AppointmentCustomClass();
            temp.ID = -1;
            temp.Patient = "";
            ListApp.Add(temp);
        }

        public class AppointmentCustomClass
        {
            Int32 _ID;
            String _time;
            String _patient;
            Int32 _ParentID;
            String _MedicalNo;
            String _VisitType;

            public String VisitType
            {
                get { return _VisitType; }
                set { _VisitType = value; }
            }

            public String MedicalNo
            {
                get { return _MedicalNo; }
                set { _MedicalNo = value; }
            }
            public Int32 ID
            {
                get { return _ID; }
                set { _ID = value; }
            }

            public String Time
            {
                get { return _time; }
                set { _time = value; }
            }

            public String Patient
            {
                get { return _patient; }
                set { _patient = value; }
            }

            public Int32 ParentID
            {
                get { return _ParentID; }
                set { _ParentID = value; }
            }
        }

        protected void cbpAppointment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            Int32 HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
            Int32 ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);

            vParamedicSchedule obj = BusinessLayer.GetvParamedicScheduleList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}", HealthcareServiceUnitID, ParamedicID, (int)selectedDate.DayOfWeek)).FirstOrDefault();
            List<AppointmentCustomClass> ListAppointment = new List<AppointmentCustomClass>();

            if (obj != null)
            {
                List<vAppointment> lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}'", HealthcareServiceUnitID, ParamedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112)));
                //int serviceInterval = BusinessLayer.GetHealthcareServiceUnit(HealthcareServiceUnitID).ServiceInterval;
                int serviceInterval = 15;
                int id = 0;

                if (obj.StartTime1 != "") SettingScheduleTable(ListAppointment, lstAppointment, obj.StartTime1, obj.EndTime1, serviceInterval, ref id);
                if (obj.StartTime2 != "") SettingScheduleTable(ListAppointment, lstAppointment, obj.StartTime2, obj.EndTime2, serviceInterval, ref id);
                if (obj.StartTime3 != "") SettingScheduleTable(ListAppointment, lstAppointment, obj.StartTime3, obj.EndTime3, serviceInterval, ref id);
                if (obj.StartTime4 != "") SettingScheduleTable(ListAppointment, lstAppointment, obj.StartTime4, obj.EndTime4, serviceInterval, ref id);
                if (obj.StartTime5 != "") SettingScheduleTable(ListAppointment, lstAppointment, obj.StartTime5, obj.EndTime5, serviceInterval, ref id);
            }

            grdAppointment.DataSource = ListAppointment;
            grdAppointment.DataBind();
        }
    }
}