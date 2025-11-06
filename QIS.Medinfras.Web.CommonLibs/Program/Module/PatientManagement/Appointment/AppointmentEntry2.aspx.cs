using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentEntry2 : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case "OP": return Constant.MenuCode.Outpatient.APPOINTMENT_2;
                case "IS": return Constant.MenuCode.Imaging.APPOINTMENT_2;
                case "LB": return Constant.MenuCode.Laboratory.APPOINTMENT_2;
                case "MD": return Constant.MenuCode.MedicalDiagnostic.APPOINTMENT_2;
                default: return Constant.MenuCode.Outpatient.APPOINTMENT_2;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        #region Error Message
        protected string GetErrMessageCompletedAppointment()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_COMPLETED_APPOINTMENT_VALIDATION);
        }
        protected string GetErrMessageSelectAppointmentFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_APPOINTMENT_FIRST_VALIDATION);
        }
        #endregion

        protected string TodayDay()
        {
            return DateTime.Now.Day.ToString();
        }

        protected string TodayMonth()
        {
            return DateTime.Now.Month.ToString();
        }

        protected string TodayYear()
        {
            return DateTime.Now.Year.ToString();
        }

        protected int PageCount = 1;
        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            InitializeControlProperties();
            BindGridPhysician(1, true, ref PageCount);

            //Helper.SetControlEntrySetting(txtRoomCode, new ControlEntrySetting(true, true, true), "mpAppointment");
            //Helper.SetControlEntrySetting(txtRoomName, new ControlEntrySetting(false, false, true), "mpAppointment");
            Helper.SetControlEntrySetting(txtVisitTypeCode, new ControlEntrySetting(true, true, true), "mpAppointment");
            Helper.SetControlEntrySetting(txtMRN, new ControlEntrySetting(true, true, true), "mpAppointment");
            Helper.SetControlEntrySetting(cboSalutationAppo, new ControlEntrySetting(true, false, false), "mpAppointment");
            Helper.SetControlEntrySetting(txtFirstName, new ControlEntrySetting(true, false, false), "mpAppointment");
            Helper.SetControlEntrySetting(txtMiddleName, new ControlEntrySetting(true, false, false), "mpAppointment");
            Helper.SetControlEntrySetting(txtFamilyName, new ControlEntrySetting(true, false, true), "mpAppointment");
            Helper.SetControlEntrySetting(cboGenderAppointment, new ControlEntrySetting(true, true, true), "mpAppointment");
            Helper.SetControlEntrySetting(txtAddress, new ControlEntrySetting(true, true, true), "mpAppointment");
            Helper.SetControlEntrySetting(txtDOBMainAppt, new ControlEntrySetting(true, true, true), "mpAppointment");
            Helper.SetControlEntrySetting(txtCorporateAccountNo, new ControlEntrySetting(true, true, false), "mpAppointment");
            Helper.SetControlEntrySetting(txtCorporateAccountName, new ControlEntrySetting(true, true, false), "mpAppointment");
            Helper.SetControlEntrySetting(txtPhoneNo, new ControlEntrySetting(true, true, true), "mpAppointment");
            Helper.SetControlEntrySetting(hdnPayerID, new ControlEntrySetting(true, true), "mpAppointment");
            Helper.SetControlEntrySetting(txtPayerCompanyCode, new ControlEntrySetting(true, false, true), "mpAppointment");
            Helper.SetControlEntrySetting(txtPayerCompanyName, new ControlEntrySetting(false, false, false), "mpAppointment");
            Helper.SetControlEntrySetting(hdnContractID, new ControlEntrySetting(true, true), "mpAppointment");
            Helper.SetControlEntrySetting(txtContractNo, new ControlEntrySetting(true, false, true), "mpAppointment");
            Helper.SetControlEntrySetting(hdnCoverageTypeID, new ControlEntrySetting(true, true), "mpAppointment");
            Helper.SetControlEntrySetting(txtCoverageTypeCode, new ControlEntrySetting(true, false, true), "mpAppointment");
            Helper.SetControlEntrySetting(txtCoverageTypeName, new ControlEntrySetting(false, false, false), "mpAppointment");
            Helper.SetControlEntrySetting(hdnEmployeeID, new ControlEntrySetting(true, true), "mpAppointment");
            Helper.SetControlEntrySetting(txtEmployeeName, new ControlEntrySetting(false, false, false), "mpAppointment");
            Helper.SetControlEntrySetting(txtParticipantNo, new ControlEntrySetting(true, false, false), "mpAppointment");
            Helper.SetControlEntrySetting(txtCoverageLimit, new ControlEntrySetting(true, false, false, "0"), "mpAppointment");
            Helper.SetControlEntrySetting(hdnIsControlClassCare, new ControlEntrySetting(true, false, true, "0"), "mpAppointment");
            //SetControlEntrySetting(txtContractSummary, new ControlEntrySetting(false, false, false));
            Helper.SetControlEntrySetting(cboControlClassCare, new ControlEntrySetting(true, false, true), "mpAppointment");
            Helper.SetControlEntrySetting(lblPayerCompany, new ControlEntrySetting(true, false), "mpAppointment");
            Helper.SetControlEntrySetting(lblCoverageType, new ControlEntrySetting(true, false), "mpAppointment");
            Helper.SetControlEntrySetting(lblContract, new ControlEntrySetting(true, false), "mpAppointment");
            Helper.SetControlEntrySetting(lblEmployee, new ControlEntrySetting(true, false), "mpAppointment");
            Helper.SetControlEntrySetting(cboAppointmentMethod, new ControlEntrySetting(true, false, true), "mpAppointment");
            Helper.SetControlEntrySetting(cboResultDeliveryPlan, new ControlEntrySetting(true, false, false), "mpAppointment");
            Helper.SetControlEntrySetting(txtResultDeliveryPlanOthers, new ControlEntrySetting(true, false, false), "mpAppointment");

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            //hdnIsBridgingToGateway.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).ParameterValue;
            //hdnBPJSKesehatanID.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).ParameterValue;
            //hdnBPJSKetenagakerjaanID.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_PENJAMIN_BPJS_KETENAGAKERJAAN).ParameterValue;
            //hdnIsAppintmentAllowBackDate.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.OP_IS_APPOINTMENT_ALLOW_BACK_DATE).ParameterValue;
            //hdnIsBridgingToQumatic.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_BRIDGING_TO_QUMATIC).ParameterValue;
            //hdnApiKeyQumatic.Value = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.QUMATIC_API_KEY).ParameterValue;
        }

        private void InitializeControlProperties()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnDepartmentID.Value = Page.Request.QueryString["id"];
                int serviceUnitUserCount = 0;
                List<SettingParameter> setvar = BusinessLayer.GetSettingParameterList(String.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}', '{9}', '{10}')"
                    , Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI
                    , Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM
                    , Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY
                    , Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN
                    , Constant.SettingParameter.FN_PENJAMIN_BPJS_KETENAGAKERJAAN
                    , Constant.SettingParameter.OP_IS_APPOINTMENT_ALLOW_BACK_DATE
                    , Constant.SettingParameter.IS_BRIDGING_TO_QUMATIC
                    , Constant.SettingParameter.QUMATIC_API_KEY
                    , Constant.SettingParameter.DEFAULT_SERVICE_UNIT_INTERVAL
                    , Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE
                    , Constant.SettingParameter.SA_MAX_APPOINTMENT_VALIDATION));

                List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(String.Format(
                            "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')"
                            , AppSession.UserLogin.HealthcareID //0
                            , Constant.SettingParameter.OP_ALLOW_RESCHEDULE_BACK_DATE //1
                            , Constant.SettingParameter.LB_IS_USING_RESULT_DELIVERY_PLAN //2
                    ));

                hdnIsAllowRescheduleBackDate.Value = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.OP_ALLOW_RESCHEDULE_BACK_DATE).FirstOrDefault().ParameterValue;
                hdnIsUsingResultDeliveryPlan.Value = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.LB_IS_USING_RESULT_DELIVERY_PLAN).FirstOrDefault().ParameterValue;

                if (hdnIsUsingResultDeliveryPlan.Value == "1")
                {
                    trResultDeliveryPlan.Attributes.Remove("style");
                }
                else
                {
                    trResultDeliveryPlan.Attributes.Add("style", "display:none");
                }

                string setvarImaging = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
                string setvarLaboratory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;

                hdnIsBridgingToGateway.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;
                hdnProviderGatewayService.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).FirstOrDefault().ParameterValue;
                hdnBPJSKesehatanID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault().ParameterValue;
                hdnBPJSKetenagakerjaanID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KETENAGAKERJAAN).FirstOrDefault().ParameterValue;
                hdnIsAppintmentAllowBackDate.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.OP_IS_APPOINTMENT_ALLOW_BACK_DATE).FirstOrDefault().ParameterValue;
                hdnIsBridgingToQumatic.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_QUMATIC).FirstOrDefault().ParameterValue;
                hdnApiKeyQumatic.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.QUMATIC_API_KEY).FirstOrDefault().ParameterValue;
                hdnIsUsingValidationMaxAppointment.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.SA_MAX_APPOINTMENT_VALIDATION).FirstOrDefault().ParameterValue;

                if (hdnDepartmentID.Value == "OP")
                {
                    serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
                }
                else if (hdnDepartmentID.Value == "IS")
                {
                    serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("HealthcareServiceUnitID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0", setvarImaging, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
                }
                else if (hdnDepartmentID.Value == "LB")
                {
                    serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("HealthcareServiceUnitID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0", setvarLaboratory, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
                }
                else if (hdnDepartmentID.Value == "MD")
                {
                    serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND HealthcareServiceUnitID NOT IN ('{3}','{4}') AND IsDeleted = 0", Constant.Facility.DIAGNOSTIC, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, setvarImaging, setvarLaboratory));
                }

                string filterExpression = "";

                if (hdnDepartmentID.Value == "OP")
                {
                    filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT);

                    if (serviceUnitUserCount > 0)
                        filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0)", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
                }
                else if (hdnDepartmentID.Value == "IS")
                {
                    filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, setvarImaging);

                    if (serviceUnitUserCount > 0)
                        filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE HealthcareServiceUnitID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0)", setvarImaging, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
                }
                else if (hdnDepartmentID.Value == "LB")
                {
                    //filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, setvarLaboratory);

                    //if (serviceUnitUserCount > 0)
                    //    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE HealthcareServiceUnitID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0)", setvarLaboratory, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);

                    filterExpression = string.Format("IsLaboratoryUnit = 1 AND IsUsingRegistration = 1 AND HealthcareID = '{0}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID);
                }
                else if (hdnDepartmentID.Value == "MD")
                {
                    filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0 AND HealthcareServiceUnitID NOT IN ('{2}','{3}')", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, setvarImaging, setvarLaboratory);

                    if (serviceUnitUserCount > 0)
                    {
                        filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0 AND HealthcareServiceUnitID NOT IN ('{3}','{4}'))", Constant.Facility.DIAGNOSTIC, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, setvarImaging, setvarLaboratory);
                    }

                    filterExpression += " AND IsLaboratoryUnit = 0";
                }

                List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression, int.MaxValue, 1, "ServiceUnitName");
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                txtServiceUnit.Text = cboServiceUnit.Text;
                txtAppointmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                hdnCalAppointmentSelectedDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsDeleted = 0 AND IsActive = 1",
                                                                                                        Constant.StandardCode.TITLE, //0
                                                                                                        Constant.StandardCode.CUSTOMER_TYPE, //1
                                                                                                        Constant.StandardCode.APPOINTMENT_METHOD, //2
                                                                                                        Constant.StandardCode.GENDER, //3
                                                                                                        Constant.StandardCode.REFERRAL, //4
                                                                                                        Constant.StandardCode.RESULT_DELIVERY_PLAN //5
                                                                                                    ));

                Methods.SetComboBoxField<StandardCode>(cboAppointmentPayer, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.CUSTOMER_TYPE).OrderByDescending(t => t.StandardCodeID).ToList(), "StandardCodeName", "StandardCodeID");
                cboAppointmentPayer.SelectedIndex = 0;

                lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
                Methods.SetComboBoxField<StandardCode>(cboSalutationAppo, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.TITLE).ToList(), "StandardCodeName", "StandardCodeID");

                Methods.SetComboBoxField<StandardCode>(cboAppointmentMethod, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.APPOINTMENT_METHOD).OrderBy(t => t.StandardCodeID).ToList(), "StandardCodeName", "StandardCodeID");
                cboAppointmentMethod.SelectedIndex = 0;

                Methods.SetComboBoxField<StandardCode>(cboResultDeliveryPlan, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.RESULT_DELIVERY_PLAN || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
                cboResultDeliveryPlan.Value = "";

                hdnDefaultServiceUnitInterval.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.DEFAULT_SERVICE_UNIT_INTERVAL).FirstOrDefault().ParameterValue;

                List<StandardCode> lstSession = new List<StandardCode>();
                lstSession.Insert(0, new StandardCode { StandardCodeName = "Sesi 1", StandardCodeID = "0" });
                lstSession.Insert(1, new StandardCode { StandardCodeName = "Sesi 2", StandardCodeID = "1" });
                lstSession.Insert(2, new StandardCode { StandardCodeName = "Sesi 3", StandardCodeID = "2" });
                lstSession.Insert(3, new StandardCode { StandardCodeName = "Sesi 4", StandardCodeID = "3" });
                lstSession.Insert(4, new StandardCode { StandardCodeName = "Sesi 5", StandardCodeID = "4" });
                Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                cboSession.SelectedIndex = 0;

                Methods.SetComboBoxField<StandardCode>(cboGenderAppointment, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.GENDER).ToList(), "StandardCodeName", "StandardCodeID");
                
                Methods.SetComboBoxField<StandardCode>(cboReferral, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.REFERRAL || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            
                List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0");
                Methods.SetComboBoxField<ClassCare>(cboControlClassCare, lstClassCare, "ClassName", "ClassID");
                cboControlClassCare.SelectedIndex = 0;

                hdnGCTariffSchemePersonal.Value = BusinessLayer.GetCustomer(1).GCTariffScheme;

                txtAppointmentHour.Attributes.Add("readonly", "readonly");
            }
        }

        private void BindGridPhysician(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);

            //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
            int daynumber = (int)selectedDate.DayOfWeek;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            string filterExpression = string.Format(
                    "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = {0} AND DayNumber = {1} UNION ALL SELECT ParamedicID FROM vParamedicScheduleDate WHERE HealthcareServiceUnitID = {0} AND ScheduleDate = '{2}') AND IsDeleted = 0 AND IsAvailable = 1",
                    cboServiceUnit.Value, daynumber, selectedDate.ToString("yyyyMMdd"));

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvParamedicMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_FIVE);
            }

            List<vParamedicMaster> lstEntity = BusinessLayer.GetvParamedicMasterList(filterExpression, Constant.GridViewPageSize.GRID_FIVE, pageIndex, "ParamedicName");

            #region validate paramedic List (Exclude Paramedic Leave)
            //foreach (vParamedicMaster e in lstEntity.ToList())
            //{
            //    List<GetParamedicLeaveScheduleCompare> et = BusinessLayer.GetParamedicLeaveScheduleCompareList(selectedDate.ToString("yyyyMMdd"), e.ParamedicID);

            //    vParamedicScheduleDate ParamedicScheduleDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
            //                                                            "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
            //                                                            cboServiceUnit.Value, e.ParamedicID, selectedDate)).FirstOrDefault();


            //    vParamedicSchedule ParamedicSchedule = BusinessLayer.GetvParamedicScheduleList(string.Format(
            //                                        "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
            //                                        cboServiceUnit.Value, e.ParamedicID, daynumber)).FirstOrDefault();

            //    //cek untuk yang jadwal yang tidak ada tanggal apakah ada yang sama dengan jadwal cuti atau tidak (Validasi ParamedicSchedule)
            //    if ((et.Where(t => t.DayNumber == daynumber).Count()) > 0)
            //    {
            //        if (ParamedicSchedule != null)
            //        {
            //            //cek apakah jadwal praktek adalah hari pertama cuti atau tidak
            //            if (ParamedicSchedule.DayNumber == et.FirstOrDefault().DayNumber)
            //            {
            //                if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 != "")
            //                {
            //                    DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                    DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
            //                    DateTime StartScheduleDateInString3 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime3);
            //                    DateTime StartScheduleDateInString4 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime4);
            //                    DateTime StartScheduleDateInString5 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime5);
            //                    DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime5);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                        else
            //                        {
            //                            ParamedicSchedule = null;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString5.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 == "")
            //                {
            //                    DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                    DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
            //                    DateTime StartScheduleDateInString3 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime3);
            //                    DateTime StartScheduleDateInString4 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime4);
            //                    DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime4);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
            //                {
            //                    DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                    DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
            //                    DateTime StartScheduleDateInString3 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime3);
            //                    DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime3);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
            //                {
            //                    DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                    DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
            //                    DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime2);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 == "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
            //                {
            //                    DateTime StartScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                    DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime1);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            //cek apakah jadwal praktek ini adalah hari terakhir cuti atau bukan
            //            else if (ParamedicSchedule.DayNumber == et.LastOrDefault().DayNumber)
            //            {
            //                if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 != "")
            //                {
            //                    DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime5);
            //                    DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime5);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 == "")
            //                {
            //                    DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime4);
            //                    DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime4);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
            //                {
            //                    DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime3);
            //                    DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime3);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
            //                {
            //                    DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime2);
            //                    DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime2);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 == "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
            //                {
            //                    DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime1);
            //                    DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime1);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
            //                        {
            //                            if (ParamedicScheduleDate == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                            else
            //                            {
            //                                ParamedicSchedule = null;
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            //semua jadwal praktek yang bukan hari pertama dan hari terakhir cuti di buang
            //            else
            //            {
            //                lstEntity.Remove(e);
            //            }
            //        }
            //    }

            //    //cek apakah ada jadwal cuti atau tidak di tanggal yang dipilih (Validasi ParamedicScheduleDate)
            //    if (et.FirstOrDefault().DayNumber != 0)
            //    {
            //        if (ParamedicScheduleDate != null)
            //        {
            //            //cek apakah jadwal praktek adalah hari pertama cuti atau tidak
            //            if (ParamedicScheduleDate.ScheduleDate == et.FirstOrDefault().Date)
            //            {
            //                if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 != "")
            //                {
            //                    DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
            //                    DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
            //                    DateTime StartScheduleDateInString3 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime3);
            //                    DateTime StartScheduleDateInString4 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime4);
            //                    DateTime StartScheduleDateInString5 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime5);
            //                    DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime5);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString5.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 == "")
            //                {
            //                    DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
            //                    DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
            //                    DateTime StartScheduleDateInString3 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime3);
            //                    DateTime StartScheduleDateInString4 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime4);
            //                    DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime4);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
            //                {
            //                    DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
            //                    DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
            //                    DateTime StartScheduleDateInString3 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime3);
            //                    DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime3);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {

            //                        if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
            //                {
            //                    DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
            //                    DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
            //                    DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime2);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 == "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
            //                {
            //                    DateTime StartScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
            //                    DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime1);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            //cek apakah jadwal praktek ini adalah hari terakhir cuti atau bukan
            //            else if (ParamedicScheduleDate.ScheduleDate == et.LastOrDefault().Date)
            //            {
            //                if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 != "")
            //                {
            //                    DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime5);
            //                    DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime5);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }

            //                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
            //                    {
            //                        if (ParamedicSchedule == null)
            //                        {
            //                            lstEntity.Remove(e);
            //                        }
            //                    }
            //                }
            //                else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 == "")
            //                {
            //                    DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime4);
            //                    DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime4);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
            //                {
            //                    DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime3);
            //                    DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime3);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
            //                {
            //                    DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime2);
            //                    DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime2);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                }
            //                else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 == "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
            //                {
            //                    DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime1);
            //                    DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

            //                    if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
            //                    {
            //                        DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
            //                        DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime1);

            //                        DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
            //                        DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

            //                        if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {
            //                        if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
            //                        {
            //                            if (ParamedicSchedule == null)
            //                            {
            //                                lstEntity.Remove(e);
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            //semua jadwal praktek yang bukan hari pertama dan hari terakhir cuti di buang
            //            else
            //            {
            //                lstEntity.Remove(e);
            //            }
            //        }
            //    }
            //}
            #endregion

            cboSession.SelectedIndex = 0;
            grdPhysician.DataSource = lstEntity;
            grdPhysician.DataBind();
        }

        private void validateParamedic(List<vParamedicMaster> lstEntity)
        {
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);

            //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
            int daynumber = (int)selectedDate.DayOfWeek;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            #region validate paramedic List (Exclude Paramedic Leave)
            foreach (vParamedicMaster e in lstEntity.ToList())
            {
                List<GetParamedicLeaveScheduleCompare> et = BusinessLayer.GetParamedicLeaveScheduleCompareList(selectedDate.ToString("yyyyMMdd"), e.ParamedicID);

                vParamedicScheduleDate ParamedicScheduleDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                        "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                        cboServiceUnit.Value, e.ParamedicID, selectedDate)).FirstOrDefault();


                vParamedicSchedule ParamedicSchedule = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                    "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                    cboServiceUnit.Value, e.ParamedicID, daynumber)).FirstOrDefault();

                //cek untuk yang jadwal yang tidak ada tanggal apakah ada yang sama dengan jadwal cuti atau tidak (Validasi ParamedicSchedule)
                if ((et.Where(t => t.DayNumber == daynumber).Count()) > 0)
                {
                    if (ParamedicSchedule != null)
                    {
                        //cek apakah jadwal praktek adalah hari pertama cuti atau tidak
                        if (ParamedicSchedule.DayNumber == et.FirstOrDefault().DayNumber)
                        {
                            if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 != "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime3);
                                DateTime StartScheduleDateInString4 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime4);
                                DateTime StartScheduleDateInString5 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime5);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime5);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                    else
                                    {
                                        ParamedicSchedule = null;
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString5.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime3);
                                DateTime StartScheduleDateInString4 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime4);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime4);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime3);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime3);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime2);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 == "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime1);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                        }
                        //cek apakah jadwal praktek ini adalah hari terakhir cuti atau bukan
                        else if (ParamedicSchedule.DayNumber == et.LastOrDefault().DayNumber)
                        {
                            if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 != "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime5);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime5);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime4);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime4);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime3);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime3);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime2);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime2);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 == "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime1);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime1);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                        }
                        //semua jadwal praktek yang bukan hari pertama dan hari terakhir cuti di buang
                        else
                        {
                            lstEntity.Remove(e);
                        }
                    }
                }

                //cek apakah ada jadwal cuti atau tidak di tanggal yang dipilih (Validasi ParamedicScheduleDate)
                if (et.FirstOrDefault().DayNumber != 0)
                {
                    if (ParamedicScheduleDate != null)
                    {
                        //cek apakah jadwal praktek adalah hari pertama cuti atau tidak
                        if (ParamedicScheduleDate.ScheduleDate == et.FirstOrDefault().Date)
                        {
                            if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 != "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime3);
                                DateTime StartScheduleDateInString4 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime4);
                                DateTime StartScheduleDateInString5 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime5);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime5);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString5.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime3);
                                DateTime StartScheduleDateInString4 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime4);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime4);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime3);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime3);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {

                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime2);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 == "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime1);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                        }
                        //cek apakah jadwal praktek ini adalah hari terakhir cuti atau bukan
                        else if (ParamedicScheduleDate.ScheduleDate == et.LastOrDefault().Date)
                        {
                            if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 != "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime5);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime5);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }

                                if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                {
                                    if (ParamedicSchedule == null)
                                    {
                                        lstEntity.Remove(e);
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime4);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime4);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime3);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime3);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime2);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime2);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 == "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime1);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime1);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                        }
                        //semua jadwal praktek yang bukan hari pertama dan hari terakhir cuti di buang
                        else
                        {
                            lstEntity.Remove(e);
                        }
                    }
                }
            }
            #endregion
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

        private void SettingScheduleTable(List<ParamedicScheduleCustom> ListDisplayAppTime, String start, String end, Int32 interval, ref int id, ref int selectedRowIndex, ref bool addStartTime)
        {
            DateTime startTime = DateTime.Parse(String.Format("2012-01-28 {0}:00", start));
            DateTime endTime = DateTime.Parse(String.Format("2012-01-28 {0}:00", end));
            List<ParamedicScheduleCustom> ListDisplayAppTimeTemp = new List<ParamedicScheduleCustom>();
            bool result = true;

            if (result)
            {
                int minuteStartTime = startTime.Minute;
                int parentID = 0;
                while (startTime < endTime)
                {
                    ParamedicScheduleCustom entity = new ParamedicScheduleCustom();
                    entity.ID = ++id;

                    if (selectedRowIndex < 1)
                    {
                        vAppointment appointment = lstAppointment.FirstOrDefault(p => p.StartTime == startTime.ToString("HH:mm") && p.GCAppointmentStatus != Constant.AppointmentStatus.DELETED);
                        if (appointment == null)
                            selectedRowIndex = id;
                    }

                    entity.Time = startTime.ToString("HH:mm");
                    if (startTime.Minute == minuteStartTime)
                    {
                        parentID = entity.ID;
                        entity.ParentID = -1;
                    }
                    else
                        entity.ParentID = parentID;
                    startTime = startTime.AddMinutes(interval);

                    ListDisplayAppTime.Add(entity);
                }
                ParamedicScheduleCustom temp = new ParamedicScheduleCustom();
                temp.ID = ++id;
                temp.EndTime = end;
                ListDisplayAppTime.Add(temp);
            }

            List<ParamedicScheduleCustom> ListDisplayAppTimeFinalCheck = new List<ParamedicScheduleCustom>();

            foreach (ParamedicScheduleCustom e in ListDisplayAppTime)
            {
                if (ListDisplayAppTime.Where(t => t.Time == e.Time).Count() > 1)
                {
                    if (ListDisplayAppTimeFinalCheck.Where(t => t.Time == e.Time).Count() == 0)
                    {
                        ListDisplayAppTimeFinalCheck.Add(e);
                    }
                }
            }

            foreach (ParamedicScheduleCustom e in ListDisplayAppTimeFinalCheck)
            {
                ListDisplayAppTime.Remove(e);
            }
        }

        List<vAppointment> lstAppointment = null;
        protected void cbpAppointment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (!String.IsNullOrEmpty(hdnParamedicID.Value))
            {
                Int32 HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
                Int32 ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                int maximumAppointment = 0;
                short nonBPJSStartNo = 0;
                short nonBPJSEndNo = 0;
                short bpjsStartNo = 0;
                short bpjsEndNo = 0;


                int dayNumber = (int)selectedDate.DayOfWeek;
                if (dayNumber == 0)
                {
                    dayNumber = 7;
                }

                vParamedicSchedule obj = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                            "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                            HealthcareServiceUnitID, ParamedicID, dayNumber)).FirstOrDefault();

                vParamedicScheduleDate objSchDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                                "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                                HealthcareServiceUnitID, ParamedicID, selectedDate)).FirstOrDefault();

                ParamedicMaster pm = BusinessLayer.GetParamedicMaster(ParamedicID);

                ValidateParamedicScSchedule(obj, objSchDate);

                List<ParamedicScheduleCustom> ListDisplayAppTime = new List<ParamedicScheduleCustom>();
                List<ParamedicScheduleCustom> ListDisplayAppTimeWithoutTimeSlot = new List<ParamedicScheduleCustom>();
                List<ParamedicScheduleCustom> ListDisplayWaitingList = new List<ParamedicScheduleCustom>();

                int selectedRowIndex = 0;

                if (obj != null && objSchDate == null)
                {
                    hdnRoomIDDefault.Value = Convert.ToString(obj.RoomID);
                    hdnRoomCodeDefault.Value = obj.RoomCode;
                    hdnRoomNameDefault.Value = obj.RoomName;

                    String exp1 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), obj.StartTime1, obj.EndTime1);
                    String exp2 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), obj.StartTime2, obj.EndTime2);
                    String exp3 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), obj.StartTime3, obj.EndTime3);
                    String exp4 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), obj.StartTime4, obj.EndTime4);
                    String exp5 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), obj.StartTime5, obj.EndTime5);

                    List<Appointment> lst1 = BusinessLayer.GetAppointmentList(exp1);
                    List<Appointment> lst2 = BusinessLayer.GetAppointmentList(exp2);
                    List<Appointment> lst3 = BusinessLayer.GetAppointmentList(exp3);
                    List<Appointment> lst4 = BusinessLayer.GetAppointmentList(exp4);
                    List<Appointment> lst5 = BusinessLayer.GetAppointmentList(exp5);

                    if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 != "")
                    {
                        List<StandardCode> lstSession = new List<StandardCode>();
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "0" });
                        lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", obj.StartTime2, obj.EndTime2), StandardCodeID = "1" });
                        lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", obj.StartTime3, obj.EndTime3), StandardCodeID = "2" });
                        lstSession.Insert(3, new StandardCode { StandardCodeName = string.Format("Sesi 4 ({0} - {1})", obj.StartTime4, obj.EndTime4), StandardCodeID = "3" });
                        lstSession.Insert(4, new StandardCode { StandardCodeName = string.Format("Sesi 5 ({0} - {1})", obj.StartTime5, obj.EndTime5), StandardCodeID = "4" });
                        Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 == "")
                    {
                        List<StandardCode> lstSession = new List<StandardCode>();
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "0" });
                        lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", obj.StartTime2, obj.EndTime2), StandardCodeID = "1" });
                        lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", obj.StartTime3, obj.EndTime3), StandardCodeID = "2" });
                        lstSession.Insert(3, new StandardCode { StandardCodeName = string.Format("Sesi 4 ({0} - {1})", obj.StartTime4, obj.EndTime4), StandardCodeID = "3" });
                        Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        List<StandardCode> lstSession = new List<StandardCode>();
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "0" });
                        lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", obj.StartTime2, obj.EndTime2), StandardCodeID = "1" });
                        lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", obj.StartTime3, obj.EndTime3), StandardCodeID = "2" });
                        Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        List<StandardCode> lstSession = new List<StandardCode>();
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "0" });
                        lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", obj.StartTime2, obj.EndTime2), StandardCodeID = "1" });
                        Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        List<StandardCode> lstSession = new List<StandardCode>();
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "0" });
                        Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                    }

                    if (hdnIsChangeParamedicOrHealthcare.Value == "1")
                    {
                        cboSession.SelectedIndex = 0;
                    }

                    if (cboSession.Value.ToString() == "0" || cboSession.Value.ToString() == "1" || cboSession.Value.ToString() == "2" || cboSession.Value.ToString() == "3" || cboSession.Value.ToString() == "4")
                    {
                        if (cboSession.Value.ToString() == "0")
                        {
                            #region take validation info
                            hdnTotalAppoMessage.Value = Convert.ToString(lst1.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessage.Value = Convert.ToString(lst1.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessage.Value = Convert.ToString(obj.MaximumAppointment1);
                            hdnMaxWaitingMessage.Value = Convert.ToString(obj.MaximumWaitingList1);

                            if (lst1.Where(a => !a.IsWaitingList).Count() >= obj.MaximumAppointment1)
                            {
                                hdnIsValidAppoMessage.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessage.Value = "1";
                            }

                            if (lst1.Where(a => a.IsWaitingList).Count() >= obj.MaximumWaitingList1)
                            {
                                hdnIsValidWaitingMessage.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessage.Value = "1";
                            }
                            #endregion

                            hdnStartTime.Value = obj.StartTime1;
                            hdnEndTime.Value = obj.EndTime1;

                            if (obj.IsAppointmentByTimeSlot1 && obj.StartTime1 != "")
                            {
                                nonBPJSStartNo = obj.ReservedQueueStartNo1;
                                nonBPJSEndNo = obj.ReservedQueueEndNo1;
                                bpjsStartNo = obj.ReservedQueueStartNoBPJS1;
                                bpjsEndNo = obj.ReservedQueueEndNoBPJS1;

                                maximumAppointment = (obj.MaximumAppointment1 + obj.MaximumAppointmentBPJS1);
                                lblMaximumAppointment.InnerText = (obj.MaximumAppointment1 + obj.MaximumAppointmentBPJS1).ToString();
                                containerAppointment.Style.Remove("display");
                                containerAppointmentNoTimeSlot.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntry.Value = "1";
                            }
                            else if (!obj.IsAppointmentByTimeSlot1 && obj.StartTime1 != "")
                            {
                                nonBPJSStartNo = obj.ReservedQueueStartNo1;
                                nonBPJSEndNo = obj.ReservedQueueEndNo1;
                                bpjsStartNo = obj.ReservedQueueStartNoBPJS1;
                                bpjsEndNo = obj.ReservedQueueEndNoBPJS1;

                                maximumAppointment = (obj.MaximumAppointment1 + obj.MaximumAppointmentBPJS1);
                                lblMaximumAppointment.InnerText = (obj.MaximumAppointment1 + obj.MaximumAppointmentBPJS1).ToString();
                                containerAppointment.Style.Add("display", "none");
                                containerAppointmentNoTimeSlot.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntry.Value = "0";
                            }
                            else
                            {
                                containerAppointment.Style.Add("display", "none");
                                containerAppointmentNoTimeSlot.Style.Add("display", "none");
                            }

                            if (!obj.IsAllowWaitingList1)
                            {
                                lblMaximumWaitingList.InnerText = "0";
                                containerWaitingList.Style.Add("display", "none");
                            }
                            else
                            {
                                lblMaximumWaitingList.InnerText = (obj.MaximumWaitingList1 + obj.MaximumWaitingListBPJS1).ToString();
                                containerWaitingList.Style.Remove("display");
                            }
                        }
                        else if (cboSession.Value.ToString() == "1")
                        {
                            #region take validation info
                            hdnTotalAppoMessage.Value = Convert.ToString(lst2.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessage.Value = Convert.ToString(lst2.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessage.Value = Convert.ToString(obj.MaximumAppointment2);
                            hdnMaxWaitingMessage.Value = Convert.ToString(obj.MaximumWaitingList2);

                            if (lst2.Where(a => !a.IsWaitingList).Count() >= obj.MaximumAppointment2)
                            {
                                hdnIsValidAppoMessage.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessage.Value = "1";
                            }

                            if (lst2.Where(a => a.IsWaitingList).Count() >= obj.MaximumWaitingList2)
                            {
                                hdnIsValidWaitingMessage.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessage.Value = "1";
                            }
                            #endregion

                            hdnStartTime.Value = obj.StartTime2;
                            hdnEndTime.Value = obj.EndTime2;

                            if (obj.IsAppointmentByTimeSlot2 && obj.StartTime2 != "")
                            {
                                nonBPJSStartNo = obj.ReservedQueueStartNo2;
                                nonBPJSEndNo = obj.ReservedQueueEndNo2;
                                bpjsStartNo = obj.ReservedQueueStartNoBPJS2;
                                bpjsEndNo = obj.ReservedQueueEndNoBPJS2;

                                maximumAppointment = (obj.MaximumAppointment2 + obj.MaximumAppointmentBPJS2);
                                lblMaximumAppointment.InnerText = (obj.MaximumAppointment2 + obj.MaximumAppointmentBPJS2).ToString();
                                containerAppointment.Style.Remove("display");
                                containerAppointmentNoTimeSlot.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntry.Value = "1";
                            }
                            else if (!obj.IsAppointmentByTimeSlot2 && obj.StartTime2 != "")
                            {
                                nonBPJSStartNo = obj.ReservedQueueStartNo2;
                                nonBPJSEndNo = obj.ReservedQueueEndNo2;
                                bpjsStartNo = obj.ReservedQueueStartNoBPJS2;
                                bpjsEndNo = obj.ReservedQueueEndNoBPJS2;

                                maximumAppointment = (obj.MaximumAppointment2 + obj.MaximumAppointmentBPJS2);
                                lblMaximumAppointment.InnerText = (obj.MaximumAppointment2 + obj.MaximumAppointmentBPJS2).ToString();
                                containerAppointment.Style.Add("display", "none");
                                containerAppointmentNoTimeSlot.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntry.Value = "0";
                            }
                            else
                            {
                                containerAppointment.Style.Add("display", "none");
                                containerAppointmentNoTimeSlot.Style.Add("display", "none");
                            }

                            if (!obj.IsAllowWaitingList2)
                            {
                                lblMaximumWaitingList.InnerText = "0";
                                containerWaitingList.Style.Add("display", "none");
                            }
                            else
                            {
                                lblMaximumWaitingList.InnerText = (obj.MaximumWaitingList2 + obj.MaximumWaitingListBPJS2).ToString();
                                containerWaitingList.Style.Remove("display");
                            }
                        }
                        else if (cboSession.Value.ToString() == "2")
                        {
                            #region take validation info
                            hdnTotalAppoMessage.Value = Convert.ToString(lst3.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessage.Value = Convert.ToString(lst3.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessage.Value = Convert.ToString(obj.MaximumAppointment3);
                            hdnMaxWaitingMessage.Value = Convert.ToString(obj.MaximumWaitingList3);

                            if (lst3.Where(a => !a.IsWaitingList).Count() >= obj.MaximumAppointment3)
                            {
                                hdnIsValidAppoMessage.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessage.Value = "1";
                            }

                            if (lst3.Where(a => a.IsWaitingList).Count() >= obj.MaximumWaitingList3)
                            {
                                hdnIsValidWaitingMessage.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessage.Value = "1";
                            }
                            #endregion

                            hdnStartTime.Value = obj.StartTime3;
                            hdnEndTime.Value = obj.EndTime3;

                            if (obj.IsAppointmentByTimeSlot3 && obj.StartTime3 != "")
                            {
                                nonBPJSStartNo = obj.ReservedQueueStartNo3;
                                nonBPJSEndNo = obj.ReservedQueueEndNo3;
                                bpjsStartNo = obj.ReservedQueueStartNoBPJS3;
                                bpjsEndNo = obj.ReservedQueueEndNoBPJS3;

                                maximumAppointment = (obj.MaximumAppointment3 + obj.MaximumAppointmentBPJS3);
                                lblMaximumAppointment.InnerText = (obj.MaximumAppointment3 + obj.MaximumAppointmentBPJS3).ToString();

                                containerAppointment.Style.Remove("display");
                                containerAppointmentNoTimeSlot.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntry.Value = "1";
                            }
                            else if (!obj.IsAppointmentByTimeSlot3 && obj.StartTime3 != "")
                            {
                                nonBPJSStartNo = obj.ReservedQueueStartNo3;
                                nonBPJSEndNo = obj.ReservedQueueEndNo3;
                                bpjsStartNo = obj.ReservedQueueStartNoBPJS3;
                                bpjsEndNo = obj.ReservedQueueEndNoBPJS3;

                                maximumAppointment = (obj.MaximumAppointment3 + obj.MaximumAppointmentBPJS3);
                                lblMaximumAppointment.InnerText = (obj.MaximumAppointment3 + obj.MaximumAppointmentBPJS3).ToString();
                                containerAppointment.Style.Add("display", "none");
                                containerAppointmentNoTimeSlot.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntry.Value = "0";
                            }
                            else
                            {
                                containerAppointment.Style.Add("display", "none");
                                containerAppointmentNoTimeSlot.Style.Add("display", "none");
                            }

                            if (!obj.IsAllowWaitingList3)
                            {
                                lblMaximumWaitingList.InnerText = "0";
                                containerWaitingList.Style.Add("display", "none");
                            }
                            else
                            {
                                lblMaximumWaitingList.InnerText = (obj.MaximumWaitingList3 + obj.MaximumWaitingListBPJS3).ToString();
                                containerWaitingList.Style.Remove("display");
                            }
                        }
                        else if (cboSession.Value.ToString() == "3")
                        {
                            #region take validation info
                            hdnTotalAppoMessage.Value = Convert.ToString(lst4.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessage.Value = Convert.ToString(lst4.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessage.Value = Convert.ToString(obj.MaximumAppointment4);
                            hdnMaxWaitingMessage.Value = Convert.ToString(obj.MaximumWaitingList4);

                            if (lst4.Where(a => !a.IsWaitingList).Count() >= obj.MaximumAppointment4)
                            {
                                hdnIsValidAppoMessage.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessage.Value = "1";
                            }

                            if (lst4.Where(a => a.IsWaitingList).Count() >= obj.MaximumWaitingList4)
                            {
                                hdnIsValidWaitingMessage.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessage.Value = "1";
                            }
                            #endregion

                            hdnStartTime.Value = obj.StartTime4;
                            hdnEndTime.Value = obj.EndTime4;

                            if (obj.IsAppointmentByTimeSlot4 && obj.StartTime4 != "")
                            {
                                nonBPJSStartNo = obj.ReservedQueueStartNo4;
                                nonBPJSEndNo = obj.ReservedQueueEndNo4;
                                bpjsStartNo = obj.ReservedQueueStartNoBPJS4;
                                bpjsEndNo = obj.ReservedQueueEndNoBPJS4;

                                maximumAppointment = (obj.MaximumAppointment4 + obj.MaximumAppointmentBPJS4);
                                lblMaximumAppointment.InnerText = (obj.MaximumAppointment4 + obj.MaximumAppointmentBPJS4).ToString();
                                containerAppointment.Style.Remove("display");
                                containerAppointmentNoTimeSlot.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntry.Value = "1";
                            }
                            else if (!obj.IsAppointmentByTimeSlot4 && obj.StartTime4 != "")
                            {
                                nonBPJSStartNo = obj.ReservedQueueStartNo4;
                                nonBPJSEndNo = obj.ReservedQueueEndNo4;
                                bpjsStartNo = obj.ReservedQueueStartNoBPJS4;
                                bpjsEndNo = obj.ReservedQueueEndNoBPJS4;

                                maximumAppointment = (obj.MaximumAppointment4 + obj.MaximumAppointmentBPJS4);
                                lblMaximumAppointment.InnerText = (obj.MaximumAppointment4 + obj.MaximumAppointmentBPJS4).ToString();
                                containerAppointment.Style.Add("display", "none");
                                containerAppointmentNoTimeSlot.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntry.Value = "0";
                            }
                            else
                            {
                                containerAppointment.Style.Add("display", "none");
                                containerAppointmentNoTimeSlot.Style.Add("display", "none");
                            }

                            if (!obj.IsAllowWaitingList4)
                            {
                                lblMaximumWaitingList.InnerText = "0";
                                containerWaitingList.Style.Add("display", "none");
                            }
                            else
                            {
                                lblMaximumWaitingList.InnerText = (obj.MaximumWaitingList4 + obj.MaximumWaitingListBPJS4).ToString();
                                containerWaitingList.Style.Remove("display");
                            }
                        }
                        if (cboSession.Value.ToString() == "4")
                        {
                            #region take validation info
                            hdnTotalAppoMessage.Value = Convert.ToString(lst5.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessage.Value = Convert.ToString(lst5.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessage.Value = Convert.ToString(obj.MaximumAppointment5);
                            hdnMaxWaitingMessage.Value = Convert.ToString(obj.MaximumWaitingList5);

                            if (lst5.Where(a => !a.IsWaitingList).Count() >= obj.MaximumAppointment5)
                            {
                                hdnIsValidAppoMessage.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessage.Value = "1";
                            }

                            if (lst5.Where(a => a.IsWaitingList).Count() >= obj.MaximumWaitingList5)
                            {
                                hdnIsValidWaitingMessage.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessage.Value = "1";
                            }
                            #endregion

                            hdnStartTime.Value = obj.StartTime5;
                            hdnEndTime.Value = obj.EndTime5;

                            if (obj.IsAppointmentByTimeSlot5 && obj.StartTime5 != "")
                            {
                                nonBPJSStartNo = obj.ReservedQueueStartNo5;
                                nonBPJSEndNo = obj.ReservedQueueEndNo5;
                                bpjsStartNo = obj.ReservedQueueStartNoBPJS5;
                                bpjsEndNo = obj.ReservedQueueEndNoBPJS5;

                                maximumAppointment = (obj.MaximumAppointment5 + obj.MaximumAppointmentBPJS5);
                                lblMaximumAppointment.InnerText = (obj.MaximumAppointment5 + obj.MaximumAppointmentBPJS5).ToString();
                                containerAppointment.Style.Remove("display");
                                containerAppointmentNoTimeSlot.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntry.Value = "1";
                            }
                            else if (!obj.IsAppointmentByTimeSlot5 && obj.StartTime5 != "")
                            {
                                nonBPJSStartNo = obj.ReservedQueueStartNo5;
                                nonBPJSEndNo = obj.ReservedQueueEndNo5;
                                bpjsStartNo = obj.ReservedQueueStartNoBPJS5;
                                bpjsEndNo = obj.ReservedQueueEndNoBPJS5;

                                maximumAppointment = (obj.MaximumAppointment5 + obj.MaximumAppointmentBPJS5);
                                lblMaximumAppointment.InnerText = (obj.MaximumAppointment5 + obj.MaximumAppointmentBPJS5).ToString();
                                containerAppointment.Style.Add("display", "none");
                                containerAppointmentNoTimeSlot.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntry.Value = "0";
                            }
                            else
                            {
                                containerAppointment.Style.Add("display", "none");
                                containerAppointmentNoTimeSlot.Style.Add("display", "none");
                            }

                            if (!obj.IsAllowWaitingList5)
                            {
                                lblMaximumWaitingList.InnerText = "0";
                                containerWaitingList.Style.Add("display", "none");
                            }
                            else
                            {
                                lblMaximumWaitingList.InnerText = (obj.MaximumWaitingList5 + obj.MaximumWaitingListBPJS5).ToString();
                                containerWaitingList.Style.Remove("display");
                            }
                        }
                    }

                    lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}'", HealthcareServiceUnitID, ParamedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112)));
                    lblTotalRegistered.InnerText = lstAppointment.Where(t => t.GCAppointmentStatus == Constant.AppointmentStatus.COMPLETE).Count().ToString();
                    lblCancelAppointment.InnerText = lstAppointment.Where(t => t.GCAppointmentStatus == Constant.AppointmentStatus.DELETED && !t.IsWaitingList).Count().ToString();

                    Int32 bpjsKesehatanID = 0;
                    Int32 bpjsKetenagakerjaanID = 0;
                    if (!String.IsNullOrEmpty(hdnBPJSKesehatanID.Value) && hdnBPJSKesehatanID.Value != "0")
                    {
                        bpjsKesehatanID = Convert.ToInt32(hdnBPJSKesehatanID.Value);
                    }

                    if (!String.IsNullOrEmpty(hdnBPJSKetenagakerjaanID.Value) && hdnBPJSKetenagakerjaanID.Value != "0")
                    {
                        bpjsKetenagakerjaanID = Convert.ToInt32(hdnBPJSKetenagakerjaanID.Value);
                    }

                    List<vAppointment> lst = lstAppointment.Where(t => t.GCAppointmentStatus != Constant.AppointmentStatus.COMPLETE && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && !t.IsWaitingList).ToList();

                    Int32 totalAppointmentNonBPJS = 0;
                    Int32 totalAppointmentBPJS = 0;
                    if (bpjsKesehatanID != 0 && bpjsKetenagakerjaanID == 0)
                    {
                        totalAppointmentNonBPJS = lst.Where(x => x.BusinessPartnerID != bpjsKesehatanID).Count();
                        totalAppointmentBPJS = lst.Where(x => x.BusinessPartnerID == bpjsKesehatanID).Count();
                    }
                    else if (bpjsKesehatanID == 0 && bpjsKetenagakerjaanID != 0)
                    {
                        totalAppointmentNonBPJS = lst.Where(x => x.BusinessPartnerID != bpjsKetenagakerjaanID).Count();
                        totalAppointmentBPJS = lst.Where(x => x.BusinessPartnerID == bpjsKetenagakerjaanID).Count();
                    }
                    else if (bpjsKesehatanID != 0 && bpjsKetenagakerjaanID != 0)
                    {
                        totalAppointmentNonBPJS = lst.Where(x => x.BusinessPartnerID != bpjsKetenagakerjaanID && x.BusinessPartnerID != bpjsKesehatanID).Count();
                        totalAppointmentBPJS = lst.Where(x => x.BusinessPartnerID == bpjsKetenagakerjaanID || x.BusinessPartnerID == bpjsKesehatanID).Count();
                    }
                    else
                    {
                        totalAppointmentNonBPJS = lst.Count();
                        totalAppointmentBPJS = 0;
                    }
                    lblTotalAppointmentNonBPJS.InnerText = totalAppointmentNonBPJS.ToString();
                    lblTotalAppointmentBPJS.InnerText = totalAppointmentBPJS.ToString();
                    txtRemarksSchedule.Text = obj.Remarks;
                    List<ParamedicVisitType> lstIntervalParamedic = BusinessLayer.GetParamedicVisitTypeList(String.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} ORDER BY VisitDuration DESC", HealthcareServiceUnitID, ParamedicID));
                    List<ServiceUnitVisitType> lstIntervalHealthcare = BusinessLayer.GetServiceUnitVisitTypeList(String.Format("HealthcareServiceUnitID = {0} ORDER BY VisitDuration DESC", HealthcareServiceUnitID));
                    int serviceIntervalParamedic = lstIntervalParamedic.Sum(t => t.VisitDuration);
                    int serviceIntervalHealthcare = lstIntervalHealthcare.Sum(t => t.VisitDuration);

                    int serviceInterval = 0;
                    serviceInterval = Convert.ToInt16(pm.VisitDurationDefault);

                    if (serviceInterval < 1)
                        serviceInterval = Convert.ToInt32(hdnDefaultServiceUnitInterval.Value);
                    int id = 0;
                    bool isAddStartTime = true;

                    //untuk dengan slot Time
                    if (obj.StartTime1 != "" && obj.IsAppointmentByTimeSlot1 && cboSession.Value.ToString() == "0") SettingScheduleTable(ListDisplayAppTime, obj.StartTime1, obj.EndTime1, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                    if (obj.StartTime2 != "" && obj.IsAppointmentByTimeSlot2 && cboSession.Value.ToString() == "1") SettingScheduleTable(ListDisplayAppTime, obj.StartTime2, obj.EndTime2, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                    if (obj.StartTime3 != "" && obj.IsAppointmentByTimeSlot3 && cboSession.Value.ToString() == "2") SettingScheduleTable(ListDisplayAppTime, obj.StartTime3, obj.EndTime3, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                    if (obj.StartTime4 != "" && obj.IsAppointmentByTimeSlot4 && cboSession.Value.ToString() == "3") SettingScheduleTable(ListDisplayAppTime, obj.StartTime4, obj.EndTime4, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                    if (obj.StartTime5 != "" && obj.IsAppointmentByTimeSlot5 && cboSession.Value.ToString() == "4") SettingScheduleTable(ListDisplayAppTime, obj.StartTime5, obj.EndTime5, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);

                    //untuk tanpa slot Time
                    if (obj.StartTime1 != "" && !obj.IsAppointmentByTimeSlot1 && cboSession.Value.ToString() == "0") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, obj.StartTime1, obj.EndTime1, maximumAppointment, nonBPJSStartNo, nonBPJSEndNo, bpjsStartNo, bpjsEndNo);
                    if (obj.StartTime2 != "" && !obj.IsAppointmentByTimeSlot2 && cboSession.Value.ToString() == "1") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, obj.StartTime2, obj.EndTime2, maximumAppointment, nonBPJSStartNo, nonBPJSEndNo, bpjsStartNo, bpjsEndNo);
                    if (obj.StartTime3 != "" && !obj.IsAppointmentByTimeSlot3 && cboSession.Value.ToString() == "2") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, obj.StartTime3, obj.EndTime3, maximumAppointment, nonBPJSStartNo, nonBPJSEndNo, bpjsStartNo, bpjsEndNo);
                    if (obj.StartTime4 != "" && !obj.IsAppointmentByTimeSlot4 && cboSession.Value.ToString() == "3") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, obj.StartTime4, obj.EndTime4, maximumAppointment, nonBPJSStartNo, nonBPJSEndNo, bpjsStartNo, bpjsEndNo);
                    if (obj.StartTime5 != "" && !obj.IsAppointmentByTimeSlot5 && cboSession.Value.ToString() == "4") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, obj.StartTime5, obj.EndTime5, maximumAppointment, nonBPJSStartNo, nonBPJSEndNo, bpjsStartNo, bpjsEndNo);

                    if (obj.StartTime1 != "" && obj.IsAllowWaitingList1 && cboSession.Value.ToString() == "0") SettingSheduleWaitingList(ListDisplayWaitingList, obj.StartTime1);
                    if (obj.StartTime2 != "" && obj.IsAllowWaitingList2 && cboSession.Value.ToString() == "1") SettingSheduleWaitingList(ListDisplayWaitingList, obj.StartTime2);
                    if (obj.StartTime3 != "" && obj.IsAllowWaitingList3 && cboSession.Value.ToString() == "2") SettingSheduleWaitingList(ListDisplayWaitingList, obj.StartTime3);
                    if (obj.StartTime4 != "" && obj.IsAllowWaitingList4 && cboSession.Value.ToString() == "3") SettingSheduleWaitingList(ListDisplayWaitingList, obj.StartTime4);
                    if (obj.StartTime5 != "" && obj.IsAllowWaitingList5 && cboSession.Value.ToString() == "4") SettingSheduleWaitingList(ListDisplayWaitingList, obj.StartTime5);
                }
                // untuk dokter yang hanya punya paramedicscheduleDate Saja
                else
                {
                    if (objSchDate != null)
                    {
                        hdnRoomIDDefault.Value = Convert.ToString(objSchDate.RoomID);
                        hdnRoomCodeDefault.Value = objSchDate.RoomCode;
                        hdnRoomNameDefault.Value = objSchDate.RoomName;

                        String expSch1 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime1, objSchDate.EndTime1);
                        String expSch2 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime2, objSchDate.EndTime2);
                        String expSch3 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime3, objSchDate.EndTime3);
                        String expSch4 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime4, objSchDate.EndTime4);
                        String expSch5 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime5, objSchDate.EndTime5);

                        List<Appointment> lstSch1 = BusinessLayer.GetAppointmentList(expSch1);
                        List<Appointment> lstSch2 = BusinessLayer.GetAppointmentList(expSch2);
                        List<Appointment> lstSch3 = BusinessLayer.GetAppointmentList(expSch3);
                        List<Appointment> lstSch4 = BusinessLayer.GetAppointmentList(expSch4);
                        List<Appointment> lstSch5 = BusinessLayer.GetAppointmentList(expSch5);

                        if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 != "")
                        {
                            List<StandardCode> lstSession = new List<StandardCode>();
                            lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "0" });
                            lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", obj.StartTime2, obj.EndTime2), StandardCodeID = "1" });
                            lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", obj.StartTime3, obj.EndTime3), StandardCodeID = "2" });
                            lstSession.Insert(3, new StandardCode { StandardCodeName = string.Format("Sesi 4 ({0} - {1})", obj.StartTime4, obj.EndTime4), StandardCodeID = "3" });
                            lstSession.Insert(4, new StandardCode { StandardCodeName = string.Format("Sesi 5 ({0} - {1})", obj.StartTime5, obj.EndTime5), StandardCodeID = "4" });
                            Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                        {
                            List<StandardCode> lstSession = new List<StandardCode>();
                            lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objSchDate.StartTime1, objSchDate.EndTime1), StandardCodeID = "0" });
                            lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", objSchDate.StartTime2, objSchDate.EndTime2), StandardCodeID = "1" });
                            lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", objSchDate.StartTime3, objSchDate.EndTime3), StandardCodeID = "2" });
                            lstSession.Insert(3, new StandardCode { StandardCodeName = string.Format("Sesi 4 ({0} - {1})", objSchDate.StartTime4, objSchDate.EndTime4), StandardCodeID = "3" });
                            Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            List<StandardCode> lstSession = new List<StandardCode>();
                            lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objSchDate.StartTime1, objSchDate.EndTime1), StandardCodeID = "0" });
                            lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", objSchDate.StartTime2, objSchDate.EndTime2), StandardCodeID = "1" });
                            lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", objSchDate.StartTime3, objSchDate.EndTime3), StandardCodeID = "2" });
                            Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            List<StandardCode> lstSession = new List<StandardCode>();
                            lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objSchDate.StartTime1, objSchDate.EndTime1), StandardCodeID = "0" });
                            lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", objSchDate.StartTime2, objSchDate.EndTime2), StandardCodeID = "1" });
                            Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            List<StandardCode> lstSession = new List<StandardCode>();
                            lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objSchDate.StartTime1, objSchDate.EndTime1), StandardCodeID = "0" });
                            Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                        }

                        if (hdnIsChangeParamedicOrHealthcare.Value == "1")
                        {
                            cboSession.SelectedIndex = 0;
                        }

                        if (cboSession.Value.ToString() == "0" || cboSession.Value.ToString() == "1" || cboSession.Value.ToString() == "2" || cboSession.Value.ToString() == "3" || cboSession.Value.ToString() == "4")
                        {
                            if (cboSession.Value.ToString() == "0")
                            {
                                #region take validation info
                                hdnTotalAppoMessage.Value = Convert.ToString(lstSch1.Where(a => !a.IsWaitingList).Count());
                                hdnTotalWaitingMessage.Value = Convert.ToString(lstSch1.Where(a => a.IsWaitingList).Count());
                                hdnMaxAppoMessage.Value = Convert.ToString(objSchDate.MaximumAppointment1);
                                hdnMaxWaitingMessage.Value = Convert.ToString(objSchDate.MaximumWaitingList1);

                                if (lstSch1.Where(a => !a.IsWaitingList).Count() >= objSchDate.MaximumAppointment1)
                                {
                                    hdnIsValidAppoMessage.Value = "0";
                                }
                                else
                                {
                                    hdnIsValidAppoMessage.Value = "1";
                                }

                                if (lstSch1.Where(a => a.IsWaitingList).Count() >= objSchDate.MaximumWaitingList1)
                                {
                                    hdnIsValidWaitingMessage.Value = "0";
                                }
                                else
                                {
                                    hdnIsValidWaitingMessage.Value = "1";
                                }
                                #endregion

                                hdnStartTime.Value = objSchDate.StartTime1;
                                hdnEndTime.Value = objSchDate.EndTime1;

                                if (objSchDate.IsAppointmentByTimeSlot1 && objSchDate.StartTime1 != "")
                                {
                                    nonBPJSStartNo = objSchDate.ReservedQueueStartNo1;
                                    nonBPJSEndNo = objSchDate.ReservedQueueEndNo1;
                                    bpjsStartNo = objSchDate.ReservedQueueStartNoBPJS1;
                                    bpjsEndNo = objSchDate.ReservedQueueEndNoBPJS1;

                                    maximumAppointment = (objSchDate.MaximumAppointment1 + objSchDate.MaximumAppointmentBPJS1);
                                    lblMaximumAppointment.InnerText = (objSchDate.MaximumAppointment1 + objSchDate.MaximumAppointmentBPJS1).ToString();
                                    containerAppointment.Style.Remove("display");
                                    containerAppointmentNoTimeSlot.Style.Add("display", "none");

                                    hdnIsShowTimeSlotContainerEntry.Value = "1";
                                }
                                else if (!objSchDate.IsAppointmentByTimeSlot1 && objSchDate.StartTime1 != "")
                                {
                                    nonBPJSStartNo = objSchDate.ReservedQueueStartNo1;
                                    nonBPJSEndNo = objSchDate.ReservedQueueEndNo1;
                                    bpjsStartNo = objSchDate.ReservedQueueStartNoBPJS1;
                                    bpjsEndNo = objSchDate.ReservedQueueEndNoBPJS1;

                                    maximumAppointment = (objSchDate.MaximumAppointment1 + objSchDate.MaximumAppointmentBPJS1);
                                    lblMaximumAppointment.InnerText = (objSchDate.MaximumAppointment1 + objSchDate.MaximumAppointmentBPJS1).ToString();
                                    containerAppointment.Style.Add("display", "none");
                                    containerAppointmentNoTimeSlot.Style.Remove("display");

                                    hdnIsShowTimeSlotContainerEntry.Value = "0";
                                }
                                else
                                {
                                    containerAppointment.Style.Add("display", "none");
                                    containerAppointmentNoTimeSlot.Style.Add("display", "none");
                                }

                                if (!objSchDate.IsAllowWaitingList1)
                                {
                                    lblMaximumWaitingList.InnerText = "0";
                                    containerWaitingList.Style.Add("display", "none");
                                }
                                else
                                {
                                    lblMaximumWaitingList.InnerText = (objSchDate.MaximumWaitingList1 + objSchDate.MaximumWaitingListBPJS1).ToString();
                                    containerWaitingList.Style.Remove("display");
                                }
                            }
                            else if (cboSession.Value.ToString() == "1")
                            {
                                #region take validation info
                                hdnTotalAppoMessage.Value = Convert.ToString(lstSch2.Where(a => !a.IsWaitingList).Count());
                                hdnTotalWaitingMessage.Value = Convert.ToString(lstSch2.Where(a => a.IsWaitingList).Count());
                                hdnMaxAppoMessage.Value = Convert.ToString(objSchDate.MaximumAppointment2);
                                hdnMaxWaitingMessage.Value = Convert.ToString(objSchDate.MaximumWaitingList2);

                                if (lstSch2.Where(a => !a.IsWaitingList).Count() >= objSchDate.MaximumAppointment2)
                                {
                                    hdnIsValidAppoMessage.Value = "0";
                                }
                                else
                                {
                                    hdnIsValidAppoMessage.Value = "1";
                                }

                                if (lstSch2.Where(a => a.IsWaitingList).Count() >= objSchDate.MaximumWaitingList2)
                                {
                                    hdnIsValidWaitingMessage.Value = "0";
                                }
                                else
                                {
                                    hdnIsValidWaitingMessage.Value = "1";
                                }
                                #endregion

                                hdnStartTime.Value = objSchDate.StartTime2;
                                hdnEndTime.Value = objSchDate.EndTime2;

                                if (objSchDate.IsAppointmentByTimeSlot2 && objSchDate.StartTime2 != "")
                                {
                                    nonBPJSStartNo = objSchDate.ReservedQueueStartNo2;
                                    nonBPJSEndNo = objSchDate.ReservedQueueEndNo2;
                                    bpjsStartNo = objSchDate.ReservedQueueStartNoBPJS2;
                                    bpjsEndNo = objSchDate.ReservedQueueEndNoBPJS2;

                                    maximumAppointment = (objSchDate.MaximumAppointment2 + objSchDate.MaximumAppointmentBPJS2);
                                    lblMaximumAppointment.InnerText = (objSchDate.MaximumAppointment2 + objSchDate.MaximumAppointmentBPJS2).ToString();
                                    containerAppointment.Style.Remove("display");
                                    containerAppointmentNoTimeSlot.Style.Add("display", "none");

                                    hdnIsShowTimeSlotContainerEntry.Value = "1";
                                }
                                else if (!objSchDate.IsAppointmentByTimeSlot2 && objSchDate.StartTime2 != "")
                                {
                                    nonBPJSStartNo = objSchDate.ReservedQueueStartNo2;
                                    nonBPJSEndNo = objSchDate.ReservedQueueEndNo2;
                                    bpjsStartNo = objSchDate.ReservedQueueStartNoBPJS2;
                                    bpjsEndNo = objSchDate.ReservedQueueEndNoBPJS2;

                                    maximumAppointment = (objSchDate.MaximumAppointment2 + objSchDate.MaximumAppointmentBPJS2);
                                    lblMaximumAppointment.InnerText = (objSchDate.MaximumAppointment2 + objSchDate.MaximumAppointmentBPJS2).ToString();
                                    containerAppointment.Style.Add("display", "none");
                                    containerAppointmentNoTimeSlot.Style.Remove("display");

                                    hdnIsShowTimeSlotContainerEntry.Value = "0";
                                }
                                else
                                {
                                    containerAppointment.Style.Add("display", "none");
                                    containerAppointmentNoTimeSlot.Style.Add("display", "none");
                                }

                                if (!objSchDate.IsAllowWaitingList2)
                                {
                                    lblMaximumWaitingList.InnerText = "0";
                                    containerWaitingList.Style.Add("display", "none");
                                }
                                else
                                {
                                    lblMaximumWaitingList.InnerText = (objSchDate.MaximumWaitingList2 + objSchDate.MaximumWaitingListBPJS2).ToString();
                                    containerWaitingList.Style.Remove("display");
                                }
                            }
                            else if (cboSession.Value.ToString() == "2")
                            {
                                #region take validation info
                                hdnTotalAppoMessage.Value = Convert.ToString(lstSch3.Where(a => !a.IsWaitingList).Count());
                                hdnTotalWaitingMessage.Value = Convert.ToString(lstSch3.Where(a => a.IsWaitingList).Count());
                                hdnMaxAppoMessage.Value = Convert.ToString(objSchDate.MaximumAppointment3);
                                hdnMaxWaitingMessage.Value = Convert.ToString(objSchDate.MaximumWaitingList3);

                                if (lstSch3.Where(a => !a.IsWaitingList).Count() >= objSchDate.MaximumAppointment3)
                                {
                                    hdnIsValidAppoMessage.Value = "0";
                                }
                                else
                                {
                                    hdnIsValidAppoMessage.Value = "1";
                                }

                                if (lstSch3.Where(a => a.IsWaitingList).Count() >= objSchDate.MaximumWaitingList3)
                                {
                                    hdnIsValidWaitingMessage.Value = "0";
                                }
                                else
                                {
                                    hdnIsValidWaitingMessage.Value = "1";
                                }
                                #endregion

                                hdnStartTime.Value = objSchDate.StartTime3;
                                hdnEndTime.Value = objSchDate.EndTime3;

                                if (objSchDate.IsAppointmentByTimeSlot3 && objSchDate.StartTime3 != "")
                                {
                                    nonBPJSStartNo = objSchDate.ReservedQueueStartNo3;
                                    nonBPJSEndNo = objSchDate.ReservedQueueEndNo3;
                                    bpjsStartNo = objSchDate.ReservedQueueStartNoBPJS3;
                                    bpjsEndNo = objSchDate.ReservedQueueEndNoBPJS3;

                                    maximumAppointment = (objSchDate.MaximumAppointment3 + objSchDate.MaximumAppointmentBPJS3);
                                    lblMaximumAppointment.InnerText = (objSchDate.MaximumAppointment3 + objSchDate.MaximumAppointmentBPJS3).ToString();
                                    containerAppointment.Style.Remove("display");
                                    containerAppointmentNoTimeSlot.Style.Add("display", "none");

                                    hdnIsShowTimeSlotContainerEntry.Value = "1";
                                }
                                else if (!objSchDate.IsAppointmentByTimeSlot3 && objSchDate.StartTime3 != "")
                                {
                                    nonBPJSStartNo = objSchDate.ReservedQueueStartNo3;
                                    nonBPJSEndNo = objSchDate.ReservedQueueEndNo3;
                                    bpjsStartNo = objSchDate.ReservedQueueStartNoBPJS3;
                                    bpjsEndNo = objSchDate.ReservedQueueEndNoBPJS3;

                                    maximumAppointment = (objSchDate.MaximumAppointment3 + objSchDate.MaximumAppointmentBPJS3);
                                    lblMaximumAppointment.InnerText = (objSchDate.MaximumAppointment3 + objSchDate.MaximumAppointmentBPJS3).ToString();
                                    containerAppointment.Style.Add("display", "none");
                                    containerAppointmentNoTimeSlot.Style.Remove("display");

                                    hdnIsShowTimeSlotContainerEntry.Value = "0";
                                }
                                else
                                {
                                    containerAppointment.Style.Add("display", "none");
                                    containerAppointmentNoTimeSlot.Style.Add("display", "none");
                                }

                                if (!objSchDate.IsAllowWaitingList3)
                                {
                                    lblMaximumWaitingList.InnerText = "0";
                                    containerWaitingList.Style.Add("display", "none");
                                }
                                else
                                {
                                    lblMaximumWaitingList.InnerText = (objSchDate.MaximumWaitingList3 + objSchDate.MaximumWaitingListBPJS3).ToString();
                                    containerWaitingList.Style.Remove("display");
                                }
                            }
                            else if (cboSession.Value.ToString() == "3")
                            {
                                #region take validation info
                                hdnTotalAppoMessage.Value = Convert.ToString(lstSch4.Where(a => !a.IsWaitingList).Count());
                                hdnTotalWaitingMessage.Value = Convert.ToString(lstSch4.Where(a => a.IsWaitingList).Count());
                                hdnMaxAppoMessage.Value = Convert.ToString(objSchDate.MaximumAppointment4);
                                hdnMaxWaitingMessage.Value = Convert.ToString(objSchDate.MaximumWaitingList4);

                                if (lstSch4.Where(a => !a.IsWaitingList).Count() >= objSchDate.MaximumAppointment4)
                                {
                                    hdnIsValidAppoMessage.Value = "0";
                                }
                                else
                                {
                                    hdnIsValidAppoMessage.Value = "1";
                                }

                                if (lstSch4.Where(a => a.IsWaitingList).Count() >= objSchDate.MaximumWaitingList4)
                                {
                                    hdnIsValidWaitingMessage.Value = "0";
                                }
                                else
                                {
                                    hdnIsValidWaitingMessage.Value = "1";
                                }
                                #endregion

                                hdnStartTime.Value = objSchDate.StartTime4;
                                hdnEndTime.Value = objSchDate.EndTime4;

                                if (objSchDate.IsAppointmentByTimeSlot4 && objSchDate.StartTime4 != "")
                                {
                                    nonBPJSStartNo = objSchDate.ReservedQueueStartNo4;
                                    nonBPJSEndNo = objSchDate.ReservedQueueEndNo4;
                                    bpjsStartNo = objSchDate.ReservedQueueStartNoBPJS4;
                                    bpjsEndNo = objSchDate.ReservedQueueEndNoBPJS4;

                                    maximumAppointment = (objSchDate.MaximumAppointment4 + objSchDate.MaximumAppointmentBPJS4);
                                    lblMaximumAppointment.InnerText = (objSchDate.MaximumAppointment4 + objSchDate.MaximumAppointmentBPJS4).ToString();
                                    containerAppointment.Style.Remove("display");
                                    containerAppointmentNoTimeSlot.Style.Add("display", "none");

                                    hdnIsShowTimeSlotContainerEntry.Value = "1";
                                }
                                else if (!objSchDate.IsAppointmentByTimeSlot4 && obj.StartTime4 != "")
                                {
                                    nonBPJSStartNo = objSchDate.ReservedQueueStartNo4;
                                    nonBPJSEndNo = objSchDate.ReservedQueueEndNo4;
                                    bpjsStartNo = objSchDate.ReservedQueueStartNoBPJS4;
                                    bpjsEndNo = objSchDate.ReservedQueueEndNoBPJS4;

                                    maximumAppointment = (objSchDate.MaximumAppointment4 + objSchDate.MaximumAppointmentBPJS4);
                                    lblMaximumAppointment.InnerText = (objSchDate.MaximumAppointment4 + objSchDate.MaximumAppointmentBPJS4).ToString();
                                    containerAppointment.Style.Add("display", "none");
                                    containerAppointmentNoTimeSlot.Style.Remove("display");

                                    hdnIsShowTimeSlotContainerEntry.Value = "0";
                                }
                                else
                                {
                                    containerAppointment.Style.Add("display", "none");
                                    containerAppointmentNoTimeSlot.Style.Add("display", "none");
                                }

                                if (!objSchDate.IsAllowWaitingList4)
                                {
                                    lblMaximumWaitingList.InnerText = "0";
                                    containerWaitingList.Style.Add("display", "none");
                                }
                                else
                                {
                                    lblMaximumWaitingList.InnerText = (objSchDate.MaximumWaitingList4 + objSchDate.MaximumWaitingListBPJS4).ToString();
                                    containerWaitingList.Style.Remove("display");
                                }
                            }
                            if (cboSession.Value.ToString() == "4")
                            {
                                #region take validation info
                                hdnTotalAppoMessage.Value = Convert.ToString(lstSch5.Where(a => !a.IsWaitingList).Count());
                                hdnTotalWaitingMessage.Value = Convert.ToString(lstSch5.Where(a => a.IsWaitingList).Count());
                                hdnMaxAppoMessage.Value = Convert.ToString(objSchDate.MaximumAppointment5);
                                hdnMaxWaitingMessage.Value = Convert.ToString(objSchDate.MaximumWaitingList5);

                                if (lstSch5.Where(a => !a.IsWaitingList).Count() >= objSchDate.MaximumAppointment5)
                                {
                                    hdnIsValidAppoMessage.Value = "0";
                                }
                                else
                                {
                                    hdnIsValidAppoMessage.Value = "1";
                                }

                                if (lstSch5.Where(a => a.IsWaitingList).Count() >= objSchDate.MaximumWaitingList5)
                                {
                                    hdnIsValidWaitingMessage.Value = "0";
                                }
                                else
                                {
                                    hdnIsValidWaitingMessage.Value = "1";
                                }
                                #endregion

                                hdnStartTime.Value = objSchDate.StartTime5;
                                hdnEndTime.Value = objSchDate.EndTime5;

                                if (objSchDate.IsAppointmentByTimeSlot5 && objSchDate.StartTime5 != "")
                                {
                                    nonBPJSStartNo = objSchDate.ReservedQueueStartNo5;
                                    nonBPJSEndNo = objSchDate.ReservedQueueEndNo5;
                                    bpjsStartNo = objSchDate.ReservedQueueStartNoBPJS5;
                                    bpjsEndNo = objSchDate.ReservedQueueEndNoBPJS5;

                                    maximumAppointment = (objSchDate.MaximumAppointment5 + objSchDate.MaximumAppointmentBPJS5);
                                    lblMaximumAppointment.InnerText = (objSchDate.MaximumAppointment5 + objSchDate.MaximumAppointmentBPJS5).ToString();
                                    containerAppointment.Style.Remove("display");
                                    containerAppointmentNoTimeSlot.Style.Add("display", "none");

                                    hdnIsShowTimeSlotContainerEntry.Value = "1";
                                }
                                else if (!objSchDate.IsAppointmentByTimeSlot5 && objSchDate.StartTime5 != "")
                                {
                                    nonBPJSStartNo = objSchDate.ReservedQueueStartNo5;
                                    nonBPJSEndNo = objSchDate.ReservedQueueEndNo5;
                                    bpjsStartNo = objSchDate.ReservedQueueStartNoBPJS5;
                                    bpjsEndNo = objSchDate.ReservedQueueEndNoBPJS5;

                                    maximumAppointment = (objSchDate.MaximumAppointment5 + objSchDate.MaximumAppointmentBPJS5);
                                    lblMaximumAppointment.InnerText = (objSchDate.MaximumAppointment5 + objSchDate.MaximumAppointmentBPJS5).ToString();
                                    containerAppointment.Style.Add("display", "none");
                                    containerAppointmentNoTimeSlot.Style.Remove("display");

                                    hdnIsShowTimeSlotContainerEntry.Value = "0";
                                }
                                else
                                {
                                    containerAppointment.Style.Add("display", "none");
                                    containerAppointmentNoTimeSlot.Style.Add("display", "none");
                                }

                                if (!objSchDate.IsAllowWaitingList5)
                                {
                                    lblMaximumWaitingList.InnerText = "0";
                                    containerWaitingList.Style.Add("display", "none");
                                }
                                else
                                {
                                    lblMaximumWaitingList.InnerText = (objSchDate.MaximumWaitingList5 + objSchDate.MaximumWaitingListBPJS5).ToString();
                                    containerWaitingList.Style.Remove("display");
                                }
                            }
                        }

                        lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}'", HealthcareServiceUnitID, ParamedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112)));
                        lblTotalRegistered.InnerText = lstAppointment.Where(t => t.GCAppointmentStatus == Constant.AppointmentStatus.COMPLETE).Count().ToString();
                        lblCancelAppointment.InnerText = lstAppointment.Where(t => t.GCAppointmentStatus == Constant.AppointmentStatus.DELETED && !t.IsWaitingList).Count().ToString();

                        Int32 bpjsKesehatanID = 0;
                        Int32 bpjsKetenagakerjaanID = 0;
                        if (!String.IsNullOrEmpty(hdnBPJSKesehatanID.Value) && hdnBPJSKesehatanID.Value != "0")
                        {
                            bpjsKesehatanID = Convert.ToInt32(hdnBPJSKesehatanID.Value);
                        }

                        if (!String.IsNullOrEmpty(hdnBPJSKetenagakerjaanID.Value) && hdnBPJSKetenagakerjaanID.Value != "0")
                        {
                            bpjsKetenagakerjaanID = Convert.ToInt32(hdnBPJSKetenagakerjaanID.Value);
                        }

                        List<vAppointment> lst = lstAppointment.Where(t => t.GCAppointmentStatus != Constant.AppointmentStatus.COMPLETE && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && !t.IsWaitingList).ToList();

                        Int32 totalAppointmentNonBPJS = 0;
                        Int32 totalAppointmentBPJS = 0;
                        if (bpjsKesehatanID != 0 && bpjsKetenagakerjaanID == 0)
                        {
                            totalAppointmentNonBPJS = lst.Where(x => x.BusinessPartnerID != bpjsKesehatanID).Count();
                            totalAppointmentBPJS = lst.Where(x => x.BusinessPartnerID == bpjsKesehatanID).Count();
                        }
                        else if (bpjsKesehatanID == 0 && bpjsKetenagakerjaanID != 0)
                        {
                            totalAppointmentNonBPJS = lst.Where(x => x.BusinessPartnerID != bpjsKetenagakerjaanID).Count();
                            totalAppointmentBPJS = lst.Where(x => x.BusinessPartnerID == bpjsKetenagakerjaanID).Count();
                        }
                        else if (bpjsKesehatanID != 0 && bpjsKetenagakerjaanID != 0)
                        {
                            totalAppointmentNonBPJS = lst.Where(x => x.BusinessPartnerID != bpjsKetenagakerjaanID && x.BusinessPartnerID != bpjsKesehatanID).Count();
                            totalAppointmentBPJS = lst.Where(x => x.BusinessPartnerID == bpjsKetenagakerjaanID || x.BusinessPartnerID == bpjsKesehatanID).Count();
                        }
                        else
                        {
                            totalAppointmentNonBPJS = lst.Count();
                            totalAppointmentBPJS = 0;
                        }
                        lblTotalAppointmentNonBPJS.InnerText = totalAppointmentNonBPJS.ToString();
                        lblTotalAppointmentBPJS.InnerText = totalAppointmentBPJS.ToString();
                        txtRemarksSchedule.Text = objSchDate.Remarks;
                        List<ParamedicVisitType> lstIntervalParamedic = BusinessLayer.GetParamedicVisitTypeList(String.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} ORDER BY VisitDuration DESC", HealthcareServiceUnitID, ParamedicID));
                        List<ServiceUnitVisitType> lstIntervalHealthcare = BusinessLayer.GetServiceUnitVisitTypeList(String.Format("HealthcareServiceUnitID = {0} ORDER BY VisitDuration DESC", HealthcareServiceUnitID));
                        int serviceIntervalParamedic = lstIntervalParamedic.Sum(t => t.VisitDuration);
                        int serviceIntervalHealthcare = lstIntervalHealthcare.Sum(t => t.VisitDuration);

                        int serviceInterval = 0;
                        serviceInterval = Convert.ToInt16(pm.VisitDurationDefault);

                        if (serviceInterval < 1)
                            serviceInterval = Convert.ToInt32(hdnDefaultServiceUnitInterval.Value);
                        int id = 0;
                        bool isAddStartTime = true;

                        //untuk dengan slot Time
                        if (objSchDate.StartTime1 != "" && objSchDate.IsAppointmentByTimeSlot1 && cboSession.Value.ToString() == "0") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime1, objSchDate.EndTime1, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                        if (objSchDate.StartTime2 != "" && objSchDate.IsAppointmentByTimeSlot2 && cboSession.Value.ToString() == "1") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime2, objSchDate.EndTime2, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                        if (objSchDate.StartTime3 != "" && objSchDate.IsAppointmentByTimeSlot3 && cboSession.Value.ToString() == "2") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime3, objSchDate.EndTime3, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                        if (objSchDate.StartTime4 != "" && objSchDate.IsAppointmentByTimeSlot4 && cboSession.Value.ToString() == "3") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime4, objSchDate.EndTime4, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                        if (objSchDate.StartTime5 != "" && objSchDate.IsAppointmentByTimeSlot5 && cboSession.Value.ToString() == "4") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime5, objSchDate.EndTime5, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);

                        //untuk tanpa slot Time
                        if (objSchDate.StartTime1 != "" && !objSchDate.IsAppointmentByTimeSlot1 && cboSession.Value.ToString() == "0") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, objSchDate.StartTime1, objSchDate.EndTime1, maximumAppointment, nonBPJSStartNo, nonBPJSEndNo, bpjsStartNo,bpjsEndNo);
                        if (objSchDate.StartTime2 != "" && !objSchDate.IsAppointmentByTimeSlot2 && cboSession.Value.ToString() == "1") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, objSchDate.StartTime2, objSchDate.EndTime2, maximumAppointment, nonBPJSStartNo, nonBPJSEndNo, bpjsStartNo, bpjsEndNo);
                        if (objSchDate.StartTime3 != "" && !objSchDate.IsAppointmentByTimeSlot3 && cboSession.Value.ToString() == "2") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, objSchDate.StartTime3, objSchDate.EndTime3, maximumAppointment, nonBPJSStartNo, nonBPJSEndNo, bpjsStartNo, bpjsEndNo);
                        if (objSchDate.StartTime4 != "" && !objSchDate.IsAppointmentByTimeSlot4 && cboSession.Value.ToString() == "3") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, objSchDate.StartTime4, objSchDate.EndTime4, maximumAppointment, nonBPJSStartNo, nonBPJSEndNo, bpjsStartNo, bpjsEndNo);
                        if (objSchDate.StartTime5 != "" && !objSchDate.IsAppointmentByTimeSlot5 && cboSession.Value.ToString() == "4") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, objSchDate.StartTime5, objSchDate.EndTime5, maximumAppointment, nonBPJSStartNo, nonBPJSEndNo, bpjsStartNo, bpjsEndNo);

                        if (objSchDate.StartTime1 != "" && objSchDate.IsAllowWaitingList1 && cboSession.Value.ToString() == "0") SettingSheduleWaitingList(ListDisplayWaitingList, objSchDate.StartTime1);
                        if (objSchDate.StartTime2 != "" && objSchDate.IsAllowWaitingList2 && cboSession.Value.ToString() == "1") SettingSheduleWaitingList(ListDisplayWaitingList, objSchDate.StartTime2);
                        if (objSchDate.StartTime3 != "" && objSchDate.IsAllowWaitingList3 && cboSession.Value.ToString() == "2") SettingSheduleWaitingList(ListDisplayWaitingList, objSchDate.StartTime3);
                        if (objSchDate.StartTime4 != "" && objSchDate.IsAllowWaitingList4 && cboSession.Value.ToString() == "3") SettingSheduleWaitingList(ListDisplayWaitingList, objSchDate.StartTime4);
                        if (objSchDate.StartTime5 != "" && objSchDate.IsAllowWaitingList5 && cboSession.Value.ToString() == "4") SettingSheduleWaitingList(ListDisplayWaitingList, objSchDate.StartTime5);
                    }
                }

                grdWaitingList.DataSource = ListDisplayWaitingList;
                grdWaitingList.DataBind();

                grdAppointmentNoTimeSlot.DataSource = ListDisplayAppTimeWithoutTimeSlot;
                grdAppointmentNoTimeSlot.DataBind();


                grdAppointment.DataSource = ListDisplayAppTime.OrderBy(o => o.Time).ToList();
                grdAppointment.DataBind();

                if (ListDisplayAppTime.Count() == 0 && ListDisplayAppTimeWithoutTimeSlot.Count() == 0)
                {
                    String filterExpresion = String.Format("ParamedicID = {0} AND IsDeleted = 0 AND ('{1}' BETWEEN StartDate AND EndDate)", ParamedicID, selectedDate.ToString("yyyyMMdd"));
                    vParamedicLeaveSchedule leaveEntity = BusinessLayer.GetvParamedicLeaveScheduleList(filterExpresion).FirstOrDefault();

                    if (leaveEntity != null)
                    {
                        trLeaveInformation.Style.Remove("display");
                        string startDate = leaveEntity.StartDate.ToString(Constant.FormatString.DATE_FORMAT) + " " + leaveEntity.StartTime;
                        string endDate = leaveEntity.EndDate.ToString(Constant.FormatString.DATE_FORMAT) + " " + leaveEntity.EndTime;
                        string leaveDateInformation = string.Format("<i>Mulai dari tanggal : </i><b>{0}</b> <i>sampai dengan</i> <b>{1}</b>",startDate,endDate);
                        lblLeaveDateInfo.InnerHtml = leaveDateInformation;

                        string leaveReason = leaveEntity.ParamedicLeaveReason + " (" + leaveEntity.LeaveOtherReason + ")";
                        lblLeaveReason.InnerText = leaveReason;
                    }
                }

                ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
                panel.JSProperties["cpResult"] = selectedRowIndex;
            }
        }

        private void SettingSheduleWaitingList(List<ParamedicScheduleCustom> lstParamedicScheduleCustom, string start)
        {
            List<vAppointment> lstTempAppointment = lstAppointment.Where(t => t.IsWaitingList && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.StartTime == start).OrderBy(a => a.QueueNo).ToList();
            int ct = 1;
            foreach (vAppointment entity in lstTempAppointment)
            {
                ParamedicScheduleCustom newEntity = new ParamedicScheduleCustom();
                newEntity.Queue = ct;
                entity.cfQueue = ct;
                lstParamedicScheduleCustom.Add(newEntity);
                ct++;
            }
            lstParamedicScheduleCustom.Add(new ParamedicScheduleCustom()
            {
                Queue = ct
            });
        }

        private void SettingShedulegrdAppointmentNoTimeSlotList(List<ParamedicScheduleCustom> lstParamedicScheduleCustom, string start, string end, int maximumAppointment, short nonBPJSStartNo, short nonBPJSEndNo, short bpjsStartNo,short bpjsEndNo)
        {
            int totalSlot = maximumAppointment == 0 ? 100 : maximumAppointment;

            #region Create Appointment Slot
            for (int i = 1; i <= totalSlot; i++)
            {
                List<AppointmentCustomClass> lstNoTimeSlot = (from p in lstAppointment.Where(p => p.QueueNo == i && !p.IsWaitingList)
                                                              select new AppointmentCustomClass { AppointmentID = p.AppointmentID, AppointmentNo = p.AppointmentNo, GCAppointmentStatus = p.GCAppointmentStatus, PatientImageUrl = p.PatientImageUrl, PatientName = p.cfPatientName, EstimatedTimeService = string.Format("{0} - {1}", p.StartTime, p.EndTime), VisitTypeName = p.VisitTypeName, StartTime = p.StartTime, EndTime = p.EndTime, CreatedInfo = string.Format("Dibuat : {0} ({1} {2})", p.CreatedBy, p.CreatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), p.CreatedDate.ToString(Constant.FormatString.TIME_FORMAT)), LastUpdatedInfo = string.Format("Terakhir Diubah : {0} {1}", p.LastUpdateBy, p.cfLastUpdatedDateInString), AppointmentStatus = string.Format("Status : {0}", p.AppointmentStatus), IsVip = p.IsVip }).ToList();

                bool isEmpty = lstNoTimeSlot.Count == 0;
                bool isBPJS = false;

                if (bpjsStartNo > 0 && bpjsEndNo > 0)
                {
                    if (i >= bpjsStartNo && i <= bpjsEndNo)
                    {
                        isBPJS = true;
                    }
                }


                ParamedicScheduleCustom newEntity = new ParamedicScheduleCustom() {
                    Queue = i,                    
                    IsBPJS = isBPJS,
                    IsEmpty = isEmpty
                };
                lstParamedicScheduleCustom.Add(newEntity);
            }
            #endregion
        }

        protected void grdAppointment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ParamedicScheduleCustom obj = e.Row.DataItem as ParamedicScheduleCustom;
            if (e.Row.RowType == DataControlRowType.DataRow && obj.Time != null)
            {
                List<vAppointment> lstTemp = new List<vAppointment>();
                List<vAppointment> lstMaster = lstAppointment.Where(t => t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).ToList();
                for (int i = 0; i < lstMaster.Count; i++)
                {
                    if (obj.Time != null)
                    {
                        vAppointment a = lstMaster[i];

                        int minuteObj = Convert.ToInt32(obj.Time.Substring(3));
                        int hourObj = Convert.ToInt32(obj.Time.Substring(0, 2));

                        int minuteAppoStart = Convert.ToInt32(a.StartTime.Substring(3));
                        int hourAppoStart = Convert.ToInt32(a.StartTime.Substring(0, 2));
                        int minuteAppoEnd = Convert.ToInt32(a.EndTime.Substring(3));
                        int hourAppoEnd = Convert.ToInt32(a.EndTime.Substring(0, 2));

                        DateTime stAppo = new DateTime(a.StartDate.Year, a.StartDate.Month, a.StartDate.Day, hourAppoStart, minuteAppoStart, 0);
                        DateTime enAppo = new DateTime(a.StartDate.Year, a.StartDate.Month, a.StartDate.Day, hourAppoEnd, minuteAppoEnd, 0);
                        DateTime stObj = new DateTime(a.StartDate.Year, a.StartDate.Month, a.StartDate.Day, hourObj, minuteObj, 0);

                        ParamedicMaster pm = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnParamedicID.Value));
                        int serviceInterval = 0;
                        serviceInterval = Convert.ToInt16(pm.VisitDurationDefault);

                        DateTime enObj = stObj.AddMinutes(serviceInterval);

                        if (stAppo.TimeOfDay >= stObj.TimeOfDay && enAppo.TimeOfDay <= enObj.TimeOfDay)
                        {
                            if (!a.IsWaitingList)
                            {
                                lstTemp.Add(a);
                            }
                        }
                        else if (stAppo.TimeOfDay == stObj.TimeOfDay && enAppo.TimeOfDay > enObj.TimeOfDay)
                        {
                            if (!a.IsWaitingList)
                            {
                                lstTemp.Add(a);
                            }
                        }
                        else if (stAppo.TimeOfDay <= stObj.TimeOfDay && enAppo.TimeOfDay <= enObj.TimeOfDay && enAppo.TimeOfDay > stObj.TimeOfDay)
                        {
                            if (!a.IsWaitingList)
                            {
                                lstTemp.Add(a);
                            }
                        }
                        else if (stAppo.TimeOfDay < stObj.TimeOfDay && enAppo.TimeOfDay > enObj.TimeOfDay)
                        {
                            if (!a.IsWaitingList)
                            {
                                lstTemp.Add(a);
                            }
                        }
                    }
                }

                List<AppointmentCustomClass> lstBindAppointment = (from p in lstTemp.ToList()
                                                                   select new AppointmentCustomClass { AppointmentID = p.AppointmentID, AppointmentNo = p.AppointmentNo, GCAppointmentStatus = p.GCAppointmentStatus, PatientImageUrl = p.PatientImageUrl, PatientName = p.cfPatientName, VisitTypeName = p.VisitTypeName, StartTime = p.StartTime, EndTime = p.EndTime, CreatedInfo = string.Format("Dibuat : {0} ({1} {2})", p.CreatedBy, p.CreatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), p.CreatedDate.ToString(Constant.FormatString.TIME_FORMAT)), LastUpdatedInfo = string.Format("Terakhir Diubah : {0} {1}", p.LastUpdateBy, p.cfLastUpdatedDateInString), AppointmentStatus = string.Format("Status : {0}", p.AppointmentStatus), IsVip = p.IsVip }).ToList();
                AppointmentCustomClass app = new AppointmentCustomClass();

                if (obj.Time == null)
                {
                    app.AppointmentID = -2;
                    lstBindAppointment.Add(app);
                }
                else if (lstBindAppointment.Count() == 0)
                {
                    app.AppointmentID = -1;
                    app.AppointmentNo = "";
                    app.GCAppointmentStatus = "";
                    app.VisitTypeName = "";
                    app.PatientName = "";
                    lstBindAppointment.Add(app);
                }
                Repeater rpt = e.Row.FindControl("rptAppointmentInformation") as Repeater;
                rpt.DataSource = lstBindAppointment;
                rpt.DataBind();
            }
        }

        protected void grdAppointmentNoTimeSlot_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ParamedicScheduleCustom obje = e.Row.DataItem as ParamedicScheduleCustom;
                List<AppointmentCustomClass> lstNoTimeSlot = (from p in lstAppointment.Where(p => p.QueueNo == obje.Queue && !p.IsWaitingList)
                                                              select new AppointmentCustomClass { AppointmentID = p.AppointmentID, AppointmentNo = p.AppointmentNo, GCAppointmentStatus = p.GCAppointmentStatus, PatientImageUrl = p.PatientImageUrl, PatientName = p.cfPatientName, EstimatedTimeService = string.Format("{0} - {1}", p.StartTime, p.EndTime), VisitTypeName = p.VisitTypeName, StartTime = p.StartTime, EndTime = p.EndTime, CreatedInfo = string.Format("Dibuat : {0} ({1} {2})", p.CreatedBy, p.CreatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), p.CreatedDate.ToString(Constant.FormatString.TIME_FORMAT)), LastUpdatedInfo = string.Format("Terakhir Diubah : {0} {1}", p.LastUpdateBy, p.cfLastUpdatedDateInString), AppointmentStatus = string.Format("Status : {0}", p.AppointmentStatus), IsVip = p.IsVip }).ToList();

                System.Drawing.Color foreColor = System.Drawing.Color.Black;
                System.Drawing.Color backColor = System.Drawing.Color.White;

                if (lstNoTimeSlot.Count == 0)
                {
                    AppointmentCustomClass app = new AppointmentCustomClass();
                    app.AppointmentID = -1;
                    app.AppointmentNo = "";
                    app.GCAppointmentStatus = "";
                    app.VisitTypeName = "";
                    app.PatientName = "";
                    lstNoTimeSlot.Add(app);
                }
                else
                {
                    obje.IsEmpty = false;
                    obje.IsBPJS = lstNoTimeSlot[0].GCCustomerType == AppSession.TipeCustomerBPJS;
                }

                if (obje.IsBPJS)
                {
                    backColor = System.Drawing.ColorTranslator.FromHtml("#adff2f");
                    foreColor = System.Drawing.Color.Black;
                }

                e.Row.Cells[1].ToolTip = "Slot untuk Pasien BPJS";
                e.Row.Cells[1].BackColor = backColor;
                e.Row.Cells[1].ForeColor = foreColor;
                e.Row.Cells[2].ToolTip = "Slot untuk Pasien BPJS";
                e.Row.Cells[2].BackColor = backColor;
                e.Row.Cells[2].ForeColor = foreColor;
                e.Row.Cells[3].ToolTip = "Slot untuk Pasien BPJS";
                e.Row.Cells[3].BackColor = backColor;
                e.Row.Cells[3].ForeColor = foreColor;

                Repeater rpt = e.Row.FindControl("rptAppointmentInformation") as Repeater;
                rpt.DataSource = lstNoTimeSlot;
                rpt.DataBind();
            }
        }

        protected void grdWaitingList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ParamedicScheduleCustom obje = e.Row.DataItem as ParamedicScheduleCustom;
                List<AppointmentCustomClass> lstBindWaitingList = (from p in lstAppointment.Where(p => p.cfQueue == obje.Queue && p.IsWaitingList && p.GCAppointmentStatus != Constant.AppointmentStatus.DELETED)
                                                                   select new AppointmentCustomClass { AppointmentID = p.AppointmentID, AppointmentNo = p.AppointmentNo, GCAppointmentStatus = p.GCAppointmentStatus, PatientImageUrl = p.PatientImageUrl, PatientName = p.cfPatientName, VisitTypeName = p.VisitTypeName, StartTime = p.StartTime, EndTime = p.EndTime, CreatedInfo = string.Format("Dibuat : {0} ({1} {2})", p.CreatedBy, p.CreatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), p.CreatedDate.ToString(Constant.FormatString.TIME_FORMAT)), LastUpdatedInfo = string.Format("Terakhir Diubah : {0} {1}", p.LastUpdateBy, p.cfLastUpdatedDateInString), AppointmentStatus = string.Format("Status : {0}", p.AppointmentStatus), IsVip = p.IsVip }).ToList();
                if (lstBindWaitingList.Count == 0)
                {
                    AppointmentCustomClass app = new AppointmentCustomClass();
                    app.AppointmentID = -1;
                    app.AppointmentNo = "";
                    app.GCAppointmentStatus = "";
                    app.VisitTypeName = "";
                    app.PatientName = "";
                    lstBindWaitingList.Add(app);
                }

                Repeater rpt = e.Row.FindControl("rptAppointmentInformation") as Repeater;
                rpt.DataSource = lstBindWaitingList;
                rpt.DataBind();
            }
        }

        private void ControlToEntity(Appointment entity)
        {
            entity.HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
            entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            entity.StartDate = entity.EndDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);

            if (!chkIsNewPatient.Checked)
            {
                entity.MRN = Convert.ToInt32(hdnMRN.Value);
                entity.IsNewPatient = false;
            }
            else
            {
                entity.IsNewPatient = true;
                if (cboSalutationAppo.Value != null)
                    entity.GCSalutation = (string)cboSalutationAppo.Value;
                else
                    entity.GCSalutation = null;
                entity.FirstName = txtFirstName.Text;
                entity.MiddleName = txtMiddleName.Text;
                entity.LastName = txtFamilyName.Text;
                entity.GCGender = (string)cboGenderAppointment.Value;
                entity.StreetName = txtAddress.Text;
                entity.PhoneNo = txtPhoneNo.Text;
                entity.MobilePhoneNo = txtMobilePhone.Text;
                entity.EmailAddress = txtEmail.Text;
                entity.CorporateAccountNo = txtCorporateAccountNo.Text;
                entity.CorporateAccountName = txtCorporateAccountName.Text;
                entity.Name = Helper.GenerateName(entity.LastName, entity.MiddleName, entity.FirstName);
            }
            entity.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
            entity.VisitDuration = Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]);
            entity.Notes = txtRemarks.Text;
            entity.CorporateAccountNo = txtCorporateAccountNo.Text;
            entity.CorporateAccountName = txtCorporateAccountName.Text;
            entity.GCAppointmentMethod = (string)cboAppointmentMethod.Value;
            entity.Session = Convert.ToInt32(cboSession.Value) + 1;

            if (cboResultDeliveryPlan.Value != null)
            {
                if (cboResultDeliveryPlan.Value.ToString() != "")
                {
                    entity.GCResultDeliveryPlan = cboResultDeliveryPlan.Value.ToString();
                    if (cboResultDeliveryPlan.Value.ToString() == Constant.ResultDeliveryPlan.OTHERS)
                    {
                        entity.ResultDeliveryPlanOthers = txtResultDeliveryPlanOthers.Text;
                    }
                    else
                    {
                        entity.ResultDeliveryPlanOthers = null;
                    }
                }
                else
                {
                    entity.GCResultDeliveryPlan = null;
                    entity.ResultDeliveryPlanOthers = null;
                }
            }
            else
            {
                entity.GCResultDeliveryPlan = null;
                entity.ResultDeliveryPlanOthers = null;
            }

        }

        private void ControlToEntityGuest(Guest entity)
        {
            entity.GuestNo = BusinessLayer.GenerateGuestNo(DateTime.Now);
            entity.FirstName = txtFirstName.Text;
            entity.MiddleName = txtMiddleName.Text;
            entity.LastName = txtFamilyName.Text;
            entity.Name = Helper.GenerateName(entity.LastName, entity.MiddleName, entity.FirstName);
            if (cboSalutationAppo.Value != null)
                entity.GCSalutation = (string)cboSalutationAppo.Value;
            else
                entity.GCSalutation = null;
            entity.GCGender = (string)cboGenderAppointment.Value;
            entity.StreetName = txtAddress.Text;
            entity.PhoneNo = txtPhoneNo.Text;
            entity.MobilePhoneNo = txtMobilePhone.Text;
            entity.EmailAddress = txtEmail.Text;
            if (Helper.GetDatePickerValue(Request.Form[txtDOBMainAppt.UniqueID]).ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                entity.DateOfBirth = Helper.GetDatePickerValue(Request.Form[txtDOBMainAppt.UniqueID]);
            }
            entity.CorporateAccountNo = txtCorporateAccountNo.Text;
            entity.CorporateAccountName = txtCorporateAccountName.Text;
            entity.IsDeleted = false;
        }

        private void ValidateParamedicScSchedule(vParamedicSchedule obj, vParamedicScheduleDate objSchDate)
        {
            Int32 ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
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

        private void UpdatePatientData(IDbContext ctx, int MRN)
        {
            PatientDao entityPatientDao = new PatientDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            Patient entityPatient = entityPatientDao.Get(MRN);
            entityPatient.MobilePhoneNo1 = txtMobilePhone.Text;
            entityPatient.EmailAddress = txtEmail.Text;

            Address entityAddress = entityAddressDao.Get((int)entityPatient.HomeAddressID);
            entityAddress.PhoneNo1 = txtPhoneNo.Text;
            entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
            entityPatient.LastUpdatedBy = AppSession.UserLogin.UserID;

            entityPatientDao.Update(entityPatient);
            entityAddressDao.Update(entityAddress);
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            bool isValidMRN = true;
            bool newQueueValid = true;
            int newQueue = 0;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao entityDao = new AppointmentDao(ctx);
            GuestDao entityGuestDao = new GuestDao(ctx);
            ServiceUnitParamedicDao entityServiceUnitParamedicDao = new ServiceUnitParamedicDao(ctx);
            Appointment entity = null;
            Guest entityGuest = null;

            if (type == "save")
            {
                Int32 HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
                Int32 ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                String appointmentID = "";

                int dayNumber = (int)selectedDate.DayOfWeek;
                if (dayNumber == 0)
                {
                    dayNumber = 7;
                }

                vParamedicSchedule obj = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                            "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                            HealthcareServiceUnitID, ParamedicID, dayNumber)).FirstOrDefault();

                vParamedicScheduleDate objSchDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                                "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                                HealthcareServiceUnitID, ParamedicID, selectedDate)).FirstOrDefault();

                if (hdnIsBridgingToGateway.Value == "1")
                {
                    //Healthcare entityHSU = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
                    if (Constant.HealthcareGatewayProvider.RSDOSOBA == hdnProviderGatewayService.Value)
                    {
                        string queue = string.Empty;
                        if (objSchDate != null)
                        {
                            queue = BridgingToGatewayGetQueueNo(txtMRN.Text, objSchDate.ParamedicCode, cboAppointmentPayer.Value.ToString(), selectedDate, objSchDate.StartTime1, "BT", HealthcareServiceUnitID.ToString(), Convert.ToInt32(cboSession.Value.ToString()));
                        }
                        else if (obj != null){
                            queue = BridgingToGatewayGetQueueNo(txtMRN.Text, obj.ParamedicCode, cboAppointmentPayer.Value.ToString(), selectedDate, obj.StartTime1, "BT", HealthcareServiceUnitID.ToString(), Convert.ToInt32(cboSession.Value.ToString()));
                        }

                        string[] queueSplit = queue.Split('|');
                        if (queueSplit[0] == "1")
                        {
                            newQueue = Convert.ToInt16(queueSplit[1]);
                        }
                        else
                        {
                            errMessage = queueSplit[1];
                            newQueueValid = false;
                            //result = false;
                        }
                    }
                }

                try
                {
                    if (newQueueValid)
                    {
                        #region AppointmentValid
                        #region Master Schedule

                        ParamedicMaster pm = BusinessLayer.GetParamedicMaster(ParamedicID);

                        ValidateParamedicScSchedule(obj, objSchDate);

                        #endregion

                        #region validate start and end time Appointment
                        int hour = 0;
                        int minute = 0;
                        string startTimeValidInString = "";
                        DateTime stAppo = DateTime.Now;
                        DateTime stAppoValid = DateTime.Now;
                        DateTime enAppo = DateTime.Now;
                        string timeSlot = cboSession.Value.ToString();
                        string startTimeCheck = "";
                        string endTimeCheck = "";

                        if (hdnIsByTimeSlot.Value == "1")
                        {
                            hour = Convert.ToInt32(Request.Form[txtAppointmentHour.UniqueID].Substring(0, 2));
                            minute = Convert.ToInt32(Request.Form[txtAppointmentHour.UniqueID].Substring(3));
                            startTimeValidInString = Request.Form[txtAppointmentHour.UniqueID];

                            if (timeSlot == "0")
                            {
                                if (objSchDate != null)
                                {
                                    if (objSchDate.StartTime1 != "")
                                    {
                                        startTimeCheck = objSchDate.StartTime1;
                                        endTimeCheck = objSchDate.EndTime1;
                                    }
                                }
                                else if (obj != null)
                                {
                                    if (obj.StartTime1 != "")
                                    {
                                        startTimeCheck = obj.StartTime1;
                                        endTimeCheck = obj.EndTime1;
                                    }
                                }
                            }
                            else if (timeSlot == "1")
                            {
                                if (objSchDate != null)
                                {
                                    if (objSchDate.StartTime2 != "")
                                    {
                                        startTimeCheck = objSchDate.StartTime2;
                                        endTimeCheck = objSchDate.EndTime2;
                                    }
                                }
                                else if (obj != null)
                                {
                                    if (obj.StartTime2 != "")
                                    {
                                        startTimeCheck = obj.StartTime2;
                                        endTimeCheck = obj.EndTime2;
                                    }
                                }
                            }
                            else if (timeSlot == "2")
                            {
                                if (objSchDate != null)
                                {
                                    if (objSchDate.StartTime3 != "")
                                    {
                                        startTimeCheck = objSchDate.StartTime3;
                                        endTimeCheck = objSchDate.EndTime3;
                                    }
                                }
                                else if (obj != null)
                                {
                                    if (obj.StartTime3 != "")
                                    {
                                        startTimeCheck = obj.StartTime3;
                                        endTimeCheck = obj.EndTime3;
                                    }
                                }
                            }
                            else if (timeSlot == "3")
                            {
                                if (objSchDate != null)
                                {
                                    if (objSchDate.StartTime4 != "")
                                    {
                                        startTimeCheck = objSchDate.StartTime4;
                                        endTimeCheck = objSchDate.EndTime4;
                                    }
                                }
                                else if (obj != null)
                                {
                                    if (obj.StartTime4 != "")
                                    {
                                        startTimeCheck = obj.StartTime4;
                                        endTimeCheck = obj.EndTime4;
                                    }
                                }
                            }
                            else if (timeSlot == "4")
                            {
                                if (objSchDate != null)
                                {
                                    if (objSchDate.StartTime5 != "")
                                    {
                                        startTimeCheck = objSchDate.StartTime5;
                                        endTimeCheck = objSchDate.EndTime5;
                                    }
                                }
                                else if (obj != null)
                                {
                                    if (obj.StartTime5 != "")
                                    {
                                        startTimeCheck = obj.StartTime5;
                                        endTimeCheck = obj.EndTime5;
                                    }
                                }
                            }
                        }
                        else if (hdnIsByNoTimeSlot.Value == "1")
                        {
                            if (timeSlot == "0")
                            {
                                if (objSchDate != null)
                                {
                                    if (objSchDate.StartTime1 != "")
                                    {
                                        String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(cboServiceUnit.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).ToString("yyyy-MM-dd"), objSchDate.StartTime1, objSchDate.EndTime1);
                                        List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                        startTimeCheck = objSchDate.StartTime1;
                                        endTimeCheck = objSchDate.EndTime1;

                                        if (!String.IsNullOrEmpty(hdnMRN.Value))
                                        {
                                            if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                            else
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                        }

                                        if (lstAppo.Count > 0)
                                        {
                                            int duration = 0;
                                            foreach (Appointment a in lstAppo)
                                            {
                                                duration = duration + a.VisitDuration;
                                            }

                                            //set jam mulai dan jam selesai Appointment
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                            minute = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                            //end
                                        }
                                        else
                                        {
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                            minute = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo;
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                        }
                                    }
                                }
                                else if (obj != null)
                                {
                                    if (obj.StartTime1 != "")
                                    {
                                        String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(cboServiceUnit.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).ToString("yyyy-MM-dd"), obj.StartTime1, obj.EndTime1);
                                        List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                        startTimeCheck = obj.StartTime1;
                                        endTimeCheck = obj.EndTime1;

                                        if (!String.IsNullOrEmpty(hdnMRN.Value))
                                        {
                                            if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                            else
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                        }

                                        if (lstAppo.Count > 0)
                                        {
                                            int duration = 0;
                                            foreach (Appointment a in lstAppo)
                                            {
                                                duration = duration + a.VisitDuration;
                                            }

                                            //set jam mulai dan jam selesai Appointment
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                            minute = Convert.ToInt32(obj.StartTime1.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                            //end
                                        }
                                        else
                                        {
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                            minute = Convert.ToInt32(obj.StartTime1.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo;
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                        }
                                    }
                                }
                            }
                            else if (timeSlot == "1")
                            {
                                if (objSchDate != null)
                                {
                                    if (objSchDate.StartTime2 != "")
                                    {
                                        String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(cboServiceUnit.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).ToString("yyyy-MM-dd"), objSchDate.StartTime2, objSchDate.EndTime2);
                                        List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                        startTimeCheck = objSchDate.StartTime2;
                                        endTimeCheck = objSchDate.EndTime2;

                                        if (!String.IsNullOrEmpty(hdnMRN.Value))
                                        {
                                            if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                            else
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                        }

                                        if (lstAppo.Count > 0)
                                        {
                                            int duration = 0;
                                            foreach (Appointment a in lstAppo)
                                            {
                                                duration = duration + a.VisitDuration;
                                            }

                                            //set jam mulai dan jam selesai Appointment
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(objSchDate.StartTime2.Substring(0, 2));
                                            minute = Convert.ToInt32(objSchDate.StartTime2.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                            //end
                                        }
                                        else
                                        {
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(objSchDate.StartTime2.Substring(0, 2));
                                            minute = Convert.ToInt32(objSchDate.StartTime2.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo;
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                        }
                                    }
                                }
                                else if (obj != null)
                                {
                                    if (obj.StartTime2 != "")
                                    {
                                        String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(cboServiceUnit.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).ToString("yyyy-MM-dd"), obj.StartTime2, obj.EndTime2);
                                        List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                        startTimeCheck = obj.StartTime2;
                                        endTimeCheck = obj.EndTime2;

                                        if (!String.IsNullOrEmpty(hdnMRN.Value))
                                        {
                                            if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                            else
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                        }

                                        if (lstAppo.Count > 0)
                                        {
                                            int duration = 0;
                                            foreach (Appointment a in lstAppo)
                                            {
                                                duration = duration + a.VisitDuration;
                                            }

                                            //set jam mulai dan jam selesai Appointment
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                            minute = Convert.ToInt32(obj.StartTime2.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                            //end
                                        }
                                        else
                                        {
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                            minute = Convert.ToInt32(obj.StartTime2.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo;
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                        }
                                    }
                                }
                            }
                            else if (timeSlot == "2")
                            {
                                if (objSchDate != null)
                                {
                                    if (objSchDate.StartTime3 != "")
                                    {
                                        String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(cboServiceUnit.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).ToString("yyyy-MM-dd"), objSchDate.StartTime3, objSchDate.EndTime3);
                                        List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                        startTimeCheck = objSchDate.StartTime3;
                                        endTimeCheck = objSchDate.EndTime3;

                                        if (!String.IsNullOrEmpty(hdnMRN.Value))
                                        {
                                            if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                            else
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                        }

                                        if (lstAppo.Count > 0)
                                        {
                                            int duration = 0;
                                            foreach (Appointment a in lstAppo)
                                            {
                                                duration = duration + a.VisitDuration;
                                            }

                                            //set jam mulai dan jam selesai Appointment
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                            minute = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                            //end
                                        }
                                        else
                                        {
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                            minute = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo;
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                        }
                                    }
                                }
                                else if (obj != null)
                                {
                                    if (obj.StartTime3 != "")
                                    {
                                        String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(cboServiceUnit.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).ToString("yyyy-MM-dd"), obj.StartTime3, obj.EndTime3);
                                        List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                        startTimeCheck = obj.StartTime3;
                                        endTimeCheck = obj.EndTime3;

                                        if (!String.IsNullOrEmpty(hdnMRN.Value))
                                        {
                                            if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                            else
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                        }

                                        if (lstAppo.Count > 0)
                                        {
                                            int duration = 0;
                                            foreach (Appointment a in lstAppo)
                                            {
                                                duration = duration + a.VisitDuration;
                                            }

                                            //set jam mulai dan jam selesai Appointment
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                            minute = Convert.ToInt32(obj.StartTime3.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                            //end
                                        }
                                        else
                                        {
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                            minute = Convert.ToInt32(obj.StartTime3.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo;
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                        }
                                    }
                                }
                            }
                            else if (timeSlot == "3")
                            {
                                if (objSchDate != null)
                                {
                                    if (objSchDate.StartTime4 != "")
                                    {
                                        String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(cboServiceUnit.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).ToString("yyyy-MM-dd"), objSchDate.StartTime4, objSchDate.EndTime4);
                                        List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                        startTimeCheck = objSchDate.StartTime4;
                                        endTimeCheck = objSchDate.EndTime4;

                                        if (!String.IsNullOrEmpty(hdnMRN.Value))
                                        {
                                            if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                            else
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                        }

                                        if (lstAppo.Count > 0)
                                        {
                                            int duration = 0;
                                            foreach (Appointment a in lstAppo)
                                            {
                                                duration = duration + a.VisitDuration;
                                            }

                                            //set jam mulai dan jam selesai Appointment
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(objSchDate.StartTime4.Substring(0, 2));
                                            minute = Convert.ToInt32(objSchDate.StartTime4.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                            //end
                                        }
                                        else
                                        {
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(objSchDate.StartTime4.Substring(0, 2));
                                            minute = Convert.ToInt32(objSchDate.StartTime4.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo;
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                        }
                                    }
                                }
                                else if (obj != null)
                                {
                                    if (obj.StartTime4 != "")
                                    {
                                        String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(cboServiceUnit.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).ToString("yyyy-MM-dd"), obj.StartTime4, obj.EndTime4);
                                        List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                        startTimeCheck = obj.StartTime4;
                                        endTimeCheck = obj.EndTime4;

                                        if (!String.IsNullOrEmpty(hdnMRN.Value))
                                        {
                                            if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                            else
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                        }

                                        if (lstAppo.Count > 0)
                                        {
                                            int duration = 0;
                                            foreach (Appointment a in lstAppo)
                                            {
                                                duration = duration + a.VisitDuration;
                                            }

                                            //set jam mulai dan jam selesai Appointment
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(obj.StartTime4.Substring(0, 2));
                                            minute = Convert.ToInt32(obj.StartTime4.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                            //end
                                        }
                                        else
                                        {
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(obj.StartTime4.Substring(0, 2));
                                            minute = Convert.ToInt32(obj.StartTime4.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo;
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                        }
                                    }
                                }
                            }
                            else if (timeSlot == "4")
                            {
                                if (objSchDate != null)
                                {
                                    if (objSchDate.StartTime5 != "")
                                    {
                                        String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(cboServiceUnit.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).ToString("yyyy-MM-dd"), objSchDate.StartTime5, objSchDate.EndTime5);
                                        List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                        startTimeCheck = objSchDate.StartTime5;
                                        endTimeCheck = objSchDate.EndTime5;

                                        if (!String.IsNullOrEmpty(hdnMRN.Value))
                                        {
                                            if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                            else
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                        }

                                        if (lstAppo.Count > 0)
                                        {
                                            int duration = 0;
                                            foreach (Appointment a in lstAppo)
                                            {
                                                duration = duration + a.VisitDuration;
                                            }

                                            //set jam mulai dan jam selesai Appointment
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(objSchDate.StartTime5.Substring(0, 2));
                                            minute = Convert.ToInt32(objSchDate.StartTime5.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                            //end
                                        }
                                        else
                                        {
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(objSchDate.StartTime5.Substring(0, 2));
                                            minute = Convert.ToInt32(objSchDate.StartTime5.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo;
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                        }
                                    }
                                }
                                else if (obj != null)
                                {
                                    if (obj.StartTime5 != "")
                                    {
                                        String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(cboServiceUnit.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).ToString("yyyy-MM-dd"), obj.StartTime5, obj.EndTime5);
                                        List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                        startTimeCheck = obj.StartTime5;
                                        endTimeCheck = obj.EndTime5;

                                        if (!String.IsNullOrEmpty(hdnMRN.Value))
                                        {
                                            if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                            else
                                            {
                                                if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                                {
                                                    isValidMRN = false;
                                                }
                                            }
                                        }

                                        if (lstAppo.Count > 0)
                                        {
                                            int duration = 0;
                                            foreach (Appointment a in lstAppo)
                                            {
                                                duration = duration + a.VisitDuration;
                                            }

                                            //set jam mulai dan jam selesai Appointment
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(obj.StartTime5.Substring(0, 2));
                                            minute = Convert.ToInt32(obj.StartTime5.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                            //end
                                        }
                                        else
                                        {
                                            DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
                                            hour = Convert.ToInt32(obj.StartTime5.Substring(0, 2));
                                            minute = Convert.ToInt32(obj.StartTime5.Substring(3));

                                            stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                            stAppoValid = stAppo;
                                            enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                        }
                                    }
                                }
                            }
                        }

                        entity = new Appointment();
                        entityGuest = new Guest();
                        if (hdnAppointmentID.Value == "")
                        {
                            ControlToEntity(entity);
                            ControlToEntityGuest(entityGuest);
                        }
                        else
                        {
                            entity = entityDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                            entity.VisitDuration = Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]);
                        }

                        if (cboReferral.Value != null)
                        {
                            entity.GCReferrerGroup = cboReferral.Value.ToString();
                        }

                        if (hdnReferrerID.Value != "" && hdnReferrerID.Value != "0")
                        {
                            entity.ReferrerID = Convert.ToInt32(hdnReferrerID.Value);
                            entity.ReferrerParamedicID = null;
                        }
                        else if (hdnReferrerParamedicID.Value != "" && hdnReferrerParamedicID.Value != "0")
                        {
                            entity.ReferrerID = null;
                            entity.ReferrerParamedicID = Convert.ToInt32(hdnReferrerParamedicID.Value);
                        }
                        else
                        {
                            entity.ReferrerID = null;
                            entity.ReferrerParamedicID = null;
                        }

                        string endTimeValidInString = "";

                        if (hdnIsWaitingList.Value == "0")
                        {
                            if (objSchDate != null)
                            {
                                if (objSchDate.StartTime1 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot1 == false)
                                {
                                    //startTimeValidInString = objSchDate.StartTime1;
                                    endTimeValidInString = objSchDate.EndTime1;
                                }
                                else if (objSchDate.StartTime2 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot2 == false)
                                {
                                    //startTimeValidInString = objSchDate.StartTime2;
                                    endTimeValidInString = objSchDate.EndTime2;
                                }
                                else if (objSchDate.StartTime3 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot3 == false)
                                {
                                    //startTimeValidInString = objSchDate.StartTime3;
                                    endTimeValidInString = objSchDate.EndTime3;
                                }
                                else if (objSchDate.StartTime4 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot4 == false)
                                {
                                    //startTimeValidInString = objSchDate.StartTime4;
                                    endTimeValidInString = objSchDate.EndTime4;
                                }
                                else if (objSchDate.StartTime5 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot5 == false)
                                {
                                    //startTimeValidInString = objSchDate.StartTime5;
                                    endTimeValidInString = objSchDate.EndTime5;
                                }
                                else
                                {
                                    DateTime dtEndTime = new DateTime(2000, 1, 1, hour, minute, 0);
                                    DateTime dtEndTimeValid = dtEndTime.AddMinutes(Convert.ToDouble(pm.VisitDurationDefault));
                                    endTimeValidInString = Convert.ToString(dtEndTimeValid.Hour) + ":" + Convert.ToString(dtEndTimeValid.Minute);
                                }
                            }
                            else if (obj != null && objSchDate == null)
                            {
                                if (obj.StartTime1 == startTimeValidInString && obj.IsAppointmentByTimeSlot1 == false)
                                {
                                    //startTimeValidInString = obj.StartTime1;
                                    endTimeValidInString = obj.EndTime1;
                                }
                                else if (obj.StartTime2 == startTimeValidInString && obj.IsAppointmentByTimeSlot2 == false)
                                {
                                    //startTimeValidInString = obj.StartTime2;
                                    endTimeValidInString = obj.EndTime2;
                                }
                                else if (obj.StartTime3 == startTimeValidInString && obj.IsAppointmentByTimeSlot3 == false)
                                {
                                    startTimeValidInString = obj.StartTime3;
                                    endTimeValidInString = obj.EndTime3;
                                }
                                else if (obj.StartTime4 == startTimeValidInString && obj.IsAppointmentByTimeSlot4 == false)
                                {
                                    //startTimeValidInString = obj.StartTime4;
                                    endTimeValidInString = obj.EndTime4;
                                }
                                else if (obj.StartTime5 == startTimeValidInString && obj.IsAppointmentByTimeSlot5 == false)
                                {
                                    //startTimeValidInString = obj.StartTime5;
                                    endTimeValidInString = obj.EndTime5;
                                }
                                else
                                {
                                    DateTime dtEndTime = new DateTime(2000, 1, 1, hour, minute, 0);
                                    DateTime dtEndTimeValid = dtEndTime.AddMinutes(Convert.ToDouble(pm.VisitDurationDefault));
                                    endTimeValidInString = Convert.ToString(dtEndTimeValid.Hour) + ":" + Convert.ToString(dtEndTimeValid.Minute);
                                }
                            }

                            DateTime end = new DateTime(2000, 1, 1, hour, minute, 0);
                            if (hdnIsByNoTimeSlot.Value == "1")
                            {
                                entity.StartTime = stAppoValid.ToString("HH:mm");
                                entity.EndTime = enAppo.ToString("HH:mm");
                            }
                            else
                            {
                                entity.StartTime = Request.Form[txtAppointmentHour.UniqueID];
                                entity.EndTime = end.AddMinutes(entity.VisitDuration).ToString("HH:mm");
                            }

                        }
                        else
                        {
                            if (objSchDate != null)
                            {
                                if (timeSlot == "0")
                                {
                                    entity.StartTime = startTimeCheck = objSchDate.StartTime1;
                                    entity.EndTime = endTimeCheck = objSchDate.StartTime1;
                                }
                                else if (timeSlot == "1")
                                {
                                    entity.StartTime = startTimeCheck = objSchDate.StartTime2;
                                    entity.EndTime = endTimeCheck = objSchDate.StartTime2;
                                }
                                else if (timeSlot == "2")
                                {
                                    entity.StartTime = startTimeCheck = objSchDate.StartTime3;
                                    entity.EndTime = endTimeCheck = objSchDate.StartTime3;
                                }
                                else if (timeSlot == "3")
                                {
                                    entity.StartTime = startTimeCheck = objSchDate.StartTime4;
                                    entity.EndTime = endTimeCheck = objSchDate.StartTime4;
                                }
                                else if (timeSlot == "4")
                                {
                                    entity.StartTime = startTimeCheck = objSchDate.StartTime5;
                                    entity.EndTime = endTimeCheck = objSchDate.StartTime5;
                                }
                            }
                            else if (obj != null && objSchDate == null)
                            {
                                if (timeSlot == "0")
                                {
                                    entity.StartTime = startTimeCheck = obj.StartTime1;
                                    entity.EndTime = endTimeCheck = obj.StartTime1;
                                }
                                else if (timeSlot == "1")
                                {
                                    entity.StartTime = startTimeCheck = obj.StartTime2;
                                    entity.EndTime = endTimeCheck = obj.StartTime2;
                                }
                                else if (timeSlot == "2")
                                {
                                    entity.StartTime = startTimeCheck = obj.StartTime3;
                                    entity.EndTime = endTimeCheck = obj.StartTime3;
                                }
                                else if (timeSlot == "3")
                                {
                                    entity.StartTime = startTimeCheck = obj.StartTime4;
                                    entity.EndTime = endTimeCheck = obj.StartTime4;
                                }
                                else if (timeSlot == "4")
                                {
                                    entity.StartTime = startTimeCheck = obj.StartTime5;
                                    entity.EndTime = endTimeCheck = obj.StartTime5;
                                }
                            }
                        }
                        #endregion

                        string filterExpression = string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND GCAppointmentStatus != '{2}' AND VisitTypeID = {3} AND isWaitingList = 0 AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", cboServiceUnit.Value, hdnParamedicID.Value, Constant.AppointmentStatus.DELETED, hdnVisitTypeID.Value, Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).ToString("yyyy-MM-dd"), startTimeValidInString, endTimeValidInString);
                        if (hdnAppointmentID.Value != "")
                            filterExpression += string.Format(" AND AppointmentID != {0}", hdnAppointmentID.Value);
                        int count = BusinessLayer.GetAppointmentRowCount(filterExpression, ctx);
                        if (count > 0 && !(hdnIsWaitingList.Value == "1") && !(hdnIsByNoTimeSlot.Value == "1"))
                        {
                            errMessage = string.Format("Jenis Kunjungan {0} sudah digunakan di slot ini. Perjanjian tidak dapat diproses.", Request.Form[txtVisitTypeName.UniqueID]);
                            result = false;
                        }
                        else
                        {
                            if (hdnAppointmentID.Value != "")
                            {
                                if (!chkIsNewPatient.Checked)
                                {
                                    entity.MRN = Convert.ToInt32(hdnMRN.Value);
                                    entity.IsNewPatient = false;
                                }
                                else
                                {
                                    entity.IsNewPatient = true;
                                    if (cboSalutationAppo.Value != null)
                                    {
                                        entity.GCSalutation = (string)cboSalutationAppo.Value;
                                        entityGuest.GCSalutation = (string)cboSalutationAppo.Value;
                                    }
                                    else
                                    {
                                        entity.GCSalutation = null;
                                        entityGuest.GCSalutation = null;
                                    }
                                    entity.FirstName = txtFirstName.Text;
                                    entity.MiddleName = txtMiddleName.Text;
                                    entity.LastName = txtFamilyName.Text;
                                    entity.StreetName = txtAddress.Text;
                                    entity.PhoneNo = txtPhoneNo.Text;
                                    entity.MobilePhoneNo = txtMobilePhone.Text;
                                    entity.EmailAddress = txtEmail.Text;
                                    entity.CorporateAccountNo = txtCorporateAccountNo.Text;
                                    entity.CorporateAccountName = txtCorporateAccountName.Text;
                                    entity.Name = Helper.GenerateName(entity.LastName, entity.MiddleName, entity.FirstName);
                                    entity.GCGender = (string)cboGenderAppointment.Value;
                                    entity.MRN = null;
                                    entity.IsNewPatient = true;

                                    if (entity.GuestID != null)
                                    {
                                        entityGuest = entityGuestDao.Get(Convert.ToInt32(entity.GuestID));
                                    }
                                    else
                                    {
                                        entityGuest = new Guest();
                                        entityGuest.GuestNo = BusinessLayer.GenerateGuestNo(DateTime.Now);
                                    }

                                    entityGuest.FirstName = txtFirstName.Text;
                                    entityGuest.MiddleName = txtMiddleName.Text;
                                    entityGuest.LastName = txtFamilyName.Text;
                                    entityGuest.FullName = Helper.GenerateFullName(entityGuest.FirstName, entityGuest.MiddleName, entityGuest.LastName);
                                    entityGuest.StreetName = txtAddress.Text;
                                    entityGuest.PhoneNo = txtPhoneNo.Text;
                                    entityGuest.MobilePhoneNo = txtMobilePhone.Text;
                                    entityGuest.EmailAddress = txtEmail.Text;
                                    entityGuest.CorporateAccountNo = txtCorporateAccountNo.Text;
                                    entityGuest.CorporateAccountName = txtCorporateAccountName.Text;
                                    entityGuest.GCGender = (string)cboGenderAppointment.Value;

                                    if (Helper.GetDatePickerValue(Request.Form[txtDOBMainAppt.UniqueID]).ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                                    {
                                        entityGuest.DateOfBirth = Helper.GetDatePickerValue(Request.Form[txtDOBMainAppt.UniqueID]);
                                    }
                                    ////if (entityGuest.DateOfBirth.ToString("dd-MMM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                                    ////{
                                    ////    txtDOBMainAppt.Text = entityGuest.cfDateOfBirthInString;
                                    ////}
                                    ////else
                                    ////{
                                    ////    txtDOBMainAppt.Text = "";
                                    ////}
                                    entityGuest.IsDeleted = false;
                                }

                                if (hdnRoomID.Value != "0" && hdnRoomID != null && hdnRoomID.Value != "")
                                {
                                    entity.RoomID = Convert.ToInt32(hdnRoomID.Value);
                                }
                                else
                                {
                                    entity.RoomID = null;
                                }

                                entity.Notes = txtRemarks.Text;
                                entity.IsWaitingList = (hdnIsWaitingList.Value == "1");
                                if (!chkIsNewPatient.Checked)
                                {
                                    UpdatePatientData(ctx, (int)entity.MRN);
                                }
                                else
                                {
                                    if (entity.GuestID != null)
                                    {
                                        entityGuest.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityGuestDao.Update(entityGuest);
                                    }
                                    else
                                    {
                                        entityGuest.CreatedBy = AppSession.UserLogin.UserID;
                                        entityGuestDao.Insert(entityGuest);
                                    }
                                }

                                if (isValidMRN)
                                {
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityDao.Update(entity);
                                    ctx.CommitTransaction();
                                }
                                else
                                {
                                    ctx.RollBackTransaction();
                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                    result = false;
                                }
                            }
                            else
                            {
                                if (!chkIsNewPatient.Checked)
                                {
                                    UpdatePatientData(ctx, (int)entity.MRN);
                                }
                                else
                                {
                                    entityGuest.CreatedBy = AppSession.UserLogin.UserID;
                                    entity.GuestID = entityGuest.GuestID = entityGuestDao.InsertReturnPrimaryKeyID(entityGuest);
                                }

                                if (hdnRoomID.Value != "0" && hdnRoomID != null && hdnRoomID.Value != "")
                                {
                                    entity.RoomID = Convert.ToInt32(hdnRoomID.Value);
                                }
                                else
                                {
                                    entity.RoomID = null;
                                }

                                entity.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                                entity.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                                appointmentID = entity.AppointmentNo = BusinessLayer.GenerateTransactionNo(entity.TransactionCode, entity.StartDate);

                                entity.GCCustomerType = cboAppointmentPayer.Value.ToString();
                                entity.IsUsingCOB = chkIsUsingCOB.Checked;

                                if (entity.GCCustomerType == Constant.CustomerType.PERSONAL)
                                {
                                    entity.BusinessPartnerID = 1;
                                    entity.ContractID = null;
                                    entity.CoverageTypeID = null;
                                    entity.CoverageLimitAmount = 0;
                                    entity.IsCoverageLimitPerDay = false;
                                    entity.GCTariffScheme = hdnGCTariffSchemePersonal.Value;
                                    entity.IsControlClassCare = false;
                                    entity.ControlClassID = null;
                                    entity.EmployeeID = null;
                                }
                                else
                                {
                                    entity.BusinessPartnerID = Convert.ToInt32(hdnPayerID.Value);
                                    entity.ContractID = Convert.ToInt32(hdnContractID.Value);
                                    entity.CoverageTypeID = Convert.ToInt32(hdnCoverageTypeID.Value);
                                    entity.CorporateAccountNo = txtParticipantNo.Text;

                                    if (!String.IsNullOrEmpty(txtCoverageLimit.Text))
                                    {
                                        entity.CoverageLimitAmount = Convert.ToDecimal(txtCoverageLimit.Text);
                                    }
                                    else
                                    {
                                        entity.CoverageLimitAmount = 0;
                                    }

                                    entity.IsCoverageLimitPerDay = chkIsCoverageLimitPerDay.Checked;
                                    entity.GCTariffScheme = hdnGCTariffScheme.Value;
                                    entity.IsControlClassCare = (hdnIsControlClassCare.Value == "1");
                                    if (entity.IsControlClassCare)
                                        entity.ControlClassID = Convert.ToInt32(cboControlClassCare.Value);
                                    else
                                        entity.ControlClassID = null;
                                    if (hdnEmployeeID.Value == "" || hdnEmployeeID.Value == "0")
                                        entity.EmployeeID = null;
                                    else
                                        entity.EmployeeID = Convert.ToInt32(hdnEmployeeID.Value);
                                }

                                #region validate maximum Appoitment & waiting List
                                if (hdnIsWaitingList.Value == "0")
                                {
                                    DateTime AppointmentStart = DateTime.Parse("2012-01-28" + ' ' + entity.StartTime);
                                    DateTime AppointmentEnd = DateTime.Parse("2012-01-28" + ' ' + entity.EndTime);

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

                                        if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 != "")
                                        {
                                            if (AppointmentStart.TimeOfDay >= objSchStart5.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd5.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objSchStart4.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd4.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objSchStart3.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd3.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objSchStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd2.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objSchStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd1.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else
                                            {
                                                ctx.RollBackTransaction();
                                                errMessage = "Durasi Kunjungan Tidak Valid.";
                                                result = false;
                                            }
                                        }
                                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                                        {
                                            if (AppointmentStart.TimeOfDay >= objSchStart4.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd4.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objSchStart3.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd3.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objSchStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd2.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objSchStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd1.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else
                                            {
                                                ctx.RollBackTransaction();
                                                errMessage = "Durasi Kunjungan Tidak Valid.";
                                                result = false;
                                            }
                                        }
                                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                                        {
                                            if (AppointmentStart.TimeOfDay >= objSchStart3.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd3.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objSchStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd2.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objSchStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd1.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else
                                            {
                                                ctx.RollBackTransaction();
                                                errMessage = "Durasi Kunjungan Tidak Valid.";
                                                result = false;
                                            }
                                        }
                                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                                        {
                                            if (AppointmentStart.TimeOfDay >= objSchStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd2.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objSchStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd1.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else
                                            {
                                                ctx.RollBackTransaction();
                                                errMessage = "Durasi Kunjungan Tidak Valid.";
                                                result = false;
                                            }
                                        }
                                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                                        {
                                            if (AppointmentStart.TimeOfDay >= objSchStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd1.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else
                                            {
                                                ctx.RollBackTransaction();
                                                errMessage = "Durasi Kunjungan Tidak Valid.";
                                                result = false;
                                            }
                                        }
                                    }
                                    else
                                    {
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

                                        if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 != "")
                                        {
                                            if (AppointmentStart.TimeOfDay >= objStart5.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd5.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objStart4.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd4.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objStart3.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd3.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd2.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd1.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else
                                            {
                                                ctx.RollBackTransaction();
                                                errMessage = "Durasi Kunjungan Tidak Valid.";
                                                result = false;
                                            }
                                        }
                                        else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "")
                                        {
                                            if (AppointmentStart.TimeOfDay >= objStart4.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd4.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objStart3.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd3.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd2.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd1.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else
                                            {
                                                ctx.RollBackTransaction();
                                                errMessage = "Durasi Kunjungan Tidak Valid.";
                                                result = false;
                                            }
                                        }
                                        else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                                        {
                                            if (AppointmentStart.TimeOfDay >= objStart3.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd3.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd2.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd1.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else
                                            {
                                                ctx.RollBackTransaction();
                                                errMessage = "Durasi Kunjungan Tidak Valid.";
                                                result = false;
                                            }
                                        }
                                        else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                                        {
                                            if (AppointmentStart.TimeOfDay >= objStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd2.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else if (AppointmentStart.TimeOfDay >= objStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd1.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else
                                            {
                                                ctx.RollBackTransaction();
                                                errMessage = "Durasi Kunjungan Tidak Valid.";
                                                result = false;
                                            }
                                        }
                                        else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                                        {
                                            if (AppointmentStart.TimeOfDay >= objStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd1.TimeOfDay)
                                            {
                                                if (isValidMRN)
                                                {
                                                    entity.IsWaitingList = false;
                                                    if (newQueue == 0)
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                                    }
                                                    else
                                                    {
                                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                                    }
                                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                                    entityDao.Insert(entity);
                                                    ctx.CommitTransaction();
                                                    BridgingToGateway(appointmentID);
                                                }
                                                else
                                                {
                                                    ctx.RollBackTransaction();
                                                    errMessage = "Appointment dengan No. Rekam Medis ini sudah ada.";
                                                    result = false;
                                                }
                                            }
                                            else
                                            {
                                                ctx.RollBackTransaction();
                                                errMessage = "Durasi Kunjungan Tidak Valid.";
                                                result = false;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    entity.IsWaitingList = true;
                                    if (newQueue == 0)
                                    {
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSession.Value) + 1), 1));
                                    }
                                    else
                                    {
                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                    }
                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                    entityDao.Insert(entity);
                                    ctx.CommitTransaction();
                                    BridgingToGateway(appointmentID);
                                }
                                #endregion
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        result = false;
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
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
            else if (type == "voidAll")
            {
                Int32 ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);

                string filterExp = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND StartDate = '{3}'", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, selectedDate.ToString("yyyy-MM-dd"));
                vAppointment entityAppointment = BusinessLayer.GetvAppointmentList(filterExp).FirstOrDefault();

                String filter = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND StartDate = '{3}'", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, selectedDate.ToString("yyyy-MM-dd"));
                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                if (lstAppo.Where(p => p.GCAppointmentStatus == Constant.AppointmentStatus.COMPLETE).Count() > 0)
                {
                    ctx.RollBackTransaction();
                    errMessage = "Dokter ini sudah memiliki Perjanjian Pasien yang sudah Registrasi.";
                    result = false;
                }
                else
                {
                    foreach (Appointment e in lstAppo)
                    {
                        e.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
                        e.GCDeleteReason = Constant.AppointmentDeleteReason.PARAMEDIC;
                        e.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(e);
                    }
                    ctx.CommitTransaction();

                    if (entityAppointment != null)
                    {
                        //#region Is Brigding To Gateway
                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            List<vAppointment> lstFrom = new List<vAppointment>();
                            foreach (Appointment e in lstAppo)
                            {
                                vAppointment entityAppo = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", e.AppointmentID)).FirstOrDefault();
                                lstFrom.Add(entityAppo);
                            }

                            GatewayService oService = new GatewayService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            string apiResult = oService.OnChangedAppointmentInformation("ALLVOID", lstFrom);
                            string[] apiResultInfo = apiResult.Split('|');
                            if (apiResultInfo[0] == "0")
                            {
                                entityAPILog.IsSuccess = false;
                                entityAPILog.MessageText = apiResultInfo[1];
                                entityAPILog.Response = apiResultInfo[1];
                                Exception ex = new Exception(apiResultInfo[1]);
                                Helper.InsertErrorLog(ex);
                            }
                            else
                            {
                                entityAPILog.MessageText = apiResultInfo[0];
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            }
                        }
                        //#endregion
                    }

                }
                return result;
            }
            else if (type == "reopen")
            {
                Appointment a = BusinessLayer.GetAppointment(Convert.ToInt32(hdnAppointmentID.Value));
                string filterExpresion = string.Empty;
                if (!a.IsNewPatient)
                {
                    filterExpresion = string.Format("ParamedicID = {0} AND StartDate = '{1}' AND GCAppointmentStatus != '{2}' AND StartTime >= '{3}' AND EndTime <= '{4}' AND MRN = {5}", a.ParamedicID, a.StartDate, Constant.AppointmentStatus.DELETED, hdnStartTime.Value, hdnEndTime.Value, a.MRN);
                }
                else
                {
                    filterExpresion = string.Format("ParamedicID = {0} AND StartDate = '{1}' AND GCAppointmentStatus != '{2}' AND StartTime >= '{3}' AND EndTime <= '{4}' AND GuestID = {5}", a.ParamedicID, a.StartDate, Constant.AppointmentStatus.DELETED, hdnStartTime.Value, hdnEndTime.Value, a.GuestID);
                }
                List<Appointment> lstAppointmentCheck = BusinessLayer.GetAppointmentList(filterExpresion);
                if (lstAppointmentCheck.Count <= 0)
                {
                    string filterExp = String.Format("AppointmentID = {0}", a.AppointmentID);
                    vAppointment entityAppointment = BusinessLayer.GetvAppointmentList(filterExp).FirstOrDefault();
                    String filter = String.Format("AppointmentID = {0}", a.AppointmentID);
                    List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);
                    a.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                    a.DeleteReason = null;
                    a.GCDeleteReason = null;
                    a.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(a);
                    ctx.CommitTransaction();

                    //#region Is Brigding To Gateway
                    if (hdnIsBridgingToGateway.Value == "1")
                    {
                        List<vAppointment> lstFrom = new List<vAppointment>();
                        foreach (Appointment e in lstAppo)
                        {
                            vAppointment entityAppo = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", e.AppointmentID)).FirstOrDefault();
                            lstFrom.Add(entityAppo);
                        }

                        GatewayService oService = new GatewayService();
                        APIMessageLog entityAPILog = new APIMessageLog();
                        string apiResult = oService.OnChangedAppointmentInformation("REOPEN", lstFrom);
                        string[] apiResultInfo = apiResult.Split('|');
                        if (apiResultInfo[0] == "0")
                        {
                            entityAPILog.IsSuccess = false;
                            entityAPILog.MessageText = apiResultInfo[1];
                            entityAPILog.Response = apiResultInfo[1];
                            Exception ex = new Exception(apiResultInfo[1]);
                            Helper.InsertErrorLog(ex);
                        }
                        else
                        {
                            entityAPILog.MessageText = apiResultInfo[0];
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        }
                    }
                    //#endregion

                    return result;
                }
                else
                {
                    errMessage = "Appointment Untuk pasien ini sudah ada di sesi ini";
                    result = false;
                }
            }
            return false;
        }

        public class ParamedicScheduleCustom
        {
            public Int32 ID { get; set; }
            public Int32 Queue { get; set; }
            public String Time { get; set; }
            public String EndTime { get; set; }
            public Int32 ParentID { get; set; }
            public Boolean IsBPJS { get; set; }
            public Boolean IsEmpty { get; set; }
        }

        public class AppointmentCustomClass
        {
            public Int32 AppointmentID { get; set; }
            public String PatientName { get; set; }
            public String EstimatedTimeService { get; set; }
            public String PatientImageUrl { get; set; }
            public String VisitTypeName { get; set; }
            public String GCAppointmentStatus { get; set; }
            public String AppointmentNo { get; set; }
            public String StartTime { get; set; }
            public String EndTime { get; set; }
            public String CreatedInfo { get; set; }
            public String LastUpdatedInfo { get; set; }
            public String AppointmentStatus { get; set; }
            public Boolean IsVip { get; set; }
            public String GCCustomerType { get; set; }
            public Boolean IsBPJS { get; set; }
            public Boolean IsAppointmentCompleted
            {
                get { return GCAppointmentStatus == Constant.AppointmentStatus.COMPLETE; }
            }
            public Boolean IsAppointmentDeleted
            {
                get { return GCAppointmentStatus == Constant.AppointmentStatus.DELETED; }
            }
        }

        #region Bridging To Gateway
        private void BridgingToGateway(String appointmentID)
        {
            String filter = "";
            String filterExp = "";
            Int32 ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);

            if (hdnMRN.Value != "")
            {
                filterExp = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND StartDate = '{3}' AND MRN = {4}", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, selectedDate.ToString("yyyy-MM-dd"), hdnMRN.Value);
                filter = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND StartDate = '{3}' AND MRN = '{4}'", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, selectedDate.ToString("yyyy-MM-dd"), hdnMRN.Value);
            }
            else
            {
                filterExp = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND StartDate = '{3}' AND AppointmentNo LIKE '%{4}%'", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, selectedDate.ToString("yyyy-MM-dd"), appointmentID);
                filter = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND StartDate = '{3}' AND AppointmentNo LIKE '%{4}%'", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, selectedDate.ToString("yyyy-MM-dd"), appointmentID);
            }

            vAppointment entityAppointment = BusinessLayer.GetvAppointmentList(filterExp).FirstOrDefault();
            List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

            if (entityAppointment != null)
            {
                #region Is Brigding To Gateway
                if (hdnIsBridgingToGateway.Value == "1")
                {
                    List<vAppointment> lstFrom = new List<vAppointment>();
                    foreach (Appointment e in lstAppo)
                    {
                        vAppointment entityAppo = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", e.AppointmentID)).FirstOrDefault();
                        lstFrom.Add(entityAppo);
                    }

                    GatewayService oService = new GatewayService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.OnChangedAppointmentInformation("NEW", lstFrom);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[1]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[0];
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
                #endregion

                #region Is Bridging to Queue Service
                if (hdnIsBridgingToQumatic.Value == "1")
                {
                    vAppointment entityAppo = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", entityAppointment.AppointmentID)).FirstOrDefault();
                    QumaticPatientInfo oPatient = null;
                    if (!chkIsNewPatient.Checked)
                    {
                        oPatient = ConvertQumaticPatientInfoToDTO(entityAppo);
                    }
                    else
                    {
                        oPatient = new QumaticPatientInfo();
                        oPatient.Name = txtFirstName.Text + " " + txtMiddleName.Text + " " + txtFamilyName.Text;
                        if ((string)cboGenderAppointment.Value == Constant.Gender.MALE)
                        {
                            oPatient.Gender = "M";
                        }
                        else
                        {
                            oPatient.Gender = "F";
                        }
                        oPatient.MobilePhone = txtPhoneNo.Text;
                        oPatient.Email = txtEmail.Text;
                    }
                    QumaticPhysicianInfo oPhysician = ConvertQumaticPhysicianInfoToDTO(entityAppo);
                    QumaticScheduleInfo oSchedule = ConvertQumaticScheduleInfoToDTO(entityAppo);
                    QumaticBodyRequestNewApm oRequest = ConvertQumaticBodyRequestToDTO(entityAppo, oPatient, oPhysician, oSchedule);

                    QueueService oService = new QueueService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.QumaticNewApmInformation(entityAppo, oPatient, oPhysician, oSchedule, oRequest);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[1]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[0];
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
                #endregion
            }
        }

        private string BridgingToGatewayGetQueueNo(string medicalNo, string paramedicCode, string customerType, DateTime date, string hour, string via, string healthcareServiceUnitID, int session)
        {
            String queue = "";

            if (hdnIsBridgingToGateway.Value == "1")
            {
                GatewayService oService = new GatewayService();
                APIMessageLog entityAPILog = new APIMessageLog();
                entityAPILog.Sender = "MEDINFRAS";
                entityAPILog.Recipient = "QUEUE ENGINE";
                string apiResult = oService.GetQueueNo(medicalNo, paramedicCode, customerType, date, hour, via, healthcareServiceUnitID, session);
                string[] apiResultInfo = apiResult.Split('|');
                if (apiResultInfo[0] == "0")
                {
                    queue = string.Format("{0}|{1}", apiResultInfo[0], apiResultInfo[1]);
                    entityAPILog.MessageDateTime = DateTime.Now;
                    entityAPILog.IsSuccess = false;
                    entityAPILog.MessageText = apiResultInfo[2];
                    entityAPILog.Response = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    Exception ex = new Exception(apiResultInfo[1]);
                    Helper.InsertErrorLog(ex);
                }
                else
                {
                    queue = apiResult;
                    entityAPILog.MessageDateTime = DateTime.Now;
                    entityAPILog.MessageText = apiResultInfo[2];
                    entityAPILog.Response = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }
            }

            return queue;
        }

        #region ConvertToDTO

        private QumaticBodyRequestNewApm ConvertQumaticBodyRequestToDTO(vAppointment oAppo, QumaticPatientInfo oPatient, QumaticPhysicianInfo oPhysician, QumaticScheduleInfo oSchedule)
        {
            QumaticBodyRequestNewApm oData = new QumaticBodyRequestNewApm();
            oData.AppointmentID = oAppo.AppointmentID;
            oData.Token = oAppo.QueueNo;

            if (!oAppo.IsNewPatient)
            {
                oData.Type = "Pasien Lama";
            }
            else
            {
                oData.Type = "Pasien Baru";
            }

            if (oAppo.GCCustomerType == Constant.CustomerType.PERSONAL)
            {
                oData.Payment = "UMUM";
            }
            else if (oAppo.GCCustomerType == Constant.CustomerType.BPJS)
            {
                oData.Payment = "BPJS";
            }
            else
            {
                oData.Payment = "ASURANSI";
            }
            oData.Patient = oPatient;
            oData.Physician = oPhysician;
            oData.Physician.Schedule = oSchedule;
            return oData;
        }

        private QumaticPatientInfo ConvertQumaticPatientInfoToDTO(vAppointment oPatient)
        {
            QumaticPatientInfo oData = new QumaticPatientInfo();
            oData.ID = oPatient.MedicalNo;
            oData.Name = oPatient.PatientName;
            oData.DateOfBirth = oPatient.DOB.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);

            if (oPatient.GCGender == Constant.Gender.MALE)
            {
                oData.Gender = "M";
            }
            else
            {
                oData.Gender = "F";
            }

            oData.MobilePhone = oPatient.MobilePhoneNo;
            oData.Email = oPatient.EmailAddress;
            return oData;
        }
        private QumaticPhysicianInfo ConvertQumaticPhysicianInfoToDTO(vAppointment oAppointment)
        {
            QumaticPhysicianInfo oData = new QumaticPhysicianInfo();
            oData.ID = oAppointment.ParamedicCode;
            oData.ServiceUnit = oAppointment.ServiceUnitCode;
            return oData;
        }
        private QumaticScheduleInfo ConvertQumaticScheduleInfoToDTO(vAppointment oAppointment)
        {
            string time = "";
            QumaticScheduleInfo oData = new QumaticScheduleInfo();
            oData.Date = oAppointment.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
            time = string.Format("{0}-{1}", oAppointment.StartTime, oAppointment.EndTime);
            oData.Time = time;
            oData.Floor = string.Empty;
            oData.Room = oAppointment.RoomName;
            return oData;
        }
        #endregion
        #endregion
    }
}