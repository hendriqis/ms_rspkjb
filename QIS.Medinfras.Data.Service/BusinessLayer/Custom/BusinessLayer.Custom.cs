using System;
using System.Collections.Generic;
using System.Data;
using QIS.Data.Core.Dal;
using System.Linq;

namespace QIS.Medinfras.Data.Service
{
    public static partial class BusinessLayer
    {
        #region GetLocationUserAccessList
        public static List<GetLocationUserList> GetLocationUserAccessList(string param)
        {
            string[] par = param.Split(';');
            string healthcareID = par[0];
            string userID = par[1];
            string transactionCode = par[2];
            string filterExpression = par[3];
            return GetLocationUserList(healthcareID, Convert.ToInt32(userID), transactionCode, filterExpression);
        }
        #endregion
        #region GetLocationAllUserAccessList
        public static List<GetLocationAllUserList> GetLocationAllUserAccessList(string param)
        {
            string[] par = param.Split(';');
            string healthcareID = par[0];
            string userID = par[1];
            string transactionCode = par[2];
            string filterExpression = par[3];
            return GetLocationAllUserList(healthcareID, Convert.ToInt32(userID), transactionCode, filterExpression);
        }
        #endregion

        #region GetHealthcareServiceUnitUserList
        public static List<GetHealthcareServiceUnitUserList> GetHealthcareServiceUnitUserList(string param)
        {
            string[] par = param.Split(';');
            string healthcareID = par[0];
            string userID = par[1];
            string departmentID = par[2];
            string filterExpression = par[3];
            return GetHealthcareServiceUnitUserList(healthcareID, Convert.ToInt32(userID), departmentID, filterExpression);
        }
        #endregion
        #region GetParamedicVisitTypeAccessList
        public static List<GetParamedicVisitTypeList> GetParamedicVisitTypeAccessList(string param)
        {
            string[] par = param.Split(';');
            string healthcareServiceUnitID = par[0];
            string paramedicID = par[1];
            string filterExpression = par[2];
            return GetParamedicVisitTypeList(Convert.ToInt32(healthcareServiceUnitID), Convert.ToInt32(paramedicID), filterExpression);
        }
        #endregion
        #region GetParamedicWorkTimes
        public static List<Variable> GetParamedicWorkTimeList(String parameter)
        {
            string[] param = parameter.Split(';');
            int healthcareServiceUnitID = Convert.ToInt32(param[0]);
            int paramedicID = Convert.ToInt32(param[1]);
            int selectedDay = Convert.ToInt32(param[2]);
            string date = param[3];
            int appointmentID = Convert.ToInt32(param[4]);

            List<Variable> result = new List<Variable>();
            int serviceInterval = 0;
            List<vParamedicSchedule> lstSchedule = BusinessLayer.GetvParamedicScheduleList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}", healthcareServiceUnitID, paramedicID, selectedDay));

            if (lstSchedule.Count > 0)
            {
                List<TimeOfDayInterval> list = GetWorkTimes(lstSchedule[0]);
                serviceInterval = BusinessLayer.GetHealthcareServiceUnit(healthcareServiceUnitID).ServiceInterval;
                string filterExpression = "";
                if (appointmentID > 0)
                    filterExpression = string.Format("AppointmentID != {0} AND ParamedicID = {1} AND StartDate = '{2}' AND GCAppointmentStatus != '{3}'", appointmentID, paramedicID, date, "0278^004");
                else
                    filterExpression = string.Format("ParamedicID = {0} AND StartDate = '{1}' AND GCAppointmentStatus != '{2}'", paramedicID, date, "0278^004");
                List<String> lstAppointmentParamedicSchedule = BusinessLayer.GetAppointmentStartTimeList(filterExpression);
                foreach (TimeOfDayInterval tod in list)
                {
                    string startTime = tod.Start.ToString(@"hh\:mm");
                    result.Add(new Variable { Code = startTime, Value = startTime });
                    TimeSpan ctrTime = tod.Start;
                    while (ctrTime < tod.End)
                    {
                        ctrTime = ctrTime.Add(new TimeSpan(0, serviceInterval, 0));
                        string temp = ctrTime.ToString(@"hh\:mm");
                        if (!lstAppointmentParamedicSchedule.Contains(temp))
                            result.Add(new Variable { Code = temp, Value = temp });
                    }
                }
            }
            return result;
        }
        private static List<TimeOfDayInterval> GetWorkTimes(vParamedicSchedule schedule)
        {
            TimeOfDayInterval tm;
            List<TimeOfDayInterval> WorkTimes = new List<TimeOfDayInterval>();
            if (schedule.StartTime1 != string.Empty)
            {
                tm = new TimeOfDayInterval(TimeSpan.Parse(schedule.StartTime1), TimeSpan.Parse(schedule.EndTime1));
                WorkTimes.Add(tm);
            }
            if (schedule.StartTime2 != string.Empty)
            {
                tm = new TimeOfDayInterval(TimeSpan.Parse(schedule.StartTime2), TimeSpan.Parse(schedule.EndTime2));
                WorkTimes.Add(tm);
            }
            if (schedule.StartTime3 != string.Empty)
            {
                tm = new TimeOfDayInterval(TimeSpan.Parse(schedule.StartTime3), TimeSpan.Parse(schedule.EndTime3));
                WorkTimes.Add(tm);
            }
            if (schedule.StartTime4 != string.Empty)
            {
                tm = new TimeOfDayInterval(TimeSpan.Parse(schedule.StartTime4), TimeSpan.Parse(schedule.EndTime4));
                WorkTimes.Add(tm);
            }
            if (schedule.StartTime5 != string.Empty)
            {
                tm = new TimeOfDayInterval(TimeSpan.Parse(schedule.StartTime5), TimeSpan.Parse(schedule.EndTime5));
                WorkTimes.Add(tm);
            }
            return WorkTimes;
        }
        #endregion
        #region GetServiceUnitParamedicList
        public static List<GetServiceUnitParamedicList> GetServiceUnitParamedicAccessList(string param)
        {
            string[] par = param.Split(';');
            string healthcareServiceUnitID = par[0];
            string filterExpression = par[1];
            return GetServiceUnitParamedicList(Convert.ToInt32(healthcareServiceUnitID), filterExpression);
        }
        #endregion
        #region GetServiceUnitUserAccessList
        public static List<GetServiceUnitUserList> GetServiceUnitUserAccessList(string param)
        {
            string[] par = param.Split(';');
            string healthcareID = par[0];
            string userID = par[1];
            string departmentID = par[2];
            string filterExpression = par[3];
            return GetServiceUnitUserList(healthcareID, Convert.ToInt32(userID), departmentID, filterExpression);
        }
        #endregion
        #region GetServiceUnitUserAccessList
        public static List<GetSupplierForReorderPO> GetGetSupplierForReorderPOList(string param)
        {
            string[] par = param.Split(';');
            string itemIDAndFilterExpression = par[0];
            string itemID = par[0];
            string filterExpression = "";
            if(itemIDAndFilterExpression.Contains("AND"))
            {
                itemID = itemIDAndFilterExpression.Split(' ')[0];
                filterExpression = itemIDAndFilterExpression.Substring(itemID.Length + 4);
            }
            return GetGetSupplierForReorderPOList(Convert.ToInt32(itemID), filterExpression);
        }
        #endregion
    }
}
