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
using Newtonsoft.Json;
using System.IO;
using System.Drawing;
using System.Text;
using Office = Microsoft.Office.Core;
using ClosedXML.Excel;
 
namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class DownloadPengisianHasilList : BasePageTrx
    {
        protected string defaultParamedicName = string.Empty;
        protected string defaultParamedicID = string.Empty;
        List<PatientChargesDt> lstEntityDt = new List<PatientChargesDt>();
        protected int counter = 0;

        protected int PageCount = 1;
        protected int CurrPage = 1;
        private const string DEFAULT_GRDVIEW_FILTER = "GCItemType = '{0}' AND IsDeleted = 0";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalCheckup.DOWNLOAD_PENGISIAN_HASIL_MCU;
        }

        protected override void InitializeDataControl()
        {
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(String.Format(
                                            "ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
                                            Constant.StandardCode.CASHIER_GROUP, //1
                                            Constant.StandardCode.SHIFT //2
                                        ));
            hdnGCCashierGroup.Value = lstSc.Where(t => t.ParentID == Constant.StandardCode.CASHIER_GROUP).ToList().FirstOrDefault().StandardCodeID;
            hdnGCShift.Value = lstSc.Where(t => t.ParentID == Constant.StandardCode.SHIFT).ToList().FirstOrDefault().StandardCodeID;

            setControlProperty();
             
        }

        protected string GetCustomerFilterExpression()
        {
            return string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM CustomerContract WHERE '{0}' BETWEEN StartDate AND EndDate AND IsDeleted = 0)", DateTime.Now.ToString("yyyyMMdd"));
        }

        private void setControlProperty()
        {
            //txtDateRegistration.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            List<vSettingParameterDt> lstSettingParameter = BusinessLayer.GetvSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}') AND HealthcareID = '{5}'",
                                                                                                   Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI,
                                                                                                   Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM,
                                                                                                   Constant.SettingParameter.MC_KODE_DEFAULT_DOKTER,
                                                                                                   Constant.SettingParameter.FN_ITEM_ID_FOR_MCU_PACKAGE_COST,
                                                                                                   Constant.SettingParameter.MC_IS_USING_REGISTRATION_PARAMEDICID,
                                                                                                   AppSession.UserLogin.HealthcareID));

            hdnDefaultItemIDMCUPackage.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_ITEM_ID_FOR_MCU_PACKAGE_COST).ParameterValue;
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnIsUsingRegistrationParamedicID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MC_IS_USING_REGISTRATION_PARAMEDICID).ParameterValue;

            ///hdnRegistrationParamedicID.Value = entity.ParamedicID.ToString();
            defaultParamedicID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MC_KODE_DEFAULT_DOKTER).ParameterValue;
            hdnDefaultParamedicID.Value = defaultParamedicID;
            defaultParamedicName = BusinessLayer.GetParamedicMaster(Convert.ToInt32(defaultParamedicID)).FullName;
            hdnDefaultParamedicName.Value = defaultParamedicName;

            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0",
                                                      Constant.StandardCode.REFERRAL,
                                                      Constant.StandardCode.CUSTOMER_TYPE,
                                                      Constant.StandardCode.ADMISSION_SOURCE,
                                                      Constant.StandardCode.VISIT_REASON,
                                                      Constant.StandardCode.ADMISSION_CONDITION,
                                                      Constant.StandardCode.PATIENT_OWNER_STATUS
                                                  );
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboRegistrationPayer, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboRegistrationPayer.SelectedIndex = 0;
            cboRegistrationPayer.Value = Constant.CustomerType.INSURANCE;

            //MCUOrderExternalGroupList.Clear();
            //MCUOrderExternalGroupListLast.Clear();
        }

        //public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        //{
        //    fieldListValue = new string[] { "ItemCode", "ItemName1" };
        //    fieldListText = new string[] { "Item Code", "Item Name 1" };
        //}

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemGroupMaster.MEDICAL_CHECKUP);
            return filterExpression;
        }

      
        private Boolean ProcessDataTemp(ref string result, ref string errMessage)
        {
            bool isSuccess = false;
            StringBuilder sb = new StringBuilder();
          
            string listRegistrationID = hdnSelectRegistrationID.Value;
            string listRegistrationIDFinish = string.Empty; 
            if (!string.IsNullOrEmpty(listRegistrationID))
             {

                IDbContext ctx = DbFactory.Configure(true);
                List<GetRegistrationListFromMCULink> lstRegLinkMCU = BusinessLayer.GetRegistrationListFromMCULinkList(listRegistrationID);
                List<vParamedicMaster> lstParamedicNotAvaible = new List<vParamedicMaster>();
                if (lstRegLinkMCU.Count > 0) 
                { 
                        //validasi jadwal dokter masing masing
                        foreach(GetRegistrationListFromMCULink row in lstRegLinkMCU)
                        {
                            vParamedicSchedule obj = null;
                            vParamedicScheduleDate objSchDate = null;
                            if ((row.ParamedicID > 0) && ((row.HealthcareServiceUnitID > 0)))
                            {
                                int dayNumber = (int)DateTime.Now.DayOfWeek;
                                if (dayNumber == 0)
                                {
                                    dayNumber = 7;
                                }
                                obj = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                                                    "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                                                    row.HealthcareServiceUnitID, row.ParamedicID, dayNumber), ctx).FirstOrDefault();

                                objSchDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                                                        "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                                                        row.HealthcareServiceUnitID, row.ParamedicID, DateTime.Now), ctx).FirstOrDefault();
                            }
                            Appointment appointment = new Appointment();
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            ValidateParamedicScSchedule(obj, objSchDate, row.RegistrationDate, row.ParamedicID);
                            if (obj == null && objSchDate == null)
                            {
                                vParamedicMaster pm = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", row.ParamedicID)).FirstOrDefault();
                                if (pm != null)
                                {
                                    sb.Append(string.Format("{0}\n", pm.ParamedicName));
                                    lstParamedicNotAvaible.Add(pm);
                                }
                            }
                            else {
                                listRegistrationIDFinish += string.Format("{0},", row.RegistrationID);
                            }

                        }


                        if (!string.IsNullOrEmpty(sb.ToString()))
                        {
                            List<vParamedicMaster> dataParamedic = lstParamedicNotAvaible
                             .GroupBy(p => new { p.ParamedicID })
                             .Select(g => g.First())
                             .ToList();
                            string paramedicName = string.Empty;
                            if (dataParamedic.Count > 0) { 
                                foreach(vParamedicMaster row in dataParamedic){
                                    paramedicName += string.Format("{0},", row.ParamedicName);
                                }
                            }
                            errMessage = string.Format("Maaf untuk jadwal dokter dibawah ini pada tanggal {0} tidak tersedia <b>{1}</b> mohon dibuatkan jadwal terlebih dahulu", txtDateRegistration1.Text, paramedicName);
                            isSuccess = false;
                        }
                        else { 
                            //processs
                            isSuccess = true;
                            listRegistrationIDFinish = listRegistrationIDFinish.Remove(listRegistrationIDFinish.Length - 1);
                            UpdateRegistrationDBLinkMCULink oData = BusinessLayer.UpdateRegistrationDBLinkMCULinkData(listRegistrationIDFinish).FirstOrDefault();
                        }
                    }
             }
            return isSuccess;
        }
    
        private void ValidateParamedicScSchedule(vParamedicSchedule obj, vParamedicScheduleDate objSchDate, DateTime selectedDate, Int32 ParamedicID)
        {
           
            List<GetParamedicLeaveScheduleCompare> objLeave = BusinessLayer.GetParamedicLeaveScheduleCompareList(selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112), ParamedicID);
            #region validate time slot
            #region if leave in period
            if (objLeave.FirstOrDefault().DayNumber != 0 && objLeave.Count() > 1)
            {
                #region set time slot Paramedic Schedule
                if (obj != null)
                {
                    if (obj.DayNumber == objLeave.FirstOrDefault().DayNumber && objLeave.FirstOrDefault().Date == selectedDate)
                    {
                        DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);

                        DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                        DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                        DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                        DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                        DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

                        DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                        DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                        DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                        DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                        DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

                        if (obj.StartTime5 != "")
                        {

                            if (startTimeDefault.TimeOfDay >= objStart5.TimeOfDay)
                            {
                                obj.EndTime5 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                            {
                                obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay <= objStart5.TimeOfDay)
                            {
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
                            {
                                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                        }
                        else if (obj.StartTime4 != "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                            {
                                obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
                            {
                                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                        }
                        else if (obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
                            {
                                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                        }
                        else if (obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                                obj.EndTime2 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                        }
                        else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                            }
                        }
                    }
                    else if (obj.DayNumber == objLeave.LastOrDefault().DayNumber && objLeave.LastOrDefault().Date == selectedDate)
                    {
                        DateTime endTime = DateTime.Parse(String.Format("2012-01-28 {0}:15", objLeave.LastOrDefault().EndTime));
                        endTime = endTime.AddMinutes(15);

                        DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.LastOrDefault().EndTime);
                        DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                        DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                        DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                        DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                        DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

                        DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                        DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                        DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                        DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                        DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

                        if (obj.StartTime5 != "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart5.TimeOfDay)
                            {
                                obj.StartTime5 = endTime.ToString("HH:mm");
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = endTime.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = endTime.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (obj.StartTime4 != "" && obj.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = endTime.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = endTime.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = endTime.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                        }
                        else if (obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (objStart1.TimeOfDay <= endTimeDefault.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                            else if (endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                        }
                    }
                    else
                    {
                        List<GetParamedicLeaveScheduleCompare> c = objLeave.Where(t => t.Date == selectedDate).ToList();
                        if (c.Count > 0)
                        {
                            obj.StartTime1 = "";
                            obj.StartTime2 = "";
                            obj.StartTime3 = "";
                            obj.StartTime4 = "";
                            obj.StartTime5 = "";

                            obj.EndTime1 = "";
                            obj.EndTime2 = "";
                            obj.EndTime3 = "";
                            obj.EndTime4 = "";
                            obj.EndTime5 = "";
                        }
                    }
                }
                #endregion

                #region set time slot Paramedic Schedule Date
                if (objSchDate != null)
                {
                    DateTime objSchStart1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime1);
                    DateTime objSchStart2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime2);
                    DateTime objSchStart3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime3);
                    DateTime objSchStart4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime4);
                    DateTime objSchStart5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime5);

                    DateTime objSchEnd1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime1);
                    DateTime objSchEnd2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime2);
                    DateTime objSchEnd3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime3);
                    DateTime objSchEnd4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime4);
                    DateTime objSchEnd5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime5);

                    if (objSchDate.ScheduleDate == objLeave.FirstOrDefault().Date)
                    {
                        DateTime startTimeDefault = DateTime.Parse(String.Format("{0} {1}:00", objSchDate.cfScheduleDateInString, objLeave.FirstOrDefault().StartTime));
                        if (objSchDate.StartTime5 != "")
                        {

                            if (startTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay)
                            {
                                objSchDate.EndTime5 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                            {
                                objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart5.TimeOfDay)
                            {
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
                            {
                                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                        }
                        else if (objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                            {
                                objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
                            {
                                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                        }
                        else if (objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
                            {
                                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                        }
                        else if (objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = "";
                            }
                        }
                    }
                    else if (objSchDate.ScheduleDate == objLeave.LastOrDefault().Date)
                    {
                        DateTime endTime = DateTime.Parse(String.Format("{0} {1}:00", objSchDate.cfScheduleDateInString, objLeave.LastOrDefault().EndTime));
                        endTime = endTime.AddMinutes(15);

                        DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.LastOrDefault().EndTime);

                        if (objSchDate.StartTime5 != "")
                        {

                            if (endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay)
                            {
                                objSchDate.StartTime5 = endTime.ToString("HH:mm");
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = endTime.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = endTime.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = endTime.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = endTime.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = endTime.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (objSchStart1.TimeOfDay <= endTimeDefault.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                        }
                    }
                    else
                    {
                        List<GetParamedicLeaveScheduleCompare> c = objLeave.Where(t => t.Date == selectedDate).ToList();
                        if (c.Count > 0)
                        {
                            objSchDate.StartTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.StartTime5 = "";

                            objSchDate.EndTime1 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.EndTime5 = "";
                        }
                    }
                }
                #endregion
            }
            #endregion
            #region if leave only in one day
            else if (objLeave.FirstOrDefault().DayNumber != 0 && objLeave.Count() == 1)
            {
                #region set time slot Paramedic Schedule
                if (obj != null)
                {
                    DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);
                    DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().EndTime);

                    DateTime startTime = startTimeDefault.AddMinutes(15);
                    DateTime endTime = endTimeDefault.AddMinutes(15);

                    DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                    DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                    DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                    DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                    DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

                    DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                    DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                    DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                    DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                    DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

                    if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay) //1/2
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            obj.StartTime2 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList2 = obj.IsAllowWaitingList1;
                            obj.MaximumWaitingList2 = obj.MaximumWaitingList1;

                            obj.IsAppointmentByTimeSlot2 = obj.IsAppointmentByTimeSlot1;
                            obj.MaximumAppointment2 = obj.MaximumAppointment1;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //2 modif
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //2 modif
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //2 modif
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            obj.StartTime2 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList3 = obj.IsAllowWaitingList2;
                            obj.MaximumWaitingList3 = obj.MaximumWaitingList2;

                            obj.IsAppointmentByTimeSlot3 = obj.IsAppointmentByTimeSlot2;
                            obj.MaximumAppointment3 = obj.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //9
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList3 = obj.IsAllowWaitingList2;
                            obj.MaximumWaitingList3 = obj.MaximumWaitingList2;

                            obj.IsAppointmentByTimeSlot3 = obj.IsAppointmentByTimeSlot2;
                            obj.MaximumAppointment3 = obj.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = obj.StartTime3;
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
                            obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

                            obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
                            obj.MaximumAppointment4 = obj.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
                        {
                            obj.StartTime1 = obj.StartTime3;
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
                            obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

                            obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
                            obj.MaximumAppointment4 = obj.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //21
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //22
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay) //24
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay) //26
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //28
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //30
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
                        {
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
                            obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

                            obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
                            obj.MaximumAppointment4 = obj.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //33
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //34
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = obj.StartTime3;
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd3.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
                        {
                            obj.StartTime1 = obj.StartTime3;
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //21
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //22
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //24
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay) //26
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay) //28
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //30
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
                        {
                            obj.StartTime5 = obj.StartTime4;
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList5 = obj.IsAllowWaitingList4;
                            obj.MaximumWaitingList5 = obj.MaximumWaitingList4;

                            obj.IsAppointmentByTimeSlot5 = obj.IsAppointmentByTimeSlot4;
                            obj.MaximumAppointment5 = obj.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay) //33
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //34
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objStart4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList5 = obj.IsAllowWaitingList4;
                            obj.MaximumWaitingList5 = obj.MaximumWaitingList4;

                            obj.IsAppointmentByTimeSlot5 = obj.IsAppointmentByTimeSlot4;
                            obj.MaximumAppointment5 = obj.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 != "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = obj.StartTime3;
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime5;
                            obj.EndTime1 = obj.EndTime5;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd3.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
                        {
                            obj.StartTime1 = obj.StartTime3;
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //21
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //24
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //26
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //28
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //30
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
                        {
                            obj.StartTime5 = obj.StartTime4;
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //33
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //34
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objStart4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay == objStart5.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime5;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime5;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime5;
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime5;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime5;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime5;
                            obj.EndTime3 = obj.EndTime5;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = endTime.ToString("HH:mm");
                            obj.EndTime5 = obj.EndTime5;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart5.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = startTime.ToString("HH:mm");
                            obj.EndTime5 = endTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                    }
                }
                #endregion

                #region set time slot Paramedic Schedule Date
                if (objSchDate != null)
                {
                    DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);
                    DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().EndTime);

                    DateTime startTime = startTimeDefault.AddMinutes(15);
                    DateTime endTime = endTimeDefault.AddMinutes(15);

                    DateTime objSchStart1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime1);
                    DateTime objSchStart2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime2);
                    DateTime objSchStart3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime3);
                    DateTime objSchStart4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime4);
                    DateTime objSchStart5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime5);

                    DateTime objSchEnd1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime1);
                    DateTime objSchEnd2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime2);
                    DateTime objSchEnd3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime3);
                    DateTime objSchEnd4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime4);
                    DateTime objSchEnd5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime5);

                    if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay) //1/2
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList2 = objSchDate.IsAllowWaitingList1;
                            objSchDate.MaximumWaitingList2 = objSchDate.MaximumWaitingList1;

                            objSchDate.IsAppointmentByTimeSlot2 = objSchDate.IsAppointmentByTimeSlot1;
                            objSchDate.MaximumAppointment2 = objSchDate.MaximumAppointment1;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //2 modif
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //2 modif
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //2 modif
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList3 = objSchDate.IsAllowWaitingList2;
                            objSchDate.MaximumWaitingList3 = objSchDate.MaximumWaitingList2;

                            objSchDate.IsAppointmentByTimeSlot3 = objSchDate.IsAppointmentByTimeSlot2;
                            objSchDate.MaximumAppointment3 = objSchDate.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //9
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList3 = objSchDate.IsAllowWaitingList2;
                            objSchDate.MaximumWaitingList3 = objSchDate.MaximumWaitingList2;

                            objSchDate.IsAppointmentByTimeSlot3 = objSchDate.IsAppointmentByTimeSlot2;
                            objSchDate.MaximumAppointment3 = objSchDate.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = objSchDate.StartTime3;
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
                            objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

                            objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
                            objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime3;
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
                            objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

                            objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
                            objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //21
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //22
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //24
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //28
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //30
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
                        {
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
                            objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

                            objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
                            objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //33
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //34
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = objSchDate.StartTime3;
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime4;
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime4;
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime3;
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //21
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //22
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //24
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay) //28
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //30
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
                        {
                            objSchDate.StartTime5 = objSchDate.StartTime4;
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList5 = objSchDate.IsAllowWaitingList4;
                            objSchDate.MaximumWaitingList5 = objSchDate.MaximumWaitingList4;

                            objSchDate.IsAppointmentByTimeSlot5 = objSchDate.IsAppointmentByTimeSlot4;
                            objSchDate.MaximumAppointment5 = objSchDate.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay) //33
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //34
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList5 = objSchDate.IsAllowWaitingList4;
                            objSchDate.MaximumWaitingList5 = objSchDate.MaximumWaitingList4;

                            objSchDate.IsAppointmentByTimeSlot5 = objSchDate.IsAppointmentByTimeSlot4;
                            objSchDate.MaximumAppointment5 = objSchDate.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 != "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = objSchDate.StartTime3;
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime5;
                            objSchDate.EndTime1 = objSchDate.EndTime5;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime4;
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime3;
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //21
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //24
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //28
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //30
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
                        {
                            objSchDate.StartTime5 = objSchDate.StartTime4;
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //33
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //34
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime5;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime5;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime5;
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime5;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime5;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime5;
                            objSchDate.EndTime3 = objSchDate.EndTime5;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");
                            objSchDate.EndTime5 = objSchDate.EndTime5;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = startTime.ToString("HH:mm");
                            objSchDate.EndTime5 = endTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                    }
                }
                #endregion
            }
            #endregion
            #endregion
        }

        private void ControlToEntityRegistration(Registration entity, RegistrationPayer entityRegistrationPayer, ConsultVisit entityVisit, PatientDiagnosis entityPatientDiagnosis, MCULink_GetRegistrationOnsite dataOnsite)
        {
            #region PatientDiagnosis
           //// ControlToEntityPatientDiagnosis(entityPatientDiagnosis);
            #endregion
            DateTime registrationDate = dataOnsite.RegistrationDate;
            String registrationTime = dataOnsite.RegistrationTime;

            entityVisit.VisitDate = entityVisit.ActualVisitDate = entity.RegistrationDate = entityPatientDiagnosis.DifferentialDate = registrationDate;
            entityVisit.VisitTime = entityVisit.ActualVisitTime = entity.RegistrationTime = entityPatientDiagnosis.DifferentialTime = registrationTime;

            entity.IsBackDate = (entity.RegistrationDate.Date != DateTime.Now.Date);
            entity.GCReferrerGroup = dataOnsite.GCReferrerGroup;

            entity.ReferralNo = dataOnsite.ReferralNo;
            entity.IsPregnant = dataOnsite.IsPregnant;
            entity.IsParturition = dataOnsite.IsParturition;
            entity.IsNeedPastoralCare = dataOnsite.IsNeedPastoralCare;
            //entity.IsFastTrack = dataOnsite.IsFastTrack; 
           /// entity.RegistrationTicketNo = txtNoTicket.Text;

            if (dataOnsite.MRN > 0 )
            {
                entity.MRN = dataOnsite.MRN;
            }
            else
            {
                entity.MRN = null;
            }
            entity.IsNewPatient = dataOnsite.IsNewPatient;

            string AgePatient = Helper.CalculateAge(dataOnsite.DateOfBirth);
            if(!string.IsNullOrEmpty(AgePatient)){
                string[] ageData = AgePatient.Split('|'); 
                if(ageData.Length > 0){
                    entity.AgeInYear = Convert.ToInt16(ageData[0]); 
                    entity.AgeInMonth = Convert.ToInt16(ageData[1]);
                    entity.AgeInDay = Convert.ToInt16(ageData[2]);
                }
               
            }
            entityVisit.ParamedicID = dataOnsite.ParamedicID;
            entityVisit.HealthcareServiceUnitID = dataOnsite.HealthcareServiceUnitID;
            entityVisit.SpecialtyID = dataOnsite.SpecialtyID;

            entityVisit.VisitTypeID = dataOnsite.VisitTypeID;

            if (dataOnsite.ReferrerID > 0)
            {
                entity.ReferrerID = dataOnsite.ReferrerID;
                entity.ReferrerParamedicID = null;
            }
            else if (dataOnsite.ReferrerParamedicID > 0)
            {
                entity.ReferrerID = null;
                entity.ReferrerParamedicID = dataOnsite.ReferrerParamedicID;
            }
            else
            {
                entity.ReferrerID = null;
                entity.ReferrerParamedicID = null;
            }
            if(dataOnsite.ReferrerDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL){
                entity.ReferrerDate = dataOnsite.ReferrerDate;
                entity.ReferrerTime = dataOnsite.ReferrerTime;
            } 

            entity.GCAdmissionSource = dataOnsite.GCAdmissionSource;
            entityVisit.ClassID = dataOnsite.ClassID;
            if (dataOnsite.RoomID > 0)
            {
                entityVisit.RoomID = dataOnsite.RoomID ;
            }
            else
            {
                entityVisit.RoomID = null;
            }
            entityVisit.BedID = null;
            entity.LinkedRegistrationID = null;

            entity.IsNewBorn = false;
            entity.IsParturition = false;
            entity.IsNeedPastoralCare = false;
            entity.IsVisitorRestriction = false;
            entity.IsFastTrack =false;
            entityVisit.ChargeClassID = dataOnsite.ChargeClassID;
            entityVisit.GCVisitReason = dataOnsite.GCVisitReason;
            entityVisit.VisitReason = dataOnsite.VisitReason;
            entityVisit.GCAdmissionCondition = dataOnsite.GCAdmissionCondition;
            entityVisit.IsMainVisit = dataOnsite.IsMainVisit;

                if (dataOnsite.LinkedToRegistrationID > 0)
                {
                    entity.LinkedToRegistrationID = dataOnsite.LinkedToRegistrationID;
                }

            entity.IsPrintingPatientCard = dataOnsite.IsPrintingPatientCard; 
            entity.GCAdmissionType = dataOnsite.GCAdmissionType; 

            entity.GCCustomerType = dataOnsite.GCCustomerType;
            entityRegistrationPayer.GCCustomerType = dataOnsite.GCCustomerType;
            //Personal => Business Partner ID 1
            entity.IsUsingCOB = dataOnsite.IsUsingCOB;
            if (entity.GCCustomerType == Constant.CustomerType.PERSONAL)
            {
                entity.BusinessPartnerID = 1;
                entity.ContractID = null;
                entity.CoverageTypeID = null;
                entity.CoverageLimitAmount = 0;
                entity.IsCoverageLimitPerDay = false;
                entity.GCTariffScheme = dataOnsite.GCTariffScheme;
                entity.IsControlClassCare = false;
                entity.ControlClassID = null;
                entity.EmployeeID = null;
            }
            else
            {
                entity.BusinessPartnerID = dataOnsite.BusinessPartnerID;
                entity.ContractID = dataOnsite.ContractID;
                if (dataOnsite.CoverageTypeID > 0)
                {
                    entity.CoverageTypeID = dataOnsite.CoverageTypeID;
                }
                else {
                    entity.CoverageTypeID = null;
                }
              
                if (!string.IsNullOrEmpty(dataOnsite.CorporateAccountNoFromRegistration))
                {
                    entity.CorporateAccountNo = dataOnsite.CorporateAccountNoFromRegistration;
                }
                else
                {
                    entity.CorporateAccountNo = dataOnsite.CorporateAccountNoFromGuest;
                }
                entity.CorporateAccountName = dataOnsite.FullName;
                entity.CoverageLimitAmount = dataOnsite.CoverageLimitAmount;
                entity.IsCoverageLimitPerDay = dataOnsite.IsCoverageLimitPerDay;
                entity.GCTariffScheme = dataOnsite.GCTariffScheme;
                entity.IsControlClassCare = dataOnsite.IsControlClassCare;
                if (dataOnsite.IsControlClassCare)
                    entity.ControlClassID = dataOnsite.ControlClassID;
                else
                    entity.ControlClassID = null;

                if (dataOnsite.EmployeeID == 0)
                    entity.EmployeeID = null;
                else
                    entity.EmployeeID = dataOnsite.EmployeeID;

                entityRegistrationPayer.BusinessPartnerID = dataOnsite.BusinessPartnerID;
                entityRegistrationPayer.ContractID = dataOnsite.ContractID;
                if (dataOnsite.CoverageTypeID > 0)
                {
                    entityRegistrationPayer.CoverageTypeID = dataOnsite.CoverageTypeID;
                }
                 
                
                if (!string.IsNullOrEmpty(dataOnsite.CorporateAccountNoFromRegistrationPayer ))
                {
                    entityRegistrationPayer.CorporateAccountNo = dataOnsite.CorporateAccountNoFromRegistrationPayer ; 
                }
                else
                {
                    entityRegistrationPayer.CorporateAccountNo = dataOnsite.CorporateAccountNoFromGuest;  
                }
                entityRegistrationPayer.CorporateAccountName = dataOnsite.FullName;  
                entityRegistrationPayer.CoverageLimitAmount = dataOnsite.CoverageLimitAmount; 
                entityRegistrationPayer.IsCoverageLimitPerDay = dataOnsite.IsCoverageLimitPerDay;
                if (dataOnsite.IsControlClassCare)
                    entityRegistrationPayer.ControlClassID = dataOnsite.ControlClassID;
                else
                    entityRegistrationPayer.ControlClassID = null;
                entityRegistrationPayer.IsPrimaryPayer = true;

            }
        }
       
      

        protected void btnExport_Click(object sender, EventArgs e)
        {
           
            #region Data
            try
            {//QisDataService.vMCUResultFormReport
                string FilterExpression = string.Format("BusinessPartnerID = '{0}' and RegistrationDate BETWEEN  '{1}' and '{2}' and IsDeleted =0 AND GCRegistrationStatus != '{3}' ", hdnPayerID.Value, Helper.GetDatePickerValue(txtDateRegistration1).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtDateRegistration2), Constant.VisitStatus.CANCELLED);

                List<vMCUResultFormReport> lst = BusinessLayer.GetvMCUResultFormReportList(FilterExpression);

                var Builder = new StringBuilder();
                string InitialHealthcare = "";
                Healthcare oHealthcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);

                if (oHealthcare != null) {
                    InitialHealthcare = oHealthcare.Initial; 
                
                }

                #region Format Laporan HASIL MCU
               ///// InitialHealthcare = "dev";
                if (InitialHealthcare == "granostic")
                {

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        ///wb.Worksheets.Add(dt, "Customers");
                        var ws = wb.Worksheets.Add("Kesimpulan");
                        ws.Cell(ToExcelCoordinates("1,1")).Value = "NAMA";
                        ws.Cell(ToExcelCoordinates("2,1")).Value = "LABORATORIUM";
                        ws.Cell(ToExcelCoordinates("3,1")).Value = "THORAX";
                        ws.Cell(ToExcelCoordinates("4,1")).Value = "MIKROBIOLOGI";
                        ws.Cell(ToExcelCoordinates("5,1")).Value = "FIT/UNFIT";
                        ws.Cell(ToExcelCoordinates("6,1")).Value = "KESIMPULAN";
                        ws.Cell(ToExcelCoordinates("7,1")).Value = "SARAN";

                        if (lst.Count > 0)
                        {

                            List<MCUFormResultFieldReporting> lstData = new List<MCUFormResultFieldReporting>();
                            foreach (vMCUResultFormReport row in lst)
                            {
                                string value = row.FormResult;
                                ///1^Penyakit Yang pernah diderita=Tidak|3^Pernah Dirawat Di Rumah Sakit=Tidak|5^Berapa lama=-|6^Pernah Operasi=Tidak|8^Pernah Kecelakaan=Tidak|9^Kecelakaan dimana=Tidak Ada|222^Riwayat Alergi=Tidak|11^Merokok=Tidak|13^Alkohol=Tidak|15^Kopi=Tidak|17^Olah Raga=Tidak|230^Napza=Tidak|235^Lain-lain=Tidak|19^Hypertensi=Tidak|21^Diabetes=Tidak|23^Sakit Jantung=Tidak|25^Ginjal=Tidak|27^Ganguan Mental=Tidak|240^Cancer=Tidak|29^Lain2=Tidak|31^Apakah Sedang Menderita Penyakit=Tidak|33^Apakah Sedang Menjalani Pengobatan Tertentu=Tidak|39^Status Gizi=UnderWeight|41^Kulit=Dalam Batas Normal|43^Rambut=Dalam Batas Normal|52^Mana Kanan Konjungtiva=Tidak Hiperemis|62^Sklera=Dalam Batas Normal|63^Pupil=Isokor|64^Buta Warna=Total|65^Bola Mata=Simetris|53^Mata Kanan - Cornea=Keruh|72^Hidung=Dalam Batas Normal|73^Lidah=Dalam Batas Normal|74^Gigi Atas=Baik|82^Gigi Bawah=Baik|90^Pharing=Dalam Batas Normal|91^Pharing Abnormal=-|92^Tonsil=Tidak Hipertrofi|94^Tiroid=Dalam Batas Normal|95^Tiroid Abnormal=-|104^Frekuensi Pernapasan=Dalam Batas Normal|106^Paru paru=Dalam Batas Normal|107^Vesikuer=Dalam Batas Normal|66^Telinga Kanan - Telinga Luar=Serumen Propt (-)|67^Telinga Kanan - Membran Tympani=Utuh|68^Telinga Kanan - Membran Timpany tidak Utuh=-|69^Telinga Kiri - Telinga Luar=Serumen Propt (-)|70^Telinga Kiri - Membran Tympani=Utuh|71^Telinga Kiri - Membran Timpany tidak Utuh=-|100^Hasil Tensi=Dalam Batas Normal|101^Iktrus Kordis=Tidak Teraba|102^Auskultasi=BJ Murni|103^Kesan Batas Jantung=Melebar|112^Inspeksi=Dalam Batas Normal|275^Inspeksi=Dalam Batas Normal|113^Nyeri Tekan=Ada|114^Nyeri Lepas=Ada|115^Hati=Tidak Teraba|277^Hati=Tidak Teraba|116^Limpa=Tidak Teraba|117^Hernia=Tidak|118^Rectal Touche=Tidak Dilakukan|119^Rectal Touche Abnormal=-|120^Ginjal=Dalam Batas Normal|121^Ballotement=Dalam Batas Normal|122^Nyeri Ketok Kanan=Negatif|123^Nyeri Ketok Kiri=Negatif|124^Genital=Dalam Batas Normal|125^Refleks Fisiologis=Positif|126^Refleks Patologis=Negatif|127^Fungsi Motorik=Dalam Batas Normal|128^Fungsi Sensorik=Dalam Batas Normal|129^Tonus Otot=Eutoni|130^Ket Abnormal Tonus Otot=Ischialgia|279^Fungsi Sensorik=Dalam Batas Normal|282^Fungsi Sensorik=Dalam Batas Normal|131^Tengkorak=Dalam Batas Normal|132^Tulang Belakang=Dalam Batas Normal|134^Gerak Atas=Dalam Batas Normal|136^Anggota Gerak Atas=Dalam Batas Normal|138^Leher=Tidak Membesar|139^Axila=Tidak Membesar|140^Inguinal=Tidak Membesar|143^Radiologi=Abnormal|147^EKG=Abnormal|155^Penyakit akibat kerja=Ada|152^Hasil Hipertensi=Ya|153^Hasil Hipertensi Derajat=Hipertensi Derajat II (>= 160/100 mmHg)|154^Penyakit Diabetes=Ya|2^Penyakit apa dan sejak kapan=|4^Pernah Dirawat Penyakit apa dan sejak kapan=|7^Operasi Apa dan kapan=|10^Kecelakan Apa dan Kapan=|224^Alergi Makanan=|226^Alergi Obat=|228^Lain-lain=|12^Jumlah Rokok(Batang/hari)=0|14^Minum Alkohol(kali/Minggu)=0|16^Minum Kopi (Gelas/hari)=0|18^Olahraga(Jam/Satu Minggu)=0|232^Sebutkan=0|285^Sebutkan=0|238^Olahraga(Jam/Satu Minggu)=0|20^Klg yg Hypertensi=|22^Klg yg Diabetes=|24^Klg yg Sakit Jantung=|26^Klg yg Sakit Ginjal=|28^Klg yg Sakit Ganguan Jiwa=|242^Klg yg Sakit Cancer=|30^Klg yg Sakit Lain2/ALergi=|32^Jika Ya, Penyakit Apa=|34^Sudah Berapa Lama=|35^Pengobatan Apa, Terkontrol Atau Tidak=|243^Kesadaran=|245^GCS=|246^Tekanan Darah=|248^Denyut Nadi=|250^Frekuensi Nafas=|252^SpO2=|254^Frekuensi Nafas=|36^Tinggi Badan=|37^Berat Badan=|38^Lingkar Perut=|40^Body Mass Index=|42^Kulit Abnormal=|44^Abnormal Rambut=|216^Visus=|217^Visus=|46^Mata Kanan - Visus + Koreksi Kanan=|56^Mata Kiri - Visus + Koreksi=|47^Mata Kanan - ADD=|57^Visus Kiri - ADD=|259^Visus=|261^Visus=|263^Lensa=|265^Lensa=|267^Lensa=|54^Lain-lain=|0^AutoRef Kanan=|0^AutoRef Kiri=|0^NCT / Tonometri Kanan=|0^NCT / Tonometri Kiri=|0^Kacamata Kanan=|0^Kacamata Kanan=|75^Keterangan-Gigi Atas=|83^Keterangan-Gigi Bawah=|93^Keterangan Tonsil=|269^Keterangan Lain-lain=|105^Nilai Pernapasan=|271^Keterangan Lain-lain=|273^Keterangan Lain-lain=|0^Ket Abnormal Tengkorak=|133^Ket Abnormal Tengkorak=|135^Ket Abnormal Anggota Gerak Atas=|137^Ket Abnormal Anggota Gerak Bawah=|141^Anamnesa=Anamnesis|142^Pemeriksaan Fisik=Pemeriksaan Fisik |144^Radiologi Abnormal=Radiologi |148^Elektro Kardiografi Description=Elektro Kardiografi|149^Audiometri=Audiometri |145^Laboratorium=Laboratorium|220^Laboratorium Mikro=Laboratorium Mikro|159^Lain-lain= Lain-lain |151^Saran=Saran |156^Keterangan Penyakit Akibat Kerja Dicurigai / Ada=Penyakit akibat kerja |161^Kesimpulan=Kesimpulan|0^Penyakit Diabetes Keterangan=Diabetes Mellitus |157^Spirometri=Spirometri |158^Pap Smear= Pap Smear |169^Nama Perusahaan 1=|170^Nama Perusahaan 2=|171^Jenis Pekerjaan 1=|172^Jenis Pekerjaan 2=|173^Faktor Fisika 1=|174^Faktor Fisika 2=|175^Faktor Kimia 1=|176^Faktor Kimia 1=|177^Faktor Biologi 1=|178^Faktor Biologi 2=|179^Faktor Psikologi 1=|180^Faktor Psikologi 2=|181^Faktor Ergonomi 1=|182^Faktor Ergonomi 2=|183^Lama tahun bekerja 1=0|184^Lama Bulan bekerja 1=0|185^Lama tahun bekerja 2=0|186^Lama bulan bekerja 2=0|48^Mata Kanan - Myopia=Tidak|49^Mata Kanan - Hypermetrop=Tidak|50^Mata Kanan - Presbiop=Tidak|51^Mata Kanan - Cilindris=Tidak|58^Mata Kiri - Myopia=Tidak|59^Mata Kiri - Hypermetrop=Tidak|60^Mata Kiri - Presbiop=Tidak|61^Mata Kiri - Cilindris=Tidak|76^Gigi Atas - Radix=Tidak|78^Gigi Atas - Caries=Tidak|80^Gigi Atas - Missing=Tidak|77^Gigi Atas - Abrasi=Tidak|79^Gigi Atas - Impacted=Tidak|81^Gigi Atas - Kalkulus=Tidak|84^Gigi Bawah - Radix=Tidak|86^Gigi Bawah - Caries=Tidak|88^Gigi Bawah - Missing=Tidak|85^Gigi Bawah - Abrasi=Tidak|87^Gigi Bawah - Impacted=Tidak|89^Gigi Bawah - Kalkulus=Tidak|187^Fisika Pencahayaan=Tidak|188^Fisika Bising=Tidak|189^Fisika Getaran=Tidak|190^Fisika Suhu=Tidak|191^Kimia Partike (Debu/Asap)=Tidak|192^Kimia Cairan=Tidak|193^Kimia Gas/Uap=Tidak|194^Biologi Bakteri=Tidak|195^Biologi Virus=Tidak|196^Biologi Jamur=Tidak|197^Ergonomi Gerak Repetitif/ Berulang=Tidak|198^Ergonomi Angkat Beban Berat=Tidak|199^Ergonomi Akward Posture=Tidak|200^Ergonomi Mata Lelah=Tidak|201^Psikologi Stress=Tidak|202^Psikologi Shift=Tidak|||||||||||||||150^Kriteria Hiperkes=Fit For Work|||||

                                string[] data = value.Split('|');
                                if (data.Length > 0)
                                {
                                    MCUFormResultFieldReporting oData = new MCUFormResultFieldReporting();
                                    oData.TransactionNo = row.RegistrationNo;
                                    oData.RegistrasiNo = row.RegistrationNo;
                                    oData.TanggalPemeriksaan = oData.TglMasuk = row.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT_1);

                                    if (row.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                                    {
                                        oData.TglLahir = row.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_1);
                                    }
                                    else
                                    {
                                        oData.TglLahir = "";
                                    }

                                    //oData.JnsKelamin = row.Gender;
                                    //oData.Lokasi = filterCsvData(row.CorporateAccountDepartment, ",");
                                    oData.NamaPegawai = filterCsvData(row.PatientName, ",");
                                    //oData.NoPegawai = row.CorporateAccountNo;
                                    //oData.ParamedicCode = row.ParamedicCode;
                                    //oData.ParamedicName = filterCsvData(row.ParamedicName, ",");
                                    //oData.Penjamin = filterCsvData(row.BusinessPartnerName, ",");
                                    //oData.Posisi = filterCsvData(row.CorporateAccountDepartment, ",");

                                    for (int i = 0; i < data.Length; i++)
                                    {

                                        string[] field = data[i].Split('^');
                                        if (field.Length > 1)
                                        {
                                            string[] dataField = field[1].Split('=');
                                            string dataFieldValue = dataField[1];
                                            if (dataField.Length > 2)
                                            {
                                                for (int indexField = 0; indexField < dataField.Length; indexField++)
                                                {
                                                    if (i > 0)
                                                    {
                                                        dataFieldValue += string.Format("{0}=", dataField[indexField]);
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(dataFieldValue))
                                                {
                                                    dataFieldValue = dataFieldValue.Remove(dataFieldValue.Length - 1);
                                                    ///dataFieldValue = filterCsvData(dataFieldValue, ",");
                                                }
                                            }

                                            #region filter


                                            dataFieldValue = filterCsvData(dataFieldValue, ",");
                                            switch (field[0])
                                            {
                                                case "144": //RADIOLOGI
                                                    oData.Col144 = dataFieldValue; //6
                                                    break;
                                                case "145": //Laboratorium
                                                    oData.Col145 = dataFieldValue; //6
                                                    break;
                                                case "220": //Laboratorium Mikro
                                                    oData.Col220 = dataFieldValue; //7
                                                    break;
                                                case "150": //Kriteria Hiperkes
                                                    oData.Col150 = dataFieldValue; //8
                                                    break;
                                                case "151": // SARAN
                                                    oData.Col151 = dataFieldValue; //9
                                                    break;
                                                case "161": //KESIMPULAN
                                                    oData.Col161 = dataFieldValue; //10
                                                    break;

                                            }
                                            #endregion
                                        }
                                    }
                                    lstData.Add(oData);
                                }
                            }

                            if (lstData.Count > 0)
                            {

                                int i =1; 
                                foreach (MCUFormResultFieldReporting row in lstData)
                                {
                                    i += 1;
                                    ws.Cell(ToExcelCoordinates(string.Format("1,{0}", i))).Value = row.NamaPegawai;
                                    ws.Cell(ToExcelCoordinates(string.Format("2,{0}", i))).Value = row.Col220;
                                    ws.Cell(ToExcelCoordinates(string.Format("3,{0}", i))).Value = row.Col144;
                                    ws.Cell(ToExcelCoordinates(string.Format("4,{0}", i))).Value = row.Col220;
                                    ws.Cell(ToExcelCoordinates(string.Format("5,{0}", i))).Value = row.Col150;
                                    ws.Cell(ToExcelCoordinates(string.Format("6,{0}", i))).Value = row.Col161;
                                    ws.Cell(ToExcelCoordinates(string.Format("7,{0}", i))).Value = row.Col151;
                                    Builder.Append("\r\n");
                                }
                            }
                        }


                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=download.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
                else if (InitialHealthcare == "RSRT")
                {

                    Builder.AppendLine("No Bukti Transaksi|TglMasuk|No.Registrasi|No.Pegawai|Penjamin|Posisi|Lokasi|Nama Pegawai|Tanggal Lahir|Jns Kelamin|TglPemeriksaan|Kode Dr|Nama Dokter|Pernah Mengindap Penyakit|Penyakit Apa dan Kapan|Pernah dirawat di RS|Sakit Apa dan Kapan|Lama dirawat|Pernah Operasi|Operasi Apa dan Kapan|Pernah Kecelakaan|Kecelakaan dimana|Kecelakaan apa dan kapan|Merokok|Jumlah Rokok(Batang)|Alkohol|Minum Alkohol(Kali)|Kopi|Minum Kopi(Gelas)|Olah Raga|Lama Olahraga(Jam)|Hipertensi|Klg yg hipertensi|Diabetes|Klg yg diabetes|Sakit Jantung|Klg yg Sakit Jantung|Sakit Ginjal|Klg yg Sakit Ginjal|Gangguann Mental|Klg yg Sakit Mental|Lain2/ALergi|Klg yg Alergi/lain2|Apa sedang menderita penyakit?|Jika Ya Penyakitnya apa|Sedang menjalani pengobatan tertentu.?|Sudah berapa lama|Pengobatan apa ? Terkontrol atau tidak|TB (cm)|BB (Kg)|Anjuran BB (Kg)|Status Gizi|BMI|Kulit|Kulit Abnormal|Rambut|Abnormal Rambut|Mata Kanan - Visus|Mata Kanan -Visus+Koreksi|Mata Kanan-Add|Mata Kanan - Myopia|Mata Kanan - Hypermetrop|Mata Kanan - Persbiop|Mata Kanan - Cilindris|Mata Kanan - Konjungtiva|Mata Kanan - Cornea|Refleks Cahaya|Mata Kiri - Visus|Mata Kiri -Visus+Koreksi|Visus Kiri - Add|Mata Kiri - Myopia|Mata Kiri - Hypermetrop|Mata Kiri - Presbiop|Mata Kiri - Cilindris|Sklera|Pupil|Buta Warna|Bola Mata|Telinga Kanan - Telinga Luar|Telinga Kanan - Membran Timpany|Telinga Kanan - Membran Timpany tidak Utuh|Telinga Kiri - Telinga Luar|Telinga Kiri - Membran Timpany|Telinga Kiri - Membran Timpany tidak Utuh|Hidung|Lidah|Gigi Atas|Keterangan Gigi Atas|Gigi Atas - Radix|Gigi Atas - Abrasi|Gigi Atas - Caries|Gigi Atas - Impacted|Gigi Atas - Missing|Gigi Atas - Kalkulus|Gigi Bawah|Keterangan Gigi bawah|Gigi Bawah - Radix|Gigi Bawah - Abrasi|Gigi Bawah - Caries|Gigi Bawah - Impacted|Gigi Bawah - Missing|Gigi Bawah - Kalkulus|Pharing|Pharing Abnormal|Tonsil|Keterangan Tonsil|Tiroid|Tiroid Abnormal|Tekanan Darah (mm/Hg)|Frequensi Nadi|Irama|Irama Kardiovaskular|Hasil Tensi|Iktrus Kordis|Auskultasi|Kesan Batas Jantung|Frekuensi Pernapasan|Nilai Pernafasan|Paru-paru|Vesikuer|Ronchi|Wheezing|Perkusi Kanan|Perkusi Kiri|Inspeksi|Nyeri Tekan|Nyeri Lepas|Hati|Limpa|Hernia|Rectal Toucher|Rectal Toucher Abnormal|Ginjal|Ballotemen|Nyeri Ketok Kanan|Nyeri Ketok Kiri|Genital|Repleks Fisiologis|Repleks Patologis|Fungsi Motorik|Fungsi Sensorik|Tonus Otot|Ket Abnormal Tonus Otot|Tenggorok|Tulang Belakang|Ket Abnormal Tulang Belakang|Gerak Atas|Ket Abnormal Gerak Atas|Gerak Bawah|Ket Abnormal Gerak Bawah|Leher|Axila|Inguinal|Anamnesa|Pemeriksaan Fisik|Radiologi|Radiologi Abnormal|Laboratorium|ECG|EKG|EKG Abnormal|Audiometri|Kriteria Hiperkes|Saran|Hasil Hipertensi|Hasil Hipertensi Derajat|Penyakit Diabetes|Penyakit Akibat Kerja|Keterangan Penyakit Akibat Kerja Dicurigai / Ada|Spirometri|Pap Smear|Lain-lain||Kesimpulan");
                    if (lst.Count > 0)
                    {

                        List<MCUFormResultFieldReporting> lstData = new List<MCUFormResultFieldReporting>();
                        foreach (vMCUResultFormReport row in lst)
                        {
                            string value = row.FormResult;
                            ///"1^Penyakit Yang pernah diderita=Tidak|3^Pernah Dirawat Di Rumah Sakit=Tidak|5^Berapa lama=-|6^Pernah Operasi Apa=Tidak|8^Pernah Kecelakaan=Tidak|9^Kecelakaan dimana=Tidak Ada|11^Merokok=Tidak|13^Alkohol=Tidak|15^Kopi=Tidak|17^Olah Raga=Tidak|19^Hypertensi=Tidak|21^Diabetes=Tidak|23^Sakit Jantung=Tidak|25^Ginjal=Tidak|27^Ganguan Mental=Tidak|29^Lain2/ALergi=Tidak|31^Apakah Sedang Menderita Penyakit=Tidak|33^Apakah Sedang Menjalani Pengobatan=Tidak|39^Status Gizi=Normal|41^Kulit=Normal|43^Rambut=Normal|45^Visus Kanan=Abnormal|46^Visus Kiri=Abnormal|59^Konjungtiva=Tidak Hiperemis|60^Skelera=Normal|61^Pupil=Isokor|62^Buta Warna=Total|63^Bola Mata=Simetris|64^Cornea=Keruh|72^Hidung=Normal|73^Lidah=Normal|74^Gigi Atas=Baik|78^Gigi Bawah=Baik|82^Pharing=Normal|83^Pharing Abnormal=-|84^Tonsil=Tidak Hypertropi|86^Tiroid=Normal|87^Tiroid Abnormal=-|88^Frekuensi Pernapasan=Normal|90^Paru paru=Normal|100^Vesikuer=Normal|105^Telinga Kanan - Telinga Luar=Serumen Propt (-)|106^Telinga Luar - Membran Tympani=Utuh|107^Telinga Luar - Membran Timpany tidak Utuh=-|108^Telinga Kiri - Telinga Luar=Serumen Propt (-)|109^Telinga Kiri - Membran Tympani=Utuh|110^Telinga Kiri - Membran Timpany tidak Utuh=-|113^Irama=Reguler|115^Hasil Tensi=Normal|116^Iktrus Kordis=Tidak Teraba|117^Auskultasi=BJ Murni|118^Kesan Batas Jantung=Melebar|119^Inspeksi=Normal|120^Nyeri Tekan=Ada|121^Nyeri Lepas=Ada|122^Hati=Tidak Teraba|123^Limpa=Tidak Teraba|124^Hernia=Tidak|125^Rectal Touche=Tidak Dilakukan|126^null=-|127^Ginjal=Normal|128^Ballotement=Normal|129^Nyeri Ketok Kanan=Negatif|130^Nyeri Ketok Kiri=Negatif|131^Genital=1|132^Refleks Fisiologis=Positif|133^Refleks Patologis=Negatif|134^Fungsi Motorik=Normal|135^Fungsi Sensorik=Normal|136^Tonus Otot=Eutoni|137^Ket Abnormal Tonus Otot=Ischialgia|140^Tulang Belakang=Normal|142^Anggota Gerak Atas=Normal|144^Anggota Gerak Atas=Normal|146^Leher=Tidak Membesar|147^Axila=Tidak Membesar|148^Inguinal=Tidak Membesar|151^Radiologi=Tidak Dilakukan|153^EKG=Tidak Dilakukan|159^Penyakit akibat kerja=Tidak|163^Hasil Hipertensi=Tidak|164^Hasil Hipertensi Derajat=-|165^Penyakit Diabetes=-|2^Penyakit apa dan sejak kapan=|4^Pernah Dirawat Penyakit apa dan sejak kapan=|7^Operasi Apa dan kapan=|10^Kecelakan Apa dan Kapan=|12^Jumlah Rokok(Batang/hari)=0|14^Alkohol(kali/Minggu)=0|16^Minum Kopi (Gelas/hari)=0|18^Olahraga(Jam/Satu Minggu)=0|20^Klg yg Hypertensi=|22^Klg yg Diabetes=|24^Klg yg Sakit Jantung=|26^Klg yg Sakit Ginjal=|28^Klg yg Sakit Ganguan Jiwa=|30^Klg yg Sakit Lain2/ALergi=|32^Jika Ya, Penyakit Apa=|34^Sudah Berapa Lama=|35^Pengobatan Apa Dan Apakah Terkontrol=|36^Tinggi Badan=180|37^Berat Badan=80|38^Anjuran Berat Badan=80|40^Body Mass Index=25.00|42^null=|44^keterangan Abnormal=|47^Visus + Koreksi Kanan=|48^Visus + Koreksi Kiri=|49^Add Visus Kanan=|50^Add Visus Kiri=|65^Lain-lain=|66^AutoRef Kanan=|67^AutoRef Kiri=|68^NCT / Tonometri Kanan=|69^NCT / Tonometri Kiri=|70^Kacamata Kanan=|71^Kacamata Kanan=|75^Gigi Atas - Keterangan=|79^Gigi Bawah - Keterangan=|85^Keterangan Tonsil=|89^Nilai Pernapasan=|111^Tekanan Darah (mm/Hg)=|112^Frequensi Nadi=|114^Irama Kardiovaskular=|139^Ket Abnormal Tengkorak=|141^Ket Abnormal Tengkorak=|143^Ket Abnormal Anggota Gerak Atas=|145^Ket Abnormal Anggota Gerak Atas=|149^Anamnesa=|150^Pemeriksaan Fisik=|152^Radiologi Abnormal=|154^Elektro Kardiografi Description=|155^Audiometri=|156^Laboratorium=|157^Lain-lain=|158^Saran=|160^Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|161^Kesimpulan=|166^Penyakit Diabetes Keterangan=|167^Spirometri=|168^Pap Smear=|169^Nama Perusahaan 1=|170^Nama Perusahaan 2=|171^Jenis Pekerjaan 1=|172^Jenis Pekerjaan 2=|173^Faktor Fisika 1=|174^Faktor Fisika 2=|175^Faktor Kimia 1=|176^Faktor Kimia 1=|177^Faktor Biologi 1=|178^Faktor Biologi 2=|179^Faktor Psikologi 1=|180^Faktor Psikologi 2=|181^Faktor Ergonomi 1=|182^Faktor Ergonomi 2=|183^Lama tahun bekerja 1=0|184^Lama Bulan bekerja 1=0|185^Lama tahun bekerja 2=0|186^Lama bulan bekerja 2=0|51^Mata Kanan - Myopia=Ya|52^Mata Kanan - Hypermetrop=Ya|53^Mata Kanan - Presbiop=Ya|54^Mata Kanan - Cilindris=Ya|55^Mata Kiri - Myopia=|56^Mata Kiri - Hypermetrop=|57^Mata Kiri - Presbiop=|58^Mata Kiri - Cilindris=|76^Gigi Atas - Radix=Ya|76_01^Gigi Atas - Caries=|76_02^Gigi Atas - Missing=|77^Gigi Atas - Abrasi=|77_01^Gigi Atas - Impacted=Ya|77_02^Gigi Atas - Kalkulus=Ya|80^Gigi Bawah - Radix=|80_01^Gigi Bawah - Caries=|80_02^Gigi Bawah - Missing=|81^Gigi Bawah - Abrasi=Ya|80_02^Gigi Bawah - Impacted=|80_03^Gigi Bawah - Kalkulus=|187^Fisika Pencahayaan=|188^Fisika Bising=|189^Fisika Getaran=|190^Fisika Suhu=|191^Kimia Partike (Debu/Asap)=|192^Kimia Cairan=|193^Kimia Gas/Uap=|194^Biologi Bakteri=|195^Biologi Virus=|196^Biologi Jamur=|197^Ergonomi Gerak Repetitif/ Berulang=|198^Ergonomi Angkat Beban Berat=|199^Ergonomi Akward Posture=|200^Ergonomi Mata Lelah=|201^Psikologi Stress=|202^Psikologi Shift=|101^Ronchi=|101^Ronchi=Ya|101^Ronchi=|101^Ronchi=|102^Wheezing=|102^Wheezing=|102^Wheezing=|102^Wheezing=|103^Perkusi Kanan=Ya|103^Perkusi Kanan=|103^Perkusi Kanan=|104^Perkusi Kiri=Ya|104^Perkusi Kiri=|104^Perkusi Kiri=|162^Kriteria Hiperkes=|162^Kriteria Hiperkes=Ya|162^Kriteria Hiperkes=|162^Kriteria Hiperkes=|162^Kriteria Hiperkes=|162^Kriteria Hiperkes=";
                            string[] data = value.Split('|');
                            if (data.Length > 0)
                            {
                                MCUFormResultFieldReporting oData = new MCUFormResultFieldReporting();
                                oData.TransactionNo = row.RegistrationNo;
                                oData.RegistrasiNo = row.RegistrationNo;
                                oData.TanggalPemeriksaan = oData.TglMasuk = row.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT_1);

                                if (row.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                                {
                                    oData.TglLahir = row.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_1);
                                }
                                else
                                {
                                    oData.TglLahir = "";
                                }

                                oData.JnsKelamin = row.Gender;
                                oData.Lokasi = filterCsvData(row.CorporateAccountDepartment, ",");
                                oData.NamaPegawai = filterCsvData(row.PatientName, ",");
                                oData.NoPegawai = row.CorporateAccountNo;
                                oData.ParamedicCode = row.ParamedicCode;
                                oData.ParamedicName = filterCsvData(row.ParamedicName, ",");
                                oData.Penjamin = filterCsvData(row.BusinessPartnerName, ",");
                                oData.Posisi = filterCsvData(row.CorporateAccountDepartment, ",");

                                for (int i = 0; i < data.Length; i++)
                                {

                                    string[] field = data[i].Split('^');
                                    if (field.Length > 1)
                                    {
                                        if (!string.IsNullOrEmpty(data[i]))
                                        {
                                            string[] dataField = field[1].Split('=');
                                            string dataFieldValue = dataField[1];
                                            if (dataField.Length > 2)
                                            {
                                                for (int indexField = 0; indexField < dataField.Length; indexField++)
                                                {
                                                    if (i > 0)
                                                    {
                                                        dataFieldValue += string.Format("{0}=", dataField[indexField]);
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(dataFieldValue))
                                                {
                                                    dataFieldValue = dataFieldValue.Remove(dataFieldValue.Length - 1);
                                                    ///dataFieldValue = filterCsvData(dataFieldValue, ",");
                                                }
                                            }

                                            #region filter
                                            dataFieldValue = filterCsvData(dataFieldValue, ",");
                                            if (dataFieldValue == "-")
                                            {
                                                dataFieldValue = " - ";
                                            }
                                            switch (field[0])
                                            {
                                                case "1":
                                                    oData.Col1 = dataFieldValue;
                                                    break;
                                                case "2":
                                                    oData.Col2 = dataFieldValue;
                                                    break;
                                                case "3":
                                                    oData.Col3 = dataFieldValue;
                                                    break;
                                                case "4":
                                                    oData.Col4 = dataFieldValue;
                                                    break;
                                                case "5":
                                                    oData.Col5 = dataFieldValue;
                                                    break;
                                                case "6":
                                                    oData.Col6 = dataFieldValue;
                                                    break;
                                                case "7":
                                                    oData.Col7 = dataFieldValue;
                                                    break;
                                                case "8":
                                                    oData.Col8 = dataFieldValue;
                                                    break;
                                                case "9":
                                                    oData.Col9 = dataFieldValue;
                                                    break;
                                                case "10":
                                                    oData.Col10 = dataFieldValue;
                                                    break;
                                                case "11":
                                                    oData.Col11 = dataFieldValue;
                                                    break;
                                                case "12":
                                                    oData.Col12 = dataFieldValue;
                                                    break;
                                                case "13":
                                                    oData.Col13 = dataFieldValue;
                                                    break;
                                                case "14":
                                                    oData.Col14 = dataFieldValue;
                                                    break;
                                                case "15":
                                                    oData.Col15 = dataFieldValue;
                                                    break;
                                                case "16":
                                                    oData.Col16 = dataFieldValue;
                                                    break;
                                                case "17":
                                                    oData.Col17 = dataFieldValue;
                                                    break;
                                                case "18":
                                                    oData.Col18 = dataFieldValue;
                                                    break;
                                                case "19":
                                                    oData.Col19 = dataFieldValue;
                                                    break;
                                                case "20":
                                                    oData.Col20 = dataFieldValue;
                                                    break;
                                                case "21":
                                                    oData.Col21 = dataFieldValue;
                                                    break;
                                                case "22":
                                                    oData.Col22 = dataFieldValue;
                                                    break;
                                                case "23":
                                                    oData.Col23 = dataFieldValue;
                                                    break;
                                                case "24":
                                                    oData.Col24 = dataFieldValue;
                                                    break;
                                                case "25":
                                                    oData.Col25 = dataFieldValue;
                                                    break;
                                                case "26":
                                                    oData.Col26 = dataFieldValue;
                                                    break;
                                                case "27":
                                                    oData.Col27 = dataFieldValue;
                                                    break;
                                                case "28":
                                                    oData.Col28 = dataFieldValue;
                                                    break;
                                                case "29":
                                                    oData.Col29 = dataFieldValue;
                                                    break;
                                                case "30":
                                                    oData.Col30 = dataFieldValue;
                                                    break;
                                                case "31":
                                                    oData.Col31 = dataFieldValue;
                                                    break;
                                                case "32":
                                                    oData.Col32 = dataFieldValue;
                                                    break;
                                                case "33":
                                                    oData.Col33 = dataFieldValue;
                                                    break;
                                                case "34":
                                                    oData.Col34 = dataFieldValue;
                                                    break;
                                                case "35":
                                                    oData.Col35 = dataFieldValue;
                                                    break;
                                                case "36":
                                                    oData.Col36 = dataFieldValue;
                                                    break;
                                                case "37":
                                                    oData.Col37 = dataFieldValue;
                                                    break;
                                                case "38":
                                                    oData.Col38 = dataFieldValue;
                                                    break;
                                                case "39":
                                                    oData.Col39 = dataFieldValue;
                                                    break;
                                                case "40":
                                                    oData.Col40 = dataFieldValue;
                                                    break;
                                                case "41":
                                                    oData.Col41 = dataFieldValue;
                                                    break;
                                                case "42":
                                                    oData.Col42 = dataFieldValue;
                                                    break;
                                                case "43":
                                                    oData.Col43 = dataFieldValue;
                                                    break;
                                                case "44":
                                                    oData.Col44 = dataFieldValue;
                                                    break;
                                                case "45":
                                                    oData.Col45 = dataFieldValue;
                                                    break;
                                                case "46":
                                                    oData.Col46 = dataFieldValue;
                                                    break;
                                                case "47":
                                                    oData.Col47 = dataFieldValue;
                                                    break;
                                                case "48":
                                                    oData.Col48 = dataFieldValue;
                                                    break;
                                                case "49":
                                                    oData.Col49 = dataFieldValue;
                                                    break;
                                                case "50":
                                                    oData.Col50 = dataFieldValue;
                                                    break;
                                                case "51":
                                                    oData.Col51 = dataFieldValue;
                                                    break;
                                                case "52":
                                                    oData.Col52 = dataFieldValue;
                                                    break;
                                                case "53":
                                                    oData.Col53 = dataFieldValue;
                                                    break;
                                                case "54":
                                                    oData.Col54 = dataFieldValue;
                                                    break;
                                                case "55":
                                                    oData.Col55 = dataFieldValue;
                                                    break;
                                                case "56":
                                                    oData.Col56 = dataFieldValue;
                                                    break;
                                                case "57":
                                                    oData.Col57 = dataFieldValue;
                                                    break;
                                                case "58":
                                                    oData.Col58 = dataFieldValue;
                                                    break;
                                                case "59":
                                                    oData.Col59 = dataFieldValue;
                                                    break;
                                                case "60":
                                                    oData.Col60 = dataFieldValue;
                                                    break;
                                                case "61":
                                                    oData.Col61 = dataFieldValue;
                                                    break;
                                                case "62":
                                                    oData.Col62 = dataFieldValue;
                                                    break;
                                                case "63":
                                                    oData.Col63 = dataFieldValue;
                                                    break;
                                                case "64":
                                                    oData.Col64 = dataFieldValue;
                                                    break;
                                                case "65":
                                                    oData.Col65 = dataFieldValue;
                                                    break;
                                                case "66":
                                                    oData.Col66 = dataFieldValue;
                                                    break;
                                                case "67":
                                                    oData.Col67 = dataFieldValue;
                                                    break;
                                                case "68":
                                                    oData.Col68 = dataFieldValue;
                                                    break;
                                                case "69":
                                                    oData.Col69 = dataFieldValue;
                                                    break;
                                                case "70":
                                                    oData.Col70 = dataFieldValue;
                                                    break;
                                                case "71":
                                                    oData.Col71 = dataFieldValue;
                                                    break;
                                                case "72":
                                                    oData.Col72 = dataFieldValue;
                                                    break;
                                                case "73":
                                                    oData.Col73 = dataFieldValue;
                                                    break;
                                                case "74":
                                                    oData.Col74 = dataFieldValue;
                                                    break;
                                                case "75":
                                                    oData.Col75 = dataFieldValue;
                                                    break;
                                                case "76":
                                                    oData.Col76 = dataFieldValue;
                                                    break;
                                                case "77":
                                                    oData.Col77 = dataFieldValue;
                                                    break;
                                                case "78":
                                                    oData.Col78 = dataFieldValue;
                                                    break;
                                                case "79":
                                                    oData.Col79 = dataFieldValue;
                                                    break;
                                                case "80":
                                                    oData.Col80 = dataFieldValue;
                                                    break;
                                                case "81":
                                                    oData.Col81 = dataFieldValue;
                                                    break;
                                                case "82":
                                                    oData.Col82 = dataFieldValue;
                                                    break;
                                                case "83":
                                                    oData.Col83 = dataFieldValue;
                                                    break;
                                                case "84":
                                                    oData.Col84 = dataFieldValue;
                                                    break;
                                                case "85":
                                                    oData.Col85 = dataFieldValue;
                                                    break;
                                                case "86":
                                                    oData.Col86 = dataFieldValue;
                                                    break;
                                                case "87":
                                                    oData.Col87 = dataFieldValue;
                                                    break;
                                                case "88":
                                                    oData.Col88 = dataFieldValue;
                                                    break;
                                                case "89":
                                                    oData.Col89 = dataFieldValue;
                                                    break;
                                                case "90":
                                                    oData.Col90 = dataFieldValue;
                                                    break;
                                                case "91":
                                                    oData.Col91 = dataFieldValue;
                                                    break;
                                                case "92":
                                                    oData.Col92 = dataFieldValue;
                                                    break;
                                                case "93":
                                                    oData.Col93 = dataFieldValue;
                                                    break;
                                                case "94":
                                                    oData.Col94 = dataFieldValue;
                                                    break;
                                                case "95":
                                                    oData.Col95 = dataFieldValue;
                                                    break;
                                                case "96":
                                                    oData.Col96 = dataFieldValue;
                                                    break;
                                                case "97":
                                                    oData.Col97 = dataFieldValue;
                                                    break;
                                                case "98":
                                                    oData.Col98 = dataFieldValue;
                                                    break;
                                                case "99":
                                                    oData.Col99 = dataFieldValue;
                                                    break;
                                                case "100":
                                                    oData.Col100 = dataFieldValue;
                                                    break;
                                                case "101":
                                                    oData.Col101 = dataFieldValue;
                                                    break;
                                                case "102":
                                                    oData.Col102 = dataFieldValue;
                                                    break;
                                                case "103":
                                                    oData.Col103 = dataFieldValue;
                                                    break;
                                                case "104":
                                                    oData.Col104 = dataFieldValue;
                                                    break;
                                                case "105":
                                                    oData.Col105 = dataFieldValue;
                                                    break;
                                                case "106":
                                                    oData.Col106 = dataFieldValue;
                                                    break;
                                                case "107":
                                                    oData.Col107 = dataFieldValue;
                                                    break;
                                                case "108":
                                                    oData.Col108 = dataFieldValue;
                                                    break;
                                                case "109":
                                                    oData.Col109 = dataFieldValue;
                                                    break;
                                                case "110":
                                                    oData.Col110 = dataFieldValue;
                                                    break;
                                                case "111":
                                                    oData.Col111 = dataFieldValue;
                                                    break;
                                                case "112":
                                                    oData.Col112 = dataFieldValue;
                                                    break;
                                                case "113":
                                                    oData.Col113 = dataFieldValue;
                                                    break;
                                                case "114":
                                                    oData.Col114 = dataFieldValue;
                                                    break;
                                                case "115":
                                                    oData.Col115 = dataFieldValue;
                                                    break;
                                                case "116":
                                                    oData.Col116 = dataFieldValue;
                                                    break;
                                                case "117":
                                                    oData.Col117 = dataFieldValue;
                                                    break;
                                                case "118":
                                                    oData.Col118 = dataFieldValue;
                                                    break;
                                                case "119":
                                                    oData.Col119 = dataFieldValue;
                                                    break;
                                                case "120":
                                                    oData.Col120 = dataFieldValue;
                                                    break;
                                                case "121":
                                                    oData.Col121 = dataFieldValue;
                                                    break;
                                                case "122":
                                                    oData.Col122 = dataFieldValue;
                                                    break;
                                                case "123":
                                                    oData.Col123 = dataFieldValue;
                                                    break;
                                                case "124":
                                                    oData.Col124 = dataFieldValue;
                                                    break;
                                                case "125":
                                                    oData.Col125 = dataFieldValue;
                                                    break;
                                                case "126":
                                                    oData.Col126 = dataFieldValue;
                                                    break;
                                                case "127":
                                                    oData.Col127 = dataFieldValue;
                                                    break;
                                                case "128":
                                                    oData.Col128 = dataFieldValue;
                                                    break;
                                                case "129":
                                                    oData.Col129 = dataFieldValue;
                                                    break;
                                                case "130":
                                                    oData.Col130 = dataFieldValue;
                                                    break;
                                                case "131":
                                                    oData.Col131 = dataFieldValue;
                                                    break;
                                                case "132":
                                                    oData.Col132 = dataFieldValue;
                                                    break;
                                                case "133":
                                                    oData.Col133 = dataFieldValue;
                                                    break;
                                                case "134":
                                                    oData.Col134 = dataFieldValue;
                                                    break;
                                                case "135":
                                                    oData.Col135 = dataFieldValue;
                                                    break;
                                                case "136":
                                                    oData.Col136 = dataFieldValue;
                                                    break;
                                                case "137":
                                                    oData.Col137 = dataFieldValue;
                                                    break;
                                                case "138":
                                                    oData.Col138 = dataFieldValue;
                                                    break;
                                                case "139":
                                                    oData.Col139 = dataFieldValue;
                                                    break;
                                                case "140":
                                                    oData.Col140 = dataFieldValue;
                                                    break;
                                                case "141":
                                                    oData.Col141 = dataFieldValue;
                                                    break;
                                                case "142":
                                                    oData.Col142 = dataFieldValue;
                                                    break;
                                                case "143":
                                                    oData.Col143 = dataFieldValue;
                                                    break;
                                                case "144":
                                                    oData.Col144 = dataFieldValue;
                                                    break;
                                                case "145":
                                                    oData.Col145 = dataFieldValue;
                                                    break;
                                                case "146":
                                                    oData.Col146 = dataFieldValue;
                                                    break;
                                                case "147":
                                                    oData.Col147 = dataFieldValue;
                                                    break;
                                                case "148":
                                                    oData.Col148 = dataFieldValue;
                                                    break;
                                                case "149":
                                                    oData.Col149 = dataFieldValue;
                                                    break;
                                                case "150":
                                                    oData.Col150 = dataFieldValue;
                                                    break;
                                                case "151":
                                                    oData.Col151 = dataFieldValue;
                                                    break;
                                                case "152":
                                                    oData.Col152 = dataFieldValue;
                                                    break;
                                                case "153":
                                                    oData.Col153 = dataFieldValue;
                                                    break;
                                                case "154":
                                                    oData.Col154 = dataFieldValue;
                                                    break;
                                                case "155":
                                                    oData.Col155 = dataFieldValue;
                                                    break;
                                                case "156":
                                                    oData.Col156 = dataFieldValue;
                                                    break;
                                                case "157":
                                                    oData.Col157 = dataFieldValue;
                                                    break;
                                                case "158":
                                                    oData.Col158 = dataFieldValue;
                                                    break;
                                                case "159":
                                                    oData.Col159 = dataFieldValue;
                                                    break;
                                                case "160":
                                                    oData.Col160 = dataFieldValue;
                                                    break;
                                                case "161":
                                                    oData.Col161 = dataFieldValue;
                                                    break;
                                                case "162":
                                                    oData.Col162 = dataFieldValue;
                                                    break;
                                                case "163":
                                                    oData.Col163 = dataFieldValue;
                                                    break;
                                                case "164":
                                                    oData.Col164 = dataFieldValue;
                                                    break;
                                                case "165":
                                                    oData.Col165 = dataFieldValue;
                                                    break;
                                                case "166":
                                                    oData.Col166 = dataFieldValue;
                                                    break;
                                                case "167":
                                                    oData.Col168 = dataFieldValue;
                                                    break;
                                                case "168":
                                                    oData.Col168 = dataFieldValue;
                                                    break;
                                                case "169":
                                                    oData.Col169 = dataFieldValue;
                                                    break;
                                                case "170":
                                                    oData.Col170 = dataFieldValue;
                                                    break;
                                                case "171":
                                                    oData.Col171 = dataFieldValue;
                                                    break;
                                                case "172":
                                                    oData.Col172 = dataFieldValue;
                                                    break;
                                                case "173":
                                                    oData.Col173 = dataFieldValue;
                                                    break;
                                                case "174":
                                                    oData.Col174 = dataFieldValue;
                                                    break;
                                                case "175":
                                                    oData.Col175 = dataFieldValue;
                                                    break;
                                                case "176":
                                                    oData.Col176 = dataFieldValue;
                                                    break;
                                                case "177":
                                                    oData.Col177 = dataFieldValue;
                                                    break;
                                                case "178":
                                                    oData.Col178 = dataFieldValue;
                                                    break;
                                                case "179":
                                                    oData.Col179 = dataFieldValue;
                                                    break;
                                                case "180":
                                                    oData.Col180 = dataFieldValue;
                                                    break;
                                                case "181":
                                                    oData.Col181 = dataFieldValue;
                                                    break;
                                                case "182":
                                                    oData.Col182 = dataFieldValue;
                                                    break;
                                                case "183":
                                                    oData.Col183 = dataFieldValue;
                                                    break;
                                                case "184":
                                                    oData.Col184 = dataFieldValue;
                                                    break;
                                                case "185":
                                                    oData.Col185 = dataFieldValue;
                                                    break;
                                                case "186":
                                                    oData.Col186 = dataFieldValue;
                                                    break;
                                                case "187":
                                                    oData.Col187 = dataFieldValue;
                                                    break;
                                                case "188":
                                                    oData.Col188 = dataFieldValue;
                                                    break;
                                                case "189":
                                                    oData.Col189 = dataFieldValue;
                                                    break;
                                                case "190":
                                                    oData.Col190 = dataFieldValue;
                                                    break;
                                                case "191":
                                                    oData.Col191 = dataFieldValue;
                                                    break;
                                                case "192":
                                                    oData.Col192 = dataFieldValue;
                                                    break;
                                                case "193":
                                                    oData.Col193 = dataFieldValue;
                                                    break;
                                                case "194":
                                                    oData.Col194 = dataFieldValue;
                                                    break;
                                                case "195":
                                                    oData.Col195 = dataFieldValue;
                                                    break;
                                                case "196":
                                                    oData.Col196 = dataFieldValue;
                                                    break;
                                                case "197":
                                                    oData.Col197 = dataFieldValue;
                                                    break;
                                                case "198":
                                                    oData.Col198 = dataFieldValue;
                                                    break;
                                                case "199":
                                                    oData.Col199 = dataFieldValue;
                                                    break;
                                                case "200":
                                                    oData.Col200 = dataFieldValue;
                                                    break;
                                                case "201":
                                                    oData.Col201 = dataFieldValue;
                                                    break;
                                                case "202":
                                                    oData.Col202 = dataFieldValue;
                                                    break;
                                                case "203":
                                                    oData.Col203 = dataFieldValue;
                                                    break;
                                                case "204":
                                                    oData.Col204 = dataFieldValue;
                                                    break;
                                                case "205":
                                                    oData.Col205 = dataFieldValue;
                                                    break;
                                                case "206":
                                                    oData.Col206 = dataFieldValue;
                                                    break;
                                                case "207":
                                                    oData.Col207 = dataFieldValue;
                                                    break;
                                                case "208":
                                                    oData.Col208 = dataFieldValue;
                                                    break;
                                                case "209":
                                                    oData.Col209 = dataFieldValue;
                                                    break;
                                                case "210":
                                                    oData.Col210 = dataFieldValue;
                                                    break;
                                                case "211":
                                                    oData.Col211 = dataFieldValue;
                                                    break;
                                                case "212":
                                                    oData.Col212 = dataFieldValue;
                                                    break;
                                                case "213":
                                                    oData.Col213 = dataFieldValue;
                                                    break;
                                                case "214":
                                                    oData.Col214 = dataFieldValue;
                                                    break;
                                                case "215":
                                                    oData.Col215 = dataFieldValue;
                                                    break;
                                            }
                                            #endregion
                                        }

                                    }
                                }
                                lstData.Add(oData);
                            }
                        }

                        if (lstData.Count > 0)
                        {



                            foreach (MCUFormResultFieldReporting row in lstData)
                            {
                                Builder.Append(string.Format("{0}|", row.TransactionNo));
                                Builder.Append(string.Format("{0}|", row.TanggalPemeriksaan));
                                Builder.Append(string.Format("{0}|", row.RegistrasiNo));
                                Builder.Append(string.Format("{0}|", row.NoPegawai));
                                Builder.Append(string.Format("{0}|", row.Penjamin));
                                Builder.Append(string.Format("{0}|", row.Posisi));
                                Builder.Append(string.Format("{0}|", row.Lokasi));
                                Builder.Append(string.Format("{0}|", row.NamaPegawai));
                                Builder.Append(string.Format("{0}|", row.TglLahir));
                                Builder.Append(string.Format("{0}|", row.JnsKelamin));
                                Builder.Append(string.Format("{0}|", row.TanggalPemeriksaan));
                                Builder.Append(string.Format("{0}|", row.ParamedicCode));
                                Builder.Append(string.Format("{0}|", row.ParamedicName));
                                Builder.Append(string.Format("{0}|", row.Col1));
                                Builder.Append(string.Format("{0}	|", row.Col2));
                                Builder.Append(string.Format("{0}	|", row.Col3));
                                Builder.Append(string.Format("{0}	|", row.Col4));
                                Builder.Append(string.Format("{0}	|", row.Col5));
                                Builder.Append(string.Format("{0}	|", row.Col6));
                                Builder.Append(string.Format("{0}	|", row.Col7));
                                Builder.Append(string.Format("{0}	|", row.Col8));
                                Builder.Append(string.Format("{0}	|", row.Col9));
                                Builder.Append(string.Format("{0}	|", row.Col10));
                                Builder.Append(string.Format("{0}	|", row.Col11));
                                Builder.Append(string.Format("{0}	|", row.Col12));
                                Builder.Append(string.Format("{0}	|", row.Col13));
                                Builder.Append(string.Format("{0}	|", row.Col14));
                                Builder.Append(string.Format("{0}	|", row.Col15));
                                Builder.Append(string.Format("{0}	|", row.Col16));
                                Builder.Append(string.Format("{0}	|", row.Col17));
                                Builder.Append(string.Format("{0}	|", row.Col18));
                                Builder.Append(string.Format("{0}	|", row.Col19));
                                Builder.Append(string.Format("{0}	|", row.Col20));
                                Builder.Append(string.Format("{0}	|", row.Col21));
                                Builder.Append(string.Format("{0}	|", row.Col22));
                                Builder.Append(string.Format("{0}	|", row.Col23));
                                Builder.Append(string.Format("{0}	|", row.Col24));
                                Builder.Append(string.Format("{0}	|", row.Col25));
                                Builder.Append(string.Format("{0}	|", row.Col26));
                                Builder.Append(string.Format("{0}	|", row.Col27));
                                Builder.Append(string.Format("{0}	|", row.Col28));
                                Builder.Append(string.Format("{0}	|", row.Col29));
                                Builder.Append(string.Format("{0}	|", row.Col30));
                                Builder.Append(string.Format("{0}	|", row.Col31));
                                Builder.Append(string.Format("{0}	|", row.Col32));
                                Builder.Append(string.Format("{0}	|", row.Col33));
                                Builder.Append(string.Format("{0}	|", row.Col34));
                                Builder.Append(string.Format("{0}	|", row.Col35));
                                Builder.Append(string.Format("{0}	|", row.Col36));
                                Builder.Append(string.Format("{0}	|", row.Col37));
                                Builder.Append(string.Format("{0}	|", row.Col38));
                                Builder.Append(string.Format("{0}	|", row.Col39));
                                Builder.Append(string.Format("{0}	|", row.Col40));
                                Builder.Append(string.Format("{0}	|", row.Col41));
                                Builder.Append(string.Format("{0}	|", row.Col42));
                                Builder.Append(string.Format("{0}	|", row.Col43));
                                Builder.Append(string.Format("{0}	|", row.Col44));
                                Builder.Append(string.Format("{0}	|", row.Col45));
                                Builder.Append(string.Format("{0}	|", row.Col46));
                                Builder.Append(string.Format("{0}	|", row.Col47));
                                Builder.Append(string.Format("{0}	|", row.Col48));
                                Builder.Append(string.Format("{0}	|", row.Col49));
                                Builder.Append(string.Format("{0}	|", row.Col50));
                                Builder.Append(string.Format("{0}	|", row.Col51));
                                Builder.Append(string.Format("{0}	|", row.Col52));
                                Builder.Append(string.Format("{0}	|", row.Col53));
                                Builder.Append(string.Format("{0}	|", row.Col54));
                                Builder.Append(string.Format("{0}	|", row.Col55));
                                Builder.Append(string.Format("{0}	|", row.Col56));
                                Builder.Append(string.Format("{0}	|", row.Col57));
                                Builder.Append(string.Format("{0}	|", row.Col58));
                                Builder.Append(string.Format("{0}	|", row.Col59));
                                Builder.Append(string.Format("{0}	|", row.Col60));
                                Builder.Append(string.Format("{0}	|", row.Col61));
                                Builder.Append(string.Format("{0}	|", row.Col62));
                                Builder.Append(string.Format("{0}	|", row.Col63));
                                Builder.Append(string.Format("{0}	|", row.Col64));
                                Builder.Append(string.Format("{0}	|", row.Col65));
                                Builder.Append(string.Format("{0}	|", row.Col66));
                                Builder.Append(string.Format("{0}	|", row.Col67));
                                Builder.Append(string.Format("{0}	|", row.Col68));
                                Builder.Append(string.Format("{0}	|", row.Col69));
                                Builder.Append(string.Format("{0}	|", row.Col70));
                                Builder.Append(string.Format("{0}	|", row.Col71));
                                Builder.Append(string.Format("{0}	|", row.Col72));
                                Builder.Append(string.Format("{0}	|", row.Col73));
                                Builder.Append(string.Format("{0}	|", row.Col74));
                                Builder.Append(string.Format("{0}	|", row.Col75));
                                Builder.Append(string.Format("{0}	|", row.Col76));
                                Builder.Append(string.Format("{0}	|", row.Col77));
                                Builder.Append(string.Format("{0}	|", row.Col78));
                                Builder.Append(string.Format("{0}	|", row.Col79));
                                Builder.Append(string.Format("{0}	|", row.Col80));
                                Builder.Append(string.Format("{0}	|", row.Col81));
                                Builder.Append(string.Format("{0}	|", row.Col82));
                                Builder.Append(string.Format("{0}	|", row.Col83));
                                Builder.Append(string.Format("{0}	|", row.Col84));
                                Builder.Append(string.Format("{0}	|", row.Col85));
                                Builder.Append(string.Format("{0}	|", row.Col86));
                                Builder.Append(string.Format("{0}	|", row.Col87));
                                Builder.Append(string.Format("{0}	|", row.Col88));
                                Builder.Append(string.Format("{0}	|", row.Col89));
                                Builder.Append(string.Format("{0}	|", row.Col90));
                                Builder.Append(string.Format("{0}	|", row.Col91));
                                Builder.Append(string.Format("{0}	|", row.Col92));
                                Builder.Append(string.Format("{0}	|", row.Col93));
                                Builder.Append(string.Format("{0}	|", row.Col94));
                                Builder.Append(string.Format("{0}	|", row.Col95));
                                Builder.Append(string.Format("{0}	|", row.Col96));
                                Builder.Append(string.Format("{0}	|", row.Col97));
                                Builder.Append(string.Format("{0}	|", row.Col98));
                                Builder.Append(string.Format("{0}	|", row.Col99));
                                Builder.Append(string.Format("{0}	|", row.Col100));
                                Builder.Append(string.Format("{0}	|", row.Col101));
                                Builder.Append(string.Format("{0}	|", row.Col102));
                                Builder.Append(string.Format("{0}	|", row.Col103));
                                Builder.Append(string.Format("{0}	|", row.Col104));
                                Builder.Append(string.Format("{0}	|", row.Col105));
                                Builder.Append(string.Format("{0}	|", row.Col106));
                                Builder.Append(string.Format("{0}	|", row.Col107));
                                Builder.Append(string.Format("{0}	|", row.Col108));
                                Builder.Append(string.Format("{0}	|", row.Col109));
                                Builder.Append(string.Format("{0}	|", row.Col110));
                                Builder.Append(string.Format("{0}	|", row.Col111));
                                Builder.Append(string.Format("{0}	|", row.Col112));
                                Builder.Append(string.Format("{0}	|", row.Col113));
                                Builder.Append(string.Format("{0}	|", row.Col114));
                                Builder.Append(string.Format("{0}	|", row.Col115));
                                Builder.Append(string.Format("{0}	|", row.Col116));
                                Builder.Append(string.Format("{0}	|", row.Col117));
                                Builder.Append(string.Format("{0}	|", row.Col118));
                                Builder.Append(string.Format("{0}	|", row.Col119));
                                Builder.Append(string.Format("{0}	|", row.Col120));
                                Builder.Append(string.Format("{0}	|", row.Col121));
                                Builder.Append(string.Format("{0}	|", row.Col122));
                                Builder.Append(string.Format("{0}	|", row.Col123));
                                Builder.Append(string.Format("{0}	|", row.Col124));
                                Builder.Append(string.Format("{0}	|", row.Col125));
                                Builder.Append(string.Format("{0}	|", row.Col126));
                                Builder.Append(string.Format("{0}	|", row.Col127));
                                Builder.Append(string.Format("{0}	|", row.Col128));
                                Builder.Append(string.Format("{0}	|", row.Col129));
                                Builder.Append(string.Format("{0}	|", row.Col130));
                                Builder.Append(string.Format("{0}	|", row.Col131));
                                Builder.Append(string.Format("{0}	|", row.Col132));
                                Builder.Append(string.Format("{0}	|", row.Col133));
                                Builder.Append(string.Format("{0}	|", row.Col134));
                                Builder.Append(string.Format("{0}	|", row.Col135));
                                Builder.Append(string.Format("{0}	|", row.Col136));
                                Builder.Append(string.Format("{0}	|", row.Col137));
                                Builder.Append(string.Format("{0}	|", row.Col138));
                                Builder.Append(string.Format("{0}	|", row.Col139));
                                Builder.Append(string.Format("{0}	|", row.Col140));
                                Builder.Append(string.Format("{0}	|", row.Col141));
                                Builder.Append(string.Format("{0}	|", row.Col142));
                                Builder.Append(string.Format("{0}	|", row.Col143));
                                Builder.Append(string.Format("{0}	|", row.Col144));
                                Builder.Append(string.Format("{0}	|", row.Col145));
                                Builder.Append(string.Format("{0}	|", row.Col146));
                                Builder.Append(string.Format("{0}	|", row.Col147));
                                Builder.Append(string.Format("{0}	|", row.Col148));
                                Builder.Append(string.Format("{0}	|", row.Col149));
                                Builder.Append(string.Format("{0}	|", row.Col150));
                                Builder.Append(string.Format("{0}	|", row.Col151));
                                Builder.Append(string.Format("{0}	|", row.Col152));
                                Builder.Append(string.Format("{0}	|", row.Col153));
                                Builder.Append(string.Format("{0}	|", row.Col154));
                                Builder.Append(string.Format("{0}	|", row.Col155));
                                Builder.Append(string.Format("{0}	|", row.Col156));
                                Builder.Append(string.Format("{0}	|", row.Col157));
                                Builder.Append(string.Format("{0}	|", row.Col158));
                                Builder.Append(string.Format("{0}	|", row.Col159));
                                Builder.Append(string.Format("{0}	|", row.Col160));
                                Builder.Append(string.Format("{0}	|", row.Col161));
                                Builder.Append(string.Format("{0}	|", row.Col162));
                                Builder.Append(string.Format("{0}	|", row.Col163));
                                Builder.Append(string.Format("{0}	|", row.Col164));
                                Builder.Append(string.Format("{0}	|", row.Col165));
                                Builder.Append(string.Format("{0}	|", row.Col166));
                                Builder.Append(string.Format("{0}	|", row.Col167));
                                Builder.Append(string.Format("{0}	|", row.Col168));
                                Builder.Append(string.Format("{0}	|", row.Col169));
                                Builder.Append(string.Format("{0}	|", row.Col170));
                                Builder.Append(string.Format("{0}	|", row.Col171));
                                Builder.Append("\r\n");

                            }


                        }
                    }
                }
                //else if (InitialHealthcare == "granostic")
                //{
                //    Builder.AppendLine("NAMA|LABORATORIUM|THORAX|MIKROBIOLOGI|FIT/UNFIT|KESIMPULAN|SARAN");
                //    if (lst.Count > 0)
                //    {

                //        List<MCUFormResultFieldReporting> lstData = new List<MCUFormResultFieldReporting>();
                //        foreach (vMCUResultFormReport row in lst)
                //        {
                //            string value = row.FormResult;
                //            ///1^Penyakit Yang pernah diderita=Tidak|3^Pernah Dirawat Di Rumah Sakit=Tidak|5^Berapa lama=-|6^Pernah Operasi=Tidak|8^Pernah Kecelakaan=Tidak|9^Kecelakaan dimana=Tidak Ada|222^Riwayat Alergi=Tidak|11^Merokok=Tidak|13^Alkohol=Tidak|15^Kopi=Tidak|17^Olah Raga=Tidak|230^Napza=Tidak|235^Lain-lain=Tidak|19^Hypertensi=Tidak|21^Diabetes=Tidak|23^Sakit Jantung=Tidak|25^Ginjal=Tidak|27^Ganguan Mental=Tidak|240^Cancer=Tidak|29^Lain2=Tidak|31^Apakah Sedang Menderita Penyakit=Tidak|33^Apakah Sedang Menjalani Pengobatan Tertentu=Tidak|39^Status Gizi=UnderWeight|41^Kulit=Dalam Batas Normal|43^Rambut=Dalam Batas Normal|52^Mana Kanan Konjungtiva=Tidak Hiperemis|62^Sklera=Dalam Batas Normal|63^Pupil=Isokor|64^Buta Warna=Total|65^Bola Mata=Simetris|53^Mata Kanan - Cornea=Keruh|72^Hidung=Dalam Batas Normal|73^Lidah=Dalam Batas Normal|74^Gigi Atas=Baik|82^Gigi Bawah=Baik|90^Pharing=Dalam Batas Normal|91^Pharing Abnormal=-|92^Tonsil=Tidak Hipertrofi|94^Tiroid=Dalam Batas Normal|95^Tiroid Abnormal=-|104^Frekuensi Pernapasan=Dalam Batas Normal|106^Paru paru=Dalam Batas Normal|107^Vesikuer=Dalam Batas Normal|66^Telinga Kanan - Telinga Luar=Serumen Propt (-)|67^Telinga Kanan - Membran Tympani=Utuh|68^Telinga Kanan - Membran Timpany tidak Utuh=-|69^Telinga Kiri - Telinga Luar=Serumen Propt (-)|70^Telinga Kiri - Membran Tympani=Utuh|71^Telinga Kiri - Membran Timpany tidak Utuh=-|100^Hasil Tensi=Dalam Batas Normal|101^Iktrus Kordis=Tidak Teraba|102^Auskultasi=BJ Murni|103^Kesan Batas Jantung=Melebar|112^Inspeksi=Dalam Batas Normal|275^Inspeksi=Dalam Batas Normal|113^Nyeri Tekan=Ada|114^Nyeri Lepas=Ada|115^Hati=Tidak Teraba|277^Hati=Tidak Teraba|116^Limpa=Tidak Teraba|117^Hernia=Tidak|118^Rectal Touche=Tidak Dilakukan|119^Rectal Touche Abnormal=-|120^Ginjal=Dalam Batas Normal|121^Ballotement=Dalam Batas Normal|122^Nyeri Ketok Kanan=Negatif|123^Nyeri Ketok Kiri=Negatif|124^Genital=Dalam Batas Normal|125^Refleks Fisiologis=Positif|126^Refleks Patologis=Negatif|127^Fungsi Motorik=Dalam Batas Normal|128^Fungsi Sensorik=Dalam Batas Normal|129^Tonus Otot=Eutoni|130^Ket Abnormal Tonus Otot=Ischialgia|279^Fungsi Sensorik=Dalam Batas Normal|282^Fungsi Sensorik=Dalam Batas Normal|131^Tengkorak=Dalam Batas Normal|132^Tulang Belakang=Dalam Batas Normal|134^Gerak Atas=Dalam Batas Normal|136^Anggota Gerak Atas=Dalam Batas Normal|138^Leher=Tidak Membesar|139^Axila=Tidak Membesar|140^Inguinal=Tidak Membesar|143^Radiologi=Abnormal|147^EKG=Abnormal|155^Penyakit akibat kerja=Ada|152^Hasil Hipertensi=Ya|153^Hasil Hipertensi Derajat=Hipertensi Derajat II (>= 160/100 mmHg)|154^Penyakit Diabetes=Ya|2^Penyakit apa dan sejak kapan=|4^Pernah Dirawat Penyakit apa dan sejak kapan=|7^Operasi Apa dan kapan=|10^Kecelakan Apa dan Kapan=|224^Alergi Makanan=|226^Alergi Obat=|228^Lain-lain=|12^Jumlah Rokok(Batang/hari)=0|14^Minum Alkohol(kali/Minggu)=0|16^Minum Kopi (Gelas/hari)=0|18^Olahraga(Jam/Satu Minggu)=0|232^Sebutkan=0|285^Sebutkan=0|238^Olahraga(Jam/Satu Minggu)=0|20^Klg yg Hypertensi=|22^Klg yg Diabetes=|24^Klg yg Sakit Jantung=|26^Klg yg Sakit Ginjal=|28^Klg yg Sakit Ganguan Jiwa=|242^Klg yg Sakit Cancer=|30^Klg yg Sakit Lain2/ALergi=|32^Jika Ya, Penyakit Apa=|34^Sudah Berapa Lama=|35^Pengobatan Apa, Terkontrol Atau Tidak=|243^Kesadaran=|245^GCS=|246^Tekanan Darah=|248^Denyut Nadi=|250^Frekuensi Nafas=|252^SpO2=|254^Frekuensi Nafas=|36^Tinggi Badan=|37^Berat Badan=|38^Lingkar Perut=|40^Body Mass Index=|42^Kulit Abnormal=|44^Abnormal Rambut=|216^Visus=|217^Visus=|46^Mata Kanan - Visus + Koreksi Kanan=|56^Mata Kiri - Visus + Koreksi=|47^Mata Kanan - ADD=|57^Visus Kiri - ADD=|259^Visus=|261^Visus=|263^Lensa=|265^Lensa=|267^Lensa=|54^Lain-lain=|0^AutoRef Kanan=|0^AutoRef Kiri=|0^NCT / Tonometri Kanan=|0^NCT / Tonometri Kiri=|0^Kacamata Kanan=|0^Kacamata Kanan=|75^Keterangan-Gigi Atas=|83^Keterangan-Gigi Bawah=|93^Keterangan Tonsil=|269^Keterangan Lain-lain=|105^Nilai Pernapasan=|271^Keterangan Lain-lain=|273^Keterangan Lain-lain=|0^Ket Abnormal Tengkorak=|133^Ket Abnormal Tengkorak=|135^Ket Abnormal Anggota Gerak Atas=|137^Ket Abnormal Anggota Gerak Bawah=|141^Anamnesa=Anamnesis|142^Pemeriksaan Fisik=Pemeriksaan Fisik |144^Radiologi Abnormal=Radiologi |148^Elektro Kardiografi Description=Elektro Kardiografi|149^Audiometri=Audiometri |145^Laboratorium=Laboratorium|220^Laboratorium Mikro=Laboratorium Mikro|159^Lain-lain= Lain-lain |151^Saran=Saran |156^Keterangan Penyakit Akibat Kerja Dicurigai / Ada=Penyakit akibat kerja |161^Kesimpulan=Kesimpulan|0^Penyakit Diabetes Keterangan=Diabetes Mellitus |157^Spirometri=Spirometri |158^Pap Smear= Pap Smear |169^Nama Perusahaan 1=|170^Nama Perusahaan 2=|171^Jenis Pekerjaan 1=|172^Jenis Pekerjaan 2=|173^Faktor Fisika 1=|174^Faktor Fisika 2=|175^Faktor Kimia 1=|176^Faktor Kimia 1=|177^Faktor Biologi 1=|178^Faktor Biologi 2=|179^Faktor Psikologi 1=|180^Faktor Psikologi 2=|181^Faktor Ergonomi 1=|182^Faktor Ergonomi 2=|183^Lama tahun bekerja 1=0|184^Lama Bulan bekerja 1=0|185^Lama tahun bekerja 2=0|186^Lama bulan bekerja 2=0|48^Mata Kanan - Myopia=Tidak|49^Mata Kanan - Hypermetrop=Tidak|50^Mata Kanan - Presbiop=Tidak|51^Mata Kanan - Cilindris=Tidak|58^Mata Kiri - Myopia=Tidak|59^Mata Kiri - Hypermetrop=Tidak|60^Mata Kiri - Presbiop=Tidak|61^Mata Kiri - Cilindris=Tidak|76^Gigi Atas - Radix=Tidak|78^Gigi Atas - Caries=Tidak|80^Gigi Atas - Missing=Tidak|77^Gigi Atas - Abrasi=Tidak|79^Gigi Atas - Impacted=Tidak|81^Gigi Atas - Kalkulus=Tidak|84^Gigi Bawah - Radix=Tidak|86^Gigi Bawah - Caries=Tidak|88^Gigi Bawah - Missing=Tidak|85^Gigi Bawah - Abrasi=Tidak|87^Gigi Bawah - Impacted=Tidak|89^Gigi Bawah - Kalkulus=Tidak|187^Fisika Pencahayaan=Tidak|188^Fisika Bising=Tidak|189^Fisika Getaran=Tidak|190^Fisika Suhu=Tidak|191^Kimia Partike (Debu/Asap)=Tidak|192^Kimia Cairan=Tidak|193^Kimia Gas/Uap=Tidak|194^Biologi Bakteri=Tidak|195^Biologi Virus=Tidak|196^Biologi Jamur=Tidak|197^Ergonomi Gerak Repetitif/ Berulang=Tidak|198^Ergonomi Angkat Beban Berat=Tidak|199^Ergonomi Akward Posture=Tidak|200^Ergonomi Mata Lelah=Tidak|201^Psikologi Stress=Tidak|202^Psikologi Shift=Tidak|||||||||||||||150^Kriteria Hiperkes=Fit For Work|||||

                //            string[] data = value.Split('|');
                //            if (data.Length > 0)
                //            {
                //                MCUFormResultFieldReporting oData = new MCUFormResultFieldReporting();
                //                oData.TransactionNo = row.RegistrationNo;
                //                oData.RegistrasiNo = row.RegistrationNo;
                //                oData.TanggalPemeriksaan = oData.TglMasuk = row.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT_1);

                //                if (row.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                //                {
                //                    oData.TglLahir = row.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_1);
                //                }
                //                else
                //                {
                //                    oData.TglLahir = "";
                //                }

                //                oData.JnsKelamin = row.Gender;
                //                oData.Lokasi = filterCsvData(row.CorporateAccountDepartment, ",");
                //                oData.NamaPegawai = filterCsvData(row.PatientName, ",");
                //                oData.NoPegawai = row.CorporateAccountNo;
                //                oData.ParamedicCode = row.ParamedicCode;
                //                oData.ParamedicName = filterCsvData(row.ParamedicName, ",");
                //                oData.Penjamin = filterCsvData(row.BusinessPartnerName, ",");
                //                oData.Posisi = filterCsvData(row.CorporateAccountDepartment, ",");

                //                for (int i = 0; i < data.Length; i++)
                //                {

                //                    string[] field = data[i].Split('^');
                //                    if (field.Length > 1)
                //                    {
                //                        string[] dataField = field[1].Split('=');
                //                        string dataFieldValue = dataField[1];
                //                        if (dataField.Length > 2)
                //                        {
                //                            for (int indexField = 0; indexField < dataField.Length; indexField++)
                //                            {
                //                                if (i > 0)
                //                                {
                //                                    dataFieldValue += string.Format("{0}=", dataField[indexField]);
                //                                }
                //                            }
                //                            if (!string.IsNullOrEmpty(dataFieldValue))
                //                            {
                //                                dataFieldValue = dataFieldValue.Remove(dataFieldValue.Length - 1);
                //                                ///dataFieldValue = filterCsvData(dataFieldValue, ",");
                //                            }
                //                        }

                //                        #region filter


                //                        dataFieldValue = filterCsvData(dataFieldValue, ",");
                //                        switch (field[0])
                //                        {
                //                            case "144": //RADIOLOGI
                //                                oData.Col144 = dataFieldValue; //6
                //                                break;
                //                            case "145": //Laboratorium
                //                                oData.Col145 = dataFieldValue; //6
                //                                break;
                //                            case "220": //Laboratorium Mikro
                //                                oData.Col220 = dataFieldValue; //7
                //                                break;
                //                            case "150": //Kriteria Hiperkes
                //                                oData.Col150 = dataFieldValue; //8
                //                                break;
                //                            case "151": // SARAN
                //                                oData.Col151 = dataFieldValue; //9
                //                                break;
                //                            case "161": //KESIMPULAN
                //                                oData.Col161 = dataFieldValue; //10
                //                                break;

                //                        }
                //                        #endregion
                //                    }
                //                }
                //                lstData.Add(oData);
                //            }
                //        }

                //        if (lstData.Count > 0)
                //        {


                //            foreach (MCUFormResultFieldReporting row in lstData)
                //            {
                //                Builder.Append(string.Format("{0}|", row.NamaPegawai));  //5
                //                Builder.Append(string.Format("{0}|", row.Col220));  //5
                //                Builder.Append(string.Format("{0}|", row.Col144));  //5
                //                Builder.Append(string.Format("{0}|", row.Col220));  //5
                //                Builder.Append(string.Format("{0}|", row.Col150));  //5
                //                Builder.Append(string.Format("{0}|", row.Col161));  //5
                //                Builder.Append(string.Format("{0}|", row.Col151));  //5

                //                Builder.Append("\r\n");

                //            }


                //        }
                //    }
                //}
                else
                {

                    Builder.AppendLine("No Bukti Transaksi,TglMasuk,No.Registrasi,No.Pegawai,Penjamin,Posisi,Lokasi,Nama Pegawai,Tanggal Lahir,Jns Kelamin,TglPemeriksaan,Kode Dr,Nama Dokter,Pernah Mengindap Penyakit,Penyakit Apa dan Kapan,Pernah dirawat di RS,Sakit Apa dan Kapan,Lama dirawat,Pernah Operasi,Operasi Apa dan Kapan,Pernah Kecelakaan,Kecelakaan dimana,Kecelakaan apa dan kapan,Merokok,Jumlah Rokok(Batang),Alkohol,Minum Alkohol(Kali),Kopi,Minum Kopi(Gelas),Olah Raga,Lama Olahraga(Jam),Hipertensi,Klg yg hipertensi,Diabetes,Klg yg diabetes,Sakit Jantung,Klg yg Sakit Jantung,Sakit Ginjal,Klg yg Sakit Ginjal,Gangguann Mental,Klg yg Sakit Mental,Lain2/ALergi,Klg yg Alergi/lain2,Apa sedang menderita penyakit?,Jika Ya Penyakitnya apa,Sedang menjalani pengobatan tertentu.?,Sudah berapa lama,Pengobatan apa ? Terkontrol atau tidak,TB (cm),BB (Kg),Anjuran BB (Kg),Status Gizi,BMI,Kulit,Kulit Abnormal,Rambut,Abnormal Rambut,Mata Kanan - Visus,Mata Kanan -Visus+Koreksi,Mata Kanan-Add,Mata Kanan - Myopia,Mata Kanan - Hypermetrop,Mata Kanan - Persbiop,Mata Kanan - Cilindris,Mata Kanan - Konjungtiva,Mata Kanan - Cornea,Refleks Cahaya,Mata Kiri - Visus,Mata Kiri -Visus+Koreksi,Visus Kiri - Add,Mata Kiri - Myopia,Mata Kiri - Hypermetrop,Mata Kiri - Presbiop,Mata Kiri - Cilindris,Sklera,Pupil,Buta Warna,Bola Mata,Telinga Kanan - Telinga Luar,Telinga Kanan - Membran Timpany,Telinga Kanan - Membran Timpany tidak Utuh,Telinga Kiri - Telinga Luar,Telinga Kiri - Membran Timpany,Telinga Kiri - Membran Timpany tidak Utuh,Hidung,Lidah,Gigi Atas,Keterangan Gigi Atas,Gigi Atas - Radix,Gigi Atas - Abrasi,Gigi Atas - Caries,Gigi Atas - Impacted,Gigi Atas - Missing,Gigi Atas - Kalkulus,Gigi Bawah,Keterangan Gigi bawah,Gigi Bawah - Radix,Gigi Bawah - Abrasi,Gigi Bawah - Caries,Gigi Bawah - Impacted,Gigi Bawah - Missing,Gigi Bawah - Kalkulus,Pharing,Pharing Abnormal,Tonsil,Keterangan Tonsil,Tiroid,Tiroid Abnormal,Tekanan Darah (mm/Hg),Frequensi Nadi,Irama,Irama Kardiovaskular,Hasil Tensi,Iktrus Kordis,Auskultasi,Kesan Batas Jantung,Frekuensi Pernapasan,Nilai Pernafasan,Paru-paru,Vesikuer,Ronchi,Wheezing,Perkusi Kanan,Perkusi Kiri,Inspeksi,Nyeri Tekan,Nyeri Lepas,Hati,Limpa,Hernia,Rectal Toucher,Rectal Toucher Abnormal,Ginjal,Ballotemen,Nyeri Ketok Kanan,Nyeri Ketok Kiri,Genital,Repleks Fisiologis,Repleks Patologis,Fungsi Motorik,Fungsi Sensorik,Tonus Otot,Ket Abnormal Tonus Otot,Tenggorok,Tulang Belakang,Ket Abnormal Tulang Belakang,Gerak Atas,Ket Abnormal Gerak Atas,Gerak Bawah,Ket Abnormal Gerak Bawah,Leher,Axila,Inguinal,Anamnesa,Pemeriksaan Fisik,Radiologi,Radiologi Abnormal,Laboratorium,ECG,EKG,EKG Abnormal,Audiometri,Kriteria Hiperkes,Saran,Hasil Hipertensi,Hasil Hipertensi Derajat,Penyakit Diabetes,Penyakit Akibat Kerja,Keterangan Penyakit Akibat Kerja Dicurigai / Ada,Spirometri,Pap Smear");
                    if (lst.Count > 0)
                    {

                        List<MCUFormResultFieldReporting> lstData = new List<MCUFormResultFieldReporting>();
                        foreach (vMCUResultFormReport row in lst)
                        {
                            string value = row.FormResult;
                            ///"1^Penyakit Yang pernah diderita=Tidak|3^Pernah Dirawat Di Rumah Sakit=Tidak|5^Berapa lama=-|6^Pernah Operasi Apa=Tidak|8^Pernah Kecelakaan=Tidak|9^Kecelakaan dimana=Tidak Ada|11^Merokok=Tidak|13^Alkohol=Tidak|15^Kopi=Tidak|17^Olah Raga=Tidak|19^Hypertensi=Tidak|21^Diabetes=Tidak|23^Sakit Jantung=Tidak|25^Ginjal=Tidak|27^Ganguan Mental=Tidak|29^Lain2/ALergi=Tidak|31^Apakah Sedang Menderita Penyakit=Tidak|33^Apakah Sedang Menjalani Pengobatan=Tidak|39^Status Gizi=Normal|41^Kulit=Normal|43^Rambut=Normal|45^Visus Kanan=Abnormal|46^Visus Kiri=Abnormal|59^Konjungtiva=Tidak Hiperemis|60^Skelera=Normal|61^Pupil=Isokor|62^Buta Warna=Total|63^Bola Mata=Simetris|64^Cornea=Keruh|72^Hidung=Normal|73^Lidah=Normal|74^Gigi Atas=Baik|78^Gigi Bawah=Baik|82^Pharing=Normal|83^Pharing Abnormal=-|84^Tonsil=Tidak Hypertropi|86^Tiroid=Normal|87^Tiroid Abnormal=-|88^Frekuensi Pernapasan=Normal|90^Paru paru=Normal|100^Vesikuer=Normal|105^Telinga Kanan - Telinga Luar=Serumen Propt (-)|106^Telinga Luar - Membran Tympani=Utuh|107^Telinga Luar - Membran Timpany tidak Utuh=-|108^Telinga Kiri - Telinga Luar=Serumen Propt (-)|109^Telinga Kiri - Membran Tympani=Utuh|110^Telinga Kiri - Membran Timpany tidak Utuh=-|113^Irama=Reguler|115^Hasil Tensi=Normal|116^Iktrus Kordis=Tidak Teraba|117^Auskultasi=BJ Murni|118^Kesan Batas Jantung=Melebar|119^Inspeksi=Normal|120^Nyeri Tekan=Ada|121^Nyeri Lepas=Ada|122^Hati=Tidak Teraba|123^Limpa=Tidak Teraba|124^Hernia=Tidak|125^Rectal Touche=Tidak Dilakukan|126^null=-|127^Ginjal=Normal|128^Ballotement=Normal|129^Nyeri Ketok Kanan=Negatif|130^Nyeri Ketok Kiri=Negatif|131^Genital=1|132^Refleks Fisiologis=Positif|133^Refleks Patologis=Negatif|134^Fungsi Motorik=Normal|135^Fungsi Sensorik=Normal|136^Tonus Otot=Eutoni|137^Ket Abnormal Tonus Otot=Ischialgia|140^Tulang Belakang=Normal|142^Anggota Gerak Atas=Normal|144^Anggota Gerak Atas=Normal|146^Leher=Tidak Membesar|147^Axila=Tidak Membesar|148^Inguinal=Tidak Membesar|151^Radiologi=Tidak Dilakukan|153^EKG=Tidak Dilakukan|159^Penyakit akibat kerja=Tidak|163^Hasil Hipertensi=Tidak|164^Hasil Hipertensi Derajat=-|165^Penyakit Diabetes=-|2^Penyakit apa dan sejak kapan=|4^Pernah Dirawat Penyakit apa dan sejak kapan=|7^Operasi Apa dan kapan=|10^Kecelakan Apa dan Kapan=|12^Jumlah Rokok(Batang/hari)=0|14^Alkohol(kali/Minggu)=0|16^Minum Kopi (Gelas/hari)=0|18^Olahraga(Jam/Satu Minggu)=0|20^Klg yg Hypertensi=|22^Klg yg Diabetes=|24^Klg yg Sakit Jantung=|26^Klg yg Sakit Ginjal=|28^Klg yg Sakit Ganguan Jiwa=|30^Klg yg Sakit Lain2/ALergi=|32^Jika Ya, Penyakit Apa=|34^Sudah Berapa Lama=|35^Pengobatan Apa Dan Apakah Terkontrol=|36^Tinggi Badan=180|37^Berat Badan=80|38^Anjuran Berat Badan=80|40^Body Mass Index=25.00|42^null=|44^keterangan Abnormal=|47^Visus + Koreksi Kanan=|48^Visus + Koreksi Kiri=|49^Add Visus Kanan=|50^Add Visus Kiri=|65^Lain-lain=|66^AutoRef Kanan=|67^AutoRef Kiri=|68^NCT / Tonometri Kanan=|69^NCT / Tonometri Kiri=|70^Kacamata Kanan=|71^Kacamata Kanan=|75^Gigi Atas - Keterangan=|79^Gigi Bawah - Keterangan=|85^Keterangan Tonsil=|89^Nilai Pernapasan=|111^Tekanan Darah (mm/Hg)=|112^Frequensi Nadi=|114^Irama Kardiovaskular=|139^Ket Abnormal Tengkorak=|141^Ket Abnormal Tengkorak=|143^Ket Abnormal Anggota Gerak Atas=|145^Ket Abnormal Anggota Gerak Atas=|149^Anamnesa=|150^Pemeriksaan Fisik=|152^Radiologi Abnormal=|154^Elektro Kardiografi Description=|155^Audiometri=|156^Laboratorium=|157^Lain-lain=|158^Saran=|160^Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|161^Kesimpulan=|166^Penyakit Diabetes Keterangan=|167^Spirometri=|168^Pap Smear=|169^Nama Perusahaan 1=|170^Nama Perusahaan 2=|171^Jenis Pekerjaan 1=|172^Jenis Pekerjaan 2=|173^Faktor Fisika 1=|174^Faktor Fisika 2=|175^Faktor Kimia 1=|176^Faktor Kimia 1=|177^Faktor Biologi 1=|178^Faktor Biologi 2=|179^Faktor Psikologi 1=|180^Faktor Psikologi 2=|181^Faktor Ergonomi 1=|182^Faktor Ergonomi 2=|183^Lama tahun bekerja 1=0|184^Lama Bulan bekerja 1=0|185^Lama tahun bekerja 2=0|186^Lama bulan bekerja 2=0|51^Mata Kanan - Myopia=Ya|52^Mata Kanan - Hypermetrop=Ya|53^Mata Kanan - Presbiop=Ya|54^Mata Kanan - Cilindris=Ya|55^Mata Kiri - Myopia=|56^Mata Kiri - Hypermetrop=|57^Mata Kiri - Presbiop=|58^Mata Kiri - Cilindris=|76^Gigi Atas - Radix=Ya|76_01^Gigi Atas - Caries=|76_02^Gigi Atas - Missing=|77^Gigi Atas - Abrasi=|77_01^Gigi Atas - Impacted=Ya|77_02^Gigi Atas - Kalkulus=Ya|80^Gigi Bawah - Radix=|80_01^Gigi Bawah - Caries=|80_02^Gigi Bawah - Missing=|81^Gigi Bawah - Abrasi=Ya|80_02^Gigi Bawah - Impacted=|80_03^Gigi Bawah - Kalkulus=|187^Fisika Pencahayaan=|188^Fisika Bising=|189^Fisika Getaran=|190^Fisika Suhu=|191^Kimia Partike (Debu/Asap)=|192^Kimia Cairan=|193^Kimia Gas/Uap=|194^Biologi Bakteri=|195^Biologi Virus=|196^Biologi Jamur=|197^Ergonomi Gerak Repetitif/ Berulang=|198^Ergonomi Angkat Beban Berat=|199^Ergonomi Akward Posture=|200^Ergonomi Mata Lelah=|201^Psikologi Stress=|202^Psikologi Shift=|101^Ronchi=|101^Ronchi=Ya|101^Ronchi=|101^Ronchi=|102^Wheezing=|102^Wheezing=|102^Wheezing=|102^Wheezing=|103^Perkusi Kanan=Ya|103^Perkusi Kanan=|103^Perkusi Kanan=|104^Perkusi Kiri=Ya|104^Perkusi Kiri=|104^Perkusi Kiri=|162^Kriteria Hiperkes=|162^Kriteria Hiperkes=Ya|162^Kriteria Hiperkes=|162^Kriteria Hiperkes=|162^Kriteria Hiperkes=|162^Kriteria Hiperkes=";
                            string[] data = value.Split('|');
                            if (data.Length > 0)
                            {
                                MCUFormResultFieldReporting oData = new MCUFormResultFieldReporting();
                                oData.TransactionNo = row.RegistrationNo;
                                oData.RegistrasiNo = row.RegistrationNo;
                                oData.TanggalPemeriksaan = oData.TglMasuk = row.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT_1);

                                if (row.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                                {
                                    oData.TglLahir = row.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_1);
                                }
                                else
                                {
                                    oData.TglLahir = "";
                                }

                                oData.JnsKelamin = row.Gender;
                                oData.Lokasi = filterCsvData(row.CorporateAccountDepartment, ",");
                                oData.NamaPegawai = filterCsvData(row.PatientName, ",");
                                oData.NoPegawai = row.CorporateAccountNo;
                                oData.ParamedicCode = row.ParamedicCode;
                                oData.ParamedicName = filterCsvData(row.ParamedicName, ",");
                                oData.Penjamin = filterCsvData(row.BusinessPartnerName, ",");
                                oData.Posisi = filterCsvData(row.CorporateAccountDepartment, ",");

                                for (int i = 0; i < data.Length; i++)
                                {

                                    string[] field = data[i].Split('^');
                                    if (field.Length > 1)
                                    {
                                        string[] dataField = field[1].Split('=');
                                        string dataFieldValue = dataField[1];
                                        if (dataField.Length > 2)
                                        {
                                            for (int indexField = 0; indexField < dataField.Length; indexField++)
                                            {
                                                if (i > 0)
                                                {
                                                    dataFieldValue += string.Format("{0}=", dataField[indexField]);
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(dataFieldValue))
                                            {
                                                dataFieldValue = dataFieldValue.Remove(dataFieldValue.Length - 1);
                                                ///dataFieldValue = filterCsvData(dataFieldValue, ",");
                                            }
                                        }

                                        #region filter
                                        dataFieldValue = filterCsvData(dataFieldValue, ",");
                                        switch (field[0])
                                        {
                                            case "1":
                                                oData.Col1 = dataFieldValue;
                                                break;
                                            case "2":
                                                oData.Col2 = dataFieldValue;
                                                break;
                                            case "3":
                                                oData.Col3 = dataFieldValue;
                                                break;
                                            case "4":
                                                oData.Col4 = dataFieldValue;
                                                break;
                                            case "5":
                                                oData.Col5 = dataFieldValue;
                                                break;
                                            case "6":
                                                oData.Col6 = dataFieldValue;
                                                break;
                                            case "7":
                                                oData.Col7 = dataFieldValue;
                                                break;
                                            case "8":
                                                oData.Col8 = dataFieldValue;
                                                break;
                                            case "9":
                                                oData.Col9 = dataFieldValue;
                                                break;
                                            case "10":
                                                oData.Col10 = dataFieldValue;
                                                break;
                                            case "11":
                                                oData.Col11 = dataFieldValue;
                                                break;
                                            case "12":
                                                oData.Col12 = dataFieldValue;
                                                break;
                                            case "13":
                                                oData.Col13 = dataFieldValue;
                                                break;
                                            case "14":
                                                oData.Col14 = dataFieldValue;
                                                break;
                                            case "15":
                                                oData.Col15 = dataFieldValue;
                                                break;
                                            case "16":
                                                oData.Col16 = dataFieldValue;
                                                break;
                                            case "17":
                                                oData.Col17 = dataFieldValue;
                                                break;
                                            case "18":
                                                oData.Col18 = dataFieldValue;
                                                break;
                                            case "19":
                                                oData.Col19 = dataFieldValue;
                                                break;
                                            case "20":
                                                oData.Col20 = dataFieldValue;
                                                break;
                                            case "21":
                                                oData.Col21 = dataFieldValue;
                                                break;
                                            case "22":
                                                oData.Col22 = dataFieldValue;
                                                break;
                                            case "23":
                                                oData.Col23 = dataFieldValue;
                                                break;
                                            case "24":
                                                oData.Col24 = dataFieldValue;
                                                break;
                                            case "25":
                                                oData.Col25 = dataFieldValue;
                                                break;
                                            case "26":
                                                oData.Col26 = dataFieldValue;
                                                break;
                                            case "27":
                                                oData.Col27 = dataFieldValue;
                                                break;
                                            case "28":
                                                oData.Col28 = dataFieldValue;
                                                break;
                                            case "29":
                                                oData.Col29 = dataFieldValue;
                                                break;
                                            case "30":
                                                oData.Col30 = dataFieldValue;
                                                break;
                                            case "31":
                                                oData.Col31 = dataFieldValue;
                                                break;
                                            case "32":
                                                oData.Col32 = dataFieldValue;
                                                break;
                                            case "33":
                                                oData.Col33 = dataFieldValue;
                                                break;
                                            case "34":
                                                oData.Col34 = dataFieldValue;
                                                break;
                                            case "35":
                                                oData.Col35 = dataFieldValue;
                                                break;
                                            case "36":
                                                oData.Col36 = dataFieldValue;
                                                break;
                                            case "37":
                                                oData.Col37 = dataFieldValue;
                                                break;
                                            case "38":
                                                oData.Col38 = dataFieldValue;
                                                break;
                                            case "39":
                                                oData.Col39 = dataFieldValue;
                                                break;
                                            case "40":
                                                oData.Col40 = dataFieldValue;
                                                break;
                                            case "41":
                                                oData.Col41 = dataFieldValue;
                                                break;
                                            case "42":
                                                oData.Col42 = dataFieldValue;
                                                break;
                                            case "43":
                                                oData.Col43 = dataFieldValue;
                                                break;
                                            case "44":
                                                oData.Col44 = dataFieldValue;
                                                break;
                                            case "45":
                                                oData.Col45 = dataFieldValue;
                                                break;
                                            case "46":
                                                oData.Col46 = dataFieldValue;
                                                break;
                                            case "47":
                                                oData.Col47 = dataFieldValue;
                                                break;
                                            case "48":
                                                oData.Col48 = dataFieldValue;
                                                break;
                                            case "49":
                                                oData.Col49 = dataFieldValue;
                                                break;
                                            case "50":
                                                oData.Col50 = dataFieldValue;
                                                break;
                                            case "51":
                                                oData.Col51 = dataFieldValue;
                                                break;
                                            case "52":
                                                oData.Col52 = dataFieldValue;
                                                break;
                                            case "53":
                                                oData.Col53 = dataFieldValue;
                                                break;
                                            case "54":
                                                oData.Col54 = dataFieldValue;
                                                break;
                                            case "55":
                                                oData.Col55 = dataFieldValue;
                                                break;
                                            case "56":
                                                oData.Col56 = dataFieldValue;
                                                break;
                                            case "57":
                                                oData.Col57 = dataFieldValue;
                                                break;
                                            case "58":
                                                oData.Col58 = dataFieldValue;
                                                break;
                                            case "59":
                                                oData.Col59 = dataFieldValue;
                                                break;
                                            case "60":
                                                oData.Col60 = dataFieldValue;
                                                break;
                                            case "61":
                                                oData.Col61 = dataFieldValue;
                                                break;
                                            case "62":
                                                oData.Col62 = dataFieldValue;
                                                break;
                                            case "63":
                                                oData.Col63 = dataFieldValue;
                                                break;
                                            case "64":
                                                oData.Col64 = dataFieldValue;
                                                break;
                                            case "65":
                                                oData.Col65 = dataFieldValue;
                                                break;
                                            case "66":
                                                oData.Col66 = dataFieldValue;
                                                break;
                                            case "67":
                                                oData.Col67 = dataFieldValue;
                                                break;
                                            case "68":
                                                oData.Col68 = dataFieldValue;
                                                break;
                                            case "69":
                                                oData.Col69 = dataFieldValue;
                                                break;
                                            case "70":
                                                oData.Col70 = dataFieldValue;
                                                break;
                                            case "71":
                                                oData.Col71 = dataFieldValue;
                                                break;
                                            case "72":
                                                oData.Col72 = dataFieldValue;
                                                break;
                                            case "73":
                                                oData.Col73 = dataFieldValue;
                                                break;
                                            case "74":
                                                oData.Col74 = dataFieldValue;
                                                break;
                                            case "75":
                                                oData.Col75 = dataFieldValue;
                                                break;
                                            case "76":
                                                oData.Col76 = dataFieldValue;
                                                break;
                                            case "77":
                                                oData.Col77 = dataFieldValue;
                                                break;
                                            case "78":
                                                oData.Col78 = dataFieldValue;
                                                break;
                                            case "79":
                                                oData.Col79 = dataFieldValue;
                                                break;
                                            case "80":
                                                oData.Col80 = dataFieldValue;
                                                break;
                                            case "81":
                                                oData.Col81 = dataFieldValue;
                                                break;
                                            case "82":
                                                oData.Col82 = dataFieldValue;
                                                break;
                                            case "83":
                                                oData.Col83 = dataFieldValue;
                                                break;
                                            case "84":
                                                oData.Col84 = dataFieldValue;
                                                break;
                                            case "85":
                                                oData.Col85 = dataFieldValue;
                                                break;
                                            case "86":
                                                oData.Col86 = dataFieldValue;
                                                break;
                                            case "87":
                                                oData.Col87 = dataFieldValue;
                                                break;
                                            case "88":
                                                oData.Col88 = dataFieldValue;
                                                break;
                                            case "89":
                                                oData.Col89 = dataFieldValue;
                                                break;
                                            case "90":
                                                oData.Col90 = dataFieldValue;
                                                break;
                                            case "91":
                                                oData.Col91 = dataFieldValue;
                                                break;
                                            case "92":
                                                oData.Col92 = dataFieldValue;
                                                break;
                                            case "93":
                                                oData.Col93 = dataFieldValue;
                                                break;
                                            case "94":
                                                oData.Col94 = dataFieldValue;
                                                break;
                                            case "95":
                                                oData.Col95 = dataFieldValue;
                                                break;
                                            case "96":
                                                oData.Col96 = dataFieldValue;
                                                break;
                                            case "97":
                                                oData.Col97 = dataFieldValue;
                                                break;
                                            case "98":
                                                oData.Col98 = dataFieldValue;
                                                break;
                                            case "99":
                                                oData.Col99 = dataFieldValue;
                                                break;
                                            case "100":
                                                oData.Col100 = dataFieldValue;
                                                break;
                                            case "101":
                                                oData.Col101 = dataFieldValue;
                                                break;
                                            case "102":
                                                oData.Col102 = dataFieldValue;
                                                break;
                                            case "103":
                                                oData.Col103 = dataFieldValue;
                                                break;
                                            case "104":
                                                oData.Col104 = dataFieldValue;
                                                break;
                                            case "105":
                                                oData.Col105 = dataFieldValue;
                                                break;
                                            case "106":
                                                oData.Col106 = dataFieldValue;
                                                break;
                                            case "107":
                                                oData.Col107 = dataFieldValue;
                                                break;
                                            case "108":
                                                oData.Col108 = dataFieldValue;
                                                break;
                                            case "109":
                                                oData.Col109 = dataFieldValue;
                                                break;
                                            case "110":
                                                oData.Col110 = dataFieldValue;
                                                break;
                                            case "111":
                                                oData.Col111 = dataFieldValue;
                                                break;
                                            case "112":
                                                oData.Col112 = dataFieldValue;
                                                break;
                                            case "113":
                                                oData.Col113 = dataFieldValue;
                                                break;
                                            case "114":
                                                oData.Col114 = dataFieldValue;
                                                break;
                                            case "115":
                                                oData.Col115 = dataFieldValue;
                                                break;
                                            case "116":
                                                oData.Col116 = dataFieldValue;
                                                break;
                                            case "117":
                                                oData.Col117 = dataFieldValue;
                                                break;
                                            case "118":
                                                oData.Col118 = dataFieldValue;
                                                break;
                                            case "119":
                                                oData.Col119 = dataFieldValue;
                                                break;
                                            case "120":
                                                oData.Col120 = dataFieldValue;
                                                break;
                                            case "121":
                                                oData.Col121 = dataFieldValue;
                                                break;
                                            case "122":
                                                oData.Col122 = dataFieldValue;
                                                break;
                                            case "123":
                                                oData.Col123 = dataFieldValue;
                                                break;
                                            case "124":
                                                oData.Col124 = dataFieldValue;
                                                break;
                                            case "125":
                                                oData.Col125 = dataFieldValue;
                                                break;
                                            case "126":
                                                oData.Col126 = dataFieldValue;
                                                break;
                                            case "127":
                                                oData.Col127 = dataFieldValue;
                                                break;
                                            case "128":
                                                oData.Col128 = dataFieldValue;
                                                break;
                                            case "129":
                                                oData.Col129 = dataFieldValue;
                                                break;
                                            case "130":
                                                oData.Col130 = dataFieldValue;
                                                break;
                                            case "131":
                                                oData.Col131 = dataFieldValue;
                                                break;
                                            case "132":
                                                oData.Col132 = dataFieldValue;
                                                break;
                                            case "133":
                                                oData.Col133 = dataFieldValue;
                                                break;
                                            case "134":
                                                oData.Col134 = dataFieldValue;
                                                break;
                                            case "135":
                                                oData.Col135 = dataFieldValue;
                                                break;
                                            case "136":
                                                oData.Col136 = dataFieldValue;
                                                break;
                                            case "137":
                                                oData.Col137 = dataFieldValue;
                                                break;
                                            case "138":
                                                oData.Col138 = dataFieldValue;
                                                break;
                                            case "139":
                                                oData.Col139 = dataFieldValue;
                                                break;
                                            case "140":
                                                oData.Col140 = dataFieldValue;
                                                break;
                                            case "141":
                                                oData.Col141 = dataFieldValue;
                                                break;
                                            case "142":
                                                oData.Col142 = dataFieldValue;
                                                break;
                                            case "143":
                                                oData.Col143 = dataFieldValue;
                                                break;
                                            case "144":
                                                oData.Col144 = dataFieldValue;
                                                break;
                                            case "145":
                                                oData.Col145 = dataFieldValue;
                                                break;
                                            case "146":
                                                oData.Col146 = dataFieldValue;
                                                break;
                                            case "147":
                                                oData.Col147 = dataFieldValue;
                                                break;
                                            case "148":
                                                oData.Col148 = dataFieldValue;
                                                break;
                                            case "149":
                                                oData.Col149 = dataFieldValue;
                                                break;
                                            case "150":
                                                oData.Col150 = dataFieldValue;
                                                break;
                                            case "151":
                                                oData.Col151 = dataFieldValue;
                                                break;
                                            case "152":
                                                oData.Col152 = dataFieldValue;
                                                break;
                                            case "153":
                                                oData.Col153 = dataFieldValue;
                                                break;
                                            case "154":
                                                oData.Col154 = dataFieldValue;
                                                break;
                                            case "155":
                                                oData.Col155 = dataFieldValue;
                                                break;
                                            case "156":
                                                oData.Col156 = dataFieldValue;
                                                break;
                                            case "157":
                                                oData.Col157 = dataFieldValue;
                                                break;
                                            case "158":
                                                oData.Col158 = dataFieldValue;
                                                break;
                                            case "159":
                                                oData.Col159 = dataFieldValue;
                                                break;
                                            case "160":
                                                oData.Col160 = dataFieldValue;
                                                break;
                                            case "161":
                                                oData.Col161 = dataFieldValue;
                                                break;
                                            case "162":
                                                oData.Col162 = dataFieldValue;
                                                break;
                                            case "163":
                                                oData.Col163 = dataFieldValue;
                                                break;
                                            case "164":
                                                oData.Col164 = dataFieldValue;
                                                break;
                                            case "165":
                                                oData.Col165 = dataFieldValue;
                                                break;
                                            case "166":
                                                oData.Col166 = dataFieldValue;
                                                break;
                                            case "167":
                                                oData.Col168 = dataFieldValue;
                                                break;
                                            case "168":
                                                oData.Col168 = dataFieldValue;
                                                break;
                                            case "169":
                                                oData.Col169 = dataFieldValue;
                                                break;
                                            case "170":
                                                oData.Col170 = dataFieldValue;
                                                break;
                                            case "171":
                                                oData.Col171 = dataFieldValue;
                                                break;
                                            case "172":
                                                oData.Col172 = dataFieldValue;
                                                break;
                                            case "173":
                                                oData.Col173 = dataFieldValue;
                                                break;
                                            case "174":
                                                oData.Col174 = dataFieldValue;
                                                break;
                                            case "175":
                                                oData.Col175 = dataFieldValue;
                                                break;
                                            case "176":
                                                oData.Col176 = dataFieldValue;
                                                break;
                                            case "177":
                                                oData.Col177 = dataFieldValue;
                                                break;
                                            case "178":
                                                oData.Col178 = dataFieldValue;
                                                break;
                                            case "179":
                                                oData.Col179 = dataFieldValue;
                                                break;
                                            case "180":
                                                oData.Col180 = dataFieldValue;
                                                break;
                                            case "181":
                                                oData.Col181 = dataFieldValue;
                                                break;
                                            case "182":
                                                oData.Col182 = dataFieldValue;
                                                break;
                                            case "183":
                                                oData.Col183 = dataFieldValue;
                                                break;
                                            case "184":
                                                oData.Col184 = dataFieldValue;
                                                break;
                                            case "185":
                                                oData.Col185 = dataFieldValue;
                                                break;
                                            case "186":
                                                oData.Col186 = dataFieldValue;
                                                break;
                                            case "187":
                                                oData.Col187 = dataFieldValue;
                                                break;
                                            case "188":
                                                oData.Col188 = dataFieldValue;
                                                break;
                                            case "189":
                                                oData.Col189 = dataFieldValue;
                                                break;
                                            case "190":
                                                oData.Col190 = dataFieldValue;
                                                break;
                                            case "191":
                                                oData.Col191 = dataFieldValue;
                                                break;
                                            case "192":
                                                oData.Col192 = dataFieldValue;
                                                break;
                                            case "193":
                                                oData.Col193 = dataFieldValue;
                                                break;
                                            case "194":
                                                oData.Col194 = dataFieldValue;
                                                break;
                                            case "195":
                                                oData.Col195 = dataFieldValue;
                                                break;
                                            case "196":
                                                oData.Col196 = dataFieldValue;
                                                break;
                                            case "197":
                                                oData.Col197 = dataFieldValue;
                                                break;
                                            case "198":
                                                oData.Col198 = dataFieldValue;
                                                break;
                                            case "199":
                                                oData.Col199 = dataFieldValue;
                                                break;
                                            case "200":
                                                oData.Col200 = dataFieldValue;
                                                break;
                                            case "201":
                                                oData.Col201 = dataFieldValue;
                                                break;
                                            case "202":
                                                oData.Col202 = dataFieldValue;
                                                break;
                                            case "203":
                                                oData.Col203 = dataFieldValue;
                                                break;
                                            case "204":
                                                oData.Col204 = dataFieldValue;
                                                break;
                                            case "205":
                                                oData.Col205 = dataFieldValue;
                                                break;
                                            case "206":
                                                oData.Col206 = dataFieldValue;
                                                break;
                                            case "207":
                                                oData.Col207 = dataFieldValue;
                                                break;
                                            case "208":
                                                oData.Col208 = dataFieldValue;
                                                break;
                                            case "209":
                                                oData.Col209 = dataFieldValue;
                                                break;
                                            case "210":
                                                oData.Col210 = dataFieldValue;
                                                break;
                                            case "211":
                                                oData.Col211 = dataFieldValue;
                                                break;
                                            case "212":
                                                oData.Col212 = dataFieldValue;
                                                break;
                                            case "213":
                                                oData.Col213 = dataFieldValue;
                                                break;
                                            case "214":
                                                oData.Col214 = dataFieldValue;
                                                break;
                                            case "215":
                                                oData.Col215 = dataFieldValue;
                                                break;
                                        }
                                        #endregion
                                    }
                                }
                                lstData.Add(oData);
                            }
                        }

                        if (lstData.Count > 0)
                        {



                            foreach (MCUFormResultFieldReporting row in lstData)
                            {
                                Builder.Append(string.Format("{0},", row.TransactionNo));
                                Builder.Append(string.Format("{0},", row.TanggalPemeriksaan));
                                Builder.Append(string.Format("{0},", row.RegistrasiNo));
                                Builder.Append(string.Format("{0},", row.NoPegawai));
                                Builder.Append(string.Format("{0},", row.Penjamin));
                                Builder.Append(string.Format("{0},", row.Posisi));
                                Builder.Append(string.Format("{0},", row.Lokasi));
                                Builder.Append(string.Format("{0},", row.NamaPegawai));
                                Builder.Append(string.Format("{0},", row.TglLahir));
                                Builder.Append(string.Format("{0},", row.JnsKelamin));
                                Builder.Append(string.Format("{0},", row.TanggalPemeriksaan));
                                Builder.Append(string.Format("{0},", row.ParamedicCode));
                                Builder.Append(string.Format("{0},", row.ParamedicName));
                                Builder.Append(string.Format("{0},", row.Col1));
                                Builder.Append(string.Format("{0}	,", row.Col2));
                                Builder.Append(string.Format("{0}	,", row.Col3));
                                Builder.Append(string.Format("{0}	,", row.Col4));
                                Builder.Append(string.Format("{0}	,", row.Col5));
                                Builder.Append(string.Format("{0}	,", row.Col6));
                                Builder.Append(string.Format("{0}	,", row.Col7));
                                Builder.Append(string.Format("{0}	,", row.Col8));
                                Builder.Append(string.Format("{0}	,", row.Col9));
                                Builder.Append(string.Format("{0}	,", row.Col10));
                                Builder.Append(string.Format("{0}	,", row.Col11));
                                Builder.Append(string.Format("{0}	,", row.Col12));
                                Builder.Append(string.Format("{0}	,", row.Col13));
                                Builder.Append(string.Format("{0}	,", row.Col14));
                                Builder.Append(string.Format("{0}	,", row.Col15));
                                Builder.Append(string.Format("{0}	,", row.Col16));
                                Builder.Append(string.Format("{0}	,", row.Col17));
                                Builder.Append(string.Format("{0}	,", row.Col18));
                                Builder.Append(string.Format("{0}	,", row.Col19));
                                Builder.Append(string.Format("{0}	,", row.Col20));
                                Builder.Append(string.Format("{0}	,", row.Col21));
                                Builder.Append(string.Format("{0}	,", row.Col22));
                                Builder.Append(string.Format("{0}	,", row.Col23));
                                Builder.Append(string.Format("{0}	,", row.Col24));
                                Builder.Append(string.Format("{0}	,", row.Col25));
                                Builder.Append(string.Format("{0}	,", row.Col26));
                                Builder.Append(string.Format("{0}	,", row.Col27));
                                Builder.Append(string.Format("{0}	,", row.Col28));
                                Builder.Append(string.Format("{0}	,", row.Col29));
                                Builder.Append(string.Format("{0}	,", row.Col30));
                                Builder.Append(string.Format("{0}	,", row.Col31));
                                Builder.Append(string.Format("{0}	,", row.Col32));
                                Builder.Append(string.Format("{0}	,", row.Col33));
                                Builder.Append(string.Format("{0}	,", row.Col34));
                                Builder.Append(string.Format("{0}	,", row.Col35));
                                Builder.Append(string.Format("{0}	,", row.Col36));
                                Builder.Append(string.Format("{0}	,", row.Col37));
                                Builder.Append(string.Format("{0}	,", row.Col38));
                                Builder.Append(string.Format("{0}	,", row.Col39));
                                Builder.Append(string.Format("{0}	,", row.Col40));
                                Builder.Append(string.Format("{0}	,", row.Col41));
                                Builder.Append(string.Format("{0}	,", row.Col42));
                                Builder.Append(string.Format("{0}	,", row.Col43));
                                Builder.Append(string.Format("{0}	,", row.Col44));
                                Builder.Append(string.Format("{0}	,", row.Col45));
                                Builder.Append(string.Format("{0}	,", row.Col46));
                                Builder.Append(string.Format("{0}	,", row.Col47));
                                Builder.Append(string.Format("{0}	,", row.Col48));
                                Builder.Append(string.Format("{0}	,", row.Col49));
                                Builder.Append(string.Format("{0}	,", row.Col50));
                                Builder.Append(string.Format("{0}	,", row.Col51));
                                Builder.Append(string.Format("{0}	,", row.Col52));
                                Builder.Append(string.Format("{0}	,", row.Col53));
                                Builder.Append(string.Format("{0}	,", row.Col54));
                                Builder.Append(string.Format("{0}	,", row.Col55));
                                Builder.Append(string.Format("{0}	,", row.Col56));
                                Builder.Append(string.Format("{0}	,", row.Col57));
                                Builder.Append(string.Format("{0}	,", row.Col58));
                                Builder.Append(string.Format("{0}	,", row.Col59));
                                Builder.Append(string.Format("{0}	,", row.Col60));
                                Builder.Append(string.Format("{0}	,", row.Col61));
                                Builder.Append(string.Format("{0}	,", row.Col62));
                                Builder.Append(string.Format("{0}	,", row.Col63));
                                Builder.Append(string.Format("{0}	,", row.Col64));
                                Builder.Append(string.Format("{0}	,", row.Col65));
                                Builder.Append(string.Format("{0}	,", row.Col66));
                                Builder.Append(string.Format("{0}	,", row.Col67));
                                Builder.Append(string.Format("{0}	,", row.Col68));
                                Builder.Append(string.Format("{0}	,", row.Col69));
                                Builder.Append(string.Format("{0}	,", row.Col70));
                                Builder.Append(string.Format("{0}	,", row.Col71));
                                Builder.Append(string.Format("{0}	,", row.Col72));
                                Builder.Append(string.Format("{0}	,", row.Col73));
                                Builder.Append(string.Format("{0}	,", row.Col74));
                                Builder.Append(string.Format("{0}	,", row.Col75));
                                Builder.Append(string.Format("{0}	,", row.Col76));
                                Builder.Append(string.Format("{0}	,", row.Col77));
                                Builder.Append(string.Format("{0}	,", row.Col78));
                                Builder.Append(string.Format("{0}	,", row.Col79));
                                Builder.Append(string.Format("{0}	,", row.Col80));
                                Builder.Append(string.Format("{0}	,", row.Col81));
                                Builder.Append(string.Format("{0}	,", row.Col82));
                                Builder.Append(string.Format("{0}	,", row.Col83));
                                Builder.Append(string.Format("{0}	,", row.Col84));
                                Builder.Append(string.Format("{0}	,", row.Col85));
                                Builder.Append(string.Format("{0}	,", row.Col86));
                                Builder.Append(string.Format("{0}	,", row.Col87));
                                Builder.Append(string.Format("{0}	,", row.Col88));
                                Builder.Append(string.Format("{0}	,", row.Col89));
                                Builder.Append(string.Format("{0}	,", row.Col90));
                                Builder.Append(string.Format("{0}	,", row.Col91));
                                Builder.Append(string.Format("{0}	,", row.Col92));
                                Builder.Append(string.Format("{0}	,", row.Col93));
                                Builder.Append(string.Format("{0}	,", row.Col94));
                                Builder.Append(string.Format("{0}	,", row.Col95));
                                Builder.Append(string.Format("{0}	,", row.Col96));
                                Builder.Append(string.Format("{0}	,", row.Col97));
                                Builder.Append(string.Format("{0}	,", row.Col98));
                                Builder.Append(string.Format("{0}	,", row.Col99));
                                Builder.Append(string.Format("{0}	,", row.Col100));
                                Builder.Append(string.Format("{0}	,", row.Col101));
                                Builder.Append(string.Format("{0}	,", row.Col102));
                                Builder.Append(string.Format("{0}	,", row.Col103));
                                Builder.Append(string.Format("{0}	,", row.Col104));
                                Builder.Append(string.Format("{0}	,", row.Col105));
                                Builder.Append(string.Format("{0}	,", row.Col106));
                                Builder.Append(string.Format("{0}	,", row.Col107));
                                Builder.Append(string.Format("{0}	,", row.Col108));
                                Builder.Append(string.Format("{0}	,", row.Col109));
                                Builder.Append(string.Format("{0}	,", row.Col110));
                                Builder.Append(string.Format("{0}	,", row.Col111));
                                Builder.Append(string.Format("{0}	,", row.Col112));
                                Builder.Append(string.Format("{0}	,", row.Col113));
                                Builder.Append(string.Format("{0}	,", row.Col114));
                                Builder.Append(string.Format("{0}	,", row.Col115));
                                Builder.Append(string.Format("{0}	,", row.Col116));
                                Builder.Append(string.Format("{0}	,", row.Col117));
                                Builder.Append(string.Format("{0}	,", row.Col118));
                                Builder.Append(string.Format("{0}	,", row.Col119));
                                Builder.Append(string.Format("{0}	,", row.Col120));
                                Builder.Append(string.Format("{0}	,", row.Col121));
                                Builder.Append(string.Format("{0}	,", row.Col122));
                                Builder.Append(string.Format("{0}	,", row.Col123));
                                Builder.Append(string.Format("{0}	,", row.Col124));
                                Builder.Append(string.Format("{0}	,", row.Col125));
                                Builder.Append(string.Format("{0}	,", row.Col126));
                                Builder.Append(string.Format("{0}	,", row.Col127));
                                Builder.Append(string.Format("{0}	,", row.Col128));
                                Builder.Append(string.Format("{0}	,", row.Col129));
                                Builder.Append(string.Format("{0}	,", row.Col130));
                                Builder.Append(string.Format("{0}	,", row.Col131));
                                Builder.Append(string.Format("{0}	,", row.Col132));
                                Builder.Append(string.Format("{0}	,", row.Col133));
                                Builder.Append(string.Format("{0}	,", row.Col134));
                                Builder.Append(string.Format("{0}	,", row.Col135));
                                Builder.Append(string.Format("{0}	,", row.Col136));
                                Builder.Append(string.Format("{0}	,", row.Col137));
                                Builder.Append(string.Format("{0}	,", row.Col138));
                                Builder.Append(string.Format("{0}	,", row.Col139));
                                Builder.Append(string.Format("{0}	,", row.Col140));
                                Builder.Append(string.Format("{0}	,", row.Col141));
                                Builder.Append(string.Format("{0}	,", row.Col142));
                                Builder.Append(string.Format("{0}	,", row.Col143));
                                Builder.Append(string.Format("{0}	,", row.Col144));
                                Builder.Append(string.Format("{0}	,", row.Col145));
                                Builder.Append(string.Format("{0}	,", row.Col146));
                                Builder.Append(string.Format("{0}	,", row.Col147));
                                Builder.Append(string.Format("{0}	,", row.Col148));
                                Builder.Append(string.Format("{0}	,", row.Col149));
                                Builder.Append(string.Format("{0}	,", row.Col150));
                                Builder.Append(string.Format("{0}	,", row.Col151));
                                Builder.Append(string.Format("{0}	,", row.Col152));
                                Builder.Append(string.Format("{0}	,", row.Col153));
                                Builder.Append(string.Format("{0}	,", row.Col154));
                                Builder.Append(string.Format("{0}	,", row.Col155));
                                Builder.Append(string.Format("{0}	,", row.Col156));
                                Builder.Append(string.Format("{0}	,", row.Col157));
                                Builder.Append(string.Format("{0}	,", row.Col158));
                                Builder.Append(string.Format("{0}	,", row.Col159));
                                Builder.Append(string.Format("{0}	,", row.Col160));
                                Builder.Append(string.Format("{0}	,", row.Col161));
                                Builder.Append(string.Format("{0}	,", row.Col162));
                                Builder.Append(string.Format("{0}	,", row.Col163));
                                Builder.Append(string.Format("{0}	,", row.Col164));
                                Builder.Append(string.Format("{0}	,", row.Col165));
                                Builder.Append(string.Format("{0}	,", row.Col166));
                                Builder.Append(string.Format("{0}	,", row.Col167));
                                Builder.Append(string.Format("{0}	,", row.Col168));
                                Builder.Append(string.Format("{0}	,", row.Col169));
                                Builder.Append(string.Format("{0}	,", row.Col170));
                                Builder.Append(string.Format("{0}	,", row.Col171));
                                Builder.Append("\r\n");

                            }


                        }
                    }
                }
               
                
                #endregion
                
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("Content-Disposition", "attachment;filename=HasilPengisianMCU.csv");
                Response.Write(Builder.ToString());
                Response.End();


            }
            catch (Exception ex)
            {
                
            }
            #endregion

        }

        private string filterCsvData(string data, string karakter)
        {
            string val = data;
            if (!string.IsNullOrEmpty(val)) {
                //////////val = data.Replace(karakter,  "");
                val = val.TrimEnd(System.Environment.NewLine.ToCharArray());
            }
            return val; 
        }

        public static string ToExcelCoordinates(string coordinates)
        {
            string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string first = coordinates.Substring(0, coordinates.IndexOf(','));
            int i = int.Parse(first);
            string second = coordinates.Substring(first.Length + 1);

            string str = string.Empty;
            while (i > 0)
            {
                str = ALPHABET[(i - 1) % 26] + str;
                i /= 26;
            }

            return str + second;
        }
    }
}